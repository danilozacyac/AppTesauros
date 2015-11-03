using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows;
using System.Windows.Input;
using mx.gob.scjn.ius_common.fachade;
using mx.gob.scjn.ius_common.TO;
using System.Windows.Media.Imaging;
using System;
using mx.gob.scjn.ius_common.gui.utils;
using System.Windows.Interop;
using System.Windows.Media;
using System.Collections.Generic;
using IUS;
using mx.gob.scjn.ius_common.utils;

namespace mx.gob.scjn.ius_common.gui.apoyos
{
    /// <summary>
    /// Interaction logic for VentanaLeyes.xaml
    /// </summary>
    public partial class VentanaLeyes : UserControl
    {
        public int TipoLey { get; set; }
        public Tesis Padre { get; set; }
#if STAND_ALONE
        public List<String> archivosAsociados { get; set; }
#else
        public String[] archivosAsociados { get; set; }
#endif
        /// <summary>
        /// El documento que se mostrará e imprimirá en en control.
        /// </summary>
        public FlowDocument Documento { get { return this.getDocumento(); } set { this.setDocumento(value); } }
        private FlowDocument documento;
        /// <summary>
        /// Define el identificador de la ley.
        /// </summary>
        public int IdLey { get { return this.getIdLey(); } set { this.setIdLey(value); } }
        private int idLey;
        /// <summary>
        ///     Identificador del artículo, define cual es el artículo dentro de la ley
        ///     que hay que desplegar
        /// </summary>
        /// <value>
        ///     <para>
        ///         El identificador del artículo.
        ///     </para>
        /// </value>
        public int IdArticulo { get { return this.getIdArticulo(); } set { this.setIdArticulo(value); } }
        private int idArticulo;
        /// <summary>
        ///     La referencia con la cual se obtiene solamente lo que se requiere presentar para
        ///     determinada tesis
        /// </summary>
        /// <value>
        ///     <para>
        ///         La referencia
        ///     </para>
        /// </value>
        public int IdRef { get { return this.getIdRef(); } set { this.setIdRef(value); } }
        private int idRef;
        /// <summary>
        ///     Posiciones para mover la ventana
        /// </summary>
        /// <remarks>
        ///     
        /// </remarks>
        private Point inicioDrag;
        /// <summary>
        ///     Como se mueve la ventana
        /// </summary>
        /// <remarks>
        ///     
        /// </remarks>
        private Point ofsetDrag;
        /// <summary>
        ///     El documento que se presenta para imprimir o copiar la ley.
        /// </summary>
        /// <value>
        ///     <para>
        ///         
        ///     </para>
        /// </value>
        /// <remarks>
        ///     
        /// </remarks>
        private DocumentoLeyTO LeyActual { get; set; }
        /// <summary>
        /// Devuelve la instancia actual del documento que tiene la ventana.
        /// </summary>
        /// <returns>El documento que se muestra en el control.</returns>
        public FlowDocument getDocumento()
        {
            return this.documento;
        }
        /// <summary>
        /// Define cual es el documento que se mostrará o imprimirá con este control.
        /// </summary>
        /// <param name="value">El documento a mostrar.</param>
        public void setDocumento(FlowDocument value)
        {
            this.contenidoLey.Document = value;
            this.contenidoLey.IsEnabled = true;
            this.contenidoLey.IsDocumentEnabled = true;
            this.contenidoLey.IsReadOnly = true;
            this.documento = value;
        }
        public VentanaLeyes():base()
        {
            InitializeComponent();
            //this.IsVisible = true;
        }

        private void Salir_MouseLeftButtonUp(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Hidden;
        }

        private void imprimir_MouseLeftButtonUp(object sender, RoutedEventArgs e)
        {
            String contenido = LeyActual.Ley.DescLey;
            Paragraph tituloLey = new Paragraph(new Run(contenido));
            tituloLey.FontSize = 14;
            tituloLey.FontFamily = new FontFamily("Arial");
            tituloLey.FontWeight = FontWeights.Bold;
            contenido = "";
            if (Anexos.Visibility == Visibility.Visible)
            {
                MessageBox.Show(Mensajes.MENSAJE_TABLAS_EXISTENTES, Mensajes.TITULO_TABLAS_EXISTENTES,
                    MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            foreach (ArticulosTO item in LeyActual.Articulo)
            {
                contenido = contenido + item.Info;
            }
            FlowDocument original = contenidoLey.Document;
            List<Paragraph> contenidoLeyParrafo = new List<Paragraph>();
            String[] parrafos = contenido.Split('\n');
            foreach (String itemString in parrafos)
            {
                if (!itemString.Replace('\n', ' ').Trim().Equals(""))
                {
                    Paragraph parrafoActual = new Paragraph(new Run(itemString));
                    parrafoActual.FontFamily = new FontFamily("Arial");
                    parrafoActual.FontSize = 12;
                    contenidoLeyParrafo.Add(parrafoActual);
                    //contenidoLeyParrafo.Add(new Paragraph(new Run("")));
                }
            }
            //contenidoLeyParrafo.FontStretch=
            contenidoLey.Document = new FlowDocument();
            contenidoLey.Document.ColumnWidth = 96 * 7;
            contenidoLey.Document.PagePadding = new Thickness(0.5 * 96);
            contenidoLey.Document.Blocks.Add(tituloLey);
            //contenidoLey.Document.Blocks.Add(new Paragraph(new Run("")));
            foreach (Paragraph item in contenidoLeyParrafo)
            {
                contenidoLey.Document.Blocks.Add(item);
            }
            PrintDialog imprimir = new PrintDialog();
            IDocumentPaginatorSource paginado = contenidoLey.Document as IDocumentPaginatorSource;
            DocumentPaginator pgn = paginado.DocumentPaginator;
            imprimir.UserPageRangeEnabled = true;
            imprimir.MinPage = 1;
            imprimir.MaxPage = (uint)pgn.PageCount;
            Nullable<Boolean> respuesta = imprimir.ShowDialog();
            if (respuesta == true)
            {
                imprimir.PrintDocument(((IDocumentPaginatorSource)(contenidoLey.Document)).DocumentPaginator,"Imprimir Documento");
            }
            contenidoLey.Document = original;
        }

        private void BarraMovimiento_DragEnter(object sender, MouseButtonEventArgs e)
        {
            ofsetDrag = e.GetPosition(this);
            if ((inicioDrag.X == -1)&&(inicioDrag.Y ==-1))
            {
                inicioDrag = e.GetPosition(Parent as Canvas);
                this.BarraMovimiento.CaptureMouse();
            }
        }
        public int getIdLey()
        {
            return this.idLey;
        }
        public void setIdLey(int value)
        {
            this.idLey = value;
        }
        public int getIdArticulo()
        {
            return this.idArticulo;
        }
        public void setIdArticulo(int value)
        {
            this.idArticulo = value;
        }
        public int getIdRef()
        {
            return this.idRef;
        }
        public void setIdRef(int value)
        {
            this.idRef = value;
        }
        public void ActualizaVentana()
        {
#if STAND_ALONE
            FachadaBusquedaTradicional fachada = new FachadaBusquedaTradicional();
            DocumentoLeyTO documento = fachada.getDocumentoLey(this.IdLey, this.IdArticulo, this.IdRef, this.TipoLey);
#else
            FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
            DocumentoLeyTO documento = fachada.getLeyes(this.IdLey, this.IdArticulo, this.IdRef);
#endif
            ArticulosTO articulo = new ArticulosTO();
            articulo.IdLey = "" + this.IdLey;
            articulo.IdRef = "" + this.IdRef;
            articulo.IdArt = "" + this.IdArticulo;
            this.archivosAsociados = fachada.getArchivosLeyes(articulo);
            fachada.Close();
            LeyActual = documento;
            this.contenidoReal.Text = documento.Ley.DescLey;
            FlowDocument documentoNuevo = new FlowDocument();
            foreach(ArticulosTO item in documento.Articulo){
                Paragraph parrafo = new Paragraph(new Run(item.Info));
                documentoNuevo.Blocks.Add(parrafo);
            }
#if STAND_ALONE
            if (this.archivosAsociados.Count > 0)
#else
            if (this.archivosAsociados.Length > 0)
#endif
            {
                this.Anexos.Visibility = Visibility.Visible;
            }
            else
            {
                this.Anexos.Visibility = Visibility.Hidden;
            }
            contenidoLey.Document = documentoNuevo;
        }

        private void BarraMovimiento_DragLeave(object sender, MouseEventArgs e)
        {
            if ((e.LeftButton == MouseButtonState.Pressed)&&(BarraMovimiento.IsMouseCaptured))
            {
                Point puntoActual = e.GetPosition(Parent as Canvas);
                puntoActual.X -= ofsetDrag.X;
                puntoActual.Y -= ofsetDrag.Y;
                //this.VisualOffset.Y = puntoActual.Y + ofsetDrag.Y;
                //this.VisualOffset.X = puntoActual.X + ofsetDrag.X;
                Canvas.SetTop(this,puntoActual.Y );
                Canvas.SetLeft(this,puntoActual.X ) ;
            }
            else
            {
                inicioDrag.X = -1;
                inicioDrag.Y = -1;
            }
        }

        private void BarraMovimiento_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            inicioDrag.X = -1;
            inicioDrag.Y = -1;
            BarraMovimiento.ReleaseMouseCapture();
        }

        private void Copiar_MouseLeftButtonUp(object sender, RoutedEventArgs e)
        {
            if (Anexos.Visibility == Visibility.Visible)
            {
                MessageBox.Show(Mensajes.MENSAJE_TABLAS_EXISTENTES, Mensajes.TITULO_TABLAS_EXISTENTES,
                    MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            FlowDocument original = this.contenidoLey.Document;
            FlowDocument documentoImprimir = new FlowDocument();
            Paragraph parrafo = new Paragraph(new Run(contenidoReal.Text));
            parrafo.FontSize = 12;
            parrafo.FontWeight = FontWeights.Bold;
            documentoImprimir.Blocks.Add(parrafo);
            foreach (ArticulosTO item in LeyActual.Articulo)
            {
                char[] enter = new char[1];
                enter[0] = '\n';
                String[] Parrafos = item.Info.Split(enter);
                foreach (String parrafoAgrega in Parrafos)
                {
                    if (!parrafoAgrega.Replace('\n', ' ').Trim().Equals(""))
                    {
                        parrafo = new Paragraph(new Run(parrafoAgrega));
                        parrafo.FontWeight = FontWeights.Normal;
                        parrafo.TextAlignment = TextAlignment.Justify;
                        documentoImprimir.Blocks.Add(parrafo);
                        documentoImprimir.Blocks.Add(new Paragraph(new Run("")));
                    }
                }
            }
            this.contenidoLey.Document = documentoImprimir;
            this.contenidoLey.SelectAll();
            this.contenidoLey.Copy();
            this.contenidoLey.Document = original;
            MessageBox.Show(Mensajes.MENSAJE_ENVIADO_PORTAPAPELES, Mensajes.TITULO_ENVIADO_PORTAPAPELES,
                MessageBoxButton.OK, MessageBoxImage.Information);

        }


        private void Guardar_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            if (!BrowserInteropHelper.IsBrowserHosted)
            {
               
                    Microsoft.Win32.SaveFileDialog guardaEn = new Microsoft.Win32.SaveFileDialog();
                    guardaEn.DefaultExt = ".rtf";
                    guardaEn.Title = "Guardar un ordenamiento";
                    guardaEn.Filter = "Archivos de Texto Enriquecido|*.rtf";
                    guardaEn.AddExtension = true;
                    FlowDocument documentoOriginal = this.contenidoLey.Document;
                    if ((bool)guardaEn.ShowDialog())
                    {
                         try
                {
                        FlowDocument documentoImprimir = new FlowDocument();
                        Paragraph parrafo = new Paragraph(new Run(contenidoReal.Text));
                        parrafo.FontSize = 12;
                        parrafo.FontWeight = FontWeights.Bold;
                        documentoImprimir.Blocks.Add(parrafo);

                        foreach (ArticulosTO item in LeyActual.Articulo)
                        {
                            char[] enter = new char[1];
                            enter[0] = '\n';
                            String[] Parrafos = item.Info.Split(enter);
                            foreach (String parrafoAgrega in Parrafos)
                            {
                                if (!parrafoAgrega.Replace('\n', ' ').Trim().Equals(""))
                                {
                                    parrafo = new Paragraph(new Run(parrafoAgrega));
                                    parrafo.FontWeight = FontWeights.Normal;
                                    parrafo.TextAlignment = TextAlignment.Justify;
                                    documentoImprimir.Blocks.Add(parrafo);
                                    documentoImprimir.Blocks.Add(new Paragraph(new Run("")));
                                }
                            }
                        }
                        this.contenidoLey.Document = documentoImprimir;
                        System.IO.FileStream archivo = new System.IO.FileStream(guardaEn.FileName, System.IO.FileMode.Create);
                        this.contenidoLey.SelectAll();
                        this.contenidoLey.Selection.Save(archivo, System.Windows.DataFormats.Rtf);
                        archivo.Flush();
                        archivo.Close();
                        
                        MessageBox.Show("El archivo fue guardado como: " + archivo.Name);
                }
                         catch (Exception exc)
                         {
                             MessageBox.Show(Mensajes.MENSAJE_ARCHIVO_ABIERTO, Mensajes.TITULO_ARCHIVO_ABIERTO,
                                   MessageBoxButton.OK, MessageBoxImage.Error);
                         }
                         this.contenidoLey.Document = documentoOriginal;
                    }
                
            }
        }

        private void Anexos_Click(object sender, RoutedEventArgs e)
        {
#if STAND_ALONE
            if (archivosAsociados.Count == 1)
#else
            if (archivosAsociados.Length == 1)
#endif
            {
                String archivo = archivosAsociados[0].Substring(0, archivosAsociados[0].Length - 3);
#if STAND_ALONE
                archivo = "Anexos\\"+ archivo;
#else
                archivo = "Anexos/"+ archivo;
#endif
                archivo += "htm";
                Informe ventanaPresentacion = new Informe(archivo);

                Padre.NavigationService.Navigate(ventanaPresentacion);
            }
            else
            {
                Padre.ventanaAnexos.listado.ItemsSource = archivosAsociados;
                Padre.ventanaAnexos.Visibility = Visibility.Visible;
            }
        }
    }
}
