using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Xceed.Wpf.DataGrid;
using mx.gob.scjn.electoral.Controller;
using mx.gob.scjn.electoral.Controller.Impl;
using mx.gob.scjn.ius_common.TO;
using mx.gob.scjn.ius_common.gui.utils;

namespace mx.gob.scjn.electoral
{
    /// <summary>
    /// Interaction logic for Acuerdos.xaml
    /// </summary>
    public partial class Acuerdos : Page
    {
        public Page Back { get; set; }
        public IAcuerdosController Controlador { get; set; }

        public Acuerdos()
        {
            InitializeComponent();
            Controlador = new AcuerdosControllerImpl(this);
        }

        public Acuerdos(Int32 Id)
        {
            InitializeComponent();
            Controlador = new AcuerdosControllerImpl(this, Id);
        }

        public Acuerdos(DataGridControl records, BusquedaTO busqueda)
        {
            InitializeComponent();
            Controlador = new AcuerdosControllerImpl(this, records, busqueda);
        }

        internal void ActualizaRangoMarcado(int inicial, int FinRango)
        {
            throw new NotImplementedException();
        }

        private void Guardar_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            Controlador.GuardarClic();
        }

        private void PortaPapeles_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            Controlador.PortapapelesClic();
        }

        private void FontMenor_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            Controlador.FontMenorClic();
        }

        private void FontSelec_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            Controlador.FontMayorClic();
        }

        private void MarcarTodo_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {

        }

        private void Desmarcar_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {

        }

        private void Marcar_ImageFailed(object sender, ExceptionRoutedEventArgs e)
        {

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

        private void regNum_KeyDown(object sender, KeyEventArgs e)
        {
            Controlador.RegNumTecla(e);
        }

        private void historial_MouseButtonDown(object sender, RoutedEventArgs e)
        {

        }

        private void Imprimir_MouseButtonDown(object sender, RoutedEventArgs e)
        {
            Controlador.ImprimirClic();
        }

        private void docCompletoImage_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            Controlador.DocumentoCompletoClic();
        }

        private void BtnTablas_Click(object sender, RoutedEventArgs e)
        {

        }

        private void parteInicio_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            Controlador.ParteInicioClic();
        }

        private void parteAnterior_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            Controlador.ParteAnteriorClic();
        }

        private void parteSiguiente_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            Controlador.ParteSiguienteClic();
        }

        private void parteFinal_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            Controlador.ParteFinalClic();
        }

        private void IrBoton_Click(object sender, RoutedEventArgs e)
        {
            Controlador.IrClic();
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
            Controlador.TacheClic();
        }

        private void Salir_MouseButtonDown(object sender, RoutedEventArgs e)
        {
            Controlador.SalirClic();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            contenidoTexto.FontSize = CalculosPropiedadesGlobales.FontSize;
        }

        private void contenidoTexto_KeyDown(object sender, KeyEventArgs e)
        {
            Controlador.ContenidoTextoTecla(e);
        }

        private void contenidoTexto_Copying(object sender, DataObjectCopyingEventArgs e)
        {
            Controlador.ContenidoTextoCopia(sender, e);
        }

        private void tabControl1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Controlador.TabControlChanged();
        }
    }
}
