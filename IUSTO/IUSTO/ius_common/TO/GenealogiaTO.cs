using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
namespace mx.gob.scjn.ius_common.TO
{
    /// <summary>
    /// Esta clase reproduce el contenido de una genealogía determinada para una tesis
    /// </summary>
    /// <remarks author="Carlos de Luna Sáenz."></remarks>
    ///
    ///
    [DataContract]
    public class GenealogiaTO
    {
        /// <summary>
        /// El identificador de la genealogía.
        /// </summary>
        /// 
        [DataMember]
        public String IdGeneralogia { get { return this.getIdGenealogia(); } set { this.setIdGenealogia(value); } }
        private String idGenealogia;
        /// <summary>
        /// El texto Correspondiente a la genealogía.
        /// </summary>
        [DataMember]
        public String TxtGenealogia { get { return this.getTxtGenealogia(); } set { this.setTxtGenealogia(value); } }
        private String txtGenealogia;
        /**
         * @return the iDGenealogia
         */
        public String getIdGenealogia()
        {
            return idGenealogia;
        }
        /**
         * @param genealogia the iDGenealogia to set
         */
        public void setIdGenealogia(String genealogia)
        {
            idGenealogia = genealogia;
        }
        /**
         * @return the txtGenealogia
         */
        public String getTxtGenealogia()
        {
            return txtGenealogia;
        }
        /**
         * @param txtGenealogia the txtGenealogia to set
         */
        public void setTxtGenealogia(String txtGenealogia)
        {
            this.txtGenealogia = txtGenealogia;
        }
    }
}
