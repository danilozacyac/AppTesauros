using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mx.gob.scjn.ius_common.TO;
namespace mx.gob.scjn.ius_common.DAO
{
    public interface EjecutoriasDAO
    {
        /**
         * Obtiene todas las ejecutorias
         */
         List<EjecutoriasTO> getAll();
        /**
         * Obtiene todas las ejecutorias que responden al panel
         * de busqueda enviado
         * @param busqueda las partes seleccionadas del panel
         * @return la lista de ejecutorias que cumplen con el criterio de
         *         búsqueda.
         */
         List<EjecutoriasTO> getEjecutorias(PartesTO busqueda);
        /**
         * Obtiene todas las ejecutorias de determinadas partes.
         * @param partes las partes a las que corresponden las ejecutorias
         * @return la lista de ejecutorias que cumplen con el criterio de búsqueda.
         */
         List<EjecutoriasTO> getEjecutorias(int[] partes);
        /**
         * Trae todas las ejecutorias que se encuentran con un id igual al
         * existente en el arreglo de enteros del objeto identificadores,
         * ordenando el resultado de acuerdo a los dos valores adicionales
         * del mismo objeto.
         * @param identificadores Los parámetros de la búsqueda de las
         *                        ejecutorias
         * @return La lista de ejecutorias que cumplen con los criterios.
         */
         List<EjecutoriasTO> getEjecutorias(MostrarPorIusTO identificadores);
        /**
         * Obtiene todas las ejecutorias de acuerdo a una búsqueda en el panel
         * de búsqueda secuencial.
         * @param busqueda la selección en el panel de búsqueda.
         * @return La lista de ejecutorias que cumplen con el criterio.
         */
         List<EjecutoriasPartesTO> getPartesPorId(MostrarPartesIdTO busqueda);
        /**
         * Realiza la búsqueda por palabras utilizando Compass.
         * @param palabras Las palabras a buscar
         * @return la lista de ejecutorias con la palabra en ella.
         */
         List<EjecutoriasTO> getPorPalabra(String palabras);
        /**
         * Obtiene una ejecutoria determinada por un Id.
         * @param id el id de la ejecutoria que se da.
         * @return la ejecutoria con el id determinado.
         */
         EjecutoriasTO getEjecutoriaPorId(int id);
        /**
         * Obtiene las tesis relacionadas con la ejecutoria.
         * @param id el identificador de la ejecutoria.
         * @return La lista con las tesis que se obtienen de la ejecutoria.
         */
         List<RelDocumentoTesisTO> getRelTesis(String id);
        /**
         * Obtiene los votos que están relacionados con una determinada
         * ejecutoria.
         * @param id el identificador de la ejecutoria.
         * @return La lsita con los votos relacionados.
         */
         List<RelVotoEjecutoriaTO> getRelVotoEjecutoria(String id);
        /**
         * Obtiene las tablas relacionadas a la ejecutoria y los datos
         * para poder pintarse.
         * @param Id La ejecutoria de la que se buscan las tablas
         * @return La lista de tablas relacionadas con la ejecutoria.
         */
         List<TablaPartesTO> getTablas(String Id);
         List<EjecutoriasTO> getEjecutoriasPorPalabra(BusquedaTO parametros);
    }
}
