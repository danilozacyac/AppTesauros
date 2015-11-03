using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace mx.gob.scjn.ius_common.gui.gui.utilities
{
    public class ItemIndexNumberConverter:IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            int indice;
            if (value is int)
            {
                indice = (int)value;
            }
            else
            {
                indice = Int32.Parse((String)value);
            }
            return indice + 1;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }

        #endregion
    }
}
