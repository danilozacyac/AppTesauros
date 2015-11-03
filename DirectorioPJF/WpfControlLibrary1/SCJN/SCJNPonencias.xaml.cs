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

namespace mx.gob.scjn.directorio.SCJN
{

    /// <summary>
    /// Interaction logic for SCJN.xaml
    /// </summary>
    public partial class SCJNPage : Page
    {
        String enterKey = "\n";

        /*Para la impresión ***********************************************/
        private FlowDocument DocumentoParaCopiar { get; set; }
        private AcuerdosTO DocumentoActual;
        Boolean bEsPleno = false;
        Boolean bEsPS = false;
        Boolean bEsSS = false;
        Boolean bEsPres = false;

        Boolean bEstoyModificando = false;

        public Boolean bImprTodo = false;
        public int nOpImpr = 0; // 0,Papelera; 1, Guardar; 2, Imprimir.
        int nEnQueComboEstoy = -1;
        String strSecAcuerdosParaImpr = "";
        String strSecGralAcuerdosParaImpr = "";
        String strMinistroParaImpr = "";
        String strIntImp = ""; // aqui vamos a guardar al integrantede la ponencia seleccionado, por si quieren imprimirlo
        String strIntegrantesSalasParaImprimir = "";
        String strSecTesisParaImpr = "";
        String strEnQueSalaEstoy = "";
        String strMinistroActual = "";
        List<DirectorioPersonasTO> lstIntegrantesPonencia = new List<DirectorioPersonasTO>();
        List<DirectorioPersonasTO> lstResSA = new List<DirectorioPersonasTO>();
        List<DirectorioPersonasTO> lstResSubSA = new List<DirectorioPersonasTO>();
        List<DirectorioPersonasTO> lstResST = new List<DirectorioPersonasTO>();
        List<DirectorioPersonasTO> lstResSGA = new List<DirectorioPersonasTO>();
        List<DirectorioMinistrosTO> lstResImpr = new List<DirectorioMinistrosTO>();
        List<DirectorioPersonasTO> lstResSGAImpr = new List<DirectorioPersonasTO>();
        List<DirectorioPersonasTO> lstResSSGAImpr = new List<DirectorioPersonasTO>();
        /*********************************************************************/

        public Page Back { get; set; }
        public SCJNPage()
        {
            InitializeComponent();

            if (!BrowserInteropHelper.IsBrowserHosted)
            { Guardar_.Visibility = Visibility.Visible; }
            else { Guardar_.Visibility = Visibility.Hidden; }
            llenaComboSalas();
        }

        private void llenaComboSalas()
        {
            List<String> lstAreas = new List<String>();
            lstAreas.Add("Presidencia");
            lstAreas.Add("Pleno");
            lstAreas.Add("Primera Sala");
            lstAreas.Add("Segunda Sala");
            comboBoxInstancia.ItemsSource = lstAreas;
            comboBoxInstancia.SelectedIndex = 0;
        }

        private List<DirectorioMinistrosTO> LlenaComboMinistros(int Filtro)
        {
            List<DirectorioMinistrosTO> lstRes = new List<DirectorioMinistrosTO>();
            List<String> lstMinistros = new List<String>();
#if STAND_ALONE
            List<DirectorioMinistrosTO> R = new List<DirectorioMinistrosTO>();
            FachadaBusquedaTradicional fachada = new FachadaBusquedaTradicional();
#else
            DirectorioMinistrosTO[] R = new DirectorioMinistrosTO[50];
            FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
#endif
            R = fachada.getDirMinistrosXFiltro(Filtro);
#if STAND_ALONE
            for (int i = 0; i < R.Count; i++)
#else
            for (int i = 0; i < R.Length; i++)
#endif
            {
                DirectorioMinistrosTO Campo = new DirectorioMinistrosTO();
                Campo.IdPersona = R[i].IdPersona;
                Campo.IdTitulo = R[i].IdTitulo;
                Campo.IdPuesto = R[i].IdPuesto;
                Campo.IdPonencia = R[i].IdPonencia;
                Campo.NombrePersona = R[i].NombrePersona;
                Campo.NombreCompleto = R[i].Cargo + " " + R[i].NombreCompleto;
                Campo.ApellidosPersona = R[i].ApellidosPersona;
                Campo.Orden = R[i].Orden;
                Campo.OrdenSala = R[i].OrdenSala;
                Campo.Sala = R[i].Sala;
                Campo.Prefijo = R[i].Prefijo;
                Campo.Posfijo = R[i].Posfijo;
                Campo.DomPersona = R[i].DomPersona;
                Campo.TelPersona = R[i].TelPersona;
                Campo.ExtPersona = R[i].ExtPersona;
                Campo.TituloPersona = R[i].TituloPersona;
                Campo.TitSemblanza = R[i].TitSemblanza;
                Campo.Cargo = R[i].Cargo;
                lstRes.Add(Campo);
                Campo = null;
            }
            this.comboBoxSalas.ItemsSource = lstRes;
            this.comboBoxSalas.SelectedIndex = 0;
            this.comboBoxSalas.SelectedItem = this.comboBoxSalas.Items.CurrentItem;
            CargaDetalleMinistro((DirectorioMinistrosTO)comboBoxSalas.SelectedItem);
            fachada.Close();
            return lstRes;
        }

        private void ActualizaIntegrantesSalas(object sender, MouseButtonEventArgs e)
        {
            Actualiza();
        }

        private void ActualizaIntegrantesSalas(object sender, MouseEventArgs e)
        {
            Actualiza();
        }

        private void ActualizaIntegrantesSalas(object sender, EventArgs e)
        {
            Actualiza();
        }

        private void Actualiza()
        {

            try
            {
                //Mientras traemos los mismos datos, depués cada uno va a tener su propia lista        
                List<String> lstAreas = new List<String>();
                clsListaAreas oAreas = new clsListaAreas();
                lstAreas = oAreas.TraeDatos();
            }

            catch (System.Exception error)
            {
                //Handle exception here
            }
        }

        private void CargaDetalle(object sender, MouseButtonEventArgs e)
        {
            //int iPos = (int)comboBoxInstancia.SelectedIndex;
            //if (iPos == 1) { nEnQueComboEstoy = 0; }
            //else{nEnQueComboEstoy = 4;}
            nEnQueComboEstoy = 4;

            try
            {
                DirectorioPersonasTO Campo = new DirectorioPersonasTO();
                Funcionarios.BringItemIntoView(Funcionarios.SelectedItem);
                Campo = (DirectorioPersonasTO)Funcionarios.SelectedItem;
                this.txtNombre.Text = Campo.NombrePersona;
                //textInstancia.Text = Campo.AdscripcionPersona;
                textCargo.Text = Campo.CargoPersona;
                textDomicilio.Text = " Pino Suárez # 2, Col. Centro, Del. Cuauhtémoc, C.P. 06065, México, D.F. "; //5Campo.DomPersona;
                textTel.Text = "Teléfono " + Campo.TelPersona + " Ext. " + Campo.ExtPersona;
                //strIntImp = textInstancia.Text + enterKey;
                strIntImp = txtNombre.Text + enterKey;
                strIntImp = strIntImp + textCargo.Text + enterKey;
                strIntImp = strIntImp + "Domicilio: " + textDomicilio.Text + enterKey;
                strIntImp = strIntImp +  textTel.Text + enterKey + enterKey + enterKey;
                Campo = null;
            }

            catch (System.Exception error)
            {
                //Handle exception here
            }
        }

        private void CargaDetalleMinistro(DirectorioMinistrosTO Min)
        {

            try
            {
                this.txtNombre.Text = Min.TituloPersona + "  " + Min.NombrePersona + "  " + Min.ApellidosPersona;

                if (Min.Prefijo.Length > 1)
                {
                    textCargo.Text = Min.Prefijo;
                }
                else
                {
                    textCargo.Text = Min.Cargo;
                }
                textDomicilio.Text = " Pino Suárez # 2, Col. Centro, Del. Cuauhtémoc, C.P. 06065, México, D.F. "; //5Campo.DomPersona;
                textTel.Text = "Teléfono " + Min.TelPersona + " Ext. " + Min.ExtPersona;
                strMinistroActual = this.txtNombre.Text + enterKey;
                strMinistroParaImpr = strMinistroActual + enterKey;
                //strMinistroParaImpr = strMinistroParaImpr + textInstancia.Text + enterKey;
                strMinistroParaImpr = strMinistroParaImpr + textDomicilio.Text + enterKey;
                strMinistroParaImpr = strMinistroParaImpr + " " + textTel.Text + enterKey;

                lstIntegrantesPonencia = llenaGridIntPonencia();
            }

            catch (System.Exception error)
            {
            }
        }

        private void CargaDetalleSGA(object sender, MouseButtonEventArgs e)
        {
            //int iPos = (int)comboBoxInstancia.SelectedIndex;
            //if (iPos == 1) { nEnQueComboEstoy = 0; }
            //else { nEnQueComboEstoy = 5; }
            nEnQueComboEstoy = 5;

            try
            {
                DirectorioPersonasTO Campo = new DirectorioPersonasTO();
                grdSecAcuerdos.BringItemIntoView(grdSecAcuerdos.SelectedItem);
                Campo = (DirectorioPersonasTO)grdSecAcuerdos.SelectedItem;
                this.txtNombre.Text = Campo.NombrePersona;
                textCargo.Text = Campo.CargoPersona;
                textDomicilio.Text = " Pino Suárez # 2, Col. Centro, Del. Cuauhtémoc, C.P. 06065, México, D.F. "; //5Campo.DomPersona;
                textTel.Text = "Teléfono " + Campo.TelPersona + " Ext.  " + Campo.ExtPersona;
                Campo = null;
                int iPos = (int)comboBoxInstancia.SelectedIndex;

                if (iPos == 1)
                {
                    strSecGralAcuerdosParaImpr = "Pleno" + enterKey;
                    strSecGralAcuerdosParaImpr = txtNombre.Text + enterKey;
                    strSecGralAcuerdosParaImpr = strSecGralAcuerdosParaImpr + textCargo.Text + enterKey;
                    strSecGralAcuerdosParaImpr = strSecGralAcuerdosParaImpr + textDomicilio.Text + enterKey;
                    strSecGralAcuerdosParaImpr = strSecGralAcuerdosParaImpr + textTel.Text + enterKey;
                    strSecGralAcuerdosParaImpr = strSecGralAcuerdosParaImpr + " Ext: " + Campo.ExtPersona + enterKey;
                }
                else
                {
                    strSecAcuerdosParaImpr = txtNombre.Text + enterKey;
                    strSecAcuerdosParaImpr = strSecAcuerdosParaImpr + textCargo.Text + enterKey;
                    strSecAcuerdosParaImpr = strSecAcuerdosParaImpr + textDomicilio.Text + enterKey;
                    strSecAcuerdosParaImpr = strSecAcuerdosParaImpr + textTel.Text + enterKey;
                    strSecAcuerdosParaImpr = strSecAcuerdosParaImpr + " Ext: " + Campo.ExtPersona + enterKey;
                }
            }

            catch (System.Exception error)
            {
                //Handle exception here
            }
        }

        private void CargaDetalleSecTesis(object sender, MouseButtonEventArgs e)
        {
            nEnQueComboEstoy = 6;

            try
            {
                DirectorioPersonasTO Campo = new DirectorioPersonasTO();
                grdSecTesis.BringItemIntoView(grdSecTesis.SelectedItem);
                Campo = (DirectorioPersonasTO)grdSecTesis.SelectedItem;
                txtNombre.Text = Campo.NombrePersona;
                strSecTesisParaImpr = textInstancia.Text + enterKey + enterKey;
                strSecTesisParaImpr = strSecTesisParaImpr + Campo.NombrePersona + enterKey;
                textCargo.Text = Campo.CargoPersona;
                strSecTesisParaImpr = strSecTesisParaImpr + Campo.CargoPersona + enterKey;
                textDomicilio.Text = "Pino Suárez # 2, Col. Centro, Del. Cuauhtémoc, C.P. 06065, México, D.F. "; //5Campo.DomPersona;
                strSecTesisParaImpr = strSecTesisParaImpr + textDomicilio.Text + enterKey;
                textTel.Text = "Teléfono " + Campo.TelPersona + " Ext. " + Campo.ExtPersona;
                strSecTesisParaImpr = strSecTesisParaImpr + "Teléfono: " + Campo.TelPersona ;
                strSecTesisParaImpr = strSecTesisParaImpr + " Ext. " + Campo.ExtPersona + enterKey + enterKey;
                Campo = null;
            }

            catch
            {
            }
        }

        private void comboBoxSalas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (!bEstoyModificando)
                {
                    lstIntegrantesPonencia = llenaGridIntPonencia();
                    //this.textInstancia.Text = comboBoxSalas.SelectedItem.ToString();  
                    CargaDetalleMinistro((DirectorioMinistrosTO)comboBoxSalas.SelectedItem);
                    
                    switch (this.comboBoxInstancia.SelectedIndex)
                    {
                        case 0: nEnQueComboEstoy = 1; break;
                        case 1: nEnQueComboEstoy = 0; break;
                        case 2: nEnQueComboEstoy = 2; break;
                        case 3: nEnQueComboEstoy = 3; break;
                    }
                }
            }
            catch (System.Exception error)
            {
                //Handle exception here
            }
        }

        private void comboBoxSalas_GotMouseCapture(object sender, MouseEventArgs e)
        {
            try
            {
                if (!bEstoyModificando)
                {
                    lstIntegrantesPonencia = llenaGridIntPonencia();
                    CargaDetalleMinistro((DirectorioMinistrosTO)comboBoxSalas.SelectedItem);
                    //nEnQueComboEstoy = 3;
                    //nEnQueComboEstoy = this.comboBoxInstancia.SelectedIndex;

                    switch (this.comboBoxInstancia.SelectedIndex)
                    {
                        case 0: nEnQueComboEstoy = 1; break;
                        case 1: nEnQueComboEstoy = 0; break;
                        case 2: nEnQueComboEstoy = 2; break;
                        case 3: nEnQueComboEstoy = 3; break; 
                    }



                }
            }
            catch (System.Exception error)
            {
                //Handle exception here
            }
        }

        private List<DirectorioPersonasTO> llenaGridIntPonencia()
        {

            List<DirectorioPersonasTO> lstRes = new List<DirectorioPersonasTO>();

            try
            {
                bEstoyModificando = true;
                int Pos = this.comboBoxSalas.SelectedIndex;

                // estamos validando cuando el selecteditem es -1
                if (Pos >= 0)
                {
                    DirectorioMinistrosTO seleccionado = (DirectorioMinistrosTO)this.comboBoxSalas.SelectedItem;
                    int Sel = seleccionado.IdPonencia;
                    //nEnQueComboEstoy = Sel;
#if STAND_ALONE
                    List<DirectorioPersonasTO> R = new List<DirectorioPersonasTO>();
                    FachadaBusquedaTradicional fachada = new FachadaBusquedaTradicional();
#else
                    DirectorioPersonasTO[] R = new DirectorioPersonasTO[50];
                    FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
#endif
                    R = fachada.getDirPersonas(Sel.ToString());
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
                        strIntegrantesSalasParaImprimir = strIntegrantesSalasParaImprimir + Campo.NombrePersona + enterKey;
                        Campo = null;
                    }
                    this.Funcionarios.ItemsSource = lstRes;
                    //this.Funcionarios.SelectedIndex = 0;
                    //CargaDetalleMinistro((DirectorioMinistrosTO)comboBoxSalas.SelectedItem);
                    fachada.Close();
                }
                bEstoyModificando = false;
            }

            catch (System.Exception error)
            {
                //Handle exception here
            }
            return lstRes;

            
        }

        private List<DirectorioPersonasTO> llenaGridIntPonencia(DirectorioMinistrosTO Ministro)
        {

            List<DirectorioPersonasTO> lstRes = new List<DirectorioPersonasTO>();

            try
            {
                bEstoyModificando = true;
                int Pos = this.comboBoxSalas.SelectedIndex;

                // estamos validando cuando el selecteditem es -1
                if (Pos >= 0)
                {
                    //DirectorioMinistrosTO seleccionado = (DirectorioMinistrosTO)this.comboBoxSalas.SelectedItem;
                    int Sel = Ministro.IdPersona ;
                    //nEnQueComboEstoy = Sel;
#if STAND_ALONE
                    List<DirectorioPersonasTO> R = new List<DirectorioPersonasTO>();
                    FachadaBusquedaTradicional fachada = new FachadaBusquedaTradicional();
#else
                    DirectorioPersonasTO[] R = new DirectorioPersonasTO[50];
                    FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
#endif
                    R = fachada.getDirPersonas(Sel.ToString());
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
                        strIntegrantesSalasParaImprimir = strIntegrantesSalasParaImprimir + Campo.NombrePersona + enterKey;
                        Campo = null;
                    }
                    this.Funcionarios.ItemsSource = lstRes;
                    //this.Funcionarios.SelectedIndex = 0;
                    CargaDetalleMinistro((DirectorioMinistrosTO)comboBoxSalas.SelectedItem);
                    fachada.Close();
                }
                bEstoyModificando = false;
            }

            catch (System.Exception error)
            {
                //Handle exception here
            }
            return lstRes;


        }

        private List<DirectorioPersonasTO> TraeSecretarios(int Sala)
        {
            bEstoyModificando = true;
            List<DirectorioPersonasTO> lstRes = new List<DirectorioPersonasTO>();
            lstRes = TraeSecTesis(Sala);
            bEstoyModificando = false;
            return lstRes;
        }

        private List<DirectorioPersonasTO> TraeSGA(int Sala)
        {
            List<DirectorioPersonasTO> lstRes = new List<DirectorioPersonasTO>();
            bEstoyModificando = true;
            try
            {
#if STAND_ALONE
                List<DirectorioPersonasTO> R = new List<DirectorioPersonasTO>();
                FachadaBusquedaTradicional fachada = new FachadaBusquedaTradicional();
#else
                    DirectorioPersonasTO[] R = new DirectorioPersonasTO[50];
                    FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
#endif
                //Sabemos que el puesto del SGA es el 16
                R = fachada.getDirPersonasXPuestoYSala("16", Sala.ToString());
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
                this.grdSecAcuerdos.ItemsSource = lstRes;
                fachada.Close();
            }

            catch (System.Exception error)
            {
                //Handle exception here
            }
            bEstoyModificando = false;
            return lstRes;
        }

        private List<DirectorioPersonasTO> TraeSecTesis(int Sala)
        {
            List<DirectorioPersonasTO> lstRes = new List<DirectorioPersonasTO>();
            bEstoyModificando = true;
            try
            {
#if STAND_ALONE
                List<DirectorioPersonasTO> R = new List<DirectorioPersonasTO>();
                FachadaBusquedaTradicional fachada = new FachadaBusquedaTradicional();
#else
                    DirectorioPersonasTO[] R = new DirectorioPersonasTO[50];
                    FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
#endif
                //Sabemos que el puesto del Sectesis es el 22
                R = fachada.getDirPersonasXPuestoYSala("22", Sala.ToString());
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
                this.grdSecTesis.ItemsSource = lstRes;
                fachada.Close();
            }

            catch (System.Exception error)
            {
                //Handle exception here
            }
            bEstoyModificando = false;
            return lstRes;
        }

        private List<DirectorioPersonasTO> TraeSecgralAcuerdos(int Sala)
        {
            List<DirectorioPersonasTO> lstRes = new List<DirectorioPersonasTO>();
            bEstoyModificando = true;
            try
            {
#if STAND_ALONE
                List<DirectorioPersonasTO> R = new List<DirectorioPersonasTO>();
                FachadaBusquedaTradicional fachada = new FachadaBusquedaTradicional();
#else
                    DirectorioPersonasTO[] R = new DirectorioPersonasTO[50];
                    FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
#endif
                //Sabemos que el puesto del Sec Gral de Acuerdos es el 6
                R = fachada.getDirPersonasXPuestoYSala("6", Sala.ToString());
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
                this.grdSecAcuerdos.ItemsSource = lstRes;
                fachada.Close();
            }

            catch (System.Exception error)
            {
                //Handle exception here
            }
            bEstoyModificando = false;
            return lstRes;
        }

        private List<DirectorioPersonasTO> TraeSubSecGralAcuerdos(int Sala)
        {
            List<DirectorioPersonasTO> lstRes = new List<DirectorioPersonasTO>();
            bEstoyModificando = true;
            try
            {
#if STAND_ALONE
                List<DirectorioPersonasTO> R = new List<DirectorioPersonasTO>();
                FachadaBusquedaTradicional fachada = new FachadaBusquedaTradicional();
#else
                    DirectorioPersonasTO[] R = new DirectorioPersonasTO[50];
                    FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
#endif
                //Sabemos que el puesto del Sectesis es el 22
                R = fachada.getDirPersonasXPuestoYSala("11", Sala.ToString());
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
                this.grdSecTesis.ItemsSource = lstRes;
                fachada.Close();
            }

            catch (System.Exception error)
            {
                //Handle exception here
            }
            bEstoyModificando = false;
            return lstRes;
        }

        private void CambiaEdolbl(int nOp)
        {

            switch (nOp)
            {

                case 0://PRES
                    this.lblPonencia.Content = " ";
                    this.lblSGA.Content = " ";
                    this.lblSecTes.Content = " ";
                    break;

                case 1://PLENO
                    this.lblPonencia.Content = " ";
                    this.lblSGA.Content = "SECRETARIO GENERAL DE ACUERDOS";// "SUBSECRETARIO GENERAL DE ACUERDOS";
                    this.lblSecTes.Content = "SUBSECRETARIO GENERAL DE ACUERDOS"; //"SECRETARIO GENERAL DE ACUERDOS";
                    break;

                case 2://PS
                    this.lblPonencia.Content = "PONENCIA";
                    this.lblSGA.Content = "SECRETARIO DE ACUERDOS";
                    this.lblSecTes.Content = "SECRETARIO DE ESTUDIO Y CUENTA ESPECIALIZADO EN TESIS";
                    break;

                case 3://SS
                    this.lblPonencia.Content = "PONENCIA";
                    this.lblSGA.Content = "SECRETARIO DE ACUERDOS";
                    this.lblSecTes.Content = "SECRETARIOS DE ESTUDIO Y CUENTA ESPECIALIZADOS EN TESIS";
                    break;
            }
        }

        private void comboBoxInstancia_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            bEstoyModificando = true;
            try
            {
                int iPos;
                iPos = (int)comboBoxInstancia.SelectedIndex;
                LimpiaGridsLista();
                LimpiaDetalles();
                LimpiaDetallesSec();
                CambiaEdolbl(iPos);
                bEsPleno = false;
                bEsPS = false;
                bEsSS = false;
                bEsPres = false;

                switch (iPos)
                {

                    case 0://Presidencia
                        lstResImpr = LlenaComboMinistros(0);
                        bEsPres = true;
                        nEnQueComboEstoy = 1;
                        strEnQueSalaEstoy = "Presidencia";
                        break;

                    case 1: //PLENO
                        lstResImpr = LlenaComboMinistros(3);
                        lstResSGAImpr = TraeSecgralAcuerdos(0);
                        lstResSSGAImpr = TraeSubSecGralAcuerdos(0);
                        bEsPleno = true;
                        nEnQueComboEstoy = 0;
                        strEnQueSalaEstoy = "Pleno";
                        break;

                    case 2://PS
                        lstResImpr = LlenaComboMinistros(1);
                        lstResSA = TraeSGA(1);
                        lstResST = TraeSecTesis(1);
                        bEsPS = true;
                        nEnQueComboEstoy = 2;
                        strEnQueSalaEstoy = "Primera Sala";
                        break;

                    case 3://SS
                        lstResImpr = LlenaComboMinistros(2);
                        lstResSA = TraeSGA(2);
                        lstResST = TraeSecTesis(2);
                        bEsSS = true;
                        nEnQueComboEstoy = 3;
                        strEnQueSalaEstoy = "Segunda Sala";
                        break;
                    default: break;
                }
                this.textInstancia.Text = strEnQueSalaEstoy;
            }

            catch (System.Exception error)
            {
                //Handle exception here
            }

            bEstoyModificando = false;
        }

        private void grd_MDC(object sender, MouseButtonEventArgs e)
        {
        }

        private void Salir_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
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

        private void LimpiaDetalles()
        {
            textInstancia.Text = "";
            textCargo.Text = "";
            textDomicilio.Text = "";
            textTel.Text = "";
        }

        private void LimpiaDetallesSec()
        {
            grdSecTesis.ItemsSource = null;
            grdSecAcuerdos.ItemsSource = null;
        }

        private void LimpiaGridsLista()
        {
            Funcionarios.ItemsSource = null;
        }

        private void Guardar(object sender, MouseButtonEventArgs e)
        {
            ////OpcionesImprimir opMinistros = new OpcionesImprimir();
            ////opMinistros.Visibility = Visibility.Visible;
            ////OpImprimir.StrCabecera = "Opciones de almacenamiento ";
            ////OpImprimir.StrMensaje = "Seleccione qué es lo que se va a Guardar en archivo";
            //String strCabecera = "Opciones de almacenamiento";
            //String strMensaje = "Seleccione lo que se va a guardar en archivo";
            //String strActual = "";
            //String strTodos = "";
            //int iPos; //para saber en que combo de instancia estamos

            //switch (nEnQueComboEstoy)
            //{

            //    case 0://Pleno
            //        strActual = "El Ministro seleccionado";
            //        strTodos = "Los Ministros integrantes del Pleno";
            //        break;

            //    case 1: //Presidencia
            //        strMensaje = "Los datos del Ministro Presidente se van a guardar en archivo";
            //        strActual = "";
            //        strTodos = "";
            //        break;

            //    case 2: //Segunda sala
            //        strActual = "El Ministro seleccionado";
            //        strTodos = "Los Ministros integrantes de la Sala";
            //        break;

            //    case 3: //Primera Sala
            //        strActual = "El Ministro seleccionado";
            //        strTodos = "Los Ministros integrantes de la Sala";
            //        break;

            //    case 4: //Los integrantes de la ponencia del Ministro
            //        strActual = "El integrante de la ponencia seleccionado";
            //        strTodos = "Todos los integrantes de la ponencia";
            //        break;

            //    case 5: //Secretario de Acuerdos
            //        //strMensaje = "El Secretario general de Acuerdos seleccionado se va guardar en archivo";
            //        //strActual = "";
            //        //strTodos = "";
            //        iPos = (int)comboBoxInstancia.SelectedIndex;

            //        if (iPos == 1)
            //        {

            //            if (lstResSGAImpr.Count > 1)
            //            {
            //                strActual = "El Secretario seleccionado";
            //                strTodos = "Los Secretario integrantes de la Sala";
            //            }
            //            else
            //            {
            //                strMensaje = "Los datos del Secretario General de Acuerdos se van a guardar en archivo";
            //                strActual = "";
            //                strTodos = "";
            //            }
            //        }
            //        else  //SECET
            //        {

            //            if (this.lstResSA.Count > 1)
            //            {
            //                //strActual = "El Secretario de estudio y cuenta especializados en tesis";
            //                //strTodos = "Los Secretarios de estudio y cuenta especializados en tesis";
            //                strActual = "El Secretario actual";
            //                strTodos = "Los Secretarios integrantes de la Sala";
            //            }
            //            else
            //            {
            //                strMensaje = "Los datos del Secretario de Acuerdos se van a guardar en archivo";
            //                strActual = "";
            //                strTodos = "";
            //            }
            //        }
            //        break;

            //    case 6://Sec Estudio y cuenta especializados en tesis

            //        iPos = (int)comboBoxInstancia.SelectedIndex;

            //        switch (iPos)
            //        {

            //            case 1:

            //                if (lstResSSGAImpr.Count > 1)
            //                {
            //                    strActual = "El Secretario General de Acuerdos";
            //                    strTodos = "Los Secretario integrantes de la Sala";
            //                }
            //                else
            //                {
            //                    strMensaje = "Los datos del Subsecretario General de Acuerdos se van a guardar en archivo";
            //                    strActual = "";
            //                    strTodos = "";
            //                }
            //                break;

            //            case 2://Primera Sala

            //                if (lstResSSGAImpr.Count > 1)
            //                {
            //                    strActual = "Secretario de Estudio y Cuenta Especializado en Tesis";
            //                    strTodos = "Los Secretarios de Estudio y Cuenta Especializados en Tesis";
            //                }
            //                else
            //                {
            //                    strMensaje = "Los datos del Secretario de Estudio y Cuenta Especializado en Tesis se van a guardar en archivo";
            //                    strActual = "";
            //                    strTodos = "";
            //                }
            //                break;

            //            case 3://Segunda Sala

            //                if (lstResST.Count > 1)
            //                {
            //                    strActual = "El Secretario de Estudio y Cuenta \n Especializado en Tesis seleccionado";
            //                    strTodos = "Los Secretarios de Estudio y Cuenta \n Especializados en Tesis integrantes de la Sala";
            //                }
            //                else
            //                {
            //                    strMensaje = "Los datos del Secretario de Estudio y Cuenta Especializado en Tesis se van a guardar en archivo";
            //                    strActual = "";
            //                    strTodos = "";
            //                }
            //                break;

            //            case 4:
            //                break;
            //        }
            //        break;
            //}
            //FTransparente.Visibility = Visibility.Visible;
         
            //OpImprimir.StrCabecera = strCabecera;
            //OpImprimir.StrMensaje = strMensaje;
            //OpImprimir.StrActual = strActual;
            //OpImprimir.StrOpcionTodos = strTodos;
            //OpImprimir.OptSalida = 1;
            //OpImprimir.BringIntoView();
            //this.OpImprimir.contenedor = this;
            //OpImprimir.Visibility = Visibility.Visible;

            //OpImprimir.TomaFondo(FTransparente);
        }

        private void Imprimir(object sender, MouseButtonEventArgs e)
        {
            //String strCabecera = "Opciones de impresión";
            //String strMensaje = "Seleccione lo que se va a imprimir";
            //String strActual = "";
            //String strTodos = "";
            //int iPos = -1;

            //switch (nEnQueComboEstoy)
            //{

            //    case 0://Pleno
            //        strActual = "El Ministro seleccionado";
            //        strTodos = "Los Ministros integrantes del Pleno";
            //        break;

            //    case 1: //Presidencia
            //        strMensaje = "Los datos del Ministro Presidente se van a imprimir";
            //        strActual = "";
            //        strTodos = "";
            //        break;

            //    case 2: //Segunda sala
            //        strActual = "El Ministro seleccionado";
            //        strTodos = "Los Ministros integrantes de la Sala";
            //        break;

            //    case 3: //Primera Sala
            //        strActual = "El Ministro seleccionado";
            //        strTodos = "Los Ministros integrantes de la Sala";
            //        break;

            //    case 4: //Los integrantes de la ponencia del Ministro
            //        strActual = "El integrante de la ponencia seleccionado";
            //        strTodos = "Todos los integrantes de la ponencia";
            //        break;

            //    case 5: //Secretario de Acuerdos
            //        //strMensaje = "El Secretario general de acuerdos seleccionado se va imprimir";
            //        //strActual = "";
            //        //strTodos = "";
            //        iPos = (int)comboBoxInstancia.SelectedIndex;

            //        if (iPos == 1) //pleno
            //        {

            //            if (lstResSA.Count > 1)
            //            {
            //                strActual = "El Secretario seleccionado";
            //                strTodos = "Los Secretarios integrantes de la Sala";
            //            }
            //            else
            //            {
            //                strMensaje = "Los datos del Secretario General de Acuerdos se van a imprimir";
            //                strActual = "";
            //                strTodos = "";
            //            }
            //        }
            //        else  //SECET
            //        {

            //            if (this.lstResSA.Count > 1)
            //            {
            //                //strActual = "El Secretario de estudio y cuenta especializados en tesis";
            //                //strTodos = "Los Secretarios de estudio y cuenta especializados en tesis";
            //                strActual = "El Secretario actual";
            //                strTodos = "Los Secretarios integrantes de la Sala";
            //            }
            //            else
            //            {
            //                strMensaje = "Los datos del Secretario de Acuerdos se van a imprimir";
            //                strActual = "";
            //                strTodos = "";
            //            }
            //        }
            //        break;

            //    case 6://Sec Estudio y cuenta especializados en tesis

            //        iPos = (int)comboBoxInstancia.SelectedIndex;

            //        switch (iPos)
            //        {

            //            case 1:

            //                if (lstResSSGAImpr.Count > 1)
            //                {
            //                    strActual = "El Secretario General de Acuerdos";
            //                    strTodos = "Los Secretario integrantes de la Sala";
            //                }
            //                else
            //                {
            //                    strMensaje = "Los datos del Subsecretario General de Acuerdos se van a imprimir";
            //                    strActual = "";
            //                    strTodos = "";
            //                }
            //                break;

            //            case 2://Primera Sala

            //                if (lstResSSGAImpr.Count > 1)
            //                {
            //                    strActual = "Secretario de Estudio y Cuenta Especializado en Tesis";
            //                    strTodos = "Los Secretarios de Estudio y Cuenta Especializados en Tesis";
            //                }
            //                else
            //                {
            //                    strMensaje = "Los datos del Secretario de Estudio y Cuenta Especializado en Tesis se van a imprimir";
            //                    strActual = "";
            //                    strTodos = "";
            //                }
            //                break;

            //            case 3://Segunda Sala

            //                if (lstResST.Count > 1)
            //                {
            //                    strActual = "El Secretario de Estudio y Cuenta \n Especializado en Tesis seleccionado";
            //                    strTodos = "Los Secretarios de Estudio y Cuenta \n Especializados en Tesis integrantes de la Sala";
            //                }
            //                else
            //                {
            //                    strMensaje = "Los datos del Secretario de Estudio y Cuenta Especializado en Tesis se van a imprimir";
            //                    strActual = "";
            //                    strTodos = "";
            //                }
            //                break;

            //            case 4:
            //                break;
            //        }
            //        break;
            //}
            //FTransparente.Visibility = Visibility.Visible;
            
            //OpImprimir.StrCabecera = strCabecera;
            //OpImprimir.StrMensaje = strMensaje;
            //OpImprimir.StrActual = strActual;
            //OpImprimir.StrOpcionTodos = strTodos;
            //OpImprimir.OptSalida = 2;
            //OpImprimir.BringIntoView();
            //this.OpImprimir.contenedor = this;
            //OpImprimir.Visibility = Visibility.Visible;
            
            //OpImprimir.TomaFondo(FTransparente);
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

        private void ImprimeRTB(RichTextBox rtbTexto)
        {
        }

        private void PortaPapStr(String strTexto)
        {
            contenidoTexto = new RichTextBox();
            this.contenidoTexto.AppendText(strTexto);
            this.contenidoTexto.SelectAll();
            contenidoTexto.Copy();
            //FondoTransparente Ftransp = new FondoTransparente();
            FTransparente.Visibility = Visibility.Visible;
           
            Aviso.StrCabecera = mx.gob.scjn.directorio.MensajesDirectorio.TITULO_PORTAPAPELES_DIR;
            Aviso.StrMensaje = mx.gob.scjn.directorio.MensajesDirectorio.MENSAJE_PORTAPAPELES_DIR;
            Aviso.Visibility = Visibility.Visible;
            Aviso.Background = Brushes.White;

            Aviso.TomaFondo(FTransparente);

            
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

        private void PortaPapeles(object sender, MouseButtonEventArgs e)
        {
            String strCabecera = "Opciones de almacenamiento";
            String strMensaje = "Seleccione lo que se va a enviar al portapapeles";
            String strActual = "";
            String strTodos = "";
            int iPos;
            int nAnchoVentanaOpciones = -1;

            switch (nEnQueComboEstoy)
            {

                case 0://Pleno
                    strActual = "El Ministro seleccionado";
                    strTodos = "Los Ministros integrantes del Pleno";
                    break;

                case 1: //Presidencia
                    strMensaje = "Los datos del Ministro Presidente se van a enviar al portapapeles";
                    strActual = "";
                    strTodos = "";
                    break;

                case 2: //Segunda sala
                    strActual = "El Ministro seleccionado";
                    strTodos = "Los Ministros integrantes de la Sala";
                    break;

                case 3: //Primera Sala
                    strActual = "El Ministro seleccionado";
                    strTodos = "Los Ministros integrantes de la Sala";
                    break;

                case 4: //Los integrantes de la ponencia del Ministro
                    strActual = "El integrante de la ponencia seleccionado";
                    strTodos = "Todos los integrantes de la ponencia";
                    break;

                case 5: //Secretario de Acuerdos
                    //strMensaje = "El Secretario general de acuerdos seleccionado se van a enviar al portapapeles";
                    strActual = "";
                    strTodos = "";
                    iPos = (int)comboBoxInstancia.SelectedIndex;

                    if (iPos == 1)
                    {

                        if (lstResSGAImpr.Count > 1)
                        {
                            strActual = "El Secretario General de Acuerdos seleccionado";
                            strTodos = "Los Secretarios integrantes de la Sala";
                        }
                        else
                        {
                            strMensaje = "Los datos del Secretario General de Acuerdos se van a enviar al portapapeles";
                            strActual = "";
                            strTodos = "";
                        }
                    }
                    else
                    {

                        if (lstResSA.Count > 1)
                        {
                            strActual = "El Secretario de Acuerdos seleccionado";
                            strTodos = "Los Secretarios de Acuerdos";
                        }
                        else
                        {
                            strMensaje = "Los datos del Secretario de Acuerdos se van a enviar al portapapeles";
                            strActual = "";
                            strTodos = "";
                        }
                    }
                    break;

                case 6://Sec Estudio y cuenta especializados en tesis
                    iPos = (int)comboBoxInstancia.SelectedIndex;

                    switch (iPos)
                    {

                        case 1:

                            if (lstResSSGAImpr.Count > 1)
                            {
                                strActual = "El Secretario General de Acuerdos";
                                strTodos = "Los Secretario integrantes de la Sala";
                            }
                            else
                            {
                                strMensaje = "Los datos del Subsecretario General de Acuerdos se van a enviar al portapapeles";
                                strActual = "";
                                strTodos = "";
                            }
                            break;

                        case 2://Primera Sala

                            if (lstResSSGAImpr.Count > 1)
                            {
                                strActual = "Secretario de Estudio y Cuenta Especializado en Tesis";
                                strTodos = "Los Secretarios de Estudio y Cuenta Especializados en Tesis";
                            }
                            else
                            {
                                strMensaje = "Los datos del Secretario de Estudio y Cuenta Especializado en Tesis se van a enviar al portapapeles";
                                strActual = "";
                                strTodos = "";
                            }
                            break;

                        case 3://Segunda Sala

                            //lstResST
                            if (lstResST.Count > 1)
                            {
                                strActual = "El Secretario de Estudio y Cuenta \n Especializado en Tesis seleccionado";
                                strTodos = "Los Secretarios de Estudio y Cuenta \n Especializados en Tesis integrantes de la Sala";
                            }
                            else
                            {
                                strMensaje = "Los datos del Secretario de Estudio y Cuenta Especializado en Tesis se van a enviar al portapapeles";
                                strActual = "";
                                strTodos = "";
                            }
                            break;

                        case 4:
                            break;
                    }
                    break;
            }

            //FondoTransparente Ftransp = new FondoTransparente();
            FTransparente.Visibility = Visibility.Visible;
 
            
            OpImprimir.StrCabecera = strCabecera;
            OpImprimir.StrMensaje = strMensaje;
            OpImprimir.StrActual = strActual;
            OpImprimir.StrOpcionTodos = strTodos;
            OpImprimir.nWidth = nAnchoVentanaOpciones;
            OpImprimir.OptSalida = 0;
            OpImprimir.BringIntoView();
            this.OpImprimir.contenedor = this;
            OpImprimir.Visibility = Visibility.Visible;

            OpImprimir.TomaFondo(FTransparente); 

            

        }

        private void Imprimir__ImageFailed(object sender, ExceptionRoutedEventArgs e)
        {
        }

        public void ImprimePonencia(Boolean bTodos, int nIdMin, Int32 nSelSalida)
        {
            String strText = "";

            switch (nEnQueComboEstoy)
            {

                case 4://integrantes de las ponencias
                    strText = strEnQueSalaEstoy + enterKey + enterKey + enterKey;
                    strText = strText + "Ponencia: " + strMinistroActual + enterKey + enterKey;

                    if (bTodos == false) //Imprimir sólo el actual
                    {
                        strText = strText + strIntImp + enterKey;
                    }
                    else  //Imprimir todos
                    {
                        strText = strText + strMinistroParaImpr + enterKey;

                        foreach (DirectorioPersonasTO ItemIntegrante in lstIntegrantesPonencia)
                        {
                            strText = strText + ItemIntegrante.CargoPersona + enterKey;
                            strText = strText + ItemIntegrante.NombrePersona + enterKey;
                            strText = strText + "Teléfono: " + ItemIntegrante.TelPersona + " Ext. " + ItemIntegrante.ExtPersona + enterKey;
                            strText = strText + enterKey;
                        }
                    }
                    break;

                case 0://Pleno
                    strText = strEnQueSalaEstoy + enterKey + enterKey + enterKey;

                    if (bTodos == false) //Imprimir sólo el actual
                    {
                        strText = strText + strMinistroParaImpr + enterKey;
                    }
                    else  //Imprimir todos
                    {
                        String strposFijo = "";

                        foreach (DirectorioMinistrosTO ItemIntegrante in lstResImpr)
                        {
                            strposFijo = ItemIntegrante.Posfijo;
                            strText = strText + ItemIntegrante.NombreCompleto + strposFijo + enterKey;
                            strText = strText + "Teléfono: " + ItemIntegrante.TelPersona + enterKey;
                            strText = strText + "Ext. " + ItemIntegrante.ExtPersona + enterKey;
                            strText = strText + enterKey;
                        }
                        #region Secretarios

                        ////Ahora ya no quieren que aparezcan los secretarios cuando se imprime a los integrantes del pleno
                        //foreach (DirectorioPersonasTO ItemIntegrante in lstResSGAImpr)
                        //{
                        //    strText = strText + ItemIntegrante.CargoPersona + enterKey;
                        //    strText = strText + ItemIntegrante.NombrePersona + enterKey;
                        //    strText = strText + "Teléfono: " + ItemIntegrante.TelPersona + enterKey;
                        //    strText = strText + "Ext. " + ItemIntegrante.ExtPersona + enterKey;
                        //    strText = strText + enterKey;
                        //}
                        //foreach (DirectorioPersonasTO ItemIntegrante in lstResSSGAImpr)
                        //{
                        //    strText = strText + ItemIntegrante.CargoPersona + enterKey;
                        //    strText = strText + ItemIntegrante.NombrePersona + enterKey;
                        //    strText = strText + "Teléfono: " + ItemIntegrante.TelPersona + enterKey;
                        //    strText = strText + "Ext. " + ItemIntegrante.ExtPersona + enterKey;
                        //    strText = strText + enterKey;
                        //}
                        #endregion
                    }
                    break;

                case 1://Pres
                    strText = strEnQueSalaEstoy + enterKey + enterKey;

                    //strText = strText + strMinistroActual + enterKey + enterKey;
                    if (bTodos == false) //Imprimir sólo el actual
                    {
                        strText = strText + strMinistroParaImpr + enterKey;
                    }
                    else  //Imprimir todos
                    {
                        strText = strText + strMinistroParaImpr + enterKey;
                    }
                    break;

                case 2://PS

                case 3://SS
                    strText = strEnQueSalaEstoy + enterKey + enterKey;

                    if (bTodos == false) //Imprimir sólo el actual
                    {
                        //strText = strText + "Ponencia: " + strMinistroActual + enterKey + enterKey;
                        strText = strText + strMinistroParaImpr + enterKey;
                    }
                    else  //Imprimir todos
                    {
                        //strText = strText + "Ponencia: " + strMinistroActual + enterKey + enterKey;
                        String strposFijo = "";

                        foreach (DirectorioMinistrosTO ItemIntegrante in lstResImpr)
                        {
                            strposFijo = ItemIntegrante.Posfijo;
                            strText = strText + ItemIntegrante.NombreCompleto + strposFijo + enterKey;
                            strText = strText + "Teléfono: " + ItemIntegrante.TelPersona + enterKey;
                            strText = strText + "Ext. " + ItemIntegrante.ExtPersona + enterKey;
                            strText = strText + enterKey;
                        }

                        //Secretario de acuerdos
                        foreach (DirectorioPersonasTO ItemIntegrante in lstResSA)
                        {
                            strText = strText + ItemIntegrante.CargoPersona + enterKey;
                            strText = strText + ItemIntegrante.NombrePersona + enterKey;
                            strText = strText + "Teléfono: " + ItemIntegrante.TelPersona + enterKey;
                            strText = strText + "Ext. " + ItemIntegrante.ExtPersona + enterKey;
                            strText = strText + enterKey;
                        }

                        //Secretarios de tesis
                        foreach (DirectorioPersonasTO ItemIntegrante in lstResST)
                        {
                            strText = strText + ItemIntegrante.CargoPersona + enterKey;
                            strText = strText + ItemIntegrante.NombrePersona + enterKey;
                            strText = strText + "Teléfono: " + ItemIntegrante.TelPersona + enterKey;
                            strText = strText + "Ext. " + ItemIntegrante.ExtPersona + enterKey;
                            strText = strText + enterKey;
                        }
                    }
                    break;

                case 6://Sec tesis

                    if (bTodos == false) //Imprimir sólo el actual
                    {
                        strText = strSecTesisParaImpr + enterKey;
                    }
                    else  //Imprimir todos
                    {

                        if ((int)comboBoxInstancia.SelectedIndex == 1) //si estamos en pleno
                        {
                            strText = strSecGralAcuerdosParaImpr;
                        }
                        else
                        {
                            strText = textInstancia.Text + enterKey + enterKey;

                            foreach (DirectorioPersonasTO ItemIntegrante in lstResST)
                            {
                                strText = strText + ItemIntegrante.NombrePersona + enterKey;
                                strText = strText + ItemIntegrante.CargoPersona + enterKey;
                                strText = strText + "Teléfono: " + ItemIntegrante.TelPersona + enterKey;
                                strText = strText + "Ext. " + ItemIntegrante.ExtPersona + enterKey;
                                strText = strText + enterKey;
                            }
                        }
                    }
                    break;

                case 5: //Secretario de Acuerdos
                    strText = strEnQueSalaEstoy + enterKey + enterKey;

                    if (bTodos == false) //Imprimir sólo el actual
                    {

                        if ((int)comboBoxInstancia.SelectedIndex == 1) //si estamos en pleno
                        {
                            strText = "Pleno";
                            strText = strText + enterKey + strSecGralAcuerdosParaImpr;
                        }
                        else
                        {
                            strText = strText + strSecAcuerdosParaImpr + enterKey;
                        }
                    }
                    else  //Imprimir todos
                    {

                        foreach (DirectorioPersonasTO ItemIntegrante in lstResSA)
                        {
                            strText = strText + ItemIntegrante.NombrePersona + enterKey;
                            strText = strText + "Teléfono: " + ItemIntegrante.TelPersona + enterKey;
                            strText = strText + "Ext. " + ItemIntegrante.ExtPersona + enterKey;
                            strText = strText + enterKey;
                        }
                    }
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

        public void ImprimePonencia_Old(int nOpcion, int nIdMin, int SelSalida)
        {
        }

        private void PortaPapeles__ImageFailed(object sender, ExceptionRoutedEventArgs e)
        {
        }

        private void comboBoxSalas_MouseDown(object sender, MouseButtonEventArgs e)
        {
        }

        private void comboBoxSalas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
        }

        private void comboBoxSalas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
        }

        private void comboBoxSalas_MouseUp(object sender, MouseButtonEventArgs e)
        {
        }

        private void grdSecTesis_LostFocus(object sender, RoutedEventArgs e)
        {

            grdSecTesis.SelectedIndex = 0;
            //grdSecTesis.SelectedItems.Remove(grdSecTesis.SelectedItems);
            //grdSecTesis.SelectedItems.Clear(); 
        }

        private void grdSecAcuerdos_LostFocus(object sender, RoutedEventArgs e)
        {
            //grdSecAcuerdos.SelectedItems.Remove(grdSecAcuerdos.SelectedItems);
            grdSecAcuerdos.SelectedItems.Clear(); 
        }

        private void Funcionarios_LostFocus(object sender, RoutedEventArgs e)
        {
            //Funcionarios.SelectedItems.Remove(Funcionarios.SelectedItems);
            Funcionarios.SelectedItems.Clear();
        }

        private void Guardar_Click(object sender, RoutedEventArgs e)
        {
            //OpcionesImprimir opMinistros = new OpcionesImprimir();
            //opMinistros.Visibility = Visibility.Visible;
            //OpImprimir.StrCabecera = "Opciones de almacenamiento ";
            //OpImprimir.StrMensaje = "Seleccione qué es lo que se va a Guardar en archivo";
            String strCabecera = "Opciones de almacenamiento";
            String strMensaje = "Seleccione lo que se va a guardar en archivo";
            String strActual = "";
            String strTodos = "";
            int iPos; //para saber en que combo de instancia estamos

            switch (nEnQueComboEstoy)
            {

                case 0://Pleno
                    strActual = "El Ministro seleccionado";
                    strTodos = "Los Ministros integrantes del Pleno";
                    break;

                case 1: //Presidencia
                    strMensaje = "Los datos del Ministro Presidente se van a guardar en archivo";
                    strActual = "";
                    strTodos = "";
                    break;

                case 2: //Segunda sala
                    strActual = "El Ministro seleccionado";
                    strTodos = "Los Ministros integrantes de la Sala";
                    break;

                case 3: //Primera Sala
                    strActual = "El Ministro seleccionado";
                    strTodos = "Los Ministros integrantes de la Sala";
                    break;

                case 4: //Los integrantes de la ponencia del Ministro
                    strActual = "El integrante de la ponencia seleccionado";
                    strTodos = "Todos los integrantes de la ponencia";
                    break;

                case 5: //Secretario de Acuerdos
                    //strMensaje = "El Secretario general de Acuerdos seleccionado se va guardar en archivo";
                    //strActual = "";
                    //strTodos = "";
                    iPos = (int)comboBoxInstancia.SelectedIndex;

                    if (iPos == 1)
                    {

                        if (lstResSGAImpr.Count > 1)
                        {
                            strActual = "El Secretario seleccionado";
                            strTodos = "Los Secretario integrantes de la Sala";
                        }
                        else
                        {
                            strMensaje = "Los datos del Secretario General de Acuerdos se van a guardar en archivo";
                            strActual = "";
                            strTodos = "";
                        }
                    }
                    else  //SECET
                    {

                        if (this.lstResSA.Count > 1)
                        {
                            //strActual = "El Secretario de estudio y cuenta especializados en tesis";
                            //strTodos = "Los Secretarios de estudio y cuenta especializados en tesis";
                            strActual = "El Secretario actual";
                            strTodos = "Los Secretarios integrantes de la Sala";
                        }
                        else
                        {
                            strMensaje = "Los datos del Secretario de Acuerdos se van a guardar en archivo";
                            strActual = "";
                            strTodos = "";
                        }
                    }
                    break;

                case 6://Sec Estudio y cuenta especializados en tesis

                    iPos = (int)comboBoxInstancia.SelectedIndex;

                    switch (iPos)
                    {

                        case 1:

                            if (lstResSSGAImpr.Count > 1)
                            {
                                strActual = "El Secretario General de Acuerdos";
                                strTodos = "Los Secretario integrantes de la Sala";
                            }
                            else
                            {
                                strMensaje = "Los datos del Subsecretario General de Acuerdos se van a guardar en archivo";
                                strActual = "";
                                strTodos = "";
                            }
                            break;

                        case 2://Primera Sala

                            if (lstResSSGAImpr.Count > 1)
                            {
                                strActual = "Secretario de Estudio y Cuenta Especializado en Tesis";
                                strTodos = "Los Secretarios de Estudio y Cuenta Especializados en Tesis";
                            }
                            else
                            {
                                strMensaje = "Los datos del Secretario de Estudio y Cuenta Especializado en Tesis se van a guardar en archivo";
                                strActual = "";
                                strTodos = "";
                            }
                            break;

                        case 3://Segunda Sala

                            if (lstResST.Count > 1)
                            {
                                strActual = "El Secretario de Estudio y Cuenta \n Especializado en Tesis seleccionado";
                                strTodos = "Los Secretarios de Estudio y Cuenta \n Especializados en Tesis integrantes de la Sala";
                            }
                            else
                            {
                                strMensaje = "Los datos del Secretario de Estudio y Cuenta Especializado en Tesis se van a guardar en archivo";
                                strActual = "";
                                strTodos = "";
                            }
                            break;

                        case 4:
                            break;
                    }
                    break;
            }
            FTransparente.Visibility = Visibility.Visible;

            OpImprimir.StrCabecera = strCabecera;
            OpImprimir.StrMensaje = strMensaje;
            OpImprimir.StrActual = strActual;
            OpImprimir.StrOpcionTodos = strTodos;
            OpImprimir.OptSalida = 1;
            OpImprimir.BringIntoView();
            this.OpImprimir.contenedor = this;
            OpImprimir.Visibility = Visibility.Visible;

            OpImprimir.TomaFondo(FTransparente);
        }

        private void Imprimir_Click(object sender, RoutedEventArgs e)
        {
            String strCabecera = "Opciones de impresión";
            String strMensaje = "Seleccione lo que se va a imprimir";
            String strActual = "";
            String strTodos = "";
            int iPos = -1;

            switch (nEnQueComboEstoy)
            {

                case 0://Pleno
                    strActual = "El Ministro seleccionado";
                    strTodos = "Los Ministros integrantes del Pleno";
                    break;

                case 1: //Presidencia
                    strMensaje = "Los datos del Ministro Presidente se van a imprimir";
                    strActual = "";
                    strTodos = "";
                    break;

                case 2: //Segunda sala
                    strActual = "El Ministro seleccionado";
                    strTodos = "Los Ministros integrantes de la Sala";
                    break;

                case 3: //Primera Sala
                    strActual = "El Ministro seleccionado";
                    strTodos = "Los Ministros integrantes de la Sala";
                    break;

                case 4: //Los integrantes de la ponencia del Ministro
                    strActual = "El integrante de la ponencia seleccionado";
                    strTodos = "Todos los integrantes de la ponencia";
                    break;

                case 5: //Secretario de Acuerdos
                    //strMensaje = "El Secretario general de acuerdos seleccionado se va imprimir";
                    //strActual = "";
                    //strTodos = "";
                    iPos = (int)comboBoxInstancia.SelectedIndex;

                    if (iPos == 1) //pleno
                    {

                        if (lstResSA.Count > 1)
                        {
                            strActual = "El Secretario seleccionado";
                            strTodos = "Los Secretarios integrantes de la Sala";
                        }
                        else
                        {
                            strMensaje = "Los datos del Secretario General de Acuerdos se van a imprimir";
                            strActual = "";
                            strTodos = "";
                        }
                    }
                    else  //SECET
                    {

                        if (this.lstResSA.Count > 1)
                        {
                            //strActual = "El Secretario de estudio y cuenta especializados en tesis";
                            //strTodos = "Los Secretarios de estudio y cuenta especializados en tesis";
                            strActual = "El Secretario actual";
                            strTodos = "Los Secretarios integrantes de la Sala";
                        }
                        else
                        {
                            strMensaje = "Los datos del Secretario de Acuerdos se van a imprimir";
                            strActual = "";
                            strTodos = "";
                        }
                    }
                    break;

                case 6://Sec Estudio y cuenta especializados en tesis

                    iPos = (int)comboBoxInstancia.SelectedIndex;

                    switch (iPos)
                    {

                        case 1:

                            if (lstResSSGAImpr.Count > 1)
                            {
                                strActual = "El Secretario General de Acuerdos";
                                strTodos = "Los Secretario integrantes de la Sala";
                            }
                            else
                            {
                                strMensaje = "Los datos del Subsecretario General de Acuerdos se van a imprimir";
                                strActual = "";
                                strTodos = "";
                            }
                            break;

                        case 2://Primera Sala

                            if (lstResSSGAImpr.Count > 1)
                            {
                                strActual = "Secretario de Estudio y Cuenta Especializado en Tesis";
                                strTodos = "Los Secretarios de Estudio y Cuenta Especializados en Tesis";
                            }
                            else
                            {
                                strMensaje = "Los datos del Secretario de Estudio y Cuenta Especializado en Tesis se van a imprimir";
                                strActual = "";
                                strTodos = "";
                            }
                            break;

                        case 3://Segunda Sala

                            if (lstResST.Count > 1)
                            {
                                strActual = "El Secretario de Estudio y Cuenta \n Especializado en Tesis seleccionado";
                                strTodos = "Los Secretarios de Estudio y Cuenta \n Especializados en Tesis integrantes de la Sala";
                            }
                            else
                            {
                                strMensaje = "Los datos del Secretario de Estudio y Cuenta Especializado en Tesis se van a imprimir";
                                strActual = "";
                                strTodos = "";
                            }
                            break;

                        case 4:
                            break;
                    }
                    break;
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
            String strMensaje = "Seleccione lo que se va a enviar al portapapeles";
            String strActual = "";
            String strTodos = "";
            int iPos;
            int nAnchoVentanaOpciones = -1;

            switch (nEnQueComboEstoy)
            {

                case 0://Pleno
                    strActual = "El Ministro seleccionado";
                    strTodos = "Los Ministros integrantes del Pleno";
                    break;

                case 1: //Presidencia
                    strMensaje = "Los datos del Ministro Presidente se van a enviar al portapapeles";
                    strActual = "";
                    strTodos = "";
                    break;

                case 2: //Segunda sala
                    strActual = "El Ministro seleccionado";
                    strTodos = "Los Ministros integrantes de la Sala";
                    break;

                case 3: //Primera Sala
                    strActual = "El Ministro seleccionado";
                    strTodos = "Los Ministros integrantes de la Sala";
                    break;

                case 4: //Los integrantes de la ponencia del Ministro
                    strActual = "El integrante de la ponencia seleccionado";
                    strTodos = "Todos los integrantes de la ponencia";
                    break;

                case 5: //Secretario de Acuerdos
                    //strMensaje = "El Secretario general de acuerdos seleccionado se van a enviar al portapapeles";
                    strActual = "";
                    strTodos = "";
                    iPos = (int)comboBoxInstancia.SelectedIndex;

                    if (iPos == 1)
                    {

                        if (lstResSGAImpr.Count > 1)
                        {
                            strActual = "El Secretario General de Acuerdos seleccionado";
                            strTodos = "Los Secretarios integrantes de la Sala";
                        }
                        else
                        {
                            strMensaje = "Los datos del Secretario General de Acuerdos se van a enviar al portapapeles";
                            strActual = "";
                            strTodos = "";
                        }
                    }
                    else
                    {

                        if (lstResSA.Count > 1)
                        {
                            strActual = "El Secretario de Acuerdos seleccionado";
                            strTodos = "Los Secretarios de Acuerdos";
                        }
                        else
                        {
                            strMensaje = "Los datos del Secretario de Acuerdos se van a enviar al portapapeles";
                            strActual = "";
                            strTodos = "";
                        }
                    }
                    break;

                case 6://Sec Estudio y cuenta especializados en tesis
                    iPos = (int)comboBoxInstancia.SelectedIndex;

                    switch (iPos)
                    {

                        case 1:

                            if (lstResSSGAImpr.Count > 1)
                            {
                                strActual = "El Secretario General de Acuerdos";
                                strTodos = "Los Secretario integrantes de la Sala";
                            }
                            else
                            {
                                strMensaje = "Los datos del Subsecretario General de Acuerdos se van a enviar al portapapeles";
                                strActual = "";
                                strTodos = "";
                            }
                            break;

                        case 2://Primera Sala

                            if (lstResSSGAImpr.Count > 1)
                            {
                                strActual = "Secretario de Estudio y Cuenta Especializado en Tesis";
                                strTodos = "Los Secretarios de Estudio y Cuenta Especializados en Tesis";
                            }
                            else
                            {
                                strMensaje = "Los datos del Secretario de Estudio y Cuenta Especializado en Tesis se van a enviar al portapapeles";
                                strActual = "";
                                strTodos = "";
                            }
                            break;

                        case 3://Segunda Sala

                            //lstResST
                            if (lstResST.Count > 1)
                            {
                                strActual = "El Secretario de Estudio y Cuenta \n Especializado en Tesis seleccionado";
                                strTodos = "Los Secretarios de Estudio y Cuenta \n Especializados en Tesis integrantes de la Sala";
                            }
                            else
                            {
                                strMensaje = "Los datos del Secretario de Estudio y Cuenta Especializado en Tesis se van a enviar al portapapeles";
                                strActual = "";
                                strTodos = "";
                            }
                            break;

                        case 4:
                            break;
                    }
                    break;
            }

            //FondoTransparente Ftransp = new FondoTransparente();
            FTransparente.Visibility = Visibility.Visible;


            OpImprimir.StrCabecera = strCabecera;
            OpImprimir.StrMensaje = strMensaje;
            OpImprimir.StrActual = strActual;
            OpImprimir.StrOpcionTodos = strTodos;
            OpImprimir.nWidth = nAnchoVentanaOpciones;
            OpImprimir.OptSalida = 0;
            OpImprimir.BringIntoView();
            this.OpImprimir.contenedor = this;
            OpImprimir.Visibility = Visibility.Visible;

            OpImprimir.TomaFondo(FTransparente); 

            
        }

        private void Funcionarios_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            //Funcionarios.SelectedItem = null;
            //Funcionarios.SelectedItems =0;
        }

        private void grdSecAcuerdos_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            //grdSecAcuerdos.SelectedItem = null;
        }

        private void grdSecTesis_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            //grdSecTesis.SelectedItem = null;
        }

        
    }
}
