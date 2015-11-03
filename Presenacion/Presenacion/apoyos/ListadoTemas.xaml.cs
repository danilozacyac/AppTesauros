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
using IUS;
using mx.gob.scjn.ius_common.TO;
using mx.gob.scjn.ius_common.gui.utils;
using mx.gob.scjn.ius_common.utils;

namespace mx.gob.scjn.ius_common.gui.apoyos
{
    /// <summary>
    /// Interaction logic for ListadoTemas.xaml
    /// </summary>
    public partial class ListadoTemas : UserControl
    {
        public Tesis NavigationService { get; set; }
        /// <summary>
        /// La lista de la relación que existe entre el voto, 
        /// acuerdo o lo que sea y las ejecutorias.
        /// </summary>
        public List<TreeNodeDataTO> ListaRelacion { get { return this.getListaRelacion(); } set { this.setListaRelacion(value); } }
        private List<TreeNodeDataTO> listaRelacion;
        private static List<TreeNodeDataTO> ListadoCompleto { get; set; }
        private List<TreeNodeDataTO> listadoOriginal { set; get; }
 
        public Historial Historia { get; set; }
        private Point inicioDrag;
        private Point ofsetDrag;

        /// <summary>
        /// Constructor por omisión.
        /// </summary>
        public ListadoTemas()
        {
            InitializeComponent();
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
        private List<TreeNodeDataTO> getListaRelacion()
        {
            return this.listaRelacion;
        }
        private void setListaRelacion(List<TreeNodeDataTO> value)
        {
            this.listaRelacion = value;
            listado.ItemsSource = this.listaRelacion;
        }

        private void listado_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            List<AsuntoTO> votoSeleccionado = (List<AsuntoTO>)listado.SelectedItems;
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
            TreeNodeDataTO original = (TreeNodeDataTO)listado.SelectedItem;
            if (original == null)
            {
                MessageBox.Show("Debe seleccionar algún tema", "Tema no seleccionado", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            BusquedaTO busqueda = NavigationService.ObtenBusquedaTO(original);
            tablaResultado paginaResultados = new tablaResultado(busqueda);
            paginaResultados.Back = NavigationService;
            if (paginaResultados.tablaResultados.Items.Count == 0)
            {
                MessageBox.Show("No existen remas para esa consulta", Mensajes.TITULO_BUSQUEDA_VACIA,
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                NavigationService.NavigationService.Navigate(paginaResultados);
            }
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

        public void ActualizaListado(List<TreeNodeDataTO> identificadores)
        {
            Dictionary<string, TreeNodeDataTO> dic = new Dictionary<string, TreeNodeDataTO>();
            foreach (TreeNodeDataTO item in identificadores)
            {
                if(!dic.ContainsKey(item.Id))
                dic.Add(item.Id, item);
            }
            List<KeyValuePair<string, TreeNodeDataTO>> listaFinal = dic.ToList();
            listadoOriginal = new List<TreeNodeDataTO>();
            List<TreeNodeDataTO> datos = new List<TreeNodeDataTO>();
            foreach (KeyValuePair<string, TreeNodeDataTO> item in listaFinal)
            {
                listadoOriginal.Add(item.Value);
                datos.Add(item.Value);
            }
            listado.ItemsSource = datos;
        }

        private void Busqueda_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Busqueda.Text.Equals(Constants.CADENA_VACIA))
            {
                listado.ItemsSource = listadoOriginal;
            }
            else
            {
                ListaRelacion = new List<TreeNodeDataTO>();
                foreach (TreeNodeDataTO item in listadoOriginal)
                {
                    String solicitado = FlowDocumentHighlight.NormalizaSinAsterisco(item.Label);
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

    }
}
