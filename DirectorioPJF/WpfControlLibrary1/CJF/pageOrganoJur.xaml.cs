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

    /// <summary>
    /// Interaction logic for pageOrganoJur.xaml
    /// </summary>
    public partial class pageOrganoJur : Page
    {
        int nTipoOJ = -1;
        String enterKey = "\n";
        /*Para la impresión ***********************************************/

        private FlowDocument DocumentoParaCopiar { get; set; }
        private AcuerdosTO DocumentoActual;
        String strOJImpr = "";
        List<DirectorioOrgJurTO> lstParaImprimir = new List<DirectorioOrgJurTO>();
        List<DirectorioPersonasTO> lstTitularesImpr = new List<DirectorioPersonasTO>();
        /******************************************************************/
        List<DirectorioCatalogosTO> lstRegiones = new List<DirectorioCatalogosTO>();
        Boolean bEstoyModificando = false;

        public Page Back { get; set; }

        private String strOJ = "";

        public pageOrganoJur()
        {
            InitializeComponent();
            //if (!BrowserInteropHelper.IsBrowserHosted)
            //{ Guardar_.Visibility =  Visibility.Visible;  }
            //else { Guardar_.Visibility = Visibility.Hidden; }
            TraeOrganoJur("TCC");
        }

        public pageOrganoJur(string Filtro)
        {
            int nTpo = -1;

            switch (Filtro)
            {

                case "TUC": nTpo = 1; break;

                case "JUZ": nTpo = 2; break;

                case "TCC": nTpo = 3; break;
                default: nTpo = -1; break;
            }
            nTipoOJ = nTpo;
            bEstoyModificando = true;
            InitializeComponent();
            lstParaImprimir = TraeOrganoJur(Filtro);
            llenaComboCircuitoXOJ(nTpo);
            llenaComboOrdinalXOJ(nTpo);
            llenaComboMateriaXOJ(nTpo);
            llenaComboOA(nTpo);
            lstRegiones = llenaComboCA(nTpo);
            strOJ = Filtro;
            bEstoyModificando = false;

            if (!BrowserInteropHelper.IsBrowserHosted)
            { Guardar_.Visibility = Visibility.Visible; }
            else { Guardar_.Visibility = Visibility.Hidden; }
        }

        private List<DirectorioOrgJurTO> TraeOrganoJur(string Filtro)
        {
            List<DirectorioOrgJurTO> lstRes = new List<DirectorioOrgJurTO>();
#if STAND_ALONE
            List<DirectorioOrgJurTO> R = null;
            FachadaBusquedaTradicional fachada = new FachadaBusquedaTradicional();
#else
            DirectorioOrgJurTO[] R = new DirectorioOrgJurTO[50];
            FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
#endif
            R = fachada.getDirOrganosJur(Filtro);
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
                //Campo.TelOrganoJur = R[i].TelOrganoJur;
                lstRes.Add(Campo);
                Campo = null;
            }
            this.grdOrganismos.ItemsSource = lstRes;
            this.grdOrganismos.SelectedIndex = 0;
            this.grdOrganismos.SelectedItem = this.grdOrganismos.Items.CurrentItem;
            CargaDetalleOJ((DirectorioOrgJurTO)grdOrganismos.SelectedItem);
#if STAND_ALONE
            int ntot = R.Count;
#else
            int ntot = R.Length;
#endif
            textCuantos.Text = "Total: " + ntot.ToString();
            fachada.Close();
            return lstRes;
        }

        private void CargaDetalle(object sender, MouseButtonEventArgs e)
        {

            try
            {
                CargaDetalleOJ((DirectorioOrgJurTO)grdOrganismos.SelectedItem);
            }

            catch { }
        }

        private void CargaDetalleOJ(DirectorioOrgJurTO Campo)
        {

            try
            {
                this.txtNombreOrg.Text = Campo.NombreOrganoJur;
                txtDomicilio.Text = Campo.DomOrganoJur;
                lstTitularesImpr = CargaTitulares(Campo.IdOrganoJur.ToString());
                strOJImpr = Campo.NombreOrganoJur + enterKey;
                strOJImpr = strOJImpr + Campo.DomOrganoJur + enterKey;
            }

            catch { }
        }

        private List<DirectorioPersonasTO> CargaTitulares(String IdOrg)
        {
            List<DirectorioPersonasTO> lstRes = new List<DirectorioPersonasTO>();
#if STAND_ALONE
            List<DirectorioPersonasTO> R = null;
            FachadaBusquedaTradicional fachada = new FachadaBusquedaTradicional();
#else
            DirectorioPersonasTO[] R = new DirectorioPersonasTO[50];
            FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
#endif
            R = fachada.getDirTitularesXOJ(IdOrg);
#if STAND_ALONE

            for (int i = 0; i < R.Count; i++)
#else

            for (int i = 0; i < R.Length; i++)
#endif
            {
                DirectorioPersonasTO Campo = new DirectorioPersonasTO();
                Campo.NombrePersona = R[i].NombrePersona;
                Campo.TelPersona = R[i].TelPersona;
                lstRes.Add(Campo);
                Campo = null;
            }
#if STAND_ALONE
            int ntot = R.Count;
#else
            int ntot = R.Length;
#endif

            if (ntot > 1)
            {
                lblAreaAdmin.Content = "TITULARES";
            }
            else
            {
                lblAreaAdmin.Content = "TITULAR";
            }
            this.Funcionarios.ItemsSource = lstRes;
            fachada.Close();
            return lstRes;
        }

        private void Salir_ImageFailed(object sender, ExceptionRoutedEventArgs e)
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

        private void llenaComboOA(int nTpo)
        {
            List<String> lstRes = new List<String>();
            lstRes.Add("OTROS");
            lstRes.Add("ÓRGANOS JURISDICCIONALES AUXILIARES");
            lstRes.Add("CENTROS AUXILIARES");
            this.comboOA.ItemsSource = lstRes;
            comboOA.SelectedIndex = 0;

            if (bEstoyModificando) { comboCA.Visibility = Visibility.Hidden; }
            //switch (nTpo) {
            //    case 1: 
            //        this.comboOA.ItemsSource = lstRes;
            //        comboOA.SelectedIndex = 2;
            //        break;
            //    case 2: 
            //        this.comboOA.ItemsSource = lstRes;
            //        comboOA.SelectedIndex = 2;
            //        break;
            //    case 3:
            //        this.comboOA.ItemsSource = lstRes;
            //        comboOA.SelectedIndex = 1;
            //        break;
            //}
        }

        private List<DirectorioCatalogosTO> llenaComboCA(int nTipo)
        {
            List<DirectorioCatalogosTO> lstRes = new List<DirectorioCatalogosTO>();
#if STAND_ALONE
            List<DirectorioCatalogosTO> R = null;
            FachadaBusquedaTradicional fachada = new FachadaBusquedaTradicional();
#else
            DirectorioCatalogosTO[] R = new DirectorioCatalogosTO[50];
            FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
#endif
            DirectorioCatalogosTO CampoInicial = new DirectorioCatalogosTO();
            //Agregamos el primer elemento
            CampoInicial.NombreElemento = "CENTROS AUXILIARES";
            CampoInicial.IdElemento = -1;
            lstRes.Add(CampoInicial);
            //R = fachada.getDirCatalogo(4);
            R = fachada.getDirCatalogoXTipo(4, nTipo);
#if STAND_ALONE

            for (int i = 0; i < R.Count; i++)
#else

            for (int i = 0; i < R.Length; i++)
#endif
            {
                DirectorioCatalogosTO Campo = new DirectorioCatalogosTO();
                Campo.NombreElemento = R[i].NombreElemento;
                Campo.IdElemento = R[i].IdElemento;
                Campo.Orden = R[i].Orden;
                lstRes.Add(Campo);
                Campo = null;
            }
            this.comboCA.ItemsSource = lstRes;
            comboCA.SelectedIndex = 0;
            fachada.Close();
            return lstRes;
        }

        private void llenaComboOrdinal()
        {
            List<DirectorioCatalogosTO> lstRes = new List<DirectorioCatalogosTO>();
#if STAND_ALONE
            List<DirectorioCatalogosTO> R = null;
            FachadaBusquedaTradicional fachada = new FachadaBusquedaTradicional();
#else
            DirectorioCatalogosTO[] R = new DirectorioCatalogosTO[50];
            FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
#endif
            DirectorioCatalogosTO CampoInicial = new DirectorioCatalogosTO();
            //Agregamos el primer elemento
            CampoInicial.NombreElemento = "Ordinal";
            CampoInicial.IdElemento = -1;
            lstRes.Add(CampoInicial);
            R = fachada.getDirCatalogo(3);
#if STAND_ALONE
            for (int i = 0; i < R.Count; i++)
#else
            for (int i = 0; i < R.Length; i++)
#endif
            {
                DirectorioCatalogosTO Campo = new DirectorioCatalogosTO();
                Campo.NombreElemento = R[i].NombreElemento;
                Campo.IdElemento = R[i].IdElemento;
                lstRes.Add(Campo);
                Campo = null;
            }
            DirectorioCatalogosTO CampoLast = new DirectorioCatalogosTO();
            CampoLast.NombreElemento = "Todos";
            CampoLast.IdElemento = 0;
            lstRes.Add(CampoLast);
            this.comboOrdinal.ItemsSource = lstRes;
            comboOrdinal.SelectedIndex = 0;
            fachada.Close();
        }

        private void llenaComboOrdinalXOJ(int nTipo)
        {
            List<DirectorioCatalogosTO> lstRes = new List<DirectorioCatalogosTO>();
#if STAND_ALONE
            List<DirectorioCatalogosTO> R = null;
            FachadaBusquedaTradicional fachada = new FachadaBusquedaTradicional();
#else
            DirectorioCatalogosTO[] R = new DirectorioCatalogosTO[50];
            FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
#endif
            DirectorioCatalogosTO CampoInicial = new DirectorioCatalogosTO();
            //Agregamos el primer elemento
            CampoInicial.NombreElemento = "Todos";
            CampoInicial.IdElemento = -1;
            lstRes.Add(CampoInicial);
            R = fachada.getDirCatalogoXTipo(3, nTipo);
#if STAND_ALONE
            for (int i = 0; i < R.Count; i++)
#else
            for (int i = 0; i < R.Length; i++)
#endif
            {
                DirectorioCatalogosTO Campo = new DirectorioCatalogosTO();
                Campo.NombreElemento = R[i].NombreElemento;
                Campo.IdElemento = R[i].IdElemento;
                lstRes.Add(Campo);
                Campo = null;
            }
            DirectorioCatalogosTO CampoLast = new DirectorioCatalogosTO();
            CampoLast.NombreElemento = "Todos";
            CampoLast.IdElemento = 0;
            //lstRes.Add(CampoLast);
            this.comboOrdinal.ItemsSource = lstRes;
            comboOrdinal.SelectedIndex = 0;
            fachada.Close();
        }

        private void llenaComboMateria()
        {
            List<DirectorioCatalogosTO> lstRes = new List<DirectorioCatalogosTO>();
#if STAND_ALONE
            List<DirectorioCatalogosTO> R = null;
            FachadaBusquedaTradicional fachada = new FachadaBusquedaTradicional();
#else
            DirectorioCatalogosTO[] R = new DirectorioCatalogosTO[50];
            FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
#endif
            //Agregamos el primer elemento
            DirectorioCatalogosTO CampoInicial = new DirectorioCatalogosTO();
            CampoInicial.NombreElemento = "Materia";
            CampoInicial.IdElemento = -1;
            lstRes.Add(CampoInicial);
            R = fachada.getDirCatalogo(2);
#if STAND_ALONE
            for (int i = 0; i < R.Count; i++)
#else
            for (int i = 0; i < R.Length; i++)
#endif
            {
                DirectorioCatalogosTO Campo = new DirectorioCatalogosTO();
                Campo.NombreElemento = R[i].NombreElemento;
                Campo.IdElemento = R[i].IdElemento;
                lstRes.Add(Campo);
                Campo = null;
            }
            DirectorioCatalogosTO CampoLast = new DirectorioCatalogosTO();
            CampoLast.NombreElemento = "Todos";
            CampoLast.IdElemento = 0;
            lstRes.Add(CampoLast);
            this.comboMateria.ItemsSource = lstRes;
            comboMateria.SelectedIndex = 0;
            fachada.Close();
        }

        private void llenaComboMateriaXOJ(int nTipo)
        {
            List<DirectorioCatalogosTO> lstRes = new List<DirectorioCatalogosTO>();
#if STAND_ALONE
            List<DirectorioCatalogosTO> R = null;
            FachadaBusquedaTradicional fachada = new FachadaBusquedaTradicional();
#else
            DirectorioCatalogosTO[] R = new DirectorioCatalogosTO[50];
            FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
#endif
            //Agregamos el primer elemento
            DirectorioCatalogosTO CampoInicial = new DirectorioCatalogosTO();
            CampoInicial.NombreElemento = "Materia";
            CampoInicial.IdElemento = -1;
            lstRes.Add(CampoInicial);
            R = fachada.getDirCatalogoXTipo(2, nTipo);
#if STAND_ALONE
            for (int i = 0; i < R.Count; i++)
#else
            for (int i = 0; i < R.Length; i++)
#endif
            {
                DirectorioCatalogosTO Campo = new DirectorioCatalogosTO();
                Campo.NombreElemento = R[i].NombreElemento;
                Campo.IdElemento = R[i].IdElemento;
                lstRes.Add(Campo);
                Campo = null;
            }
            DirectorioCatalogosTO CampoLast = new DirectorioCatalogosTO();
            CampoLast.NombreElemento = "Todos";
            CampoLast.IdElemento = 0;
            lstRes.Add(CampoLast);
            this.comboMateria.ItemsSource = lstRes;
            comboMateria.SelectedIndex = 0;
            fachada.Close();
        }

        private void llenaComboCircuito()
        {
            List<DirectorioCatalogosTO> lstRes = new List<DirectorioCatalogosTO>();
#if STAND_ALONE
            List<DirectorioCatalogosTO> R = null;
            FachadaBusquedaTradicional fachada = new FachadaBusquedaTradicional();
#else
            DirectorioCatalogosTO[] R = new DirectorioCatalogosTO[50];
            FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
#endif
            //Agregamos el primer elemento
            DirectorioCatalogosTO CampoInicial = new DirectorioCatalogosTO();
            CampoInicial.NombreElemento = "Todos";
            CampoInicial.IdElemento = -1;
            lstRes.Add(CampoInicial);
            R = fachada.getDirCatalogo(1);
#if STAND_ALONE
            for (int i = 0; i < R.Count; i++)
#else
            for (int i = 0; i < R.Length; i++)
#endif
            {
                DirectorioCatalogosTO Campo = new DirectorioCatalogosTO();
                Campo.NombreElemento = R[i].NombreElemento;
                Campo.IdElemento = R[i].IdElemento;
                lstRes.Add(Campo);
                Campo = null;
            }
            DirectorioCatalogosTO CampoLast = new DirectorioCatalogosTO();
            CampoLast.NombreElemento = "Todos";
            CampoLast.IdElemento = 0;
            //lstRes.Add(CampoLast);
            this.comboCircuito.ItemsSource = lstRes;
            comboCircuito.SelectedIndex = 0;
            fachada.Close();
        }

        private void llenaComboCircuitoXOJ(int nTipo)
        {
            List<DirectorioCatalogosTO> lstRes = new List<DirectorioCatalogosTO>();
#if STAND_ALONE
            List<DirectorioCatalogosTO> R = null;
            FachadaBusquedaTradicional fachada = new FachadaBusquedaTradicional();
#else
            DirectorioCatalogosTO[] R = new DirectorioCatalogosTO[50];
            FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
#endif
            //Agregamos el primer elemento
            DirectorioCatalogosTO CampoInicial = new DirectorioCatalogosTO();
            CampoInicial.NombreElemento = "Todos";
            CampoInicial.IdElemento = -1;
            lstRes.Add(CampoInicial);
            R = fachada.getDirCatalogoXTipo(1, nTipo);
#if STAND_ALONE
            for (int i = 0; i < R.Count; i++)
#else
            for (int i = 0; i < R.Length; i++)
#endif
            {
                DirectorioCatalogosTO Campo = new DirectorioCatalogosTO();
                Campo.NombreElemento = R[i].NombreElemento;
                Campo.IdElemento = R[i].IdElemento;
                lstRes.Add(Campo);
                Campo = null;
            }
            DirectorioCatalogosTO CampoLast = new DirectorioCatalogosTO();
            CampoLast.NombreElemento = "Todos";
            CampoLast.IdElemento = 0;
            //lstRes.Add(CampoLast);
            this.comboCircuito.ItemsSource = lstRes;
            comboCircuito.SelectedIndex = 0;
            fachada.Close();
        }

        private void btnFiltrar_Click(object sender, RoutedEventArgs e)
        {
            int iCto = -1;
            DirectorioCatalogosTO dTmpCto = new DirectorioCatalogosTO();

            if (comboCircuito.SelectedIndex > 0)
            {
                dTmpCto = (DirectorioCatalogosTO)comboCircuito.SelectedItem;
                iCto = dTmpCto.IdElemento;
            }
            int iOrd = -1;
            DirectorioCatalogosTO dTmpOrd = new DirectorioCatalogosTO();

            if (this.comboOrdinal.SelectedIndex > 0)
            {
                dTmpOrd = (DirectorioCatalogosTO)comboOrdinal.SelectedItem;
                iOrd = dTmpOrd.IdElemento;
            }
            int iMat = -1;
            DirectorioCatalogosTO dTmpMat = new DirectorioCatalogosTO();

            if (this.comboMateria.SelectedIndex > 0)
            {
                dTmpMat = (DirectorioCatalogosTO)comboMateria.SelectedItem;
                iMat = dTmpMat.IdElemento;
            }
            lstParaImprimir = TraeOrganoJurXFiltro(strOJ, iOrd, iMat, iCto, 0, lstParaImprimir);
        }

        private List<DirectorioOrgJurTO> TraeOrganoJurXFiltro(string Filtro, int ord, int mat, int cto, int Region, List<DirectorioOrgJurTO> lstOriginal)
        {
            this.Cursor = Cursors.Wait;


            List<DirectorioOrgJurTO> lstRes = new List<DirectorioOrgJurTO>();
#if STAND_ALONE
            List<DirectorioOrgJurTO> R = null;
            FachadaBusquedaTradicional fachada = new FachadaBusquedaTradicional();
#else
            DirectorioOrgJurTO[] R = new DirectorioOrgJurTO[50];
            FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
#endif
            R = fachada.getDirOrganosJurXFiltro(Filtro, ord, mat, cto, Region);
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
                //Campo.TelOrganoJur = R[i].TelOrganoJur;
                lstRes.Add(Campo);
                Campo = null;
            }
#if STAND_ALONE
            int ntot = R.Count;
#else
            int ntot = R.Length;
#endif

            this.Cursor = Cursors.Arrow;


            if (ntot > 0)
            {
                textCuantos.Text = "Total: " + ntot.ToString();
                this.grdOrganismos.ItemsSource = lstRes;
                this.grdOrganismos.SelectedIndex = 0;
                this.grdOrganismos.SelectedItem = this.grdOrganismos.Items.CurrentItem;
                CargaDetalleOJ((DirectorioOrgJurTO)grdOrganismos.SelectedItem);
                fachada.Close();
                return lstRes;
            }
            else
            {
                MessageBox.Show("No existen organismos jurisdiccionales con los criterios solicitados", MensajesDirectorio.TITULO_MENSAJES);
                fachada.Close();
                return lstOriginal;
            }
        }

        private void btnTodos_Click(object sender, RoutedEventArgs e)
        {
            comboOA.IsEnabled = false;
            comboCA.IsEnabled = false;
            chkOtros.IsChecked = false;
            lstParaImprimir = TraeOrganoJurXFiltro(strOJ, 0, 0, 0, 0, lstParaImprimir);
        }
        # region CHECK UNCHECK De los filtros

        private void chkOrdinal_Checked(object sender, RoutedEventArgs e)
        {
            this.comboOrdinal.IsEnabled = true;
            comboCA.Opacity = .05;
            comboOA.IsEnabled = false;
        }

        private void chkOrdinal_Unchecked(object sender, RoutedEventArgs e)
        {
            comboOrdinal.SelectedIndex = 0;
            comboOA.IsEnabled = false;
            this.comboOrdinal.IsEnabled = false;
        }

        private void chkMateria_Checked(object sender, RoutedEventArgs e)
        {
            this.chkOtros.IsChecked = false;
            comboOA.IsEnabled = false;
            comboCA.Opacity = .05;
            this.comboMateria.IsEnabled = true;
        }

        private void chkMateria_Unchecked(object sender, RoutedEventArgs e)
        {
            comboMateria.SelectedIndex = 0;
            this.comboMateria.IsEnabled = false;
        }

        private void chkCircuito_Checked(object sender, RoutedEventArgs e)
        {
            this.chkOtros.IsChecked = false;
            comboOA.IsEnabled = false;
            comboCA.Opacity = .05;
            this.comboCircuito.IsEnabled = true;
        }

        private void chkCircuito_Unchecked(object sender, RoutedEventArgs e)
        {
            this.chkOtros.IsChecked = false;
            comboCircuito.SelectedIndex = 0;
            this.comboCircuito.IsEnabled = false;
        }

        private void chkTodos_Checked(object sender, RoutedEventArgs e)
        {
        }

        private void chkOtros_Checked(object sender, RoutedEventArgs e)
        {
            comboOA.IsEnabled = true;
            this.chkCircuito.IsChecked = false;
            this.chkMateria.IsChecked = false;
            this.chkOrdinal.IsChecked = false;

            switch (nTipoOJ)
            {

                case 1:
                    comboOA.SelectedIndex = 2;
                    comboCA.Opacity = 1.0;
                    comboCA.Visibility = Visibility.Visible;
                    comboCA.IsEnabled = true;
                    break;

                case 2:
                    comboOA.SelectedIndex = 0;
                    lstParaImprimir = TraeOrganoJurXFiltro(strOJ, 0, 50, 50, 50, lstParaImprimir);
                    break;

                case 3:
                    comboOA.SelectedIndex = 1; break;
            }
        }

        private void chkOtros_Unchecked(object sender, RoutedEventArgs e)
        {
            comboOA.IsEnabled = false;
            comboCA.Opacity = .05;
            comboCA.SelectedIndex = 0;
            comboCA.IsEnabled = false;
            lstParaImprimir = TraeOrganoJurXFiltro(strOJ, 0, 0, 0, 0, lstParaImprimir);
        }
        # endregion CHECK UNCHECK

        private void comboCA_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            DirectorioCatalogosTO CampoActual = new DirectorioCatalogosTO();


            if (strOJ.Length > 0)
            {
                //                int nReg = comboCA.SelectedIndex + 1;
                //int nReg = comboCA.SelectedIndex;

                CampoActual = lstRegiones[comboCA.SelectedIndex];

                int nReg = CampoActual.Orden;// lstRegiones.Index(comboCA.SelectedIndex).ToString(); 


                if (nReg >= 1)
                {
                    lstParaImprimir = TraeOrganoJurXFiltro(strOJ, 0, 0, 0, nReg, lstParaImprimir);
                }
            }
        }

        private void comboOA_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            try
            {

                if (bEstoyModificando) { }
                else
                {
                    int nOp = comboOA.SelectedIndex;

                    switch (nOp)
                    {

                        case 0: //Estado Inicial, Otros: Juzgados y cateos
                            comboCA.Opacity = .05;
                            comboCA.SelectedIndex = 0;
                            comboCA.IsEnabled = false;
                            lstParaImprimir = TraeOrganoJurXFiltro(strOJ, 0, 50, 50, 50, lstParaImprimir);
                            break;

                        case 1: // OJA que no son de cateos ni pertenecen a regiones
                            comboCA.Visibility = Visibility.Visible;
                            comboCA.Opacity = .1;
                            comboCA.SelectedIndex = 0;
                            comboCA.IsEnabled = false;
                            lstParaImprimir = TraeOrganoJurXFiltro(strOJ, 0, 0, 0, 40, lstParaImprimir);
                            break;

                        case 2:
                            comboCA.Visibility = Visibility.Visible;
                            comboCA.Opacity = 1.0;
                            comboCA.IsEnabled = true;
                            break;
                    }
                }
            }

            catch (System.Exception error)
            {
                //Handle exception here
            }
        }

        private void circuitos_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
        }

        #region Sección Almacenamiento

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

        private void Guardar(object sender, MouseButtonEventArgs e)
        {
            //String strCabecera = "Opciones de almacenamiento";
            //String strMensaje = "Seleccione lo que se va a guardar en disco ";
            //String strActual = "";
            //String strTodos = "";
            //int nT = -1;
            //OpcionesImprimir opMinistros = new OpcionesImprimir();
            //opMinistros.Visibility = Visibility.Visible;
            //nT = lstParaImprimir.Count();

            //if (nT > 1)
            //{
            //    strActual = "Órgano Jurisdiccional actual";
            //    strTodos = "Todos los órganos jurisdiccionales";
            //}
            //else if (nT == 1)
            //{
            //    strMensaje = "Los datos del Órgano Jurisdiccional se van a guardar en disco";
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
            ////OpImprimir.StrActual = "Órgano Jurisdiccional actual";
            ////OpImprimir.StrOpcionTodos = "Todos los órganos jurisdiccionales";
            ////OpImprimir.OptSalida = 1;
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

            //nT = lstParaImprimir.Count();

            //if (nT > 1)
            //{
            //    strActual = "Órgano Jurisdiccional actual";
            //    strTodos = "Todos los órganos jurisdiccionales";
            //}
            //else if (nT == 1)
            //{
            //    strMensaje = "Los datos del Órgano Jurisdiccional se van a imprimir";
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

            ////OpcionesImprimir opMinistros = new OpcionesImprimir();
            ////opMinistros.Visibility = Visibility.Visible;
            ////OpImprimir.StrCabecera = "Opciones de impresión";
            ////OpImprimir.StrMensaje = "Seleccione lo que se va a Imprimir ";
            ////OpImprimir.StrActual = "Órgano Jurisdiccional actual";
            ////OpImprimir.StrOpcionTodos = "Todos los órganos jurisdiccionales";
            ////OpImprimir.OptSalida = 2;
            ////OpImprimir.BringIntoView();
            ////this.OpImprimir.contenedor = this;
            ////OpImprimir.Visibility = Visibility.Visible;
        }

        private void PortaPapeles(object sender, MouseButtonEventArgs e)
        {

            String strCabecera = "Opciones de almacenamiento";
            String strMensaje = "Seleccione lo que se va a enviar al portapapeles ";
            String strActual = "";
            String strTodos = "";
            int nT = -1;
            OpcionesImprimir opMinistros = new OpcionesImprimir();
            opMinistros.Visibility = Visibility.Visible;
            nT = lstParaImprimir.Count();

            if (nT > 1)
            {
                strActual = "Órgano Jurisdiccional actual";
                strTodos = "Todos los Órganos Jurisdiccionales";
            }
            else if (nT == 1)
            {
                strMensaje = "Los datos del Órgano Jurisdiccional actual se van a enviar al portapapeles";
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
            //OpImprimir.StrActual = "Órgano Jurisdiccional actual";
            //OpImprimir.StrOpcionTodos = "Todos los Órganos jurisdiccionales";
            //OpImprimir.OptSalida = 0;
            //OpImprimir.BringIntoView();
            //this.OpImprimir.contenedor = this;
            //OpImprimir.Visibility = Visibility.Visible;
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

        public void ImprimeListado(Boolean bTodo, Int32 nSelSalida)
        {
            String strText = "";

            List<DirectorioPersonasTO> lstTitulares = new List<DirectorioPersonasTO>();

            if (bTodo == true) //Imprimir todos 
            {

                foreach (DirectorioOrgJurTO ItemIntegrante in lstParaImprimir)
                {
                    strText = strText + ItemIntegrante.NombreOrganoJur + enterKey;
                    strText = strText + ItemIntegrante.DomOrganoJur + enterKey;

                    lstTitulares = CargaTitulares(ItemIntegrante.IdOrganoJur.ToString());

                    foreach (DirectorioPersonasTO ItemTitular in lstTitulares)
                    {
                        strText = strText + ItemTitular.NombrePersona + enterKey;
                        //strText = strText + ItemTitular.ExtPersona  + enterKey;
                        strText = strText + "Tel." + ItemTitular.TelPersona + enterKey;

                    }




                    //strText = strText + "Teléfono: " + ItemIntegrante.TelOrganoJur + enterKey;
                    //strText = strText + "Extensión: " + ItemIntegrante.ExtOrganoJur + enterKey;
                    strText = strText + enterKey;
                }
            }
            else   //Imprimir sólo el actual
            {
                strText = strOJImpr + enterKey + enterKey;
                strText = strText + "Titulares " + enterKey + enterKey;

                foreach (DirectorioPersonasTO ItemIntegrante in lstTitularesImpr)
                {
                    strText = strText + ItemIntegrante.NombrePersona + enterKey;
                    strText = strText + "Teléfono: " + ItemIntegrante.TelPersona + enterKey;
                    strText = strText + enterKey;
                }
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

        private void PortaPapStr(String strTexto)
        {
            contenidoTexto = new RichTextBox();
            contenidoTexto.AppendText(strTexto);
            contenidoTexto.SelectAll();
            contenidoTexto.Copy();
            FTransparente.Visibility = Visibility.Visible;
            Aviso.TomaFondo(FTransparente);

            Aviso.StrCabecera = mx.gob.scjn.directorio.MensajesDirectorio.TITULO_PORTAPAPELES_DIR;
            Aviso.StrMensaje = mx.gob.scjn.directorio.MensajesDirectorio.MENSAJE_PORTAPAPELES_DIR;
            Aviso.Visibility = Visibility.Visible;
            Aviso.Background = Brushes.White;


        }

        #endregion

        private void Guardar_Click(object sender, RoutedEventArgs e)
        {
            String strCabecera = "Opciones de almacenamiento";
            String strMensaje = "Seleccione lo que se va a guardar en disco ";
            String strActual = "";
            String strTodos = "";
            int nT = -1;
            OpcionesImprimir opMinistros = new OpcionesImprimir();
            opMinistros.Visibility = Visibility.Visible;
            nT = lstParaImprimir.Count();

            if (nT > 1)
            {
                strActual = "Órgano Jurisdiccional actual";
                strTodos = "Todos los Órganos Jurisdiccionales";
            }
            else if (nT == 1)
            {
                strMensaje = "Los datos del Órgano Jurisdiccional se van a guardar en disco";
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

            nT = lstParaImprimir.Count();

            if (nT > 1)
            {
                strActual = "Órgano Jurisdiccional actual";
                strTodos = "Todos los Órganos Jurisdiccionales";
            }
            else if (nT == 1)
            {
                strMensaje = "Los datos del Órgano Jurisdiccional se van a imprimir";
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
            nT = lstParaImprimir.Count();

            if (nT > 1)
            {
                strActual = "Órgano Jurisdiccional actual";
                strTodos = "Todos los Órganos Jurisdiccionales";
            }
            else if (nT == 1)
            {
                strMensaje = "Los datos del Órgano Jurisdiccional actual se van a enviar al portapapeles";
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

        }

    }
}
