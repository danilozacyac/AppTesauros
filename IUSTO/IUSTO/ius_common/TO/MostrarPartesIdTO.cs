using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mx.gob.scjn.ius_common.TO
{
    /**
     * Esta clase es para la habilitación de busquedas de
     * partes de las ejecutorias.
     * @author Carlos de Luna Saenz
     *
     */
    public class MostrarPartesIdTO
    {
        int id;
        String orderBy;
        String orderType;
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
    }
}
