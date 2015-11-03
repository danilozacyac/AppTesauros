using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mx.gob.scjn.ius_common.TO;

namespace mx.gob.scjn.ius_common.DAO
{
public interface AcuerdosDAO {
	/**
	 * Obtiene todos los acuerdos en BD.
	 * @return la lista con todos los acuerdos.
	 */
	 List <AcuerdosTO> getAll();
	/**
	 * Obtiene los acuerdos que coinciden con determinadas partes.
	 * @param partes Las partes de la búsqueda.
	 * @return Los acuerdos que coinciden con la búsqueda
	 */
     List<AcuerdosTO> getPorPartes(int[]  partes);
	/**
	 * Obtiene una lista de acuerdos que tienen un consecutivo determinado.
	 * @param Consec el consecutivo de la búsqeda.
	 * @returnLos acuerdos que coinciden con dicha búsqueda.
	 */
	 List <AcuerdosTO> getPorConsec(int Consec);
	/**
	 * Obtiene los acuerdos que coinciden con una época determinada.
	 * @param epocas los parámetros de búsqueda.
	 * @return la lista que coincide con los criterios de búsqueda.
	 */
	 List <AcuerdosTO> getAcuerdos(EpocasTO epocas);
	/**
	 * Obtiene los acuerdos que coinciden con una lista de épocas.
	 * @param epocas las épocas de la búsqueda.
	 * @return La lista de los acuerdos coincidentes. 
	 */
	 List <AcuerdosTO> getAcuerdos(PartesTO epocas);
	/**
	 * Busca un acuerdo que contenga un patrón de busqueda por palabras.
	 * @param palabra la palabra buscada.
	 * @param orderBy El campo para ordenar
	 * @param orderType El tipo de ordenamiento.
	 * @return La lista de resultados coincidentes.
	 */
     List<AcuerdosTO> getAcuerdosPorPalabra(BusquedaTO parametros);
	/**
	 * Obtiene los acuerdos que coinciden con una lista de ids determinado
	 * @param parametros los parámetros de la búsqueda.
	 * @return La lista que coincide con los IUS solicitados.
	 */
	 List <AcuerdosTO> getAcuerdos(MostrarPorIusTO parametros);
	/**
	 * Obtiene un acuerdo en específico.
	 * @param id el identificador del acuerdo.
	 * @return El acuerdo en específico.
	 */
	 AcuerdosTO getAcuerdo(int id);
	/**
	 * Obtiene las partes de un documento en específico.
	 * @param parametros El identificador y demás parámetros de búsqueda de las partes.
	 * @return la lista de partes de los acuerdos que coinciden con la búsqueda.
	 */
	 List<AcuerdosPartesTO> getAcuerdosPartes(MostrarPartesIdTO parametros);

     List<TablaPartesTO> getTablas(string p);
}
}
