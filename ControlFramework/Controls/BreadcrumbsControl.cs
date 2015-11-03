using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace JaStDev.ControlFramework.Controls
{

   /// <summary>
   /// A control that works similar to the breadcrumb control found in windows explorer.  It displays and allows editing of a selection path.
   /// </summary>
   public class BreadcrumbsControl : Selector
   {
      static BreadcrumbsControl()
      {
         DefaultStyleKeyProperty.OverrideMetadata(typeof(BreadcrumbsControl), new FrameworkPropertyMetadata(typeof(BreadcrumbsControl)));
         EventManager.RegisterClassHandler(typeof(BreadcrumbsControl), MenuItem.ClickEvent, new RoutedEventHandler(OnMenuItemClicked));
         EventManager.RegisterClassHandler(typeof(BreadcrumbsControl), Button.ClickEvent, new RoutedEventHandler(OnBtnBreadcrumbClicked));
      }

      #region Prop

      #region Seeds


      /// <summary>
      /// Gets/sets the list of possible values that can be used as the start of a path.
      /// </summary>
      /// <remarks>
      /// When the <see cref="BreadcrumbsControl.Items"/> or <see cref="BreadcrumbsControl.ItemsSource"/> is assigned/changed, there
      /// is no check done that the first item is really from this list.  It is just suggested to the user.
      /// </remarks>
      public IEnumerable  Seeds
      {
         get { return (IEnumerable )GetValue(SeedsProperty); }
         set { SetValue(SeedsProperty, value); }
      }

      // Using a DependencyProperty as the backing store for Seeds.  This enables animation, styling, binding, etc...
      public static readonly DependencyProperty SeedsProperty =
          DependencyProperty.Register("Seeds", typeof(IEnumerable ), typeof(BreadcrumbsControl), new UIPropertyMetadata(null));



      #endregion


      #region SeedTemplate


      /// <summary>
      /// gets/sets the template to use for the dropdown menu that displays a list with possible next <see cref="Breadcrumb"/> items.
      /// </summary>
      public DataTemplate SeedTemplate
      {
         get { return (DataTemplate)GetValue(SeedTemplateProperty); }
         set { SetValue(SeedTemplateProperty, value); }
      }

      // Using a DependencyProperty as the backing store for SeedItemTemplate.  This enables animation, styling, binding, etc...
      public static readonly DependencyProperty SeedTemplateProperty =
          DependencyProperty.Register("SeedTemplate", typeof(DataTemplate), typeof(BreadcrumbsControl), new UIPropertyMetadata(null));



      #endregion

      #region SeedTemplateSelector


      /// <summary>
      /// Gets/sets the DataTemplateSelector to use in the drop down menu used to display the list of possible next <see cref="Breadcrumb"/>s.
      /// </summary>
      public DataTemplateSelector SeedItemTemplateSelector
      {
         get { return (DataTemplateSelector)GetValue(SeedTemplateSelectorProperty); }
         set { SetValue(SeedTemplateSelectorProperty, value); }
      }

      // Using a DependencyProperty as the backing store for SeedItemTemplateSelector.  This enables animation, styling, binding, etc...
      public static readonly DependencyProperty SeedTemplateSelectorProperty =
          DependencyProperty.Register("SeedTemplateSelector", typeof(DataTemplateSelector), typeof(BreadcrumbsControl), new UIPropertyMetadata(null));



      #endregion

      #region SeedItemContainerStyle


      /// <summary>
      /// Gets/sets the style applied to the dropdown menu that contains all the possible next <see cref="Breadcrumb"/>.  This is usually 
      /// a style that is applied to menuItems.
      /// </summary>
      public Style SeedContainerStyle
      {
         get { return (Style)GetValue(SeedContainerStyleProperty); }
         set { SetValue(SeedContainerStyleProperty, value); }
      }

      // Using a DependencyProperty as the backing store for SeedItemContainerStyle.  This enables animation, styling, binding, etc...
      public static readonly DependencyProperty SeedContainerStyleProperty =
          DependencyProperty.Register("SeedContainerStyle", typeof(Style), typeof(BreadcrumbsControl), new UIPropertyMetadata(null));



      #endregion

      #region SeedItemContainerStyleSelector



      public StyleSelector SeedContainerStyleSelector
      {
         get { return (StyleSelector)GetValue(SeedContainerStyleSelectorProperty); }
         set { SetValue(SeedContainerStyleSelectorProperty, value); }
      }

      // Using a DependencyProperty as the backing store for SeedContainerStyleSelector.  This enables animation, styling, binding, etc...
      public static readonly DependencyProperty SeedContainerStyleSelectorProperty =
          DependencyProperty.Register("SeedContainerStyleSelector", typeof(StyleSelector), typeof(BreadcrumbsControl), new UIPropertyMetadata(null));



      #endregion
      
      #endregion




      

      #region functions

      /// <summary>
      /// Determines if the specified item is (or is eligible to be) its own container.
      /// </summary>
      /// <param name="item">The item to check.</param>
      /// <returns>
      /// true if the item is (or is eligible to be) its own container; otherwise, false.
      /// </returns>
      protected override bool IsItemItsOwnContainerOverride(object item)
      {
         return item is Breadcrumb;
      }

      /// <summary>
      /// Creates or identifies the element that is used to display the given item.
      /// </summary>
      /// <returns>
      /// The element that is used to display the given item.
      /// </returns>
      protected override DependencyObject GetContainerForItemOverride()
      {
         return new Breadcrumb();
      }

      static void OnBtnBreadcrumbClicked(object aSender, RoutedEventArgs e)
      {
         BreadcrumbsControl iSender = aSender as BreadcrumbsControl;
         ButtonBase iBtn = e.OriginalSource as ButtonBase;
         if (iSender != null && iBtn != null && iBtn.Name == "BtnBreadcrumb")
         {
            Breadcrumb iParent = iBtn.TemplatedParent as Breadcrumb;
            iSender.SetPathFrom(iParent.Header, null);
            iSender.CoerceValue(BreadcrumbsControl.ItemsSourceProperty);               //if we are bound to an itemsSource, this makes certain that the bound value can also be updated.
            e.Handled = true;
         }
      }

      /// <summary>
      /// Called when a menu item is activated. This is used to allow the user to select a new path using a menu.
      /// </summary>
      /// <param name="aSender"></param>
      /// <param name="e"></param>
      static void OnMenuItemClicked(object aSender, RoutedEventArgs e)
      {
         MenuItem iMenu = e.OriginalSource as MenuItem;
         BreadcrumbsControl iSender = aSender as BreadcrumbsControl;
         if (iMenu != null && iSender != null)
         {
            List<object> iPath = new List<object>();
            ItemsControl iOwner = iMenu;
            do
            {                                                                                         //this technique would allow for submenu items (build a path with the menu)
               iPath.Insert(0, ((MenuItem)(iOwner)).Header);                                          //cast is ok: verified by the loop + initial assign.
               iOwner = ItemsControl.ItemsControlFromItemContainer(iOwner);
            } while (iOwner is MenuItem && ((MenuItem)iOwner).Role != MenuItemRole.TopLevelHeader);   //don't need to add the arrow key to the list
            if (iOwner is MenuItem)                                                                   //if it's the toplevel header, 1 more up is the menu, which we need to check if the Templatedparent is a breadcrumb or the control: breadcrumbControl is set path from start.
            {
               iOwner = ItemsControl.ItemsControlFromItemContainer(iOwner);
               if (iOwner is Menu)
               {
                  if (iOwner.TemplatedParent is BreadcrumbsControl)
                     iSender.SetPathFrom(null, iPath);
                  else
                  {
                     Breadcrumb iBreadCrumb = iOwner.TemplatedParent as Breadcrumb;
                     if (iBreadCrumb != null)
                        iSender.SetPathFrom(iBreadCrumb.DataContext, iPath);
                  }
                  e.Handled = true;
               }
            }
         }
      }

      /// <summary>
      /// Tries to build the path (Items) starting from the specified object.
      /// </summary>
      /// <remarks>
      /// The starting point speciefies an object that is in the current <see cref="BreadcrumbsControl.Path"/> list.
      /// </remarks>
      /// <param name="start">The last object to include from the old list in the new list.  When null, the previous path will be
      /// completly erased.</param>
      /// <param name="path">The new part to add after 'start'.  This can be null</param>
      internal void SetPathFrom(object start, List<object> path)
      {
         IList iItems = null;
         if (ItemsSource != null)
            iItems = ItemsSource as IList;
         else
            iItems = Items;
         if (Items != null)
         {
            if (start != null)
            {
               while (iItems.Count > 0 && iItems[iItems.Count - 1] != start)
                  iItems.RemoveAt(iItems.Count - 1);
            }
            else
               iItems.Clear();
            if (path != null)
            {
               foreach (object i in path)
                  iItems.Add(i);
            }
         }
      }

      #endregion
   }
}
