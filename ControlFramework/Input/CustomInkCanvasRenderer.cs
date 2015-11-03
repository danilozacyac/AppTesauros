using System;
using System.Linq;
using System.Windows.Input;
using System.Windows.Input.StylusPlugIns;
using System.Windows.Media;

namespace JaStDev.ControlFramework.Input
{
   /// <summary>
   /// A <see cref=" System.Windows.Input.StylusPlugIns.DynamicRenderer"/> descendent that draws 
   /// strokes with an alpha color value dependent on the average stylus pressure value of the stroke.
   /// </summary>
   public class CustomInkCanvasRenderer: DynamicRenderer
   {
      /// <summary>
      /// Draws the ink in real-time so it appears to "flow" from the tablet pen or other pointing device.
      /// </summary>
      /// <remarks>
      /// Makes an average of the pressure values of each point and uses this value to add transparency.
      /// </remarks>
      /// <param name="drawingContext">The <see cref="T:System.Windows.Media.DrawingContext"/> object onto 
      /// which the stroke is rendered.</param>
      /// <param name="stylusPoints">
      /// The <see cref="T:System.Windows.Input.StylusPointCollection"/> that represents the segment of the stroke 
      /// to draw.
      /// </param>
      /// <param name="geometry">
      /// A <see cref="T:System.Windows.Media.Geometry"/> that represents the path of the mouse pointer.
      /// </param>
      /// <param name="fillBrush">A Brush that specifies the appearance of the current stroke.</param>
protected override void OnDraw(DrawingContext drawingContext, 
   StylusPointCollection stylusPoints, Geometry geometry, Brush fillBrush)
{
   SolidColorBrush iBrush = fillBrush as SolidColorBrush;
   if (iBrush != null)
   {
      float iPressure = (from i in stylusPoints 
                         select i.PressureFactor).Average();
      Color iTemp = iBrush.Color;
      //can't directly change the A of a color that is assigned 
      //to somehting
      iTemp.A = (byte)((float)byte.MaxValue * iPressure);                                          
      base.OnDraw(drawingContext, stylusPoints, geometry, 
         new SolidColorBrush(iTemp));
   }
   else
      base.OnDraw(drawingContext, stylusPoints, geometry, fillBrush);
}
   }
}
