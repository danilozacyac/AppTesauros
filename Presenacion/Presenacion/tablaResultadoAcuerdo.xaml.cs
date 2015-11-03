using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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
    public partial class tablaResultadoAcuerdo : Page
    {
        #region properties
        private bool impreso;
        /// <summary>
        /// El lugar a donde va a regresar.
        /// </summary>
        public Page Back;
        /// <summary>
        /// La lista original de los resultados.
        /// </summary>
        List<AcuerdoSimplificadoTO> resultadoAcuerdos;
        /// <summary>
        /// La lista que se muestra en el datagrid después de ordenarse
        /// </summary>
        List<AcuerdoSimplificadoTO> muestraActual;
        /// <summary>
        /// Los parámetros de la búsqueda.
        /// </summary>
        BusquedaTO busqueda;
        /// <summary>
        /// El controlador para ordenar un String.
        /// </summary>
        public String OrdenarPor { get { return this.getOrdernarPor(); } set { this.setOrdenarPor(value); } }
        private String ordenarPor;
        /// <summary>
        /// Documento que se imprimirá
        /// </summary>
        DocumentoListadoAcuerdo documentoImpresion { get; set; }
        //private FlowDocument documentoListadoimpresion;
        BackgroundWorker worker;
        protected bool verPnlOrdenar;
        public static readonly DependencyProperty RowHeightProperty =
    DependencyProperty.Register("RowHeight", typeof(int), typeof(tablaResultadoAcuerdo), new UIPropertyMetadata(99));
        protected bool verFlechas;
        protected bool mostrarIP = true;
        #endregion
        #region pruebas
        /// <summary>
        /// Iniciación por omisión en pruebas para el objeto, en
        /// este modo de pruebas los parámetros de la búsqueda son fijos
        /// </summary>
        public tablaResultadoAcuerdo()
        {
            InitializeComponent();
            buscaAcuerdos();
            this.Title="Resultado de la Búsqueda";
            this.tablaResultado.ItemsSource = resultadoAcuerdos;
            this.tablaResultado.SelectedIndex = 0;
            this.RegistrosLabel.Content = "Registros: " + resultadoAcuerdos.Count;
            tablaResultado.FontSize = Constants.FONTSIZE;
        }
        public tablaResultadoAcuerdo(int[] identificadores)
        {
            InitializeComponent();
            MostrarPorIusTO parametros = new MostrarPorIusTO();
            parametros.OrderBy = "Consec";
            parametros.OrderType = "asc";
#if STAND_ALONE
            FachadaBusquedaTradicional fachada = new FachadaBusquedaTradicional();
            parametros.Listado = identificadores.ToList();
            List<AcuerdosTO> arreglo = fachada.getAcuerdosPorIds(parametros);
#else
            FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
            parametros.Listado = identificadores;
            AcuerdosTO[] arreglo = fachada.getAcuerdos(parametros);
#endif
            resultadoAcuerdos = new List<AcuerdoSimplificadoTO>();
            foreach (AcuerdosTO item in arreglo)
            {
                resultadoAcuerdos.Add(new AcuerdoSimplificadoTO(item));
            }
            this.tablaResultado.ItemsSource = resultadoAcuerdos;
            if (identificadores.Length < 2)
            {
                verFlechas = false;
                IrANum.Visibility = Visibility.Hidden;
                btnIrA.Visibility = Visibility.Hidden;
                RegistrosLabel.Visibility = Visibility.Hidden;
                lblIrA.Visibility = Visibility.Hidden;
                inicio.Visibility = Visibility.Hidden;
                anterior.Visibility = Visibility.Hidden;
                siguiente.Visibility = Visibility.Hidden;
                final.Visibility = Visibility.Hidden;
            }
            else
            {
                verFlechas = true;
            }
            this.RegistrosLabel.Content = "Registros: " + resultadoAcuerdos.Count;
            this.tablaResultado.SelectedIndex = 0;
            fachada.Close();
            tablaResultado.FontSize = Constants.FONTSIZE;
        }
        /// <summary>
        /// Constructor por omisión para búsqueda por panel
        /// </summary>
        /// <param name="buscar">Los parámetros de la búsqueda.</param>
        public tablaResultadoAcuerdo(BusquedaTO buscar)
        {
            InitializeComponent();
            this.Title = "Buscando secuencialmente";
            busqueda = buscar;
            buscaAcuerdos();
            if (buscar.Acuerdos[6][0] || buscar.Acuerdos[6][1])
            {
                TituloAcuerdo.Title = "Otros";
               // PnlOrdenar.LblTipo.Content = "Tipo";
            }
            this.tablaResultado.ItemsSource = resultadoAcuerdos;
            if (tablaResultado.Items.Count < 2)
            {
                verFlechas = false;
                IrANum.Visibility = Visibility.Hidden;
                btnIrA.Visibility = Visibility.Hidden;
                RegistrosLabel.Visibility = Visibility.Hidden;
                lblIrA.Visibility = Visibility.Hidden;
                inicio.Visibility = Visibility.Hidden;
                anterior.Visibility = Visibility.Hidden;
                siguiente.Visibility = Visibility.Hidden;
                final.Visibility = Visibility.Hidden;
            }
            else
            {
                verFlechas = true;
            }
            this.WindowTitle = "Visualizar registros " + TituloAcuerdo.Title;
            this.tablaResultado.SelectedIndex = 0;
            this.Expresion.Content = CalculosGlobales.Expresion(buscar);
            this.RegistrosLabel.Content = "Registros: " + resultadoAcuerdos.Count;
            tablaResultado.FontSize = Constants.FONTSIZE;
        }
        /// <summary>
        /// Busca las tesis por omisión de las pruebas
        /// </summary>
        /// <returns>La lista de las ejecutorias que coinciden 
        /// con los parámetros de la búsqueda</returns>
        protected List<AcuerdoSimplificadoTO> buscaAcuerdos()
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
            busqueda.TipoBusqueda = Constants.BUSQUEDA_ACUERDO;
            Boolean browserHosted = BrowserInteropHelper.IsBrowserHosted;
#if STAND_ALONE
            FachadaBusquedaTradicional fachada = new FachadaBusquedaTradicional();
            List<AcuerdosTO> acuerdosObtenidos = null;
#else
            FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
            AcuerdosTO[] acuerdosObtenidos = null;
#endif
            try
            {
                acuerdosObtenidos = fachada.getIdConsultaPanel(busqueda).Acuerdos;
                resultadoAcuerdos = new List<AcuerdoSimplificadoTO>();
                foreach (AcuerdosTO actual in acuerdosObtenidos)
                {
                   resultadoAcuerdos.Add(new AcuerdoSimplificadoTO(actual));
                }
                List<int> listaIds= new List<int>();
                foreach (AcuerdoSimplificadoTO item in resultadoAcuerdos)
                {
                    listaIds.Add(Int32.Parse(item.Id));
                }
                MostrarPorIusTO ids = new MostrarPorIusTO();
                ids.OrderBy = "consecIndx";
                ids.OrderType = "asc";
#if STAND_ALONE
                ids.Listado = listaIds;
                List<AcuerdosTO> arregloAcuerdos = fachada.getAcuerdosPorIds(ids);
#else
                ids.Listado=listaIds.ToArray();
                AcuerdosTO[] arregloAcuerdos = fachada.getAcuerdos(ids);
#endif
                resultadoAcuerdos.Clear();
                foreach(AcuerdosTO actual in arregloAcuerdos)
                {
                    resultadoAcuerdos.Add(new AcuerdoSimplificadoTO(actual));
                }
                fachada.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                if (!EventLog.SourceExists("IUS"))
                {
                    EventLog.CreateEventSource("IUS", "IUS");
                }
                EventLog Logg = new EventLog("IUS");
                Logg.Source = "IUS";
                String mensaje = "TablaResultadoAcuerdo.xaml.cs Exception at BuscaAcuerdos()\n" + e.Message + e.StackTrace;
                Logg.WriteEntry(mensaje);
                Logg.Close();
                resultadoAcuerdos = new List<AcuerdoSimplificadoTO>();
            }

            return resultadoAcuerdos;
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
            List<AcuerdoSimplificadoTO> resultado = null;
            DocumentoListadoAcuerdo generador = new DocumentoListadoAcuerdo(tablaResultado.Items, worker);
            resultado = generador.ListaImpresion;
            documentoImpresion = generador;
            e.Result = resultado;
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
            if (e.Cancelled)
            {
                tablaResultado.Visibility = Visibility.Visible;
                impresionViewer.Visibility = Visibility.Hidden;
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
                //documentoImpresion = new DocumentoListadoAcuerdo();
                FlowDocument resultado = documentoImpresion.generaDocumento((List<AcuerdoSimplificadoTO>)e.Result);// (FlowDocument)XamlReader.Load(documento.GetStream());
                impresionViewer.Document = resultado;
                impresionViewer.Visibility = Visibility.Visible;//Means everything went as expected 
                impresionViewer.Background = Brushes.White;
                resultado.Background = Brushes.White;
                if (mostrarIP)
                {
                    MostrarImpresionPrel();
                }
                mostrarIP = true;
            }
            impreso = true;
            Esperar.Visibility = Visibility.Collapsed;
            Cursor = Cursors.Arrow;
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
        }

        private void tablaResultado_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            AcuerdosPagina muestraAcuerdo = new AcuerdosPagina(this.tablaResultado, busqueda);
            Page pagina = (Page)Application.Current.MainWindow.Content;
            muestraAcuerdo.Back = pagina;
            pagina.NavigationService.Navigate(muestraAcuerdo);
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

        private void OrdenarPor_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            this.PnlOrdenar.Padre = this;
            this.PnlOrdenar.Visibility = Visibility.Visible;
        }
        private void setOrdenarPor(string value)
        {
            ordenarPor = value;
            if (muestraActual == null)
            {
                muestraActual = (List<AcuerdoSimplificadoTO>)tablaResultado.ItemsSource;
            }
            switch (value.ToLower())
            {
                case "locabr":
                    IComparer<AcuerdoSimplificadoTO> comparador = new AcuerdosConsecIndxTOComp();
                    this.muestraActual.Sort(comparador);
                    tablaResultado.ItemsSource = null;
                    tablaResultado.ItemsSource = muestraActual;
                    break;
                case "ius":
                    comparador = new AcuerdosIdTOComp();
                    this.muestraActual.Sort(comparador);
                    tablaResultado.ItemsSource = null;
                    tablaResultado.ItemsSource = muestraActual;
                    break;
                case "rubro":
                    comparador = new AcuerdosRubroTOComp();
                    this.muestraActual.Sort(comparador);
                    tablaResultado.ItemsSource = null;
                    tablaResultado.ItemsSource = muestraActual;
                    break;
                case "tpotesis":
                    comparador = new AcuerdosTpoTesisTOComp();
                    this.muestraActual.Sort(comparador);
                    tablaResultado.ItemsSource = null;
                    tablaResultado.ItemsSource = muestraActual;
                    break;

            }
            tablaResultado.SelectedIndex = 0;
        }

        private string getOrdernarPor()
        {
            return ordenarPor;
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

        private void tablaResultado_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            int registroActual = tablaResultado.SelectedIndex + 1;
            RegistrosLabel.Content = registroActual + " de " + tablaResultado.Items.Count;
        }

        private void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            mostrarIP = false;
            imprimir_onClick(sender, e);
            while (!impreso)
            {
                System.Windows.Forms.Application.DoEvents();
            }
            EsconderImpresionPrel();
            Microsoft.Win32.SaveFileDialog guardaEn = new Microsoft.Win32.SaveFileDialog();
            guardaEn.DefaultExt = ".rtf";
            guardaEn.Title = "Guardar listado de " + TituloAcuerdo.Title.ToString().ToLower();
            guardaEn.Filter = "Archivos de Texto Enriquecido|*.rtf";
            guardaEn.AddExtension = true;
            if ((bool)guardaEn.ShowDialog())
            {
                DocumentoGuardar.Document = documentoImpresion.DocumentoImpresion;
                try
                {
                    System.IO.FileStream archivo = new System.IO.FileStream(guardaEn.FileName, System.IO.FileMode.Create);
                    this.DocumentoGuardar.SelectAll();
                    this.DocumentoGuardar.Selection.Save(archivo, System.Windows.DataFormats.Rtf);
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
        /// <summary>
        ///     Muestra los componentes relacionados con la impresión
        ///     preliminar y esconde los demás
        /// </summary>
        private void MostrarImpresionPrel()
        {
            BtnDisminuyeAlto.Visibility = Visibility.Hidden;
            BtnAumentaAlto.Visibility = Visibility.Hidden;
            BtnGuardar.Visibility = Visibility.Hidden;
            BtnVisualizar.Visibility = Visibility.Hidden;
            verPnlOrdenar = PnlOrdenar.Visibility == Visibility.Visible;
            PnlOrdenar.Visibility = Visibility.Hidden;
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
            OrdenarPorImage.Visibility = Visibility.Hidden;
        }
        /// <summary>
        ///     Esconde los componentes relacionados con la impresión preliminar y
        ///     muestra los que no.
        /// </summary>
        private void EsconderImpresionPrel()
        {
            BtnDisminuyeAlto.Visibility = Visibility.Visible;
            BtnAumentaAlto.Visibility = Visibility.Visible;
            if (!BrowserInteropHelper.IsBrowserHosted) BtnGuardar.Visibility = Visibility.Visible;
            BtnVisualizar.Visibility = Visibility.Visible;
            if (verPnlOrdenar) PnlOrdenar.Visibility = Visibility.Visible;
            Titulo.Visibility = Visibility.Visible;
            RegistrosLabel.Visibility = Visibility.Visible;
            Expresion.Visibility = Visibility.Visible;
            imprimir.ToolTip = Constants.VistaPreliminar;
            imprimir.Visibility = Visibility.Visible;
            salir.Visibility = Visibility.Visible;
            BtnTache.Visibility = Visibility.Hidden;
            btnIrA.Visibility = Visibility.Visible;
            tablaResultado.Visibility = Visibility.Visible;
            impresionViewer.Visibility = Visibility.Hidden;
            imprimePapel.Visibility = Visibility.Hidden;
            IrANum.Visibility = Visibility.Visible;
            lblIrA.Visibility = Visibility.Visible;
            inicio.Visibility = Visibility.Visible;
            anterior.Visibility = Visibility.Visible;
            siguiente.Visibility = Visibility.Visible;
            final.Visibility = Visibility.Visible;
            OrdenarPorImage.Visibility = Visibility.Visible;
            if (!verFlechas)
            {
                verFlechas = false;
                IrANum.Visibility = Visibility.Hidden;
                btnIrA.Visibility = Visibility.Hidden;
                RegistrosLabel.Visibility = Visibility.Hidden;
                lblIrA.Visibility = Visibility.Hidden;
                inicio.Visibility = Visibility.Hidden;
                anterior.Visibility = Visibility.Hidden;
                siguiente.Visibility = Visibility.Hidden;
                final.Visibility = Visibility.Hidden;
            }
        }
        /// <summary>
        ///     Sirve para cancelar la impresión
        /// </summary>
        /// <param name="sender" type="object">
        ///     <para>
        ///         el botón
        ///     </para>
        /// </param>
        /// <param name="e" type="System.Windows.RoutedEventArgs">
        ///     <para>
        ///         Los argumentos del click
        ///     </para>
        /// </param>
        private void BtnTache_Click(object sender, RoutedEventArgs e)
        {
            EsconderImpresionPrel();
        }
        /// <summary>
        ///     Cambia el registro seleccionado en el datagrid al indicado.
        /// </summary>
        private void btnIrA_Click(object sender, RoutedEventArgs e)
        {
            if(IrANum.Text.Equals("")){
                MessageBox.Show(Mensajes.MENSAJE_CAMPO_TEXTO_NUMERO_VACIO,
                    Mensajes.TITULO_CAMPO_TEXTO_VACIO,
                    MessageBoxButton.OK, MessageBoxImage.Error);
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
        /// <summary>
        ///     Hubo un enter en el textbox de ir a y debe realizar el
        ///     procedimiento como si hubieran dado click al botón.
        /// </summary>
        /// <param name="sender" type="object">
        ///     <para>
        ///         El TextBox
        ///     </para>
        /// </param>
        /// <param name="e" type="System.Windows.Input.KeyEventArgs">
        ///     <para>
        ///         Las teclas oprimidas
        ///     </para>
        /// </param>
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

        private void BtnAlmacenar_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnVisualizar_Click(object sender, RoutedEventArgs e)
        {
            if (tablaResultado.SelectedItem != null)
            {
                tablaResultado_MouseDoubleClick(sender, null);
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
     }
}
