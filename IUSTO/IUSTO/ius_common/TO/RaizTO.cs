using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mx.gob.scjn.ius_common.TO
{
    public class RaizTO
    {
        public String Id { get; set; }
        public String Nivel { get; set; }
        public String Descripcion { get; set; }
        public String TemaMay { get; set; }
        public int Consecutivo { get; set; }
        public int Posicion { get; set; }
        public int Muestra { get; set; }
        public int Cuantas { get; set; }
        public int Desplazamiento { get; set; }
        public String Tabla { get; set; }
        public int NoIus { get; set; }
        public object OLE { get; set; }
        public String Hiperlink { get; set; }
        public String Padre { get; set; }
    }
}
