using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace mx.gob.scjn.ius_common.TO
{
    [DataContract]
    public class FiltrosTO
    {
        /// <summary>
        /// El valor del tipo de filtro:
        /// IUS_FILTRO_JURIS
        /// IUS_FILTRO_PONENTE
        /// IUS_FILTRO_ASUNTO
        /// 
        /// </summary>
        [DataMember]
        public int TipoFiltro;
        /// <summary>
        /// vALOR DEL ID DEL FILTRO
        /// </summary>
        [DataMember]
        public int ValorFiltro;
        /// <summary>
        /// Algún otro valor requerido para ejecutar  la busqueda
        /// </summary>
        [DataMember]
        public int ValorAdicional;
    }
}
