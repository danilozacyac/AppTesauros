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

namespace mx.gob.scjn.directorio.CJF
{

    /// <summary>
    /// Interaction logic for CJFFuncionariosAdmin.xaml
    /// </summary>
    public partial class CJFFuncionariosAdmin : Page
    {
        List<String> strArrNC = new List<String>();
        List<DirectorioPersonasTO> lstRes = new List<DirectorioPersonasTO>();
        List<DirectorioPersonasTO> lstUltimoFiltro = new List<DirectorioPersonasTO>();
        String enterKey = "\n";
        /*Para la impresión ***********************************************/

        private FlowDocument DocumentoParaCopiar { get; set; }
        private AcuerdosTO DocumentoActual;
        String strFAImpr = "";
        List<DirectorioPersonasTO> lstResImpr = new List<DirectorioPersonasTO>();
        String strFAdminImpr = "";
        /******************************************************************/

        public CJFFuncionariosAdmin()
        {
            InitializeComponent();
            //lstResImpr = TraeFuncAdmin();
            lstUltimoFiltro = lstResImpr = lstRes = TraeFuncAdmin();

            if (!BrowserInteropHelper.IsBrowserHosted)
            { Guardar_.Visibility = Visibility.Visible; }
            else { Guardar_.Visibility = Visibility.Hidden; }
        }

        public Page Back { get; set; }

        private List<DirectorioPersonasTO> TraeFuncAdmin()
        {
            List<DirectorioPersonasTO> lstRes = new List<DirectorioPersonasTO>();
#if STAND_ALONE
            List<DirectorioPersonasTO> R = null;
            FachadaBusquedaTradicional fachada = new FachadaBusquedaTradicional();
#else
            DirectorioPersonasTO[] R = new DirectorioPersonasTO[50];
            FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
#endif
            R = fachada.getDirPersonasFuncAdmin("CJF");
#if STAND_ALONE
            for (int i = 0; i < R.Count; i++)
#else
            for (int i = 0; i < R.Length; i++)
#endif
            {
                DirectorioPersonasTO Campo = new DirectorioPersonasTO();
                Campo.IdPersona = R[i].IdPersona;
                Campo.NombrePersona = R[i].NombrePersona;
                Campo.NombreStrPersona = R[i].NombreStrPersona;
                Campo.DomPersona = R[i].DomPersona;
                Campo.TelPersona = R[i].TelPersona;
                Campo.ExtPersona = R[i].ExtPersona;
                Campo.TituloPersona = R[i].TituloPersona;
                Campo.CargoPersona = R[i].CargoPersona;
                Campo.AdscripcionPersona = R[i].AdscripcionPersona;
                Campo.Posfijo = R[i].Posfijo;

                lstRes.Add(Campo);
                //vamos a usar esta variable para poder ubicar los nombres en el texto de acceso rapido
                string tmp = R[i].NombreStrPersona;
                tmp = tmp.Replace(" x ", "");
                tmp = tmp.Trim();
                strArrNC.Add(tmp);
                Campo = null;
            }
            this.Funcionarios.ItemsSource = lstRes;
            this.Funcionarios.SelectedIndex = 0;
            this.Funcionarios.SelectedItem = this.Funcionarios.Items.CurrentItem;
            textCuantos.Text = lstRes.Count().ToString();
            CargaDetalleFA((DirectorioPersonasTO)Funcionarios.SelectedItem);
            fachada.Close();
            return lstRes;
        }

        private void CargaDetalle(object sender, MouseButtonEventArgs e)
        {
            DirectorioPersonasTO Campo = new DirectorioPersonasTO();
            Funcionarios.BringItemIntoView(Funcionarios.SelectedItem);
            Campo = (DirectorioPersonasTO)Funcionarios.SelectedItem;
            textNombre.Text = Campo.NombrePersona + enterKey + Campo.Posfijo;
            textCargo.Text = Campo.AdscripcionPersona;
            textDom.Text = Campo.DomPersona;
            textTel.Text = "Teléfono: " + Campo.TelPersona;// +"Ext. " + Campo.ExtPersona;
            strFAImpr = Campo.NombrePersona + enterKey;
            strFAImpr = strFAImpr + Campo.AdscripcionPersona + enterKey;
            strFAImpr = strFAImpr + Campo.DomPersona + enterKey;
            strFAImpr = strFAImpr + "Teléfono: " + Campo.TelPersona + enterKey;
            strFAImpr = strFAImpr + enterKey + enterKey;
            Campo = null;
        }

        private void CargaDetalleFA(DirectorioPersonasTO FA)
        {
            textNombre.Text = FA.NombrePersona ;
            textCargo.Text = FA.AdscripcionPersona;
            textDom.Text = FA.DomPersona;
            textTel.Text = "Teléfono: " + FA.TelPersona;// +"Ext. " + Campo.ExtPersona;
            //textCargo.Text = FA.AdscripcionPersona;
            //textTel.Text = FA.DomPersona + "  " + FA.TelPersona + "  " + FA.ExtPersona;
            strFAImpr = FA.NombrePersona + enterKey;
            strFAImpr = strFAImpr + FA.AdscripcionPersona + enterKey;
            strFAImpr = strFAImpr + FA.DomPersona + enterKey;
            strFAImpr = strFAImpr + "Teléfono: " + FA.TelPersona + enterKey;
            strFAImpr = strFAImpr + enterKey + enterKey;
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

        private void txtAccesoRapido_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            txtAccesoRapido.Text = "";
        }

        private void txtAccesoRapido_TextChanged_old(object sender, TextChangedEventArgs e)
        {
            String StrB = txtAccesoRapido.Text;
            int nPos = -1;
            int nPosAnt = -1;
            Boolean bTextoOk = false;

            if (strArrNC.Count > 0)
            {

                if (StrB.Length > 0)
                {
                    ValidarCadenas strOk = new ValidarCadenas(StrB);
                    bTextoOk = strOk.CadenaOK();
                    StrB = strOk.QuitaCarMalos(StrB);

                    //strOk.QuitaCarMalos(); 
                    if (bTextoOk)
                    {
                        //nPos = BuscaPalInicio(txtAccesoRapido.Text);
                        nPos = BuscaPalInicio(StrB);
                        PointTo(nPos);

                        if (nPos > nPosAnt)
                        {
                            nPosAnt = nPos;
                            cambiaStTxtAR(0);
                        }
                        else
                        {
                            cambiaStTxtAR(2);
                        }
                    }
                    else
                    {
                        cambiaStTxtAR(1);
                    }
                }
                else if (StrB.Length == 0) //si no hay texto (o borró todo), nos vamos al inicio del grid
                {
                    PointTo(0);
                }
            }
        }

        private void txtAccesoRapido_TextChanged(object sender, TextChangedEventArgs e)
        {
            String StrB = txtAccesoRapido.Text;
            Boolean bTextoOk = false;
            Boolean bOk = false;

            if (strArrNC.Count > 0)
            {

                if (StrB.Length > 0)
                {
                    ValidarCadenas strOk = new ValidarCadenas(StrB);
                    bTextoOk = strOk.CadenaOK();

                    if (bTextoOk)
                    {
                        bOk = FiltraFuncionarios2(txtAccesoRapido.Text);

                        if (bOk)
                        {
                            cambiaStTxtAR(0);
                        }
                    }
                    else
                    {
                        cambiaStTxtAR(1);
                    }
                }
                else if (StrB.Length == 0) //si no hay texto (o borró todo), nos vamos al inicio del grid
                {
                    lstUltimoFiltro = lstResImpr = TraeFuncAdmin();
                    cambiaStTxtAR(0);
                }
            }
        }

        private Boolean FiltraFuncionarios2(String strPal)
        {
            Boolean bHayDatos = false;
            char[] delimiterChars = { ' ', ',', '.', ':', '\t' };
            List<DirectorioPersonasTO> lstTmp = new List<DirectorioPersonasTO>();
            lstTmp = lstRes;
            string[] words = strPal.Split(delimiterChars);

            foreach (string strToken in words)
            {

                if (strToken.Length > 0)
                {
                    lstTmp = FiltroXPal(lstTmp, lstTmp, strToken);
                }
            }
            lstResImpr = lstTmp;

            if (lstResImpr.Count > 0)
            {
                this.Funcionarios.ItemsSource = lstResImpr;
                lstUltimoFiltro = lstResImpr;
                lblHayDatos.Visibility = Visibility.Hidden;
                lblHayDatos.Content = "";
                bHayDatos = true;
                txtAccesoRapido.Foreground = Brushes.Black;
            }
            else
            {
                this.Funcionarios.ItemsSource = lstUltimoFiltro;
                cambiaStTxtAR(2);
            }
            this.Funcionarios.SelectedIndex = 0;
            this.Funcionarios.SelectedItem = this.Funcionarios.Items.CurrentItem;
            textCuantos.Text = this.Funcionarios.Items.Count.ToString();
            CargaDetalleFA((DirectorioPersonasTO)Funcionarios.SelectedItem);
            return bHayDatos;
        }

        private List<DirectorioPersonasTO> FiltroXPal(List<DirectorioPersonasTO> Lista, List<DirectorioPersonasTO> ListaUltRes, String strPal)
        {
            string strPalabra = strPal.ToUpper();
            String strLocal = strPalabra;
            ValidarCadenas strOKLocal = new ValidarCadenas(strLocal);
            strLocal = strOKLocal.QuitaCarMalosN(strLocal);
            List<DirectorioPersonasTO> lstFiltro = new List<DirectorioPersonasTO>();

            for (int nPos = 0; nPos <= Lista.Count - 1; nPos++)
            {

                if (Lista[nPos].NombreStrPersona.Contains(strLocal))
                {
                    lstFiltro.Add(Lista[nPos]);
                }
            }
            return lstFiltro;
        }

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

        private int BuscaPalInicio(String strPal)
        {
            int nPos = 0;
            int nRes = -1;
            string strPalabra = strPal.ToUpper();

            for (nPos = 0; nPos < strArrNC.Count; nPos++)
            {
                String strTmp = strArrNC[nPos];

                if (strArrNC[nPos].StartsWith(strPalabra))
                {
                    nRes = nPos;
                    nPos = strArrNC.Count + 1; // nos salimos a la primera ocurrencia, porque queremos que se quede en el primer item que cumpla la condición
                }
            }
            return nRes;
        }

        private void Restablecer_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            PointTo(0);
            txtAccesoRapido.Text = "";
        }

        private void PointTo(int nPos)
        {
            this.Funcionarios.SelectedIndex = nPos;
            Funcionarios.BringItemIntoView(Funcionarios.SelectedItem);

            try
            {
                CargaDetalleFA((DirectorioPersonasTO)Funcionarios.SelectedItem);
            }

            catch { }
        }

        private void txtAccesoRapido_GotFocus(object sender, RoutedEventArgs e)
        {
            txtAccesoRapido.Text = "";
            txtAccesoRapido.Foreground = Brushes.Black;
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
            //    strActual = "El servidor público actual";
            //    strTodos = "Todos los servidores públicos";
            //}
            //else if (nT == 1)
            //{
            //    strMensaje = "Los datos del servidor público actual se van a guardar en disco";
            //    strActual = "";
            //    strTodos = "";
            //}

            //FTransparente.Visibility = Visibility.Visible;
            //OpImprimir.TomaFondo(FTransparente); 

            //OpImprimir.StrCabecera = strCabecera;
            //OpImprimir.StrMensaje = strMensaje;
            //OpImprimir.StrActual = strActual;
            //OpImprimir.StrOpcionTodos = strTodos;
            //OpImprimir.OptSalida = 1;
            //OpImprimir.BringIntoView();
            //this.OpImprimir.contenedor = this;
            //OpImprimir.Visibility = Visibility.Visible;

            ////OpcionesImprimir opMinistros = new OpcionesImprimir();
            ////opMinistros.Visibility = Visibility.Visible;
            ////OpImprimir.StrCabecera = "Opciones de almacenamiento";
            ////OpImprimir.StrMensaje = "Seleccione lo que se va a guardar en disco ";
            ////OpImprimir.StrActual = "Servidor público actual";
            ////OpImprimir.StrOpcionTodos = "Los servidores públicos listados";
            ////OpImprimir.OptSalida = 1;
            ////OpImprimir.BringIntoView();
            ////this.OpImprimir.contenedor = this;
            ////OpImprimir.Visibility = Visibility.Visible;
        }

        //private void Imprimir(object sender, MouseButtonEventArgs e)
        //{
        //}
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
            //    strActual = "El servidor público actual";
            //    strTodos = "Todos los servidores públicos";
            //}
            //else if (nT == 1)
            //{
            //    strMensaje = "Los datos del servidor público actual se van a enviar al portapapeles";
            //    strActual = "";
            //    strTodos = "";
            //}

            //FTransparente.Visibility = Visibility.Visible;
            //OpImprimir.TomaFondo(FTransparente); 

            //OpImprimir.StrCabecera = strCabecera;
            //OpImprimir.StrMensaje = strMensaje;
            //OpImprimir.StrActual = strActual;
            //OpImprimir.StrOpcionTodos = strTodos;
            //OpImprimir.OptSalida = 0;
            //OpImprimir.BringIntoView();
            //this.OpImprimir.contenedor = this;
            //OpImprimir.Visibility = Visibility.Visible;
            ////OpcionesImprimir opMinistros = new OpcionesImprimir();
            ////opMinistros.Visibility = Visibility.Visible;
            ////OpImprimir.StrCabecera = "Opciones de almacenamiento";
            ////OpImprimir.StrMensaje = "Seleccione lo que se va a enviar al portapapeles ";
            ////OpImprimir.StrActual = "Servidor público actual";
            ////OpImprimir.StrOpcionTodos = "Los servidores públicos listados";
            ////OpImprimir.OptSalida = 0;
            ////OpImprimir.BringIntoView();
            ////this.OpImprimir.contenedor = this;
            ////OpImprimir.Visibility = Visibility.Visible;
        }

        private void Imprimir(object sender, MouseButtonEventArgs e)
        {

            //String strCabecera = "Opciones de impresión";
            //String strMensaje = "Seleccione lo que se va a imprimir";
            //String strActual = "";
            //String strTodos = "";
            //int nT = -1;

            //OpcionesImprimir opMinistros = new OpcionesImprimir();
            //opMinistros.Visibility = Visibility.Visible;

            //nT = lstUltimoFiltro.Count();

            //if (nT > 1)
            //{
            //    strCabecera = "Opciones de impresión";
            //    strMensaje = "Seleccione lo que se va a Imprimir ";
            //    strActual = "El servidor público actual";
            //    strTodos = "Todos los servidores públicos";
            //}
            //else if (nT == 1)
            //{
            //    strMensaje = "Los datos del servidor público actual se van a imprimir";
            //    strActual = "";
            //    strTodos = "";
            //}
            //FTransparente.Visibility = Visibility.Visible;
            //OpImprimir.TomaFondo(FTransparente); 


            //OpImprimir.StrCabecera = strCabecera;
            //OpImprimir.StrMensaje = strMensaje;
            //OpImprimir.StrActual = strActual;
            //OpImprimir.StrOpcionTodos = strTodos;
            //OpImprimir.OptSalida = 2;
            //OpImprimir.BringIntoView();
            //this.OpImprimir.contenedor = this;
            //OpImprimir.Visibility = Visibility.Visible;

            //////mx.gob.scjn.directorio.OpcionesImprimir OpImprimir = new OpcionesImprimir();
            //////opMinistros.Visibility = Visibility.Visible;
            ////OpImprimir.StrCabecera = "Opciones de impresión";
            ////OpImprimir.StrMensaje = "Seleccione lo que se va a Imprimir ";
            ////OpImprimir.StrActual = "Servidor público actual";
            ////OpImprimir.StrOpcionTodos = "Los servidores públicos listados";
            ////OpImprimir.OptSalida = 2;
            ////OpImprimir.BringIntoView();
            ////OpImprimir.contenedor = this;
            //////this.OpImprimir.contenedor = this;
            ////OpImprimir.Visibility = Visibility.Visible;
        }

        public void ImprimeListado(Boolean bTodo, Int32 nSelSalida)
        {
            String strText = "";

            if (bTodo == true) //Imprimir todos 
            {

                foreach (DirectorioPersonasTO ItemIntegrante in lstResImpr)
                {
                    strText = strText + ItemIntegrante.NombrePersona + enterKey;
                    strText = strText + ItemIntegrante.AdscripcionPersona + enterKey;
                    strText = strText + ItemIntegrante.DomPersona + enterKey;
                    strText = strText + "Teléfono: " + ItemIntegrante.TelPersona + enterKey;
                    strText = strText + enterKey;
                }
            }
            else   //Imprimir sólo el actual
            {
                strText = strFAImpr + enterKey;
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
                    MessageBox.Show("El archivo fue guardado como: " + archivo.Name,
                        MensajesDirectorio.TITULO_MENSAJES,
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
                //  string errorMsg = "";
                //  errorMsg = "Printer Error:"  + e.ToString();
                ////MessageBox(errorMsg);
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

        private void Imprimir__ImageFailed(object sender, ExceptionRoutedEventArgs e)
        {
        }

        private void btnBuscar_Click(object sender, RoutedEventArgs e)
        {
            //String strCadena = txtCampoBusqueda.Text;
            //Boolean bInicio = (Boolean)optInicio.IsChecked;
            //strCadena = strCadena.ToUpper();
            //if (strCadena.Length > 0)
            //{
            //    lstResImpr = TraeFuncionarios("", bInicio, strCadena);
            //}
            //else{ }
            String strCadena = txtCampoBusqueda.Text;
            String strQry = "";
            Boolean bInicio = (Boolean)optInicio.IsChecked;
            strCadena = strCadena.ToUpper();
            String strLocal = strCadena;
            ValidarCadenas strOKLocal = new ValidarCadenas(strLocal);
            strLocal = strOKLocal.QuitaCarMalosN(strLocal);
            BusquedaDirectorio Busq = new BusquedaDirectorio();
            strQry = Busq.GeneraQuery("__CJF_FuncCJF", strLocal, " [__CJF_FuncCJF].NombreStr");

            if (strCadena.Length > 0)
            {
                lstResImpr = TraeFuncionariosConQuery(strQry, 2);
            }
            else { }
        }

        private List<DirectorioPersonasTO> TraeFuncionarios(string Filtro, Boolean bInicio, String strCadena)
        {
            List<DirectorioPersonasTO> lstRes = new List<DirectorioPersonasTO>();
#if STAND_ALONE
            List<DirectorioPersonasTO> R = null;
            FachadaBusquedaTradicional fachada = new FachadaBusquedaTradicional();
#else
            DirectorioPersonasTO[] R = new DirectorioPersonasTO[50];
            FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
#endif
            String strLocal = strCadena;
            ValidarCadenas strOKLocal = new ValidarCadenas(strLocal);
            strLocal = strOKLocal.QuitaCarMalosN(strLocal);
            R = fachada.getDirPersonasFuncAdminFiltro("CJF", bInicio, strLocal);
#if STAND_ALONE
            int nT = R.Count;
#else
            int nT = R.Length;
#endif
            if (nT > 0)
            {

                for (int i = 0; i < nT; i++)
                {
                    DirectorioPersonasTO Campo = new DirectorioPersonasTO();
                    Campo.IdPersona = R[i].IdPersona;
                    Campo.NombrePersona = R[i].CargoPersona + " " + R[i].NombrePersona;
                    Campo.DomPersona = R[i].DomPersona;
                    Campo.TelPersona = R[i].TelPersona;
                    Campo.ExtPersona = R[i].ExtPersona;
                    Campo.TituloPersona = R[i].TituloPersona;
                    Campo.CargoPersona = R[i].CargoPersona;
                    Campo.AdscripcionPersona = R[i].AdscripcionPersona;
                    lstRes.Add(Campo);
                    //vamos a usar esta variable para poder ubicar los nombres en el texto de acceso rapido
                    string tmp = R[i].NombreStrPersona;
                    tmp = tmp.Replace(" x ", "");
                    tmp = tmp.Trim();
                    strArrNC.Add(tmp);
                    Campo = null;
                }
                this.Funcionarios.ItemsSource = lstRes;
                this.Funcionarios.SelectedIndex = 0;
                this.Funcionarios.SelectedItem = this.Funcionarios.Items.CurrentItem;
                CargaDetalleFA((DirectorioPersonasTO)Funcionarios.SelectedItem);
                textCuantos.Text = nT.ToString();
            }
            else
            {
                textCuantos.Text = "";
                MessageBox.Show("Su búsqueda no obtuvo resultados", MensajesDirectorio.TITULO_MENSAJES);
            }
            fachada.Close();
            return lstRes;
        }

        private List<DirectorioPersonasTO> TraeFuncionariosConQuery(string strQuery, int nTipo)
        {
            List<DirectorioPersonasTO> lstRes = new List<DirectorioPersonasTO>();
#if STAND_ALONE
            List<DirectorioPersonasTO> R = null;
            FachadaBusquedaTradicional fachada = new FachadaBusquedaTradicional();
#else
            DirectorioPersonasTO[] R = new DirectorioPersonasTO[50];
            FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
#endif
            //ValidarCadenas strOKLocal = new ValidarCadenas(strLocal);
            //strLocal = strOKLocal.QuitaCarMalosN(strLocal);
            R = fachada.getDirPersonasConQuery(strQuery, nTipo);
#if STAND_ALONE
            int nT = R.Count;
#else
            int nT = R.Length;
#endif

            if (nT > 0)
            {

                for (int i = 0; i < nT; i++)
                {
                    DirectorioPersonasTO Campo = new DirectorioPersonasTO();
                    Campo.IdPersona = R[i].IdPersona;
                    Campo.NombrePersona = R[i].CargoPersona + " " + R[i].NombrePersona;
                    Campo.DomPersona = R[i].DomPersona;
                    Campo.TelPersona = R[i].TelPersona;
                    Campo.ExtPersona = R[i].ExtPersona;
                    Campo.TituloPersona = R[i].TituloPersona;
                    Campo.CargoPersona = R[i].CargoPersona;
                    Campo.AdscripcionPersona = R[i].AdscripcionPersona;
                    lstRes.Add(Campo);
                    //vamos a usar esta variable para poder ubicar los nombres en el texto de acceso rapido
                    string tmp = R[i].NombreStrPersona;
                    tmp = tmp.Replace(" x ", "");
                    tmp = tmp.Trim();
                    strArrNC.Add(tmp);
                    Campo = null;
                }
                this.Funcionarios.ItemsSource = lstRes;
                this.Funcionarios.SelectedIndex = 0;
                this.Funcionarios.SelectedItem = this.Funcionarios.Items.CurrentItem;
                CargaDetalleFA((DirectorioPersonasTO)Funcionarios.SelectedItem);
                textCuantos.Text = nT.ToString();
            }
            else
            {
                textCuantos.Text = "";
                MessageBox.Show("Su búsqueda no obtuvo resultados", MensajesDirectorio.TITULO_MENSAJES);
            }
            fachada.Close();
            return lstRes;
        }

        private void btnRestablecer_Click(object sender, RoutedEventArgs e)
        {
            lstResImpr = TraeFuncAdmin();
            txtCampoBusqueda.Text = " ";
        }

        private void txtCampoBusqueda_TextChanged(object sender, TextChangedEventArgs e)
        {
            String StrB = txtCampoBusqueda.Text;
            Boolean bTextoOk = false;
            txtCampoBusqueda.Foreground = Brushes.Black;
            btnRestablecer.IsEnabled = true;
            btnBuscar.IsEnabled = true;

            if (StrB.Length > 0)
            {
                ValidarCadenas strOk = new ValidarCadenas(StrB);
                bTextoOk = strOk.CadenaOK();

                if (bTextoOk)
                {

                    if (txtCampoBusqueda.Text.Length > 0) this.txtAccesoRapido.Text = "";
                }
                else
                {
                    txtCampoBusqueda.Foreground = Brushes.Red;
                    btnRestablecer.IsEnabled = false;
                    btnBuscar.IsEnabled = false;
                }
            }
        }

        private void txtCampoBusqueda_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.Key.Equals(Key.Return))
            {
                btnBuscar_Click(sender, e);
            }
            else
            {
                //Nothing.
            }
        }

        private void txtCampoBusqueda_KeyUp(object sender, KeyEventArgs e)
        {
            //if (e.Key.Equals('\n'))
            //  {
            //      btnBuscar_Click(sender, e);
            //  }  
            //  else  
            //  {  
            //  //Nothing.
            //  }  
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
                strCabecera = "Opciones de impresión";
                strMensaje = "Seleccione lo que se va a Imprimir ";
                strActual = "El servidor público actual";
                strTodos = "Todos los servidores públicos";
            }
            else if (nT == 1)
            {
                strMensaje = "Los datos del servidor público actual se van a imprimir";
                strActual = "";
                strTodos = "";
            }
            FTransparente.Visibility = Visibility.Visible;
            OpImprimir.TomaFondo(FTransparente);


            OpImprimir.StrCabecera = strCabecera;
            OpImprimir.StrMensaje = strMensaje;
            OpImprimir.StrActual = strActual;
            OpImprimir.StrOpcionTodos = strTodos;
            OpImprimir.OptSalida = 2;
            OpImprimir.BringIntoView();
            this.OpImprimir.contenedor = this;
            OpImprimir.Visibility = Visibility.Visible;

            ////mx.gob.scjn.directorio.OpcionesImprimir OpImprimir = new OpcionesImprimir();
            ////opMinistros.Visibility = Visibility.Visible;
            //OpImprimir.StrCabecera = "Opciones de impresión";
            //OpImprimir.StrMensaje = "Seleccione lo que se va a Imprimir ";
            //OpImprimir.StrActual = "Servidor público actual";
            //OpImprimir.StrOpcionTodos = "Los servidores públicos listados";
            //OpImprimir.OptSalida = 2;
            //OpImprimir.BringIntoView();
            //OpImprimir.contenedor = this;
            ////this.OpImprimir.contenedor = this;
            //OpImprimir.Visibility = Visibility.Visible;
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
                strActual = "El servidor público actual";
                strTodos = "Todos los servidores públicos";
            }
            else if (nT == 1)
            {
                strMensaje = "Los datos del servidor público actual se van a guardar en disco";
                strActual = "";
                strTodos = "";
            }

            FTransparente.Visibility = Visibility.Visible;
            OpImprimir.TomaFondo(FTransparente);

            OpImprimir.StrCabecera = strCabecera;
            OpImprimir.StrMensaje = strMensaje;
            OpImprimir.StrActual = strActual;
            OpImprimir.StrOpcionTodos = strTodos;
            OpImprimir.OptSalida = 1;
            OpImprimir.BringIntoView();
            this.OpImprimir.contenedor = this;
            OpImprimir.Visibility = Visibility.Visible;

            //OpcionesImprimir opMinistros = new OpcionesImprimir();
            //opMinistros.Visibility = Visibility.Visible;
            //OpImprimir.StrCabecera = "Opciones de almacenamiento";
            //OpImprimir.StrMensaje = "Seleccione lo que se va a guardar en disco ";
            //OpImprimir.StrActual = "Servidor público actual";
            //OpImprimir.StrOpcionTodos = "Los servidores públicos listados";
            //OpImprimir.OptSalida = 1;
            //OpImprimir.BringIntoView();
            //this.OpImprimir.contenedor = this;
            //OpImprimir.Visibility = Visibility.Visible;
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
                strActual = "El servidor público actual";
                strTodos = "Todos los servidores públicos";
            }
            else if (nT == 1)
            {
                strMensaje = "Los datos del servidor público actual se van a enviar al portapapeles";
                strActual = "";
                strTodos = "";
            }

            FTransparente.Visibility = Visibility.Visible;
            OpImprimir.TomaFondo(FTransparente);

            OpImprimir.StrCabecera = strCabecera;
            OpImprimir.StrMensaje = strMensaje;
            OpImprimir.StrActual = strActual;
            OpImprimir.StrOpcionTodos = strTodos;
            OpImprimir.OptSalida = 0;
            OpImprimir.BringIntoView();
            this.OpImprimir.contenedor = this;
            OpImprimir.Visibility = Visibility.Visible;
            //OpcionesImprimir opMinistros = new OpcionesImprimir();
            //opMinistros.Visibility = Visibility.Visible;
            //OpImprimir.StrCabecera = "Opciones de almacenamiento";
            //OpImprimir.StrMensaje = "Seleccione lo que se va a enviar al portapapeles ";
            //OpImprimir.StrActual = "Servidor público actual";
            //OpImprimir.StrOpcionTodos = "Los servidores públicos listados";
            //OpImprimir.OptSalida = 0;
            //OpImprimir.BringIntoView();
            //this.OpImprimir.contenedor = this;
            //OpImprimir.Visibility = Visibility.Visible;
        }
    }
}
