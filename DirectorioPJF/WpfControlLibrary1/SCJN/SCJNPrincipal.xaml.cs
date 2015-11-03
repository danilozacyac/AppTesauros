using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace mx.gob.scjn.directorio.SCJN
{

    /// <summary>
    /// Interaction logic for SCJNPrincipal.xaml
    /// </summary>
    public partial class SCJNPrincipal : Page
    {
        private Brush ColorBase = Brushes.Moccasin;
        private Brush ColorClaro = Brushes.Maroon;
        private Brush ColorFuenteEtiqueta = Brushes.White;  

        private double margenTextoEnter = 1.0;
        private int margenTextoLeave = 0;
        private double margenTextoActivo = 2.0;
        private int nFrameActivo = 0;

        public SCJNPrincipal()
        {
            InitializeComponent();
        }

        public Page Back { get; set; }

        private void VERMIN(object sender, MouseButtonEventArgs e)
        {
            SCJNMinistros SCJN_MIN = new SCJNMinistros();
            this.framContenedor.Navigate(SCJN_MIN);
            ActualizaEstados(1);
        }

        private void VERSCJN(object sender, MouseButtonEventArgs e)
        {
            SCJNPage SCJN_P = new SCJNPage();
            this.framContenedor.Navigate(SCJN_P);
            ActualizaEstados(2);
        }

        private void VERAA(object sender, MouseButtonEventArgs e)
        {
            SCJNAreasAdmin SCJN_AA = new SCJNAreasAdmin();
            this.framContenedor.Navigate(SCJN_AA);
            ActualizaEstados(3);
        }

        private void VERFP(object sender, MouseButtonEventArgs e)
        {
            SCJNFuncionariosAdmin CJF_FA = new SCJNFuncionariosAdmin();
            this.framContenedor.Navigate(CJF_FA);
            ActualizaEstados(4);
        }

        private void ActualizaEstados(int nActivo)
        {
            nFrameActivo = nActivo;
            lblMIN.Background = ColorBase;
            lblSCJN.Background = ColorBase;
            lblAA.Background = ColorBase;
            lblFP.Background = ColorBase;
            lblMIN.FontSize = 12;
            lblSCJN.FontSize = 12;
            lblAA.FontSize = 12;
            lblFP.FontSize = 12;

            switch (nActivo)
            {

                case 1: lblMIN.Background = ColorBase;
                    lblMIN.FontSize = 15;
                    break;

                case 2: lblSCJN.Background = ColorBase;
                    lblSCJN.FontSize = 15;
                    break;

                case 3: lblAA.Background = ColorBase;
                    lblAA.FontSize = 15;
                    break;

                case 4: lblFP.Background = ColorBase;
                    lblFP.FontSize = 15;
                    break;
            }
        }

        private void lblMIN_MouseEnter(object sender, MouseEventArgs e)
        {
            lblMIN.BorderThickness = new Thickness(margenTextoEnter);
        }

        private void lblAA_MouseEnter(object sender, MouseEventArgs e)
        {
            lblAA.BorderThickness = new Thickness(margenTextoEnter);
        }

        private void lblAA_MouseLeave(object sender, MouseEventArgs e)
        {
            lblAA.BorderThickness = new Thickness(margenTextoLeave);
        }

        private void lblMIN_MouseLeave(object sender, MouseEventArgs e)
        {
            lblMIN.BorderThickness = new Thickness(margenTextoLeave);
        }

        private void lblSCJN_MouseEnter(object sender, MouseEventArgs e)
        {
            lblSCJN.BorderThickness = new Thickness(margenTextoEnter);
        }

        private void lblSCJN_MouseLeave(object sender, MouseEventArgs e)
        {
            lblSCJN.BorderThickness = new Thickness(margenTextoLeave);
        }

        private void lblFP_MouseEnter(object sender, MouseEventArgs e)
        {
            lblFP.BorderThickness = new Thickness(margenTextoEnter);
        }

        private void lblFP_MouseLeave(object sender, MouseEventArgs e)
        {
            lblFP.BorderThickness = new Thickness(margenTextoLeave);
        }

        private void Salir_MouseButtonDown(object sender, MouseButtonEventArgs e)
        {

            //if (Back == null)
            //{
            //    this.NavigationService.GoBack();
            //}
            //else
            //{
            //    this.NavigationService.Navigate(Back);
            //}
        }

        private void Guardar(object sender, MouseButtonEventArgs e)
        {
        }

        private void Imprimir(object sender, MouseButtonEventArgs e)
        {
            //switch (nFrameActivo)
            //{
            //    case 0: ImprListMin();
            //        break;
            //    case 1: ImprListMin();
            //        break;
            //    case 2: lblSCJN.Background = Brushes.Brown;
            //        lblSCJN.FontSize = 15;
            //        break;
            //    case 3: lblAA.Background = Brushes.Brown;
            //        lblAA.FontSize = 15;
            //        break;
            //    case 4: lblFP.Background = Brushes.Brown;
            //        lblFP.FontSize = 15;
            //        break;
            //}
        }

        private void PortaPapeles(object sender, MouseButtonEventArgs e)
        {
        }

        private void Imprimir__ImageFailed(object sender, ExceptionRoutedEventArgs e)
        {
        }

        private void Salir_MouseButtonDown(object sender, RoutedEventArgs e)
        {

            if (Back == null)
            {
                this.NavigationService.GoBack();
            }
            else
            {
                this.NavigationService.Navigate(Back);
            }

        }
        //private void ImprListMin()
        //{
        //    System.Type typeFrame = framContenedor.GetType();
        //    //if (typeFrame == typeof(SCJNMinistros))
        //    //{
        //        SCJNMinistros SCJN_MIN = new SCJNMinistros();
        //        SCJN_MIN.ImprimeListadoMinistros();
        //        //SCJNMinistros   nuevoFrame = new (SCJNMinistros)framContenedor();
        //    //}
        //}
        //        private void ImprimeMinistros() {
        ////            List<DirectorioMinistrosTO> lstResImpr = new List<DirectorioMinistrosTO>();
        //            foreach (item in (lstResImpr))
        //            {
        //                identificadores.Add(Int32.Parse(item.Id));
        //            }
        //        }
    }
}
