using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mx.gob.scjn.ius_common.TO;
using mx.gob.scjn.ius_common.context;
using mx.gob.scjn.ius_common.DAO;
using Common.Logging;

namespace mx.gob.scjn.ius_common.ius
{
///<summary>
///Clase que se hace cargo de la lógica de negocios relacionada
/// con los acuerdos.
/// </summary>
/// <remarks author="Carlos de Luna Sáenz"></remarks>
public class Acuerdos {
	/**
	 * La bitácora de salida de eventos.
	 */
	private ILog log = LogManager.GetLogger("mx.gob.scjn.iuscommon.ius.Ejecutorias");
	/**
	 * Contexto de spring de donde se accederá a los DAOs
	 */
	private IUSApplicationContext contexto;
	/**
	 * Nuestro constructor.
	 */
	public Acuerdos(){
		log.Info("Iniciando el contexto, es posible que haya que copiar los xml hacia las pruebas");
		try{
		    this.contexto = new IUSApplicationContext();
		} catch (Exception e ) {
			log.Info("Fallo al iniciar el contexto, la excepcion es:"+e.Message);
			log.Info(e.StackTrace);
		}
	}
	/**
	 * Obtiene una lista de Acuerdos con todos los que existan en la BD.
	 * @return La lista de todos los acuerdos existentes.
	 */
	public List <AcuerdosTO> getAll(){
		AcuerdosDAO daoAcuerdos = (AcuerdosDAO)contexto.getInitialContext().GetObject("AcuerdosDAO");
		List <AcuerdosTO> resultado =  daoAcuerdos.getAll();
		return resultado;
	}
	/**
	 * Obtiene los acuerdos que tengan la palabra buscada.
	 * @param palabra la palabra a buscar.
	 * @param orderBy la columna por la cual se ordena la busqueda
	 * @param orderType el tipo de ordenamiento
	 * @return la lista de resultados
	 */
    public List<AcuerdosTO> getAcuerdosPorPalabra(BusquedaTO parametros)
    {
		AcuerdosDAO daoAcuerdos = (AcuerdosDAO)contexto.getInitialContext().GetObject("AcuerdosDAO");
		List <AcuerdosTO> resultado =  daoAcuerdos.getAcuerdosPorPalabra(parametros);
		return resultado;		
	}
	/**
	 * Obtiene una lista de acuerdos conforme a la seleccion realizada por
	 * el panel.
	 * @param partes las partes seleccionadas dentro del panel.
	 * @return La lista de acuerdos que coinciden con el criterio de
	 *         búsqueda.
	 */
	public List <AcuerdosTO> getPanel(BusquedaTO panel){
        bool[][] partesPalabra = panel.Acuerdos;
        String orderBy = panel.OrdenarPor;
        String orderType = "asc";
        int partesBuscar = obtenPartesInt(panel);
        PartesTO parte = new PartesTO();
        AcuerdosDAO daoTesis = (AcuerdosDAO)contexto.getInitialContext().GetObject("AcuerdosDAO");
        parte.setParte(partesBuscar);
        parte.setOrderBy(orderBy);
        parte.setOrderType(orderType);
        /******************************************************************
         ********           Busqueda por palabra con, probablemente  ******
         ********           muchas selecciones en el panel, solo     ******
         ******************************************************************/
        if (panel.Palabra != null)
        {
            int[] conjuntoPartes = obtenPartes(panel);
            return daoTesis.getAcuerdosPorPalabra(panel);
        }
        /************Secuencial*****************/

        int partes = obtenPartesInt(panel);
        //PartesTO parte = new PartesTO();
        parte.setOrderBy(panel.getOrdenarPor());
        AcuerdosDAO daoAcuerdos = (AcuerdosDAO)contexto.getInitialContext().GetObject("AcuerdosDAO");
        parte.setParte(partes);
        List<AcuerdosTO> resultado;
        resultado = daoAcuerdos.getAcuerdos(parte);
        return resultado; 
        
	}
	
	private EpocasTO obtenPartes(bool[][] origen){
		EpocasTO partes = new EpocasTO();
		List<EpocasSalasTO> epocasSalas = new List<EpocasSalasTO>();
		EpocasSalasTO epocaActual = null;
		int ancho = 0;
		int largo = 0;
		int recorridoAncho = 0;
		int recorridoLargo = 0;
		ancho = origen[0].Length;
		largo = origen.Length;
		for (recorridoAncho = 0; 
		     recorridoAncho < ancho; 
		     recorridoAncho ++){
			for (recorridoLargo = 0; 
			     recorridoLargo < largo; 
			     recorridoLargo ++){
				if (origen[recorridoLargo][recorridoAncho]){
					epocaActual = new EpocasSalasTO();
					epocaActual.setSala(recorridoLargo+1);
					epocaActual.setEpoca(5-recorridoAncho);
					epocasSalas.Add(epocaActual);
				}
			}
		}
		partes.setEpocasSalas(epocasSalas);
		return partes;
	}
    /// <summary>
    /// Obtiene las épocas y las salas de las que se 
    /// quiere realizar la búsqueda.
    /// <param name="busqueda"> Un objeto con los arreglos que contienen las 
    ///        selecciones del usuario para la búsqueda.</param>
    /// <returns> La lista de todas las tesis que cumplen con los
    ///         parámetros establecidos.</returns>
    ////
    private int[] obtenPartes(BusquedaTO busqueda)
    {
        List<int> epocasSalas = new List<int>();
        int ancho = 0;
        int largo = 0;
        int recorridoAncho = 0;
        int recorridoLargo = 0;
        int contador = 0;
        ancho = busqueda.getEpocas()[0].Length;
        largo = busqueda.getEpocas().Length;
        for (recorridoAncho = 0;
             recorridoAncho < ancho;
             recorridoAncho++)
        {
            for (recorridoLargo = 0;
                 recorridoLargo < largo;
                 recorridoLargo++)
            {
                contador++;
                if (busqueda.getEpocas()[recorridoLargo][recorridoAncho])
                {
                    epocasSalas.Add(contador);
                }
            }
        }
        contador = 100;
        ancho = busqueda.getApendices()[0].Length;
        largo = busqueda.getApendices().Length;
        for (recorridoAncho = 0;
             recorridoAncho < ancho;
             recorridoAncho++)
        {
            for (recorridoLargo = 0;
                 recorridoLargo < largo;
                 recorridoLargo++)
            {
                contador++;
                if (busqueda.getApendices()[recorridoLargo][recorridoAncho])
                {
                    epocasSalas.Add(contador);
                }
            }
        }
        contador = 150;
        ancho = busqueda.getAcuerdos()[0].Length;
        largo = busqueda.getAcuerdos().Length;
        for (recorridoAncho = 0;
             recorridoAncho < ancho;
             recorridoAncho++)
        {
            for (recorridoLargo = 0;
                 recorridoLargo < largo;
                 recorridoLargo++)
            {
                contador++;
                if (busqueda.getAcuerdos()[recorridoLargo][recorridoAncho])
                {
                    epocasSalas.Add(contador);
                }
            }
        }
        return epocasSalas.ToArray();
    }
	/**
	 * Obtiene el entero de una parte dada en el panel de búsqueda.
	 * @param busqueda La representación lógica del panel que se llenó para
	 *                 la búsqueda
	 * @return El número correspondiente a la parte
	 */
	private int obtenPartesInt(BusquedaTO busqueda){
		int partes = 0;
		int parte = 0;
		int ancho = 0;
		int largo = 0;
		int recorridoAncho = 0;
		int recorridoLargo = 0;
		int contador = 0;
		ancho = busqueda.getEpocas()[0].Length;
		largo = busqueda.getEpocas().Length;
		for (recorridoAncho = 0; 
		     recorridoAncho < ancho; 
		     recorridoAncho ++){
			for (recorridoLargo = 0; 
			     recorridoLargo < largo; 
			     recorridoLargo ++){
				contador ++;
				if (busqueda.getEpocas()[recorridoLargo][recorridoAncho]){
					parte = contador;
				}
			}
		}
		if (parte == 0){
			contador = 100;
			ancho = busqueda.getApendices()[0].Length;
			largo = busqueda.getApendices().Length;
			for (recorridoAncho = 0; 
			     recorridoAncho < ancho; 
			     recorridoAncho ++){
				for (recorridoLargo = 0; 
				     recorridoLargo < largo; 
				     recorridoLargo ++){
					contador ++;
					if (busqueda.getApendices()[recorridoLargo][recorridoAncho]){
						parte = contador;
					}
				}
			}
		}
		if (parte == 0){
			contador = 150;
			ancho = busqueda.getAcuerdos()[0].Length;
			largo = busqueda.getAcuerdos().Length;
			for (recorridoAncho = 0; 
			     recorridoAncho < ancho; 
			     recorridoAncho ++){
				for (recorridoLargo = 0; 
				     recorridoLargo < largo; 
				     recorridoLargo ++){
					contador ++;
					if (busqueda.getAcuerdos()[recorridoLargo][recorridoAncho]){
						parte = contador;
					}
				}
			}
		}
		partes = parte-1;
		return partes;
	}
	/**
	 * Obtiene una lista de los acuerdos que cumplen con una serie de 
	 * identificadores.
	 * @param busqueda Un objeto con los arreglos que contienen los ids
	 *                   para la búsqueda, en el campo IUS va el id.
	 * @return La lista de todas los acuerdos que cumplen con los
	 *         parámetros establecidos.
	 */
	public List<AcuerdosTO> getAcuerdos(MostrarPorIusTO busqueda) {
		AcuerdosDAO daoAcuerdos= (AcuerdosDAO)contexto.getInitialContext().GetObject("AcuerdosDAO");
		List <AcuerdosTO> resultado =  daoAcuerdos.getAcuerdos(busqueda);
		return resultado;
	}
	/**
	 * Obtiene las partes de un acuerdo determinado.
	 * @param id El identificador del acuerdo
	 * @param orderBy campo por el cual se quiere ordenar
	 * @param orderType tipo de ordenamiento
	 * @return La lista de las partes.
	 */
	public List<AcuerdosPartesTO> getPartesAcuerdos(int id, String orderBy, String orderType) {
		MostrarPartesIdTO parametros = new MostrarPartesIdTO();
		parametros.setId(id);
		parametros.setOrderBy(orderBy);
		parametros.setOrderType(orderType);
		AcuerdosDAO daoAcuerdos= (AcuerdosDAO)contexto.getInitialContext().GetObject("AcuerdosDAO");
		List <AcuerdosPartesTO> resultado =  daoAcuerdos.getAcuerdosPartes(parametros);
		return resultado;
	}
	/**
	 * Obtiene un acuerdo determinado
	 * @param id el id del acuerdo
	 * @return el acuerdo requerido.
	 */
	public AcuerdosTO getAcuerdo(int id) {
		AcuerdosDAO daoAcuerdos= (AcuerdosDAO)contexto.getInitialContext().GetObject("AcuerdosDAO");
		AcuerdosTO resultado =  daoAcuerdos.getAcuerdo(id);
		return resultado;
	}
    /**
             * Obtiene una lista de las tablas que corresponden al mismo Id
             * de ejecutoria.
             * @param id identificador de la ejecutoria.
             * @return la lista de las tablas de la ejecutoria.
             */
    public List<TablaPartesTO> getTablas(int id)
    {
        AcuerdosDAO acuerdos = (AcuerdosDAO)contexto.getInitialContext().GetObject("AcuerdosDAO");
        List<TablaPartesTO> resultado = acuerdos.getTablas(id + "");
        return resultado;
    }
}
}
