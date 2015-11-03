using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace mx.gob.scjn.directorio.CJF
{

    /// <summary>
    /// Interaction logic for CJFPrincipal.xaml
    /// </summary>
    public partial class CJFPrincipal : Page
    {

        private Brush ColorBase = (Brush)Application.Current.TryFindResource("ColorBase");
        private Brush ColorClaro = (Brush)Application.Current.TryFindResource("ColorClaro");
        private Brush ColorFuenteEtiqueta = (Brush)Application.Current.TryFindResource("ColorFuenteEtiqueta");
       
        private double margenTextoEnter = 1.0;
        private int margenTextoLeave = 0;
        private int FonSizeOver = 13;
        private int FonSizeOut = 11;

        public CJFPrincipal()
        {
            InitializeComponent();
        }

        public Page Back { get; set; }

        private void VerFuncAdmin(object sender, MouseButtonEventArgs e)
        {
            CJFFuncionariosAdmin CJF_FuncAdmin = new CJFFuncionariosAdmin();
            this.framContenedor.Navigate(CJF_FuncAdmin);
            ActualizaEstados(3);
        }

        public void IconoRegresarVisible(Boolean bVisible)
        {

            if (bVisible)
            {
                Salir.Visibility = Visibility.Visible;
            }
            else
            {
                Salir.Visibility = Visibility.Hidden;
            }
        }
        # region Actualizar Frame

        private void VERCONS(object sender, MouseButtonEventArgs e)
        {
            CJFConsejeros CJF_CONS = new CJFConsejeros();
            CJF_CONS.contenedor = this;
            this.framContenedor.Navigate(CJF_CONS);
            ActualizaEstados(10);
        }

        private void VERTCC(object sender, MouseButtonEventArgs e)
        {
            pageOrganoJur CJF_TCC = new pageOrganoJur("TCC");
            this.framContenedor.Navigate(CJF_TCC);
            ActualizaEstados(8);
        }

        private void VERJUZ(object sender, MouseButtonEventArgs e)
        {
            pageOrganoJur CJF_JUZ = new pageOrganoJur("JUZ");
            this.framContenedor.Navigate(CJF_JUZ);
            ActualizaEstados(9);
        }

        private void VERTUC(object sender, MouseButtonEventArgs e)
        {
            pageOrganoJur CJF_TUC = new pageOrganoJur("TUC");
            this.framContenedor.Navigate(CJF_TUC);
            ActualizaEstados(7);
        }

        private void VerJuecesMag(object sender, MouseButtonEventArgs e)
        {
            CJFFuncionarios CJF_JUEMAG = new CJFFuncionarios();
            this.framContenedor.Navigate(CJF_JUEMAG);
            ActualizaEstados(4);
        }

        private void VerAA(object sender, MouseButtonEventArgs e)
        {
            CJFAreasAdmin CJF_AA = new CJFAreasAdmin(0);
            this.framContenedor.Navigate(CJF_AA);
            ActualizaEstados(5);
        }

        private void VerCJF(object sender, MouseButtonEventArgs e)
        {
            CJFComisiones CJF_COM = new CJFComisiones();
            this.framContenedor.Navigate(CJF_COM);
            ActualizaEstados(2);
        }

        private void VerOA(object sender, MouseButtonEventArgs e)
        {
            CJFAreasAdmin CJF_AA = new CJFAreasAdmin(1);
            this.framContenedor.Navigate(CJF_AA);
            ActualizaEstados(1);
        }

        #endregion  Actualizar Frame

        #region Eventos Etiquetas Menu

        private void lblCJF_MouseEnter(object sender, MouseEventArgs e)
        {
            lblCJF.BorderThickness = new Thickness(margenTextoEnter);
        }

        private void lblCJF_MouseLeave(object sender, MouseEventArgs e)
        {
            lblCJF.BorderThickness = new Thickness(margenTextoLeave);
        }

        private void lblJUZ_MouseEnter(object sender, MouseEventArgs e)
        {
            lblJUZ.BorderThickness = new Thickness(margenTextoEnter);
        }

        private void lblJUZ_MouseLeave(object sender, MouseEventArgs e)
        {
            lblJUZ.BorderThickness = new Thickness(margenTextoLeave);
        }

        private void lblTCC_MouseEnter(object sender, MouseEventArgs e)
        {
            lblTCC.BorderThickness = new Thickness(margenTextoEnter);
        }

        private void lblTCC_MouseLeave(object sender, MouseEventArgs e)
        {
            lblTCC.BorderThickness = new Thickness(margenTextoLeave);
        }

        private void lblFuncAdmin_MouseLeave(object sender, MouseEventArgs e)
        {
            lblFuncAdmin.BorderThickness = new Thickness(margenTextoLeave);
        }

        private void lblTUC_MouseEnter(object sender, MouseEventArgs e)
        {
            lblTUC.BorderThickness = new Thickness(margenTextoEnter);
        }

        private void lblTUC_MouseLeave(object sender, MouseEventArgs e)
        {
            lblTUC.BorderThickness = new Thickness(margenTextoLeave);
        }

        private void lblJuecMag_MouseEnter(object sender, MouseEventArgs e)
        {
            lblJuecMag.BorderThickness = new Thickness(margenTextoEnter);
        }

        private void lblJuecMag_MouseLeave(object sender, MouseEventArgs e)
        {
            lblJuecMag.BorderThickness = new Thickness(margenTextoLeave);
        }

        private void lblAA_MouseEnter(object sender, MouseEventArgs e)
        {
            lblAA.BorderThickness = new Thickness(margenTextoEnter);
        }

        private void lblAA_MouseLeave(object sender, MouseEventArgs e)
        {
            lblAA.BorderThickness = new Thickness(margenTextoLeave);
        }

        private void lblAAOA_MouseEnter(object sender, MouseEventArgs e)
        {
            lblAAOA.BorderThickness = new Thickness(margenTextoEnter);
        }

        private void lblAAOA_MouseLeave(object sender, MouseEventArgs e)
        {
            lblAAOA.BorderThickness = new Thickness(margenTextoLeave);
        }

        private void lblFuncAdmin_MouseEnter(object sender, MouseEventArgs e)
        {
            lblFuncAdmin.BorderThickness = new Thickness(margenTextoEnter);
        }

        private void lblCON_MouseEnter(object sender, MouseEventArgs e)
        {
            lblCON.BorderThickness = new Thickness(margenTextoEnter);
        }

        private void lblCON_MouseLeave(object sender, MouseEventArgs e)
        {
            lblCON.BorderThickness = new Thickness(margenTextoLeave);
        }

        private void lblOC_MouseEnter(object sender, MouseEventArgs e)
        {
            lblOfC.BorderThickness = new Thickness(margenTextoEnter);
        }

        private void lblOC_MouseLeave(object sender, MouseEventArgs e)
        {
            lblOfC.BorderThickness = new Thickness(margenTextoLeave);
        }

        private void lblCircuitos_MouseEnter(object sender, MouseEventArgs e)
        {
            lblCircuitos.BorderThickness = new Thickness(margenTextoEnter);
        }

        private void lblCircuitos_MouseLeave(object sender, MouseEventArgs e)
        {
            lblCircuitos.BorderThickness = new Thickness(margenTextoLeave);
        }

        #endregion Eventos Etiquetas Menu

        private void Salir_ImageFailed(object sender, ExceptionRoutedEventArgs e)
        { }

        private void Salir_MouseButtonDown(object sender, MouseButtonEventArgs e)
        {
            //try
            //{
            //    if (Back == null)
            //    {
            //        this.NavigationService.GoBack();
            //    }
            //    else
            //    {
            //        this.NavigationService.Navigate(Back);
            //    }
            //}
            //catch (System.Exception error)
            //{
            //    //Handle exception here
            //}
        }

        private void ActualizaEstados(int nActivo)
        {
            lblAAOA.Background = ColorBase;
            lblCJF.Background = ColorBase;
            lblFuncAdmin.Background = ColorBase;
            lblJuecMag.Background = ColorBase;
            lblAA.Background = ColorBase;
            lblTUC.Background = ColorBase;
            lblTCC.Background = ColorBase;
            lblJUZ.Background = ColorBase;
            lblCON.Background = ColorBase;
            lblOfC.Background = ColorBase;
            lblCircuitos.Background = ColorBase;
            Salir.Visibility = Visibility.Visible;
            lblAAOA.FontSize = FonSizeOut;
            lblCJF.FontSize = FonSizeOut;
            lblFuncAdmin.FontSize = FonSizeOut;
            lblJuecMag.FontSize = FonSizeOut;
            lblAA.FontSize = FonSizeOut;
            lblTUC.FontSize = FonSizeOut;
            lblTCC.FontSize = FonSizeOut;
            lblJUZ.FontSize = FonSizeOut;
            lblCON.FontSize = FonSizeOut;
            lblOfC.FontSize = FonSizeOut;
            lblCircuitos.FontSize = FonSizeOut;

            switch (nActivo)
            {

                case 1:
                    lblAAOA.Background = ColorBase;
                    lblAAOA.FontSize = FonSizeOver;
                    break;

                case 2:
                    lblCJF.Background = ColorBase;
                    lblCJF.FontSize = FonSizeOver;
                    break;

                case 3: lblFuncAdmin.Background = ColorBase;
                    lblFuncAdmin.FontSize = FonSizeOver;
                    break;

                case 4: lblJuecMag.Background = ColorBase;
                    lblJuecMag.FontSize = FonSizeOver;
                    break;

                case 5: lblAA.Background = ColorBase;
                    lblAA.FontSize = FonSizeOver;
                    break;

                case 6: lblJuecMag.Background = ColorBase;
                    lblJuecMag.FontSize = FonSizeOver;
                    break;

                case 7: lblTUC.Background = ColorBase;
                    lblTUC.FontSize = FonSizeOver;
                    break;

                case 8: lblTCC.Background = ColorBase;
                    lblTCC.FontSize = FonSizeOver;
                    break;

                case 9: lblJUZ.Background = ColorBase;
                    lblJUZ.FontSize = FonSizeOver;
                    break;

                case 10: lblCON.Background = ColorBase;
                    lblCON.FontSize = FonSizeOver;
                    break;

                case 11: lblOfC.Background = ColorBase;
                    lblOfC.FontSize = FonSizeOver - 1;
                    break;

                case 12: lblCircuitos.Background = ColorBase;
                    lblCircuitos.FontSize = FonSizeOver - 1;
                    break;
            }
        }

        private void VEROC(object sender, MouseButtonEventArgs e)
        {
            CJFOfCorrespondencia CJF_OC = new CJFOfCorrespondencia();
            CJF_OC.contenedor = this;
            this.framContenedor.Navigate(CJF_OC);
            ActualizaEstados(11);
        }

        private void VERCT(object sender, MouseButtonEventArgs e)
        {
            CJFCircuitos CJF_CT = new CJFCircuitos();
            CJF_CT.contenedor = this;
            this.framContenedor.Navigate(CJF_CT);
            ActualizaEstados(12);
        }

        private void Salir_MouseButtonDown(object sender, RoutedEventArgs e)
        {

            try
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

            catch (System.Exception error)
            {
                //Handle exception here
            }
        }
    }
}
