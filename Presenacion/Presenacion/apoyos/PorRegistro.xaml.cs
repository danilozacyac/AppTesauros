﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using mx.gob.scjn.ius_common.fachade;
using mx.gob.scjn.ius_common.TO;
using IUS;
using mx.gob.scjn.ius_common.gui.utils;
using System.Windows.Interop;
using Microsoft.Win32;
using JaStDev.ControlFramework.Controls;
using mx.gob.scjn.ius_common.utils;

namespace mx.gob.scjn.ius_common.gui.apoyos
{
    /// <summary>
    /// Interaction logic for PorRegistro.xaml
    /// </summary>
    public partial class PorRegistro : UserControl
    {
        private Point inicioDrag;
        private Point ofsetDrag;
        protected Help help = new Help();
        /// <summary>
        /// El servicio de navegación con el que se mostrarán los
        /// resultados de las búsquedas.
        /// </summary>
        public NavigationService NavigationService { get; set; }
        /// <summary>
        /// El tipo de Búsqueda que se pretende tener.
        /// </summary>
        public int TipoBusqueda { get; set; }

        ///<summary>
        /// Indica que la búsqueda se hará por tesis.
        /// </summary>
        public const int BUSQUEDA_REGISTRO_TESIS=1;

        /// <summary>
        /// Indica que la busqueda es ppor ejecutorias.
        /// </summary>
        public const int BUSQUEDA_REGITRO_EJECUTORIAS =2;

        /// <summary>
        /// Indica que la búsqueda es de votos.
        /// </summary>
        public const int BUSQUEDA_REGISTRO_VOTOS = 3;

        /// <summary>
        /// Indica que la búsqueda es de Acuerdos.
        /// </summary>
        public const int BUSQUEDA_REGISTRO_ACUERDOS=4;

        /// <summary>
        /// Constructor predeterminado
        /// </summary>
        /// 
        public PorRegistro()
        {
            InitializeComponent();
            TipoBusqueda = BUSQUEDA_REGISTRO_TESIS;
        }
        private void actualizaTexto(String numeroNuevo)
        {
            if(this.registroActual.Text.Length<7)
            this.registroActual.Text += numeroNuevo;
        }

        /// <summary>
        /// Esconde la ventana para que se pueda seguir  trabajando sin ella.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Salir_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            this.registroActual.Text = "";
            this.ListaSeleccionados.Items.Clear();
            this.Visibility = Visibility.Hidden;
        }

        private void boton1_Click(object sender, RoutedEventArgs e)
        {
            actualizaTexto("1");
        }

        private void boton2_Click(object sender, RoutedEventArgs e)
        {
            actualizaTexto("2");
        }

        private void boton3_Click(object sender, RoutedEventArgs e)
        {
            actualizaTexto("3");
        }

        private void boton4_Click(object sender, RoutedEventArgs e)
        {
            actualizaTexto("4");
        }

        private void boton5_Click(object sender, RoutedEventArgs e)
        {
            actualizaTexto("5");
        }

        private void boton6_Click(object sender, RoutedEventArgs e)
        {
            actualizaTexto("6");
        }

        private void boton7_Click(object sender, RoutedEventArgs e)
        {
            actualizaTexto("7");
        }

        private void boton8_Click(object sender, RoutedEventArgs e)
        {
            actualizaTexto("8");
        }

        private void boton9_Click(object sender, RoutedEventArgs e)
        {
            actualizaTexto("9");
        }

        private void boton0_Click(object sender, RoutedEventArgs e)
        {
            actualizaTexto("0");
        }

        private void anade_Click(object sender, RoutedEventArgs e)
        {
            if (!registroActual.Text.Equals(""))
            {
                String registro = verificaExistencia(registroActual.Text);
                MessageBoxResult respuesta = MessageBoxResult.Yes;
                if ((registro!=null)&&(!registro.Equals(registroActual.Text)))
                {
                    respuesta = MessageBox.Show(Mensajes.MENSAJE_MODIFICA_NUMERO_IUS_1 + registro + Mensajes.MENSAJE_MODIFICA_NUMERO_IUS_2,
                        Mensajes.TITULO_MODIFICA_NUMERO_IUS, MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
                }
                if (respuesta == MessageBoxResult.Yes)
                {
                    registroActual.Text = "";
                    if (registro != null)
                    {
                        if (!ListaSeleccionados.Items.Contains(registro))
                        {
                            ListaSeleccionados.Items.Add(registro);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show(Mensajes.MENSAJE_FALTA_NUMERO_REGISTRO, Mensajes.TITULO_FALTA_NUMERO_REGISTRO,
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private String verificaExistencia(String registro)
        {
            try
            {
#if STAND_ALONE
                FachadaBusquedaTradicional fachada = new FachadaBusquedaTradicional();
#else
                FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
#endif
                Window1 ventanaPadre = null;
                ComboBoxItem seleccion = null;
                switch (this.TipoBusqueda)
                {
                    case BUSQUEDA_REGISTRO_TESIS:
                        TesisTO tesisResultado;
                        tesisResultado = fachada.getTesisPorRegistro(registro);
                        fachada.Close();
                        if (tesisResultado.Ius != null)
                        {
                            if (tesisResultado.Parte.Equals("99"))
                            {
                                MessageBox.Show(Mensajes.MENSAJE_REGISTRO_NO_ENCONTRADO + registro,
                                    Mensajes.TITULO_REGISTRO_NO_ENCONTRADO, MessageBoxButton.OK, MessageBoxImage.Information);
                                return null;
                            }
                            return tesisResultado.Ius;
                        }
                        else
                        {
                            MessageBox.Show(Mensajes.MENSAJE_REGISTRO_NO_ENCONTRADO+registro,
                                Mensajes.TITULO_REGISTRO_NO_ENCONTRADO, MessageBoxButton.OK,MessageBoxImage.Information);
                            return null;
                        }
                    case BUSQUEDA_REGISTRO_ACUERDOS:
                        AcuerdosTO acuerdoResultado;
                        acuerdoResultado = fachada.getAcuerdoPorId(Int32.Parse(registro));
                        fachada.Close();
                        String parte = acuerdoResultado.ParteT;

                        if ((acuerdoResultado != null) && (acuerdoResultado.Id != null))
                        {
                            ventanaPadre = (Window1)Application.Current.MainWindow.Content;
                            seleccion = (ComboBoxItem)ventanaPadre.tipoDocumento.SelectedValue;
                            if (seleccion.Content.Equals("Otros") &&
                                (parte.Equals("156") || parte.Equals("163") || parte.Equals("170")))
                            {
                                return acuerdoResultado.Id;
                            }
                            else if (seleccion.Content.Equals("Acuerdos") &&
                                !((parte.Equals("156") || parte.Equals("163") || parte.Equals("170"))))
                            {
                                return acuerdoResultado.Id;
                            }
                            else
                            {
                                MessageBox.Show(Mensajes.MENSAJE_REGISTRO_NO_ENCONTRADO + registro, Mensajes.TITULO_REGISTRO_NO_ENCONTRADO,
                                    MessageBoxButton.OK, MessageBoxImage.Error);
                                return null;
                            }
                        }
                        else
                        {
                            MessageBox.Show(Mensajes.MENSAJE_REGISTRO_NO_ENCONTRADO + registro, Mensajes.TITULO_REGISTRO_NO_ENCONTRADO,
                                    MessageBoxButton.OK, MessageBoxImage.Error);
                            return null;
                        }
                    case BUSQUEDA_REGISTRO_VOTOS:
                        ventanaPadre = (Window1)Application.Current.MainWindow.Content;
                        seleccion = (ComboBoxItem)ventanaPadre.tipoDocumento.SelectedValue;
                        VotosTO votoResultado;
                        votoResultado = fachada.getVotosPorId(Int32.Parse(registro));
                        fachada.Close();
                        if ((votoResultado != null)&&(votoResultado.Id!=null))
                        {
                            if (seleccion.Content.Equals("Octava parte: Votos")
                                && (Int32.Parse(votoResultado.ParteT) > 48))
                                return votoResultado.Id;
                            else if (seleccion.Content.Equals("Votos")
                                && (Int32.Parse(votoResultado.ParteT) < 49))
                                return votoResultado.Id;
                            else
                            {
                                MessageBox.Show(Mensajes.MENSAJE_REGISTRO_NO_ENCONTRADO + registro, Mensajes.TITULO_REGISTRO_NO_ENCONTRADO,
                                    MessageBoxButton.OK, MessageBoxImage.Error);
                                return null;
                            }
                        }
                        else
                        {
                            MessageBox.Show(Mensajes.MENSAJE_REGISTRO_NO_ENCONTRADO + registro, Mensajes.TITULO_REGISTRO_NO_ENCONTRADO,
                                    MessageBoxButton.OK, MessageBoxImage.Error);
                            return null;
                        }
                        break;
                    case BUSQUEDA_REGITRO_EJECUTORIAS:
                        EjecutoriasTO ejecutaResultado;
                        ejecutaResultado = fachada.getEjecutoriaPorId(Int32.Parse(registro));
                        fachada.Close();
                        if ((ejecutaResultado != null)&&(ejecutaResultado.Id!=null))
                        {
                            ventanaPadre = (Window1)Application.Current.MainWindow.Content;
                            seleccion = (ComboBoxItem)ventanaPadre.tipoDocumento.SelectedValue;
                            if (seleccion.Content.Equals("Ejecutorias") && !((ejecutaResultado.ParteT.Equals("30")||(Int32.Parse(ejecutaResultado.ParteT)>48))))
                                return ejecutaResultado.Id;
                            else if (seleccion.Content.Equals("Sentencias de otros Tribunales") && (ejecutaResultado.ParteT.Equals("30")))
                                return ejecutaResultado.Id;
                            else if (seleccion.Content.Equals("Octava parte: Ejecutorias")
                                && (Int32.Parse(ejecutaResultado.ParteT) > 48 && Int32.Parse(ejecutaResultado.ParteT) < 62))
                                return ejecutaResultado.Id;
                            else if (seleccion.Content.Equals("Novena parte: Sentencias")
                            && (Int32.Parse(ejecutaResultado.ParteT) == 63))
                                return ejecutaResultado.Id;
                            else
                            {
                                MessageBox.Show(Mensajes.MENSAJE_REGISTRO_NO_ENCONTRADO + registro, Mensajes.TITULO_REGISTRO_NO_ENCONTRADO,
                                    MessageBoxButton.OK, MessageBoxImage.Error);
                                return null;
                            }
                        }
                        else
                        {
                            MessageBox.Show(Mensajes.MENSAJE_REGISTRO_NO_ENCONTRADO + registro, Mensajes.TITULO_REGISTRO_NO_ENCONTRADO,
                                    MessageBoxButton.OK, MessageBoxImage.Error);
                            return null;
                        }
                        break;
                    default:
                        return null;
                        break;
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show("Ocurrió el siguiente error:\n" + exc.Message + "\n Revise que el IUS y sus servicios estén funcionando correctamente.");
                return null;
            }
        }

        
        private void Quita_Click(object sender, RoutedEventArgs e)
        {
            ListaSeleccionados.Items.Remove(ListaSeleccionados.SelectedItem);
        }

        private void Buscar_Click(object sender, RoutedEventArgs e)
        {
            if ((ListaSeleccionados.Items.Count == 0)&&(registroActual.Text.Equals("")))
            {
                MessageBox.Show(Mensajes.MENSAJE_NO_HAY_REGISTROS, 
                    Mensajes.TITULO_NO_HAY_REGISTROS, 
                    MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            if ((ListaSeleccionados.Items.Count == 0) &&(!registroActual.Text.Equals("")))
            {
                anade_Click(sender, e);
            }
            if (ListaSeleccionados.Items.Count == 0)
            {
                return;
            } 
            //int[] ids;
            List<int> inicial = new List<int>();
            foreach (object item in ListaSeleccionados.Items)
            {
                String contenido = (String)item;
                int actual = Int32.Parse(contenido);
                inicial.Add(actual);
            }
            ListaSeleccionados.Items.Clear();
            this.Visibility = Visibility.Hidden;
            switch (TipoBusqueda)
            {
                case BUSQUEDA_REGISTRO_TESIS:
                    tablaResultado paginaResultadosTesis = new tablaResultado(inicial);
                    paginaResultadosTesis.Back =(Page) this.NavigationService.Content;
                    this.NavigationService.Navigate(paginaResultadosTesis);
                    break;
                case BUSQUEDA_REGITRO_EJECUTORIAS:
                    tablaResultadoEjecutoria paginaResultadosEjec = new tablaResultadoEjecutoria(inicial.ToArray());
                    paginaResultadosEjec.Back = (Page)this.NavigationService.Content;
                    this.NavigationService.Navigate(paginaResultadosEjec);
                    break;
                case BUSQUEDA_REGISTRO_VOTOS:
                    tablaResultadoVotos paginaResultadosVotos = new tablaResultadoVotos(inicial.ToArray());
                    paginaResultadosVotos.Back = (Page)this.NavigationService.Content;
                    this.NavigationService.Navigate(paginaResultadosVotos);
                    break;
                case BUSQUEDA_REGISTRO_ACUERDOS:
                    Window1 ventanaPadre = (Window1)Application.Current.MainWindow.Content;
                    tablaResultadoAcuerdo paginaResultadosAcuer = new tablaResultadoAcuerdo(inicial.ToArray());
                    paginaResultadosAcuer.Back = (Page)this.NavigationService.Content;
                    ComboBoxItem seleccion = (ComboBoxItem)ventanaPadre.tipoDocumento.SelectedValue;
                    if (seleccion.Content.Equals("Otros"))
                    {
                        paginaResultadosAcuer.TituloAcuerdo.Title = "Otros";
                        paginaResultadosAcuer.WindowTitle = "Visualizar registros Otros";
                    }
                    this.NavigationService.Navigate(paginaResultadosAcuer);

                    break;
                default:
                    break;
            }
        }

        private void BarraMovimiento_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ofsetDrag = e.GetPosition(BarraMovimiento);
            if ((inicioDrag.X == -1) && (inicioDrag.Y == -1))
            {
                inicioDrag = e.GetPosition(Parent as Canvas);
                this.BarraMovimiento.CaptureMouse();
            }
        }

        private void BarraMovimiento_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            inicioDrag.X = -1;
            inicioDrag.Y = -1;
            BarraMovimiento.ReleaseMouseCapture();

        }

        private void BarraMovimiento_MouseMove(object sender, MouseEventArgs e)
        {
            if ((e.LeftButton == MouseButtonState.Pressed) && (BarraMovimiento.IsMouseCaptured))
            {
                Point puntoActual = e.GetPosition(Parent as Canvas);
                puntoActual.X -= ofsetDrag.X;
                puntoActual.Y -= ofsetDrag.Y;
                Canvas.SetTop(this, puntoActual.Y);
                Canvas.SetLeft(this, puntoActual.X);
            }
            else
            {
                inicioDrag.X = -1;
                inicioDrag.Y = -1;
            }

        }

        private void UserControl_KeyDown(object sender, KeyEventArgs e)
        {
            String ARCHIVO_AYUDA = "ius.chm";
            if (!BrowserInteropHelper.IsBrowserHosted)
            {
                RegistryKey clase = Registry.LocalMachine.OpenSubKey(Constants.IUS_REGISTRY_ENTRY);
                String PathDirectorio = (String)clase.GetValue("Ruta") + "\\ius.chm";
                ARCHIVO_AYUDA = PathDirectorio;
            }
            help.DefaultHelpFile = ARCHIVO_AYUDA;
            if (e.Key == Key.F1)
            {
                help.ShowHelpTopic("Consulta_tradicional");
                //help.ShowHelp();
            }
            
        }
    }
}
