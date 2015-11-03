using System.Windows.Controls;
using System;
using mx.gob.scjn.ius_common.TO;
using mx.gob.scjn.ius_common.gui.utils;
using System.Collections.Generic;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows;
using mx.gob.scjn.ius_common.fachade;
using mx.gob.scjn.ius_common.utils;
using System.Windows.Media.Imaging;
using mx.gob.scjn.ius_common.gui.impresion;
using System.Windows.Media;
using System.Windows.Interop;
using mx.gob.scjn.ius_common.gui.gui.impresion;
//using System.Speech.Synthesis;

namespace IUS
{
    /// <summary>
    /// Interaction logic for Tesis.xaml
    /// </summary>
    public partial class Tesis : Page
    {
        #region propiedades
        /// <summary>
        /// El control del datagrid propio de la clase, para ser tratado como
        /// proveedor de datos en el caso de el uso de las flechas de navegación.
        /// </summary>
        private Xceed.Wpf.DataGrid.DataGridControl ArregloTesis { get; set; }
        //public readonly DependencyProperty HistorialVisibilidad = DependencyProperty.Register("HistorialVisibiidad",
        //    typeof(Visibility), typeof(Tesis), new PropertyMetadata(Visibility.Visible));
        /// <summary>
        /// El identificador de la genealogía que tiene la tesis actual.
        /// </summary>
        private String genealogiaId { get; set; }
        /// <summary>
        /// La posición que tiene el documento dentro del proveedor de datos.
        /// </summary>
        private int posicion = 0;
        private FlowDocument documentoPrecedentes;
        private FlowDocument documentoRubro;
        public Object Parametro { get; set; }
        public Boolean ImprimirCompleto { get; set; }
        /// <summary>
        /// La relación con los documentos de ejecutoria que tiene la tesis.
        /// </summary>
#if STAND_ALONE
        private List<RelDocumentoTesisTO> documentosEjecutoria { get; set; }
#else
        private RelDocumentoTesisTO[] documentosEjecutoria { get; set; }
#endif
        /// <summary>
        /// La relación que tiene la tesis con los votos que la produjeron.
        /// </summary>
#if STAND_ALONE
        private List<RelDocumentoTesisTO> documentosVotos { get; set; }
#else
        private RelDocumentoTesisTO[] documentosVotos { get; set; }
#endif
        /// <summary>
        /// El documento de la tesis actual.
        /// </summary>
        private TesisTO DocumentoActual;
        /// <summary>
        /// Las observaciones realizadas a los  documentos de la tesis.
        /// </summary>
#if STAND_ALONE
        List<OtrosTextosTO> observaciones;
#else
        OtrosTextosTO[] observaciones;
#endif
        ///<summary>
        ///El historial que existe en la navegacion.
        ///</summary>
        public Historial Historia { get { return getHistoria(); } set { setHistoria(value); } }
        private Historial historia;
        public Visibility HistorialVisibilidad_
        {
            get {
                if (Historia == null)
                {
                    //SetValue(HistorialVisibilidad, Visibility.Collapsed);
                    return Visibility.Collapsed;
                }
                return Historia.Lista.Count > 0 ? Visibility.Visible : Visibility.Collapsed; } }
        /// <summary>
        /// La lista del historial y sus ligas
        /// </summary>
        private List<IUSHyperlink> ArregloLigas;
        public List<List<RelacionFraseArticulosTO>> listaLeyes;
        /// <summary>
        /// La busqueda solicitada desde el panel
        /// </summary>
        public BusquedaTO busquedaSolicitada { get; set; }
        ///
        /// <summary>Página a la que debe regresar.</summary>
        /// 
        public Page Back;
        /// <summary>
        ///     Registros que han sido marcados
        /// </summary>
        /// <remarks>
        ///     La llave del arreglo es el Identificador del documento
        /// </remarks>
        public static HashSet<int> marcados;
        private FlowDocument documentoCopia { get; set; }
        
        /// <summary>
        ///     Define si la frase fue localizada o no
        /// </summary>
        /// <value>
        ///     <para>
        ///         True si la frase se encontro, False en caso contrario
        ///     </para>
        /// </value>
        protected bool EncontradaFrase { get; set; }
        /// <summary>
        ///     Define si la contradicción existe, si lo es hay que pintar su botón
        /// </summary>
        protected bool contradiccionExiste;
        /// <summary>
        ///     Define si la ventana de rangos está abierta
        /// </summary>
        protected bool verVentanaRangos;
        /// <summary>
        ///     Define si la ventana emergente está abierta.
        /// </summary>
        protected bool verVentanaEmergente { get; set; }
        protected bool verVentanaListaEjecutorias { get; set; }
        protected bool verVentanaListaVotos { get; set; }
        protected bool verVentanaListadoLeyes { get; set; }
        protected bool verVentanitaLeyes { get; set; }
        protected bool verVentanaAnexos { get; set; }
        protected bool verVentanaHistorial { get; set; }
        protected bool verMaterias { get; set; }
        protected bool verFontMayor { get; set; }
        protected bool verFontMenor { get; set; }
        protected bool verBtnTemas { get; set; }
        protected bool mostrarVP { get; set; }
        public bool EstablecerBotonSalirVisibilidad { get { return (Salir.Visibility == Visibility.Visible); } set { if (!value)Salir.Visibility = Visibility.Collapsed; } }
        #endregion
        #region constructores
        /// <summary>
        /// Constructor por omisión. Usese solo como prueba, no llena ninguna
        /// parte del documento.
        /// </summary>
        /// 
        public Tesis()
        {
            InitializeComponent();
            mostrarVP = true;
            ventanitaLeyes.Padre = this;
            Parametro = null;
            ArregloLigas = null;
            this.contenidoTexto.SetValue(RichTextBox.FontSizeProperty, CalculosPropiedadesGlobales.FontSize);
            verFontMenor = true;
            verFontMayor = true;
            ventanaImprimirCompleta.Padre = this;
        }
        /// <summary>
        /// Constructor  para presentar solamente una tesis, 
        /// sin ligas a otras tesis.
        /// </summary>
        /// <param name="componenteObjeto">La tesis a mostrar</param>
        public Tesis(TesisTO componenteObjeto)
        {
            InitializeComponent();
            //Historia = new Historial();
            mostrarVP = true;
            this.contenidoTexto.SetValue(RichTextBox.FontSizeProperty, CalculosPropiedadesGlobales.FontSize);
            verFontMenor = true;
            verFontMayor = true;
            ArregloLigas = new List<IUSHyperlink>();
            ventanitaLeyes.Padre = this;
            ventanaImprimirCompleta.Padre = this;
#if STAND_ALONE
            FachadaBusquedaTradicional fachada = new FachadaBusquedaTradicional();
#else
            FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
#endif
            TesisTO documentoActual = componenteObjeto;
            MostrarDatos(documentoActual);
            fachada.Close();
            //Si entran a este constructor es por que vienen 
            //para ver solamente un registro
            this.inicioLista.Visibility = Visibility.Hidden;
            this.anteriorLista.Visibility = Visibility.Hidden;
            this.siguienteLista.Visibility = Visibility.Hidden;
            this.ultimoLista.Visibility = Visibility.Hidden;
            this.regNum.Visibility = Visibility.Hidden;
            this.RegNum.Visibility = Visibility.Hidden;
            this.IrALabel.Visibility = Visibility.Hidden;
            //this.IrBoton.Visibility = Visibility.Hidden;
            this.MarcarTodo.Visibility = Visibility.Hidden;
            this.Marcar.Visibility = Visibility.Hidden;
            this.Desmarcar.Visibility = Visibility.Hidden;
            Parametro = this;
        }

        public Tesis(BusquedaTO panel)
        {
            InitializeComponent();
            mostrarVP = true;
            Parametro = this;
            this.contenidoTexto.SetValue(RichTextBox.FontSizeProperty, CalculosPropiedadesGlobales.FontSize);
            ventanaImprimirCompleta.Padre = this;
            verFontMenor = true;
            verFontMayor = true;
        }
        /// <summary>
        /// Constructor cuando la tesis a mostrar es parte de un registro
        /// seleccionado desde el resultado de la búsqueda
        /// </summary>
        /// <param name="records">Los registros del resultado de la búsqueda</param>
        /// <param name="busqueda">Los parámetros que orifinaron la búsqueda</param>
        public Tesis(Xceed.Wpf.DataGrid.DataGridControl records, BusquedaTO panel)
        {
            InitializeComponent();
            //mostrarVP = true;
            CalculosPropiedadesGlobales.FontSize = Constants.FONTSIZE; 
            ArregloLigas = new List<IUSHyperlink>();
            marcados = new HashSet<int>();
            ventanitaLeyes.Padre = this;
            TesisSimplificadaTO tesisMostrar = (TesisSimplificadaTO)records.SelectedItem;
            ventanaImprimirCompleta.Padre = this;
            posicion = records.SelectedIndex;
#if STAND_ALONE
            FachadaBusquedaTradicional fachada = new FachadaBusquedaTradicional();
#else
            FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
#endif
            TesisTO documentoActual = fachada.getTesisPorRegistroLiga(tesisMostrar.Ius);
            documentoActual.Ius = tesisMostrar.Ius;
            fachada.Close();
            this.ArregloTesis = records;
            //Si hay palabras que pintar
            if ((panel != null))
            {
                this.busquedaSolicitada = panel;
            }
            //fin de las palabras a pintar
            MostrarDatos(documentoActual);
            Historia = new Historial();
            if (records.Items.Count == 1)
            {
                inicioLista.Visibility = Visibility.Hidden;
                anteriorLista.Visibility = Visibility.Hidden;
                siguienteLista.Visibility = Visibility.Hidden;
                ultimoLista.Visibility = Visibility.Hidden;
                IrALabel.Visibility = Visibility.Hidden;
                //this.IrBoton.Visibility = Visibility.Hidden;
                regNum.Visibility = Visibility.Hidden;
                RegNum.Visibility = Visibility.Hidden;
                Marcar.Visibility = Visibility.Hidden;
                MarcarTodo.Visibility = Visibility.Hidden;
                Desmarcar.Visibility = Visibility.Hidden;
            }            
            Parametro = this;
            this.contenidoTexto.SetValue(RichTextBox.FontSizeProperty, CalculosPropiedadesGlobales.FontSize);
            verFontMenor = true;
            verFontMayor = true;
        }
        #endregion
        /// <summary>
        /// Hay que mostrar la ventana del historial.
        /// </summary>
        /// <param name="sender">mouse</param>
        /// <param name="e">donde dio el click</param>
        private void historial_MouseButtonDown(object sender, RoutedEventArgs e)
        {
            if (this.Historia.Lista.Count < 2)
            {
                MessageBox.Show(Mensajes.MENSAJE_HISTORIAL_CON_UNO, Mensajes.TITULO_HISTORIAL_CON_UNO,
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            this.Historia.ventana = this.ventanaHistorial;
            this.Historia.NavigationProvider = this;
            this.Historia.OpenWindow();
        }
        #region imprimir
        private void Imprimir_MouseButtonDown(object sender, RoutedEventArgs e)
        {
            DocumentoTesis documento;
            if (imprimePapel.Visibility == Visibility.Hidden)
            {
                //impresion.ToolTip = Constants.VISTA_PRELIMINAR_FUERA;
                impresion.Document = null;
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
                    documento = new DocumentoTesis(DocumentoActual);
                }
                else if (resultadoMsgBox.Equals(MessageBoxResult.Yes))
                {
                    documento = new DocumentoTesis(marcados, false);
                }
                else
                {
                    return;
                }
                impresion.Document = documento.Documento; //(IDocumentPaginatorSource)documentoXps;
                documentoCopia = documento.DocumentoCopia;
                if (mostrarVP)
                {
                    MuestraVistaPrel();
                }
            }
            else
            {
              //  impresion.Document = documento.Documento; //(IDocumentPaginatorSource)documentoXps;
                //impresion.ToolTip = Constants.VISTA_PRELIMINAR;
                impresion.Visibility = Visibility.Hidden;
                tabControl1.Visibility = Visibility.Visible;
                imprimePapel.Visibility = Visibility.Hidden;
                textoAbuscar.Visibility = Visibility.Visible;
                Buscar.Visibility = Visibility.Visible;
                Expresion.Visibility = Visibility.Visible;
                EscondeVistaPrel();
            }
        }
        #endregion
        #region ejecutoria

        private void ejecutoria_Click(object sender, RoutedEventArgs e)
        {
#if STAND_ALONE
            if (documentosEjecutoria.Count == 1)
#else
            if (documentosEjecutoria.Length == 1)
#endif
            {
                try
                {
                    foreach (IUSNavigationService item in Historia.Lista)
                    {
                        if ((item.Id == Int32.Parse(documentosEjecutoria[0].Id)) && (item.TipoVentana == IUSNavigationService.EJECUTORIA))
                        {
                            MessageBox.Show(Mensajes.MENSAJE_VENTANA_ABIERTA, Mensajes.TITULO_VENTANA_ABIERTA,
                                MessageBoxButton.OK, MessageBoxImage.Information);
                            EjecutoriaPagina paginaEje = (EjecutoriaPagina)item.ParametroConstructor;
                            paginaEje.ventanaHistorial.Visibility = Visibility.Hidden;
                            paginaEje.ventanaListadoTesis.Visibility = Visibility.Hidden;
                            paginaEje.ventanaListadoVotos.Visibility = Visibility.Hidden;
                            NavigationService.Navigate(paginaEje);
                            return;
                        }
                    }
                    EjecutoriaPagina ejecutoriaAsociada = new EjecutoriaPagina(Int32.Parse(documentosEjecutoria[0].Id));
                    ejecutoriaAsociada.Historia = this.Historia;
                    this.NavigationService.Navigate(ejecutoriaAsociada);
                }
                catch (Exception exc)
                {
                    MessageBox.Show("Dieron Click muy rápido"+exc.Message);
                }
            }
            else
            {
                List<int> identificadores = new List<int>();
                this.ventanaListaEjecutorias.NavigationService = this.NavigationService;
                foreach (RelDocumentoTesisTO item in documentosEjecutoria)
                {
                    identificadores.Add(Int32.Parse(item.Id));
                }
                MostrarPorIusTO parametros = new MostrarPorIusTO();
                parametros.OrderBy = "ConsecIndx";
                parametros.OrderType = "asc";
#if STAND_ALONE
                FachadaBusquedaTradicional fachada = new FachadaBusquedaTradicional();
                parametros.Listado = identificadores;
                List<EjecutoriasTO> ejecutoriasRelacionadas = fachada.getEjecutoriasPorIds(parametros);
#else
            FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
                parametros.Listado = identificadores.ToArray();
                EjecutoriasTO[] ejecutoriasRelacionadas = fachada.getEjecutoriasPorIds(parametros);
#endif
                List<EjecutoriasTO> listaFinal = new List<EjecutoriasTO>();
                foreach (EjecutoriasTO item in ejecutoriasRelacionadas)
                {
                    listaFinal.Add(item);
                }
                this.ventanaListaEjecutorias.ListaRelacion = listaFinal;
                this.ventanaListaEjecutorias.Historia = this.Historia;
                this.ventanaListaEjecutorias.Visibility = Visibility.Visible;
            }
        }
        #endregion
        #region voto
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
                    if ((item.Id == Int32.Parse(documentosVotos[0].Id)) && (item.TipoVentana == IUSNavigationService.VOTO))
                    {
                        MessageBox.Show(Mensajes.MENSAJE_VENTANA_ABIERTA, Mensajes.TITULO_VENTANA_ABIERTA,
                            MessageBoxButton.OK, MessageBoxImage.Information);
                        VotosPagina votoPag = (VotosPagina)item.ParametroConstructor;
                        votoPag.ventanaHistorial.Visibility = Visibility.Hidden;
                        votoPag.ventanaListadoEjecutorias.Visibility = Visibility.Hidden;
                        votoPag.ventanaListadoTesis.Visibility = Visibility.Hidden; ;
                        NavigationService.Navigate(votoPag);
                        return;
                    }
                }
                VotosPagina votoAsociado = new VotosPagina(Int32.Parse(documentosVotos[0].Id));
                votoAsociado.Historia = this.Historia;
                votoAsociado.Back = this;
                this.NavigationService.Navigate(votoAsociado);
            }
            else
            {
                List<int> identificadores = new List<int>();
                this.ventanaListaVotos.NavigationService = this;
                foreach (RelDocumentoTesisTO item in documentosVotos)
                {
                    identificadores.Add(Int32.Parse(item.Id));
                }
                MostrarPorIusTO parametros = new MostrarPorIusTO();
                parametros.OrderBy = "ConsecIndx";
                parametros.OrderType = "asc";
#if STAND_ALONE
                FachadaBusquedaTradicional fachada = new FachadaBusquedaTradicional();
                parametros.Listado = identificadores;
                List<VotosTO> votosRelacionadas = fachada.getVotosPorIds(parametros);
#else
            FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
                parametros.Listado = identificadores.ToArray();
                VotosTO[] votosRelacionadas = fachada.getVotosPorIds(parametros);
#endif
                List<VotoSimplificadoTO> listaFinal = new List<VotoSimplificadoTO>();
                foreach (VotosTO item in votosRelacionadas)
                {
                    VotoSimplificadoTO itemVerdadero = new VotoSimplificadoTO(item);
                    listaFinal.Add(itemVerdadero);
                }
                this.ventanaListaVotos.ListaRelacion = listaFinal;
                this.ventanaListaVotos.Historia = this.Historia;
                this.ventanaListaVotos.NavigationService = this;
                this.ventanaListaVotos.Visibility = Visibility.Visible;
            }
        }
        #endregion
        #region genealogia
        private void genealogia_MouseButtonDown(object sender, RoutedEventArgs e)
        {
#if STAND_ALONE
            FachadaBusquedaTradicional fachada = new FachadaBusquedaTradicional();
#else
            FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
#endif
            GenealogiaTO genealogiaObj = fachada.getGenalogia(DocumentoActual.IdGenealogia);
            fachada.Close();
            FlowDocument documentoGenealogia = new FlowDocument();
            Paragraph parrafoGenealogia = new Paragraph(new Run(genealogiaObj.TxtGenealogia));
            parrafoGenealogia.TextAlignment = TextAlignment.Justify;
            documentoGenealogia.Blocks.Add(parrafoGenealogia);
            ventanaEmergente.titulo.Text = "Genealogía";
            ventanaEmergente.contenido.Document = documentoGenealogia;
            ventanaEmergente.Visibility = Visibility.Visible;
        }
        #endregion
        #region botonesLista
        private void inicioLista_MouseButtonDown(object sender, RoutedEventArgs e)
        {
#if STAND_ALONE
            FachadaBusquedaTradicional fachada = new FachadaBusquedaTradicional();
#else
            FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
#endif
            List<TesisSimplificadaTO> presentadorDatos = (List<TesisSimplificadaTO>)this.ArregloTesis.ItemsSource;
            TesisSimplificadaTO tesisMostrar = presentadorDatos[0];
            posicion = 0;
            this.ArregloTesis.SelectedIndex = posicion;
            TesisTO documentoActual = fachada.getTesisPorRegistro(tesisMostrar.Ius);
            fachada.Close();
            //documentoActual.Ius=tesisMostrar.Ius;
            MostrarDatos(documentoActual);
            this.Historia.Lista.Clear();
            this.Historia = historia;
        }

        private void anteriorLista_MouseButtonDown(object sender, RoutedEventArgs e)
        {
            List<TesisSimplificadaTO> presentadorDatos = (List<TesisSimplificadaTO>)this.ArregloTesis.ItemsSource;
            TesisSimplificadaTO tesisMostrar = null;
            if (posicion == 0)
            {
                tesisMostrar = presentadorDatos[0];
            }
            else
            {
                posicion--;
                this.ArregloTesis.SelectedIndex = posicion;
                tesisMostrar = presentadorDatos[posicion];
            }
            TesisSimplificadaTO documentoActual = new TesisSimplificadaTO();
            documentoActual.Ius=tesisMostrar.Ius;
            while(MostrarDatos(documentoActual)==Constants.ERROR_FACHADA){}
            this.Historia.Lista.Clear();
            this.Historia = historia;
        }

        private int MostrarDatos(TesisSimplificadaTO documentoActual)
        {
#if STAND_ALONE
            FachadaBusquedaTradicional fachada = new FachadaBusquedaTradicional();
#else
            FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
#endif
            TesisTO tesisVerdadera = fachada.getTesisPorRegistroLiga(documentoActual.Ius);
            fachada.Close();
            if (tesisVerdadera.Ius != null)
            {
                MostrarDatos(tesisVerdadera);
                return Constants.NO_ERROR;
            }
            else
            {
                return Constants.ERROR_FACHADA;
            }
        }

        private void siguienteLista_MouseButtonDown(object sender, RoutedEventArgs e)
        {
            List<TesisSimplificadaTO> presentadorDatos = (List<TesisSimplificadaTO>)this.ArregloTesis.ItemsSource;
            TesisSimplificadaTO tesisMostrar = null;
            if (posicion >= presentadorDatos.Count - 1)
            {
                posicion = presentadorDatos.Count - 1;
                tesisMostrar = presentadorDatos[posicion];
            }
            else
            {
                posicion++;
                this.ArregloTesis.SelectedIndex = posicion;
                tesisMostrar = presentadorDatos[posicion];
            }
            TesisSimplificadaTO documentoActual = new TesisSimplificadaTO();
            documentoActual.Ius = tesisMostrar.Ius;
            MostrarDatos(documentoActual);
            this.Historia.Lista.Clear();
            this.Historia = historia;
        }

        private void ultimoLista_MouseButtonDown(object sender, RoutedEventArgs e)
        {
            List<TesisSimplificadaTO> presentadorDatos = (List<TesisSimplificadaTO>)this.ArregloTesis.ItemsSource;
            TesisSimplificadaTO tesisMostrar = null;
            posicion = presentadorDatos.Count - 1;
            this.ArregloTesis.SelectedIndex = posicion;
            tesisMostrar = presentadorDatos[posicion];
            TesisSimplificadaTO documentoActual = new TesisSimplificadaTO();
            documentoActual.Ius = tesisMostrar.Ius;
            MostrarDatos(documentoActual);
            this.Historia.Lista.Clear();
            this.Historia = historia;
        }
        #endregion
        #region Salir
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
        #endregion
        #region observaciones

        private void observacionesBot_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            FlowDocument documentoObservaciones = new FlowDocument();
            foreach (OtrosTextosTO item in observaciones)
            {
                if (Int32.Parse(item.TipoNota) == 2)
                {
                    Paragraph parrafoObservacion = new Paragraph(new Run(item.Textos));
                    parrafoObservacion.TextAlignment = TextAlignment.Justify;
                    documentoObservaciones.Blocks.Add(parrafoObservacion);
                }
            }
            ventanaEmergente.contenido.Document = documentoObservaciones;
            ventanaEmergente.titulo.Text = "Observaciones";
            ventanaEmergente.Visibility = Visibility.Visible;

        }
        #endregion
        /// <summary>
        /// Genera que se muestre un registro determinado dentro de
        /// el contenido de la lista de resultados
        /// </summary>
        /// <param name="sender">Parámetros de la Interfaz Gráfica</param>
        /// <param name="e">Parámetros de la inerfaz Gráfica</param>
        private void IrBoton_Click(object sender, RoutedEventArgs e)
        {
            if (regNum.Text.Equals(""))
            {
                MessageBox.Show(Mensajes.MENSAJE_CAMPO_TEXTO_NUMERO_VACIO, Mensajes.TITULO_CAMPO_REQUERIDO,
                    MessageBoxButton.OK, MessageBoxImage.Error);
                regNum.Focus();
                return;
            }

            else
            {
                int registro = Int32.Parse(regNum.Text);
                List<TesisSimplificadaTO> arregloTesisActual = (List<TesisSimplificadaTO>)ArregloTesis.ItemsSource;
                if (registro > 0 && registro <= arregloTesisActual.Count)
                {
                    registro--;
                    TesisSimplificadaTO tesisActual = arregloTesisActual[registro];
                    TesisSimplificadaTO tesisCompleta = new TesisSimplificadaTO();
                    tesisCompleta.Ius = tesisActual.Ius;
                    posicion = registro;
                    ArregloTesis.SelectedIndex = registro;
                    MostrarDatos(tesisCompleta);
                    regNum.Text = "";
                }
                else
                {
                    regNum.Text = "";
                    int registroIus = CalculosGlobales.EncuentraPosicionTesis((List<TesisSimplificadaTO>)ArregloTesis.ItemsSource, registro);
                    if (registroIus < 0)
                    {
                        MessageBox.Show(Mensajes.MENSAJE_CONSECUTIVO_NO_VALIDO, Mensajes.TITULO_CONSECUTIVO_NO_VALIDO,
                            MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    }
                    else
                    {
                        TesisSimplificadaTO tesisActual = arregloTesisActual[registroIus];
                        TesisSimplificadaTO tesisCompleta = new TesisSimplificadaTO();
                        tesisCompleta.Ius = tesisActual.Ius;
                        posicion = registroIus;
                        ArregloTesis.SelectedIndex = registroIus;
                        MostrarDatos(tesisCompleta);
                        regNum.Text = "";
                    }
                }
            }
        }
        /// <summary>
        /// Genera el procedimiento de poner el historial al día incluyendo a la tesis en caso de no tenerla.
        /// </summary>
        /// <param name="value">El historial que se usará.</param>
        protected void setHistoria(Historial value)
        {
            try
            {
                this.historia = value;
                IUSNavigationService entradaHistorial = new IUSNavigationService();
                entradaHistorial.Id = Int32.Parse(this.DocumentoActual.Ius);
                entradaHistorial.ParametroConstructor = Parametro;
                entradaHistorial.TipoVentana = IUSNavigationService.TESIS;
                revisaHistorial(entradaHistorial);
                historia.NavigationProvider = this;
                foreach (IUSHyperlink item in ArregloLigas)
                {
                    item.Historia = value;
                    item.PaginaTarget = this;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(Mensajes.MENSAJE_INTENTE_DE_NUEVO, Mensajes.TITULO_INTENTE_DE_NUEVO,
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        /// <summary>
        /// Obtiene el historial que se está usando.
        /// </summary>
        /// <returns>El historial actual.</returns>
        protected Historial getHistoria()
        {
            return this.historia;
        }
        /// <summary>
        /// Muestra los datos de una tesis determinada.
        /// </summary>
        /// <param name="documentoActual">El documento que se va a mostrar en pantalla.</param>
        private void MostrarDatos(TesisTO documentoActual)
        {
            try
            {
                ArregloLigas.Clear();
#if STAND_ALONE
                List<OtrosTextosTO> contradiccion;
                List<TreeNodeDataTO> TemasRelacionados;
#else
                OtrosTextosTO[] contradiccion;
                TreeNodeDataTO[] TemasRelacionados;
#endif
                this.TesisTesisLabel.Content = documentoActual.Tesis;
                String fuente = new String(documentoActual.LocAbr.ToCharArray());
                int lugarPuntoyComa = fuente.IndexOf(";");
                //String juris = fuente.Substring(0, lugarPuntoyComa);
                fuente = fuente.Substring(lugarPuntoyComa + 1, fuente.Length - lugarPuntoyComa - 1);
                lugarPuntoyComa = fuente.IndexOf(";");
                String epoca = fuente.Substring(0, lugarPuntoyComa);
                fuente = fuente.Substring(lugarPuntoyComa + 1, fuente.Length - lugarPuntoyComa - 1);
                if(epoca.Contains("Época"))
                lugarPuntoyComa = fuente.IndexOf(";");
                //String origen = fuente.Substring(0, lugarPuntoyComa);
                fuente = fuente.Substring(lugarPuntoyComa + 1, fuente.Length - lugarPuntoyComa - 1);
                lugarPuntoyComa = fuente.IndexOf(";");
                //origen = fuente.Substring(0, lugarPuntoyComa);
                fuente = fuente.Substring(lugarPuntoyComa + 1, fuente.Length - lugarPuntoyComa - 1);
                lugarPuntoyComa = fuente.IndexOf(";");
               //String fecha = fuente.Substring(0, lugarPuntoyComa);
                this.FechaText.Text = documentoActual.VolumenPrefijo + " " + documentoActual.Volumen;//fecha;
                lugarPuntoyComa = fuente.IndexOf(";");
                this.PaginaLabel.Content = "Pág. "+documentoActual.Pagina;
                
                if (documentoActual.Parte.Equals("99"))
                {
                    verMaterias = false;
                    AnuncioMaterias.Visibility = Visibility.Hidden;
                    Materias.Visibility = Visibility.Hidden;
                }
                else
                {
                    verMaterias = true;
                }
                if (documentoActual.ExistenNG)
                {
                    BtnNotaGenerica.Visibility = Visibility.Visible;
                }
                else
                {
                    BtnNotaGenerica.Visibility = Visibility.Hidden;
                }
                this.jurisLabel.Content = documentoActual.LocAbr.Contains("[J]") ? "Jurisprudencia" : "Tesis Aislada";//juris.Trim().Equals("[J]") ? "Jurisprudencia" : "Tesis Aislada";
                this.IUSLabel.Content = documentoActual.Ius;
                this.EpocaLabel.Content = documentoActual.Epoca;
                this.fuenteLabel.Content = documentoActual.Fuente;
                this.SalaLabel.Content = documentoActual.Sala;
                documentoPrecedentes = new FlowDocument();
                documentoRubro = new FlowDocument();
                FlowDocument documentoTexto = new FlowDocument();
                Paragraph precedentesParrafo = obtenLigas(documentoActual.Precedentes, documentoActual.Ius, Constants.SECCION_LIGAS_PRECEDENTES);
                precedentesParrafo.TextAlignment = TextAlignment.Justify;
                precedentesParrafo.FontStyle = FontStyles.Italic;
                documentoPrecedentes.Blocks.Add(precedentesParrafo);
                Paragraph rubroParrafo = obtenLigas(documentoActual.Rubro, documentoActual.Ius, Constants.SECCION_LIGAS_RUBRO);
                Paragraph textoparrafo = obtenLigas(documentoActual.Texto, documentoActual.Ius, Constants.SECCION_LIGAS_TEXTO);
                rubroParrafo.FontWeight = FontWeights.Bold;
                rubroParrafo.TextAlignment = TextAlignment.Justify;
                textoparrafo.TextAlignment = TextAlignment.Justify;
                documentoRubro.Blocks.Add(rubroParrafo);
                documentoTexto.Blocks.Add(textoparrafo);
                this.contenidoTexto.Document = documentoTexto;
                this.contenidoTexto.IsReadOnly = true;
                documentoTexto.IsEnabled = true;
                this.contenidoTexto.IsEnabled = true;
                if (documentoActual.IdGenealogia.Equals("0"))
                {
                    genealogia.Visibility = Visibility.Hidden;
                }
                else
                {
                    genealogia.Visibility = Visibility.Visible;
                }
#if STAND_ALONE
                FachadaBusquedaTradicional fachada = new FachadaBusquedaTradicional();
#else
                FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
#endif
                contradiccion = fachada.getNotasContradiccionPorIus(documentoActual.Ius);
                Materias.Content = fachada.getMateriasTesis(documentoActual.Ius);
                this.documentosEjecutoria = fachada.getEjecutoriaTesis(documentoActual.Ius);
                this.documentosVotos = fachada.getVotosTesis(documentoActual.Ius);
                LbxTemas.Items.Clear();
                BtnTemas.Visibility = documentoActual.ExistenTemas ? Visibility.Visible : Visibility.Collapsed;
                ventanaTemas.NavigationService = this;
                FrmTemas.SetValue(Panel.ZIndexProperty, 1);
                FrmTemas.Visibility = Visibility.Collapsed;
                LbxTemas.IsEditable = false;
                contradiccionExiste = false;
#if STAND_ALONE
                if (contradiccion.Count > 0)
#else
                if (contradiccion.Length > 0)
#endif
                {
                    contradiccionExiste = true;
                    this.contradiccion.Visibility = Visibility.Visible;
                    FlowDocument contenidoContradiccion = new FlowDocument();
                    Paragraph parrafo = new Paragraph();
                    IUSHyperlink liga = new IUSHyperlink();
                    if (contradiccion[0].version < 2)
                        liga.Inlines.Add(new Run("Superada por Contradicción"));
                    else
                        liga.Inlines.Add(new Run(contradiccion[0].TxtNotas));
                    liga.PaginaTarget = this;
                    liga.FontSize = 13;
                    if (contradiccion[0].TipoNota.Equals("2"))
                    {
                        liga.Tag = "Ejecutoria(" + contradiccion[0].Textos + ")";
                    }
                    else
                    {
                        liga.Tag = "Tesis(" + contradiccion[0].Textos + ")";
                    }
                    parrafo.Inlines.Add(liga);
                    
                    this.contradiccion.Document = contenidoContradiccion;
                    this.ArregloLigas.Add(liga);
                    contenidoContradiccion.Blocks.Add(parrafo);
                    this.contradiccion.IsDocumentEnabled = true;
                    this.contradiccion.IsEnabled = true;
                    this.contradiccion.IsReadOnly = true;
                }
                else
                {
                    this.contradiccion.Visibility = Visibility.Hidden;
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
                if (this.documentosEjecutoria.Count == 0)
#else
                if (this.documentosEjecutoria.Length == 0)
#endif
                {
                    ejecutoria.Visibility = Visibility.Hidden;
                }
                else
                {
                    ejecutoria.Visibility = Visibility.Visible;
                }
                observaciones = fachada.getOtrosTextosPorIus(documentoActual.Ius);
#if STAND_ALONE
                if (observaciones.Count == 0)
#else
                if (observaciones.Length == 0)
#endif
                {
                    observacionesBot.Visibility = Visibility.Hidden;
                    BtnConcordancia.Visibility = Visibility.Hidden;
                }
                else
                {
                    int observacionesCont = 0;
                    int concordanciasCont = 0;
                    foreach (OtrosTextosTO item in observaciones)
                    {
                        switch (item.TipoNota)
                        {
                            case "2":
                                observacionesCont++;
                                break;
                            case "3":
                                concordanciasCont++;
                                break;
                        }
                    }
                    if (observacionesCont > 0)
                    {
                        observacionesBot.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        observacionesBot.Visibility = Visibility.Hidden;
                    }
                    if (concordanciasCont > 0)
                    {
                        BtnConcordancia.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        BtnConcordancia.Visibility = Visibility.Hidden;
                    }
                }
                fachada.Close();
                if (ArregloTesis != null)
                {
                    List<TesisSimplificadaTO> lista = (List<TesisSimplificadaTO>)(ArregloTesis.ItemsSource);
                    int posicionReal = posicion + 1;
                    RegNum.Content = "" + posicionReal + " / " + lista.Count;
                    if (this.busquedaSolicitada != null)
                    {
                        Expresion.Content = CalculosGlobales.Expresion(this.busquedaSolicitada);
                    }
                    else
                    {
                        Expresion.Content = CalculosGlobales.Expresion(new List<int>());
                    }
                }
                else
                {
                    RegNum.Visibility = Visibility.Hidden;
                    anteriorLista.Visibility = Visibility.Hidden;
                    inicioLista.Visibility = Visibility.Hidden;
                    siguienteLista.Visibility = Visibility.Hidden;
                    ultimoLista.Visibility = Visibility.Hidden;
                    BloqueTextoIrA.Visibility = Visibility.Hidden;
                    IrALabel.Visibility = Visibility.Hidden;
                    //IrBoton.Visibility = Visibility.Hidden;
                    regNum.Visibility = Visibility.Hidden;
                    Expresion.Visibility = Visibility.Hidden;
                }
                DocumentoActual = documentoActual;
                impresion.Visibility = Visibility.Hidden;
                textoAbuscar.Text = Constants.CADENA_VACIA;
                imprimePapel.Visibility = Visibility.Hidden;
                tabControl1.Visibility = Visibility.Visible;
                ventanaEmergente.Visibility = Visibility.Hidden;
                ventanaHistorial.Visibility = Visibility.Hidden;
                ventanitaLeyes.Visibility = Visibility.Hidden;
                ventanaAnexos.Visibility = Visibility.Hidden;
                ventanaListaEjecutorias.Visibility = Visibility.Hidden;
                ventanaListaVotos.Visibility = Visibility.Hidden;
                textoAbuscar.Visibility = Visibility.Visible;
                Buscar.Visibility = Visibility.Visible;
                Expresion.Visibility = Visibility.Visible;
                if ((this.busquedaSolicitada != null) && (busquedaSolicitada.Palabra != null))
                {
                    PintaTesis(busquedaSolicitada.Palabra);
                }
                documentoRubro.Blocks.Add(documentoTexto.Blocks.FirstBlock);
                contenidoTexto.Document = documentoRubro;
                contenidoTexto.Document.Blocks.Add(new Paragraph(new Run("")));
                contenidoTexto.Document.Blocks.Add(documentoPrecedentes.Blocks.FirstBlock);
                if (marcados.Contains(Int32.Parse(DocumentoActual.Ius)))
                {
                    Image imagenNueva = new Image();
                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri("/General;component/images/PALOMA1.png", UriKind.Relative);
                    bitmap.EndInit();
                    this.Marcar.Source = bitmap;
                }
                else
                {
                    Image imagenNueva = new Image();
                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri("/General;component/images/MARCAR1.png", UriKind.Relative);
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
                if ((this.busquedaSolicitada != null) && (busquedaSolicitada.Palabra != null))
                {
                    FlowDocumentHighlight.UbicarPrimera(contenidoTexto);
                }
            }
            catch (Exception exc)
            {
                DocumentoActual = documentoActual;
                System.Console.WriteLine("Verifique que la conexión y los servicios de IUS estén disponibles: \n"
                    + " MostrarDatosException on Tesis " + exc.Message);
            }
        }
        #region ligas
        /// <summary>
        /// Define la cadena de texto para generar las ligas del documento hacia
        /// leyes u otros objetos similares.
        /// </summary>
        /// <param name="texto"> El texto ue tendrá la liga</param>
        /// <param name="ius"> El Ius del documento </param>
        /// <param name="seccion"> La seccion donde estará la liga</param>
        /// <returns>El párrafo con la liga adecuada.</returns>
        protected Paragraph obtenLigas(String texto, String ius, int seccion)
        {
            try
            {
                Paragraph resultado = new Paragraph(new Run(texto));
#if STAND_ALONE
            FachadaBusquedaTradicional fachada = new FachadaBusquedaTradicional();
            List<RelacionTO> listaRelaciones = fachada.getRelaciones(Int32.Parse(ius), seccion);
#else
                FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
                RelacionTO[] listaRelaciones = fachada.getRelaciones(Int32.Parse(ius), seccion);
#endif
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
                    foreach (RelacionTO item in listaRelaciones)
                    {
                        posicionFinal = Int32.Parse(item.Posicion);
                        if ((posicionFinal < posicionInicial))//viene mal el dato
                        {
                            posicionInicial = texto.IndexOf(item.MiDescriptor);
                        }
                        if (!(texto.Substring(posicionFinal).StartsWith(item.MiDescriptor)))
                        {
                            posicionFinal = texto.IndexOf(item.MiDescriptor);
                        }
                        //try
                        //{
                            cadenaNueva = texto.Substring(posicionInicial, posicionFinal - posicionInicial);
                        //}
                        //catch (Exception fueraDeRango)
                        //{
                        //    cadenaNueva = texto.Substring(posicionInicial, posicionFinal - (2 * posicionInicial));
                        //}
                        resultadosParciales.Add(cadenaNueva);
                        resultado.Inlines.Add(cadenaNueva);
                        resultado.Inlines.Add(creaLiga(item, item.MiDescriptor));
                        posicionInicial = posicionFinal + item.MiDescriptor.Length;
                    }
                    if (posicionInicial < texto.Length)
                    {
                        resultado.Inlines.Add(texto.Substring(posicionInicial));
                    }
                    resultado.IsEnabled = true;
                    return resultado;
                }
            }
            catch (Exception exc)
            {
                Paragraph resultado = new Paragraph(new Run(texto));
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
        private IUSHyperlink creaLiga(RelacionTO liga, string contenido)
        {
            RelacionFraseTesisTO tesis = null;
            RelacionFraseArticulosTO articulos = null;
#if STAND_ALONE
            FachadaBusquedaTradicional fachadaBusqueda = new FachadaBusquedaTradicional();
#else
            FachadaBusquedaTradicionalClient fachadaBusqueda = new FachadaBusquedaTradicionalClient();
#endif
            IUSHyperlink ligaNueva = new IUSHyperlink(this.ventanitaLeyes);
            ligaNueva.Inlines.Add(new Run(contenido));
            ligaNueva.IsEnabled = true;
            switch (liga.Tipo)
            {
                case "100":
                    ligaNueva = new IUSHyperlink(this.tabControl1);
                    ligaNueva.Inlines.Add(new Run(contenido));
                    ligaNueva.IsEnabled = true;
                    ligaNueva.Tag = "PDF(T" + liga.Ius + liga.IdRel + ".pdf)";
                    ligaNueva.Historia = Historia;
                    ligaNueva.PaginaTarget = this;
                    break;
                case "110":
                    ligaNueva = new IUSHyperlink(this.tabControl1);
#if STAND_ALONE
                    tesis = fachadaBusqueda.getRelacionesFraseTesis(Int32.Parse(liga.Ius,0),
                        Int32.Parse(liga.IdRel))[0];
#else
                    tesis = fachadaBusqueda.getRelacionesFrasesTesis(Int32.Parse(liga.Ius),
                        Int32.Parse(liga.IdRel))[0];
#endif
                    ligaNueva.Inlines.Add(new Run(contenido));
                    ligaNueva.IsEnabled = true;
                    ligaNueva.Tag = "PDF(" + tesis.IdLink + ".pdf)";
                    ligaNueva.Historia = Historia;
                    ligaNueva.PaginaTarget = this;
                    break;
                case "10":
#if STAND_ALONE
                    tesis = fachadaBusqueda.getRelacionesFraseTesis(Int32.Parse(liga.Ius),
                        Int32.Parse(liga.IdRel))[0];
#else
                    tesis = fachadaBusqueda.getRelacionesFrasesTesis(Int32.Parse(liga.Ius),
                        Int32.Parse(liga.IdRel))[0];
#endif
                    liga.Tipo = tesis.Tipo;
                    break;

            }
            ///
            switch (liga.Tipo)
            {
                case "0":
#if STAND_ALONE
                    List<RelacionFraseArticulosTO> articulosRelacionados = fachadaBusqueda.getRelacionesFraseArticulos(Int32.Parse(liga.Ius),
                        Int32.Parse(liga.IdRel),0);
                    if (articulosRelacionados.Count < 2)
#else
                    RelacionFraseArticulosTO[] articulosRelacionados = fachadaBusqueda.getRelacionesFrasesArticulos(Int32.Parse(liga.Ius),
                        Int32.Parse(liga.IdRel),0);
                    if (articulosRelacionados.Length < 2)
#endif
                    {
                        articulos = articulosRelacionados[0];
                        ligaNueva.Tag = "ventanaEmergente(" + articulos.IdLey +
                            "," + articulos.IdArt + "," + articulos.IdRef + ");";
                    }
                    else
                    {
                        ligaNueva = new IUSHyperlink(this.ventanaListadoLeyes);
                        ligaNueva.Inlines.Add(new Run(contenido));
                        ligaNueva.IsEnabled = true;
                        if (listaLeyes == null)
                        {
                            listaLeyes = new List<List<RelacionFraseArticulosTO>>();
                        }
                        ligaNueva.Tag = "ventanaEmergente(" + 0 +
                            "," + 0 + "," + listaLeyes.Count + ")";
                        ligaNueva.ListaLeyes = new List<RelacionFraseArticulosTO>();
                        foreach (RelacionFraseArticulosTO item in articulosRelacionados)
                        {
                            ligaNueva.ListaLeyes.Add(item);
                        }
                        listaLeyes.Add(ligaNueva.ListaLeyes);
                        this.ventanaListadoLeyes.VentanaLeyes = this.ventanitaLeyes;
                    }
                    ligaNueva.Historia = Historia;
                    ligaNueva.PaginaTarget = this;
                    break;
                case "1":
#if STAND_ALONE
                    List<RelacionFraseTesisTO> relsCompletas = fachadaBusqueda.getRelacionesFraseTesis(Int32.Parse(liga.Ius),
                        Int32.Parse(liga.IdRel));
#else
                    RelacionFraseTesisTO[] relsCompletas = fachadaBusqueda.getRelacionesFrasesTesis(Int32.Parse(liga.Ius),
                        Int32.Parse(liga.IdRel));
#endif
                    tesis = relsCompletas[0];
                    ligaNueva.PaginaTarget = this;
                    ligaNueva.Tag = "Tesis(" + tesis.IdLink + ")";
                    ligaNueva.Historia = Historia;
                    ligaNueva.PaginaTarget = this;
                    break;
                case "2":
#if STAND_ALONE
                    tesis = fachadaBusqueda.getRelacionesFraseTesis(Int32.Parse(liga.Ius),
                        Int32.Parse(liga.IdRel))[0];
#else
                    tesis = fachadaBusqueda.getRelacionesFrasesTesis(Int32.Parse(liga.Ius),
                        Int32.Parse(liga.IdRel))[0];
#endif
                    ligaNueva.PaginaTarget = this;
                    ligaNueva.Tag = "Ejecutoria(" + tesis.IdLink + ")";
                    ligaNueva.Historia = Historia;
                    ligaNueva.PaginaTarget = this;
                    break;
                case "3":
#if STAND_ALONE
                    tesis = fachadaBusqueda.getRelacionesFraseTesis(Int32.Parse(liga.Ius),
                        Int32.Parse(liga.IdRel))[0];
#else
                    tesis = fachadaBusqueda.getRelacionesFrasesTesis(Int32.Parse(liga.Ius),
                        Int32.Parse(liga.IdRel))[0];
#endif
                    ligaNueva.PaginaTarget = this;
                    ligaNueva.Tag = "Votos(" + tesis.IdLink + ")";
                    ligaNueva.Historia = Historia;
                    ligaNueva.PaginaTarget = this;
                    break;
                case "50":
#if STAND_ALONE
                    articulosRelacionados = fachadaBusqueda.getRelacionesFraseArticulos(Int32.Parse(liga.Ius),
                        Int32.Parse(liga.IdRel),1);
                    if (articulosRelacionados.Count < 2)
#else
                    articulosRelacionados = fachadaBusqueda.getRelacionesFrasesArticulos(Int32.Parse(liga.Ius),
                        Int32.Parse(liga.IdRel),1);
                    if (articulosRelacionados.Length < 2)
#endif
                    {
                        articulos = articulosRelacionados[0];
                        ligaNueva.Tag = "ventanaEmergente(" + articulos.IdLey +
                            "," + articulos.IdArt + "," + articulos.IdRef + ",1);";
                    }
                    else
                    {
                        ligaNueva = new IUSHyperlink(this.ventanaListadoLeyes);
                        ligaNueva.Inlines.Add(new Run(contenido));
                        ligaNueva.IsEnabled = true;
                        if (listaLeyes == null)
                        {
                            listaLeyes = new List<List<RelacionFraseArticulosTO>>();
                        }
                        ligaNueva.Tag = "ventanaEmergente(" + 0 +
                            "," + 0 + "," + listaLeyes.Count + ",1)";
                        ligaNueva.ListaLeyes = new List<RelacionFraseArticulosTO>();
                        foreach (RelacionFraseArticulosTO item in articulosRelacionados)
                        {
                            ligaNueva.ListaLeyes.Add(item);
                        }
                        listaLeyes.Add(ligaNueva.ListaLeyes);
                        this.ventanaListadoLeyes.VentanaLeyes = this.ventanitaLeyes;
                    }
                    ligaNueva.Historia = Historia;
                    ligaNueva.PaginaTarget = this;
                    break;
                case "100":
                    break;
                case "110":
                    break;
                default:
#if STAND_ALONE
                    relsCompletas=fachadaBusqueda.getRelacionesFraseTesis(Int32.Parse(liga.Ius),
                        Int32.Parse(liga.IdRel));
#else
                    relsCompletas=fachadaBusqueda.getRelacionesFrasesTesis(Int32.Parse(liga.Ius),
                        Int32.Parse(liga.IdRel));
#endif
                    tesis = relsCompletas[0];
                    ligaNueva.PaginaTarget = this;
                    ligaNueva.Tag = "Acuerdos(" + tesis.IdLink + ")";
                    ligaNueva.Historia = Historia;
                    ligaNueva.PaginaTarget = this;
                    break;
            }
            fachadaBusqueda.Close();
            ArregloLigas.Add(ligaNueva);
            return ligaNueva;
        }
        #endregion
        #region HistorialRevisar
        /// <summary>
        /// Verifica que nuestro documento actual esté dentro del lugar que le corresponde en el
        /// historial de documentos
        /// </summary>
        /// <param name="entrada">El servicio para generar ligas, contiene los datos 
        /// del documento.</param>
        public void revisaHistorial(IUSNavigationService entrada)
        {
            Boolean encontrado = false;
            if (Historia == null)
            {
                historia = new Historial();
            }
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
#if STAND_ALONE
            Nullable<Boolean> respuesta = true;// dialogoImpresion.ShowDialog();
#else
            Nullable<Boolean> respuesta = dialogoImpresion.ShowDialog();
#endif
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
                    dialogoImpresion.PrintDocument(pgn, "Impresión de tesis");
                }
                catch (Exception exc)
                {
                    MessageBox.Show(Mensajes.MENSAJE_IMPRESORA, Mensajes.TITULO_ARCHIVO_ABIERTO,
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
                EscondeVistaPrel();
            }
        }

        #endregion
        #region pintaTesis
#if STAND_ALONE
        internal void PintaTesis(List<BusquedaPalabraTO> palabras)
#else
        internal void PintaTesis(BusquedaPalabraTO[] palabras) 
#endif
        {

            List<String> listaPalabras = new List<string>();
            List<String> frases = new List<string>();
            foreach (BusquedaPalabraTO item in palabras)
            {
                BusquedaPalabraTO palabrejasTO = new BusquedaPalabraTO();
                if (item.Jurisprudencia == Constants.BUSQUEDA_PALABRA_ALMACENADA)
                {
#if STAND_ALONE
                    FachadaBusquedaTradicional fachada = new FachadaBusquedaTradicional();
                    List<BusquedaAlmacenadaTO> busquedas = fachada.getBusquedasAlmacenadas(SeguridadUsuariosTO.UsuarioActual.Usuario);
#else
            FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
                    BusquedaAlmacenadaTO[] busquedas = fachada.getBusquedasAlmacenadas(SeguridadUsuariosTO.UsuarioActual.Usuario);
#endif
                    fachada.Close();
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

                List<String> anadeLista = FlowDocumentHighlight.obtenPalabras(palabrejasTO);
                foreach (String anadir in anadeLista)
                {
                    if ((!anadir.ToLower().Equals("n")) 
                        && (!anadir.ToLower().Equals("y"))
                        && (!anadir.ToLower().Equals("o"))
                        && (!anadir.Trim().Equals(Constants.SEPARADOR_FRASES.Trim())
                        && (!anadir.Trim().Equals(Constants.CADENA_VACIA))))
                    {
                        listaPalabras.Add(anadir);
                    }
                }
                anadeLista = FlowDocumentHighlight.obtenFrases(palabrejasTO);
                foreach (String anadir in anadeLista)
                {
                    frases.Add(anadir);
                }
                List<int> campos = new List<int>();
                if (palabrejasTO.Campos != null)
                {
                    campos = new List<int>(palabrejasTO.Campos);
                }
                if (campos.Contains(Constants.BUSQUEDA_PALABRA_CAMPO_RUBRO))
                {
                    documentoRubro = FlowDocumentHighlight.imprimeToken(documentoRubro,listaPalabras, Brushes.Red);
                    documentoRubro = FlowDocumentHighlight.imprimeToken(documentoRubro, frases, Brushes.DarkGreen);
                }
                if (campos.Contains(Constants.BUSQUEDA_PALABRA_CAMPO_PRECE))
                {
                    documentoPrecedentes = FlowDocumentHighlight.imprimeToken(documentoPrecedentes, listaPalabras, Brushes.Red);
                    documentoPrecedentes = FlowDocumentHighlight.imprimeToken(documentoPrecedentes, frases, Brushes.DarkGreen);
                }
                if (campos.Contains(Constants.BUSQUEDA_PALABRA_CAMPO_TEXTO))
                {
                    contenidoTexto.Document = FlowDocumentHighlight.imprimeToken(contenidoTexto.Document, listaPalabras, Brushes.Red);
                    contenidoTexto.Document = FlowDocumentHighlight.imprimeToken(contenidoTexto.Document, frases, Brushes.DarkGreen);
                }
            }
        }
        #endregion
        #region portapapeles
        private void PortaPapeles_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            mostrarVP = false;
            DocumentoTesis.ImprimirLocalizacion = true;
            //Imprimir_MouseButtonDown(sender, e);
            DocumentoTesis documento = null;
            //impresion.Document = documento.Documento;
            //FlowDocument documentoOriginal = this.contenidoTexto.Document;
            MessageBoxResult resultado = MessageBoxResult.No;
            if (marcados!=null && marcados.Count > 0)
            {
                resultado = MessageBox.Show(Mensajes.MENSAJE_MARCADOS_ACCION, Mensajes.TITULO_MARCADOS_ACCION,
                    MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
            }
            if (resultado.Equals(MessageBoxResult.No))
            {
                documento = new DocumentoTesis(DocumentoActual);
            }
            else if (resultado.Equals(MessageBoxResult.Yes))
            {
                documento = new DocumentoTesis(marcados, false);
            }
            else
            {
                mostrarVP = true;
                return;
            }
            FlowDocument documentoImprimir = documento.DocumentoCopia;
            this.RtbCopyPaste.Document = documentoImprimir;
            this.RtbCopyPaste.SelectAll();
            RtbCopyPaste.Copy();
            //MostrarDatos(DocumentoActual);
            //this.contenidoTexto.Document = documentoOriginal;
            MuestraVistaPrel();
            EscondeVistaPrel();
            mostrarVP = true;
            MessageBox.Show(Mensajes.MENSAJE_ENVIADO_PORTAPAPELES,
                Mensajes.TITULO_ENVIADO_PORTAPAPELES, MessageBoxButton.OK,
                MessageBoxImage.Information);
        }
        #endregion
        #region buscar
        /// <summary>
        ///     Genera la búsqueda por palabra desde el botón de buscar
        /// </summary>
        private void Buscar_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            String validar = Validadores.BusquedaPalabraDocumento(this.textoAbuscar);
            if (!validar.Equals(Constants.CADENA_VACIA))
            {
                return;
            }
            String busquedaTexto = this.textoAbuscar.Text;
            String[] respuesta = BuscarPalabra(busquedaTexto);
            if (respuesta != null)
            {
                MessageBox.Show(respuesta[0], respuesta[1],
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        private String[] BuscarPalabra(String busquedaTexto)
        {
            String [] respuesta = null;
            TextPointer inicio = this.contenidoTexto.Selection.Start;
            TextPointer final = this.contenidoTexto.Selection.End;
            TextPointer lugarActual = null;
            if (inicio.CompareTo(final) == 0)
            {
                lugarActual = inicio;// this.contenidoTexto.Document.ContentStart;
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
                    respuesta=new String[2];
                    respuesta[0] = Mensajes.MENSAJE_NO_HAY_MAS_COINCIDENCIAS;
                    respuesta[1] = Mensajes.TITULO_NO_HAY_MAS_COINCIDENCIAS;
                    return respuesta;
                }
                else
                {
                    respuesta = new String[2];
                    respuesta[0] = Mensajes.MENSAJE_NO_HAY_COINCIDENCIAS;
                    respuesta[1] = Mensajes.TITULO_NO_HAY_COINCIDENCIAS;
                    return respuesta;
                }
                //TextRange rango = new TextRange(contenidoTexto.Document.ContentStart, contenidoTexto.Document.ContentStart);
                //rango.Select(contenidoTexto.Document.ContentStart, contenidoTexto.Document.ContentStart);
            }
            else
            {
                EncontradaFrase = true;
                inicio = this.contenidoTexto.Selection.Start;
                final = this.contenidoTexto.Selection.End;
                this.contenidoTexto.Selection.Select(inicio, final);
                this.contenidoTexto.Focus();
                return respuesta;
            }
        }
        #endregion
        #region guardar
        private void Guardar_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            mostrarVP = false;
            if (!BrowserInteropHelper.IsBrowserHosted)
            {
                Microsoft.Win32.SaveFileDialog guardaEn = new Microsoft.Win32.SaveFileDialog();
                guardaEn.DefaultExt = ".rtf";
                guardaEn.Title = "Guardar una tesis";
                guardaEn.Filter = "Archivos de Texto Enriquecido|*.rtf";
                guardaEn.AddExtension = true;
                EscondeVistaPrel();
                if ((bool)guardaEn.ShowDialog())
                {
                    DocumentoTesis.ImprimirLocalizacion = true;
                    Imprimir_MouseButtonDown(sender, e);
                    FlowDocument documentoImprimir = documentoCopia as FlowDocument;
                    impresion.Document = null;
                    EscondeVistaPrel();
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
                FlowDocument documentoImprimir = impresion.Document as FlowDocument;
                impresion.Document = null;
                this.contenidoTexto.Document = documentoImprimir;
                System.IO.IsolatedStorage.IsolatedStorageFileStream archivo = new System.IO.IsolatedStorage.
                    IsolatedStorageFileStream("texto.rtf", System.IO.FileMode.Create);
                this.contenidoTexto.SelectAll();
                this.contenidoTexto.Selection.Save(archivo, System.Windows.DataFormats.Text);
                archivo.Flush();
                archivo.Close();
                MostrarDatos(DocumentoActual);
                MessageBox.Show("El archivo fue guardado como: " + archivo.Name);
            }
            EscondeVistaPrel();
            mostrarVP = true;
        }
        #endregion
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
                verFontMayor = false;
            }
        }
        #endregion
        #region marcarTodo
        private void MarcarTodo_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            int faltantes = 50 - marcados.Count;
            if (faltantes > ArregloTesis.Items.Count)
            {
                // El número de documentos en el arreglo cabe en los faltantes
                foreach (TesisSimplificadaTO item in ArregloTesis.Items)
                {
                    marcados.Add(Int32.Parse(item.Ius));
                }
                MessageBox.Show(Mensajes.MENSAJE_TODOS_PORTAPAPELES + ArregloTesis.Items.Count +
                    Mensajes.MENSAJE_TODOS_PROTAPAPELES2, Mensajes.TITULO_TODOS_PORTAPAPELES,
                    MessageBoxButton.OK, MessageBoxImage.Information);
                Marcar_MouseEnter(sender, null);
            }
            else
            {
                if (marcados.Count < 50)
                {
                    this.ventanaRangos.Visibility = Visibility.Visible;
                    this.ventanaRangos.InicioRango = 1;
                    this.ventanaRangos.FinRango = 50;
                    this.ventanaRangos.DiferenciaRangos = faltantes-1;
                    this.ventanaRangos.StrMensaje = Mensajes.MENSAJE_RANGO_MARCAR + faltantes;
                    this.ventanaRangos.Contenedor = this;
                    this.ventanaRangos.RegistroFinal = this.ArregloTesis.Items.Count;
                }
                else
                {
                    MessageBox.Show(Mensajes.MENSAJE_RANGO_YA_NO_HAY, Mensajes.TITULO_RANGO,
                        MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
            }
        }
        #endregion
        #region desmarcar
        private void Desmarcar_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            MessageBoxResult resultado=MessageBoxResult.Yes;
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
                bitmap.UriSource = new Uri("/General;component/images/MARCAR1.png", UriKind.Relative);
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
            if (!marcados.Contains(Int32.Parse(this.DocumentoActual.Ius)))
            {
                Image imagenNueva = new Image();
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri("/General;component/images/MARCAR2.png", UriKind.Relative);
                bitmap.EndInit();
                this.Marcar.Source = bitmap;
                this.Marcar.ToolTip = Mensajes.TOOLTIP_SIN_MARCAR;
            }
            else
            {
                Image imagenNueva = new Image();
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri("/General;component/images/PALOMA1.png", UriKind.Relative);
                bitmap.EndInit();
                this.Marcar.Source = bitmap;
                this.Marcar.ToolTip = Mensajes.TOOLTIP_MARCADO;
            }
        }

        private void Marcar_MouseLeave(object sender, MouseEventArgs e)
        {
            if (!marcados.Contains(Int32.Parse(this.DocumentoActual.Ius)))
            {
                //Image imagenNueva = new Image();
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri("/General;component/images/MARCAR1.png", UriKind.Relative);
                bitmap.EndInit();
                this.Marcar.Source = bitmap;
                this.Marcar.ToolTip = Mensajes.TOOLTIP_SIN_MARCAR;
            }
            else
            {
                //Image imagenNueva = new Image();
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri("/General;component/images/PALOMA1.png", UriKind.Relative);
                bitmap.EndInit();
                this.Marcar.Source = bitmap;
                this.Marcar.ToolTip = Mensajes.TOOLTIP_MARCADO;
            }
        }

        private void Marcar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!marcados.Contains(Int32.Parse(DocumentoActual.Ius)))
            {
                if (marcados.Count <= 50)
                {
                    marcados.Add(Int32.Parse(DocumentoActual.Ius));
                    Globals.Marcados.Add(Int32.Parse(DocumentoActual.Ius));
                    Image imagenNueva = new Image();
                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri("/General;component/images/PALOMA1.png", UriKind.Relative);
                    bitmap.EndInit();
                    this.Marcar.Source = bitmap;
                    this.Marcar.ToolTip = Mensajes.TOOLTIP_MARCADO;
                }
                else
                {
                    MessageBox.Show("El documento no se puede marcar ya que hay 50 documentos guardados", "Limite de marcados alcanzado", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                marcados.Remove(Int32.Parse(DocumentoActual.Ius));
                Globals.Marcados.Remove(Int32.Parse(DocumentoActual.Ius));
                Image imagenNueva = new Image();
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri("/General;component/images/MARCAR1.png", UriKind.Relative);
                bitmap.EndInit();
                this.Marcar.Source = bitmap;
                this.Marcar.ToolTip = Mensajes.TOOLTIP_SIN_MARCAR;
            }
        }
        #endregion
        #region fontmenor
        private void FontMenor_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            FontMayor.Visibility = Visibility.Visible;
            verFontMayor = true;
           // this.contenidoTexto.SelectAll();
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
        #endregion
        private void textoAbuscar_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.contenidoTexto.Selection.Select(contenidoTexto.Document.ContentStart, contenidoTexto.Document.ContentStart);
            EncontradaFrase = false;
        }
        public void ActualizaRangoMarcado(int inicio, int final)
        {
            for (int contador = inicio; contador <= final; contador++)
            {
                TesisSimplificadaTO agregado = (TesisSimplificadaTO)this.ArregloTesis.Items.GetItemAt(contador -1);
                marcados.Add(Int32.Parse(agregado.Ius));
            }
            if (marcados.Contains(Int32.Parse(DocumentoActual.Ius)))
            {
                marcados.Add(Int32.Parse(DocumentoActual.Ius));
                Image imagenNueva = new Image();
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri("/General;component/images/PALOMA1.png", UriKind.Relative);
                bitmap.EndInit();
                this.Marcar.Source = bitmap;
            }
        }

        private void BtnConcordancia_Click(object sender, RoutedEventArgs e)
        {
            FlowDocument documentoObservaciones = new FlowDocument();
            foreach (OtrosTextosTO item in observaciones)
            {
                if (Int32.Parse(item.TipoNota) == 3)
                {
                    Paragraph parrafoObservacion = new Paragraph(new Run(item.Textos));
                    parrafoObservacion.TextAlignment = TextAlignment.Justify;
                    documentoObservaciones.Blocks.Add(parrafoObservacion);
                }
            }
            ventanaEmergente.contenido.Document = documentoObservaciones;
            ventanaEmergente.titulo.Text = "Concordancia";
            ventanaEmergente.Visibility = Visibility.Visible;

        }

        private void contenidoTexto_SelectionChanged(object sender, RoutedEventArgs e)
        {
        }

        private void regNum_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                this.IrBoton_Click(sender, null);
            }
            if ((e.Key == Key.Decimal)||(e.Key==Key.OemPeriod))
            {
                e.Handled = true;
            }
        }

        private void BtnTache_Click(object sender, RoutedEventArgs e)
        {
            EscondeVistaPrel();
        }
        #region vistaPreliminar
        /// <summary>
        ///     Esconde los elementos que tienen que ver con la vista preliminar de
        ///     impresión y muestra los que no.
        /// </summary>
        private void EscondeVistaPrel()
        {
            if(verVentanaRangos)
            ventanaRangos.Visibility = Visibility.Visible;
            if(verVentanaEmergente)
            ventanaEmergente.Visibility = Visibility.Visible;
            if(verVentanaListaEjecutorias)
            ventanaListaEjecutorias.Visibility = Visibility.Visible;
            if(verVentanaListaVotos)
            ventanaListaVotos.Visibility = Visibility.Visible;
            if(verVentanaListadoLeyes)
            ventanaListadoLeyes.Visibility = Visibility.Visible;
            if(verVentanitaLeyes)
            ventanitaLeyes.Visibility = Visibility.Visible;
            if(verVentanaAnexos)
            ventanaAnexos.Visibility = Visibility.Visible;
            if(verVentanaHistorial)
            ventanaHistorial.Visibility = Visibility.Visible;
            BtnTemas.Visibility = verBtnTemas ? Visibility.Visible : Visibility.Hidden;
            //BtnImprimeDirecto.Visibility = Visibility.Visible;
            if ((ArregloTesis != null) && (ArregloTesis.Items.Count > 1))
            {
                this.anteriorLista.Visibility = Visibility.Visible;
                this.inicioLista.Visibility = Visibility.Visible;
                this.siguienteLista.Visibility = Visibility.Visible;
                this.ultimoLista.Visibility = Visibility.Visible;
                IrALabel.Visibility = Visibility.Visible;
                //IrBoton.Visibility = Visibility.Visible;
                RegNum.Visibility = Visibility.Visible;
                regNum.Visibility = Visibility.Visible;
                Marcar.Visibility = Visibility.Visible;
                MarcarTodo.Visibility = Visibility.Visible;
                Desmarcar.Visibility = Visibility.Visible;
            }
                historial.Visibility = Visibility.Visible;
            if (!DocumentoActual.IdGenealogia.Equals("0"))
            {
                genealogia.Visibility = Visibility.Visible;
            }
#if STAND_ALONE
            if (documentosEjecutoria.Count > 0)
#else
            if (documentosEjecutoria.Length > 0)
#endif
            {
                ejecutoria.Visibility = Visibility.Visible;
            }
#if STAND_ALONE
            if (documentosVotos.Count > 0)
#else
            if (documentosVotos.Length > 0)
#endif
            {
                voto.Visibility = Visibility.Visible;
            }
            foreach (OtrosTextosTO item in observaciones)
            {
                if (item.TipoNota.Equals("2"))
                {
                    observacionesBot.Visibility = Visibility.Visible;
                }
                else if (item.TipoNota.Equals("3"))
                {
                    BtnConcordancia.Visibility = Visibility.Visible;
                }
            }
            PortaPapeles.Visibility = Visibility.Visible;
            Imprimir.Visibility = Visibility.Visible;
            imprimePapel.Visibility = Visibility.Hidden;
            BtnTache.Visibility = Visibility.Hidden;
            if(verFontMayor) FontMayor.Visibility = Visibility.Visible;
            if(verFontMenor) FontMenor.Visibility = Visibility.Visible;
            Salir.Visibility = Visibility.Visible;
            if (verMaterias)
            {
                AnuncioMaterias.Visibility = Visibility.Visible;
                Materias.Visibility = Visibility.Visible;
            }
            Guardar.Visibility = !BrowserInteropHelper.IsBrowserHosted ? Visibility.Visible : Visibility.Hidden;
            TesisLabel.Visibility = Visibility.Visible;
            TesisTesisLabel.Visibility = Visibility.Visible;
            fuenteLabel.Visibility = Visibility.Visible;
            EpocaLabel.Visibility = Visibility.Visible;
            IUSLabel.Visibility = Visibility.Visible;
            SalaLabel.Visibility = Visibility.Visible;
            FechaLabel.Visibility = Visibility.Visible;
            PaginaLabel.Visibility = Visibility.Visible;
            jurisLabel.Visibility = Visibility.Visible;
            if (contradiccionExiste)
            {
                contradiccion.Visibility = Visibility.Visible;
            }
            LblPalabraBuscar.Visibility = Visibility.Visible;
            textoAbuscar.Visibility = Visibility.Visible;
            Buscar.Visibility = Visibility.Visible;
            Expresion.Visibility = Visibility.Visible;
            tabControl1.Visibility = Visibility.Visible;
            impresion.Visibility = Visibility.Hidden;
        }
        /// <summary>
        ///     Muestra los elementos que tienen que ver con la vista preliminar de
        ///     impresión y esconde los que no
        /// </summary>
        private void MuestraVistaPrel()
        {
            verVentanaRangos=(ventanaRangos.Visibility.Equals(Visibility.Visible));
            verVentanaEmergente= (ventanaEmergente.Visibility==Visibility.Visible);
            verVentanaListaEjecutorias= (ventanaListaEjecutorias.Visibility==Visibility.Visible);
            verVentanaListaVotos=(ventanaListaVotos.Visibility==Visibility.Visible);
            verVentanaListadoLeyes=ventanaListadoLeyes.Visibility==Visibility.Visible;
            verVentanitaLeyes=ventanitaLeyes.Visibility==Visibility.Visible;
            verVentanaAnexos=ventanaAnexos.Visibility==Visibility.Visible;
            verVentanaHistorial = ventanaHistorial.Visibility == Visibility.Visible;
            verBtnTemas = BtnTemas.Visibility == Visibility.Visible;
            BtnTemas.Visibility = Visibility.Hidden;
            BtnImprimeDirecto.Visibility = Visibility.Hidden;
            ventanaRangos.Visibility=Visibility.Hidden;
            ventanaEmergente.Visibility = Visibility.Hidden;;
            ventanaListaEjecutorias.Visibility = Visibility.Hidden;
            ventanaListaVotos.Visibility = Visibility.Hidden;
            ventanaListadoLeyes.Visibility = Visibility.Hidden;
            ventanitaLeyes.Visibility = Visibility.Hidden;
            ventanaAnexos.Visibility = Visibility.Hidden;
            ventanaHistorial.Visibility = Visibility.Hidden;
            impresion.Visibility = Visibility.Visible;
            tabControl1.Visibility = Visibility.Hidden;
            this.anteriorLista.Visibility = Visibility.Hidden;
            this.inicioLista.Visibility = Visibility.Hidden;
            this.siguienteLista.Visibility = Visibility.Hidden;
            this.ultimoLista.Visibility = Visibility.Hidden;
            IrALabel.Visibility = Visibility.Hidden;
            //IrBoton.Visibility = Visibility.Hidden;
            RegNum.Visibility = Visibility.Hidden;
            regNum.Visibility = Visibility.Hidden;
            historial.Visibility = Visibility.Hidden;
            genealogia.Visibility = Visibility.Hidden;
            ejecutoria.Visibility = Visibility.Hidden;
            voto.Visibility = Visibility.Hidden;
            observacionesBot.Visibility = Visibility.Hidden;
            BtnConcordancia.Visibility = Visibility.Hidden;
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
            AnuncioMaterias.Visibility = Visibility.Hidden;
            Materias.Visibility = Visibility.Hidden;
            Guardar.Visibility = Visibility.Hidden;
            TesisLabel.Visibility = Visibility.Hidden;
            TesisTesisLabel.Visibility = Visibility.Hidden;
            fuenteLabel.Visibility = Visibility.Hidden;
            EpocaLabel.Visibility = Visibility.Hidden;
            IUSLabel.Visibility = Visibility.Hidden;
            SalaLabel.Visibility = Visibility.Hidden;
            FechaLabel.Visibility = Visibility.Hidden;
            PaginaLabel.Visibility = Visibility.Hidden;
            jurisLabel.Visibility = Visibility.Hidden;
            contradiccion.Visibility = Visibility.Hidden;
            LblPalabraBuscar.Visibility = Visibility.Hidden;
            textoAbuscar.Visibility = Visibility.Hidden;
            Buscar.Visibility = Visibility.Hidden;
            Expresion.Visibility = Visibility.Hidden;
        }
        #endregion

        private void textoAbuscar_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Buscar_MouseLeftButtonDown(sender, e);
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Mensajes.TOPICO_AYUDA = "visualizacion_de_las_tesis.htm";
            ventanaHistorial.Visibility = Visibility.Hidden;
            contenidoTexto.FontSize = CalculosPropiedadesGlobales.FontSize;
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
            RichTextBox rtb =sender as RichTextBox;
            TbxCopiar.Text=contenidoTexto.Selection.Text;
            e.CancelCommand();
            TbxCopiar.SelectAll();
            TbxCopiar.Copy();
        }

        private void BtnImprimeDirecto_Click(object sender, RoutedEventArgs e)
        {
            //MessageBoxResult res = MessageBox.Show("¿Imprimir la tesis completa?", "Tipo de impresión",
            //    MessageBoxButton.YesNo, MessageBoxImage.Question);
            //DocumentoTesis.ImprimirLocalizacion = res == MessageBoxResult.Yes;
            ventanaImprimirCompleta.Visibility = Visibility.Visible;
        }

        public void ImprimeTesis()
        {
            DocumentoTesis.ImprimirLocalizacion = ImprimirCompleto;
            impresion.Document = null;
            if (marcados == null) marcados = new HashSet<int>();
            Imprimir_MouseButtonDown(null, null);
            if ((marcados.Count > 0) && (impresion.Document == null)) return;
            EscondeVistaPrel();
            imprimePapel_MouseLeftButtonDown(null, null);

        }
        private void CerrarTemas_Click(object sender, RoutedEventArgs e)
        {
            FrmTemas.Visibility = Visibility.Collapsed;
            BtnTemas.Visibility = Visibility.Visible;
        }

        private void BtnTemas_Click(object sender, RoutedEventArgs e)
        {
#if STAND_ALONE
            FachadaBusquedaTradicional fac = new FachadaBusquedaTradicional();
            List<TreeNodeDataTO> TemasRelacionados = fac.getTemasRelacionados(DocumentoActual.Ius);
#else
            FachadaBusquedaTradicionalClient fac = new FachadaBusquedaTradicionalClient();
            TreeNodeDataTO[] TemasRelacionados = fac.getTemasRelacionados(DocumentoActual.Ius);
#endif
            fac.Close();            
            LbxTemas.SelectedIndex = 0;
#if STAND_ALONE
            ventanaTemas.ActualizaListado(TemasRelacionados);
#else
            List<TreeNodeDataTO> litaNodos = new List<TreeNodeDataTO>(TemasRelacionados);
            ventanaTemas.ActualizaListado(litaNodos);
#endif
            ventanaTemas.Visibility = Visibility.Visible;
        }

        private void BtnMostrarTemas_Click(object sender, RoutedEventArgs e)
        {
            ComboBoxItem original = (ComboBoxItem)LbxTemas.SelectedItem;
            if (original == null)
            {
                MessageBox.Show("Debe seleccionar algún tema", "Tema no seleccionado", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            BusquedaTO busqueda = ObtenBusquedaTO((TreeNodeDataTO)original.Tag);
            tablaResultado paginaResultados = new tablaResultado(busqueda);
            paginaResultados.Back = this;
            if (paginaResultados.tablaResultados.Items.Count == 0)
            {
                MessageBox.Show(Mensajes.MENSAJE_BUSQUEDA_TEMATICA_VACIA, Mensajes.TITULO_BUSQUEDA_VACIA,
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                this.NavigationService.Navigate(paginaResultados);
            }
        }

        public BusquedaTO ObtenBusquedaTO(TreeNodeDataTO nodo)
        {
            BusquedaTO busqueda = new BusquedaTO();
            Boolean[][] epocas = new Boolean[Constants.EPOCAS_LARGO][];
            for (int i = 0; i < Constants.EPOCAS_LARGO; i++)
            {
                epocas[i] = new Boolean[Constants.EPOCAS_ANCHO];
            }
            busqueda.Epocas = epocas;
            epocas = new Boolean[Constants.ACUERDOS_ANCHO][];
            for (int i = 0; i < Constants.ACUERDOS_ANCHO; i++)
            {
                epocas[i] = new Boolean[Constants.ACUERDOS_LARGO];
            }
            busqueda.Acuerdos = epocas;
            epocas = new Boolean[Constants.APENDICES_ANCHO][];
            for (int i = 0; i < Constants.APENDICES_ANCHO; i++)
            {
                epocas[i] = new Boolean[Constants.APENDICES_LARGO];
            }
            busqueda.Apendices = epocas;
            busqueda.TipoBusqueda = Constants.BUSQUEDA_TESIS_TEMATICA;
#if STAND_ALONE
            busqueda.Clasificacion = new List<ClassificacionTO>();
            busqueda.Clasificacion.Add(new ClassificacionTO());
            busqueda.Clasificacion[0].DescTipo = nodo.Href;//Href, originalmente
            busqueda.Clasificacion[0].IdTipo = Int32.Parse(nodo.Id);
#else
            busqueda.clasificacion = new ClassificacionTO[1];
            busqueda.clasificacion[0] = new ClassificacionTO();
            busqueda.clasificacion[0].DescTipo = nodo.Href;//Href, originalmente
            busqueda.clasificacion[0].IdTipo = Int32.Parse(nodo.Id);
#endif
            if (nodo.Label.Contains("["))
            {
                busqueda.OrdenarPor = ((String)nodo.Label).Substring(0, ((String)nodo.Label).IndexOf('['));
            }
            else
            {
                busqueda.OrdenarPor = (String)nodo.Label;
            }
            return busqueda;
        }

        private void TextoHablado_Click(object sender, RoutedEventArgs e)
        {
            //SpeechSynthesizer synth = new SpeechSynthesizer();
            //String textoLeer = DocumentoActual.Texto;
            ////synth.Voice.Culture.ThreeLetterISOLanguageName="MEX";
            //synth.Speak(textoLeer);
        }

        private void RegNum_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Clipboard.Clear();
            try
            {
                //Clipboard.SetText((String)IUSLabel.Content, TextDataFormat.UnicodeText);
                Paragraph par = new Paragraph(new Run((String)IUSLabel.Content));
                par.FontFamily = new FontFamily(Constants.FONT_USAR);
                par.FontSize = 15;
                this.RtbCopyPaste.Document=new FlowDocument(par);
                this.RtbCopyPaste.SelectAll();
                this.RtbCopyPaste.Copy();
                MessageBox.Show(Mensajes.MENSAJE_IUS_ENVIADO_PORTAPAPELES, Mensajes.TITULO_ENVIADO_PORTAPAPELES,
                     MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception exc)
            {
                MessageBox.Show(Mensajes.MENSAJE_ERROR_PORTAPAPELES, Mensajes.TITULO_ERROR,
                    MessageBoxButton.OK, MessageBoxImage.Error);

            }
        }

        private void BtnNotaGenerica_Click(object sender, RoutedEventArgs e)
        {
            FlowDocument documentoObservaciones = new FlowDocument();

            Paragraph parrafoObservacion = new Paragraph(new Run(Mensajes.NOTA_GENERICA_1));
            parrafoObservacion.TextAlignment = TextAlignment.Justify;
            documentoObservaciones.Blocks.Add(parrafoObservacion);
             parrafoObservacion = new Paragraph(new Run(Mensajes.NOTA_GENERICA_2));
            parrafoObservacion.TextAlignment = TextAlignment.Justify;
            documentoObservaciones.Blocks.Add(parrafoObservacion);
             parrafoObservacion = new Paragraph(new Run(Mensajes.NOTA_GENERICA_3));
            parrafoObservacion.TextAlignment = TextAlignment.Justify;
            documentoObservaciones.Blocks.Add(parrafoObservacion);
             parrafoObservacion = new Paragraph(new Run(Mensajes.NOTA_GENERICA_4));
            parrafoObservacion.TextAlignment = TextAlignment.Justify;
            documentoObservaciones.Blocks.Add(parrafoObservacion);
             parrafoObservacion = new Paragraph(new Run(Mensajes.NOTA_GENERICA_5));
            parrafoObservacion.TextAlignment = TextAlignment.Justify;
            documentoObservaciones.Blocks.Add(parrafoObservacion);
            ventanaEmergente.contenido.Document = documentoObservaciones;
            ventanaEmergente.titulo.Text = "Notas genéricas";
            ventanaEmergente.Visibility = Visibility.Visible;
        }
    }
}