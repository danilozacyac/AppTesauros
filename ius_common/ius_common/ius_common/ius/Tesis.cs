using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mx.gob.scjn.ius_common.DAO;
using mx.gob.scjn.ius_common.TO;
using mx.gob.scjn.ius_common.context;
using Common.Logging;
using mx.gob.scjn.ius_common.utils;
using System.Data;

namespace mx.gob.scjn.ius_common.ius
{
    /// <summary>
    /// Establece toda la lógica de negocios para las tesis
    /// </summary>
    public class Tesis
    {
        private ILog log = LogManager.GetLogger("mx.gob.scjn.iuscommon.ius.Tesis");
        private IUSApplicationContext contexto;
        private static List<int> conNotasGenericas;
        /// <summary>
        /// Inicia el objeto obteniendo los contextos necesarios.
        /// </summary>
        public Tesis()
        {
            try
            {
                this.contexto = new IUSApplicationContext();
            }
            catch (Exception e)
            {
                log.Debug("Problemas al iniciar el contexto");
                log.Debug(e.StackTrace);
            }
        }
        /// <summary>
        /// Obtiene y devuelve todas las relaciones de la tesis
        /// para que sean pintadas en la vista.
        /// </summary>
        public List<RelacionTO> getRelaciones(int ius)
        {
            TesisDAO daoTesis = (TesisDAO)contexto.getInitialContext().GetObject("TesisDAO");
            List<RelacionTO> relaciones = daoTesis.getRelaciones(ius);
            return relaciones;
        }
        /// <summary>
        /// Obtiene y devuelve todas las relaciones de la tesis
        /// para que sean pintadas en la vista.
        ///</summary>
        public List<RelacionTO> getRelacionesIUSSeccion(int ius, int seccion)
        {
            RelacionTO parametros = new RelacionTO();
            parametros.setIus("" + ius);
            parametros.setSeccion("" + seccion);
            TesisDAO daoTesis = (TesisDAO)contexto.getInitialContext().GetObject("TesisDAO");
            List<RelacionTO> relaciones = daoTesis.getRelacionesIUSSeccion(parametros);
            return relaciones;
        }

        /// <summary>
        /// Obtiene y devuelve todas las relaciones de la tesis
        /// con los articulos de Legislación Federal
        /// para que sean pintadas en la vista.
        /// </summary>
        public List<RelacionFraseArticulosTO> getRelacionesFraseArticulos(int ius, int idRel, int tipo)
        {
            RelacionFraseArticulosTO parametros = new RelacionFraseArticulosTO();
            parametros.setIus("" + ius);
            parametros.setIdRel("" + idRel);
            TesisDAO daoTesis = (TesisDAO)contexto.getInitialContext().GetObject("TesisDAO");
            List<RelacionFraseArticulosTO> relaciones = null;
            if (tipo == 0)
                relaciones = daoTesis.getRelacionFraseArticulos(parametros);
            else
                relaciones = daoTesis.getRelacionFraseArticulosEst(parametros);
            return relaciones;
        }
        /// <summary>
        /// Obtiene y devuelve todas las relaciones de la tesis
        /// con otras tesis
        /// para que sean pintadas en la vista.
        /// </summary>
        public List<RelacionFraseTesisTO> getRelacionesFraseTesis(int ius, int idRel)
        {
            RelacionFraseTesisTO parametros = new RelacionFraseTesisTO();
            parametros.setIus("" + ius);
            parametros.setIdRel("" + idRel);
            TesisDAO daoTesis = (TesisDAO)contexto.getInitialContext().GetObject("TesisDAO");
            List<RelacionFraseTesisTO> relaciones = daoTesis.getRelacionFraseTesis(parametros);
            return relaciones;
        }
        /// <summary>
        /// Obtiene un documento específico por IUS
        /// </summary>
        /// <param name="ius"> El número de IUS del documento a buscar</param>
        /// <returns> El documento buscado</returns>
        ///
        public TesisTO getTesisPorIus(int ius)
        {
            TesisDAO daoTesis = (TesisDAO)contexto.getInitialContext().GetObject("TesisDAO");
            TesisTO tesis = daoTesis.getTesisPorIus(ius);
            if (conNotasGenericas == null)
            {
                conNotasGenericas = daoTesis.getNotasGenericas();
            }
            if (conNotasGenericas.Contains(Int32.Parse(tesis.Ius)))
            {
                tesis.ExistenNG = true;
            }
            else
            {
                tesis.ExistenNG = false;
            }
            return tesis;
        }
        /// <summary>
        /// Obtiene un documento específico por IUS, si esta eliminada regresa la tesis
        /// por la que se reemplazó
        /// </summary>
        /// <param name="ius"> El número de IUS del documento a buscar</param>
        /// <returns> El documento buscado, si este es null es que nunca ha existido.</returns>
        public TesisTO getTesisPorRegistro(int ius)
        {
            TesisDAO daoTesis = (TesisDAO)contexto.getInitialContext().GetObject("TesisDAO");
            TesisTO tesisInicial = daoTesis.getTesisPorIus(ius);
            if((tesisInicial!=null)&&(tesisInicial.Parte!=null)&&!tesisInicial.Parte.Equals("99")) return tesisInicial;
            TesisTO tesis = daoTesis.getTesisEliminada(ius);
            if ((tesis == null) || (tesis.Ius == null))
            {
                tesis = daoTesis.getTesisReferenciadas(ius);
                if ((tesis == null) || (tesis.Ius == null))
                {
                    tesis = daoTesis.getTesisPorIus(ius);
                }
                else
                {
                    tesis = daoTesis.getTesisPorIus(Int32.Parse(tesis.Ius));
                }
            }
            if (conNotasGenericas == null)
            {
                conNotasGenericas = daoTesis.getNotasGenericas();
            }
            if (tesis.Ius == null)
            {
                tesis.ExistenNG = false;
            }
            else if (conNotasGenericas.Contains(Int32.Parse(tesis.Ius)))
            {
                tesis.ExistenNG = true;
            }
            else
            {
                tesis.ExistenNG = false;
            }
            return tesis;
        }
        /// <summary>
        /// Obtiene un documento específico por IUS, para una lista
        /// </summary>
        /// <param name="ius"> El número de IUS del documento a buscar</param>
        /// <returns> El documento buscado, si este es null es que nunca ha existido.</returns>
        public TesisTO getTesisPorRegistroParaLista(int ius)
        {
            TesisDAO daoTesis = (TesisDAO)contexto.getInitialContext().GetObject("TesisDAO");
            TesisTO tesis =daoTesis.getTesisPorIusParaLista(ius);
            return tesis;
        }
        /// <summary>
        /// Obtiene un documento específico por IUS, si esta eliminada regresa la tesis
        /// por la que se reemplazó
        /// </summary>
        /// <param name="ius"> El número de IUS del documento a buscar</param>
        /// <returns> El documento buscado, si este es null es que nunca ha existido.</returns>
        public TesisTO getTesisPorConsecIndx(int ConsecIndx)
        {
            TesisDAO daoTesis = (TesisDAO)contexto.getInitialContext().GetObject("TesisDAO");
            TesisTO tesis = daoTesis.getTesisPorConsecIndx(ConsecIndx);
            return tesis;
        }
        /// <summary>
        /// Obtiene todas las tesis.
        /// </summary>
        /// <returns> Una lista con todas las tesis.</returns>
        public Dictionary<long,TesisTO> getAll()
        {
            TesisDAO daoTesis = (TesisDAO)contexto.GetObject("TesisDAO");
            Dictionary<long,TesisTO> resultado = daoTesis.getAll();
            return resultado;
        }

        /// <summary>
        /// Obtiene una lista de las tesis que cumplen con una serie de partes
        /// de acuerdo a el orden de los páneles de búsqueda.
        /// </summary>
        /// <param name="busqueda">Un objeto con los arreglos que contienen las 
        ///        selecciones del usuario para la búsqueda.</param>
        /// <returns>La lista de todas las tesis que cumplen con los
        ///         parámetros establecidos.</returns>
        ///         
        public List<TesisTO> getTesis(BusquedaTO busqueda)
        {
            EpocasTO partes = new EpocasTO();//obtenPartes(busqueda);
            TesisDAO daoTesis = (TesisDAO)contexto.getInitialContext().GetObject("TesisDAO");
            List<TesisTO> resultado = daoTesis.getTesis(partes);
            return resultado;
        }

        /// <summary>
        /// Obtiene una lista de las tesis que cumplen con una serie de 
        /// identificadores.
        /// </summary>
        /// <param name="busqueda"> Un objeto con los arreglos que contienen los ids
        ///                   para la búsqueda.</param>
        /// <returns> La lista de todas las tesis que cumplen con los
        ///         parámetros establecidos.</returns>
        public List<TesisTO> getTesis(MostrarPorIusTO busqueda)
        {
            TesisDAO daoTesis = (TesisDAO)contexto.getInitialContext().GetObject("TesisDAO");
            DataTableReader resultado;
            if (busqueda.getBusquedaEspecialValor() == null)
            {
                if (busqueda.getFilterBy() != null)
                {
                    if (busqueda.getFilterBy().Equals(IUSConstants.FILTRADO_POR_TESIS_AISLADAS_CONSTANTE))
                    {
                        busqueda.setFilterBy(IUSConstants.FILTRADO_POR_TESIS_AISLADAS_COLUMNA);
                        busqueda.setFilterValue(IUSConstants.FILTRADO_POR_TESIS_AISLADAS_VALOR);
                    }
                    else if (busqueda.getFilterBy().Equals(IUSConstants.FILTRADO_POR_CONTRADICCION_CONSTANTE))
                    {
                        busqueda.setFilterBy(IUSConstants.FILTRADO_POR_CONTRADICCION_COLUMNA);
                        busqueda.setFilterValue(IUSConstants.FILTRADO_POR_CONTRADICCION_VALOR);
                    }
                    else if (busqueda.getFilterBy().Equals(IUSConstants.FILTRADO_POR_CONTROVERSIAS_CONSTANTE))
                    {
                        busqueda.setFilterBy(IUSConstants.FILTRADO_POR_CONTROVERSIAS_COLUMNA);
                        busqueda.setFilterValue(IUSConstants.FILTRADO_POR_CONTROVERSIAS_VALOR);
                    }
                    else if (busqueda.getFilterBy().Equals(IUSConstants.FILTRADO_POR_REITERACIONES_CONSTANTE))
                    {
                        busqueda.setFilterBy(IUSConstants.FILTRADO_POR_REITERACIONES_COLUMNA);
                        busqueda.setFilterValue(IUSConstants.FILTRADO_POR_REITERACIONES_VALOR);
                    }
                    else if (busqueda.getFilterBy().Equals(IUSConstants.FILTRADO_POR_ACCIONES_CONSTANTE))
                    {
                        busqueda.setFilterBy(IUSConstants.FILTRADO_POR_ACCIONES_COLUMNA);
                        busqueda.setFilterValue(IUSConstants.FILTRADO_POR_ACCIONES_VALOR);
                    }
                    if (busqueda.getFilterBy().Equals(IUSConstants.FILTRADO_JURISPRUDENCIA))
                    {
                        resultado = daoTesis.getTesisJurisprudencia(busqueda);
                    }
                    else if (busqueda.getFilterBy().Equals(IUSConstants.FILTRADO_NINGUNO))
                    {
                        resultado = daoTesis.getTesis(busqueda);
                    }
                    else
                    {
                        resultado = daoTesis.getTesisConFiltro(busqueda);
                    }
                }
                else
                {
                    resultado = daoTesis.getTesis(busqueda);
                }
            }
            else
            {
                if (busqueda.getFilterBy() != null)
                {
                    if (busqueda.getFilterBy().Equals(IUSConstants.FILTRADO_POR_TESIS_AISLADAS_CONSTANTE))
                    {
                        busqueda.setFilterBy(IUSConstants.FILTRADO_POR_TESIS_AISLADAS_COLUMNA);
                        busqueda.setFilterValue(IUSConstants.FILTRADO_POR_TESIS_AISLADAS_VALOR);
                    }
                    else if (busqueda.getFilterBy().Equals(IUSConstants.FILTRADO_POR_CONTRADICCION_CONSTANTE))
                    {
                        busqueda.setFilterBy(IUSConstants.FILTRADO_POR_CONTRADICCION_COLUMNA);
                        busqueda.setFilterValue(IUSConstants.FILTRADO_POR_CONTRADICCION_VALOR);
                    }
                    else if (busqueda.getFilterBy().Equals(IUSConstants.FILTRADO_POR_CONTROVERSIAS_CONSTANTE))
                    {
                        busqueda.setFilterBy(IUSConstants.FILTRADO_POR_CONTROVERSIAS_COLUMNA);
                        busqueda.setFilterValue(IUSConstants.FILTRADO_POR_CONTROVERSIAS_VALOR);
                    }
                    else if (busqueda.getFilterBy().Equals(IUSConstants.FILTRADO_POR_REITERACIONES_CONSTANTE))
                    {
                        busqueda.setFilterBy(IUSConstants.FILTRADO_POR_REITERACIONES_COLUMNA);
                        busqueda.setFilterValue(IUSConstants.FILTRADO_POR_REITERACIONES_VALOR);
                    }
                    else if (busqueda.getFilterBy().Equals(IUSConstants.FILTRADO_POR_ACCIONES_CONSTANTE))
                    {
                        busqueda.setFilterBy(IUSConstants.FILTRADO_POR_ACCIONES_COLUMNA);
                        busqueda.setFilterValue(IUSConstants.FILTRADO_POR_ACCIONES_VALOR);
                    }
                    if (busqueda.getFilterBy().Equals(IUSConstants.FILTRADO_JURISPRUDENCIA))
                    {
                        resultado = daoTesis.getTesisEspecialJurisprudencia(busqueda);
                    }
                    else if (busqueda.getFilterBy().Equals(IUSConstants.FILTRADO_NINGUNO))
                    {
                        resultado = daoTesis.getTesisEspecial(busqueda);
                    }
                    else
                    {
                        resultado = daoTesis.getTesisEspecialConFiltro(busqueda);
                    }
                }
                else
                {
                    resultado = daoTesis.getTesisEspecial(busqueda);
                }
            }
            List<TesisTO> lista = new List<TesisTO>();
            //foreach (DataRow fila in resultado.Rows)
            while(resultado.Read())
            {
                TesisTO tesisActual = new TesisTO();
                tesisActual.setIus("" + resultado["ius"]);
                if (busqueda.getBusquedaEspecialValor() == null)
                {
                    tesisActual.setParte("" + resultado["parte"]);
                    tesisActual.setRubro("" + resultado["rubro"]);
                    //tesisActual.setEpoca("" + fila["epoca"]);
                    //tesisActual.setSala("" + fila["sala"]);
                    tesisActual.setTesis("" + resultado["tesis"]);
                    tesisActual.setLocAbr("" + resultado["locAbr"]);
                    tesisActual.setTa_tj("" + resultado["ta_tj"]);
                    tesisActual.setImageOther("" + resultado["imageOther"]);
                }
                else
                {
                    tesisActual.setParte("");
                    tesisActual.setRubro("");
                    tesisActual.setEpoca("");
                    tesisActual.setSala("");
                    tesisActual.setTesis("");
                    tesisActual.setLocAbr("");
                    tesisActual.setTa_tj("");
                    tesisActual.setImageOther("");
                }
                tesisActual.setConsecIndx("" + resultado["consecIndx"]);
                tesisActual.OrdenInstancia = (int)resultado["OrdenInstancia"];
                tesisActual.OrdenTesis = (int)resultado["OrdenTesis"];
                tesisActual.OrdenRubro = (int)resultado["OrdenRubro"];
                tesisActual.setTpoTesis("" + resultado["tpoTesis"]);
                tesisActual.Ponentes = new int[5];
                tesisActual.Ponentes[0] = (short)resultado["tpoPonente1"];
                tesisActual.Ponentes[1] = (short)resultado["tpoPonente2"];
                tesisActual.Ponentes[2] = (short)resultado["tpoPonente3"];
                tesisActual.Ponentes[3] = (short)resultado["tpoPonente4"];
                tesisActual.Ponentes[4] = (short)resultado["tpoPonente5"];
                tesisActual.TipoPonente = new int[5];
                tesisActual.TipoPonente[0] = (short)resultado["tpoPon1"];
                tesisActual.TipoPonente[1] = (short)resultado["tpoPon2"];
                tesisActual.TipoPonente[2] = (short)resultado["tpoPon3"];
                tesisActual.TipoPonente[3] = (short)resultado["tpoPon4"];
                tesisActual.TipoPonente[4] = (short)resultado["tpoPon5"];
                tesisActual.TipoTesis = new int[5];
                tesisActual.TipoTesis[0] = (short)resultado["TpoAsunto1"];
                tesisActual.TipoTesis[1] = (short)resultado["TpoAsunto2"];
                tesisActual.TipoTesis[2] = (short)resultado["TpoAsunto3"];
                tesisActual.TipoTesis[3] = (short)resultado["TpoAsunto4"];
                tesisActual.TipoTesis[4] = (short)resultado["TpoAsunto5"];
                //tesisActual.setImageWeb("" + fila["imageWeb"]);
                //tesisActual.setDescTpoTesis("" + fila["descTpoTesis"]);
                lista.Add(tesisActual);
            }
            resultado.Close();
            //conexion.Close();
            return lista;
        }

        /// <summary>
        /// Obtiene una lista de las tesis que cumplen con una serie de 
        /// identificadores.
        /// </summary>
        /// <param name="busqueda"> Un objeto con los arreglos que contienen los ids
        ///                   para la búsqueda.</param>
        /// <returns> La lista de todas las tesis que cumplen con los
        ///         parámetros establecidos.</returns>
        public PaginadorTO getTesisPaginador(MostrarPorIusTO busqueda)
        {
            TesisDAO daoTesis = (TesisDAO)contexto.getInitialContext().GetObject("TesisDAO");
            PaginadorTO resultado;
            if (busqueda.getBusquedaEspecialValor() == null)
            {
                    return null;
            }
            else
            {
                if (busqueda.getFilterBy() != null)
                {
                    return null;
                }
                else
                {
                    resultado = daoTesis.getTesisEspecialPaginador(busqueda);
                }
            }
            
            //conexion.Close();
            return resultado;
        }
        
        /// <summary>
        /// Obtiene una lista de las tesis que cumplen con una serie de partes
        ///         de acuerdo a el orden de los páneles de búsqueda.
        /// </summary>
        /// <param name="busqueda"> Un objeto con los arreglos que contienen las 
        ///        selecciones del usuario para la búsqueda.</param>
        /// <returns>La lista de todas las tesis que cumplen con los
        ///         parámetros establecidos.</returns>
        public List<TesisTO> getTesisPorParte(BusquedaTO busqueda)
        {
            TesisDAO daoTesis = (TesisDAO)contexto.getInitialContext().GetObject("TesisDAO");
            int partes = obtenPartesInt(busqueda);
            PartesTO parte = new PartesTO();
            parte.setOrderBy(busqueda.getOrdenarPor());
            parte.setParte(partes);
            List<TesisTO> resultado;
            if (busqueda.getFiltrarPor() != null)
            {
                if (busqueda.getFiltrarPor().Equals(IUSConstants.FILTRADO_POR_TESIS_AISLADAS_CONSTANTE))
                {
                    parte.setFilterBy(IUSConstants.FILTRADO_POR_TESIS_AISLADAS_COLUMNA);
                    parte.setFilterValue(IUSConstants.FILTRADO_POR_TESIS_AISLADAS_VALOR);
                }
                else if (busqueda.getFiltrarPor().Equals(IUSConstants.FILTRADO_POR_CONTRADICCION_CONSTANTE))
                {
                    parte.setFilterBy(IUSConstants.FILTRADO_POR_CONTRADICCION_COLUMNA);
                    parte.setFilterValue(IUSConstants.FILTRADO_POR_CONTRADICCION_VALOR);
                }
                else if (busqueda.getFiltrarPor().Equals(IUSConstants.FILTRADO_POR_CONTROVERSIAS_CONSTANTE))
                {
                    parte.setFilterBy(IUSConstants.FILTRADO_POR_CONTROVERSIAS_COLUMNA);
                    parte.setFilterValue(IUSConstants.FILTRADO_POR_CONTROVERSIAS_VALOR);
                }
                else if (busqueda.getFiltrarPor().Equals(IUSConstants.FILTRADO_POR_REITERACIONES_CONSTANTE))
                {
                    parte.setFilterBy(IUSConstants.FILTRADO_POR_REITERACIONES_COLUMNA);
                    parte.setFilterValue(IUSConstants.FILTRADO_POR_REITERACIONES_VALOR);
                }
                else if (busqueda.getFiltrarPor().Equals(IUSConstants.FILTRADO_POR_ACCIONES_CONSTANTE))
                {
                    parte.setFilterBy(IUSConstants.FILTRADO_POR_ACCIONES_COLUMNA);
                    parte.setFilterValue(IUSConstants.FILTRADO_POR_ACCIONES_VALOR);
                }
                if (busqueda.getFiltrarPor().Equals(IUSConstants.FILTRADO_JURISPRUDENCIA))
                {
                    resultado = daoTesis.getTesisJurisprudencia(parte);
                }
                else if (busqueda.getFiltrarPor().Equals(IUSConstants.FILTRADO_NINGUNO))
                {
                    resultado = daoTesis.getTesis(parte);
                }
                else
                {
                    resultado = daoTesis.getTesisConFiltro(parte);
                }
            }
            else
            {
                resultado = daoTesis.getTesis(parte);
            }
            return resultado;
        }

        
        /// <summary>
        /// Obtiene una lista de los ids de tesis que cumplen con una serie de partes
        ///         de acuerdo a el orden de los páneles de búsqueda.
        ///         </summary>
        /// <param name="busqueda"> Un objeto con los arreglos que contienen las 
        ///        selecciones del usuario para la búsqueda.</param>
        /// <returns> La lista de todas las tesis que cumplen con los
        ///         parámetros establecidos.</returns>
        public List<TesisTO> getIdTesisPorParte(BusquedaTO busqueda)
        {
            TesisDAO daoTesis = (TesisDAO)contexto.getInitialContext().GetObject("TesisDAO");
            /******************************************************************
             *******           Busqueda por palabra con, probablemente  *******
             *******           muchas selecciones en el panel, solo     *******
             ******************************************************************/
            if (busqueda.Palabra != null)
            {
                int[] conjuntoPartes = obtenPartes(busqueda);
                return daoTesis.getIusPorPalabra(busqueda);
            }
            if (busqueda.TipoBusqueda == IUSConstants.BUSQUEDA_TESIS_TEMATICA)
            {
                String tabla = busqueda.Clasificacion.ElementAt(0).DescTipo;
                String identificador = ""+busqueda.Clasificacion.ElementAt(0).IdTipo;
                return daoTesis.getTesisSubtemasSinonimoPalabra(tabla, identificador);
                //if (tabla.Equals(IUSConstants.BUSQUEDA_TESIS_THESAURO))
                //{
                //    return daoTesis.getTesisTesauro(identificador);
                //}
                //else if (!tabla.ToLower().Contains("alterno"))
                //{
                //    return daoTesis.getTesisSubtemas(tabla, identificador);
                //}
                //else
                //{
                //    return daoTesis.getTesisSinonimos(tabla, identificador);
                //}
            }
            /************Secuencial*****************/
            int partes = obtenPartesInt(busqueda);
            PartesTO parte = new PartesTO();
            parte.setOrderBy(busqueda.getOrdenarPor());
            parte.setParte(partes);
            List<TesisTO> resultado;
            if (busqueda.getFiltrarPor() != null)
            {
                if (busqueda.getFiltrarPor().Equals(IUSConstants.FILTRADO_POR_TESIS_AISLADAS_CONSTANTE))
                {
                    parte.setFilterBy(IUSConstants.FILTRADO_POR_TESIS_AISLADAS_COLUMNA);
                    parte.setFilterValue(IUSConstants.FILTRADO_POR_TESIS_AISLADAS_VALOR);
                }
                else if (busqueda.getFiltrarPor().Equals(IUSConstants.FILTRADO_POR_CONTRADICCION_CONSTANTE))
                {
                    parte.setFilterBy(IUSConstants.FILTRADO_POR_CONTRADICCION_COLUMNA);
                    parte.setFilterValue(IUSConstants.FILTRADO_POR_CONTRADICCION_VALOR);
                }
                else if (busqueda.getFiltrarPor().Equals(IUSConstants.FILTRADO_POR_CONTROVERSIAS_CONSTANTE))
                {
                    parte.setFilterBy(IUSConstants.FILTRADO_POR_CONTROVERSIAS_COLUMNA);
                    parte.setFilterValue(IUSConstants.FILTRADO_POR_CONTROVERSIAS_VALOR);
                }
                else if (busqueda.getFiltrarPor().Equals(IUSConstants.FILTRADO_POR_REITERACIONES_CONSTANTE))
                {
                    parte.setFilterBy(IUSConstants.FILTRADO_POR_REITERACIONES_COLUMNA);
                    parte.setFilterValue(IUSConstants.FILTRADO_POR_REITERACIONES_VALOR);
                }
                else if (busqueda.getFiltrarPor().Equals(IUSConstants.FILTRADO_POR_ACCIONES_CONSTANTE))
                {
                    parte.setFilterBy(IUSConstants.FILTRADO_POR_ACCIONES_COLUMNA);
                    parte.setFilterValue(IUSConstants.FILTRADO_POR_ACCIONES_VALOR);
                }
                if (busqueda.getFiltrarPor().Equals(IUSConstants.FILTRADO_JURISPRUDENCIA))
                {
                    resultado = daoTesis.getTesisJurisprudencia(parte);
                }
                else if (busqueda.getFiltrarPor().Equals(IUSConstants.FILTRADO_NINGUNO))
                {
                    resultado = daoTesis.getIdTesis(parte);
                }
                else
                {
                    resultado = daoTesis.getTesisConFiltro(parte);
                }
            }
            else
            {
                resultado = daoTesis.getIdTesis(parte);
            }
            return resultado;
        }

        public PaginadorTO getIdTesisPorPartePaginador(BusquedaTO busqueda)
        {
            TesisDAO daoTesis = (TesisDAO)contexto.getInitialContext().GetObject("TesisDAO");
            Dictionary<int, int[]> secciones = new Dictionary<int, int[]>();
            Dictionary<int, int[]> seccionesParam = new Dictionary<int, int[]>();
            if (busqueda.Secciones != null)
            {
                foreach (int[] item in busqueda.Secciones)
                {
                    seccionesParam.Add(item[0], item);
                }
            }
            if (busqueda.Secciones != null)
            {
                foreach (int[] array in busqueda.Secciones)
                {
                    secciones[array[0]] = array;
                }
            }
            if (busqueda.Palabra != null)
            {
                int[] conjuntoPartes = obtenPartes(busqueda);
                return daoTesis.getIusPorPalabraPaginador(busqueda, seccionesParam);
            }
            if (busqueda.TipoBusqueda == IUSConstants.BUSQUEDA_TESIS_TEMATICA)
            {
                String tabla = busqueda.Clasificacion.ElementAt(0).DescTipo;
                String identificador = "" + busqueda.Clasificacion.ElementAt(0).IdTipo;
                return daoTesis.getTesisSubtemasSinonimoPalabraPaginador(tabla, identificador);
            }
            int partes = obtenPartesInt(busqueda);
            PartesTO parte = new PartesTO();
            parte.setOrderBy(busqueda.getOrdenarPor());
            parte.setParte(partes);
            PaginadorTO resultado;
            
            resultado = daoTesis.getTesisPaginador(parte, busqueda.Tribunales, seccionesParam);
            return resultado;
        }
        /// <summary>
        /// Obtiene una lista de las tesis que cumplen con una serie de partes
        ///         de acuerdo a el orden de los páneles de búsqueda.
        ///         </summary>
        /// <param name="busqueda"> Un objeto con los arreglos que contienen las 
        ///        selecciones del usuario para la búsqueda.</param>
        /// <param name="orderBy"> La columna por la cual se quiere ordenar la consulta.</param>
        /// <param name="orderType"> El tipo de ordenamiento que se quiere.</param>
        /// <returns> La lista de todas las tesis que cumplen con los
        ///         parámetros establecidos.</returns>
        public List<TesisTO> getTesisPorParte(BusquedaTO busqueda,
                String orderBy, String orderType)
        {
            int partes = obtenPartesInt(busqueda);
            PartesTO parte = new PartesTO();
            TesisDAO daoTesis = (TesisDAO)contexto.getInitialContext().GetObject("TesisDAO");
            parte.setParte(partes);
            parte.setOrderBy(orderBy);
            parte.setOrderType(orderType);
            List<TesisTO> resultado = daoTesis.getTesis(parte);
            return resultado;
        }
        /// <summary>
        /// Obtiene una lista de las tesis que cumplen tienen una determinada palabra
        ///        en la busqueda.
        ///        </summary>
        /// <param name="palabra"> la palabra a buscar en las tesis.</param>
        /// <param name="orderBy"> La columna por la cual se quiere ordenar la consulta.</param>
        /// <param name="orderType"> El tipo de ordenamiento que se quiere.</param>
        /// <returns> La lista de todas las tesis que cumplen con los
        ///         parámetros establecidos.</returns>
        public List<TesisTO> getTesisPorPalabra(String busqueda, String tipo,
                String orderBy, String orderType)
        {
            TesisDAO daoTesis = (TesisDAO)contexto.getInitialContext().GetObject("TesisDAO");
            BusquedaTO buscar = new BusquedaTO();
            BusquedaPalabraTO buscarPal = new BusquedaPalabraTO();
            buscarPal.Expresion = busqueda;
            if (tipo.Equals("juris"))
            {
                buscarPal.Jurisprudencia = IUSConstants.BUSQUEDA_PALABRA_JURIS;
            }
            else if (tipo.Equals("tesis"))
            {
                buscarPal.Jurisprudencia = IUSConstants.BUSQUEDA_PALABRA_TESIS;
            }
            else
            {
                buscarPal.Jurisprudencia = IUSConstants.BUSQUEDA_PALABRA_AMBAS;
            }
            buscar.Palabra = new List<BusquedaPalabraTO>();
            buscar.Palabra.Add(buscarPal);
            List<TesisTO> resultado = daoTesis.getIusPorPalabra(buscar);
            List<int> ordenamiento = new List<int>();
            foreach (TesisTO item in resultado)
            {
                ordenamiento.Add(Int32.Parse(item.getIus()));
            }
            MostrarPorIusTO parametros = new MostrarPorIusTO();
            parametros.setOrderBy(orderBy);
            parametros.setOrderType(orderType);
            parametros.setListado(ordenamiento);
            resultado = daoTesis.getIusTesis(parametros);
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

            for (recorridoLargo = 0;
                 recorridoLargo < largo;
                 recorridoLargo++)
            {
                for (recorridoAncho = 0;
                recorridoAncho < ancho;
                recorridoAncho++)
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

        /// <summary>
        /// Obtiene la parte sobre la cual se deberá realizar la búsqueda
        /// tomando como base las características de las selecciones del panel.
        /// </summary>
        /// <param name="busqueda"> Las selecciones del Panel.</param>
        /// <returns> La parte conforme a la BD.</returns>
        ///
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

            for (recorridoLargo = 0;
                 recorridoLargo < largo;
                 recorridoLargo++)
            {
                for (recorridoAncho = 0;
             recorridoAncho < ancho;
             recorridoAncho++)
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
            partes = parte - 1;
            return partes;
        }
        /// <summary>
        /// Obtiene la genealogía de un determinado Id.
        /// </summary>
        /// <param name="id"> el id de la genealogía</param>
        /// <returns> El objeto de transferencia con su id y su texto.</returns>
        public GenealogiaTO getGenealogia(String id)
        {
            TesisDAO daoTesis = (TesisDAO)contexto.getInitialContext().GetObject("TesisDAO");
            return daoTesis.getGenealogia(id);
        }
        /// <summary>
        /// Obtiene todas las observaciones que pueda tener una tesis.
        /// </summary>
        /// <param name="ius"> El ius del cual se buscan las observaciones.</param>
        /// <returns> la lista de las observaciones.</returns>
        public List<OtrosTextosTO> getOtrosTextosPorIus(String ius)
        {
            TesisDAO tesis = (TesisDAO)contexto.getInitialContext().GetObject("TesisDAO");
            return tesis.getOtrosTextosPorIus(ius);
        }
        /// <summary>
        /// Obtiene todas las contradicciones que pueda tener una tesis.
        /// </summary>
        /// <param name="ius"> El ius del cual se buscan las contradicciones.</param>
        /// <returns> la lista de las contradicciones.</returns>
        public List<OtrosTextosTO> getNotasContradiccionesPorIus(String ius)
        {
            TesisDAO tesis = (TesisDAO)contexto.getInitialContext().GetObject("TesisDAO");
            return tesis.getNotasContradiccionesPorIus(ius);
        }
        /// <summary>
        /// Obtiene de la búsqueda por indices los
        /// hijos del arbol o el nodo hoja final para 
        /// su presentación haciendo la traducción
        /// del TO de indices al de el árbol.
        /// </summary>
        public List<TreeNodeDataTO> getNodos(int padre)
        {
            IndicesDAO indices = (IndicesDAO)contexto.getInitialContext().GetObject("IndicesDAO");
            List<CIndicesTO> resultadoCIndices = null;
            List<TCCTO> resultadoTCC = null;
            Boolean leaf = false;
            List<TreeNodeDataTO> nodos = new List<TreeNodeDataTO>();
            if ((padre > 100) && (padre < 200))
            {//Tribunales Colegiados de Circuito
                leaf = true;
                resultadoTCC = indices.getTribunal(padre - 100);
                foreach (TCCTO item in resultadoTCC)
                {
                    int identificador = item.getIdTribunal() + 1000;
                    TreeNodeDataTO nodoNuevo = new TreeNodeDataTO();
                    String referencia = IUSConstants.BUSQUEDA_INDICES_TRIBUNALES + identificador;
                    nodoNuevo.setHref(referencia);
                    nodoNuevo.setId(identificador + "");
                    nodoNuevo.setLabel(item.getDescripcion());
                    referencia = IUSConstants.FRAME_DERECHA + "";
                    nodoNuevo.setTarget(referencia);
                    nodoNuevo.setIsLeaf(leaf);
                    nodos.Add(nodoNuevo);
                }

            }
            else
            {
                resultadoCIndices = indices.obtenHijos(padre);
                foreach (CIndicesTO item in resultadoCIndices)
                {
                    TreeNodeDataTO nodoNuevo = new TreeNodeDataTO();
                    if (item.getIdInd() >= 200)
                    {//SCJN, los secuenciales no sirven por que tambien hay que incluir más tesis
                        nodoNuevo.setHref(IUSConstants.BUSQUEDA_INDICES_SECUENCIAL + item.getCadenaKey());
                        nodoNuevo.setTarget(IUSConstants.FRAME_DERECHA);
                        nodoNuevo.setId(item.getCadenaKey());
                        leaf = true;
                    }
                    else if ((item.getIdInd() > 2) && (item.getIdInd() < 10))
                    {//Finales de Materia
                        nodoNuevo.setHref(IUSConstants.BUSQUEDA_INDICES_MATERIA + item.getIdInd());
                        nodoNuevo.setTarget(IUSConstants.FRAME_DERECHA);
                        nodoNuevo.setId(item.getCadenaKey());
                        leaf = true;
                    }
                    else
                    {
                        nodoNuevo.setHref("javascript:{setId(" + item.getIdInd() + ");}");
                        nodoNuevo.setTarget(null);
                        nodoNuevo.setId(item.getIdInd() + "");
                        leaf = false;
                    }
                    nodoNuevo.setLabel(item.getCadenaDesc());
                    nodoNuevo.setIsLeaf(leaf);
                    nodos.Add(nodoNuevo);
                }
            }
            return nodos;
        }

        public List<TesisTO> getTesisPaginadas(int idConsulta, int PosicionPaginador)
        {
            TesisDAO tesis = (TesisDAO)contexto.getInitialContext().GetObject("TesisDAO");
            return tesis.getTesisPaginadas(idConsulta, PosicionPaginador);
        }

        public List<TesisTO> getTesisPaginadasParaDespliegue(List<int> idsConsulta)
        {
            TesisDAO tesis = (TesisDAO)contexto.getInitialContext().GetObject("TesisDAO");
            return tesis.getTesisPaginadasParaDespliegue(idsConsulta);
        }
        public void BorraPaginador(Int32 Id)
        {
            TesisDAO tesis = (TesisDAO)contexto.getInitialContext().GetObject("TesisDAO");
            tesis.BorraPaginador(Id);
        }
        /// <summary>
        /// Regresa la tesis que coincide con el ius dado.
        /// </summary>
        /// <param name="id"> el identificador de la tesis.</param>
        /// <returns> la tesis que coincide con el ius dado.</returns>
        public TesisTO getTesisPorIus(String id)
        {
            TesisDAO daoTesis = (TesisDAO)contexto.getInitialContext().GetObject("TesisDAO");
            TesisTO tesis = daoTesis.getTesisPorIus(Int32.Parse(id));
            if (conNotasGenericas == null)
            {
                conNotasGenericas = daoTesis.getNotasGenericas();
            }
            if (conNotasGenericas.Contains(Int32.Parse(tesis.Ius)))
            {
                tesis.ExistenNG = true;
            }
            else
            {
                tesis.ExistenNG = false;
            }
            return tesis;
        }
        /// <summary>
        /// Obtiene las ejecutorias relacionadas con la tesis.
        /// </summary>
        /// <param name="ius"> el identificador de la tesis.</param>
        /// <returns> La lista con las ejecutorias que se obtienen de la tesis.</returns>
        ///
        public List<RelDocumentoTesisTO> getRelEjecutorias(String ius)
        {
            TesisDAO tesis = (TesisDAO)contexto.getInitialContext().GetObject("TesisDAO");
            return tesis.getRelEjecutorias(ius);
        }
        /// <summary>
        /// Obtiene los votos relacionadas con la tesis.
        /// </summary>
        /// <param name="ius"> el identificador de la tesis.</param>
        /// <returns> La lista con los votos que se obtienen de la tesis.</returns>
        public List<RelDocumentoTesisTO> getRelVotos(String ius)
        {
            TesisDAO tesis = (TesisDAO)contexto.getInitialContext().GetObject("TesisDAO");
            return tesis.getRelVotos(ius);
        }
        /// <summary>
        /// Obtiene los textos de los tipos de tesis.
        /// </summary>
        /// <param name="ius"> El identificador de la tesis de la cual se quieren las
        ///            materias a las que pertenece.</param>
        /// <returns> La cadena ya formada con las materias a las que pertenece
        ///         la tesis.</returns>
        ///
        public String getMaterias(String ius)
        {
            TesisDAO tesis = (TesisDAO)contexto.getInitialContext().GetObject("TesisDAO");
            List<String> materias = tesis.getMaterias(ius);
            String resultado = IUSConstants.CADENA_VACIA;
            foreach (String item in materias)
            {
                resultado += item + ", ";
            }
            if (!(resultado.Equals(IUSConstants.CADENA_VACIA) || (resultado.Equals(" , "))))
            {
                resultado = resultado.Substring(0, resultado.Length - 2);
            }
            return resultado;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ius"></param>
        /// <returns></returns>

        public List<String> getFuentes()
        {
            TesisDAO tesis = (TesisDAO)contexto.getInitialContext().GetObject("TesisDAO");
            List<String> resultado = tesis.getFuentes();
            return resultado;
        }

        /// <summary>
        /// Establece la búsqueda por índice adecuada para cada caso y devuelve
        /// la lista para poder ser mostrada.
        /// </summary>
        /// <param name="idBusqueda"> el identificador de la búsqueda a realizar.</param>
        /// <param name="ordenar"> La columna por la que se ordena el resultado.</param>
        /// <returns> La lista que coincide con el criterio de búsqueda.</returns>
        ///
        public List<TesisTO> getTesisPorIndices(String idBusqueda, String ordenar)
        {
            TesisDAO tesis = (TesisDAO)contexto.getInitialContext().GetObject("TesisDAO");
            String tipoBusqueda = idBusqueda.Substring(0, 1);
            MostrarPorIusTO parametros = new MostrarPorIusTO();
            parametros.setOrderBy(ordenar);
            parametros.setOrderType(IUSConstants.ORDER_TYPE_DEFAULT);
            if (tipoBusqueda.Equals(IUSConstants.BUSQUEDA_INDICES_SECUENCIAL_PREFIX))
            {
                parametros.setBusquedaEspecialValor(idBusqueda.Substring(1));
                return tesis.getTesisPorInstancias(parametros);
            }
            else if (tipoBusqueda.Equals(IUSConstants.BUSQUEDA_INDICES_MATERIA_PREFIX))
            {
                if (parametros.getOrderBy().Equals(IUSConstants.ORDER_INDICE))
                {
                    parametros.setOrderBy(IUSConstants.ORDER_INDICE_LETRA);
                }
                parametros.setBusquedaEspecialValor(idBusqueda.Substring(1, 1));
                parametros.setLetra(Int32.Parse(idBusqueda.Substring(idBusqueda.Length - 1)));
                return tesis.getTesisPorMaterias(parametros);
            }
            else
            {
                parametros.setBusquedaEspecialValor(idBusqueda.Substring(1));
                return tesis.getTesisPorTribunal(parametros);
            }
        }

        /// <summary>
        /// Establece la búsqueda por índice adecuada para cada caso y devuelve
        /// la lista para poder ser mostrada.
        /// </summary>
        /// <param name="idBusqueda"> el identificador de la búsqueda a realizar.</param>
        /// <param name="ordenar"> La columna por la que se ordena el resultado.</param>
        /// <returns> La lista que coincide con el criterio de búsqueda.</returns>
        ///
        public PaginadorTO getTesisPorIndicesPaginador(String idBusqueda, String ordenar)
        {
            TesisDAO tesis = (TesisDAO)contexto.getInitialContext().GetObject("TesisDAO");
            String tipoBusqueda = idBusqueda.Substring(0, 1);
            MostrarPorIusTO parametros = new MostrarPorIusTO();
            parametros.setOrderBy(ordenar);
            parametros.setOrderType(IUSConstants.ORDER_TYPE_DEFAULT);
            if (tipoBusqueda.Equals(IUSConstants.BUSQUEDA_INDICES_SECUENCIAL_PREFIX))
            {
                parametros.setBusquedaEspecialValor(idBusqueda.Substring(1));
                return tesis.getTesisPorInstanciasPaginador(parametros);
            }
            else if (tipoBusqueda.Equals(IUSConstants.BUSQUEDA_INDICES_MATERIA_PREFIX))
            {
                if (parametros.getOrderBy().Equals(IUSConstants.ORDER_INDICE))
                {
                    parametros.setOrderBy(IUSConstants.ORDER_INDICE_LETRA);
                }
                parametros.setBusquedaEspecialValor(idBusqueda.Substring(1, 1));
                parametros.setLetra(Int32.Parse(idBusqueda.Substring(2,idBusqueda.Length - 2)));
                return tesis.getTesisPorMateriasPaginador(parametros);
            }
            else
            {
                parametros.setBusquedaEspecialValor(idBusqueda.Substring(1));
                return tesis.getTesisPorTribunalPaginador(parametros);
            }
        }

        /// <summary>
        /// Establece la búsqueda por índice adecuada para cada caso y devuelve
        /// la lista para poder ser mostrada.
        /// </summary>
        /// <param name="parametros"> los parámetros de la búsqueda a realizar</param>
        /// <returns> La lista que coincide con el criterio de búsqueda.</returns>
        public List<TesisTO> getTesisPorIndices(MostrarPorIusTO parametros)
        {
            TesisDAO tesis = (TesisDAO)contexto.getInitialContext().GetObject("TesisDAO");
            List<TesisTO> resultado;
            String idBusqueda = parametros.getBusquedaEspecialValor();
            String tipoBusqueda = idBusqueda.Substring(0, 1);
            parametros.setBusquedaEspecialValor(idBusqueda.Substring(1,1));
            parametros.setOrderType(IUSConstants.ORDER_TYPE_DEFAULT);
            if (tipoBusqueda.Equals(IUSConstants.BUSQUEDA_INDICES_SECUENCIAL_PREFIX))
            {
                if (parametros.getFilterBy() != null)
                {
                    if (parametros.getFilterBy().Equals(IUSConstants.FILTRADO_POR_TESIS_AISLADAS_CONSTANTE))
                    {
                        parametros.setFilterBy(IUSConstants.FILTRADO_POR_TESIS_AISLADAS_COLUMNA);
                        parametros.setFilterValue(IUSConstants.FILTRADO_POR_TESIS_AISLADAS_VALOR);
                    }
                    else if (parametros.getFilterBy().Equals(IUSConstants.FILTRADO_POR_CONTRADICCION_CONSTANTE))
                    {
                        parametros.setFilterBy(IUSConstants.FILTRADO_POR_CONTRADICCION_COLUMNA);
                        parametros.setFilterValue(IUSConstants.FILTRADO_POR_CONTRADICCION_VALOR);
                    }
                    else if (parametros.getFilterBy().Equals(IUSConstants.FILTRADO_POR_CONTROVERSIAS_CONSTANTE))
                    {
                        parametros.setFilterBy(IUSConstants.FILTRADO_POR_CONTROVERSIAS_COLUMNA);
                        parametros.setFilterValue(IUSConstants.FILTRADO_POR_CONTROVERSIAS_VALOR);
                    }
                    else if (parametros.getFilterBy().Equals(IUSConstants.FILTRADO_POR_REITERACIONES_CONSTANTE))
                    {
                        parametros.setFilterBy(IUSConstants.FILTRADO_POR_REITERACIONES_COLUMNA);
                        parametros.setFilterValue(IUSConstants.FILTRADO_POR_REITERACIONES_VALOR);
                    }
                    else if (parametros.getFilterBy().Equals(IUSConstants.FILTRADO_POR_ACCIONES_CONSTANTE))
                    {
                        parametros.setFilterBy(IUSConstants.FILTRADO_POR_ACCIONES_COLUMNA);
                        parametros.setFilterValue(IUSConstants.FILTRADO_POR_ACCIONES_VALOR);
                    }
                    if (parametros.getFilterBy().Equals(IUSConstants.FILTRADO_JURISPRUDENCIA))
                    {
                        resultado = tesis.getIndicesJurisprudencia(parametros);
                    }
                    else if (parametros.getFilterBy().Equals(IUSConstants.FILTRADO_NINGUNO))
                    {
                        resultado = tesis.getTesisPorInstancias(parametros);
                    }
                    else
                    {
                        resultado = tesis.getIndicesConFiltro(parametros);
                    }
                }
                else
                {
                    resultado = tesis.getTesisPorInstancias(parametros);
                }
                return resultado;
            }
            else if (tipoBusqueda.Equals(IUSConstants.BUSQUEDA_INDICES_MATERIA_PREFIX))
            {
                parametros.setBusquedaEspecialValor(idBusqueda.Substring(1, 1));
                parametros.setLetra(Int32.Parse(idBusqueda.Substring(2)));
                if (parametros.getFilterBy() != null)
                {
                    if (parametros.getFilterBy().Equals(IUSConstants.FILTRADO_POR_TESIS_AISLADAS_CONSTANTE))
                    {
                        parametros.setFilterBy(IUSConstants.FILTRADO_POR_TESIS_AISLADAS_COLUMNA);
                        parametros.setFilterValue(IUSConstants.FILTRADO_POR_TESIS_AISLADAS_VALOR);
                    }
                    else if (parametros.getFilterBy().Equals(IUSConstants.FILTRADO_POR_CONTRADICCION_CONSTANTE))
                    {
                        parametros.setFilterBy(IUSConstants.FILTRADO_POR_CONTRADICCION_COLUMNA);
                        parametros.setFilterValue(IUSConstants.FILTRADO_POR_CONTRADICCION_VALOR);
                    }
                    else if (parametros.getFilterBy().Equals(IUSConstants.FILTRADO_POR_CONTROVERSIAS_CONSTANTE))
                    {
                        parametros.setFilterBy(IUSConstants.FILTRADO_POR_CONTROVERSIAS_COLUMNA);
                        parametros.setFilterValue(IUSConstants.FILTRADO_POR_CONTROVERSIAS_VALOR);
                    }
                    else if (parametros.getFilterBy().Equals(IUSConstants.FILTRADO_POR_REITERACIONES_CONSTANTE))
                    {
                        parametros.setFilterBy(IUSConstants.FILTRADO_POR_REITERACIONES_COLUMNA);
                        parametros.setFilterValue(IUSConstants.FILTRADO_POR_REITERACIONES_VALOR);
                    }
                    else if (parametros.getFilterBy().Equals(IUSConstants.FILTRADO_POR_ACCIONES_CONSTANTE))
                    {
                        parametros.setFilterBy(IUSConstants.FILTRADO_POR_ACCIONES_COLUMNA);
                        parametros.setFilterValue(IUSConstants.FILTRADO_POR_ACCIONES_VALOR);
                    }
                    if (parametros.getFilterBy().Equals(IUSConstants.FILTRADO_JURISPRUDENCIA))
                    {
                        resultado = tesis.getMateriasJurisprudencia(parametros);
                    }
                    else if (parametros.getFilterBy().Equals(IUSConstants.FILTRADO_NINGUNO))
                    {
                        resultado = tesis.getTesisPorMaterias(parametros);
                    }
                    else
                    {
                        resultado = tesis.getMateriasConFiltro(parametros);
                    }
                }
                else
                {
                    resultado = tesis.getTesisPorMaterias(parametros);
                }
                return resultado;
            }
            else
            {
                if (parametros.getFilterBy() != null)
                {
                    if (parametros.getFilterBy().Equals(IUSConstants.FILTRADO_POR_TESIS_AISLADAS_CONSTANTE))
                    {
                        parametros.setFilterBy(IUSConstants.FILTRADO_POR_TESIS_AISLADAS_COLUMNA);
                        parametros.setFilterValue(IUSConstants.FILTRADO_POR_TESIS_AISLADAS_VALOR);
                    }
                    else if (parametros.getFilterBy().Equals(IUSConstants.FILTRADO_POR_CONTRADICCION_CONSTANTE))
                    {
                        parametros.setFilterBy(IUSConstants.FILTRADO_POR_CONTRADICCION_COLUMNA);
                        parametros.setFilterValue(IUSConstants.FILTRADO_POR_CONTRADICCION_VALOR);
                    }
                    else if (parametros.getFilterBy().Equals(IUSConstants.FILTRADO_POR_CONTROVERSIAS_CONSTANTE))
                    {
                        parametros.setFilterBy(IUSConstants.FILTRADO_POR_CONTROVERSIAS_COLUMNA);
                        parametros.setFilterValue(IUSConstants.FILTRADO_POR_CONTROVERSIAS_VALOR);
                    }
                    else if (parametros.getFilterBy().Equals(IUSConstants.FILTRADO_POR_REITERACIONES_CONSTANTE))
                    {
                        parametros.setFilterBy(IUSConstants.FILTRADO_POR_REITERACIONES_COLUMNA);
                        parametros.setFilterValue(IUSConstants.FILTRADO_POR_REITERACIONES_VALOR);
                    }
                    else if (parametros.getFilterBy().Equals(IUSConstants.FILTRADO_POR_ACCIONES_CONSTANTE))
                    {
                        parametros.setFilterBy(IUSConstants.FILTRADO_POR_ACCIONES_COLUMNA);
                        parametros.setFilterValue(IUSConstants.FILTRADO_POR_ACCIONES_VALOR);
                    }
                    if (parametros.getFilterBy().Equals(IUSConstants.FILTRADO_JURISPRUDENCIA))
                    {
                        resultado = tesis.getTribunalJurisprudencia(parametros);
                    }
                    else if (parametros.getFilterBy().Equals(IUSConstants.FILTRADO_NINGUNO))
                    {
                        resultado = tesis.getTesisPorTribunal(parametros);
                    }
                    else
                    {
                        resultado = tesis.getTribunalConFiltro(parametros);
                    }
                }
                else
                {
                    resultado = tesis.getTesisPorTribunal(parametros);
                }
                return resultado;
            }
        }
        /// <summary>
        /// Obtiene los nodos de la búsqueda temática.
        /// </summary>
        /// <param name="id">El identificador del nodo padre.</param>
        /// <returns></returns>
        public List<TreeNodeDataTO> getRaizTemática(String id)
        {
            TesisDAO tesis = (TesisDAO)contexto.getInitialContext().GetObject("TesisDAO");
            List<RaizTO> resultadoRaiz = null;
            if (!id.Contains("T"))
            {
                resultadoRaiz = tesis.getRaizTematica(id);
            }
            else
            {
                String newId = id.Substring(1);
                resultadoRaiz = tesis.getRaizConstitucional(newId);
            }
            List<TreeNodeDataTO> resultado = new List<TreeNodeDataTO>();
            foreach (RaizTO item in resultadoRaiz)
            {
                TreeNodeDataTO nodo = new TreeNodeDataTO();
                nodo.Id = item.Id;
                nodo.Href = item.Hiperlink;
                nodo.Target = item.Tabla;
                nodo.Padre = item.Padre;
                nodo.Label = item.Descripcion.ToUpper();
                nodo.IsLeaf = (item.Cuantas == 0);
                resultado.Add(nodo);
            }
            return resultado;
        }


        /// <summary>
        /// Obtiene los nodos de la búsqueda temática, tomando en cuenta que la
        /// descripción debe tener una palabra determinada
        /// </summary>
        /// <param name="id">El identificador del nodo padre.</param>
        /// <returns></returns>
        public List<TreeNodeDataTO> getRaizTemática(String id, String Busqueda)
        {
            TesisDAO tesis = (TesisDAO)contexto.getInitialContext().GetObject("TesisDAO");
            List<RaizTO> resultadoRaiz = null;
            if (!id.Contains("T"))
            {
                resultadoRaiz = tesis.getRaizTematica(id);
            }
            else
            {
                String newId = id.Substring(1);
                resultadoRaiz = tesis.getRaizConstitucional(newId, Busqueda);
            }
            List<TreeNodeDataTO> resultado = new List<TreeNodeDataTO>();
            foreach (RaizTO item in resultadoRaiz)
            {
                TreeNodeDataTO nodo = new TreeNodeDataTO();
                nodo.Id = item.Id;
                nodo.Href = item.Hiperlink;
                nodo.Target = item.Tabla;
                nodo.Padre = item.Padre;
                nodo.Label = item.Descripcion.ToUpper();
                nodo.IsLeaf = (item.Cuantas == 0);
                resultado.Add(nodo);
            }
            resultado = ObtenListaCompleta(resultado);
            return resultado;
        }

        private List<TreeNodeDataTO> ObtenListaCompleta(List<TreeNodeDataTO> resultado)
        {
            List<TreeNodeDataTO> listaTemas = new List<TreeNodeDataTO>();
            List<TreeNodeDataTO> listaSubtemas = new List<TreeNodeDataTO>();
            List<TreeNodeDataTO> listaResultado = new List<TreeNodeDataTO>();
            foreach (TreeNodeDataTO item in resultado)
            {
                if (item.Padre.Equals("0") || item.Padre.Equals("T0"))
                {
                    listaTemas.Add(item);
                }
                else
                {
                    listaSubtemas.Add(item);
                }
            }
            foreach (TreeNodeDataTO item in listaTemas)
            {
                listaResultado.Add(item);
            }
            foreach (TreeNodeDataTO item in listaSubtemas)
            {
                listaResultado.Add(item);
            }
            listaSubtemas = obtenPadres(listaSubtemas);
            if (listaSubtemas.Count > 0)
            {
                listaSubtemas = ObtenListaCompleta(listaSubtemas);
                foreach (TreeNodeDataTO item in listaSubtemas)
                {
                    listaResultado.Add(item);
                }
            }
            return listaResultado;
        }

        private List<TreeNodeDataTO> obtenPadres(List<TreeNodeDataTO> listaSubtemas)
        {
            List<TreeNodeDataTO> listadoFinal = new List<TreeNodeDataTO>();
            TesisDAO tesis = (TesisDAO)contexto.getInitialContext().GetObject("TesisDAO");
            List<String> listaIdsPadres = new List<string>();
            foreach (TreeNodeDataTO item in listaSubtemas)
            {
                listaIdsPadres.Add(item.Padre);
                //TreeNodeDataTO nodoAdicional = tesis.getNodoConstitucional(item.Padre);
                //if (nodoAdicional.Id != null)
                //{
                //    listadoFinal.Add(nodoAdicional);
                //}
            }
            listadoFinal = tesis.getNodosConstitucionales(listaIdsPadres);
            return listadoFinal;
        }


        public List<TreeNodeDataTO> getSubtemas(String tabla)
        {
            TesisDAO tesis = (TesisDAO)contexto.getInitialContext().GetObject("TesisDAO");
            List<RaizTO> resultadoRaiz = null;
            resultadoRaiz = tesis.getSubtemas(tabla);
            List<TreeNodeDataTO> resultado = new List<TreeNodeDataTO>();
            foreach (RaizTO item in resultadoRaiz)
            {
                TreeNodeDataTO nodo = new TreeNodeDataTO();
                nodo.Id = item.Id;
                nodo.Padre = item.Padre;
                nodo.Href = tabla;
                nodo.Target = item.Tabla;
                nodo.Label = item.Descripcion.ToUpper().Trim() +" [" + item.Cuantas + "]";
                nodo.IsLeaf = (item.Cuantas == 0);
                resultado.Add(nodo);
            }
            return resultado;
        }

        public List<TreeNodeDataTO> getSinonimos(String tabla, String ID)
        {
            TesisDAO tesis = (TesisDAO)contexto.getInitialContext().GetObject("TesisDAO");
            List<RaizTO> resultadoRaiz = null;
            if (tabla.ToLower().Contains("alternos"))
            {
                resultadoRaiz = tesis.getSinonimos(tabla, ID);
            }
            else
            {
                resultadoRaiz = tesis.getSubtemas(tabla);
            }
            List<TreeNodeDataTO> resultado = new List<TreeNodeDataTO>();
            foreach (RaizTO item in resultadoRaiz)
            {
                TreeNodeDataTO nodo = new TreeNodeDataTO();
                nodo.Id = item.Id;
                nodo.Padre = item.Padre;
                nodo.Href = tabla;
                nodo.Target = item.Tabla;
                nodo.Label = item.Descripcion + " [" + item.Cuantas + "]";
                nodo.IsLeaf = (item.Cuantas == 0);
                resultado.Add(nodo);
            }
            return resultado;
        }

        public TreeNodeDataTO getIdConstitucional(string id)
        {
            TesisDAO tesis = (TesisDAO)contexto.getInitialContext().GetObject("TesisDAO");
            RaizTO resultadoRaiz = tesis.getIdConstitucional(id);
            TreeNodeDataTO resultado = new TreeNodeDataTO();
            resultado.Id = resultadoRaiz.Id;
            resultado.Href = resultadoRaiz.Hiperlink;
            resultado.Target = resultadoRaiz.Tabla;
            resultado.Padre = resultadoRaiz.Padre;
            resultado.Label = resultadoRaiz.Descripcion;
            resultado.IsLeaf = (resultadoRaiz.Cuantas == 0);
            return resultado;
        }

        public TreeNodeDataTO getAscendenteConstitucional(string id)
        {
            TesisDAO tesis = (TesisDAO)contexto.getInitialContext().GetObject("TesisDAO");
            RaizTO resultadoRaiz = tesis.getAscendenteConstitucional(id);
            TreeNodeDataTO resultado = new TreeNodeDataTO();
            resultado.Id = resultadoRaiz.Id;
            resultado.Href = resultadoRaiz.Hiperlink;
            resultado.Target = resultadoRaiz.Tabla;
            resultado.Padre = resultadoRaiz.Padre;
            resultado.Label = resultadoRaiz.Descripcion;
            resultado.IsLeaf = (resultadoRaiz.Cuantas == 0);
            return resultado;
        }

        public List<TreeNodeDataTO> getProximidadConstitucional(String Id)
        {
            TesisDAO tesis = (TesisDAO)contexto.getInitialContext().GetObject("TesisDAO");
            List<RaizTO> resultadoRaiz = null;
                String newId = Id.Substring(1);
                resultadoRaiz = tesis.getSinonimoConstitucional(newId, IUSConstants.TIPO_SINONIMO_CONSTITUCIONAL);
            List<TreeNodeDataTO> resultado = new List<TreeNodeDataTO>();
            foreach (RaizTO item in resultadoRaiz)
            {
                TreeNodeDataTO nodo = new TreeNodeDataTO();
                nodo.Id = item.Id;
                nodo.Href = item.Hiperlink;
                nodo.Target = item.Tabla;
                nodo.Padre = item.Padre;
                nodo.Label = item.Descripcion;
                nodo.IsLeaf = (item.Cuantas == 0);
                resultado.Add(nodo);
            }
            return resultado;
        }

        public List<TreeNodeDataTO> getSinonimoConstitucional(string Id)
        {
            TesisDAO tesis = (TesisDAO)contexto.getInitialContext().GetObject("TesisDAO");
            List<RaizTO> resultadoRaiz = null;
            String newId = Id.Substring(1);
            resultadoRaiz = tesis.getSinonimoConstitucional(newId, IUSConstants.TIPO_PROXIMIDAD_CONSTITUCIONAL);
            List<TreeNodeDataTO> resultado = new List<TreeNodeDataTO>();
            foreach (RaizTO item in resultadoRaiz)
            {
                TreeNodeDataTO nodo = new TreeNodeDataTO();
                nodo.Id = item.Id;
                nodo.Href = item.Hiperlink;
                nodo.Target = item.Tabla;
                nodo.Padre = item.Padre;
                nodo.Label = item.Descripcion;
                nodo.IsLeaf = (item.Cuantas == 0);
                resultado.Add(nodo);
            }
            return resultado;
        }

        public List<TesisTO> getTesis(BusquedaAlmacenadaTO busqueda)
        {
            int tipo = busqueda.TipoBusqueda;
            switch (tipo)
            {
                case IUSConstants.BUSQUEDA_TESIS_SIMPLE:
                    BusquedaTO parametros = new BusquedaTO();
                    parametros.TipoBusqueda = tipo;
                    parametros.Tribunales = busqueda.Tribunales.ToList();
                    parametros.OrdenarPor = IUSConstants.ORDER_DEFAULT;
                    parametros.Acuerdos = generaAcuerdos(IUSConstants.BLOQUE_VACIO);
                    generaPanelesTesis(busqueda.Epocas, parametros);
                    if (busqueda.Expresiones != null && busqueda.Expresiones.Count > 0)
                    {
                        parametros.Palabra = new List<BusquedaPalabraTO>();
                        foreach (ExpresionBusqueda item in busqueda.Expresiones)
                        {
                            BusquedaPalabraTO itemGuardar = new BusquedaPalabraTO();
                            itemGuardar.ValorLogico = item.Operador;
                            itemGuardar.Expresion = item.Expresion;
                            itemGuardar.Jurisprudencia = item.IsJuris;
                            itemGuardar.Campos = item.Campos;
                            parametros.Palabra.Add(itemGuardar);
                        }
                    }
                    return getIdTesisPorParte(parametros);
                    break;
                case IUSConstants.BUSQUEDA_TESIS_TEMATICA:
                    TesisDAO daoTesis = (TesisDAO)contexto.GetObject("TesisDAO");
                    parametros = new BusquedaTO();
                    parametros.TipoBusqueda = tipo;
                    String tabla = obtenTabla(busqueda.ValorBusqueda);
                    String identificador = obtenIdentificadorTematico(busqueda.ValorBusqueda);
                    return daoTesis.getTesisSubtemasSinonimoPalabra(tabla, identificador);
                    //if (tabla.Equals(IUSConstants.BUSQUEDA_TESIS_THESAURO))
                    //{
                    //    return daoTesis.getTesisTesauro(identificador);
                    //}
                    //else if (!tabla.ToLower().Contains("alterno"))
                    //{
                    //    return daoTesis.getTesisSubtemas(tabla, identificador);
                    //}
                    //else
                    //{
                    //    return daoTesis.getTesisSinonimos(tabla, identificador);
                    //}
                case IUSConstants.BUSQUEDA_ESPECIALES:
                    //daoTesis = (TesisDAO)contexto.GetObject("TesisDAO");
                    MostrarPorIusTO parametrosEspeciales = new MostrarPorIusTO();
                    parametrosEspeciales.BusquedaEspecialValor = busqueda.ValorBusqueda;
                    parametrosEspeciales.OrderBy = IUSConstants.ORDER_DEFAULT;
                    parametrosEspeciales.OrderType = IUSConstants.ORDER_TYPE_DEFAULT;
                    return getTesis(parametrosEspeciales);
            }
            return new List<TesisTO>();
        }

        private string obtenIdentificadorTematico(string p)
        {
            String[] parametros = p.Split(IUSConstants.SEPARADOR_FRASES[0].ToCharArray());
            return parametros[5].Trim();
        }

        private string obtenTabla(string p)
        {
            String[] parametros = p.Split(IUSConstants.SEPARADOR_FRASES[0].ToCharArray());
            return parametros[0].Trim();
        }

        private void generaPanelesTesis(int[] p, BusquedaTO busqueda)
        {
            Boolean[][] matrizActual;
            int largo, ancho;
            //Comencemos con la epoca
            largo = IUSConstants.EPOCAS_LARGO;
            ancho = IUSConstants.EPOCAS_ANCHO;
            matrizActual = new bool[largo][];
            int contador = 0;
            for (int j = 0; j < largo; j++)
            {
                Boolean[] lineaActual = new bool[ancho];
                matrizActual[j] = lineaActual;
            }

            for (int j = 0; j < largo; j++)
            {
                for (int i = 0; i < ancho; i++)
                {
                    matrizActual[j][i] = p.Contains(contador);
                    contador++;
                }
            }
            busqueda.Epocas = matrizActual;
            //Siguen los apendices
            largo = IUSConstants.APENDICES_LARGO;
            ancho = IUSConstants.APENDICES_ANCHO;
            matrizActual = new bool[ancho][];
            contador = 100;
            for (int j = 0; j < ancho; j++)
            {
                Boolean[] lineaActual = new bool[largo];
                matrizActual[j] = lineaActual;
            }
            for (int i = 0; i < largo; i++)
            {
                for (int j = 0; j < ancho; j++)
                {

                    matrizActual[j][i] = p.Contains(contador);
                    contador++;
                }
            }

            busqueda.Apendices = matrizActual;
        }

        private bool[][] generaAcuerdos(int p)
        {
            Boolean[][] matrizActual;
            int largo, ancho;

            //Al final siguen los acuerdos
            largo = IUSConstants.ACUERDOS_LARGO;
            ancho = IUSConstants.ACUERDOS_ANCHO;
            matrizActual = new bool[ancho][];
            for (int j = 0; j < ancho; j++)
            {
                Boolean[] lineaActual = new bool[largo];
                for (int i = 0; i < largo; i++)
                {
                    lineaActual[i] = false;
                }
                matrizActual[j] = lineaActual;
            }
            return matrizActual;
        }
        /// <summary>
        /// Obtiene el catálogo de ponentes.
        /// </summary>
        /// <returns>La lista de los ponentes.</returns>
        public List<PonenteTO> getCatalogoPonente()
        {
            TesisDAO tesis = (TesisDAO)contexto.GetObject("TesisDAO");
            return tesis.getCatalogoPonente();
        }

        internal List<AsuntoTO> getCatalogoAsunto()
        {
            TesisDAO tesis = (TesisDAO)contexto.GetObject("TesisDAO");
            return tesis.getCatalogoAsunto();
        }

        internal List<TreeNodeDataTO> getSubtemas(string tabla, string busqueda)
        {
            TesisDAO tesis = (TesisDAO)contexto.getInitialContext().GetObject("TesisDAO");
            List<RaizTO> resultadoRaiz = null;
            resultadoRaiz = tesis.getSubtemas(tabla, busqueda);
            List<TreeNodeDataTO> resultado = new List<TreeNodeDataTO>();
            foreach (RaizTO item in resultadoRaiz)
            {
                TreeNodeDataTO nodo = new TreeNodeDataTO();
                nodo.Id = item.Id;
                nodo.Padre = item.Padre;
                nodo.Href = tabla;
                nodo.Target = item.Tabla;
                nodo.Label = item.Descripcion + " [" + item.Cuantas + "]";
                nodo.IsLeaf = (item.Cuantas == 0);
                resultado.Add(nodo);
            }
            return resultado;
        }

        internal TreeNodeDataTO getSubtema(string tabla, string valor)
        {
            TesisDAO tesis = (TesisDAO)contexto.GetObject("TesisDAO");
            RaizTO tema = tesis.getSubtema(tabla, valor);
            TreeNodeDataTO nodo = new TreeNodeDataTO();
            nodo.Id = tema.Id;
            nodo.Padre = tema.Padre;
            nodo.Href = tabla;
            nodo.Target = tema.Tabla;
            nodo.Label = tema.TemaMay + " [" + tema.Cuantas + "]";
            nodo.IsLeaf = (tema.Cuantas == 0);
            return nodo;
        }

        public List<TreeNodeDataTO> getTemasRelacionados(string Id)
        {
            TesisDAO tesis = (TesisDAO)contexto.GetObject("TesisDAO");
            String TemasIdProg = tesis.getIdProg(Id);
            String[] Temas = TemasIdProg.Split();
            List<TreeNodeDataTO> resultado = new List<TreeNodeDataTO>();
            foreach (String tema in Temas)
            {
                int valorNoTema=0;
                if ((!Int32.TryParse(tema,out valorNoTema))&&(!tema.Trim().Equals(String.Empty)))
                {
                    String tabla = obtenTablaBuscar(tema);
                    String StringId = obtenIdTemaBuscar(tema);
                    
                    RaizTO temaVerdadero = tesis.getSubtemaTesis(tabla, StringId);
                    if (temaVerdadero != null)
                    {
                        TreeNodeDataTO nodo = new TreeNodeDataTO();
                        nodo.Id = temaVerdadero.Id;
                        nodo.Padre = temaVerdadero.Padre;
                        nodo.Href = tabla;
                        nodo.Target = tabla;
                        nodo.Label = temaVerdadero.TemaMay;// +" [" + temaVerdadero.Cuantas + "]";
                        nodo.IsLeaf = (temaVerdadero.Cuantas == 0);
                        resultado.Add(nodo);
                    }
                }
            }
            return resultado;
        }

        private string obtenTablaBuscar(string Id)
        {
            char[] digitos = {'1','2','3','4','5','6','7','8','9','0'};
            return Id.Substring(0, Id.IndexOfAny(digitos));
        }

        private string obtenIdTemaBuscar(string Id)
        {
            char[] digitos = { '1', '2', '3', '4', '5', '6', '7', '8', '9', '0' };
            return Id.Substring(Id.IndexOfAny(digitos));
        }

        public List<TipoPonenteTO> getCatalogoTipoPonente()
        {
            TesisDAO tesis = (TesisDAO)contexto.getInitialContext().GetObject("TesisDAO");
            return tesis.getCatalogoTipoPonente();
        }

        public List<CategoriaDocTO> GetCategorias()
        {
            TesisDAO tesis = (TesisDAO)contexto.getInitialContext().GetObject("TesisDAO");
            return tesis.GetCategorias();
        }

        internal List<DocumentoTO> GetDocumento(int Categoria)
        {
            TesisDAO tesis = (TesisDAO) contexto.getInitialContext().GetObject("TesisDAO");
            return tesis.GetDocumanto(Categoria);
        }

        public int getVolumen(int p)
        {
            TesisDAO tesis = (TesisDAO) contexto.getInitialContext().GetObject("TesisDAO");
            return tesis.getVolumen(p);
        }

        internal TesisTO getTesisPaginadaPorPosicion(int paginador, int posicion)
        {
            TesisDAO tesis = (TesisDAO)contexto.getInitialContext().GetObject("TesisDAO");
            return tesis.getTesisPaginadaPorPosicion(paginador, posicion);
        }

        internal List<TomoTO> getTomosPrimerNivel()
        {
            TesisDAO tesis = (TesisDAO) contexto.getInitialContext().GetObject("TesisDAO");
            return tesis.getTomosPrimerNivel();
        }

        public List<SeccionTO> getTomosSecciones(int p)
        {
            TesisDAO tesis = (TesisDAO)contexto.getInitialContext().GetObject("TesisDAO");
            List<SeccionTO> Resultado = tesis.getTomos(p);
            List<SeccionTO> ResTmp = new List<SeccionTO>();
            ResTmp.AddRange(Resultado);
            if (Resultado.Count == 0)
            {
                List<SeccionTO> Secciones = tesis.getSecciones(p);
                Resultado.AddRange(Secciones);
                return Resultado;
            }
            foreach (SeccionTO item in Resultado)
            {
                List<SeccionTO> Secciones = tesis.getSecciones(item.Id);
                ResTmp.AddRange(Secciones);
            }
            Resultado = ResTmp;
            return Resultado;
        }
    }
}
