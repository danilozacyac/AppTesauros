using System;
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
using mx.gob.scjn.ius_common.gui.Guardar;
using System.Windows.Interop;

namespace mx.gob.scjn.ius_common.gui.apoyos
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
            this.ListaBusquedas.Padre = this;
        }

        public BuscaPalabras(BusquedaTO parametrosIniciales)
        {
            InitializeComponent();
            parametros = parametrosIniciales;
            condiciones = new List<BusquedaPalabraTO>();
            listaDespliegue = new List<BusquedaListaTO>();
            OpcionesBusca.ItemsSource = condiciones;
            Expresion.Text = CalculosGlobales.Expresion(parametrosIniciales).Substring(19);
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
                tablaResultado ventanaNueva = new tablaResultado(parametros);
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
                NavigationService.Navigate(new Window1());
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
                condicionEnEspanol.Epocas = CalculosGlobales.EtiquetaEpocas(parametros);
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

        private void Ver_MouseLeftButtonDown(object sender, RoutedEventArgs e)
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
                SeguridadUsuariosTO.UsuarioActual=usuarioOmision;
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

        /// <summary>
        /// Recupera una búsqueda Almacenada de Tesis y la muestra
        /// </summary>
        /// <param name="busquedaAlmacenadaTO">La búsqueda Almacenada</param>
        public void BuscaAlmacenada(BusquedaAlmacenadaTO busquedaAlmacenadaTO)
        {
            // Verificar que la búsqueda sea realmente de Tesis
            if ((busquedaAlmacenadaTO.TipoBusqueda == Constants.BUSQUEDA_TESIS_SIMPLE)
                || (busquedaAlmacenadaTO.TipoBusqueda == Constants.BUSQUEDA_TESIS_TEMATICA)
                || (busquedaAlmacenadaTO.TipoBusqueda == Constants.BUSQUEDA_ESPECIALES))
            {
                tablaResultado ventana = new tablaResultado(busquedaAlmacenadaTO);
                if (ventana.tablaResultados.Items.Count == 0)
                {
                    MessageBox.Show(Mensajes.MENSAJE_BUSQUEDA_VACIA, Mensajes.TITULO_BUSQUEDA_VACIA,
                        MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }
                ventana.Back = this;
                this.NavigationService.Navigate(ventana);
            }
        }
        /// <summary>
        /// Incorpora la búsquedaAlmacenada a la lista para su ejecución.
        /// </summary>
        /// <param name="busqueda">La búsqueda a incorporar.</param>
        public void IncorporaBusqueda(BusquedaAlmacenadaTO busqueda)
        {
            BusquedaListaTO item = new BusquedaListaTO();
            item.Expresion = busqueda.Nombre;
            item.Epocas= CalculosGlobales.EtiquetaEpocas(busqueda);
            this.listaDespliegue.Add(item);
            BusquedaPalabraTO itemBusqueda = new BusquedaPalabraTO();
            itemBusqueda.Jurisprudencia = Constants.BUSQUEDA_PALABRA_ALMACENADA;
#if STAND_ALONE
            itemBusqueda.Campos = new List<int>();
            itemBusqueda.Campos.Add(busqueda.id);
#else
            itemBusqueda.Campos = new int[1];
            itemBusqueda.Campos[0] = busqueda.id;
#endif
            itemBusqueda.Expresion = busqueda.Nombre; //Solo como referencia
            this.condiciones.Add(itemBusqueda);
            item.Seccion=Constants.BUSQUEDA_ALMACENADA;
            if (condiciones.Count > 1)
            {
                switch (this.txtOperador.SelectedIndex)
                {
                    case 0:
                        itemBusqueda.ValorLogico = Constants.BUSQUEDA_PALABRA_OP_Y;
                        item.Operador = "Y";
                        break;
                    case 1:
                        itemBusqueda.ValorLogico = Constants.BUSQUEDA_PALABRA_OP_O;
                        item.Operador = "O";
                        break;
                    case 2:
                        itemBusqueda.ValorLogico = Constants.BUSQUEDA_PALABRA_OP_NO;
                        item.Operador = "N";
                        break;
                }
            }
            this.OpcionesBusca.ItemsSource = null;
            this.OpcionesBusca.ItemsSource = listaDespliegue;
            this.OpcionesBusca.SelectedIndex = 0;
            this.txtOperador.Visibility = Visibility.Visible;
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
            this.Palabra.Text = busqueda.Expresiones[0].Expresion.Replace(Constants.SEPARADOR_FRASES.Trim(), " ");
            this.Texto.IsChecked = busqueda.Expresiones[0].Campos.Contains(Constants.BUSQUEDA_PALABRA_CAMPO_TEXTO);
            this.Rubro.IsChecked = busqueda.Expresiones[0].Campos.Contains(Constants.BUSQUEDA_PALABRA_CAMPO_RUBRO);
            this.Precedentes.IsChecked = busqueda.Expresiones[0].Campos.Contains(Constants.BUSQUEDA_PALABRA_CAMPO_PRECE);
            this.Localizacion.IsChecked = busqueda.Expresiones[0].Campos.Contains(Constants.BUSQUEDA_PALABRA_CAMPO_LOC);
            switch (busqueda.Expresiones[0].IsJuris)
            {
                case Constants.BUSQUEDA_PALABRA_AMBAS:
                    this.Ambas.IsChecked = true;
                    break;
                case Constants.BUSQUEDA_PALABRA_JURIS:
                    this.Jurisprudencia.IsChecked = true;
                    break;
                default:
                    this.TesisAisladas.IsChecked = true;
                    break;
            }
        }
        /// <summary>
        /// Almacena la expresión actual.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Almacena_MouseLeftButtonDown(object sender, RoutedEventArgs e)
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
                if ((bool)Localizacion.IsChecked || (bool)Rubro.IsChecked
                    || (bool)Texto.IsChecked || (bool)Precedentes.IsChecked)
                {
                    int juris = Constants.BUSQUEDA_PALABRA_AMBAS;
                    List<int> campos = new List<int>();
                    //BuscaPalabras padreActual = (BuscaPalabras)Padre;
                    if ((bool)Jurisprudencia.IsChecked)
                    {
                        juris = Constants.BUSQUEDA_PALABRA_JURIS;
                    }
                    else if ((bool)TesisAisladas.IsChecked)
                    {
                        juris = Constants.BUSQUEDA_PALABRA_TESIS;
                    }
                    if ((bool)Localizacion.IsChecked)
                    {
                        campos.Add(Constants.BUSQUEDA_PALABRA_CAMPO_LOC);
                    }
                    if ((bool)Rubro.IsChecked)
                    {
                        campos.Add(Constants.BUSQUEDA_PALABRA_CAMPO_RUBRO);
                    }
                    if ((bool)Texto.IsChecked)
                    {
                        campos.Add(Constants.BUSQUEDA_PALABRA_CAMPO_TEXTO);
                    }
                    if ((bool)Precedentes.IsChecked)
                    {
                        campos.Add(Constants.BUSQUEDA_PALABRA_CAMPO_PRECE);
                    }

                    BusquedaTO parametrosExpresion = new BusquedaTO();
                    parametrosExpresion.Acuerdos = parametros.Acuerdos;
                    parametrosExpresion.Apendices = parametros.Apendices;
#if STAND_ALONE
                    parametrosExpresion.Clasificacion = parametros.Clasificacion;
                    parametrosExpresion.Palabra = new List<BusquedaPalabraTO>();
                    parametrosExpresion.Palabra.Add(new BusquedaPalabraTO());
                    parametrosExpresion.Palabra[0].Campos = campos;
#else
                    parametrosExpresion.clasificacion = parametros.clasificacion;
                    parametrosExpresion.Palabra = new BusquedaPalabraTO[1];
                    parametrosExpresion.Palabra[0] = new BusquedaPalabraTO();
                    parametrosExpresion.Palabra[0].Campos = campos.ToArray();
#endif
                    parametrosExpresion.Epocas = parametros.Epocas;
                    parametrosExpresion.OrdenarPor = parametros.OrdenarPor;
                    parametrosExpresion.Palabra[0].Expresion = Palabra.Text;
                    parametrosExpresion.TipoBusqueda = parametros.TipoBusqueda;
                    parametrosExpresion.Tribunales = parametros.Tribunales;
                    parametrosExpresion.Palabra[0].Jurisprudencia = juris;
                    this.AlmacenaExpresion.Padre = this;
                    this.AlmacenaExpresion.ActualizaVentana(parametrosExpresion, null);
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

        private void Recuperar_Click(object sender, RoutedEventArgs e)
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
            this.ListaBusquedas.TipoBotones = ListaBusquedasAlmacenadas.BOTONES_RECUPERA;
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

        private void Palabra_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                this.Realizar_Click(sender,e);
            }
            //System.Windows.Forms.Application.DoEvents();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Mensajes.TOPICO_AYUDA = "busqueda_por_palabra.htm";
        }

    }
}
