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
using mx.gob.scjn.ius_common.TO;
using mx.gob.scjn.ius_common.gui.utils;
using System.Collections.ObjectModel;
using System.Configuration;
using mx.gob.scjn.ius_common.fachade;

namespace mx.gob.scjn.ius_common.gui.apoyos
{
    /// <summary>
    /// Interaction logic for ListadoSecciones.xaml
    /// </summary>
    public partial class ListadoSecciones : UserControl
    {
        public Page NavigationService { get; set; }
        /// <summary>
        /// La lista de identificadores tribunales seleccionados
        /// </summary>
        public int[][] Seleccionados
        {
            get
            {
                return ObtenSeleccionados();
            }
        }

        public int TomoActual
        {
            set
            {
                if (Arboles.ContainsKey(value))
                {
                    TrvSecciones.ItemsSource = null;
                    TrvSecciones.Items.Clear();
                    TrvSecciones.ItemsSource = Arboles[value];
                    TrvSecciones.Items.Refresh();

                }
            }
        }

        private Point inicioDrag;
        private Point ofsetDrag;
        private static Dictionary<Int32, ObservableCollection<TreeViewItem>> Arboles { get; set; }
        public ListadoSecciones()
        {
            InitializeComponent();
            if (Arboles == null)
            {
                Arboles = GeneraArboles();
            }
            else
            {
                foreach (KeyValuePair<int, ObservableCollection<TreeViewItem>> item in Arboles)
                {
                    ObservableCollection<TreeViewItem> lista = item.Value;
                    ((CheckBox)lista[0].Header).IsChecked = false;
                    ((CheckBox)lista[0].Header).IsChecked = true;
                }
            }

        }

        private Dictionary<int, ObservableCollection<TreeViewItem>> GeneraArboles()
        {
#if STAND_ALONE
            FachadaBusquedaTradicional fachada = new FachadaBusquedaTradicional();
            List<TomoTO> Tomos = fachada.getTomosPrimerNivel();
#else
            FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
            List<TomoTO> Tomos = fachada.getTomosPrimerNivel().ToList();
#endif
            Dictionary<Int32, ObservableCollection<TreeViewItem>> Resultado = new Dictionary<int, ObservableCollection<TreeViewItem>>();
            Dictionary<int, ObservableCollection<TreeViewItem>> seccionesTomo = new Dictionary<int, ObservableCollection<TreeViewItem>>();
            Dictionary<int, TreeViewItem> desplegadoDeArbol = new Dictionary<int, TreeViewItem>();
            foreach (TomoTO item in Tomos)
            {

#if STAND_ALONE
                List<SeccionTO> AConvertir = fachada.getTomosSecciones(item.Id);
#else
                List<SeccionTO> AConvertir = fachada.getTomosSecciones(item.Id).ToList();
#endif
                TreeViewItem trvTomo = new TreeViewItem();
                CheckBox cbx = new CheckBox();
                cbx.Content = item.Descripcion;
                trvTomo.Header = cbx;
                trvTomo.IsExpanded = true;
                trvTomo.ItemsSource = new ObservableCollection<TreeViewItem>();
                if (!seccionesTomo.ContainsKey(item.Id))
                {
                    seccionesTomo.Add(item.Id, new ObservableCollection<TreeViewItem>());
                }
                seccionesTomo[item.Id].Add(trvTomo);
                desplegadoDeArbol.Add(item.Id, trvTomo);
                foreach (SeccionTO itemSec in AConvertir)
                {
                    TreeViewItem itemRes = new TreeViewItem();
                    itemRes.IsExpanded = true;
                    itemRes.ItemsSource = new ObservableCollection<TreeViewItem>();
                    CheckBox cbxItem = new CheckBox();
                    itemRes.Header = cbxItem;
                    cbxItem.Content = itemSec.Descripcion;
                    itemRes.Tag = itemSec;
                    if (!seccionesTomo.ContainsKey(itemSec.Padre))
                    {
                        seccionesTomo.Add(itemSec.Padre, new ObservableCollection<TreeViewItem>());
                    }
                    if (itemSec.Padre == 0) itemSec.Padre = itemSec.Tomo;
                    //seccionesTomo[itemSec.Padre].Add(itemRes);
                    desplegadoDeArbol.Add(itemSec.Id, itemRes);
                    if (desplegadoDeArbol.ContainsKey(itemSec.Padre))
                    {
                        ((ObservableCollection<TreeViewItem>)desplegadoDeArbol[itemSec.Padre].ItemsSource).Add(itemRes);

                    }
                }
                TrvSecciones.ItemsSource = null;
                TrvSecciones.Items.Clear();
                TrvSecciones.ItemsSource = seccionesTomo[item.Id];
                TrvSecciones.Items.Refresh();
                GeneraEventos(TrvSecciones.Items);
                ((CheckBox)trvTomo.Header).IsChecked = true;
            }
            foreach (TomoTO item in Tomos)
            {
                Resultado[item.Id] = seccionesTomo[item.Id];
            }
            return Resultado;
        }

        private void GeneraEventos(ItemCollection itemCollection)
        {
            foreach (TreeViewItem item in itemCollection)
            {
                CheckBox itemCbx = (CheckBox)item.Header;
                itemCbx.Checked += new RoutedEventHandler(itemCbx_Checked);
                itemCbx.Unchecked += new RoutedEventHandler(itemCbx_Checked);
                GeneraEventos(item.Items);
            }
        }

        private object RevisandoHijos { get; set; }

        void itemCbx_Checked(object sender, RoutedEventArgs e)
        {
            //object algo = ConfigurationManager.AppSettings["Prueba"];
            CheckBox valor = ((CheckBox)sender);
            TreeViewItem nodo = (TreeViewItem)valor.Parent;
            if (RevisandoHijos == null) RevisandoHijos = sender;
            if ((valor.IsChecked != null))
            {
                foreach (TreeViewItem item in nodo.Items)
                {
                    CheckBox itemValor = (CheckBox)item.Header;
                    itemValor.IsChecked = valor.IsChecked;
                }
            }
            if (sender.Equals(RevisandoHijos))
            {
                ItemsControl nodoPadre = ItemsControl.ItemsControlFromItemContainer(nodo);
                if ((nodoPadre != null)
                    && nodoPadre.GetType().Equals(typeof(TreeViewItem)))
                {
                    //TreeViewItem nodoPadre = (TreeViewItem)VisualTreeHelper.GetParent(nodo);
                    ActualizaValorPadre((TreeViewItem)nodoPadre);
                }
                RevisandoHijos = null;
            }
        }

        private void ActualizaValorPadre(TreeViewItem nodoPadre)
        {
            int cuantos = 0;
            int cuantosNulos = 0;
            foreach (TreeViewItem item in nodoPadre.Items)
            {
                CheckBox contenido = (CheckBox)item.Header;
                if ((contenido.IsChecked == null))
                {
                    cuantosNulos++;
                }
                if ((contenido.IsChecked != null) && (bool)contenido.IsChecked)
                {
                    cuantos++;
                }
            }
            CheckBox cbx = (CheckBox)nodoPadre.Header;
            if ((cuantos == 0) && (cuantosNulos == 0))
            {
                cbx.IsChecked = false;
            }
            else if (cuantos < nodoPadre.Items.Count)
            {
                cbx.IsChecked = null;
            }
            else
            {
                cbx.IsChecked = true;
            }
            ItemsControl nodoAbuelo = ItemsControl.ItemsControlFromItemContainer(nodoPadre);
            if ((nodoAbuelo != null) && nodoAbuelo.GetType().Equals(typeof(TreeViewItem)))
            {
                ActualizaValorPadre((TreeViewItem)nodoAbuelo);
            }
        }

        private void Salir_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Hidden;
        }

        private void BarraMovimiento_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ofsetDrag = e.GetPosition(this);
            if ((inicioDrag.X == -1) && (inicioDrag.Y == -1))
            {
                inicioDrag = e.GetPosition(Parent as Canvas);
                this.BarraMovimiento.CaptureMouse();
            }
        }

        private void BarraMovimiento_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            inicioDrag.X = -1;
            inicioDrag.Y = -1;
            BarraMovimiento.ReleaseMouseCapture();
        }

        private void BarraMovimiento_MouseMove(object sender, MouseEventArgs e)
        {

            if ((e.LeftButton == MouseButtonState.Pressed) && (BarraMovimiento.IsMouseCaptured))
            {
                Point puntoActual = e.GetPosition(Parent as Canvas);
                puntoActual.X -= ofsetDrag.X;
                puntoActual.Y -= ofsetDrag.Y;
                Canvas.SetTop(this, puntoActual.Y);
                Canvas.SetLeft(this, puntoActual.X);
            }
            else
            {
                inicioDrag.X = -1;
                inicioDrag.Y = -1;
            }
        }

        private int[][] ObtenSeleccionados()
        {
            List<int[]> Resultado = new List<int[]>();
            int contador = 0;
            foreach (KeyValuePair<int, ObservableCollection<TreeViewItem>> item in Arboles)
            {
                Resultado.Add(new int[2]);
                List<int> seleccionadosTomo = new List<int>();
                switch (item.Key)
                {
                    case 100:
                        seleccionadosTomo.Add(140);
                        break;
                    case 200:
                        seleccionadosTomo.Add(145);
                        break;
                    case 400:
                        seleccionadosTomo.Add(141);
                        break;
                    case 500:
                        seleccionadosTomo.Add(142);
                        break;
                    case 600:
                        seleccionadosTomo.Add(143);
                        break;
                    case 700:
                        seleccionadosTomo.Add(144);
                        break;
                    case 800:
                        seleccionadosTomo.Add(146);
                        break;
                    case 900:
                        seleccionadosTomo.Add(147);
                        break;
                }
                CheckBox Todos = ((CheckBox)((TreeViewItem)item.Value[0]).Header);
                if (Todos.IsChecked == true)
                {
                    seleccionadosTomo.Add(-1);
                }
                else
                {
                    //////////Recorrer todos los hijos de último nivel y añadir al resultado
                    TreeViewItem Raiz = (TreeViewItem)TrvSecciones.Items[0];
                    foreach (TreeViewItem itemRaiz in Raiz.Items)
                    {
                        ObtenSecciones(itemRaiz, seleccionadosTomo);
                    }
                }
                Resultado[contador] = seleccionadosTomo.ToArray();
                contador++;
            }
            return Resultado.ToArray();
        }

        private void ObtenSecciones(TreeViewItem itemRaiz, List<int> seleccionadosTomo)
        {
            CheckBox seleccionado = (CheckBox)itemRaiz.Header;
            if (seleccionado.IsChecked==true)
            {
                SeccionTO datos = (SeccionTO)itemRaiz.Tag;
                seleccionadosTomo.Add(datos.Id);
            }
            foreach (TreeViewItem itemTribunal in itemRaiz.Items)
            {
                SeccionTO datos = (SeccionTO)itemTribunal.Tag;
                seleccionado = (CheckBox)itemTribunal.Header;
                if (seleccionado.IsChecked == true)
                    seleccionadosTomo.Add(datos.Id);
                if (itemTribunal.Items.Count > 0)
                {
                    ObtenSecciones(itemTribunal, seleccionadosTomo);
                }
            }
        }
    }
}
