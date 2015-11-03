using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using mx.gob.scjn.ius_common.utils;

namespace mx.gob.scjn.ius_common.gui.salir
{
    /// <summary>
    /// Interaction logic for Salida.xaml
    /// </summary>
    public partial class Salida : Page
    {
        public Salida()
        {
            InitializeComponent();
            Uri lugar = new Uri(IUSConstants.IUS_RUTA_ANEXOS + "Salir/Aviso.htm", UriKind.Absolute);
            Contenido.Source = lugar;
            
        }

        private void Librerias_Click(object sender, RoutedEventArgs e)
        {
            Uri lugar = new Uri(IUSConstants.IUS_RUTA_ANEXOS + "Salir/directorio.htm", UriKind.Absolute);
            Contenido.Source = lugar;

        }

        private void Catalogo_Click(object sender, RoutedEventArgs e)
        {

            Uri lugar = new Uri(IUSConstants.IUS_RUTA_ANEXOS + "Salir/CATALOGO.htm", UriKind.Absolute);
            Contenido.Source = lugar;
        }

        private void Salir_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            if (BrowserInteropHelper.IsBrowserHosted)
            {
                MessageBox.Show("Por favor cierre la ventana para concluir", "Fin de la Aplicación",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                Application.Current.Shutdown(0);
            }
        }
    }
}
