using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace mx.gob.scjn.ius_common.TO
{
    [DataContract]
    public class DocumentoTO
    {
        [DataMember]
        public int TipoDocumento { get; set; }
        [DataMember]
        public String Descripcion { get; set; }
        [DataMember]
        public String Clase { get; set; }
        [DataMember]
        public String Ensamblado { get; set; }
        [DataMember]
        public String ClaseTablaResultado { get; set; }
        [DataMember]
        public String EnsambladoTR { get; set; }
        [DataMember]
        public String MetodoFachada { get; set; }
        [DataMember]
        public bool? EsPaginado { get; set; }
        [DataMember]
        public String Propiedad { get; set; }
        [DataMember]
        public int CategoriaDoc { get; set; }
        [DataMember]
        public String Logo { get; set; }
        [DataMember]
        public int TipoBusqueda { get; set; }
    }
}
