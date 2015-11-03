using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;
using JaStDev.ControlFramework.Utility;

namespace JaStDev.ControlFramework.Controls
{
   /// <summary>
   /// Declares a destination and rule to be used by a <see cref="DistributionPanel"/> so that it can select the proper destination for
   /// it's children.
   /// </summary>
   /// <remarks>
   /// This needs to be a <see cref="FrameworkElement"/> so that the binding can work easely.
   /// </remarks>
   public class DistributionTarget: FrameworkElement
   {

      #region fields
      ObservableCollection<UIElement> fItems = new ObservableCollection<UIElement>();           //Observable cause we need to update HasItems whenever required and since this list is provided for internal use, we need a reliable way to get the update, could be improved upon.
      #endregion

      #region ctor

      /// <summary>
      /// Default constructor.
      /// </summary>
      public DistributionTarget()
      {
         fItems.CollectionChanged += Items_CollectionChanged;
      }

      
      #endregion

      #region prop

      #region Target
      /// <summary>
      /// Gets/sets the target list for objects who's <see cref="Rule"/> value evaluates to true.
      /// </summary>
      /// <remarks>
      /// This can be the <see cref="System.Windows.Controls.Panel.Children"/> property of a Panel, or a custom list
      /// that does extra processing after or before the list changes.
      /// </remarks>
      public IList Target
      {
         get { return (IList)GetValue(TargetProperty); }
         set { SetValue(TargetProperty, value); }
      }

      /// <summary>
      /// Identifies the Target dependency property.
      /// </summary>
      public static readonly DependencyProperty TargetProperty =
          DependencyProperty.Register("Target", typeof(IList), typeof(DistributionTarget), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnTargetChanged)));

      static void OnTargetChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
      {
         DistributionPanel iPanel = TreeHelper.FindInLTree<DistributionPanel>(sender);
         if (iPanel != null && iPanel.IsLoaded == true)
            iPanel.RebuildDistributionFor((DistributionTarget)sender);
      }
      #endregion

      #region Value
      /// <summary>
      /// Gets/sets the value that is used to compare with <see cref="DistributionPanel.Get"/>.
      /// <remarks>
      /// If the property value equals this value, the <see cref="DistributionPanel"/> will use the value found in <see cref="DistributionRule.Target"/>
      /// as it distribution point.
      /// </remarks>
      /// </summary>
      public object Value
      {
         get { return (object)GetValue(ValueProperty); }
         set { SetValue(ValueProperty, value); }
      }

      /// <summary>
      /// Identifies the Value dependency property.
      /// </summary>
      public static readonly DependencyProperty ValueProperty =
          DependencyProperty.Register("Value", typeof(object), typeof(DistributionPanel), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnValueChanged)));

      static void OnValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
      {
         DistributionPanel iPanel = TreeHelper.FindInLTree<DistributionPanel>(sender);
         if (iPanel != null)
            iPanel.RebuildDistribution();
      }
      #endregion


      #region HasItems
      /// <summary>
      /// Gets wether this target has any items in it's collection.
      /// </summary>
      /// <remarks>
      /// This is a convenience property that is probably most usefull from xaml.  Since this property
      /// is automatically updated whenever this state changes, it allows you to bind to it and change something
      /// else like a Visibility or Background property to indicate that there are or aren't any items anymore.
      /// </remarks>
      public bool HasItems
      {
         get { return (bool)GetValue(HasItemsProperty); }
      }

      internal static readonly DependencyPropertyKey HasItemsKey = DependencyProperty.RegisterReadOnly("HasItems", typeof(bool), typeof(DistributionTarget), new FrameworkPropertyMetadata(false));

      /// <summary>
      /// Identifies the HasItems dependency property.
      /// </summary>
      public static readonly DependencyProperty HasItemsProperty = HasItemsKey.DependencyProperty;
      //public static readonly DependencyProperty HasItemsProperty = DependencyProperty.Register("HasItems", typeof(bool), typeof(DistributionTarget), new FrameworkPropertyMetadata(false)); 
      #endregion

      
      /// <summary>
      /// gets the list of UIElements that have been added to this target by the DistributionPanel.
      /// </summary>
      /// <remarks>
      /// This allows the distribution panel to quickly find items back.
      /// </remarks>
      internal protected IList<UIElement> Items
      {
         get
         {
            return fItems;
         }
      }


      #endregion

      #region Functions

      /// <summary>
      /// Whenever the list is changed, we update <see cref="DistributionTarget.HasItems"/> (we check if there are any items left in our list).
      /// </summary>
      /// <remarks>
      /// Don't need to take <see cref="AnimatedTarget.UsePlaceHolder"/> into account (they don't remain on the list). This
      /// is because placeholder objects are always shown collapsed (they are only placeholders afterall) and don't effect
      /// if there are any items in the list logically (even though there techniquely could be).
      /// </remarks>
      void Items_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
      {
         //HasItems = fItems.Count > 0;
         SetValue(HasItemsKey, fItems.Count > 0);
      }

      /// <summary>
      /// Called to add an item to a target.
      /// </summary>
      /// <remarks>
      /// Overwrite this function if you want to do some extra processing when an item is added to the target list.  This is the
      /// only function you need to overwrite for handling this type of event.
      /// </remarks>
      /// <param name="item">The element being added.</param>
      internal protected virtual void AddChildToTarget(UIElement item)
      {
         IList iList = Target;
         if (iList != null)
            iList.Add(item);
         Items.Add(item);                                                     //Items is a protected property that contains all the UIElements assigned to this target.
      }

      /// <summary>
      /// Called to remove an item from a distribution list.
      /// </summary>
      /// <remarks>
      /// This is the only function that performs this task, so you don't need to overwrite other methods for handling removes.  
      /// Overwrite it when you need extra processing before or after a child is removed from a target.
      /// </remarks>
      /// <param name="item">The item to remove.</param>
      internal protected virtual void RemoveChildFromTarget(UIElement item)
      {
         IList iList = Target;
         if (iList != null)
            iList.Remove(item);
         Items.Remove(item);                                                  //Items is a protected property that contains all the UIElements assigned to this target.
      }


      #endregion

   }
}
