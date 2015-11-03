using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms.Integration;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media.Animation;
using IUS.Indices;
using JaStDev.ControlFramework.Controls;
using Microsoft.Win32;
using mx.gob.scjn.directorio.gui;
using mx.gob.scjn.electoral;
using mx.gob.scjn.ius_common.TO;
using mx.gob.scjn.ius_common.fachade;
using mx.gob.scjn.ius_common.gui;
using mx.gob.scjn.ius_common.gui.BusquedaGenerica;
using mx.gob.scjn.ius_common.gui.Config;
using mx.gob.scjn.ius_common.gui.Tematica;
using mx.gob.scjn.ius_common.gui.noticia;
using mx.gob.scjn.ius_common.gui.salir;
using mx.gob.scjn.ius_common.gui.utils;
using mx.gob.scjn.ius_common.utils;

[assembly: AllowPartiallyTrustedCallers]
namespace IUS
{


    /// <summary>
    /// Interaction logic for PaginaInicial.xaml
    /// </summary>
    public partial class PaginaInicial : Page
    {
        Help help = new Help();

        public PaginaInicial()
        {
            InitializeComponent();
            Storyboard entrada = (Storyboard)this.Resources["Entrada_Story"];//= (Storyboard) this.Resources["Entrada_Story"];
#if STAND_ALONE
            ElementHost.EnableModelessKeyboardInterop(Application.Current.MainWindow);
            FachadaBusquedaTradicional fachada = new FachadaBusquedaTradicional();
#else
            FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
#endif
            TblActualizado.Text = fachada.getActualizadoA();
            fachada.Close();
            //entrada.Begin();
            //entrada = (Storyboard) this.Resources["Informacion_Story"];
            entrada.Begin();
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
            TbxBusquedaRapida.Focus();
            TbxBusquedaRapida.SelectAll();
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

        private void Config_Btn_Click(object sender, RoutedEventArgs e)
        {
            Configuracion ventanaConfig = new Configuracion();
            ventanaConfig.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            ventanaConfig.KeyDown += new KeyEventHandler(Page_KeyDown);
            ventanaConfig.ShowDialog();
        }

        private void Buscar_Btn_Click(object sender, RoutedEventArgs e)
        {
            if (TbxBusquedaRapida.Text.Equals(String.Empty)|| TbxBusquedaRapida.Text.Equals("Consulta global"))
            {
                MessageBox.Show(Mensajes.MENSAJE_CAMPO_TEXTO_VACIO, Mensajes.TITULO_ADVERTENCIA,
                    MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            if (Validadores.BusquedaPalabraTexto(this.TbxBusquedaRapida).Equals(""))
            {
                Cursor = Cursors.Wait;
                bool camposSeleccionads = false;
                BusquedaTO busqueda = new BusquedaTO();
                busqueda.Epocas = new bool[Constants.EPOCAS_LARGO][];
                for (int contador = 0; contador < Constants.EPOCAS_LARGO; contador++)
                {
                    busqueda.Epocas[contador] = new bool[Constants.EPOCAS_ANCHO];
                    for (int llenador = 0; llenador < Constants.EPOCAS_ANCHO; llenador++)
                    {

                        busqueda.Epocas[contador][llenador] = true;
                    }
                }
                busqueda.Apendices = new bool[Constants.APENDICES_ANCHO][];
                for (int contador = 0; contador < Constants.APENDICES_ANCHO; contador++)
                {
                    busqueda.Apendices[contador] = new bool[Constants.APENDICES_LARGO];
                    for (int llenador = 0; llenador < Constants.APENDICES_LARGO; llenador++)
                    {

                        busqueda.Apendices[contador][llenador] = true;
                    }
                }
                busqueda.Acuerdos = new bool[Constants.ACUERDOS_ANCHO][];
                for (int contador = 0; contador < Constants.ACUERDOS_ANCHO; contador++)
                {
                    busqueda.Acuerdos[contador] = new bool[Constants.ACUERDOS_LARGO];
                    for (int llenador = 0; llenador < Constants.ACUERDOS_LARGO; llenador++)
                    {

                        busqueda.Acuerdos[contador][llenador] = true;
                    }
                }
                busqueda.TipoBusqueda = Constants.BUSQUEDA_TESIS_SIMPLE;
#if STAND_ALONE
                busqueda.Palabra = new List<BusquedaPalabraTO>();
                busqueda.Palabra.Add(new BusquedaPalabraTO());
#else
                busqueda.Palabra = new BusquedaPalabraTO[1];
                busqueda.Palabra[0] = new BusquedaPalabraTO();
#endif
                List<int> campos = new List<int>();
                    campos.Add(Constants.BUSQUEDA_PALABRA_CAMPO_LOC);
                    campos.Add(Constants.BUSQUEDA_PALABRA_CAMPO_TEXTO);
                    campos.Add(Constants.BUSQUEDA_PALABRA_CAMPO_PRECE);
                    campos.Add(Constants.BUSQUEDA_PALABRA_CAMPO_RUBRO);
                    campos.Add(Constants.BUSQUEDA_PALABRA_CAMPO_ASUNTO);
                    campos.Add(Constants.BUSQUEDA_PALABRA_CAMPO_EMISOR);
                    campos.Add(Constants.BUSQUEDA_PALABRA_CAMPO_TEMA);
                    camposSeleccionads = true;
                if (!camposSeleccionads)
                {
                    MessageBox.Show(Mensajes.MENSAJE_CAMPO_NO_SELECCIONADO,
                        Mensajes.TITULO_CAMPO_NO_SELECCIONADO,
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                int juris = Constants.BUSQUEDA_PALABRA_AMBAS;
                busqueda.Palabra[0].Jurisprudencia = juris;
#if STAND_ALONE
                busqueda.Palabra[0].Campos = campos;
#else
                busqueda.Palabra[0].Campos = campos.ToArray();
#endif
                busqueda.OrdenarPor = Constants.ORDER_DEFAULT;
                busqueda.Palabra[0].Expresion = CalculosGlobales.SeparaExpresiones(TbxBusquedaRapida.Text);
                ResultadosBusqueda Tesis = new ResultadosBusqueda(busqueda);
                Tesis.Back = this;
                if (
                    (Tesis.tablaResultados.ItemsSource == null)
                    || (Tesis.tablaResultados.Items.Count == 0))
                {
                    MessageBox.Show(Mensajes.MENSAJE_BUSQUEDA_VACIA,
                        Mensajes.TITULO_BUSQUEDA_VACIA, MessageBoxButton.OK,
                        MessageBoxImage.Exclamation);
                    Cursor =Cursors.Arrow;
                    return;
                }
                this.NavigationService.Navigate(Tesis);
                List<TreeViewItem> listaCats = (List<TreeViewItem>)Tesis.tablaResultados.ItemsSource;
                listaCats = (List<TreeViewItem>)listaCats[0].ItemsSource;
                TreeViewItem item = (TreeViewItem)listaCats[0];
                item.IsSelected = true;
                Cursor = Cursors.Arrow;
            }
        }

        private void TbxBusquedaRapida_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Buscar_Btn_Click(sender, null);
            }
        }

        private void TbxBusquedaRapida_GotFocus(object sender, RoutedEventArgs e)
        {
            if (TbxBusquedaRapida.Text.Equals("Acceso general"))
            {
                TbxBusquedaRapida.Text = String.Empty;
            }
            
        }

        private void TbxBusquedaRapida_LostFocus(object sender, RoutedEventArgs e)
        {
            if (TbxBusquedaRapida.Text.Equals(String.Empty))
            {
                TbxBusquedaRapida.Text = "Acceso general";
                TbxBusquedaRapida.SelectAll();
            }
        }


    }
}
