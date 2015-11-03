using System;
using System.Windows;
using System.Windows.Controls;

namespace JaStDev.ControlFramework.Controls
{
   /// <summary>
   /// A replacement for the standard TabPanel used on TabControls which only uses 1 row.
   /// </summary>
   /// <remarks>
   /// <para>
   /// <see cref="TabPanel.Orientation"/> determins if items are displayed horizontally from left to right or vertically
   /// from top to bottom. When there is not enough room to fit all the items in the available width/height, it will shrink all 
   /// items as needed untill they do.  The property <see cref="TabPanel.AllowOverspil"/> determins if items are shrunk untill they
   /// all fit, or only as much as they allow.
   /// </para>
   /// <para>
   /// This panel is ideally suited for TabControls (due to the way it handles margins), but you can also use it standalone. 
   /// <see cref="ShrinkPanel"/> is a similar panel but which handles the margin differently and is therefor better suited for
   /// a more general purpose situation.
   /// </para>
   /// <para>
   /// This panel is used by <see cref="BreadcrumbsControl"/> to display it's <see cref="Breadcrumb"/>s.
   /// </para>
   /// </remarks>
   public class TabPanel: Panel
   {
      double fMaxFixed = 0;

     

      /// <summary>
      /// Measures the child elements of a TabPanel before they are arranged during the ArrangeOverride pass.
      /// </summary>
      /// <param name="availableSize">An upper Size limit that cannot be exceeded.</param>
      /// <returns>The Size that represents the upper size limit of the element.</returns>
      protected override Size MeasureOverride(Size availableSize)
      {
         Size iDesired;
         Size iTotal;
         double iTargetSize;

         if (Orientation == Orientation.Horizontal)
         {
            if (TargetSize > availableSize.Width)
               iTargetSize = availableSize.Width;
            else
               iTargetSize = TargetSize;
            iTotal = GetTotalSize(availableSize);

            fMaxFixed = iTotal.Height;
            if (iTargetSize < iTotal.Width)                           //we must recalculate the size giving each item a max value.
            {
               double iActualSize = 0;
               if (AllowOverspil == false)
               {
                  iActualSize = availableSize.Width;
                  Size iNew = new Size(iTargetSize / InternalChildren.Count, availableSize.Height);
                  foreach (UIElement i in InternalChildren)
                     i.Measure(iNew);
               }
               else
               {
                  Size iSize = new Size(iTargetSize / InternalChildren.Count, availableSize.Height);
                  int iCount = 0;
                  foreach (UIElement i in InternalChildren)
                  {
                     i.Measure(iSize);
                     iDesired = GetDesiredSizeWithoutMargin(i);
                     iTargetSize -= iDesired.Width;
                     iActualSize += iDesired.Width;
                     iCount++;
                     iSize = new Size(iTargetSize / (InternalChildren.Count - iCount), availableSize.Height);
                  }
               }
               return new Size(iActualSize, iTotal.Height);
            }
         }
         else
         {
            if (TargetSize < availableSize.Height)
               iTargetSize = availableSize.Height;
            else
               iTargetSize = TargetSize;
            iTotal = GetTotalSize(availableSize);

            fMaxFixed = iTotal.Width;
            if (iTargetSize < iTotal.Height)                           //we must recalculate the size giving each item a max value.
            {
               double iActualSize = 0;
               if (AllowOverspil == false)
               {
                  iActualSize = availableSize.Height;
                  Size iNew = new Size(availableSize.Width, iTargetSize / InternalChildren.Count);
                  foreach (UIElement i in InternalChildren)
                     i.Measure(iNew);
               }
               else
               {
                  Size iSize = new Size(availableSize.Width, iTargetSize / InternalChildren.Count);
                  int iCount = 0;
                  foreach (UIElement i in InternalChildren)
                  {
                     i.Measure(iSize);
                     iDesired = GetDesiredSizeWithoutMargin(i);
                     iTargetSize -= iDesired.Height;
                     iActualSize += iDesired.Height;
                     iCount++;
                     iSize = new Size(availableSize.Width, iTargetSize / (InternalChildren.Count - iCount));
                  }
               }
               return new Size(iTotal.Width, iActualSize);
            }
         }
         return iTotal;
      }

      /// <summary>
      /// Calculates how much size the items would occupy without any restrictions (first pass).
      /// </summary>
      /// <param name="availableSize">The total size available to the panel.</param>
      /// <returns>The total desired size of all the items.</returns>
      protected Size GetTotalSize(Size availableSize)
      {
         Size iRes = new Size();
         Size iDesired;
         UIElement iItem;

         if (Orientation == Orientation.Horizontal)
         {
            for (int i = 0; i < InternalChildren.Count; i++)
            {
               iItem = InternalChildren[i];
               if (iItem != null)
               {
                  if (iItem.Visibility == Visibility.Collapsed)
                     continue;
                  iItem.Measure(availableSize);
                  iDesired = GetDesiredSizeWithoutMargin(iItem);
                  if (iRes.Height < iDesired.Height)
                  {
                     iRes.Height = iDesired.Height;
                  }
                  iRes.Width += iDesired.Width;
               }
            }
         }
         else
         {
            for (int i = 0; i < InternalChildren.Count; i++)
            {
               iItem = InternalChildren[i];
               if (iItem != null)
               {
                  if (iItem.Visibility == Visibility.Collapsed)
                     continue;
                  iItem.Measure(availableSize);
                  iDesired = GetDesiredSizeWithoutMargin(iItem);
                  if (iRes.Width < iDesired.Width)
                  {
                     iRes.Width = iDesired.Width;
                  }
                  iRes.Height += iDesired.Height;
               }
            }
         }
         return iRes;
      }

      protected static Size GetDesiredSizeWithoutMargin(UIElement item)
      {
         Thickness thickness = (Thickness)item.GetValue(FrameworkElement.MarginProperty);
         Size size = new Size();
         size.Height = Math.Max(0.0, item.DesiredSize.Height - thickness.Top - thickness.Bottom);
         size.Width = Math.Max(0.0, item.DesiredSize.Width - thickness.Left - thickness.Right);
         return size;

      }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="finalSize"></param>
      /// <returns></returns>
      protected override Size ArrangeOverride(Size finalSize)
      {
         Rect iFinalRect = new Rect();
         UIElement iItem;
         Size iDesired;

         if (Orientation == Orientation.Horizontal)
         {
            iFinalRect.Height = fMaxFixed;
            for (int i = 0; i < InternalChildren.Count; i++)
            {
               iItem = InternalChildren[i];
               if (iItem != null)
               {
                  iDesired = GetDesiredSizeWithoutMargin(iItem);
                  iFinalRect.Width = iDesired.Width;
                  iItem.Arrange(iFinalRect);
                  iFinalRect.X += iDesired.Width;
               }
            }
         }
         else
         {
            iFinalRect.Width = fMaxFixed;
            for (int i = 0; i < InternalChildren.Count; i++)
            {
               iItem = InternalChildren[i];
               if (iItem != null)
               {
                  iDesired = GetDesiredSizeWithoutMargin(iItem);
                  iFinalRect.Height = iDesired.Height;
                  iItem.Arrange(iFinalRect);
                  iFinalRect.Y += iDesired.Height;
               }
            }
         }
         return finalSize;
      }


      #region prop

      /// <summary>
      /// Gets/sets the size of the panel that is fixed.  This depends on the <see cref="TabPanel.Orientation"/> value.
      /// </summary>
      protected double MaxFixed
      {
         get
         {
            return fMaxFixed;
         }
         set
         {
            fMaxFixed = value;
         }
      }



      #region AllowOverSpil
      /// <summary>
      /// Gets/sets wether all items have to fit the available width/height or that the is allowed to have items that are not visible
      /// because they couldn't fit the available space.
      /// </summary>
      /// <remarks>
      /// When this property is false, all the items will fit in the available space.  When true, items will be shrunken as much as possible.
      /// </remarks>
      public bool AllowOverspil
      {
         get { return (bool)GetValue(AllowOverspilProperty); }
         set { SetValue(AllowOverspilProperty, value); }
      }

      /// <summary>
      /// Identifies the AllowOverspil dependency property.
      /// </summary>
      public static readonly DependencyProperty AllowOverspilProperty =
          DependencyProperty.Register("AllowOverspil", typeof(bool), typeof(TabPanel), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsMeasure)); 
      #endregion



      #region Orientation
      /// <summary>
      /// Identifies the Orientation dependency property.
      /// </summary>
      public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register("Orientation", typeof(Orientation), typeof(TabPanel), new FrameworkPropertyMetadata(Orientation.Horizontal, FrameworkPropertyMetadataOptions.AffectsMeasure));

      /// <summary>
      /// Gets/sets the direction that the children will be stacked.
      /// </summary>
      public Orientation Orientation
      {
         get
         {
            return (Orientation)base.GetValue(OrientationProperty);
         }
         set
         {
            base.SetValue(OrientationProperty, value);
         }
      } 
      #endregion

      #region TargetSize


      /// <summary>
      /// Gets/sets the desired maximum size that the panel should try to achieve when <see cref="TabPanel.AllowOverspil"/> is true.
      /// </summary>
      /// <remarks>
      /// This property is usefull if the panel is used in an ItemsControl that contains a <see cref="System.Windows.Control.ScrollViewer"/>.
      /// In this case, the panel doesn't know the size restrictions, cause they are lifted by the scrollviewer, which means that the panel
      /// will never try to shrink items.  When this property is set, and smaller than the allowed size, items will by shrunk so that they
      /// can (potentially) fit this size. 
      /// </remarks>
      public double TargetSize
      {
         get { return (double)GetValue(TargetSizeProperty); }
         set { SetValue(TargetSizeProperty, value); }
      }

      // Using a DependencyProperty as the backing store for TargetSize.  This enables animation, styling, binding, etc...
      public static readonly DependencyProperty TargetSizeProperty =
          DependencyProperty.Register("TargetSize", typeof(double), typeof(TabPanel), new FrameworkPropertyMetadata(double.PositiveInfinity, FrameworkPropertyMetadataOptions.AffectsMeasure));



      #endregion




      #endregion

   }
}
