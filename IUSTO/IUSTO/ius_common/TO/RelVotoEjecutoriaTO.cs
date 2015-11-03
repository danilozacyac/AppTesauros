using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace mx.gob.scjn.ius_common.TO
{
    ///<summary>
    /// Define la relación existente entre un voto y una ejecutoria.
    /// </summary>
    /// <remarks author="Carlos de Luna Saenz"></remarks>
    ///
   [DataContract]
    public class RelVotoEjecutoriaTO
    {
        /// <summary>
        /// El voto que está relacionado con la ejecutoria.
        /// </summary>
        /// 
       [DataMember]
        public String Voto { get { return this.getVoto(); } set { this.setVoto(value); } }
        private String voto;
        /// <summary>
        /// La ejecutoria que está relacionada con el voto.
        /// </summary>
        /// 
       [DataMember]
        public String Ejecutoria { get { return this.getEjecutoria(); } set { this.setEjecutoria(value); } }
        private String ejecutoria;
        /// <summary>
        /// obtiene al voto que esta relacionado con una ejecutoria.
        /// </summary>
        /// <return> El voto relacionado</return>
        ///

        public String getVoto()
        {
            return this.voto;
        }
        /// <summary>
        /// Establece el valor del identificador del voto.
        /// </summary>
        /// <param name="votoParam"> el voto de la relación
        /// </param>

        public void setVoto(String votoParam)
        {
            this.voto = votoParam;
        }
        ///<summary>
        /// Obtiene la ejecutoria de la relación.
        /// </summary>
        /// <return> La ejecutoria de la relación.
        ///</return>

        public String getEjecutoria()
        {
            return this.ejecutoria;
        }
        /// <summary>
        /// Se establece el valor de la ejecutoria en la relación.
        /// </summary>
        /// <param name="ejecutoriaParam"> ejecutoria La ejecutoria.</param>
        ///
        public void setEjecutoria(String ejecutoriaParam)
        {
            this.ejecutoria = ejecutoriaParam;
        }
    }
}
