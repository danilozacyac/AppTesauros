using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using AppTesauro09wpf.UserControl;

namespace AppTesauro09wpf
{
    /// <summary>
    /// Interaction logic for BuscarPorRegistro.xaml
    /// </summary>
    public partial class BuscarPorRegistro : Window
    {
        public BuscarPorRegistro()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void TxtRegistroIus_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = IsTextAllowed(e.Text);
        }

        private static bool IsTextAllowed(string text)
        {
            Regex regex = new Regex("[^0-9.-]+");
            return regex.IsMatch(text);
        }

        private void BtnBuscar_Click(object sender, RoutedEventArgs e)
        {
            TemaToViewModel model = new TemaToViewModel(TxtRegistroIus.Text);
            IusTreeView.DataContext = model.GetTemasPorIus(null);
        }
    }
}
