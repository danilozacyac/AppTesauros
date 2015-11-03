using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace mx.gob.scjn.ius_common.TO
{
    [DataContract]
    public class UsuarioTO
    {
        [DataMember]
        public String Usuario { get; set; }
        [DataMember]
        public String Passwd { get; set; }
        [DataMember]
        public String Nombre { get; set; }
        [DataMember]
        public String Apellidos { get; set; }
        [DataMember]
        public bool Enviar { get; set; }
        [DataMember]
        public String Correo { get; set; }
    }
}