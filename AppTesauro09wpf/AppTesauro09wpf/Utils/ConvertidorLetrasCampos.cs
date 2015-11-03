using System;
using System.Linq;
using System.Windows.Data;
using TesauroUtilities;

namespace AppTesauro09wpf.Utils
{
    class ConvertidorLetrasCampos : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            String indice ="";
            int[] indiceArray = null;
            if (value is int[])
            {
                indiceArray = (int[])value;
            }
            else
            {
                return "";
            }
            foreach (int item in indiceArray)
            {
                switch (item)
                {
                    case Constants.BUSQUEDA_PALABRA_CAMPO_TEXTO:
                        indice += "T";
                        break;
                    case Constants.BUSQUEDA_PALABRA_CAMPO_RUBRO:
                        indice += "R";
                        break;
                    case Constants.BUSQUEDA_PALABRA_CAMPO_PRECE:
                        indice += "P";
                        break;
                    case Constants.BUSQUEDA_PALABRA_CAMPO_LOC:
                        indice += "L";
                        break;
                }
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
