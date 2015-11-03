using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace mx.gob.scjn.ius_common.TO
{
    [DataContract]
    public class BusquedaAlmacenadaTO
    {
        /// <summary>
        /// El tipo de Busqueda que se requiere hacer, ver IUSConstants.
        /// </summary>
        /// <seealso cref="IUSConstants"/>
        [DataMember]
        public int TipoBusqueda { get; set; }
        /// <summary>
        /// El nombre de la busqueda
        /// </summary>
        [DataMember]
        public String Nombre { get; set; }
        /// <summary>
        /// El valor que tiene la búsqueda en caso de que se requiera
        /// solamente un valor, como en el caso de la
        /// búsqueda temática.
        /// </summary>
        [DataMember]
        public String ValorBusqueda { get; set; }
        /// <summary>
        /// El identificador de la búsqueda.
        /// </summary>
        [DataMember]
        public int id { get; set; }
        /// <summary>
        /// Las épocas sobre la cual se realiza la búsqueda.
        /// </summary>
        [DataMember]
        public int[] Epocas { get; set; }
        /// <summary>
        /// Registros que forman parte de la búsqueda.
        /// </summary>
        [DataMember]
        public int[] BusquedaRegistro { get; set; }
        ///<summary>
        /// La lista de las expresiones que tiene la búsqueda.
        ///</summary>
        [DataMember]
        public List<ExpresionBusqueda> Expresiones { get; set; }
        /// <summary>
        /// Lista de los filtros aplicados cuando se guardó la búsqueda.
        /// </summary>
        [DataMember]
        public List<FiltrosTO> Filtros { get; set; }
        /// <summary>
        ///     La expresión que representa el tipo de búsqueda
        /// </summary>
        [DataMember]
        public String Expresion { get; set; }
        /// <summary>
        /// Los tribunales donde se realizará la búsqueda
        /// </summary>
        [DataMember]
        public int[] Tribunales { get; set; }
    }
}
