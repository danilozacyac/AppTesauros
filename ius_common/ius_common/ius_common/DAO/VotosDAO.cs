using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mx.gob.scjn.ius_common.TO;

namespace mx.gob.scjn.ius_common.DAO
{
public interface VotosDAO {
	/**
	 * Obtiene todos los votos
	 * @return la lista de todos los votos
	 */
    List <VotosTO> getAll();
	/**
	 * Obtiene todos los votos de una serie de partes.
	 * @param partes El criterio de la búsqueda.
	 * @return Los votos que cumplen con el criterio de búsqueda.
	 */
	 List <VotosTO> getPorPartes(int[] partes);
	/**
	 * Busca los votos que contienen un determinado consecutivo.
	 * @param Consec el consecutivo del voto.
	 * @return La lista con los votos que cumplen con el criterio.
	 */
	 List <VotosTO> getPorConsec(int Consec);
	/**
	 * Obtiene los votos por epocas determinadas.
	 * @param epocas Los parámetros de búsqueda.
	 * @return La lista de votos que coinciden.
	 */
	 List <VotosTO> getVotos(EpocasTO epocas);
	/**
	 * Obtiene los votos que coinciden con las partes enviadas
	 * @param epocas Los parámetros de la búsqueda
	 * @return La lista de coincidencias.
	 */
	 List <VotosTO> getVotos(PartesTO epocas);
	/**
	 * Encuentra los votos que coinciden con una palabra determinada
	 * @param palabra La palabra o patrón de búsqueda
	 * @param orderBy Se ordena pro eel campo descrito
	 * @param orderType Se ordena de manera ASCendente o DESCendiente
	 * @return La lista de coincidencias.
	 */
	 List <VotosTO> getVotosPorPalabra(BusquedaTO busqueda);
	/**
	 * Obtiene los votos que tienen identificadores determinados.
	 * @param parametros La lista de los identificadores
	 * @return La lista de coincidencias
	 */
	 List <VotosTO> getVotos(MostrarPorIusTO parametros);
	/**
	 * Obtiene un determinado voto
	 * @param id el identificador del voto
	 * @return El voto coincidente
	 */
	 VotosTO getVotos(int  id);
	/**
	 * Obtiene la lista de partes de un voto
	 * @param parametros Los parámetros de búsqueda
	 * @return La lista de las partes del voto.
	 */
	 List<VotosPartesTO> getVotosPartes(MostrarPartesIdTO parametros);
	/**
	 * Obtiene las tesis relacionadas con la ejecutoria.
	 * @param id el identificador de la ejecutoria.
	 * @return La lista con las tesis que se obtienen de la ejecutoria.
	 */
	 List<RelDocumentoTesisTO> getRelTesis(String id);
	/**
	 * Obtiene las ejecutorias que están relacionados con un determinado
	 * voto.
	 * @param id el identificador del voto.
	 * @return La lista con las ejecutorias relacionados.
	 */
	 List<RelVotoEjecutoriaTO> getRelVotoEjecutoria(String id);
    /// <summary>
    /// Obtiene las clasificaciones de los votos
    /// </summary>
    /// <returns>La lista de todas las clasificaciones</returns>
     List<ClassificacionTO> getClasificacion();
    /// <summary>
    /// Obitiene los votos que cumplen con una busqueda secuencial
    /// y una clasificacion determinada.
    /// </summary>
    /// <param name="parte">Los parametros de la busqueda secueencial</param>
    /// <param name="list">La lista de clasificaciones que se buscarán</param>
    /// <returns>La lista de votos que cumplen con los parámetros</returns>
     List<VotosTO> getVotos(PartesTO parte, List<ClassificacionTO> list);
    /// <summary>
    /// Obtiene las tablas que pertenecen a un determinado voto
    /// </summary>
    /// <param name="p">El identificador del Voto</param>
    /// <returns>Las tablas que le corresponden</returns>
     List<TablaPartesTO> getTablas(string p);
}
}
