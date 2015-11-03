using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace mx.gob.scjn.ius_common.TO
{
    [DataContract]
    public class LeyTO
    {
        /// <summary>
        /// El identificador de la Ley.
        /// </summary>
        [DataMember]
        public String IdLey { get { return this.getIdLey(); } set { this.setIdLey(value); } }
        private String idLey;
        /// <summary>
        /// La descripcion de la ley.
        /// </summary>
        [DataMember]
        public String DescLey { get { return this.getDescLey(); } set { this.setDescLey(value); } }
        private String descLey;
        /// <summary>
        /// El consecutivo de la ley.
        /// </summary>
        [DataMember]
        public String Consec { get { return this.getConsec(); } set { this.setConsec(value); } }
        private String consec;
        /// <summary>
        /// La descripción de la ley en forma abreviada.
        /// </summary>
        [DataMember]
        public String DescAbr { get { return this.getDescAbr(); } set { this.setDescAbr(value); } }
        private String descAbr;
        /// <summary>
        /// Define  si es o no decreto o el tipo de dereto por el que
        /// se promulga.
        /// </summary>
        [DataMember]
        public String Decreto { get { return this.getDecreto(); } set { this.setDecreto(value); } }
        private String decreto;
        /// <summary>
        /// Define si esta  ley debe o no ser visible.
        /// </summary>
        [DataMember]
        public String Visible { get { return this.getVisible(); } set { this.setVisible(value); } }
        private String visible;
        /// <summary>
        /// Establece la reerencia que pueda tener la Ley.
        /// </summary>
        [DataMember]
        public String TieneRefs { get { return this.getTieneRefs(); } set { this.setTieneRefs(value); } }
        private String tieneRefs;
        /// <summary>
        /// Establece la cadena con el nombre de la ley.
        /// </summary>
        [DataMember]
        public String NombrStr { get { return this.getNombreStr(); } set { this.setNombreStr(value); } }
        private String nombreStr;
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
         * @return the descLey
         */
        public String getDescLey()
        {
            return descLey;
        }
        /**
         * @param descLey the descLey to set
         */
        public void setDescLey(String descLey)
        {
            this.descLey = descLey;
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
         * @return the descAbr
         */
        public String getDescAbr()
        {
            return descAbr;
        }
        /**
         * @param descAbr the descAbr to set
         */
        public void setDescAbr(String descAbr)
        {
            this.descAbr = descAbr;
        }
        /**
         * @return the decreto
         */
        public String getDecreto()
        {
            return decreto;
        }
        /**
         * @param decreto the decreto to set
         */
        public void setDecreto(String decreto)
        {
            this.decreto = decreto;
        }
        /**
         * @return the visible
         */
        public String getVisible()
        {
            return visible;
        }
        /**
         * @param visible the visible to set
         */
        public void setVisible(String visible)
        {
            this.visible = visible;
        }
        /**
         * @return the tieneRefs
         */
        public String getTieneRefs()
        {
            return tieneRefs;
        }
        /**
         * @param tieneRefs the tieneRefs to set
         */
        public void setTieneRefs(String tieneRefs)
        {
            this.tieneRefs = tieneRefs;
        }
        /**
         * @return the nombreStr
         */
        public String getNombreStr()
        {
            return nombreStr;
        }
        /**
         * @param nombreStr the nombreStr to set
         */
        public void setNombreStr(String nombreStr)
        {
            this.nombreStr = nombreStr;
        }
    }
}
