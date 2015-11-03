using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace mx.gob.scjn.ius_common.TO
{
    ///<summary>
    /// Establece cuando una ejecutoria, voto o acuerdo tienen una
    /// tabla adherida.
    /// </summary>
    /// <remarks author="Carlos de Luna Sáenz"></remarks>
    ///
    [DataContract]
    public class TablaPartesTO
    {
        /// <summary>
        /// Define el tipo de relación que existe entre el PDF
        /// y el tipo de documento:<p>
        /// <ol>
        ///    <li> Ejecutorias.</li>
        ///    <li> Acuerdos.</li>
        ///    <li> Votos.</li>
        /// </ol>
        ///</p>
        ///</summary>
        ///
        [DataMember]
        public int IdTipo { get { return this.getIdTpo(); } set { this.setIdTpo(value); } }
        private int idTpo;
        /// <summary>
        /// El identificador del documento al cual esta relacionado
        /// el PDF</summary>
        ///
        [DataMember]
        public int Id { get { return this.getId(); } set { this.setId(value); } }
        private int id;
        ///<summary>
        /// El nombre del archivo de la tabla.
        ///</summary>
        ///
        [DataMember]
        public String Archivo { get { return this.getArchivo(); } set { this.setArchivo(value); } }
        private String archivo;
        /// <summary>
        /// La posicion dentro del archivo completo.
        ///</summary>
        ///
        [DataMember]
        public int Posicion { get { return this.getPosicion(); } set { this.setPosicion(value); } }
        private int posicion;
        /// <summary>
        /// El tamaño de la frase a ser pintada en la liga de referencia.
        /// </summary>
        /// 
        [DataMember]
        public int Tamanio { get { return this.getTamanio(); } set { this.setTamanio(value); } }
        private int tamanio;
        /// <summary>
        /// Frase de la liga.
        ///</summary>
        ///
        [DataMember]
        public String Frase { get { return this.getFrase(); } set { this.setFrase(value); } }
        private String frase;
        ///<summary>
        /// Parte del documento en la que se encuentra al frase.
        /// </summary>
        /// 
        [DataMember]
        public int Parte { get { return this.getParte(); } set { this.setParte(value); } }
        private int parte;
        /// <summary>
        /// La posición donde se encuentra la liga en relación a su parte.
        /// </summary>
        /// 
        [DataMember]
        public int PosicionParte { get { return this.getPosicionParte(); } set { this.setPosicionParte(value); } }
        private int posicionParte;
        /// <summary>
        /// Obtiene el identificador del tipo de documento
        /// </summary>
        /// <returns>El tipo de documento</returns>
        /// <see cref="IdTpo"/>
        public int getIdTpo()
        {
            return idTpo;
        }
        /// <summary>
        /// Define el identificador del tipo de documento
        /// </summary>
        /// <param name="idTpo">El tipo de documento</param>
        /// <see cref="IdTpo"/>
        public void setIdTpo(int idTpo)
        {
            this.idTpo = idTpo;
        }
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
         * @return the archivo
         */
        public String getArchivo()
        {
            return archivo;
        }
        /**
         * @param archivo the archivo to set
         */
        public void setArchivo(String archivo)
        {
            this.archivo = archivo;
        }
        /**
         * @return the posicion
         */
        public int getPosicion()
        {
            return posicion;
        }
        /**
         * @param posicion the posicion to set
         */
        public void setPosicion(int posicion)
        {
            this.posicion = posicion;
        }
        /**
         * @return the tamanio
         */
        public int getTamanio()
        {
            return tamanio;
        }
        /**
         * @param tamanio the tamanio to set
         */
        public void setTamanio(int tamanio)
        {
            this.tamanio = tamanio;
        }
        /**
         * @return the frase
         */
        public String getFrase()
        {
            return frase;
        }
        /**
         * @param frase the frase to set
         */
        public void setFrase(String frase)
        {
            this.frase = frase;
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
         * @return the posicionParte
         */
        public int getPosicionParte()
        {
            return posicionParte;
        }
        /**
         * @param posicionParte the posicionParte to set
         */
        public void setPosicionParte(int posicionParte)
        {
            this.posicionParte = posicionParte;
        }

    }
}
