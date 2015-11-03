using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Odbc;
using mx.gob.scjn.ius_common.TO;
using mx.gob.scjn.ius_common.context;
using System.Data.Common;
using Lucene.Net.Search;
using Lucene.Net.QueryParsers;
using mx.gob.scjn.ius_common.utils;
using Lucene.Net.Documents;
using log4net;
using System.Diagnostics;
namespace mx.gob.scjn.ius_common.DAO.impl
{
    class EjecutoriasDAOImpl : EjecutoriasDAO
    {
        public DBContext contextoBD;
        private ILog log = LogManager.GetLogger("mx.gob.scjn.ius_common.DAO.impl.EjecutoriasDAOImpl");
        public EjecutoriasDAOImpl()
        {
            IUSApplicationContext contexto = new IUSApplicationContext();
            contextoBD = (DBContext)contexto.getInitialContext().GetObject("dataSource");
        }
        #region EjecutoriasDAO Members

        public List<mx.gob.scjn.ius_common.TO.EjecutoriasTO> getAll()
        {
            DbConnection conexion = contextoBD.ContextConectionEVA;
            try
            {
                DataAdapter query = contextoBD.dataAdapter("Select * from ejecutorias", conexion);
                DataSet datos = new DataSet();
                query.Fill(datos);
                List<EjecutoriasTO> lista = new List<EjecutoriasTO>();
                DataTableReader tabla = datos.Tables["ejecutorias"].CreateDataReader();
                conexion.Close();
                //foreach(DataRow fila in tabla.Rows)
                while (tabla.Read())
                {
                    EjecutoriasTO ejecutoriaActual = new EjecutoriasTO();
                    ejecutoriaActual.setConsec((String)tabla["ConsecIndx"]);
                    ejecutoriaActual.setId((String)tabla["id"]);
                    lista.Add(ejecutoriaActual);
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
                String mensaje = "EjecutoriasDAOImpl Exception at GetAll()\n" + e.Message + e.StackTrace;
                Logg.WriteEntry(mensaje);
                Logg.Close();
                return new List<EjecutoriasTO>();
            }
        }

        public List<mx.gob.scjn.ius_common.TO.EjecutoriasTO> getEjecutorias(mx.gob.scjn.ius_common.TO.PartesTO busqueda)
        {
            DbConnection conexion = contextoBD.ContextConectionEVA;
            try
            {
                String queryString = "select id, consecIndx, tpoAsunto, epoca, sala, fuente, " +
                                            "volumen, pagina ,promovente, OrdenPromovente, OrdenAsunto " +
                                            " from ejecutoria " +
                                  "where id in(Select distinct(id) from ejecutoria where parteT = " +
                                  busqueda.getParte() + ")  order by " + busqueda.getOrderBy() + " " + busqueda.getOrderType();
                DataAdapter query = contextoBD.dataAdapter(queryString, conexion);
                DataSet datos = new DataSet();
                query.Fill(datos);
                List<EjecutoriasTO> lista = new List<EjecutoriasTO>();
                DataTableReader tabla = datos.Tables[0].CreateDataReader();
                conexion.Close();
                //foreach (DataRow fila in tabla.Rows)
                while (tabla.Read())
                {
                    EjecutoriasTO ejecutoriaActual = new EjecutoriasTO();
                    //ejecutoriaActual.setConsec((String)fila["ConsecIndx"]);
                    ejecutoriaActual.setId("" + tabla["id"]);
                    ejecutoriaActual.setConsecIndx("" + tabla["consecIndx"]);
                    ejecutoriaActual.setTpoAsunto("" + tabla["tpoAsunto"]);
                    ejecutoriaActual.setPromovente("" + tabla["promovente"]);
                    ejecutoriaActual.Epoca = "" + tabla["epoca"];
                    ejecutoriaActual.Sala = "" + tabla["sala"];

                    ejecutoriaActual.Fuente = "" + tabla["fuente"];
                    ejecutoriaActual.Volumen = "" + tabla["volumen"];
                    ejecutoriaActual.Pagina = "" + tabla["pagina"];
                    ejecutoriaActual.OrdenarAsunto = (int)tabla["OrdenAsunto"];
                    ejecutoriaActual.OrdenarPromovente = (int)tabla["OrdenPromovente"];
                    lista.Add(ejecutoriaActual);
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
                String mensaje = "EjecutoriasDAOImpl Exception at GetEjecutorias(PartesTO)\n" + e.Message + e.StackTrace;
                Logg.WriteEntry(mensaje);
                Logg.Close();
                return new List<EjecutoriasTO>();
            }
        }

        public List<mx.gob.scjn.ius_common.TO.EjecutoriasTO> getEjecutorias(int[] partes)
        {
            throw new NotImplementedException();
        }

        public List<mx.gob.scjn.ius_common.TO.EjecutoriasTO> getEjecutorias(mx.gob.scjn.ius_common.TO.MostrarPorIusTO identificadores)
        {
            DbConnection conexion = contextoBD.ContextConectionEVA;
            try
            {
                DataSet datos = new DataSet();
                String select = "select A.ParteT,   A.Consec,   A.Id,        A.Tesis,        C.descInst As sala,    B.DescEpocaAbr As Epoca," +
                                          "        E.txtVolumen As volumen, F.txtvolumen As subvolumen, D.DescFte As fuente,   A.Pagina,    A.Rubro,        E.Orden As VolOrden,         A.ConsecIndx," +
                                          "        A.Procesado,A.TpoAsunto,A.Promovente,A.Clasificacion,A.Complemento, A.OrdenPromovente, A.OrdenAsunto " +
                                          "  from Ejecutoria As A,"+
                                          "       cepocas As B, cinsts As C," +
                                          "       cfuentes As D, volumen As E, subvolumen as F " +
                                          "   where  " +
                                          "        A.epoca = B.idEpoca " +
                                          "    AND A.sala = C.idInst" +
                                          "    AND A.fuente = D.idFte" +
                                          "    AND A.volumen = E.volumen" +
                                          "    AND A.idSubVolumen = F.idSubvolumen" +
                                          "    AND A.id IN(";
                foreach (int item in identificadores.Listado)
                {
                    select += item + ",";

                }
                select = select.Substring(0, select.Length - 1);
                select += ") ";
                select += "order by " + identificadores.getOrderBy() + " " +
                    identificadores.getOrderType();
                DataAdapter query = contextoBD.dataAdapter(select, conexion);
                query.Fill(datos);
                List<EjecutoriasTO> lista = new List<EjecutoriasTO>();
                DataTableReader tabla = datos.Tables[0].CreateDataReader();
                conexion.Close();
                //foreach (DataRow fila in tabla.Rows)
                while (tabla.Read())
                {
                    EjecutoriasTO ejecutoriaActual = new EjecutoriasTO();
                    ejecutoriaActual.setClasificacion("" + tabla["clasificacion"]);
                    ejecutoriaActual.setId("" + tabla["id"]);
                    ejecutoriaActual.setComplemento("" + tabla["complemento"]);
                    ejecutoriaActual.setConsec("" + tabla["consec"]);
                    ejecutoriaActual.setConsecIndx("" + tabla["consecIndx"]);
                    ejecutoriaActual.setEpoca("" + tabla["epoca"]);
                    ejecutoriaActual.setFuente("" + tabla["fuente"]);
                    ejecutoriaActual.setPagina("" + tabla["pagina"]);
                    ejecutoriaActual.setParteT("" + tabla["ParteT"]);
                    ejecutoriaActual.setProcesado("" + tabla["Procesado"]);
                    ejecutoriaActual.setPromovente("" + tabla["Promovente"]);
                    ejecutoriaActual.setRubro("" + tabla["Rubro"]);
                    ejecutoriaActual.setSala("" + tabla["sala"]);
                    ejecutoriaActual.setTesis("" + tabla["tesis"]);
                    ejecutoriaActual.setTpoAsunto("" + tabla["TpoAsunto"]);
                    ejecutoriaActual.setVolOrden("" + tabla["volOrden"]);
                    String subVolumen = DBNull.Value.Equals(tabla["subvolumen"])
                        || ((String)tabla["subvolumen"]).Trim().Equals(String.Empty)
                        ? String.Empty
                        : ", " + tabla["subvolumen"];
                    ejecutoriaActual.setVolumen("" + tabla["volumen"]+ subVolumen);
                    ejecutoriaActual.OrdenarAsunto = (int)tabla["OrdenAsunto"];
                    ejecutoriaActual.OrdenarPromovente = (int)tabla["OrdenPromovente"];
                    lista.Add(ejecutoriaActual);
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
                String mensaje = "EjecutoriasDAOImpl Exception at GetEjecutorias(MostrarPorIUSTO)\n" + e.Message + e.StackTrace;
                Logg.WriteEntry(mensaje);
                Logg.Close();

                return new List<EjecutoriasTO>();
            }
        }

        public List<mx.gob.scjn.ius_common.TO.EjecutoriasPartesTO> getPartesPorId(mx.gob.scjn.ius_common.TO.MostrarPartesIdTO busqueda)
        {
            DbConnection conexion = contextoBD.ContextConectionEVA;
            try
            {
                DataAdapter query = contextoBD.dataAdapter("select Id, Parte, TxtParte " +
                                                             "from parteejecutoria where id = " + busqueda.getId() +
                                                             " order by " + busqueda.getOrderBy() + " " + busqueda.getOrderType(), conexion);
                DataSet datos = new DataSet();
                query.Fill(datos);
                List<EjecutoriasPartesTO> lista = new List<EjecutoriasPartesTO>();
                DataTableReader tabla = datos.Tables[0].CreateDataReader();

                //foreach (DataRow fila in tabla.Rows)
                while (tabla.Read())
                {
                    EjecutoriasPartesTO parteActual = new EjecutoriasPartesTO();
                    parteActual.setId((int)tabla["id"]);
                    parteActual.setParte((byte)tabla["Parte"]);
                    parteActual.setTxtParte((String)tabla["txtParte"]);
                    lista.Add(parteActual);
                }
                //conexion.Close();

                return lista;
            }
            catch (Exception e)
            {
                conexion.Close();
                if (!EventLog.SourceExists("IUS"))
                {
                    EventLog.CreateEventSource("IUS", "IUS");
                }
                EventLog Logg = new EventLog("IUS");
                Logg.Source = "IUS";
                String mensaje = "EjecutoriasDAOImpl Exception at GetPartesPorOd(MostrarPartesId)\n" + e.Message + e.StackTrace;
                Logg.WriteEntry(mensaje);
                Logg.Close();
                return new List<EjecutoriasPartesTO>();
            }
        }

        public List<mx.gob.scjn.ius_common.TO.EjecutoriasTO> getPorPalabra(string palabras)
        {
            throw new NotImplementedException();
        }

        public mx.gob.scjn.ius_common.TO.EjecutoriasTO getEjecutoriaPorId(int id)
        {
            DbConnection conexion = contextoBD.ContextConectionEVA;
            if (conexion.State != ConnectionState.Open) conexion.Open();
            try
            {
                String SqlString = "select A.ParteT,   A.Consec,   A.Id,        A.Tesis,        C.descInst As sala,    B.DescEpocaAbr As Epoca," +
                                   "               E.txtVolumen As volumen,  D.DescFte As fuente,   A.Pagina,    A.Rubro, " +
                                   "               E.Orden As VolOrden,         A.ConsecIndx, F.txtVolumen as Subvolumen, " +
                                   "               A.Procesado, A.TpoAsunto, A.Promovente, A.Clasificacion, A.Complemento, A.OrdenAsunto, A.OrdenPromovente" +
                                   "             from Ejecutoria As A,  cepocas As B, cinsts As C, cfuentes As D, volumen As E, subVolumen as F where  a.id =" + id +
                                   "                                                            AND A.epoca = B.idEpoca " +
                                   "                                                            AND A.sala = C.idInst" +
                                   "                                                            AND A.fuente = D.idFte" +
                                   "                                                            AND A.IDSUBVOLUMEN= F.IDSUBVOLUMEN " +
                                   "                                                            AND A.volumen = E.volumen     ";
                DataAdapter query = contextoBD.dataAdapter(SqlString, conexion);
                DataSet datos = new DataSet();

                query.Fill(datos);
                List<EjecutoriasTO> lista = new List<EjecutoriasTO>();
                //DataTable tablaCompleta = datos.Tables[0];
                DataTableReader tabla = null;
                DataTable tablaOriginal = datos.Tables[0];
                bool hecho = false;
                bool huboExcepcion = false;
                while (!hecho)
                {
                    try
                    {
                        tabla = tablaOriginal.CreateDataReader();

                        hecho = true;
                    }
                    catch (Exception e)
                    {
                        huboExcepcion = true;
                        hecho = true;
                    }
                }
                //foreach (DataRow fila in tablaCompleta.Rows)
                if (huboExcepcion)
                {
                    foreach (DataRow fila in tablaOriginal.Rows)
                    {
                        EjecutoriasTO ejecutoriaActual = new EjecutoriasTO();
                        ejecutoriaActual.setClasificacion("" + fila["clasificacion"]);
                        ejecutoriaActual.setId("" + fila["id"]);
                        ejecutoriaActual.setComplemento("" + fila["complemento"]);
                        ejecutoriaActual.setConsec("" + fila["consec"]);
                        ejecutoriaActual.setConsecIndx("" + fila["consecIndx"]);
                        ejecutoriaActual.setEpoca("" + fila["epoca"]);
                        ejecutoriaActual.setFuente("" + fila["fuente"]);
                        ejecutoriaActual.setPagina("" + fila["pagina"]);
                        ejecutoriaActual.setParteT("" + fila["ParteT"]);
                        ejecutoriaActual.setProcesado("" + fila["Procesado"]);
                        ejecutoriaActual.setPromovente("" + fila["Promovente"]);
                        ejecutoriaActual.setRubro("" + fila["Rubro"]);
                        ejecutoriaActual.setSala("" + fila["sala"]);
                        ejecutoriaActual.setTesis("" + fila["tesis"]);
                        ejecutoriaActual.setTpoAsunto("" + fila["TpoAsunto"]);
                        ejecutoriaActual.setVolOrden("" + fila["volOrden"]);
                        String subVolumen = DBNull.Value.Equals(tabla["subvolumen"])
                        || ((String)tabla["subvolumen"]).Trim().Equals(String.Empty)
                        ? String.Empty
                        : ", " + tabla["subvolumen"];
                        ejecutoriaActual.setVolumen("" + fila["volumen"] + subVolumen);
                        ejecutoriaActual.OrdenarAsunto = (int)fila["OrdenAsunto"];
                        ejecutoriaActual.OrdenarPromovente = (int)fila["OrdenPromovente"];
                        lista.Add(ejecutoriaActual);
                    }
                }
                else
                {
                    while (tabla.Read())
                    {
                        EjecutoriasTO ejecutoriaActual = new EjecutoriasTO();
                        ejecutoriaActual.setClasificacion("" + tabla["clasificacion"]);
                        ejecutoriaActual.setId("" + tabla["id"]);
                        ejecutoriaActual.setComplemento("" + tabla["complemento"]);
                        ejecutoriaActual.setConsec("" + tabla["consec"]);
                        ejecutoriaActual.setConsecIndx("" + tabla["consecIndx"]);
                        ejecutoriaActual.setEpoca("" + tabla["epoca"]);
                        ejecutoriaActual.setFuente("" + tabla["fuente"]);
                        ejecutoriaActual.setPagina("" + tabla["pagina"]);
                        ejecutoriaActual.setParteT("" + tabla["ParteT"]);
                        ejecutoriaActual.setProcesado("" + tabla["Procesado"]);
                        ejecutoriaActual.setPromovente("" + tabla["Promovente"]);
                        ejecutoriaActual.setRubro("" + tabla["Rubro"]);
                        ejecutoriaActual.setSala("" + tabla["sala"]);
                        ejecutoriaActual.setTesis("" + tabla["tesis"]);
                        ejecutoriaActual.setTpoAsunto("" + tabla["TpoAsunto"]);
                        ejecutoriaActual.setVolOrden("" + tabla["volOrden"]);
                        String subVolumen = DBNull.Value.Equals(tabla["subvolumen"])
                        || ((String)tabla["subvolumen"]).Trim().Equals(String.Empty)
                        ? String.Empty
                        : ", " + tabla["subvolumen"];
                        ejecutoriaActual.setVolumen("" + tabla["volumen"] + subVolumen);
                        ejecutoriaActual.OrdenarAsunto = (int)tabla["OrdenAsunto"];
                        ejecutoriaActual.OrdenarPromovente = (int)tabla["OrdenPromovente"];
                        lista.Add(ejecutoriaActual);
                    }
                    tabla.Close();
                }
                conexion.Close();
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
                String mensaje = "EjecutoriasDAOImpl Exception at GetEjecutoriaPorId(int)\n" + e.Message + e.StackTrace;
                Logg.WriteEntry(mensaje);
                Logg.Close();
                return new EjecutoriasTO();
            }
        }

        public List<mx.gob.scjn.ius_common.TO.RelDocumentoTesisTO> getRelTesis(string id)
        {
            DbConnection conexion = contextoBD.ContextConection;
            try
            {
                DataAdapter query = contextoBD.dataAdapter("select ius, idPte, cve from relPartes where idPte=" +
                    id + " and cve=2", conexion);
                DataSet datos = new DataSet();
                query.Fill(datos);
                List<RelDocumentoTesisTO> lista = new List<RelDocumentoTesisTO>();
                DataTableReader tabla = datos.Tables[0].CreateDataReader();
                conexion.Close();
                //foreach (DataRow fila in tabla.Rows)
                while (tabla.Read())
                {
                    RelDocumentoTesisTO relacion = new RelDocumentoTesisTO();
                    relacion.Ius = "" + tabla["ius"];
                    relacion.Id = "" + tabla["idPte"];
                    relacion.TpoRel = "" + tabla["cve"];
                    lista.Add(relacion);
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
                String mensaje = "EjecutoriasDAOImpl Exception at GetPelTesis(String)\n" + e.Message + e.StackTrace;
                Logg.WriteEntry(mensaje);
                Logg.Close();
                return new List<RelDocumentoTesisTO>();
            }
        }

        public List<mx.gob.scjn.ius_common.TO.RelVotoEjecutoriaTO> getRelVotoEjecutoria(string id)
        {
            DbConnection conexion = contextoBD.ContextConection;
            try
            {
                DataAdapter query = contextoBD.dataAdapter("select idVoto, idEjecutoria " +
                                                            " from rrrrrrelvotoejecutoria" +
                                                            " where idEjecutoria=" + id, conexion);
                DataSet datos = new DataSet();
                query.Fill(datos);
                List<RelVotoEjecutoriaTO> lista = new List<RelVotoEjecutoriaTO>();
                DataTableReader tabla = datos.Tables[0].CreateDataReader();
                conexion.Close();
                //foreach (DataRow fila in tabla.Rows)
                while (tabla.Read())
                {
                    RelVotoEjecutoriaTO relacion = new RelVotoEjecutoriaTO();
                    relacion.Ejecutoria = "" + tabla["idEjecutoria"];
                    relacion.Voto = "" + tabla["idVoto"];
                    lista.Add(relacion);
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
                String mensaje = "EjecutoriasDAOImpl Exception at GetRelVotoEjecutoria(String)\n" + e.Message + e.StackTrace;
                Logg.WriteEntry(mensaje);
                Logg.Close();
                return new List<RelVotoEjecutoriaTO>();
            }
        }

        public List<TablaPartesTO> getTablas(string Id)
        {
            DbConnection conexion = contextoBD.ContextConection;
            try
            {
                String SqlQuery = "Select Idtpo, Id, Archivo, Posicion, Tamanio, Frase, Parte, PosicionParte " +
                    " FROM zTablasPartes where (IdTpo IN (2,22,7,6) ) AND id=" + Id + " order by posicion";
                DataAdapter query = contextoBD.dataAdapter(SqlQuery, conexion);
                DataSet datos = new DataSet();
                query.Fill(datos);
                List<TablaPartesTO> lista = new List<TablaPartesTO>();
                DataTableReader tabla = datos.Tables[0].CreateDataReader();

                //foreach (DataRow fila in tabla.Rows)
                while (tabla.Read())
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
                conexion.Close();
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
                String mensaje = "EjecutoriasDAOImpl Exception at GetTablas(String)\n" + e.Message + e.StackTrace;
                Logg.WriteEntry(mensaje);
                Logg.Close();
                return new List<TablaPartesTO>();
            }
        }
        /// <summary>
        /// Obtiene el resultado de la búsqueda por palabras
        /// </summary>
        /// <param name="parametros">Los parámetros de la búsqueda</param>
        /// <returns></returns>
        public List<EjecutoriasTO> getEjecutoriasPorPalabra(BusquedaTO parametros)
        {
            SpanishAnalyzer analyzer = new SpanishAnalyzer();
            List<String> querysEscritos = new List<string>();
            String queryEpocas = "";
            BooleanQuery QueryCompleto = new BooleanQuery();
            BooleanQuery queryGlobal = new BooleanQuery();
            IndexSearcher searcher = new IndexSearcher(IUSConstants.DIRECCION_INDEX_EJECUTORIAS);

            if ((parametros.OrdenarPor == null) || (parametros.OrdenarPor.Equals(IUSConstants.ORDER_DEFAULT)))
            {
                parametros.OrdenarPor = IUSConstants.ORDER_RUBRO;
            }
            String ordenarPor = parametros.OrdenarPor == null ? IUSConstants.ORDER_DEFAULT : parametros.OrdenarPor;
            List<EjecutoriasTO> resultado = new List<EjecutoriasTO>();
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
                EjecutoriasTO itemVerdadero = new EjecutoriasTO();
                itemVerdadero.Id = item.Get("id");
                itemVerdadero.OrdenarPromovente = int.Parse(item.Get("OrdenPromovente"));
                itemVerdadero.OrdenarAsunto = int.Parse(item.Get("OrdenAsunto"));
                itemVerdadero.ConsecIndx = item.Get("consecIndx");
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

            }
            return resultado;
        }

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
            //bool siguientePuesto = false;
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
                        if (campos.Campos.Contains(IUSConstants.BUSQUEDA_PALABRA_CAMPO_ASUNTO))
                        {
                            if (regreso.EndsWith(")"))
                            {
                                regreso = regreso + " OR asunto:(" + item + ")";
                            }
                            else
                            {
                                regreso = regreso + "asunto:(" + item + ")";
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
                        if (campos.Campos.Contains(IUSConstants.BUSQUEDA_PALABRA_CAMPO_ASUNTO))
                        {
                            if (regreso.EndsWith(")"))
                            {
                                regreso = regreso + " OR asunto:(" + item + ")";
                            }
                            else
                            {
                                regreso = regreso + "asunto:(" + item + ")";
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

        #endregion

        /// <summary>
        /// Genera los tokens de palabras y frases para los querys
        /// </summary>
        /// <param name="p">La expresión de la que se obtienen las palabras y querys</param>
        /// <returns>La expresión de palabras y frases</returns>
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
        /// <summary>
        /// Obtiene las frases que hay en una expresión dada
        /// </summary>
        /// <param name="cadenaActual"></param>
        /// <returns></returns>
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
        /// <returns> Las partes donde están las ejecutorias.</returns>
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
                    if (busqueda.getEpocas()[recorridoLargo][recorridoAncho])
                    {
                        epocasSalas.Add(contador);
                    }
                    contador++;
                }
            }
            return epocasSalas.ToArray();
        }

    }
}
