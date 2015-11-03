using System;
using System.Linq;

namespace mx.gob.scjn.directorio
{

    class clsTexto
    {

        public String ReparaMe(String strFte)
        {
            String strCadena = strFte;

            if (strCadena.Length > 0)
            {
                strCadena.ToUpper();
            }
            return strCadena;
        }
    }
}
