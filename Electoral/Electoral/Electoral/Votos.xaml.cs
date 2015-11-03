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
    /// Interaction logic for Votos.xaml
    /// </summary>
    public partial class Votos : Page
    {
        public Page Back { get; set; }
        public IVotoElectoralController Controlador { get; set; }

        public Votos()
        {
            InitializeComponent();
            Controlador = new VotoElectoralControllerImpl(this);
        }

        public Votos(Int32 Id)
        {
            InitializeComponent();
            Controlador = new VotoElectoralControllerImpl(this, Id);
        }

        public Votos(DataGridControl tablaResultados, BusquedaTO busqueda)
        {
            InitializeComponent();
            Controlador = new VotoElectoralControllerImpl(this, tablaResultados, busqueda);
        }

        private void Guardar_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            Controlador.GuardarClic();
        }

        private void PortaPapeles_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            Controlador.PortaPapelesClic();
        }

        private void FontSelec_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            Controlador.AumentaFontClic();
        }

        private void FontMenor_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            Controlador.DisminuyeFontClic();
        }

        private void Imprimir_MouseButtonDown(object sender, RoutedEventArgs e)
        {
            Controlador.ImprimirClic();
        }

        private void tesis_MouseButtonDown(object sender, RoutedEventArgs e)
        {
            Controlador.TesisClic();
        }

        private void ejecutoria_MouseButtonDown(object sender, RoutedEventArgs e)
        {
            Controlador.EjecutoriaClic();
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

        private void regNum_KeyDown(object sender, KeyEventArgs e)
        {
            Controlador.RegNumTecla(e);
        }

        private void IrBoton_Click(object sender, RoutedEventArgs e)
        {
            Controlador.IrClic();
        }

        private void Salir_MouseButtonDown(object sender, RoutedEventArgs e)
        {
            Controlador.SalirClic();
        }

        private void imprimePapel_ImageFailed(object sender, ExceptionRoutedEventArgs e)
        {
            Controlador.ImprimePapelClic();
        }

        private void BtnTache_Click(object sender, RoutedEventArgs e)
        {
            Controlador.TacheClic();
        }

        private void textoAbuscar_KeyDown(object sender, KeyEventArgs e)
        {
            Controlador.TextoBuscarTecla(e);
        }

        private void textoAbuscar_TextChanged(object sender, TextChangedEventArgs e)
        {
            Controlador.TextoBuscarCambio(e);
        }

        private void Buscar_Click(object sender, RoutedEventArgs e)
        {
            Controlador.BuscaClic();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            contenidoTexto.FontSize = CalculosPropiedadesGlobales.FontSize;
        }

        private void docCompletoImage_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            Controlador.DocumentoCompletoClic();
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

        private void contenidoTexto_KeyDown(object sender, KeyEventArgs e)
        {
            Controlador.ContenidoTextoTecla(e);
        }

        private void contenidoTexto_Copying(object sender, DataObjectCopyingEventArgs e)
        {
            Controlador.ContenidoTextoCopia(sender, e);
        }

        private void imprimePapel_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            Controlador.ImprimePapelClic();
        }

        private void tabControl1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Controlador.TabControlChanged();
        }
    }
}
