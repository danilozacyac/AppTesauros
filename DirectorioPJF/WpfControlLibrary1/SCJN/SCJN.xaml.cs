
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using mx.gob.scjn.ius_common.fachade;
using mx.gob.scjn.ius_common.TO;
using mx.gob.scjn.ius_common.util;

namespace mx.gob.scjn.directorio.SCJN
{

    /// <summary>
    /// Interaction logic for SCJN.xaml
    /// </summary>
    public partial class SCJNPage : Page
    {

        public Page Back { get; set; }

        public SCJNPage()
        {
            InitializeComponent();
            //llenaGrid();
            llenaComboSalas();
        }

        public void llenaGrid()
        {

            //List<String> lstAreas = new List<String>();

            //clsListaAreas oAreas = new clsListaAreas();
            //lstAreas = oAreas.TraeDatos();
            //comboBoxInstancia.ItemsSource = lstAreas;
            ////comboBoxSalas.ItemsSource = lstAreas;
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
            //LlenaComboMinistros(0);

        }

        private void LlenaComboMinistros(int Filtro)
        {

            List<DirectorioMinistrosTO> lstRes = new List<DirectorioMinistrosTO>();
            DirectorioMinistrosTO[] R = new DirectorioMinistrosTO[50];
            List<String> lstMinistros = new List<String>();

            //String strPar = Filtro.ToString();

            FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();

            R = fachada.getDirMinistrosXFiltro(Filtro);

            for (int i = 0; i < R.Length; i++)
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

            CargaDetalleMinistro((DirectorioMinistrosTO)comboBoxSalas.SelectedItem);
            fachada.Close();

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
            catch (System.Exception e)
            {
                //Handle exception here
            }
        }

        private void CargaDetalle(object sender, MouseButtonEventArgs e)
        {
            try
            {

                DirectorioPersonasTO Campo = new DirectorioPersonasTO();

                Funcionarios.BringItemIntoView(Funcionarios.SelectedItem);
                Campo = (DirectorioPersonasTO)Funcionarios.SelectedItem;
                textInstancia.Text = Campo.NombrePersona;
                textCargo.Text = Campo.CargoPersona;
                textDomicilio.Text = " Pino Suarez # 2, Col. Centro, Delg. Cuauhtémoc, C.P. 06065, México, D.F. "; //5Campo.DomPersona;
                textTel.Text = Campo.TelPersona + "  " + Campo.ExtPersona;

                //textInstancia.Text = Funcionarios.SelectedItem("1");
                //textInstancia.Text = Funcionarios.CurrentItem("2"); 
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
                textInstancia.Text = Min.TituloPersona + "  " + Min.NombrePersona + "  " + Min.ApellidosPersona;
                textCargo.Text = Min.Cargo;
                textDomicilio.Text = " Pino Suarez # 2, Col. Centro, Delg. Cuauhtémoc, C.P. 06065, México, D.F. "; //5Campo.DomPersona;
                textTel.Text = Min.TelPersona + "  " + Min.ExtPersona;
            }
            catch (System.Exception e)
            {
                //Handle exception here
            }
        }

        private void CargaDetalleSGA(object sender, MouseButtonEventArgs e)
        {
            try
            {
                DirectorioPersonasTO Campo = new DirectorioPersonasTO();

                grdSecAcuerdos.BringItemIntoView(grdSecAcuerdos.SelectedItem);
                Campo = (DirectorioPersonasTO)grdSecAcuerdos.SelectedItem;
                textInstancia.Text = Campo.NombrePersona;
                textCargo.Text = Campo.CargoPersona;
                textDomicilio.Text = " Pino Suarez # 2, Col. Centro, Delg. Cuauhtémoc, C.P. 06065, México, D.F. "; //5Campo.DomPersona;
                textTel.Text = Campo.TelPersona + "  " + Campo.ExtPersona;
                Campo = null;
            }
            catch (System.Exception error)
            {
                //Handle exception here
            }
        }

        private void CargaDetalleSecTesis(object sender, MouseButtonEventArgs e)
        {

            try
            {

                DirectorioPersonasTO Campo = new DirectorioPersonasTO();

                grdSecTesis.BringItemIntoView(grdSecTesis.SelectedItem);
                Campo = (DirectorioPersonasTO)grdSecTesis.SelectedItem;
                textInstancia.Text = Campo.NombrePersona;
                textCargo.Text = Campo.CargoPersona;
                textDomicilio.Text = " Pino Suarez # 2, Col. Centro, Delg. Cuauhtémoc, C.P. 06065, México, D.F. "; //5Campo.DomPersona;
                textTel.Text = Campo.TelPersona + "  " + Campo.ExtPersona;
                Campo = null;
            }

            catch
            {

            }
        }

        private void comboBoxSalas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int Pos = this.comboBoxSalas.SelectedIndex;

            // estamos validando cuando el selecteditem es -1
            if (Pos >= 0)
            {
                DirectorioMinistrosTO seleccionado = (DirectorioMinistrosTO)this.comboBoxSalas.SelectedItem;
                int Sel = seleccionado.IdPonencia;

                List<DirectorioPersonasTO> lstRes = new List<DirectorioPersonasTO>();
                DirectorioPersonasTO[] R = new DirectorioPersonasTO[50];

                FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();

                R = fachada.getDirPersonas(Sel.ToString());

                for (int i = 0; i < R.Length; i++)
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

                this.Funcionarios.ItemsSource = lstRes;
                //this.Funcionarios.SelectedIndex = 0;

                CargaDetalleMinistro((DirectorioMinistrosTO)comboBoxSalas.SelectedItem);

                fachada.Close();

            }

        }

        private void TraeSecretarios(int Sala)
        {

            TraeSGA(Sala);

            TraeSecTesis(Sala);

        }

        private void TraeSGA(int Sala)
        {
            try
            {

                List<DirectorioPersonasTO> lstRes = new List<DirectorioPersonasTO>();
                DirectorioPersonasTO[] R = new DirectorioPersonasTO[50];

                FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();

                //Sabemos que el puesto del SGA es el 16
                R = fachada.getDirPersonasXPuestoYSala("16", Sala.ToString());

                for (int i = 0; i < R.Length; i++)
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
            catch (System.Exception e)
            {
                //Handle exception here
            }
        }

        private void TraeSecTesis(int Sala)
        {
            try
            {

                List<DirectorioPersonasTO> lstRes = new List<DirectorioPersonasTO>();
                DirectorioPersonasTO[] R = new DirectorioPersonasTO[50];

                FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();

                //Sabemos que el puesto del Sectesis es el 22
                R = fachada.getDirPersonasXPuestoYSala("22", Sala.ToString());

                for (int i = 0; i < R.Length; i++)
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
            catch (System.Exception e)
            {
                //Handle exception here
            }
        }

        private void TraeSecgralAcuerdos(int Sala)
        {
            try
            {

                List<DirectorioPersonasTO> lstRes = new List<DirectorioPersonasTO>();
                DirectorioPersonasTO[] R = new DirectorioPersonasTO[50];

                FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();

                //Sabemos que el puesto del Sec Gral de Acuerdos es el 6
                R = fachada.getDirPersonasXPuestoYSala("6", Sala.ToString());

                for (int i = 0; i < R.Length; i++)
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
            catch (System.Exception e)
            {
                //Handle exception here
            }
        }

        private void TraeSubSecGralAcuerdos(int Sala)
        {
            try
            {
                List<DirectorioPersonasTO> lstRes = new List<DirectorioPersonasTO>();
                DirectorioPersonasTO[] R = new DirectorioPersonasTO[50];
                FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();

                //Sabemos que el puesto del Sectesis es el 22
                R = fachada.getDirPersonasXPuestoYSala("11", Sala.ToString());

                for (int i = 0; i < R.Length; i++)
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
            catch (System.Exception e)
            {
                //Handle exception here
            }
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
                    this.lblSGA.Content = "SUBSECRETARIO GENERAL DE ACUERDOS";
                    this.lblSecTes.Content = "SECRETARIO GENERAL DE ACUERDOS";

                    break;

                case 2://PS
                    this.lblPonencia.Content = "PONENCIA";
                    this.lblSGA.Content = "SECRETARIO DE ACUERDOS";
                    this.lblSecTes.Content = "SECRETARIO DE ESTUDIO Y CUENTA ESPECIALIZADO EN TESIS";
                    break;

                case 3://SS
                    this.lblPonencia.Content = "PONENCIA";
                    this.lblSGA.Content = "SECRETARIO DE ACUERDOS";
                    this.lblSecTes.Content = "SECRETARIO DE ESTUDIO Y CUENTA ESPECIALIZADO EN TESIS";

                    break;
            }
        }



        private void comboBoxInstancia_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                int iPos;
                iPos = (int)comboBoxInstancia.SelectedIndex;

                LimpiaGridsLista();
                LimpiaDetalles();
                LimpiaDetallesSec();
                CambiaEdolbl(iPos);

                switch (iPos)
                {
                    case 0://Presidencia
                        LlenaComboMinistros(0);
                        break;
                    case 1: //PLENO
                        LlenaComboMinistros(3);
                        //TraeSecretarios(1);
                        TraeSecgralAcuerdos(0);
                        TraeSubSecGralAcuerdos(0);
                        break;

                    case 2://PS
                        LlenaComboMinistros(1);
                        TraeSecretarios(1);
                        break;
                    case 3://SS
                        LlenaComboMinistros(2);
                        TraeSecretarios(2);
                        break;


                    default: break;
                }
            }
            catch (System.Exception error)
            {
                //Handle exception here
            }
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

    }

}
