using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using mx.gob.scjn.ius_common.utils;

namespace mx.gob.scjn.ius_common.gui.noticia
{
    /// <summary>
    /// Interaction logic for NoticiaHistorica.xaml
    /// </summary>
    public partial class MarcoJuridico : Page
    {
        public Page Back { get; set; }
        public MarcoJuridico()
        {
            try
            {
                InitializeComponent();
                //FlowDocument DocumentoActual = Contenido.Document;
                //TextRange texto = new TextRange(DocumentoActual.ContentStart, DocumentoActual.ContentEnd);
                Uri lugar = new Uri(IUSConstants.IUS_RUTA_ANEXOS + "noticia/Marco.htm", UriKind.Absolute);
                Contenido.Source = lugar;
                //StreamResourceInfo flujoDeDatos = Application.GetResourceStream(lugar);
                //texto.Load(flujoDeDatos.Stream, DataFormats.Rtf);
            }
            catch (Exception exc)
            {
                MessageBox.Show("Error al cargar el documento:" + exc.Message, "Error en el documento", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void Salir_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack() ;
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
            //if (dialogo.ShowDialog() == true)
            //{
            //    dialogo.PrintDocument(((IDocumentPaginatorSource)this.Contenido.Document).DocumentPaginator, "Imprimir Documento");
            //}
        }

    }
}
