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

namespace mx.gob.scjn.ius_common.gui.apoyos
{
    /// <summary>
    /// Interaction logic for ListadoEjecutorias.xaml
    /// </summary>
    public partial class ListadoClasificacion : UserControl
    {
        public tablaResultadoVotos NavigationService { get; set; }
        /// <summary>
        /// La lista de la relación que existe entre el voto, 
        /// acuerdo o lo que sea y las ejecutorias.
        /// </summary>
        public List<ClassificacionTO> ListaRelacion { get { return this.getListaRelacion(); } set { this.setListaRelacion(value); } }
        private List<ClassificacionTO> listaRelacion;
        public Historial Historia { get; set; }
        private Point inicioDrag;
        private Point ofsetDrag;
        public List<int> existen { get { return this.existen; } set { setExisten(value); } }
        private List<int> Existen;

        private void setExisten(List<int> value)
        {
            List<ClassificacionTO> ensenar = new List<ClassificacionTO>();
            foreach (ClassificacionTO item in ListaRelacion)
            {
                if(value.Contains(item.IdTipo))
                    ensenar.Add(item);
            }
            listado.ItemsSource = ensenar;
            this.Existen = value;
        }
        /// <summary>
        /// Constructor por omisión.
        /// </summary>
        public ListadoClasificacion()
        {
            InitializeComponent();
            try
            {
#if STAND_ALONE
                FachadaBusquedaTradicional fachada = new FachadaBusquedaTradicional();
                List<ClassificacionTO> listaInicial = fachada.getClasificacion();
#else
                FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
                ClassificacionTO[] listaInicial = fachada.getClasificacion();
#endif
                ListaRelacion = new List<ClassificacionTO>();
                foreach (ClassificacionTO item in listaInicial)
                {
                    ListaRelacion.Add(item);
                }
                if (fachada != null)
                {
                    fachada.Close();
                }
            }
            catch (Exception e)
            {
                ListaRelacion = new List<ClassificacionTO>();
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
        }
        private List<ClassificacionTO> getListaRelacion()
        {
            return this.listaRelacion;
        }
        private void setListaRelacion(List<ClassificacionTO> value)
        {
            this.listaRelacion = value;
            listado.ItemsSource = this.listaRelacion;
        }

        private void listado_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            List<ClassificacionTO> votoSeleccionado = (List<ClassificacionTO>)listado.SelectedItems;
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
            List<ClassificacionTO> resultado = new List<ClassificacionTO>();
            foreach (ClassificacionTO item in listado.SelectedItems)
            {
                resultado.Add(item);
            }
            if (resultado.Count == 0)
            {
                MessageBox.Show("Debe seleccionar por lo menos una clasificación", "No hay clasificaciones seleccionadas", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            NavigationService.ActualizaClasificacion(resultado);
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
    }
}
