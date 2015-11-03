using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using mx.gob.scjn.ius_common.TO;
using mx.gob.scjn.ius_common.fachade;

namespace mx.gob.scjn.directorio.CJF
{

    public partial class CJFComisiones : Page
    {
        String enterKey = "\n";
        /*Para la impresión ***********************************************/
        private FlowDocument DocumentoParaCopiar { get; set; }
        private AcuerdosTO DocumentoActual;
        Boolean bEsPleno = false;
        Boolean bEsPS = false;
        Boolean bEsSS = false;
        Boolean bEsPres = false;

        public Boolean bImprTodo = false;
        public int nOpImpr = 0; // 0,Papelera; 1, Guardar; 2, Imprimir.
        int nEnQueComboEstoy = -1;
        String strConsejeroParaImpr = "";
        String strConsejeroActual = "";
        String strSecTCPParaImpr = "";
        //String strConsejeroParaImpr = "";
        String strIntImp = ""; // aqui vamos a guardar al integrantede la ponencia seleccionado, por si quieren imprimirlo
        String strComParaImpr = "";
        int nIdComisionActual = -1;
        List<DirectorioPersonasTO> lstIntegrantesComision = new List<DirectorioPersonasTO>();
        List<DirectorioPersonasTO> lstIntegrantesPonencia = new List<DirectorioPersonasTO>();
        List<DirectorioPersonasTO> lstResSTCP = new List<DirectorioPersonasTO>();
        List<DirectorioOrgJurTO> lstResComisiones = new List<DirectorioOrgJurTO>();
        /*********************************************************************/

        public Page Back { get; set; }
        public CJFComisiones()
        {
            InitializeComponent();
            lstResComisiones = LlenaComboComisiones();

            if (!BrowserInteropHelper.IsBrowserHosted)
            { Guardar_.Visibility = Visibility.Visible; }
            else { Guardar_.Visibility = Visibility.Hidden; }
        }

        private List<DirectorioOrgJurTO> LlenaComboComisiones()
        {
            List<DirectorioOrgJurTO> lstRes = new List<DirectorioOrgJurTO>();

            try
            {
#if STAND_ALONE
                List<DirectorioOrgJurTO> R = null;
                FachadaBusquedaTradicional fachada = new FachadaBusquedaTradicional();
#else
                DirectorioOrgJurTO[] R = new DirectorioOrgJurTO[50];
                FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
#endif
                R = fachada.getDirComisiones();
#if STAND_ALONE
                for (int i = 0; i < R.Count; i++)
#else
                for (int i = 0; i < R.Length; i++)
#endif
                {
                    DirectorioOrgJurTO Campo = new DirectorioOrgJurTO();
                    Campo.NombreOrganoJur = R[i].NombreOrganoJur;
                    Campo.DomOrganoJur = R[i].DomOrganoJur;
                    Campo.IdOrganoJur = R[i].IdOrganoJur;
                    lstRes.Add(Campo);
                    Campo = null;
                }
                this.comboBoxComisiones.ItemsSource = lstRes;
                this.comboBoxComisiones.SelectedIndex = 0;
                this.comboBoxComisiones.SelectedItem = this.comboBoxComisiones.Items.CurrentItem;
                fachada.Close();
            }

            catch (System.Exception error)
            {
                //Handle exception here
            }
            return lstRes;
        }

        private void CargaDetalleSGA(object sender, MouseButtonEventArgs e)
        {
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

        private void CargaDetalleSTCP(object sender, MouseButtonEventArgs e)
        {
            nEnQueComboEstoy = 3;
            LimpiaAreaDetalle();

            try
            {
                DirectorioPersonasTO Campo = new DirectorioPersonasTO();
                grdSTCP.BringItemIntoView(grdSTCP.SelectedItem);
                Campo = (DirectorioPersonasTO)grdSTCP.SelectedItem;
                //textInstancia.Text = Campo.TituloPersona + "  " + Campo.NombrePersona;
                textCargo.Text = Campo.CargoPersona;
                textDomicilio.Text = Campo.DomPersona;
                textTel.Text = "Teléfono " + Campo.TelPersona + "  Ext. " + Campo.ExtPersona;
                Campo = null;
            }

            catch
            {
            }
        }

        private void CargaDetalleIntPonente(object sender, MouseButtonEventArgs e)
        {
            nEnQueComboEstoy = 4;

            try
            {
                LimpiaAreaDetalle();
                DirectorioPersonasTO Campo = new DirectorioPersonasTO();
                grdPonencia.BringItemIntoView(grdPonencia.SelectedItem);
                Campo = (DirectorioPersonasTO)grdPonencia.SelectedItem;
                //textInstancia.Text = Campo.TituloPersona + "  " + Campo.NombrePersona;
                textCargo.Text = Campo.CargoPersona;
                textDomicilio.Text = Campo.DomPersona;
                textTel.Text = "Teléfono " + Campo.TelPersona + "  Ext. " + Campo.ExtPersona;
                strIntImp = Campo.CargoPersona + enterKey;
                //strIntImp = strIntImp + Campo.TituloPersona + "  " + Campo.NombrePersona + enterKey;
                strIntImp = strIntImp + Campo.NombrePersona + enterKey;
                strIntImp = strIntImp + Campo.DomPersona + enterKey;
                strIntImp = strIntImp + Campo.TelPersona + "  Ext. " + Campo.ExtPersona + enterKey;
                strIntImp = strIntImp + enterKey;
                Campo = null;
            }

            catch (System.Exception error)
            {
                //Handle exception here
            }
        }

        private void comboBoxComisiones_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //this.Cursor = Cursors.Wait; 
            
            nEnQueComboEstoy = 1;

            try
            {
                int Pos = this.comboBoxComisiones.SelectedIndex;
                LimpiaAreaDetalle();

                if (Pos > -1)
                {
                    DirectorioOrgJurTO seleccionado = (DirectorioOrgJurTO)this.comboBoxComisiones.SelectedItem;
                    int Sel = seleccionado.IdOrganoJur;
                    string strNSel = seleccionado.NombreOrganoJur;
                    lstIntegrantesComision = TraeConsCom(Sel, strNSel);
                    lstResSTCP = TraeSTCP(Sel);
                    textInstancia.Text = seleccionado.NombreOrganoJur;
                    strComParaImpr = seleccionado.NombreOrganoJur;
                    nIdComisionActual = seleccionado.IdOrganoJur;
                    this.Consejeros.ItemsSource = lstIntegrantesComision;
                    this.Consejeros.SelectedIndex = 0;
                    this.Consejeros.SelectedItem = this.Consejeros.Items.CurrentItem;
                }
            }

            

            catch (System.Exception error)
            {
                //Handle exception here
            }

            //this.Cursor = Cursors.Arrow; 

        }

        private void LimpiaAreaDetalle()
        {
            //textInstancia.Text = ""; //Este no lo limpiamos, porque siempre es el mismo. si cambiamos de comisión, el combo lo limpia.
            textCargo.Text = "";
            textDomicilio.Text = "";
            textTel.Text = "";
        }

        private List<DirectorioPersonasTO> TraeConsCom(int IdCom, string strNSel)
        {
            List<DirectorioPersonasTO> lstRes = new List<DirectorioPersonasTO>();

            try
            {
                int Pos = this.comboBoxComisiones.SelectedIndex;
#if STAND_ALONE
                List<DirectorioPersonasTO> R = null;
                FachadaBusquedaTradicional fachada = new FachadaBusquedaTradicional();
#else
                DirectorioPersonasTO[] R = new DirectorioPersonasTO[50];
                FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
#endif
                R = fachada.getDirConsejerosIntComisiones(IdCom);
#if STAND_ALONE
                for (int i = 0; i < R.Count; i++)
#else
                for (int i = 0; i < R.Length; i++)
#endif
                {
                    DirectorioPersonasTO Campo = new DirectorioPersonasTO();
                    Campo.IdPersona = R[i].IdPersona;
                    Campo.NombrePersona = R[i].TituloPersona + " " + R[i].NombrePersona;
                    Campo.DomPersona = R[i].DomPersona;
                    Campo.TelPersona = R[i].TelPersona;
                    Campo.ExtPersona = R[i].ExtPersona;
                    Campo.TituloPersona = R[i].TituloPersona;
                    Campo.CargoPersona = R[i].CargoPersona;
                    Campo.AdscripcionPersona = strNSel;
                    lstRes.Add(Campo);
                    Campo = null;
                }
                this.Consejeros.SelectionMode = SelectionMode.Single;
                this.Consejeros.ItemsSource = lstRes;
                this.Consejeros.SelectedIndex = 0;
                this.Consejeros.SelectedItem = this.Consejeros.Items.CurrentItem;
                CargaDetalleConsejero((DirectorioPersonasTO)Consejeros.SelectedItem);
                //CargaDetalle();
                fachada.Close();
            }

            catch (System.Exception error)
            {
                //Handle exception here
            }
            return lstRes;
        }

        private List<DirectorioPersonasTO> TraeConsejerosComision(int IdCom)
        {
            List<DirectorioPersonasTO> lstRes = new List<DirectorioPersonasTO>();

            try
            {
                int Pos = this.comboBoxComisiones.SelectedIndex;
#if STAND_ALONE
                List<DirectorioPersonasTO> R = null;
                FachadaBusquedaTradicional fachada = new FachadaBusquedaTradicional();
#else
                DirectorioPersonasTO[] R = new DirectorioPersonasTO[50];
                FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
#endif
                R = fachada.getDirConsejerosIntComisiones(IdCom);
#if STAND_ALONE
                for (int i = 0; i < R.Count; i++)
#else
                for (int i = 0; i < R.Length; i++)
#endif
                {
                    DirectorioPersonasTO Campo = new DirectorioPersonasTO();
                    Campo.IdPersona = R[i].IdPersona;
                    Campo.NombrePersona = R[i].TituloPersona + " " + R[i].NombrePersona;
                    Campo.DomPersona = R[i].DomPersona;
                    Campo.TelPersona = R[i].TelPersona;
                    Campo.ExtPersona = R[i].ExtPersona;
                    Campo.TituloPersona = R[i].TituloPersona;
                    Campo.CargoPersona = R[i].CargoPersona;
                    lstRes.Add(Campo);
                    Campo = null;
                }
                fachada.Close();
            }

            catch (System.Exception error) { }
            return lstRes;
        }

        private List<DirectorioPersonasTO> TraeSTCP(int IdCom)
        {
            List<DirectorioPersonasTO> lstRes = new List<DirectorioPersonasTO>();

            try
            {
                int Pos = this.comboBoxComisiones.SelectedIndex;
#if STAND_ALONE
                List<DirectorioPersonasTO> R = null;
                FachadaBusquedaTradicional fachada = new FachadaBusquedaTradicional();
#else
                DirectorioPersonasTO[] R = new DirectorioPersonasTO[50];
                FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
#endif
                R = fachada.getDirSTCPComision(IdCom);
#if STAND_ALONE
                for (int i = 0; i < R.Count; i++)
#else
                for (int i = 0; i < R.Length; i++)
#endif
                {
                    DirectorioPersonasTO Campo = new DirectorioPersonasTO();
                    Campo.IdPersona = R[i].IdPersona;
                    Campo.NombrePersona = R[i].TituloPersona + " " + R[i].NombrePersona + " " + R[i].ApellidosPersona;
                    Campo.DomPersona = R[i].DomPersona;
                    Campo.TelPersona = R[i].TelPersona;
                    Campo.ExtPersona = R[i].ExtPersona;
                    Campo.TituloPersona = R[i].TituloPersona;
                    Campo.CargoPersona = R[i].CargoPersona;
                    lstRes.Add(Campo);
                    strSecTCPParaImpr = Campo.CargoPersona + enterKey;
                    strSecTCPParaImpr = strSecTCPParaImpr + Campo.NombrePersona + enterKey;
                    strSecTCPParaImpr = strSecTCPParaImpr + Campo.DomPersona + enterKey;
                    strSecTCPParaImpr = strSecTCPParaImpr + "Teléfono: " + Campo.TelPersona + enterKey;
                    strSecTCPParaImpr = strSecTCPParaImpr + "Extensión: " + Campo.ExtPersona + enterKey;
                    Campo = null;
                }
                this.grdSTCP.ItemsSource = lstRes;
                fachada.Close();
            }

            catch (System.Exception error)
            {
                //Handle exception here
            }
            return lstRes;
        }

        private void CargaDetalleConsejero()
        {
            string strExt="";
            string strExts = "";
            string[] arrExt;// = strExt.Split(new Char[] { ' ', ',' });

            try
            {
                DirectorioPersonasTO Campo = new DirectorioPersonasTO();
                Consejeros.BringItemIntoView(Consejeros.SelectedItem);
                Campo = (DirectorioPersonasTO)Consejeros.SelectedItem;
                textInstancia.Text = Campo.AdscripcionPersona;
                textCargo.Text = Campo.CargoPersona;
                textDomicilio.Text = Campo.DomPersona;
                textTel.Text = "Teléfono " + Campo.TelPersona;
                
                strExt = Campo.ExtPersona;
                arrExt = strExt.Split(new Char[] {' ', ','});
                
                if (arrExt.Length > 1)
                {
                    strExts = "  Exts. ";
                    for (int i = 0; i < arrExt.Length; i++)  strExts += arrExt[i];
                }
                else
                {
                    strExts = "  Ext. " + Campo.ExtPersona;
                }
                textTel.Text += strExts;
                
                strConsejeroParaImpr = Campo.CargoPersona + " " + Campo.NombrePersona + enterKey;
                strConsejeroParaImpr = strConsejeroParaImpr + Campo.AdscripcionPersona + enterKey;
                strConsejeroParaImpr = strConsejeroParaImpr + Campo.DomPersona + enterKey;
                strConsejeroParaImpr = strConsejeroParaImpr + "Teléfono " + Campo.TelPersona + "  Ext. " + Campo.ExtPersona + enterKey;
                strConsejeroParaImpr = strConsejeroParaImpr + enterKey;
                strConsejeroActual = Campo.CargoPersona + " " + Campo.NombrePersona;  //esta vriable la usamos para imprimir solo el nombre
                Campo = null;
            }

            catch (System.Exception error)
            {
                //Handle exception here
            }
        }

        private void CargaDetalleConsejero(DirectorioPersonasTO Campo)
        {
            string strExt = "";
            string strExts = "";
            string[] arrExt;// = strExt.Split(new Char[] { ' ', ',' });

            try
            {
                textInstancia.Text = Campo.AdscripcionPersona;
                textCargo.Text = Campo.CargoPersona;
                textDomicilio.Text = Campo.DomPersona;
                textTel.Text = "Teléfono " + Campo.TelPersona;
                  
                strExt = Campo.ExtPersona;
                arrExt = strExt.Split(new Char[] { ' ', ',' });
                // para diferenciar entre una o varias extensiones
                if (arrExt.Length > 1) textTel.Text += "  Exts. " + strExt;
                else textTel.Text += "  Ext. " + strExt;
                
                strConsejeroParaImpr = Campo.CargoPersona + " " + Campo.NombrePersona + enterKey;
                //strConsejeroParaImpr = strConsejeroParaImpr + Campo.AdscripcionPersona + enterKey;
                strConsejeroParaImpr = strConsejeroParaImpr + Campo.DomPersona + enterKey;
                strConsejeroParaImpr = strConsejeroParaImpr + "Teléfono " + Campo.TelPersona + "  Ext. " + Campo.ExtPersona + enterKey;
                strConsejeroParaImpr = strConsejeroParaImpr + enterKey;
                lstIntegrantesPonencia = TraeIntegrantesPonenciaCons(Campo.IdPersona);
                strConsejeroActual = Campo.CargoPersona + " " + Campo.NombrePersona;  //esta vriable la usamos para imprimir solo el nombre
            }

            catch (System.Exception error)
            {
                //Handle exception here
            }
        }

        private List<DirectorioPersonasTO> TraeIntegrantesPonenciaCons(int Sel)
        {
            List<DirectorioPersonasTO> lstRes = new List<DirectorioPersonasTO>();

            try
            {
                // estamos validando cuando el selecteditem es -1
#if STAND_ALONE
                List<DirectorioPersonasTO> R = null;
                FachadaBusquedaTradicional fachada = new FachadaBusquedaTradicional();
#else
                DirectorioPersonasTO[] R = new DirectorioPersonasTO[50];
                FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
#endif
                R = fachada.getDirCJFIntPonencias(Sel);
#if STAND_ALONE
                for (int i = 0; i < R.Count; i++)
#else
                for (int i = 0; i < R.Length; i++)
#endif
                {
                    DirectorioPersonasTO Campo = new DirectorioPersonasTO();
                    Campo.IdPersona = R[i].IdPersona;
                    Campo.NombrePersona = R[i].TituloPersona + " " + R[i].NombrePersona;
                    Campo.DomPersona = R[i].DomPersona;
                    Campo.TelPersona = R[i].TelPersona;
                    Campo.ExtPersona = R[i].ExtPersona;
                    Campo.TituloPersona = R[i].TituloPersona;
                    Campo.CargoPersona = R[i].CargoPersona;
                    lstRes.Add(Campo);
                    Campo = null;
                }
                this.grdPonencia.ItemsSource = lstRes;
                this.grdPonencia.SelectedIndex = 0;
                //this.grdPonencia.SelectedItem = this.grdPonencia.Items.CurrentItem;
                fachada.Close();
            }

            catch (System.Exception error)
            {
                //Handle exception here
            }
            return lstRes;
        }

        private List<DirectorioPersonasTO> TraeIntegrantesPonenciaConsejero(int idConsejero)
        {
            List<DirectorioPersonasTO> lstRes = new List<DirectorioPersonasTO>();

            try
            {
#if STAND_ALONE
                List<DirectorioPersonasTO> R = null;
                FachadaBusquedaTradicional fachada = new FachadaBusquedaTradicional();
#else
                DirectorioPersonasTO[] R = new DirectorioPersonasTO[50];
                FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
#endif
                R = fachada.getDirCJFIntPonencias(idConsejero);
#if STAND_ALONE
                for (int i = 0; i < R.Count; i++)
#else
                for (int i = 0; i < R.Length; i++)
#endif
                {
                    DirectorioPersonasTO Campo = new DirectorioPersonasTO();
                    Campo.IdPersona = R[i].IdPersona;
                    Campo.NombrePersona = R[i].TituloPersona + " " + R[i].NombrePersona;
                    Campo.DomPersona = R[i].DomPersona;
                    Campo.TelPersona = R[i].TelPersona;
                    Campo.ExtPersona = R[i].ExtPersona;
                    Campo.TituloPersona = R[i].TituloPersona;
                    Campo.CargoPersona = R[i].CargoPersona;
                    lstRes.Add(Campo);
                    Campo = null;
                }
                fachada.Close();
            }

            catch (System.Exception error) { }
            return lstRes;
        }

        private void CargaDetalle(object sender, MouseButtonEventArgs e)
        {
            nEnQueComboEstoy = 2;
            //CargaDetalleConsejero((DirectorioPersonasTO)Consejeros.SelectedItem);
            //TraeIntegrantesPonenciaCons(Consejeros.SelectedItem);
            CargaDetalle();
        }

        private void CargaDetalle()
        {

            try
            {
                LimpiaAreaDetalle();
                int Pos = this.Consejeros.SelectedIndex;

                // estamos validando cuando el selecteditem es -1
                if (Pos >= 0)
                {
                    DirectorioPersonasTO seleccionado = (DirectorioPersonasTO)this.Consejeros.SelectedItem;
                    int Sel = seleccionado.IdPersona;
                    lstIntegrantesPonencia = TraeIntegrantesPonenciaCons(Sel);
                    CargaDetalleConsejero((DirectorioPersonasTO)Consejeros.SelectedItem);
                }
            }

            catch (System.Exception error)
            {
                //Handle exception here
            }
        }

        private void Consejeros_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            CargaDetalle();
            CargaDetalleConsejero();
        }

        private void Consejeros_GotMouseCapture(object sender, MouseEventArgs e)
        {
            //Consejeros_MouseLeftButtonDown(sender, e);
            CargaDetalleConsejero();
        }

        private void Consejeros_MouseUp(object sender, MouseButtonEventArgs e)
        {
            nEnQueComboEstoy = 2;
            CargaDetalle();
            CargaDetalleConsejero();
        }

        private void Guardar(object sender, MouseButtonEventArgs e)
        {
            ////OpcionesImprimir opConsejeros = new OpcionesImprimir();
            ////opConsejeros.Visibility = Visibility.Visible;
            ////OpImprimir.StrCabecera = "Opciones de almacenamiento ";
            ////OpImprimir.StrMensaje = "Seleccione qué es lo que se va a Guardar en archivo";
            ////OpImprimir.OptSalida = 1;
            ////OpImprimir.BringIntoView();
            ////this.OpImprimir.contenedor = this;
            ////OpImprimir.Visibility = Visibility.Visible;
            //String strCabecera = "Opciones de almacenamiento";
            //String strMensaje = "Seleccione lo que se va a guardar en archivo ";
            //String strActual = "";
            //String strTodos = "";

            //switch (nEnQueComboEstoy)
            //{

            //    case 0://Pleno
            //        strCabecera = "";
            //        strMensaje = "";
            //        strActual = "";
            //        strTodos = "";
            //        break;

            //    case 1: //Las comisiones
            //        strActual = "La comisión actual";
            //        strTodos = "Todas las comisiones";
            //        break;

            //    case 2: //Consejeros integrantes de la comisión
            //        ////strActual = "El consejero seleccionado";
            //        ////strTodos = "Los consejeros integrantes";
            //        //strActual = "El integrante seleccionado";
            //        //strTodos = "Los integrantes de la comisión";
            //        strActual = "El integrante seleccionado";
            //        strTodos = "Todos los integrantes";
            //        break;

            //    case 3: //El Sec. Tec. de Com. Permanente
            //        strMensaje = "Los datos del Secretario Técnico de Comisión Permanente se van a guardar en archivo";
            //        strActual = "";
            //        strTodos = "";
            //        break;

            //    case 4: //Los integrantes de la ponencia del Consejero
            //        //strActual = "El integrante seleccionado";
            //        //strTodos = "Todos los integrantes";
            //        strActual = "El integrante de la ponencia seleccionado";
            //        strTodos = "Todos los integrantes de la ponencia";
            //        break;

            //    case 5: //Secretario de Acuerdos
            //        strActual = "";
            //        strTodos = "";
            //        break;

            //    case 6://Sec tesis
            //        strActual = "";
            //        strTodos = "";
            //        break;
            //}
            //OpcionesImprimir opConsejeros = new OpcionesImprimir();
            //opConsejeros.Visibility = Visibility.Visible;
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


        }

        private void Imprimir(object sender, MouseButtonEventArgs e)
        {
            //String strCabecera = "Opciones de impresión";
            //String strMensaje = "Seleccione lo que se va a Imprimir ";
            //String strActual = "";
            //String strTodos = "";

            //switch (nEnQueComboEstoy)
            //{

            //    case 0://Pleno
            //        strCabecera = "";
            //        strMensaje = "";
            //        strActual = "";
            //        strTodos = "";
            //        break;

            //    case 1: //Las comisiones
            //        strActual = "La comisión actual";
            //        strTodos = "Todas las comisiones";
            //        break;

            //    case 2: //Consejeros integrantes de la comisión
            //        ////strActual = "El consejero seleccionado";
            //        ////strTodos = "Los consejeros integrantes";
            //        //strActual = "El integrante seleccionado";
            //        //strTodos = "Los integrantes de la comisión";
            //        strActual = "El integrante seleccionado";
            //        strTodos = "Todos los integrantes";
            //        break;

            //    case 3: //El Sec. Tec. de Com. Permanente
            //        strMensaje = "Los datos del Secretario Técnico de Comisión Permanente se van a imprimir";
            //        strActual = "";
            //        strTodos = "";
            //        break;

            //    case 4: //Los integrantes de la ponencia del Consejero
            //        //strActual = "El integrante seleccionado";
            //        //strTodos = "Todos los integrantes";
            //        strActual = "El integrante de la ponencia seleccionado";
            //        strTodos = "Todos los integrantes de la ponencia";
            //        break;

            //    case 5: //Secretario de Acuerdos
            //        strActual = "";
            //        strTodos = "";
            //        break;

            //    case 6://Sec tesis
            //        strActual = "";
            //        strTodos = "";
            //        break;
            //}

            //FTransparente.Visibility = Visibility.Visible;
            //OpImprimir.TomaFondo(FTransparente);

            //OpcionesImprimir opConsejeros = new OpcionesImprimir();
            //opConsejeros.Visibility = Visibility.Visible;
            //OpImprimir.StrCabecera = strCabecera;
            //OpImprimir.StrMensaje = strMensaje;
            //OpImprimir.StrActual = strActual;
            //OpImprimir.StrOpcionTodos = strTodos;
            //OpImprimir.OptSalida = 2;
            //OpImprimir.BringIntoView();
            //this.OpImprimir.contenedor = this;
            //OpImprimir.Visibility = Visibility.Visible;


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

        private void PortaPapeles(object sender, MouseButtonEventArgs e)
        {
            //String strCabecera = "Opciones de almacenamiento";
            //String strMensaje = "Seleccione lo que se va a enviar al portapapeles ";
            //String strActual = "";
            //String strTodos = "";

            //FondoTransparente FT = new FondoTransparente();

            //switch (nEnQueComboEstoy)
            //{

            //    case 0://Pleno
            //        strCabecera = "";
            //        strMensaje = "";
            //        strActual = "";
            //        strTodos = "";
            //        break;

            //    case 1: //Las comisiones
            //        strActual = "La comisión actual";
            //        strTodos = "Todas las comisiones";
            //        break;

            //    case 2: //Consejeros integrantes de la comisión
            //        //strActual = "El consejero seleccionado";
            //        //strTodos = "Los consejeros integrantes";
            //        strActual = "El integrante seleccionado";
            //        strTodos = "Todos los integrantes";
            //        break;

            //    case 3: //El Sec. Tec. de Com. Permanente
            //        strMensaje = "Los datos del Secretario Técnico de Comisión Permanente se van a enviar al portapapeles ";
            //        strActual = "";
            //        strTodos = "";
            //        break;

            //    case 4: //Los integrantes de la ponencia del Consejero
            //        strActual = "El integrante de la ponencia seleccionado";
            //        strTodos = "Todos los integrantes de la ponencia";
            //        break;

            //    case 5: //Secretario de Acuerdos
            //        strActual = "";
            //        strTodos = "";
            //        break;

            //    case 6://Sec tesis
            //        strActual = "";
            //        strTodos = "";
            //        break;
            //}

            //FTransparente.Width = this.Width;
            //FTransparente.Height = this.Height;
            //FTransparente.Visibility = Visibility.Visible;
            //OpImprimir.TomaFondo(FTransparente);

            //OpcionesImprimir opMinistros = new OpcionesImprimir();
            //opMinistros.Visibility = Visibility.Visible;
            //OpImprimir.StrCabecera = strCabecera;// "Opciones de almacenamiento";
            //OpImprimir.StrMensaje = strMensaje; //"Seleccione qué es lo que se va a enviar al portapapeles ";
            //OpImprimir.StrActual = strActual;// "Comisión actual";
            //OpImprimir.StrOpcionTodos = strTodos;//"Todas las áreas";
            //OpImprimir.OptSalida = 0;
            //OpImprimir.BringIntoView();
            //this.OpImprimir.contenedor = this;
            //OpImprimir.Visibility = Visibility.Visible;

        }

        private void Imprimir_Test(String strTexto)
        {

            DocumentoAcuerdoDirec documento;

            documento = new DocumentoAcuerdoDirec(strTexto);
            //FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
            //object documentoXps = fachada.getDocumentoTesis(this.DocumentoActual.Ius);
            impresion.Document = documento.Documento; //(IDocumentPaginatorSource)documentoXps;

            //fachada.Close();
            DocumentoParaCopiar = documento.Copia;
            impresion.Visibility = Visibility.Hidden;
            impresion.Background = Brushes.White;
        }

        private void Imprimir__ImageFailed(object sender, ExceptionRoutedEventArgs e)
        {
        }

        public void ImprimePonencia(Boolean bTodos, int nIdMin, Int32 nSelSalida)
        {
            String strText = "";
            List<DirectorioPersonasTO> lstCons = new List<DirectorioPersonasTO>();
            List<DirectorioPersonasTO> lstIntPonencia = new List<DirectorioPersonasTO>();
            List<DirectorioPersonasTO> lstSTCP = new List<DirectorioPersonasTO>();

            switch (nEnQueComboEstoy)
            {

                case 1: //Las comisiones
                    strText = strComParaImpr + enterKey;

                    if (bTodos) //Imprimir Todos
                    {
                        strText = enterKey; // quitamos a la comisión actual para que se imprima en su lugar

                        foreach (DirectorioOrgJurTO ItemComision in lstResComisiones)
                        {
                            strText = strText + ItemComision.NombreOrganoJur + enterKey;
                            //strText = strText + ItemIntegrante.DomOrganoJur + enterKey;
                            //strText = strText + "Extensión: " + ItemIntegrante.ExtOrganoJur  + enterKey;
                            strText = strText + enterKey;
                            lstCons = TraeConsejerosComision(ItemComision.IdOrganoJur);

                            foreach (DirectorioPersonasTO ItemCons in lstCons)
                            {
                                strText = strText + ItemCons.CargoPersona + " ";
                                strText = strText + ItemCons.NombrePersona + enterKey;
                                strText = strText + "  " + ItemCons.DomPersona + enterKey;
                                strText = strText + "  Teléfono: " + ItemCons.TelPersona;
                                strText = strText + "    Extensión: " + ItemCons.ExtPersona + enterKey;
                                strText = strText + enterKey;
                                lstIntPonencia = TraeIntegrantesPonenciaConsejero(ItemCons.IdPersona);

                                foreach (DirectorioPersonasTO ItemIP in lstIntPonencia)
                                {
                                    strText = strText + "  " + ItemIP.NombrePersona + enterKey;
                                    strText = strText + "  " + ItemIP.CargoPersona + enterKey;
                                    strText = strText + "  " + ItemIP.DomPersona + enterKey;
                                    strText = strText + "  Teléfono: " + ItemIP.TelPersona;
                                    strText = strText + " Extensión: " + ItemIP.ExtPersona + enterKey;
                                    strText = strText + enterKey;
                                }
                                strText = strText + enterKey;
                            }
                            lstSTCP = TraeSTCP(ItemComision.IdOrganoJur);

                            foreach (DirectorioPersonasTO ItemSTCP in lstSTCP)
                            {
                                strText = strText + ItemSTCP.NombrePersona + enterKey;
                                strText = strText + ItemSTCP.CargoPersona + enterKey;
                                strText = strText + ItemSTCP.DomPersona + enterKey;
                                strText = strText + ItemSTCP.TelPersona + enterKey;
                                strText = strText + ItemSTCP.ExtPersona + enterKey;
                            }
                            strText = strText + enterKey + enterKey;
                        }
                    }
                    else  //Imprimir la com. actual
                    {
                        strText = strText + enterKey;
                        //lstCons = TraeConsejerosComision(nIdComisionActual);
                        lstCons = TraeConsejerosComision(nIdComisionActual);

                        foreach (DirectorioPersonasTO ItemCons in lstCons)
                        {
                            strText = strText + ItemCons.CargoPersona + " ";
                            strText = strText + ItemCons.NombrePersona + enterKey;
                            strText = strText + "  " + ItemCons.DomPersona + enterKey;
                            strText = strText + "  Teléfono: " + ItemCons.TelPersona;
                            strText = strText + "    Extensión: " + ItemCons.ExtPersona + enterKey;
                            strText = strText + enterKey + enterKey;
                            lstIntPonencia = TraeIntegrantesPonenciaConsejero(ItemCons.IdPersona);

                            foreach (DirectorioPersonasTO ItemIP in lstIntPonencia)
                            {
                                strText = strText + "  " + ItemIP.NombrePersona + enterKey;
                                strText = strText + "  " + ItemIP.CargoPersona + enterKey;
                                strText = strText + "  " + ItemIP.DomPersona + enterKey;
                                strText = strText + "  Teléfono: " + ItemIP.TelPersona;
                                strText = strText + " Extensión: " + ItemIP.ExtPersona + enterKey;
                                strText = strText + enterKey;
                            }
                            strText = strText + enterKey;
                        }
                        lstSTCP = TraeSTCP(nIdComisionActual);

                        foreach (DirectorioPersonasTO ItemSTCP in lstSTCP)
                        {
                            strText = strText + ItemSTCP.NombrePersona + enterKey;
                            strText = strText + ItemSTCP.CargoPersona + enterKey;
                            strText = strText + ItemSTCP.DomPersona + enterKey;
                            strText = strText + "  Teléfono: " + ItemSTCP.TelPersona + enterKey;
                            strText = strText + " Extensión: " + ItemSTCP.ExtPersona + enterKey;
                        }
                        strText = strText + enterKey + enterKey;
                    }
                    break;

                case 2: //Consejeros integrantes de la comisión

                    if (bTodos) //Imprimir Todos
                    {
                        strText = strComParaImpr + enterKey + enterKey + enterKey;

                        foreach (DirectorioPersonasTO ItemIntegrante in lstIntegrantesComision)
                        {
                            strText = strText + ItemIntegrante.NombrePersona + enterKey;
                            strText = strText + ItemIntegrante.DomPersona + enterKey;
                            strText = strText + "Teléfono: " + ItemIntegrante.TelPersona + enterKey;
                            strText = strText + "Extensión: " + ItemIntegrante.ExtPersona + enterKey;
                            strText = strText + enterKey;
                        }
                        // Que siempre no se van a imprimir los STCP
                        //strText = strText + enterKey + enterKey + strSecTCPParaImpr + enterKey;
                    }
                    else  //Imprimir Consejero sel.
                    {
                        strText = strComParaImpr + enterKey + enterKey;
                        strText = strText + strConsejeroParaImpr + enterKey;
                        //foreach (DirectorioPersonasTO ItemIntegrante in lstIntegrantesPonencia)
                        //{
                        //    strText = strText + ItemIntegrante.CargoPersona + enterKey + ItemIntegrante.NombrePersona + enterKey;
                        //    strText = strText + ItemIntegrante.DomPersona + enterKey;
                        //    strText = strText + "Teléfono: " + ItemIntegrante.TelPersona + enterKey;
                        //    strText = strText + "Extensión: " + ItemIntegrante.ExtPersona + enterKey;
                        //    strText = strText + enterKey;
                        //}
                    }
                    break;

                case 3: //El Sec. Tec. de Com. Permanente
                    strText = strComParaImpr + enterKey + enterKey;
                    strText = strText + strSecTCPParaImpr + enterKey;
                    strText = strText + enterKey;
                    break;

                case 4: //Los integrantes de la ponencia del Consejero
                    strText = strComParaImpr + enterKey + enterKey;
                    strText = strText + strConsejeroActual + enterKey + enterKey + enterKey;

                    if (bTodos == false) //Imprimir sólo el actual
                    {
                        strText = strText + strIntImp + enterKey;
                    }
                    else  //Imprimir todos
                    {

                        //strText = strText + strConsejeroParaImpr + enterKey + enterKey;
                        foreach (DirectorioPersonasTO ItemIntegrante in lstIntegrantesPonencia)
                        {
                            strText = strText + ItemIntegrante.CargoPersona + enterKey;
                            strText = strText + ItemIntegrante.NombrePersona + enterKey;
                            strText = strText + "Teléfono: " + ItemIntegrante.TelPersona + " Extensión: " + ItemIntegrante.ExtPersona + enterKey;
                            strText = strText + enterKey;
                        }
                        // Que mejor se van a imprimir los STCP aqui 20090212
                        //Que mejor no 20090217
                        //strText = strText + enterKey + enterKey + strSecTCPParaImpr + enterKey;
                    }
                    break;

                case 0://Pleno
                    break;

                case 6://Sec tesis
                    break;

                case 5: //Secretario de Acuerdos
                    break;
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

        private void PortaPapeles__ImageFailed(object sender, ExceptionRoutedEventArgs e)
        {
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

        private void PortaPapeles_Click(object sender, RoutedEventArgs e)
        {
            String strCabecera = "Opciones de almacenamiento";
            String strMensaje = "Seleccione lo que se va a enviar al portapapeles ";
            String strActual = "";
            String strTodos = "";

            FondoTransparente FT = new FondoTransparente();

            switch (nEnQueComboEstoy)
            {

                case 0://Pleno
                    strCabecera = "";
                    strMensaje = "";
                    strActual = "";
                    strTodos = "";
                    break;

                case 1: //Las comisiones
                    strActual = "La comisión actual";
                    strTodos = "Todas las comisiones";
                    break;

                case 2: //Consejeros integrantes de la comisión
                    //strActual = "El consejero seleccionado";
                    //strTodos = "Los consejeros integrantes";
                    strActual = "El integrante seleccionado";
                    strTodos = "Todos los integrantes";
                    break;

                case 3: //El Sec. Tec. de Com. Permanente
                    strMensaje = "Los datos del Secretario Técnico de Comisión Permanente se van a enviar al portapapeles ";
                    strActual = "";
                    strTodos = "";
                    break;

                case 4: //Los integrantes de la ponencia del Consejero
                    strActual = "El integrante de la ponencia seleccionado";
                    strTodos = "Todos los integrantes de la ponencia";
                    break;

                case 5: //Secretario de Acuerdos
                    strActual = "";
                    strTodos = "";
                    break;

                case 6://Sec tesis
                    strActual = "";
                    strTodos = "";
                    break;
            }

            FTransparente.Width = this.Width;
            FTransparente.Height = this.Height;
            FTransparente.Visibility = Visibility.Visible;
            OpImprimir.TomaFondo(FTransparente);

            OpcionesImprimir opMinistros = new OpcionesImprimir();
            opMinistros.Visibility = Visibility.Visible;
            OpImprimir.StrCabecera = strCabecera;// "Opciones de almacenamiento";
            OpImprimir.StrMensaje = strMensaje; //"Seleccione qué es lo que se va a enviar al portapapeles ";
            OpImprimir.StrActual = strActual;// "Comisión actual";
            OpImprimir.StrOpcionTodos = strTodos;//"Todas las áreas";
            OpImprimir.OptSalida = 0;
            OpImprimir.BringIntoView();
            this.OpImprimir.contenedor = this;
            OpImprimir.Visibility = Visibility.Visible;

        }

        private void Imprimir_Click(object sender, RoutedEventArgs e)
        {
            String strCabecera = "Opciones de impresión";
            String strMensaje = "Seleccione lo que se va a Imprimir ";
            String strActual = "";
            String strTodos = "";

            switch (nEnQueComboEstoy)
            {

                case 0://Pleno
                    strCabecera = "";
                    strMensaje = "";
                    strActual = "";
                    strTodos = "";
                    break;

                case 1: //Las comisiones
                    strActual = "La comisión actual";
                    strTodos = "Todas las comisiones";
                    break;

                case 2: //Consejeros integrantes de la comisión
                    ////strActual = "El consejero seleccionado";
                    ////strTodos = "Los consejeros integrantes";
                    //strActual = "El integrante seleccionado";
                    //strTodos = "Los integrantes de la comisión";
                    strActual = "El integrante seleccionado";
                    strTodos = "Todos los integrantes";
                    break;

                case 3: //El Sec. Tec. de Com. Permanente
                    strMensaje = "Los datos del Secretario Técnico de Comisión Permanente se van a imprimir";
                    strActual = "";
                    strTodos = "";
                    break;

                case 4: //Los integrantes de la ponencia del Consejero
                    //strActual = "El integrante seleccionado";
                    //strTodos = "Todos los integrantes";
                    strActual = "El integrante de la ponencia seleccionado";
                    strTodos = "Todos los integrantes de la ponencia";
                    break;

                case 5: //Secretario de Acuerdos
                    strActual = "";
                    strTodos = "";
                    break;

                case 6://Sec tesis
                    strActual = "";
                    strTodos = "";
                    break;
            }

            FTransparente.Visibility = Visibility.Visible;
            OpImprimir.TomaFondo(FTransparente);

            OpcionesImprimir opConsejeros = new OpcionesImprimir();
            opConsejeros.Visibility = Visibility.Visible;
            OpImprimir.StrCabecera = strCabecera;
            OpImprimir.StrMensaje = strMensaje;
            OpImprimir.StrActual = strActual;
            OpImprimir.StrOpcionTodos = strTodos;
            OpImprimir.OptSalida = 2;
            OpImprimir.BringIntoView();
            this.OpImprimir.contenedor = this;
            OpImprimir.Visibility = Visibility.Visible;

        }

        private void Guardar_Click(object sender, RoutedEventArgs e)
        {
            //OpcionesImprimir opConsejeros = new OpcionesImprimir();
            //opConsejeros.Visibility = Visibility.Visible;
            //OpImprimir.StrCabecera = "Opciones de almacenamiento ";
            //OpImprimir.StrMensaje = "Seleccione qué es lo que se va a Guardar en archivo";
            //OpImprimir.OptSalida = 1;
            //OpImprimir.BringIntoView();
            //this.OpImprimir.contenedor = this;
            //OpImprimir.Visibility = Visibility.Visible;
            String strCabecera = "Opciones de almacenamiento";
            String strMensaje = "Seleccione lo que se va a guardar en archivo ";
            String strActual = "";
            String strTodos = "";

            switch (nEnQueComboEstoy)
            {

                case 0://Pleno
                    strCabecera = "";
                    strMensaje = "";
                    strActual = "";
                    strTodos = "";
                    break;

                case 1: //Las comisiones
                    strActual = "La comisión actual";
                    strTodos = "Todas las comisiones";
                    break;

                case 2: //Consejeros integrantes de la comisión
                    ////strActual = "El consejero seleccionado";
                    ////strTodos = "Los consejeros integrantes";
                    //strActual = "El integrante seleccionado";
                    //strTodos = "Los integrantes de la comisión";
                    strActual = "El integrante seleccionado";
                    strTodos = "Todos los integrantes";
                    break;

                case 3: //El Sec. Tec. de Com. Permanente
                    strMensaje = "Los datos del Secretario Técnico de Comisión Permanente se van a guardar en archivo";
                    strActual = "";
                    strTodos = "";
                    break;

                case 4: //Los integrantes de la ponencia del Consejero
                    //strActual = "El integrante seleccionado";
                    //strTodos = "Todos los integrantes";
                    strActual = "El integrante de la ponencia seleccionado";
                    strTodos = "Todos los integrantes de la ponencia";
                    break;

                case 5: //Secretario de Acuerdos
                    strActual = "";
                    strTodos = "";
                    break;

                case 6://Sec tesis
                    strActual = "";
                    strTodos = "";
                    break;
            }
            OpcionesImprimir opConsejeros = new OpcionesImprimir();
            opConsejeros.Visibility = Visibility.Visible;
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
        }
    }
}
