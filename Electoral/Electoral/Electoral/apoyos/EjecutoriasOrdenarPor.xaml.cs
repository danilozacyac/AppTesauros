using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Navigation;
using mx.gob.scjn.electoral;

namespace mx.gob.scjn.electoral_common.gui.apoyos
{
    /// <summary>
    /// Interaction logic for ListadoEjecutorias.xaml
    /// </summary>
    public partial class EjecutoriasOrdenarPor : UserControl
    {
        public NavigationService NavigationService { get; set; }
        /// <summary>
        /// La lista de la relación que existe entre el voto, 
        /// acuerdo o lo que sea y las ejecutorias.
        /// </summary>
        public TablaResultadoEjecutoria Padre { get; set; }
        private Point inicioDrag;
        private Point ofsetDrag;
        /// <summary>
        /// Constructor por omisión.
        /// </summary>
        public EjecutoriasOrdenarPor()
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

        private void LblLoc_MouseEnter(object sender, MouseEventArgs e)
        {
            Label etiquetaActual = (Label)sender;
            etiquetaActual.Foreground = Brushes.Red;
            etiquetaActual.Background = Brushes.Coral;
        }

        private void LblLoc_MouseLeave(object sender, MouseEventArgs e)
        {
            Label etiquetaActual = (Label)sender;
            etiquetaActual.Foreground = Brushes.Black;
            etiquetaActual.Background = Brushes.Bisque;
        }

        private void LblLoc_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Padre.Controlador.OrdenarPor = "ConsecIndx";
            this.Visibility = Visibility.Hidden;
        }

        private void LblRubro_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Padre.Controlador.OrdenarPor = "Rubro";
            this.Visibility = Visibility.Hidden;
        }

        private void LblTesis_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Padre.Controlador.OrdenarPor = "Promovente";
            this.Visibility = Visibility.Hidden;
        }

        private void LblIus_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Padre.Controlador.OrdenarPor = "Id";
            this.Visibility = Visibility.Hidden;
        }

    }
}
