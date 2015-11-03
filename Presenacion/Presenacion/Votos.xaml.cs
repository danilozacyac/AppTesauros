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
    public partial class VotosPagina : Page
    {
        private Xceed.Wpf.DataGrid.DataGridControl ArregloVotos { get; set; }
        private String genealogiaId { get; set; }
#if STAND_ALONE
        private List<RelDocumentoTesisTO> documentosTesis { get; set; }
        private List<RelVotoEjecutoriaTO> documentosEjecutorias { get; set; }
        private List<VotosPartesTO> partes { get; set; }
#else
        private RelDocumentoTesisTO[] documentosTesis { get; set; }
        private RelVotoEjecutoriaTO[] documentosEjecutorias { get; set; }
        private VotosPartesTO[] partes { get; set; }
#endif
        private int numeroParte { get; set; }
        private int posicion = 0;
        private BusquedaTO Busqueda { get; set; }
        private VotoSimplificadoTO DocumentoActual;
        public Historial Historia { get { return this.getHistorial(); } set { this.setHistorial(value); } }
        private Historial historia;
        private Object ParametroConstruccion { get; set; }
        public Page Back;
        public static HashSet<int> marcados;
        private FlowDocument DocumentoCopia { get; set; }
        protected bool EncontradaFrase { get; set; }
        protected bool ExistenTablas;
        protected bool verVentanaListadoEjecutorias;
        protected bool verVentanaListadoTesis;
        protected bool verVentanaRangos;
        protected bool verVentanaHistorial;
        protected bool verVentanaListadoAnexos;
        protected bool verFontMayor;
        protected bool verFontMenor;

        public VotosPagina()
        {
            InitializeComponent();
            ParametroConstruccion = this;
            this.contenidoTexto.SetValue(RichTextBox.FontSizeProperty, CalculosPropiedadesGlobales.FontSize);
            verFontMenor = true;
            verFontMayor = true;
        }
        public VotosPagina(int id)
        {
            InitializeComponent();
            ParametroConstruccion = this;
            marcados = new HashSet<int>();
#if STAND_ALONE
            FachadaBusquedaTradicional fachada = new FachadaBusquedaTradicional();
#else
            FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
#endif
            VotoSimplificadoTO documentoActual = new VotoSimplificadoTO( fachada.getVotosPorId(id));
            MostrarDatos(documentoActual);
            fachada.Close();
            this.contenidoTexto.SetValue(RichTextBox.FontSizeProperty, CalculosPropiedadesGlobales.FontSize);
            verFontMenor = true;
            verFontMayor = true;
            Expresion.Visibility = Visibility.Hidden;
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
            this.Desmarcar.Visibility = Visibility.Hidden;
            this.MarcarTodo.Visibility = Visibility.Hidden;
        }
        /// <summary>
        /// Constructor en el que se obtienen los datos de una tabla determinada.
        /// </summary>
        /// <param name="records"></param>
        public VotosPagina(Xceed.Wpf.DataGrid.DataGridControl records, BusquedaTO parametros)
        {
            InitializeComponent();
            Busqueda = parametros;
            CalculosPropiedadesGlobales.FontSize = Constants.FONTSIZE;
            this.contenidoTexto.SetValue(RichTextBox.FontSizeProperty, CalculosPropiedadesGlobales.FontSize);
            verFontMenor = true;
            verFontMayor = true;
            marcados = new HashSet<int>();
            ParametroConstruccion = this;
            VotoSimplificadoTO votosMostrar = (VotoSimplificadoTO)records.SelectedItem;
#if STAND_ALONE
            FachadaBusquedaTradicional fachada = new FachadaBusquedaTradicional();
#else
            FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
#endif
            VotoSimplificadoTO documentoActual = new VotoSimplificadoTO(fachada.getVotosPorId(Int32.Parse(votosMostrar.Id)));
            this.ArregloVotos = records;
            posicion = records.SelectedIndex;
            if (parametros != null)
            {
                Expresion.Content = CalculosGlobales.Expresion(parametros);
            }
            else
            {
                List<int> registros = new List<int>();
                Expresion.Content = CalculosGlobales.Expresion(registros);
            }
            MostrarDatos(documentoActual);
            this.Historia.RootElement = records;
            fachada.Close();
            if (this.ArregloVotos.Items.Count < 2)
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
        }
        /// <summary>
        /// Muestra los datos de una tesis determinada.
        /// </summary>
        private void MostrarDatos(VotoSimplificadoTO documentoActual)
        {
            DocumentoActual = documentoActual;
            this.PaginaLabel.Content = "Página: " + documentoActual.Pagina;
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
            documentosEjecutorias = fachada.getEjecutoriaPorVoto(documentoActual.Id);
#else
            FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
            documentosEjecutorias = fachada.getEjecutoriasPorVoto(documentoActual.Id);
#endif
            documentosTesis = fachada.getTesisPorVoto(documentoActual.Id);
            partes = fachada.getVotosPartesPorId(Int32.Parse(documentoActual.Id));
            Paragraph textoParrafo;
#if STAND_ALONE
            if (documentosTesis.Count == 0)
#else
            if (documentosTesis.Length == 0)
#endif
            {
                textoParrafo = ObtenLigas(documentoActual.Rubro + "\n\n" + partes[0].TxtParte, documentoActual.Id, 1);
                tesis.Visibility = Visibility.Hidden;
            }
            else
            {
                textoParrafo = ObtenLigas(partes[0].TxtParte, documentoActual.Id, 1);
                tesis.Visibility = Visibility.Visible;
            }
#if STAND_ALONE
            if (documentosEjecutorias.Count == 0)
#else
            if (documentosEjecutorias.Length == 0)
#endif
            {
                ejecutoria.Visibility = Visibility.Hidden;
            }
            else
            {
                ejecutoria.Visibility = Visibility.Visible;
            }
            textoParrafo.TextAlignment = TextAlignment.Justify;
            textoParrafo.FontWeight = FontWeights.Normal;
            documentoRubro.Blocks.Add(textoParrafo);
            numeroParte = 0;
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
#else
                if ((!encontrado) && (numeroParte < (partes.Length - 1)))
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
            if (ArregloVotos != null)
            {
                List<VotoSimplificadoTO> lista = (List<VotoSimplificadoTO>)(ArregloVotos.ItemsSource);
                int posicionReal = posicion + 1;
                RegNum.Content = "" + posicionReal + " / " + lista.Count;
                DocumentoActual = documentoActual;
                tabControl1.Visibility = Visibility.Visible;
                //Si es un solo registro en los arreglos esconder los de navegacion
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
            entradaHistoria.TipoVentana = IUSNavigationService.VOTO;
            hist.Lista.Add(entradaHistoria);
            this.Historia = hist;
            textoAbuscar.Text = Constants.CADENA_VACIA;
            impresion.Visibility = Visibility.Hidden;
            ventanaHistorial.Visibility = Visibility.Hidden;
            ventanaListadoAnexos.Visibility = Visibility.Hidden;
            ventanaListadoEjecutorias.Visibility = Visibility.Hidden;
            ventanaListadoTesis.Visibility = Visibility.Hidden;
            ventanaRangos.Visibility = Visibility.Hidden;
            tabControl1.Visibility = Visibility.Visible;
            imprimePapel.Visibility = Visibility.Hidden;
            textoAbuscar.Visibility = Visibility.Visible;
            Buscar.Visibility = Visibility.Visible;
            Expresion.Visibility = Visibility.Visible;
            if (marcados.Contains(Int32.Parse(documentoActual.Id)))
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
            
        }

        private void historial_MouseEnter(object sender, MouseEventArgs e)
        {
            Image imagenNueva = new Image();
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri("/General;component/images/HISTORIAL2.png", UriKind.Relative);
            bitmap.EndInit();
            this.historial.Source = bitmap;
        }

        private void historial_MouseLeave(object sender, MouseEventArgs e)
        {
            Image imagenNueva = new Image();
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri("/General;component/images/HISTORIAL1.png", UriKind.Relative);
            bitmap.EndInit();
            this.historial.Source = bitmap;

        }

        private void historial_MouseButtonDown(object sender, MouseButtonEventArgs e)
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
            if (BtnTablas.Visibility == Visibility.Visible)
            {
                MessageBox.Show(Mensajes.MENSAJE_TABLAS_EXISTENTES, Mensajes.TITULO_TABLAS_EXISTENTES,
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            if (imprimePapel.Visibility == Visibility.Hidden)
            {
                DocumentoVoto documento;
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
                    documento = new DocumentoVoto(DocumentoActual, numeroParte+1);
                }
                else if (resultadoMsgBox.Equals(MessageBoxResult.Yes))
                {
                    documento = new DocumentoVoto(marcados);
                }
                else
                {
                    return;
                }
                //FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
                //object documentoXps = fachada.getDocumentoTesis(this.DocumentoActual.Ius);
                impresion.Document = documento.Documento; //(IDocumentPaginatorSource)documentoXps;
                //fachada.Close();
                DocumentoCopia = documento.Copia;
                impresion.Visibility = Visibility.Visible;
                impresion.Background = Brushes.White;
                tabControl1.Visibility = Visibility.Hidden;
                imprimePapel.Visibility = Visibility.Visible;
                textoAbuscar.Visibility = Visibility.Hidden;
                Buscar.Visibility = Visibility.Hidden;
                Expresion.Visibility = Visibility.Hidden;
                Guardar.Visibility = Visibility.Hidden;
                PortaPapeles.Visibility = Visibility.Hidden;
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
        #endregion

 
        private void ejecutoria_MouseButtonDown(object sender, RoutedEventArgs e)
        {
#if STAND_ALONE
            if (documentosEjecutorias.Count == 1)
#else
            if (documentosEjecutorias.Length == 1)
#endif
            {
                try
                {
                    foreach (IUSNavigationService item in Historia.Lista)
                    {
                        if ((item.Id == Int32.Parse(documentosEjecutorias[0].Ejecutoria)) && (item.TipoVentana == IUSNavigationService.EJECUTORIA))
                        {
                            MessageBox.Show(Mensajes.MENSAJE_VENTANA_ABIERTA, Mensajes.TITULO_VENTANA_ABIERTA,
                                MessageBoxButton.OK, MessageBoxImage.Information);
                            EjecutoriaPagina navegar = (EjecutoriaPagina)item.ParametroConstructor;
                            NavigationService.Navigate(navegar);
                            navegar.ventanaHistorial.Visibility = Visibility.Hidden;
                            navegar.ventanaListadoVotos.Visibility = Visibility.Hidden;
                            return;
                        }
                    }

                    //FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
                    EjecutoriaPagina ejecutoriaAsociada = new EjecutoriaPagina(Int32.Parse(documentosEjecutorias[0].Ejecutoria));
                    ejecutoriaAsociada.Historia = Historia;
                    this.NavigationService.Navigate(ejecutoriaAsociada);
                }
                catch (Exception exc)
                {
                    MessageBox.Show(Mensajes.MENSAJE_PROBLEMAS_FACHADA+exc.Message, Mensajes.TITULO_PROBLEMAS_FACHADA,
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    //Log.debug("Dieron Click muy rápido"+e.Message);
                }
            }
            else
            {
                List<int> identificadores = new List<int>();
                this.ventanaListadoEjecutorias.NavigationService = this.NavigationService;
                foreach (RelVotoEjecutoriaTO item in documentosEjecutorias)
                {
                    identificadores.Add(Int32.Parse(item.Ejecutoria));
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
                fachada.Close();
                this.ventanaListadoEjecutorias.ListaRelacion = listaFinal;
                this.ventanaListadoEjecutorias.Historia = Historia;
                this.ventanaListadoEjecutorias.NavigationService = this.NavigationService;
                this.ventanaListadoEjecutorias.Visibility = Visibility.Visible;
            }
        }


        private void inicioLista_MouseButtonDown(object sender, MouseButtonEventArgs e)
        {
            List<VotoSimplificadoTO> presentadorDatos = (List<VotoSimplificadoTO>)this.ArregloVotos.ItemsSource;
            VotoSimplificadoTO ejecutoriasMostrar = presentadorDatos[0];
            posicion = 0;
#if STAND_ALONE
            FachadaBusquedaTradicional fachada = new FachadaBusquedaTradicional();
#else
            FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
#endif
            VotoSimplificadoTO documentoActual = new VotoSimplificadoTO( fachada.getVotosPorId(Int32.Parse(ejecutoriasMostrar.Id)));
            fachada.Close();
            this.ArregloVotos.SelectedIndex = posicion;
            this.ArregloVotos.BringItemIntoView(this.ArregloVotos.SelectedItem);
            MostrarDatos(documentoActual);
        }

        private void anteriorLista_MouseButtonDown(object sender, MouseButtonEventArgs e)
        {
            List<VotoSimplificadoTO> presentadorDatos = (List<VotoSimplificadoTO>)this.ArregloVotos.ItemsSource;
            VotoSimplificadoTO votoMostrar = null;
            if (posicion == 0)
            {
                votoMostrar = presentadorDatos[0];
            }
            else
            {
                posicion--;
                votoMostrar = presentadorDatos[posicion];
            }
#if STAND_ALONE
            FachadaBusquedaTradicional fachada = new FachadaBusquedaTradicional();
#else
            FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
#endif
            VotoSimplificadoTO documentoActual = new VotoSimplificadoTO(fachada.getVotosPorId(Int32.Parse(votoMostrar.Id)));
            fachada.Close();
            this.ArregloVotos.SelectedIndex = posicion;
            this.ArregloVotos.BringItemIntoView(this.ArregloVotos.SelectedItem);
            MostrarDatos(documentoActual);
        }

        private void siguienteLista_MouseButtonDown(object sender, MouseButtonEventArgs e)
        {
            List<VotoSimplificadoTO> presentadorDatos = (List<VotoSimplificadoTO>)this.ArregloVotos.ItemsSource;
            VotoSimplificadoTO votosMostrar = null;
            if (posicion >= presentadorDatos.Count - 1)
            {
                posicion = presentadorDatos.Count - 1;
                votosMostrar = presentadorDatos[posicion];
            }
            else
            {
                posicion++;
                votosMostrar = presentadorDatos[posicion];
            }
#if STAND_ALONE
            FachadaBusquedaTradicional fachada = new FachadaBusquedaTradicional();
#else
            FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
#endif
            VotoSimplificadoTO documentoActual = new VotoSimplificadoTO(fachada.getVotosPorId(Int32.Parse(votosMostrar.Id)));
            fachada.Close();
            this.ArregloVotos.SelectedIndex = posicion;
            this.ArregloVotos.BringItemIntoView(this.ArregloVotos.SelectedItem);
            MostrarDatos(documentoActual);
        }

        private void ultimoLista_MouseButtonDown(object sender, MouseButtonEventArgs e)
        {
            List<VotoSimplificadoTO> presentadorDatos = (List<VotoSimplificadoTO>)this.ArregloVotos.ItemsSource;
            VotoSimplificadoTO votoMostrar = null;
            posicion = presentadorDatos.Count - 1;
            votoMostrar = presentadorDatos[posicion];
#if STAND_ALONE
            FachadaBusquedaTradicional fachada = new FachadaBusquedaTradicional();
#else
            FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
#endif
            VotoSimplificadoTO documentoActual = new VotoSimplificadoTO(fachada.getVotosPorId(Int32.Parse(votoMostrar.Id)));
            fachada.Close();
            this.ArregloVotos.SelectedIndex = posicion;
            this.ArregloVotos.BringItemIntoView(this.ArregloVotos.SelectedItem);
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
                            navegar.ventanaListaVotos.Visibility = Visibility.Hidden;
                            navegar.ventanaListaEjecutorias.Visibility = Visibility.Hidden;
                            navegar.ventanaHistorial.Visibility = Visibility.Hidden;
                            NavigationService.Navigate(navegar);
                            return;
                        }
                    }
#if STAND_ALONE
                    FachadaBusquedaTradicional fachada = new FachadaBusquedaTradicional();
#else
            FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
#endif
                    TesisTO tesisVerdadera = fachada.getTesisPorRegistro(documentosTesis[0].Ius);
                    Tesis tesisAsociada = new Tesis(tesisVerdadera);
                    tesisAsociada.Historia = Historia;
                    this.NavigationService.Navigate(tesisAsociada);
                }
                catch (Exception exc)
                {

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
                List<TesisTO> tesisRelacionadas = fachada.getTesisPorIus(parametros);
#else
            FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
                parametros.Listado = identificadores.ToArray();
                TesisTO[] tesisRelacionadas = fachada.getTesisPorIus(parametros);
#endif
                List<TesisSimplificadaTO> listaFinal = new List<TesisSimplificadaTO>();
                foreach (TesisTO item in tesisRelacionadas)
                {
                    TesisSimplificadaTO itemVerdadero = new TesisSimplificadaTO();
                    itemVerdadero.ConsecIndx = item.ConsecIndx;
                    itemVerdadero.Ius = item.Ius;
                   
                    listaFinal.Add(itemVerdadero);
                }
                this.ventanaListadoTesis.TesisMostrar = listaFinal;
                this.ventanaListadoTesis.NavigationService = this.NavigationService;
                this.ventanaListadoTesis.Historia = Historia;
                this.ventanaListadoTesis.Visibility = Visibility.Visible;
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
            Paragraph textoParrafo  = null;
#if STAND_ALONE
            if ((documentosTesis.Count == 0) && (numeroParte == 0))
#else
            if ((documentosTesis.Length == 0) &&(numeroParte==0))
#endif
            {
                textoParrafo = new Paragraph(new Run(DocumentoActual.Rubro + "\n\n" + partes[numeroParte].TxtParte));
            }
            else
            {
                textoParrafo = new Paragraph(new Run(partes[numeroParte].TxtParte));
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
        private void docCompletoImage_MouseEnter(object sender, MouseEventArgs e)
        {
            Image imagenNueva = new Image();
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri("/General;component/images/VER DOCUMENTO2.png", UriKind.Relative);
            bitmap.EndInit();
            this.docCompletoImage.Source = bitmap;
        }

        private void docCompletoImage_MouseLeave(object sender, MouseEventArgs e)
        {
            Image imagenNueva = new Image();
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri("/General;component/images/VER DOCUMENTO1.png", UriKind.Relative);
            bitmap.EndInit();
            this.docCompletoImage.Source = bitmap;
        }

        private void docCompletoImage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
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
            int contador = 1;
            foreach (VotosPartesTO item in partes)
            {
                Paragraph textoParrafo = ObtenLigas(item.TxtParte, DocumentoActual.Id, contador);
                contador++;
                textoParrafo.FontWeight = FontWeights.Normal;
                textoParrafo.TextAlignment = TextAlignment.Justify;
                documentoRubro.Blocks.Add(textoParrafo);
            }
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
            numeroParte = -1;
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
            List<VotoSimplificadoTO> arregloVotosActual = (List<VotoSimplificadoTO>)ArregloVotos.ItemsSource;
            if (registro > 0 && registro <= arregloVotosActual.Count)
            {
                registro--;
                VotoSimplificadoTO votoActual = arregloVotosActual[registro];
#if STAND_ALONE
                FachadaBusquedaTradicional fachada = new FachadaBusquedaTradicional();
#else
            FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
#endif
                VotoSimplificadoTO votoCompleto = new VotoSimplificadoTO(fachada.getVotosPorId(Int32.Parse(votoActual.Id)));
                fachada.Close();
                posicion = registro;
                MostrarDatos(votoCompleto);
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
        /// Devuelve el historial de ventanas actual.
        /// </summary>
        /// <returns></returns>
        protected Historial getHistorial()
        {
            return this.historia;
        }
        /// <summary>
        /// Establece el historial de ventanas actual.
        /// </summary>
        /// <param name="value">El historial de ventanas.</param>
        protected void setHistorial(Historial value)
        {
            this.historia = value;
            IUSNavigationService entradaHistorial = new IUSNavigationService();
            entradaHistorial.Id = Int32.Parse(this.DocumentoActual.Id);
            entradaHistorial.ParametroConstructor = ParametroConstruccion;
            entradaHistorial.TipoVentana = IUSNavigationService.VOTO;
            revisaHistorial(entradaHistorial);
            historia.NavigationProvider = this;
        }
        /// <summary>
        /// Revisa si la ventana actual ya estaba incluida.
        /// </summary>
        /// <param name="entrada"></param>
        protected void revisaHistorial(IUSNavigationService entrada)
        {
            Boolean encontrado = false;
            if (Historia.Lista == null)
            {
                List<IUSNavigationService> lista = new List<IUSNavigationService>();
                Historia.Lista = lista;
            }
            foreach (IUSNavigationService item in Historia.Lista)
            {
                encontrado = ((item.Id == entrada.Id) && (item.TipoVentana == IUSNavigationService.VOTO)) || encontrado;
            }
            if (!encontrado)
            {
                Historia.Lista.Add(entrada);
            }
        }

        private void inicioLista_MouseEnter(object sender, MouseEventArgs e)
        {
            Image imagenNueva = new Image();
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri("/General;component/images/FLECHA-BB2.png", UriKind.Relative);
            bitmap.EndInit();
            this.inicioLista.Source = bitmap;
        }

        private void inicioLista_MouseLeave(object sender, MouseEventArgs e)
        {
            Image imagenNueva = new Image();
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri("/General;component/images/FLECHA-BB1.png", UriKind.Relative);
            bitmap.EndInit();
            this.inicioLista.Source = bitmap;
        }

        private void siguienteLista_MouseEnter(object sender, MouseEventArgs e)
        {
            Image imagenNueva = new Image();
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri("/General;component/images/FLECHA-A2.png", UriKind.Relative);
            bitmap.EndInit();
            this.siguienteLista.Source = bitmap;
        }

        private void siguienteLista_MouseLeave(object sender, MouseEventArgs e)
        {
            Image imagenNueva = new Image();
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri("/General;component/images/FLECHA-A1.png", UriKind.Relative);
            bitmap.EndInit();
            this.siguienteLista.Source = bitmap;
        }

        private void anteriorLista_MouseEnter(object sender, MouseEventArgs e)
        {
            Image imagenNueva = new Image();
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri("/General;component/images/FLECHA-B2.png", UriKind.Relative);
            bitmap.EndInit();
            this.anteriorLista.Source = bitmap;
        }

        private void anteriorLista_MouseLeave(object sender, MouseEventArgs e)
        {
            Image imagenNueva = new Image();
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri("/General;component/images/FLECHA-B1.png", UriKind.Relative);
            bitmap.EndInit();
            this.anteriorLista.Source = bitmap;
        }

        private void ultimoLista_MouseEnter(object sender, MouseEventArgs e)
        {
            Image imagenNueva = new Image();
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri("/General;component/images/FLECHA-AA2.png", UriKind.Relative);
            bitmap.EndInit();
            this.ultimoLista.Source = bitmap;
        }

        private void ultimoLista_MouseLeave(object sender, MouseEventArgs e)
        {
            Image imagenNueva = new Image();
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri("/General;component/images/FLECHA-AA1.png", UriKind.Relative);
            bitmap.EndInit();
            this.ultimoLista.Source = bitmap;
        }
        #region guardar
        private void Guardar_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            if (!BrowserInteropHelper.IsBrowserHosted)
            {
                Imprimir_MouseButtonDown(sender, e);
                Microsoft.Win32.SaveFileDialog guardaEn = new Microsoft.Win32.SaveFileDialog();
                guardaEn.DefaultExt = ".rtf";
                guardaEn.Title = "Guardar un voto";
                guardaEn.Filter = "Archivos de Texto Enriquecido|*.rtf";
                guardaEn.AddExtension = true;
                EscondeVistaPrel();
                if ((bool)guardaEn.ShowDialog())
                {
                    FlowDocument documentoImprimir = DocumentoCopia;
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
                FlowDocument documentoImprimir = DocumentoCopia;
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
                EscondeVistaPrel();
            }
        }
        #endregion
        #region portapapeles
        private void PortaPapeles_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            //FlowDocument documentoOriginal = this.contenidoTexto.Document;
            Imprimir_MouseButtonDown(sender, e);
            FlowDocument documentoImprimir = DocumentoCopia;
            impresion.Document = null;
            this.RtbCopyPaste.Document = documentoImprimir;
            this.RtbCopyPaste.SelectAll();
            this.RtbCopyPaste.Copy();
            //MostrarDatos(DocumentoActual);
            //this.contenidoTexto.Document = documentoOriginal;
            EscondeVistaPrel();
            MessageBox.Show(Mensajes.MENSAJE_ENVIADO_PORTAPAPELES,
                Mensajes.TITULO_ENVIADO_PORTAPAPELES, MessageBoxButton.OK, MessageBoxImage.Information);
        }
        #endregion
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
            this.contenidoTexto.SetValue(RichTextBox.FontSizeProperty, CalculosPropiedadesGlobales.FontSize);
        }
        #endregion
        #region marcarTodo
        private void MarcarTodo_MouseEnter(object sender, MouseEventArgs e)
        {
            Image imagenNueva = new Image();
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri("/General;component/images/MARCAR TODOS2.png", UriKind.Relative);
            bitmap.EndInit();
            this.MarcarTodo.Source = bitmap;
        }

        private void MarcarTodo_MouseLeave(object sender, MouseEventArgs e)
        {
            Image imagenNueva = new Image();
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri("/General;component/images/MARCAR TODOS1.png", UriKind.Relative);
            bitmap.EndInit();
            this.MarcarTodo.Source = bitmap;
        }

        private void MarcarTodo_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            int faltantes = 50 - marcados.Count;
            if (faltantes > this.ArregloVotos.Items.Count)
            {
                // El número de documentos en el arreglo cabe en los faltantes
                foreach (VotoSimplificadoTO item in ArregloVotos.Items)
                {
                    marcados.Add(Int32.Parse(item.Id));
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
                this.ventanaRangos.RegistroFinal = this.ArregloVotos.Items.Count;
            }
        }
        #endregion
        #region desmarcar
        private void Desmarcar_MouseEnter(object sender, MouseEventArgs e)
        {
            Image imagenNueva = new Image();
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri("/General;component/images/DESMARCAR2.png", UriKind.Relative);
            bitmap.EndInit();
            this.Desmarcar.Source = bitmap;
        }

        private void Desmarcar_MouseLeave(object sender, MouseEventArgs e)
        {
            Image imagenNueva = new Image();
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri("/General;component/images/DESMARCAR1.png", UriKind.Relative);
            bitmap.EndInit();
            this.Desmarcar.Source = bitmap;
        }

        private void Desmarcar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
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
            if (!marcados.Contains(Int32.Parse(this.DocumentoActual.Id)))
            {
                Image imagenNueva = new Image();
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri("/General;component/images/MARCAR2.png", UriKind.Relative);
                bitmap.EndInit();
                this.Marcar.Source = bitmap;
            }
            else
            {
                Image imagenNueva = new Image();
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri("/General;component/images/PALOMA1.png", UriKind.Relative);
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
                bitmap.UriSource = new Uri("/General;component/images/MARCAR1.png", UriKind.Relative);
                bitmap.EndInit();
                this.Marcar.Source = bitmap;
            }
            else
            {
                Image imagenNueva = new Image();
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri("/General;component/images/PALOMA1.png", UriKind.Relative);
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
                bitmap.UriSource = new Uri("/General;component/images/PALOMA1.png", UriKind.Relative);
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
        private void Buscar_MouseEnter(object sender, MouseEventArgs e)
        {
            Image imagenNueva = new Image();
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri("/General;component/images/BUSQUEDA2.png", UriKind.Relative);
            bitmap.EndInit();
            this.Buscar.Source = bitmap;
        }

        private void Buscar_MouseLeave(object sender, MouseEventArgs e)
        {
            Image imagenNueva = new Image();
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri("/General;component/images/BUSQUEDA1.png", UriKind.Relative);
            bitmap.EndInit();
            this.Buscar.Source = bitmap;
        }

        private void Buscar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
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
        private void imprimePapel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
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
                    dialogoImpresion.PrintDocument(pgn, "Impresión de votos");
                }
                catch (Exception exc)
                {
                    MessageBox.Show(Mensajes.MENSAJE_IMPRESORA, Mensajes.TITULO_ARCHIVO_ABIERTO,
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
                EscondeVistaPrel();
            }
        }

        private void imprimePapel_MouseEnter(object sender, MouseEventArgs e)
        {
            Image imagenNueva = new Image();
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri("/General;component/images/IMPRIMIR2.png", UriKind.Relative);
            bitmap.EndInit();
            this.imprimePapel.Source = bitmap;
        }

        private void imprimePapel_MouseLeave(object sender, MouseEventArgs e)
        {
            Image imagenNueva = new Image();
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri("/General;component/images/IMPRIMIR1.png", UriKind.Relative);
            bitmap.EndInit();
            this.imprimePapel.Source = bitmap;
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
                VotoSimplificadoTO agregado = (VotoSimplificadoTO)this.ArregloVotos.Items.GetItemAt(contador - 1);
                marcados.Add(Int32.Parse(agregado.Id));
            }
            if (marcados.Contains(Int32.Parse(DocumentoActual.Id)))
            {
                marcados.Add(Int32.Parse(DocumentoActual.Id));
                Image imagenNueva = new Image();
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri("/General;component/images/PALOMA1.png", UriKind.Relative);
                bitmap.EndInit();
                this.Marcar.Source = bitmap;
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
            List<TablaPartesTO> listaRelaciones = fachada.getTablaVoto(Int32.Parse(id));
#else
            FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
            TablaPartesTO[] listaRelaciones = fachada.getTablaVoto(Int32.Parse(id));
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
            Informe ventanaNueva = new Informe("Anexos/"+ anexoSeleccionado.Archivo.Substring(0, anexoSeleccionado.Archivo.Length - 3) + "htm");
#endif
            NavigationService.Navigate(ventanaNueva);
            ///Esta parte es para mostrarlo en un tab en vez de una ventana nueva, debe
            ///quedar así hasta que Microsoft arregle sus problemas de rendering.
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
            //}
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
            if(verVentanaListadoEjecutorias)  ventanaListadoEjecutorias.Visibility = Visibility.Visible;
            if(verVentanaListadoTesis) ventanaListadoTesis.Visibility = Visibility.Visible;
            if(verVentanaRangos) ventanaRangos.Visibility = Visibility.Visible;
            if(verVentanaHistorial) ventanaHistorial.Visibility = Visibility.Visible;
            if(verVentanaListadoAnexos) ventanaListadoAnexos.Visibility = Visibility.Visible;

            if ((ArregloVotos != null) && (ArregloVotos.Items.Count > 1))
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
                historial.Visibility = Visibility.Visible;
#if STAND_ALONE
                if (documentosTesis.Count > 0)
#else
                        if (documentosTesis.Length > 0)
#endif
                {
                tesis.Visibility = Visibility.Visible;
            }
#if STAND_ALONE
                if (documentosEjecutorias.Count > 0)
#else
            if ( documentosEjecutorias.Length > 0)
#endif
                {
                ejecutoria.Visibility = Visibility.Visible;
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
            SalaLabel.Visibility = Visibility.Visible;
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
        /// <summary>
        ///     Muestra la vista preliminar de impresión
        /// </summary>
        private void MuestraVistaPrel()
        {
            verVentanaListadoEjecutorias = ventanaListadoEjecutorias.Visibility == Visibility.Visible;
            verVentanaListadoTesis= ventanaListadoTesis.Visibility==Visibility.Visible;
            verVentanaRangos = ventanaRangos.Visibility == Visibility.Visible;
            verVentanaHistorial = ventanaHistorial.Visibility == Visibility.Visible;
            verVentanaListadoAnexos = ventanaListadoAnexos.Visibility == Visibility.Visible;
            ventanaListadoEjecutorias.Visibility = Visibility.Hidden;
            ventanaListadoTesis.Visibility = Visibility.Hidden;
            ventanaRangos.Visibility = Visibility.Hidden;
            ventanaHistorial.Visibility = Visibility.Hidden;
            ventanaListadoAnexos.Visibility = Visibility.Hidden;

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
            tesis.Visibility = Visibility.Hidden;
            ejecutoria.Visibility = Visibility.Hidden;
            BtnTablas.Visibility = Visibility.Hidden;
            PortaPapeles.Visibility = Visibility.Hidden;
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
                Buscar_MouseLeftButtonDown(sender, null);
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
