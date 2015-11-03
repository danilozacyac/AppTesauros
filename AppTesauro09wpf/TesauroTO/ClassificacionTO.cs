using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace mx.gob.scjn.ius_common.TO
{
    public class ClassificacionTO
    {
        public int Parte { get; set; }
        public int IdTipo { get; set; }
        public String DescTipo { get; set; }
        public int Activo { get; set; }
    }
}
