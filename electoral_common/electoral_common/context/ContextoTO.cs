using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mx.gob.scjn.electoral_common.context
{
    class ContextoTO
    {
        private Object objeto;
        private String tipo;
        public Object GetObjeto()
        {
            return this.objeto;
        }
        public String GetTipo()
        {
            return this.tipo;
        }
        public void SetObjeto(Object objetoParam)
        {
            this.objeto = objetoParam;
        }
        public void SetTipo(String tipoParam)
        {
            this.tipo = tipoParam;
        }
    }
}
