using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace mx.gob.scjn.ius_common.TO
{
    /// <summary>
    /// Simula un paginador de datos
    /// </summary>
    /// 
    [DataContract]
    public class PaginadorTO
    {
        /// <summary>
        /// Da el máximo Id utilizado para el paginador
        /// </summary>
        /// 
        [DataMember]
        public static int MaxId { get; set; }
        /// <summary>
        /// Establece el Id de este objeto
        /// </summary>
        /// 
        [DataMember]
        public int Id { get { return id; } set { } }
        /// <summary>
        /// El valor interno del identificador
        /// </summary>
        /// 
        
        private int id { get; set; }
        /// <summary>
        /// El momento de última consulta del objeto
        /// </summary>
        [DataMember]
        public DateTime TimeStamp {get;set;}
        /// <summary>
        /// Establece si está o no activo, esto para que pueda ser desechado
        /// </summary>
        [DataMember]
        public bool Activo
        {
            get
            {
                return activo;
            }
            set
            {
                activo = value;
            }
        }
        /// <summary>
        /// El valor interno de Activo
        /// </summary>
        private bool activo { get; set; }
        /// <summary>
        /// El largo del arreglo
        /// </summary>
        [DataMember]
        public int Largo
        {
            get
            {
                if (ResultadoIds == null)
                    return -1;
                else
                    return largo;
            }
            set
            {
                largo = value;
            }
        }
        /// <summary>
        /// El valor interno del largo del arreglo
        /// </summary>
        
        private int largo { get; set; }
        /// <summary>
        /// La lista de los resultados que se tiene en memoria
        /// correspondiente al paginador.
        /// </summar>y
        public List<Int32> ResultadoIds
        {
            get
            {
                return this.resultadoIds;
            }
            set
            {
                this.resultadoIds = value;
                if (value != null)
                {
                    Largo = value.Count;
                }
                else
                {
                    Largo = -1;
                }
            }
        }
        /// <summary>
        /// El valor interno de la lista de resultados
        /// </summary>
        private List<Int32> resultadoIds { get; set; }
        /// <summary>
        /// Un identificador del tipo de búsqueda a la que pertenece el paginador
        /// </summary>
        [DataMember]
        public int TipoBusqueda { get; set; }
        /// <summary>
        /// El constructor pro omisión define el valor del Id;
        /// </summary>
        public PaginadorTO()
        {
            this.id = ++MaxId;
            if (this.Id > 2100000000)
            {
                this.Id = 0;
                MaxId = 0;
            }
        }
    }
}
