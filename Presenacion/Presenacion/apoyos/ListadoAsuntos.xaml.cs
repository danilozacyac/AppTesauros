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
    public partial class ListadoAsuntos: UserControl
    {
        public tablaResultado NavigationService { get; set; }
        /// <summary>
        /// La lista de la relación que existe entre el voto, 
        /// acuerdo o lo que sea y las ejecutorias.
        /// </summary>
        public List<AsuntoTO> ListaRelacion { get { return this.getListaRelacion(); } set { this.setListaRelacion(value); } }
        private List<AsuntoTO> listaRelacion;
        private static List<AsuntoTO> ListadoCompleto { get; set; }
        private List<AsuntoTO> listadoOriginal { set; get; }
 
        public Historial Historia { get; set; }
        private Point inicioDrag;
        private Point ofsetDrag;

        /// <summary>
        /// Constructor por omisión.
        /// </summary>
        public ListadoAsuntos()
        {
            InitializeComponent();
            try
            {
                if (ListadoCompleto == null)
                {
#if STAND_ALONE
                    FachadaBusquedaTradicional fachada = new FachadaBusquedaTradicional();
                    List<AsuntoTO> listaInicial = fachada.getCatalogoAsunto();
#else
                FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
                AsuntoTO[] listaInicial = fachada.getCatalogoAsunto();
#endif
                    ListaRelacion = new List<AsuntoTO>();
                    foreach (AsuntoTO item in listaInicial)
                    {
                        ListaRelacion.Add(item);
                    }
                    fachada.Close();
                    listadoOriginal = ListaRelacion;
                    ListadoCompleto = ListaRelacion;
                }
                else
                {
                    ListaRelacion = ListadoCompleto;
                    listadoOriginal = ListadoCompleto;
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
        private List<AsuntoTO> getListaRelacion()
        {
            return this.listaRelacion;
        }
        private void setListaRelacion(List<AsuntoTO> value)
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
            List<AsuntoTO> resultado = new List<AsuntoTO>();
            foreach (AsuntoTO item in listado.SelectedItems)
            {
                resultado.Add(item);
            }
            if (resultado.Count == 0)
            {
                MessageBox.Show(Mensajes.MENSAJE_SELECCIONA_ASUNTO, Mensajes.TITULO_ADVERTENCIA,
                    MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            NavigationService.ActualizaAsunto(resultado);
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

        public void ActualizaListado(int[] identificadores)
        {
            listadoOriginal = new List<AsuntoTO>();
            foreach (AsuntoTO item in ListadoCompleto)
            {
                if (identificadores.Contains(item.IdTipo))
                {
                    listadoOriginal.Add(item);
                }
            }
            listado.ItemsSource = listadoOriginal;
        }

        private void Busqueda_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Busqueda.Text.Equals(Constants.CADENA_VACIA))
            {
                listado.ItemsSource = listadoOriginal;
            }
            else
            {
                ListaRelacion = new List<AsuntoTO>();
                foreach (AsuntoTO item in listadoOriginal)
                {
                    String solicitado = FlowDocumentHighlight.NormalizaSinAsterisco(item.DescTipo);
                    if (solicitado.StartsWith(FlowDocumentHighlight.NormalizaSinAsterisco(Busqueda.Text)))
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
