using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using IUS.gui.utilities;
using JaStDev.ControlFramework.Controls;
using Microsoft.Win32;
using mx.gob.scjn.ius_common.TO;
using mx.gob.scjn.ius_common.gui.Guardar;
using mx.gob.scjn.ius_common.gui.apoyos;
using mx.gob.scjn.ius_common.gui.utils;
using mx.gob.scjn.ius_common.utils;

namespace IUS
{
    /// <summary>
    /// Lógica de interacción para Window1.xaml
    /// </summary>
    public partial class Window1 : Page
    {
        BotonesCheckBox controlesEpoca;
        BotonesCheckBox controlesApendices;
        BotonesCheckBox controlesAcuerdos;
        public Page Back { get; set; }
        //BackgroundWorker worker;
        Help help = new Help();
        public Window1()
        {
            InitializeComponent();
            estableceCheckboxes();
        }
        /// <summary>
        /// Define los objetos de BotonCheckBox para los distintos paneles.
        /// </summary>
        public void estableceCheckboxes()
        {
            CheckBox[][] epocas;
            CheckBox[][] apendices;
            CheckBox[][] acuerdos;
            Boolean[][] matrizActual;
            Button[] botones;
            int largo, ancho;
            
            //Comencemos con la epoca
            largo = Constants.EPOCAS_ANCHO;
            ancho = Constants.EPOCAS_LARGO;
            epocas = new CheckBox[ancho][];
            matrizActual = new bool[ancho][];
            
            for (int j = 0; j < ancho; j++)
                {
                    CheckBox[] arreglo = new CheckBox[largo];
                    Boolean[] lineaActual = new bool[largo];
                    for (int i = 0; i < largo; i++)
                    {
                        Type tipo = this.GetType();
                        string checkboxstr = "epocasH" + j + "V" + i;
                        this.epocasH0V0.IsChecked = false;
                        CheckBox item;
                        try
                        {
                            item = (CheckBox)tipo.InvokeMember(checkboxstr,
                                BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.Instance,
                                null, this, null);
                            item.IsChecked = false;
                        }
                        catch (MissingFieldException e)
                        {
                            item = null;
                            System.Console.Write("Error: " + e.Message);
                        }
                        lineaActual[i] = false;
                        arreglo[i] = item;
                    }
                    epocas[j] = arreglo;
                    matrizActual[j] = lineaActual;
                }
            controlesEpoca = new BotonesCheckBox();
            botones = new Button[ancho];
            botones[0] = butonPleno;
            botones[1] = butonSala1;
            botones[2] = butonSala2;
            botones[3] = butonSala3;
            botones[4] = butonSala4;
            botones[5] = butonSalaAux;
            botones[6] = butonTCC;
            controlesEpoca.SetBotonesVerticales(botones);
            botones = new Button[largo];
            botones[0] = botonEpoca9;
            botones[1] = botonEpoca8;
            botones[2] = botonEpoca7;
            botones[3] = botonEpoca6;
            botones[4] = botonEpoca5;
            botones[5] = botonInformes;
            controlesEpoca.SetBotonesHorizontales(botones);
            controlesEpoca.SetBotonTodos(botonEpocasTodos);
            controlesEpoca.SetCheckBoxes(epocas);
            controlesEpoca.SetValores(matrizActual);
            //Siguen los apendices
            largo = Constants.APENDICES_LARGO;
            ancho = Constants.APENDICES_ANCHO;
            apendices = new CheckBox[ancho][];
            matrizActual = new bool[ancho][];

            for (int j = 0; j < ancho; j++)
            {
                CheckBox[] arreglo = new CheckBox[largo];
                Boolean[] lineaActual = new bool[largo];
                for (int i = 0; i < largo; i++)
                {
                    Type tipo = this.GetType();
                    string checkboxstr = "ApendiceH" + j + "V" + i;
                    CheckBox item;
                    try
                    {
                        item = (CheckBox)tipo.InvokeMember(checkboxstr,
                           BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.Instance,
                           null, this, null);
                        item.IsChecked = false;
                    }
                    catch (MissingFieldException e)
                    {
                        System.Console.WriteLine(e.Message);
                        item = null;
                    }
                    arreglo[i] = item;
                }
                apendices[j] = arreglo;
                matrizActual[j] = lineaActual;
            }
            controlesApendices = new BotonesCheckBox();
            botones = new Button[ancho];
            botones[0] = botonConst;
            botones[1] = botonPenal;
            botones[2] = botonAdmin;
            botones[3] = botonCivil;
            botones[4] = botonLaboral;
            botones[5] = botonComun;
            botones[6] = botonConfComp;
            botones[7] = botonElectoral;
            controlesApendices.SetBotonesVerticales(botones);
            botones = new Button[largo];
            botones[0] = botonAct2002;
            botones[1] = botonAct2001;
            botones[2] = boton19172000;
            botones[3] = boton19171988;
            botones[4] = boton19541988;
            controlesApendices.SetBotonesHorizontales(botones);
            controlesApendices.SetBotonTodos(botonApendicesTodos);
            controlesApendices.SetCheckBoxes(apendices);
            controlesApendices.SetValores(matrizActual);
            //Al final siguen los acuerdos
            largo = Constants.ACUERDOS_LARGO;
            ancho = Constants.ACUERDOS_ANCHO;
            acuerdos = new CheckBox[ancho][];
            matrizActual = new bool[ancho][];

            for (int j = 0; j < ancho; j++)
            {
                CheckBox[] arreglo = new CheckBox[largo];
                Boolean[] lineaActual = new bool[largo];
                for (int i = 0; i < largo; i++)
                {
                    Type tipo = this.GetType();
                    string checkboxstr = "AcuerdoH" + j + "V" + i;
                    CheckBox item ;
                    try
                    {
                        item = (CheckBox)tipo.InvokeMember(checkboxstr,
                            BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.Instance,
                            null, this, null);
                        item.IsChecked = false;
                    }
                    catch (MissingFieldException e)
                    {
                        item = null;
                    }
                    arreglo[i] = item;
                }
                acuerdos[j] = arreglo;
                matrizActual[j] = lineaActual;
            }
            controlesAcuerdos = new BotonesCheckBox();
            botones = new Button[ancho];
            botones[0] = botonPlenoSCJN;
            botones[1] = botonCJF;
            botones[2] = botonPresidencia;
            botones[3] = botonCGA;
            botones[4] = botonComites;
            botones[5] = botonConjuntos;
            botones[6] = botonOtros;
            controlesAcuerdos.SetBotonesVerticales(botones);
            botones = new Button[largo];
            botones[0] = boton9a;
            botones[1] = boton8a;
            controlesAcuerdos.SetBotonesHorizontales(botones);
            controlesAcuerdos.SetBotonTodos(botonAcuerdosTodos);
            controlesAcuerdos.SetCheckBoxes(acuerdos);
            controlesAcuerdos.SetValores(matrizActual);
            controlesAcuerdos.InhabilitaTodos();
        }
        /// <summary>
        /// Cambia las selecciones adecuadas de acuerdo a 
        /// la selección hecha por el usuario.
        /// </summary>
        /// <param name="sender">Quien manda el evento, el tipoDocumento en este caso</param>
        /// <param name="e">El evento</param>
        private void tipoDocumento_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int seleccion = tipoDocumento.SelectedIndex;
            switch(seleccion){
                case 0:
                    habilitaTodas();
                    if (controlesAcuerdos != null)
                    {
                        controlesAcuerdos.InhabilitaTodos();
                    }
                    break;
                case 3:
                    if (controlesAcuerdos != null)
                    {
                        controlesAcuerdos.HabilitaTodos();
                        controlesAcuerdos.InhabilitaColumna(6);
                    }
                    if (controlesApendices != null)
                    {
                        controlesApendices.InhabilitaTodos();
                    }
                    if (controlesEpoca != null)
                    {
                        controlesEpoca.InhabilitaTodos();
                    }
                    break;
                case 4:
                    if (controlesAcuerdos != null)
                    {
                        controlesAcuerdos.InhabilitaTodos();
                        controlesAcuerdos.HabilitaColumna(6);
                    }
                    if (controlesApendices != null)
                    {
                        controlesApendices.InhabilitaTodos();
                    }
                    if (controlesEpoca != null)
                    {
                        controlesEpoca.InhabilitaTodos();
                    }
                    break;
                default:
                    habilitaParcial();
                    break;
            }
        }
        private void habilitaTodas()
        {
            if (controlesEpoca != null)
            {
                this.controlesEpoca.HabilitaTodos();
            }
            if (controlesApendices != null)
            {
                this.controlesApendices.HabilitaTodos();
            }
            if (controlesAcuerdos != null)
            {
                this.controlesAcuerdos.HabilitaTodos();
            }
            //MessageBox.Show("Habilitar Todas");
        }
        private void habilitaParcial()
        {
            this.controlesEpoca.HabilitaFila(0);
            this.controlesEpoca.HabilitaFila(1);
            this.controlesEpoca.InhabilitaFila(2);
            this.controlesEpoca.InhabilitaFila(3);
            this.controlesEpoca.InhabilitaFila(4);
            this.controlesEpoca.InhabilitaFila(5);
            this.controlesApendices.InhabilitaTodos();
            this.controlesAcuerdos.InhabilitaTodos();
            //MessageBox.Show("Habilita Parcialmente");
        }

        private void botonEpocasTodos_Click(object sender, RoutedEventArgs e)
        {
           // controlesEpoca.todos_click(sender, e);
        }
        /// <summary>
        /// Se genera la búsqueda cuando hay una busqueda secuencial.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void secuencial_Click(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount < 2)
            {
                Cursor = Cursors.Wait;
                Inhabilita();
                if (tipoDocumento.Text.Equals("Tesis"))
                {
                    BusquedaTO parametros = new BusquedaTO();
                    controlesAcuerdos.actualizaValores();
                    parametros.Acuerdos = controlesAcuerdos.Valores;
                    controlesApendices.actualizaValores();
                    parametros.Apendices = controlesApendices.Valores;
                    controlesEpoca.actualizaValores();
                    parametros.Epocas = controlesEpoca.Valores;
                    parametros.TipoBusqueda = Constants.BUSQUEDA_TESIS_SIMPLE;
                    parametros.OrdenarPor = "ConsecIndx";
                    int totalSeleccionados = controlesEpoca.Seleccionados + controlesApendices.Seleccionados + controlesAcuerdos.Seleccionados;
                    if (totalSeleccionados > 1)
                    {
                        MessageBox.Show(Mensajes.MENSAJE_CASILLAS_SIN_SELECCIONAR, Mensajes.TITULO_CASILLAS_SIN_SELECCIONAR,
                            MessageBoxButton.OK, MessageBoxImage.Error);
                        //this.Espera.Visibility = Visibility.Hidden;
                        Cursor = Cursors.Arrow;
                        Habilita();
                        return;
                    }
                    else if (totalSeleccionados < 1)
                    {
                        MessageBox.Show(Mensajes.MENSAJE_CASILLAS_SIN_SELECCIONAR, Mensajes.TITULO_CASILLAS_SIN_SELECCIONAR,
                            MessageBoxButton.OK, MessageBoxImage.Error);
                        //this.Espera.Visibility = Visibility.Hidden;
                        Cursor = Cursors.Arrow;
                        Habilita();
                        return;
                    }
                    tablaResultado ventanaResultado = new tablaResultado(parametros);
                    ventanaResultado.Back = this;
                    if ((ventanaResultado.tablaResultados.Items == null)
                        || (ventanaResultado.tablaResultados.Items.Count == 0))
                    {
                        MessageBox.Show(Mensajes.MENSAJE_BUSQUEDA_VACIA,
                            Mensajes.TITULO_BUSQUEDA_VACIA, MessageBoxButton.OK,
                            MessageBoxImage.Exclamation);
                        Habilita();
                        Cursor = Cursors.Arrow;
                        return;
                    }
                    this.NavigationService.Navigate(ventanaResultado);
                }
                else if (tipoDocumento.Text.Equals("Ejecutorias"))
                {
                    BusquedaTO parametros = new BusquedaTO();
                    controlesAcuerdos.actualizaValores();
                    parametros.Acuerdos = controlesAcuerdos.Valores;
                    controlesApendices.actualizaValores();
                    parametros.Apendices = controlesApendices.Valores;
                    controlesEpoca.actualizaValores();
                    parametros.Epocas = controlesEpoca.Valores;
                    parametros.OrdenarPor = "ConsecIndx";
                    parametros.TipoBusqueda = Constants.BUSQUEDA_EJECUTORIAS;
                    int totalSeleccionados = controlesEpoca.Seleccionados + controlesApendices.Seleccionados + controlesAcuerdos.Seleccionados;
                    if (totalSeleccionados > 1)
                    {
                        MessageBox.Show(Mensajes.MENSAJE_CASILLAS_SIN_SELECCIONAR, Mensajes.TITULO_CASILLAS_SIN_SELECCIONAR,
                            MessageBoxButton.OK, MessageBoxImage.Error);
                        Habilita();
                        //this.Espera.Visibility = Visibility.Hidden;
                        Cursor = Cursors.Arrow;
                        return;
                    }
                    else if (totalSeleccionados < 1)
                    {
                        MessageBox.Show(Mensajes.MENSAJE_CASILLAS_SIN_SELECCIONAR, Mensajes.TITULO_CASILLAS_SIN_SELECCIONAR,
                            MessageBoxButton.OK, MessageBoxImage.Error);
                        //this.Espera.Visibility = Visibility.Hidden;
                        Habilita();
                        Cursor = Cursors.Arrow;
                        return;
                    }
                    tablaResultadoEjecutoria ventanaResultado = new tablaResultadoEjecutoria(parametros);
                    ventanaResultado.Back = this;
                    if (ventanaResultado.tablaResultado.Items.Count <= 0)
                    {
                        MessageBox.Show(Mensajes.MENSAJE_BUSQUEDA_VACIA,
                            Mensajes.TITULO_BUSQUEDA_VACIA, MessageBoxButton.OK,
                            MessageBoxImage.Exclamation);
                        Habilita();
                        Cursor = Cursors.Arrow;
                        return;
                    }
                    this.NavigationService.Navigate(ventanaResultado);
                }
                else if (tipoDocumento.Text.Equals("Votos"))
                {
                    BusquedaTO parametros = new BusquedaTO();
                    controlesAcuerdos.actualizaValores();
                    parametros.Acuerdos = controlesAcuerdos.Valores;
                    controlesApendices.actualizaValores();
                    parametros.Apendices = controlesApendices.Valores;
                    controlesEpoca.actualizaValores();
                    parametros.Epocas = controlesEpoca.Valores;
                    parametros.OrdenarPor = "ConsecIndx";
                    parametros.TipoBusqueda = Constants.BUSQUEDA_VOTOS;
                    int totalSeleccionados = controlesEpoca.Seleccionados + controlesApendices.Seleccionados + controlesAcuerdos.Seleccionados;
                    if (totalSeleccionados > 1)
                    {
                        MessageBox.Show(Mensajes.MENSAJE_CASILLAS_SIN_SELECCIONAR, Mensajes.TITULO_CASILLAS_SIN_SELECCIONAR,
                            MessageBoxButton.OK, MessageBoxImage.Error);
                        //this.Espera.Visibility = Visibility.Hidden;
                        Cursor = Cursors.Arrow;
                        Habilita();
                        return;
                    }
                    else if (totalSeleccionados < 1)
                    {
                        MessageBox.Show(Mensajes.MENSAJE_CASILLAS_SIN_SELECCIONAR, Mensajes.TITULO_CASILLAS_SIN_SELECCIONAR,
                            MessageBoxButton.OK, MessageBoxImage.Error);
                        //this.Espera.Visibility = Visibility.Hidden;
                        Cursor = Cursors.Arrow;
                        Habilita();
                        return;
                    }
                    tablaResultadoVotos ventanaResultado = new tablaResultadoVotos(parametros);
                    ventanaResultado.Back = this;
                    int totales = ventanaResultado.tablaResultado.Items.Count;/* +
                        ventanaResultado.tablaResultadoConcurrentes.Items.Count +
                        ventanaResultado.tablaResultadoMinoritario.Items.Count +
                        ventanaResultado.tablaResultadoParticulares.Items.Count;*/
                    if (totales <= 0)
                    {
                        MessageBox.Show(Mensajes.MENSAJE_BUSQUEDA_VACIA,
                            Mensajes.TITULO_BUSQUEDA_VACIA, MessageBoxButton.OK,
                            MessageBoxImage.Exclamation);
                        Cursor = Cursors.Arrow;
                        Habilita();
                        return;
                    }
                    this.NavigationService.Navigate(ventanaResultado);
                }
                else if (tipoDocumento.Text.Equals("Acuerdos") || tipoDocumento.Text.Equals("Otros"))
                {
                    BusquedaTO parametros = new BusquedaTO();
                    controlesAcuerdos.actualizaValores();
                    parametros.Acuerdos = controlesAcuerdos.Valores;
                    controlesApendices.actualizaValores();
                    parametros.Apendices = controlesApendices.Valores;
                    controlesEpoca.actualizaValores();
                    parametros.Epocas = controlesEpoca.Valores;
                    parametros.TipoBusqueda = Constants.BUSQUEDA_ACUERDO;
                    parametros.OrdenarPor = "ConsecIndx";
                    int totalSeleccionados = controlesEpoca.Seleccionados + controlesApendices.Seleccionados + controlesAcuerdos.Seleccionados;
                    if (totalSeleccionados > 1)
                    {
                        MessageBox.Show(Mensajes.MENSAJE_CASILLAS_SIN_SELECCIONAR, Mensajes.TITULO_CASILLAS_SIN_SELECCIONAR,
                            MessageBoxButton.OK, MessageBoxImage.Error);
                        // this.Espera.Visibility = Visibility.Hidden;
                        Cursor = Cursors.Arrow;
                        Habilita();
                        return;
                    }
                    else if (totalSeleccionados < 1)
                    {
                        MessageBox.Show(Mensajes.MENSAJE_CASILLAS_SIN_SELECCIONAR, Mensajes.TITULO_CASILLAS_SIN_SELECCIONAR,
                            MessageBoxButton.OK, MessageBoxImage.Error);
                        //this.Espera.Visibility = Visibility.Hidden;
                        Cursor = Cursors.Arrow;
                        Habilita();
                        return;
                    }
                    tablaResultadoAcuerdo ventanaResultado = new tablaResultadoAcuerdo(parametros);
                    //this.Espera.Visibility = Visibility.Hidden;
                    ventanaResultado.Back = this;
                    if (ventanaResultado.tablaResultado.Items.Count <= 0)
                    {
                        MessageBox.Show(Mensajes.MENSAJE_BUSQUEDA_VACIA,
                            Mensajes.TITULO_BUSQUEDA_VACIA, MessageBoxButton.OK,
                            MessageBoxImage.Exclamation);
                        Cursor = Cursors.Arrow;
                        Habilita();
                        return;
                    }
                    this.NavigationService.Navigate(ventanaResultado);
                }
                Cursor = Cursors.Arrow;
            }
        }
        
        private void secuencial_MouseEnter(object sender, MouseEventArgs e)
        {
            Image imagenNueva = new Image();
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri("/General;component/images/Bus-secuencial2.png", UriKind.Relative);
            bitmap.EndInit();
            this.secuencial.Source = bitmap;
        }

        private void secuencial_MouseLeave(object sender, MouseEventArgs e)
        {
            Image imagenNueva = new Image();
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri("/General;component/images/Bus-secuencial.png", UriKind.Relative);
            bitmap.EndInit();
            this.secuencial.Source = bitmap;
        }

        private void PorRegistro_MouseEnter(object sender, MouseEventArgs e)
        {
            Image imagenNueva = new Image();
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri("/General;component/images/Bus-registro2.png", UriKind.Relative);
            bitmap.EndInit();
            this.PorRegistro.Source = bitmap;
        }

        private void PorRegistro_MouseLeave(object sender, MouseEventArgs e)
        {
            Image imagenNueva = new Image();
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri("/General;component/images/Bus-registro.png", UriKind.Relative);
            bitmap.EndInit();
            this.PorRegistro.Source = bitmap;
        }

        private void PorRegistro_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            System.Windows.Forms.Application.DoEvents();
            this.ventanaPorRegistro.NavigationService = this.NavigationService;
            int seleccion = tipoDocumento.SelectedIndex;
            switch (seleccion)
            {
                case 0:
                    this.ventanaPorRegistro.TipoBusqueda = mx.gob.scjn.ius_common.gui.apoyos.PorRegistro.BUSQUEDA_REGISTRO_TESIS;
                    break;
                case 1:
                    this.ventanaPorRegistro.TipoBusqueda = mx.gob.scjn.ius_common.gui.apoyos.PorRegistro.BUSQUEDA_REGITRO_EJECUTORIAS;
                    break;
                case 2:
                    this.ventanaPorRegistro.TipoBusqueda = mx.gob.scjn.ius_common.gui.apoyos.PorRegistro.BUSQUEDA_REGISTRO_VOTOS;
                    break;
                case 3:
                case 4:
                    this.ventanaPorRegistro.TipoBusqueda = mx.gob.scjn.ius_common.gui.apoyos.PorRegistro.BUSQUEDA_REGISTRO_ACUERDOS;
                    break;
                default:
                    return;
            }
            this.ventanaPorRegistro.Visibility = Visibility.Visible;
            this.ventanaPorRegistro.registroActual.Focus();
        }

        private void Salir_MouseEnter(object sender, MouseEventArgs e)
        {
            Image imagenNueva = new Image();
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri("/General;component/images/Salir-2.png", UriKind.Relative);
            bitmap.EndInit();
            this.Salir.Source = bitmap;
        }

        private void Salir_MouseLeave(object sender, MouseEventArgs e)
        {
            Image imagenNueva = new Image();
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri("/General;component/images/Salir-1.png", UriKind.Relative);
            bitmap.EndInit();
            this.Salir.Source = bitmap;
        }

        private void Salir_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (Back == null)
            {
                this.NavigationService.RemoveBackEntry();
                PaginaInicial inicio = new PaginaInicial();
                this.NavigationService.Navigate(inicio);
                this.NavigationService.RemoveBackEntry();
            }
            else
            {
                this.NavigationService.Navigate(Back);
            }
        }

        private void PorPalabra_MouseEnter(object sender, MouseEventArgs e)
        {
            Image imagenNueva = new Image();
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri("/General;component/images/Bus-palabra2.png", UriKind.Relative);
            bitmap.EndInit();
            this.PorPalabra.Source = bitmap;
        }

        private void PorPalabra_MouseLeave(object sender, MouseEventArgs e)
        {
            Image imagenNueva = new Image();
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri("/General;component/images/Bus-palabra.png", UriKind.Relative);
            bitmap.EndInit();
            this.PorPalabra.Source = bitmap;
        }

        private void PorPalabra_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            System.Windows.Forms.Application.DoEvents();
            if (e.ClickCount == 1)
            {
                BusquedaTO parametros = new BusquedaTO();
                if (tipoDocumento.Text.Equals("Tesis"))
                {
                    controlesAcuerdos.actualizaValores();
                    parametros.Acuerdos = controlesAcuerdos.Valores;
                    controlesApendices.actualizaValores();
                    parametros.Apendices = controlesApendices.Valores;
                    controlesEpoca.actualizaValores();
                    parametros.Epocas = controlesEpoca.Valores;
                    parametros.TipoBusqueda = Constants.BUSQUEDA_TESIS_SIMPLE;
                    parametros.OrdenarPor = "ConsecIndx";
                    int totalSeleccionados = controlesEpoca.Seleccionados + controlesApendices.Seleccionados + controlesAcuerdos.Seleccionados;
                    if (totalSeleccionados == 0)
                    {
                        MessageBox.Show(Mensajes.MENSAJE_SELECIONE_UNA_CASILLA,
                            Mensajes.TITULO__SELECIONE_UNA_CASILLA, MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        return;
                    }
                    BuscaPalabras ventanaResultado = new BuscaPalabras(parametros);
                    ventanaResultado.Padre = this;
                    this.NavigationService.Navigate(ventanaResultado);
                    return;
                }
                else if (tipoDocumento.Text.Equals("Ejecutorias"))
                {
                    controlesAcuerdos.actualizaValores();
                    parametros.Acuerdos = controlesAcuerdos.Valores;
                    controlesApendices.actualizaValores();
                    parametros.Apendices = controlesApendices.Valores;
                    controlesEpoca.actualizaValores();
                    parametros.Epocas = controlesEpoca.Valores;
                    parametros.OrdenarPor = "ConsecIndx";
                    parametros.TipoBusqueda = Constants.BUSQUEDA_EJECUTORIAS;
                    int totalSeleccionados = controlesEpoca.Seleccionados + controlesApendices.Seleccionados + controlesAcuerdos.Seleccionados;
                    if (totalSeleccionados == 0)
                    {
                        MessageBox.Show(Mensajes.MENSAJE_SELECIONE_UNA_CASILLA,
                             Mensajes.TITULO__SELECIONE_UNA_CASILLA, MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        return;
                    }
                    BuscaPalabrasEjecutorias ventanaBusquedaEjecutorias = new BuscaPalabrasEjecutorias(parametros);
                    ventanaBusquedaEjecutorias.Padre = this;
                    this.NavigationService.Navigate(ventanaBusquedaEjecutorias);
                    return;
                }
                else if (tipoDocumento.Text.Equals("Votos"))
                {
                    controlesAcuerdos.actualizaValores();
                    parametros.Acuerdos = controlesAcuerdos.Valores;
                    controlesApendices.actualizaValores();
                    parametros.Apendices = controlesApendices.Valores;
                    controlesEpoca.actualizaValores();
                    parametros.Epocas = controlesEpoca.Valores;
                    parametros.OrdenarPor = "ConsecIndx";
                    parametros.TipoBusqueda = Constants.BUSQUEDA_VOTOS;
                    int totalSeleccionados = controlesEpoca.Seleccionados + controlesApendices.Seleccionados + controlesAcuerdos.Seleccionados;
                    if (totalSeleccionados == 0)
                    {
                        MessageBox.Show(Mensajes.MENSAJE_SELECIONE_UNA_CASILLA,
                             Mensajes.TITULO__SELECIONE_UNA_CASILLA, MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        return;
                    }
                    BuscaPalabrasVotos ventanaBusquedaVotos = new BuscaPalabrasVotos(parametros);
                    ventanaBusquedaVotos.Padre = this;
                    this.NavigationService.Navigate(ventanaBusquedaVotos);
                    return;
                }
                else if (tipoDocumento.Text.Equals("Acuerdos") || tipoDocumento.Text.Equals("Otros"))
                {
                    controlesAcuerdos.actualizaValores();
                    parametros.Acuerdos = controlesAcuerdos.Valores;
                    controlesApendices.actualizaValores();
                    parametros.Apendices = controlesApendices.Valores;
                    controlesEpoca.actualizaValores();
                    parametros.Epocas = controlesEpoca.Valores;
                    parametros.TipoBusqueda = Constants.BUSQUEDA_ACUERDO;
                    parametros.OrdenarPor = "ConsecIndx";
                    int totalSeleccionados = controlesEpoca.Seleccionados + controlesApendices.Seleccionados + controlesAcuerdos.Seleccionados;

                    if (totalSeleccionados == 0)
                    {
                        MessageBox.Show(Mensajes.MENSAJE_SELECIONE_UNA_CASILLA,
                         Mensajes.TITULO__SELECIONE_UNA_CASILLA, MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        return;
                    }
                    AcuerdoBuscaPalabras ventanaBusquedaAcuerdos = new AcuerdoBuscaPalabras(parametros);
                    ventanaBusquedaAcuerdos.Padre = this;
                    this.NavigationService.Navigate(ventanaBusquedaAcuerdos);
                    return;
                }
            }
        }
        private void Page_KeyDown(object sender, KeyEventArgs e)
        {
            //System.Windows.Forms.Application.DoEvents();
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

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Habilita();
            //this.Espera.Visibility = Visibility.Hidden;
        }

        private void Habilita()
        {
            Borde.IsEnabled = true;
            this.KeyDown += Page_KeyDown;
        }
        private void Inhabilita()
        {
            Borde.IsEnabled = false;
        }
        #region worker
        //This event is fired on the background thread, and is where you would do all your work 
        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            //Espera.Visibility = Visibility.Visible;
        }
        /// <summary>
        /// Define que hacer cuando va avanzando el progreso del trabajador.
        /// </summary>
        /// <param name="sender">Quien lo envia</param>
        /// <param name="e">Datos del progreso</param>
        private void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //EsperaBarra.Value = e.ProgressPercentage;
        }
        /// <summary>
        /// Que hacer cuando se termine el trabajo, en este caso la generación de los datos de impresión.
        /// </summary>
        /// <param name="sender">Quien lo envía</param>
        /// <param name="e">Datos de como se completó.</param>
        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                //Espera.Visibility = Visibility.Hidden;
            }
            else if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message, "Error al generar el documento", MessageBoxButton.OK, MessageBoxImage.Error);
                //Espera.Visibility = Visibility.Hidden;
            }
            else
            {
                //Espera.Visibility = Visibility.Visible;     //Get the result that was assigned 
            }
            Cursor = Cursors.Arrow;
        }
        #endregion 
    
        internal void BuscaAlmacenada(BusquedaAlmacenadaTO busquedaAlmacenadaTO)
        {
            if ((busquedaAlmacenadaTO.TipoBusqueda == Constants.BUSQUEDA_TESIS_SIMPLE)
                ||(busquedaAlmacenadaTO.TipoBusqueda==Constants.BUSQUEDA_TESIS_TEMATICA)
                || (busquedaAlmacenadaTO.TipoBusqueda == Constants.BUSQUEDA_ESPECIALES))
            {
                tablaResultado listaResultados = new tablaResultado(busquedaAlmacenadaTO);
                listaResultados.Back = this;
                if (listaResultados.tablaResultados.Items.Count != 0)
                {
                    this.NavigationService.Navigate(listaResultados);
                }
                else
                {
                    MessageBox.Show(Mensajes.MENSAJE_BUSQUEDA_VACIA, Mensajes.TITULO_BUSQUEDA_VACIA,
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        private void Recuperar_MouseEnter(object sender, MouseEventArgs e)
        {
            Image imagenNueva = new Image();
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri("/General;component/images/recuperar22.png", UriKind.Relative);
            bitmap.EndInit();
            this.Recuperar.Source = bitmap;
        }

        private void Recuperar_MouseLeave(object sender, MouseEventArgs e)
        {
            Image imagenNueva = new Image();
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri("/General;component/images/recuperar.png", UriKind.Relative);
            bitmap.EndInit();
            this.Recuperar.Source = bitmap;
        }

        private void Recuperar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.ListaBusquedas.Padre = this;
            this.ListaBusquedas.btnIncorporar.Visibility = Visibility.Hidden;
            if (BrowserInteropHelper.IsBrowserHosted)
            {
                if ((SeguridadUsuariosTO.UsuarioActual.Usuario == null) ||
                   (SeguridadUsuariosTO.UsuarioActual.Nombre == null) ||
                   (SeguridadUsuariosTO.UsuarioActual.Nombre.Equals("")))
                {
                    LoginRegistro login = new LoginRegistro();
                    login.Back = this;
                    this.NavigationService.Navigate(login);
                }
                else
                {
                    this.ListaBusquedas.Actualiza(SeguridadUsuariosTO.UsuarioActual.Usuario);
                    this.ListaBusquedas.btnIncorporar.Visibility = Visibility.Hidden;
                    if (ListaBusquedas.listado.Items.Count > 0)
                    {
                        this.ListaBusquedas.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        MessageBox.Show(Mensajes.MENSAJE_NO_BUSQUEDAS_ALMACENADAS,
                            Mensajes.TITULO_NO_BUSQUEDAS_ALMACENADAS,
                            MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    }
                }
            }
            else
            {
                this.ListaBusquedas.Actualiza(Constants.USUARIO_OMISION);
                if (ListaBusquedas.listado.Items.Count > 0)
                {
                    this.ListaBusquedas.Visibility = Visibility.Visible;
                    this.ListaBusquedas.btnIncorporar.Visibility = Visibility.Hidden;
                }
                else
                {
                    MessageBox.Show(Mensajes.MENSAJE_NO_BUSQUEDAS_ALMACENADAS,
                            Mensajes.TITULO_NO_BUSQUEDAS_ALMACENADAS,
                            MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
            }
        }
        private void CommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = Keyboard.FocusedElement is UIElement;
        }

        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            help.ShowHelpFor(Keyboard.FocusedElement as UIElement);
        }
    }
}
