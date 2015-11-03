using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mx.gob.scjn.ius_common.TO;

namespace mx.gob.scjn.ius_common.DAO
{
    /// <summary>
    /// Se encarga de generar y obtener los datos de los índices
    /// para la presentación de tesis.
    /// </summary>
    /// <remarks author="Carlos de Luna Sáenz"></remarks>
    ///
    ///
    public interface IndicesDAO
    {
        /**
         * Obtiene los hijos de un índice padre y regresa la
         * lista de estos.
         * @param idPadre el identificador del índice del cual se buscan los hijos.
         * @return La lista de los hijos.
         */
         List<CIndicesTO> obtenHijos(int idPadre);
        /**
         * Cuando los índices llegan al tribunal es necesario
         * obtener los datos de los tribunales.
         */
         List<TCCTO> getTribunal(int idPadre);
    }
}
