using System;
using System.Linq;
using System.Windows.Data;
using System.Windows.Media;

namespace AppTesauro09wpf.Converters
{
    public class ForegroundConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {

            if (value != null)
            {
                int number = 0;
                int.TryParse(value.ToString(), out number);


                if (number == 1)
                    return new SolidColorBrush(Colors.Blue);
                else if (number == 2)
                    return new SolidColorBrush(Colors.Red);
                if (number == 99)
                    return new SolidColorBrush(Colors.IndianRed);
                else if (number > 0)
                    return new SolidColorBrush(Colors.Orange);
                else
                    return new SolidColorBrush(Colors.Black);

                                    
            }

            return new SolidColorBrush(Colors.Black);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
