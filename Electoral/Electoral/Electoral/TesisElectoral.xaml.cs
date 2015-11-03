using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Xceed.Wpf.DataGrid;
using mx.gob.scjn.electoral.Controller;
using mx.gob.scjn.electoral.Controller.Impl;
using mx.gob.scjn.ius_common.TO;

namespace mx.gob.scjn.electoral
{
    /// <summary>
    /// Interaction logic for TesisElectoral.xaml
    /// </summary>
    public partial class TesisElectoral : Page
    {
        public Page Back { get; set; }
        public ITesisElectoralController Controlador { get; set; }
        public TesisElectoral()
        {
            InitializeComponent();
            Controlador = new TesisElectoralControllerImpl(this);
        }
        public TesisElectoral(DataGridControl DataGrid, BusquedaTO busqueda)
        {
            InitializeComponent();
            Controlador = new TesisElectoralControllerImpl(this, DataGrid, busqueda);
        }
        public TesisElectoral(TesisTO Documento)
        {
            InitializeComponent();
            Controlador = new TesisElectoralControllerImpl(this, Documento);
        }
        internal void ActualizaRangoMarcado(int inicial, int FinRango)
        {
            Controlador.ActualizaRangoMarcado(inicial, FinRango);
        }

        private void PortaPapeles_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            Controlador.PortaPapelesClic();
        }

        private void Guardar_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            Controlador.GuardarClic();
        }

        private void FontSelec_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            Controlador.FontMayorClic();
        }

        private void FontMenor_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            Controlador.FontMenorClic();
        }

        private void MarcarTodo_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            Controlador.MarcarTodoClic();
        }

        private void Desmarcar_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            Controlador.DesmarcarClic();
        }

        private void Marcar_MouseEnter(object sender, MouseEventArgs e)
        {
            Controlador.MarcarEnter();
        }

        private void Marcar_MouseLeave(object sender, MouseEventArgs e)
        {
            Controlador.MarcarSalir();
        }

        private void Marcar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Controlador.MarcarClic();
        }

        private void BtnConcordancia_Click(object sender, RoutedEventArgs e)
        {
            Controlador.ConcordanciaClic();
        }

        private void Imprimir_MouseButtonDown(object sender, RoutedEventArgs e)
        {
            Controlador.ImprmirClic();
        }

        private void ejecutoria_Click(object sender, RoutedEventArgs e)
        {
            Controlador.EjecutoriaClic();
        }

        private void voto_MouseButtonDown(object sender, RoutedEventArgs e)
        {
            Controlador.VotoClic();
        }

        private void observacionesBot_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            Controlador.ObservacionesClic();
        }

        private void genealogia_MouseButtonDown(object sender, RoutedEventArgs e)
        {
            Controlador.GenealogiaClic();
        }

        private void inicioLista_MouseButtonDown(object sender, RoutedEventArgs e)
        {
            Controlador.InicioListaClic();
        }

        private void anteriorLista_MouseButtonDown(object sender, RoutedEventArgs e)
        {
            Controlador.AnteriorListaClic();
        }

        private void siguienteLista_MouseButtonDown(object sender, RoutedEventArgs e)
        {
            Controlador.SiguienteListaClic();
        }

        private void ultimoLista_MouseButtonDown(object sender, RoutedEventArgs e)
        {
            Controlador.UltimoListaClic();
        }

        private void IrBoton_Click(object sender, RoutedEventArgs e)
        {
            Controlador.IrClic();
        }

        private void Salir_MouseButtonDown(object sender, RoutedEventArgs e)
        {
            Controlador.SalirClic();
        }

        private void regNum_KeyDown(object sender, KeyEventArgs e)
        {
            Controlador.RegNumTecla(e);
        }

        private void contenidoTexto_Copying(object sender, DataObjectCopyingEventArgs e)
        {
            Controlador.ContenidoTextoCopia(sender,e);
        }

        private void contenidoTexto_SelectionChanged(object sender, RoutedEventArgs e)
        {
            Controlador.ContenidoTextoSeleccion();
        }

        private void contenidoTexto_KeyDown(object sender, KeyEventArgs e)
        {
            Controlador.ContenidoTextoTecla(e);
        }

        private void textoAbuscar_TextChanged(object sender, TextChangedEventArgs e)
        {
            Controlador.TextoBuscarCambio();
        }

        private void textoAbuscar_KeyDown(object sender, KeyEventArgs e)
        {
            Controlador.TextoBuscarTecla(e);
        }

        private void Buscar_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            Controlador.BuscarClic();
        }

        private void imprimePapel_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            Controlador.ImprimePapelClic();
        }

        private void BtnTache_Click(object sender, RoutedEventArgs e)
        {
            Controlador.BtnTacheClic();
        }

        private void TabDoc_Selected(object sender, RoutedEventArgs e)
        {

        }
    }
}
