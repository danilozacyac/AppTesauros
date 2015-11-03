using System;
using System.Linq;
using System.Security;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using mx.gob.scjn.electoral.Controller;
using mx.gob.scjn.electoral.Controller.Impl;

[assembly: AllowPartiallyTrustedCallers]
namespace mx.gob.scjn.electoral
{
    /// <summary>
    /// Interaction logic for MenuElectoral.xaml
    /// </summary>
    public partial class MenuElectoral : Page
    {
        public IMenuElectoral Controlador { get; set; }
        public Page Back { get; set; }

        public MenuElectoral()
        {
            InitializeComponent();
            Controlador = new MenuElectoralImpl();
            Controlador.Ventana = this;
            Controlador.InicializaPaneles();
        }

        private void BtnSalir_Click(object sender, RoutedEventArgs e)
        {
            Controlador.BtnSalirClic();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Controlador != null)
            {
                Controlador.CbxTipoDocChanged();
            }
        }

        private void ImgBtnLC_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Controlador.BtnSecuencialClic();
        }

        private void ImgBtnBP_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Controlador.BtnPalabraClic(e);
        }

        private void ImgBtnBR_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Controlador.BtnRegistroClic();
        }

        private void ImgBtnLC_MouseEnter(object sender, MouseEventArgs e)
        {
            Image imagenNueva = new Image();
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri("/General;component/images/Bus-secuencial2.png", UriKind.Relative);
            bitmap.EndInit();
            this.ImgBtnLC.Source = bitmap;

        }

        private void ImgBtnLC_MouseLeave(object sender, MouseEventArgs e)
        {
            Image imagenNueva = new Image();
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri("/General;component/images/Bus-secuencial.png", UriKind.Relative);
            bitmap.EndInit();
            this.ImgBtnLC.Source = bitmap;

        }

        private void ImgBtnBP_MouseEnter(object sender, MouseEventArgs e)
        {
            Image imagenNueva = new Image();
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri("/General;component/images/Bus-palabra2.png", UriKind.Relative);
            bitmap.EndInit();
            this.ImgBtnBP.Source = bitmap;
        }

        private void ImgBtnBP_MouseLeave(object sender, MouseEventArgs e)
        {
            Image imagenNueva = new Image();
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri("/General;component/images/Bus-palabra.png", UriKind.Relative);
            bitmap.EndInit();
            this.ImgBtnBP.Source = bitmap;

        }

        private void ImgBtnBR_MouseEnter(object sender, MouseEventArgs e)
        {
            Image imagenNueva = new Image();
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri("/General;component/images/Bus-registro2.png", UriKind.Relative);
            bitmap.EndInit();
            this.ImgBtnBR.Source = bitmap;
        }

        private void ImgBtnBR_MouseLeave(object sender, MouseEventArgs e)
        {
            Image imagenNueva = new Image();
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri("/General;component/images/Bus-registro.png", UriKind.Relative);
            bitmap.EndInit();
            this.ImgBtnBR.Source = bitmap;
        }
    }
}
