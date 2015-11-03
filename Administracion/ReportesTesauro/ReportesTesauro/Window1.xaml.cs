using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ReportesTesauro
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        object actual { get; set; }
        public Window1()
        {
            InitializeComponent();
        }

        private void BtnReporteCE_Click(object sender, RoutedEventArgs e)
        {
            ParametrosReporteCE parametros = new ParametrosReporteCE();
            parametros.SetValue(Grid.ColumnProperty, 1);
            if (actual != null)
            {
                GrdPrincipal.Children.Remove((UIElement)actual);
            }
            actual = parametros;
            GrdPrincipal.Children.Add(parametros);
        }
    }
}
