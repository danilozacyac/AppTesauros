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
    public partial class BuscaPalabras : Page
    {
        BusquedaTO parametros;
        List<BusquedaPalabraTO> condiciones;
        List<BusquedaListaTO> listaDespliegue;
        public Page Padre { get; set; }
        public BuscaPalabras()
        {
            InitializeComponent();
        }
        public BuscaPalabras(BusquedaTO parametrosIniciales)
        {
            InitializeComponent();
            parametros = parametrosIniciales;
            condiciones = new List<BusquedaPalabraTO>();
            listaDespliegue = new List<BusquedaListaTO>();
            OpcionesBusca.ItemsSource = condiciones;
            Expresion.Text = "Tesis Electorales";// CalculosGlobales.Expresion(parametrosIniciales).Substring(19);
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
                MessageBoxResult respuesta = MessageBox.Show(Mensajes.MENSAJE_INCORPORAR_PALABRA, Mensajes.TITULO_ADVERTENCIA,
                    MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (respuesta == MessageBoxResult.Yes)
                {
                    Incorpora_Click(sender, e);
                }
                else if(listaDespliegue.Count==0)
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
                TablaResultados ventanaNueva = new TablaResultados(parametros);
                ventanaNueva.Back = this;
                if ((ventanaNueva.tablaResultados.Items == null)
                    || (ventanaNueva.tablaResultados.Items.Count == 0))
                {
                    MessageBox.Show(Mensajes.MENSAJE_BUSQUEDA_VACIA,
                        Mensajes.TITULO_BUSQUEDA_VACIA, MessageBoxButton.OK,
                        MessageBoxImage.Exclamation);
                    //GridPrincipal.Background = Brushes.Maroon;
                    Cursor = Cursors.Arrow;
                    return;
                }
                Cursor = Cursors.Arrow;
                NavigationService.Navigate(ventanaNueva);
            }
            else
            {
                MessageBox.Show(Mensajes.MENSAJE_CAMPO_REQUERIDO, Mensajes.TITULO_CAMPO_REQUERIDO,
                    MessageBoxButton.OK, MessageBoxImage.Exclamation);
                NavigationService.Navigate(this);
                //Application.Current.MainWindow.BringIntoView(Application.Current.MainWindow.Content);
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
            if (Validadores.BusquedaPalabraTexto(this.Palabra).Equals(""))
            {
                BusquedaPalabraTO nuevaCondicion = new BusquedaPalabraTO();
                BusquedaListaTO condicionEnEspanol = new BusquedaListaTO();
                List<int> campos = new List<int>();
                bool seleccionado = false;
                while (Palabra.Text.Contains("  "))
                {
                    Palabra.Text = Palabra.Text.Replace("  ", " ");
                }
                condicionEnEspanol.Seccion = "";
                if ((bool)Localizacion.IsChecked)
                {
                    campos.Add(Constants.BUSQUEDA_PALABRA_CAMPO_LOC);
                    seleccionado = true;
                    condicionEnEspanol.Seccion += " Loc. ";
                }
                if ((bool)Rubro.IsChecked)
                {
                    campos.Add(Constants.BUSQUEDA_PALABRA_CAMPO_RUBRO);
                    seleccionado = true;
                    condicionEnEspanol.Seccion += " Rubro ";
                }
                if ((bool)Texto.IsChecked)
                {
                    campos.Add(Constants.BUSQUEDA_PALABRA_CAMPO_TEXTO);
                    seleccionado = true;
                    condicionEnEspanol.Seccion += " Texto ";
                }
                if ((bool)Precedentes.IsChecked)
                {
                    campos.Add(Constants.BUSQUEDA_PALABRA_CAMPO_PRECE);
                    seleccionado = true;
                    condicionEnEspanol.Seccion += " Preced. ";
                }
                if (!seleccionado)
                {
                    MessageBox.Show(Mensajes.MENSAJE_CAMPO_NO_SELECCIONADO,
                        Mensajes.TITULO_CAMPO_NO_SELECCIONADO, MessageBoxButton.OK,
                        MessageBoxImage.Exclamation);
                    return;
                }
                if ((Palabra.Text == null) || (Palabra.Text.Trim().Equals("")))
                {
                    MessageBox.Show(Mensajes.MENSAJE_CAMPO_TEXTO_VACIO,
                        Mensajes.TITULO_CAMPO_TEXTO_VACIO, MessageBoxButton.OK,
                        MessageBoxImage.Exclamation);
                    Palabra.Focus();
                    return;
                }
#if STAND_ALONE
                nuevaCondicion.Campos = campos;
#else
                nuevaCondicion.Campos = campos.ToArray();
#endif
                nuevaCondicion.Expresion = CalculosGlobales.SeparaExpresiones( Palabra.Text);
                condicionEnEspanol.Expresion = Palabra.Text;
                condicionEnEspanol.Epocas = Expresion.Text;
                int juris = Constants.BUSQUEDA_PALABRA_AMBAS;
                if ((bool)Jurisprudencia.IsChecked)
                {
                    juris = Constants.BUSQUEDA_PALABRA_JURIS;
                    condicionEnEspanol.Seccion += " [J] ";
                }
                else if ((bool)TesisAisladas.IsChecked)
                {
                    juris = Constants.BUSQUEDA_PALABRA_TESIS;
                    condicionEnEspanol.Seccion += " [T.A.] ";
                }
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
                condiciones.Add(nuevaCondicion);
                condicionEnEspanol.Epocas = " Tesis Electorales ";//CalculosGlobales.EtiquetaEpocas(parametros);
                listaDespliegue.Add(condicionEnEspanol);
                OpcionesBusca.ItemsSource = null;
                listaDespliegue.ElementAt(0).Operador = "";
                OpcionesBusca.ItemsSource = listaDespliegue;
                OpcionesBusca.SelectedIndex = -1;
                OpcionesBusca.SelectedIndex = listaDespliegue.Count-1;
                lblOperador.Visibility = Visibility.Visible;
                txtOperador.Visibility = Visibility.Visible;
                Palabra.Text = "";
                Palabra.Focus();
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
                if (listaDes.Count > 0)
                {
                    listaDes.ElementAt(0).Operador = "";
                    OpcionesBusca.SelectedIndex = 0;
                }
                listaDespliegue = listaDes;
               
                if (condiciones.Count == 0)
                {
                    this.lblOperador.Visibility = Visibility.Hidden;
                    this.txtOperador.SelectedIndex = 0;
                    this.txtOperador.Visibility = Visibility.Hidden;
                }
                Palabra.Focus();
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
                this.Realizar_Click(sender,e);
            }
        }

    }
}
