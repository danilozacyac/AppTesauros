using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mx.gob.scjn.ius_common.TO;

namespace mx.gob.scjn.ius_common.DAO
{
    public interface LeyDAO
    {
        /// <summary>
        /// Obtiene una ley en específico.
        /// </summary>
        /// <param name="id">el identificador de la ley</param>
        /// <return> Los datos de la ley</return>
        ///
         LeyTO getLeyPorId(int id);
        /// <summary>
        /// Obtiene una lista de artículos que coinciden
        /// con los parámetros especificados.</summary>
        /// <param name="articuloIdLey">Los parámetros de búsqueda</param>
        /// <return> La lista de artículos que coinciden con los parámetros.</return>
        ///
         List<ArticulosTO> getArticulos(ArticulosTO articuloIdLey);
         /// <summary>
         ///     Obtiene una lista de los archivos asociados como anexos a la ley
         /// </summary>
         /// <param name="articulo" type="mx.gob.scjn.ius_common.TO.ArticulosTO">
         ///     <para>
         ///         El identificador del artículo, ley y referencia para encontrar los
         ///         archivos asociados.
         ///     </para>
         /// </param>
         /// <returns>
         ///     La lista de los archivos asociados.
         /// </returns>
         List<String> getArchivos(ArticulosTO articulo);

         List<ArticulosTO> getArticulosPorIUS(long tesis);

         List<ArticulosTO> getArticulosEst(ArticulosTO parametros);
    }
}
