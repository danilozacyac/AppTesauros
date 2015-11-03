using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using mx.gob.scjn.ius_common.TO;
using mx.gob.scjn.ius_common.fachade;
using mx.gob.scjn.ius_common.utils;

namespace mx.gob.scjn.ius_common.gui.BusquedaGenerica
{
    /// <summary>
    /// Interaction logic for ResultadosBusqueda.xaml
    /// </summary>
    public partial class ResultadosBusqueda : Page
    {
        public Page Back { get; set; }
        private BusquedaTO ConsultaInicial { get; set; }
        public ResultadosBusqueda(BusquedaTO consulta)
        {
            InitializeComponent();
            ConsultaInicial = consulta;
            FachadaBusquedaTradicional fachada = new FachadaBusquedaTradicional();
            /// Esta parte es para cuando existan diferentes categorias.
            List<CategoriaDocTO> cats = fachada.GetCategorias();
            //Dictionary<int, List<DocumentoTO>> docs = new Dictionary<int,List<DocumentoTO>>();
            List<TreeViewItem> items = new List<TreeViewItem>();
            foreach (CategoriaDocTO item in cats)
            {
                //int docCount = 0;
                int docCountPar = 0;
                List<DocumentoTO> doc = fachada.GetDocumento(item.CategoriaDoc);
                List<TreeViewItem> hijos = new List<TreeViewItem>();
                foreach (DocumentoTO itemdoc in doc)
                {
                    docCountPar = ObtenDocumentos(itemdoc, consulta);
                    if (docCountPar > 0)
                    {
                        TreeViewItem docParTvi = new TreeViewItem();
                        docParTvi.Header = itemdoc.Descripcion + "[" + docCountPar + "]";
                        docParTvi.Tag = itemdoc;
                        docParTvi.Selected += new RoutedEventHandler(DocParTvi_Selected);
                        hijos.Add(docParTvi);
                    }
                }
                if (hijos.Count > 0)
                {
                    TreeViewItem categoria = new TreeViewItem();
                    if (item.imagen.Trim().Equals("X"))
                    {
                        categoria.Header = item.Descripcion;
                    }
                    else
                    {
                        TextBlock bloqueTexto = new TextBlock();
                        bloqueTexto.Text = item.Descripcion;
                        Image imagen = new Image();
                        BitmapImage bitmap = new BitmapImage();
                        bitmap.BeginInit();
                        bitmap.UriSource = new Uri(item.imagen, UriKind.Relative);
                        bitmap.EndInit();
                        imagen.Source = bitmap;
                        Grid ambos = new Grid();
                        
                        ambos.ColumnDefinitions.Add(new ColumnDefinition());
                        ambos.ColumnDefinitions.Add(new ColumnDefinition());
                        imagen.SetValue(Grid.ColumnProperty, 0);
                        ambos.Children.Add(imagen);
                        bloqueTexto.SetValue(Grid.ColumnProperty, 1);
                        ambos.Children.Add(bloqueTexto);
                        categoria.Header = ambos;
                    }
                    categoria.ItemsSource = hijos;
                    categoria.IsExpanded = true;
                    items.Add(categoria);
                }
            }
            tablaResultados.Items.Clear();
            tablaResultados.ItemsSource = items;
        }

        void DocParTvi_Selected(object sender, RoutedEventArgs e)
        {
            TreeViewItem item = (TreeViewItem)sender;
            DocumentoTO doc = (DocumentoTO)item.Tag;
            AssemblyName nombreEns= new AssemblyName(doc.EnsambladoTR);
            Assembly ensamblado = Assembly.Load(nombreEns);
            object[] args = new object[1];
            args[0] = ObtenConsulta(ConsultaInicial, doc);
            Page pagina = (Page)ensamblado.CreateInstance(doc.ClaseTablaResultado,
                false, BindingFlags.CreateInstance, null, args, null, null);
            FrmContenido.Content = pagina;
        }

        private BusquedaTO ObtenConsulta(BusquedaTO consulta, DocumentoTO itemdoc)
        {
            consulta.TipoBusqueda = itemdoc.TipoBusqueda;
            if (consulta.TipoBusqueda == Constants.BUSQUEDA_ACUERDO)
            {
                consulta.Acuerdos[Constants.ACUERDOS_ANCHO - 1][1] = false;
                consulta.Acuerdos[Constants.ACUERDOS_ANCHO - 1][0] = false;
                consulta.Acuerdos[0][0] = true;
                consulta.Acuerdos[1][0] = true;
                consulta.Acuerdos[2][0] = true;
                consulta.Acuerdos[3][0] = true;
                consulta.Acuerdos[4][0] = true;
                consulta.Acuerdos[5][0] = true;
                consulta.Acuerdos[0][1] = true;
                consulta.Acuerdos[1][1] = true;
                consulta.Acuerdos[2][1] = true;
                consulta.Acuerdos[3][1] = true;
                consulta.Acuerdos[4][1] = true;
                consulta.Acuerdos[5][1] = true;
            }
            if (consulta.TipoBusqueda == Constants.BUSQUEDA_OTROS)
            {
                consulta.Acuerdos[Constants.ACUERDOS_ANCHO - 1][1] = true;
                consulta.Acuerdos[Constants.ACUERDOS_ANCHO - 1][0] = true;
                consulta.Acuerdos[0][0] = false;
                consulta.Acuerdos[1][0] = false;
                consulta.Acuerdos[2][0] = false;
                consulta.Acuerdos[3][0] = false;
                consulta.Acuerdos[4][0] = false;
                consulta.Acuerdos[5][0] = false;
                consulta.Acuerdos[0][1] = false;
                consulta.Acuerdos[1][1] = false;
                consulta.Acuerdos[2][1] = false;
                consulta.Acuerdos[3][1] = false;
                consulta.Acuerdos[4][1] = false;
                consulta.Acuerdos[5][1] = false;
                consulta.TipoBusqueda = Constants.BUSQUEDA_ACUERDO;
            }
            return consulta;
        }

        private int ObtenDocumentos(DocumentoTO itemdoc, BusquedaTO consulta)
        {
            //Assembly ensamblado = Assembly.Load(itemdoc.Ensamblado);
            FachadaBusquedaTradicional fac = new FachadaBusquedaTradicional();
            MethodInfo met =  fac.GetType().GetMethod(itemdoc.MetodoFachada);
            object[] args = new object[1];
            args[0] = ObtenConsulta(ConsultaInicial, itemdoc);
            if ((bool)itemdoc.EsPaginado)
            {
                PaginadorTO pag = (PaginadorTO)met.Invoke(fac, args);
                int resulta = pag.Largo;
                fac.BorraPaginador(pag.Id);
                return resulta;
            }
            else
            {
                if (itemdoc.Propiedad.Equals("X"))
                {
                    ICollection lista = (ICollection)met.Invoke(fac, args);
                    return lista.Count;
                }
                else
                {
                    object resultado = met.Invoke(fac, args);
                    PropertyInfo prop = resultado.GetType().GetProperty(itemdoc.Propiedad);
                    ICollection col = (ICollection)prop.GetValue(resultado, null);
                    return col.Count;
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
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
    }
}
