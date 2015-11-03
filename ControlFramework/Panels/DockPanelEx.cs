using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace JaStDev.ControlFramework.Controls
{
   /// <summary>
   /// A DockPanel descendent that will stretch the last 'visible' child to fill the remaining available space.
   /// </summary>
   /// <remarks>
   /// <para>
   /// The major difference between this class and the standard DockPanel is that the the latter will stretch the last child, not the last visible child.
   /// This means that, if the last child in a standard DockPanel is invisible, it will not resize the item before that, possibly leaving space open.
   /// </para>
   /// <para>
   /// This class also provides a property to easely find out how many items are visible.
   /// </para>
   /// </remarks>
   public class DockPanelEx : DockPanel
   {
      /// <summary>
      /// The default constructor.
      /// </summary>
      public DockPanelEx()
         : base()
      {
      }

      /// <summary>
      /// Arranges and sizes the content of a DockPanelEx object.
      /// </summary>
      /// <param name="arrangeSize">The computed size that is used to arrange the content</param>
      /// <returns>The size of the content. </returns>
      protected override Size ArrangeOverride(Size arrangeSize)
      {
         UIElementCollection iChildren = InternalChildren;
         Debug.Assert(iChildren != null);
         int iCount = iChildren.Count;
         int iLastVis = IndexOfLastVisible;
         double x = 0;
         double y = 0;
         double iUsedWidth = 0;
         double iUsedHeight = 0;

         for (int i = 0; i <= iLastVis; i++)
         {
            UIElement element = iChildren[i];
            if (element != null)
            {
               Size desiredSize = element.DesiredSize;
               Rect finalRect = new Rect(x, y, Math.Max((double)0, (double)(arrangeSize.Width - (x + iUsedWidth))), Math.Max((double)0, (double)(arrangeSize.Height - (y + iUsedHeight))));
               if (i < iLastVis)
               {
                  switch (GetDock(element))
                  {
                     case Dock.Left:
                        x += desiredSize.Width;
                        finalRect.Width = desiredSize.Width;
                        break;

                     case Dock.Top:
                        y += desiredSize.Height;
                        finalRect.Height = desiredSize.Height;
                        break;

                     case Dock.Right:
                        iUsedWidth += desiredSize.Width;
                        finalRect.X = Math.Max((double)0, (double)(arrangeSize.Width - iUsedWidth));
                        finalRect.Width = desiredSize.Width;
                        break;

                     case Dock.Bottom:
                        iUsedHeight += desiredSize.Height;
                        finalRect.Y = Math.Max((double)0, (double)(arrangeSize.Height - iUsedHeight));
                        finalRect.Height = desiredSize.Height;
                        break;
                  }
               }
               element.Arrange(finalRect);
            }
         }
         return arrangeSize;
      }

      /// <summary>
      /// Gets the index nr of the last visible element.
      /// </summary>
      protected int IndexOfLastVisible
      {
         get
         {
            UIElementCollection iChildren = InternalChildren;
            Debug.Assert(iChildren != null);
            for (int i = iChildren.Count - 1; i >= 0; i--)
            {
               if (iChildren[i] != null && iChildren[i].Visibility == System.Windows.Visibility.Visible)
                  return i;
            }
            return -1;
         }
      }

      /// <summary>
      /// Gets the total number of children that have their 'Visibility' property set to 'Visible.
      /// </summary>
      /// <remarks>
      /// This is a convenience property to easely find Visibility count information.
      /// </remarks>
      public int VisibleChildCount
      {
         get
         {
            int iRes = 0;
            foreach (UIElement i in this.Children)
            {
               if (i != null && i.Visibility == Visibility.Visible)
               {
                  iRes++;
               }
            }
            return iRes;
         }
      }

      /// <summary>
      /// Gets the UIElement that is the last visible item on the panel.
      /// </summary>
      public UIElement LastVisibleChild
      {
         get
         {
            UIElementCollection iChildren = Children;
            for (int i = iChildren.Count - 1; i >= 0; i--)
            {
               if (iChildren[i] != null && iChildren[i].Visibility == Visibility.Visible)
               {
                  return iChildren[i];
               }
            }
            return null;
         }
      }
   }
}
