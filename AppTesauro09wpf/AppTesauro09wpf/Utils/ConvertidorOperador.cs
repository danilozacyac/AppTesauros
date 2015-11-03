using System;
using System.Linq;
using System.Windows.Data;
using TesauroUtilities;

namespace AppTesauro09wpf.Utils
{
    public class ConvertidorOperador : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            String indice = "";
            int indiceArray = -1;
            if (value is int)
            {
                indiceArray = (int)value;
            }
            else
            {
                return "";
            }
                switch (indiceArray)
                {
                    case Constants.BUSQUEDA_PALABRA_OP_NO:
                        indice += "NO";
                        break;
                    case Constants.BUSQUEDA_PALABRA_OP_O:
                        indice += "O";
                        break;
                    case Constants.BUSQUEDA_PALABRA_OP_Y:
                        indice += "Y";
                        break;
                }
            return indice;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
        #endregion
    }
}
