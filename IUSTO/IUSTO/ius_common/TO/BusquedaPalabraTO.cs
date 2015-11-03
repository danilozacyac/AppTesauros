using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Lucene.Net.Search;

namespace mx.gob.scjn.ius_common.TO
{
    [DataContract]
    public class BusquedaPalabraTO
    {
        public const int BUSQUEDA_PALABRA_OP_Y = 1;
        public const int BUSQUEDA_PALABRA_OP_O = 2;
        public const int BUSQUEDA_PALABRA_OP_NO = 3;
        [DataMember]
        public List<int> Campos { get { return this.getCampos(); } set { this.setCampos(value); } }
        private List<int> campos;
        [DataMember]
        public int Jurisprudencia { get { return this.getJurisprudencia(); } set { this.setJurisprudencia(value); } }
        private int jurisprudencia;
        [DataMember]
        public String Expresion { get { return this.getExpresion(); } set { this.setExpresion(value); } }
        private String expresion;
        [DataMember]
        public int ValorLogico { get; set; }
        private int valorLogico;
        public BooleanClause.Occur Ocurrencia { get; set; }
        protected void setValorLogico(int value)
        {
            this.valorLogico = value;
            switch (value)
            {
                case BUSQUEDA_PALABRA_OP_Y :
                    Ocurrencia = BooleanClause.Occur.MUST;
                    break;
                case BUSQUEDA_PALABRA_OP_O:
                    Ocurrencia = BooleanClause.Occur.SHOULD;
                    break;
                case BUSQUEDA_PALABRA_OP_NO:
                    Ocurrencia = BooleanClause.Occur.MUST_NOT;
                    break;
                default:
                    Ocurrencia = BooleanClause.Occur.MUST;
                    break;
            }
        }
        private void setCampos(List<int> value)
        {
            this.campos=value;
        }

        private List<int> getCampos()
        {
            return this.campos;
        }
        private void setJurisprudencia(int value)
        {
            this.jurisprudencia = value;
        }

        private int getJurisprudencia()
        {
            return this.jurisprudencia;
        }

        private void setExpresion(string value)
        {
            this.expresion=value;
        }

        private string getExpresion()
        {
            return this.expresion;
        }

    }
}
