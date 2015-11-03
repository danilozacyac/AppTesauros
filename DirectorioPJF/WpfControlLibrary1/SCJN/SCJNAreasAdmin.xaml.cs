using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using mx.gob.scjn.directorio.General;
using mx.gob.scjn.ius_common.TO;
using mx.gob.scjn.ius_common.fachade;

namespace mx.gob.scjn.directorio.SCJN
{

    /// <summary>
    /// Interaction logic for SCJNAreasAdmin.xaml
    /// </summary>
    public partial class SCJNAreasAdmin : Page
    {
        String enterKey = "\n";
        /*Para la impresión ***********************************************/

        private FlowDocument DocumentoParaCopiar { get; set; }
        private AcuerdosTO DocumentoActual;
        String strAAImpr = "";
        List<DirectorioOrgJurTO> lstParaImprimir = new List<DirectorioOrgJurTO>();
        /******************************************************************/
        List<String> strArrNC = new List<String>();
        List<DirectorioOrgJurTO> lstRes = new List<DirectorioOrgJurTO>();
        List<DirectorioOrgJurTO> lstUltimoFiltro = new List<DirectorioOrgJurTO>();

        public SCJNAreasAdmin()
        {
            InitializeComponent();
            lstUltimoFiltro = lstParaImprimir = TraeOrganoJur();

            if (!BrowserInteropHelper.IsBrowserHosted)
            { Guardar_.Visibility = Visibility.Visible; }
            else { Guardar_.Visibility = Visibility.Hidden; }
        }

        public Page Back { get; set; }

        private List<DirectorioOrgJurTO> TraeOrganoJur()
        {
            List<DirectorioOrgJurTO> lstResLocal = new List<DirectorioOrgJurTO>();
            lstRes = new List<DirectorioOrgJurTO>();
#if STAND_ALONE
            List<DirectorioOrgJurTO> R = null;
            FachadaBusquedaTradicional fachada = new FachadaBusquedaTradicional();
#else
            DirectorioOrgJurTO[] R = new DirectorioOrgJurTO[50];
            FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
#endif
            R = fachada.getDirAreasAdmin();
#if STAND_ALONE
            for (int i = 0; i < R.Count; i++)
#else
            for (int i = 0; i < R.Length; i++)
#endif
            {
                DirectorioOrgJurTO Campo = new DirectorioOrgJurTO();
                Campo.IdOrganoJur = R[i].IdOrganoJur;
                Campo.NombreOrganoJur = R[i].NombreOrganoJur;
                Campo.DomOrganoJur = R[i].DomOrganoJur;
                Campo.TelOrganoJur = R[i].TelOrganoJur;
                Campo.NombreStrOrganoJur = R[i].NombreStrOrganoJur;
                Campo.TitularSolo = R[i].TitularSolo;
                lstRes.Add(Campo);
                lstResLocal.Add(Campo);
                //vamos a usar esta variable para poder ubicar los nombres en el texto de acceso rapido
                string tmp = R[i].NombreStrOrganoJur;
                tmp = tmp.Replace(" x ", "");
                tmp = tmp.Trim();
                strArrNC.Add(tmp);
                Campo = null;
            }
            this.AreasAdmin.ItemsSource = lstRes;
            this.AreasAdmin.SelectedIndex = 0;
            this.AreasAdmin.SelectedItem = this.AreasAdmin.Items.CurrentItem;
            textCuantos.Text = this.AreasAdmin.Items.Count.ToString();
            CargaDetalleAA((DirectorioOrgJurTO)AreasAdmin.SelectedItem);
            fachada.Close();
            return lstResLocal;
        }

        private void CargaDetalle(object sender, MouseButtonEventArgs e)
        {
            try
            {
                CargaDetalleAA((DirectorioOrgJurTO)AreasAdmin.Items.CurrentItem);
            }

            catch { }
        }

        private void CargaDetalleAA(DirectorioOrgJurTO Campo)
        {
            try
            {
                this.textNombreArea.Text = Campo.NombreOrganoJur;
                textTitular.Text = Campo.TitularSolo;
                textDomTel.Text = Campo.DomOrganoJur + " Tel. " + Campo.TelOrganoJur;
                strAAImpr = Campo.NombreOrganoJur + enterKey;
                strAAImpr = strAAImpr + Campo.TitularSolo + enterKey;
                strAAImpr = strAAImpr + Campo.DomOrganoJur + enterKey;
                strAAImpr = strAAImpr + " Tel. " + Campo.TelOrganoJur + enterKey;
                Campo = null;
            }

            catch { }
        }

        private void txtAccesoRapido_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            txtAccesoRapido.Text = "";
        }

        private void txtAccesoRapido_TextChanged(object sender, TextChangedEventArgs e)
        {
            String StrB = txtAccesoRapido.Text;
            Boolean bHayDatos = false;
            Boolean bTextoOk = false;

            if (strArrNC.Count > 0)
            {
                StrB.Trim();

                if (StrB.Length > 0)
                {
                    ValidarCadenas strOk = new ValidarCadenas(StrB);
                    bTextoOk = strOk.CadenaOK();

                    if (bTextoOk)
                    {
                        bHayDatos = FiltraAreas(txtAccesoRapido.Text);

                        if (bHayDatos)
                        {
                            cambiaStTxtAR(0);
                        }
                    }
                    else { cambiaStTxtAR(1); }
                }
                else if (StrB.Length == 0) //si no hay texto (o borró todo), nos vamos al inicio del grid
                {
                    lstParaImprimir= lstUltimoFiltro = lstRes;
                    this.AreasAdmin.ItemsSource = lstRes;
                    this.AreasAdmin.SelectedIndex = 0;
                    this.AreasAdmin.SelectedItem = this.AreasAdmin.Items.CurrentItem;
                    textCuantos.Text = this.AreasAdmin.Items.Count.ToString();   // strArrNC.Count.ToString();
                    CargaDetalleAA((DirectorioOrgJurTO)AreasAdmin.SelectedItem);
                    cambiaStTxtAR(0);
                }
            }
        }

        // Cambia el status del acceso rápido
        private void cambiaStTxtAR(int nStatus)
        {
            switch (nStatus)
            {

                case 0: // Estado Inicial
                    lblHayDatos.Visibility = Visibility.Hidden;
                    lblHayDatos.Content = "";
                    txtAccesoRapido.Foreground = Brushes.Black;
                    break;

                case 1: // Caracteres no permitido
                    lblHayDatos.Visibility = Visibility.Visible;
                    lblHayDatos.Content = "Caracter no permitido";
                    txtAccesoRapido.Foreground = Brushes.Red;
                    break;

                case 2: // Ya no hay nada que buscar
                    lblHayDatos.Visibility = Visibility.Visible;
                    lblHayDatos.Content = "No hay coincidencias";
                    txtAccesoRapido.Foreground = Brushes.Red;
                    break;
            }
        }

        private Boolean FiltraAreas(String strPal)
        {
            Boolean bHayDatos = false;
            lstParaImprimir = FiltroXPal(lstRes, lstUltimoFiltro, strPal);

            if (lstParaImprimir.Count > 0)
            {
                this.AreasAdmin.ItemsSource = lstParaImprimir;
                lstUltimoFiltro = lstParaImprimir;
                lblHayDatos.Visibility = Visibility.Hidden;
                lblHayDatos.Content = "";
                bHayDatos = true;
                txtAccesoRapido.Foreground = Brushes.Black;
            }
            else
            {
                this.AreasAdmin.ItemsSource = lstUltimoFiltro;
                cambiaStTxtAR(2);
            }
            this.AreasAdmin.SelectedIndex = 0;
            this.AreasAdmin.SelectedItem = this.AreasAdmin.Items.CurrentItem;
            textCuantos.Text = this.AreasAdmin.Items.Count.ToString();
            CargaDetalleAA((DirectorioOrgJurTO)AreasAdmin.SelectedItem);
            return bHayDatos;
        }

        private List<DirectorioOrgJurTO> FiltroXPal(List<DirectorioOrgJurTO> Lista, List<DirectorioOrgJurTO> ListaUltRes, String strPal)
        {
            string strPalabra = strPal.ToUpper();
            String strLocal = strPalabra;
            ValidarCadenas strOKLocal = new ValidarCadenas(strLocal);
            strLocal = strOKLocal.QuitaCarMalosN(strLocal);
            List<DirectorioOrgJurTO> lstFiltro = new List<DirectorioOrgJurTO>();

            for (int nPos = 0; nPos <= Lista.Count - 1; nPos++)
            {

                if (Lista[nPos].NombreStrOrganoJur.Contains(strLocal))
                {
                    lstFiltro.Add(Lista[nPos]);
                }
            }
            return lstFiltro;
        }

        private void txtAccesoRapido_GotFocus(object sender, RoutedEventArgs e)
        {
            txtAccesoRapido.Text = "";
            txtAccesoRapido.Foreground = Brushes.Black;
        }

        private void Salir_MouseButtonDown(object sender, MouseButtonEventArgs e)
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

        private void Salir_ImageFailed(object sender, ExceptionRoutedEventArgs e)
        {
        }

        private void Guardar(object sender, MouseButtonEventArgs e)
        {

            //String strCabecera = "Opciones de almacenamiento";
            //String strMensaje = "Seleccione lo que se va a guardar en disco ";
            //String strActual = "";
            //String strTodos = "";
            //int nT = -1;
            //OpcionesImprimir opMinistros = new OpcionesImprimir();
            //opMinistros.Visibility = Visibility.Visible;
            //nT = lstUltimoFiltro.Count();

            //if (nT > 1)
            //{
            //    strActual = "Área actual";
            //    strTodos = "Todas las áreas";
            //}
            //else if (nT == 1)
            //{
            //    strMensaje = "Los datos de la Área actual se van a guardar en disco";
            //    strActual = "";
            //    strTodos = "";
            //}

            //FTransparente.Height = this.Height;
            //FTransparente.Width = this.Width;

            //OpImprimir.TomaFondo(FTransparente); 

            //OpImprimir.StrCabecera = strCabecera;
            //OpImprimir.StrMensaje = strMensaje;
            //OpImprimir.StrActual = strActual;
            //OpImprimir.StrOpcionTodos = strTodos;
            //OpImprimir.OptSalida = 1;
            //OpImprimir.BringIntoView();
            //this.OpImprimir.contenedor = this;
            //OpImprimir.Visibility = Visibility.Visible;
        }

        private void PortaPapeles(object sender, MouseButtonEventArgs e)
        {
            //String strCabecera = "Opciones de almacenamiento";
            //String strMensaje = "Seleccione lo que se va a enviar al portapapeles ";
            //String strActual = "";
            //String strTodos = "";
            //int nT = -1;
            //OpcionesImprimir opMinistros = new OpcionesImprimir();
            //opMinistros.Visibility = Visibility.Visible;
            //nT = lstUltimoFiltro.Count();

            //if (nT > 1)
            //{
            //    strActual = "Área actual";
            //    strTodos = "Todas las áreas";
            //}
            //else if (nT == 1)
            //{
            //    strMensaje = "Los datos de la Área actual se van a enviar al portapapeles";
            //    strActual = "";
            //    strTodos = "";
            //}

            ////FTransparente.Visibility = Visibility.Visible;
            
            ////FondoTransparente Ft = new FondoTransparente();
            //FTransparente.Height = this.Height;
            //FTransparente.Width = this.Width;

            //OpImprimir.TomaFondo(FTransparente); 
                        
            //OpImprimir.StrCabecera = strCabecera;
            //OpImprimir.StrMensaje = strMensaje;
            //OpImprimir.StrActual = strActual;
            //OpImprimir.StrOpcionTodos = strTodos;
            //OpImprimir.OptSalida = 0;
            //OpImprimir.BringIntoView();
            
            //this.OpImprimir.contenedor = this;
            //OpImprimir.Visibility = Visibility.Visible;

        
        }

        private void Imprimir(object sender, MouseButtonEventArgs e)
        {
            String strCabecera = "Opciones de impresión";
            String strMensaje = "Seleccione lo que se va a imprimir";
            String strActual = "";
            String strTodos = "";
            int nT = -1;
            OpcionesImprimir opMinistros = new OpcionesImprimir();
            opMinistros.Visibility = Visibility.Visible;
            nT = lstUltimoFiltro.Count();

            if (nT > 1)
            {
                strActual = "Área actual";
                strTodos = "Todas las áreas";
            }
            else if (nT == 1)
            {
                strMensaje = "Los datos de la Área actual se van a imprimir";
                strActual = "";
                strTodos = "";
            }

            FTransparente.Visibility = Visibility.Visible;
            
            OpImprimir.StrCabecera = strCabecera;
            OpImprimir.StrMensaje = strMensaje;
            OpImprimir.StrActual = strActual;
            OpImprimir.StrOpcionTodos = strTodos;
            OpImprimir.OptSalida = 2;
            OpImprimir.BringIntoView();
            this.OpImprimir.contenedor = this;
            OpImprimir.Visibility = Visibility.Visible;

            OpImprimir.TomaFondo(FTransparente); 

        }

        public void ImprimeListado(Boolean bTodo, Int32 nSelSalida)
        {
            String strText = "";

            if (bTodo == true) //Imprimir todos 
            {

                foreach (DirectorioOrgJurTO ItemIntegrante in lstParaImprimir)
                {
                    strText = strText + ItemIntegrante.NombreOrganoJur + enterKey;
                    strText = strText + "Titular: " + ItemIntegrante.TitularSolo + enterKey;
                    strText = strText + "Teléfono: " + ItemIntegrante.TelOrganoJur + enterKey;
                    strText = strText + enterKey;
                }
            }
            else   //Imprimir sólo el actual
            {
                strText = strAAImpr + enterKey;
            }

            switch (nSelSalida)
            {

                case 0: //Papelera
                    PortaPapStr(strText);
                    break;

                case 1: //Archivo
                    GuardarEnArchivo(strText);
                    break;

                case 2: //Imprimir
                    ImprimeStr(strText);
                    break;
            }
        }

        private void ImprimeStr(String strTexto)
        {
            try
            {

                DocumentoAcuerdoDirec documento;

                documento = new DocumentoAcuerdoDirec(strTexto);

                DocumentoParaCopiar = documento.Copia;
                impresion.Document = documento.Documento; //(IDocumentPaginatorSource)documentoXps;
                impresion.Visibility = Visibility.Hidden;
                impresion.Background = Brushes.White;
                PrintDialog pd = new PrintDialog();

                if ((pd.ShowDialog() == true))
                {
                    IDocumentPaginatorSource pag = impresion.Document as IDocumentPaginatorSource;
                    pd.PrintDocument(pag.DocumentPaginator, "Directorio");
                }
                impresion.Visibility = Visibility.Hidden;
            }

            catch (System.Exception error)
            {
                MessageBox.Show(MensajesDirectorio.MENSAJE_Y_LA_IMPRESORA, MensajesDirectorio.TITULO_MENSAJES,
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void PortaPapStr(String strTexto)
        {
            contenidoTexto = new RichTextBox();
            this.contenidoTexto.AppendText(strTexto);
            this.contenidoTexto.SelectAll();
            contenidoTexto.Copy();
           
            FTransparente.Visibility = Visibility.Visible;
            Aviso.TomaFondo(FTransparente);
      
            Aviso.StrCabecera = mx.gob.scjn.directorio.MensajesDirectorio.TITULO_PORTAPAPELES_DIR;
            Aviso.StrMensaje = mx.gob.scjn.directorio.MensajesDirectorio.MENSAJE_PORTAPAPELES_DIR;
            Aviso.Visibility = Visibility.Visible;
            Aviso.Background = Brushes.White;
            

        }

        private void GuardarEnArchivo(String strTexto)
        {
            try
            {

                if (!BrowserInteropHelper.IsBrowserHosted)
                {
                    Microsoft.Win32.SaveFileDialog guardaEn = new Microsoft.Win32.SaveFileDialog();
                    guardaEn.DefaultExt = ".rtf";
                    guardaEn.FileName = "DirectorioPJF";
                    guardaEn.Title = "Guardar";
                    guardaEn.Filter = "Archivos de Texto Enriquecido|*.rtf";
                    guardaEn.AddExtension = true;

                    if ((bool)guardaEn.ShowDialog())
                    {
                        contenidoTexto = new RichTextBox();
                        contenidoTexto.AppendText(strTexto);
                        System.IO.FileStream archivo = new System.IO.FileStream(guardaEn.FileName, System.IO.FileMode.Create);
                        this.contenidoTexto.SelectAll();
                        this.contenidoTexto.Selection.Save(archivo, System.Windows.DataFormats.Rtf);
                        archivo.Flush();
                        archivo.Close();
                        MessageBox.Show("El archivo fue guardado como: " + archivo.Name, MensajesDirectorio.TITULO_MENSAJES);
                    }
                }
                else
                {
                    contenidoTexto = new RichTextBox();
                    contenidoTexto.AppendText(strTexto);
                    System.IO.IsolatedStorage.IsolatedStorageFileStream archivo = new System.IO.IsolatedStorage.
                        IsolatedStorageFileStream("texto.rtf", System.IO.FileMode.Create);
                    this.contenidoTexto.SelectAll();
                    this.contenidoTexto.Selection.Save(archivo, System.Windows.DataFormats.Text);
                    archivo.Flush();
                    archivo.Close();
                    MessageBox.Show("El archivo fue guardado como: " + archivo.Name, MensajesDirectorio.TITULO_MENSAJES,
                        MessageBoxButton.OK,
                        MessageBoxImage.Exclamation);
                }
                impresion.Visibility = Visibility.Hidden;
            }

            catch (System.Exception error)
            {
                MessageBox.Show(MensajesDirectorio.MENSAJE_ARCHIVO_ABIERTO, MensajesDirectorio.TITULO_ARCHIVO_ABIERTO,
                   MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Imprimir__ImageFailed(object sender, ExceptionRoutedEventArgs e)
        {
        }

        private void txtAccesoRapido_KeyDown(object sender, KeyEventArgs e)
        {
        }

        private void Guardar_Click(object sender, RoutedEventArgs e)
        {
            String strCabecera = "Opciones de almacenamiento";
            String strMensaje = "Seleccione lo que se va a guardar en disco ";
            String strActual = "";
            String strTodos = "";
            int nT = -1;
            OpcionesImprimir opMinistros = new OpcionesImprimir();
            opMinistros.Visibility = Visibility.Visible;
            nT = lstUltimoFiltro.Count();

            if (nT > 1)
            {
                strActual = "Área actual";
                strTodos = "Todas las áreas";
            }
            else if (nT == 1)
            {
                strMensaje = "Los datos de la Área actual se van a guardar en disco";
                strActual = "";
                strTodos = "";
            }

            FTransparente.Height = this.Height;
            FTransparente.Width = this.Width;

            OpImprimir.TomaFondo(FTransparente);

            OpImprimir.StrCabecera = strCabecera;
            OpImprimir.StrMensaje = strMensaje;
            OpImprimir.StrActual = strActual;
            OpImprimir.StrOpcionTodos = strTodos;
            OpImprimir.OptSalida = 1;
            OpImprimir.BringIntoView();
            this.OpImprimir.contenedor = this;
            OpImprimir.Visibility = Visibility.Visible;
        }

        private void Imprimir_Click(object sender, RoutedEventArgs e)
        {
            String strCabecera = "Opciones de impresión";
            String strMensaje = "Seleccione lo que se va a imprimir";
            String strActual = "";
            String strTodos = "";
            int nT = -1;
            OpcionesImprimir opMinistros = new OpcionesImprimir();
            opMinistros.Visibility = Visibility.Visible;
            nT = lstUltimoFiltro.Count();

            if (nT > 1)
            {
                strActual = "Área actual";
                strTodos = "Todas las áreas";
            }
            else if (nT == 1)
            {
                strMensaje = "Los datos de la Área actual se van a imprimir";
                strActual = "";
                strTodos = "";
            }

            FTransparente.Visibility = Visibility.Visible;

            OpImprimir.StrCabecera = strCabecera;
            OpImprimir.StrMensaje = strMensaje;
            OpImprimir.StrActual = strActual;
            OpImprimir.StrOpcionTodos = strTodos;
            OpImprimir.OptSalida = 2;
            OpImprimir.BringIntoView();
            this.OpImprimir.contenedor = this;
            OpImprimir.Visibility = Visibility.Visible;

            OpImprimir.TomaFondo(FTransparente); 
        }

        private void PortaPapeles_Click(object sender, RoutedEventArgs e)
        {
            String strCabecera = "Opciones de almacenamiento";
            String strMensaje = "Seleccione lo que se va a enviar al portapapeles ";
            String strActual = "";
            String strTodos = "";
            int nT = -1;
            OpcionesImprimir opMinistros = new OpcionesImprimir();
            opMinistros.Visibility = Visibility.Visible;
            nT = lstUltimoFiltro.Count();

            if (nT > 1)
            {
                strActual = "Área actual";
                strTodos = "Todas las áreas";
            }
            else if (nT == 1)
            {
                strMensaje = "Los datos de la Área actual se van a enviar al portapapeles";
                strActual = "";
                strTodos = "";
            }

            //FTransparente.Visibility = Visibility.Visible;

            //FondoTransparente Ft = new FondoTransparente();
            FTransparente.Height = this.Height;
            FTransparente.Width = this.Width;

            OpImprimir.TomaFondo(FTransparente);

            OpImprimir.StrCabecera = strCabecera;
            OpImprimir.StrMensaje = strMensaje;
            OpImprimir.StrActual = strActual;
            OpImprimir.StrOpcionTodos = strTodos;
            OpImprimir.OptSalida = 0;
            OpImprimir.BringIntoView();

            this.OpImprimir.contenedor = this;
            OpImprimir.Visibility = Visibility.Visible;
        }
    }
}
