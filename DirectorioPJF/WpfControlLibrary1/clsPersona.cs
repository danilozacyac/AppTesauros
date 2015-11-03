using System;
using System.Collections.Generic;
using System.Linq;
//using IUSTOWeb.TO.Comparador;
using mx.gob.scjn.ius_common.fachade;

namespace mx.gob.scjn.directorio
{

    class clsPersona
    {

        #region Propiedades de la clase

        private int nId;
        private String strNombreCompleto;
        private String strNombre;
        private String strApp;
        private String strPrefijo;
        private String strPosfijo;
        private String strDomicilio;
        private String strTituloSemplanza;
        private String strArchivo;
        private String[] strTelefonos;
        private int IdOrganoPertenece;

        #endregion

        public clsPersona()
        {
        }

        public clsPersona(int IdPersona)
        {
        }

        public String NombreCompleto()
        {
            string strNComp;
            strNComp = "Este es el nombre completo";
            return strNComp;
        }

        public String OrganoAlQuePertenece()
        {
            string strOrgano = "";
            strOrgano = "Este es el Órgano al que pertenece";
            return strOrgano;
        }

        public String Domicilio()
        {
            string strDom = "";
            strDom = "Esta es la Dirección";
            return strDom;
        }

        public List<String> TraeDatos()
        {
            List<String> lstRes = new List<String>();
#if STAND_ALONE
            FachadaBusquedaTradicional fachada = new FachadaBusquedaTradicional();
            List<String> lista = fachada.TraeNombresAreas(2);
            int nT = lista.Count;
#else
            FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
            String[] lista = fachada.TraeNombresAreas(2);
            int nT = lista.Length;
#endif
            //TesisTO[] resultadoFachada = fachada.getTesisPorIus(BuscaEspecial);
            //resultadoTesis = new List<TesisSimplificadaTO>();
            //fachada.Close();
            //foreach (TesisTO item in resultadoFachada)
            //{
            //}

            for (int i = 0; (i < nT); i++)
            {
                lstRes.Add(lista[i]);
            }
            return lstRes;
        }
    }
}
