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

namespace mx.gob.scjn.ius_common.gui.apoyos
{
    /// <summary>
    /// Interaction logic for VentanaHistorial.xaml
    /// </summary>
    public partial class VentanaHistorial : UserControl
    {
        private Point inicioDrag;
        private Point ofsetDrag;

        /// <summary>
        /// Se requiere para poder generar la navegación, la referencia es hacia
        /// la página sobre la cual se hará la navegación.
        /// </summary>
        public Page NavigatorReference { set; get; }
        /// <summary>
        /// El constructor por omisión es usado por el XAML, ya que no se pueden usar
        /// parámetros de construcción cuando se utiliza y los controles
        /// de usuario no se pueden mostrar cuando son generados programaticamente.
        /// </summary>
        public VentanaHistorial()
        {
            InitializeComponent();
        }

        private void Salir_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Hidden;
        }

        private void BarraMovimiento_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ofsetDrag = e.GetPosition(BarraMovimiento);
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
