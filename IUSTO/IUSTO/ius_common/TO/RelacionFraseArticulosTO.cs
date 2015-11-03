using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace mx.gob.scjn.ius_common.TO
{
    ///<summary>
    /// Representa la relación existente entre una frase determinada
    /// dentro de una tesis y un artículo de la legislación federal.
    /// </summary>
    /// <remarks >
    /// author Carlos de Luna Sáenz
    /// </remarks>
    ///
    [DataContract]
    public class RelacionFraseArticulosTO
    {
        /// <summary>
        /// El numero de IUS
        /// </summary>
        /// 
        [DataMember]
        public String Ius { get { return this.getIus(); } set { this.setIus(value); } }
        private String ius;
        /// <summary>
        /// El identificador de la relacion.
        /// </summary>
        /// 
        [DataMember]
        public String IdRel { get { return this.getIdRel(); } set { this.setIdRel(value); } }
        private String idRel;
        /// <summary>
        /// Identificador de referencia 
        /// para las tablas que contienen 
        /// la liga propiamente dicha.
        /// </summary>
        /// 
        [DataMember]
        public String IdRef { get { return this.getIdRef(); } set { this.setIdRef(value); } }
        private String idRef;
        /// <summary>
        /// El identificador de la ley a la que se hará
        /// Referencia en la liga.
        /// </summary>
        /// 
        [DataMember]
        public String IdLey { get { return this.getIdLey(); } set { this.setIdLey(value); } }
        private String idLey;
        /// <summary>
        /// El identificador del artículo al que hace referencia la ley.
        /// </summary>
        /// 
        [DataMember]
        public String IdArt { get { return this.getIdArt(); } set { this.setIdArt(value); } }
        private String idArt;
        /// <summary>
        /// Consecutivo.
        /// </summary>
        /// 
        public String Consec { get { return this.getConsec(); } set { this.setConsec(value); } }
        private String consec;
        /**
         * @return the iUS
         */
        public String getIus()
        {
            return ius;
        }
        /**
         * @param iusParam the iUS to set
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
         * @param idRelParam the idRel to set
         */
        public void setIdRel(String idRelParam)
        {
            idRel = idRelParam;
        }
        /**
         * @return the idRef
         */
        public String getIdRef()
        {
            return idRef;
        }
        /**
         * @param idRefParam the idRef to set
         */
        public void setIdRef(String idRefParam)
        {
            idRef = idRefParam;
        }
        /**
         * @return the idLey
         */
        public String getIdLey()
        {
            return idLey;
        }
        /**
         * @param idLeyParam the idLey to set
         */
        public void setIdLey(String idLeyParam)
        {
            idLey = idLeyParam;
        }
        /**
         * @return the idArt
         */
        public String getIdArt()
        {
            return idArt;
        }
        /**
         * @param idArtParam the idArt to set
         */
        public void setIdArt(String idArtParam)
        {
            idArt = idArtParam;
        }
        /**
         * @return the consec
         */
        public String getConsec()
        {
            return consec;
        }
        /**
         * @param consecParam the consec to set
         */
        public void setConsec(String consecParam)
        {
            consec = consecParam;
        }
    }
}
