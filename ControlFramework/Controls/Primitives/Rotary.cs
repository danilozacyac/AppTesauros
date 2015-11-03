using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace JaStDev.ControlFramework.Controls.Primitives
{
   /// <summary>
   /// A Track implementation that shows a Rotary.
   /// </summary>
   /// <remarks>
   /// <para>
   /// Use this class in a Slider or ScrollBar template as a replacement for the Track control to change the appearance to a rotary.
   /// </para>
   /// <para>
   /// Scroll page up and down is not yet working properly.
   /// </para>
   /// <para>
   /// This type of rotary is rather CPY intensive
   /// </para>
   /// </remarks>
   public class Rotary: Track
   {
      #region Fields
      
      RotateTransform fTransform = new RotateTransform();
      Control fValueIndicator;
      bool fIsRotaryDrag = false;
      double fPrevDragValue;                                   //stores the previous value of ValueFromDistance
      
      #endregion

      #region Const
      
      static Rotary()
      {
         Track.ValueProperty.OverrideMetadata(typeof(Rotary), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(OnValueChanged)));
         EventManager.RegisterClassHandler(typeof(Rotary), Thumb.DragStartedEvent, new DragStartedEventHandler(OnThumbDragStarted));

      } 

      #endregion

      #region Prop

      /// <summary>
      /// Gets/sets the control that is used to indicate the current position of the rotary.
      /// </summary>
      /// <remarks>
      /// This is a control so that styles and templates can be used.
      /// </remarks>
      public Control ValueIndicator
      {
         get
         {
            return fValueIndicator;
         }
         set
         {
            UpdateComponent(fValueIndicator, value);
            fValueIndicator = value;
            if (fValueIndicator != null)
               fValueIndicator.RenderTransform = fTransform;
         }
      }

      /// <summary>
      /// Gets/sets the angle value that corresponds to the minimum value of the Track.
      /// </summary>
      public double MinimumAngle
      {
         get { return (double)GetValue(MinimumAngleProperty); }
         set { SetValue(MinimumAngleProperty, value); }
      }

      // Using a DependencyProperty as the backing store for MinimumAngle.  This enables animation, styling, binding, etc...
      public static readonly DependencyProperty MinimumAngleProperty =
          DependencyProperty.Register("MinimumAngle", typeof(double), typeof(Rotary), new FrameworkPropertyMetadata(30.0, FrameworkPropertyMetadataOptions.AffectsArrange));



      /// <summary>
      /// Gets/sets the angle value that corresponds to the maximum value of the Track.
      /// </summary>
      public double MaximumAngle
      {
         get { return (double)GetValue(MaximumAngleProperty); }
         set { SetValue(MaximumAngleProperty, value); }
      }

      // Using a DependencyProperty as the backing store for MaximumAngle.  This enables animation, styling, binding, etc...
      public static readonly DependencyProperty MaximumAngleProperty =
          DependencyProperty.Register("MaximumAngle", typeof(double), typeof(Rotary), new FrameworkPropertyMetadata(330.0, FrameworkPropertyMetadataOptions.AffectsArrange));



      /// <summary>
      /// Gets/sets the precision factor that is applied to the drag manouver.
      /// </summary>
      /// <remarks>
      /// This property can be used to adjust the precision during dragging.  This factor is multiplied with the drag value.
      /// Setting this value grater than 1 gives a coarser precision, smaller than 1 gives a finer precision.
      /// </remarks>
      public double Precision
      {
         get { return (double)GetValue(PrecisionProperty); }
         set { SetValue(PrecisionProperty, value); }
      }

      // Using a DependencyProperty as the backing store for Precision.  This enables animation, styling, binding, etc...
      public static readonly DependencyProperty PrecisionProperty =
          DependencyProperty.Register("Precision", typeof(double), typeof(Rotary), new UIPropertyMetadata(1.0));




      /// <summary>
      /// Determins the way dragging is performed.
      /// </summary>
      /// <remarks>
      /// When true: drag must be done in a rotary fashion, in the direction of the the control.
      /// When false, dragging is done like a normal scrollbar.
      /// </remarks>
      public bool IsRotaryDrag
      {
         get
         {
            return fIsRotaryDrag;
         }
         set
         {
            if (fIsRotaryDrag != value)
            {
               fIsRotaryDrag = value;
               if (Thumb != null)
               {
                  if (fIsRotaryDrag == true)
                     Thumb.RenderTransform = fTransform;
                  else
                     Thumb.RenderTransform = null;
               }
            }
         }
      }


      /// <summary>
      /// Gets the Angle that corresponds to the current value of the Rotary.
      /// </summary>
      /// <remarks>
      /// This prop provides the angle value that is used to draw the indicator (and possibly the knub). 
      /// </remarks>
      public double Angle
      {
         get { return (double)GetValue(AngleProperty); }
      }

      // Using a DependencyProperty as the backing store for Angle.  This enables animation, styling, binding, etc...
      public static readonly DependencyProperty AngleProperty =
          DependencyProperty.Register("Angle", typeof(double), typeof(Rotary), new FrameworkPropertyMetadata (0.0, null, new CoerceValueCallback(CoerceAngle)));

      static object CoerceAngle(DependencyObject sender, object val)
      {
         Rotary iSender = (Rotary)sender;
         return iSender.GetAngle();
      }



      #endregion


      #region functions

      /// <summary>
      /// Initializes the drag correctly.
      /// </summary
      private static void OnThumbDragStarted(object sender, DragStartedEventArgs e)
      {
         Rotary iSender = sender as Rotary;
         if (iSender != null)
            iSender.fPrevDragValue = 0.0;
      }

      /// <summary>
      /// Removes the old object and adds the new object to the visual tree.
      /// </summary>
      /// <param name="oldValue">The previous control</param>
      /// <param name="newValue">The new control</param>
      private void UpdateComponent(Control oldValue, Control newValue)
      {
         if (oldValue != newValue)
         {
            if (oldValue != null)
               base.RemoveVisualChild(oldValue);
            if (newValue != null)
               base.AddVisualChild(newValue);
            base.InvalidateMeasure();
            base.InvalidateArrange();
         }
      }

      /// <summary>
      /// Called when the 'Value' property is changed. adjusts the rotation.
      /// </summary>
      static void OnValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
      {
         Rotary iSelf = (Rotary)sender;
         iSelf.CoerceValue(Rotary.AngleProperty);
      }

      /// <summary>
      /// Calculates the angle that corresponds the the 'Value' of the Track object.  This is determined by the <see cref="Rotaray.MinimumAngle"/> 
      /// and <see cref="Rotary.MaximumAngle"/> and <see cref="Track.Minimum"/> and <see cref="Track.Maximum"/> properties. and ofcourse the current
      /// value of the Rotary (track).
      /// </summary>
      /// <returns>The angle in degrees that is equivalent to the current value.</returns>
      private double GetAngle()
      {
         return MinimumAngle +  (Value / (Maximum - Minimum) * (MaximumAngle - MinimumAngle));
      }


      /// <summary>
      /// Arranges all the children in the Rotary.  This is currently just the thumb.
      /// </summary>
      protected override Size ArrangeOverride(Size arrangeSize)
      {
         if (fTransform != null)
            fTransform.Angle = GetAngle();

         Rect iFull = new Rect(new Point(0, 0), arrangeSize);

         if (IncreaseRepeatButton != null)
            IncreaseRepeatButton.Arrange(GetIncreaseRepeatBtnRect(arrangeSize));
         if (DecreaseRepeatButton != null)
            DecreaseRepeatButton.Arrange(GetDecreaseRepeatBtnRect(arrangeSize));

         if (Thumb != null)
         {
            Thumb.Arrange(iFull);
            if (fTransform != null)
            {
               fTransform.CenterX = arrangeSize.Width / 2;
               fTransform.CenterY = arrangeSize.Height / 2;
            }
         }
         if (ValueIndicator != null)
            ValueIndicator.Arrange(iFull);
         return arrangeSize;
      }

      /// <summary>
      /// Gets the rect that arranges the DecreaseRepeat button.
      /// </summary>
      /// <returns>A rect that the DecreaseRepeatButton can use to arrange itself.</returns>
      private Rect GetDecreaseRepeatBtnRect(Size arrangeSize)
      {
         if (Orientation == Orientation.Horizontal)
         {
            if (IsDirectionReversed == false)
               return new Rect(new Point(0, 0), new Point(arrangeSize.Width / 2, arrangeSize.Height));
            else
               return new Rect(new Point(arrangeSize.Width / 2, 0), new Point(arrangeSize.Width, arrangeSize.Height));
         }
         else
         {
            if (IsDirectionReversed == false)
               return new Rect(new Point(0, 0), new Point(arrangeSize.Width, arrangeSize.Height / 2));
            else
               return new Rect(new Point(0, arrangeSize.Height / 2), new Point(arrangeSize.Width, arrangeSize.Height));
         }
      }

      /// <summary>
      /// Gets the rect that arranges the IncreaseRepeat button.
      /// </summary>
      /// <returns>A rect that the IncreaseRepeatButton can use to arrange itself.</returns>
      private Rect GetIncreaseRepeatBtnRect(Size arrangeSize)
      {
         if (Orientation == Orientation.Horizontal)
         {
            if (IsDirectionReversed == false)
               return new Rect(new Point(arrangeSize.Width / 2, 0), new Point(arrangeSize.Width, arrangeSize.Height));
            else
               return new Rect(new Point(0, 0), new Point(arrangeSize.Width, arrangeSize.Height));
         }
         else
         {
            if (IsDirectionReversed == false)
               return new Rect(new Point(0, arrangeSize.Height / 2), new Point(arrangeSize.Width, arrangeSize.Height));
            else
               return new Rect(new Point(0, 0), new Point(arrangeSize.Width, arrangeSize.Height / 2));
               
         }
      }

      protected override Size MeasureOverride(Size availableSize)
      {
         Size desiredSize;
         if (Thumb != null)
         {
            Thumb.Measure(availableSize);
            desiredSize = this.Thumb.DesiredSize;
         }
         else
            desiredSize = new Size(0, 0);
         if (ValueIndicator != null)
         {
            ValueIndicator.Measure(availableSize);
            desiredSize = new Size((ValueIndicator.DesiredSize.Height > DesiredSize.Width) ? ValueIndicator.DesiredSize.Width : DesiredSize.Width,
                                    (ValueIndicator.DesiredSize.Height > DesiredSize.Height) ? ValueIndicator.DesiredSize.Height : DesiredSize.Height);
                                    
         }
         return desiredSize;

      }



      public override double ValueFromDistance(double horizontal, double vertical)
      {
         Debug.Print(vertical.ToString());
         double iDir = IsDirectionReversed ? -1.0 : 1;
         double iRes;
         if (Orientation == Orientation.Horizontal)
         {
            iRes = (horizontal -fPrevDragValue) * Precision;
            fPrevDragValue = horizontal;
         }
         else
         {
            iRes =  (-vertical - fPrevDragValue) * Precision;
            fPrevDragValue = -vertical;
         }

         if (IsDirectionReversed == true)
            return -iRes;
         return iRes;
      }

      /// <summary>
      /// Called when the Collection of visual children is changed.
      /// </summary>
      /// <remarks>
      /// Checks if there is a Thumb, if so, sets the RenderTransform used to display the rotation value.
      /// </remarks>
      /// <param name="visualAdded">The visual that was added.</param>
      /// <param name="visualRemoved">The visual that was removed.</param>
      protected override void OnVisualChildrenChanged(DependencyObject visualAdded, DependencyObject visualRemoved)
      {
         base.OnVisualChildrenChanged(visualAdded, visualRemoved);
         Thumb iThumb;
         iThumb = visualRemoved as Thumb;
         if (iThumb != null)                                                     //make certain that all references are removed.
            iThumb.RenderTransform = null;
         if (IsRotaryDrag == true)
         {
            iThumb = visualAdded as Thumb;
            if (iThumb != null)                                  //if the newly added visual is the Thumb
               iThumb.RenderTransform = fTransform;
         }
      }

      protected override int VisualChildrenCount
      {
         get
         {
            int iRes = base.VisualChildrenCount;
            if (ValueIndicator != null)
               iRes++;
            return iRes;
         }
      }

      protected override Visual GetVisualChild(int index)
      {
         if (ValueIndicator != null && index == VisualChildrenCount - 1)
            return ValueIndicator;
         else
            return base.GetVisualChild(index);
      }

      #endregion
   }
}
