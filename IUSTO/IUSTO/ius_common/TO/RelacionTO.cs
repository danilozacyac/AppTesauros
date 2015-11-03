using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace mx.gob.scjn.ius_common.TO
{
    /// <summary>
    /// Esta clase es el objeto de transferencia de las
    /// relaciones entre las diversas partes.
    /// </summary>
    /// <remarks author="Carlos de Luna Sáenz"></remarks>
    ///
    [DataContract]
    public class RelacionTO
    {
        /// <summary>
        /// El número de Ius de la tesis
        /// </summary>
        [DataMember]
        public String Ius { get { return this.getIus(); } set { this.setIus(value); } }
        private String ius;
        /// <summary>
        /// El identificador de la relación.
        /// </summary>
        /// 
        [DataMember]
        public String IdRel { get { return this.getIdRel(); } set { this.setIdRel(value); } }
        private String idRel;
        /// <summary>
        /// El descriptor de la relación
        /// </summary>
        /// 
        [DataMember]
        public String MiDescriptor { get { return this.getMiDescriptor(); } set { this.setMiDescriptor(value); } }
        private String miDescriptor;
        /// <summary>
        /// La sección a la que pertenece la relación.
        /// </summary>
        /// 
        [DataMember]
        public String Seccion { get { return this.getSeccion(); } set { this.setSeccion(value); } }
        private String seccion;
        /// <summary>
        /// La posición en caracteres donde empieza la liga.
        /// </summary>
        /// 
        [DataMember]
        public String Posicion { get { return this.getPosicion(); } set { this.setPosicion(value); } }
        private String posicion;
        /// <summary>
        /// El tipo de liga que es
        /// </summary>
        /// 
        [DataMember]
        public String Tipo { get { return this.getTipo(); } set { setTipo(value); } }
        private String tipo;
        /**
         * @return the iUS
         */
        public String getIus()
        {
            return ius;
        }
        /**
         * @param ius the iUS to set
         */
        public void setIus(String iusParam)
        {
            ius = iusParam;
        }
        /**
         * @return the idRel
         */
        public String getIdRel()
        {
            return idRel;
        }
        /**
         * @param idRel the idRel to set
         */
        public void setIdRel(String idRel)
        {
            this.idRel = idRel;
        }
        /**
         * @return the miDescriptor
         */
        public String getMiDescriptor()
        {
            return miDescriptor;
        }
        /**
         * @param miDescriptor the miDescriptor to set
         */
        public void setMiDescriptor(String miDescriptor)
        {
            this.miDescriptor = miDescriptor;
        }
        /**
         * @return the seccion
         */
        public String getSeccion()
        {
            return seccion;
        }
        /**
         * @param seccion the seccion to set
         */
        public void setSeccion(String seccion)
        {
            this.seccion = seccion;
        }
        /**
         * @return the posicion
         */
        public String getPosicion()
        {
            return posicion;
        }
        /**
         * @param posicion the posicion to set
         */
        public void setPosicion(String posicionParam)
        {
            posicion = posicionParam;
        }
        /**
         * @return the tipo
         */
        public String getTipo()
        {
            return tipo;
        }
        /**
         * @param tipo the tipo to set
         */
        public void setTipo(String tipoParam)
        {
            tipo = tipoParam;
        }

    }
}
