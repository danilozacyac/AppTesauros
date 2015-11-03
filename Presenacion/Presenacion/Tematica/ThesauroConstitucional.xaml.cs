using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using IUS;
using mx.gob.scjn.ius_common.TO;
using mx.gob.scjn.ius_common.gui.Guardar;
using mx.gob.scjn.ius_common.gui.impresion;
using mx.gob.scjn.ius_common.gui.utils;
using mx.gob.scjn.ius_common.utils;

namespace mx.gob.scjn.ius_common.gui.Tematica
{
    /// <summary>
    /// Interaction logic for ConsultaTematica.xaml
    /// </summary>
    public partial class ThesauroConstitucional : Page
    {
        /// <summary>
        ///     Representa la página a la que tendría que regresar al oprimir
        ///     el botón correspondiente.
        /// </summary>
        /// <value>
        ///     <para>
        ///         La página para regresar.
        ///     </para>
        /// </value>
        /// <remarks>
        ///     Utilizada en todos los objetos Page a excepcion de la salida.
        /// </remarks>
        public Page Back { get; set; }
        /// <summary>
        ///     Datos a desplegar
        /// </summary>
        /// <value>
        ///     <para>
        ///         Los datos a desplegar.
        ///     </para>
        /// </value>
        private ThesauroTreeView Vista { get; set; }
        /// <summary>
        ///     Define si se debe presentar o no el panel de almacenar después
        ///     de haber presentado la impresión preliminar.
        /// </summary>
        /// <value>
        ///     <para>
        ///         True si es que debe presentarse, False en caso contrario.
        ///     </para>
        /// </value>
        private bool verPnlAlmacenar { get; set; }
        /// <summary>
        /// Define si se tiene o no que mostrar la vista preliminar, esto es para evitar
        /// que en las opciones de guardado se realice dicha acción
        /// </summary>
        private bool MuestraVP { get; set; }
        /// <summary>
        ///     Constructor por omisión
        /// </summary>
        public ThesauroConstitucional()
        {
            InitializeComponent();
            MuestraVP = true;
            Vista = new ThesauroTreeView();
            Vista.HijosTemas.ElementAt(0).IsSelected = true;
            this.Temas.ItemsSource = Vista.HijosTemas;
            Vista.ActualizaSubtemas("1");
            this.SubtemasTree.ItemsSource = Vista.HijosSubtemas;
        }
        /// <summary>
        ///     Evento que maneja lo que sucede cuando un tema es seleccionado
        /// </summary>
        /// <param name="sender" type="object">
        ///     <para>
        ///         El item que fue seleccionado
        ///     </para>
        /// </param>
        /// <param name="e" type="System.Windows.Input.MouseButtonEventArgs">
        ///     <para>
        ///         Los argumentos del Mouse
        ///     </para>
        /// </param>
        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            // Vista = new ThesauroTreeView("Subtemas");
            TreeViewItem seleccionado = (TreeViewItem)Temas.SelectedItem;
            TreeNodeDataTO objeto = (TreeNodeDataTO)seleccionado.Tag;
            Vista.ActualizaSubtemas(objeto.Id);
            Vista.IncorporaCentral(objeto.Id);
            Vista.IncorporaAscendente(objeto.Id);
            Vista.HijosSubtemas.ElementAt(1).IsSelected = true;
            this.SubtemasTree.ItemsSource = Vista.HijosSubtemas;
        }

        /// <summary>
        ///     Regresa a la página "Back".
        /// </summary>
        /// <param name="sender" type="object">
        ///     <para>
        ///         El botón de regresar.
        ///     </para>
        /// </param>
        /// <param name="e" type="System.Windows.RoutedEventArgs">
        ///     <para>
        ///         Los parámetros del click.
        ///     </para>
        /// </param>
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


        private void BloqueSubtema_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //ThesauroTreeView Vista = new ThesauroTreeView("Subtemas");
            TreeViewItem seleccionado = (TreeViewItem)SubtemasTree.SelectedItem;
            TreeNodeDataTO objeto = (TreeNodeDataTO)seleccionado.Tag;
            Vista.ActualizaSinonimos(objeto.Id);
            Vista.ActualizaProximidad(objeto.Id);
            this.Sinonimos.ItemsSource = Vista.HijosSinonimos;
            this.Proximidad.ItemsSource = Vista.HijosProximidad;
            this.Sinonimos.SelectedValuePath = null;
        }

        private void BtnBuscar_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            if (Buscar.Text.Length == 0)
            {
                MessageBox.Show(Mensajes.MENSAJE_CAMPO_TEXTO_VACIO,
                    Mensajes.TITULO_CAMPO_TEXTO_VACIO, MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }
            Boolean noPermitidos = false;
            foreach (String item in Constants.NO_PERMITIDOS)
            {
                noPermitidos = noPermitidos || Buscar.Text.Contains(item);
            }
            if (noPermitidos || Buscar.Text.Contains("\"") || Buscar.Text.Contains("*"))
            {
                MessageBox.Show(Mensajes.MENSAJE_NO_PERMITIDOS, Mensajes.TITULO_NO_PERMITIDOS,
                    MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            int encontrados = Vista.ActualizaTodo(Buscar.Text);
            if (encontrados > 0)
            {
                this.lblFiltrado.Content = Mensajes.ETIQUETA_TESAURO + Buscar.Text;
                Buscar.Text = Constants.CADENA_VACIA;
                this.Temas.ItemsSource = Vista.HijosTemas;
                this.SubtemasTree.ItemsSource = Vista.HijosSubtemas;
                this.Sinonimos.ItemsSource = Vista.HijosSinonimos;
                this.Proximidad.ItemsSource = Vista.HijosProximidad;
            }
            else
            {
                MessageBox.Show(Mensajes.MENSAJE_BUSQUEDA_VACIA, Mensajes.TITULO_BUSQUEDA_VACIA,
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void Visualizar_MouseDown(object sender, RoutedEventArgs e)
        {
            TreeViewItem original = (TreeViewItem)Sinonimos.SelectedItem;
            if (original == null)
            {
                original = (TreeViewItem)this.Proximidad.SelectedItem;
            }
            if (original == null)
            {
                original = (TreeViewItem)SubtemasTree.SelectedItem;
            }
            if (original == null)
            {
                MessageBox.Show("Debe seleccionar algún subtema", "Subtema no seleccionado", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            TreeNodeDataTO nodo = (TreeNodeDataTO)original.Tag;

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
            busqueda.Clasificacion[0].DescTipo = Constants.BUSQUEDA_TESIS_THESAURO;
            busqueda.Clasificacion[0].IdTipo = Int32.Parse(nodo.Id);
#else
            busqueda.clasificacion = new ClassificacionTO[1];
            busqueda.clasificacion[0] = new ClassificacionTO();
            busqueda.clasificacion[0].DescTipo = Constants.BUSQUEDA_TESIS_THESAURO;
            busqueda.clasificacion[0].IdTipo = Int32.Parse(nodo.Id);
#endif
            TreeViewItem tema = (TreeViewItem)Temas.SelectedItem;
            busqueda.OrdenarPor = original.Header+" ["+ tema.Header+"]";
            tablaResultado paginaResultados = new tablaResultado(busqueda);
            paginaResultados.Back = this;
            if (paginaResultados.tablaResultados.Items.Count == 0)
            {
                MessageBox.Show(Mensajes.MENSAJE_BUSQUEDA_VACIA, Mensajes.TITULO_BUSQUEDA_VACIA,
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                this.NavigationService.Navigate(paginaResultados);
            }
        }

        private void Temas_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            TextBlock_MouseLeftButtonDown(sender, null);
        }

        private void SubtemasTree_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            BloqueSubtema_MouseLeftButtonDown(sender, null);
        }

        private void BtnAlmacenar_Click(object sender, RoutedEventArgs e)
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
                    Usuario = new UsuarioTO();
                    Usuario.Usuario = Constants.USUARIO_OMISION;
                }
            }
            //PnlAlmacenar.Visibility = Visibility.Visible;
            PnlAlmacenar.Padre = this;
            PnlAlmacenar.Titulo.Content = Constants.TITULO_CONSULTA;
            TreeViewItem nodo = null;
            nodo = (TreeViewItem)Sinonimos.SelectedItem;
            if (nodo == null)
            {
                nodo = (TreeViewItem)SubtemasTree.SelectedItem;
            }
            BusquedaTO busquedaAGuardar = ObtenBusquedaTO(nodo);
            PnlAlmacenar.ActualizaVentana(busquedaAGuardar, null);
            PnlAlmacenar.TbxGuardar.Text = "Búsqueda Tesauro: " + nodo.Header + " [" + ((TreeViewItem)Temas.SelectedItem).Header + "]";
            PnlAlmacenar.Guarda();
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
            busqueda.Clasificacion[0].IdTipo = Int32.Parse(((TreeNodeDataTO)nodo.Tag).Id);
#else
            busqueda.clasificacion = new ClassificacionTO[1];
            busqueda.clasificacion[0] = new ClassificacionTO();
            busqueda.clasificacion[0].DescTipo = ((TreeNodeDataTO)nodo.Tag).Href;//Href, originalmente
            busqueda.clasificacion[0].IdTipo = Int32.Parse(((TreeNodeDataTO)nodo.Tag).Id);
#endif
            busqueda.OrdenarPor = (String)nodo.Header;
            return busqueda;
        }


        private void BtnImprimir_Click(object sender, RoutedEventArgs e)
        {
            if (ImprimePapel.Visibility == Visibility.Hidden)
            {
                /*Obtener el item seleccionado y ver quien es el padre
                 * para ver si es de primer o segundo nivel*/
                TreeViewItem seleccionado = (TreeViewItem)this.SubtemasTree.SelectedItem;
                TreeNodeDataTO datos = (TreeNodeDataTO)seleccionado.Tag;
                DocumentoThesauro documento = null;
                documento = new DocumentoThesauro((List<TreeViewItem>)SubtemasTree.ItemsSource,
                    (List<TreeViewItem>)Sinonimos.ItemsSource, (List<TreeViewItem>)Proximidad.ItemsSource,
                    (String)((TreeViewItem)SubtemasTree.SelectedItem).Header);
                Imprimir.Document = documento.Documento;
                if (MuestraVP)
                {
                    MostrarVistaPrel();
                }
            }
            else
            {
                ImprimePapel.Visibility = Visibility.Hidden;
                Imprimir.Visibility = Visibility.Hidden;
                Temas.Visibility = Visibility.Visible;
                GrdSinonimos.Visibility = Visibility.Visible;
                GrdProximidad.Visibility = Visibility.Visible;
                GrdEstructura.Visibility = Visibility.Visible;
            }

        }

        //private void BtnImprimir_Click2(object sender, RoutedEventArgs e)
        //{
        //    if (ImprimePapel.Visibility == Visibility.Hidden)
        //    {
        //        /*Obtener el item seleccionado y ver quien es el padre
        //         * para ver si es de primer o segundo nivel*/
        //        DocumentoThesauro documento = null;
        //        foreach (TreeViewItem item in Temas.ItemsSource)
        //        {
        //            item.IsSelected = true;
        //            foreach (TreeViewItem seleccionado in SubtemasTree.Items)
        //            {
        //                seleccionado.IsSelected = true;
        //                TreeNodeDataTO datos = (TreeNodeDataTO)seleccionado.Tag;
        //                if (documento == null)
        //                {
        //                    documento = new DocumentoThesauro((List<TreeViewItem>)SubtemasTree.ItemsSource,
        //                        (List<TreeViewItem>)Sinonimos.ItemsSource, (List<TreeViewItem>)Proximidad.ItemsSource,
        //                        (String)((TreeViewItem)SubtemasTree.SelectedItem).Header);
        //                }
        //                else
        //                {
        //                    documento = new DocumentoThesauro((List<TreeViewItem>)SubtemasTree.ItemsSource,
        //                        (List<TreeViewItem>)Sinonimos.ItemsSource, (List<TreeViewItem>)Proximidad.ItemsSource,
        //                        (String)((TreeViewItem)SubtemasTree.SelectedItem).Header, documento.Documento);
        //                }
        //            }
        //            Section seccionActual = new Section();
        //            seccionActual.BreakPageBefore = true;
        //            documento.Documento.Blocks.Add(seccionActual);
        //        }
        //        Imprimir.Document = documento.Documento;
        //        if (MuestraVP)
        //        {
        //            MostrarVistaPrel();
        //        }
        //    }
        //    else
        //    {
        //        ImprimePapel.Visibility = Visibility.Hidden;
        //        Imprimir.Visibility = Visibility.Hidden;
        //        Temas.Visibility = Visibility.Visible;
        //        GrdSinonimos.Visibility = Visibility.Visible;
        //        GrdProximidad.Visibility = Visibility.Visible;
        //        GrdEstructura.Visibility = Visibility.Visible;
        //    }
        //}
        
        /// <summary>
        ///     Muestra los componentes de la vista preliminar y esconde los que no
        ///     son de esta.
        /// </summary>
        private void MostrarVistaPrel()
        {
            ImprimePapel.Visibility = Visibility.Visible;
            Imprimir.Visibility = Visibility.Visible;
            BtnImprimir.Visibility = Visibility.Hidden;
            BtnTache.Visibility = Visibility.Visible;
            Temas.Visibility = Visibility.Hidden;
            GrdSinonimos.Visibility = Visibility.Hidden;
            GrdProximidad.Visibility = Visibility.Hidden;
            GrdEstructura.Visibility = Visibility.Hidden;
            BtnAlmacenar.Visibility = Visibility.Hidden;
            BtnBuscar.Visibility = Visibility.Hidden;
            BtnRestaurar.Visibility = Visibility.Hidden;
            Visualizar.Visibility = Visibility.Hidden;
            Buscar.Visibility = Visibility.Hidden;
            if (PnlAlmacenar.Visibility == Visibility.Visible) verPnlAlmacenar = true;
            PnlAlmacenar.Visibility = Visibility.Hidden;
            lblFiltrado.Visibility = Visibility.Hidden;
            IDLabel.Visibility = Visibility.Hidden;
            IDSLabel.Visibility = Visibility.Hidden;
            Regresar.Visibility = Visibility.Hidden;
        }
        private void EscondeVistaPrel()
        {
            ImprimePapel.Visibility = Visibility.Hidden; ;
            Imprimir.Visibility = Visibility.Hidden;
            BtnImprimir.Visibility = Visibility.Visible;
            BtnTache.Visibility = Visibility.Hidden;
            Temas.Visibility = Visibility.Visible;
            GrdSinonimos.Visibility = Visibility.Visible;
            GrdProximidad.Visibility = Visibility.Visible;
            GrdEstructura.Visibility = Visibility.Visible;
            BtnAlmacenar.Visibility = Visibility.Visible;
            BtnBuscar.Visibility = Visibility.Visible;
            BtnRestaurar.Visibility = Visibility.Visible;
            Visualizar.Visibility = Visibility.Visible;
            Buscar.Visibility = Visibility.Visible;
            if (verPnlAlmacenar)PnlAlmacenar.Visibility = Visibility.Visible;
            lblFiltrado.Visibility = Visibility.Visible;
            IDLabel.Visibility = Visibility.Visible;
            IDSLabel.Visibility = Visibility.Visible;
            Regresar.Visibility = Visibility.Visible;
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
                    printingDialog.PrintDocument(pag.DocumentPaginator, "Impresión Temática");
                }
                catch (Exception exc)
                {
                    MessageBox.Show(Mensajes.MENSAJE_IMPRESORA, Mensajes.TITULO_ARCHIVO_ABIERTO,
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            EscondeVistaPrel();
        }

        private void BtnRestaurar_Click(object sender, RoutedEventArgs e)
        {
            Vista = new ThesauroTreeView();
            this.Temas.ItemsSource = Vista.HijosTemas;
            this.lblFiltrado.Content = Mensajes.ETIQUETA_TESAURO_ORIGINAL;
            Buscar.Text = "";
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

        private void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
