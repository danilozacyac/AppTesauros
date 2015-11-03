using System.Runtime.Serialization;
using System;
namespace mx.gob.scjn.ius_common.TO
{
    /// <summary>
    /// Da los datos para la muestra de funcionarios cuando
    /// se hace la consulta de magistrados en el histórico.
    /// </summary>
    /// <remarks author="Carlos de Luna Saenz"></remarks>
    ///
    [DataContract]
    public class FuncionariosTO
    {
        /// <summary>
        /// El identificador de la epoca/instancia/fecha
        /// a la que pertenece el funcionario.
        ///</summary>
        ///
        [DataMember]
        public String Id { get { return this.getId(); } set { this.setId(value); } }
        private String id;
        /// <summary>
        /// El nombre del funcionario
        ///</summary>
        ///
        [DataMember]
        public String Funcionario { get { return this.getFuncionario(); } set { this.setFuncionario(value); } }
        private String funcionario;
        ///<sumary>
        /// El consecutivo con el que debe aparecer en la lista.
        ///</sumary>
        ///
        [DataMember]
        public String Consec { get { return this.getConsec(); } set { this.setConsec(value); } }
        private String consec;
        ///<summary>
        ///Obtiene el identificador del objeto y lo devuelve
        ///</summary>
        /// <return> el Identificador del funcionario.</return>
        ///
        public String getId()
        {
            return id;
        }
        /**
         * @param id the id to set
         */
        public void setId(String id)
        {
            this.id = id;
        }
        /**
         * @return the funcionario
         */
        public String getFuncionario()
        {
            return funcionario;
        }
        /**
         * @param funcionario the funcionario to set
         */
        public void setFuncionario(String funcionario)
        {
            this.funcionario = funcionario;
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

    }
}