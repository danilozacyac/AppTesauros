using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Odbc;
using mx.gob.scjn.ius_common.context;
using System.Data;
using mx.gob.scjn.ius_common.TO;
using System.Data.Common;
using log4net;
using mx.gob.scjn.ius_common.utils;
using Lucene.Net.Search;
using Lucene.Net.QueryParsers;
using Lucene.Net.Documents;
using System.Diagnostics;

namespace mx.gob.scjn.ius_common.DAO.impl
{
    public class AcuerdosDAOImpl:AcuerdosDAO
    {
        public DBContext contextoBD;
        private ILog log = LogManager.GetLogger("mx.gob.scjn.ius_common.DAO.impl.AcuerdosDAOImpl");

        public AcuerdosDAOImpl()
        {
            IUSApplicationContext contexto = new IUSApplicationContext();
            contextoBD = (DBContext)contexto.getInitialContext().GetObject("dataSource");
        }

        #region AcuerdosDAO Members

        public List<mx.gob.scjn.ius_common.TO.AcuerdosTO> getAll()
        {
            throw new NotImplementedException();
        }

        public List<mx.gob.scjn.ius_common.TO.AcuerdosTO> getPorPartes(int[] partes)
        {
            throw new NotImplementedException();
        }

        public List<mx.gob.scjn.ius_common.TO.AcuerdosTO> getPorConsec(int Consec)
        {
            throw new NotImplementedException();
        }

        public List<mx.gob.scjn.ius_common.TO.AcuerdosTO> getAcuerdos(mx.gob.scjn.ius_common.TO.EpocasTO epocas)
        {
            throw new NotImplementedException();
        }

        public List<AcuerdosTO> getAcuerdos(mx.gob.scjn.ius_common.TO.PartesTO epocas)
        {
            DbConnection conexion = contextoBD.ContextConectionEVA;
            try
            {
                DataAdapter query = contextoBD.dataAdapter
                                ("select A.ParteT,        A.Consec,     A.Id,        A.Tesis,     C.descInst As sala," +
                                 "       B.descEpocaAbr As epoca,     F.txtVolumen As Volumen,    A.Fuente,    A.Pagina,    A.Rubro," +
                                 "       F.Orden As VolOrden,    A.ConsecIndx, A.Procesado, A.TpoAsunto, A.Promovente," +
                                 "       A.Clasificacion, A.Complemento, A.OrdenTema, A.OrdenAcuerdo "+
                                 " from Acuerdo As A, cepocas As B, cinsts As C,  volumen As F " +
                                 " where parteT="+epocas.getParte()+
                                 "       AND A.epoca = B.idEpoca "+
                                 "       AND A.sala = C.idInst " + 
                                 "       AND A.volumen = F.volumen  " +
                                 "  order by " + epocas.getOrderBy() + " " + epocas.getOrderType(), conexion);
                DataSet datos = new DataSet();
                query.Fill(datos);
                List<AcuerdosTO> lista = new List<AcuerdosTO>();
                DataTableReader tabla = datos.Tables[0].CreateDataReader();
                conexion.Close();
                //foreach (DataRow fila in tabla.Rows)
                while(tabla.Read())
                {
                    AcuerdosTO acuerdoActual = new AcuerdosTO();
                    acuerdoActual.setId("" + tabla["id"]);
                    acuerdoActual.setParteT("" + tabla["ParteT"]);
                    acuerdoActual.OrdenAcuerdo = (int)tabla["OrdenAcuerdo"];
                    acuerdoActual.OrdenTema = (int)tabla["OrdenTema"];
                    acuerdoActual.ConsecIndx = "" + tabla["ConsecIndx"];
                    lista.Add(acuerdoActual);
                }
                //conexion.Close();
                return lista;
            }
            catch (Exception e)
            {
                conexion.Close();
                log.Debug(e.Message);
                if (!EventLog.SourceExists("IUS"))
                {
                    EventLog.CreateEventSource("IUS","IUS");
                }
                EventLog Logg = new EventLog("IUS");
                    Logg.Source = "IUS";
                    String mensaje = "AcuerdoDAOImpl Exception at GetAcuerdos(PartesTO" + e.Message + e.StackTrace;
                    Logg.WriteEntry(mensaje);
                    Logg.Close();
                
                return new List<AcuerdosTO>();
            };
        }
        /// <summary>
        /// Obtiene los acuerdos resultantes de una búsqueda por palabra
        /// </summary>
        /// <param name="parametros">Los parámetros de la búsqueda</param>
        /// <returns>Los acuerdos que coincidan con los resultados</returns>
        public List<AcuerdosTO> getAcuerdosPorPalabra(BusquedaTO parametros)
        {
            SpanishAnalyzer analyzer = new SpanishAnalyzer();
            List<String> querysEscritos = new List<string>();
            String queryEpocas = "";
            String queryActual = "";
            BooleanQuery QueryCompleto = new BooleanQuery();
            BooleanQuery queryGlobal = new BooleanQuery();
            IndexSearcher searcher = new IndexSearcher(IUSConstants.DIRECCION_INDEX_ACUERDOS);
            if ((parametros.OrdenarPor == null) || (parametros.OrdenarPor.Equals(IUSConstants.ORDER_DEFAULT)))
            {
                //parametros.OrdenarPor = IUSConstants.ORDER_RUBRO;
                parametros.OrdenarPor = "consecIndx";
            }
            String ordenarPor = parametros.OrdenarPor == null ? IUSConstants.ORDER_DEFAULT : parametros.OrdenarPor;
            List<AcuerdosTO> resultado = new List<AcuerdosTO>();
            int[] partes = obtenPartes(parametros);
            String query = "";
            List<String> numeros = new List<string>();
            foreach (int item in partes)
            {
                query = "" + item;
                numeros.Add(query);
            }
            queryEpocas = String.Join(" OR ", numeros.ToArray());
            queryEpocas = "(parteT: (" + queryEpocas + "))";
            List<BusquedaPalabraTO> listado = FuncionesGenerales.GeneralizaBusqueda(parametros.Palabra);
            foreach (BusquedaPalabraTO palabras in listado)
            {
                querysEscritos.Add(obtenQueryPalabras(palabras, queryEpocas));
            }
            String queryTotal = "(";
            int primero = 0;
            foreach (String item in querysEscritos)
            {
                String operador = "";
                if (primero != 0)
                {
                    if (listado.ElementAt(primero).ValorLogico == IUSConstants.BUSQUEDA_PALABRA_OP_O)
                    {
                        operador = " OR ";
                    }
                    else if (listado.ElementAt(primero).ValorLogico == IUSConstants.BUSQUEDA_PALABRA_OP_NO)
                    {
                        operador = " NOT ";
                    }
                    else if ((listado.ElementAt(primero).ValorLogico == IUSConstants.BUSQUEDA_PALABRA_OP_Y)
                        || (listado.ElementAt(primero).ValorLogico == 0))
                    {
                        operador = " AND ";
                    }
                }
                if (queryTotal.Equals("("))
                {
                    queryTotal += "(" + item + ")";
                }
                else
                {
                    queryTotal = "(" + queryTotal + ")" + operador + "(" + item.ToString() + ")";
                }
                primero++;

            }
            queryTotal += ")";
            QueryParser queryGeneral = new QueryParser("", analyzer);

            queryGlobal.Add(queryGeneral.Parse(queryTotal), BooleanClause.Occur.SHOULD);
            Hits hits = searcher.Search(queryGlobal, new Sort(ordenarPor));
            int itemActual = 0;
            for (itemActual = 0; itemActual < hits.Length(); itemActual++)
            {
                Document item = hits.Doc(itemActual);
                AcuerdosTO itemVerdadero = new AcuerdosTO();
                itemVerdadero.Id = item.Get("id");
                itemVerdadero.ConsecIndx = item.Get("consecIndx");
                itemVerdadero.OrdenAcuerdo = int.Parse(item.Get("ordenAcuerdo"));
                itemVerdadero.OrdenTema = int.Parse(item.Get("ordenTema"));
                itemVerdadero.ParteT = ""+item.Get("parteT");
                itemVerdadero.Rubro = ""+item.Get("Rubro");
                resultado.Add(itemVerdadero);
            }
            try
            {
                searcher.Close();
            }
            catch (Exception e)
            {
                /** Pueden ocurrir errores por el manejo de los archivos temporales*/
                log.Error("No se pudo borrar el archivo temporal:\n" + e.Message + "\n" + e.StackTrace);
                if (!EventLog.SourceExists("IUS"))
                {
                    EventLog.CreateEventSource("IUS", "IUS");
                }
                EventLog Logg = new EventLog("IUS");
                Logg.Source = "IUS";
                String mensaje = "AcuerdoDAOImpl Exception at GetAcuerdosPorPalabra(BusquedaTO) \n" + e.Message + e.StackTrace;
                Logg.WriteEntry(mensaje);
                Logg.Close();
            }
            return resultado;
        }

        public List<AcuerdosTO> getAcuerdos(MostrarPorIusTO parametros)
        {
            DbConnection conexion = contextoBD.ContextConectionEVA;
            if (parametros.Listado.Count == 0) return new List<AcuerdosTO>();
            try
            {
                String select = "";
                select = "select A.ParteT,        A.Consec,     A.Id,        A.Tesis,     C.descInst As sala, " +
                                 "       B.descEpocaAbr As epoca,     D.txtVolumen As volumen,    E.DescFteAbr As Fuente,    A.Pagina,    A.Rubro," +
                                 "       D.orden As VolOrden,      A.ConsecIndx, A.Procesado, A.TpoAsunto, A.Promovente, " +
                                 "       A.Clasificacion, A.Complemento, A.OrdenTema, A.OrdenAcuerdo, F.txtVolumen as subvolumen " +
                                 "  from Acuerdo As A, cepocas As B, cinsts As C, volumen As D, cfuentes As E, subVolumen As F " +
                                 "             where A.epoca = B.idEpoca" +
                                 "               AND A.sala = C.idInst " +
                                 "               AND A.volumen = D.volumen" +
                                 "               AND A.fuente = E.idfte" +
                                 "               AND A.idsubVolumen = F.idSubvolumen"+
                                 "               AND A.id IN (";
                foreach (int item in parametros.Listado)
                {
                    select += "" + item + ",";
                }
                select = select.Substring(0, select.Length - 1);
                select += ") order by " + parametros.OrderBy + " " + parametros.OrderType;
                //OdbcConnection conexion = contextoBD.contextConection;//new OdbcConnection(connectionString);
                DataAdapter query = contextoBD.dataAdapter
                                (select, conexion);
                DataSet datos = new DataSet();
                query.Fill(datos);
                List<AcuerdosTO> lista = new List<AcuerdosTO>();
                DataTableReader tabla = datos.Tables[0].CreateDataReader();
                conexion.Close();
                //foreach (DataRow fila in tabla.Rows)
                while(tabla.Read())
                {
                    AcuerdosTO acuerdoActual = new AcuerdosTO();
                    acuerdoActual.setId("" + tabla["id"]);
                    acuerdoActual.setParteT("" + tabla["ParteT"]);
                    acuerdoActual.setConsec("" + tabla["Consec"]);
                    acuerdoActual.setTesis("" + tabla["Tesis"]);
                    acuerdoActual.setSala("" + tabla["sala"]);
                    acuerdoActual.setEpoca("" + tabla["epoca"]);
                    String subVolumen = DBNull.Value.Equals(tabla["subvolumen"]) 
                        || ((String)tabla["subvolumen"]).Trim().Equals(String.Empty)
                        ? String.Empty 
                        : ", " + tabla["subvolumen"];
                    acuerdoActual.setVolumen("" + tabla["volumen"]+ subVolumen);
                    acuerdoActual.setFuente("" + tabla["Fuente"]);
                    acuerdoActual.setPagina("" + tabla["Pagina"]);
                    acuerdoActual.setRubro("" + tabla["Rubro"]);
                    acuerdoActual.setVolOrden("" + tabla["VolOrden"]);
                    acuerdoActual.setConsecIndx("" + tabla["ConsecIndx"]);
                    acuerdoActual.setProcesado("" + tabla["Procesado"]);
                    acuerdoActual.setTpoAsunto("" + tabla["TpoAsunto"]);
                    acuerdoActual.setPromovente("" + tabla["Promovente"]);
                    acuerdoActual.setClasificacion("" + tabla["Clasificacion"]);
                    acuerdoActual.setComplemento("" + tabla["Complemento"]);
                    acuerdoActual.OrdenTema = (int)tabla["ordenTema"];
                    acuerdoActual.OrdenAcuerdo = (int)tabla["ordenAcuerdo"];
                    lista.Add(acuerdoActual);
                }
                //conexion.Close();
                return lista;
            }
            catch (Exception e)
            {
                conexion.Close(); 
                log.Debug(e.Message);
                if (!EventLog.SourceExists("IUS"))
                {
                    EventLog.CreateEventSource("IUS", "IUS");
                }
                EventLog Logg = new EventLog("IUS");
                Logg.Source = "IUS";
                String mensaje = "AcuerdoDAOImpl Exception at GetAcuerdos(MostrarPorIusTO) " + e.Message + e.StackTrace;
                Logg.WriteEntry(mensaje);
                Logg.Close();
                return new List<AcuerdosTO>();
            }
        }

        public mx.gob.scjn.ius_common.TO.AcuerdosTO getAcuerdo(int id)
        {
            DbConnection conexion = contextoBD.ContextConectionEVA;
            try
            {
                String select = "select A.ParteT,        A.Consec,     A.Id,        A.Tesis,     C.descInst As sala," +
                                "       B.descEpocaAbr As epoca,     D.txtVolumen As volumen,    E.DescFteAbr As Fuente,    A.Pagina,    A.Rubro," +
                                "       D.Orden As VolOrden,      A.ConsecIndx, A.Procesado, A.TpoAsunto, A.Promovente,"+
                         " A.Clasificacion, A.Complemento, A.OrdenAcuerdo, A.OrdenTema, F.txtvolumen as subvolumen "+
                         "  from Acuerdo As A, cepocas As B, cinsts As C, volumen As D, cfuentes As E, subVolumen as F " +
                         "                     where A.id = "+id+
                         "                       AND A.epoca = B.idEpoca"+
                         "                       AND A.sala = C.idInst "+
                         "                       AND A.volumen = D.volumen"+
                         "                       AND A.fuente = E.idfte" +
                         "                       AND A.idSubVolumen = F.idSubvolumen";
                DataAdapter query = contextoBD.dataAdapter(select,conexion);
                DataSet datos = new DataSet();
                query.Fill(datos);
                List<AcuerdosTO> lista = new List<AcuerdosTO>();
                DataTableReader tabla = datos.Tables[0].CreateDataReader();
                conexion.Close();
                //foreach (DataRow fila in tabla.Rows)
                while(tabla.Read())
                {
                    AcuerdosTO acuerdoActual = new AcuerdosTO();
                    acuerdoActual.setId("" + tabla["id"]);
                    acuerdoActual.setParteT("" + tabla["ParteT"]);
                    acuerdoActual.setConsec("" + tabla["Consec"]);
                    acuerdoActual.setTesis("" + tabla["Tesis"]);
                    acuerdoActual.setSala("" + tabla["sala"]);
                    acuerdoActual.setEpoca("" + tabla["epoca"]);
                    String subVolumen = DBNull.Value.Equals(tabla["subvolumen"])
                        || ((String)tabla["subVolumen"]).Equals(String.Empty)
                        ? String.Empty
                        : ", " + tabla["subVolumen"];
                    acuerdoActual.setVolumen("" + tabla["volumen"]+ subVolumen);
                    acuerdoActual.setFuente("" + tabla["Fuente"]);
                    acuerdoActual.setPagina("" + tabla["Pagina"]);
                    acuerdoActual.setRubro("" + tabla["Rubro"]);
                    acuerdoActual.setVolOrden("" + tabla["VolOrden"]);
                    acuerdoActual.setConsecIndx("" + tabla["ConsecIndx"]);
                    acuerdoActual.setProcesado("" + tabla["Procesado"]);
                    acuerdoActual.setTpoAsunto("" + tabla["TpoAsunto"]);
                    acuerdoActual.setPromovente("" + tabla["Promovente"]);
                    acuerdoActual.setClasificacion("" + tabla["Clasificacion"]);
                    acuerdoActual.setComplemento("" + tabla["Complemento"]);
                    acuerdoActual.OrdenTema = (int)tabla["ordenTema"];
                    acuerdoActual.OrdenAcuerdo = (int)tabla["ordenAcuerdo"];
                    lista.Add(acuerdoActual);
                }
                //conexion.Close();
                return lista[0];
            }
            catch (Exception e)
            {
                conexion.Close();
                log.Debug(e.Message);
                if (!EventLog.SourceExists("IUS"))
                {
                    EventLog.CreateEventSource("IUS", "IUS");
                }
                EventLog Logg = new EventLog("IUS");
                Logg.Source = "IUS";
                String mensaje = "AcuerdoDAOImpl Exception at GetAcuerdo(int) \n" + e.Message + e.StackTrace;
                Logg.WriteEntry(mensaje);
                Logg.Close();
                return new AcuerdosTO();
            }
        }

        public List<mx.gob.scjn.ius_common.TO.AcuerdosPartesTO> getAcuerdosPartes(mx.gob.scjn.ius_common.TO.MostrarPartesIdTO parametros)
        {
            DbConnection conexion = contextoBD.ContextConectionEVA;
            try
            {
                String select = "select Id, Parte, txtParte " +
                                "  from parteacuerdo where id = "+ parametros.getId()+
                                " order by "+parametros.getOrderBy()+" "+parametros.getOrderType();
                DataAdapter query = contextoBD.dataAdapter
                                (select,conexion);
                DataSet datos = new DataSet();
                query.Fill(datos);
                List<AcuerdosPartesTO> lista = new List<AcuerdosPartesTO>();
                DataTableReader tabla = datos.Tables[0].CreateDataReader();
                conexion.Close();
                //foreach (DataRow fila in tabla.Rows)
                while(tabla.Read())
                {
                    AcuerdosPartesTO acuerdoActual = new AcuerdosPartesTO();
                    acuerdoActual.setId((int)tabla["id"]);
                    acuerdoActual.setParte((byte)tabla["Parte"]);
                    acuerdoActual.setTxtParte("" + tabla["txtParte"]);
                    lista.Add(acuerdoActual);
                }
                //conexion.Close();
                return lista;
            }
            catch (Exception e)
            {
                conexion.Close();
                log.Debug(e.Message);
                if (!EventLog.SourceExists("IUS"))
                {
                    EventLog.CreateEventSource("IUS", "IUS");
                }
                EventLog Logg = new EventLog("IUS");
                Logg.Source = "IUS";
                String mensaje = "AcuerdoDAOImpl Exception at GetAcuerdosPartes(MostrarPartesIdTO)\n" + e.Message + e.StackTrace;
                Logg.WriteEntry(mensaje);
                Logg.Close();
                return new List<AcuerdosPartesTO>();
            }
        }

        #endregion
        private string obtenQueryPalabras(BusquedaPalabraTO palabras, String queryEpocas)
        {
            if (palabras.Jurisprudencia != IUSConstants.BUSQUEDA_PALABRA_ALMACENADA)
            {
                String queryActual = "";
                String valorLogico = "";
                switch (palabras.ValorLogico)
                {
                    case IUSConstants.BUSQUEDA_PALABRA_OP_Y:
                        valorLogico = " AND ";// BooleanClause.Occur.MUST;
                        break;
                    case IUSConstants.BUSQUEDA_PALABRA_OP_O:
                        valorLogico = " OR ";//BooleanClause.Occur.SHOULD;
                        break;
                    case IUSConstants.BUSQUEDA_PALABRA_OP_NO:
                        valorLogico = " NOT ";//BooleanClause.Occur.MUST_NOT;
                        break;
                    default:
                        valorLogico = " AND ";//BooleanClause.Occur.MUST;
                        break;
                }
                queryActual = "(" + queryEpocas;
                if (!valorLogico.Equals(" NOT "))
                {
                    switch (palabras.Jurisprudencia)
                    {
                        case IUSConstants.BUSQUEDA_PALABRA_AMBAS:
                            break;
                        case IUSConstants.BUSQUEDA_PALABRA_JURIS:
                            queryActual = queryActual + " AND (ta_tj: 1)";
                            break;
                        case IUSConstants.BUSQUEDA_PALABRA_TESIS:
                            queryActual = queryActual + " AND (ta_tj: 0)";
                            break;
                        default:
                            break;
                    }
                }
                String queryPalabrasFrases = "";
                String prefijo = "AND (";
                String[] palabrasExpresion = palabras.Expresion.Split(IUSConstants.SEPARADOR_FRASES, StringSplitOptions.RemoveEmptyEntries);
                List<String> losEsperados = new List<string>();
                foreach (String palabraExpresionActual in palabrasExpresion)
                {

                    foreach (String itemTotal in ObtenQuery(palabraExpresionActual, palabras))
                    {
                        losEsperados.Add(itemTotal);
                    }
                    losEsperados.Add(" AND ");
                }
                losEsperados.RemoveAt(losEsperados.Count - 1);
                bool iniciaparenteis = true;
                foreach (String itemEsperado in losEsperados)
                {
                    String queryPalabrasFrasesTemp = "(";
                    switch (itemEsperado.Trim())
                    {
                        case "AND":
                            queryPalabrasFrasesTemp = " AND ";
                            queryPalabrasFrases += itemEsperado;
                            iniciaparenteis = false;
                            break;
                        case "OR":
                            queryPalabrasFrasesTemp = " OR ";
                            queryPalabrasFrases += itemEsperado;
                            iniciaparenteis = false;
                            break;
                        case "NOT":
                            queryPalabrasFrasesTemp = " NOT ";
                            queryPalabrasFrases += itemEsperado;
                            iniciaparenteis = false;
                            break;
                        default:
                            iniciaparenteis = true;
                            queryPalabrasFrasesTemp = queryPalabrasFrasesTemp + itemEsperado;
                            queryPalabrasFrases = queryPalabrasFrases + queryPalabrasFrasesTemp;
                            if (iniciaparenteis)
                            {
                                queryPalabrasFrases = "(" + queryPalabrasFrases;
                            }
                            queryPalabrasFrases = completaParentesis(queryPalabrasFrases);
                            break;
                    }

                }
                queryPalabrasFrases = prefijo + queryPalabrasFrases + ")";
                queryPalabrasFrases = completaParentesis(queryPalabrasFrases);
                queryActual = queryActual + queryPalabrasFrases + ")";
                return queryActual;
            }
            else
            {
                ///Se trata de una búsqueda Almacenada que hay que obtener para integrar su
                ///propio query
                IUSApplicationContext contexto = new IUSApplicationContext();
                GuardarExpresionDAO guardarExpresion = (GuardarExpresionDAO)contexto.GetObject("GuardarExpresionDAO");
                BusquedaAlmacenadaTO busquedaAlmacenada = guardarExpresion.ObtenBusqueda(palabras.Campos[0]);
                if (
                    (busquedaAlmacenada.TipoBusqueda == IUSConstants.BUSQUEDA_TESIS_SIMPLE) ||
                    (busquedaAlmacenada.TipoBusqueda == IUSConstants.BUSQUEDA_PALABRA_TESIS))
                {
                    queryEpocas = "(";
                    List<int> epocas = busquedaAlmacenada.Epocas.ToList();
                    foreach (int itemEpoca in epocas)
                    {
                        queryEpocas += itemEpoca + " OR ";
                    }
                    queryEpocas = queryEpocas.Substring(0, queryEpocas.Length - 4) + ")";
                    queryEpocas = "(parte: (" + queryEpocas + "))";
                    String query = "";
                    foreach (ExpresionBusqueda itemBusquedaA in busquedaAlmacenada.Expresiones)
                    {
                        BusquedaPalabraTO expresionAObtener = new BusquedaPalabraTO();
                        expresionAObtener.Campos = itemBusquedaA.Campos;
                        expresionAObtener.Expresion = itemBusquedaA.Expresion;
                        expresionAObtener.Jurisprudencia = itemBusquedaA.IsJuris;
                        expresionAObtener.ValorLogico = itemBusquedaA.Operador;
                        query = query + " " + obtenQueryPalabras(expresionAObtener, queryEpocas);
                    }
                    return query;
                }
                else
                {
                    return "(idProg: " + busquedaAlmacenada.Expresiones.ElementAt(0).Expresion + ")";
                }
                return "";
            }
        }

        private string completaParentesis(string queryPalabrasFrasesTemp)
        {
            String temporal = queryPalabrasFrasesTemp;
            int excedentes = 0;
            while (!temporal.Equals(""))
            {
                if (temporal.Contains('('))
                {
                    excedentes++;
                    temporal = temporal.Substring(temporal.IndexOf('(') + 1);
                }
                else
                {
                    temporal = "";
                }
            }
            temporal = queryPalabrasFrasesTemp;
            while (!temporal.Equals(""))
            {
                if (temporal.Contains(')'))
                {
                    excedentes--;
                    temporal = temporal.Substring(temporal.IndexOf(')') + 1);
                }
                else
                {
                    temporal = "";
                }
            }
            for (int anadido = 0; anadido < excedentes; anadido++)
            {
                queryPalabrasFrasesTemp += ")";
            }
            return queryPalabrasFrasesTemp;
        }

        private List<string> ObtenQuery(String p, BusquedaPalabraTO campos)
        {
            List<String> listaResultado = new List<string>();
            List<String> resultado = new List<string>();// obtenPalabras(p);
            String parametro = p;
            parametro = parametro.Trim();
            bool siguientePuesto = false;
            while (!parametro.Equals(""))
            {
                String siguiente = generaTokenPalabraFrase(parametro);
                if (siguiente.Equals(""))
                {
                    parametro = "";
                }
                if (parametro.Substring(0, 1).Equals("\""))//Frase
                {
                    siguiente = "\"" + siguiente + "\"";
                }
                parametro = parametro.Substring(siguiente.Length).Trim();
                switch (siguiente.Trim())
                {
                    case "Y":
                        siguiente = " AND ";
                        break;
                    case "O":
                        siguiente = " OR ";
                        break;
                    case "N":
                        siguiente = " NOT ";
                        break;
                }
                resultado.Add(siguiente);
            }
            String regreso = "";
            foreach (String item in resultado)
            {
                if ((!item.Equals(" NOT ")) && (!item.Equals(" AND "))
                    && (!item.Equals(" OR ")))
                {
                    if (regreso.Equals(""))
                    {
                        regreso = "(" + regreso + "( ";
                        if (campos.Campos.Contains(IUSConstants.BUSQUEDA_PALABRA_CAMPO_TEXTO))
                        {
                            regreso = regreso + "texto:(" + item + ")";
                        }
                        if (campos.Campos.Contains(IUSConstants.BUSQUEDA_PALABRA_CAMPO_TEMA))
                        {
                            if (regreso.EndsWith(")"))
                            {
                                regreso = regreso + " OR rubro:(" + item + ")";
                            }
                            else
                            {
                                regreso = regreso + "rubro:(" + item + ")";
                            }
                        }
                        if (campos.Campos.Contains(IUSConstants.BUSQUEDA_PALABRA_CAMPO_LOC))
                        {
                            if (regreso.EndsWith(")"))
                            {
                                regreso = regreso + " OR locAbr:(" + item + ")";
                            }
                            else
                            {
                                regreso = regreso + "locAbr:(" + item + ")";
                            }
                        }
                        //regreso += " AND ";
                    }
                    else
                    {
                        regreso = "(" + regreso + "(";
                        if (campos.Campos.Contains(IUSConstants.BUSQUEDA_PALABRA_CAMPO_TEXTO))
                        {
                            regreso = regreso + "texto:(" + item + ")";
                        }
                        if (campos.Campos.Contains(IUSConstants.BUSQUEDA_PALABRA_CAMPO_TEMA))
                        {
                            if (regreso.EndsWith(")"))
                            {
                                regreso = regreso + " OR rubro:(" + item + ")";
                            }
                            else
                            {
                                regreso = regreso + "rubro:(" + item + ")";
                            }
                        }
                        if (campos.Campos.Contains(IUSConstants.BUSQUEDA_PALABRA_CAMPO_LOC))
                        {
                            if (regreso.EndsWith(")"))
                            {
                                regreso = regreso + " OR locAbr:(" + item + ")";
                            }
                            else
                            {
                                regreso = regreso + "locAbr:(" + item + ")";
                            }
                        }
                        regreso += "))";
                    }
                    if (regreso.Length > 5)
                    {
                        if (regreso.Substring(regreso.Length - 5).Equals(" AND "))
                        {
                            regreso = regreso.Substring(0, regreso.Length - 5);
                        }
                    }

                    listaResultado.Add(regreso);
                    regreso = "";
                }
                else
                {
                    listaResultado.Add(item);
                }
            }
            return listaResultado;
        }

        private String generaTokenPalabraFrase(string p)
        {
            String resultado = null;
            //Verificar si lo que sigue es frase.
            if (p.Substring(0, 1).Equals("\""))
            {
                resultado = p.Substring(1);
                resultado = resultado.Substring(0, resultado.IndexOf('"'));
                return resultado;
            }
            else
            {
                //es una palabra.
                char[] caracterBlanco = { ' ' };
                resultado = FuncionesGenerales.Normaliza(p.Split(caracterBlanco)[0]);
                //resultado = p.Split(caracterBlanco)[0].ToUpper();
                return resultado.Trim();
            }
        }

        private List<String> obtenFrases(String cadenaActual)
        {
            if (!cadenaActual.Contains("\""))
            {
                return new List<String>();
            }
            int comillaInicial = 0;
            List<String> resultado = new List<string>();
            while (comillaInicial < 2)
            {
                if (comillaInicial == 0)
                {
                    int posicionComilla = cadenaActual.IndexOf('"');
                    String anteriorAComilla = cadenaActual.Substring(posicionComilla + 1, cadenaActual.Length - (posicionComilla + 1));
                    cadenaActual = anteriorAComilla;
                    posicionComilla = cadenaActual.IndexOf('"');
                    anteriorAComilla = cadenaActual.Substring(0, posicionComilla);
                    resultado.Add(anteriorAComilla);
                    cadenaActual = cadenaActual.Substring(posicionComilla + 1);
                    posicionComilla = cadenaActual.IndexOf('"');
                    cadenaActual = cadenaActual.Substring(posicionComilla + 1);
                }
                //else
                //{
                //    cadenaActual = cadenaActual.Substring(posicionComilla);
                //    como
                //}
                if (!cadenaActual.Contains('"'))
                {
                    //resultado.Concat(cadenaActual.Split(separadores.ToCharArray()));
                    comillaInicial = 2;
                }
            }
            return resultado;
        }

        public static List<String> obtenPalabras(String cadenaActual)
        {
            String separadores = " ";
            if (!cadenaActual.Contains("\""))
            {
                return new List<String>(cadenaActual.Split(separadores.ToCharArray()));
            }
            int comillaInicial = 0;
            List<String> resultado = new List<string>();
            while (comillaInicial < 2)
            {
                if (comillaInicial == 0)
                {
                    int posicionComilla = cadenaActual.IndexOf('"');
                    String anteriorAComilla = cadenaActual.Substring(0, posicionComilla);
                    anteriorAComilla = anteriorAComilla.Trim();
                    resultado.Concat(anteriorAComilla.Split(separadores.ToCharArray()));
                    cadenaActual = cadenaActual.Substring(posicionComilla);
                    posicionComilla = cadenaActual.IndexOf('"');
                    cadenaActual = cadenaActual.Substring(posicionComilla + 1);
                }
                if (!cadenaActual.Contains('"'))
                {
                    cadenaActual = cadenaActual.Trim();
                    String[] parcial = cadenaActual.Split(separadores.ToCharArray());
                    foreach (String item in parcial)
                    {
                        if (!item.Equals(""))
                        {
                            String anadir = item;
                            if (anadir.ToUpper().Trim().Equals("Y"))
                            {
                                anadir = " AND ";
                            }
                            if (anadir.ToUpper().Trim().Equals("O"))
                            {
                                anadir = " OR ";
                            }
                            if (anadir.ToUpper().Trim().Equals("N"))
                            {
                                anadir = " NOT ";
                            }
                            resultado.Add(anadir);
                        }
                    }
                    comillaInicial = 2;
                    return resultado;
                }
            }
            return new List<string>();
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
                    if (busqueda.getEpocas()[recorridoLargo][recorridoAncho])
                    {
                        epocasSalas.Add(contador);
                    }
                    contador++;
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
                    if (busqueda.getApendices()[recorridoLargo][recorridoAncho])
                    {
                        epocasSalas.Add(contador);
                    }
                    contador++;
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
                    if (busqueda.getAcuerdos()[recorridoLargo][recorridoAncho])
                    {
                        epocasSalas.Add(contador);
                    }
                    contador++;
                }
            }
            return epocasSalas.ToArray();
        }
        /// <summary>
        /// Busca en la tabla de zTablasPartes y regresa las tablas relacionadas con cada acuerdo.
        /// </summary>
        /// <param name="Id">El identificador del acuerdo</param>
        /// <returns>Las tablas relacionadas con el.</returns>
        public List<TablaPartesTO> getTablas(string Id)
        {
            DbConnection conexion = contextoBD.ContextConection;
            try
            {
                DataAdapter query = contextoBD.dataAdapter("Select Idtpo, Id, Archivo, Posicion, Tamanio, Frase, Parte, PosicionParte " +
                    " FROM zTablasPartes where idTpo=4 AND id=" + Id, conexion);
                DataSet datos = new DataSet();
                query.Fill(datos);
                List<TablaPartesTO> lista = new List<TablaPartesTO>();
                DataTableReader tabla = datos.Tables[0].CreateDataReader();
                conexion.Close();
                //foreach (DataRow fila in tabla.Rows)
                while(tabla.Read())
                {
                    TablaPartesTO item = new TablaPartesTO();
                    item.IdTipo = (byte)tabla["idTpo"];
                    item.Id = (int)tabla["id"];
                    item.Archivo = "" + tabla["Archivo"];
                    item.Posicion = (int)tabla["posicion"];
                    item.Tamanio = (short)tabla["tamanio"];
                    item.Frase = "" + tabla["frase"];
                    item.Parte = (byte)tabla["Parte"];
                    item.PosicionParte = (int)tabla["PosicionParte"];
                    lista.Add(item);
                }
                //conexion.Close();
                return lista;
            }
            catch (Exception e)
            {
                conexion.Close();
                log.Debug(e.Message);
                if (!EventLog.SourceExists("IUS"))
                {
                    EventLog.CreateEventSource("IUS", "IUS");
                }
                EventLog Logg = new EventLog("IUS");
                Logg.Source = "IUS";
                String mensaje = "AcuerdoDAOImpl Exception at GetTablas(String) \n" + e.Message + e.StackTrace;
                Logg.WriteEntry(mensaje);
                Logg.Close();
                return new List<TablaPartesTO>();
            }
        }

    }
}
