using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using mx.gob.scjn.electoral.Controller;
using mx.gob.scjn.electoral.Controller.Impl;
using mx.gob.scjn.ius_common.TO;
using mx.gob.scjn.ius_common.gui.utils;

namespace mx.gob.scjn.electoral
{
    /// <summary>
    /// Interaction logic for TablaResultados.xaml
    /// </summary>
    public partial class TablaResultados : Page
    {
        ITablaResultadosController Controlador { get; set; }
        public Page Back { get; set; }
        private BusquedaTO busqueda { get; set; }
        /// <summary>
        ///     propiedad que define el alto de las celdas
        /// </summary>
        /// <remarks>
        ///     
        /// </remarks>
        public static readonly DependencyProperty RowHeightProperty =
            DependencyProperty.Register("RowHeight", typeof(int), typeof(TablaResultados), new UIPropertyMetadata(99));

        public TablaResultados()
        {
            InitializeComponent();
            Controlador = new TablaResultadosControllerImpl(this);
        }

        public TablaResultados(BusquedaTO Busqueda)
        {
            busqueda = Busqueda;
            InitializeComponent();
            Controlador = new TablaResultadosControllerImpl(this, busqueda);
        }
        public TablaResultados(List<int> identificadores)
        {
            InitializeComponent();
            Controlador = new TablaResultadosControllerImpl(this, identificadores);
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

        private void imprimir_onClick(object sender, RoutedEventArgs e)
        {
            Controlador.ImprimirClic();
        }

        private void Guardar_Click(object sender, RoutedEventArgs e)
        {
            Controlador.GuardarClic();
        }

        private void salir_onClick(object sender, RoutedEventArgs e)
        {
            Controlador.SalirClic();
        }

        private void Filtros_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void tablaResultados_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (Controlador != null)
            {
                Controlador.CambioSeleccion();
            }
        }
        private void tablaResultados_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Controlador.TablaDobleClic();
        }

        private void imprimePapel_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            Controlador.ImprimePapelClic();
        }

        private void BtnTache_Click(object sender, RoutedEventArgs e)
        {
            Controlador.BtnTacheClic();
        }

        private void inicio_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            Controlador.InicioClic();
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

        private void btnIrA_Click(object sender, RoutedEventArgs e)
        {
            Controlador.IrAClic();
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

        private void BtnVisualizar_Click(object sender, RoutedEventArgs e)
        {
            if (tablaResultados.SelectedItem != null)
            {
                tablaResultados_MouseDoubleClick(sender, null);
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
            }
        }
    }
}
