using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mx.gob.scjn.ius_common.fachade;
using System.Collections.ObjectModel;
using mx.gob.scjn.ius_common.TO;
using System.Windows;
using System.Windows.Controls;

namespace mx.gob.scjn.ius_common.utils
{
    public class IndicesTreeView
    {
        public ObservableCollection<TreeViewItem> Hijos { get { return this.hijos; } set { this.setHijos(value); } }
        private ObservableCollection<TreeViewItem> hijos;
        public TreeNodeDataTO NodoActual { get { return this.nodoActual; } set { this.setNodoActual(value); } }
        private TreeNodeDataTO nodoActual;

        public IndicesTreeView()
        {
            IniciaTreeView();
        }
        public void ObtenHijos(TreeNodeDataTO nodo)
        {
            this.NodoActual = nodo;
        }
        private void IniciaTreeView()
        {
            TreeNodeDataTO nodoInicial = new TreeNodeDataTO();
            nodoInicial.Label = "Inicio";
            nodoInicial.Id = "1";
            nodoInicial.Href = "";
            nodoInicial.IsLeaf = false;
            this.NodoActual = nodoInicial;
            hijos = hijosNuevos(nodoInicial);
        }

        public void setHijos(ObservableCollection<TreeViewItem> value)
        {
            this.hijos= value;
        }

        public void setNodoActual(TreeNodeDataTO value)
        {
            nodoActual=value;
        }
        public ObservableCollection<TreeViewItem> hijosNuevos(TreeNodeDataTO value)
        {
            try
            {
                this.nodoActual = value;
                if (!this.NodoActual.IsLeaf)
                {
                    FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
                    TreeNodeDataTO[] hijosBuscados = fachada.getIndicesNodosHijos(this.nodoActual.Id);
                    //ObservableCollection<IndicesTreeView> resultado = new ObservableCollection<IndicesTreeView>();
                    fachada.Close();
                    ObservableCollection<TreeViewItem> nodosFinales = new ObservableCollection<TreeViewItem>();
                    foreach (TreeNodeDataTO item in hijosBuscados)
                    {
                        TreeViewItem itemVerdadero = new TreeViewItem();
                        itemVerdadero.Header = item.Label;
                        itemVerdadero.ToolTip = item.Label;
                        itemVerdadero.Tag = item;
                        itemVerdadero.ItemsSource = hijosNuevos(item);
                        nodosFinales.Add(itemVerdadero);
                    }
                    return nodosFinales;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception excep)
            {
                MessageBox.Show("Hubo problemas para iniciar el arbol o traer los datos necesarios: "+ excep.Message);
                return null;
            }
        }
    }
}
