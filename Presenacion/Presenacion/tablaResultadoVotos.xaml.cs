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
using System.Windows.Media.Imaging;
using Xceed.Wpf.DataGrid;
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
    public partial class tablaResultadoVotos : Page
    {
        #region properties
        private bool impreso { get; set; }
        /// <summary>
        /// El lugar a donde debe regresar.
        /// </summary>
        public Page Back { get; set; }
        /// <summary>
        /// La lista que se mostrará dentro del control de DataGrid
        /// </summary>
        List<VotoSimplificadoTO> resultadoVotos { get; set; }
        List<VotoSimplificadoTO> resultadoOriginalOtros { get; set; }
        private DataGridControl TablaResultado { get; set; }
        List<VotoSimplificadoTO> muestraActual { get; set; }
        /// <summary>
        /// Los parámetros de la búsqueda.
        /// </summary>
        BusquedaTO busqueda;
        public String OrdenarPor { get { return this.getOrdenarPor(); } set { this.setOrdenarPor(value);} }
        private String ordenarPor;
        /// <summary>
        /// Documento que se imprimirá
        /// </summary>
        DocumentoListadoVoto documentoImpresion { get; set; }
        //private FlowDocument documentoListadoImpresion { get; set; }
        BackgroundWorker worker;
        public static readonly DependencyProperty RowHeightProperty =
            DependencyProperty.Register("RowHeight", typeof(int), typeof(tablaResultadoVotos), new UIPropertyMetadata(99));
        protected bool verClasificacion { get; set; }
        protected bool verPnlOrdenar { get; set; }
        protected bool verFlechas { get; set; }
        protected bool verOriginal { get; set; }
        protected bool verBtnClasificacion { get; set; }
        #endregion
        #region constructores
        /// <summary>
        /// Iniciación por omisión en pruebas para el objeto, en
        /// este modo de pruebas los parámetros de la búsqueda son fijos
        /// </summary>
        public tablaResultadoVotos()
        {
            InitializeComponent();
            verFlechas = false;
            this.Clasificacion.NavigationService = this;
            buscaTesis();
            this.Title = "Resultado de la Búsqueda";
            this.tablaResultado.ItemsSource = resultadoVotos;
            this.RegistrosLabel.Content = "Registros: " + resultadoVotos.Count;
            tablaResultado.FontSize = Constants.FONTSIZE;
            tablaResultadoConcurrentes.FontSize = Constants.FONTSIZE;
            tablaResultadoMinoritario.FontSize = Constants.FONTSIZE;
            tablaResultadoParticulares.FontSize = Constants.FONTSIZE;
            tablaResultadoTodos.FontSize = Constants.FONTSIZE;
        }
        public tablaResultadoVotos(int[] identificadores)
        {
            InitializeComponent();
            this.Clasificacion.NavigationService = this;
            MostrarPorIusTO parametros = new MostrarPorIusTO();
            parametros.OrderBy = "Consec";
            parametros.OrderType = "asc";
            resultadoVotos = new List<VotoSimplificadoTO>();
            List<VotoSimplificadoTO> votosConcurrentes = new List<VotoSimplificadoTO>();
            List<VotoSimplificadoTO> votosMinoritario = new List<VotoSimplificadoTO>();
            List<VotoSimplificadoTO> votosParticular = new List<VotoSimplificadoTO>();
#if STAND_ALONE
            FachadaBusquedaTradicional fachada = new FachadaBusquedaTradicional();
            parametros.Listado = identificadores.ToList();
            List<VotosTO> arreglo = fachada.getVotosPorIds(parametros);
#else
            FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
            parametros.Listado = identificadores;
            VotosTO[] arreglo = fachada.getVotosPorIds(parametros);
#endif
            List<VotoSimplificadoTO> votosTodos = new List<VotoSimplificadoTO>();
            foreach (VotosTO item in arreglo)
            {
                VotoSimplificadoTO itemTemporal = new VotoSimplificadoTO(item);
                votosTodos.Add(new VotoSimplificadoTO(item));
                if (item.Clasificacion.Equals(Constants.VOTO_CONCURRENTE))
                {
                    votosConcurrentes.Add(new VotoSimplificadoTO(item));
                }
                else if (item.Clasificacion.Equals(Constants.VOTO_MINORITARIO))
                {
                    votosMinoritario.Add(new VotoSimplificadoTO(item));
                }
                else if (item.Clasificacion.Equals(Constants.VOTO_PARTICULAR))
                {
                    votosParticular.Add(new VotoSimplificadoTO(item));
                }
                else
                {
                    resultadoVotos.Add(itemTemporal);
                }
            }
            this.tablaResultado.ItemsSource =  resultadoVotos;
            resultadoOriginalOtros =  resultadoVotos;
            this.tablaResultadoConcurrentes.ItemsSource = votosConcurrentes;
            this.tablaResultadoMinoritario.ItemsSource = votosMinoritario;
            this.tablaResultadoParticulares.ItemsSource = votosParticular;
            this.tablaResultadoTodos.ItemsSource = votosTodos;
            HabilitaVotos();
            TbiConcurrentes_GotFocus(null, null);
            fachada.Close();
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
            this.RegistrosLabel.Content = "Registros: " +  resultadoVotos.Count;
            tablaResultado.FontSize = Constants.FONTSIZE;
            tablaResultadoConcurrentes.FontSize = Constants.FONTSIZE;
            tablaResultadoMinoritario.FontSize = Constants.FONTSIZE;
            tablaResultadoParticulares.FontSize = Constants.FONTSIZE;
            tablaResultadoTodos.FontSize = Constants.FONTSIZE;

        }

        private void HabilitaVotos()
        {
            if (this.tablaResultadoConcurrentes.Items.Count == 0)
            {
                this.TbiConcurrentes.IsEnabled = false;
            }
            if (this.tablaResultadoMinoritario.Items.Count == 0)
            {
                this.TbiMinoria.IsEnabled = false;
            }
            if (this.tablaResultadoParticulares.Items.Count == 0)
            {
                this.TbiParticulares.IsEnabled = false;
            }
            if (this.tablaResultadoTodos.Items.Count == 0)
            {
                this.TbiTodos.IsEnabled = false;
            }
            if (this.tablaResultado.Items.Count == 0)
            {
                this.TbiOtros.IsEnabled = false;
            }
        }
        /// <summary>
        /// Búsqueda general de secuencial por panel.
        /// </summary>
        /// <param name="buscar">Los parámetros de búsqueda del panel</param>
        public tablaResultadoVotos(BusquedaTO buscar)
        {
            InitializeComponent();
            this.Title = "Buscando secuencialmente";
            busqueda = buscar;
            buscaTesis();
            this.tablaResultado.ItemsSource = resultadoVotos;
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
            this.RegistrosLabel.Content = "Registros: " + resultadoVotos.Count;
            this.Clasificacion.NavigationService = this;
            List<int> listaClasificados = new List<int>();
            foreach (VotoSimplificadoTO item in resultadoVotos)
            {
                listaClasificados.Add(Int32.Parse(item.Clasificacion));
            }
            this.Clasificacion.existen = listaClasificados;
            TbiConcurrentes_GotFocus(null, null);
            tablaResultado.FontSize = Constants.FONTSIZE;
            tablaResultadoConcurrentes.FontSize = Constants.FONTSIZE;
            tablaResultadoMinoritario.FontSize = Constants.FONTSIZE;
            tablaResultadoParticulares.FontSize = Constants.FONTSIZE;
            tablaResultadoTodos.FontSize = Constants.FONTSIZE;
            HabilitaVotos();
        }
        /// <summary>
        /// Busca las tesis por omisión de las pruebas
        /// </summary>
        /// <returns>La lista de las ejecutorias que coinciden 
        /// con los parámetros de la búsqueda</returns>
        protected List<VotoSimplificadoTO> buscaTesis()
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
            //FachadaBusquedaTradicionalClient fachadaImpl = null;
            busqueda.TipoBusqueda = Constants.BUSQUEDA_VOTOS;
            Boolean browserHosted = BrowserInteropHelper.IsBrowserHosted;
#if STAND_ALONE
            FachadaBusquedaTradicional fachada = new FachadaBusquedaTradicional();
            List<VotosTO> votosObtenidos = null;
#else
            FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
            VotosTO[] votosObtenidos = null;
#endif
            try
            {
                votosObtenidos = fachada.getIdConsultaPanel(busqueda).Votos;
                resultadoVotos = new List<VotoSimplificadoTO>();
                List<VotoSimplificadoTO> votosTodos = new List<VotoSimplificadoTO>();
                List<VotoSimplificadoTO> votosConcurrentes = new List<VotoSimplificadoTO>();
                List<VotoSimplificadoTO> votosMinoritario = new List<VotoSimplificadoTO>();
                List<VotoSimplificadoTO> votosParticular = new List<VotoSimplificadoTO>();
                foreach (VotosTO actual in votosObtenidos)
                {
                    VotoSimplificadoTO itemTemporal = new VotoSimplificadoTO(actual);
                    resultadoVotos.Add(itemTemporal);
                }
                List<int> listaIds = new List<int>();
                foreach (VotoSimplificadoTO item in resultadoVotos)
                {
                    listaIds.Add(Int32.Parse(item.Id));
                }
                MostrarPorIusTO ids = new MostrarPorIusTO();
                ids.OrderBy = "consecIndx";
                ids.OrderType = "asc";
#if STAND_ALONE
                ids.Listado = listaIds;
                List<VotosTO> arregloVotos = fachada.getVotosPorIds(ids);
#else
                ids.Listado=listaIds.ToArray();
                VotosTO[] arregloVotos = fachada.getVotosPorIds(ids);
#endif
                resultadoVotos.Clear();
                foreach (VotosTO actual in arregloVotos)
                {
                    VotoSimplificadoTO itemTemporal = new VotoSimplificadoTO(actual);
                    votosTodos.Add(new VotoSimplificadoTO(actual));
                    if (actual.Clasificacion.Equals(Constants.VOTO_CONCURRENTE))
                    {
                        votosConcurrentes.Add(new VotoSimplificadoTO(actual));
                    }
                    else if (actual.Clasificacion.Equals(Constants.VOTO_MINORITARIO))
                    {
                        votosMinoritario.Add(new VotoSimplificadoTO(actual));
                    }
                    else if (actual.Clasificacion.Equals(Constants.VOTO_PARTICULAR))
                    {
                        votosParticular.Add(new VotoSimplificadoTO(actual));
                    }
                    else
                    {
                        resultadoVotos.Add(itemTemporal);
                    }
                }
                fachada.Close();
                resultadoOriginalOtros = votosTodos;
                //resultadoVotos = votosTodos;
                tablaResultadoTodos.ItemsSource = votosTodos;
                tablaResultadoMinoritario.ItemsSource = votosMinoritario;
                tablaResultadoParticulares.ItemsSource = votosParticular;
                tablaResultadoConcurrentes.ItemsSource = votosConcurrentes;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                resultadoVotos = new List<VotoSimplificadoTO>();
            }

            return resultadoVotos;
        }
        #endregion
   
        private void imprimir_onClick(object sender, RoutedEventArgs e)
        {
            TablaResultado = null;
            if (TbiMinoria.IsSelected)
            {
                TablaResultado = tablaResultadoMinoritario;
            }
            else if (TbiConcurrentes.IsSelected)
            {
                TablaResultado = tablaResultadoConcurrentes;
            }
            else if (TbiParticulares.IsSelected)
            {
                TablaResultado = tablaResultadoParticulares;
            }
            else if (TbiTodos.IsSelected)
            {
                TablaResultado = tablaResultadoTodos;
            }
            else
            {
                TablaResultado = tablaResultado;
            }
            if (impresionViewer.Visibility.Equals(Visibility.Hidden))
            {
                imprimir.ToolTip = Constants.VISTA_PRELIMINAR_FUERA;
                MessageBoxResult resultadoAdv = MessageBoxResult.Yes;
                if (TablaResultado.Items.Count > 2000)
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
                    FlowDocumentReader documentoAEscribir = new FlowDocumentReader();
                    //documentoListadoImpresion = null;
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
            DataGridControl TablaMostrar = null;
                TablaMostrar = TablaResultado;
            VotosPagina muestraVotos = new VotosPagina(TablaMostrar, busqueda);
            Page pagina = (Page)Application.Current.MainWindow.Content;
            muestraVotos.Back = pagina;
            pagina.NavigationService.Navigate(muestraVotos);
        }
        private void inicio_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            TablaResultado = null;
            if (TbiMinoria.IsSelected)
            {
                TablaResultado = tablaResultadoMinoritario;
            }
            else if (TbiConcurrentes.IsSelected)
            {
                TablaResultado = tablaResultadoConcurrentes;
            }
            else if (TbiParticulares.IsSelected)
            {
                TablaResultado = tablaResultadoParticulares;
            }
            else if (TbiTodos.IsSelected)
            {
                TablaResultado = tablaResultadoTodos;
            }
            else
            {
                TablaResultado = tablaResultado;
            }
            TablaResultado.SelectedIndex = 0;
            TablaResultado.BringItemIntoView(TablaResultado.SelectedItem);

        }

        private void anterior_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            TablaResultado = null;
            if (TbiMinoria.IsSelected)
            {
                TablaResultado = tablaResultadoMinoritario;
            }
            else if (TbiConcurrentes.IsSelected)
            {
                TablaResultado = tablaResultadoConcurrentes;
            }
            else if (TbiParticulares.IsSelected)
            {
                TablaResultado = tablaResultadoParticulares;
            }
            else if (TbiTodos.IsSelected)
            {
                TablaResultado = tablaResultadoTodos;
            }
            else
            {
                TablaResultado = tablaResultado;
            }
            if (TablaResultado.SelectedIndex > 0)
            {
                TablaResultado.SelectedIndex--;
            }
            TablaResultado.BringItemIntoView(TablaResultado.SelectedItem);
        }

        private void siguiente_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            TablaResultado = null;
            if (TbiMinoria.IsSelected)
            {
                TablaResultado = tablaResultadoMinoritario;
            }
            else if (TbiConcurrentes.IsSelected)
            {
                TablaResultado = tablaResultadoConcurrentes;
            }
            else if (TbiParticulares.IsSelected)
            {
                TablaResultado = tablaResultadoParticulares;
            }
            else if (TbiTodos.IsSelected)
            {
                TablaResultado = tablaResultadoTodos;
            }
            else
            {
                TablaResultado = tablaResultado;
            }
            if (TablaResultado.Items.Count > TablaResultado.SelectedIndex)
            {
                TablaResultado.SelectedIndex++;
            }
            TablaResultado.BringItemIntoView(TablaResultado.SelectedItem);
        }

        private void final_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            TablaResultado = null;
            if (TbiMinoria.IsSelected)
            {
                TablaResultado = tablaResultadoMinoritario;
            }
            else if (TbiConcurrentes.IsSelected)
            {
                TablaResultado = tablaResultadoConcurrentes;
            }
            else if (TbiParticulares.IsSelected)
            {
                TablaResultado = tablaResultadoParticulares;
            }
            else if (TbiTodos.IsSelected)
            {
                TablaResultado = tablaResultadoTodos;
            }
            else
            {
                TablaResultado = tablaResultado;
            }
            TablaResultado.SelectedIndex = TablaResultado.Items.Count - 1;
            TablaResultado.BringItemIntoView(TablaResultado.SelectedItem);
        }

        private void tablaResultados_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (TbiOtros.IsSelected)
            {
                int registroActual = tablaResultado.SelectedIndex + 1;
                RegistrosLabel.Content = registroActual + "/" + tablaResultado.Items.Count;
            }
            else if (TbiConcurrentes.IsSelected)
            {
                int registroActual = tablaResultadoConcurrentes.SelectedIndex + 1;
                RegistrosLabel.Content = registroActual + "/" + tablaResultadoConcurrentes.Items.Count;
            }
            else if (TbiTodos.IsSelected)
            {
                int registroActual = tablaResultadoTodos.SelectedIndex + 1;
                RegistrosLabel.Content = registroActual + "/" + tablaResultadoTodos.Items.Count;
            }
            else if (TbiMinoria.IsSelected)
            {
                int registroActual = tablaResultadoMinoritario.SelectedIndex + 1;
                RegistrosLabel.Content = registroActual + "/" + tablaResultadoMinoritario.Items.Count;
            }
            else if (TbiParticulares.IsSelected)
            {
                int registroActual = tablaResultadoParticulares.SelectedIndex + 1;
                RegistrosLabel.Content = registroActual + "/" + tablaResultadoParticulares.Items.Count;
            }
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
            TablaResultado = null;
            if (TbiMinoria.IsSelected)
            {
                TablaResultado = tablaResultadoMinoritario;
            }
            else if (TbiConcurrentes.IsSelected)
            {
                TablaResultado = tablaResultadoConcurrentes;
            }
            else if (TbiParticulares.IsSelected)
            {
                TablaResultado = tablaResultadoParticulares;
            }
            else
            {
                TablaResultado = tablaResultado;
            }
            int registroSeleccionado = Int32.Parse(IrANum.Text);
            registroSeleccionado--;
            if ((TablaResultado.Items.Count > registroSeleccionado)
                || (registroSeleccionado < 0))
            {
                TablaResultado.SelectedIndex = registroSeleccionado;
                IrANum.Text = "";
            }
            else
            {
                MessageBox.Show(Mensajes.MENSAJE_CONSECUTIVO_NO_VALIDO,
                    Mensajes.TITULO_CONSECUTIVO_NO_VALIDO, MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
            TablaResultado.BringItemIntoView(TablaResultado.SelectedItem);
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

        private void btnClasificacion_MouseEnter(object sender, MouseEventArgs e)
        {
            Image imagenNueva = new Image();
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri("/General;component/images/Filtrar2.png", UriKind.Relative);
            bitmap.EndInit();
            this.btnClasificacion.Source = bitmap;
        }

        private void btnClasificacion_MouseLeave(object sender, MouseEventArgs e)
        {
            Image imagenNueva = new Image();
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri("/General;component/images/Filtrar1.png", UriKind.Relative);
            bitmap.EndInit();
            this.btnClasificacion.Source = bitmap;
        }

        private void btnClasificacion_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Clasificacion.Visibility = Visibility.Visible;
        }


        internal void ActualizaClasificacion(List<ClassificacionTO> resultado)
        {
            if (busqueda != null)
            {
                ClassificacionTO[] clasificado = resultado.ToArray();
#if STAND_ALONE
                busqueda.Clasificacion = resultado;
                FachadaBusquedaTradicional fachada = new FachadaBusquedaTradicional();
                List<VotosTO> votosObtenidos;
#else
                busqueda.clasificacion = clasificado;
            FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
                VotosTO[] votosObtenidos;
#endif
                try
                {
                    votosObtenidos = fachada.getIdConsultaPanel(busqueda).Votos;
                    resultadoVotos = new List<VotoSimplificadoTO>();
                    foreach (VotosTO actual in votosObtenidos)
                    {
                        VotoSimplificadoTO itemTemporal = new VotoSimplificadoTO(actual);
                        resultadoVotos.Add(itemTemporal);
                    }
                    muestraActual = resultadoVotos;
                    List<int> listaIds = new List<int>();
                    foreach (VotoSimplificadoTO item in muestraActual)
                    {
                        listaIds.Add(Int32.Parse(item.Id));
                    }
                    MostrarPorIusTO ids = new MostrarPorIusTO();
                    ids.OrderBy = "consecIndx";
                    ids.OrderType = "asc";
#if STAND_ALONE
                    ids.Listado = listaIds;
                    List<VotosTO> arregloVotos = fachada.getVotosPorIds(ids);
#else
                    ids.Listado = listaIds.ToArray();
                    VotosTO[] arregloVotos = fachada.getVotosPorIds(ids);
#endif
                    muestraActual.Clear();
                    foreach (VotosTO actual in arregloVotos)
                    {
                        VotoSimplificadoTO itemTemporal = new VotoSimplificadoTO(actual);
                        muestraActual.Add(itemTemporal);
                    }
                    fachada.Close();
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                    resultadoVotos = new List<VotoSimplificadoTO>();
                }
                tablaResultado.ItemsSource = resultadoVotos;
                if (resultadoOriginalOtros.Count > 0) tablaResultado.SelectedIndex = 0;
                TbiOtros.Focus();
            }
            return;
        }

        private void btnOrdenar_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            this.pnlOrdenar.Padre = this;
            this.pnlOrdenar.Visibility = Visibility.Visible;
        }
        private void setOrdenarPor(string value)
        {
            DataGridControl DgActual = null;
            if (TbiOtros.IsSelected)
            {
                muestraActual = (List<VotoSimplificadoTO>)tablaResultado.ItemsSource;
                DgActual = tablaResultado;
            }
            else if (TbiMinoria.IsSelected)
            {
                muestraActual = (List<VotoSimplificadoTO>)tablaResultadoMinoritario.ItemsSource;
                DgActual = tablaResultadoMinoritario;
            }
            else if (TbiConcurrentes.IsSelected)
            {
                muestraActual = (List<VotoSimplificadoTO>)tablaResultadoConcurrentes.ItemsSource;
                DgActual = tablaResultadoConcurrentes;
            }
            else if (TbiParticulares.IsSelected)
            {
                muestraActual = (List<VotoSimplificadoTO>)tablaResultadoParticulares.ItemsSource;
                DgActual = tablaResultadoParticulares;
            }
            else
            {
                muestraActual = (List<VotoSimplificadoTO>)tablaResultadoTodos.ItemsSource;
                DgActual = tablaResultadoTodos;
            }

            if (muestraActual == null)
            {
                muestraActual = (List<VotoSimplificadoTO>)tablaResultado.ItemsSource;
                DgActual = tablaResultado;
            }
            switch (value.ToLower())
            {
                case "emisor":
                    IComparer<VotoSimplificadoTO> comparador = new VotoSimplificadoTOComp();
                    this.muestraActual.Sort(comparador);
                    DgActual.ItemsSource = null;
                    DgActual.ItemsSource = muestraActual;
                    break;
                case "locabr":
                    comparador = new VotoSimplificadoConsecIndxTOComp();
                    this.muestraActual.Sort(comparador);
                    DgActual.ItemsSource = null;
                    DgActual.ItemsSource = muestraActual;
                    break;
                case "ius":
                    comparador = new VotoSimplificadoIdTOComp();
                    this.muestraActual.Sort(comparador);
                    DgActual.ItemsSource = null;
                    DgActual.ItemsSource = muestraActual;
                    break;
                case "rubro":
                    comparador = new VotoSimplificadoRubroTOComp();
                    this.muestraActual.Sort(comparador);
                    DgActual.ItemsSource = null;
                    DgActual.ItemsSource = muestraActual;
                    break;
            }
            DgActual.SelectedIndex = 0;
            ordenarPor = value;
        }

        private string getOrdenarPor()
        {
            return this.ordenarPor;
        }
        #region BackgroundWorker Events

        //This event is fired on the background thread, and is where you would do all your work 
        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {            
            impreso = false;
            List<VotoSimplificadoTO> resultado = null;
            DocumentoListadoVoto generador = new DocumentoListadoVoto(TablaResultado.Items, worker);
            documentoImpresion = generador;
            resultado = generador.ListaImpresion;
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
                TbcDataGrids.Visibility = Visibility.Visible;
                impresionViewer.Visibility = Visibility.Hidden;
            }
            else if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message, "Error al generar el documento", MessageBoxButton.OK, MessageBoxImage.Error);
                TbcDataGrids.Visibility = Visibility.Visible;
                impresionViewer.Visibility = Visibility.Hidden;
                imprimePapel.Visibility = Visibility.Hidden;
            }
            else
            {
                //Package pack = PackageStore.GetPackage(new Uri("pack://temp.xps"));
                //PackagePart documento =  pack.GetPart(new Uri("/temp.xps", UriKind.Relative));
                FlowDocument resultado = DocumentoListadoVoto.generaDocumento((List<VotoSimplificadoTO>)e.Result);// (FlowDocument)XamlReader.Load(documento.GetStream());
                impresionViewer.Document = resultado as IDocumentPaginatorSource;
                impresionViewer.Visibility = Visibility.Visible;//Means everything went as expected 
                impresionViewer.Background = Brushes.White;
                resultado.Background = Brushes.White;
                MostrarImpresionPrel();
            }
            impreso = true;
            Esperar.Visibility = Visibility.Collapsed;
            Cursor = Cursors.Arrow;
        }
        #endregion
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
        private void MostrarImpresionPrel()
        {
            BtnDisminuyeAlto.Visibility = Visibility.Hidden;
            BtnAumentaAlto.Visibility = Visibility.Hidden;
            BtnGuardar.Visibility = Visibility.Hidden;
            verClasificacion = Clasificacion.Visibility == Visibility.Visible;
            verPnlOrdenar = pnlOrdenar.Visibility == Visibility.Visible;
            verOriginal = BtnOriginal.Visibility == Visibility.Visible;
            verBtnClasificacion = btnClasificacion.Visibility == Visibility.Visible;
            BtnOriginal.Visibility = Visibility.Hidden;
            Clasificacion.Visibility = Visibility.Hidden;
            pnlOrdenar.Visibility = Visibility.Hidden;
            Titulo.Visibility = Visibility.Hidden;
            RegistrosLabel.Visibility = Visibility.Hidden;
            //Expresion.Visibility = Visibility.Hidden;
            //idExpresion.Visibility = Visibility.Hidden;
            TbcDataGrids.Visibility = Visibility.Hidden;
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
            btnOrdenar.Visibility = Visibility.Hidden;
            btnClasificacion.Visibility = Visibility.Hidden;
            BtnVisualizar.Visibility = Visibility.Hidden;
        }
        private void EsconderImpresionPrel()
        {
            BtnDisminuyeAlto.Visibility = Visibility.Visible;
            BtnAumentaAlto.Visibility = Visibility.Visible;
            BtnOriginal.Visibility = verOriginal ? Visibility.Visible : Visibility.Hidden;
            if (!BrowserInteropHelper.IsBrowserHosted) BtnGuardar.Visibility = Visibility.Visible;
            if (verPnlOrdenar) pnlOrdenar.Visibility = Visibility.Visible;
            if (verClasificacion) Clasificacion.Visibility = Visibility.Visible;
            Titulo.Visibility = Visibility.Visible;
            imprimir.ToolTip = Constants.VistaPreliminar;
            imprimir.Visibility = Visibility.Visible;
            salir.Visibility = Visibility.Visible;
            BtnTache.Visibility = Visibility.Hidden;
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
            TbcDataGrids.Visibility = Visibility.Visible;
            impresionViewer.Visibility = Visibility.Hidden;
            imprimePapel.Visibility = Visibility.Hidden;
            btnOrdenar.Visibility = Visibility.Visible;
            if(verBtnClasificacion) btnClasificacion.Visibility = Visibility.Visible;
            BtnVisualizar.Visibility = Visibility.Visible;
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
            
            if ((tablaResultado.SelectedItem != null)
                ||(tablaResultadoMinoritario.SelectedItem != null)
                ||(tablaResultadoConcurrentes.SelectedItem != null)
                ||(tablaResultadoTodos.SelectedItem != null)
                ||(tablaResultadoParticulares.SelectedItem != null))
            {
                tablaResultado_MouseDoubleClick(sender, null);
            }
        }
        private void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (!BrowserInteropHelper.IsBrowserHosted)
            {
                imprimir_onClick(sender, e);
                while (!impreso)
                {
                    System.Windows.Forms.Application.DoEvents();
                }
                Microsoft.Win32.SaveFileDialog guardaEn = new Microsoft.Win32.SaveFileDialog();
                guardaEn.DefaultExt = ".rtf";
                guardaEn.Title = "Guardar listado de votos";
                guardaEn.Filter = "Archivos de Texto Enriquecido|*.rtf";
                guardaEn.AddExtension = true;
                EsconderImpresionPrel();
                if ((bool)guardaEn.ShowDialog())
                {
                    FlowDocument documentoImprimir = DocumentoListadoVoto.generaDocumento(documentoImpresion.ListaImpresion);
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

        private void TbiConcurrentes_GotFocus(object sender, RoutedEventArgs e)
        {
            if (TbiParticulares.IsSelected)
            {
                muestraActual = (List<VotoSimplificadoTO>)tablaResultadoParticulares.ItemsSource;
                btnClasificacion.Visibility = Visibility.Hidden;
                TablaResultado = tablaResultadoParticulares;
                if (tablaResultadoParticulares.SelectedIndex < 0) tablaResultadoParticulares.SelectedIndex = 0;
            }
            else if (TbiConcurrentes.IsSelected)
            {
                muestraActual = (List<VotoSimplificadoTO>)tablaResultadoConcurrentes.ItemsSource;
                btnClasificacion.Visibility = Visibility.Hidden;
                TablaResultado = tablaResultadoConcurrentes;
                if (tablaResultadoConcurrentes.SelectedIndex < 0) tablaResultadoConcurrentes.SelectedIndex = 0;
            }
            else if (TbiMinoria.IsSelected)
            {
                muestraActual = (List<VotoSimplificadoTO>)tablaResultadoMinoritario.ItemsSource;
                btnClasificacion.Visibility = Visibility.Hidden;
                TablaResultado = tablaResultadoMinoritario;
                if (tablaResultadoMinoritario.SelectedIndex < 0) tablaResultadoMinoritario.SelectedIndex = 0;
            }
            else if (TbiTodos.IsSelected)
            {
                muestraActual = (List<VotoSimplificadoTO>)tablaResultadoTodos.ItemsSource;
                btnClasificacion.Visibility = Visibility.Hidden;
                TablaResultado = tablaResultadoTodos;
                if (tablaResultadoTodos.SelectedIndex < 0) tablaResultadoTodos.SelectedIndex = 0;
            }
            BtnOriginal.Visibility = Visibility.Hidden;
            verClasificacion = false;
            verOriginal = false;
            tablaResultados_PropertyChanged(null, null);
        }

        private void TbiOtros_GotFocus(object sender, RoutedEventArgs e)
        {
            btnClasificacion.Visibility = Visibility.Visible;
            BtnOriginal.Visibility = Visibility.Visible;
            verClasificacion = true;
            verOriginal = true;
            TablaResultado = tablaResultado;
            muestraActual = (List<VotoSimplificadoTO>)tablaResultado.ItemsSource;
            tablaResultados_PropertyChanged(null, null);
        }

        private void BtnOriginal_Click(object sender, RoutedEventArgs e)
        {
            tablaResultado.ItemsSource = resultadoOriginalOtros;
            if (resultadoOriginalOtros.Count > 0) tablaResultado.SelectedIndex = 0;
        }
    }
}
