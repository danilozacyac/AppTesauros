using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input.StylusPlugIns;
using System.Windows.Media;
using JaStDev.ControlFramework.Input;

namespace JaStDev.ControlFramework.Controls
{
   /// <summary>
   /// An InkCanvas that can draw it's lines using a transparency based on the pressure of the stylus.
   /// </summary>
   public class TransparentInkCanvas: InkCanvas
   {
      #region IsPressuredTransparency

      /// <summary>
      /// IsPressuredTransparency Dependency Property
      /// </summary>
      public static readonly DependencyProperty IsPressuredTransparencyProperty =
          DependencyProperty.Register("IsPressuredTransparency", typeof(bool), typeof(TransparentInkCanvas),
              new FrameworkPropertyMetadata((bool)false,
                  new PropertyChangedCallback(OnIsPressuredTransparencyChanged)));

      /// <summary>
      /// Gets or sets the IsPressuredTransparency property.  This dependency property 
      /// indicates wether strokes are drawn with a transparency dependent on the pressure that was excercised on the stylus.
      /// </summary>
      public bool IsPressuredTransparency
      {
         get { return (bool)GetValue(IsPressuredTransparencyProperty); }
         set { SetValue(IsPressuredTransparencyProperty, value); }
      }

      /// <summary>
      /// Handles changes to the IsPressuredTransparency property.
      /// </summary>
      private static void OnIsPressuredTransparencyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
      {
         ((TransparentInkCanvas)d).OnIsPressuredTransparencyChanged(e);
      }

      /// <summary>
      /// Provides derived classes an opportunity to handle changes to the IsPressuredTransparency property.
      /// </summary>
      protected virtual void OnIsPressuredTransparencyChanged(DependencyPropertyChangedEventArgs e)
      {
         if ((bool)e.NewValue == true)
            DynamicRenderer = new CustomInkCanvasRenderer();
         else
            DynamicRenderer = new DynamicRenderer();
      }

      #endregion

/// <summary>
/// Raises the <see cref="E:System.Windows.Controls.InkCanvas.StrokeCollected"/> event.
/// </summary>
/// <param name="e">The event data.</param>
protected override void OnStrokeCollected(InkCanvasStrokeCollectedEventArgs e)
{
   if (IsPressuredTransparency == true)
   {
      float iPressure = (from i in e.Stroke.StylusPoints select i.PressureFactor).Average();
      Color iTemp = e.Stroke.DrawingAttributes.Color;
      iTemp.A = (byte)((float)byte.MaxValue * iPressure);
      e.Stroke.DrawingAttributes.Color = iTemp;
   }
   base.OnStrokeCollected(e);
}

   }
}
