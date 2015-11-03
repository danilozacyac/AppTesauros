using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace mx.gob.scjn.directorio.CJF
{
    /// <summary>
    /// Interaction logic for InicialCJF.xaml
    /// </summary>
    public partial class InicialCJF : Page
    {
        public Page Back { get; set; }
        public InicialCJF()
        {
            InitializeComponent();
        }

       

        private void btnIni_MouseEnter(object sender, MouseEventArgs e)
        {
            this.Background = btnOver.Background;
        }

        private void btnIni_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Background = btnClick.Background;
        }

        private void btnIni_MouseLeave(object sender, MouseEventArgs e)
        {
            this.Background = btnSolo.Background;
        }

        private void button3_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
