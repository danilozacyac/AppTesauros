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

namespace mx.gob.scjn.ius_common.gui.apoyos
{
    /// <summary>
    /// Interaction logic for ListadoEjecutorias.xaml
    /// </summary>
    public partial class ListadoEjecutorias : UserControl
    {
        public NavigationService NavigationService { get; set; }
        /// <summary>
        /// La lista de la relación que existe entre el voto, 
        /// acuerdo o lo que sea y las ejecutorias.
        /// </summary>
        public List<EjecutoriasTO> ListaRelacion { get { return this.getListaRelacion(); } set { this.setListaRelacion(value); } }
        private List<EjecutoriasTO> listaRelacion;
        public Historial Historia { get; set; }
        private Point inicioDrag;
        private Point ofsetDrag;
        /// <summary>
        /// Constructor por omisión.
        /// </summary>
        public ListadoEjecutorias()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Constructor con un listado especial
        /// </summary>
        /// <param name="lista"></param>
        public ListadoEjecutorias(List<EjecutoriasTO> lista)
        {
            InitializeComponent();
            listado.ItemsSource = lista;
            ListaRelacion = lista;
            Registros.Content = "Registros: " + ListaRelacion.Count;
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
        private List<EjecutoriasTO> getListaRelacion()
        {
            return this.listaRelacion;
        }
        private void setListaRelacion(List<EjecutoriasTO> value)
        {
            this.listaRelacion = value;
            listado.ItemsSource = this.listaRelacion;
            this.Registros.Content = "Registros: " + value.Count;
        }

        private void listado_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            EjecutoriasTO ejecutoriaSeleccionada = (EjecutoriasTO)listado.SelectedItem;
            EjecutoriaPagina ejecutoriaAsociada = null;
            foreach (IUSNavigationService item in Historia.Lista)
            {
                if ((item.Id == Int32.Parse(ejecutoriaSeleccionada.Id)) && (item.TipoVentana == IUSNavigationService.EJECUTORIA))
                {
                    MessageBox.Show(Mensajes.MENSAJE_VENTANA_ABIERTA, Mensajes.TITULO_VENTANA_ABIERTA,
                        MessageBoxButton.OK, MessageBoxImage.Information);
                    ejecutoriaAsociada = (EjecutoriaPagina)item.ParametroConstructor;
                    ejecutoriaAsociada.ventanaHistorial.Visibility = Visibility.Hidden;
                    ejecutoriaAsociada.ventanaListadoTesis.Visibility = Visibility.Hidden;
                    ejecutoriaAsociada.ventanaListadoVotos.Visibility = Visibility.Hidden;
                    NavigationService.Navigate(ejecutoriaAsociada);
                    return;
                }
            }

            ejecutoriaAsociada = new EjecutoriaPagina(Int32.Parse(ejecutoriaSeleccionada.Id));
            ejecutoriaAsociada.Historia = Historia;
            this.NavigationService.Navigate(ejecutoriaAsociada);
            //this.Visibility = Visibility.Hidden;
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
                //this.VisualOffset.Y = puntoActual.Y + ofsetDrag.Y;
                //this.VisualOffset.X = puntoActual.X + ofsetDrag.X;
                Canvas.SetTop(this, puntoActual.Y);
                Canvas.SetLeft(this, puntoActual.X);
            }
            else
            {
                inicioDrag.X = -1;
                inicioDrag.Y = -1;
            }
        }
    }
}
