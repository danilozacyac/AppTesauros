using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using mx.gob.scjn.ius_common.TO;
using mx.gob.scjn.ius_common.utils;

namespace mx.gob.scjn.directorio.General
{

    /// <summary>
    /// Interaction logic for Semblanza.xaml
    /// </summary>
    public partial class Semblanza : Page
    {

        public Page Back { get; set; }
        public Page contenedor { get; set; }

        private FlowDocument DocumentoParaCopiar { get; set; }
        WebBrowser SemblanzaHTML_Copy = new WebBrowser();

        private String strArchivo = "";

        public Semblanza()
        {
            InitializeComponent();

            try
            {
                Uri lugar = new Uri("pack://siteoforigin:,,,/General/Semblanzas/GDGP.HTML", UriKind.Absolute);
                SemblanzaHTML.Source = lugar;
                SemblanzaHTML_Copy.Source = lugar;
            }

            catch (Exception exc) { MessageBox.Show("Error al cargar el documento:" + exc.Message, MensajesDirectorio.TITULO_MENSAJES, MessageBoxButton.OK, MessageBoxImage.Error); }
        }

        //      public Semblanza(DirectorioMinistrosTO Persona)
        public Semblanza(Object Persona)
        {
            DirectorioMinistrosTO oPersona = (DirectorioMinistrosTO)Persona;
            InitializeComponent();
            strArchivo = MuestraSemblanza(oPersona);
        }

        private String MuestraSemblanza(DirectorioMinistrosTO Persona)
        {
            string Archivo = "";

            try
            {
                txtTituloSemblanza.Text = Persona.TitSemblanza;
#if STAND_ALONE
                Archivo = IUSConstants.IUS_RUTA_ANEXOS + "General/Semblanzas/" + Persona.ArchivoSemblanza;
#else
                Archivo = "pack://siteoforigin:,,,/General/Semblanzas/" + Persona.ArchivoSemblanza;
#endif
                Uri lugar = new Uri(Archivo, UriKind.Absolute);
                SemblanzaHTML.Source = lugar;
            }

            catch (Exception exc)
            {
            }
            return Archivo;
        }

        private void Salir_MouseEnter(object sender, MouseEventArgs e)
        {
        }

        private void Salir_MouseLeave(object sender, MouseEventArgs e)
        {
        }

        private void Salir_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
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

        private void Salir_ImageFailed(object sender, ExceptionRoutedEventArgs e)
        {
        }

        private void Guardar(object sender, MouseButtonEventArgs e)
        {
        }

        private void Imprimir(object sender, MouseButtonEventArgs e)
        {
            //MessageBox.Show(MensajesDirectorio.MENSAJE_IMPRIMIR_SEMBLANZA, MensajesDirectorio.TITULO_MENSAJES);
        }

        private void PortaPapeles(object sender, MouseButtonEventArgs e)
        {
            //MessageBox.Show(MensajesDirectorio.MENSAJE_ENVIAR_AL_PORTAPAPELES, MensajesDirectorio.TITULO_MENSAJES);
        }

        private void Salir(object sender, MouseButtonEventArgs e)
        {
            //this.NavigationService.GoBack();
        }

        private void Imprimir__ImageFailed(object sender, ExceptionRoutedEventArgs e)
        {
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void Guardar_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Imprimir_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(MensajesDirectorio.MENSAJE_IMPRIMIR_SEMBLANZA, MensajesDirectorio.TITULO_MENSAJES);

        }

        private void PortaPapeles_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(MensajesDirectorio.MENSAJE_ENVIAR_AL_PORTAPAPELES, MensajesDirectorio.TITULO_MENSAJES);
        }

        private void Salir_MouseButtonDown(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();

        }
    }
}
