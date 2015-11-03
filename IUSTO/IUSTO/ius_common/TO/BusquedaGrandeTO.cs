using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace mx.gob.scjn.ius_common.TO
{
    [DataContract]
    public class BusquedaGrandeTO
    {
        /// <summary>
        /// Número de registros por página de información 
        /// que se enviará a la capa de presentación.
        /// </summary>
        /// 
        [DataMember]
        public const int REGISTROS_PAGINA = 20;
        /// <summary>
        /// Los registros de la página que será mostrada por la capa de presentación.
        /// </summary>
        /// 
        [DataMember]
        public List<TesisTO> RegistrosSolicitados { get { return this.getRegistrosSolicitados(); } set { this.setRegistrosSolicitados(value);} }
        private List<TesisTO> registrosSolicitados;
        /// <summary>
        /// Número de registros existentes en la lista global.
        /// </summary>
        /// 
        [DataMember]
        public int NumeroRegistros { get { return this.getNumeroRegistros(); } set { this.setNumeroRegistros(value); } }
        private int numeroRegistros;
        /// <summary>
        /// Define si se usará o no paginación en el resultado.
        /// </summary>
        /// 
        [DataMember]
        public bool UsarPaginacion { get; set; }

        private int getNumeroRegistros()
        {
            return this.numeroRegistros;
        }

        private void setNumeroRegistros(int value)
        {
            this.numeroRegistros = value;
        }
        private List<TesisTO> getRegistrosSolicitados()
        {
            return this.registrosSolicitados;
        }
        private void setRegistrosSolicitados(List<TesisTO> value)
        {
            this.registrosSolicitados = value;
        }

    }
}
