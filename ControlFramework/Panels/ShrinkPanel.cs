using System;

namespace JaStDev.ControlFramework.Controls
{
   /// <summary>
   /// Note: not yet finisned! A panel which displays it's items Horizontally or Vertically and will try to shrink items as needed to try to fit them in 
   /// the available space.
   /// </summary>
   /// <remarks>
   /// <para>
   /// <see cref="TabPanel.Orientation"/> determins if items are displayed horizontally from left to right or vertically
   /// from top to bottom. When there is not enough room to fit all the items in the available width/height, it will try to shrink all 
   /// items as mush as possible. The property <see cref="TabPanel.AllowOverspil"/> determins if items are shrunk untill they
   /// all fit, or only as much as they allow.
   /// </para>
   /// <para>
   /// This panel is a general purpose implementation of the functionality found in <see cref="TabPanel"/> which is more suited for use
   /// in TabControls.
   /// </para>
   /// </remarks>
   public class ShrinkPanel: TabPanel
   {

     

    
   }
}
