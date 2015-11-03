using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
namespace mx.gob.scjn.ius_common.TO
{
    /// <summary>
    /// Esta clase representa a los nodos de presentación de un arbol
    /// de despliegue, es útil si la información se desplegará de manera
    /// de árbol y se debe utilizar como objeto de transferencia entre
    /// las capas de negocio y presentación, exclusivamente. El objeto
    /// de negocio será el encargado de hacer la conversión para que el
    /// objeto de presentación haga únicamente el pintado del nodo sin
    /// tener que saber como es que son tratados los datos.</summary>
    /// <remarks name="author"> Carlos de Luna Sáenz.</remarks>
    /// 
    [DataContract]
    public class TreeNodeDataTO
    {
        /// <summary>
        /// La etiqueta que va a mostrar el arbol de selección.
        /// </summary>
        /// 
        [DataMember]
        public String Label { get { return this.getLabel(); } set { this.setLabel(value); } }
        private String label;
        [DataMember]
        public String Padre { get { return this.getPadre(); } set { this.setPadre(value); } }
        private String padre;
        /// <summary>
        /// La liga a la que se va a dirigir el arbol de selección.
        /// </summary>
        /// 
        [DataMember]
        public String Href { get { return this.getHref(); } set { this.setHref(value); } }
        private String href;
        /// <summary>
        /// El identificador del objeto del nodo.
        /// </summary>
        /// 
        [DataMember]
        public String Id { get { return this.getId(); } set { this.setId(value); } }
        private String id;
        /// <summary>
        /// El frame donde se abrirá. En la parte de negocio un 0
        /// indica nodo terminal, y un 1 indica que todavía tiene hijos.
        /// </summary>
        /// 
        [DataMember]
        public String Target { get { return this.getTarget(); } set { this.setTarget(value); } }
        private String target;
        /// <summary>
        /// Indica si es hoja o rama.
        /// </summary>
        /// 
        [DataMember]
        public Boolean IsLeaf { get { return this.getIsLeaf(); } set { this.setIsLeaf(value); } }
        private Boolean isLeaf;
        /// <summary>
        /// Indica si es una hoja, en caso
        /// afirmativo o una rama en caso negativo.</summary>
        /// <returns> Verdadero si es hoja, Falso si no lo es.</returns>
        ///
        public Boolean getIsLeaf()
        {
            return isLeaf;
        }
        /// <summary>
        /// Define el valor para indicar al árbol si es o no
        /// una hoja.</summary>
        /// <param name="isLeaf"> verdadero para indicar que es hoja.</param>
        ///
        public void setIsLeaf(Boolean isLeaf)
        {
            this.isLeaf = isLeaf;
        }
        /// <summary>
        /// Obtiene el valor de la etiqueta a desplegar.
        /// </summary>
        /// <returns> La etiqueta a desplegar.
        /// </returns>
        public String getLabel()
        {
            return label;
        }
        /// <summary>
        /// Define la etiqueta a desplegar.
        /// </summary>
        /// <param name="label"> La etiqueta.</param>
        ///
        public void setLabel(String label)
        {
            this.label = label;
        }
        /// <summary>
        /// Obtiene la liga o tipo de liga que se tiene que desplegar.
        /// 0.- Obtener más hijos
        /// 1.- Desplegar resultados.
        /// </summary>
        /// <returns> La referencia a desplegar.</returns>
        ////
        public String getHref()
        {
            return href;
        }
        /// <summary>
        /// Obtiene la liga o tipo de liga que se tiene que desplegar.
        /// 0.- Obtener más hijos
        /// 1.- Desplegar resultados.
        /// </summary>
        /// <param name="href"> La referencia a desplegar.</param>
        ///
        public void setHref(String href)
        {
            this.href = href;
        }
        /// <summary>
        /// Define el identificador del objeto al cual representa el nodo.
        /// </summary>
        /// <returns>el identificador del objeto.</returns>
        ///
        public String getId()
        {
            return id;
        }
        /// <summary>
        /// Define el identificador del objeto que representa el nodo.
        /// </summary>
        /// <param name="id"> El id del objeto.</param>
        ///
        public void setId(String id)
        {
            this.id = id;
        }
        /// <summary>
        /// Define el target de despliegue de la información.
        /// </summary>
        /// <returns> El destino de la información.</returns>
        ///
        public String getTarget()
        {
            return target;
        }
        /// <summary>
        /// Define el destino de la información.
        /// </summary>
        /// <param name="target">el destino de la información</param>
        ///
        public void setTarget(String target)
        {
            this.target = target;
        }
        private String getPadre()
        {
            return padre;
        }
        private void setPadre(String value)
        {
            this.padre = value;
        }
    }
}
