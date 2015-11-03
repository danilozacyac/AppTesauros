using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace mx.gob.scjn.directorio.CJF
{

    /// <summary>
    /// Interaction logic for CJFCircuitos.xaml
    /// </summary>
    public partial class CJFCircuitos : Page
    {

        public Page Back { get; set; }
        public Page contenedor { get; set; }
        public CJFCircuitos()
        {
            InitializeComponent();

            try
            {
                Uri lugar = new Uri("pack://siteoforigin:,,,/General/Circuitos.htm", UriKind.Absolute);
                Circuito.Source = lugar;
            }

            catch (Exception exc) { MessageBox.Show("Error al cargar el documento:" + exc.Message, MensajesDirectorio.TITULO_MENSAJES, MessageBoxButton.OK, MessageBoxImage.Error); }
        }
    }
}
