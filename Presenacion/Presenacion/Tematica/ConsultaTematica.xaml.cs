using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using IUS;
using mx.gob.scjn.ius_common.TO;
using mx.gob.scjn.ius_common.gui.Guardar;
using mx.gob.scjn.ius_common.gui.gui.impresion;
using mx.gob.scjn.ius_common.gui.gui.utilities;
using mx.gob.scjn.ius_common.gui.impresion;
using mx.gob.scjn.ius_common.gui.utils;
using mx.gob.scjn.ius_common.utils;

namespace mx.gob.scjn.ius_common.gui.Tematica
{
    /// <summary>
    /// Interaction logic for ConsultaTematica.xaml
    /// </summary>
    public partial class ConsultaTematica : Page
    {
        public Page Back { get; set; }
        private TematicaTreeView Vista { get; set; }
        private bool verGuardar;
        public bool verVease;
        public ConsultaTematica()
        {
            InitializeComponent();
            String [] nodos={"Constitucional", "PENAL"};
            Vista = new TematicaTreeView();
            Vista.ActualizaSubtemas("PEN");
            LblSubtemas.Content = "PENAL";
            this.SubtemasTree.ItemsSource = Vista.HijosSubtemas;
            foreach (TreeViewItem item in Vista.HijosTemas)
            {
                if ((item.ItemsSource != null) && (((List<TreeViewItem>)item.ItemsSource).Count > 0))
                {
                    item.IsTextSearchEnabled = true;
                    item.SetValue(TextSearch.TextPathProperty, "Header");
                    item.IsExpanded = true;
                }
            }
            this.Temas.ItemsSource = Vista.HijosTemas;
            TreeViewItem seleccionar = ((List<TreeViewItem>)this.Temas.ItemsSource).ElementAt(1);
            seleccionar.IsSelected = true;
            seleccionar = ((List<TreeViewItem>)this.Temas.ItemsSource).ElementAt(1);
            seleccionar.IsSelected = true;
            //TreeViewUtilities.SeleccionaNodo(1, this.Temas);
            //TreeViewUtilities.SeleccionaNodo(0, this.SubtemasTree);
        }


        private void Regresar_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            if (Back != null)
            {
                this.NavigationService.Navigate(Back);
            }
            else
            {
                this.NavigationService.GoBack();
            }
        }

        private void BloqueSubtema_MouseLeftButtonDown(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            TreeView inicial = this.SubtemasTree;
            if (inicial.SelectedItem != null)
            {
                this.Buscar.Text = "";
                TreeNodeDataTO objeto = (TreeNodeDataTO)((TreeViewItem)inicial.SelectedItem).Tag;
                Vista.ActualizaSinonimos(objeto.Href, objeto.Id);
                this.Sinonimos.ItemsSource = Vista.HijosSinonimos;
                this.Sinonimos.SelectedValuePath = null;
                int valorTarget = Int32.Parse(objeto.Target);
                bool veaseHabilitado = ((TreeViewItem)inicial.SelectedItem).Foreground == Brushes.Blue;
                if ((valorTarget > 0)&&(veaseHabilitado))
                {
                    BtnVease.Visibility = Visibility.Visible;
                }
                else
                {
                    BtnVease.Visibility = Visibility.Hidden;
                }
            }
        }

        private void BtnBuscar_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            Boolean noPermitidos = false;
            foreach (String item in Constants.NO_PERMITIDOS)
            {
                noPermitidos = noPermitidos || Buscar.Text.Contains(item);
            }
            if (noPermitidos || Buscar.Text.Contains("\"")||Buscar.Text.Contains("*"))
            {
                MessageBox.Show(Mensajes.MENSAJE_NO_PERMITIDOS, Mensajes.TITULO_NO_PERMITIDOS,
                    MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            String textoBuscar = FlowDocumentHighlight.Normaliza(Buscar.Text).Trim();
            if (textoBuscar.Equals(""))
            {
                MessageBox.Show(Mensajes.MENSAJE_CAMPO_TEXTO_VACIO, Mensajes.TITULO_CAMPO_TEXTO_VACIO,
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            TreeViewItem seleccionado = (TreeViewItem)Temas.SelectedItem;
            if (seleccionado.Tag.Equals("CON"))
            {
                ThesauroConstitucional nuevaVentana = new ThesauroConstitucional();
                this.NavigationService.Navigate(nuevaVentana);
                nuevaVentana.Back = this;
            }
            else
            {
                Vista.ActualizaSubtemas((String)seleccionado.Tag, textoBuscar);
                if (Vista.HijosSubtemas.Count > 0)
                {
                    this.SubtemasTree.ItemsSource = Vista.HijosSubtemas;
                }
                else
                {
                    MessageBox.Show(Mensajes.MENSAJE_NO_RESULTADO_TEMATICA, Mensajes.TITULO_NO_RESULTADO_TEMATICA,
                        MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
            }
        }


        private void Visualizar_MouseDown(object sender, RoutedEventArgs e)
        {
            TreeViewItem original = (TreeViewItem)SubtemasTree.SelectedItem;
            TreeViewItem originalsinonimo = (TreeViewItem)Sinonimos.SelectedItem;
            TreeViewItem nodo = null;
            if (original == null)
            {
                MessageBox.Show("Debe seleccionar algún subtema", "Subtema no seleccionado", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            if (originalsinonimo == null)
            {
                nodo = original;
            }
            else
            {
                nodo = originalsinonimo;
            }
            BusquedaTO busqueda = ObtenBusquedaTO(nodo);
            tablaResultado paginaResultados = new tablaResultado(busqueda);
            paginaResultados.Back = this;
            if (paginaResultados.tablaResultados.Items.Count == 0)
            {
                MessageBox.Show(Mensajes.MENSAJE_BUSQUEDA_TEMATICA_VACIA, Mensajes.TITULO_BUSQUEDA_VACIA,
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                this.NavigationService.Navigate(paginaResultados);
            }
        }

        private BusquedaTO ObtenBusquedaTO(TreeViewItem nodo)
        {
            BusquedaTO busqueda = new BusquedaTO();
            Boolean[][] epocas = new Boolean[Constants.EPOCAS_LARGO][];
            for (int i = 0; i < Constants.EPOCAS_LARGO; i++)
            {
                epocas[i] = new Boolean[Constants.EPOCAS_ANCHO];
            }
            busqueda.Epocas = epocas;
            epocas = new Boolean[Constants.ACUERDOS_ANCHO][];
            for (int i = 0; i < Constants.ACUERDOS_ANCHO; i++)
            {
                epocas[i] = new Boolean[Constants.ACUERDOS_LARGO];
            }
            busqueda.Acuerdos = epocas;
            epocas = new Boolean[Constants.APENDICES_ANCHO][];
            for (int i = 0; i < Constants.APENDICES_ANCHO; i++)
            {
                epocas[i] = new Boolean[Constants.APENDICES_LARGO];
            }
            busqueda.Apendices = epocas;
            busqueda.TipoBusqueda = Constants.BUSQUEDA_TESIS_TEMATICA;
#if STAND_ALONE
            busqueda.Clasificacion = new List<ClassificacionTO>();
            busqueda.Clasificacion.Add(new ClassificacionTO());
            busqueda.Clasificacion[0].DescTipo = ((TreeNodeDataTO)nodo.Tag).Href;//Href, originalmente
            busqueda.Clasificacion[0].IdTipo = Int32.Parse(nodo.Uid);
#else
            busqueda.clasificacion = new ClassificacionTO[1];
            busqueda.clasificacion[0] = new ClassificacionTO();
            busqueda.clasificacion[0].DescTipo = ((TreeNodeDataTO)nodo.Tag).Href;//Href, originalmente
            busqueda.clasificacion[0].IdTipo = Int32.Parse(nodo.Uid);
#endif
            busqueda.OrdenarPor = ((String)nodo.Header).Substring(0, ((String)nodo.Header).IndexOf('[')) + "[" + this.LblSubtemas.Content + "]";
            return busqueda;
        }

        private void Temas_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            TreeViewItem seleccionado = (TreeViewItem)Temas.SelectedItem;
            if (seleccionado.Tag.Equals("CON"))
            {
                ThesauroConstitucional nuevaVentana = new ThesauroConstitucional();
                this.NavigationService.Navigate(nuevaVentana);
                nuevaVentana.Back = this;
                ((List<TreeViewItem>)Temas.ItemsSource).ElementAt(1).IsSelected = true;
            }
            else
            {
                Vista.ActualizaSubtemas((String)seleccionado.Tag);
                this.SubtemasTree.ItemsSource = Vista.HijosSubtemas;
                this.SubtemasTree.IsTextSearchEnabled = true;
                this.SubtemasTree.SetValue(TextSearch.TextPathProperty, "Header");
                this.LblSubtemas.Content = seleccionado.Header;
                if ((seleccionado.Tag.Equals("IJA"))
                    || (seleccionado.Tag.Equals("SAR"))
                    || (seleccionado.Tag.Equals("ELE")))
                {
                    ImgExpandir.Visibility = Visibility.Hidden;
                    ImgRestaurar.Visibility = Visibility.Hidden;
                }
                else
                {
                    ImgExpandir.Visibility = Visibility.Visible;
                    ImgRestaurar.Visibility = Visibility.Visible;
                }
            }
            TreeViewItem nodo0 = ((List<TreeViewItem>)this.SubtemasTree.ItemsSource).ElementAt(0);
            nodo0.IsSelected = true;
            nodo0.BringIntoView();
           // TreeViewUtilities.SeleccionaNodo(0, this.SubtemasTree);
        }

        private void ImgExpandir_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            TreeViewUtilities.ExpandeTodo(SubtemasTree);
        }

        private void ImgRestaurar_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            Buscar.Text = "";
            Temas_SelectedItemChanged(null, null);
            //TreeViewUtilities.ContraeTodo(SubtemasTree);
        }

        private void BtnImprimir_Click(object sender, RoutedEventArgs e)
        {
            if (ImprimePapel.Visibility == Visibility.Hidden)
            {
                /*Obtener el item seleccionado y ver quien es el padre
                 * para ver si es de primer o segundo nivel*/
                TreeViewItem seleccionado = (TreeViewItem)this.SubtemasTree.SelectedItem;
                TreeNodeDataTO datos = (TreeNodeDataTO)seleccionado.Tag;
                TreeViewItem TemaSeleccionado = (TreeViewItem)Temas.SelectedItem;
                String[] encabezados = new String[3];
                if (Temas.Items.Contains(TemaSeleccionado))
                {
                    encabezados[2] = (String)TemaSeleccionado.Header;
                }
                else
                {
                    encabezados[1] = (String)TemaSeleccionado.Header;
                    foreach (TreeViewItem temaPadre in Temas.Items)
                    {
                        if ((temaPadre.Items != null) && (temaPadre.Items.Contains(TemaSeleccionado)))
                        {
                            encabezados[2] = (String)temaPadre.Header;
                        }
                    }
                }
                DocumentoTematica documento=null;
                if (datos.Padre.Equals("0"))
                {
                    encabezados[0] = (String)this.LblSubtemas.Content;
                    documento = new DocumentoTematica((List<TreeViewItem>)SubtemasTree.ItemsSource, encabezados);
                }
                else
                {
                    TreeViewItem nodo = Vista.Padres[((TreeNodeDataTO)((TreeViewItem)SubtemasTree.SelectedItem).Tag).Padre];
                    encabezados[0]=(String)nodo.Header;
                    documento = new DocumentoTematica((List<TreeViewItem>)nodo.ItemsSource, encabezados);
                }
                Imprimir.Document = documento.Documento;
                MuestraVistaPrel();
            }
            else
            {
                ImprimePapel.Visibility = Visibility.Hidden;
                Imprimir.Visibility = Visibility.Hidden;
                Temas.Visibility = Visibility.Visible;
                StkSinonimos.Visibility = Visibility.Visible;
                GrdSubtemas.Visibility = Visibility.Visible;
            }
        }
        /// <summary>
        ///     Muestra la vista preliminar de impresión y oculta todos los controles
        ///     que no tienen relación.
        /// </summary>
        private void MuestraVistaPrel()
        {
            verVease = (BtnVease.Visibility == Visibility.Visible);
            ImprimePapel.Visibility = Visibility.Visible;
            Imprimir.Visibility = Visibility.Visible;
            BtnTache.Visibility = Visibility.Visible;
            Temas.Visibility = Visibility.Hidden;
            StkSinonimos.Visibility = Visibility.Hidden;
            GrdSubtemas.Visibility = Visibility.Hidden;
            BtnBuscar.Visibility = Visibility.Hidden;
            Visualizar.Visibility = Visibility.Hidden;
            ImgExpandir.Visibility = Visibility.Hidden;
            ImgRestaurar.Visibility = Visibility.Hidden;
            BtnImprimir.Visibility = Visibility.Hidden;
            BtnBusquedaAlmacenada.Visibility = Visibility.Hidden;
            BtnVease.Visibility = Visibility.Hidden;
            verGuardar = GuardarExpresion.Visibility == Visibility.Visible;
            GuardarExpresion.Visibility = Visibility.Hidden;
            Regresar.Visibility = Visibility.Hidden;
            Buscar.Visibility = Visibility.Hidden;
        }
        /// <summary>
        ///     Esconde los controles relacionados con la vista preliminar y
        ///     muestra los que no tienen relación y se estaban mostrando en la pantalla.
        /// </summary>
        protected void EscondeVistaPrel()
        {
            Buscar.Visibility = Visibility.Visible;
            Regresar.Visibility = Visibility.Visible;
            ImprimePapel.Visibility = Visibility.Hidden;
            Imprimir.Visibility = Visibility.Hidden;
            BtnTache.Visibility = Visibility.Hidden;
            Temas.Visibility = Visibility.Visible;
            StkSinonimos.Visibility = Visibility.Visible;
            GrdSubtemas.Visibility = Visibility.Visible;
            BtnBuscar.Visibility = Visibility.Visible;
            Visualizar.Visibility = Visibility.Visible;
            ImgExpandir.Visibility = Visibility.Visible;
            ImgRestaurar.Visibility = Visibility.Visible;
            BtnImprimir.Visibility = Visibility.Visible;
            BtnBusquedaAlmacenada.Visibility = Visibility.Visible;
            if(verVease) BtnVease.Visibility = Visibility.Visible;
            if(verGuardar) GuardarExpresion.Visibility = Visibility.Visible;
        }

        private void ImprimePapel_Click(object sender, RoutedEventArgs e)
        {
            PrintDialog printingDialog = new PrintDialog();
            printingDialog.UserPageRangeEnabled = true;
            if ((bool)printingDialog.ShowDialog())
            {
                try
                {
                    IDocumentPaginatorSource pag = Imprimir.Document as IDocumentPaginatorSource;

                    if (printingDialog.PageRangeSelection == PageRangeSelection.UserPages)
                    {
                        ((DocumentPaginatorWrapper)pag.DocumentPaginator).PaginaInicial = printingDialog.PageRange.PageFrom;
                        ((DocumentPaginatorWrapper)pag.DocumentPaginator).PaginaFinal = printingDialog.PageRange.PageTo;
                    }

                    printingDialog.PrintDocument(pag.DocumentPaginator, "Impresión Temática");
                    EscondeVistaPrel();
                }
                catch (Exception exc)
                {
                    MessageBox.Show(Mensajes.MENSAJE_IMPRESORA, Mensajes.TITULO_ARCHIVO_ABIERTO,
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void BtnBusquedaAlmacenada_Click(object sender, RoutedEventArgs e)
        {
            UsuarioTO Usuario = new UsuarioTO();
            if (BrowserInteropHelper.IsBrowserHosted)
            {
                if ((SeguridadUsuariosTO.UsuarioActual.Usuario == null) ||
                   (SeguridadUsuariosTO.UsuarioActual.Nombre == null) ||
                   (SeguridadUsuariosTO.UsuarioActual.Nombre.Equals("")))
                {
                    LoginRegistro login = new LoginRegistro();
                    login.Back = this;
                    this.NavigationService.Navigate(login);
                    return;
                }
                else
                {
                    Usuario = SeguridadUsuariosTO.UsuarioActual;
                    //GuardarExpresion.Visibility = Visibility.Visible;
                }
            }
            else
            {
                Usuario = new UsuarioTO();
                Usuario.Usuario = Constants.USUARIO_OMISION;
                SeguridadUsuariosTO.UsuarioActual = Usuario;
            }
            GuardarExpresion.Padre = this;
            GuardarExpresion.Titulo.Content = Constants.TITULO_CONSULTA;
            TreeViewItem nodo = null;
            nodo = (TreeViewItem)Sinonimos.SelectedItem;
            if (nodo == null)
            {
                nodo = (TreeViewItem)SubtemasTree.SelectedItem;
            }
            BusquedaTO busquedaAGuardar = ObtenBusquedaTO(nodo);
            GuardarExpresion.ActualizaVentana(busquedaAGuardar, null);
            GuardarExpresion.TbxGuardar.Text = "Búsqueda Temática: "+ nodo.Header+" ["+LblSubtemas.Content+"]";
            GuardarExpresion.Guarda();
            //GuardarExpresion.Visibility = Visibility.Visible;
        }

        private void BtnVease_Click(object sender, RoutedEventArgs e)
        {
            TreeNodeDataTO nodo = (TreeNodeDataTO)((TreeViewItem)this.SubtemasTree.SelectedItem).Tag;
            TreeViewItem nuevaSeleccion = Vista.Padres[nodo.Target];
            nodo = (TreeNodeDataTO)nuevaSeleccion.Tag;
            if (!nodo.Padre.Equals("0"))
            {
                TreeViewItem Padre = Vista.Padres[nodo.Padre];
                Padre.IsExpanded = true;
            }
            nuevaSeleccion.IsSelected = true;
            //((TreeViewItem)this.SubtemasTree.SelectedItem).IsSelected = false;
            nuevaSeleccion.BringIntoView();
        }

        private void Buscar_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                BtnBuscar_MouseLeftButtonDown(sender, e);
            }
        }

        private void BtnTache_Click(object sender, RoutedEventArgs e)
        {
            EscondeVistaPrel();
        }
    }
}
