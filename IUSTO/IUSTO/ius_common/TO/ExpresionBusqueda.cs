using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace mx.gob.scjn.ius_common.TO
{
    [DataContract]
    public class ExpresionBusqueda
    {
        [DataMember]
        public String Expresion { get; set; }
        [DataMember]
        public int Operador { get; set; }
        [DataMember]
        public List<int> Campos { get; set; }
        [DataMember]
        public int IsJuris { get; set; }
    }
}
