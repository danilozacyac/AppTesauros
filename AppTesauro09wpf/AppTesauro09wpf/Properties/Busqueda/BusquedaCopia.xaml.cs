using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TesauroMiddleTier;
using TesauroTO;
using TesauroUtilities;

namespace AppTesauro09wpf.Busqueda
{
    /// <summary>
    /// Interaction logic for BusquedaCopia.xaml
    /// </summary>
    public partial class BusquedaCopia : Window
    {
        /// <summary>
        /// Acción que decidió tomar el usuario copiar o cancelar
        /// </summary>
        public int Resultado { get; set; }
        /// <summary>
        /// Temas que seleccionó el usuario, pueden ser 
        /// </summary>
        public List<TemaTO> TemasSeleccioandos { get { return _TemasSeleccionados; }
            set
            {
             _TemasSeleccionados = value;
             ArmaArbol();
            }
        }
        private List<TemaTO> _TemasSeleccionados { get; set; }
        /// <summary>
        /// Lista que se toma a partir de los objetos seleccionados en la lista.
        /// </summary>
        public List<TemaTO> ListaCopiar { get { return _ListaCopiar; } }
        private List<TemaTO> _ListaCopiar { get; set; }

        public BusquedaCopia()
        {
            InitializeComponent();
        }

        public BusquedaCopia(String busqueda)
        {
            InitializeComponent();
            IFachadaTesauro fac = new FachadaTesauro();
            TemasSeleccioandos = fac.GetTemas(busqueda);
            if (TemasSeleccioandos.Count > 0)
            {
                ArmaArbol();
            }
        }

        private void ArmaArbol()
        {
            List<TreeViewItem> nodos = new List<TreeViewItem>();
            foreach (TemaTO item in TemasSeleccioandos)
            {
                TreeViewItem nodo = GeneraRama(item);
                nodos.Add(nodo);
            }
            TviResultado.ItemsSource = nodos;
        }

        private TreeViewItem GeneraRama(TemaTO item)
        {
            IFachadaTesauro fac = new FachadaTesauro();
            TreeViewItem resultado = new TreeViewItem();
            if (item == null)
            {
                return null;
            }
            CheckBox combo = new CheckBox();
            combo.Content = item.Descripcion;
            combo.Tag = resultado;
            //combo.IsThreeState = true;
            combo.Click += new RoutedEventHandler(Combo_Click);
            resultado.Header = combo;
            resultado.Tag = item;
            List<TemaTO> hijos = fac.GetHijos(item.IDTema);
            List<TreeViewItem> hijosNodos = new List<TreeViewItem>();
            //foreach (TemaTO itemNuevo in hijos)
            //{
            //    TreeViewItem hijoPresente = GeneraRama(itemNuevo);
            //    if (hijoPresente != null)
            //    {
            //        hijosNodos.Add(hijoPresente);
            //    }
            //}
            resultado.ItemsSource = hijosNodos;
            return resultado;
        }

        void Combo_Click(object sender, RoutedEventArgs e)
        {
            CheckBox sen = (CheckBox)sender;
            if (sen.IsChecked == null)
            {
                return;
            }
            
            TreeViewItem tviSender = (TreeViewItem)sen.Tag;
            
            TemaTO tema = (TemaTO) tviSender.Tag;
            IFachadaTesauro fac = new FachadaTesauro();
            lblRuta.Text = fac.GetRuta(tema);
            SeleccionarHijos(tviSender, null);
            List<TreeViewItem> listaActualiza = ActualizaPadre(tviSender, sen.IsChecked, (List<TreeViewItem>)TviResultado.ItemsSource);
            bool? valor = true;
            foreach (TreeViewItem item in listaActualiza)
            {

                if (item.ItemsSource != null)
                {
                    bool? valor2  = ((CheckBox)item.Header).IsChecked ;
                    if ((valor2 != null) && (valor != null))
                    {
                        valor = (bool)valor && (bool)valor2;
                        if (item != tviSender)
                        valor = (bool)valor ? valor : null;
                        
                    }
                    else
                    {
                        valor = null;
                    }
                }
                CheckBox cbx = ((CheckBox)item.Header);
                cbx.IsChecked = valor;
            }
        }

        private void CbxTodos_Click(object sender, RoutedEventArgs e)
        {
            SeleccionaTodos((bool)CbxTodos.IsChecked);
        }
        
        private void SeleccionaTodos(bool valor)
        {
            foreach (TreeViewItem tvi in TviResultado.ItemsSource)
            {
                CheckBox item = (CheckBox)tvi.Header;
                item.IsChecked = valor;
            }
        }

        private void SeleccionarHijos(object sender, MouseEventArgs e)
        {
            TreeViewItem selected = (TreeViewItem)sender;
            bool? valor = (bool)((CheckBox)selected.Header).IsChecked;
            foreach (TreeViewItem tvi in selected.ItemsSource)
            {
                CheckBox item = (CheckBox)tvi.Header;
                item.IsChecked = valor;
                if (tvi.ItemsSource != null)
                {
                    SeleccionarHijos(tvi, null);
                }
            }
        }

        private List<TreeViewItem> ActualizaPadre(TreeViewItem tvi, bool? valor, List<TreeViewItem> items)
        {
            List<TreeViewItem> resultado = null;
            foreach(TreeViewItem itemActual in items){
                List<TreeViewItem> resultadoNuevo =ActualizaPadre(tvi, valor, (List<TreeViewItem>)itemActual.ItemsSource);
                resultado = (resultadoNuevo==null)? resultado:resultadoNuevo;
                if (itemActual != tvi)
                {
                    if (resultado != null)
                    {
                        List<TreeViewItem> temporal = new List<TreeViewItem>();
                        foreach (TreeViewItem path in resultado)
                        {
                            temporal.Add(path);
                        }
                        foreach (TreeViewItem path in temporal)
                        {
                            TemaTO tema = (TemaTO)path.Tag;
                            TemaTO temaActual = (TemaTO)itemActual.Tag;
                            if (tema.IDPadre == temaActual.IDTema)
                            {
                                resultado.Add(itemActual);
                            }
                        }
                    }
                    //return Resultado;
                }
                else
                {
                    if (resultado == null)
                    {
                        resultado = new List<TreeViewItem>();
                    }
                    resultado.Add(itemActual);
                }
            }
            return resultado;
        }

        private void BtnCopiar_Click(object sender, RoutedEventArgs e)
        {
            _ListaCopiar = null;
            List<TreeViewItem> seleccionados = ObtenListaSeleccionados((List<TreeViewItem>)TviResultado.ItemsSource);
            foreach (TreeViewItem item in seleccionados)
            {
                if (_ListaCopiar == null)
                {
                    _ListaCopiar = new List<TemaTO>();
                }
                _ListaCopiar.Add((TemaTO)item.Tag);
            }
            Resultado = Constants.COPIAR;
            this.Close();
        }

        private List<TreeViewItem> ObtenListaSeleccionados(List<TreeViewItem> listaAnalizar)
        {
            List<TreeViewItem> seleccionados = null;
            foreach (TreeViewItem item in listaAnalizar)
            {
                CheckBox cbx = (CheckBox)item.Header;
                if ((cbx.IsChecked == null) || (cbx.IsChecked == true))
                {
                    if (seleccionados == null)
                    {
                        seleccionados = new List<TreeViewItem>();
                    }
                    seleccionados.Add(item);
                    if (item.ItemsSource != null)
                    {
                        List<TreeViewItem> adicionales = ObtenListaSeleccionados((List<TreeViewItem>)item.ItemsSource);
                        if (adicionales != null)
                        {
                            foreach (TreeViewItem adicional in adicionales)
                            {
                                seleccionados.Add(adicional);
                            }
                        }
                    }
                }
            }
            return seleccionados;
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            Resultado = Constants.CANCELAR;
            this.Close();
        }

        private void BtnIgnorar_Click(object sender, RoutedEventArgs e)
        {
            Resultado = Constants.IGNORAR;
            this.Close();
        }
    }
}
