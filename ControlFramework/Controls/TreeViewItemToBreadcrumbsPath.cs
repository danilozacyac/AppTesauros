using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace JaStDev.ControlFramework.Controls
{
   /// <summary>
   /// A general purpose converter that is able to sync the <see cref="System.Windows.Controls.TreeView.SelectedItem"/> property (containing a TreeViewItem) of a
   /// Treeview with the <see cref="BreadcrumbsControl.ItemsSource"/> of a breadcrumbsControl. 
   /// </summary>
   /// <remarks>
   /// <para>
   /// This converter is only suitable if the SelectedItem property contains a TreeViewItem. This means that the treeview (or any TreeViewItems)
   /// can't be populated using ItemsSource (unless it contains TreeViewItems). You can however, use this source as a good starting point
   /// for any custom path converters. You could alternatively also use the <see cref="ObjectToBreadcrumbsPath"/> converter.
   /// </para>
   /// <para>
   /// This converter doesn't implement the ConvertBack function cause <see cref="System.Windows.Controls.TreeView.SelectedItem"/> can't be 
   /// assigned to.  This is why we use a WeakEvent pattern to monitor changes in the result list of the convert function.  The
   /// BreadcrumbsControl changes this list whenever the user presses on a breadcrumb or adds a new one.
   /// </para>
   /// </remarks>
   public class TreeViewItemToBreadcrumbsPath : IValueConverter, IWeakEventListener
   {
      #region IValueConverter Members

      public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
      {
         TreeViewItem iVal = value as TreeViewItem;
         if (iVal != null)
         {
            ObservableCollection<TreeViewItem> iRes = new ObservableCollection<TreeViewItem>();                //this will become the path displayed in the breadcrumbs control.
            while (iVal != null)
            {
               iRes.Insert(0, iVal);                                                                           //since we are walking from end point up the tree to the starting point, we need to use insert so that the resulting list is in the correct order.
               iVal =ItemsControl.ItemsControlFromItemContainer(iVal) as TreeViewItem;                         //we get the parent node of the current one (walking up the tree).  If you have a data object, this could be a property 'Owner' or 'Parent' for instance.
            }
            CollectionChangedEventManager.AddListener(iRes, this);                                             //The user can change this list by clicking on a breadcrumb. We need to respond to this to update the new SelectedItem property of the TreeView. We use a WeakEvent pattern to make certain there are no mem leaks.
            return iRes;
         }
         return null;
      }

      public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
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
         if (managerType == typeof(CollectionChangedEventManager) && sender is ObservableCollection<TreeViewItem>)      //required check to make certain we respond to the correct event.
         {
            ObservableCollection<TreeViewItem> iSender = (ObservableCollection<TreeViewItem>)sender;
            if (iSender.Count > 0)
               iSender[0].IsSelected = true;
            return true;
         }
         return false;
      }

      #endregion
   }
}
