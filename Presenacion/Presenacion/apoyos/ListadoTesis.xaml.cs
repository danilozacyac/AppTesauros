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
    /// Interaction logic for ListadoTesis.xaml
    /// </summary>
    public partial class ListadoTesis : UserControl
    {
        public List<TesisSimplificadaTO> TesisMostrar { get { return this.getTesisMostrar(); } set { this.setTesisMostrar(value); } }
        private List<TesisSimplificadaTO> tesisMostrar;
        public NavigationService NavigationService { get; set; }
        public Historial Historia { get; set; }
        private Point inicioDrag;
        private Point ofsetDrag;

        private void setTesisMostrar(List<TesisSimplificadaTO> value)
        {
            listado.ItemsSource = null;
            listado.ItemsSource = value;
            listado.Items.Refresh();
            this.tesisMostrar= value;
            Registros.Content = value.Count;
        }

        private List<TesisSimplificadaTO> getTesisMostrar()
        {
            return this.tesisMostrar;
        }

        public ListadoTesis()
        {
            InitializeComponent();
        }

        private void Salir_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Hidden;
        }

        private void listado_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TesisSimplificadaTO tesisSeleccionada = (TesisSimplificadaTO)listado.SelectedItem;
            Tesis tesisAsociada = null;
            if (tesisSeleccionada != null)
            {
                foreach (IUSNavigationService item in Historia.Lista)
                {
                    if ((item.Id == Int32.Parse(tesisSeleccionada.Ius)) && (item.TipoVentana == IUSNavigationService.TESIS))
                    {
                        MessageBox.Show(Mensajes.MENSAJE_VENTANA_ABIERTA, Mensajes.TITULO_VENTANA_ABIERTA,
                            MessageBoxButton.OK, MessageBoxImage.Information);
                        tesisAsociada = (Tesis)item.ParametroConstructor;
                        tesisAsociada.ventanaHistorial.Visibility = Visibility.Hidden;
                        tesisAsociada.ventanaListaEjecutorias.Visibility = Visibility.Hidden;
                        tesisAsociada.ventanaListaVotos.Visibility = Visibility.Hidden; ;
                        NavigationService.Navigate(tesisAsociada);
                        return;
                    }
                }
#if STAND_ALONE
                FachadaBusquedaTradicional fachada = new FachadaBusquedaTradicional();
#else
                FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
#endif
                TesisTO nuevaTesis = fachada.getTesisPorRegistro(tesisSeleccionada.Ius);
                tesisAsociada = new Tesis(nuevaTesis);
                tesisAsociada.Historia = Historia;
                this.NavigationService.Navigate(tesisAsociada);
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

        private void BarraMovimiento_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
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
