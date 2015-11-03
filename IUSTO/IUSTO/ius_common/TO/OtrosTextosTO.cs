using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace mx.gob.scjn.ius_common.TO
{
    [DataContract]
    public class OtrosTextosTO
    {
        [DataMember]
        public String Ius { get { return this.getIus(); } set { this.setIus(value); } }
        private String ius;
        [DataMember]
        public String TipoNota { get { return this.getTipoNota(); } set { this.setTipoNota(value); } }
        private String tipoNota;
        [DataMember]
        public String Textos { get { return this.getTextos(); } set { this.setTextos(value); } }
        private String textos;
        [DataMember]
        public String TxtNotas { get; set; }
        [DataMember]
        public int version { get; set; }

        /**
         * @return the ius
         */
        public String getIus()
        {
            return ius;
        }
        /**
         * @param ius the ius to set
         */
        public void setIus(String ius)
        {
            this.ius = ius;
        }
        /**
         * @return the tipoNota
         */
        public String getTipoNota()
        {
            return tipoNota;
        }
        /**
         * @param tipoNota the tipoNota to set
         */
        public void setTipoNota(String tipoNota)
        {
            this.tipoNota = tipoNota;
        }
        /**
         * @return the textos
         */
        public String getTextos()
        {
            return textos;
        }
        /**
         * @param textos the textos to set
         */
        public void setTextos(String textos)
        {
            this.textos = textos;
        }

    }
}
