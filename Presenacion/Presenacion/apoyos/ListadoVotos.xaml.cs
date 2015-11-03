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
    public partial class ListadoVotos : UserControl
    {
        public Page NavigationService { get; set; }
        /// <summary>
        /// La lista de la relación que existe entre el voto, 
        /// acuerdo o lo que sea y las ejecutorias.
        /// </summary>
        public List<VotoSimplificadoTO> ListaRelacion { get { return this.getListaRelacion(); } set { this.setListaRelacion(value); } }
        private List<VotoSimplificadoTO> listaRelacion;
        public Historial Historia { get; set; }
        private Point inicioDrag;
        private Point ofsetDrag;

        /// <summary>
        /// Constructor por omisión.
        /// </summary>
        public ListadoVotos()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Constructor con un listado especial
        /// </summary>
        /// <param name="lista"></param>
        public ListadoVotos(List<VotosTO> lista)
        {
            InitializeComponent();
            listaRelacion = new List<VotoSimplificadoTO>();
            foreach (VotosTO item in lista)
            {
                VotoSimplificadoTO itemVerdadero = new VotoSimplificadoTO(item);
                listaRelacion.Add(itemVerdadero);
            }
            listado.ItemsSource = listaRelacion;
            //ListaRelacion = lista;
            Registros.Content = lista.Count;
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
        private List<VotoSimplificadoTO> getListaRelacion()
        {
            return this.listaRelacion;
        }
        private void setListaRelacion(List<VotoSimplificadoTO> value)
        {
            this.listaRelacion = value;
            listado.ItemsSource = this.listaRelacion;
            this.Registros.Content = "Registros: " + value.Count;
        }

        private void listado_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            VotoSimplificadoTO votoSeleccionado = (VotoSimplificadoTO)listado.SelectedItem;
            VotosPagina votoAsociado = null;
            if (votoSeleccionado != null)
            {
                foreach (IUSNavigationService item in Historia.Lista)
                {
                    if ((item.Id == Int32.Parse(votoSeleccionado.Id)) && (item.TipoVentana == IUSNavigationService.VOTO))
                    {
                        MessageBox.Show(Mensajes.MENSAJE_VENTANA_ABIERTA, Mensajes.TITULO_VENTANA_ABIERTA,
                            MessageBoxButton.OK, MessageBoxImage.Information);
                        votoAsociado = (VotosPagina)item.ParametroConstructor;
                        NavigationService.NavigationService.Navigate(votoAsociado);
                        return;
                    }
                }

                votoAsociado = new VotosPagina(Int32.Parse(votoSeleccionado.Id));
                votoAsociado.Historia = Historia;
                this.NavigationService.NavigationService.Navigate(votoAsociado);
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
    }
}
