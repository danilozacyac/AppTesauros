using System;
using System.Runtime.Serialization;
namespace mx.gob.scjn.ius_common.TO
{
    /// <summary>
    /// Sirve para la obtención de datos de la muestra de
    /// magistrads en las noticias históricas.
    /// </summary>
    /// <remarks author="Carlos de Luna Sáenz"></remarks>
    ///
    [DataContract]
    public class EpocaMagistradoTO
    {
        /// <summary>
        /// La epoca de la cual son requeridos los magistrados.
        /// </summary>
        /// 
        [DataMember]
        public String Epoca { get { return this.getEpoca(); } set { this.setEpoca(value); } }
        private String epoca;
        /// <summary>
        /// La instancia a la que pertenecen.
        /// </summary>
        /// 
        [DataMember]
        public String Sala { get { return this.getSala(); } set { this.setSala(value); } }
        private String sala;
        /// <summary>
        /// El identificador de la sala/epoca/fecha.
        /// </summary>
        /// 
        [DataMember]
        public String Id { get { return this.getId(); } set { this.setId(value); } }
        private String id;
        /// <summary>
        /// La fecha o descripción de cuando hubo magistrados nuevos.
        /// </summary>
        /// 
        [DataMember]
        public String Fecha { get { return this.getFecha(); } set { this.setFecha(value); } }
        private String fecha;
        /// <summary></summary>
        /// <return></return> 
        ///
        public String getEpoca()
        {
            return epoca;
        }
        /**
         * @param epoca the epoca to set
         */
        public void setEpoca(String epoca)
        {
            this.epoca = epoca;
        }
        /**
         * @return the sala
         */
        public String getSala()
        {
            return sala;
        }
        /**
         * @param sala the sala to set
         */
        public void setSala(String sala)
        {
            this.sala = sala;
        }
        /**
         * @return the id
         */
        public String getId()
        {
            return id;
        }
        /**
         * @param id the id to set
         */
        public void setId(String id)
        {
            this.id = id;
        }
        /**
         * @return the fecha
         */
        public String getFecha()
        {
            return fecha;
        }
        /**
         * @param fecha the fecha to set
         */
        public void setFecha(String fecha)
        {
            this.fecha = fecha;
        }
    }
}