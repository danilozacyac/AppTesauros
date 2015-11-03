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
using mx.gob.scjn.ius_common.gui.utils;
using mx.gob.scjn.ius_common.fachade;
using mx.gob.scjn.ius_common.utils;

namespace mx.gob.scjn.ius_common.gui.apoyos
{
    /// <summary>
    /// Interaction logic for ListadoEjecutorias.xaml
    /// </summary>
    public partial class ListadoPonente: UserControl
    {
        public tablaResultado NavigationService { get; set; }
        /// <summary>
        /// La lista de la relación que existe entre el voto, 
        /// acuerdo o lo que sea y las ejecutorias.
        /// </summary>
        public List<PonenteTO> ListaRelacion { get { return this.getListaRelacion(); } set { this.setListaRelacion(value); } }
        private List<PonenteTO> listaRelacion;
        private static List<PonenteTO> ListadoCompleto { get; set; }
        private static List<TipoPonenteTO> ListadoTipoOriginal { get; set; }
        private List<PonenteTO> listadoOriginal { set; get; }
        public Historial Historia { get; set; }
        private Point inicioDrag;
        private Point ofsetDrag;
        protected static int[] TiposActuales { get; set; }
        /// <summary>
        /// Constructor por omisión.
        /// </summary>
        public ListadoPonente()
        {
            InitializeComponent();
            try
            {
                if (ListadoCompleto == null)
                {
#if STAND_ALONE
                    FachadaBusquedaTradicional fachada = new FachadaBusquedaTradicional();
                    List<PonenteTO> listaInicial = fachada.getCatalogoPonente();
                    List<TipoPonenteTO> listaTipoInicial = fachada.getCatalogoTipoPonente();
#else
                FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
                PonenteTO[] listaInicial = fachada.getCatalogoPonente();
                    TipoPonenteTO[] listaTipoInicial = fachada.getCatalogoTipoPonente();
#endif
                    ListaRelacion = new List<PonenteTO>();
                    ListadoTipoOriginal = new List<TipoPonenteTO>();
                    foreach (PonenteTO item in listaInicial)
                    {
                        ListaRelacion.Add(item);
                    }
                    foreach (TipoPonenteTO item in listaTipoInicial)
                    {
                        ListadoTipoOriginal.Add(item);
                    }
                    fachada.Close();
                    listadoOriginal = ListaRelacion;
                    ListadoCompleto = ListaRelacion;
                }
                else
                {
                    listadoOriginal = ListadoCompleto;
                    ListaRelacion = ListadoCompleto;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Problemas en la iniciación del objeto de clasificación: " + e.Message, "Error al iniciar", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        /// <summary>
        /// Esconde el control si es que se oprime el mouse
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Salir_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Hidden;
            this.Busqueda.Text = "";
        }
        private List<PonenteTO> getListaRelacion()
        {
            return this.listaRelacion;
        }
        private void setListaRelacion(List<PonenteTO> value)
        {
            this.listaRelacion = value;
            listado.ItemsSource = this.listaRelacion;
        }

        private void listado_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            List<PonenteTO> votoSeleccionado = (List<PonenteTO>)listado.SelectedItems;
            if (votoSeleccionado != null)
            {
                //VotosPagina votoAsociado = new VotosPagina(Int32.Parse(votoSeleccionado.Id));
                //votoAsociado.Historia = Historia;
                //this.NavigationService.NavigationService.Navigate(votoAsociado);
                //this.Visibility = Visibility.Hidden;
            }
        }

        private void BarraMovimiento_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ofsetDrag = e.GetPosition(this);
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

        private void btnSeleccionar_Click(object sender, RoutedEventArgs e)
        {
            List<PonenteTO> resultado = new List<PonenteTO>();
            foreach (PonenteTO item in listado.SelectedItems)
            {
                resultado.Add(item);
            }
            if (resultado.Count == 0)
            {
                MessageBox.Show(Mensajes.MENSAJE_SELECCIONA_PONENTE, Mensajes.TITULO_ADVERTENCIA,
                    MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            TipoPonenteTO TipoSeleccionado = (TipoPonenteTO)((ComboBoxItem)(CbxTipoPonente.SelectedItem)).Tag;
            NavigationService.ActualizaPonente(resultado, TipoSeleccionado.IdTipo);
            this.Visibility = Visibility.Hidden;
        }

        private void Limpiar_Click(object sender, RoutedEventArgs e)
        {
            listado.SelectedItems.Clear();
        }

        private void SeleccionarTodo_Click(object sender, RoutedEventArgs e)
        {
            foreach (object item in listado.ItemsSource)
            {
                listado.SelectedItems.Add(item);
            }
        }
        /// <summary>
        /// Filtra el resultado por lo escrito dentro del campo de texto.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Busqueda_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Busqueda.Text.Equals(Constants.CADENA_VACIA))
            {
                listado.ItemsSource = listadoOriginal;
            }
            else
            {
                ListaRelacion = new List<PonenteTO>();
                foreach (PonenteTO item in listadoOriginal)
                {
                    String solicitado = FlowDocumentHighlight.NormalizaSinAsterisco(item.DescTipo);
                    if (solicitado.Contains(FlowDocumentHighlight.NormalizaSinAsterisco(Busqueda.Text)))
                    {
                        ListaRelacion.Add(item);
                    }
                }
                if (ListaRelacion.Count == 0)
                {
                    MessageBox.Show(Mensajes.MENSAJE_SIN_REGISTROS, Mensajes.TITULO_SIN_REGISTROS,
                        MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    Busqueda.Text = Busqueda.Text.Substring(0, Busqueda.Text.Length - 1);
                    Busqueda.Focus();
                }
                else
                {
                    listado.ItemsSource = ListaRelacion;
                }
            }
        }
        public void ActualizaListado(int[] identificadores, int[] tipos)
        {
            Busqueda.Text = "";
            listadoOriginal = new List<PonenteTO>();
            foreach (PonenteTO item in ListadoCompleto)
            {
                if (identificadores.Contains(item.IdTipo))
                {
                    listadoOriginal.Add(item);
                }
            }
            listado.ItemsSource = listadoOriginal;
            List<TipoPonenteTO> listadoActual = new List<TipoPonenteTO>();
            TipoPonenteTO dummie = new TipoPonenteTO();
            dummie.DescTipo="Todos";
            dummie.IdTipo=-1;
            listadoActual.Add(dummie);
            CbxTipoPonente.Items.Clear();
            foreach (TipoPonenteTO item in ListadoTipoOriginal)
            {
                if (tipos.Contains(item.IdTipo))
                {
                    listadoActual.Add(item);
                }
            }
            foreach (TipoPonenteTO item in listadoActual)
            {
                ComboBoxItem nuevoItem = new ComboBoxItem();
                nuevoItem.Content = item.DescTipo;
                nuevoItem.Tag = item;
                CbxTipoPonente.Items.Add(nuevoItem);
            }
            CbxTipoPonente.SelectedIndex = 0;
            TiposActuales = tipos;
        }

        private void CbxTipoPonente_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CbxTipoPonente.SelectedItem != null)
            {
                if (NavigationService != null)
                {
                    TipoPonenteTO SelectedTipo = (TipoPonenteTO)((ComboBoxItem)CbxTipoPonente.SelectedItem).Tag;
                    this.NavigationService.ActualizaListaPonente(SelectedTipo.IdTipo);
                }
            }
        }

        internal void ActualizaListado(int[] identificadores)
        {
            listadoOriginal = new List<PonenteTO>();
            Busqueda.Text = "";
            foreach (PonenteTO item in ListadoCompleto)
            {
                if (identificadores.Contains(item.IdTipo))
                {
                    listadoOriginal.Add(item);
                }
            }
            listado.ItemsSource = listadoOriginal;
        }
    }
}
