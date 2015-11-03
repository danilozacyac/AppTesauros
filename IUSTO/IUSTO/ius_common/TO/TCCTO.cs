using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mx.gob.scjn.ius_common.TO
{
    /**
     * Maneja los datos referentes a los
     * Tribunales Colegiados de Circuito
     * @author Carlos de Luna Saenz
     *
     */
    public class TCCTO
    {
        /**
         * El identificador del Tribunal
         * Colegiado de Circuito.
         */
        private int idTribunal;
        /**
         * Descripcion del Tribunal.
         */
        private String descripcion;
        /**
         * Circuito al que pertenece.
         */
        private int circuito;
        /**
         * Materia sobre la que dicta.
         */
        private int materia;
        /**
         * Orden
         */
        private int ord;
        /**
         * Descripcion en Mayúsculas
         */
        private String descripcionMayusculas;
        /**
         * Contador de la tabla.
         */
        private int contador;
        /**
         * @return the idTribunal
         */
        public int getIdTribunal()
        {
            return idTribunal;
        }
        /**
         * @param idTribunal the idTribunal to set
         */
        public void setIdTribunal(int idTribunal)
        {
            this.idTribunal = idTribunal;
        }
        /**
         * @return the descripcion
         */
        public String getDescripcion()
        {
            return descripcion;
        }
        /**
         * @param descripcion the descripcion to set
         */
        public void setDescripcion(String descripcion)
        {
            this.descripcion = descripcion;
        }
        /**
         * @return the circuito
         */
        public int getCircuito()
        {
            return circuito;
        }
        /**
         * @param circuito the circuito to set
         */
        public void setCircuito(int circuito)
        {
            this.circuito = circuito;
        }
        /**
         * @return the materia
         */
        public int getMateria()
        {
            return materia;
        }
        /**
         * @param materia the materia to set
         */
        public void setMateria(int materia)
        {
            this.materia = materia;
        }
        /**
         * @return the ord
         */
        public int getOrd()
        {
            return ord;
        }
        /**
         * @param ord the ord to set
         */
        public void setOrd(int ord)
        {
            this.ord = ord;
        }
        /**
         * @return the descripcionMayusculas
         */
        public String getDescripcionMayusculas()
        {
            return descripcionMayusculas;
        }
        /**
         * @param descripcionMayusculas the descripcionMayusculas to set
         */
        public void setDescripcionMayusculas(String descripcionMayusculas)
        {
            this.descripcionMayusculas = descripcionMayusculas;
        }
        /**
         * @return the contador
         */
        public int getContador()
        {
            return contador;
        }
        /**
         * @param contador the contador to set
         */
        public void setContador(int contador)
        {
            this.contador = contador;
        }
    }
}
