using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using mx.gob.scjn.ius_common.gui.utils;

namespace mx.gob.scjn.ius_common.gui.noticia
{
    /// <summary>
    /// Interaction logic for SeleccionMinistros.xaml
    /// </summary>
    public partial class SeleccionMinistros : Page
    {
        protected int Seleccion { get; set; }
        public Page Back { get; set; }
        protected int[] validos ;
        public SeleccionMinistros()
        {
            InitializeComponent();
            Seleccion = 10;
            validos = new int[25];
            validos[0] =10;
            validos[1] =11;
            validos[2] =12;
            validos[3] =13;
            validos[4] =14;
            validos[5] =15;
            validos[6] =20;
            validos[7] =21;
            validos[8] =22;
            validos[9] =23;
            validos[10] = 24;
            validos[11] = 30;
            validos[12] = 31;
            validos[13] = 32;
            validos[14] =33;
            validos[15] = 34;
            validos[16] = 35;
            validos[17] = 36;
            validos[18] =40;
            validos[19] = 41;
            validos[20] = 42;
            validos[21] = 43;
            validos[22] = 44;
            validos[23] = 45;
            validos[24] = 46;
        }

        private void Visualizar_Click(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show("Seleccion = " + Seleccion);
            if (validos.Contains(Seleccion))
            {
                FramesResultados resultados = new FramesResultados(Seleccion);
                if (this.Back != null)
                {
                    resultados.Back = this.Back;
                }
                this.NavigationService.Navigate(resultados);
            }
            else
            {
                MessageBox.Show(Mensajes.MENSAJE_SELECCIONE_EPOCA, Mensajes.TITULO_SELECCIONE_EPOCA,
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Radios_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton boton = (RadioButton)sender;
            Seleccion = Int32.Parse((String)boton.Tag);
        }

        private void Salir_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            if (this.Back == null)
            {
                this.NavigationService.GoBack();
            }
            else
            {
                this.NavigationService.Navigate(Back);
            }
        }
    }
}
