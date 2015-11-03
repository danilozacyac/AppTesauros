using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using mx.gob.scjn.electoral;
using mx.gob.scjn.ius_common.gui.utils;

namespace mx.gob.scjn.electoral_common.gui.apoyos
{
    /// <summary>
    /// Interaction logic for ObtenerRango.xaml
    /// </summary>
    public partial class ObtenerRango : UserControl
    {
        /// <summary>
        /// El mensaje que mostrará el Control.
        /// </summary>
        public String StrMensaje { get {return this.getStrMensaje(); } set { this.setStrMensaje(value); } }
        private String strMensaje;
        public Int32 diferenciaRangos { get; set; }
        public Int32 registroFinal { get; set; }
        public Int32 InicioRango { get; set; }
        public Int32 FinRango { get; set; }
        public Page contenedor { get; set; }
        private Point inicioDrag;
        private Point ofsetDrag;

        public ObtenerRango()
        {
            InitializeComponent();
        }

        private void setStrMensaje(String value)
        {
            this.strMensaje = value;
            this.Mensaje.Text = value;
        }

        private String getStrMensaje()
        {
            return strMensaje;
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            if ((this.Inicio.Text == null) || (this.Inicio.Text.Equals("")))
            {
                MessageBox.Show(Mensajes.MENSAJE_RANGO_INICIAL_VACIO, Mensajes.TITULO_RANGO,
                    MessageBoxButton.OK, MessageBoxImage.Error);
                this.Inicio.Focus();
                return;
            }
            if ((this.Final.Text == null) || (this.Final.Text.Equals("")))
            {
                MessageBox.Show(Mensajes.MENSAJE_RANGO_FINAL_VACIO, Mensajes.TITULO_RANGO,
                    MessageBoxButton.OK, MessageBoxImage.Error);
                this.Final.Focus();
                return;
            }
            if ((Int32.Parse(this.Final.Text) == 0) || (Int32.Parse(this.Inicio.Text)==0))
            {
                MessageBox.Show(Mensajes.MENSAJE_RANGO_INVALIDO, Mensajes.TITULO_RANGO,
    MessageBoxButton.OK, MessageBoxImage.Error);
                return;

            }
            if ((this.Final.Text == null) || (this.Final.Text.Equals("")))
            {
                MessageBox.Show(Mensajes.MENSAJE_RANGO_INICIAL_VACIO, Mensajes.TITULO_RANGO,
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            int inicial = Int32.Parse(this.Inicio.Text);
            int registroUltimo = Int32.Parse(this.Final.Text);
            if (inicial > registroUltimo)
            {
                MessageBox.Show(Mensajes.MENSAJE_RANGO_FINAL_MENOR, 
                    Mensajes.TITULO_RANGO, MessageBoxButton.OK, 
                    MessageBoxImage.Error);
                return;
            }
            if (registroUltimo > registroFinal)
            {
                MessageBox.Show(Mensajes.MENSAJE_RANGO_MUY_ALTO+": "+registroFinal,
                    Mensajes.TITULO_RANGO, MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }
            if ((registroUltimo - inicial) > diferenciaRangos)
            {
                MessageBox.Show(Mensajes.MENSAJE_RANGO_MUCHOS_SELECCIONADOS + diferenciaRangos + " adicionales",
                    Mensajes.TITULO_RANGO, MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }
            InicioRango = inicial;
            FinRango = registroUltimo;
            if (typeof(TesisElectoral) == contenedor.GetType())
            {
                TesisElectoral ventanaTesis = (TesisElectoral)contenedor;
                ventanaTesis.ActualizaRangoMarcado(inicial, FinRango);
            }
            else if (typeof(Ejecutoria) == contenedor.GetType())
            {
                Ejecutoria ventanaEjecutoria = (Ejecutoria)contenedor;
                ventanaEjecutoria.ActualizaRangoMarcado(inicial, FinRango);
            }
            else if (typeof(Acuerdos) == contenedor.GetType())
            {
                Acuerdos ventanaAcuerdo = (Acuerdos)contenedor;
                ventanaAcuerdo.ActualizaRangoMarcado(inicial, FinRango);
            }
            else if (typeof(Votos) == contenedor.GetType())
            {
                Votos ventanaVotos = (Votos)contenedor;
                //ventanaVotos.ActualizaRangoMarcado(inicial, FinRango);
            }
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

        /// <summary>
        /// Esconde el control si es que se oprime el mouse
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Salir_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Hidden;
            Inicio.Text = "";
            Final.Text = "";
        }

        private void Inicio_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Key == Key.OemPeriod) || (e.Key == Key.Decimal))
            {
                e.Handled = true;
            }
        }
    }
}
