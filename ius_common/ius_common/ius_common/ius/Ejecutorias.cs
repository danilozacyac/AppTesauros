using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Logging;
using mx.gob.scjn.ius_common.context;
using mx.gob.scjn.ius_common.TO;
using mx.gob.scjn.ius_common.DAO;

namespace mx.gob.scjn.ius_common.ius
{
    /// <summary>
    /// Es la clase que se encarga de todo lo relacionado con los documentos de la ejecutoria,
    /// tanto el documento en general como sus partes.
    /// </summary>
    public class Ejecutorias
    {
        /// <summary>
        /// El manejador de logs.
        /// </summary>
        private ILog log = LogManager.GetLogger("mx.gob.scjn.iuscommon.ius.Ejecutorias");
        /// <summary>
        /// El contexto general de la aplicación que se encarga de la obtención de los DAO.
        /// </summary>
        private IUSApplicationContext contexto;
        /// <summary>
        /// Constructor por omisión, carga el contexto y el log.
        /// </summary>
        public Ejecutorias()
        {
            log.Info("Iniciando el contexto, es posible que haya que copiar los xml hacia las pruebas");
            try
            {
                this.contexto = new IUSApplicationContext();
            }
            catch (Exception e)
            {
                log.Info("Fallo al iniciar el contexto, la excepcion es:" + e.Message);
                log.Debug(e.StackTrace);
            }
        }
        ///<summary>
        /// Obtiene la ejecutoria correspondiente al id recibido.
        /// </summary>
        /// <param name="id"> el número de identificación de la ejecutorai</param>
        /// <returns>la ejecutoria con el id recibido</returns>
        public EjecutoriasTO getEjecutoria(int id)
        {
            EjecutoriasDAO ejecutoria = (EjecutoriasDAO)contexto.getInitialContext().GetObject("EjecutoriasDAO");
            EjecutoriasTO resultado = ejecutoria.getEjecutoriaPorId(id);
            return resultado;
        }
        ///<summary>
        /// Obtiene todas las ejecutorias
        /// </summary>
        /// <returns> la lista de todas las ejecutorias</returns>
        public List<EjecutoriasTO> getAll()
        {
            EjecutoriasDAO daoEjecutorias = (EjecutoriasDAO)contexto.getInitialContext().GetObject("EjecutoriasDAO");
            List<EjecutoriasTO> resultado = daoEjecutorias.getAll();
            return resultado;
        }
        /// <summary>
        /// Obtiene las ejecutorias correspondientes a la selección de un panel de consulta.
        /// </summary>
        /// <param name="busquedaCompleta">La búsqueda completa con sus parámetros.</param>
        /// <returns>La lista de aquellas ejecutorias que cumplen con los criterios de busqueda</returns>
        public List<EjecutoriasTO> getEjecutorias(BusquedaTO busquedaCompleta)
        {
            bool[][] partes = busquedaCompleta.Epocas;
            String orderBy = busquedaCompleta.OrdenarPor;
            String orderType = "asc";
            int partesBuscar = obtenPartesInt(partes);
            PartesTO parte = new PartesTO();
            EjecutoriasDAO daoTesis = (EjecutoriasDAO)contexto.getInitialContext().GetObject("EjecutoriasDAO");
            parte.setParte(partesBuscar);
            parte.setOrderBy(orderBy);
            parte.setOrderType(orderType);
            /******************************************************************
             ********           Busqueda por palabra con, probablemente  ******
             ********           muchas selecciones en el panel, solo     ******
             ******************************************************************/
            if (busquedaCompleta.Palabra != null)
            {
                int[] conjuntoPartes = obtenPartes(busquedaCompleta);
                return daoTesis.getEjecutoriasPorPalabra(busquedaCompleta);
            }
            /************Secuencial*****************/

            List<EjecutoriasTO> resultado = daoTesis.getEjecutorias(parte);
            return resultado;
        }

        /// <summary>
        /// Obtiene una lista de las ejecutorias que cumplen con una serie de 
        /// identificadores.
        /// </summary>
        /// <param name="busqueda">
        ///                   Un objeto con los arreglos que contienen los ids
        ///                   para la búsqueda, en el campo IUS va el id.
        /// </param>
        /// <returns>
        ///         La lista de todas las ejecutorias que cumplen con los
        ///         parámetros establecidos.
        ///</returns>
        public List<EjecutoriasTO> getEjecutorias(MostrarPorIusTO busqueda)
        {
            EjecutoriasDAO daoEjecutorias = (EjecutoriasDAO)contexto.getInitialContext().GetObject("EjecutoriasDAO");
            List<EjecutoriasTO> resultado = daoEjecutorias.getEjecutorias(busqueda);
            return resultado;
        }
        /**
         * Obtiene una lista de las ejecutorias que tienen una determinada palabra
         *        en la busqueda.
         * @param palabra la palabra a buscar en las tesis.
         * @param orderBy La columna por la cual se quiere ordenar la consulta.
         * @param orderType El tipo de ordenamiento que se quiere.
         * @return La lista de todas las tesis que cumplen con los
         *         parámetros establecidos.
         */
        public List<EjecutoriasTO> getEjecutoriasPorPalabra(String busqueda,
                String orderBy, String orderType)
        {
            //		List <Integer> numerosIds = null;
            EjecutoriasDAO daoEjecutorias = (EjecutoriasDAO)contexto.getInitialContext().GetObject("EjecutoriasDAO");
            List<EjecutoriasTO> resultado = daoEjecutorias.getPorPalabra(busqueda);
            //		List <EjecutoriasTO> resultado =  daoTesis.getEjecutorias(numerosIds);
            return resultado;
        }

        /**
         * Obtiene una lista de ejecutorias que corresponden al mismo Id
         * pero que contienen sus distintas partes dentro de la BD.
         * @param id identificador de la ejecutoria.
         * @param orderBy sobre cual columna se ordena.
         * @param orderType de que forma se ordena.
         * @return la lista de las partes de la ejecutoria.
         */
        public List<EjecutoriasPartesTO> getPartesEjecutoria(int id,
                String orderBy, String orderType)
        {
            MostrarPartesIdTO parametros = new MostrarPartesIdTO();
            parametros.setId(id);
            parametros.setOrderBy(orderBy);
            parametros.setOrderType(orderType);
            EjecutoriasDAO ejecutoria = (EjecutoriasDAO)contexto.getInitialContext().GetObject("EjecutoriasDAO");
            List<EjecutoriasPartesTO> resultado = ejecutoria.getPartesPorId(parametros);
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
            EjecutoriasDAO ejecutoria = (EjecutoriasDAO)contexto.getInitialContext().GetObject("EjecutoriasDAO");
            List<TablaPartesTO> resultado = ejecutoria.getTablas(id + "");
            return resultado;
        }
        /**
         * Obtiene todas las tesis relacionadas con una ejecutoria dada.
         * @param id el identificador de la ejecutoria.
         * @return la lista con la relación de números de tesis.
         */
        public List<RelDocumentoTesisTO> getRelTesis(String id)
        {
            EjecutoriasDAO ejecutorias = (EjecutoriasDAO)contexto.getInitialContext().GetObject("EjecutoriasDAO");
            List<RelDocumentoTesisTO> resultado = ejecutorias.getRelTesis(id);
            return resultado;
        }
        /// <summary>
        /// Obtiene todas los votos relacionadas con una ejecutoria dada.
        /// </summary>
        /// <param name="id"> el identificador de la ejecutoria.</param>
        /// <returns> la lista con la relación de números de voto.</returns>
        ///
        public List<RelVotoEjecutoriaTO> getRelVotoEjecutoria(String id)
        {
            EjecutoriasDAO ejecutorias = (EjecutoriasDAO)contexto.getInitialContext().GetObject("EjecutoriasDAO");
            List<RelVotoEjecutoriaTO> resultado = ejecutorias.getRelVotoEjecutoria(id);
            return resultado;
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
            return epocasSalas.ToArray();
        }
        /**
         * Obtiene las partes correspondientes a los valores seleccionados
         * en el panel de búsqueda.
         * @param busqueda El contenido del resultado de seleccion en el panel
         *                 de búsqueda.
         * @return El entero de la parte que se seleccionó.
         */
        private int obtenPartesInt(bool[][] busqueda)
        {
            int partes = 0;
            int parte = 0;
            int ancho = 0;
            int largo = 0;
            int recorridoAncho = 0;
            int recorridoLargo = 0;
            int contador = 0;
            ancho = busqueda[0].Length;
            largo = busqueda.Length;
            for (recorridoAncho = 0;
                 recorridoAncho < ancho;
                 recorridoAncho++)
            {
                for (recorridoLargo = 0;
                     recorridoLargo < largo;
                     recorridoLargo++)
                {
                    contador++;
                    if (busqueda[recorridoLargo][recorridoAncho])
                    {
                        parte = contador;
                    }
                }
            }
            partes = parte - 1;
            return partes;
        }

    }
}