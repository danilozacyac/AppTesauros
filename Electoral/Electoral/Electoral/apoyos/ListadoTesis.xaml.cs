using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using mx.gob.scjn.electoral;
using mx.gob.scjn.electoral_common.TO;
using mx.gob.scjn.ius_common.TO;
using mx.gob.scjn.ius_common.fachade;

namespace mx.gob.scjn.electoral_common.gui.apoyos
{
    /// <summary>
    /// Interaction logic for ListadoTesis.xaml
    /// </summary>
    public partial class ListadoTesis : UserControl
    {
        public List<TesisSimplificadaElectoralTO> TesisMostrar { get { return this.getTesisMostrar(); } set { this.setTesisMostrar(value); } }
        private List<TesisSimplificadaElectoralTO> tesisMostrar;
        public Page NavigationService { get; set; }
        private Point inicioDrag;
        private Point ofsetDrag;

        private void setTesisMostrar(List<TesisSimplificadaElectoralTO> value)
        {
            listado.ItemsSource = value;
            this.tesisMostrar= value;
            Registros.Content = value.Count;
        }

        private List<TesisSimplificadaElectoralTO> getTesisMostrar()
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
            TesisSimplificadaElectoralTO tesisSeleccionada = (TesisSimplificadaElectoralTO)listado.SelectedItem;
            TesisElectoral tesisAsociada = null;
            if (tesisSeleccionada != null)
            {
#if STAND_ALONE
                FachadaBusquedaTradicional fachada = new FachadaBusquedaTradicional();
#else
                FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
#endif
                TesisTO nuevaTesis = fachada.getTesisElectoralPorRegistro(tesisSeleccionada.Ius);
                tesisAsociada = new TesisElectoral(nuevaTesis);
                this.NavigationService.NavigationService.Navigate(tesisAsociada);
                tesisAsociada.Back = this.NavigationService;
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
