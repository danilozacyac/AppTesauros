using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mx.gob.scjn.ius_common.TO
{
    /**
     * Define los parámetros a utilizar en la búsqueda por partes
     * @author Carlos de Luna Saenz
     *
     */
    public class PartesElectoralTO
    {
        /**
         * Define si el ordenamiento es ascendente o descendente.
         */
        private String orderType;
        /**
         * Número de parte a buscar.
         */
        private int[] parte;
        /**
         * La columna por la cual se tiene que ordenar la consulta.
         */
        private String orderBy;
        /**
         * La columna por la cual se va a filtrar.
         */
        private String filterBy;
        /**
         * El valor del filtrado.
         */
        private String filterValue;
        /**
         * @return the filterValue
         */
        public String getFilterValue()
        {
            return filterValue;
        }
        /**
         * @param filterValue the filterValue to set
         */
        public void setFilterValue(String filterValue)
        {
            this.filterValue = filterValue;
        }
        /**
         * la columna por la cual se va a filtra.
         * @return filterByParam la colummna a filtrar
         */
        public String getFilterBy()
        {
            return filterBy;
        }
        /**
         * la columna por la cual se va a filtra.
         * @param filterByParam la colummna a filtrar
         */
        public void setFilterBy(String filterByParam)
        {
            this.filterBy = filterByParam;
        }
        /**
         * Obtiene el número de parte por el cual se va a obtener 
         * los datos dentro del query;
         * @return la parte por la cual se botendrán los resultados
         */
        public int[] getParte()
        {
            return parte;
        }
        /**
         * Define el número de parte por el cual se tiene los resultados.
         * @param La parte a buscar.
         */
        public void setParte(int[] parte)
        {
            this.parte = parte;
        }
        /**
         * define la columna del ordenamiento.
         * @return la columna por la cual se va a ordenar.
         */
        public String getOrderBy()
        {
            return orderBy;
        }
        /**
         * la columna por la cual se va a ordenar.
         * @param orderBy la colummna a ordenar
         */
        public void setOrderBy(String orderBy)
        {
            this.orderBy = orderBy;
        }
        /**
         * Constructor vacio por si alguna aplicación 
         * necesita de su existencia.
         * Se define una columna para ordena por onisión.
         */
        public PartesElectoralTO()
        {
            this.setOrderBy("ius");
            this.setOrderType("asc");
        }
        /**
         * @return the orderType
         */
        public String getOrderType()
        {
            return orderType;
        }
        /**
         * @param orderType the orderType to set
         */
        public void setOrderType(String orderType)
        {
            this.orderType = orderType;
        }
    }
}
