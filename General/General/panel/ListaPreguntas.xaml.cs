using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace General.panel
{
    /// <summary>
    /// Interaction logic for ListaPreguntas.xaml
    /// </summary>
    public partial class ListaPreguntas : UserControl
    {
        public static readonly DependencyProperty TextoProperty =
    DependencyProperty.Register(
    "Texto", typeof(Boolean),
    typeof(Boolean), null
    );
        public bool Texto
        {
            get { return (bool)GetValue(TextoProperty); }
            set { SetValue(TextoProperty, value); }
        }

        public ListaPreguntas()
        {
            InitializeComponent();
        }
    }
}