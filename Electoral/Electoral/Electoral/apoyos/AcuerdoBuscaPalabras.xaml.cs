﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using mx.gob.scjn.electoral;
using mx.gob.scjn.ius_common.TO;
using mx.gob.scjn.ius_common.gui.utils;
using mx.gob.scjn.ius_common.utils;

namespace mx.gob.scjn.electoral_common.gui.apoyos
{
    /// <summary>
    /// Dialogo para la busqueda por palabras.
    /// </summary>
    public partial class AcuerdoBuscaPalabras : Page
    {
        BusquedaTO parametros;
        List<BusquedaPalabraTO> condiciones;
        List<BusquedaListaTO> listaDespliegue;
        public Page Padre { get; set; }
        public AcuerdoBuscaPalabras()
        {
            InitializeComponent();
        }
        public AcuerdoBuscaPalabras(BusquedaTO parametrosIniciales)
        {
            InitializeComponent();
            parametros = parametrosIniciales;
            condiciones = new List<BusquedaPalabraTO>();
            listaDespliegue = new List<BusquedaListaTO>();
            OpcionesBusca.ItemsSource = condiciones;
            Expresion.Text = "Acuerdos electorales";
            Expresion.Text = Expresion.Text.Replace("Lectura Secuencial:", "");
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
            if (!Palabra.Text.Equals(""))
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
#else
                parametros.Palabra = condiciones.ToArray();
#endif
                TablaResultadoAcuerdos ventanaNueva = new TablaResultadoAcuerdos(parametros);
                ventanaNueva.Back = this;
                if ((ventanaNueva.tablaResultado.Items == null) ||
                    (ventanaNueva.tablaResultado.Items.Count == 0))
                {
                    MessageBox.Show(Mensajes.MENSAJE_BUSQUEDA_VACIA,
                        Mensajes.TITULO_BUSQUEDA_VACIA, MessageBoxButton.OK,
                        MessageBoxImage.Exclamation);
                    Cursor = Cursors.Arrow;
                    return;
                }
                Cursor = Cursors.Arrow;
                NavigationService.Navigate(ventanaNueva);
            }
            else
            {
                MessageBox.Show(Mensajes.MENSAJE_CAMPO_REQUERIDO, Mensajes.TITULO_CAMPO_REQUERIDO,
                    MessageBoxButton.OK,MessageBoxImage.Exclamation);
                Cursor = Cursors.Arrow;
            }
        }

        private void Regresar_Click(object sender, RoutedEventArgs e)
        {
            if (Padre == null)
            {
                NavigationService.Navigate(new MenuElectoral());
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
            if (Validadores.BusquedaPalabraTexto(this.Palabra).Equals(""))
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
                if ((bool)Rubro.IsChecked)
                {
                    campos.Add(Constants.BUSQUEDA_PALABRA_CAMPO_TEMA);
                    seleccionado = true;
                    condicionEnEspanol.Seccion += " Tema ";
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
                condicionEnEspanol.Epocas = Expresion.Text;
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
                condicionEnEspanol.Epocas = "Acuerdos Electorales";
                listaDespliegue.Add(condicionEnEspanol);
                listaDespliegue.ElementAt(0).Operador = "";
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
                    OpcionesBusca.SelectedIndex = 0;
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

        //private void Ver_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        //{
        //    if (BrowserInteropHelper.IsBrowserHosted)
        //    {
        //        if (SeguridadUsuariosTO.UsuarioActual == null || SeguridadUsuariosTO.UsuarioActual.Nombre == null)
        //        {
        //            LoginRegistro login = new LoginRegistro();
        //            login.Back = this;
        //            this.NavigationService.Navigate(login);
        //            return;
        //        }
        //    }
        //    else
        //    {
        //        UsuarioTO usuarioOmision = new UsuarioTO();
        //        usuarioOmision.Usuario = Constants.USUARIO_OMISION;
        //        SeguridadUsuariosTO.UsuarioActual=usuarioOmision;
        //    }
        //    this.ListaBusquedas.Actualiza(SeguridadUsuariosTO.UsuarioActual.Usuario);
        //    this.ListaBusquedas.Padre = this;
        //    if (ListaBusquedas.listado.Items.Count > 0)
        //    {
        //        this.ListaBusquedas.Visibility = Visibility.Visible;
        //    }
        //    else
        //    {
        //        MessageBox.Show(Mensajes.MENSAJE_NO_BUSQUEDAS_ALMACENADAS,
        //            Mensajes.TITULO_NO_BUSQUEDAS_ALMACENADAS, MessageBoxButton.OK,
        //            MessageBoxImage.Exclamation);
        //    }
        //}

        internal void BuscaAlmacenada(BusquedaAlmacenadaTO busquedaAlmacenadaTO)
        {
            MessageBox.Show("Encontrada");
        }

        private void Palabra_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Realizar_Click(sender, e);
            }
        }

    }
}
