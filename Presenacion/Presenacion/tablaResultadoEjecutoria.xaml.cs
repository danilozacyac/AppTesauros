using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using mx.gob.scjn.ius_common.TO;
using mx.gob.scjn.ius_common.TO.Comparador;
using mx.gob.scjn.ius_common.fachade;
using mx.gob.scjn.ius_common.gui.gui.impresion;
using mx.gob.scjn.ius_common.gui.impresion;
using mx.gob.scjn.ius_common.gui.utils;
using mx.gob.scjn.ius_common.utils;

namespace IUS
{
    /// <summary>
    /// Lógica de interacción para tablaResultado.xaml
    /// </summary>
    public partial class tablaResultadoEjecutoria : Page
    {
        #region properties
        private bool impreso;
        /// <summary>
        /// Variable que nos sirve para generar el ordenamiento.
        /// </summary>
        public String OrdenarPor { get { return this.getOrdernarPor(); } set { this.setOrdenarPor(value); } }
        private String ordenarPor;
        /// <summary>
        /// A donde va a regresar.
        /// </summary>
        public Page Back;
        /// <summary>
        /// La lista que se mostrará dentro del control de DataGrid
        /// </summary>
        List<EjecutoriasSimplificadaTO> resultadoEjecutorias;
        /// <summary>
        /// La lista que se mostrará dentro del control de DataGrid, una vez ordenada.
        /// </summary>
        List<EjecutoriasSimplificadaTO> muestraActual;
        /// <summary>
        /// Los parámetros de la búsqueda.
        /// </summary>
        BusquedaTO busqueda;
        /// <summary>
        /// Documento que se imprimirá
        /// </summary>
        DocumentoListadoEjecutoria documentoImpresion { get; set; }
        //private FlowDocument documentoListadoimpresion;
        BackgroundWorker worker;
        public static readonly DependencyProperty RowHeightProperty =
    DependencyProperty.Register("RowHeight", typeof(int), typeof(tablaResultadoEjecutoria), new UIPropertyMetadata(99));
        protected bool verEjecutoriasOrdenarPor { get; set; }
        protected bool verFlechas { get; set; }
        protected bool mostrarVP { get; set; }
        protected bool impresionCancelada { get; set; }
        #endregion
        #region constructores
        /// <summary>
        /// Iniciación por omisión en pruebas para el objeto, en
        /// este modo de pruebas los parámetros de la búsqueda son fijos
        /// </summary>
        public tablaResultadoEjecutoria()
        {
            InitializeComponent();
            mostrarVP = true;
            this.ListaOrdenar.Padre = this;
            buscaTesis();
            this.Title="Resultado de la Búsqueda";
            this.tablaResultado.ItemsSource = resultadoEjecutorias;
            this.muestraActual = resultadoEjecutorias;
            this.RegistrosLabel.Content = "Registros: " + resultadoEjecutorias.Count;
            tablaResultado.FontSize = Constants.FONTSIZE;
        }
        /// <summary>
        /// Constructor para la búsqueda por registros.
        /// </summary>
        /// <param name="identificadores">Los registros a desplegar</param>
        public tablaResultadoEjecutoria(int[] identificadores)
        {
            InitializeComponent();
            mostrarVP = true;
            this.ListaOrdenar.Padre = this;
            Expresion.Content = "Busqueda por Registros";
            MostrarPorIusTO parametros = new MostrarPorIusTO();
            parametros.OrderBy = "Consec";
            parametros.OrderType = "asc";
            resultadoEjecutorias = new List<EjecutoriasSimplificadaTO>();
#if STAND_ALONE
            FachadaBusquedaTradicional fachada = new FachadaBusquedaTradicional();
            parametros.Listado = identificadores.ToList();
            List<EjecutoriasTO> arreglo = fachada.getEjecutoriasPorIds(parametros);
#else
            FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
            parametros.Listado = identificadores;
            EjecutoriasTO[] arreglo = fachada.getEjecutoriasPorIds(parametros);
#endif
            foreach (EjecutoriasTO item in arreglo)
            {
                EjecutoriasSimplificadaTO ejecutoriaActual = new EjecutoriasSimplificadaTO();
                ejecutoriaActual.Id = item.Id;
                resultadoEjecutorias.Add(ejecutoriaActual);
            }
            this.tablaResultado.ItemsSource = resultadoEjecutorias;
            this.muestraActual = resultadoEjecutorias;
            fachada.Close();
            if (tablaResultado.Items.Count > 1)
            {
                verFlechas = true;
            }
            else
            {
                verFlechas = false;
                RegistrosLabel.Visibility = Visibility.Hidden;
                btnIrA.Visibility = Visibility.Hidden;
                IrANum.Visibility = Visibility.Hidden;
                lblIrA.Visibility = Visibility.Hidden;
                inicio.Visibility = Visibility.Hidden;
                anterior.Visibility = Visibility.Hidden;
                siguiente.Visibility = Visibility.Hidden;
                final.Visibility = Visibility.Hidden;
            }
            this.RegistrosLabel.Content = "Registros: " + resultadoEjecutorias.Count;
            tablaResultado.FontSize = Constants.FONTSIZE;
        }

        /// <summary>
        /// Constructor para la búsqueda ordinaria de secuenciales por panel
        /// </summary>
        /// <param name="buscar">Los parámetros del panel de búsqueda</param>
        public tablaResultadoEjecutoria(BusquedaTO buscar)
        {
            InitializeComponent();
            mostrarVP = true;
            this.ListaOrdenar.Padre = this;
            this.Title = "Buscando secuencialmente";
            Expresion.Content= CalculosGlobales.Expresion(buscar);
            Expresion.IsEnabled = false;
            busqueda = buscar;
            buscaTesis();
            this.tablaResultado.ItemsSource = resultadoEjecutorias;
            this.muestraActual = resultadoEjecutorias;
            this.RegistrosLabel.Content = "Registros: " + resultadoEjecutorias.Count;
            tablaResultado.FontSize = Constants.FONTSIZE;
        }
        /// <summary>
        /// Busca las tesis por omisión de las pruebas
        /// </summary>
        /// <returns>La lista de las ejecutorias que coinciden 
        /// con los parámetros de la búsqueda</returns>
        protected List<EjecutoriasSimplificadaTO> buscaTesis()
        {
            if (busqueda == null)
            {
                busqueda = new BusquedaTO();
                Boolean[][] epocas = new Boolean[7][];
                for (int i = 0; i < 7; i++)
                {
                    epocas[i] = new Boolean[6];
                    for (int j = 0; j < 6; j++)
                    {
                        if (i == 0 && j == 1)
                        {
                            epocas[i][j] = true;
                        }
                        else
                        {
                            epocas[i][j] = false;
                        }
                    }
                }
                busqueda.Acuerdos = null;
                busqueda.Apendices = null;
                busqueda.Epocas = epocas;
                busqueda.OrdenarPor = "ConsecIndx";
            }
            busqueda.TipoBusqueda = Constants.BUSQUEDA_EJECUTORIAS;
            Boolean browserHosted = BrowserInteropHelper.IsBrowserHosted;
#if STAND_ALONE
            FachadaBusquedaTradicional fachada = new FachadaBusquedaTradicional();
            List<EjecutoriasTO> ejecutoriasObtenidas = null;
#else
            FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
            EjecutoriasTO[] ejecutoriasObtenidas = null;
#endif
            try
            {
                ejecutoriasObtenidas = fachada.getIdConsultaPanel(busqueda).Ejecutorias;
                resultadoEjecutorias = new List<EjecutoriasSimplificadaTO>();
                foreach (EjecutoriasTO actual in ejecutoriasObtenidas)
                {
                    EjecutoriasSimplificadaTO ejecutoriaActual = new EjecutoriasSimplificadaTO();
                    ejecutoriaActual.Id = actual.Id;
                    ejecutoriaActual.OrdenarAsunto = actual.OrdenarAsunto;
                    ejecutoriaActual.OrdenarPromovente = actual.OrdenarPromovente;
                    ejecutoriaActual.ConsecIndx = actual.ConsecIndx;
                   resultadoEjecutorias.Add(ejecutoriaActual);
                }
                fachada.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                resultadoEjecutorias = new List<EjecutoriasSimplificadaTO>();
            }

            return resultadoEjecutorias;
        }
        #endregion

        private void imprimir_onClick(object sender, RoutedEventArgs e)
        {
            if (impresionViewer.Visibility.Equals(Visibility.Hidden))
            {
                imprimir.ToolTip = Constants.VISTA_PRELIMINAR_FUERA;
                MessageBoxResult resultadoAdv = MessageBoxResult.Yes;
                if (tablaResultado.Items.Count > 2000)
                {
                    resultadoAdv = MessageBox.Show(Mensajes.MENSAJE_MUCHOS_REGISTROS,
                         Mensajes.TITULO_MUCHOS_REGISTROS, MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
                }
                if (resultadoAdv == MessageBoxResult.Yes)
                {
                    documentoImpresion = null;
                    impresionViewer.Document = null;
                    //EsperaBarra.Maximum = tablaResultados.Items.Count;
                    EsperaBarra.Value = 0;
                    Esperar.Visibility = Visibility.Visible;
                    //FlowDocumentReader documentoAEscribir = new FlowDocumentReader();
                    //documentoListadoimpresion = null;
                    worker = new BackgroundWorker() { WorkerReportsProgress = true, WorkerSupportsCancellation = true };
                    worker.ProgressChanged += new ProgressChangedEventHandler(worker_ProgressChanged);
                    worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(worker_RunWorkerCompleted);
                    worker.DoWork += new DoWorkEventHandler(worker_DoWork);
                    worker.RunWorkerAsync();
                    Cursor = Cursors.Wait;
                }
            }
            else
            {
                imprimir.ToolTip = Constants.VistaPreliminar;
                tablaResultado.Visibility = Visibility.Visible;
                impresionViewer.Visibility = Visibility.Hidden;
                imprimePapel.Visibility = Visibility.Hidden;
            }
        }

        #region BackgroundWorker Events

        //This event is fired on the background thread, and is where you would do all your work 
        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            impreso = false;
           // List<EjecutoriasTO> resultado = null;
            DocumentoListadoEjecutoria generador = new DocumentoListadoEjecutoria(tablaResultado.Items, worker);
            //resultado = generador.ListaImpresion;
            if (worker.CancellationPending)
            {
                e.Cancel = true;
            }
            e.Result = generador;
        }
        /// <summary>
        /// Define que hacer cuando va avanzando el progreso del trabajador.
        /// </summary>
        /// <param name="sender">Quien lo envia</param>
        /// <param name="e">Datos del progreso</param>
        private void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            EsperaBarra.Value = e.ProgressPercentage;
        }
        /// <summary>
        /// Que hacer cuando se termine el trabajo, en este caso la generación de los datos de impresión.
        /// </summary>
        /// <param name="sender">Quien lo envía</param>
        /// <param name="e">Datos de como se completó.</param>
        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            impresionCancelada = false;
            if (e.Cancelled)
            {
                tablaResultado.Visibility = Visibility.Visible;
                impresionViewer.Visibility = Visibility.Hidden;
                this.impresionCancelada = true;
            }
            else if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message, "Error al generar el documento", MessageBoxButton.OK, MessageBoxImage.Error);
                tablaResultado.Visibility = Visibility.Visible;
                impresionViewer.Visibility = Visibility.Hidden;
                imprimePapel.Visibility = Visibility.Hidden;
            }
            else
            {
                //Package pack = PackageStore.GetPackage(new Uri("pack://temp.xps"));
                //PackagePart documento =  pack.GetPart(new Uri("/temp.xps", UriKind.Relative));
                DocumentoListadoEjecutoria generador = (DocumentoListadoEjecutoria)e.Result;
                List<EjecutoriasTO> resultado = generador.ListaImpresion;
                documentoImpresion = generador;
                TxbEspere.Text = Mensajes.GENERANDO_DOCUMENTO;
                //Ordenar la lista antes de generar el documento
                String ordenacion = ordenarPor == null ? "consecindx" : ordenarPor;
                switch (ordenacion.ToLower())
                {
                    case Constants.ORDENAR_CONSEC:
                        IComparer<EjecutoriasTO>comparador = new EjecutoriasConsecIndxNSTOComp();
                        resultado.Sort(comparador);
                        break;
                    case Constants.ORDENAR_ID:
                        comparador = new EjecutoriasIdNSTOComp();
                        resultado.Sort(comparador);
                        break;
                }

                impresionViewer.Document = DocumentoListadoEjecutoria.generaDocumento(resultado);
                if (!DocumentoListadoEjecutoria.cancelado)
                {
                    impresionViewer.Background = Brushes.White;
                    //resultado.Background = Brushes.White;
                    if (mostrarVP)
                    {
                        MostrarImpresionPrel();
                    }
                }                
            }
            impreso = true;
            Esperar.Visibility = Visibility.Collapsed;
            Cursor = Cursors.Arrow;
        }
        #endregion 
        #region vistapreliminar
        private void MostrarImpresionPrel()
        {
            verFlechas=(tablaResultado.Items.Count>0);
            BtnDisminuyeAlto.Visibility = Visibility.Hidden;
            BtnAumentaAlto.Visibility = Visibility.Hidden;
            BtnGuardar.Visibility = Visibility.Hidden;
            BtnVisualizar.Visibility = Visibility.Hidden;
            verEjecutoriasOrdenarPor = ListaOrdenar.Visibility == Visibility.Visible;
            ListaOrdenar.Visibility = Visibility.Hidden;
            Titulo.Visibility = Visibility.Hidden;
            RegistrosLabel.Visibility = Visibility.Hidden;
            Expresion.Visibility = Visibility.Hidden;
            tablaResultado.Visibility = Visibility.Hidden;
            impresionViewer.Visibility = Visibility.Visible;
            imprimePapel.Visibility = Visibility.Visible;
            IrANum.Visibility = Visibility.Hidden;
            lblIrA.Visibility = Visibility.Hidden;
            btnIrA.Visibility = Visibility.Hidden;
            imprimir.ToolTip = Constants.VISTA_PRELIMINAR_FUERA;
            imprimir.Visibility = Visibility.Hidden;
            salir.Visibility = Visibility.Hidden;
            BtnTache.Visibility = Visibility.Visible;
            imprimePapel.Visibility = Visibility.Visible;
            inicio.Visibility = Visibility.Hidden;
            anterior.Visibility = Visibility.Hidden;
            siguiente.Visibility = Visibility.Hidden;
            final.Visibility = Visibility.Hidden;
            Ordena.Visibility = Visibility.Hidden;
        }
        private void EsconderImpresionPrel()
        {
            BtnDisminuyeAlto.Visibility = Visibility.Visible;
            BtnAumentaAlto.Visibility = Visibility.Visible;
            if (!BrowserInteropHelper.IsBrowserHosted) BtnGuardar.Visibility = Visibility.Visible;
            BtnVisualizar.Visibility = Visibility.Visible;
            if (verEjecutoriasOrdenarPor) ListaOrdenar.Visibility = Visibility.Visible;
            Titulo.Visibility = Visibility.Visible;
            Expresion.Visibility = Visibility.Visible;
            imprimir.ToolTip = Constants.VistaPreliminar;
            imprimir.Visibility = Visibility.Visible;
            salir.Visibility = Visibility.Visible;
            BtnTache.Visibility = Visibility.Hidden;
            tablaResultado.Visibility = Visibility.Visible;
            impresionViewer.Visibility = Visibility.Hidden;
            imprimePapel.Visibility = Visibility.Hidden;
            if (verFlechas)
            {
                IrANum.Visibility = Visibility.Visible;
                btnIrA.Visibility = Visibility.Visible;
                RegistrosLabel.Visibility = Visibility.Visible;
                lblIrA.Visibility = Visibility.Visible;
                inicio.Visibility = Visibility.Visible;
                anterior.Visibility = Visibility.Visible;
                siguiente.Visibility = Visibility.Visible;
                final.Visibility = Visibility.Visible;
            }
            Ordena.Visibility = Visibility.Visible;
        }

        #endregion
        private void salir_onClick(object sender, RoutedEventArgs e)
        {
            if (Back == null)
            {
                this.NavigationService.GoBack();
            }
            else
            {
                this.NavigationService.Navigate(Back);
            }
            this.NavigationService.RemoveBackEntry();
        }

        private void tablaResultado_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            EjecutoriaPagina muestraEjecutoria = new EjecutoriaPagina(this.tablaResultado, this.busqueda);
            Page pagina = (Page)Application.Current.MainWindow.Content;
            muestraEjecutoria.Back = pagina;
            pagina.NavigationService.Navigate(muestraEjecutoria);
        }

        private void inicio_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            tablaResultado.SelectedIndex = 0;
            tablaResultado.BringItemIntoView(tablaResultado.SelectedItem);

        }

        private void anterior_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            if (tablaResultado.SelectedIndex > 0)
            {
                tablaResultado.SelectedIndex--;
            }
            tablaResultado.BringItemIntoView(tablaResultado.SelectedItem);
        }

        private void siguiente_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            if (tablaResultado.Items.Count > tablaResultado.SelectedIndex)
            {
                tablaResultado.SelectedIndex++;
            }
            tablaResultado.BringItemIntoView(tablaResultado.SelectedItem);
        }
        private void final_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            tablaResultado.SelectedIndex = tablaResultado.Items.Count - 1;
            tablaResultado.BringItemIntoView(tablaResultado.SelectedItem);
        }

        private void tablaResultados_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            int registroActual = tablaResultado.SelectedIndex + 1;
            RegistrosLabel.Content = registroActual + "/" + tablaResultado.Items.Count;
        }

        private void btnIrA_Click(object sender, RoutedEventArgs e)
        {
            if (IrANum.Text.Equals(""))
            {
                MessageBox.Show(Mensajes.MENSAJE_CAMPO_TEXTO_NUMERO_VACIO, Mensajes.TITULO_CAMPO_REQUERIDO,
                    MessageBoxButton.OK, MessageBoxImage.Error);
                IrANum.Focus();
                return;
            }
            int registroSeleccionado = Int32.Parse(IrANum.Text);
            registroSeleccionado--;
            if ((tablaResultado.Items.Count > registroSeleccionado)
                || (registroSeleccionado < 0))
            {
                tablaResultado.SelectedIndex = registroSeleccionado;
                IrANum.Text = "";
            }
            else
            {
                MessageBox.Show(Mensajes.MENSAJE_CONSECUTIVO_NO_VALIDO,
                    Mensajes.TITULO_CONSECUTIVO_NO_VALIDO, MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
            tablaResultado.BringItemIntoView(tablaResultado.SelectedItem);
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (tablaResultado.SelectedItem != null)
            {
                tablaResultado.BringItemIntoView(tablaResultado.SelectedItem);
            }
            else
            {
                tablaResultado.SelectedIndex = 0;
                tablaResultado.BringItemIntoView(tablaResultado.SelectedItem);
            }
            if (Application.Current.MainWindow.Content != this)
            {
                salir.Visibility = Visibility.Collapsed;
            }
        }

        private void imprimePapel_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            PrintDialog dialogoImpresion = new PrintDialog();
            IDocumentPaginatorSource paginado = impresionViewer.Document as IDocumentPaginatorSource;
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
                    dialogoImpresion.PrintDocument(pgn, "Impresión de la tabla de resultados");

                    EsconderImpresionPrel();
                }
                catch (Exception exc)
                {
                    MessageBox.Show(Mensajes.MENSAJE_IMPRESORA, Mensajes.TITULO_ARCHIVO_ABIERTO,
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        /// <summary>
        /// Define por que campo se ordenará la lista de resultados
        /// </summary>
        /// <param name="value">El valor del campo de ordenamiento</param>
        private void setOrdenarPor(string value)
        {

            switch (value.ToLower())
            {
                case Constants.ORDENAR_PROMOVENTE:
                    IComparer<EjecutoriasSimplificadaTO> comparador = new EjecutoriaTOComp();
                    this.muestraActual.Sort(comparador);
                    tablaResultado.ItemsSource = null;
                    tablaResultado.ItemsSource = muestraActual;
                    break;
                case Constants.ORDENAR_CONSEC:
                    comparador = new EjecutoriasConsecIndxTOComp();
                    this.muestraActual.Sort(comparador);
                    tablaResultado.ItemsSource = null;
                    tablaResultado.ItemsSource = muestraActual;
                    break;
                case Constants.ORDENAR_ID:
                    comparador = new EjecutoriasIdTOComp();
                    this.muestraActual.Sort(comparador);
                    tablaResultado.ItemsSource = null;
                    tablaResultado.ItemsSource = muestraActual;
                    break;
                case Constants.ORDENAR_ASUNTO:
                    comparador = new EjecutoriasRubroTOComp();
                    this.muestraActual.Sort(comparador);
                    tablaResultado.ItemsSource = null;
                    tablaResultado.ItemsSource = muestraActual;
                    break;
            }
            tablaResultado.SelectedIndex = 0;
            ordenarPor = value;
        }

        private string getOrdernarPor()
        {
            return this.ordenarPor;
        }

        private void Ordena_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            this.ListaOrdenar.Visibility = Visibility.Visible;
        }

        private void BtnTache_Click(object sender, RoutedEventArgs e)
        {
            EsconderImpresionPrel();
        }

        private void IrANum_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                btnIrA_Click(sender, e);
            }
            if ((e.Key == Key.Decimal) || (e.Key == Key.OemPeriod))
            {
                e.Handled = true;
            }
        }

        private void BtnVisualizar_Click(object sender, RoutedEventArgs e)
        {
            if (tablaResultado.SelectedItem != null)
            {
                tablaResultado_MouseDoubleClick(sender, null);
            }
        }

        private void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            mostrarVP = false;
            if (!BrowserInteropHelper.IsBrowserHosted)
            {
                imprimir_onClick(sender, e);
                while (!impreso)
                {
                    System.Windows.Forms.Application.DoEvents();
                }
                if (!impresionCancelada)
                {
                    Microsoft.Win32.SaveFileDialog guardaEn = new Microsoft.Win32.SaveFileDialog();
                    guardaEn.DefaultExt = ".rtf";
                    guardaEn.Title = "Guardar listado de ejecutorias";
                    guardaEn.Filter = "Archivos de Texto Enriquecido|*.rtf";
                    guardaEn.AddExtension = true;
                    EsconderImpresionPrel();
                    mostrarVP = true;
                    if ((bool)guardaEn.ShowDialog())
                    {
                        FlowDocument documentoImprimir = DocumentoListadoEjecutoria.generaDocumento(documentoImpresion.ListaImpresion);
                        impresionViewer.Document = null;
                        this.Contenido.Document = documentoImprimir;
                        try
                        {
                            System.IO.FileStream archivo = new System.IO.FileStream(guardaEn.FileName, System.IO.FileMode.Create);
                            this.Contenido.SelectAll();
                            this.Contenido.Selection.Save(archivo, System.Windows.DataFormats.Rtf);
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
                    }
                }
            }
        }

        private void BtnAumentaAlto_Click(object sender, RoutedEventArgs e)
        {
            if ((Int32)GetValue(RowHeightProperty) < 300)
            {
                SetValue(RowHeightProperty, (Int32)GetValue(RowHeightProperty) + 5);
            }
        }

        private void BtnDisminuyeAlto_Click(object sender, RoutedEventArgs e)
        {
            if ((Int32)GetValue(RowHeightProperty) > 10)
            {
                SetValue(RowHeightProperty, (Int32)GetValue(RowHeightProperty) - 5);
            }
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult res = MessageBox.Show(Mensajes.MENSAJE_CANCELAR, Mensajes.TITULO_CANCELAR,
                        MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
            if (res.Equals(MessageBoxResult.Yes))
            {
                worker.CancelAsync();
                DocumentoListadoEjecutoria.cancelado = true;
            }
        }

     }
}
