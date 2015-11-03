using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace mx.gob.scjn.ius_common.TO
{
    /**
     * @author cluna
     *
     */
    [DataContract]
    public class ResultadosPanelTO
    {
        /**
         * La lista de Tesis de la consulta
         */
        [DataMember]
        public List<TesisTO> Tesis { get { return tesis; } set { setTesis(value); } }
        private List<TesisTO> tesis;
        /**
         * La lista de ejecutorias de la consulta
         */
        [DataMember]
        public List<EjecutoriasTO> Ejecutorias { get { return ejecutorias; } set { setEjecutorias(value); } }
        private List<EjecutoriasTO> ejecutorias;
        /**
         * La lista de votos de la consulta
         */
        [DataMember]
        public List<VotosTO> Votos { get { return votos; } set { setVotos(value); } }
        private List<VotosTO> votos;
        /**
         * La lista de acuerdos de la consulta
         */
        [DataMember]
        public List<AcuerdosTO> Acuerdos { get { return this.acuerdos; } set { setAcuerdos(value); } }
        private List<AcuerdosTO> acuerdos;
        /**
         * El tipo de resultado que se envia
         */
        private int tipoResultado;
        [DataMember]
        public int TipoResultado { get { return tipoResultado; } set { setTipoResultado(value); } }
        /**
         * @return the tesis
         */
        public List<TesisTO> getTesis()
        {
            return tesis;
        }
        /**
         * @param tesis the tesis to set
         */
        public void setTesis(List<TesisTO> tesis)
        {
            this.tesis = tesis;
        }
        /**
         * @return the ejecutorias
         */
        public List<EjecutoriasTO> getEjecutorias()
        {
            return ejecutorias;
        }
        /**
         * @param ejecutorias the ejecutorias to set
         */
        public void setEjecutorias(List<EjecutoriasTO> ejecutorias)
        {
            this.ejecutorias = ejecutorias;
        }
        /**
         * @return the tipoResultado
         */
        public int getTipoResultado()
        {
            return tipoResultado;
        }
        /**
         * @param tipoResultado the tipoResultado to set
         */
        public void setTipoResultado(int tipoResultado)
        {
            this.tipoResultado = tipoResultado;
        }
        /**
         * @return the acuerdos
         */
        public List<AcuerdosTO> getAcuerdos()
        {
            return acuerdos;
        }
        /**
         * @param acuerdos the acuerdos to set
         */
        public void setAcuerdos(List<AcuerdosTO> acuerdos)
        {
            this.acuerdos = acuerdos;
        }
        /**
         * @return the votos
         */
        public List<VotosTO> getVotos()
        {
            return votos;
        }
        /**
         * @param votos the votos to set
         */
        public void setVotos(List<VotosTO> votos)
        {
            this.votos = votos;
        }

    }
}
