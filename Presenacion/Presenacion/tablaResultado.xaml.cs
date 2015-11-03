using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using IUS.Indices;
using mx.gob.scjn.ius_common.TO;
using mx.gob.scjn.ius_common.gui.Controller;
using mx.gob.scjn.ius_common.gui.gui.utilities;
using mx.gob.scjn.ius_common.gui.impresion;
using mx.gob.scjn.ius_common.gui.utils;

namespace IUS
{
    /// <summary>
    /// Lógica de interacción para tablaResultado.xaml
    /// </summary>
    /// 

    public partial class tablaResultado : Page
    {
        #region properties
        /// <summary>
        /// Variable que nos sirve para generar el ordenamiento.
        /// </summary>
        public String OrdenarPor { get { return this.getOrdernarPor(); } set { Controlador.OrdenarPor=value; } }
        private String ordenarPor;
        public Inicial PaginaIndices { get; set; }
       
        /// <summary>
        ///     propiedad que define el alto de las celdas
        /// </summary>
        /// <remarks>
        ///     
        /// </remarks>
        public static readonly DependencyProperty RowHeightProperty =
            DependencyProperty.Register("RowHeight", typeof(int), typeof(tablaResultado), new UIPropertyMetadata(99));
        ///<summary>
        /// Conjunto de los filtros solicitados por el usuario.
        /// </summary>
        /// 
        public List<FiltrosTO> FiltrosSolicitados { get; set; }
        /// <summary>
        /// Lista con las tesis que forman parte de nuestro resultado.
        /// </summary>
        public List<TesisSimplificadaTO> resultadoTesis;
        /// <summary>
        /// Muestra la lista actual de tesis simplificadas filtrada
        /// por algún valor de tpoTesis.
        /// </summary>
        protected List<TesisSimplificadaTO> muestraActual { get; set; }
        /// <summary>
        /// La busqueda que se solicita a partir del panel.
        /// </summary>
        public BusquedaTO busqueda { get; set; }
        /// <summary>
        /// Busqueda especial.
        /// </summary>
        MostrarPorIusTO BuscaEspecial { get; set; }
        ///<summary>
        ///La pagina a la que hay que regresar.
        /// </summary>
        public Page Back { get; set; }
        /// <summary>
        /// La representación del tipo de búsqueda.
        /// </summary>
        public int tipoBusqueda { get; set; }
        /// <summary>
        ///     Define si se muestran o no los botones de tesis aisladas
        /// </summary>
        /// <remarks>
        ///     
        /// </remarks>
        protected bool aisladasMuestra = false;
        protected bool flechasMuestra = true;
        protected bool jurisMuestra = false;
        protected bool accionesMuestra = false;
        protected bool controversiaMuestra = false;
        protected bool reiteraMuestra = false;
        protected bool contradiccionMuestra = false;
        protected bool impreso { get; set; }
        /// <summary>
        /// El documento para imprimir.
        /// </summary>
        DocumentoListadoTesis documentoImpresion { get; set; }
        private FlowDocument documentoListadoimpresion;
        FlowDocument DocumentoListadoImpresion
        {
            get { return this.documentoListadoimpresion; }
            set { this.documentoImpresion.Documento = value; this.documentoListadoimpresion = value; }
        }
        protected bool verPnlOrdenar { get; set; }
        protected bool verPnlPonentes { get; set; }
        protected bool verPnlAsuntos { get; set; }
        protected bool verPnlAlmacenar { get; set; }
        protected bool verSalir { get; set; }
        protected bool verAcciones { get; set; }
        protected bool verControversias { get; set; }
        protected bool verReiteracion { get; set; }
        protected bool verContradiccion { get; set; }
        protected bool verBtnPonentes { get; set; }
        protected bool verBtnAsuntos { get; set; }
        protected bool verLetras { get; set; }
        private bool almacenaEspecial = false;
        protected MostrarPorIusTO busquedaEspeciales { get; set; }
        protected bool mostrarVP { get; set; }
        protected TesisControl Controlador { get; set; }
        #endregion
        #region constructores

        /// <summary>
        /// Iniciación por omisión en pruebas para el objeto, en
        /// este modo de pruebas los parámetros de la búsqueda son fijos
        /// </summary>
        public tablaResultado()
        {
            if (Application.Current.MainWindow.Content.GetType() == typeof(tablaResultado))
            {
                tablaResultado VentanaActual = (tablaResultado)Application.Current.MainWindow.Content;
                BusquedaTO busquedaActual = VentanaActual.busqueda;
                if ((busquedaActual != null) && (busquedaActual.Palabra != null)
#if STAND_ALONE
                    && (busquedaActual.Palabra.Count > 0))
#else
                    && (busquedaActual.Palabra.Length > 0))
#endif
                {
                    VisibilidadGlobal.verAlmacenar = true;
                }
            }
            VisibilidadGlobal.verAlmacenar = false;
            this.Controlador = new TesisControl(this);
        }
        /// <summary>
        /// Busqueda generada a partir de una búsqueda que se guardó en la Base de Datos.
        /// </summary>
        /// <param name="busqueda">La busqueda a realizar.</param>
        public tablaResultado(BusquedaAlmacenadaTO busqueda)
        {
            InitializeComponent();
            this.Controlador = new TesisControl(this, busqueda);
        }
        /// <summary>
        /// Constructor para la búsqueda por registros
        /// </summary>
        /// <param name="parametrosEspeciales"></param>
        public tablaResultado(List<int> parametrosEspeciales)
        {
            InitializeComponent();
            Controlador = new TesisControl(this, parametrosEspeciales);
        }

        /// <summary>
        /// Constructor utilizado para búsquedas especiales.
        /// </summary>
        /// <param name="parametrosEspeciales">Los parámetros iniciales de la búsqueda</param>
        /// 
        public tablaResultado(MostrarPorIusTO parametrosEspeciales)
        {
            InitializeComponent();
            Controlador = new TesisControl(this, parametrosEspeciales);
        }

        /// <summary>
        /// La búsqueda por panel.
        /// </summary>
        /// <param name="buscar">Los parámetros del panel</param>

        public tablaResultado(BusquedaTO buscar)
        {
            InitializeComponent();
            Controlador = new TesisControl(this, buscar);
        }        
        #endregion
        private void aisladas_MouseEnter(object sender, MouseEventArgs e)
        {
            Image imagenNueva = new Image();
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri("/General;component/images/Aisladas2.png", UriKind.Relative);
            bitmap.EndInit();
            this.aisladas.Source = bitmap;
        }

        private void aisladas_MouseLeave(object sender, MouseEventArgs e)
        {
            Image imagenNueva = new Image();
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri("/General;component/images/Aisladas.png", UriKind.Relative);
            bitmap.EndInit();
            this.aisladas.Source = bitmap;
        }
        /// <summary>
        /// Filtra la búsqueda actual en base a que sean puras tesis aisladas.
        /// </summary>
        /// <param name="sender">Mouse</param>
        /// <param name="e">Argumentos del mouse</param>
        private void aisladas_onClick(object sender, MouseButtonEventArgs e)
        {
            Controlador.AisladasClic();
        }
        /// <summary>
        /// Genera la vista previa del documento a imprimir.
        /// </summary>
        /// <param name="sender">El Mouse</param>
        /// <param name="e">El evento del click</param>
        private void imprimir_onClick(object sender, RoutedEventArgs e)
        {
            Controlador.ImprimesionPreliminarClic();
        }

        /// <summary>
        /// Regresa a la búsqueda original.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void original_onClick(object sender, RoutedEventArgs e)
        {
            Controlador.OriginalClic();
        }

        private void jurisprudencia_MouseEnter(object sender, MouseEventArgs e)
        {
            Image imagenNueva = new Image();
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri("/General;component/images/jurisprudencia-2.png", UriKind.Relative);
            bitmap.EndInit();
            this.jurisprudencia.Source = bitmap;
        }
        private void jurisprudencia_MouseLeave(object sender, MouseEventArgs e)
        {
            Image imagenNueva = new Image();
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri("/General;component/images/jurisprudencia-1.png", UriKind.Relative);
            bitmap.EndInit();
            this.jurisprudencia.Source = bitmap;
        }
        /// <summary>
        /// Genera una búsqueda por jurisprudencia.
        /// </summary>
        /// <param name="sender">El mouse</param>
        /// <param name="e">El click</param>
        private void jurisprudencia_onClick(object sender, MouseButtonEventArgs e)
        {
            Controlador.JurisprudenciaClic();
        }

        private void contradiccion_MouseEnter(object sender, MouseEventArgs e)
        {
            Image imagenNueva = new Image();
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri("/General;component/images/CONTRADICCION2.png", UriKind.Relative);
            bitmap.EndInit();
            this.contradiccion.Source = bitmap;
        }
        private void contradiccion_MouseLeave(object sender, MouseEventArgs e)
        {
            Image imagenNueva = new Image();
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri("/General;component/images/contradiccion1.png", UriKind.Relative);
            bitmap.EndInit();
            this.contradiccion.Source = bitmap;
        }
        /// <summary>
        /// Genera la búsqueda por contradiccion.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void contradiccion_onClick(object sender, MouseButtonEventArgs e)
        {
            Controlador.ContradiccionClic();
        }

        private void acciones_MouseEnter(object sender, MouseEventArgs e)
        {
            Image imagenNueva = new Image();
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri("/General;component/images/ACCIONES2.png", UriKind.Relative);
            bitmap.EndInit();
            this.acciones.Source = bitmap;
        }
        private void acciones_MouseLeave(object sender, MouseEventArgs e)
        {
            Image imagenNueva = new Image();
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri("/General;component/images/ACCIONES.png", UriKind.Relative);
            bitmap.EndInit();
            this.acciones.Source = bitmap;
        }
        /// <summary>
        /// Genera la búsqueda de acciones de inconstitucionalidad.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void acciones_onClick(object sender, MouseButtonEventArgs e)
        {
            Controlador.AccionesClic();
        }

        private void reiteraciones_MouseEnter(object sender, MouseEventArgs e)
        {
            Image imagenNueva = new Image();
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri("/General;component/images/REITERACIONES2.png", UriKind.Relative);
            bitmap.EndInit();
            this.reiteraciones.Source = bitmap;
        }
        private void reiteraciones_MouseLeave(object sender, MouseEventArgs e)
        {
            Image imagenNueva = new Image();
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri("/General;component/images/REITERACIONES.png", UriKind.Relative);
            bitmap.EndInit();
            this.reiteraciones.Source = bitmap;
        }
        /// <summary>
        /// Genera la búsqueda por reiteración de tesis.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
        private void reiteraciones_onClick(object sender, MouseButtonEventArgs e)
        {
            Controlador.ReiteracionesClic();
        }
        private void controversias_MouseEnter(object sender, MouseEventArgs e)
        {
            Image imagenNueva = new Image();
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri("/General;component/images/CONTROVERSIAS2.png", UriKind.Relative);
            bitmap.EndInit();
            this.controversias.Source = bitmap;
        }

        private void controvercias_MouseLeave(object sender, MouseEventArgs e)
        {
            Image imagenNueva = new Image();
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri("/General;component/images/CONTROVERSIAS.png", UriKind.Relative);
            bitmap.EndInit();
            this.controversias.Source = bitmap;
        }
        /// <summary>
        /// Genera la búsqueda de Controversias constitucionales.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void controversias_onClick(object sender, MouseButtonEventArgs e)
        {
            Controlador.ControversiasClic();
        }

        private void salir_onClick(object sender, RoutedEventArgs e)
        {
            Controlador.End();
            Controlador = null;
            if (Back == null)
            {
                this.NavigationService.GoBack();
                this.tablaResultados.ItemsSource = null;
                this.resultadoTesis = null;
                this.muestraActual = null;
            }
            else
            {
                this.NavigationService.Navigate(Back);
                this.NavigationService.RemoveBackEntry();
            }
            this.Esperar.KeyDown -= Esperar_KeyDown;
        }

        private void tablaResultados_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Controlador.ResultadosDobleClic();
        }


        private void imprimePapel_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            Controlador.Imprime();
        }

        private void inicio_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            tablaResultados.SelectedIndex = 0;
            tablaResultados.BringItemIntoView(tablaResultados.SelectedItem);

        }

        private void anterior_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            if (tablaResultados.SelectedIndex > 0)
            {
                tablaResultados.SelectedIndex--;
            }
            tablaResultados.BringItemIntoView(tablaResultados.SelectedItem);
        }


        private void siguiente_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            if (tablaResultados.Items.Count > tablaResultados.SelectedIndex)
            {
                tablaResultados.SelectedIndex++;
            }
            tablaResultados.BringItemIntoView(tablaResultados.SelectedItem);
        }


        private void final_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            tablaResultados.SelectedIndex = tablaResultados.Items.Count - 1;
            tablaResultados.BringItemIntoView(tablaResultados.SelectedItem);
        }

        private void tablaResultados_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            int registroActual = tablaResultados.SelectedIndex + 1;
            RegistrosLabel.Content = registroActual + " de " + tablaResultados.Items.Count;
        }

        private void btnIrA_Click(object sender, RoutedEventArgs e)
        {
            Controlador.IrAClic();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (Controlador != null)
            {
                Controlador.Habilita();
            }
            if (tablaResultados.SelectedItem != null)
            {
                tablaResultados.BringItemIntoView(tablaResultados.SelectedItem);
            }
            if (Application.Current.MainWindow.Content != this)
            {
                salir.Visibility = Visibility.Collapsed;
            }
        }


        private void OrdenarPor_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            Controlador.OrdenarPorClic();
        }

        private string getOrdernarPor()
        {
            return this.ordenarPor;
        }

        private void Letra_Click(object sender, RoutedEventArgs ev)
        {
            Controlador.LetrasClic((Button) sender);
        }

       
        public void ActualizaPonente(List<PonenteTO> resultado, int TipoPonente)
        {
            if (TipoPonente == -1)
            {
                Controlador.ActualizaPonente(resultado);
            }
            else
            {
                Controlador.ActualizaPonente(resultado, TipoPonente);
            }
        }

        private void ImgPonentes_MouseEnter(object sender, MouseEventArgs e)
        {
            Image imagenNueva = new Image();
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri("/General;component/images/Filtrar-PONENCIA2.png", UriKind.Relative);
            bitmap.EndInit();
            this.ImgPonentes.Source = bitmap;
        }

        private void ImgPonentes_MouseLeave(object sender, MouseEventArgs e)
        {
            Image imagenNueva = new Image();
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri("/General;component/images/Filtrar-PONENCIA1.png", UriKind.Relative);
            bitmap.EndInit();
            this.ImgPonentes.Source = bitmap;
        }

        private void ImgPonentes_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Controlador.PonentesClic();
        }

        private void ImgAsuntos_MouseEnter(object sender, MouseEventArgs e)
        {
            Image imagenNueva = new Image();
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri("/General;component/images/Filtrar-ASUNTO2.png", UriKind.Relative);
            bitmap.EndInit();
            this.ImgAsuntos.Source = bitmap;
        }

        private void ImgAsuntos_MouseLeave(object sender, MouseEventArgs e)
        {
            Image imagenNueva = new Image();
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri("/General;component/images/Filtrar-ASUNTO1.png", UriKind.Relative);
            bitmap.EndInit();
            this.ImgAsuntos.Source = bitmap;
        }

        private void ImgAsuntos_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Controlador.AsuntosClic();
        }

        internal void ActualizaAsunto(List<AsuntoTO> resultado)
        {
            Controlador.ActualizaAsunto(resultado);
        }
        /// <summary>
        /// Guarda el listado en un archivo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Guardar_Click(object sender, RoutedEventArgs e)
        {
            Controlador.GuardarClic();
        }

        private void Almacenar_Click(object sender, RoutedEventArgs e)
        {
            Controlador.AlmacenarClic();
        }

        private void Filtros_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void IrANum_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                this.btnIrA_Click(sender, null);
            }
            if (e.Key == Key.Decimal)
            {
                e.Handled = true;
            }
        }

        private void BtnTache_Click(object sender, RoutedEventArgs e)
        {
            Controlador.BtnTacheClic();
        }

        private void BtnVisualizar_Click(object sender, RoutedEventArgs e)
        {
            if (tablaResultados.SelectedItem != null)
            {
                tablaResultados_MouseDoubleClick(sender, null);
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

        private void Esperar_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key.Equals(Key.Escape))
            {
                Controlador.worker.CancelAsync();
            }
        }

        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult res = MessageBox.Show(Mensajes.MENSAJE_CANCELAR, Mensajes.TITULO_CANCELAR,
                        MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
            if (res.Equals(MessageBoxResult.Yes))
            {
                Controlador.worker.CancelAsync();
                DocumentoListadoTesis.cancelado = true;
            }
        }

        internal void ActualizaListaPonente(int Tipo)
        {
            Controlador.ActualizaListaPonente(Tipo);
        }
    }
}
