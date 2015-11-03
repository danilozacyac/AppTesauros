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
using mx.gob.scjn.ius_common.fachade;
using mx.gob.scjn.ius_common.TO;
using IUS;
using mx.gob.scjn.ius_common.gui.utils;
using mx.gob.scjn.ius_common.utils;

namespace mx.gob.scjn.ius_common.gui.apoyos
{
    /// <summary>
    /// Interaction logic for ListaBusquedasAlmacenadas.xaml
    /// </summary>
    public partial class ListaBusquedasAlmacenadas : UserControl
    {
        /// <summary>
        /// Punto de inicio del movimiento de la ventana auxiliar
        /// </summary>
        private Point inicioDrag;
        /// <summary>
        /// Punto donde se deposita la ventana
        /// </summary>
        private Point ofsetDrag;
        /// <summary>
        /// La página que está llamando a la ventana auxiliar
        /// </summary>
        public Page Padre { get; set; }
        /// <summary>
        /// Tipo de Botones a Mostrar.
        /// </summary>
        public int TipoBotones { get; set; }
        /// <summary>
        /// Constante de botones por omisión (Recuperar)
        /// </summary>
        public const int BOTONES_RECUPERA = 1;
        /// <summary>
        /// Constante de botones para la incorporación a busquedas por palabras.
        /// </summary>
        public const int BOTONES_VER = 2;
        /// <summary>
        /// Constructor por omisión.
        /// </summary>
        public ListaBusquedasAlmacenadas()
        {
            InitializeComponent();
            TipoBotones = BOTONES_RECUPERA;
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


        
        /// <summary>
        /// Actualiza el contenido del Grid con las búsquedas pertenecientes al usuario.
        /// </summary>
        /// <param name="usuario">El nombre del usuario.</param>
        public void Actualiza(String usuario)
        {
#if STAND_ALONE
            FachadaBusquedaTradicional fachada = new FachadaBusquedaTradicional();
            List<BusquedaAlmacenadaTO> resultadoFachada = fachada.getBusquedasAlmacenadas(usuario);
#else
            FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
            BusquedaAlmacenadaTO[] resultadoFachada = fachada.getBusquedasAlmacenadas(usuario);
#endif
            List<BusquedaAlmacenadaTO> listaParaGrid = new List<BusquedaAlmacenadaTO>();
            listado.ItemsSource = listaParaGrid;
            if (TipoBotones == BOTONES_RECUPERA)
            {
                foreach (BusquedaAlmacenadaTO item in resultadoFachada)
                {
                        listaParaGrid.Add(item);
                } 
                this.btnAceptar.Visibility = Visibility.Visible;
                this.btnEliminar.Visibility = Visibility.Hidden;
                this.btnIncorporar.Visibility = Visibility.Visible;
                this.btnSeleccionar.Visibility = Visibility.Hidden;
                this.label1.Content = "Consultas almacenadas";
            }
            else
            {
                foreach (BusquedaAlmacenadaTO item in resultadoFachada)
                {
                    if ((item.TipoBusqueda != Constants.BUSQUEDA_TESIS_TEMATICA))
                        listaParaGrid.Add(item);
                }
                this.btnAceptar.Visibility = Visibility.Hidden;
                this.btnEliminar.Visibility = Visibility.Visible;
                this.btnIncorporar.Visibility = Visibility.Hidden;
                this.btnSeleccionar.Visibility = Visibility.Visible;
                this.label1.Content = "Expresiones almacenadas";
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

        private void btnAceptar_Click(object sender, RoutedEventArgs e)
        {
            if (listado.SelectedItem == null)
            {
                MessageBox.Show(Mensajes.MENSAJE_SELECCIONAR_BUSQUEDA, Mensajes.TITULO_SELECCIONAR_BUSQUEDA,
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (typeof(Window1) == Padre.GetType())
            {
                Window1 instancia = (Window1)Padre;
                instancia.BuscaAlmacenada((BusquedaAlmacenadaTO)listado.SelectedItem);
            }
            if (typeof(BuscaPalabras) == Padre.GetType())
            {
                BuscaPalabras instancia = (BuscaPalabras)Padre;
                instancia.BuscaAlmacenada((BusquedaAlmacenadaTO)listado.SelectedItem);
            }
            this.Visibility = Visibility.Hidden;
        }

        private void btnIncorporar_Click(object sender, RoutedEventArgs e)
        {
            if (listado.SelectedItem == null)
            {
                MessageBox.Show(Mensajes.MENSAJE_SELECCIONAR_BUSQUEDA, Mensajes.TITULO_SELECCIONAR_BUSQUEDA,
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (typeof(BuscaPalabras) == Padre.GetType())
            {
                BuscaPalabras instancia = (BuscaPalabras)Padre;
                instancia.IncorporaBusqueda((BusquedaAlmacenadaTO)listado.SelectedItem);
            }
            this.Visibility = Visibility.Hidden;

        }

        private void btnSeleccionar_Click(object sender, RoutedEventArgs e)
        {
            if (listado.SelectedItem == null)
            {
                MessageBox.Show(Mensajes.MENSAJE_SELECCIONAR_BUSQUEDA, Mensajes.TITULO_SELECCIONAR_BUSQUEDA,
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (typeof(BuscaPalabras) == Padre.GetType())
            {
                BuscaPalabras instancia = (BuscaPalabras)Padre;
                instancia.SeleccionaBusquedaAlmacenada((BusquedaAlmacenadaTO)listado.SelectedItem);
                this.Visibility = Visibility.Hidden;
            }else if (typeof(BuscaPalabrasEjecutorias) == Padre.GetType())
            {
                BuscaPalabrasEjecutorias instancia = (BuscaPalabrasEjecutorias)Padre;
                instancia.SeleccionaBusquedaAlmacenada((BusquedaAlmacenadaTO)listado.SelectedItem);
                this.Visibility = Visibility.Hidden;
            }
            else if (typeof(BuscaPalabrasVotos) == Padre.GetType())
            {
                BuscaPalabrasVotos instancia = (BuscaPalabrasVotos)Padre;
                instancia.SeleccionaBusquedaAlmacenada((BusquedaAlmacenadaTO)listado.SelectedItem);
                this.Visibility = Visibility.Hidden;
            }
            else if (typeof(AcuerdoBuscaPalabras) == Padre.GetType())
            {
                AcuerdoBuscaPalabras instancia = (AcuerdoBuscaPalabras)Padre;
                instancia.SeleccionaBusquedaAlmacenada((BusquedaAlmacenadaTO)listado.SelectedItem);
                this.Visibility = Visibility.Hidden;
            }
        }

        private void btnEliminar_Click(object sender, RoutedEventArgs e)
        {
            if (listado.SelectedItem != null)
            {
                BusquedaAlmacenadaTO busquedaEliminar = (BusquedaAlmacenadaTO)listado.SelectedItem;
#if STAND_ALONE
                FachadaBusquedaTradicional fachada = new FachadaBusquedaTradicional();
#else
                FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
#endif
                fachada.EliminarBusqueda(busquedaEliminar);
                MessageBox.Show(Mensajes.MENSAJE_BUSQUEDA_ELIMINADA, Mensajes.TITULO_BUSQUEDA_ELIMINADA,
                    MessageBoxButton.OK, MessageBoxImage.Information);
                List<BusquedaAlmacenadaTO> listadoNuevo = new List<BusquedaAlmacenadaTO>();
                foreach (BusquedaAlmacenadaTO item in listado.Items)
                {
                    if (item != busquedaEliminar)
                    {
                        listadoNuevo.Add(item);
                    }
                }
                listado.ItemsSource = null;
                listado.ItemsSource = listadoNuevo;
            }
        }
    }
}
