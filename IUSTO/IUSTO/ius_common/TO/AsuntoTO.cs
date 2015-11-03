using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace mx.gob.scjn.ius_common.TO
{
    [DataContract]
    public class AsuntoTO
    {
        [DataMember]
        public int Parte { get; set; }
        [DataMember]
        public int IdTipo { get; set; }
        [DataMember]
        public String DescTipo { get; set; }
        [DataMember]
        public int Activo { get; set; }
        [DataMember]
        public String LetraIni { get; set; }
    }
}
