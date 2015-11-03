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
    public partial class EjecutoriaPagina : Page
    {
        #region fields
        private FlowDocument DocumentoCopia { get; set; }
        /// <summary>
        /// La pagina a donde debe regresar.
        /// </summary>
        public Page Back;
        /// <summary>
        /// Cuando se construye a partir de una lista que se encuentra en el
        /// dataGridControl esta queda aquí.
        /// </summary>
        private Xceed.Wpf.DataGrid.DataGridControl ArregloTesis{get;set;}
        /// <summary>
        /// El identificador de la genealogía.
        /// </summary>
        private String genealogiaId { get; set; }
        /// <summary>
        /// Los documentos de tesis que tiene relacionada la ejecutoria.
        /// </summary>
#if STAND_ALONE
        private List<RelDocumentoTesisTO> documentosTesis { get; set; }
#else
        private RelDocumentoTesisTO[] documentosTesis { get; set; }
#endif
        /// <summary>
        /// Los documentos de Votos relacionados con la tesis.
        /// </summary>
#if STAND_ALONE
        private List<RelVotoEjecutoriaTO> documentosVotos { get; set; }
#else
        private RelVotoEjecutoriaTO[] documentosVotos { get; set; }
#endif
        /// <summary>
        /// Cada una de las partes que componen la eecutoria de la tesis
        /// </summary>
#if STAND_ALONE
        private List<EjecutoriasPartesTO> partes { get; set; }
#else
        private EjecutoriasPartesTO[] partes { get; set; }
#endif
        /// <summary>
        /// La posición en la que nos encontramos dentro de la selección del datagrid.
        /// </summary>
        private int posicion = 0;
        /// <summary>
        /// La ejecutoria actual.
        /// </summary>
        private EjecutoriasTO DocumentoActual;
        /// <summary>
        /// El número de parte que se muestra en el momento.
        /// </summary>
        private int numeroParte { get; set; }
        /// <summary>
        /// El historial que se mostrará.
        /// </summary>
        public Historial Historia { get { return getHistoria(); } set { this.setHistoria(value); } }
        private Historial historia;
        /// <summary>
        /// El parámetro con el que se mando llamar el constructor.
        /// </summary>
        /// 
        private Object ParametroConstruccion { get; set; }
        ///<summary>
        ///El objeto de la busqueda original cuando es secuencial o por palabras
        ///</summary>
        ///
        private BusquedaTO Busqueda { get; set; }
        /// <summary>
        /// La lista de documentos marcados para impresión o copia.
        /// </summary>
        private static HashSet<int> marcados { get; set; }
        /// <summary>
        ///     Define si la frase a encontrar fue ya o no localizada.
        /// </summary>
        /// 
        protected bool EncontradaFrase { get; set; }
        private bool ExistenTablas { get; set; }
        protected bool verVentanaListadoVotos { get; set; }
        protected bool verVentanaListadoTesis { get; set; }
        protected bool verVentanaHistorial { get; set; }
        protected bool verVentanaRangos { get; set; }
        protected bool verVentanaAnexos { get; set; }
        protected bool verHistorial { get; set; }
        protected bool verFontMenor { get; set; }
        protected bool verFontMayor { get; set; }

        #endregion
        #region constructores
        /// <summary>
        /// Constructor por omisión
        /// </summary>
        public EjecutoriaPagina()
        {
            InitializeComponent();
            this.ParametroConstruccion = this;
            marcados = new HashSet<int>();
            verFontMenor = true;
            verFontMayor = true;
        }
        public EjecutoriaPagina(int id)
        {
            InitializeComponent();
            this.contenidoTexto.SetValue(RichTextBox.FontSizeProperty, CalculosPropiedadesGlobales.FontSize);
            verFontMenor = true;
            verFontMayor = true;
            this.ParametroConstruccion = this;
#if STAND_ALONE
            FachadaBusquedaTradicional fachada = new FachadaBusquedaTradicional();
#else
            FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
#endif
            EjecutoriasTO documentoActual = fachada.getEjecutoriaPorId(id);
            MostrarDatos(documentoActual);
            fachada.Close();
            //Si entran a este constructor es por que vienen para ver solamente un registro
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
            this.Expresion.Visibility = Visibility.Hidden;
        }
        /// <summary>
        /// Constructor en el que se obtienen los datos de una tabla determinada.
        /// </summary>
        /// <param name="records">Los registros a mostrar</param>
        public EjecutoriaPagina(Xceed.Wpf.DataGrid.DataGridControl records, BusquedaTO parametrosBusqueda)
        {
            InitializeComponent();
            this.contenidoTexto.SetValue(RichTextBox.FontSizeProperty, CalculosPropiedadesGlobales.FontSize);
            verFontMenor = true;
            verFontMayor = true;
            CalculosPropiedadesGlobales.FontSize = Constants.FONTSIZE;
            marcados = new HashSet<int>();
            if (records.Items.Count < 2)
            {
                this.inicioLista.Visibility = Visibility.Hidden;
                this.anteriorLista.Visibility = Visibility.Hidden;
                this.siguienteLista.Visibility = Visibility.Hidden;
                this.ultimoLista.Visibility = Visibility.Hidden;
                this.regNum.Visibility = Visibility.Hidden;
                this.RegNum.Visibility = Visibility.Hidden;
                this.IrALabel.Visibility = Visibility.Hidden;
                this.IrBoton.Visibility = Visibility.Hidden;
            }
            this.Busqueda = parametrosBusqueda;
            this.ParametroConstruccion = this;
            EjecutoriasSimplificadaTO ejecutoriaMostrar = (EjecutoriasSimplificadaTO)records.SelectedItem;
            posicion = records.SelectedIndex;
#if STAND_ALONE
            FachadaBusquedaTradicional fachada = new FachadaBusquedaTradicional();
#else
            FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
#endif
            EjecutoriasTO documentoActual = fachada.getEjecutoriaPorId(Int32.Parse(ejecutoriaMostrar.Id));
            fachada.Close();
            this.ArregloTesis = records;
            if (parametrosBusqueda != null)
            {
                Expresion.Content = CalculosGlobales.Expresion(parametrosBusqueda);
            }
            else
            {
                List<int> registros = new List<int>();
                Expresion.Content = CalculosGlobales.Expresion(registros);
            }
            MostrarDatos(documentoActual);
            this.Historia.RootElement = records;
        }
        #endregion
        /// <summary>
        /// Muestra los datos de una ejecutoria determinada.
        /// </summary>
        private void MostrarDatos(EjecutoriasTO documentoActual){
            DocumentoActual = documentoActual;
            numeroParte = 0;
            this.PaginaLabel.Content = "Página: "+documentoActual.Pagina;
            this.EpocaLabel.Content = documentoActual.Epoca;
            this.fuenteLabel.Content = documentoActual.Fuente;
            this.SalaLabel.Content = documentoActual.Sala;
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
            partes = fachada.getPartesPorId(Int32.Parse(documentoActual.Id));
            this.documentosTesis = fachada.getTesisPorEjecutoria(documentoActual.Id);
            Paragraph textoParrafo;
#if STAND_ALONE
            if (documentosTesis.Count > 0)
#else
            if (documentosTesis.Length > 0)
#endif
            {
                textoParrafo = ObtenLigas(partes[0].TxtParte, documentoActual.Id, 1);
            }
            else
            {
                textoParrafo = ObtenLigas(documentoActual.Rubro + "\n\n" + partes[0].TxtParte, documentoActual.Id, 1);
            }
            textoParrafo.TextAlignment = TextAlignment.Justify;
            textoParrafo.FontWeight = FontWeights.Normal;
            documentoRubro.Blocks.Add(textoParrafo);
            this.documentosVotos = fachada.getVotoPorEjecutoria(documentoActual.Id);
#if STAND_ALONE
            if (this.documentosTesis.Count == 0)
#else
            if (this.documentosTesis.Length == 0)
#endif
            {
                this.tesis.Visibility = Visibility.Hidden;
            }
            else
            {
                this.tesis.Visibility = Visibility.Visible;
            }
#if STAND_ALONE
            if (this.documentosVotos.Count == 0)
#else
            if (this.documentosVotos.Length == 0)
#endif
            {
                voto.Visibility = Visibility.Hidden;
            }
            else
            {
                voto.Visibility = Visibility.Visible;
            }
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
                NumeroPartes.Content = "Parte: 1 de " + partes.Count;
#else
                NumeroPartes.Content = "Parte: 1 de " + partes.Length;
#endif
                docCompletoImage.Visibility = Visibility.Visible;
            }
            this.contenidoTexto.Document = documentoRubro;
            if ((Busqueda != null) && (Busqueda.Palabra != null))
            {
                foreach (BusquedaPalabraTO item in Busqueda.Palabra)
                {
                    BusquedaPalabraTO palabrejasTO = new BusquedaPalabraTO();
                    if (item.Jurisprudencia == Constants.BUSQUEDA_PALABRA_ALMACENADA)
                    {
#if STAND_ALONE
                        FachadaBusquedaTradicional fachadaAlmacena = new FachadaBusquedaTradicional();
                        List<BusquedaAlmacenadaTO> busquedas = fachadaAlmacena.getBusquedasAlmacenadas(SeguridadUsuariosTO.UsuarioActual.Usuario);
#else
            FachadaBusquedaTradicionalClient fachadaAlmacena = new FachadaBusquedaTradicionalClient();
                        BusquedaAlmacenadaTO[] busquedas = fachadaAlmacena.getBusquedasAlmacenadas(SeguridadUsuariosTO.UsuarioActual.Usuario);
#endif
                        fachadaAlmacena.Close();
                        String palabrejas = "";
                        foreach (BusquedaAlmacenadaTO itemAlamacenada in busquedas)
                        {
                            if (itemAlamacenada.id == item.Campos[0])
                            {
                                foreach (ExpresionBusqueda itemExpresion in itemAlamacenada.Expresiones)
                                {
                                    palabrejas += itemExpresion.Expresion;
                                    palabrejas += " ";
                                    palabrejasTO.Campos = itemExpresion.Campos;
                                }
                            }
                        }
                        palabrejasTO.Expresion = palabrejas;
                    }
                    else
                    {
                        palabrejasTO = item;
                    }
                    List<String> listapalabras = FlowDocumentHighlight.obtenPalabras(palabrejasTO);
                    this.contenidoTexto.Document = FlowDocumentHighlight.
                        imprimeToken(contenidoTexto.Document, 
                                     listapalabras, Brushes.Red);
                    List<String> frases = FlowDocumentHighlight.obtenFrases(palabrejasTO);
                    documentoRubro = FlowDocumentHighlight.imprimeToken(documentoRubro, frases, Brushes.DarkGreen);
                }
                
            }
            this.contenidoTexto.IsReadOnly = true;
            this.contenidoTexto.IsEnabled = true;
            fachada.Close();
            bool encontrado = false;
            bool encontradoverdadero = false;
            bool buscar = false;
            if ((Busqueda != null) && (Busqueda.Palabra != null))
            {
                foreach (BusquedaPalabraTO itemPalabra in Busqueda.Palabra)
                {
                    buscar=(itemPalabra.Campos.Contains(Constants.BUSQUEDA_PALABRA_CAMPO_TEXTO)
                        ||itemPalabra.Campos.Contains(Constants.BUSQUEDA_PALABRA_CAMPO_RUBRO)
                        ||itemPalabra.Campos.Contains(Constants.BUSQUEDA_PALABRA_CAMPO_ASUNTO));
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
#else
                if ((!encontrado) && (numeroParte < (partes.Length-1)))
                {
                    if (numeroParte < (partes.Length - 1))
#endif
                    {
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
                List<EjecutoriasSimplificadaTO> lista = (List<EjecutoriasSimplificadaTO>)(ArregloTesis.ItemsSource);
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
                DocumentoActual = documentoActual;
                tabControl1.Visibility = Visibility.Visible;
            }
            Historial hist = new Historial();
            hist = new Historial();
            hist.Lista = new List<IUSNavigationService>();
            IUSNavigationService entradaHistoria = new IUSNavigationService();
            entradaHistoria.Id = Int32.Parse(documentoActual.Id);
            entradaHistoria.NavigationTarget = this;
            entradaHistoria.ParametroConstructor = this;
            entradaHistoria.TipoVentana = IUSNavigationService.EJECUTORIA;
            hist.Lista.Add(entradaHistoria);
            this.Historia = hist; 
            TextoABuscar.Text = Constants.CADENA_VACIA;
            this.ventanaHistorial.Visibility = Visibility.Hidden;
            this.ventanaListadoTesis.Visibility = Visibility.Hidden;
            this.ventanaListadoVotos.Visibility = Visibility.Hidden;
            this.impresion.Visibility = Visibility.Hidden;
            this.ventanaAnexos.Visibility = Visibility.Hidden;
            this.imprimePapel.Visibility = Visibility.Hidden;
            this.BuscarImage.Visibility = Visibility.Visible;
            this.TextoABuscar.Visibility = Visibility.Visible;
            this.Expresion.Visibility = Visibility.Visible;
            List<object> items = new List<object>();
            if (marcados != null)
            {
                Image imagenNueva = new Image();
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                if (marcados.Contains(Int32.Parse(DocumentoActual.Id)))
                {
                    bitmap.UriSource = new Uri("images/PALOMA1.png", UriKind.Relative);
                }
                else
                {
                    bitmap.UriSource = new Uri("images/MARCAR1.png", UriKind.Relative);
                }
                bitmap.EndInit();
                this.Marcar.Source = bitmap;
            }
            

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
            List<TablaPartesTO> listaRelaciones = fachada.getTablaEjecutorias(Int32.Parse(id));
            ventanaAnexos.listado.ItemsSource = listaRelaciones;
            if (listaRelaciones.Count== 0)
#else
            FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
            TablaPartesTO[] listaRelaciones = fachada.getTablaEjecutorias(Int32.Parse(id));
            ventanaAnexos.listado.ItemsSource = listaRelaciones.ToList();
            if (listaRelaciones.Length == 0)
#endif
            {
                TablasAnexos.Visibility = Visibility.Hidden;
                ExistenTablas = false;
            }
            else
            {
                ExistenTablas = true;
                TablasAnexos.Visibility = Visibility.Visible;
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
                        try
                        {
                            if (posicionFinal < posicionInicial) posicionFinal = posicionInicial;
                            cadenaNueva = texto.Substring(posicionInicial, posicionFinal - posicionInicial);
                            String resto = texto.Substring(posicionInicial + (posicionFinal - posicionInicial));
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
                        catch (Exception e)
                        {
                            resultado = new Paragraph(new Run(texto));
                            posicionInicial = texto.Length-1;
                        }
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
            //RelacionFraseTesisTO tesis = null;
            //RelacionFraseArticulosTO articulos = null;
            //FachadaBusquedaTradicionalClient fachadaBusqueda = new FachadaBusquedaTradicionalClient();
            IUSHyperlink ligaNueva = new IUSHyperlink(this.tabControl1);
            ligaNueva.Inlines.Add(new Run(contenido));
            ligaNueva.IsEnabled = true;
            ligaNueva.Tag = "PDF(" + liga.Archivo + ")";
            ligaNueva.Historia = Historia;
            ligaNueva.PaginaTarget = this;
            //fachadaBusqueda.Close();
            return ligaNueva;
        }

        private void historial_MouseButtonDown(object sender, RoutedEventArgs e)
        {
            if (this.Historia.Lista.Count < 2)
            {
                MessageBox.Show(Mensajes.MENSAJE_HISTORIAL_CON_UNO, Mensajes.TITULO_HISTORIAL_CON_UNO,
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            Historia.ventana = this.ventanaHistorial;
            Historia.OpenWindow();
        }

        #region imprimir

        private void Imprimir_MouseButtonDown(object sender, RoutedEventArgs e)
        {
            if (TablasAnexos.Visibility == Visibility.Visible)
            {
                MessageBox.Show(Mensajes.MENSAJE_TABLAS_EXISTENTES, Mensajes.TITULO_TABLAS_EXISTENTES,
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }

            if (imprimePapel.Visibility == Visibility.Hidden)
            {
                DocumentoEjecutorias documento;
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
                    EjecutoriasSimplificadaTO ejecutoriaImprimir = new EjecutoriasSimplificadaTO();
                    ejecutoriaImprimir.Id = DocumentoActual.Id;
                    documento = new DocumentoEjecutorias(ejecutoriaImprimir, numeroParte+1);
                }
                else if (resultadoMsgBox.Equals(MessageBoxResult.Yes))
                {
                    documento = new DocumentoEjecutorias(marcados);
                }
                else
                {
                    return;
                }
                //FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
                //object documentoXps = fachada.getDocumentoTesis(this.DocumentoActual.Ius);
                impresion.Document = documento.Documento; //(IDocumentPaginatorSource)documentoXps;
                DocumentoCopia = documento.Copia;
                //fachada.Close();
                impresion.Visibility = Visibility.Visible;
                impresion.Background = Brushes.White;
                tabControl1.Visibility = Visibility.Hidden;
                imprimePapel.Visibility = Visibility.Visible;
                TextoABuscar.Visibility = Visibility.Hidden;
                BuscarImage.Visibility = Visibility.Hidden;
                Expresion.Visibility = Visibility.Hidden;
                MuestraVistaPrel();
            }
            else
            {
                impresion.Visibility = Visibility.Hidden;
                //impresion.Background = Brushes.Transparent;
                tabControl1.Visibility = Visibility.Visible;
                imprimePapel.Visibility = Visibility.Hidden;
                TextoABuscar.Visibility = Visibility.Visible;
                BuscarImage.Visibility = Visibility.Visible;
                Expresion.Visibility = Visibility.Visible;
                EscondeVistaPrel();
            }
        }
        #endregion
 
        private void voto_MouseButtonDown(object sender, RoutedEventArgs e)
        {
#if STAND_ALONE
            if (documentosVotos.Count == 1)
#else
            if (documentosVotos.Length == 1)
#endif
            {
                foreach (IUSNavigationService item in Historia.Lista)
                {
                    if ((item.Id == Int32.Parse(documentosVotos[0].Voto)) && (item.TipoVentana == IUSNavigationService.VOTO))
                    {
                        MessageBox.Show(Mensajes.MENSAJE_VENTANA_ABIERTA, Mensajes.TITULO_VENTANA_ABIERTA,
                            MessageBoxButton.OK, MessageBoxImage.Information);
                        VotosPagina ventaVoto = (VotosPagina)item.ParametroConstructor;
                        ventaVoto.ventanaHistorial.Visibility = Visibility.Hidden;
                        ventaVoto.ventanaListadoEjecutorias.Visibility = Visibility.Hidden;
                        ventaVoto.ventanaListadoTesis.Visibility = Visibility.Hidden;
                        NavigationService.Navigate(ventaVoto);
                        return;
                    }
                }

                   // FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
                    VotosPagina votoAsociado = new VotosPagina(Int32.Parse(documentosVotos[0].Voto));
                    votoAsociado.Historia = Historia;
                    this.NavigationService.Navigate(votoAsociado);
            }
            else
            {
                List<int> identificadores = new List<int>();
                this.ventanaListadoTesis.NavigationService = this.NavigationService;
                foreach (RelVotoEjecutoriaTO item in documentosVotos)
                {
                    identificadores.Add(Int32.Parse(item.Voto));
                }
                MostrarPorIusTO parametros = new MostrarPorIusTO();
                parametros.OrderBy = "ConsecIndx";
                parametros.OrderType = "asc";
#if STAND_ALONE
                FachadaBusquedaTradicional fachada = new FachadaBusquedaTradicional();
                parametros.Listado = identificadores;
                List<VotosTO> votosRelacionados = fachada.getVotosPorIds(parametros);
#else
            FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
                parametros.Listado = identificadores.ToArray();
                VotosTO[] votosRelacionados = fachada.getVotosPorIds(parametros);
#endif
                List<VotoSimplificadoTO> listaFinal = new List<VotoSimplificadoTO>();
                foreach (VotosTO item in votosRelacionados)
                {
                    VotoSimplificadoTO itemVerdadero = new VotoSimplificadoTO(item);
                    listaFinal.Add(itemVerdadero);
                }
                this.ventanaListadoVotos.ListaRelacion = listaFinal;
                this.ventanaListadoVotos.Historia = Historia;
                this.ventanaListadoVotos.NavigationService = this;
                this.ventanaListadoVotos.Visibility = Visibility.Visible;
            }
        }


        private void inicioLista_MouseButtonDown(object sender, RoutedEventArgs e)
        {
            List<EjecutoriasSimplificadaTO> presentadorDatos = (List<EjecutoriasSimplificadaTO>)this.ArregloTesis.ItemsSource;
            EjecutoriasSimplificadaTO ejecutoriasMostrar = presentadorDatos[0];
            posicion = 0;
            this.ArregloTesis.SelectedIndex = posicion;
            this.ArregloTesis.BringItemIntoView(this.ArregloTesis.SelectedItem);
#if STAND_ALONE
            FachadaBusquedaTradicional fachada = new FachadaBusquedaTradicional();
#else
            FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
#endif
            EjecutoriasTO documentoActual = fachada.getEjecutoriaPorId(Int32.Parse(ejecutoriasMostrar.Id));
            fachada.Close();
            MostrarDatos(documentoActual);
        }

        private void anteriorLista_MouseButtonDown(object sender, RoutedEventArgs e)
        {
            List<EjecutoriasSimplificadaTO> presentadorDatos = (List<EjecutoriasSimplificadaTO>)this.ArregloTesis.ItemsSource;
            EjecutoriasSimplificadaTO ejecutoriaMostrar = null;
            if (posicion == 0)
            {
                ejecutoriaMostrar = presentadorDatos[0];
            }
            else
            {
                posicion--;
                ejecutoriaMostrar = presentadorDatos[posicion];
            }
#if STAND_ALONE
            FachadaBusquedaTradicional fachada = new FachadaBusquedaTradicional();
#else
            FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
#endif
            EjecutoriasTO documentoActual = fachada.getEjecutoriaPorId(Int32.Parse(ejecutoriaMostrar.Id));
            fachada.Close();
            this.ArregloTesis.SelectedIndex = posicion;
            this.ArregloTesis.BringItemIntoView(this.ArregloTesis.SelectedItem);
            MostrarDatos(documentoActual);
        }

        private void siguienteLista_MouseButtonDown(object sender, RoutedEventArgs e)
        {
            List<EjecutoriasSimplificadaTO> presentadorDatos = (List<EjecutoriasSimplificadaTO>)this.ArregloTesis.ItemsSource;
            EjecutoriasSimplificadaTO ejecutoriaMostrar = null;
            if (posicion >= presentadorDatos.Count - 1)
            {
                posicion = presentadorDatos.Count - 1;
                ejecutoriaMostrar = presentadorDatos[posicion];
            }
            else
            {
                posicion++;
                ejecutoriaMostrar = presentadorDatos[posicion];
            }
#if STAND_ALONE
            FachadaBusquedaTradicional fachada = new FachadaBusquedaTradicional();
#else
            FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
#endif
            EjecutoriasTO documentoActual = fachada.getEjecutoriaPorId(Int32.Parse(ejecutoriaMostrar.Id));
            fachada.Close();
            this.ArregloTesis.SelectedIndex = posicion;
            this.ArregloTesis.BringItemIntoView(this.ArregloTesis.SelectedItem);
            MostrarDatos(documentoActual);
        }

        private void ultimoLista_MouseButtonDown(object sender, RoutedEventArgs e)
        {
            List<EjecutoriasSimplificadaTO> presentadorDatos = (List<EjecutoriasSimplificadaTO>)this.ArregloTesis.ItemsSource;
            EjecutoriasSimplificadaTO ejecutoriaMostrar = null;
            posicion = presentadorDatos.Count - 1;
            ejecutoriaMostrar = presentadorDatos[posicion];
#if STAND_ALONE
            FachadaBusquedaTradicional fachada = new FachadaBusquedaTradicional();
#else
            FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
#endif
            EjecutoriasTO documentoActual = fachada.getEjecutoriaPorId(Int32.Parse(ejecutoriaMostrar.Id));
            fachada.Close();
            this.ArregloTesis.SelectedIndex = posicion;
            this.ArregloTesis.BringItemIntoView(this.ArregloTesis.SelectedItem);
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
            if (numeroParte < (partes.Count - 1))
#else
            if (numeroParte < (partes.Length-1))
#endif
            {
                numeroParte++;
            }
            actualizaTexto();
        }

        private void tesis_MouseButtonDown(object sender, RoutedEventArgs e)
        {
#if STAND_ALONE
            if (documentosTesis.Count == 1)
#else
            if (documentosTesis.Length == 1)
#endif
            {
                try
                {
                    foreach (IUSNavigationService item in Historia.Lista)
                    {
                        if ((item.Id == Int32.Parse(documentosTesis[0].Ius)) && (item.TipoVentana == IUSNavigationService.TESIS))
                        {
                            MessageBox.Show(Mensajes.MENSAJE_VENTANA_ABIERTA, Mensajes.TITULO_VENTANA_ABIERTA,
                                MessageBoxButton.OK, MessageBoxImage.Information);
                            Tesis navegar = (Tesis)item.ParametroConstructor;
                            navegar.ventanaHistorial.Visibility = Visibility.Hidden;
                            navegar.ventanaListaEjecutorias.Visibility = Visibility.Hidden;
                            navegar.ventanaListaVotos.Visibility = Visibility.Hidden;
                            NavigationService.Navigate(navegar);
                            return;
                        }
                    }

#if STAND_ALONE
                    FachadaBusquedaTradicional fachada = new FachadaBusquedaTradicional();
#else
            FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
#endif
                    TesisTO tesisCompleta = fachada.getTesisPorRegistro(documentosTesis[0].Ius);
                    Tesis tesisAsociada = new Tesis(tesisCompleta);
                    tesisAsociada.Historia = Historia;
                    this.NavigationService.Navigate(tesisAsociada);
                }
                catch (Exception exc)
                {
                    System.Console.WriteLine("Dieron Click muy rápido"+exc.Message);
                }
            }
            else
            {
                List<int> identificadores = new List<int>();
                this.ventanaListadoTesis.NavigationService = this.NavigationService;
                foreach (RelDocumentoTesisTO item in documentosTesis)
                {
                    identificadores.Add(Int32.Parse(item.Ius));
                }
                MostrarPorIusTO parametros = new MostrarPorIusTO();
                parametros.OrderBy = "ConsecIndx";
                parametros.OrderType = "asc";
#if STAND_ALONE
                FachadaBusquedaTradicional fachada = new FachadaBusquedaTradicional();
                parametros.Listado = identificadores;
                List<TesisTO> ejecutoriasRelacionadas = fachada.getTesisPorIus(parametros);
#else
            FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
                parametros.Listado = identificadores.ToArray();
                TesisTO[] ejecutoriasRelacionadas = fachada.getTesisPorIus(parametros);
#endif
                List<TesisSimplificadaTO> listaFinal = new List<TesisSimplificadaTO>();
                foreach (TesisTO item in ejecutoriasRelacionadas)
                {
                    TesisSimplificadaTO itemCompleto = new TesisSimplificadaTO();
                    itemCompleto.Ius = item.Ius;
                    itemCompleto.ConsecIndx = item.ConsecIndx;
                    listaFinal.Add(itemCompleto);
                }
                this.ventanaListadoTesis.Historia = Historia;
                this.ventanaListadoTesis.TesisMostrar = listaFinal;
                this.ventanaListadoTesis.Visibility = Visibility.Visible;
                //MessageBox.Show("hola");
                fachada.Close();
            }
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
            FlowDocument documentoRubro = new FlowDocument();
            Paragraph textoParrafo =null;
#if STAND_ALONE
            if ((documentosTesis.Count == 0) && (numeroParte == 0))
#else
            if ((documentosTesis.Length == 0) && (numeroParte == 0))
#endif
            {
                textoParrafo = ObtenLigas(DocumentoActual.Rubro + "\n\n"
                    + partes[numeroParte].TxtParte, DocumentoActual.Id, numeroParte + 1);
            }
            else
            {
                textoParrafo = ObtenLigas(partes[numeroParte].TxtParte, DocumentoActual.Id, numeroParte + 1);
            }
            textoParrafo.FontWeight=FontWeights.Normal;
            textoParrafo.TextAlignment=TextAlignment.Justify;
            documentoRubro.Blocks.Add(textoParrafo);
            int parteReal = numeroParte + 1;
#if STAND_ALONE
            NumeroPartes.Content = "Parte: " + parteReal + " / " + partes.Count;
#else
            NumeroPartes.Content = "Parte: " + parteReal + " / " + partes.Length;
#endif
            contenidoTexto.Document = documentoRubro;
            if ((Busqueda != null) && (Busqueda.Palabra != null))
            {
                foreach (BusquedaPalabraTO item in Busqueda.Palabra)
                {
                    BusquedaPalabraTO palabrejasTO = new BusquedaPalabraTO();
                    if (item.Jurisprudencia == Constants.BUSQUEDA_PALABRA_ALMACENADA)
                    {
#if STAND_ALONE
                        FachadaBusquedaTradicional fachadaAlmacena = new FachadaBusquedaTradicional();
                        List<BusquedaAlmacenadaTO> busquedas = fachadaAlmacena.getBusquedasAlmacenadas(SeguridadUsuariosTO.UsuarioActual.Usuario);
#else
            FachadaBusquedaTradicionalClient fachadaAlmacena = new FachadaBusquedaTradicionalClient();
                        BusquedaAlmacenadaTO[] busquedas = fachadaAlmacena.getBusquedasAlmacenadas(SeguridadUsuariosTO.UsuarioActual.Usuario);
#endif
                        fachadaAlmacena.Close();
                        String palabrejas = "";
                        foreach (BusquedaAlmacenadaTO itemAlamacenada in busquedas)
                        {
                            if (itemAlamacenada.id == item.Campos[0])
                            {
                                foreach (ExpresionBusqueda itemExpresion in itemAlamacenada.Expresiones)
                                {
                                    palabrejas += itemExpresion.Expresion;
                                    palabrejas += " ";
                                    palabrejasTO.Campos = itemExpresion.Campos;
                                }
                            }
                        }
                        palabrejasTO.Expresion = palabrejas;
                    }
                    else
                    {
                        palabrejasTO = item;
                    }
                    List<String> listapalabras = FlowDocumentHighlight.obtenPalabras(palabrejasTO);
                    documentoRubro = FlowDocumentHighlight.
                        imprimeToken(documentoRubro,
                                     listapalabras, Brushes.Red);
                    List<String> frases = FlowDocumentHighlight.obtenFrases(palabrejasTO);
                    documentoRubro = FlowDocumentHighlight.imprimeToken(documentoRubro, frases, Brushes.DarkGreen);
                }
            }
            this.contenidoTexto.Document=documentoRubro;
        }

        private void docCompletoImage_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            FlowDocument documentoRubro = new FlowDocument();
#if STAND_ALONE
            if (documentosTesis.Count == 0)
#else
            if (documentosTesis.Length == 0)
#endif
            {
                documentoRubro.Blocks.Add(new Paragraph(new Run(DocumentoActual.Rubro)));
            }
            foreach (EjecutoriasPartesTO item in partes)
            {
                Paragraph textoParrafo = ObtenLigas(item.TxtParte,""+item.Id,item.Parte);
                textoParrafo.FontWeight = FontWeights.Normal;
                textoParrafo.TextAlignment = TextAlignment.Justify;
                documentoRubro.Blocks.Add(textoParrafo);
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
            parteSiguiente.Visibility = Visibility.Hidden;
            parteAnterior.Visibility = Visibility.Hidden;
            parteFinal.Visibility = Visibility.Hidden;
            parteInicio.Visibility = Visibility.Hidden;
            NumeroPartes.Visibility = Visibility.Hidden;
            docCompletoImage.Visibility = Visibility.Hidden;
            numeroParte = -2;
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
            List<EjecutoriasSimplificadaTO> arregloEjecutoriasActual = (List<EjecutoriasSimplificadaTO>)ArregloTesis.ItemsSource;
            if (registro > 0 && registro <= arregloEjecutoriasActual.Count)
            {
                registro--;
                EjecutoriasSimplificadaTO ejecutoriaActual = arregloEjecutoriasActual[registro];
#if STAND_ALONE
                FachadaBusquedaTradicional fachada = new FachadaBusquedaTradicional();
#else
            FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
#endif
                EjecutoriasTO ejecutoriaCompleta = fachada.getEjecutoriaPorId(Int32.Parse(ejecutoriaActual.Id));
                fachada.Close();
                posicion = registro;
                MostrarDatos(ejecutoriaCompleta);
                regNum.Text = "";
            }
            else
            {
                regNum.Text = "";
                MessageBox.Show(Mensajes.MENSAJE_CONSECUTIVO_NO_VALIDO, Mensajes.TITULO_CONSECUTIVO_NO_VALIDO,
                    MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }

        }
        public Historial getHistoria()
        {
            return this.historia;
        }
        public void setHistoria(Historial value)
        {
            this.historia = value;
            IUSNavigationService entradaHistorial = new IUSNavigationService();
            entradaHistorial.Id = Int32.Parse(this.DocumentoActual.Id);
            entradaHistorial.ParametroConstructor = ParametroConstruccion;
            entradaHistorial.TipoVentana = IUSNavigationService.EJECUTORIA;
            entradaHistorial.NavigationTarget = this;
            revisaHistorial(entradaHistorial);
            historia.NavigationProvider = this;
        }
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
                encontrado = ((item.Id == entrada.Id)&&(item.TipoVentana==IUSNavigationService.EJECUTORIA)) || encontrado;
            }
            if (!encontrado)
            {
                Historia.Lista.Add(entrada);
            }
        }

        private void BuscarImage_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            String validar = Validadores.BusquedaPalabraDocumento(this.TextoABuscar);
            if (!validar.Equals(Constants.CADENA_VACIA))
            {
                return;
            }
            String busquedaTexto = this.TextoABuscar.Text;
            TextPointer inicio = this.contenidoTexto.Selection.Start;
            TextPointer final = this.contenidoTexto.Selection.End;
            TextPointer lugarActual = null;
            if (inicio.CompareTo(final) == 0)
            {
                lugarActual = inicio;//this.contenidoTexto.Document.ContentStart;
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


        private void Guardar_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            if (!BrowserInteropHelper.IsBrowserHosted)
            {
                Imprimir_MouseButtonDown(sender, e);
                Microsoft.Win32.SaveFileDialog guardaEn = new Microsoft.Win32.SaveFileDialog();
                guardaEn.DefaultExt = ".rtf";
                guardaEn.Title = "Guardar una ejecutoria";
                guardaEn.Filter = "Archivos de Texto Enriquecido|*.rtf";
                guardaEn.AddExtension = true;
                EscondeVistaPrel();
                if ((bool)guardaEn.ShowDialog())
                {
                    FlowDocument documentoImprimir = DocumentoCopia;
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
                FlowDocument documentoImprimir = DocumentoCopia;
                impresion.Document = null;
                this.RtbCopyPaste.Document = documentoImprimir;
                System.IO.IsolatedStorage.IsolatedStorageFileStream archivo = new System.IO.IsolatedStorage.
                    IsolatedStorageFileStream("texto.rtf", System.IO.FileMode.Create);
                this.RtbCopyPaste.SelectAll();
                this.RtbCopyPaste.Selection.Save(archivo, System.Windows.DataFormats.Text);
                archivo.Flush();
                archivo.Close();
                MostrarDatos(DocumentoActual);
                MessageBox.Show("El archivo fue guardado como: " + archivo.Name);
            }
        }

        private void Portapapeles_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            //FlowDocument documentoOriginal = this.contenidoTexto.Document;
            Imprimir_MouseButtonDown(sender, e);
            FlowDocument documentoImprimir = DocumentoCopia;
            impresion.Document = null;
            this.RtbCopyPaste.Document = documentoImprimir;
            this.RtbCopyPaste.SelectAll();
            RtbCopyPaste.Copy();
            //MostrarDatos(DocumentoActual);
            //this.contenidoTexto.Document = documentoOriginal;
            EscondeVistaPrel();
            MessageBox.Show(Mensajes.MENSAJE_ENVIADO_PORTAPAPELES,
                Mensajes.TITULO_ENVIADO_PORTAPAPELES,
                MessageBoxButton.OK, MessageBoxImage.Information);
        }


        private void FontMayor_MouseLeftButtonDown(object sender, RoutedEventArgs e)
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
                verFontMayor = false;
            }
        }


        private void FontMenor_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            //this.contenidoTexto.SelectAll();
            FontMayor.Visibility = Visibility.Visible;
            verFontMayor = true;
            if (CalculosPropiedadesGlobales.FontSize > Constants.FONT_MENOR)
            {
                CalculosPropiedadesGlobales.FontSize--;
                this.contenidoTexto.SetValue(RichTextBox.FontSizeProperty, CalculosPropiedadesGlobales.FontSize);
            }
            else
            {
                FontMenor.Visibility = Visibility.Hidden;
                verFontMenor = false;
            }

        }

        #region marcarTodo

        private void MarcarTodo_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            int faltantes = 50 - marcados.Count;
            if (faltantes > ArregloTesis.Items.Count)
            {
                // El número de documentos en el arreglo cabe en los faltantes
                foreach (EjecutoriasSimplificadaTO Item in ArregloTesis.Items)
                {
                    marcados.Add(Int32.Parse(Item.Id));
                }
                MessageBox.Show(Mensajes.MENSAJE_TODOS_PORTAPAPELES + ArregloTesis.Items.Count +
                    Mensajes.MENSAJE_TODOS_PROTAPAPELES2, Mensajes.TITULO_TODOS_PORTAPAPELES,
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                this.ventanaRangos.Visibility = Visibility.Visible;
                this.ventanaRangos.InicioRango = 1;
                this.ventanaRangos.FinRango = 50;
                this.ventanaRangos.DiferenciaRangos = faltantes;
                this.ventanaRangos.StrMensaje = Mensajes.MENSAJE_RANGO_MARCAR + faltantes;
                this.ventanaRangos.RegistroFinal = this.ArregloTesis.Items.Count;
                this.ventanaRangos.Contenedor = this;
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
        private void Marcar_MouseEnter(object sender, MouseEventArgs e)
        {
            Image imagenNueva = new Image();
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            if (marcados.Contains(Int32.Parse(DocumentoActual.Id)))
            {
                bitmap.UriSource = new Uri("images/PALOMA1.png", UriKind.Relative);
            }
            else
            {
                bitmap.UriSource = new Uri("images/MARCAR2.png", UriKind.Relative);
            }
            bitmap.EndInit();
            this.Marcar.Source = bitmap;
        }

        private void Marcar_MouseLeave(object sender, MouseEventArgs e)
        {
            Image imagenNueva = new Image();
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            if (marcados.Contains(Int32.Parse(DocumentoActual.Id)))
            {
                bitmap.UriSource = new Uri("images/PALOMA1.png", UriKind.Relative);
            }
            else
            {
                bitmap.UriSource = new Uri("images/MARCAR1.png", UriKind.Relative);
            }
            bitmap.EndInit();
            this.Marcar.Source = bitmap;
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
                MessageBox.Show(Mensajes.MENSAJE_TODOS_PORTAPAPELES, Mensajes.TITULO_TODOS_PORTAPAPELES,
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        
        private void TextoABuscar_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.contenidoTexto.Selection.Select(contenidoTexto.Document.ContentStart, contenidoTexto.Document.ContentStart);
            EncontradaFrase = false;
        }


        internal void ActualizaRangoMarcado(int inicial, int FinRango)
        {
            for (int contador = inicial; contador <= FinRango; contador++)
            {
                EjecutoriasSimplificadaTO agregado = (EjecutoriasSimplificadaTO)this.ArregloTesis.Items.GetItemAt(contador - 1);
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

        private void TablasAnexos_Click(object sender, RoutedEventArgs e)
        {
            ventanaAnexos.Visibility = Visibility.Visible;
            ventanaAnexos.NavigationService = this;
        }



        public void MuestraAnexo(TablaPartesTO anexoSeleccionado)
        {
#if STAND_ALONE
            Informe ventanaNueva = new Informe(IUSConstants.IUS_RUTA_ANEXOS + anexoSeleccionado.Archivo.Substring(0,anexoSeleccionado.Archivo.Length-3) + "htm");
#else
            Informe ventanaNueva = new Informe("Anexos/"+anexoSeleccionado.Archivo.Substring(0,anexoSeleccionado.Archivo.Length-3) + "htm");
#endif
            NavigationService.Navigate(ventanaNueva);
            /// Esto es para poner en los Tabs los anexos, debe quedar así hasta que Microsoft
            /// solucione sus problemas de rendering.
            /// 
            //bool encontrado = false;
            //String tituloEsperado = anexoSeleccionado.Archivo.ToUpper();
            //tituloEsperado = tituloEsperado.Substring(0, tituloEsperado.Length - 3);
            //foreach (TabItem item in tabControl1.Items)
            //{
            //    bool esEste = item.Header.ToString().ToUpper().Equals(tituloEsperado);
            //    encontrado = encontrado || esEste;
            //    if (esEste)
            //    {
            //        item.Focus();
            //    }
            //}
            //if (!encontrado)
            //{
            //    TabItem nuevoTab = new TabItem();
            //    WebBrowser nuevoFrame = new WebBrowser();
            //    String tituloTab = tituloEsperado;
            //    Uri uriPdf = new Uri("Pack://siteoforigin:,,,/Anexos/" + tituloTab + "htm");
            //    nuevoTab.Header = tituloTab;
            //    nuevoTab.Content = nuevoFrame;
            //    tabControl1.Items.Add(nuevoTab);
            //    nuevoTab.Focus();
            //    nuevoFrame.Navigate(uriPdf);
            //    nuevoFrame.IsEnabled = false;
            //    this.Controles.Children.Remove(this.ventanasEmergentes);
            //    this.Controles.Children.Add(this.ventanasEmergentes);
            //    TablasAnexos_Click(null, null);
            //}
        }
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
                    dialogoImpresion.PrintDocument(pgn, "Impresión de ejecutorias");
                }
                catch (Exception exc)
                {
                    MessageBox.Show(Mensajes.MENSAJE_IMPRESORA, Mensajes.TITULO_ARCHIVO_ABIERTO,
                            MessageBoxButton.OK, MessageBoxImage.Error);
                }
                EscondeVistaPrel();
            }
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
        private void EscondeVistaPrel()
        {
            if(verVentanaListadoVotos) ventanaListadoVotos.Visibility=Visibility.Visible;
            if(verVentanaListadoTesis) ventanaListadoTesis.Visibility = Visibility.Visible;
            if (verVentanaHistorial) ventanaHistorial.Visibility = Visibility.Visible;
            if (verVentanaRangos) ventanaRangos.Visibility = Visibility.Visible;
            if (verVentanaAnexos) ventanaAnexos.Visibility = Visibility.Visible;
            
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
                //Marcar.Visibility = Visibility.Visible;
                //MarcarTodo.Visibility = Visibility.Visible;
                //Desmarcar.Visibility = Visibility.Visible;
            }
            if (verHistorial)
            {
                historial.Visibility = Visibility.Visible;
            }
#if STAND_ALONE
            if (documentosTesis.Count > 0)
#else
            if (documentosTesis.Length > 0)
#endif
            {
                tesis.Visibility = Visibility.Visible;
            }
#if STAND_ALONE
            if (documentosVotos.Count > 0)
#else
            if (documentosVotos.Length > 0)
#endif
            {
                voto.Visibility = Visibility.Visible;
            }
            Portapapeles.Visibility = Visibility.Visible;
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
            SalaLabel.Visibility = Visibility.Visible;
            VolumenLabel.Visibility = Visibility.Visible;
            PaginaLabel.Visibility = Visibility.Visible;
            if (ExistenTablas)
            {
                TablasAnexos.Visibility = Visibility.Visible;
            }
            LblPalabraBuscar.Visibility = Visibility.Visible;
            TextoABuscar.Visibility = Visibility.Visible;
            BuscarImage.Visibility = Visibility.Visible;
            Expresion.Visibility = Visibility.Visible;
            tabControl1.Visibility = Visibility.Visible;
            impresion.Visibility = Visibility.Hidden;
#if STAND_ALONE
            if ((partes.Count > 1) && (numeroParte > -1))
#else
            if ((partes.Length > 1) && (numeroParte > -1))
#endif
            {
                NumeroPartes.Visibility = Visibility.Visible;
                docCompletoImage.Visibility = Visibility.Visible;
                parteInicio.Visibility = Visibility.Visible;
                parteAnterior.Visibility = Visibility.Visible;
                parteSiguiente.Visibility = Visibility.Visible;
                parteFinal.Visibility = Visibility.Visible;
            }
        }
        private void MuestraVistaPrel()
        {
            verVentanaListadoVotos = ventanaListadoVotos.Visibility == Visibility.Visible;
            verVentanaListadoTesis = ventanaListadoTesis.Visibility == Visibility.Visible;
            verVentanaHistorial = ventanaHistorial.Visibility == Visibility.Visible;
            verVentanaRangos = ventanaRangos.Visibility == Visibility.Visible;
            verVentanaAnexos = ventanaRangos.Visibility == Visibility.Visible;
            ventanaListadoVotos.Visibility=Visibility.Hidden;
            ventanaListadoTesis.Visibility=Visibility.Hidden;
            ventanaHistorial.Visibility=Visibility.Hidden;
            ventanaRangos.Visibility=Visibility.Hidden;
            ventanaAnexos.Visibility = Visibility.Hidden;
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
            if (historial.Visibility == Visibility.Visible) verHistorial = true;
            historial.Visibility = Visibility.Hidden;
            tesis.Visibility = Visibility.Hidden;
            voto.Visibility = Visibility.Hidden;
            TablasAnexos.Visibility = Visibility.Hidden;
            Portapapeles.Visibility = Visibility.Hidden;
            Imprimir.Visibility = Visibility.Hidden;
            imprimePapel.Visibility = Visibility.Visible;
            BtnTache.Visibility = Visibility.Visible;
            FontMayor.Visibility = Visibility.Hidden;
            FontMenor.Visibility = Visibility.Hidden;
            //Marcar.Visibility = Visibility.Hidden;
            //MarcarTodo.Visibility = Visibility.Hidden;
            //Desmarcar.Visibility = Visibility.Hidden;
            Salir.Visibility = Visibility.Hidden;
            Guardar.Visibility = Visibility.Hidden;
            fuenteLabel.Visibility = Visibility.Hidden;
            EpocaLabel.Visibility = Visibility.Hidden;
            IdLabel.Visibility = Visibility.Hidden;
            SalaLabel.Visibility = Visibility.Hidden;
            VolumenLabel.Visibility = Visibility.Hidden;
            PaginaLabel.Visibility = Visibility.Hidden;
            LblPalabraBuscar.Visibility = Visibility.Hidden;
            TextoABuscar.Visibility = Visibility.Hidden;
            BuscarImage.Visibility = Visibility.Hidden;
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

        private void TextoABuscar_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                BuscarImage_MouseLeftButtonDown(sender, e);
            }
        }

        private void contenidoTexto_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key.Equals(Key.Enter) && (!TextoABuscar.Text.Equals("")))
            {
                BuscarImage_MouseLeftButtonDown(sender, null);
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
