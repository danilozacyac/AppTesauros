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
    /// Interaction logic for Ejecutoria.xaml
    /// </summary>
    public partial class Ejecutoria : Page
    {
        public Page Back { get; set; }
        public IEjecutoriaElectoral Controlador { get; set; }

        public Ejecutoria()
        {
            InitializeComponent();
            Controlador = new EjecutoriaElectoralImpl(this);
        }

        public Ejecutoria(Int32 Id)
        {
            InitializeComponent();
            Controlador = new EjecutoriaElectoralImpl(this, Id);
        }

        public Ejecutoria(DataGridControl dg, BusquedaTO busqueda)
        {
            InitializeComponent();
            Controlador = new EjecutoriaElectoralImpl(this, dg, busqueda);
        }
        internal void ActualizaRangoMarcado(int inicial, int FinRango)
        {
            Controlador.ActualizaRango(inicial, FinRango);
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Controlador.IniciaPagina();
        }

        private void Guardar_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            Controlador.GuardarClic();
        }

        private void Portapapeles_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            Controlador.PortapapelesClic();
        }

        private void FontMayor_MouseLeftButtonDown(object sender, RoutedEventArgs e)
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
        }

        private void Marcar_MouseLeave(object sender, MouseEventArgs e)
        {
            
        }

        private void Marcar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Controlador.MarcarClic();
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
            Controlador.RegNumLetra(e);
        }

        private void IrBoton_Click(object sender, RoutedEventArgs e)
        {
            Controlador.IrClic();
        }

        private void Salir_MouseButtonDown(object sender, RoutedEventArgs e)
        {
            Controlador.SalirClic();
        }

        private void Imprimir_MouseButtonDown(object sender, RoutedEventArgs e)
        {
            Controlador.ImprimirClic();
        }

        private void docCompletoImage_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            Controlador.DocumentoCompletoClic();
        }

        private void TablasAnexos_Click(object sender, RoutedEventArgs e)
        {
            Controlador.AnexosClic();
        }

        private void tesis_MouseButtonDown(object sender, RoutedEventArgs e)
        {
            Controlador.TesisClic();
        }

        private void voto_MouseButtonDown(object sender, RoutedEventArgs e)
        {
            Controlador.VotoClic();
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

        private void imprimePapel_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            Controlador.ImprimePapelClic();
        }

        private void BtnTache_Click(object sender, RoutedEventArgs e)
        {
            Controlador.TacheClic();
        }

        private void contenidoTexto_Copying(object sender, DataObjectCopyingEventArgs e)
        {
            Controlador.ContenidoTextoCopy(e);
        }

        private void TextoABuscar_KeyDown(object sender, KeyEventArgs e)
        {
            Controlador.TextoABuscarTecla(e);
        }

        private void BuscarImage_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            Controlador.BuscarClic();
        }

        private void contenidoTexto_KeyDown(object sender, KeyEventArgs e)
        {
            Controlador.ContenidoTextoTecla();
        }

        private void TextoABuscar_TextChanged(object sender, TextChangedEventArgs e)
        {
            Controlador.TextoBuscarTecla(e);
        }

        private void tabControl1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Controlador.TabControlChanged();
        }
    }
}
