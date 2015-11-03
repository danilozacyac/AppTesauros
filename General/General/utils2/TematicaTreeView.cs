using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mx.gob.scjn.ius_common.TO;
using mx.gob.scjn.ius_common.fachade;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;
using mx.gob.scjn.ius_common.gui.utils;
namespace mx.gob.scjn.ius_common.utils
{
    /// <summary>
    /// Maneja la recepción de datos y su establecimiento para poder presentarse
    /// en los árboles de la búsqueda temática.
    /// </summary>
    public class TematicaTreeView
    {
        /// <summary>
        /// El Nodo Inicial
        /// </summary>
        public TreeViewItem NodoTemasActual { get; set; }
        public Dictionary<String, TreeViewItem> Padres { get; set; }
        private TreeViewItem nodoTemasActual;
        /// <summary>
        /// Obtiene el color del que se debe pintar el subtema.
        /// </summary>
        //public Brush ColorSubTema { get {return this.getColorSubtema(); } }
        /// <summary>
        /// El arbol de temas
        /// </summary>
        public List<TreeViewItem> HijosTemas { get; set; }
        /// <summary>
        /// El nodo del subtema elegido
        /// </summary>
        public TreeViewItem NodoSubtemaActual { get; set; }
        /// <summary>
        /// Arbol del subtema Elegido
        /// </summary>
        public List<TreeViewItem> HijosSubtemas { get; set; }
        /// <summary>
        /// Nodo de los sinónimos del subtema elegido
        /// </summary>
        public TreeViewItem NodoSinonimoActual { get; set; }
        /// <summary>
        /// El arbol de los sinonimos elegidos
        /// </summary>
        public List<TreeViewItem> HijosSinonimos { get; set; }

        //public bool IsExpanded { get; set; }
        /// <summary>
        /// Constructor por omisión. Genera los árboles iniciales
        /// </summary>
        public TematicaTreeView()
        {
            try
            {
                FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
                TreeNodeDataTO[] arreglo = fachada.getRaizTematica("0");
                fachada.Close();
                List<TreeViewItem> lista = new List<TreeViewItem>();
                foreach (TreeNodeDataTO item in arreglo)
                {
                    TreeViewItem itemActual = new TreeViewItem();
                    //itemActual.NodoTemasActual = new TreeViewItem();
                    itemActual.Tag = item.Target;
                    itemActual.Header = item.Label;
                    itemActual.Uid = item.Id;
                    itemActual.ItemsSource = ObtenHijos(item.Id);
                    lista.Add(itemActual);
                }
                this.HijosTemas = lista;
            }
            catch (Exception e)
            {
                MessageBox.Show("Problemas con la iniciación de la consulta tematica, favor de revisar los servicios de IUS " + e.Message);
            }
        }
        /// <summary>
        /// Genera el arbol de las consultas temáticas tomando en cuenta que no es nodo inicial
        /// de los temas, es decir es para actualizar los subtemas o sinónimos
        /// </summary>
        /// <param name="tipoArbol">El tipo de árbol a actualizar</param>
        public TematicaTreeView(String tipoArbol)
        {
            this.NodoTemasActual = new TreeViewItem();
            this.NodoTemasActual.Header = tipoArbol;
        }
        /// <summary>
        /// Obtiene los hijos de los temas del parámetro "p"
        /// </summary>
        /// <param name="p">El padre del que se quieren los hijos</param>
        /// <returns>La lista de los nodos hijos</returns>
        private List<TreeViewItem> ObtenHijos(string p)
        {
            FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
            TreeNodeDataTO[] arreglo = fachada.getRaizTematica(p);
            fachada.Close();
            List<TreeViewItem> lista = new List<TreeViewItem>();
            foreach (TreeNodeDataTO item in arreglo)
            {
                TreeViewItem itemActual = new TreeViewItem();
                itemActual.Header=item.Label;
                itemActual.Uid = item.Id;
                itemActual.Tag = item.Target;
                itemActual.ItemsSource = ObtenHijos(item.Id);
                lista.Add(itemActual);
            }
            return lista;
        }
        /// <summary>
        /// Actualiza los subtemas dentro del conjunto de árboles que se despliegan.
        /// </summary>
        /// <param name="tabla">La tabla del arbol.</param>
        public void ActualizaSubtemas(String tabla)
        {
            FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
            TreeNodeDataTO[] arreglo = fachada.getSubtemas(tabla);
            fachada.Close();
            this.NodoSubtemaActual = new TreeViewItem();
            this.NodoSubtemaActual.Tag = "0";
            this.NodoSubtemaActual.Header = "";
            //List<String> idPadres = new List<string>();
            //idPadres.Add("0");
            Padres = new Dictionary<string, TreeViewItem>(); ;
            List<TreeViewItem> listaFinal = new List<TreeViewItem>();
            //padres.Add(this);
            List<TreeViewItem> listaPadre = null;
            //listaPadre = new List<TreeViewItem>();
            Padres.Add("0",this.NodoSubtemaActual);
            foreach (TreeNodeDataTO item in arreglo)
            {
                TreeViewItem itemActual = new TreeViewItem();
                itemActual = new TreeViewItem();
                itemActual.Tag = item;
                itemActual.Uid = item.Id;
                itemActual.Header = item.Label;
                int valorTarget = Int32.Parse(item.Target);
                Brush resultado = null;
                if (valorTarget > 0)
                {
                    resultado = Brushes.Blue;
                }
                else if (item.Padre.Equals("0"))
                {
                    resultado = Brushes.Black;
                }
                else
                {
                    resultado = Brushes.Red;
                }
                itemActual.Foreground = resultado;
                //idPadres.Add(item.Id);
                Padres.Add(item.Id,itemActual);
                if (item.Padre.Equals("0"))
                {
                    listaFinal.Add(itemActual);
                }
                //int lugarPadre = idPadres.FindIndex(item.Padre.Equals);
                TreeViewItem itemPadre = Padres[item.Padre];
                if (itemPadre.ItemsSource == null)
                {
                    listaPadre = new List<TreeViewItem>();
                }
                else
                {
                    listaPadre = (List<TreeViewItem>)itemPadre.ItemsSource;
                }
                listaPadre.Add(itemActual);
                //if (itemPadre != padres.ElementAt(0))
                //{
                //    itemActual. = itemPadre;
                //}
                itemPadre.ItemsSource = listaPadre;
            }
            //foreach (TreeViewItem item in listaFinal)
            //{
            //    if ((item.ItemsSource != null) && (((List<TreeViewItem>)item.ItemsSource).Count > 0))
            //    {
            //        item.IsTextSearchEnabled = true;
            //        item.SetValue(TextSearch.TextPathProperty, "Header");
            //    }
            //}
            HijosSubtemas = listaFinal;
        }

        /// <summary>
        /// Actualiza los subtemas dentro del conjunto de árboles que se despliegan.
        /// </summary>
        /// <param name="tabla">La tabla del arbol.</param>
        public void ActualizaSubtemas(String tabla, String busqueda)
        {
            FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
            TreeNodeDataTO[] arreglo = fachada.getSubtemasPalabra(tabla, busqueda);
            fachada.Close();
            List<TreeViewItem> listaFinal = new List<TreeViewItem>();
            this.NodoSubtemaActual = new TreeViewItem();
            this.NodoSubtemaActual.Tag = "0";
            this.NodoSubtemaActual.Header = "";
            List<String> idPadres = new List<string>();
            idPadres.Add("0");
            List<TreeViewItem> padres = new List<TreeViewItem>();
            padres.Add(this.NodoSubtemaActual);
            foreach (TreeNodeDataTO item in arreglo)
            {
                TreeViewItem itemActual = new TreeViewItem();
                itemActual = new TreeViewItem();
                itemActual.Header = item.Label;
                itemActual.Tag = item;
                itemActual.Uid = item.Id;
                int valorTarget = Int32.Parse(item.Target);
                Brush resultado = null;
                if (valorTarget > 0)
                {
                    resultado = Brushes.Blue;
                }
                else if (item.Padre.Equals("0"))
                {
                    resultado = Brushes.Black;
                }
                else
                {
                    resultado = Brushes.Red;
                }
                itemActual.Foreground = resultado;
                idPadres.Add(item.Id);
                padres.Add(itemActual);
                itemActual.IsExpanded = true;
                int lugarPadre = idPadres.FindIndex(item.Padre.Equals);
                if (lugarPadre == -1)
                {
                    lugarPadre = 0;
                }
                TreeViewItem itemPadre = padres.ElementAt(lugarPadre);
                List<TreeViewItem> listaHijos = null;
                if (item.Padre.Equals("0"))
                {
                    listaFinal.Add(itemActual);
                }
                else
                {
                    if (!idPadres.Contains(item.Padre))
                    {
                        fachada = new FachadaBusquedaTradicionalClient();
                        TreeNodeDataTO padre = fachada.getSubtema(tabla, item.Padre);
                        fachada.Close();
                        TreeViewItem itemPadreNuevo = new TreeViewItem();
                        itemPadreNuevo.Header = padre.Label;
                        itemPadreNuevo.Tag = padre;
                        itemPadreNuevo.Uid = padre.Id;
                        int valorTargetNuevo = Int32.Parse(padre.Target);
                        Brush resultadoNuevo = null;
                        if (valorTargetNuevo > 0)
                        {
                            resultadoNuevo = Brushes.Blue;
                        }
                        else if (padre.Padre.Equals("0"))
                        {
                            resultadoNuevo = Brushes.Black;
                        }
                        else
                        {
                            resultadoNuevo = Brushes.Red;
                        }
                        itemPadreNuevo.Foreground = resultadoNuevo;
                        listaFinal.Add(itemPadreNuevo);
                        padres.Add(itemPadreNuevo);
                        itemPadre = itemPadreNuevo;
                        itemPadre.IsExpanded = true;
                        idPadres.Add(padre.Id);
                    }
                    else
                    {
                        itemPadre = padres.ElementAt(idPadres.IndexOf(item.Padre));
                        itemPadre.IsExpanded = true;
                    }
                }
                if (itemPadre.ItemsSource == null)
                {
                    listaHijos = new List<TreeViewItem>();
                }
                else
                {
                    listaHijos = (List<TreeViewItem>)itemPadre.ItemsSource;
                }
                listaHijos.Add(itemActual);
                itemPadre.ItemsSource = listaHijos;
                String[] palabras = FlowDocumentHighlight.Normaliza(busqueda).Split();
                bool pintaNodo = true;
                foreach (String itemPalabras in palabras)
                {
                    pintaNodo = pintaNodo && (FlowDocumentHighlight.Normaliza(item.Label).Trim().Contains(FlowDocumentHighlight.Normaliza(itemPalabras)));
                }
                if (pintaNodo)
                {
                    itemActual.Foreground = Brushes.DarkGreen;
                    itemActual.FontWeight = FontWeights.Bold;
                }
            }
            if (listaFinal.Count > 0)
            {
                HijosSubtemas = listaFinal;
            }
            else
            {
                HijosSubtemas = listaFinal;
            }
        }
        
        /// <summary>
        /// Pone el color al subtema a presentar
        /// </summary>
        /// <returns>El color que le corresponde al subtema</returns>
      /*  private Brush getColorSubtema()
        {
            Brush resultado = null;
            int valorTarget = Int32.Parse((String)NodoSubtemaActual.Tag);
            if (valorTarget>0)
            {
                resultado = Brushes.Blue;
            }
            else if (NodoSubtemaActual.Padre.Equals("0"))
            {
                resultado = Brushes.Black;
            }
            else
            {
                resultado = Brushes.Red;
            }
            return resultado;
        }*/
        /// <summary>
        /// Actualiza el árbol de sinónimos
        /// </summary>
        /// <param name="tabla">La tabla de la que se obtendrán lso sinónimos</param>
        /// <param name="Id">El identificador del subtema</param>
        public void ActualizaSinonimos(string tabla, String Id)
        {
            TreeNodeDataTO[] arreglo = null;
            try
            {
                FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
                arreglo = fachada.getSinonimos(tabla + "_alternos", Id);
                fachada.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show("Ocurrio una excepción al momento de cargar los sinónimos: " + e.Message,
                    "Error al cargar sinónimos", MessageBoxButton.OK, MessageBoxImage.Error);
                arreglo = new TreeNodeDataTO[0];
            }
            this.NodoSinonimoActual = new TreeViewItem();
            this.NodoSinonimoActual.Uid = "0";
            this.NodoSinonimoActual.Header = "";
            this.HijosSinonimos = new List<TreeViewItem>();
            foreach (TreeNodeDataTO item in arreglo)
            {
                TreeViewItem itemActual = new TreeViewItem();
                itemActual = new TreeViewItem();
                itemActual.Header = item.Label;
                itemActual.Tag = item;
                itemActual.Uid = item.Id;
                HijosSinonimos.Add(itemActual);
            }
        }
    }
}