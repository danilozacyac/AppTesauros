using System;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace JaStDev.ControlFramework.Utility
{
   /// <summary>
   /// Provides 2 way binding support between a boolean and another value that is compared to the parameter argument.
   /// </summary>
   /// <remarks>
   /// This is usefull to compare to string constants for instance. A common scenario is a 'Checked' property of a toggle button that
   /// needs to be bound to a property.  If this property has a specific value, the button should be checked, and visa versa.
   /// <para>
   /// Note: for converting back from bool to object, only the 'true' value can be correctly interpreted (in which case
   /// the 'parameter' value is returned). Otherwise, <see cref="System.Windows.DependencyProperty.UnsetValue"/> is returned.
   /// </para>
   /// </remarks>
   public class ObjectToBoolConverter : IValueConverter
   {
      #region IValueConverter Members

      public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
      {
         return value.Equals(parameter);
      }

      public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
      {
         if ((bool)value == true) return parameter;
         else return DependencyProperty.UnsetValue;
      }

      #endregion
   }
}
