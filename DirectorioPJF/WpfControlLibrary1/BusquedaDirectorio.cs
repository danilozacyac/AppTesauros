using System;
using System.Linq;

namespace mx.gob.scjn.directorio
{

    class BusquedaDirectorio
    {
        bool bEsPrimero = true;

        public String CadenaBusqueda;
  
        public String GeneraQuery(String sTabla, String strTextoBusq, String strDescMay)
        {
            String strQry;
            strQry = AcomodaTextoParaBuscar(sTabla, strTextoBusq, strDescMay);
            return strQry;
        }

        private String AcomodaTextoParaBuscar(String sTabla, String strTextoBusq, String strDescMay)
        {
            String strquery = "";
            string strTextBusc = strTextoBusq;
            char[] delimiterChars = { ' ', ',', '.', ':', '\t' };
            string strCadConsulta = "";
            strquery = " WHERE (  ";
            string[] words = strTextBusc.Split(delimiterChars);

            foreach (string strToken in words)
            {
                strCadConsulta = strCadConsulta + PegaItem(strToken, strDescMay, "Y");
            }
            return strquery + strCadConsulta + " )";
        }

        private String AjustaParaBuscarFrases(String strTextoBusq)
        {
            return "";
        }

        private String PegaItem(String strVar, String strDescMay, String strOperador)
        {
            String strVarLocal;
            String strOperadorLocal;
            String strResp = "";
            Boolean bOp;
            bOp = true;
            strVarLocal = strVar.Trim();
            strOperadorLocal = strOperador;

            if ((strOperadorLocal.Length) == 0)
            {
                strOperadorLocal = "Y";
            }

            switch (strOperadorLocal)
            {

                case "O":

                    if (bEsPrimero)
                    {
                        strResp = strDescMay + " LIKE ";
                        bEsPrimero = false;
                    }
                    else
                    {
                        strResp = " OR " + strDescMay + "  LIKE ";
                    }
                    break;

                case "Y":

                case " ":

                    if (bEsPrimero)
                    {
                        strResp = strDescMay + "  LIKE ";
                        bEsPrimero = false;
                    }
                    else
                    {
                        strResp = " AND " + strDescMay + "  LIKE ";
                    }
                    break;

                case "N":

                    if (bEsPrimero)
                    {
                        strResp = strDescMay + "  NOT LIKE ";
                        bEsPrimero = false;
                    }
                    else
                    {
                        strResp = " AND " + strDescMay + "  NOT LIKE ";
                    }
                    break;
                default: break;
            }
            return strResp + " " + " '% " + strVar + " %' ";
        }
    }
}
