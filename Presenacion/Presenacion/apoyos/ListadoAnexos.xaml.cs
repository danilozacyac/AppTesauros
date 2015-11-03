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
    public partial class ListadoAnexos: UserControl
    {
        public Page NavigationService { get; set; }
        /// <summary>
        /// La lista de la relación que existe entre el voto, 
        /// acuerdo o lo que sea y las ejecutorias.
        /// </summary>
        public List<TablaPartesTO> ListaRelacion { get { return this.getListaRelacion(); } set { this.setListaRelacion(value); } }
        private List<TablaPartesTO> listaRelacion;
        public Historial Historia { get; set; }
        private Point inicioDrag;
        private Point ofsetDrag;

        /// <summary>
        /// Constructor por omisión.
        /// </summary>
        public ListadoAnexos()
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
        }
        private List<TablaPartesTO> getListaRelacion()
        {
            return this.listaRelacion;
        }
        private void setListaRelacion(List<TablaPartesTO> value)
        {
            this.listaRelacion = value;
            listado.ItemsSource = this.listaRelacion;
        }

        private void listado_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TablaPartesTO anexoSeleccionado = (TablaPartesTO)listado.SelectedItem;
            if (anexoSeleccionado != null)
            {
                if (NavigationService.GetType() == typeof(EjecutoriaPagina))
                {
                    EjecutoriaPagina padre = (EjecutoriaPagina)NavigationService;
                    padre.MuestraAnexo(anexoSeleccionado);
                }
                else if (NavigationService.GetType() == typeof(VotosPagina))
                {
                    VotosPagina padre = (VotosPagina)NavigationService;
                    padre.MuestraAnexo(anexoSeleccionado);
                }
                else if (NavigationService.GetType() == typeof(AcuerdosPagina))
                {
                    AcuerdosPagina padre = (AcuerdosPagina)NavigationService;
                    padre.MuestraAnexo(anexoSeleccionado);
                }
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
