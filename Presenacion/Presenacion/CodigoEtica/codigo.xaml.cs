using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using mx.gob.scjn.ius_common.utils;

namespace IUS
{
    /// <summary>
    /// Interaction logic for presentacion.xaml
    /// </summary>
    public partial class codigo : Page
    {
        public codigo()
        {
            try
            {
                InitializeComponent();
                contenido.Source = new Uri(IUSConstants.IUS_RUTA_ANEXOS + "CodigoEtica/CodigoPDF.htm",
                    UriKind.Absolute);
                
                //TextRange rango = new TextRange(contenido.Document.ContentStart, contenido.Document.ContentEnd);
                //Uri lugar = new Uri("/Presenacion;component/presentacion/Presentacion.rtf", UriKind.Relative);
                //Uri lugar = new Uri("/Presenacion;component/presentacion/Presentacion.htm", UriKind.Relative);
                //StreamResourceInfo informacionArchivo = Application.GetResourceStream(lugar);
                //Stream archivo = informacionArchivo.Stream;
                //rango.Load(archivo, DataFormats.Html);
                //contenido.NavigateToStream(archivo);
            }
            catch (Exception e)
            {
                MessageBox.Show("Excepcion al cargar el archivo : " + e.Message);
            }
        }

        private void Salir_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }

        private void Imprimir_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            PrintDialog pd = new PrintDialog();

            if ((pd.ShowDialog() == true))
            {

                //use either one of the below

                //pd.PrintVisual(contenido as Visual, "Imperimiendo Presentación");

          //      pd.PrintDocument((((IDocumentPaginatorSource)contenido.Document).DocumentPaginator),
          //"printing as paginator");

            }
        }

        private void PortaPapeles_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            //TextRange rango = new TextRange(contenido.Document.ContentStart, contenido.Document.ContentEnd);
            //contenido.Copy();
            MessageBox.Show("Texto enviado al portapapeles", "Copia realizada", MessageBoxButton.OK, MessageBoxImage.Asterisk);
        }
    }
}
