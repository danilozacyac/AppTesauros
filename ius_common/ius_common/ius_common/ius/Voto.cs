using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Logging;
using mx.gob.scjn.ius_common.TO;
using mx.gob.scjn.ius_common.context;
using mx.gob.scjn.ius_common.DAO;

namespace mx.gob.scjn.ius_common.ius
{
    /**
     * Clase que se hace cargo de la lógica de negocios relacionada
     * con los votos.
     * @author Carlos de Luna Sáenz
     *
     */
    public class Votos
    {
        /**
         * La bitácora de salida de eventos.
         */
        private ILog log = LogManager.GetLogger("mx.gob.scjn.iuscommon.ius.Votos");
        /**
         * Contexto de spring de donde se accederá a los DAOs
         */
        private IUSApplicationContext contexto;
        /**
         * Nuestro constructor.
         */
        public Votos()
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
        /**
         * Obtiene una lista de votos con todos los que existan en la BD.
         * @return La lista de todos los votos existentes.
         */

        public List<VotosTO> getAll()
        {
            VotosDAO daoVotos = (VotosDAO)contexto.getInitialContext().GetObject("VotosDAO");
            List<VotosTO> resultado = daoVotos.getAll();
            return resultado;
        }
        /**
         * Obtiene los votos que tengan la palabra buscada.
         * @param palabra la palabra a buscar.
         * @param orderBy la columna por la cual se ordena la busqueda
         * @param orderType el tipo de ordenamiento
         * @return la lista de resultados
         */
        public List<VotosTO> getVotosPorPalabra(String palabra, String orderBy, String orderType)
        {
            //VotosDAO daoVotos = (VotosDAO)contexto.getInitialContext().GetObject("VotosDAO");
            //List <VotosTO> resultado =  daoVotos.getVotosPorPalabra(palabra, orderBy, orderType);
            //return resultado;		
            throw new NotImplementedException();
        }
        /**
         * Obtiene una lista de votos conforme a la seleccion realizada por
         * el panel.
         * @param partes las partes seleccionadas dentro del panel.
         * @return La lista de votos que coinciden con el criterio de
         *         búsqueda.
         */

        public List<VotosTO> getPanel(BusquedaTO panel)
        {
            int partes = obtenPartesInt(panel);
            PartesTO parte = new PartesTO();
            parte.setOrderBy(panel.getOrdenarPor());
            VotosDAO daoVotos = (VotosDAO)contexto.getInitialContext().GetObject("VotosDAO");
            parte.setParte(partes);
            List<VotosTO> resultado;
            /******************************************************************
                 ********           Busqueda por palabra con, probablemente  ******
                 ********           muchas selecciones en el panel, solo     ******
                 ******************************************************************/
            if (panel.Palabra != null)
            {
                int[] conjuntoPartes = obtenPartes(panel);
                return daoVotos.getVotosPorPalabra(panel);
            }
            /************Secuencial*****************/
            if (panel.Clasificacion == null)
            {
                resultado = daoVotos.getVotos(parte);
            }
            else
            {
                resultado = daoVotos.getVotos(parte, panel.Clasificacion);
            }
            return resultado;
        }

        private EpocasTO obtenPartes(bool[][] origen)
        {
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
                 recorridoAncho++)
            {
                for (recorridoLargo = 0;
                     recorridoLargo < largo;
                     recorridoLargo++)
                {
                    if (origen[recorridoLargo][recorridoAncho])
                    {
                        epocaActual = new EpocasSalasTO();
                        epocaActual.setSala(recorridoLargo + 1);
                        epocaActual.setEpoca(5 - recorridoAncho);
                        epocasSalas.Add(epocaActual);
                    }
                }
            }
            partes.setEpocasSalas(epocasSalas);
            return partes;
        }
        /**
         * Obtiene el entero de una parte dada en el panel de búsqueda.
         * @param busqueda La representación lógica del panel que se llenó para
         *                 la búsqueda
         * @return El número correspondiente a la parte
         */
        private int obtenPartesInt(BusquedaTO busqueda)
        {
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
                 recorridoAncho++)
            {
                for (recorridoLargo = 0;
                     recorridoLargo < largo;
                     recorridoLargo++)
                {
                    contador++;
                    if (busqueda.getEpocas()[recorridoLargo][recorridoAncho])
                    {
                        parte = contador;
                    }
                }
            }
            if (parte == 0)
            {
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
                            parte = contador;
                        }
                    }
                }
            }
            if (parte == 0)
            {
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
                            parte = contador;
                        }
                    }
                }
            }
            partes = parte - 1;
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
        public List<VotosTO> getVotos(MostrarPorIusTO busqueda)
        {
            VotosDAO daoVotos = (VotosDAO)contexto.getInitialContext().GetObject("VotosDAO");
            List<VotosTO> resultado = daoVotos.getVotos(busqueda);
            return resultado;
        }

        public List<VotosPartesTO> getPartesAcuerdos(int id, String orderBy, String orderType)
        {
            MostrarPartesIdTO parametros = new MostrarPartesIdTO();
            parametros.setId(id);
            parametros.setOrderBy(orderBy);
            parametros.setOrderType(orderType);
            VotosDAO daoVotos = (VotosDAO)contexto.getInitialContext().GetObject("VotosDAO");
            List<VotosPartesTO> resultado = daoVotos.getVotosPartes(parametros);
            return resultado;
        }
        public VotosTO getVotos(int id)
        {
            VotosDAO daoVotos = (VotosDAO)contexto.getInitialContext().GetObject("VotosDAO");
            VotosTO resultado = daoVotos.getVotos(id);
            return resultado;
        }
        /**
         * Obtiene todas las tesis relacionadas con un voto dado.
         * @param id el identificador del voto.
         * @return la lista con la relación de números de tesis.
         */
        public List<RelDocumentoTesisTO> getRelTesis(String id)
        {
            VotosDAO ejecutorias = (VotosDAO)contexto.getInitialContext().GetObject("VotosDAO");
            List<RelDocumentoTesisTO> resultado = ejecutorias.getRelTesis(id);
            return resultado;
        }

        /**
         * Obtiene todas las ejecutorias relacionadas con un voto dado.
         * @param id el identificador del voto.
         * @return la lista con la relación de números de ejecutoria.
         */
        public List<RelVotoEjecutoriaTO> getRelVotoEjecutoria(String id)
        {
            VotosDAO votos = (VotosDAO)contexto.getInitialContext().GetObject("VotosDAO");
            List<RelVotoEjecutoriaTO> resultado = votos.getRelVotoEjecutoria(id);
            return resultado;
        }

        public List<ClassificacionTO> getClasificacion()
        {
            VotosDAO votos = (VotosDAO)contexto.getInitialContext().GetObject("VotosDAO");
            List<ClassificacionTO> resultado = votos.getClasificacion();
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


        public List<TablaPartesTO> getTablas(int id)
        {
            VotosDAO votos = (VotosDAO)contexto.getInitialContext().GetObject("VotosDAO");
            List<TablaPartesTO> resultado = votos.getTablas(id + "");
            return resultado;
        }
    }
}
