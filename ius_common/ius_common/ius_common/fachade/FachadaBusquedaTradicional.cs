using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mx.gob.scjn.ius_common.TO;
using mx.gob.scjn.ius_common.ius;
using Common.Logging;
using System.Data;
using mx.gob.scjn.ius_common.utils;
using mx.gob.scjn.electoral_common.electoral;

namespace mx.gob.scjn.ius_common.fachade
{
    /// <summary>
    /// Define los puntos de acceso a la lógica de negocios.
    /// </summary>
    public class FachadaBusquedaTradicional
    {
        private readonly ILog log = LogManager.GetLogger("mx.gob.scjn.ius_common.FachadaBusquedaTradicional");
        
        public bool VerificaUsuario(String usuario, String Password)
        {
            GuardarExpresion guardar = new GuardarExpresion();
            return guardar.VerificaUsuario(usuario, Password);
        }
        ///
        /// <summary>
        /// Obtiene todas las ejecutorias.
        /// </summary>
        /// <returns> La lista de Ejecutorias.</returns>
        public List<EjecutoriasTO> getAllEjecutorias()
        {
            log.Info("Entrando a mx.gob.scjn.iuscommon.fachade.FachadaBusquedaTradicional");
            Ejecutorias ejecutorias = new Ejecutorias();
            log.Info("Ejecutorias iniciadas");
            return ejecutorias.getAll();
        }

        /// <summary>
        /// Obtiene todas las observaciones relacionadas con una tesis.
        /// </summary>
        /// <param name="ius">El identificador de la tesis de la que se buscan las
        ///        observaciones.</param>
        /// <returns>La lista de observaciones.</returns>
        public List<OtrosTextosTO> getOtrosTextosPorIus(String ius)
        {
            log.Info("Entrando a mx.gob.scjn.iuscommon.fachade.FachadaBusquedaTradicional");
            Tesis tesis = new Tesis();
            log.Info("Ejecutorias iniciadas");
            return tesis.getOtrosTextosPorIus(ius);
        }

        /// <summary>
        /// Obtiene todas las observaciones relacionadas con una tesis.
        /// </summary>
        /// <param name="ius">El identificador de la tesis 
        /// de la que se buscan las observaciones.</param>
        /// <returns>La lista de observaciones.</returns>
        public List<OtrosTextosTO> getNotasContradiccionPorIus(String ius)
        {
            log.Info("Entrando a getNotasContradiccionPorIus");
            Tesis tesis = new Tesis();
            log.Info("Ejecutorias iniciadas");
            return tesis.getNotasContradiccionesPorIus(ius);
        }

        /// <summary>
        /// Obtiene una ejecutoria de acuerdo a su Id.
        /// </summary>
        /// <param name="id">El identificador de la ejecutoria</param>
        /// <returns>La ejecutoria correspondiente al Id</returns>
        public EjecutoriasTO getEjecutoriaPorId(int id)
        {
            Ejecutorias ejecutoria = new Ejecutorias();
            return ejecutoria.getEjecutoria(id);
        }
        /// <summary>
        /// obtiene un acuerdo de acuerdo a su Id.
        /// </summary>
        /// <param name="id">El id del Acuerdo</param>
        /// <returns>El Acuerdo correspondiente al Id</returns>
        public AcuerdosTO getAcuerdoPorId(int id)
        {
            Acuerdos acuerdo = new Acuerdos();
            return acuerdo.getAcuerdo(id);
        }

        /// <summary>
        /// Obtiene el documento de una ley.
        /// </summary>
        /// <param name="idLey">El identificador de la ley</param>
        /// <param name="idArticulo">El identificador del Artículo</param>
        /// <param name="idRef">La referencia del Artículo</param>
        /// <returns>el documento llenado con los datos de una ley</returns>
        public DocumentoLeyTO getDocumentoLey(int idLey, int idArticulo, int idRef,int TipoLey)
        {
            log.Info("Entrando a la obtencion del documento de ley");
            Ley ley = new Ley();
            return ley.getDocumentoLey(idLey, idArticulo, idRef, TipoLey);
        }


        /// <summary>
        /// Obtiene un listado con todas las tesis en la BD.
        /// </summary>
        /// <returns>la lista de las tesis.</returns>
        public Dictionary<long, TesisTO> getAllTesis()
        {
            log.Info("Entrando a mx.gob.scjn.iuscommon.fachade.FachadaBusquedaTradicional");
            Tesis tesis = new Tesis();
            log.Info("Tesis iniciadas");
            return tesis.getAll();
        }

        /// <summary>
        /// Obtiene un listado de las relaciones de ligas que hay con una tesis con otras tesis.
        /// </summary>
        /// <param name="ius">El número de Ius del  que se buscan las relaciones</param>
        /// <param name="idRel">El identificador de relaciones</param>
        /// <returns>el listado de las relaciones que tienen vínculo a una tesis.</returns>
        public List<RelacionFraseTesisTO> getRelacionesFraseTesis(int ius, int idRel)
        {
            log.Debug("Entrando a getRelacionesFraseTesis (Integer, Integer)");
            Tesis tesis = new Tesis();
            return tesis.getRelacionesFraseTesis(ius, idRel);
        }
        /// <summary>
        /// Obtiene un listado de las relaciones de ligas que hay con una tesis con la legislación federal.
        /// </summary>
        /// <returns> el listado de las relaciones que tienen vínculo a una tesis.</returns>
        public List<RelacionFraseArticulosTO> getRelacionesFraseArticulos(int ius, int idRel, int tipo)
        {
            log.Debug("Entrando a getRelaciones (Integer, Integer)");
            Tesis tesis = new Tesis();
            return tesis.getRelacionesFraseArticulos(ius, idRel, tipo);
        }
        /// <summary>
        /// Obtiene un listado de las relaciones de ligas que hay con una tesis.
        /// </summary>
        /// <returns> el listado de las relaciones que tienen vínculo a una tesis.</returns>
        public List<RelacionTO> getRelaciones(int ius, int seccion)
        {
            log.Debug("Entrando a getRelaciones (Integer, Integer)");
            Tesis tesis = new Tesis();
            return tesis.getRelacionesIUSSeccion(ius, seccion);
        }

        /// <summary>
        /// Regresa todos los datos relacionados con el documento.
        ///</summary>
        public TesisTO getDocumento(int ius)
        {
            log.Info("Entrando a la búsqueda del documento " + ius);
            Tesis tesis = new Tesis();
            log.Info("Buscando la tesis");
            return tesis.getTesisPorIus(ius);
        }
        ///<summary>
        /// Toma una lista de Enteros y devuelve las tesis que tienen dichos IUS.
        /// </summary>
        /// <param name="ids"> los identificadores de los IUS a buscar.</param>
        /// <returns> la lista correspondiente a dichos ids.</returns>
        public List<TesisTO> /*DataTable*/ getTesisPorIus(MostrarPorIusTO ids)
        {
            Tesis tesis = new Tesis();
            return tesis.getTesis(ids);
        }
        ///<summary>
        /// Toma una lista de Enteros y devuelve las ejecutorias 
        /// que tienen dichos Ids.
        /// </summary>
        /// <param name="ids"> los identificadores de las ejecutorias a buscar.</param>
        /// <returns> la lista correspondiente a dichos ids.</returns>
        public List<EjecutoriasTO> getEjecutoriasPorIds(MostrarPorIusTO ids)
        {
            Ejecutorias ejecutorias = new Ejecutorias();
            return ejecutorias.getEjecutorias(ids);
        }
        /// <summary>
        /// Toma los parámetros y hace una búsqueda por palabra.
        /// </summary>
        /// <param name="palabras"> Palabras que se buscarán dentro de la tesis.</param>
        /// <param name="orderBy"> columna por la cual se va a ordenar.</param>
        /// <param name="orderType"> ascendente o descendente.</param>
        /// <returns> La lista de tesis que cumplen con la búsqueda.</returns>
        public List<TesisTO> getTesisPorPalabra(String palabra, String tipo,
                String orderBy, String orderType)
        {
            Tesis tesis = new Tesis();
            return tesis.getTesisPorPalabra(palabra, tipo, orderBy, orderType);
        }
        /// <summary>
        /// Toma los parámetros y hace una búsqueda por palabra.</summary>
        /// <param name="palabras"> Palabras que se buscarán dentro de la ejecutoria.</param>
        /// <param name="orderBy"> columna por la cual se va a ordenar.</param>
        /// <param name="orderType"> ascendente o descendente.</param>
        /// <returns> La lista de ejecutorias que cumplen con la búsqueda.</returns>
        public List<EjecutoriasTO> getEjecutoriasPorPalabra(String palabra,
                String orderBy, String orderType)
        {
            Ejecutorias ejecutoria = new Ejecutorias();
            return ejecutoria.getEjecutoriasPorPalabra(palabra, orderBy, orderType);
        }
        /// <summary>
        /// Toma los parámetros seleccionados en el panel y decide quien debe hacer la búsqueda
        /// y reenvía la petición al objeto adecuado.
        /// </summary>
        /// <param name="busqueda"> los parámetros de la busqueda del panel </param>
        /// <returns> La lista de resultados dentro de un objeto tipo ResultadosPanelTO</returns>
        public ResultadosPanelTO getConsultaPanel(BusquedaTO busqueda)
        {
            Tesis tesis;
            Acuerdos acuerdos;
            Ejecutorias ejecutorias;
            List<TesisTO> resultadoTesis;
            List<EjecutoriasTO> resultadoEjecutoria;
            List<AcuerdosTO> resultadoAcuerdos;
            ResultadosPanelTO resultado;
            resultado = new ResultadosPanelTO();
            if (busqueda.getTipoBusqueda() == IUSConstants.BUSQUEDA_TESIS_SIMPLE)
            {
                tesis = new Tesis();
                resultadoTesis = tesis.getTesisPorParte(busqueda);
                resultado.setTesis(resultadoTesis);
            }
            else if (busqueda.getTipoBusqueda() == IUSConstants.BUSQUEDA_ACUERDO)
            {
                acuerdos = new Acuerdos();
                resultadoAcuerdos = acuerdos.getPanel(busqueda);
                resultado.setAcuerdos(resultadoAcuerdos);
            }
            else if (busqueda.getTipoBusqueda() == IUSConstants.BUSQUEDA_EJECUTORIAS)
            {
                ejecutorias = new Ejecutorias();
                resultadoEjecutoria = ejecutorias.getEjecutorias(busqueda);
                resultado.setEjecutorias(resultadoEjecutoria);
            }
            return resultado;
        }
        /// <summary>
        /// Toma los parámetros seleccionados en el panel y decide quien debe hacer la búsqueda
        /// y reenvía la petición al objeto adecuado.
        /// </summary>
        /// <param name="busqueda"> los parámetros de la busqueda del panel </param>
        /// <returns> La lista de ids de los resultados dentro de un objeto 
        /// tipo ResultadosPanelTO</returns>
        public ResultadosPanelTO getIdConsultaPanel(BusquedaTO busqueda)
        {
            Tesis tesis;
            Acuerdos acuerdos;
            Ejecutorias ejecutorias;
            Votos votos;
            List<TesisTO> resultadoTesis;
            List<EjecutoriasTO> resultadoEjecutoria;
            List<AcuerdosTO> resultadoAcuerdos;
            List<VotosTO> resultadoVotos;
            ResultadosPanelTO resultado;
            resultado = new ResultadosPanelTO();
            if ((busqueda.getTipoBusqueda() == IUSConstants.BUSQUEDA_TESIS_SIMPLE) ||
                (busqueda.TipoBusqueda == IUSConstants.BUSQUEDA_TESIS_TEMATICA))
            {
                tesis = new Tesis();
                resultadoTesis = tesis.getIdTesisPorParte(busqueda);
                resultado.setTesis(resultadoTesis);
            }
            else if (busqueda.getTipoBusqueda() == IUSConstants.BUSQUEDA_ACUERDO)
            {
                acuerdos = new Acuerdos();
                resultadoAcuerdos = acuerdos.getPanel(busqueda);
                resultado.setAcuerdos(resultadoAcuerdos);
            }
            else if (busqueda.getTipoBusqueda() == IUSConstants.BUSQUEDA_EJECUTORIAS)
            {
                ejecutorias = new Ejecutorias();
                resultadoEjecutoria = ejecutorias.getEjecutorias(busqueda);
                resultado.setEjecutorias(resultadoEjecutoria);
            }
            else if (busqueda.getTipoBusqueda() == IUSConstants.BUSQUEDA_VOTOS)
            {
                votos = new Votos();
                resultadoVotos = votos.getPanel(busqueda);
                resultado.setVotos(resultadoVotos);
            }
            return resultado;
        }
        /// <summary>
        /// Busca las tesis de forma que pueda tenerse un paginador para
        /// controlarlas
        /// </summary>
        /// <param name="busqueda">La búsqueda a realizar</param>
        /// <returns>El paginador de las tesis</returns>
        public PaginadorTO getIdTesisConsultaPanel(BusquedaTO busqueda)
        {
            Tesis tesis;
            PaginadorTO resultado;
            tesis = new Tesis();
            resultado = tesis.getIdTesisPorPartePaginador(busqueda);
            return resultado;
        }

        /// <summary>
        /// Busca las tesis de forma que pueda tenerse un paginador para
        /// controlarlas
        /// </summary>
        /// <param name="busqueda">La búsqueda a realizar</param>
        /// <returns>El paginador de las tesis</returns>
        public PaginadorTO getIdTesisConsultaElectoralPanel(BusquedaTO busqueda)
        {
            TesisElectoral tesis;
            PaginadorTO resultado;
            tesis = new TesisElectoral();
            resultado = tesis.getIdTesisPorPartePaginador(busqueda);
            return resultado;
        }


        public PaginadorTO getIdTesisEspeciales(MostrarPorIusTO busqueda)
        {
            Tesis tesis;
            PaginadorTO resultado;
            tesis = new Tesis();
            resultado = tesis.getTesisPaginador(busqueda);
            return resultado;
        }

        public void BorraPaginador(Int32 Id)
        {
            Tesis tesis = new Tesis();
            tesis.BorraPaginador(Id);
        }
        public List<TesisTO> getTesisPaginadas(int IdPaginador, int PosicionPaginador)
        {
            Tesis tesis = new Tesis();
            return tesis.getTesisPaginadas(IdPaginador, PosicionPaginador);
        }

        public String getActualizadoA()
        {
            Magistrados magis = new Magistrados();
            return magis.getActualizadoA();
        }

        public List<TesisTO> getTesisPaginadasParaDespliegue(List<int> Ids)
        {
            Tesis tesis = new Tesis();
            return tesis.getTesisPaginadasParaDespliegue(Ids);
        }
        /// <summary>
        /// Obtiene las partes de una ejecutoria con un Id determinado.
        /// </summary>
        /// <param name="id"> el identificador de la ejecutoria</param>
        /// <returns> la lista de las partes de la ejecutoria.</returns>
        public List<EjecutoriasPartesTO> getPartesPorId(int id)
        {
            Ejecutorias ejecutoria = new Ejecutorias();
            return ejecutoria.getPartesEjecutoria(id, "parte", "asc");
        }
        /// <summary>
        /// Obtiene las partes de un acuerdo con un Id determinado.
        /// </summary>
        /// <param name="id"> el identificador del acuerdo</param>
        /// <returns> la lista de las partes del acuerdo.</returns>
        public List<AcuerdosPartesTO> getAcuerdoPartesPorId(int id)
        {
            Acuerdos acuerdo = new Acuerdos();
            return acuerdo.getPartesAcuerdos(id, "parte", "asc");
        }
        /// <summary>
        /// Obtiene la genealogía que coincide con 
        /// el identificador proporcionado.
        /// <param name=" id el identificador para la búsqueda.
        /// <returns> El objeto que representa la genealogía.
        public GenealogiaTO getGenalogia(String id)
        {
            Tesis tesis = new Tesis();
            return tesis.getGenealogia(id);
        }
        /// <summary>
        /// Obtiene los acuerdos que tienen una palabra determinada.
        ///</summary>
        public List<AcuerdosTO> getAcuerdosPorPalabra(BusquedaTO parametros)
        {
            Acuerdos acuerdo = new Acuerdos();
            return acuerdo.getAcuerdosPorPalabra(parametros);
        }
        /// <summary>
        /// Obtiene la lista de aquellos acuerdos que tienen un determinado
        /// id contenido en la lista de enteros del objeto, así como el orden 
        /// y tipo del ordenamiento del mismo. 
        /// <param name="parametros"> los ids, el tipo de ordenamiento y la columna a ordenar.</param>
        /// <returns> la lista de los acuerdos que cumplen con lo establecido.</returns>
        public List<AcuerdosTO> getAcuerdosPorIds(MostrarPorIusTO parametros)
        {
            Acuerdos acuerdos = new Acuerdos();
            return acuerdos.getAcuerdos(parametros);
        }
        /// <summary>
        /// Obtiene los votos que tienen una palabra determinada.
        /// </summary>
        public List<VotosTO> getVotosPorPalabra(String palabra,
            String orderBy, String orderType)
        {
            Votos voto = new Votos();
            return voto.getVotosPorPalabra(palabra, orderBy, orderType);
        }
        /// <summary>
        /// Obtiene la lista de aquellos votos que tienen un determinado
        /// id contenido en la lista de enteros del objeto, así como el orden 
        /// y tipo del ordenamiento del mismo. 
        /// </summary>
        /// <param name="parametros"> los ids, el tipo de ordenamiento y la columna a ordenar.</param>
        /// <returns> la lista de los votos que cumplen con lo establecido.</returns>
        public List<VotosTO> getVotosPorIds(MostrarPorIusTO parametros)
        {
            Votos voto = new Votos();
            return voto.getVotos(parametros);
        }
        /// <summary>
        /// obtiene un voto de acuerdo a su Id.
        /// </summary>
        /// <param name="id"></param>
        public VotosTO getVotosPorId(int id)
        {
            Votos votos = new Votos();
            return votos.getVotos(id);
        }
        /// <summary>
        /// Obtiene las partes de un voto con un Id determinado.
        /// </summary>
        /// <param name="id"> el identificador del voto</param>
        /// <returns> la lista de las partes del voto.</returns>
        public List<VotosPartesTO> getVotosPartesPorId(int id)
        {
            Votos voto = new Votos();
            return voto.getPartesAcuerdos(id, "parte", "asc");
        }
        /// <summary>
        /// Obtiene una tesis de un IUS específico.
        /// </summary>
        /// <param name="id"> el ius que se busca</param>
        /// <returns> la Tesis que contiene dicho IUS</returns>
        public TesisTO getTesisPorIus(String id)
        {
            Tesis tesis = new Tesis();
            return tesis.getTesisPorIus(id);
        }
        /// <summary>
        /// Obtiene una tesis de un IUS específico, si este ya fue eliminado
        /// regresa el IUS por el que se eliminó.
        /// </summary>
        /// <param name="id"> el ius que se busca.</param>
        /// <returns> la Tesis que contiene dicho IUS.</returns>
        public TesisTO getTesisPorRegistro(String id)
        {
            Tesis tesis = new Tesis();
            return tesis.getTesisPorRegistro(Int32.Parse(id));
        }
        public TesisTO getTesisPorRegistroLiga(String id)
        {
            Tesis tesis = new Tesis();
            return tesis.getTesisPorIus(id);
        }
        /// <summary>
        /// Obtiene una tesis de un IUS específico, si este ya fue eliminado
        /// regresa el IUS por el que se eliminó, para listarla
        /// </summary>
        /// <param name="id"> el ius que se busca.</param>
        /// <returns> la Tesis que contiene dicho IUS.</returns>
        public TesisTO getTesisPorRegistroParaLista(String id)
        {
            Tesis tesis = new Tesis();
            return tesis.getTesisPorRegistroParaLista(Int32.Parse(id));
        }
        /// <summary>
        /// Obtiene una tesis de un ConsecIndx específico.
        /// </summary>
        /// <param name="id"> el ius que se busca.</param>
        /// <returns> la Tesis que contiene dicho IUS.</returns>
        public TesisTO getTesisPorConsecIndx(String id)
        {
            Tesis tesis = new Tesis();
            return tesis.getTesisPorConsecIndx(Int32.Parse(id));
        }
        /// <summary>
        /// Regresa las relaciones de una tesis con un voto determinado.
        /// </summary>
        /// <param name="ius">el ius sobre el que se buscan los votos.</param>
        /// <returns> la lista de los votos relacionados.</returns>
        public List<RelDocumentoTesisTO> getVotosTesis(String ius)
        {
            Tesis tesis = new Tesis();
            return tesis.getRelVotos(ius);
        }
        /// <summary>
        /// Regresa las relaciones de una tesis con ejecutorias determinado.
        /// </summary>
        /// <param name="ius"> el ius sobre el que se buscan las ejecutorias.</param>
        /// <returns> la lista de las ejecutorias relacionados.</returns>
        public List<RelDocumentoTesisTO> getEjecutoriaTesis(String ius)
        {
            Tesis tesis = new Tesis();
            return tesis.getRelEjecutorias(ius);
        }
        /// <summary>
        /// Regresa las relaciones de una ejecutoria con las tesis.
        /// </summary>
        /// <param name="id"> el id sobre el que se buscan las tesis.</param>
        /// <returns> la lista de las tesis relacionadas.</returns>
        public List<RelDocumentoTesisTO> getTesisPorEjecutoria(String id)
        {
            Ejecutorias ejecutoria = new Ejecutorias();
            return ejecutoria.getRelTesis(id);
        }
        /// <summary>
        /// Regresa las relaciones de un voto con las tesis.
        /// </summary>
        /// <param name="id">el id sobre el que se buscan las tesis.</param>
        /// <returns> la lista de las tesis relacionadas.</returns>
        public List<RelDocumentoTesisTO> getTesisPorVoto(String id)
        {
            Votos voto = new Votos();
            return voto.getRelTesis(id);
        }
        /// <summary>
        /// Regresa las relaciones de un voto con las ejecutorias.
        /// <param name=" id el id del voto.
        /// <returns> la lista de las ejecutorias relacionadas.
        public List<RelVotoEjecutoriaTO> getEjecutoriaPorVoto(String id)
        {
            Votos voto = new Votos();
            return voto.getRelVotoEjecutoria(id);
        }
        /// <summary>
        /// Regresa las relaciones de una ejecutoria con los votos.
        /// <param name=" id el id de la ejecutoria.
        /// <returns> la lista de los votos relacionados.
        public List<RelVotoEjecutoriaTO> getVotoPorEjecutoria(String id)
        {
            Ejecutorias ejecutoria = new Ejecutorias();
            return ejecutoria.getRelVotoEjecutoria(id);
        }
        /// <summary>
        /// Obtiene una cadena con el listado de las materias que 
        /// tiene una tesis determinada.
        /// <param name="ius">el IUS de la tesis de la que se buscarán las materias.</param>
        /// <returns> Las materias a las que pertenece una tesis determinada.</returns>
        public String getMateriasTesis(String ius)
        {
            Tesis tesis = new Tesis();
            return tesis.getMaterias(ius);
        }
        /// <summary>
        /// Obtiene las tablas relacionadas a una ejecutoria.
        /// </summary>
        /// <param name="id">El identificador de la ejecutoria de 
        ///           la cual se requieren las tablas.</param>
        /// <returns> La lista con las tablas pertenecientes a la ejecutoria.</returns>
        public List<TablaPartesTO> getTablaEjecutorias(int id)
        {
            Ejecutorias ejecutoria = new Ejecutorias();
            return ejecutoria.getTablas(id);
        }
        /// <summary>
        /// Obtiene los nodos de los árboles para la presentación.
        /// </summary>
        /// <param name="padre"> El padre de los nodos que se buscarán.</param>
        /// <returns> La lista con los nodos del arbol cuyos hijos corresponden
        ///         a la búsqueda solicitada.</returns>
        public List<TreeNodeDataTO> getIndicesNodosHijos(String padre)
        {
            Tesis tesis = new Tesis();
            return tesis.getNodos(Int32.Parse(padre));
        }
        /// <summary>
        /// Obtiene una lista de tesis del índice solicitado.
        /// </summary>
        /// <param name="idBusqueda"> es el identificador del índice seleccionado.</param>
        /// <returns> Las tesis que deben estar en el índice seleccionado.</returns>
        public List<TesisTO> getTesisPorIndices(String idBusqueda, String ordenar)
        {
            Tesis tesis = new Tesis();
            return tesis.getTesisPorIndices(idBusqueda, ordenar);
        }

        /// <summary>
        /// Obtiene una lista de tesis del índice solicitado.
        /// </summary>
        /// <param name="idBusqueda"> es el identificador del índice seleccionado.</param>
        /// <returns> El paginador con las tesis que deben estar en el índice seleccionado.</returns>
        public PaginadorTO getTesisPorIndicesPaginador(String idBusqueda, String ordenar)
        {
            Tesis tesis = new Tesis();
            return tesis.getTesisPorIndicesPaginador(idBusqueda, ordenar);
        }

        /// <summary>
        /// Obtiene una lista de tesis del índice solicitado con los filtros requeridos.
        /// </summary>
        /// <param name="parametros">los parametros de la búsqueda a realizar.</param>
        /// <returns> Las tesis que deben estar en el índice seleccionado.</returns>
        public List<TesisTO> getTesisPorIndices(MostrarPorIusTO parametros)
        {
            Tesis tesis = new Tesis();
            return tesis.getTesisPorIndices(parametros);
        }
        /// <summary>
        /// Obtiene la lista de fechas en una sala/epoca determinada.
        /// </summary>
        /// <param name="parametro">Los datos conocidos del
        /// magistrado que se busca</param>
        /// <returns>La lista de las Fechas y periodos que hubieron por 
        /// época de magistrado.</returns>
        public List<EpocaMagistradoTO> getFechasMagistrados(EpocaMagistradoTO parametro)
        {
            Magistrados magis = new Magistrados();
            return magis.getFechasMagistrados(parametro);
        }

        /// <summary>
        /// Obtiene la lista de Funcionarios de acuerdo a un id de fecha.
        /// </summary>
        /// <param name="id">El identificador del funcionario buscado.</param>
        /// <returns>La lista de los funcionarios que 
        /// coinciden con ese identificador.</returns>
        /// 
        public List<FuncionariosTO> getFuncionarios(String id)
        {
            Magistrados magis = new Magistrados();
            return magis.getFuncionarios(id);
        }


        public List<TreeNodeDataTO> getRaizTemática(string id)
        {
            Tesis tesis = new Tesis();
            return tesis.getRaizTemática(id);
        }

        public List<ClassificacionTO> getClasificacion()
        {
            Votos votos = new Votos();
            return votos.getClasificacion();
        }

        public List<TreeNodeDataTO> getSubtemas(String tabla)
        {
            Tesis tesis = new Tesis();
            return tesis.getSubtemas(tabla);
        }
        public List<TreeNodeDataTO> getSinonimos(String tabla, String Id)
        {
            Tesis tesis = new Tesis();
            return tesis.getSinonimos(tabla, Id);
        }

        public TreeNodeDataTO getIdConstitucional(string id)
        {
            Tesis tesis = new Tesis();
            return tesis.getIdConstitucional(id);
        }
        public TreeNodeDataTO getAscendenteConstitucional(string id)
        {
            Tesis tesis = new Tesis();
            return tesis.getAscendenteConstitucional(id);
        }

        public List<TreeNodeDataTO> getSinonimoConstitucional(string Id)
        {
            Tesis tesis = new Tesis();
            return tesis.getSinonimoConstitucional(Id);
        }

        public List<TreeNodeDataTO> getProximidadConstitucional(string Id)
        {
            Tesis tesis = new Tesis();
            return tesis.getProximidadConstitucional(Id);
        }

        public UsuarioTO ObtenDatosUsuario(string usuario)
        {
            GuardarExpresion guardar = new GuardarExpresion();
            return guardar.ObtenDatosUsuario(usuario);
        }

        public List<BusquedaAlmacenadaTO> getBusquedasAlmacenadas(string usuario)
        {
            GuardarExpresion guardar = new GuardarExpresion();
            return guardar.getBusquedasAlmacenadas(usuario);
        }

        public int RegistrarUsuario(UsuarioTO usuario)
        {
            GuardarExpresion guardar = new GuardarExpresion();
            return guardar.RegistrarUsuario(usuario);
        }
        public int RegistrarBusqueda(BusquedaAlmacenadaTO busqueda, String usuario)
        {
            GuardarExpresion guardar = new GuardarExpresion();
            return guardar.RegistrarBusqueda(busqueda, usuario);
        }
        public int EliminarBusqueda(BusquedaAlmacenadaTO busqueda)
        {
            GuardarExpresion guardar = new GuardarExpresion();
            return guardar.EliminarBusqueda(busqueda);
        }
        public List<TesisTO> getTesis(BusquedaAlmacenadaTO busqueda)
        {
            Tesis tesis = new Tesis();
            return tesis.getTesis(busqueda);
        }
        public List<PonenteTO> getCatalogoPonente()
        {
            Tesis tesis = new Tesis();
            return tesis.getCatalogoPonente();
        }
        public List<AsuntoTO> getCatalogoAsunto()
        {
            Tesis tesis = new Tesis();
            return tesis.getCatalogoAsunto();
        }

        public List<TreeNodeDataTO> getSubtemasPalabra(string tabla, string busqueda)
        {
      
            Tesis tesis = new Tesis();
            return tesis.getSubtemas(tabla,busqueda);
        }

        /// <summary>
        /// Obtiene las tablas relacionadas a un Acuerdo.
        /// </summary>
        /// <param name="id">El identificador del acuerdo de 
        ///           la cual se requieren las tablas.</param>
        /// <returns> La lista con las tablas pertenecientes al acuerdo.</returns>
        public List<TablaPartesTO> getTablaAcuerdos(int id)
        {
            Acuerdos acuerdos = new Acuerdos();
            return acuerdos.getTablas(id);
        }

        public List<TablaPartesTO> getTablaVoto(int id)
        {
            Votos votos = new Votos();
            return votos.getTablas(id);
        }
        #region Directorio
        public List<DirectorioPersonasTO> getDirPersonas(String id)
        {
            DirectorioPersonas DirP = new DirectorioPersonas();
            return DirP.getDirPersonas(id);
        }

        public List<DirectorioOrgJurTO> getDirOrganosJur(String id)
        {
            DirectorioOrganosJur DirP = new DirectorioOrganosJur();
            return DirP.getDirOrganosJur(id);
        }

        public List<DirectorioOrgJurTO> getDirOrganosJurXFiltro(String Filtro, int nOrd, int nMat, int nCto, int Region)
        {
            DirectorioOrganosJur DirP = new DirectorioOrganosJur();
            return DirP.getDirOrganosJurXFiltro(Filtro, nOrd, nMat, nCto, Region);
        }

        public List<DirectorioOrgJurTO> getDirOrganosJurXId(int nIdOrgJud)
        {
            DirectorioOrganosJur DirP = new DirectorioOrganosJur();
            return DirP.getDirOrganosJurXId(nIdOrgJud);
        }

        public List<DirectorioOrgJurTO> getDirOfCorrespondencia()
        {
            DirectorioOrganosJur DirP = new DirectorioOrganosJur();
            return DirP.getDirOfCorrespondencia();
        }

            


        public List<DirectorioOrgJurTO> getDirOrganosJurXIdTitular(int nIdOrgTitular)
        {
            DirectorioOrganosJur DirP = new DirectorioOrganosJur();
            return DirP.getDirOrganosJurXIdTitular(nIdOrgTitular);
        }
        public List<DirectorioOrgJurTO> getDirComisiones()
        {
            DirectorioOrganosJur DirP = new DirectorioOrganosJur();
            return DirP.getDirComisiones();
        }

        public List<DirectorioOrgJurTO> getDirAreasAdmin()
        {
            DirectorioOrganosJur DirP = new DirectorioOrganosJur();
            return DirP.getDirAreasAdmin();
        }

        public List<DirectorioOrgJurTO> getDirAreasAdminCJF(int nTipo)
        {
            DirectorioOrganosJur DirP = new DirectorioOrganosJur();
            return DirP.getDirAreasAdminCJF(nTipo);
        }

        public List<DirectorioCatalogosTO> getDirCatalogo(int idnTipoCatalogo)
        {
            DirectorioCatalogos DirP = new DirectorioCatalogos();
            return DirP.getDirCatalogo(idnTipoCatalogo);
        }

        public List<DirectorioCatalogosTO> getDirCatalogoXTipo(int idnTipoCatalogo, int TpoOJ)
        {
            DirectorioCatalogos DirP = new DirectorioCatalogos();
            return DirP.getDirCatalogoXTipo(idnTipoCatalogo, TpoOJ);
        }


        public List<DirectorioPersonasTO> getDirPersonasFuncAdmin(String Filtro)
        {
            DirectorioPersonas DirP = new DirectorioPersonas();
            return DirP.getDirPersonasFuncAdmin(Filtro);
        }

        public List<DirectorioPersonasTO> getDirPersonasFuncAdminFiltro(string Filtro, Boolean bInicio, String strCadena)
        {
            DirectorioPersonas DirP = new DirectorioPersonas();
            return DirP.getDirPersonasFuncAdminFiltro(Filtro, bInicio, strCadena);
        }

         public List<DirectorioPersonasTO> getDirPersonasConQuery(string strQuery, int nTipoPersona)
        {
            DirectorioPersonas DirP = new DirectorioPersonas();
            return DirP.getDirPersonasConQuery(strQuery, nTipoPersona);
        }

       public List<DirectorioPersonasTO> getDirJuecesMag(string Filtro, Boolean bInicio, String strCadena)
        {
            DirectorioPersonas DirP = new DirectorioPersonas();
            return DirP.getDirJuecesMag(Filtro, bInicio, strCadena);
        }

        public List<DirectorioPersonasTO> getDirPersonasXPuestoYSala(String id, String Sala)
        {
            DirectorioPersonas DirP = new DirectorioPersonas();
            return DirP.getDirPersonasXPuestoYSala(id, Sala);
        }

        public List<DirectorioPersonasTO> getDirPersonasXPuesto(String id)
        {
            DirectorioPersonas DirP = new DirectorioPersonas();
            return DirP.getDirPersonasXPuesto(id);
        }

        public List<DirectorioPersonasTO> getDirTitularesXOJ(string Filtro)
        {
            DirectorioPersonas DirP = new DirectorioPersonas();
            return DirP.getDirTitularesXOJ(Filtro);
        }

        public List<DirectorioPersonasTO> getDirConsejerosIntComisiones(int IdCom)
        {
            DirectorioPersonas DirP = new DirectorioPersonas();
            return DirP.getDirConsejerosIntComisiones(IdCom);
        }

        public List<DirectorioPersonasTO> getDirSTCPComision(int IdCom)
        {
            DirectorioPersonas DirP = new DirectorioPersonas();
            return DirP.getDirSTCPComision(IdCom);
        }

        public List<DirectorioPersonasTO> getDirCJFIntPonencias(int idConsejero)
        {
            DirectorioPersonas DirP = new DirectorioPersonas();
            return DirP.getDirCJFIntPonencias(idConsejero);
        }

        public List<DirectorioPersonasTO> getDirTodasLasPersonas()
        {
            DirectorioPersonas DirP = new DirectorioPersonas();
            return DirP.getDirTodasLasPersonas();
        }

        public List<DirectorioMinistrosTO> getDirTodosLosMinistros()
        {
            DirectorioMinistros DirP = new DirectorioMinistros();
            return DirP.getDirTodosLosMinistros();
        }

        public List<DirectorioMinistrosTO> getDirMinistrosXFiltro(int Filtro)
        {
            DirectorioMinistros DirP = new DirectorioMinistros();
            return DirP.getDirMinistrosXFiltro(Filtro);
        }
        public List<DirectorioMinistrosTO> getDirConsejerosXFiltro(String Filtro)
        {
            DirectorioMinistros DirP = new DirectorioMinistros();
            return DirP.getDirConsejerosXFiltro(Filtro);
        }

        public List<DirectorioMinistrosTO> getDirMagistradosXFiltro(String Filtro)
        {
            DirectorioMinistros DirP = new DirectorioMinistros();
            return DirP.getDirMagistradosXFiltro(Filtro);
        }

        public List<DirectorioMinistrosTO> getDirMinistro(String Id)
        {
            DirectorioMinistros DirP = new DirectorioMinistros();
            return DirP.getDirMinistro(Id);
        }

        # endregion Directorio


        public List<string> TraeNombresAreas(int Tipo)
        {
            throw new NotImplementedException();
        }

        public List<TreeNodeDataTO> getRaizTematicaConstitucional(string id, string Busqueda)
        {
            Tesis tesis = new Tesis();
            return tesis.getRaizTemática(id, Busqueda);
        }

        public List<string> getArchivosLeyes(ArticulosTO articulo)
        {
            Ley ley = new Ley();
            return ley.getArchivosLeyes(articulo);
        }

        public TreeNodeDataTO getSubtema(string tabla, string valor)
        {
            Tesis tesis = new Tesis();
            return tesis.getSubtema(tabla, valor);
        }

        public TesisTO getTesisPorIUS(string ius)
        {
            Tesis tesis = new Tesis();
            return tesis.getTesisPorIus(Int32.Parse(ius));
        }

        public void RecuperaUsuario(string correo)
        {
            GuardarExpresion guardar = new GuardarExpresion();
            guardar.RecuperaUsuario(correo);
        }
        //public PaginadorTO getTesisPorBusqueda(BusquedaTO busqueda)
        //{
        //    Tesis tesis = new Tesis();
        //    return tesis.getTesis(busqueda);
        //}

        public void Close()
        {
            //Solo por compatibilidad de fachadas
        }

        public List<TesisTO> getTesisElePaginadas(int IdPaginador, int PosicionPaginador)
        {
            TesisElectoral ele = new TesisElectoral();
            return ele.getTesisPaginadas(IdPaginador, PosicionPaginador);
        }

        public void Abort()
        {
            //Para compatibilidad al momento de estar en la fachada Thread
        }

        public TesisTO getTesisPorRegistroParaListaElectoral(string Ius)
        {
            TesisElectoral tesis = new TesisElectoral();
            return tesis.getTesisPorRegistroParaLista(Int32.Parse(Ius));
        }

        public TesisTO getTesisElectoralPorRegistroLiga(string Ius)
        {
            TesisElectoral tesis = new TesisElectoral();
            return tesis.getTesisPorRegistroLiga(Ius);
        }

        public List<RelDocumentoTesisTO> getVotosTesisElectoral(string Ius)
        {
            TesisElectoral tesis = new TesisElectoral();
            return tesis.getVotosTesis(Ius);
        }

        public List<RelDocumentoTesisTO> getEjecutoriaTesisElectoral(string Ius)
        {
            TesisElectoral tesis = new TesisElectoral();
            return tesis.getEjecutoriaTesis(Ius);
        }

        public List<OtrosTextosTO> getNotasContradiccionPorIusElectoral(string Ius)
        {
            TesisElectoral tesis = new TesisElectoral();
            return tesis.getNotasContradiccionPorIus(Ius);
        }

        public String getMateriasTesisElectoral(string Ius)
        {
            TesisElectoral tesis = new TesisElectoral();
            return tesis.getMateriasTesis(Ius);
        }

        public List<OtrosTextosTO> getOtrosTextosPorIusElectoral(string Ius)
        {
            TesisElectoral tesis = new TesisElectoral();
            return tesis.getOtrosTextosPorIus(Ius);
        }

        public TesisTO getTesisElectoralPorRegistro(string id)
        {
            TesisElectoral tesis = new TesisElectoral();
            return tesis.getTesisPorRegistro(Int32.Parse(id));
        }

        public List<TesisTO> getTesisElectoralPorIus(MostrarPorIusTO envio)
        {
            TesisElectoral tesis = new TesisElectoral();
            return tesis.getTesisPorIus(envio);
        }

        public List<EjecutoriasTO> getEjecutoriasElectoralPorIds(MostrarPorIusTO parametros)
        {
            EjecutoriasElectoral ejecutoria = new EjecutoriasElectoral();
            return ejecutoria.getEjecutoriaPorIds(parametros);
        }

        public List<VotosTO> getVotosElectoralPorIds(MostrarPorIusTO parametros)
        {
            VotosElectoral voto = new VotosElectoral();
            return voto.getVotos(parametros);
        }

        public GenealogiaTO getGenalogiaElectoral(string p)
        {
            throw new NotImplementedException();
        }

        public AcuerdosTO getAcuerdoElectoralPorId(int p)
        {
            AcuerdosElectoral Acuerdo = new AcuerdosElectoral();
            return Acuerdo.getAcuerdoPorId(p);
        }

        public VotosTO getVotosElectoralPorId(int id)
        {
            VotosElectoral voto = new VotosElectoral();
            return voto.getVotos(id);
        }

        public EjecutoriasTO getEjecutoriaElectoralPorId(int id)
        {
            EjecutoriasElectoral ejecutoria = new EjecutoriasElectoral();
            return ejecutoria.getEjecutoria(id);
        }

        public ResultadosPanelTO getIdConsultaPanelElectoral(BusquedaTO busqueda)
        {
            AcuerdosElectoral acuerdos;
            EjecutoriasElectoral ejecutorias;
            VotosElectoral votos;
            List<EjecutoriasTO> resultadoEjecutoria;
            List<AcuerdosTO> resultadoAcuerdos;
            List<VotosTO> resultadoVotos;
            ResultadosPanelTO resultado;
            resultado = new ResultadosPanelTO();
            if ((busqueda.getTipoBusqueda() == IUSConstants.BUSQUEDA_ACUERDO) ||
                (busqueda.getTipoBusqueda() == IUSConstants.BUSQUEDA_OTROS))
            {
                acuerdos = new AcuerdosElectoral();
                resultadoAcuerdos = acuerdos.getPanel(busqueda);
                resultado.setAcuerdos(resultadoAcuerdos);
            }
            else if (busqueda.getTipoBusqueda() == IUSConstants.BUSQUEDA_EJECUTORIAS)
            {
                ejecutorias = new EjecutoriasElectoral();
                resultadoEjecutoria = ejecutorias.getEjecutorias(busqueda);
                resultado.setEjecutorias(resultadoEjecutoria);
            }
            else if (busqueda.getTipoBusqueda() == IUSConstants.BUSQUEDA_VOTOS)
            {
                votos = new VotosElectoral();
                resultadoVotos = votos.getPanel(busqueda);
                resultado.setVotos(resultadoVotos);
            }
            return resultado;
        }

        public List<EjecutoriasPartesTO> getPartesElectoralPorId(int p)
        {
            EjecutoriasElectoral ejecutoria = new EjecutoriasElectoral();
            return ejecutoria.getPartesEjecutoria(p, "parte", "asc");
        }

        public List<RelDocumentoTesisTO> getTesisPorEjecutoriaElectoral(string Id)
        {
            EjecutoriasElectoral ejecutoria = new EjecutoriasElectoral();
            return ejecutoria.getTesis(Id);
        }

        public List<TablaPartesTO> getTablaEjecutoriasElectoral(int Id)
        {
            EjecutoriasElectoral ejecutoria = new EjecutoriasElectoral();
            return ejecutoria.getTablas(Id);
        }

        public List<RelVotoEjecutoriaTO> getEjecutoriaElectoralPorVoto(string Id)
        {
            VotosElectoral votos = new VotosElectoral();
            return votos.getEjecutorias(Id);
        }

        public List<RelDocumentoTesisTO> getTesisElectoralPorVoto(string Id)
        {
            VotosElectoral votos = new VotosElectoral();
            return votos.getTesis(Id);
        }

        public List<VotosPartesTO> getVotosPartesElectoralPorId(int id)
        {
            VotosElectoral voto = new VotosElectoral();
            return voto.getPartesAcuerdos(id, "parte", "asc");
        }

        public List<AcuerdosTO> getAcuerdosElectoralPorIds(MostrarPorIusTO parametros)
        {
            AcuerdosElectoral acuerdo = new AcuerdosElectoral();
            return acuerdo.getAcuerdoPorId(parametros);
        }

        public List<AcuerdosPartesTO> getAcuerdoPartesElectoralPorId(int id)
        {
            AcuerdosElectoral acuerdo = new AcuerdosElectoral();
            return acuerdo.getPartesAcuerdos(id, "parte", "asc");
        }

        public List<TreeNodeDataTO> getTemasRelacionados(string Id)
        {
            Tesis tesis = new Tesis();
            return tesis.getTemasRelacionados(Id);
        }

        public List<TipoPonenteTO> getCatalogoTipoPonente()
        {
            Tesis tesis = new Tesis();
            return tesis.getCatalogoTipoPonente();
        }

        public List<CategoriaDocTO> GetCategorias()
        {
            Tesis tesis = new Tesis();
            return tesis.GetCategorias();
        }

        public List<DocumentoTO> GetDocumento(int Categoria)
        {
            Tesis tesis = new Tesis();
            return tesis.GetDocumento(Categoria);
        }

        public List<ArticulosTO> getArchivosLeyPorIUS(long tesis)
        {
            Ley ley = new Ley();
            return ley.getArchivosLeyPorIUS(tesis);
        }

        public TesisTO getTesisElectoralPorIUS(string loc)
        {
            TesisElectoral tesis = new TesisElectoral();
            return tesis.getTesisPorLoc(loc);
        }

        public TesisTO getTesisPaginadaPorPosicion(int paginador, int posicion)
        {
            Tesis tesis = new Tesis();
            return tesis.getTesisPaginadaPorPosicion(paginador, posicion);
        }

        public List<TomoTO> getTomosPrimerNivel()
        {
            Tesis tesis = new Tesis();
            return tesis.getTomosPrimerNivel();
        }

        public List<SeccionTO> getTomosSecciones(int p)
        {
            Tesis tesis = new Tesis();
            return tesis.getTomosSecciones(p);
        }

        public List<InformesTO> getInformes()
        {
            Informes info = new Informes();
            return info.getInformes();
        }
    }
}
