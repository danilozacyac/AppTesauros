using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using mx.gob.scjn.ius_common.TO;

namespace IUS
{
    /// <summary>
    /// Interaction logic for ConsultasEspeciales.xaml
    /// </summary>
    public partial class ConsultasEspeciales : Page
    {
        public Page Back { get; set; }
        public ConsultasEspeciales()
        {
            InitializeComponent();
        }

        private void treeView1_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Cursor = Cursors.Wait;
            Borde.IsHitTestVisible = false;
            TreeView arbol = (TreeView)sender;
            TreeViewItem seleccionado = (TreeViewItem)arbol.SelectedItem;
            if (seleccionado != null)
            {
                String seleccion = (String)seleccionado.Header;
                MostrarPorIusTO parametros = new MostrarPorIusTO ();
                switch (seleccion)
                {
                    case "Suspensión del acto reclamado":
                        parametros.BusquedaEspecialValor = "1";
                        break;
                    case "Improcedencia del juicio de amparo":
                        parametros.BusquedaEspecialValor = "2";
                        break;
                    case "Investigación sobre la violación grave de garantías individuales":
                        parametros.BusquedaEspecialValor = "51";
                        break;
                    case "Investigación sobre la violación grave del voto público":
                        parametros.BusquedaEspecialValor = "52";
                        break;
                    case "Controversias constitucionales":
                        parametros.BusquedaEspecialValor = "53";
                        break;
                    case "Acciones de inconstitucionalidad":
                        parametros.BusquedaEspecialValor = "54";
                        break;
                    case "Jurisprudencia por contradicción de tesis":
                        parametros.BusquedaEspecialValor = "61";
                        break;
                    case "Jurisprudencia por reiteración y tesis aisladas provenientes de la resolución de contradicciones":
                        parametros.BusquedaEspecialValor = "62";
                        break;
                    case "Electoral":
                        parametros.BusquedaEspecialValor = "15";
                        break;
                    default:
                        Cursor = Cursors.Arrow;
                        Borde.IsHitTestVisible = true;
                        return;
                }
                parametros.OrderBy = "ConsecIndx";
                parametros.OrderType = "asc";
                parametros.FilterValue = (String)seleccionado.Header;
                tablaResultado paginaTesis = new tablaResultado(parametros);
                paginaTesis.Back = this;
                this.NavigationService.Navigate(paginaTesis);
            }
            Borde.IsHitTestVisible = true;
            Cursor = Cursors.Arrow;
        }

        private void Salir_MouseLeftButtonDown(object sender, RoutedEventArgs e)
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

        private void TreeViewItem_Selected(object sender, RoutedEventArgs e)
        {

        }

        private void treeView1_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {

        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Borde.IsHitTestVisible = true;
        }
    }
}
