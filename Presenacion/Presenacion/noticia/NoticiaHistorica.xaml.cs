using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using IUS;
using mx.gob.scjn.ius_common.utils;

namespace mx.gob.scjn.ius_common.gui.noticia
{
    /// <summary>
    /// Interaction logic for NoticiaHistorica.xaml
    /// </summary>
    public partial class NoticiaHistorica : Page
    {
        public Page Back { get; set; }
        public NoticiaHistorica()
        {
            InitializeComponent();
            try
            {
                //FlowDocument DocumentoActual = Contenido.Document;
                //TextRange texto = new TextRange(DocumentoActual.ContentStart, DocumentoActual.ContentEnd);
                Uri lugar = new Uri(IUSConstants.IUS_RUTA_ANEXOS + "noticia/Noticia.htm", UriKind.RelativeOrAbsolute);
                //StreamResourceInfo flujoDeDatos = Application.GetResourceStream(lugar);
                Contenido.Source = lugar;
                //texto.Load(flujoDeDatos.Stream, DataFormats.Rtf);
            }
            catch (Exception exc)
            {
                MessageBox.Show("Error al cargar el documento:" + exc.Message, "Error en el documento", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Introduccion_Click(object sender, RoutedEventArgs e)
        {
            //Uri location = new Uri(@"pack://siteoforigin:,,,/noticia/introduccion.rtf", UriKind.Absolute);
            //FlowDocument DocumentoRTF = new FlowDocument();
            //this.Contenido.Source = location;
        }

        private void Marco_Click(object sender, RoutedEventArgs e)
        {
            MarcoJuridico vantanaNueva = new MarcoJuridico();
            vantanaNueva.Back = this;
            this.NavigationService.Navigate(vantanaNueva);
            //Uri location = new Uri(@"pack://siteoforigin:,,,/noticia/marcojuridico.htm");
            //this.Contenido.Source = location;
        }

        private void Salir_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            if (Back != null)
            {
                this.NavigationService.Navigate(Back);
            }
            else
            {
                this.NavigationService.Navigate(new PaginaInicial());
            }
        }

        private void Ministros_Click(object sender, RoutedEventArgs e)
        {
            SeleccionMinistros paginaSeleccion = new SeleccionMinistros();
            this.NavigationService.Navigate(paginaSeleccion);
        }


        private void Copiar_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            //this.Contenido.SelectAll();
            //this.Contenido.Copy();
            //MessageBox.Show("Documento copiado a portapapeles", "Copia exitosa", MessageBoxButton.OK, MessageBoxImage.Information);
        }


        private void Imprimir_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            //PrintDialog dialogo = new PrintDialog();
            //if (dialogo.ShowDialog()==true)
            //{
            //    dialogo.PrintDocument(((IDocumentPaginatorSource)this.Contenido.Document).DocumentPaginator, "Imprimir Documento");
            //}
        }

    }
}
