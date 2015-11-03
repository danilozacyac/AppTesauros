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
    public partial class ListadoLeyes : UserControl
    {
        public Tesis NavigationService { get; set; }
        /// <summary>
        /// La lista de la relación que existe entre el voto, 
        /// acuerdo o lo que sea y las ejecutorias.
        /// </summary>
        public List<RelacionFraseArticulosTO> ListaRelacion { get { return this.getListaRelacion(); } set { this.setListaRelacion(value); } }
        private List<RelacionFraseArticulosTO> listaRelacion;
        private List<DocumentoADesplegarTO> documentos;
        public Historial Historia { get; set; }
        public int TipoLey { get; set; }
        private Point inicioDrag;
        private Point ofsetDrag;
        public VentanaLeyes VentanaLeyes { get; set; }
        /// <summary>
        /// Constructor por omisión.
        /// </summary>
        public ListadoLeyes()
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
        private List<RelacionFraseArticulosTO> getListaRelacion()
        {
            return this.listaRelacion;
        }
        private void setListaRelacion(List<RelacionFraseArticulosTO> value)
        {
            this.listaRelacion = value;
            this.documentos = new List<DocumentoADesplegarTO>();
#if STAND_ALONE
            FachadaBusquedaTradicional fachada = new FachadaBusquedaTradicional();
#else
            FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
#endif
            foreach (RelacionFraseArticulosTO item in value)
            {
#if STAND_ALONE
                DocumentoLeyTO leyActual = fachada.getDocumentoLey(Int32.Parse(item.IdLey),
                    Int32.Parse(item.IdArt), Int32.Parse(item.IdRef),TipoLey);
#else
                DocumentoLeyTO leyActual = fachada.getLeyes(Int32.Parse(item.IdLey),
                    Int32.Parse(item.IdArt), Int32.Parse(item.IdRef));
#endif
                DocumentoADesplegarTO docDesplegar = new DocumentoADesplegarTO(leyActual);
                this.documentos.Add(docDesplegar);
                
            }
            listado.ItemsSource = this.documentos;
            this.Registros.Content = "Registros: " + value.Count;
        }

        private void listado_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (listado.SelectedIndex != -1)
            {
                int item = listado.SelectedIndex;
                RelacionFraseArticulosTO leySeleccionado = listaRelacion.ElementAt(item);
                this.VentanaLeyes.IdArticulo = Int32.Parse(leySeleccionado.IdArt);
                this.VentanaLeyes.IdLey = Int32.Parse(leySeleccionado.IdLey);
                this.VentanaLeyes.IdRef = Int32.Parse(leySeleccionado.IdRef);
                this.VentanaLeyes.ActualizaVentana();
                this.VentanaLeyes.Visibility = Visibility.Visible;
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

        internal void ActualizaVentana()
        {
            this.Visibility=Visibility.Visible;
        }
    }
}
