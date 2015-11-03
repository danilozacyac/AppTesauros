using mx.gob.scjn.ius_common.TO;
using System.Collections.Generic;
using System;
using System.Data;

namespace mx.gob.scjn.ius_common.DAO
{
    /// <summary>
    /// Define los accesos que se tendrán para obtener los datos de
    /// diferentes consultas relacionadas con las tesis.
    /// </summary>
    /// <remarks name="author"> Carlos de Luna.</remarks>
    public interface TesisDAO
    {
        /// <summary>
        /// Obtiene todas las tesis existentes.
        /// </summary>
        /// <returns> La lista con todas las tesis.</returns>
        Dictionary<long, TesisTO> getAll();
        /// <summary>
        /// Obtiene las tesis que cumplen con un determinado patrón de
        /// Busqueda dado por un conjunto de épocas.</summary>
        /// <param name="busqueda"> Los parámetros de la búsqueda</param>
        /// <returns> La lista de las tesis que coinciden con los 
        ///         criterios seleccionados</returns>
        List<TesisTO> getTesis(EpocasTO busqueda);
        /// <summary>
        /// Obtiene las tesis que coinciden con los criterios
        /// de las partes buscadas.</summary>
        /// <param name="parte"> El criterio de la Búsqueda</param>
        /// <returns> La lista de las tesis que coinciden con los 
        ///         criterios seleccionados</returns>
        ///
        List<TesisTO> getTesis(PartesTO parte);
        PaginadorTO getTesisPaginador(PartesTO parte, List<int> tribunales, Dictionary<int, int[]> secciones);
        /// <summary>
        /// Obtiene una lista de identificadores de tesis que coinciden con
        /// las partes seleccionadas.
        /// </summary>
        /// <param name="parte"> las partes seleccionadas.</param>
        /// <returns> Las tesis que coinciden con los criterios de búsqueda.</returns>
        ///
        List<TesisTO> getIdTesis(PartesTO parte);
        /// <summary>
        /// Obtiene una serie de partes filtrada de una manera en específico.
        /// </summary>
        /// <param name="parte"> Los parámetros de búsqueda.</param>
        /// <returns> La lista de coincidencias con la búsqueda.</returns>
        ///
        List<TesisTO> getTesisConFiltro(PartesTO parte);
        /// <summary>
        /// Obtiene las tesis que tienen jurisprudencia dentro de una parte determinada.
        /// </summary>
        /// <param name="parte"> los parámetros de búsqueda.</param>
        /// <returns> La lista de coincidencias de la búsqueda</returns>
        ////
        List<TesisTO> getTesisJurisprudencia(PartesTO parte);
        ///
        /// Obtiene una lista de tesis filtrada de una manera en específico,
        /// obteniendo todos los datos y no solo el IUS.
        /// @param parte Los datos de la búsqueda.
        /// @return Las coincidencias de la Búsqueda.
        ////
        /*List<TesisTO>*/ DataTableReader getTesisConFiltro(MostrarPorIusTO parte);
        ///////
        /// Obtiene una lista de tesis que coinciden con la 
        /// característica de ser jurisprudencia.
        /// @param parte Los parámetros de búsqueda.
        /// @return La lista de resultados coincidentes.
        ////
        /*List<TesisTO>*/ DataTableReader getTesisJurisprudencia(MostrarPorIusTO parte);
        ///////
        /// Obtiene una tesis en específico dada por un IUS.
        /// @param ius El IUS a buscar
        /// @return La tesis que contiene dicho IUS
        ////
        TesisTO getTesisPorIus(int ius);
        /// <summary>
        /// Obtiene una tesis para ser listada.
        /// </summary>
        /// <param name="ius">El ius de la tesis</param>
        /// <returns>La tesis que tiene dicho ius</returns>
        TesisTO getTesisPorIusParaLista(int ius);
        ///////
        /// Obtiene las relaciones que tiene la tesis.
        /// @param ius El IUS que busca las relaciones.
        /// @return La lista de relaciones que tiene la tesis.
        ////
        List<RelacionTO> getRelaciones(int ius);
        ///////
        /// Obtiene la lista que tiene una sección en específico de una
        /// tesis
        /// @param IUSSeccion Los parámetros de búsqueda
        /// @return La lista de las relaciones.
        ////
        List<RelacionTO> getRelacionesIUSSeccion(RelacionTO IUSSeccion);
        ///////
        /// La relación con las frases existentes en una tesis.
        /// @param idRelIus El identificador de Relacion del IUS
        /// @return Los datos de la relación.
        ////
        List<RelacionFraseTesisTO> getRelacionFraseTesis(RelacionFraseTesisTO idRelIus);
        ///////
        /// La relación con las frases existentes en una tesis con algún artículo.
        /// @param idRelIus El identificador de relación buscado
        /// @return La relación buscada.
        ////
        List<RelacionFraseArticulosTO> getRelacionFraseArticulos(RelacionFraseArticulosTO idRelIus);
        ///////
        /// Obtiene las tesis que pertenecen a la lista de enteros dentro
        /// del objeto identificadores.
        /// @param identificadores el objeto con los parámetros de búsqueda.
        /// @return la Lista de coincidencias.
        ////
        /*List<TesisTO>*/DataTableReader getTesis(MostrarPorIusTO identificadores);
        ///////
        /// Obtiene las identificaciones de las tesis que 
        /// pertenecen a la lista de enteros dentro
        /// del objeto identificadores.
        /// @param identificadores el objeto con los parámetros de búsqueda.
        /// @return la Lista de coincidencias.
        ////
        List<TesisTO> getIusTesis(MostrarPorIusTO identificadores);
        ///////
        /// Encuentra una tesis que contenga un determinado patrón dentro de su texto. 
        /// @param palabras El patrón de búsqueda de palabras.
        /// @return La lista de coincidencias.
        ////
        List<TesisTO> getIusPorPalabra(BusquedaTO palabras);
        ///////
        /// Obtiene las genealogías de la tesis.
        /// @param id el id de la genealogía
        /// @return La genealogía de la tesis.
        ////
        GenealogiaTO getGenealogia(String id);
        ///////
        /// Obtiene las observaciones a una tesis.
        /// @param ius El IUS de la tesis
        /// @return Las observaciones hechas.
        ////
        List<OtrosTextosTO> getOtrosTextosPorIus(String ius);
        ///////
        /// Obtiene las contradicciones por las que fue
        /// superada una tesis.
        /// @param ius El IUS de la tesis
        /// @return Las contradicciones hechas.
        ////
        List<OtrosTextosTO> getNotasContradiccionesPorIus(String ius);
        ///////
        /// Obtiene las ejecutorias relacionadas con la tesis.
        /// @param ius el identificador de la tesis.
        /// @return La lista con las ejecutorias que se obtienen de la tesis.
        ////
        List<RelDocumentoTesisTO> getRelEjecutorias(String ius);
        ///////
        /// Obtiene los votos relacionadas con la tesis.
        /// @param ius el identificador de la tesis.
        /// @return La lista con los votos que se obtienen de la tesis.
        ////
        List<RelDocumentoTesisTO> getRelVotos(String ius);
        ///////
        /// Obtiene la lista de materias (ya descritas y no su id) de una tesis relacionada.
        /// @param ius El identificador de la tesis a buscar
        ////
        List<String> getMaterias(String ius);
        ///////
        /// Obtiene una tesis tomando en cuenta que el IUS que se envia es el número 
        /// de una tesis eliminada.
        ////
        TesisTO getTesisEliminada(int  ius);
        /// <summary>
        ///     Obtiene el IUS de la tesis referenciada en caso de que
        ///     exista como tal.
        /// </summary>
        /// <param name="ius" type="int">
        ///     <para>
        ///         El número de IUS que se quiere verificar como vivo.
        ///     </para>
        /// </param>
        /// <returns>
        ///     Un objeto de teis que solamente contiene el número de IUS válido.
        /// </returns>
        TesisTO getTesisReferenciadas(int ius);
        ///////
        /// Obtiene una lista de tesis filtrada de una manera en específico,
        /// obteniendo todos los datos y no solo el IUS. Trae las tesis desde
        /// las busquedas especiales.
        /// @param parte Los datos de la búsqueda.
        /// @return Las coincidencias de la Búsqueda.
        ////
        /*List<TesisTO>*/ DataTableReader getTesisEspecialConFiltro(MostrarPorIusTO parte);
        ///////
        /// Obtiene una lista de tesis que coinciden con la 
        /// característica de ser jurisprudencia. Trae las tesis desde
        /// las busquedas especiales.
        /// @param parte Los parámetros de búsqueda.
        /// @return La lista de resultados coincidentes.
        ////
        /*List<TesisTO>*/ DataTableReader getTesisEspecialJurisprudencia(MostrarPorIusTO parte);
        ///////
        /// Obtiene las tesis que pertenecen a la lista de enteros dentro
        /// del objeto identificadores. Trae las tesis desde
        /// las busquedas especiales.
        /// @param identificadores el objeto con los parámetros de búsqueda.
        /// @return la Lista de coincidencias.
        ////
        /*List<TesisTO>*/ DataTableReader getTesisEspecial(MostrarPorIusTO identificadores);
        ///////
        /// Obtiene las tesis que pertenecen a la lista de enteros dentro
        /// del objeto identificadores. Trae las tesis desde
        /// las busquedas especiales.
        /// @param identificadores el objeto con los parámetros de búsqueda.
        /// @return El paginador que contiene la Lista de coincidencias.
        ////
        PaginadorTO getTesisEspecialPaginador(MostrarPorIusTO identificadores);
        PaginadorTO getTesisPorMateriasPaginador(MostrarPorIusTO parametros);
        PaginadorTO getTesisPorInstanciasPaginador(MostrarPorIusTO parametros);
        PaginadorTO getTesisPorTribunalPaginador(MostrarPorIusTO parametros);
        ///////
        /// Obtiene las tesis que pertenecen a una instancia determinada
        /// de acuerdo a los parámetros de búsqueda por Indice.
        /// @param parametros Los parametros de la búsqueda
        /// @return La lista con las tesis que cumplen con los parámetros de búsqueda.
        ////
        List<TesisTO> getTesisPorInstancias(MostrarPorIusTO parametros);
        ///////
        /// Obtiene una lista de tesis del índice solicitado con los filtros requeridos.
        /// @param parametros los parametros de la búsqueda a realizar.
        /// @return Las tesis que deben estar en el índice seleccionado.
        ////
        List<TesisTO> getIndicesJurisprudencia(MostrarPorIusTO parametros);
        ///////
        /// Obtiene una lista de tesis del índice solicitado con los filtros requeridos.
        /// @param parametros los parametros de la búsqueda a realizar.
        /// @return Las tesis que deben estar en el índice seleccionado.
        ////
        List<TesisTO> getIndicesConFiltro(MostrarPorIusTO parametros);
        ///////
        /// Obtiene las tesis que pertenecen a una materia determinada
        /// de acuerdo a los parámetros de búsqueda por Indice.
        /// @param parametros Los parametros de la búsqueda
        /// @return La lista con las tesis que cumplen con los parámetros de búsqueda.
        ////
        List<TesisTO> getTesisPorMaterias(MostrarPorIusTO parametros);
        ///////
        /// Obtiene una lista de tesis del índice de materias solicitado con los filtros requeridos.
        /// @param parametros los parametros de la búsqueda a realizar.
        /// @return Las tesis que deben estar en el índice de la materia seleccionado.
        ////
        List<TesisTO> getMateriasJurisprudencia(MostrarPorIusTO parametros);
        ///////
        /// Obtiene una lista de tesis del índice de materias solicitado con los filtros requeridos.
        /// @param parametros los parametros de la búsqueda a realizar.
        /// @return Las tesis que deben estar en el índice de la materia seleccionado.
        ////
        List<TesisTO> getMateriasConFiltro(MostrarPorIusTO parametros);
        ///////
        /// Obtiene las tesis que pertenecen a un tribunal colegiado determinada
        /// de acuerdo a los parámetros de búsqueda por Indice.
        /// @param parametros Los parametros de la búsqueda
        /// @return La lista con las tesis que cumplen con los parámetros de búsqueda.
        ////
        List<TesisTO> getTesisPorTribunal(MostrarPorIusTO parametros);
        ///////
        /// Obtiene una lista de tesis del tribunal colegiado solicitado con los filtros requeridos.
        /// @param parametros los parametros de la búsqueda a realizar.
        /// @return Las tesis que deben estar en el tribunal colegiado seleccionado.
        ////
        List<TesisTO> getTribunalJurisprudencia(MostrarPorIusTO parametros);
        ///////
        /// Obtiene una lista de tesis del índice de materias solicitado con los filtros requeridos.
        /// @param parametros los parametros de la búsqueda a realizar.
        /// @return Las tesis que deben estar en el índice de la materia seleccionado.
        ////
        List<TesisTO> getTribunalConFiltro(MostrarPorIusTO parametros);
        /// <summary>
        /// Obtiene los nodos de la tabla raiz para la consulta temática
        /// </summary>
        /// <param name="id">El identificador padre</param>
        /// <returns>La lista de los nodos hijos</returns>
        List<RaizTO> getRaizTematica(string id);
        /// <summary>
        /// Obtiene los subtemas seleccionados de acuerdo al identificador del padre
        /// y la tabla a la que pertenecen.
        /// </summary>
        /// <param name="tabla">La tabla de la que se obtienen los datos</param>
        /// <returns>La lista de datos que corresponden al criterio de búsqueda.</returns>
        List<RaizTO> getSubtemas( String tabla);
        ///<summary>
        ///Obtiene una lista de tesis relacionadas con un subtema o sinónimo
        ///determinado.
        ///</summary>
        ///<param name="tabla">
        ///La tabla donde se deberá buscar.
        ///</param>
        ///<param name="valor">
        ///El valor del subtema elegido, el identificador.
        ///</param>
        ///<returns>La lista de las tesis pertenecientes al subtema o sinónimo</returns>
        List<TesisTO> getTesisSubtemas(String tabla, String valor);
        /// <summary>
        /// obtiene una tesis que corresponde a un Consecutivo en específico.
        /// </summary>
        /// <param name="ConsecIndx">El consecutivo a buscar</param>
        /// <returns>La tesis correspondiente</returns>
        TesisTO getTesisPorConsecIndx(int ConsecIndx);
        /// <summary>
        /// Encuentra los sinónimos de una tabla dada con un ID determinado.
        /// </summary>
        /// <param name="tabla">la tabla donde se buscará el sinónimo</param>
        /// <param name="ID">El identificador para el sinónimo</param>
        /// <returns>La lista de Sinónimos</returns>
        List<RaizTO> getSinonimos(string tabla, String ID);
        /// <summary>
        /// Obtiene la lista de Temas en el tesauro constitucional.
        /// </summary>
        /// <param name="newId">Identificador del tema</param>
        /// <returns>La lista de temas del tesauro constitucional</returns>
        List<RaizTO> getRaizConstitucional(string newId);
        /// <summary>
        /// Obtiene la lista de Temas en el tesauro constitucional.
        /// </summary>
        /// <param name="newId">Identificador del tema</param>
        /// <returns>La lista de temas del tesauro constitucional</returns>
        List<RaizTO> getRaizConstitucional(string newId, String busqueda);
        /// <summary>
        /// obtiene la lista de sinónimos del temático.
        /// </summary>
        /// <param name="tabla">La tabla de la cual se obtendrán los sinónimos</param>
        /// <param name="identificador">El identificador del tema</param>
        /// <returns>La lista de sinónimos existentes</returns>
        List<TesisTO> getTesisSinonimos(string tabla, string identificador);
        /// <summary>
        /// Obtiene Los datos de un tema en el tesauro constitucional.
        /// </summary>
        /// <param name="id">El identificador del tema</param>
        /// <returns>Los datos del tema</returns>
        RaizTO getIdConstitucional(string id);
        /// <summary>
        /// Obtiene los ascendentes del tesauro constitucional.
        /// </summary>
        /// <param name="id">El identificador del tesauro del que se quiere el ascendente</param>
        /// <returns>El nodo ascendente del tesauro constitucional</returns>
        RaizTO getAscendenteConstitucional(string id);
        /// <summary>
        /// Obtiene el nodo de los sinónimos constitucionales
        /// </summary>
        /// <param name="newId">El identificador del tema en el tesauro</param>
        /// <param name="p"></param>
        /// <returns>El nodo con los sinónimos</returns>
        List<RaizTO> getSinonimoConstitucional(string newId, int p);
        /// <summary>
        /// Obtiene la lista de tesis del tesauro
        /// </summary>
        /// <param name="identificador">el identificador del cual se buscan las tesis</param>
        /// <returns>La lista de las tesis del tesauro.</returns>
        List<TesisTO> getTesisTesauro(string identificador);
        /// <summary>
        /// Obtiene el catálogo de los ponentes de las tesis
        /// </summary>
        /// <returns>El catálogo completo de ponentes</returns>
        List<PonenteTO> getCatalogoPonente();
        /// <summary>
        /// Obtiene el catálogo de Asuntos.
        /// </summary>
        /// <returns>El catálogo de asuntos</returns>
        List<AsuntoTO> getCatalogoAsunto();

        List<RaizTO> getSubtemas(string tabla, string busqueda);

        TreeNodeDataTO getNodoConstitucional(string id);

        List<TreeNodeDataTO> getNodosConstitucionales(List<String> ids);

        RaizTO getSubtema(string tabla, string valor);

        List<TesisTO> getTesisSubtemasSinonimoPalabra(string tabla, string identificador);

        PaginadorTO getIusPorPalabraPaginador(BusquedaTO busqueda, Dictionary<int, int[]> secciones);

        PaginadorTO getTesisSubtemasSinonimoPalabraPaginador(string tabla, string identificador);

        List<TesisTO> getTesisPaginadas(int idConsulta, int PosicionPaginador);

        void BorraPaginador(int Id);

        List<TesisTO> getTesisPaginadasParaDespliegue(List<int> ids);

        string getIdProg(string Id);

        RaizTO getSubtemaTesis(string p, string p_2);

        List<TipoPonenteTO> getCatalogoTipoPonente();

        List<CategoriaDocTO> GetCategorias();

        List<DocumentoTO> GetDocumanto(int Categoria);

        int getVolumen(int ius);

        List<RelacionFraseArticulosTO> getRelacionFraseArticulosEst(RelacionFraseArticulosTO parametros);

        List<string> getFuentes();

        TesisTO getTesisPaginadaPorPosicion(int paginador, int posicion);

        List<TomoTO> getTomosPrimerNivel();

        List<SeccionTO> getTomos(int p);

        List<SeccionTO> getSecciones(int p);

        List<int> getNotasGenericas();
    }
}