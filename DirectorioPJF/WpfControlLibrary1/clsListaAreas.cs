using System;
using System.Collections.Generic;
using System.Linq;
//using IUSTOWeb.TO.Comparador;
using mx.gob.scjn.ius_common.fachade;

namespace mx.gob.scjn.directorio
{

    class clsListaAreas
    {

        public clsListaAreas()
        {
        }

        public clsListaAreas(int nTipo)
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
