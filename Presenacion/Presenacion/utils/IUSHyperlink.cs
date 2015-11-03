using System;
using System.Collections.Generic;
using System.Security.Permissions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Navigation;
using IUS;
using mx.gob.scjn.ius_common.TO;
using mx.gob.scjn.ius_common.fachade;
using mx.gob.scjn.ius_common.gui.apoyos;
using mx.gob.scjn.ius_common.utils;

namespace mx.gob.scjn.ius_common.gui.utils
{
    [UIPermissionAttribute(SecurityAction.InheritanceDemand, Unrestricted = false)]

    public class IUSHyperlink : Span
    {
        /// <summary>
        /// El elemento a mostrar cuando se oprima la liga.
        /// </summary>
        protected UIElement elementoUI;
        /// <summary>
        /// El servicio de navegación para el caso de tener que cambiar de
        /// pantalla
        /// </summary>
        public Page PaginaTarget {
            get { return this.getNavigationService(); }
            set { this.setNavigationService(value); }
        }
        private Page navigationService;
        /// <summary>
        /// El historial para la pantalla nueva
        /// </summary>
        public Historial Historia { get; set; }
        /// <summary>
        /// El URI de navegación que se despegará.
        /// </summary>
        public Uri NavigateUri { get; set; }
        /// <summary>
        /// En caso de ser una liga que lleva a una relación de Articulos
        /// se tiene esta lista para que se despliegue.
        /// </summary>
        public List<RelacionFraseArticulosTO> ListaLeyes { get; set; }
        /// <summary>
        /// Este constructor es para cuando hay que abrir un control de usuario
        /// en lugar de una ventana nueva.
        /// </summary>
        /// <param name="showElement">El Control de usuario que se mostrará</param>
        public IUSHyperlink(UIElement showElement):base()
        {
            this.BeginInit();
            this.ToolTip = "Para ver la información correspondiente dé clic sobre la liga";
            this.IsEnabled = true;
           
            this.Cursor = Cursors.Hand;
            this.Foreground = Brushes.Blue;
            elementoUI = showElement;
            this.EndInit();
        }
        /// <summary>
        /// Constructor  por omisión, genera una liga hacia la página de la SCJN
        /// </summary>
            public IUSHyperlink():base()
            {
                this.BeginInit();
                this.ToolTip = Constants.TOOLTIP_LIGAS;
                this.NavigateUri = new Uri(Constants.URI_SCJN);
                this.IsEnabled = true;
                this.Cursor = Cursors.Hand;
                this.Foreground = Brushes.Blue;
                this.EndInit();
            }

        /// <summary>
        /// El click sobre la liga
        /// </summary>
        /// <param name="e">Los argumentos de la interfaz gráfica</param>
        protected override void  OnMouseLeftButtonDown(MouseButtonEventArgs e)
            {
                base.OnMouseLeftButtonDown(e);
                MyHyperlink_RequestNavigate(this, new RoutedEventArgs ());
            }
        /// <summary>
        /// La acción de navegación después del click.
        /// </summary>
        /// <param name="sender">Parámetros de la interfaz Gráfica</param>
        /// <param name="e">Parámetros de la interfaz gráfica</param>
            void MyHyperlink_RequestNavigate(object sender, RoutedEventArgs e)
            {
                String tagInterno = null;
                if (typeof(String) == Tag.GetType())
                {
                    tagInterno = (String)this.Tag;
                }
                else
                {
                    try
                    {
                        if (this.Tag.GetType() == typeof(Tesis))
                        {
                            Tesis ventanaTesis = (Tesis)this.Tag;
                            ventanaTesis.ventanaHistorial.Visibility = Visibility.Hidden;
                            ventanaTesis.ventanaListaEjecutorias.Visibility = Visibility.Hidden;
                            ventanaTesis.ventanaListaVotos.Visibility = Visibility.Hidden; ;
                        }
                        if (this.Tag.GetType() == typeof(EjecutoriaPagina))
                        {
                            EjecutoriaPagina ventanaTesis = (EjecutoriaPagina)this.Tag;
                            ventanaTesis.ventanaHistorial.Visibility = Visibility.Hidden;
                            ventanaTesis.ventanaListadoVotos.Visibility = Visibility.Hidden;
                            ventanaTesis.ventanaListadoTesis.Visibility = Visibility.Hidden; ;
                        }
                        if (this.Tag.GetType() == typeof(VotosPagina))
                        {
                            VotosPagina ventanaTesis = (VotosPagina)this.Tag;
                            ventanaTesis.ventanaHistorial.Visibility = Visibility.Hidden;
                            ventanaTesis.ventanaListadoTesis.Visibility = Visibility.Hidden;
                            ventanaTesis.ventanaListadoEjecutorias.Visibility = Visibility.Hidden;
                        }
                        if (this.PaginaTarget.NavigationService == null)
                        {
                            NavigationWindow navWind = (NavigationWindow)Application.Current.MainWindow;
                            PaginaTarget = (Page)navWind.Content;
                            PaginaTarget.NavigationService.Navigate(this.Tag);
                        }
                        else
                        {
                            PaginaTarget.NavigationService.Navigate(this.Tag);
                        }
                        return;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(Mensajes.MENSAJE_ERROR_LIGA_1 + ex.Message + Mensajes.MENSAJE_ERROR_LIGA_2 +
                        PaginaTarget.NavigationService + "," + this.Tag,
                            "Problemas con la liga", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        return;
                    }
                }
                if (PaginaTarget == null)
                {
                    NavigationWindow navWind = (NavigationWindow)Application.Current.MainWindow;
                    PaginaTarget = (Page)navWind.Content;
                    if (PaginaTarget.GetType() == typeof(Tesis))
                    {
                        Tesis tesina = (Tesis)PaginaTarget;
                        Historia = tesina.Historia;
                    }
                }

                if (tagInterno.Contains("ventanaEmergente"))//Una liga de leyes
                {
                    String numeros = tagInterno.Substring(tagInterno.IndexOf('(') + 1, tagInterno.IndexOf(')') - tagInterno.IndexOf('(') - 1);
                    String coma2 = ",";
                    Char[] coma = coma2.ToCharArray();
                    String[] referencias = numeros.Split(coma);
                    if (referencias[0].Equals("0") && referencias[1].Equals("0") )// Es una lista de leyes
                    {
                        if (PaginaTarget.GetType() == typeof(Tesis))
                        {
                            Tesis tesis = (Tesis)PaginaTarget;
                            this.elementoUI = tesis.ventanaListadoLeyes;
                            ListadoLeyes ventanita = (ListadoLeyes)this.elementoUI;
                            int elemento = Int32.Parse(referencias[2]);
                            if (this.ListaLeyes == null)
                            {
                                this.ListaLeyes = tesis.listaLeyes[elemento];
                            }
                            ventanita.ListaRelacion = this.ListaLeyes;
                            ventanita.ActualizaVentana();
                        }
                    }
                    else
                    {
                        if (PaginaTarget.GetType() == typeof(Tesis))
                        {
                            Tesis tesis = (Tesis)PaginaTarget;
                            this.elementoUI = tesis.ventanitaLeyes;
                            VentanaLeyes ventanita = (VentanaLeyes)this.elementoUI;
                            ventanita.IdLey = Int32.Parse(referencias[0]);
                            ventanita.IdArticulo = Int32.Parse(referencias[1]);
                            ventanita.IdRef = Int32.Parse(referencias[2]);
                            ventanita.ActualizaVentana();
                        }
                    }
                    this.elementoUI.Visibility = Visibility.Visible;
                }
                else if (tagInterno.Contains("Acuerdos"))//Una liga a un acuerdo nuevo
                {
                    int DocumentoId = Int32.Parse(tagInterno.Substring(9, tagInterno.Length - 10));
                    foreach (IUSNavigationService item in Historia.Lista)
                    {
                        if ((item.Id == DocumentoId) && (item.TipoVentana == IUSNavigationService.ACUERDO))
                        {
                            MessageBox.Show(Mensajes.MENSAJE_VENTANA_ABIERTA, Mensajes.TITULO_VENTANA_ABIERTA,
                                MessageBoxButton.OK, MessageBoxImage.Information);
                            PaginaTarget.NavigationService.Navigate(item.ParametroConstructor);
                            return;
                        }
                    }
#if STAND_ALONE
                    FachadaBusquedaTradicional fachada = new FachadaBusquedaTradicional();
#else
                    FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
#endif
                    AcuerdosTO tesisCompleta = fachada.getAcuerdoPorId(DocumentoId);
                    AcuerdosPagina paginaTesis;
                    AcuerdosPagina paginaAcuerdos;
                    if (tesisCompleta.Id != null)
                    {
                        paginaTesis = new AcuerdosPagina(Int32.Parse(tesisCompleta.Id));
                        paginaTesis.Historia = Historia;
                        paginaTesis.Back = PaginaTarget;
                        PaginaTarget.NavigationService.Navigate(paginaTesis);
                    }
                    else
                    {
                        paginaAcuerdos = new AcuerdosPagina(Int32.Parse(tagInterno.Substring(9, tagInterno.Length - 10)));
                        paginaAcuerdos.Historia = Historia;
                        paginaAcuerdos.Back = PaginaTarget;
                        PaginaTarget.NavigationService.Navigate(paginaAcuerdos);
                    }
                    fachada.Close();
                }
                else if (tagInterno.Contains("Tesis"))
                {
                    int DocumentoId = Int32.Parse(tagInterno.Substring(6, tagInterno.Length - 7));
                    foreach (IUSNavigationService item in Historia.Lista)
                    {
                        if ((item.Id == DocumentoId) && (item.TipoVentana == IUSNavigationService.TESIS))
                        {
                            MessageBox.Show(Mensajes.MENSAJE_VENTANA_ABIERTA, Mensajes.TITULO_VENTANA_ABIERTA,
                                MessageBoxButton.OK, MessageBoxImage.Information);
                            Tesis paginitaTesis = (Tesis)item.ParametroConstructor;// (Tesis)PaginaTarget;
                            paginitaTesis.ventanaHistorial.Visibility = Visibility.Hidden;
                            PaginaTarget.NavigationService.Navigate(item.ParametroConstructor);
                            return;
                        }
                    }
#if STAND_ALONE
                    FachadaBusquedaTradicional fachada = new FachadaBusquedaTradicional();
#else
                    FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
#endif
                    TesisTO tesisCompleta = fachada.getTesisPorRegistroLiga(tagInterno.Substring(6, tagInterno.Length - 7));
                    Tesis paginaTesis = new Tesis(tesisCompleta);
                    paginaTesis.Historia = Historia;
                    fachada.Close();
                    paginaTesis.Back = PaginaTarget;
                    PaginaTarget.NavigationService.Navigate(paginaTesis);
                }
                else if (tagInterno.Contains("Ejecutoria"))
                {
                    int DocumentoId = Int32.Parse(tagInterno.Substring(11, tagInterno.Length - 12));
                    foreach (IUSNavigationService item in Historia.Lista)
                    {
                        if ((item.Id == DocumentoId) && (item.TipoVentana == IUSNavigationService.EJECUTORIA))
                        {
                            MessageBox.Show(Mensajes.MENSAJE_VENTANA_ABIERTA, Mensajes.TITULO_VENTANA_ABIERTA,
                                MessageBoxButton.OK, MessageBoxImage.Information);
                            EjecutoriaPagina paginitaTesis = (EjecutoriaPagina)item.ParametroConstructor;// (EjecutoriaPagina)PaginaTarget;
                            paginitaTesis.ventanaHistorial.Visibility = Visibility.Hidden;
                            PaginaTarget.NavigationService.Navigate(item.ParametroConstructor);
                            return;
                        }
                    }
#if STAND_ALONE
                    FachadaBusquedaTradicional fachada = new FachadaBusquedaTradicional();
#else
                    FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
#endif
                    EjecutoriasTO ejecutoriaCompleta = fachada.getEjecutoriaPorId(Int32.Parse(tagInterno.Substring(11, tagInterno.Length - 12)));
                    EjecutoriaPagina paginaEjecutoria = new EjecutoriaPagina(Int32.Parse(ejecutoriaCompleta.Id));
                    paginaEjecutoria.Historia = Historia;
                    fachada.Close();
                    paginaEjecutoria.Back = PaginaTarget;
                    PaginaTarget.NavigationService.Navigate(paginaEjecutoria);
                }
                else if (tagInterno.Contains("Voto"))
                {
                    int DocumentoId = Int32.Parse(tagInterno.Substring(6, tagInterno.Length - 7));
                    foreach (IUSNavigationService item in Historia.Lista)
                    {
                        if ((item.Id == DocumentoId) && (item.TipoVentana == IUSNavigationService.VOTO))
                        {
                            MessageBox.Show(Mensajes.MENSAJE_VENTANA_ABIERTA, Mensajes.TITULO_VENTANA_ABIERTA,
                                MessageBoxButton.OK, MessageBoxImage.Information);
                            VotosPagina paginitaTesis = (VotosPagina)item.ParametroConstructor;// (VotosPagina)PaginaTarget;
                            paginitaTesis.ventanaHistorial.Visibility = Visibility.Hidden;
                            PaginaTarget.NavigationService.Navigate(item.ParametroConstructor);
                            return;
                        }
                    }
#if STAND_ALONE
                    FachadaBusquedaTradicional fachada = new FachadaBusquedaTradicional();
#else
                    FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
#endif
                    VotosTO votoCompleta = fachada.getVotosPorId(Int32.Parse(tagInterno.Substring(6, tagInterno.Length - 7)));
                    VotosPagina paginaVoto = new VotosPagina(Int32.Parse(votoCompleta.Id));
                    paginaVoto.Historia = Historia;
                    fachada.Close();
                    paginaVoto.Back = PaginaTarget;
                    PaginaTarget.NavigationService.Navigate(paginaVoto);
                }
                else if (tagInterno.Contains("PDF"))
                {
                    try
                    {
                        if (this.elementoUI == null)
                        {
                            if (this.PaginaTarget.NavigationService == null)
                            {
                                NavigationWindow navWind = (NavigationWindow)Application.Current.MainWindow;
                                PaginaTarget = (Page)navWind.Content;
                            }
                            ///Esto es para ponerlo dentro de los tabs, mientras Microsoft
                            ///No arregle el orden de los renderings se tendrá que hacer por medio
                            ///de una nueva ventana.
                            ///
                            //if (typeof(Tesis) == this.PaginaTarget.GetType())
                            //{
                            //    Tesis ventanaTesis = (Tesis)this.PaginaTarget;
                            //    this.elementoUI = ventanaTesis.tabControl1;
                            //}
                            //else if (typeof(AcuerdosPagina) == this.PaginaTarget.GetType())
                            //{
                            //    AcuerdosPagina ventanaTesis = (AcuerdosPagina)this.PaginaTarget;
                            //    this.elementoUI = ventanaTesis.tabControl1;
                            //}
                            //else if (typeof(EjecutoriaPagina) == this.PaginaTarget.GetType())
                            //{
                            //    EjecutoriaPagina ventanaTesis = (EjecutoriaPagina)this.PaginaTarget;
                            //    this.elementoUI = ventanaTesis.tabControl1;
                            //}
                            //else if (typeof(VotosPagina) == this.PaginaTarget.GetType())
                            //{
                            //    VotosPagina ventanaTesis = (VotosPagina)this.PaginaTarget;
                            //    this.elementoUI = ventanaTesis.tabControl1;
                            //}
                        }

                        ///IDEM
                        //TabItem nuevoTab = new TabItem();
                        //TabControl control = (TabControl)this.elementoUI;
                        //WebBrowser nuevoFrame = new WebBrowser();
                        String tituloTab = tagInterno.Substring(4, tagInterno.Length - 8);
                        //Uri uriPdf = new Uri("Pack://siteoforigin:,,,/Anexos/" + tituloTab + "htm");
                        //nuevoTab.Header = tituloTab;
                        //nuevoTab.Content = nuevoFrame;
                        //control.Items.Add(nuevoTab);
                        //nuevoTab.Focus();
                        //nuevoFrame.Navigate(uriPdf);
                        //nuevoFrame.IsEnabled = false;
#if STAND_ALONE
                        Informe ventanaNueva = new Informe(IUSConstants.IUS_RUTA_ANEXOS + tituloTab + "htm");
#else
                        Informe ventanaNueva = new Informe("Anexos/" + tituloTab + "htm");
#endif
                        
                        PaginaTarget.NavigationService.Navigate(ventanaNueva);
                    }
                    catch (Exception exc)
                    {
                        MessageBox.Show(Mensajes.MENSAJE_PROBLEMAS_FACHADA + exc.Message, "Problema al obtener el PDF", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            private Page getNavigationService()
            {
                return navigationService;
            }
            private void setNavigationService(Page value)
            {
                this.navigationService = value;
            }
    }

}
