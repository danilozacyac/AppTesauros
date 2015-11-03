using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace mx.gob.scjn.ius_common.TO
{
    /// <summary>
    /// Representa la relación entre las distintas frases
    /// de las tesis.
    /// </summary>
    /// <remarks author="Carlos de Luna Sáenz"></remarks>
    ///
    [DataContract]
    public class RelacionFraseTesisTO
    {
        /// <summary>
        /// El tipo de liga que se presentará
        /// </summary>
        /// 
        [DataMember]
        public String Tipo { get { return this.getTipo(); } set { this.setTipo(value); } }
        private String tipo;
        /// <summary>
        /// El número de Ius.
        /// </summary>
        /// 
        [DataMember]
        public String Ius { get { return this.getIus(); } set { this.setIus(value); } }
        private String ius;
        /// <summary>
        /// El identificador de la relación.
        /// </summary>
        /// 
        [DataMember]
        public String IdRel { get { return this.getIdRel(); } set { this.setIdRel(value); } }
        private String idRel;
        /// <summary>
        /// El identificador de la liga.
        /// </summary>
        /// 
        [DataMember]
        public String IdLink { get { return this.getIdLink(); } set { this.setIdLink(value); } }
        private String idLink;
        /// <summary>
        /// Consecutivo
        /// </summary>
        [DataMember]
        public String Consec { get { return this.getConsec(); } set { this.setConsec(value); } }
        private String consec;
        /**
         * @return the tipo
         */
        public String getTipo()
        {
            return tipo;
        }
        /**
         * @param tipo the tipo to set
         */
        public void setTipo(String tipoParam)
        {
            tipo = tipoParam;
        }
        /**
         * @return the iUS
         */
        public String getIus()
        {
            return ius;
        }
        /**
         * @param ius the iUS to set
         */
        public void setIus(String iusParam)
        {
            ius = iusParam;
        }
        /**
         * @return the idRel
         */
        public String getIdRel()
        {
            return idRel;
        }
        /**
         * @param idRel the idRel to set
         */
        public void setIdRel(String idRelParam)
        {
            idRel = idRelParam;
        }
        /**
         * @return the idLink
         */
        public String getIdLink()
        {
            return idLink;
        }
        /**
         * @param idLink the idLink to set
         */
        public void setIdLink(String idLinkParam)
        {
            idLink = idLinkParam;
        }
        /**
         * @return the consec
         */
        public String getConsec()
        {
            return consec;
        }
        /**
         * @param consec the consec to set
         */
        public void setConsec(String consecParam)
        {
            consec = consecParam;
        }
    }
}
