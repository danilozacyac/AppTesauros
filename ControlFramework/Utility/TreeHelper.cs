using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace JaStDev.ControlFramework.Utility
{
   /// <summary>
   /// A helper class for finding objects in the visual or logical tree of a wpf app.
   /// </summary>
   public class TreeHelper
   {
      /// <summary>
      /// Looks up the first object in the <see cref="System.Windows.Controls.Control.Template"/>
      /// of the control and returns this.
      /// </summary>
      /// <param name="name">The name of the object to look for.</param>
      /// <param name="control">The control to look in.</param>
      /// <returns></returns>
      public static object FindChildInTemplate(string name, Control control)
      {
         if (control != null && name != null)
         {
            ControlTemplate iTemplate = control.Template;
            if (control.IsLoaded == true && iTemplate != null)
               return iTemplate.FindName(name, control);
         }
         return null;
      }

      /// <summary>
      /// Searches up the visual tree starting at the specified object untill it finds the first object of the specified type or null.
      /// </summary>
      /// <param name="start">The object to start walking up the tree.</param>
      /// <typeparam name="T">The type to look for</typeparam>
      public static T FindInTree<T>(DependencyObject start) where T : class
      {
         while (start != null && !(start is T))
            start = VisualTreeHelper.GetParent(start);
         return start as T;
      }

      /// <summary>
      /// Searches up the Logical tree starting at the specified object untill it finds an object of the specified type or null.
      /// </summary>
      /// <param name="aStart">The object to start walking up the tree.</param>
      /// <param name="aType">The type to look for.</param>
      /// <returns></returns>
      public static T FindInLTree<T>(DependencyObject aStart) where T : class
      {
         while (aStart != null && !(aStart is T))
         {
            aStart = LogicalTreeHelper.GetParent(aStart);
         }
         return aStart as T;
      }

      /// <summary>
      /// Searches up the visual tree starting at the specified point untill it finds an object that has a dataContext of the specified
      /// type.
      /// </summary>
      /// <typeparam name="T">The data type to look for.</typeparam>
      /// <param name="start">The UI element to start looking at.</param>
      /// <returns>The DataContext object that was found.</returns>
      public static T FindInTreeAsDataContext<T>(DependencyObject aStart) where T : class
      {
         while (aStart != null && (aStart is FrameworkElement && !(((FrameworkElement)aStart).DataContext is T)))
         {
            aStart = VisualTreeHelper.GetParent(aStart);
         }
         if (aStart is FrameworkElement)
            return ((FrameworkElement)aStart).DataContext as T;
         else
            return null;
      }

      /// <summary>
      /// Looks up the Panel used by the ItemsControl
      /// </summary>
      /// <remarks>
      /// This function searches all the children of the 
      /// </remarks>
      /// <param name="list"></param>
      /// <returns></returns>
      public static Panel FindPanelFor(ItemsControl list)
      {
         return FindPanelForInternal(list);
      }

      /// <summary>
      /// Recursive part of <see cref="App.FindInPanelFor"/>.
      /// </summary>
      /// <param name="item">The item who's children to walk through in search of the first panel that is an ItemsHost.</param>
      /// <returns></returns>
      private static Panel FindPanelForInternal(DependencyObject item)
      {
         for (int i = 0; i < VisualTreeHelper.GetChildrenCount(item); i++)
         {
            Panel iPanel = VisualTreeHelper.GetChild(item, i) as Panel;
            if (iPanel != null && iPanel.IsItemsHost == true)
               return iPanel;
            else
            {
               Panel iRes = FindPanelForInternal(VisualTreeHelper.GetChild(item, i));
               if (iRes != null)
                  return iRes;
            }
         }
         return null;
      }
   }
}
