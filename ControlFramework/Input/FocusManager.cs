using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace JaStDev.ControlFramework.Input
{
   /// <summary>
   /// A class to set the focused object on a Window.
   /// </summary>
   /// <remarks>
   /// This class provides an attached property you can use to make a FrameworkElement on a window have keyboard focus.
   /// </remarks>
   /// <example>
   /// The followind example demonstrates how to make the TextBox have focus:
   /// <code lang="xml">
   /// </code>
   /// </example>
   public class FocusManager : DependencyObject
   {
      static RoutedEventHandler fLoaded = new RoutedEventHandler(Object_Loaded);
      static RoutedEventHandler fSelectorLoaded = new RoutedEventHandler(Selector_Loaded);

      #region IsFocused

      /// <summary>
      /// Gets if this object will try to focus itself after it is loaded or not.
      /// </summary>
      /// <remarks>
      /// See <see cref="FocusManager.SetIsFocused"/> for more info.
      /// </remarks>
      /// <param name="obj">The object to get the value for.</param>
      /// <returns>True if this object will try to focus itself after it is loaded, otherwise false.</returns>
      public static bool GetIsFocused(DependencyObject obj) 
      {
         return (bool)obj.GetValue(IsFocusedProperty);
      }

      /// <summary>
      /// Sets if this object will try to focus itself after it is loaded or not.
      /// </summary>
      /// <remarks>
      /// If you set this attached property to true, the object will get an event handler for it's loaded event which will try to move 
      /// keyboard focus to the object.
      /// </remarks>
      /// <param name="obj">The object to set the value for.</param>
      /// <param name="value">True if this object will try to focus itself after it is loaded, otherwise false.</param>
      public static void SetIsFocused(DependencyObject obj, bool value)
      {
         obj.SetValue(IsFocusedProperty, value);
      }

      // Using a DependencyProperty as the backing store for IsFocused.  This enables animation, styling, binding, etc...
      public static readonly DependencyProperty IsFocusedProperty =
          DependencyProperty.RegisterAttached("IsFocused", typeof(bool), typeof(FocusManager), new UIPropertyMetadata(false, new PropertyChangedCallback(OnIsFocusedChanged)));

      /// <summary>
      /// Either removes the event handler from the item or registers an event handler called when the object is loaded to move focus.
      /// </summary>
      /// <remarks>
      /// We use an event so that focus is called when the object is loaded, if we would call from here, it will be called when the object is being
      /// created and not yet loaded, which would result in a failed Focus call.
      /// </remarks>
      static void OnIsFocusedChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
      {
         FrameworkElement iSender = sender as FrameworkElement;
         if (iSender != null)
         {
            if ((bool)e.NewValue == true)
               iSender.Loaded += fLoaded;
            else
               iSender.Loaded -= fLoaded;
         }
      } 

      #endregion



      #region IsSelectedItemFocused

      /// <summary>
      /// Gets if this object will try move focus to it's selected item when loaded or not.
      /// </summary>
      /// <remarks>
      /// See <see cref="FocusManager.SetIsSelectedItemFocused"/> for more info.
      /// </remarks>
      /// <param name="obj">The object to set the value for.</param>
      /// <returns>True if this object will try to focus it's selected item after it is loaded, otherwise false.</returns>
      public static bool GetIsSelectedItemFocused(DependencyObject obj)
      {
         return (bool)obj.GetValue(IsSelectedItemFocusedProperty);
      }

      /// <summary>
      /// Sets if this object will try move focus to it's selected item when loaded or not.
      /// </summary>
      /// <remarks>
      /// <para>
      /// This property can only be assigned to Selector controls (like ListBox and ListView).
      /// </para>
      /// <para>
      /// If you set this attached property to true, the object will get an event handler for it's loaded event which will try to move 
      /// keyboard focus to the first selected item.
      /// </para>
      /// <para>
      /// Note: this technique will only work if the Container used by the selector is focusable.
      /// </para>
      /// </remarks>
      /// <param name="obj">The object to set the value for.</param>
      /// <param name="value">True if this object will try to focus it's selected item after it is loaded, otherwise false.</param>
      public static void SetIsSelectedItemFocused(DependencyObject obj, bool value)
      {
         obj.SetValue(IsSelectedItemFocusedProperty, value);
      }

      // Using a DependencyProperty as the backing store for IsSelectedItemFocused.  This enables animation, styling, binding, etc...
      public static readonly DependencyProperty IsSelectedItemFocusedProperty =
          DependencyProperty.RegisterAttached("IsSelectedItemFocused", typeof(bool), typeof(FocusManager), new UIPropertyMetadata(false, new PropertyChangedCallback(OnIsSelectedItemFocusedChanged)));

      /// <summary>
      /// Either removes the event handler from the item or registers an event handler called when the object is loaded to move focus.
      /// </summary>
      /// <remarks>
      /// We use an event so that focus is called when the object is loaded, if we would call from here, it will be called when the object is being
      /// created and not yet loaded, which would result in a failed Focus call.
      /// </remarks>
      static void OnIsSelectedItemFocusedChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
      {
         Selector iSender = sender as Selector;
         if (iSender != null)
         {
            if ((bool)e.NewValue == true)
               iSender.Loaded += fSelectorLoaded;
            else
               iSender.Loaded -= fSelectorLoaded;
         }
      }  
      #endregion


      /// <summary>
      /// Moves focus to the item after it has been loaded.
      /// </summary>
      /// <remarks>
      /// The event handler is removed from the object to clean up.
      /// </remarks>
      static void Object_Loaded(object sender, RoutedEventArgs e)
      {
         FrameworkElement iSender = sender as FrameworkElement;
         Debug.Assert(iSender != null);
         iSender.Focus();
         iSender.Loaded -= Object_Loaded;
      }

      /// <summary>
      /// Moves focus to the selected item after it has been loaded.
      /// </summary>
      /// <remarks>
      /// The event handler is removed from the object to clean up.
      /// </remarks>
      static void Selector_Loaded(object sender, RoutedEventArgs e)
      {
         Selector iSender = sender as Selector;
         Debug.Assert(iSender != null);
         if (iSender.SelectedIndex > -1)
         {
            FrameworkElement iSub = iSender.ItemContainerGenerator.ContainerFromIndex(iSender.SelectedIndex) as FrameworkElement;
            if (iSub != null)
               iSub.Focus();
            else
               iSender.Focus();
         }
         else
            iSender.Focus();
         iSender.Loaded -= Object_Loaded;
      }

   }
}
