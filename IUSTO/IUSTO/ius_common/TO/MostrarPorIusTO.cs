using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace mx.gob.scjn.ius_common.TO
{
    [DataContract]
    public class MostrarPorIusTO
    {
        ///<summary>
        /// El listado con los enteros que se buscan o se encontraron.
        ///</summary>
        private List<int> listado;
        [DataMember]
        public List<int> Listado { get { return listado; } set { setListado(value); } }
        /**
         * El nombre de la columna o campo por el cual
         * se ordenará el resultado.
         */
        private String orderBy;
        [DataMember]
        public String OrderBy { get { return getOrderBy(); } set { setOrderBy(value); } }
        /**
         * El tipo de ordenamiento, si es asc o desc,
         */
        private String orderType;
        [DataMember]
        public String OrderType { get { return getOrderType(); } set { setOrderType(value); } }
        /**
         * La columna por la cual se va a filtrar.
         */
        private String filterBy;
        [DataMember]
        public String FilterBy { get { return getFilterBy(); } set { setFilterBy(value); } }
        /**
         * El valor por el que se filtrará.
         */
        private String filterValue;
        [DataMember]
        public String FilterValue { get { return getFilterValue(); } set { setFilterValue(value); } }
        /**
         * En las búsquedas especiales indica el valor de la
         * búsqueda.<BR>
         * En las búsquedas por Indice el valor del índice buscado.
         */
        private String busquedaEspecialValor;
        [DataMember]
        public String BusquedaEspecialValor { get { return getBusquedaEspecialValor(); } set { setBusquedaEspecialValor(value); } }
        /**
         * En las búsquedas por índice -el caso de materias-
         * la letra por la cual se filtrará el resultado.
         */
        private int letra;
        [DataMember]
        public int Letra { get { return getLetra(); } set { setLetra(value); } }
        /**
         * En las búsquedas por índices -el caso de materias-
         * la tabla de donde se obtienen los datos.
         */
        private String tabla;
        [DataMember]
        public String Tabla { get { return getTabla(); } set { setTabla(value); } }
        /**
         * @return the tabla
         */
        public String getTabla()
        {
            return tabla;
        }
        /**
         * @param tabla the tabla to set
         */
        public void setTabla(String tabla)
        {
            this.tabla = tabla;
        }
        /**
         * @return the letra
         */
        public int getLetra()
        {
            return letra;
        }
        /**
         * @param letra the letra to set
         */
        public void setLetra(int letra)
        {
            this.letra = letra;
        }
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
         * @return the filterBy
         */
        public String getFilterBy()
        {
            return filterBy;
        }
        /**
         * @param filterBy the filterBy to set
         */
        public void setFilterBy(String filterBy)
        {
            this.filterBy = filterBy;
        }
        /**
         * @return the listado
         */
        public List<int> getListado()
        {
            return listado;
        }
        /**
         * @param listado the listado to set
         */
        public void setListado(List<int> listado)
        {
            this.listado = listado;
        }
        /**
         * @return the orderBy
         */
        public String getOrderBy()
        {
            return orderBy;
        }
        /**
         * @param orderBy the orderBy to set
         */
        public void setOrderBy(String orderBy)
        {
            this.orderBy = orderBy;
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
        /**
         * @return the busquedaEspecialValor
         */
        public String getBusquedaEspecialValor()
        {
            return busquedaEspecialValor;
        }
        /**
         * @param busquedaEspecialValor the busquedaEspecialValor to set
         */
        public void setBusquedaEspecialValor(String busquedaEspecialValor)
        {
            this.busquedaEspecialValor = busquedaEspecialValor;
        }
    }
}
