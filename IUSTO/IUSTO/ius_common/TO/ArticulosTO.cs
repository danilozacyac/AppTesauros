using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace mx.gob.scjn.ius_common.TO
{
    [DataContract]
    public class ArticulosTO
    {
        /// <summary>
        /// El identificador de la ley a la que pertenece el articulo.
        /// </summary>
        [DataMember]
        public String IdLey { get { return this.getIdLey(); } set { this.setIdLey(value); } }
        private String idLey;
        /// <summary>
        /// La referencia hacia la tesis.
        /// </summary>
        [DataMember]
        public String IdRef { get { return this.getIdRef(); } set { this.setIdRef(value); } }
        private String idRef;
        /// <summary>
        /// El identificador del Artículo.
        /// </summary>
        [DataMember]
        public String IdArt { get { return this.getIdArt(); } set { this.setIdArt(value); } }
        private String idArt;
        /// <summary>
        /// El renglón del articulo al que se hace referencia dentro de la tesis.
        /// </summary>
        [DataMember]
        public String Renglon { get { return this.getRenglon(); } set { this.setRenglon(value); } }
        private String renglon;
        [DataMember]
        public String Consec { get { return this.getConsec(); } set { this.setConsec(value); } }
        private String consec;
        [DataMember]
        public String Tipo { get { return this.getTipo(); } set { this.setTipo(value); } }
        private String tipo;
        [DataMember]
        public String NumArt { get { return this.getNumArt(); } set { this.setNumArt(value); } }
        private String numArt;
        [DataMember]
        public String Info { get { return this.getInfo(); } set { this.setInfo(value); } }
        private String info;
        [DataMember]
        public String InfoT { get { return this.getInfoT(); } set { this.setInfoT(value); } }
        private String infoT;
        /**
         * @return the idLey
         */
        public String getIdLey()
        {
            return idLey;
        }
        /**
         * @param idLey the idLey to set
         */
        public void setIdLey(String idLey)
        {
            this.idLey = idLey;
        }
        /**
         * @return the idRef
         */
        public String getIdRef()
        {
            return idRef;
        }
        /**
         * @param idRef the idRef to set
         */
        public void setIdRef(String idRef)
        {
            this.idRef = idRef;
        }
        /**
         * @return the idArt
         */
        public String getIdArt()
        {
            return idArt;
        }
        /**
         * @param idArt the idArt to set
         */
        public void setIdArt(String idArt)
        {
            this.idArt = idArt;
        }
        /**
         * @return the renglon
         */
        public String getRenglon()
        {
            return renglon;
        }
        /**
         * @param renglon the renglon to set
         */
        public void setRenglon(String renglon)
        {
            this.renglon = renglon;
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
        public void setConsec(String consec)
        {
            this.consec = consec;
        }
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
        public void setTipo(String tipo)
        {
            this.tipo = tipo;
        }
        /**
         * @return the numArt
         */
        public String getNumArt()
        {
            return numArt;
        }
        /**
         * @param numArt the numArt to set
         */
        public void setNumArt(String numArt)
        {
            this.numArt = numArt;
        }
        /**
         * @return the info
         */
        public String getInfo()
        {
            return info;
        }
        /**
         * @param info the info to set
         */
        public void setInfo(String info)
        {
            this.info = info;
        }
        /**
         * @return the infoT
         */
        public String getInfoT()
        {
            return infoT;
        }
        /**
         * @param infoT the infoT to set
         */
        public void setInfoT(String infoT)
        {
            this.infoT = infoT;
        }

    }
}
