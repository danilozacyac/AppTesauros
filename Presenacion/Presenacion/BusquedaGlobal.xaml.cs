using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using IUS;
using mx.gob.scjn.ius_common.TO;
using mx.gob.scjn.ius_common.gui.utils;
using mx.gob.scjn.ius_common.utils;

namespace mx.gob.scjn.ius_common.gui
{
    /// <summary>
    /// Interaction logic for BusquedaGlobal.xaml
    /// </summary>
    public partial class BusquedaGlobal : Page
    {
        public Page Back { get; set; }
        public BusquedaGlobal()
        {
            InitializeComponent();
            this.Busqueda.Focus();
        }

        private void Salir_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            if (Back == null)
            {
                this.NavigationService.GoBack();
            }
            else
            {
                this.NavigationService.Navigate(Back);
            }
        }

        private void BuscaBoton_Click(object sender, RoutedEventArgs e)
        {
            if (Validadores.BusquedaPalabraTexto(this.Busqueda).Equals(""))
            {
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

                        busqueda.Acuerdos[contador][llenador] = false;
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
                if ((bool)Localizacion.IsChecked)
                {
                    campos.Add(Constants.BUSQUEDA_PALABRA_CAMPO_LOC);
                    camposSeleccionads = true;
                }
                if ((bool)Texto.IsChecked)
                {
                    campos.Add(Constants.BUSQUEDA_PALABRA_CAMPO_TEXTO);
                    camposSeleccionads = true;
                }
                if ((bool)Precedentes.IsChecked)
                {
                    campos.Add(Constants.BUSQUEDA_PALABRA_CAMPO_PRECE);
                    camposSeleccionads = true;
                }
                if ((bool)Rubro.IsChecked)
                {
                    campos.Add(Constants.BUSQUEDA_PALABRA_CAMPO_RUBRO);
                    camposSeleccionads = true;
                }
                if (!camposSeleccionads)
                {
                    MessageBox.Show(Mensajes.MENSAJE_CAMPO_NO_SELECCIONADO,
                        Mensajes.TITULO_CAMPO_NO_SELECCIONADO,
                        MessageBoxButton.OK,MessageBoxImage.Error);
                    return;
                }
                int juris = Constants.BUSQUEDA_PALABRA_AMBAS;
                if ((bool)Jurisprudencia.IsChecked)
                {
                    juris = Constants.BUSQUEDA_PALABRA_JURIS;
                }
                else if ((bool)TesisAisladas.IsChecked)
                {
                    juris = Constants.BUSQUEDA_PALABRA_TESIS;
                }
                busqueda.Palabra[0].Jurisprudencia = juris;
#if STAND_ALONE
                busqueda.Palabra[0].Campos = campos;
#else
                busqueda.Palabra[0].Campos = campos.ToArray();
#endif
                busqueda.OrdenarPor = Constants.ORDER_DEFAULT;
                busqueda.Palabra[0].Expresion = CalculosGlobales.SeparaExpresiones(Busqueda.Text);
                //busqueda.Palabra[0].Expresion = this.Busqueda.Text;
                //busqueda.Palabra[0].Jurisprudencia = Constants.BUSQUEDA_PALABRA_AMBAS;
                tablaResultado tesis = new tablaResultado(busqueda);
                tesis.Back = this;
                if ((tesis.tablaResultados.ItemsSource == null) 
                    || (tesis.tablaResultados.Items.Count == 0))
                {
                    MessageBox.Show(Mensajes.MENSAJE_BUSQUEDA_VACIA,
                        Mensajes.TITULO_BUSQUEDA_VACIA, MessageBoxButton.OK,
                        MessageBoxImage.Exclamation);
                    return;
                }
                this.NavigationService.Navigate(tesis);
            }
        }

        private void Busqueda_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                BuscaBoton_Click(sender, e);
            }
        }

    }
}
