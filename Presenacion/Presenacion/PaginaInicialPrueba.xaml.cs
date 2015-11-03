using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media.Animation;
using IUS.Indices;
using JaStDev.ControlFramework.Controls;
using Microsoft.Win32;
using mx.gob.scjn.directorio.gui;
using mx.gob.scjn.electoral;
using mx.gob.scjn.ius_common.fachade;
using mx.gob.scjn.ius_common.gui;
using mx.gob.scjn.ius_common.gui.Tematica;
using mx.gob.scjn.ius_common.gui.noticia;
using mx.gob.scjn.ius_common.gui.salir;
using mx.gob.scjn.ius_common.gui.utils;
using mx.gob.scjn.ius_common.utils;

namespace IUS
{


    /// <summary>
    /// Interaction logic for PaginaInicial.xaml
    /// </summary>
    public partial class PaginaInicialPrueba : Page
    {
        Help help = new Help();

        public PaginaInicialPrueba()
        {
            InitializeComponent();
#if STAND_ALONE
            Storyboard entrada = (Storyboard)this.Resources["Entrada_Story"];//= (Storyboard) this.Resources["Entrada_Story"];
            FachadaBusquedaTradicional fachada = new FachadaBusquedaTradicional();
            entrada.Begin();
#else
            FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
            Btn_cuadro.Opacity = 0;
            Btn_Regresar_1.Opacity = 0;
            Cabeza_Menu.Opacity = 0;
            Menu_Botones.Opacity = 0;
            Menu_Informacion.Opacity = 0;
            Pie_Menu.Opacity = 0;
            //Btn_cuadro.IsEnabled = false;
            //Btn_Regresar_1.IsEnabled = false;
            //Cabeza_Menu.IsEnabled = false;
            //Menu_Botones.IsEnabled = false;
            //Menu_Informacion.IsEnabled = false;
            //Pie_Menu.IsEnabled = false;
            Btn_cuadro.IsHitTestVisible = false;
            Btn_Regresar_1.IsHitTestVisible = false;
            Cabeza_Menu.IsHitTestVisible = false;
            //Menu_Botones.IsHitTestVisible = false;
            textBlock.IsHitTestVisible = false;
            Menu_Informacion.IsHitTestVisible = false;
            Pie_Menu.IsHitTestVisible = false;
            //Btn_cuadro.SetValue(Panel.ZIndexProperty, 0);
            //Btn_Regresar_1.SetValue(Panel.ZIndexProperty, 0);
            //Cabeza_Menu.SetValue(Panel.ZIndexProperty, 0);
            //Menu_Botones.SetValue(Panel.ZIndexProperty, 0);
            //Menu_Informacion.SetValue(Panel.ZIndexProperty, 0);
            //Pie_Menu.SetValue(Panel.ZIndexProperty, 0);
            //ImgCentro.SetValue(Panel.ZIndexProperty, 0);

            Menu_Consulta.Opacity = 1;
            Btn_Tradicional.IsEnabled = true;
            Btn_Tematica.IsEnabled = true;
            Btn_Especiales.IsEnabled = true;
            Btn_Global.IsEnabled = true;
            Btn_Electoral.IsEnabled = true;
            Btn_Indices.IsEnabled = true;
            Btn_Regresar_2.IsEnabled = true;
            //Btn_Tradicional.SetValue(Panel.ZIndexProperty,100);
            //Btn_Tematica.SetValue(Panel.ZIndexProperty,100);
            //Btn_Especiales.SetValue(Panel.ZIndexProperty, 100);
            //Btn_Global.SetValue(Panel.ZIndexProperty, 100);
            //Btn_Electoral.SetValue(Panel.ZIndexProperty, 100);
            //Btn_Indices.SetValue(Panel.ZIndexProperty, 100);
            //Btn_Regresar_2.SetValue(Panel.ZIndexProperty, 100);
            Btn_Extras.Opacity = 1;
            Btn_Regresar_2.Opacity = 1;
#endif
            TblActualizado.Text = fachada.getActualizadoA();
            fachada.Close();
            //entrada = (Storyboard) this.Resources["Informacion_Story"];
            //entrada.Begin();
            //entrada = (Storyboard) this.Resources["Regreso1_Informacion"];
            //entrada.Begin();
            //entrada=(Storyboard) this.Resources["Consulta_Story"];
            //entrada.Begin();
            //entrada = (Storyboard)this.Resources["Regresa2_Consultas"];
            //entrada.Begin();
        }

        private void ConsultaTradicional_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            Window1 Panel = new Window1();
            Panel.Back = this;
            NavigationService.Navigate(Panel);
        }

        private void ConsultasEspeciales_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            ConsultasEspeciales consultas = new ConsultasEspeciales();
            consultas.Back = this;
            NavigationService.Navigate(consultas);
        }

        private void NoticiaHistorica_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            NoticiaHistorica noticia = new NoticiaHistorica();
            noticia.Back = this;
            NavigationService.Navigate(noticia);
        }


        private void Presentacion_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            presentacion ventanaPresentacion = new presentacion();
            this.NavigationService.Navigate(ventanaPresentacion);
        }


        private void Indices_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            Inicial paginaIndices = new Inicial();
            paginaIndices.Back = this;
            this.NavigationService.Navigate(paginaIndices);
        }

        private void Directorio_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            PresentacionDirectorio paginaDirectorio = new PresentacionDirectorio();
            paginaDirectorio.Back = this;
            this.NavigationService.Navigate(paginaDirectorio);
        }

        private void Tematica_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            Cursor = Cursors.Wait;
            ConsultaTematica paginaDirectorio = new ConsultaTematica();
            paginaDirectorio.Back = this;
            this.NavigationService.Navigate(paginaDirectorio);
            Cursor = Cursors.Arrow;
        }

        private void Globales_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            BusquedaGlobal pagina = new BusquedaGlobal();
            pagina.Back = this;
            this.NavigationService.Navigate(pagina);
        }

        private void Etica_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            codigo ventanaPresentacion = new codigo();
            this.NavigationService.Navigate(ventanaPresentacion);
        }

        private void Page_KeyDown(object sender, KeyEventArgs e)
        {
            String ARCHIVO_AYUDA = "ius.chm";
            if (!BrowserInteropHelper.IsBrowserHosted)
            {
                RegistryKey clase = Registry.LocalMachine.OpenSubKey(Constants.IUS_REGISTRY_ENTRY);
                String PathDirectorio = (String)clase.GetValue("Ruta") + "\\ius.chm";
                ARCHIVO_AYUDA = PathDirectorio;
            }
            if (e.Key == Key.F1)
            {
                help.DefaultHelpFile = ARCHIVO_AYUDA;
                help.ShowHelp();
            }
        }

        private void CommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = Keyboard.FocusedElement is UIElement;
        }

        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            help.ShowHelp();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.KeyDown += new KeyEventHandler(Page_KeyDown);
            
        }

        private void conozca_Click(object sender, RoutedEventArgs e)
        {
            if (BrowserInteropHelper.IsBrowserHosted)
            {
                MessageBox.Show(Mensajes.MENSAJE_NO_SE_PUEDE_VER_CORTE, Mensajes.TITULO_NO_SE_PUEDE_VER_CORTE,
                    MessageBoxButton.OK, MessageBoxImage.Information);
                //Uri lugar = new Uri("pack://siteofOrigin:,,,/Anexos/SCJN.htm", UriKind.RelativeOrAbsolute);
                //NavigationService.Navigate(lugar);
            }
            else
            {
                LlamaSCJN();
            }
        }

        private void LlamaSCJN()
        {
            Process.Start("http://www.scjn.gob.mx");
        }

        private void Ayuda_Btn_Click(object sender, RoutedEventArgs e)
        {
            if (BrowserInteropHelper.IsBrowserHosted)
            {
                MessageBox.Show(Mensajes.MENSAJE_AYUDA, Mensajes.TITULO_AYUDA,
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                help.DefaultHelpFile = "ius.chm";
                help.ShowHelp();
            }
        }

        private void Informe_Btn_Click(object sender, RoutedEventArgs e)
        {
            Informe ventana = new Informe();
            this.NavigationService.Navigate(ventana);
            
        }

        private void Salir_Btn_Click(object sender, RoutedEventArgs e)
        {
            Salida ventana = new Salida();
            this.NavigationService.Navigate(ventana);
        }

        private void IElectoral_Btn_Click(object sender, RoutedEventArgs e)
        {
            MenuElectoral ventana = new MenuElectoral();
            ventana.Controlador.Back = this;
            this.NavigationService.Navigate(ventana);
        }


    }
}
