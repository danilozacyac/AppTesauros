using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using mx.gob.scjn.ius_common.TO;
using mx.gob.scjn.ius_common.fachade;
using mx.gob.scjn.ius_common.gui.gui.impresion;
using mx.gob.scjn.ius_common.gui.impresion;
using mx.gob.scjn.ius_common.gui.utils;
using mx.gob.scjn.ius_common.utils;

namespace IUS
{
    /// <summary>
    /// Interaction logic for Tesis.xaml
    /// </summary>
    public partial class AcuerdosPagina : Page
    {
        private Xceed.Wpf.DataGrid.DataGridControl ArregloTesis{get;set;}
        private String genealogiaId { get; set; }
#if STAND_ALONE
        private List<AcuerdosPartesTO> partes { get; set; }
#else
        private AcuerdosPartesTO[] partes { get; set; }
#endif
        private int posicion = 0;
        private AcuerdosTO DocumentoActual;
        private int numeroParte { get; set; }
        public Page Back { get; set; }
        public static HashSet<int> marcados;
        public Historial Historia { get {return  this.getHistoria();} set {this.setHistoria(value) ;} }
        private Historial historia;
        private FlowDocument DocumentoParaCopiar { get; set; }
        private BusquedaTO Busqueda { get; set; }
        protected bool EncontradaFrase { get; set; }
        protected bool ExistenTablas { get; set; }
        protected bool verVentanaRangos { get; set; }
        protected bool verVentanaListadoAnexos { get; set; }
        protected bool verFlechas { get; set; }
        protected bool verFontMayor { get; set; }
        protected bool verFontMenor { get; set; }
        /// <summary>
        /// La lista del historial y sus ligas
        /// </summary>
       // private List<IUSHyperlink> ArregloLigas;
        /// <summary>
        /// Constructor por omisión
        /// </summary>
        public AcuerdosPagina()
        {
            InitializeComponent();
            marcados = new HashSet<int>();
        }
        /// <summary>
        /// Muestra un acuerdo solamente, no proviene de una lista de resultados.
        /// </summary>
        /// <param name="id">El identificador del acuerdo</param>
        public AcuerdosPagina(int id)
        {
            InitializeComponent();
            marcados = new HashSet<int>();
            this.contenidoTexto.SetValue(RichTextBox.FontSizeProperty, CalculosPropiedadesGlobales.FontSize);
#if STAND_ALONE
            FachadaBusquedaTradicional fachada = new FachadaBusquedaTradicional();
#else
            FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
#endif
            AcuerdosTO documentoActual = fachada.getAcuerdoPorId(id);
            MostrarDatos(documentoActual);
            DocumentoActual = documentoActual;
            fachada.Close();
            Expresion.Visibility = Visibility.Hidden;
            //Si entran a este constructor es por que vienen para ver solamente un registro
            verFlechas = false;
            this.inicioLista.Visibility = Visibility.Hidden;
            this.anteriorLista.Visibility = Visibility.Hidden;
            this.siguienteLista.Visibility = Visibility.Hidden;
            this.ultimoLista.Visibility = Visibility.Hidden;
            this.regNum.Visibility = Visibility.Hidden;
            this.RegNum.Visibility = Visibility.Hidden;
            this.IrALabel.Visibility = Visibility.Hidden;
            this.IrBoton.Visibility = Visibility.Hidden;
            this.Marcar.Visibility = Visibility.Hidden;
            this.MarcarTodo.Visibility = Visibility.Hidden;
            this.Desmarcar.Visibility = Visibility.Hidden;
        }
        /// <summary>
        /// Constructor en el que se obtienen los datos de una tabla determinada.
        /// </summary>
        /// <param name="records">Los registros a mostrar</param>
        public AcuerdosPagina(Xceed.Wpf.DataGrid.DataGridControl records, BusquedaTO busqueda)
        {
            InitializeComponent();
            marcados = new HashSet<int>();
            CalculosPropiedadesGlobales.FontSize = Constants.FONTSIZE; 
            Busqueda = busqueda;
            this.contenidoTexto.SetValue(RichTextBox.FontSizeProperty, CalculosPropiedadesGlobales.FontSize);
            AcuerdoSimplificadoTO acuerdosMostrar = (AcuerdoSimplificadoTO)records.SelectedItem;
            posicion = records.SelectedIndex;
#if STAND_ALONE
            FachadaBusquedaTradicional fachada = new FachadaBusquedaTradicional();
#else
            FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
#endif
            AcuerdosTO documentoActual = fachada.getAcuerdoPorId(Int32.Parse(acuerdosMostrar.Id));
            fachada.Close();
            if (busqueda != null)
            {
                Expresion.Content = CalculosGlobales.Expresion(busqueda);
            }
            else
            {
                List<int> registros = new List<int>();
                Expresion.Content = CalculosGlobales.Expresion(registros);
            }
            this.ArregloTesis = records;
            if (ArregloTesis.Items.Count < 2)
            {
                verFlechas = false;
                this.inicioLista.Visibility = Visibility.Hidden;
                this.anteriorLista.Visibility = Visibility.Hidden;
                this.siguienteLista.Visibility = Visibility.Hidden;
                this.ultimoLista.Visibility = Visibility.Hidden;
                this.regNum.Visibility = Visibility.Hidden;
                this.RegNum.Visibility = Visibility.Hidden;
                this.IrALabel.Visibility = Visibility.Hidden;
                this.IrBoton.Visibility = Visibility.Hidden;

            }
            MostrarDatos(documentoActual);
        }
        /// <summary>
        /// Muestra los datos de una ejecutoria determinada.
        /// </summary>
        private void MostrarDatos(AcuerdosTO documentoActual){
            DocumentoActual = documentoActual;
            this.AcuerdoLabel.Content = documentoActual.Tesis;
            this.PaginaLabel.Content = "Página: "+documentoActual.Pagina;
            this.EpocaLabel.Content = documentoActual.Epoca;
            if ((documentoActual.ParteT.Equals("156")) || (documentoActual.ParteT.Equals("163")))
            {
                WindowTitle = "Otros";
            }
            this.fuenteLabel.Content = documentoActual.Fuente;
            this.SalaLabel.Text = documentoActual.Sala;
            this.IdLabel.Content = documentoActual.Id;
            this.VolumenLabel.Content = documentoActual.Volumen;
            FlowDocument documentoPrecedentes = new FlowDocument();
            FlowDocument documentoRubro = new FlowDocument();
            this.contenidoTexto.Document = documentoRubro;
#if STAND_ALONE
            FachadaBusquedaTradicional fachada = new FachadaBusquedaTradicional();
#else
            FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
#endif
            partes = fachada.getAcuerdoPartesPorId(Int32.Parse(documentoActual.Id));
            Paragraph textoParrafo = ObtenLigas(documentoActual.Rubro + "\n\n" + partes[0].TxtParte, documentoActual.Id, 1);
            textoParrafo.TextAlignment = TextAlignment.Justify;
            textoParrafo.FontWeight = FontWeights.Normal;
            documentoRubro.Blocks.Add(textoParrafo);
            numeroParte = 0;
            //textoParrafo=ObtenLigas(partes[0].TxtParte, documentoActual.Id, 1);
            //textoParrafo = new Paragraph(new Run(partes[0].TxtParte));
            //documentoRubro.Blocks.Add(textoParrafo);
            //this.documentosVotos = fachada.getVotoPorEjecutoria(documentoActual.Id);
#if STAND_ALONE
            if (partes.Count <= 1)
#else
            if (partes.Length <= 1)
#endif
            {
                parteSiguiente.Visibility = Visibility.Hidden;
                parteAnterior.Visibility = Visibility.Hidden;
                parteFinal.Visibility = Visibility.Hidden;
                parteInicio.Visibility = Visibility.Hidden;
                NumeroPartes.Visibility = Visibility.Hidden;
                docCompletoImage.Visibility = Visibility.Hidden;
            }
            else
            {
                parteSiguiente.Visibility = Visibility.Visible;
                parteAnterior.Visibility = Visibility.Visible;
                parteFinal.Visibility = Visibility.Visible;
                parteInicio.Visibility = Visibility.Visible;
                NumeroPartes.Visibility = Visibility.Visible;
#if STAND_ALONE
                NumeroPartes.Content = "Parte: 1 / " + partes.Count;
#else
                NumeroPartes.Content = "Parte: 1 / " + partes.Length;
#endif
                docCompletoImage.Visibility = Visibility.Visible;
            }
            this.contenidoTexto.Document = documentoRubro;
            if ((Busqueda != null) && (Busqueda.Palabra != null))
            {
                foreach (BusquedaPalabraTO item in Busqueda.Palabra)
                {
                    List<String> listapalabras = FlowDocumentHighlight.obtenPalabras(item);
                    this.contenidoTexto.Document = FlowDocumentHighlight.
                        imprimeToken(contenidoTexto.Document,
                                     listapalabras, Brushes.Red);
                    List<String> frases = FlowDocumentHighlight.obtenFrases(item);
                    documentoRubro = FlowDocumentHighlight.imprimeToken(documentoRubro, frases, Brushes.DarkGreen);
                }
            }
            this.contenidoTexto.IsReadOnly = true;
            fachada.Close();
            bool encontrado = false;
            bool encontradoverdadero = false;
            bool buscar = false;
            if ((Busqueda != null) && (Busqueda.Palabra != null))
            {
                foreach (BusquedaPalabraTO itemPalabra in Busqueda.Palabra)
                {
                    buscar = (itemPalabra.Campos.Contains(Constants.BUSQUEDA_PALABRA_CAMPO_TEXTO)
                        || itemPalabra.Campos.Contains(Constants.BUSQUEDA_PALABRA_CAMPO_RUBRO)
                        || itemPalabra.Campos.Contains(Constants.BUSQUEDA_PALABRA_CAMPO_ASUNTO));
                }
            }
            numeroParte = 0;
            while (!encontrado)
            {
                if (buscar)
                {
                    encontrado = FlowDocumentHighlight.UbicarPrimera(contenidoTexto);
                    encontradoverdadero = encontrado;
                }
                else
                {
                    encontrado = true;
                }
#if STAND_ALONE
                if ((!encontrado) && (numeroParte < (partes.Count - 1)))
                {
                    if (numeroParte < (partes.Count - 1))
                    {
#else
                if ((!encontrado) && (numeroParte < (partes.Length - 1)))
                {
                    if (numeroParte < (partes.Length - 1))
                    {
#endif
                        numeroParte++;
                    }
                    actualizaTexto();
                }
                else
                {
                    encontrado = true;
                }
            }
#if STAND_ALONE
            if (!encontradoverdadero && (numeroParte == (partes.Count - 1)))
#else
            if (!encontradoverdadero && (numeroParte == (partes.Length - 1)))
#endif
            {
                numeroParte = 0;
                actualizaTexto();
            } 
            if (ArregloTesis != null)
            {
                List<AcuerdoSimplificadoTO> lista = (List<AcuerdoSimplificadoTO>)(ArregloTesis.ItemsSource);
                int posicionReal = posicion + 1;
                RegNum.Content = "" + posicionReal + " / " + lista.Count;
                DocumentoActual = documentoActual;
                //impresion.Visibility = Visibility.Hidden;
                tabControl1.Visibility = Visibility.Visible;
                //ventanaEmergente.Visibility = Visibility.Hidden;
            }
            else
            {
                posicion = 0;
                tabControl1.Visibility = Visibility.Visible;
            }
            textoAbuscar.Text = Constants.CADENA_VACIA;
            this.Buscar.Visibility = Visibility.Visible;
            this.textoAbuscar.Visibility = Visibility.Visible;
            this.imprimePapel.Visibility = Visibility.Hidden;
            this.impresion.Visibility = Visibility.Hidden;
            if (marcados.Contains(Int32.Parse(documentoActual.Id)))
            {
                Image imagenNueva = new Image();
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri("images/PALOMA1.png", UriKind.Relative);
                bitmap.EndInit();
                this.Marcar.Source = bitmap;
            }
            else
            {
                Image imagenNueva = new Image();
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri("images/MARCAR1.png", UriKind.Relative);
                bitmap.EndInit();
                this.Marcar.Source = bitmap;
            }
            List<object> items = new List<object>();
            foreach (TabItem item in tabControl1.Items)
            {
                items.Add(item);
            }
            foreach (object item in items)
            {
                if (item != TabTexto)
                {
                    tabControl1.Items.Remove(item);
                }
            }
        }


        private void historial_MouseButtonDown(object sender, RoutedEventArgs e)
        {

        }

        private void Imprimir_MouseButtonDown(object sender, RoutedEventArgs e)
        {
            if (BtnTablas.Visibility == Visibility.Visible)
            {
                MessageBox.Show(Mensajes.MENSAJE_TABLAS_EXISTENTES, Mensajes.TITULO_TABLAS_EXISTENTES,
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            if (imprimePapel.Visibility == Visibility.Hidden)
            {
                DocumentoAcuerdo documento;
                MessageBoxResult resultadoMsgBox = MessageBoxResult.Cancel;
                if ((marcados != null) && (marcados.Count > 0))
                {
                    resultadoMsgBox = MessageBox.Show(Mensajes.MENSAJE_MARCADOS_ACCION,
                        Mensajes.TITULO_MARCADOS_ACCION, MessageBoxButton.YesNoCancel,
                        MessageBoxImage.Question);
                }
                else
                {
                    resultadoMsgBox = MessageBoxResult.No;
                }
                if (resultadoMsgBox.Equals(MessageBoxResult.No))
                {
                    documento = new DocumentoAcuerdo(DocumentoActual, numeroParte+1);
                }
                else if (resultadoMsgBox.Equals(MessageBoxResult.Yes))
                {
                    documento = new DocumentoAcuerdo(marcados);
                }
                else
                {
                    return;
                }
                //FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
                //object documentoXps = fachada.getDocumentoTesis(this.DocumentoActual.Ius);
                DocumentoParaCopiar = documento.Copia;
                impresion.Document = documento.Documento; //(IDocumentPaginatorSource)documentoXps;
                impresion.Visibility = Visibility.Visible;
                impresion.Background = Brushes.White;
                tabControl1.Visibility = Visibility.Hidden;
                imprimePapel.Visibility = Visibility.Visible;
                textoAbuscar.Visibility = Visibility.Hidden;
                Buscar.Visibility = Visibility.Hidden;
                Expresion.Visibility = Visibility.Hidden;
                MuestraVistaPrel();
            }
            else
            {
                impresion.Visibility = Visibility.Hidden;
                //impresion.Background = Brushes.Transparent;
                tabControl1.Visibility = Visibility.Visible;
                imprimePapel.Visibility = Visibility.Hidden;
                textoAbuscar.Visibility = Visibility.Visible;
                Buscar.Visibility = Visibility.Visible;
                Expresion.Visibility = Visibility.Visible;
                EscondeVistaPrel();
            }
        }
        #region marcarTodo

        private void MarcarTodo_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            int faltantes = 50 - marcados.Count;
            if (faltantes > this.ArregloTesis.Items.Count)
            {
                // El número de documentos en el arreglo cabe en los faltantes
                foreach (AcuerdoSimplificadoTO Item in ArregloTesis.Items)
                {
                    marcados.Add(Int32.Parse(Item.Id));
                }
                MessageBox.Show(Mensajes.MENSAJE_TODOS_PORTAPAPELES, Mensajes.TITULO_TODOS_PORTAPAPELES,
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                this.ventanaRangos.Visibility = Visibility.Visible;
                this.ventanaRangos.InicioRango = 1;
                this.ventanaRangos.FinRango = 50;
                this.ventanaRangos.DiferenciaRangos = faltantes;
                this.ventanaRangos.StrMensaje = "Defina el rango de los registros a marcar";
                this.ventanaRangos.Contenedor = this;
                this.ventanaRangos.RegistroFinal = this.ArregloTesis.Items.Count;
            }
        }
        #endregion
        #region desmarcar
        private void Desmarcar_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            MessageBoxResult resultado = MessageBoxResult.Yes;
            if (marcados.Count > 0)
            {
                resultado = MessageBox.Show(Mensajes.MENSAJE_DESMARCAR_TODO,
                    Mensajes.TITULO_DESMARCAR_TODO, MessageBoxButton.YesNo, MessageBoxImage.Question);
            }
            if (resultado == MessageBoxResult.Yes)
            {
                //marcados.Remove(Int32.Parse(DocumentoActual.Ius));
                marcados.Clear();
                Image imagenNueva = new Image();
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri("images/MARCAR1.png", UriKind.Relative);
                bitmap.EndInit();
                this.Marcar.Source = bitmap;
                MessageBox.Show(Mensajes.MENSAJE_SIN_MARCAS, Mensajes.TITULO_SIN_MARCAS,
                    MessageBoxButton.OK, MessageBoxImage.Information);
                this.Marcar.ToolTip = Mensajes.TOOLTIP_SIN_MARCAR;
            }
        }
        #endregion
        #region marcar
        private void Marcar_MouseEnter(object sender, MouseEventArgs e)
        {
            if (!marcados.Contains(Int32.Parse(this.DocumentoActual.Id)))
            {
                Image imagenNueva = new Image();
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri("images/MARCAR2.png", UriKind.Relative);
                bitmap.EndInit();
                this.Marcar.Source = bitmap;
            }
            else
            {
                Image imagenNueva = new Image();
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri("images/PALOMA1.png", UriKind.Relative);
                bitmap.EndInit();
                this.Marcar.Source = bitmap;
            }
        }

        private void Marcar_MouseLeave(object sender, MouseEventArgs e)
        {
            if (!marcados.Contains(Int32.Parse(this.DocumentoActual.Id)))
            {
                Image imagenNueva = new Image();
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri("images/MARCAR1.png", UriKind.Relative);
                bitmap.EndInit();
                this.Marcar.Source = bitmap;
            }
            else
            {
                Image imagenNueva = new Image();
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri("images/PALOMA1.png", UriKind.Relative);
                bitmap.EndInit();
                this.Marcar.Source = bitmap;
            }
        }

        private void Marcar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (marcados.Count <= 50)
            {
                marcados.Add(Int32.Parse(DocumentoActual.Id));
                Image imagenNueva = new Image();
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri("images/PALOMA1.png", UriKind.Relative);
                bitmap.EndInit();
                this.Marcar.Source = bitmap;
            }
            else
            {
                MessageBox.Show("El documento no se puede marcar ya que hay 50 documentos guardados", "Limite de marcados alcanzado", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        #endregion
        #region buscar
        private void Buscar_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            String validar = Validadores.BusquedaPalabraDocumento(this.textoAbuscar);
            if (!validar.Equals(Constants.CADENA_VACIA))
            {
                return;
            }
            String busquedaTexto = this.textoAbuscar.Text;
            TextPointer inicio = this.contenidoTexto.Selection.Start;
            TextPointer final = this.contenidoTexto.Selection.End;
            TextPointer lugarActual = null;
            if (inicio.CompareTo(final) == 0)
            {
                lugarActual = this.contenidoTexto.Document.ContentStart;
            }
            else
            {
                lugarActual = this.contenidoTexto.Selection.End;
            }
            lugarActual = FlowDocumentHighlight.Find(busquedaTexto, this.contenidoTexto, lugarActual);
            if (lugarActual.CompareTo(this.contenidoTexto.Document.ContentEnd) == 0)
            {
                if (EncontradaFrase)
                {
                    MessageBox.Show(Mensajes.MENSAJE_NO_HAY_MAS_COINCIDENCIAS, Mensajes.TITULO_NO_HAY_MAS_COINCIDENCIAS,
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show(Mensajes.MENSAJE_NO_HAY_COINCIDENCIAS, Mensajes.TITULO_NO_HAY_COINCIDENCIAS,
                        MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
                TextRange rango = new TextRange(contenidoTexto.Document.ContentStart, contenidoTexto.Document.ContentStart);
                rango.Select(contenidoTexto.Document.ContentStart, contenidoTexto.Document.ContentStart);
            }
            else
            {
                EncontradaFrase = true;
                inicio = this.contenidoTexto.Selection.Start;
                final = this.contenidoTexto.Selection.End;
                this.contenidoTexto.Selection.Select(inicio, final);
                this.contenidoTexto.Focus();
            }
        }
        #endregion
        #region imprimepapel
        private void imprimePapel_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            PrintDialog dialogoImpresion = new PrintDialog();
            IDocumentPaginatorSource paginado = impresion.Document as IDocumentPaginatorSource;
            DocumentPaginator pgn = paginado.DocumentPaginator;
            dialogoImpresion.UserPageRangeEnabled = true;
            dialogoImpresion.MinPage = 1;
            dialogoImpresion.MaxPage = (uint)pgn.PageCount;
            Nullable<Boolean> respuesta = dialogoImpresion.ShowDialog();
            //Debe mandar a la impresora con las opciones del usuario
            if (respuesta == true)
            {
                try
                {
                    if (dialogoImpresion.PageRangeSelection == PageRangeSelection.UserPages)
                    {
                        ((DocumentPaginatorWrapper)pgn).PaginaInicial = dialogoImpresion.PageRange.PageFrom;
                        ((DocumentPaginatorWrapper)pgn).PaginaFinal = dialogoImpresion.PageRange.PageTo;
                    }
                    dialogoImpresion.PrintDocument(pgn, "Impresión de acuerdos/otros");

                    EscondeVistaPrel();
                }
                catch (Exception exc)
                {
                    MessageBox.Show(Mensajes.MENSAJE_IMPRESORA, Mensajes.TITULO_ARCHIVO_ABIERTO,
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        #endregion
        private void textoAbuscar_TextChanged(object sender, TextChangedEventArgs e)
        {
            EncontradaFrase = false;
            this.contenidoTexto.Selection.Select(contenidoTexto.Document.ContentStart, contenidoTexto.Document.ContentStart);
        }
        public void ActualizaRangoMarcado(int inicio, int final)
        {
            for (int contador = inicio; contador <= final; contador++)
            {
                AcuerdoSimplificadoTO agregado = (AcuerdoSimplificadoTO)this.ArregloTesis.Items.GetItemAt(contador - 1);
                marcados.Add(Int32.Parse(agregado.Id));
            }
            if (marcados.Contains(Int32.Parse(DocumentoActual.Id)))
            {
                marcados.Add(Int32.Parse(DocumentoActual.Id));
                Image imagenNueva = new Image();
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri("images/PALOMA1.png", UriKind.Relative);
                bitmap.EndInit();
                this.Marcar.Source = bitmap;
            }
        }
        private void inicioLista_MouseButtonDown(object sender, RoutedEventArgs e)
        {
            List<AcuerdoSimplificadoTO> presentadorDatos = (List<AcuerdoSimplificadoTO>)this.ArregloTesis.ItemsSource;
            AcuerdosTO acuerdoMostrar = new AcuerdosTO();
            acuerdoMostrar.Id = presentadorDatos[0].Id;
            posicion = 0;
            this.ArregloTesis.SelectedIndex = posicion;
            this.ArregloTesis.BringItemIntoView(this.ArregloTesis.SelectedItem);
#if STAND_ALONE
            FachadaBusquedaTradicional fachada = new FachadaBusquedaTradicional();
#else
            FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
#endif
            AcuerdosTO documentoActual = fachada.getAcuerdoPorId(Int32.Parse(acuerdoMostrar.Id));
            fachada.Close();
            MostrarDatos(documentoActual);
        }

        private void anteriorLista_MouseButtonDown(object sender, RoutedEventArgs e)
        {
            List<AcuerdoSimplificadoTO> presentadorDatos = (List<AcuerdoSimplificadoTO>)this.ArregloTesis.ItemsSource;
            AcuerdosTO acuerdoMostrar = null;
            if (posicion == 0)
            {
                acuerdoMostrar = new AcuerdosTO();
                acuerdoMostrar.Id = presentadorDatos[0].Id;
            }
            else
            {
                posicion--;
                acuerdoMostrar = new AcuerdosTO();
                acuerdoMostrar.Id = presentadorDatos[posicion].Id;
            }
#if STAND_ALONE
            FachadaBusquedaTradicional fachada = new FachadaBusquedaTradicional();
#else
            FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
#endif
            AcuerdosTO documentoActual = fachada.getAcuerdoPorId(Int32.Parse(acuerdoMostrar.Id));
            fachada.Close();
            this.ArregloTesis.SelectedIndex = posicion;
            this.ArregloTesis.BringItemIntoView(this.ArregloTesis.SelectedItem);
            MostrarDatos(documentoActual);
        }

        private void siguienteLista_MouseButtonDown(object sender, RoutedEventArgs e)
        {
            List<AcuerdoSimplificadoTO> presentadorDatos = (List<AcuerdoSimplificadoTO>)this.ArregloTesis.ItemsSource;
            AcuerdosTO acuerdoMostrar = null;

            if (posicion >= presentadorDatos.Count - 1)
            {
                posicion = presentadorDatos.Count - 1;
                acuerdoMostrar = new AcuerdosTO();
                acuerdoMostrar.Id = presentadorDatos[posicion].Id;
            }
            else
            {
                posicion++;
                acuerdoMostrar = new AcuerdosTO();
                acuerdoMostrar.Id = presentadorDatos[posicion].Id;
            }
#if STAND_ALONE
            FachadaBusquedaTradicional fachada = new FachadaBusquedaTradicional();
#else
            FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
#endif
            AcuerdosTO documentoActual = fachada.getAcuerdoPorId(Int32.Parse(acuerdoMostrar.Id));
            fachada.Close();
            this.ArregloTesis.SelectedIndex = posicion;
            this.ArregloTesis.BringItemIntoView(this.ArregloTesis.SelectedItem);
            MostrarDatos(documentoActual);
        }

        private void ultimoLista_MouseButtonDown(object sender, RoutedEventArgs e)
        {
            List<AcuerdoSimplificadoTO> presentadorDatos = (List<AcuerdoSimplificadoTO>)this.ArregloTesis.ItemsSource;
            AcuerdosTO acuerdoMostrar = null;
            posicion = presentadorDatos.Count - 1;
            this.ArregloTesis.SelectedIndex = posicion;
            this.ArregloTesis.BringItemIntoView(this.ArregloTesis.SelectedItem);
            acuerdoMostrar = new AcuerdosTO();
            acuerdoMostrar.Id = presentadorDatos[posicion].Id;
#if STAND_ALONE
            FachadaBusquedaTradicional fachada = new FachadaBusquedaTradicional();
#else
            FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
#endif
            AcuerdosTO documentoActual = fachada.getAcuerdoPorId(Int32.Parse(acuerdoMostrar.Id));
            fachada.Close();
            MostrarDatos(documentoActual);
        }

        private void regNum_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
        private void Salir_MouseButtonDown(object sender, RoutedEventArgs e)
        {
            if (Back == null)
            {
                this.NavigationService.GoBack();
            }
            else
            {
                this.NavigationService.Navigate(Back);
            }
        }

        private void parteInicio_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            numeroParte = 0;
            actualizaTexto();
        }

        private void parteAnterior_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            if (numeroParte > 0)
            {
                numeroParte--;
            }
            actualizaTexto();
        }

        private void parteSiguiente_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
#if STAND_ALONE
            if (numeroParte < (partes.Count-1))
            {
#else
            if (numeroParte < (partes.Length-1))
            {
#endif
                numeroParte++;
            }
            actualizaTexto();
        }

        private void parteFinal_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
#if STAND_ALONE
            numeroParte = partes.Count - 1;
#else
            numeroParte = partes.Length-1;
#endif
            actualizaTexto();
        }

        private void actualizaTexto()
        {
            String textoImpreso = null;
            if (numeroParte == 0)
            {
                textoImpreso = DocumentoActual.Rubro +"\n\n" + partes[numeroParte].TxtParte;
            }
            else
            {
                textoImpreso = partes[numeroParte].TxtParte;
            }
            FlowDocument documentoRubro = new FlowDocument();
            Paragraph textoParrafo = ObtenLigas(textoImpreso,DocumentoActual.Id,numeroParte+1);
            textoParrafo.FontWeight=FontWeights.Normal;
            textoParrafo.TextAlignment=TextAlignment.Justify;
            documentoRubro.Blocks.Add(textoParrafo);
            int parteReal = numeroParte + 1;
#if STAND_ALONE
            NumeroPartes.Content = "Parte: " + parteReal + " / " + partes.Count;
#else
            NumeroPartes.Content = "Parte: " + parteReal + " / " + partes.Length;
#endif
            if ((Busqueda != null) && (Busqueda.Palabra != null))
            {
                foreach (BusquedaPalabraTO item in Busqueda.Palabra)
                {
                    List<String> listapalabras = FlowDocumentHighlight.obtenPalabras(item);
                    documentoRubro = FlowDocumentHighlight.
                        imprimeToken(documentoRubro,
                                     listapalabras, Brushes.Red);
                    List<String> frases = FlowDocumentHighlight.obtenFrases(item);
                    documentoRubro = FlowDocumentHighlight.imprimeToken(documentoRubro, frases, Brushes.DarkGreen);
                }

            }
            this.contenidoTexto.Document=documentoRubro;
        }

        private void docCompletoImage_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            FlowDocument documentoRubro = new FlowDocument();
            //if (documentosTesis.Length == 0)
            //{
                documentoRubro.Blocks.Add(new Paragraph(new Run(DocumentoActual.Rubro)));
            //}
                int contador = 1;
                String TextoCompleto = "";
            foreach (AcuerdosPartesTO item in partes)
            {
                TextoCompleto += item.TxtParte;
                contador++;
            }
            Paragraph textoParrafo = ObtenLigas(TextoCompleto, DocumentoActual.Id, -1);
            contador++;
            textoParrafo.FontWeight = FontWeights.Normal;
            textoParrafo.TextAlignment = TextAlignment.Justify;
            documentoRubro.Blocks.Add(textoParrafo);
            if ((Busqueda != null) && (Busqueda.Palabra != null))
            {
                foreach (BusquedaPalabraTO item in Busqueda.Palabra)
                {
                    List<String> listapalabras = FlowDocumentHighlight.obtenPalabras(item);
                    documentoRubro = FlowDocumentHighlight.
                        imprimeToken(documentoRubro,
                                     listapalabras, Brushes.Red);
                    List<String> frases = FlowDocumentHighlight.obtenFrases(item);
                    documentoRubro = FlowDocumentHighlight.imprimeToken(documentoRubro, frases, Brushes.DarkGreen);
                }
            }
            numeroParte = -2;
            this.contenidoTexto.Document = documentoRubro;
            parteSiguiente.Visibility = Visibility.Hidden;
            parteAnterior.Visibility = Visibility.Hidden;
            parteFinal.Visibility = Visibility.Hidden;
            parteInicio.Visibility = Visibility.Hidden;
            NumeroPartes.Visibility = Visibility.Hidden;
            docCompletoImage.Visibility = Visibility.Hidden;
        }

        private void IrBoton_Click(object sender, RoutedEventArgs e)
        {
            if (regNum.Text.Equals(""))
            {
                MessageBox.Show(Mensajes.MENSAJE_CAMPO_TEXTO_NUMERO_VACIO, Mensajes.TITULO_CAMPO_REQUERIDO,
                    MessageBoxButton.OK, MessageBoxImage.Error);
                regNum.Focus();
                return;
            }
            int registro = Int32.Parse(regNum.Text);
            List<AcuerdoSimplificadoTO> arregloAcuerdosActual = (List<AcuerdoSimplificadoTO>)ArregloTesis.ItemsSource;
            if (registro > 0 && registro <= arregloAcuerdosActual.Count)
            {
                registro--;
                AcuerdoSimplificadoTO acuerdoActual = arregloAcuerdosActual[registro];
#if STAND_ALONE
                FachadaBusquedaTradicional fachada = new FachadaBusquedaTradicional();
#else
            FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
#endif
                AcuerdosTO acuerdoCompleta = fachada.getAcuerdoPorId(Int32.Parse(acuerdoActual.Id));
                fachada.Close();
                posicion = registro;
                MostrarDatos(acuerdoCompleta);
                regNum.Text = "";
            }
            else
            {
                regNum.Text = "";
                MessageBox.Show(Mensajes.MENSAJE_CONSECUTIVO_NO_VALIDO,Mensajes.TITULO_CONSECUTIVO_NO_VALIDO,
                    MessageBoxButton.OK,MessageBoxImage.Exclamation);
            }

        }

        /// <summary>
        /// Genera el procedimiento de poner el historial al día incluyendo a la tesis en caso de no tenerla.
        /// </summary>
        /// <param name="value">El historial que se usará.</param>
        protected void setHistoria(Historial value)
        {
            this.historia = value;
            IUSNavigationService entradaHistorial = new IUSNavigationService();
            entradaHistorial.Id = Int32.Parse(this.DocumentoActual.Id);
            entradaHistorial.ParametroConstructor = this;
            entradaHistorial.TipoVentana = IUSNavigationService.ACUERDO;
            revisaHistorial(entradaHistorial);
            historia.NavigationProvider = this;
            //foreach (IUSHyperlink item in ArregloLigas)
            //{
            //    item.Historia = value;
            //}
        }
        /// <summary>
        /// Obtiene el historial que se está usando.
        /// </summary>
        /// <returns>El historial actual.</returns>
        protected Historial getHistoria()
        {
            return this.historia;
        }
        #region fontmenor
        private void FontMenor_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            FontMayor.Visibility = Visibility.Visible;
            verFontMayor = true;
            if (CalculosPropiedadesGlobales.FontSize > Constants.FONT_MENOR)
            {
                //this.contenidoTexto.SelectAll();
                CalculosPropiedadesGlobales.FontSize--;
                this.contenidoTexto.SetValue(RichTextBox.FontSizeProperty, CalculosPropiedadesGlobales.FontSize);
            }
            else
            {
                FontMenor.Visibility = Visibility.Hidden;
                verFontMenor = false;
            }
        }

        #endregion
        /// <summary>
        /// Verifica que nuestro documento actual esté dentro del lugar que le corresponde en el
        /// historial de documentos
        /// </summary>
        /// <param name="entrada">El servicio para generar ligas, contiene los datos 
        /// del documento.</param>
        public void revisaHistorial(IUSNavigationService entrada)
        {
            Boolean encontrado = false;
            if (Historia.Lista == null)
            {
                List<IUSNavigationService> lista = new List<IUSNavigationService>();
                Historia.Lista = lista;
            }
            foreach (IUSNavigationService item in Historia.Lista)
            {
                encontrado = ((item.Id == entrada.Id) && (item.TipoVentana == IUSNavigationService.TESIS)) || encontrado;
            }
            if (!encontrado)
            {
                Historia.Lista.Add(entrada);
            }
        }

        #region  fontselect
        private void FontSelec_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            FontMenor.Visibility = Visibility.Visible;
            verFontMenor = true;
            if (CalculosPropiedadesGlobales.FontSize < Constants.FONT_MAYOR)
            {
                //this.contenidoTexto.SelectAll();
                CalculosPropiedadesGlobales.FontSize++;
                this.contenidoTexto.SetValue(RichTextBox.FontSizeProperty, CalculosPropiedadesGlobales.FontSize);
            }
            else
            {
                FontMayor.Visibility = Visibility.Hidden;
                verFontMayor = true;
            }
        }
        #endregion
        #region guardar
        private void Guardar_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            if (!BrowserInteropHelper.IsBrowserHosted)
            {
                Imprimir_MouseButtonDown(sender, e);
                Microsoft.Win32.SaveFileDialog guardaEn = new Microsoft.Win32.SaveFileDialog();
                guardaEn.DefaultExt = ".rtf";
                guardaEn.Title = "Guardar un acuerdo";
                guardaEn.Filter = "Archivos de Texto Enriquecido|*.rtf";
                guardaEn.AddExtension = true;
                EscondeVistaPrel();
                if ((bool)guardaEn.ShowDialog())
                {
                    FlowDocument documentoImprimir = DocumentoParaCopiar;
                    impresion.Document = null;
                    this.RtbCopyPaste.Document = documentoImprimir;
                    try
                    {
                        System.IO.FileStream archivo = new System.IO.FileStream(guardaEn.FileName, System.IO.FileMode.Create);
                        this.RtbCopyPaste.SelectAll();
                        this.RtbCopyPaste.Selection.Save(archivo, System.Windows.DataFormats.Rtf);
                        archivo.Flush();
                        archivo.Close();
                        MessageBox.Show(Mensajes.MENSAJE_ARCHIVO_GUARDADO + archivo.Name,
                            Mensajes.TITULO_ARCHIVO_GUARDADO, MessageBoxButton.OK,
                            MessageBoxImage.Information);
                    }
                    catch (Exception exc)
                    {
                        MessageBox.Show(Mensajes.MENSAJE_ARCHIVO_ABIERTO, Mensajes.TITULO_ARCHIVO_ABIERTO,
                           MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    MostrarDatos(DocumentoActual);
                }
            }
            else
            {
                Imprimir_MouseButtonDown(sender, e);
                FlowDocument documentoImprimir = DocumentoParaCopiar;
                impresion.Document = null;
                this.contenidoTexto.Document = documentoImprimir;
                System.IO.IsolatedStorage.IsolatedStorageFileStream archivo = new System.IO.IsolatedStorage.
                    IsolatedStorageFileStream("texto.rtf", System.IO.FileMode.Create);
                this.contenidoTexto.SelectAll();
                this.contenidoTexto.Selection.Save(archivo, System.Windows.DataFormats.Text);
                archivo.Flush();
                archivo.Close();
                MostrarDatos(DocumentoActual);
                MessageBox.Show(Mensajes.MENSAJE_GUARDADO + archivo.Name,
                    Mensajes.TITULO_GUARDADO,
                    MessageBoxButton.OK,
                    MessageBoxImage.Exclamation);
            }
        }
        #endregion
        #region portapapeles
        private void PortaPapeles_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            //FlowDocument documentoOriginal = this.contenidoTexto.Document;
            Imprimir_MouseButtonDown(sender, e);
            FlowDocument documentoImprimir = DocumentoParaCopiar;
            impresion.Document = null;
            this.RtbCopyPaste.Document = documentoImprimir;
            this.RtbCopyPaste.SelectAll();
            this.RtbCopyPaste.Copy();
            //MostrarDatos(DocumentoActual);
            //this.contenidoTexto.Document = documentoOriginal;
            EscondeVistaPrel();
            MessageBox.Show(Mensajes.MENSAJE_ENVIADO_PORTAPAPELES,
                Mensajes.TITULO_ENVIADO_PORTAPAPELES,
                MessageBoxButton.OK, MessageBoxImage.Information);
        }
        #endregion
        /// <summary>
        /// Define la cadena de texto para generar las ligas del documento hacia
        /// leyes u otros objetos similares.
        /// </summary>
        /// <param name="texto"> El texto ue tendrá la liga</param>
        /// <param name="ius"> El Ius del documento </param>
        /// <param name="seccion"> La seccion donde estará la liga</param>
        /// <returns>El párrafo con la liga adecuada.</returns>
        protected Paragraph ObtenLigas(String texto, String id, int parte)
        {
            Paragraph resultado = new Paragraph(new Run(texto));
#if STAND_ALONE
            FachadaBusquedaTradicional fachada = new FachadaBusquedaTradicional();
            List<TablaPartesTO> listaRelaciones = fachada.getTablaAcuerdos(Int32.Parse(id));
#else
            FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
            TablaPartesTO[] listaRelaciones = fachada.getTablaAcuerdo(Int32.Parse(id));
#endif
            ventanaListadoAnexos.listado.ItemsSource = listaRelaciones.ToList();
#if STAND_ALONE
            if (listaRelaciones.Count == 0)
#else
            if (listaRelaciones.Length == 0)
#endif
            {
                BtnTablas.Visibility = Visibility.Hidden;
                ExistenTablas = false;
            }
            else
            {
                ExistenTablas = true;
                BtnTablas.Visibility = Visibility.Visible;
            }
            fachada.Close();
            List<String> resultadosParciales = new List<String>();
            String cadenaNueva;
            int posicionInicial = 0, posicionFinal = 0;
#if STAND_ALONE
            if (listaRelaciones.Count == 0)
#else
            if (listaRelaciones.Length == 0)
#endif
            {
                return resultado;
            }
            else
            {
                resultado = new Paragraph();
                foreach (TablaPartesTO item in listaRelaciones)
                {
                    if (parte == -1)
                    {
                        posicionFinal = item.Posicion;
                    }
                    else if (parte == item.Parte)
                    {
                        posicionFinal = item.PosicionParte;
                    }
                    else
                    {
                        posicionFinal = -1;
                    }
                    if (posicionFinal != -1)
                    {
                        if (posicionFinal < posicionInicial) posicionFinal = posicionInicial;
                        String resto = "";
                        if ((posicionFinal - posicionInicial) > texto.Length)
                        {
                            cadenaNueva = resto; 
                            resto=texto;
                            posicionFinal = 0;
                        }
                        else
                        {
                            cadenaNueva = texto.Substring(posicionInicial, posicionFinal - posicionInicial);
                            resto =  texto.Substring(posicionInicial + (posicionFinal - posicionInicial));
                        }
                        if (!resto.StartsWith(item.Frase))
                        {
                            int restoPos = resto.IndexOf(item.Frase);
                            if (restoPos != -1)
                            {
                                posicionFinal = posicionFinal + restoPos;
                            }
                            else
                            {
                                posicionFinal = texto.IndexOf(item.Frase);
                            }
                            cadenaNueva = texto.Substring(posicionInicial, posicionFinal - posicionInicial);
                        }
                        resultadosParciales.Add(cadenaNueva);
                        resultado.Inlines.Add(cadenaNueva);
                        resultado.Inlines.Add(creaLiga(item, item.Frase));
                        posicionInicial = posicionFinal + item.Frase.Length;
                    }
                }
                resultado.Inlines.Add(texto.Substring(posicionInicial));
                resultado.IsEnabled = true;
                return resultado;
            }
        }

        /// <summary>
        /// Crea la liga necesaria de acuerdo a los datos solicitados haciendo
        /// una verificación del tipo de liga y llenando el campo TAG
        /// con los valores necesarios para que el click funcione adecuadamente.
        /// </summary>
        /// <param name="liga">La liga que se pintará</param>
        /// <param name="contenido">El contenido de la liga</param>
        /// <returns>La liga ya lista para añadirse al documento</returns>
        private IUSHyperlink creaLiga(TablaPartesTO liga, string contenido)
        {
#if STAND_ALONE
            FachadaBusquedaTradicional fachadaBusqueda = new FachadaBusquedaTradicional();
#else
            FachadaBusquedaTradicionalClient fachadaBusqueda = new FachadaBusquedaTradicionalClient();
#endif
            IUSHyperlink ligaNueva = new IUSHyperlink(this.tabControl1);
            ligaNueva.Inlines.Add(new Run(contenido));
            ligaNueva.IsEnabled = true;
            ligaNueva.Tag = "PDF(" + liga.Archivo + ")";
            ligaNueva.Historia = Historia;
            ligaNueva.PaginaTarget = this;
            fachadaBusqueda.Close();
            return ligaNueva;
        }

        public void MuestraAnexo(TablaPartesTO anexoSeleccionado)
        {
#if STAND_ALONE
            Informe ventanaNueva = new Informe(IUSConstants.IUS_RUTA_ANEXOS + anexoSeleccionado.Archivo.Substring(0, anexoSeleccionado.Archivo.Length - 3) + "htm");
#else
            Informe ventanaNueva = new Informe("Anexos/" + anexoSeleccionado.Archivo.Substring(0, anexoSeleccionado.Archivo.Length - 3) + "htm");
#endif
            NavigationService.Navigate(ventanaNueva);
        }

        private void regNum_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                this.IrBoton_Click(sender, null);
            }
            if ((e.Key == Key.Decimal) || (e.Key == Key.OemPeriod))
            {
                e.Handled = true;
            }
        }

        private void BtnTablas_Click(object sender, RoutedEventArgs e)
        {
            ventanaListadoAnexos.Visibility = Visibility.Visible;
            ventanaListadoAnexos.NavigationService = this;
        }
        private void EscondeVistaPrel()
        {
            this.AcuerdoLabel.Visibility = Visibility.Visible;
            if (verVentanaRangos) ventanaRangos.Visibility = Visibility.Visible;
            if (verVentanaListadoAnexos) ventanaListadoAnexos.Visibility = Visibility.Visible;

            if ((ArregloTesis != null) && (ArregloTesis.Items.Count > 1))
            {
                this.anteriorLista.Visibility = Visibility.Visible;
                this.inicioLista.Visibility = Visibility.Visible;
                this.siguienteLista.Visibility = Visibility.Visible;
                this.ultimoLista.Visibility = Visibility.Visible;
                IrALabel.Visibility = Visibility.Visible;
                IrBoton.Visibility = Visibility.Visible;
                RegNum.Visibility = Visibility.Visible;
                regNum.Visibility = Visibility.Visible;
            }
            PortaPapeles.Visibility = Visibility.Visible;
            Imprimir.Visibility = Visibility.Visible;
            imprimePapel.Visibility = Visibility.Hidden;
            BtnTache.Visibility = Visibility.Hidden;
            if(verFontMayor) FontMayor.Visibility = Visibility.Visible;
            if(verFontMenor) FontMenor.Visibility = Visibility.Visible;
            Salir.Visibility = Visibility.Visible;
            Guardar.Visibility = !BrowserInteropHelper.IsBrowserHosted ? Visibility.Visible : Visibility.Hidden;
            fuenteLabel.Visibility = Visibility.Visible;
            EpocaLabel.Visibility = Visibility.Visible;
            IdLabel.Visibility = Visibility.Visible;
            LblSala.Visibility = Visibility.Visible;
            VolumenLabel.Visibility = Visibility.Visible;
            PaginaLabel.Visibility = Visibility.Visible;
            if (ExistenTablas)
            {
                BtnTablas.Visibility = Visibility.Visible;
            }
            LblPalabraBuscar.Visibility = Visibility.Visible;
            textoAbuscar.Visibility = Visibility.Visible;
            Buscar.Visibility = Visibility.Visible;
            Expresion.Visibility = Visibility.Visible;
            tabControl1.Visibility = Visibility.Visible;
            impresion.Visibility = Visibility.Hidden;
#if STAND_ALONE
            if ((partes.Count > 1) && (numeroParte > -1))
#else
            if ((partes.Length > 1) && (numeroParte > -1))
#endif
            {
                docCompletoImage.Visibility = Visibility.Visible;
                parteInicio.Visibility = Visibility.Visible;
                parteAnterior.Visibility = Visibility.Visible;
                parteSiguiente.Visibility = Visibility.Visible;
                parteFinal.Visibility = Visibility.Visible;
                NumeroPartes.Visibility = Visibility.Visible;
            }
        }
        private void MuestraVistaPrel()
        {
            AcuerdoLabel.Visibility = Visibility.Hidden;
            verVentanaRangos = ventanaRangos.Visibility==Visibility.Visible;
            verVentanaListadoAnexos = ventanaListadoAnexos.Visibility == Visibility.Visible;
            ventanaRangos.Visibility=Visibility.Hidden;
            ventanaListadoAnexos.Visibility=Visibility.Hidden;
            impresion.Visibility = Visibility.Visible;
            tabControl1.Visibility = Visibility.Hidden;
            this.anteriorLista.Visibility = Visibility.Hidden;
            this.inicioLista.Visibility = Visibility.Hidden;
            this.siguienteLista.Visibility = Visibility.Hidden;
            this.ultimoLista.Visibility = Visibility.Hidden;
            IrALabel.Visibility = Visibility.Hidden;
            IrBoton.Visibility = Visibility.Hidden;
            RegNum.Visibility = Visibility.Hidden;
            regNum.Visibility = Visibility.Hidden;
            historial.Visibility = Visibility.Hidden;
            //tesis.Visibility = Visibility.Hidden;
            //ejecutoria.Visibility = Visibility.Hidden;
            BtnTablas.Visibility = Visibility.Hidden;
            PortaPapeles.Visibility = Visibility.Hidden;
            Imprimir.Visibility = Visibility.Hidden;
            imprimePapel.Visibility = Visibility.Visible;
            BtnTache.Visibility = Visibility.Visible;
            FontMayor.Visibility = Visibility.Hidden;
            FontMenor.Visibility = Visibility.Hidden;
            Marcar.Visibility = Visibility.Hidden;
            MarcarTodo.Visibility = Visibility.Hidden;
            Desmarcar.Visibility = Visibility.Hidden;
            Salir.Visibility = Visibility.Hidden;
            Guardar.Visibility = Visibility.Hidden;
            fuenteLabel.Visibility = Visibility.Hidden;
            EpocaLabel.Visibility = Visibility.Hidden;
            IdLabel.Visibility = Visibility.Hidden;
            LblSala.Visibility = Visibility.Hidden;
            VolumenLabel.Visibility = Visibility.Hidden;
            PaginaLabel.Visibility = Visibility.Hidden;
            LblPalabraBuscar.Visibility = Visibility.Hidden;
            textoAbuscar.Visibility = Visibility.Hidden;
            Buscar.Visibility = Visibility.Hidden;
            Expresion.Visibility = Visibility.Hidden;
            parteAnterior.Visibility = Visibility.Hidden;
            parteInicio.Visibility = Visibility.Hidden;
            parteSiguiente.Visibility = Visibility.Hidden;
            parteFinal.Visibility = Visibility.Hidden;
            docCompletoImage.Visibility = Visibility.Hidden;
            NumeroPartes.Visibility = Visibility.Hidden;
        }

        private void BtnTache_Click(object sender, RoutedEventArgs e)
        {
            EscondeVistaPrel();
        }

        private void textoAbuscar_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Buscar_MouseLeftButtonDown(sender, e);
            }
        }

        private void contenidoTexto_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key.Equals(Key.Enter) && (!textoAbuscar.Text.Equals("")))
            {
                Buscar_MouseLeftButtonDown(sender, null);
            }
        }

        private void contenidoTexto_Copying(object sender, DataObjectCopyingEventArgs e)
        {
            e.Handled = true;
            RichTextBox rtb = sender as RichTextBox;
            TbxCopiar.Text = contenidoTexto.Selection.Text;
            e.CancelCommand();
            TbxCopiar.SelectAll();
            TbxCopiar.Copy();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            contenidoTexto.FontSize = CalculosPropiedadesGlobales.FontSize;
        }
    }
}
