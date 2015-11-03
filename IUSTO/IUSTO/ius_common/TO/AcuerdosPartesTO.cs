using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace mx.gob.scjn.ius_common.TO
{
    /// <summary>
    /// Esta clase sirve para traer todas las partes de una ejecutoria.
    /// </summary>
    /// <remarks author="Carlos de Luna Sáenz"/>
    ///
    [DataContract]
    public class AcuerdosPartesTO
    {
        [DataMember]
        public int Id { get { return this.getId(); } set { this.setId(value); } }
        private int id;
        [DataMember]
        public int Parte { get { return this.getParte(); } set { this.setParte(value); } }
        private int parte;
        [DataMember]
        public String TxtParte { get { return this.getTxtParte(); } set { this.setTxtParte(value); } }
        private String txtParte;
        /**
         * @return the id
         */
        public int getId()
        {
            return id;
        }
        /**
         * @param id the id to set
         */
        public void setId(int id)
        {
            this.id = id;
        }
        /**
         * @return the parte
         */
        public int getParte()
        {
            return parte;
        }
        /**
         * @param parte the parte to set
         */
        public void setParte(int parte)
        {
            this.parte = parte;
        }
        /**
         * @return the txtParte
         */
        public String getTxtParte()
        {
            return txtParte;
        }
        /**
         * @param txtParte the txtParte to set
         */
        public void setTxtParte(String txtParte)
        {
            this.txtParte = txtParte;
        }

    }
}
