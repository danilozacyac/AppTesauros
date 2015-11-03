using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace JaStDev.ControlFramework.Controls
{
   /// <summary>
   /// The event arguments for a <see cref="BreadcrumbsConverterEventHandler"/>
   /// </summary>
   public class BreadcrumbsConverterEventArgs: EventArgs
	{
      /// <summary>
      /// Gets/sets the TreeView that provides the path for the <see cref="BreadcrumbsControl"/>.
      /// </summary>
      public TreeView Source { get; set; }

      /// <summary>
      /// Gets/sets the data object that this event is raised for.
      /// </summary>
      public object Data { get; set; }
	}

   /// <summary>
   /// A delegate used by <see cref="ObjectToBreadcrumbsPath"/> to get the parent of an object.
   /// </summary>
   /// <param name="sender">The object taht raised event, usually an <see cref="ObjectToBreadcrumbsPath"/>.</param>
   /// <param name="e">The event arguments.</param>
   /// <returns>The parent object of the object passed along in the event arguments.</returns>
   public delegate object BreadcrumbsConverterEventHandler(object sender, BreadcrumbsConverterEventArgs e);

   /// <summary>
   /// A general purpose converter that is able to sync the <see cref="System.Windows.Controls.TreeView.SelectedItem"/> property 
   /// (containing any type of object) of a Treeview with the <see cref="BreadcrumbsControl.ItemsSource"/> of a breadcrumbsControl. 
   /// </summary>
   /// <remarks>
   /// <para>
   /// This converter expects a <see cref="System.Windows.Controls.TreeView"/> as the first binding and an object in the second.  The
   /// second binding is usually to the SelectedItem property of the TreeView.  We need both references so we can properly find and
   /// change the value.
   /// </para>
   /// <para>
   /// You should also provide an event handler for <see cref="ObjectToBreadcrumbsPath.GetParent"/> so the converter can build a correct 
   /// selection path.
   /// </para>
   /// <para>
   /// This converter can be used for any type of data.  There is a small drawback in that it is a bit slower compared to the
   /// <see cref="TreeViewItemBreadcrumbsPath"/> converter. 
   /// </para>
   /// <para>
   /// This converter doesn't implement the ConvertBack function cause <see cref="System.Windows.Controls.TreeView.SelectedItem"/> can't be 
   /// assigned to.  This is why we use a WeakEvent pattern to monitor changes in the result list of the convert function.  The
   /// BreadcrumbsControl changes this list whenever the user presses on a breadcrumb or adds a new one.
   /// </para>
   /// </remarks>
   public class ObjectToBreadcrumbsPath : IMultiValueConverter, IWeakEventListener
   {
      #region inner types

      /// <summary>
      /// Stores the TreeView so that we can look up TreeViewItems when the list has changed.
      /// </summary>
      class BreadcrumbsPath: ObservableCollection<object>
      {
         /// <summary>
         /// Gets/sets the TreeView from which this list stores a path.
         /// </summary>
         public TreeView Source { get; set; }
      }

      #endregion

      /// <summary>
      /// Raised when we need to find the parent object in of an object in a tree.
      /// </summary>
      /// <remarks>
      /// This event must be implemented, otherwise the converter is not possible to find the path starting from the
      /// currently seledted object to the root (TreeView). 
      /// </remarks>
      public event BreadcrumbsConverterEventHandler GetParent;

      #region IMultiValueConverter Members

      public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
      {
         if (values.Length >= 2)
         {
            TreeView iSource = values[0] as TreeView;
            object iVal = values[1];
            if (iSource != DependencyProperty.UnsetValue && iVal != DependencyProperty.UnsetValue)
            {
               BreadcrumbsPath iRes = new BreadcrumbsPath() { Source = iSource };                                    //this will become the path displayed in the breadcrumbs control.
               do
               {
                  iRes.Insert(0, iVal);                                                                              //since we are walking from end point up the tree to the starting point, we need to use insert so that the resulting list is in the correct order.
                  if (GetParent != null)
                     iVal = GetParent(this, new BreadcrumbsConverterEventArgs() { Data = iVal, Source = iSource });  //we use the event handler to get the parent object of the current one.  
                  else
                     iVal = null;                                                                                    //if there is no event handler, this will get us out of the loop.
               } while (iVal != null);
               CollectionChangedEventManager.AddListener(iRes, this);                                                //The user can change this list by clicking on a breadcrumb. We need to respond to this to update the new SelectedItem property of the TreeView. We use a WeakEvent pattern to make certain there are no mem leaks.
               return iRes;
            }
         }
         return null;
      }

      public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
      {
         throw new NotImplementedException();
      }

      #endregion

      #region IWeakEventListener Members

      /// <summary>
      /// Called when the collection is changed.  We need to update the SelectedItem.
      /// </summary>
      public bool ReceiveWeakEvent(Type managerType, object sender, EventArgs e)
      {
         if (managerType == typeof(CollectionChangedEventManager) && sender is BreadcrumbsPath)      //required check to make certain we respond to the correct event.
         {
            BreadcrumbsPath iSender = (BreadcrumbsPath)sender;
            if (iSender.Count > 0)
            {
               ItemsControl iOwner = iSender.Source;
               foreach (object i in iSender)
               {
                  iOwner = iOwner.ItemContainerGenerator.ContainerFromItem(i) as TreeViewItem;
                  if (iOwner == null)
                     break;
               }
               if (iOwner != null)
                  ((TreeViewItem)iOwner).IsSelected = true;
            }
            return true;
         }
         return false;
      }

      #endregion
   
   }
}
