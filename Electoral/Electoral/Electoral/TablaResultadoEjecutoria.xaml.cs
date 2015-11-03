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
    /// Interaction logic for TablaResultadoEjecutoria.xaml
    /// </summary>
    public partial class TablaResultadoEjecutoria : Page
    {
        public Page Back { get; set; }
        public ITablaResultadoEjecutoriaController Controlador { get; set; }
        public static readonly DependencyProperty RowHeightProperty =
    DependencyProperty.Register("RowHeight", typeof(int), typeof(TablaResultadoEjecutoria), new UIPropertyMetadata(99));

        public TablaResultadoEjecutoria()
        {
            InitializeComponent();
            Controlador = new TablaResultadosEjecutoriaControllerImpl(this);
        }
        public TablaResultadoEjecutoria(BusquedaTO busqueda)
        {
            InitializeComponent();
            Controlador = new TablaResultadosEjecutoriaControllerImpl(this, busqueda);
        }

                /// <summary>
        /// Constructor para la búsqueda por registros.
        /// </summary>
        /// <param name="identificadores">Los registros a desplegar</param>
        public TablaResultadoEjecutoria(int[] identificadores)
        {
            InitializeComponent();
            Controlador = new TablaResultadosEjecutoriaControllerImpl(this, identificadores);
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Controlador.PaginaCargada();
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

        private void Ordena_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            Controlador.OrdenaClic();
        }

        private void IrANum_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Controlador.IrAClic();
            }
        }

        private void btnIrA_Click(object sender, RoutedEventArgs e)
        {
            Controlador.IrAClic();
        }

        private void imprimir_onClick(object sender, RoutedEventArgs e)
        {
            Controlador.ImprimirClic();
        }

        private void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            Controlador.GuardarClic();
        }

        private void BtnVisualizar_Click(object sender, RoutedEventArgs e)
        {
            if (tablaResultado.SelectedItem != null)
            {
                Controlador.TablaResultadoDobleClic();
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

        private void salir_onClick(object sender, RoutedEventArgs e)
        {
            Controlador.SalirClic();
        }

        private void tablaResultados_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (Controlador != null)
            {
                Controlador.TablaResultadoCambio();
            }
        }

        private void tablaResultado_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Controlador.TablaResultadoDobleClic();
        }

        private void imprimePapel_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            Controlador.ImprimePapelClic();
        }

        private void BtnTache_Click(object sender, RoutedEventArgs e)
        {
            Controlador.TacheClic();
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            Controlador.CancelarClic();
        }
    }
}
