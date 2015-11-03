using System;
using System.Linq;
using System.Windows;
using TesauroUtilities;

namespace AppTesauro09wpf.Utils
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class MoverCopiar : Window
    {
        public int Resultado { get; set; }
        public MoverCopiar()
        {
            InitializeComponent();
            Resultado = Constants.CANCELAR;
        }

        private void BtnCopiar_Click(object sender, RoutedEventArgs e)
        {
            Resultado = Constants.COPIAR;
            this.Close();
        }

        private void BtnMover_Click(object sender, RoutedEventArgs e)
        {
            Resultado = Constants.MOVER;
            this.Close();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            Resultado = Constants.CANCELAR;
            this.Close();
        }
    }
}
