using System;
using System.Collections.Generic;
using System.Linq;
//using IUSTOWeb.TO.Comparador;
using mx.gob.scjn.ius_common.fachade;

namespace mx.gob.scjn.directorio
{

    class clsArea
    {

        #region Propiedades de la clase

        private int nId;
        private String strNombreCompleto;
        private String strPrefijo;
        private String strPosfijo;
        private String strDomicilio;
        private String[] strTitulares;
        private String strArchivo;
        private String[] strTelefonos;

        #endregion

        public clsArea()
        {
        }

        public clsArea(int IdPersona)
        {
        }

        public List<String> TraeDatos()
        {
#if STAND_ALONE
            FachadaBusquedaTradicional fachada = new FachadaBusquedaTradicional();
            List<String> lista = fachada.TraeNombresAreas(2);
            int nT = lista.Count;
#else
            FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
            String[] lista = fachada.TraeNombresAreas(2);
            int nT = lista.Length;
#endif
            List<String> lstRes = new List<String>();

            for (int i = 0; (i < nT); i++)
            {
                lstRes.Add(lista[i]);
            }
            return lstRes;
        }
    }
}
