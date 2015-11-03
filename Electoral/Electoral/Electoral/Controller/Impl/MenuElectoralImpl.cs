using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using General.gui.utilities;
using mx.gob.scjn.electoral_common.gui.apoyos;
using mx.gob.scjn.ius_common.TO;
using mx.gob.scjn.ius_common.fachade;
using mx.gob.scjn.ius_common.gui.panel;
using mx.gob.scjn.ius_common.gui.utils;
using mx.gob.scjn.ius_common.utils;

namespace mx.gob.scjn.electoral.Controller.Impl
{
    class MenuElectoralImpl:IMenuElectoral
    {
        private PanelDinamico PnlTesis { get; set; }
        private PanelDinamico PnlAcuerdos { get; set; }
        private PanelDinamico PnlEjecutorias { get; set; }
        private PanelDinamico PnlVotos { get; set; }

        #region IMenuElectoral Members
        public MenuElectoral Ventana
        {
            get
            {
                return ventana;
            }
            set
            {
                ventana = value;
            }
        }

        private MenuElectoral ventana { get; set; }
        public UIElement Back
        {
            get
            {
                return back;
            }
            set
            {
                back=value;
            }
        }
        private UIElement back { get; set; }
        public void InicializaPaneles()
        {
            BCInitTO parametros = new BCInitTO();
            BCInitTO parametros2 = new BCInitTO();
            Brush colorLetras = Brushes.Maroon;
            parametros.Orientacion = BCInitTO.HORIZONTAL;
            parametros.Prefijo = "Tesis";
            parametros.TextoBotonesH = new String[2];
            parametros.TextoBotonesH[0] = "Sala Superior";
            parametros.TextoBotonesH[1] = "Salas Regionales";
            parametros.TextoBotonesV = new String[2];
            parametros.TextoBotonesV[0] = "3a. Época";
            parametros.TextoBotonesV[1] = "4a. Época";
            PnlTesis=new PanelDinamico(parametros);
            PnlTesis.BchControles.GetCheckBoxes()[0][1].IsEnabled = false;
            PnlTesis.BchControles.GetCheckBoxes()[0][1].Visibility = Visibility.Hidden;
            PnlTesis.Foreground = colorLetras;
            PnlTesis.BchControles.todos_click(null, null);
            Ventana.CnvLugarParaCbxs.Children.Add(PnlTesis);
            parametros2 = new BCInitTO();
            parametros2.Prefijo = "Ejecutoria";
            parametros2.TextoBotonesH = new String[1];
            parametros2.TextoBotonesH[0] = "Sala Superior";
            parametros2.TextoBotonesV = new String[1];
            parametros2.TextoBotonesV[0] = "Todo";
            PnlEjecutorias = new PanelDinamico(parametros2);
            PnlEjecutorias.Foreground = colorLetras;
            PnlVotos = new PanelDinamico(parametros2);
            PnlVotos.Foreground = colorLetras;
            PnlAcuerdos = new PanelDinamico(parametros);
            PnlAcuerdos.Foreground = colorLetras;
        }

        public void CbxTipoDocChanged()
        {
            Ventana.CnvLugarParaCbxs.Children.Clear();
            if (Ventana.CbxTipoDoc.SelectedIndex != 0)
            {
                Ventana.CbxJurisprudencia.Visibility = Visibility.Collapsed;
                Ventana.CbxRelevantes.Visibility = Visibility.Collapsed;
            }
            if (Ventana.CbxTipoDoc.SelectedIndex == 0)
            {
                Ventana.CbxJurisprudencia.Visibility = Visibility.Visible;
                Ventana.CbxRelevantes.Visibility = Visibility.Visible;
                Ventana.CnvLugarParaCbxs.Children.Add(PnlTesis);
                PnlTesis.BchControles.todos_click(null, null);
                if (!(bool)PnlTesis.BchControles.GetCheckBoxes()[0][0].IsChecked)
                {
                    PnlTesis.BchControles.todos_click(null, null);
                }
            }
            else if (Ventana.CbxTipoDoc.SelectedIndex == 1)
            {
                Ventana.CnvLugarParaCbxs.Children.Add(PnlEjecutorias);
                PnlEjecutorias.BchControles.todos_click(null, null);
                if (!(bool)PnlEjecutorias.BchControles.GetCheckBoxes()[0][0].IsChecked)
                {
                    PnlEjecutorias.BchControles.todos_click(null, null);
                }
            }
            else if (Ventana.CbxTipoDoc.SelectedIndex == 2)
            {
                Ventana.CnvLugarParaCbxs.Children.Add(PnlVotos);
                PnlVotos.BchControles.todos_click(null, null);
                if (!(bool)PnlVotos.BchControles.GetCheckBoxes()[0][0].IsChecked)
                {
                    PnlVotos.BchControles.todos_click(null, null);
                }
            }
            else if (Ventana.CbxTipoDoc.SelectedIndex == 3)
            {
                Ventana.CnvLugarParaCbxs.Children.Add(PnlAcuerdos);
                PnlAcuerdos.BchControles.todos_click(null, null);
                if (!(bool)PnlAcuerdos.BchControles.GetCheckBoxes()[0][0].IsChecked)
                {
                    PnlAcuerdos.BchControles.todos_click(null, null);
                }
            }
        }

        public void BtnSecuencialClic()
        {
            int seleccionado = Ventana.CbxTipoDoc.SelectedIndex;
            switch (seleccionado)
            {
                case 0:
                    BusquedaTO parametros = new BusquedaTO();
                    parametros.TipoBusqueda = Constants.BUSQUEDA_TESIS_SIMPLE;
                    PnlTesis.BchControles.actualizaValores();
                    parametros.OrdenarPor = Constants.ORDER_DEFAULT;
                    parametros.Epocas = PnlTesis.BchControles.getValores();
                    int contador = 0;
                    foreach (bool[] itemIni in parametros.Epocas)
                    {
                        foreach (bool item in itemIni)
                        {
                            if (item) contador++;
                        }
                    }
                    if (contador == 0)
                    {
                        MessageBox.Show(Mensajes.MENSAJE_CASILLAS_SIN_SELECCIONAR_ELE, Mensajes.TITULO_ADVERTENCIA,
                            MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        return;
                    }
#if STAND_ALONE
                    parametros.Clasificacion = new List<ClassificacionTO>();
#else
                    parametros.clasificacion=new ClassificacionTO[1];
#endif
                    ClassificacionTO juris = new ClassificacionTO();
                    juris.Activo = ((bool)this.ventana.CbxJurisprudencia.IsChecked ? 1 : 0) +
                        ((bool)this.ventana.CbxRelevantes.IsChecked ? 2 : 0);
                    if (juris.Activo == 0)
                    {
                        MessageBox.Show(Mensajes.MENSAJE_CAMPO_REQUERIDO, Mensajes.TITULO_CAMPO_REQUERIDO,
                            MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
#if STAND_ALONE
                    parametros.Clasificacion.Add(juris);
                    FachadaBusquedaTradicional fachada = new FachadaBusquedaTradicional();
#else
                    parametros.clasificacion[0]=juris;
                    FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
#endif
                    TablaResultados VentanaResultados = new TablaResultados(parametros);
                    VentanaResultados.Back = this.Ventana;
                    if ((VentanaResultados.tablaResultados.Items == null) ||
                        (VentanaResultados.tablaResultados.Items.Count == 0))
                    {
                        MessageBox.Show(Mensajes.MENSAJE_BUSQUEDA_VACIA, Mensajes.TITULO_BUSQUEDA_VACIA,
                            MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }
                    Ventana.NavigationService.Navigate(VentanaResultados);
                    break;
                case 1:
                    parametros = new BusquedaTO();
                    PnlEjecutorias.BchControles.actualizaValores();
                    parametros.Epocas = PnlEjecutorias.BchControles.Valores;
                    parametros.OrdenarPor = "ConsecIndx";
                    parametros.TipoBusqueda = Constants.BUSQUEDA_EJECUTORIAS;
                    int totalSeleccionados = PnlEjecutorias.BchControles.Seleccionados;
                    if (totalSeleccionados > 1)
                    {
                        MessageBox.Show(Mensajes.MENSAJE_CASILLAS_SIN_SELECCIONAR_ELE, Mensajes.TITULO_CASILLAS_SIN_SELECCIONAR,
                            MessageBoxButton.OK, MessageBoxImage.Error);
                        //Habilita();
                        //this.Espera.Visibility = Visibility.Hidden;
                        Ventana.Cursor = Cursors.Arrow;
                        return;
                    }
                    else if (totalSeleccionados < 1)
                    {
                        MessageBox.Show(Mensajes.MENSAJE_CASILLAS_SIN_SELECCIONAR_ELE, Mensajes.TITULO_CASILLAS_SIN_SELECCIONAR,
                            MessageBoxButton.OK, MessageBoxImage.Error);
                        //this.Espera.Visibility = Visibility.Hidden;
                        //Habilita();
                        Ventana.Cursor = Cursors.Arrow;
                        return;
                    }
                    TablaResultadoEjecutoria ventanaResultado = new TablaResultadoEjecutoria(parametros);
                    ventanaResultado.Back = Ventana;
                    if (ventanaResultado.tablaResultado.Items.Count <= 0)
                    {
                        MessageBox.Show(Mensajes.MENSAJE_BUSQUEDA_VACIA,
                            Mensajes.TITULO_BUSQUEDA_VACIA, MessageBoxButton.OK,
                            MessageBoxImage.Exclamation);
                        //Habilita();
                        Ventana.Cursor = Cursors.Arrow;
                        return;
                    }
                    Ventana.NavigationService.Navigate(ventanaResultado);
                    break;
                case 2:
                    parametros = new BusquedaTO();
                    PnlVotos.BchControles.actualizaValores();
                    parametros.Epocas = PnlVotos.BchControles.Valores;
                    parametros.OrdenarPor = "ConsecIndx";
                    parametros.TipoBusqueda = Constants.BUSQUEDA_VOTOS;
                    totalSeleccionados = PnlVotos.BchControles.Seleccionados;
                    if (totalSeleccionados > 1)
                    {
                        MessageBox.Show(Mensajes.MENSAJE_CASILLAS_SIN_SELECCIONAR_ELE, Mensajes.TITULO_CASILLAS_SIN_SELECCIONAR,
                            MessageBoxButton.OK, MessageBoxImage.Error);
                        //this.Espera.Visibility = Visibility.Hidden;
                        Ventana.Cursor = Cursors.Arrow;
                        //Habilita();
                        return;
                    }
                    else if (totalSeleccionados < 1)
                    {
                        MessageBox.Show(Mensajes.MENSAJE_CASILLAS_SIN_SELECCIONAR_ELE, Mensajes.TITULO_CASILLAS_SIN_SELECCIONAR,
                            MessageBoxButton.OK, MessageBoxImage.Error);
                        //this.Espera.Visibility = Visibility.Hidden;
                        Ventana.Cursor = Cursors.Arrow;
                        //Habilita();
                        return;
                    }
                    TablaResultadoVotos ventanaResultadoVoto = new TablaResultadoVotos(parametros);
                    ventanaResultadoVoto.Back = Ventana;
                    if (ventanaResultadoVoto.tablaResultado.Items.Count <= 0)
                    {
                        MessageBox.Show(Mensajes.MENSAJE_BUSQUEDA_VACIA,
                            Mensajes.TITULO_BUSQUEDA_VACIA, MessageBoxButton.OK,
                            MessageBoxImage.Exclamation);
                        Ventana.Cursor = Cursors.Arrow;
                        //Habilita();
                        return;
                    }
                    Ventana.NavigationService.Navigate(ventanaResultadoVoto);
                    break;
                case 3:
                    parametros = new BusquedaTO();
                    PnlAcuerdos.BchControles.actualizaValores();
                    parametros.Acuerdos = PnlAcuerdos.BchControles.Valores;
                    parametros.TipoBusqueda = Constants.BUSQUEDA_ACUERDO;
                    parametros.OrdenarPor = "ConsecIndx";
                    totalSeleccionados = PnlAcuerdos.BchControles.Seleccionados;
                    if (totalSeleccionados < 1)
                    {
                        MessageBox.Show(Mensajes.MENSAJE_CASILLAS_SIN_SELECCIONAR_ELE, Mensajes.TITULO_CASILLAS_SIN_SELECCIONAR,
                            MessageBoxButton.OK, MessageBoxImage.Error);
                        //this.Espera.Visibility = Visibility.Hidden;
                        Ventana.Cursor = Cursors.Arrow;
                        //Habilita();
                        return;
                    }
                    TablaResultadoAcuerdos ventanaResultadoAc = new TablaResultadoAcuerdos(parametros);
                    //this.Espera.Visibility = Visibility.Hidden;
                    ventanaResultadoAc.Back = Ventana;
                    if (ventanaResultadoAc.tablaResultado.Items.Count <= 0)
                    {
                        MessageBox.Show(Mensajes.MENSAJE_BUSQUEDA_VACIA,
                            Mensajes.TITULO_BUSQUEDA_VACIA, MessageBoxButton.OK,
                            MessageBoxImage.Exclamation);
                        Ventana.Cursor = Cursors.Arrow;
                        //Habilita();
                        return;
                    }
                    Ventana.NavigationService.Navigate(ventanaResultadoAc);
                    break;
            }
            Ventana.Cursor = Cursors.Arrow;
        }

        public void BtnRegistroClic()
        {
            Ventana.ventanaPorRegistro.NavigationService = Ventana.NavigationService;
            int seleccion = Ventana.CbxTipoDoc.SelectedIndex;
            switch (seleccion)
            {
                case 0:
                    Ventana.ventanaPorRegistro.TipoBusqueda = mx.gob.scjn.electoral_common.gui.apoyos.PorRegistro.BUSQUEDA_REGISTRO_TESIS;
                    break;
                case 1:
                    Ventana.ventanaPorRegistro.TipoBusqueda = mx.gob.scjn.electoral_common.gui.apoyos.PorRegistro.BUSQUEDA_REGITRO_EJECUTORIAS;
                    break;
                case 2:
                    Ventana.ventanaPorRegistro.TipoBusqueda = mx.gob.scjn.electoral_common.gui.apoyos.PorRegistro.BUSQUEDA_REGISTRO_VOTOS;
                    break;
                case 3:
                case 4:
                    Ventana.ventanaPorRegistro.TipoBusqueda = mx.gob.scjn.electoral_common.gui.apoyos.PorRegistro.BUSQUEDA_REGISTRO_ACUERDOS;
                    break;
                default:
                    return;
            }
            
            
            Ventana.ventanaPorRegistro.Visibility = Visibility.Visible;
            Ventana.ventanaPorRegistro.registroActual.Focus();
        }

        public void BtnPalabraClic(MouseButtonEventArgs e)
        {
            if (e.ClickCount == 1)
            {
                BusquedaTO parametros = new BusquedaTO();
                if (Ventana.CbxTipoDoc.Text.Equals("Tesis"))
                {
                    PnlTesis.BchControles.actualizaValores();
                    parametros.Epocas = PnlTesis.BchControles.Valores;
                    parametros.TipoBusqueda = Constants.BUSQUEDA_TESIS_SIMPLE;
                    parametros.OrdenarPor = "ConsecIndx";
                    int totalSeleccionados = PnlTesis.BchControles.Seleccionados;
                    if (totalSeleccionados == 0)
                    {
                        MessageBox.Show(Mensajes.MENSAJE_CASILLAS_SIN_SELECCIONAR_ELE,
                            Mensajes.TITULO__SELECIONE_UNA_CASILLA, MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        return;
                    }
                    BuscaPalabras ventanaResultado = new BuscaPalabras(parametros);
                    ventanaResultado.Padre = Ventana;
                    Ventana.NavigationService.Navigate(ventanaResultado);
                    return;
                }
                else if (Ventana.CbxTipoDoc.Text.Equals("Sentencias"))
                {
                    PnlEjecutorias.BchControles.actualizaValores();
                    parametros.Epocas = PnlEjecutorias.BchControles.Valores;
                    parametros.OrdenarPor = "ConsecIndx";
                    parametros.TipoBusqueda = Constants.BUSQUEDA_EJECUTORIAS;
                    int totalSeleccionados = PnlEjecutorias.BchControles.Seleccionados;
                    if (totalSeleccionados == 0)
                    {
                        MessageBox.Show(Mensajes.MENSAJE_CASILLAS_SIN_SELECCIONAR_ELE,
                             Mensajes.TITULO__SELECIONE_UNA_CASILLA, MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        return;
                    }
                    BuscaPalabrasEjecutorias ventanaBusquedaEjecutorias = new BuscaPalabrasEjecutorias(parametros);
                    ventanaBusquedaEjecutorias.Padre = Ventana;
                    Ventana.NavigationService.Navigate(ventanaBusquedaEjecutorias);
                    return;
                }
                else if (Ventana.CbxTipoDoc.Text.Equals("Votos"))
                {
                    PnlVotos.BchControles.actualizaValores();
                    parametros.Epocas = PnlVotos.BchControles.Valores;
                    parametros.OrdenarPor = "ConsecIndx";
                    parametros.TipoBusqueda = Constants.BUSQUEDA_VOTOS;
                    int totalSeleccionados = PnlVotos.BchControles.Seleccionados;
                    if (totalSeleccionados == 0)
                    {
                        MessageBox.Show(Mensajes.MENSAJE_CASILLAS_SIN_SELECCIONAR_ELE,
                             Mensajes.TITULO__SELECIONE_UNA_CASILLA, MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        return;
                    }
                    BuscaPalabrasVotos ventanaBusquedaVotos = new BuscaPalabrasVotos(parametros);
                    ventanaBusquedaVotos.Padre = Ventana;
                    Ventana.NavigationService.Navigate(ventanaBusquedaVotos);
                    return;
                }
                else if (Ventana.CbxTipoDoc.Text.Equals("Acuerdos"))
                {
                    PnlAcuerdos.BchControles.actualizaValores();
                    parametros.Acuerdos = PnlAcuerdos.BchControles.Valores;
                    parametros.TipoBusqueda = Constants.BUSQUEDA_ACUERDO;
                    parametros.OrdenarPor = "ConsecIndx";
                    int totalSeleccionados = PnlAcuerdos.BchControles.Seleccionados;

                    if (totalSeleccionados == 0)
                    {
                        MessageBox.Show(Mensajes.MENSAJE_CASILLAS_SIN_SELECCIONAR_ELE,
                         Mensajes.TITULO__SELECIONE_UNA_CASILLA, MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        return;
                    }
                    AcuerdoBuscaPalabras ventanaBusquedaAcuerdos = new AcuerdoBuscaPalabras(parametros);
                    ventanaBusquedaAcuerdos.Padre = Ventana;
                    Ventana.NavigationService.Navigate(ventanaBusquedaAcuerdos);
                    return;
                }
            }
        }

        public void BtnSalirClic()
        {
            ventana.NavigationService.Navigate(Back);
        }

        #endregion
    }
}
