using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using mx.gob.scjn.ius_common.TO;
using mx.gob.scjn.ius_common.utils;

namespace IUS.Indices
{
    /// <summary>
    /// Presentación de las categorías de los índices
    /// </summary>
    public partial class Inicial : Page
    {
        public Page Back { get; set; }
        public Inicial()
        {
            InitializeComponent();
            foreach (TreeViewItem item in contieneArbol.Items)
            {
                item.IsExpanded = true;
                foreach (TreeViewItem itemSegundo in item.Items)
                {
                    itemSegundo.IsExpanded = true;
                }
                {
                    ;
                }
            }
        }

        private void Salir_MouseButtonDown(object sender, RoutedEventArgs e)
        {
            if (Back == null)
            {
                PaginaInicial pagina = new PaginaInicial();
                this.NavigationService.Navigate(pagina);
            }
            else
            {
                this.NavigationService.Navigate(Back);
            }
        }

        private void TextBlock_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            Cursor = Cursors.Wait;
            CnvEsperar.Visibility = Visibility.Visible;
            TreeView arbol = (TreeView)sender;
            TreeViewItem bloque = (TreeViewItem)arbol.SelectedItem;
            if (bloque.ItemsSource != null)
            {
                this.contieneResultados.Content = null;
                this.CnvEsperar.Visibility = Visibility.Hidden;
                Cursor = Cursors.Arrow;
                return;
            }
            TreeNodeDataTO nodo = (TreeNodeDataTO)bloque.Tag;
            if (nodo.IsLeaf)
            {
                BusquedaTO parametros = new BusquedaTO();
                parametros.TipoBusqueda = Constants.BUSQUEDA_INDICES;
                parametros.OrdenarPor = Constants.ORDER_INDICE;
                //parametros.O = Constants.ORDER_TYPE_DEFAULT;
                parametros.FiltrarPor = nodo.Id;
#if STAND_ALONE
                parametros.Clasificacion = new List<ClassificacionTO>();
                parametros.Clasificacion.Add(new ClassificacionTO());
                parametros.Clasificacion[0].DescTipo = nodo.Label;
#else
                parametros.clasificacion = new ClassificacionTO[1];
                parametros.clasificacion[0] = new ClassificacionTO();
                parametros.clasificacion[0].DescTipo = nodo.Label;
#endif
                tablaResultado resultado = new tablaResultado(parametros);
                //resultado.Back = this;
                resultado.PaginaIndices = this;
                this.contieneResultados.Navigate(resultado);
                Cursor = Cursors.Arrow;
            }
            else
            {
                Cursor = Cursors.Arrow;
            }
            CnvEsperar.Visibility = Visibility.Hidden;
        }

    }
}
