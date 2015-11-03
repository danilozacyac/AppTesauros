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
using IUSTOWeb.TO.Comparador;
namespace mx.gob.scjn.ius_common.utils
{
    public class ThesauroTreeView
    {
        /// <summary>
        ///     El texto que se tiene en la búsqueda realizada
        /// </summary>
        public String TextoBusquedaPintar { get; set; }
        /// <summary>
        /// El Nodo Inicial
        /// </summary>
        public TreeNodeDataTO NodoTemasActual { get; set; }
        private TreeNodeDataTO nodoTemasActual;
        ///<summary>
        ///El conjunto de proximidades probables de un
        ///subtema
        ///</summary>
        public List<TreeViewItem> HijosProximidad { get; set; }
        /// <summary>
        /// La proximidad Actual.
        /// </summary>
        public TreeNodeDataTO NodoProximidadActual { get; set; }
        /// <summary>
        /// Obtiene el color del que se debe pintar el subtema.
        /// </summary>
        public Brush ColorSubTema { get {return this.getColorSubtema(); } }
        /// <summary>
        /// El arbol de temas
        /// </summary>
        public List<TreeViewItem> HijosTemas { get; set; }

        /// <summary>
        /// El nodo del subtema elegido
        /// </summary>
        public TreeNodeDataTO NodoSubtemaActual { get; set; }
        /// <summary>
        /// Arbol del subtema Elegido
        /// </summary>
        public List<TreeViewItem> HijosSubtemas { get; set; }
        /// <summary>
        /// Nodo de los sinónimos del subtema elegido
        /// </summary>
        public TreeNodeDataTO NodoSinonimoActual { get; set; }
        //El arbol de los sinonimos elegidos
        public List<TreeViewItem> HijosSinonimos { get; set; }
        /// <summary>
        ///     Mientras exista algo en esta
        ///     variable las búsquedas de subtemas
        ///     se debe realizar usándola.
        /// </summary>
        /// <value>
        ///     <para>
        ///         La búsqueda a realizarse.
        ///     </para>
        /// </value>
        /// <remarks>
        ///     
        /// </remarks>
        public String Busqueda { get; set; }
        /// <summary>
        /// Constructor por omisión. Genera los árboles iniciales
        /// </summary>
        public ThesauroTreeView()
        {
            try
            {
                FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
                TreeNodeDataTO[] arreglo = fachada.getRaizTematica("T0");
                fachada.Close();
                List<TreeViewItem> lista = new List<TreeViewItem>();
                foreach (TreeNodeDataTO item in arreglo)
                {
                    TreeViewItem itemVerdadero = new TreeViewItem();
                    itemVerdadero.Header = item.Label;
                    itemVerdadero.Tag = item;
                    lista.Add(itemVerdadero);
                }
                this.HijosTemas = lista;

            }
            catch (Exception e)
            {
                MessageBox.Show("Problemas con la iniciación de la consulta tematica, favor de revisar los servicios de IUS " + e.Message);
            }
        }
        private List<TreeViewItem> ObtenHijos(string id, string label)
        {
            FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
            this.nodoTemasActual = new TreeNodeDataTO();
            this.nodoTemasActual.Id = id;
            this.nodoTemasActual.Label = label;
            TreeNodeDataTO[] arreglo = fachada.getRaizTematica("T"+id);
            fachada.Close();
            List<TreeViewItem> lista = new List<TreeViewItem>();
            foreach (TreeNodeDataTO item in arreglo)
            {
                TreeViewItem itemActual = new TreeViewItem();//item.Id, item.Label);
                itemActual.Header = item.Label;
                itemActual.Tag = item;
                lista.Add(itemActual);
            }
            return lista;
        }
        public ThesauroTreeView(String tipoArbol)
        {
            this.NodoTemasActual = new TreeNodeDataTO();
            this.NodoTemasActual.Label = tipoArbol;
        }
        private List<TreeViewItem> ObtenHijos(string p)
        {
            FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
            TreeNodeDataTO[] arreglo = fachada.getRaizTematica(p);
            fachada.Close();
            List<TreeViewItem> lista = new List<TreeViewItem>();
            foreach (TreeNodeDataTO item in arreglo)
            {
                TreeViewItem itemActual = new TreeViewItem(); 
                //itemActual.ItemsSource=ObtenHijos(item.Id, item.Label);
                itemActual.Tag = item;
                itemActual.ItemsSource = ObtenHijos(item.Id);
                lista.Add(itemActual);
            }
            return lista;
        }
        public List<TreeViewItem> ActualizaSubtemas(String ID)
        {
            FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
            TreeNodeDataTO[] arreglo = fachada.getRaizTematica("T"+ID);
            fachada.Close();
            List<String> idPadres = new List<string>();
            idPadres.Add(ID);
            TreeViewItem itemInicial = new TreeViewItem();
            itemInicial.Header = "Ficticio";
            generaSubtemas(arreglo, itemInicial, ID);
            this.HijosSubtemas = (List<TreeViewItem>)itemInicial.ItemsSource;
            return (List<TreeViewItem>)itemInicial.ItemsSource;
        }

        protected void generaSubtemas(TreeNodeDataTO[] arreglo,TreeViewItem itemInicial, String ID)
        {
            List<String> idPadres = new List<string>();
            idPadres.Add(ID);
            List<TreeViewItem> padres = new List<TreeViewItem>();
            padres.Add(itemInicial);
            foreach (TreeNodeDataTO item in arreglo)
            {
                if (!item.Padre.Equals("0"))
                {
                    TreeViewItem itemActual = new TreeViewItem();
                    itemActual.Tag = item;
                    itemActual.Header = item.Label;
                    if ((Busqueda != null) && !Busqueda.Equals(""))
                    {
                        String[] palabras = TextoBusquedaPintar.Split();
                        bool pintaNodo = true;
                        foreach (String itemPalabras in palabras)
                        {
                            pintaNodo = pintaNodo && (FlowDocumentHighlight.Normaliza(item.Label).Trim().Contains(FlowDocumentHighlight.Normaliza(itemPalabras)));
                        }
                        if (pintaNodo)
                        {
                            itemActual.Foreground = Brushes.Green;
                        }

                    }
                    idPadres.Add(item.Id);
                    padres.Add(itemActual);
                    int lugarPadre = idPadres.FindIndex(item.Padre.Equals);

                    TreeViewItem itemPadre =  padres.ElementAt(lugarPadre);
                    if (itemPadre.ItemsSource == null)
                    {
                        itemPadre.ItemsSource = new List<TreeViewItem>();
                    }
                    List<TreeViewItem> listaPadre = (List<TreeViewItem>)itemPadre.ItemsSource;
                    itemActual.ItemsSource = ActualizaSubtemas(item.Id);
                    if (TextoBusquedaPintar != null && TextoBusquedaPintar.Contains(item.Label))
                    {
                        itemActual.Foreground = Brushes.Green;
                    }
                    listaPadre.Add(itemActual);
                    itemPadre.ItemsSource = listaPadre;
                }
            }
        }
        private Brush getColorSubtema()
        {
            Brush resultado = null;
            String valorTipo = NodoSubtemaActual.Label.Substring(0,3);
            if (valorTipo.Equals("AS "))
            {
                resultado = Brushes.Red;
            }
            else if (valorTipo.Substring(0,3).Equals("CE "))
            {
                resultado = Brushes.Blue;
            }
            else
            {
                resultado = Brushes.Black;
            }
            return resultado;
        }


        public void ActualizaSinonimos(String Id)
        {
            FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
            TreeNodeDataTO[] arreglo = fachada.getSinonimoConstitucional("T"+Id);
            fachada.Close();
            this.HijosSinonimos = new List<TreeViewItem>();
            foreach (TreeNodeDataTO item in arreglo)
            {
                TreeViewItem itemActual = new TreeViewItem();
                itemActual.Tag = item;
                itemActual.Header = item.Label;
                if (TextoBusquedaPintar != null)
                {
                    String[] palabras = TextoBusquedaPintar.Split();
                    bool pintaNodo = true;
                    foreach (String itemPalabras in palabras)
                    {
                        pintaNodo = pintaNodo && (FlowDocumentHighlight.Normaliza(item.Label).Trim().Contains(FlowDocumentHighlight.Normaliza(itemPalabras)));
                        
                    }
                    if (pintaNodo)
                    {
                        itemActual.Foreground = Brushes.Green;
                    }
                }
                HijosSinonimos.Add(itemActual);
            }
        }
        public void ActualizaProximidad(String Id)
        {
            FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
            TreeNodeDataTO[] arreglo = fachada.getProximidadConstitucional("T"+Id);
            fachada.Close();
            //this.NodoSinonimoActual = new TreeNodeDataTO();
            //this.NodoSinonimoActual.Id = "0";
            //this.NodoSinonimoActual.Label = "Sinónimos";
            this.HijosProximidad = new List<TreeViewItem>();
            foreach (TreeNodeDataTO item in arreglo)
            {
                TreeViewItem itemActual = new TreeViewItem();
                itemActual.Tag = item;
                itemActual.Header = "RP "+item.Label;
                if ((TextoBusquedaPintar != null) && (!TextoBusquedaPintar.Equals("")))
                {
                    String[] palabras = TextoBusquedaPintar.Split();
                    bool pintaNodo = true;
                    foreach (String itemPalabras in palabras)
                    {
                        pintaNodo = pintaNodo && (FlowDocumentHighlight.Normaliza(item.Label).Trim().Contains(FlowDocumentHighlight.Normaliza(itemPalabras)));
                        
                    }
                    if (pintaNodo)
                    {
                        itemActual.Foreground = Brushes.Green;
                    }
                }
                HijosProximidad.Add(itemActual);
            }
        }

        public void IncorporaCentral(string id)
        {
            FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
            TreeNodeDataTO nodoNuevo = fachada.getIdConstitucional(id);
            nodoNuevo.Label = "CE " + nodoNuevo.Label;
            fachada.Close();
            List<TreeViewItem> nuevaLista = new List<TreeViewItem>();
            TreeViewItem nodoVerdadero = new TreeViewItem();
            nodoVerdadero.Header = nodoNuevo.Label;
            nodoVerdadero.Tag = nodoNuevo;
            //nodoVerdadero.NodoSubtemaActual = nodoNuevo;
            nodoVerdadero.Foreground = Brushes.Blue;
            nuevaLista.Add(nodoVerdadero);
            if ((TextoBusquedaPintar != null) && (!TextoBusquedaPintar.Equals("")))
            {
                String[] palabras = TextoBusquedaPintar.Split();
                bool pintaNodo = true;
                foreach (String itemPalabras in palabras)
                {
                    pintaNodo = pintaNodo && (FlowDocumentHighlight.Normaliza((String)nodoVerdadero.Header).Trim().Contains(FlowDocumentHighlight.Normaliza(itemPalabras)));
                    
                }
                if (pintaNodo)
                {
                    nodoVerdadero.Foreground = Brushes.Green;
                }
            }
            nuevaLista.Add(nodoVerdadero);
            foreach (TreeViewItem item in HijosSubtemas)
            {
                nuevaLista.Add(item);
            }
            HijosSubtemas = nuevaLista;
        }

        public void IncorporaAscendente(string id)
        {
            FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
            TreeNodeDataTO nodoNuevo = fachada.getAscendenteConstitucional(id);
            nodoNuevo.Label = "IA " + nodoNuevo.Label;
            fachada.Close();
            List<TreeViewItem> nuevaLista = new List<TreeViewItem>();
            TreeViewItem nodoVerdadero = new TreeViewItem();
            nodoVerdadero.Header = nodoNuevo.Label;
            nodoVerdadero.Tag = nodoNuevo;
            nodoVerdadero.Foreground = Brushes.Red;
            if ((TextoBusquedaPintar != null) && (!TextoBusquedaPintar.Equals("")))
            {
                String[] palabras = TextoBusquedaPintar.Split();
                foreach (String itemPalabras in palabras)
                {
                    if (FlowDocumentHighlight.Normaliza((String)nodoVerdadero.Header).Trim().Contains(FlowDocumentHighlight.Normaliza(itemPalabras)))
                    {
                        nodoVerdadero.Foreground = Brushes.Green;
                    }
                }
            }
            nuevaLista.Add(nodoVerdadero);

            foreach (TreeViewItem item in HijosSubtemas)
            {
                nuevaLista.Add(item);
            }
            HijosSubtemas = nuevaLista;
        }

        public int ActualizaTodo(string Busqueda)
        {
            TextoBusquedaPintar = FlowDocumentHighlight.Normaliza(Busqueda);
            int Resultado = 0;
            FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
            TreeNodeDataTO[] temas = fachada.getRaizTematicaConstitucional("T0", FlowDocumentHighlight.Normaliza(Busqueda));
            if (temas.Length > 0)
            {
                temas = ObtenTemas(temas);
                this.Busqueda = Busqueda;
                Resultado = temas.Length;
            }
            else
            {
                return -1;
            }
            fachada.Close();
            List<TreeNodeDataTO> listaOrdenada = temas.ToList();
            TreeNodeDataTOComp comparador = new TreeNodeDataTOComp();
            listaOrdenada.Sort(comparador);
            temas = listaOrdenada.ToArray();
            List<TreeViewItem> temasLista = new List<TreeViewItem>();
            foreach (TreeNodeDataTO item  in temas)
            {
                TreeViewItem itemVerdadero = new TreeViewItem();
                itemVerdadero.Header = item.Label;                
                itemVerdadero.Tag = item;
                temasLista.Add(itemVerdadero);
            }
            temasLista.ElementAt(0).IsSelected = true;
            this.HijosTemas = temasLista;
            return Resultado;
        }

        private TreeNodeDataTO[] ObtenTemas(TreeNodeDataTO[] temas)
        {
            List<TreeNodeDataTO> resultado = new List<TreeNodeDataTO>();
            List<String> ids = new List<string>();
            foreach (TreeNodeDataTO item in temas)
            {
                if (item.Padre.Equals("0") || item.Padre.Equals("T0"))
                {
                    if (!ids.Contains(item.Id))
                    {
                        resultado.Add(item);
                        ids.Add(item.Id);
                    }
                }
            }
            return resultado.ToArray();
        }
    }
}