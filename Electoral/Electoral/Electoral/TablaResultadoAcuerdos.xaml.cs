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
    /// Interaction logic for TablaResultadoAcuerdos.xaml
    /// </summary>
    public partial class TablaResultadoAcuerdos : Page
    {
        public Page Back { get; set; }
        public ITablaResultadosAcuerdosController Controlador { get; set; }
        public static readonly DependencyProperty RowHeightProperty =
DependencyProperty.Register("RowHeight", typeof(int), typeof(TablaResultadoAcuerdos), new UIPropertyMetadata(99));

        public TablaResultadoAcuerdos()
        {
            InitializeComponent();
            Controlador = new TablaResultadosAcuerdosControllerImpl(this);
        }
        public TablaResultadoAcuerdos(BusquedaTO busqueda)
        {
            InitializeComponent();
            Controlador = new TablaResultadosAcuerdosControllerImpl(this, busqueda);
        }
        public TablaResultadoAcuerdos(int[] identificadores)
        {
            InitializeComponent();
            Controlador = new TablaResultadosAcuerdosControllerImpl(this, identificadores);
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

        private void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            Controlador.GuardarClic();
        }

        private void OrdenarPor_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            Controlador.OrdenarPor();
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

        private void salir_onClick(object sender, RoutedEventArgs e)
        {
            Controlador.SalirClic();
        }

        private void tablaResultado_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (Controlador != null)
            {
                Controlador.TablaResultadoPropertyChanged(sender, e);
            }
        }

        private void imprimePapel_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            Controlador.ImprimePapelClic();
        }

        private void BtnTache_Click(object sender, RoutedEventArgs e)
        {
            Controlador.TacheClic();
        }
        private void tablaResultado_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Controlador.VisualizarClic();
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
