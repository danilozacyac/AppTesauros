using System;
using System.Linq;
using System.Security;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using mx.gob.scjn.directorio.CJF;
using mx.gob.scjn.directorio.General;
using mx.gob.scjn.directorio.SCJN;
using mx.gob.scjn.directorio.TEPJF;

[assembly: AllowPartiallyTrustedCallers]
namespace mx.gob.scjn.directorio.gui
{

    /// <summary>
    /// Página de inicio para el directorio del Poder Judicial de la Federación.
    /// </summary>
    public partial class PresentacionDirectorio : Page
    {

        public Page Back;
        public PresentacionDirectorio()
        {
            InitializeComponent();
        }

        private void Imagen_MouseEnter(object sender, MouseEventArgs e)
        {
            try
            {
                Image imagen = (Image)sender;
                imagen.Opacity = 1;
            }
            catch (System.Exception error)

            {
                //Handle exception here
            }
        }

        private void Imagen_MouseLeave(object sender, MouseEventArgs e)
        {
            try
            {
                Image imagen = (Image)sender;
                imagen.Opacity = 0.5;
            }
            catch (System.Exception error)
            {
                //Handle exception here
            }
        }

        private void Salir_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 1)
            {
                this.NavigationService.Navigate(Back);
            }
        }

        private void TEF_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (e.ClickCount == 1)
                {
                    TRIFEMagistrados pageTRIFE = new TRIFEMagistrados();
                    pageTRIFE.Back = this;
                    this.NavigationService.Navigate(pageTRIFE);
                }
            }
            catch (System.Exception error)
            {
                //Handle exception here
            }
        }

        private void CJF_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (e.ClickCount == 1)
                {
                    CJFPrincipal pagCJF = new CJFPrincipal();
                    pagCJF.Back = this;
                    this.NavigationService.Navigate(pagCJF);
                }
            }
            catch (System.Exception error)
            {
                //Handle exception here
            }
        }

        private void CJF_ImageFailed(object sender, EventArgs e)
        {
            try
            {                
                pageOrganismo pagCJFOrg = new pageOrganismo();
                pagCJFOrg.Back = this;
                this.NavigationService.Navigate(pagCJFOrg);
            }
            catch (System.Exception error)
            {
                //Handle exception here
            }
        }

        private void SCJN_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (e.ClickCount == 1)
                {
                    SCJNPrincipal pagSCJN = new SCJNPrincipal();
                    pagSCJN.Back = this;
                    this.NavigationService.Navigate(pagSCJN);
                }
            }
            catch (System.Exception error)
            {
                //Handle exception here
            }
        }

        private void SCJN_ImageFailed(object sender, EventArgs e) { }
        private void CJF_ImageFailed(object sender, ExceptionRoutedEventArgs e) { }
        private void TEF_ImageFailed(object sender, ExceptionRoutedEventArgs e) { }
        private void SCJN_ImageFailed(object sender, ExceptionRoutedEventArgs e) { }
        private void Salir_ImageFailed(object sender, ExceptionRoutedEventArgs e) { }
    }
}
