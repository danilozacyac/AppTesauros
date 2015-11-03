using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace mx.gob.scjn.directorio
{

    /// <summary>
    /// Mensajes.xaml
    /// </summary>
    public partial class MensajesAvisos : UserControl
    {
        FondoTransparente FTransp;

        public MensajesAvisos()
        {
            InitializeComponent();

        }

        void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            Visibility = Visibility.Collapsed;
            FTransp.Visibility = Visibility.Hidden;
        }

        public void MuestraMensajes(String strCabecera, String strMensaje)
        {
            setStrCabecera(strCabecera);
            setStrMensaje(strMensaje);
            this.Visibility = Visibility.Visible;
        }

        public void TomaFondo(FondoTransparente Ft)
        {
            FTransp = Ft;
            FTransp.Visibility = Visibility.Visible;
        }


        public String StrMensaje { get { return this.getStrMensaje(); } set { this.setStrMensaje(value); } }

        private String strMensaje;

        public String StrCabecera { get { return this.getStrCabecera(); } set { this.setStrCabecera(value); } }

        private String strCabecera;
        private void setStrCabecera(String value)
        {
            this.strCabecera = value;
            this.Titulo.Content = value;
        }

        private String getStrCabecera()
        {
            return strCabecera;
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
            try
            {

                this.Visibility = Visibility.Hidden;
                Visibility = Visibility.Collapsed;
                FTransp.Visibility = Visibility.Hidden;
            }
            catch (System.Exception error)
            {
                //Handle exception here
            }
        }

        private void Salir_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            //this.Visibility = Visibility.Hidden;
        }

        private void BarraMovimiento_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
        }

        private void BarraMovimiento_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
        }

        private void BarraMovimiento_MouseMove(object sender, MouseEventArgs e)
        {
        }

        private void Salir_MouseButtonDown(object sender, RoutedEventArgs e)
        {
            try
            {

                this.Visibility = Visibility.Hidden;
                Visibility = Visibility.Collapsed;
                FTransp.Visibility = Visibility.Hidden;
            }
            catch (System.Exception error)
            {
                //Handle exception here
            }
 
        }


    }
}
