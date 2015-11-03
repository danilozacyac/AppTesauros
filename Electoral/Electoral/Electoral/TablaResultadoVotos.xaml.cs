using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using mx.gob.scjn.electoral.Controller;
using mx.gob.scjn.electoral.Controller.Impl;
using mx.gob.scjn.ius_common.TO;

namespace mx.gob.scjn.electoral
{
    /// <summary>
    /// Interaction logic for TablResultadoVotos.xaml
    /// </summary>
    public partial class TablaResultadoVotos : Page
    {
        public Page Back {get;set;}
        public ITablaResultadoVotoElectoral Controlador { get; set; }
        public static readonly DependencyProperty RowHeightProperty =
            DependencyProperty.Register("RowHeight", typeof(int), typeof(TablaResultadoVotos), new UIPropertyMetadata(99));

        public TablaResultadoVotos()
        {
            InitializeComponent();
            Controlador = new TablaResultadoVotoElectoralControllerImpl(this);
        }
        public TablaResultadoVotos(BusquedaTO busqueda)
        {
            InitializeComponent();
            Controlador = new TablaResultadoVotoElectoralControllerImpl(this, busqueda);
        }

        public TablaResultadoVotos(int[] identificadores)
        {
            InitializeComponent();
            Controlador = new TablaResultadoVotoElectoralControllerImpl(this, identificadores);
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

        private void inicio_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            Controlador.InicioClic();
        }

        private void anterior_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            Controlador.AnteriorClic();
        }

        private void siguiente_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            Controlador.SiguienteClic();
        }

        private void final_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            Controlador.FinalClic();
        }

        private void IrANum_KeyDown(object sender, KeyEventArgs e)
        {
            Controlador.IrTecla(e);
        }

        private void btnIrA_Click(object sender, RoutedEventArgs e)
        {
            Controlador.IrClic();
        }

        private void imprimir_onClick(object sender, RoutedEventArgs e)
        {
            Controlador.ImprimirClic();
        }

        private void salir_onClick(object sender, RoutedEventArgs e)
        {
            Controlador.SalirClic();
        }

        private void btnOrdenar_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            
        }

        private void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            Controlador.GuardarClic();
        }

        private void BtnVisualizar_Click(object sender, RoutedEventArgs e)
        {
            Controlador.VisualizarClic();
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

        private void tablaResultados_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (Controlador != null)
            {
                Controlador.CambioRegistro();
            }
        }

        private void tablaResultado_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Controlador.VisualizarClic();
        }

        private void imprimePapel_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            Controlador.ImprimePapelClic();
        }

        private void BtnTache_Click(object sender, RoutedEventArgs e)
        {
            Controlador.TacheClic();
        }
    }
}
