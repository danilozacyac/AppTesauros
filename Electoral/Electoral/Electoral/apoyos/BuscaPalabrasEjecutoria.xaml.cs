using System;
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
    public partial class BuscaPalabrasEjecutorias : Page
    {
        BusquedaTO parametros;
        List<BusquedaPalabraTO> condiciones;
        List<BusquedaListaTO> listaDespliegue;
        public Page Padre { get; set; }
        public BuscaPalabrasEjecutorias()
        {
            InitializeComponent();
        }
        public BuscaPalabrasEjecutorias(BusquedaTO parametrosIniciales)
        {
            InitializeComponent();
            parametros = parametrosIniciales;
            condiciones = new List<BusquedaPalabraTO>();
            listaDespliegue = new List<BusquedaListaTO>();
            OpcionesBusca.ItemsSource = condiciones;
            Expresion.Text = "Sentencias electorales";
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
                TablaResultadoEjecutoria ventanaNueva = new TablaResultadoEjecutoria(parametros);
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
                if ((bool)Asunto.IsChecked)
                {
                    campos.Add(Constants.BUSQUEDA_PALABRA_CAMPO_ASUNTO);
                    seleccionado = true;
                    condicionEnEspanol.Seccion += " Asunto ";
                }
                if ((bool)Tema.IsChecked)
                {
                    campos.Add(Constants.BUSQUEDA_PALABRA_CAMPO_TEMA);
                    seleccionado = true;
                    condicionEnEspanol.Seccion += " Tema ";
                }
                if (!seleccionado)
                {
                    MessageBox.Show(Mensajes.MENSAJE_CAMPO_NO_SELECCIONADO,
                        Mensajes.TITULO_CAMPO_NO_SELECCIONADO,
                        MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }
                if ((Palabra.Text == null) || (Palabra.Text.Trim().Equals("")))
                {
                    MessageBox.Show(Mensajes.MENSAJE_CAMPO_TEXTO_VACIO,
                        Mensajes.TITULO_CAMPO_TEXTO_VACIO,
                        MessageBoxButton.OK, MessageBoxImage.Exclamation);
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
                        condicionEnEspanol.Operador = "N";
                        break;
                }
                condicionEnEspanol.Epocas = "Sentencias";//CalculosGlobales.EtiquetaEpocas(parametros);
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
                if (listaDespliegue.Count > 0)
                {
                    listaDespliegue.ElementAt(0).Operador = "";
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

    }
}
