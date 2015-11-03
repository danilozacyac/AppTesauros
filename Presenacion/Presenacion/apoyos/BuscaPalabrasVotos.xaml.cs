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
using mx.gob.scjn.ius_common.TO;
using IUS;
using mx.gob.scjn.ius_common.utils;
using mx.gob.scjn.ius_common.gui.utils;
using System.Windows.Interop;
using mx.gob.scjn.ius_common.gui.Guardar;

namespace mx.gob.scjn.ius_common.gui.apoyos
{
    /// <summary>
    /// Dialogo para la busqueda por palabras.
    /// </summary>
    public partial class BuscaPalabrasVotos : Page
    {
        BusquedaTO parametros;
        List<BusquedaPalabraTO> condiciones;
        List<BusquedaListaTO> listaDespliegue;
        public Page Padre { get; set; }
        public BuscaPalabrasVotos()
        {
            InitializeComponent();
        }
        public BuscaPalabrasVotos(BusquedaTO parametrosIniciales)
        {
            InitializeComponent();
            parametros = parametrosIniciales;
            condiciones = new List<BusquedaPalabraTO>();
            listaDespliegue = new List<BusquedaListaTO>();
            OpcionesBusca.ItemsSource = condiciones;
            Expresion.Text = CalculosGlobales.Expresion(parametrosIniciales).Replace("Lectura Secuencial:", Constants.CADENA_VACIA);
            Expresion.IsEnabled = false;
            Palabra.Focus();
        }

        private void Realizar_Click(object sender, RoutedEventArgs e)
        {
            if (listaDespliegue.Count == 0)
            {
                Incorpora_Click(sender, e);
            }
            if (listaDespliegue.Count == 0)
            {
                return;
            }
            if (!Palabra.Text.Equals(Constants.CADENA_VACIA))
            {
                MessageBoxResult respuesta = MessageBox.Show(Mensajes.MENSAJE_INCORPORAR_PALABRA,
                    Mensajes.TITULO_INCORPORAR, MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (respuesta == MessageBoxResult.Yes)
                {
                    Incorpora_Click(sender, e);
                }
                else if (listaDespliegue.Count == 0)
                {
                    return;
                }

            }
            Cursor = Cursors.Wait;
            if (condiciones.Count > 0)
            {
#if STAND_ALONE
                parametros.Palabra = condiciones;
                parametros.Clasificacion = new List<ClassificacionTO>();
#else
                parametros.Palabra = condiciones.ToArray();
                parametros.clasificacion=new ClassificacionTO[0];
#endif
                tablaResultadoVotos ventanaNueva = new tablaResultadoVotos(parametros);
                ventanaNueva.Back = this;
                int totales = 0;
                try
                {
                    totales = ventanaNueva.tablaResultadoTodos.Items.Count;// +
                          //ventanaNueva.tablaResultadoConcurrentes.Items.Count +
                          //ventanaNueva.tablaResultadoMinoritario.Items.Count +
                          //ventanaNueva.tablaResultadoParticulares.Items.Count;
                }
                catch (Exception exc)
                {
                    totales = 0;
                }
                if (totales == 0)
                {
                    MessageBox.Show(Mensajes.MENSAJE_BUSQUEDA_VACIA,
                        Mensajes.TITULO_BUSQUEDA_VACIA, MessageBoxButton.OK,
                        MessageBoxImage.Exclamation);
                    Cursor = Cursors.Arrow;
                    return;
                }
                NavigationService.Navigate(ventanaNueva);
                Cursor = Cursors.Arrow;
                NavigationService.Navigate(ventanaNueva);
            }
            else
            {
                MessageBox.Show(Mensajes.MENSAJE_CAMPO_REQUERIDO,
                    Mensajes.TITULO_CAMPO_REQUERIDO,
                    MessageBoxButton.OK, MessageBoxImage.Exclamation);
                Cursor = Cursors.Arrow;
            }
        }

        private void Regresar_Click(object sender, RoutedEventArgs e)
        {
            if (Padre == null)
            {
                NavigationService.Navigate(new Window1());
            }
            else
            {
                NavigationService.Navigate(Padre);
            }
        }

        private void Incorpora_Click(object sender, RoutedEventArgs e)
        {
            while (Palabra.Text.Contains("  "))
            {
                Palabra.Text = Palabra.Text.Replace("  ", " ");
            } 
            if (Validadores.BusquedaPalabraTexto(Palabra).Equals(Constants.CADENA_VACIA))
            {
                if (!(bool)Localizacion.IsChecked)
                {
                    if (this.Palabra.Text.Contains("."))
                    {
                        MessageBox.Show(Mensajes.MENSAJE_NO_PERMITIDOS, Mensajes.TITULO_NO_PERMITIDOS,
                            MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        return;
                    }
                }
                BusquedaPalabraTO nuevaCondicion = new BusquedaPalabraTO();
                BusquedaListaTO condicionEnEspanol = new BusquedaListaTO();
                List<int> campos = new List<int>();
                bool seleccionado = false;
                condicionEnEspanol.Seccion = "";
                if ((bool)Localizacion.IsChecked)
                {
                    campos.Add(Constants.BUSQUEDA_PALABRA_CAMPO_LOC);
                    seleccionado = true;
                    condicionEnEspanol.Seccion += " Loc. ";
                }
                if ((bool)Texto.IsChecked)
                {
                    campos.Add(Constants.BUSQUEDA_PALABRA_CAMPO_TEXTO);
                    seleccionado = true;
                    condicionEnEspanol.Seccion += " Texto ";
                }
                if ((bool)Asunto.IsChecked)
                {
                    campos.Add(Constants.BUSQUEDA_PALABRA_CAMPO_ASUNTO);
                    seleccionado = true;
                    condicionEnEspanol.Seccion += " Asunto ";
                }
                if ((bool)Emisor.IsChecked)
                {
                    campos.Add(Constants.BUSQUEDA_PALABRA_CAMPO_EMISOR);
                    seleccionado = true;
                    condicionEnEspanol.Seccion += " Emisor ";
                }
                if (!seleccionado)
                {
                    MessageBox.Show("Debe seleccionar por lo menos un campo para su consulta");
                    return;
                }
                if ((Palabra.Text == null) || (Palabra.Text.Trim().Equals("")))
                {
                    MessageBox.Show("Debe tener un término de búsqueda en el texto");
                    Palabra.Focus();
                    return;
                }
#if STAND_ALONE
                nuevaCondicion.Campos = campos;
#else
                nuevaCondicion.Campos = campos.ToArray();
#endif
                nuevaCondicion.Expresion = CalculosGlobales.SeparaExpresiones(Palabra.Text);
                condicionEnEspanol.Expresion = Palabra.Text;
                condicionEnEspanol.Epocas = CalculosGlobales.EtiquetaEpocas(parametros);
                int juris = Constants.BUSQUEDA_PALABRA_AMBAS;
                nuevaCondicion.Jurisprudencia = juris;
                switch (this.txtOperador.SelectedIndex)
                {
                    case 0:
                        nuevaCondicion.ValorLogico = Constants.BUSQUEDA_PALABRA_OP_Y;
                        condicionEnEspanol.Operador = "Y";
                        break;
                    case 1:
                        nuevaCondicion.ValorLogico = Constants.BUSQUEDA_PALABRA_OP_O;
                        condicionEnEspanol.Operador = "O";
                        break;
                    case 2:
                        nuevaCondicion.ValorLogico = Constants.BUSQUEDA_PALABRA_OP_NO;
                        condicionEnEspanol.Operador = "NO";
                        break;
                }
                condiciones.Add(nuevaCondicion);
                this.listaDespliegue.Add(condicionEnEspanol);
                this.listaDespliegue.ElementAt(0).Operador = "";
                OpcionesBusca.ItemsSource = null;
                OpcionesBusca.ItemsSource = listaDespliegue;
                OpcionesBusca.SelectedIndex = -1;
                OpcionesBusca.SelectedIndex = listaDespliegue.Count - 1;
                lblOperador.Visibility = Visibility.Visible;
                txtOperador.Visibility = Visibility.Visible;
                Palabra.Text = "";
            }
        }
        

        private void Eliminar_Click(object sender, RoutedEventArgs e)
        {
            if (OpcionesBusca.SelectedItem != null)
            {
                List<BusquedaPalabraTO> lista = new List<BusquedaPalabraTO>();
                int contador = 0;
                foreach (BusquedaPalabraTO item in condiciones)
                {
                    if (contador != OpcionesBusca.SelectedIndex)
                    {
                        lista.Add((BusquedaPalabraTO)item);
                    }
                    contador++;
                }
                condiciones = lista;
                List<BusquedaListaTO> listaDes = new List<BusquedaListaTO>();
                contador = 0;
                foreach (BusquedaListaTO item in this.listaDespliegue)
                {
                    if (contador != OpcionesBusca.SelectedIndex)
                    {
                        listaDes.Add((BusquedaListaTO)item);
                    }
                    contador++;
                }
                OpcionesBusca.ItemsSource = listaDes;
                listaDespliegue = listaDes;
                if (listaDes.Count > 0)
                {
                    listaDes.ElementAt(0).Operador = "";
                    this.OpcionesBusca.SelectedIndex = 0;
                }
                if (condiciones.Count == 0)
                {
                    this.lblOperador.Visibility = Visibility.Hidden;
                    this.txtOperador.SelectedIndex = 0;
                    this.txtOperador.Visibility = Visibility.Hidden;
                }
            }
        }

        private void Copiar_Click(object sender, RoutedEventArgs e)
        {
            if (OpcionesBusca.SelectedItem != null)
            {
                BusquedaListaTO objetoSeleccionado = (BusquedaListaTO)OpcionesBusca.SelectedItem;
                this.Palabra.Text = objetoSeleccionado.Expresion;
            }
        }

        private void Palabra_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Realizar_Click(sender, e);
            }
        }

        private void Ver_Click(object sender, RoutedEventArgs e)
        {
            if (BrowserInteropHelper.IsBrowserHosted)
            {
                if (SeguridadUsuariosTO.UsuarioActual == null || SeguridadUsuariosTO.UsuarioActual.Nombre == null)
                {
                    LoginRegistro login = new LoginRegistro();
                    login.Back = this;
                    this.NavigationService.Navigate(login);
                    return;
                }
            }
            else
            {
                UsuarioTO usuarioOmision = new UsuarioTO();
                usuarioOmision.Usuario = Constants.USUARIO_OMISION;
                SeguridadUsuariosTO.UsuarioActual = usuarioOmision;
            }
            this.ListaBusquedas.TipoBotones = ListaBusquedasAlmacenadas.BOTONES_VER;
            this.ListaBusquedas.Actualiza(SeguridadUsuariosTO.UsuarioActual.Usuario);
            this.ListaBusquedas.Padre = this;
            if (ListaBusquedas.listado.Items.Count > 0)
            {
                this.ListaBusquedas.Visibility = Visibility.Visible;
            }
            else
            {
                MessageBox.Show(Mensajes.MENSAJE_NO_BUSQUEDAS_ALMACENADAS, Mensajes.TITULO_NO_BUSQUEDAS_ALMACENADAS,
                    MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private void Almacena_Click(object sender, RoutedEventArgs e)
        {
            UsuarioTO usuarioOmision = null;
            if (!BrowserInteropHelper.IsBrowserHosted)
            {
                usuarioOmision = new UsuarioTO();
                usuarioOmision.Usuario = Constants.USUARIO_OMISION;
                SeguridadUsuariosTO.UsuarioActual = usuarioOmision;
            }
            if ((SeguridadUsuariosTO.UsuarioActual == null)
                || ((SeguridadUsuariosTO.UsuarioActual.Usuario == null))
                || (SeguridadUsuariosTO.UsuarioActual.Usuario.Equals("")))
            {
                LoginRegistro login = new LoginRegistro();
                login.Back = this;
                this.NavigationService.Navigate(login);
                return;
            }
            if ((Palabra.Text != null) && (!Palabra.Text.Equals("")))
            {
                if ((bool)Localizacion.IsChecked || (bool)Emisor.IsChecked
                    || (bool)Texto.IsChecked || (bool)Asunto.IsChecked)
                {
                    this.AlmacenaExpresion.Padre = this;
                    this.AlmacenaExpresion.ActualizaVentana(parametros, null);
                    this.AlmacenaExpresion.Visibility = Visibility.Visible;
                }
                else
                {
                    MessageBox.Show(Mensajes.MENSAJE_CAMPO_REQUERIDO, Mensajes.TITULO_CAMPO_REQUERIDO,
                        MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
            }
            else
            {
                MessageBox.Show(Mensajes.MENSAJE_CAMPO_REQUERIDO, Mensajes.TITULO_CAMPO_REQUERIDO,
                    MessageBoxButton.OK, MessageBoxImage.Exclamation);
                Palabra.Focus();
            }
        }


        /// <summary>
        /// Toma la primer expresión de la búsqueda seleccionada y la pone dentro de los textos.
        /// </summary>
        /// <param name="busqueda">La búsqueda Seleccionada</param>
        public void SeleccionaBusquedaAlmacenada(BusquedaAlmacenadaTO busqueda)
        {
#if STAND_ALONE
            if (busqueda.Expresiones == null || busqueda.Expresiones.Count == 0)
#else
            if (busqueda.Expresiones == null || busqueda.Expresiones.Length == 0)
#endif
            {
                MessageBox.Show(Mensajes.MENSAJE_BUSQUEDA_SIN_EXPRESIONES, Mensajes.TITULO_BUSQUEDA_SIN_EXPRESIONES,
                    MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            this.Palabra.Text = busqueda.Expresiones[0].Expresion.Replace(Constants.SEPARADOR_FRASES, " ");
            this.Texto.IsChecked = busqueda.Expresiones[0].Campos.Contains(Constants.BUSQUEDA_PALABRA_CAMPO_TEXTO);
            this.Emisor.IsChecked = busqueda.Expresiones[0].Campos.Contains(Constants.BUSQUEDA_PALABRA_CAMPO_EMISOR);
            this.Asunto.IsChecked = busqueda.Expresiones[0].Campos.Contains(Constants.BUSQUEDA_PALABRA_CAMPO_ASUNTO);
            this.Localizacion.IsChecked = busqueda.Expresiones[0].Campos.Contains(Constants.BUSQUEDA_PALABRA_CAMPO_LOC);
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Mensajes.TOPICO_AYUDA = "busqueda_por_palabra.htm";
        }
    }
}