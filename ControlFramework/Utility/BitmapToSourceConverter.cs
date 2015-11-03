using System;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace JaStDev.ControlFramework.Utility
{
   /// <summary>
   /// Converts a <see cref="System.Drawing.Bitmap"/> to a <see cref="System.Windows.Media.Imaging.BitmapSource"/>
   /// that can be used in a WPF application.
   /// </summary>
   public class BitmapToSourceConverter : IValueConverter
   {
      #region IValueConverter Members

      public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
      {
         System.Drawing.Bitmap iBitmap = value as System.Drawing.Bitmap;
         if (iBitmap != null)
         {
            BitmapSource iRes = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(iBitmap.GetHbitmap(),
                                IntPtr.Zero, Int32Rect.Empty, System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());
            return iRes;
         }
         return null;
      }

      public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
      {
         throw new NotImplementedException();
      }

      #endregion
   }
}
