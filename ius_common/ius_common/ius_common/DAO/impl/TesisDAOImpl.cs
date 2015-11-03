using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Timers;
using Lucene.Net.Documents;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Microsoft.Win32;
using mx.gob.scjn.ius_common.TO;
using mx.gob.scjn.ius_common.context;
using mx.gob.scjn.ius_common.utils;

namespace mx.gob.scjn.ius_common.DAO.impl
{
    /// <summary>
    /// Implementa la interfaz de DAO para las tesis
    /// </summary>
    public class TesisDAOImpl : TesisDAO
    {
        /// <summary>
        ///     El contexto que maneja la Base de Datos.
        /// </summary>
        private DBContext contextoBD;
        private Timer workerBorraConsultas;
        //private static Dictionary<long, TesisTO> tesisCompletas;
        private static long ConsecIndx = 0;

        private static Dictionary<int, PaginadorTO> ConsultasActuales { get; set; }

        private static Dictionary<int, int> VolumenSeccion { get; set; }

        public TesisDAOImpl()
        {
            IUSApplicationContext contexto = new IUSApplicationContext();
            contextoBD = (DBContext)contexto.getInitialContext().GetObject("dataSource");
            ConsultasActuales = new Dictionary<int, PaginadorTO>();
            workerBorraConsultas = new Timer();
            workerBorraConsultas.Interval = 600000;
            workerBorraConsultas.Elapsed += worker_doWork;
            workerBorraConsultas.Start();
            //tesisCompletas = getAll();
        }

        /// <summary>
        /// Obtiene todos los registros de las tesis.
        /// </summary>
        public Dictionary<long, TesisTO> getAll()
        {
            TesisTO tesisActual = new TesisTO();
            DbConnection conexion = contextoBD.ContextConection;
            try
            {
                DataAdapter query = contextoBD.dataAdapter("Select " +
                                                           " IUS, ordenTesis, ordenRubro, OrdenInstancia, consecIndx, tpoTesis, " +
                                                           " TpoAsunto1, TpoAsunto2, TpoAsunto3, TpoAsunto4, TpoAsunto5, " +
                                                           " tpoPonente1, tpoPonente2, tpoPonente3, tpoPonente4, tpoPonente5 " +
                                                           " from tesis order by consecIndx", conexion);
                DataSet datos = new DataSet();
                query.Fill(datos);
                Dictionary<long, TesisTO> lista = new Dictionary<long, TesisTO>();
                DataTableReader tabla = datos.Tables[0].CreateDataReader();

                //foreach (DataRow fila in tabla.Rows)
                while (tabla.Read())
                {
                    tesisActual = new TesisTO();
                    tesisActual.ConsecIndxInt = (int)tabla["ConsecIndx"];
                    tesisActual.ConsecIndx = "" + tabla["ConsecIndx"];
                    tesisActual.setIus("" + tabla["ius"]);
                    tesisActual.TpoTesis = "" + tabla["tpoTesis"];
                    tesisActual.OrdenInstancia = (int)tabla["OrdenInstancia"];
                    tesisActual.OrdenRubro = (int)tabla["OrdenRubro"];
                    tesisActual.OrdenTesis = (int)tabla["OrdenTesis"];
                    tesisActual.Ponentes = new int[5];
                    tesisActual.Ponentes[0] = (short)tabla["tpoPonente1"];
                    tesisActual.Ponentes[1] = (short)tabla["tpoPonente2"];
                    tesisActual.Ponentes[2] = (short)tabla["tpoPonente3"];
                    tesisActual.Ponentes[3] = (short)tabla["tpoPonente4"];
                    tesisActual.Ponentes[4] = (short)tabla["tpoPonente5"];
                    tesisActual.TipoTesis = new int[5];
                    tesisActual.TipoTesis[0] = (short)tabla["TpoAsunto1"];
                    tesisActual.TipoTesis[1] = (short)tabla["TpoAsunto2"];
                    tesisActual.TipoTesis[2] = (short)tabla["TpoAsunto3"];
                    tesisActual.TipoTesis[3] = (short)tabla["TpoAsunto4"];
                    tesisActual.TipoTesis[4] = (short)tabla["TpoAsunto5"];
                    lista.Add(tesisActual.ConsecIndxInt, tesisActual);
                }
                tabla.Close();
                conexion.Close();
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
                String mensaje = "TesisDAOImpl Exception at getAll\n" + e.Message + e.StackTrace;
                Logg.WriteEntry(mensaje);
                Logg.Close();
                return new Dictionary<long, TesisTO>();
            }
        }

        public List<TesisTO> getTesis(EpocasTO busqueda)
        {
            DbConnection conexion = contextoBD.ContextConection;
            try
            {
                string selectQuery = " select Parte,   IUS,         Rubro,   Epoca, tpoTesis," +
                                     " Sala,    Tesis,      LocAbr, ta_tj, ordentesis, ordeninstancia, ordenRubro, " +
                                     " TpoAsunto1, TpoAsunto2, TpoAsunto3, TpoAsunto4, TpoAsunto5, " +
                                     " tpoPonente1, tpoPonente2, tpoPonente3, tpoPonente4, tpoPonente5 " +
                                     "from Tesis where A.epoca = B.idEpoca " +
                                     "AND A.sala = C.idInst AND A.tpoTesis = D.id AND (";
                foreach (EpocasSalasTO item in busqueda.epocasSalas)
                {
                    selectQuery += "(";
                    selectQuery += "epoca=";
                    selectQuery += item.getEpoca();
                    selectQuery += "AND sala=";
                    selectQuery += item.getSala();
                    selectQuery += ") OR";
                }
                selectQuery = selectQuery.Substring(0, selectQuery.Length - 2);
                DataAdapter query = contextoBD.dataAdapter(selectQuery, conexion);
                DataSet datos = new DataSet();
                query.Fill(datos);
                List<TesisTO> lista = new List<TesisTO>();
                DataTableReader tabla = datos.Tables["tesis"].CreateDataReader();

                //foreach (DataRow fila in tabla.Rows)
                while (tabla.Read())
                {
                    TesisTO tesisActual = new TesisTO();
                    tesisActual.setConsec((String)tabla["ConsecIndx"]);
                    tesisActual.setIus((String)tabla["ius"]);
                    tesisActual.setIus((String)tabla["tpoTesis"]);
                    tesisActual.OrdenInstancia = (int)tabla["OrdenInstancia"];
                    tesisActual.OrdenTesis = (int)tabla["OrdenTesis"];
                    tesisActual.OrdenRubro = (int)tabla["OrdenRubro"];
                    tesisActual.Ponentes = new int[5];
                    tesisActual.Ponentes[0] = (int)tabla["tpoPonente1"];
                    tesisActual.Ponentes[1] = (int)tabla["tpoPonente2"];
                    tesisActual.Ponentes[2] = (int)tabla["tpoPonente3"];
                    tesisActual.Ponentes[3] = (int)tabla["tpoPonente4"];
                    tesisActual.Ponentes[4] = (int)tabla["tpoPonente5"];
                    tesisActual.TipoTesis = new int[5];
                    tesisActual.TipoTesis[0] = (int)tabla["TpoAsunto1"];
                    tesisActual.TipoTesis[1] = (int)tabla["TpoAsunto2"];
                    tesisActual.TipoTesis[2] = (int)tabla["TpoAsunto3"];
                    tesisActual.TipoTesis[3] = (int)tabla["TpoAsunto4"];
                    tesisActual.TipoTesis[4] = (int)tabla["TpoAsunto5"];
                    lista.Add(tesisActual);
                }
                tabla.Close();
                conexion.Close();
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
                String mensaje = "TesisDAOImpl Exception at getTesis(EpocasTO\n" + e.Message + e.StackTrace;
                Logg.WriteEntry(mensaje);
                Logg.Close();

                return new List<TesisTO>();
            }
        }

        public List<TesisTO> getTesis(PartesTO parte)
        {
            DbConnection conexion = contextoBD.ContextConection;
            try
            {
                string selectQuery = "         select A.Parte,   A.IUS,     A.Rubro,     B.descEpoca As epoca," +
                                     "                C.descInst As sala,    A.Tesis,   A.LocAbr, A.ConsecIndx, A.ta_tj,      A.tpoTesis, " +
                                     "                D.imageWeb,           D.descripcion As descTpoTesis, D.imageOther," +
                                     " A.TpoAsunto1, A.TpoAsunto2, A.TpoAsunto3, A.TpoAsunto4, A.TpoAsunto5, " +
                                     " A.tpoPonente1, A.tpoPonente2, A.tpoPonente3, A.tpoPonente4, A.tpoPonente5 " +
                                     "          from Tesis As A, cepocas As B, cinsts As C, ctpoTesis As D" +
                                     "          where parte=" + parte.getParte() + " AND A.epoca = B.idEpoca AND A.sala = C.idInst AND A.tpoTesis = D.id" +
                                     "          order by " + parte.getOrderBy() + " " + parte.getOrderType();
                DataAdapter query = contextoBD.dataAdapter(selectQuery, conexion);
                DataSet datos = new DataSet();
                query.Fill(datos);
                List<TesisTO> lista = new List<TesisTO>();

                DataTableReader tabla = datos.Tables[0].CreateDataReader();
                //foreach (DataRow fila in tabla.Rows)
                while (tabla.Read())
                {
                    TesisTO tesisActual = new TesisTO();
                    tesisActual.setParte((String)tabla["Parte"]);
                    tesisActual.setRubro((String)tabla["Rubro"]);
                    tesisActual.setEpoca((String)tabla["epoca"]);
                    tesisActual.setSala((String)tabla["sala"]);
                    tesisActual.setTesis((String)tabla["tesis"]);
                    tesisActual.setLocAbr((String)tabla["LocAbr"]);
                    tesisActual.setConsec((String)tabla["ConsecIndx"]);
                    tesisActual.setTa_tj((String)tabla["ta_tj"]);
                    tesisActual.setTpoTesis((String)tabla["tpoTesis"]);
                    tesisActual.setImageWeb((String)tabla["imageWeb"]);
                    tesisActual.setDescTpoTesis((String)tabla["descTpoTesis"]);
                    tesisActual.setImageOther((String)tabla["imageOther"]);
                    tesisActual.setIus((String)tabla["ius"]);
                    tesisActual.Ponentes = new int[5];
                    tesisActual.Ponentes[0] = (int)tabla["tpoPonente1"];
                    tesisActual.Ponentes[1] = (int)tabla["tpoPonente2"];
                    tesisActual.Ponentes[2] = (int)tabla["tpoPonente3"];
                    tesisActual.Ponentes[3] = (int)tabla["tpoPonente4"];
                    tesisActual.Ponentes[4] = (int)tabla["tpoPonente5"];
                    tesisActual.TipoTesis = new int[5];
                    tesisActual.TipoTesis[0] = (int)tabla["TpoAsunto1"];
                    tesisActual.TipoTesis[1] = (int)tabla["TpoAsunto2"];
                    tesisActual.TipoTesis[2] = (int)tabla["TpoAsunto3"];
                    tesisActual.TipoTesis[3] = (int)tabla["TpoAsunto4"];
                    tesisActual.TipoTesis[4] = (int)tabla["TpoAsunto5"];
                    lista.Add(tesisActual);
                }
                tabla.Close();
                conexion.Close();
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
                String mensaje = "TesisDAOImpl Exception at getTesis(PartesTO\n" + e.Message + e.StackTrace;
                Logg.WriteEntry(mensaje);
                Logg.Close();

                return new List<TesisTO>();
            }
        }

        public List<TesisTO> getIdTesis(PartesTO parte)
        {
            DbConnection conexion = contextoBD.ContextConection;
            try
            {
                string selectQuery = "    select A.IUS, A.ConsecIndx, A.tpoTesis, A.OrdenTesis, A.OrdenInstancia, A.OrdenRubro, " +
                                     " A.TpoAsunto1, A.TpoAsunto2, A.TpoAsunto3, A.TpoAsunto4, A.TpoAsunto5, " +
                                     " A.tpoPonente1, A.tpoPonente2, A.tpoPonente3, A.tpoPonente4, A.tpoPonente5 " +
                                     "    from Tesis As A" +
                                     "    where parte=" + parte.getParte() +
                                     "    order by " + parte.getOrderBy() + " " + parte.getOrderType();
                DataAdapter query = contextoBD.dataAdapter(selectQuery, conexion);
                DataSet datos = new DataSet();
                query.Fill(datos);
                List<TesisTO> lista = new List<TesisTO>();
                DataTableReader tabla = datos.Tables[0].CreateDataReader();

                //foreach (DataRow fila in tabla.Rows)
                while (tabla.Read())
                {
                    TesisTO tesisActual = new TesisTO();
                    tesisActual.setIus("" + tabla[0]);
                    tesisActual.ConsecIndx = "" + tabla["ConsecIndx"];
                    tesisActual.TpoTesis = "" + tabla["TpoTesis"];
                    tesisActual.OrdenTesis = (int)tabla["OrdenTesis"];
                    tesisActual.OrdenInstancia = (int)tabla["ordenInstancia"];
                    tesisActual.OrdenRubro = (int)tabla["OrdenRubro"];
                    tesisActual.Ponentes = new int[5];
                    tesisActual.Ponentes[0] = (short)tabla["tpoPonente1"];
                    tesisActual.Ponentes[1] = (short)tabla["tpoPonente2"];
                    tesisActual.Ponentes[2] = (short)tabla["tpoPonente3"];
                    tesisActual.Ponentes[3] = (short)tabla["tpoPonente4"];
                    tesisActual.Ponentes[4] = (short)tabla["tpoPonente5"];
                    tesisActual.TipoTesis = new int[5];
                    tesisActual.TipoTesis[0] = (short)tabla["TpoAsunto1"];
                    tesisActual.TipoTesis[1] = (short)tabla["TpoAsunto2"];
                    tesisActual.TipoTesis[2] = (short)tabla["TpoAsunto3"];
                    tesisActual.TipoTesis[3] = (short)tabla["TpoAsunto4"];
                    tesisActual.TipoTesis[4] = (short)tabla["TpoAsunto5"];
                    lista.Add(tesisActual);
                }
                tabla.Close();
                conexion.Close();
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
                String mensaje = "TesisDAOImpl Exception at getIdTesis(PartesTO\n" + e.Message + e.StackTrace;
                Logg.WriteEntry(mensaje);
                Logg.Close();
                return new List<TesisTO>();
            }
        }

        public PaginadorTO getTesisPaginador(PartesTO parte, List<int> Tribunales, Dictionary<int, int[]> secciones)
        {
            DbConnection conexion = contextoBD.ContextConection;

            try
            {
                string selectQuery = null;
                if (Tribunales == null || Tribunales.Count == 0 || Tribunales[0] == -1)
                    selectQuery = "    select A.IUS, A.Parte, B.Seccion " +
                                  "    from Tesis As A, Volumen As B" +
                                  "    where A.Volumen = B.Volumen AND parte=" + parte.getParte() +
                                  "    order by " + parte.getOrderBy() + " " + parte.getOrderType();
                else
                    selectQuery = "select A.IUS, A.Instancia, A.Parte, B.Seccion " +
                                  "    from Tesis As A, Volumen As B" +
                                  "    where A.Volumen = B.Volumen AND parte=" + parte.getParte() +
                                  "    order by " + parte.getOrderBy() + " " + parte.getOrderType();
                DataAdapter query = contextoBD.dataAdapter(selectQuery, conexion);
                DataSet datos = new DataSet();
                query.Fill(datos);

                PaginadorTO resultado = new PaginadorTO();
                List<Int32> lista = new List<Int32>();

                DataTableReader tabla = null;
                tabla = datos.Tables[0].CreateDataReader();
                //foreach (DataRow fila in tabla.Rows)
                while (tabla.Read())
                {
                    Int32 tesisActual = (Int32)tabla["ius"];
                    Int32 parteActual = (Byte)tabla["parte"];
                    Int32 seccionTesis = (Int32)tabla["Seccion"];
                    if (Tribunales != null &&
                        Tribunales.Count != 0 &&
                        Tribunales[0] != -1 &&
                        parteActual < 100 &&
                        (parteActual % 7) == 6)
                    {
                        Int32 instancia = (int)tabla["instancia"];
                        if (Tribunales.Contains(instancia))
                            lista.Add(tesisActual);
                    }
                    else if (parteActual > 139 && parteActual < 148)
                    {
                        if (secciones.Keys.Contains(parteActual)) 
                        {
                            int[] seccionesAcomparar = secciones[parteActual];
                            if (seccionesAcomparar.Contains(seccionTesis) || seccionesAcomparar[1] == -1)
                                lista.Add(tesisActual); 
                        }
                    }
                    else
                    {
                        lista.Add(tesisActual);
                    }
                }

                tabla.Close();
                conexion.Close();
                resultado.Largo = lista.Count;
                resultado.Activo = true;
                resultado.TimeStamp = DateTime.Now;
                resultado.ResultadoIds = lista;
                resultado.TipoBusqueda = IUSConstants.BUSQUEDA_TESIS_SIMPLE;
                #if RED_JUR
                GuardarPaginador(resultado);
                #else
                ConsultasActuales.Add(resultado.Id, resultado);
                #endif
                return resultado;
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
                String mensaje = "TesisDAOImpl Exception at getTesisPaginador(PartesTO\n" + e.Message + e.StackTrace;
                Logg.WriteEntry(mensaje);
                Logg.Close();
                return null;
            }
        }

        private void GuardarPaginador(PaginadorTO resultado)
        {
            BackgroundWorker workerGuardaPag = new BackgroundWorker();
            workerGuardaPag.DoWork += new DoWorkEventHandler(workerGuardaPag_DoWork);
            workerGuardaPag.RunWorkerAsync(resultado);
        }

        void workerGuardaPag_DoWork(object sender, DoWorkEventArgs e)
        {
            PaginadorTO resultado = (PaginadorTO)e.Argument;
            if (resultado.Largo == 0)
                return;
            String sqlqueryA = "insert into " + IUSConstants.PAGINADOR_BD + "Paginador (Id, IUS, consec) ";
            String sqlQueryInicial = "insert into " +
                                     IUSConstants.PAGINADOR_BD + "IdsActivos(id, time, tipoBusqueda, cuantos) values (" +
                                     resultado.Id + ", CURRENT_TIMESTAMP, " + resultado.TipoBusqueda + "," + resultado.Largo + ")";
            DbConnection conexion = contextoBD.ContextConection;
            String sqlquery = sqlqueryA;
            try
            {
                int contador = 1;
                DbCommand com = contextoBD.CommandRegisterPag(sqlQueryInicial, conexion);
                if (com.Connection.State != ConnectionState.Open)
                {
                    com.Connection.Open();
                }
                com.ExecuteNonQuery();
                conexion.Close();
                //sqlquery = String.Empty;
                foreach (int item in resultado.ResultadoIds)
                {
                    if ((contador % 200) == 0)
                    {
                        if (contador != 0)
                        {
                            sqlquery = sqlquery.Substring(0, sqlquery.Length - 11);
                            DbCommand command = contextoBD.CommandRegisterPag(sqlquery, conexion);
                            if (command.Connection.State != ConnectionState.Open)
                            {
                                command.Connection.Open();
                            }
                            command.ExecuteNonQuery();
                            conexion.Close();
                            sqlquery = IUSConstants.CADENA_VACIA;
                        }
                        sqlquery = sqlqueryA + " select " + resultado.Id + ", " + item + ", " + contador + " UNION ALL ";
                    }
                    else
                        sqlquery += " select " + resultado.Id + ", " + item + ", " + contador + " UNION ALL ";
                    ;
                    contador++;
                }

                if (!sqlquery.Equals(IUSConstants.CADENA_VACIA))
                {
                    sqlquery = sqlquery.Substring(0, sqlquery.Length - 11);
                    DbCommand command = contextoBD.CommandRegisterPag(sqlquery, conexion);
                    if (command.Connection.State != ConnectionState.Open)
                    {
                        command.Connection.Open();
                    }
                    command.ExecuteNonQuery();
                    sqlquery = IUSConstants.CADENA_VACIA;
                }
            }
            catch (Exception exc)
            {
                if (!EventLog.SourceExists("IUS"))
                {
                    EventLog.CreateEventSource("IUS", "IUS");
                }
                EventLog Logg = new EventLog("IUS");
                Logg.Source = "IUS";
                String mensaje = "TesisDAOImpl Exception at workerGrardarPag_DoWork(PartesTO\n" + exc.Message + exc.StackTrace;
                Logg.WriteEntry(mensaje);
                Logg.Close();
            }
            conexion.Close();
        }

        public List<TesisTO> getTesisConFiltro(PartesTO parte)
        {
            DbConnection conexion = contextoBD.ContextConection;
            try
            {
                string selectQuery = "select A.Parte,               A.IUS,       A.Rubro,     B.descEpoca As epoca," +
                                     "       C.descInst As sala,       A.Tesis,     A.LocAbr,    A.ConsecIndx, " +
                                     "       A.ta_tj,               A.tpoTesis,  D.imageWeb,  D.descripcion As descTpoTesis, " +
                                     " A.TpoAsunto1, A.TpoAsunto2, A.TpoAsunto3, A.TpoAsunto4, A.TpoAsunto5, " +
                                     " A.tpoPonente1, A.tpoPonente2, A.tpoPonente3, A.tpoPonente4, A.tpoPonente5, " +
                                     "       D.imageOther " +
                                     " from Tesis As A, cepocas As B, cinsts As C, ctpoTesis As D" +
                                     " where parte = " + parte.getParte() +
                                     "   AND " + parte.getFilterBy() + " = " + parte.getFilterValue() +
                                     "   AND A.epoca = B.idEpoca " +
                                     "   AND A.sala  = C.idInst " +
                                     "   AND A.tpoTesis = D.id " +
                                     " order by " + parte.getOrderBy() + " " + parte.getOrderType();
                DataAdapter query = contextoBD.dataAdapter(selectQuery, conexion);
                DataSet datos = new DataSet();
                query.Fill(datos);
                List<TesisTO> lista = new List<TesisTO>();
                DataTableReader tabla = datos.Tables[0].CreateDataReader();

                //foreach (DataRow fila in tabla.Rows)
                while (tabla.Read())
                {
                    TesisTO tesisActual = new TesisTO();
                    tesisActual.setParte("" + tabla["Parte"]);
                    tesisActual.setRubro("" + tabla["Rubro"]);
                    tesisActual.setEpoca("" + tabla["epoca"]);
                    tesisActual.setSala("" + tabla["sala"]);
                    tesisActual.setTesis("" + tabla["tesis"]);
                    tesisActual.setLocAbr("" + tabla["LocAbr"]);
                    tesisActual.setConsec("" + tabla["ConsecIndx"]);
                    tesisActual.setTa_tj("" + tabla["ta_tj"]);
                    tesisActual.setTpoTesis("" + tabla["tpoTesis"]);
                    tesisActual.setImageWeb("" + tabla["imageWeb"]);
                    tesisActual.setDescTpoTesis("" + tabla["descTpoTesis"]);
                    tesisActual.setImageOther("" + tabla["imageOther"]);
                    tesisActual.setIus("" + tabla["ius"]);
                    tesisActual.Ponentes = new int[5];
                    tesisActual.Ponentes[0] = (short)tabla["tpoPonente1"];
                    tesisActual.Ponentes[1] = (short)tabla["tpoPonente2"];
                    tesisActual.Ponentes[2] = (short)tabla["tpoPonente3"];
                    tesisActual.Ponentes[3] = (short)tabla["tpoPonente4"];
                    tesisActual.Ponentes[4] = (short)tabla["tpoPonente5"];
                    tesisActual.TipoTesis = new int[5];
                    tesisActual.TipoTesis[0] = (short)tabla["TpoAsunto1"];
                    tesisActual.TipoTesis[1] = (short)tabla["TpoAsunto2"];
                    tesisActual.TipoTesis[2] = (short)tabla["TpoAsunto3"];
                    tesisActual.TipoTesis[3] = (short)tabla["TpoAsunto4"];
                    tesisActual.TipoTesis[4] = (short)tabla["TpoAsunto5"];
                    lista.Add(tesisActual);
                }
                tabla.Close();
                conexion.Close();
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
                String mensaje = "TesisDAOImpl Exception at getTesisConFiltro(PartesTO\n" + e.Message + e.StackTrace;
                Logg.WriteEntry(mensaje);
                Logg.Close();
                return new List<TesisTO>();
            }
        }

        public List<TesisTO> getTesisJurisprudencia(PartesTO parte)
        {
            DbConnection conexion = contextoBD.ContextConection;
            try
            {
                string selectQuery = "select A.Parte,   A.IUS,      A.Rubro,     B.descEpoca As epoca," +
                                     "       C.descInst As sala,       A.Tesis,     A.LocAbr, " +
                                     "       A.ConsecIndx, " +
                                     "       A.ta_tj,   A.tpoTesis, D.imageWeb,  D.descripcion As descTpoTesis, D.imageOther" +
                                     " from Tesis As A, cepocas As B, cinsts As C, ctpoTesis As D" +
                                     " where parte = " + parte.getParte() +
                                     "       AND A.ta_tj = 1 " +
                                     "       AND A.epoca = B.idEpoca " +
                                     "       AND A.sala  = C.idInst " +
                                     "       AND A.tpoTesis = D.id" +
                                     " order by " + parte.getOrderBy() + " " + parte.getOrderType();
                DataAdapter query = contextoBD.dataAdapter(selectQuery, conexion);
                DataSet datos = new DataSet();
                query.Fill(datos);
                List<TesisTO> lista = new List<TesisTO>();
                DataTableReader tabla = datos.Tables[0].CreateDataReader();

                //foreach (DataRow fila in tabla.Rows)
                while (tabla.Read())
                {
                    TesisTO tesisActual = new TesisTO();
                    tesisActual.setParte("" + tabla["Parte"]);
                    tesisActual.setRubro("" + tabla["Rubro"]);
                    tesisActual.setEpoca("" + tabla["epoca"]);
                    tesisActual.setSala("" + tabla["sala"]);
                    tesisActual.setTesis("" + tabla["tesis"]);
                    tesisActual.setLocAbr("" + tabla["LocAbr"]);
                    tesisActual.setConsec("" + tabla["ConsecIndx"]);
                    tesisActual.setTa_tj("" + tabla["ta_tj"]);
                    tesisActual.setTpoTesis("" + tabla["tpoTesis"]);
                    tesisActual.setImageWeb("" + tabla["imageWeb"]);
                    tesisActual.setDescTpoTesis("" + tabla["descTpoTesis"]);
                    tesisActual.setImageOther("" + tabla["imageOther"]);
                    tesisActual.setIus("" + tabla["ius"]);
                    lista.Add(tesisActual);
                }
                tabla.Close();

                conexion.Close();
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
                String mensaje = "TesisDAOImpl Exception at getTesisJurisprudencia(PartesTO\n" + e.Message + e.StackTrace;
                Logg.WriteEntry(mensaje);
                Logg.Close();
                return new List<TesisTO>();
            }
        }

        public /*List<TesisTO>*/ DataTableReader getTesisConFiltro(MostrarPorIusTO parte)
        {
            DbConnection conexion = contextoBD.ContextConection;
            try
            {
                string selectQuery = " select A.Parte,   A.IUS,      A.Rubro,     B.descEpoca As epoca," +
                                     "        C.descInst As sala,       A.Tesis,     A.LocAbr, A.ConsecIndx, " +
                                     "        A.ta_tj,   A.tpoTesis, D.imageWeb,  D.descripcion As descTpoTesis, D.imageOther" +
                                     " from Tesis As A, cepocas As B, cinsts As C, ctpoTesis As D " +
                                     " where  " + parte.getFilterBy() + " = " + parte.getFilterValue() +
                                     "    AND A.epoca = B.idEpoca " +
                                     "    AND A.sala  = C.idInst " +
                                     "    AND A.tpoTesis = D.id" +
                                     "    AND ius IN (";
                foreach (int item in parte.getListado())
                {
                    selectQuery += item;
                    selectQuery += ",";
                }
                selectQuery = selectQuery.Substring(0, selectQuery.Length - 1);
                selectQuery += ")";
                selectQuery += " order by " + parte.getOrderBy() + " " + parte.getOrderType();
                DataAdapter query = contextoBD.dataAdapter(selectQuery, conexion);
                DataSet datos = new DataSet();
                query.Fill(datos);
                List<TesisTO> lista = new List<TesisTO>();
                DataTableReader tabla = datos.Tables[0].CreateDataReader();
                conexion.Close();
                return tabla;
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
                String mensaje = "TesisDAOImpl Exception at getTesisConfiltro(MostrarPorIusTO\n" + e.Message + e.StackTrace;
                Logg.WriteEntry(mensaje);
                Logg.Close();
                return (new DataTable()).CreateDataReader();// List<TesisTO>();
            }
        }

        public DataTableReader /*List<TesisTO>*/ getTesisJurisprudencia(MostrarPorIusTO parte)
        {
            try
            {
                DbConnection conexion = contextoBD.ContextConection;
                string selectQuery = " select A.Parte,   A.IUS,      A.Rubro,     B.descEpoca As epoca," +
                                     "        C.descInst As sala,       A.Tesis,     A.LocAbr, A.ConsecIndx, " +
                                     "        A.ta_tj,   A.tpoTesis, D.imageWeb,  D.descripcion As descTpoTesis, D.imageOther" +
                                     " from Tesis As A, cepocas As B, cinsts As C, ctpoTesis As D " +
                                     " where  a.ta_tj = 1 " +
                                     "    AND A.epoca = B.idEpoca " +
                                     "    AND A.sala  = C.idInst " +
                                     "    AND A.tpoTesis = D.id" +
                                     "    AND ius IN (";
                foreach (int item in parte.getListado())
                {
                    selectQuery += item;
                    selectQuery += ",";
                }
                selectQuery = selectQuery.Substring(0, selectQuery.Length - 1);
                selectQuery += ")";
                selectQuery += " order by " + parte.getOrderBy() + " " + parte.getOrderType();
                DataAdapter query = contextoBD.dataAdapter(selectQuery, conexion);
                DataSet datos = new DataSet();
                query.Fill(datos);
                List<TesisTO> lista = new List<TesisTO>();
                DataTableReader tabla = datos.Tables[0].CreateDataReader();
                conexion.Close();
                return tabla;
            }
            catch (Exception e)
            {
                if (!EventLog.SourceExists("IUS"))
                {
                    EventLog.CreateEventSource("IUS", "IUS");
                }
                EventLog Logg = new EventLog("IUS");
                Logg.Source = "IUS";
                String mensaje = "TesisDAOImpl Exception at getTesisJurisprudencia(MostrarPorIusTO\n" + e.Message + e.StackTrace;
                Logg.WriteEntry(mensaje);
                Logg.Close();
                return (new DataTable()).CreateDataReader();//List<TesisTO>();
            }
        }

        public TesisTO getTesisPorIus(Int32 ius)
        {
            DbConnection conexion = contextoBD.ContextConection;
            //DbConnection conexion = new SqlConnection(ConfigurationManager.ConnectionStrings["IusProduccion"].ToString());

            try
            {
                string selectQuery = " select A.Parte,   A.Consec,  A.IUS,         A.Rubro,    A.Texto,     A.Precedentes, B.DescEpoca  as epoca," +
                                     "        C.descInst As sala,    E.DescFte As fuente,  F.txtVolumen as volumen,  A.Tesis,    A.Pagina,    A.ta_tj,      A.Materia1," +
                                     "        A.Materia2,         A.Materia3,        A.IdGenealogia,  F.Orden as volOrden, A.ConsecIndx,A.IdTCC,      A.InfAnexos," +
                                     " A.TpoAsunto1, A.TpoAsunto2, A.TpoAsunto3, A.TpoAsunto4, A.TpoAsunto5, " +
                                     " A.tpoPonente1, A.tpoPonente2, A.tpoPonente3, A.tpoPonente4, A.tpoPonente5, " +
                                     "        A.LocAbr,  A.NumLetra,A.ConsecLetra, A.Instancia,A.ConsecInst,      A.tpoTesis, " +
                                     "        D.imageWeb,           D.descripcion as descTpoTesis, D.imageOther, A.ExistenTemas, G.txtVolumen as subvolumen" +
                                     " from Tesis As A, cepocas As B, cinsts As C, ctpoTesis As D, cfuentes As E, volumen As F, subVolumen as G where ius = " + ius.ToString() +
                                     "                     AND A.epoca = B.idEpoca " +
                                     "                           AND A.sala = C.idInst " +
                                     "                           AND A.tpoTesis = D.id" +
                                     "                           AND A.fuente = E.IdFte" +
                                     "                           AND A.Volumen = F.volumen" +
                                     "                           AND A.idSubVolumen = G.idSubVolumen";
                if (conexion.State != ConnectionState.Open)
                    conexion.Open();
                DataAdapter query = contextoBD.dataAdapter(selectQuery, conexion);
                DataSet datos = new DataSet();
                query.Fill(datos);
                List<TesisTO> lista = new List<TesisTO>();
                DataTableReader tabla = datos.Tables[0].CreateDataReader();

                //foreach (DataRow fila in tabla.Rows)
                while (tabla.Read())
                {
                    TesisTO tesisActual = new TesisTO();
                    tesisActual.setParte("" + tabla["Parte"]);
                    tesisActual.setConsec("" + tabla["Consec"]);
                    tesisActual.setRubro("" + tabla["Rubro"]);
                    tesisActual.setTexto("" + tabla["texto"]);
                    tesisActual.setPrecedentes("" + tabla["precedentes"]);
                    tesisActual.setEpoca("" + tabla["epoca"]);
                    tesisActual.setSala("" + tabla["sala"]);
                    tesisActual.setFuente("" + tabla["fuente"]);
                    String subvolumen = String.Empty + tabla["subVolumen"];
                    if (!subvolumen.Trim().Equals(String.Empty))
                        tesisActual.setVolumen("" + tabla["volumen"] + ", " + tabla["subVolumen"]);
                    else
                        tesisActual.setVolumen(String.Empty + tabla["volumen"]);
                    tesisActual.setVolOrden("" + tabla["volOrden"]);
                    tesisActual.setInfAnexos("" + tabla["infAnexos"]);
                    tesisActual.setIdTCC("" + tabla["idTCC"]);
                    tesisActual.setNumLetra("" + tabla["numLetra"]);
                    tesisActual.setConsecLetra("" + tabla["consecLetra"]);
                    tesisActual.setInstancia("" + tabla["instancia"]);
                    tesisActual.setConsecInst("" + tabla["consecInst"]);
                    tesisActual.setTesis("" + tabla["tesis"]);
                    tesisActual.setPagina("" + tabla["pagina"]);
                    tesisActual.setLocAbr("" + tabla["LocAbr"]);
                    tesisActual.setConsec("" + tabla["ConsecIndx"]);
                    tesisActual.setTa_tj("" + tabla["ta_tj"]);
                    tesisActual.setMateria1("" + tabla["materia1"]);
                    tesisActual.setMateria2("" + tabla["materia2"]);
                    tesisActual.setMateria3("" + tabla["materia3"]);
                    tesisActual.setIdGenealogia("" + tabla["idGenealogia"]);
                    tesisActual.setTpoTesis("" + tabla["tpoTesis"]);
                    tesisActual.setImageWeb("" + tabla["imageWeb"]);
                    tesisActual.setDescTpoTesis("" + tabla["descTpoTesis"]);
                    tesisActual.setImageOther("" + tabla["imageOther"]);
                    tesisActual.setIus("" + tabla["ius"]);
                    tesisActual.Ponentes = new int[5];
                    tesisActual.Ponentes[0] = (short)tabla["tpoPonente1"];
                    tesisActual.Ponentes[1] = (short)tabla["tpoPonente2"];
                    tesisActual.Ponentes[2] = (short)tabla["tpoPonente3"];
                    tesisActual.Ponentes[3] = (short)tabla["tpoPonente4"];
                    tesisActual.Ponentes[4] = (short)tabla["tpoPonente5"];
                    tesisActual.TipoTesis = new int[5];
                    tesisActual.TipoTesis[0] = (short)tabla["TpoAsunto1"];
                    tesisActual.TipoTesis[1] = (short)tabla["TpoAsunto2"];
                    tesisActual.TipoTesis[2] = (short)tabla["TpoAsunto3"];
                    tesisActual.TipoTesis[3] = (short)tabla["TpoAsunto4"];
                    tesisActual.TipoTesis[4] = (short)tabla["TpoAsunto5"];
                    tesisActual.ExistenTemas = (byte)tabla["ExistenTemas"] == 1;

                    lista.Add(tesisActual);
                }
                tabla.Close();

                conexion.Close();
                if (lista.Count > 0)
                {
                    return lista[0];
                }
                else
                {
                    return new TesisTO();
                }
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
                String mensaje = "TesisDAOImpl Exception at getTesisPorIus(Int32\n" + e.Message + e.StackTrace;
                Logg.WriteEntry(mensaje);
                Logg.Close();
                return new TesisTO();
            }
        }

        public int getVolumen(int ius)
        {
            DbConnection conexion = contextoBD.ContextConection;
            try
            {
                string selectQuery = " select  Volumen, ius " +
                                     " from Tesis where ius = " + ius.ToString();
                DataAdapter query = contextoBD.dataAdapter(selectQuery, conexion);
                DataSet datos = new DataSet();
                if (conexion.State != ConnectionState.Open)
                    conexion.Open();
                query.Fill(datos);
                DataTableReader tabla = datos.Tables[0].CreateDataReader();
                int Resultado = -1;

                //foreach (DataRow fila in tabla.Rows)
                while (tabla.Read())
                {
                    Resultado = (short)tabla["Volumen"];
                }
                tabla.Close();
                conexion.Close();

                return Resultado;
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
                String mensaje = "TesisDAOImpl Exception at getTesisPorIus(Int32\n" + e.Message + e.StackTrace;
                Logg.WriteEntry(mensaje);
                Logg.Close();
                return -1;
            }
        }

        public TesisTO getTesisPorIusParaLista(Int32 ius)
        {
            DbConnection conexion = contextoBD.ContextConection;
            try
            {
                string selectQuery = " select A.IUS,                A.Rubro, " +
                                     "        C.descInst As sala,   A.Tesis,              A.ta_tj, " +
                                     "        A.ConsecIndx,         " +
                                     "        A.TpoAsunto1,         A.TpoAsunto2,         A.TpoAsunto3,  A.TpoAsunto4, A.TpoAsunto5, " +
                                     "        A.tpoPonente1,        A.tpoPonente2,        A.tpoPonente3, A.tpoPonente4,A.tpoPonente5, " +
                                     "        A.LocAbr,             A.tpoTesis, " +
                                     "        D.descripcion as descTpoTesis, D.imageOther" +
                                     " from Tesis As A, cinsts As C, ctpoTesis As D  where ius = " + ius.ToString() +
                                     "                     AND  A.sala = C.idInst " +
                                     "                     AND A.tpoTesis = D.id";
                DataAdapter query = contextoBD.dataAdapter(selectQuery, conexion);
                DataSet datos = new DataSet();
                if (conexion.State != ConnectionState.Open)
                    conexion.Open();
                query.Fill(datos);
                List<TesisTO> lista = new List<TesisTO>();
                DataTableReader tabla = datos.Tables[0].CreateDataReader();

                //foreach (DataRow fila in tabla.Rows)
                while (tabla.Read())
                {
                    TesisTO tesisActual = new TesisTO();
                    //tesisActual.setParte("" + fila["Parte"]);
                    //tesisActual.setConsec("" + fila["Consec"]);
                    tesisActual.setRubro((String)tabla["Rubro"]);
                    //tesisActual.setTexto("" + fila["texto"]);
                    //tesisActual.setPrecedentes("" + fila["precedentes"]);
                    //tesisActual.setEpoca("" + fila["epoca"]);
                    tesisActual.setSala("" + tabla["sala"]);
                    //tesisActual.setFuente("" + fila["fuente"]);
                    //tesisActual.setVolumen("" + fila["volumen"]);
                    //tesisActual.setVolOrden("" + fila["volOrden"]);
                    //tesisActual.setInfAnexos("" + fila["infAnexos"]);
                    //tesisActual.setIdTCC("" + fila["idTCC"]);
                    //tesisActual.setNumLetra("" + fila["numLetra"]);
                    //tesisActual.setConsecLetra("" + fila["consecLetra"]);
                    //tesisActual.setInstancia("" + fila["instancia"]);
                    //tesisActual.setConsecInst("" + fila["consecInst"]);
                    tesisActual.setTesis("" + tabla["tesis"]);
                    //tesisActual.setPagina("" + fila["pagina"]);
                    tesisActual.setLocAbr("" + tabla["LocAbr"]);
                    tesisActual.setConsec("" + tabla["ConsecIndx"]);
                    tesisActual.setTa_tj("" + tabla["ta_tj"]);
                    //tesisActual.setMateria1("" + fila["materia1"]);
                    //tesisActual.setMateria2("" + fila["materia2"]);
                    //tesisActual.setMateria3("" + fila["materia3"]);
                    //tesisActual.setIdGenealogia("" + fila["idGenealogia"]);
                    tesisActual.setTpoTesis("" + tabla["tpoTesis"]);
                    //tesisActual.setImageWeb("" + fila["imageWeb"]);
                    tesisActual.setDescTpoTesis("" + tabla["descTpoTesis"]);
                    tesisActual.setImageOther("" + tabla["imageOther"]);
                    tesisActual.setIus("" + tabla["ius"]);
                    tesisActual.Ponentes = new int[5];
                    tesisActual.Ponentes[0] = (short)tabla["tpoPonente1"];
                    tesisActual.Ponentes[1] = (short)tabla["tpoPonente2"];
                    tesisActual.Ponentes[2] = (short)tabla["tpoPonente3"];
                    tesisActual.Ponentes[3] = (short)tabla["tpoPonente4"];
                    tesisActual.Ponentes[4] = (short)tabla["tpoPonente5"];
                    tesisActual.TipoTesis = new int[5];
                    tesisActual.TipoTesis[0] = (short)tabla["TpoAsunto1"];
                    tesisActual.TipoTesis[1] = (short)tabla["TpoAsunto2"];
                    tesisActual.TipoTesis[2] = (short)tabla["TpoAsunto3"];
                    tesisActual.TipoTesis[3] = (short)tabla["TpoAsunto4"];
                    tesisActual.TipoTesis[4] = (short)tabla["TpoAsunto5"];
                    lista.Add(tesisActual);
                }
                tabla.Close();
                conexion.Close();
                //conexion.Close();
                return lista[0];
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
                String mensaje = "TesisDAOImpl Exception at getTesisPorIusParaLista(Int32 = " + ius + " )\n" + e.Message + "\n" + e.StackTrace;
                Logg.WriteEntry(mensaje);
                Logg.Close();
                return new TesisTO();
            }
        }

        public TesisTO getTesisPorConsecIndx(Int32 ConsecIndx)
        {
            DbConnection conexion = contextoBD.ContextConection;
            try
            {
                string selectQuery = " select A.Parte,   A.Consec,  A.IUS,         A.Rubro,    A.Texto,     A.Precedentes, B.DescEpoca  as epoca," +
                                     "        C.descInst As sala,    E.DescFte As fuente,  F.txtVolumen as volumen,  A.Tesis,    A.Pagina,    A.ta_tj,      A.Materia1," +
                                     "        A.Materia2,         A.Materia3,        A.IdGenealogia,        F.Orden as volOrden, A.ConsecIndx,A.IdTCC,      A.InfAnexos," +
                                     "        A.LocAbr,  A.NumLetra,A.ConsecLetra, A.Instancia,A.ConsecInst,      A.tpoTesis, " +
                                     "        D.imageWeb,           D.descripcion as descTpoTesis, D.imageOther, " +
                                     " A.TpoAsunto1, A.TpoAsunto2, A.TpoAsunto3, A.TpoAsunto4, A.TpoAsunto5, " +
                                     " A.tpoPonente1, A.tpoPonente2, A.tpoPonente3, A.tpoPonente4, A.tpoPonente5 " +
                                     " from Tesis As A, cepocas As B, cinsts As C, ctpoTesis As D, cfuentes As E, volumen As F where ConsecIndx = " + ConsecIndx.ToString() +
                                     "                     AND A.epoca = B.idEpoca " +
                                     "                           AND A.sala = C.idInst " +
                                     "                           AND A.tpoTesis = D.id" +
                                     "                           AND A.fuente = E.IdFte" +
                                     "                           AND A.Volumen = F.volumen";
                DataAdapter query = contextoBD.dataAdapter(selectQuery, conexion);
                DataSet datos = new DataSet();
                query.Fill(datos);
                List<TesisTO> lista = new List<TesisTO>();
                DataTableReader tabla = datos.Tables[0].CreateDataReader();

                //foreach (DataRow fila in tabla.Rows)
                while (tabla.Read())
                {
                    TesisTO tesisActual = new TesisTO();
                    tesisActual.setParte("" + tabla["Parte"]);
                    tesisActual.setConsec("" + tabla["Consec"]);
                    tesisActual.setRubro("" + tabla["Rubro"]);
                    tesisActual.setTexto("" + tabla["texto"]);
                    tesisActual.setPrecedentes("" + tabla["precedentes"]);
                    tesisActual.setEpoca("" + tabla["epoca"]);
                    tesisActual.setSala("" + tabla["sala"]);
                    tesisActual.setFuente("" + tabla["fuente"]);
                    tesisActual.setVolumen("" + tabla["volumen"]);
                    tesisActual.setVolOrden("" + tabla["volOrden"]);
                    tesisActual.setInfAnexos("" + tabla["infAnexos"]);
                    tesisActual.setIdTCC("" + tabla["idTCC"]);
                    tesisActual.setNumLetra("" + tabla["numLetra"]);
                    tesisActual.setConsecLetra("" + tabla["consecLetra"]);
                    tesisActual.setInstancia("" + tabla["instancia"]);
                    tesisActual.setConsecInst("" + tabla["consecInst"]);
                    tesisActual.setTesis("" + tabla["tesis"]);
                    tesisActual.setPagina("" + tabla["pagina"]);
                    tesisActual.setLocAbr("" + tabla["LocAbr"]);
                    tesisActual.setConsec("" + tabla["ConsecIndx"]);
                    tesisActual.setTa_tj("" + tabla["ta_tj"]);
                    tesisActual.setMateria1("" + tabla["materia1"]);
                    tesisActual.setMateria2("" + tabla["materia2"]);
                    tesisActual.setMateria3("" + tabla["materia3"]);
                    tesisActual.setIdGenealogia("" + tabla["idGenealogia"]);
                    tesisActual.setTpoTesis("" + tabla["tpoTesis"]);
                    tesisActual.setImageWeb("" + tabla["imageWeb"]);
                    tesisActual.setDescTpoTesis("" + tabla["descTpoTesis"]);
                    tesisActual.setImageOther("" + tabla["imageOther"]);
                    tesisActual.setIus("" + tabla["ius"]);
                    tesisActual.Ponentes = new int[5];
                    tesisActual.Ponentes[0] = (short)tabla["tpoPonente1"];
                    tesisActual.Ponentes[1] = (short)tabla["tpoPonente2"];
                    tesisActual.Ponentes[2] = (short)tabla["tpoPonente3"];
                    tesisActual.Ponentes[3] = (short)tabla["tpoPonente4"];
                    tesisActual.Ponentes[4] = (short)tabla["tpoPonente5"];
                    tesisActual.TipoTesis = new int[5];
                    tesisActual.TipoTesis[0] = (short)tabla["TpoAsunto1"];
                    tesisActual.TipoTesis[1] = (short)tabla["TpoAsunto2"];
                    tesisActual.TipoTesis[2] = (short)tabla["TpoAsunto3"];
                    tesisActual.TipoTesis[3] = (short)tabla["TpoAsunto4"];
                    tesisActual.TipoTesis[4] = (short)tabla["TpoAsunto5"];
                    lista.Add(tesisActual);
                }
                tabla.Close();

                conexion.Close();
                return lista[0];
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
                String mensaje = "TesisDAOImpl Exception at getTesisPorConsecIndx(Int32\n" + e.Message + e.StackTrace;
                Logg.WriteEntry(mensaje);
                Logg.Close();
                return new TesisTO();
            }
        }

        public TesisTO getTesisEliminada(Int32 ius)
        {
            DbConnection conexion = contextoBD.ContextConection;
            try
            {
                string selectQuery = " select A.Parte,   A.Consec,  A.IUS,         A.Rubro,    A.Texto,     A.Precedentes, B.DescEpoca As Epoca," +
                                     "        C.descInst As sala,    E.DescFte As fuente,  F.txtVolumen As volumen,     A.Tesis,    A.Pagina,    A.ta_tj,      A.Materia1," +
                                     "        A.Materia2,A.Materia3,A.IdGenealogia,F.Orden As volOrden, A.ConsecIndx,A.IdTCC,      A.InfAnexos," +
                                     "        A.LocAbr,  A.NumLetra,A.ConsecLetra, A.Instancia,A.ConsecInst,      A.tpoTesis, " +
                                     "        D.imageWeb,           D.descripcion As descTpoTesis, D.imageOther" +
                                     " from Tesis As A, cepocas As B, cinsts As C, ctpoTesis As D, cfuentes As E, volumen As F,  tesiseliminadas As G " +
                                     " where G.ius = " + ius.ToString() +
                                     "        AND G.sustituida = A.ius" +
                                     "        AND A.epoca = B.idEpoca " +
                                     "        AND A.sala = C.idInst " +
                                     "        AND A.tpoTesis = D.id" +
                                     "        AND A.fuente = E.IdFte" +
                                     "        AND A.Volumen = F.volumen";
                DataAdapter query = contextoBD.dataAdapter(selectQuery, conexion);
                DataSet datos = new DataSet();
                query.Fill(datos);
                List<TesisTO> lista = new List<TesisTO>();
                DataTableReader tabla = datos.Tables[0].CreateDataReader();

                //foreach (DataRow fila in tabla.Rows)
                while (tabla.Read())
                {
                    TesisTO tesisActual = new TesisTO();
                    tesisActual.setParte("" + tabla["Parte"]);
                    tesisActual.setConsec("" + tabla["Consec"]);
                    tesisActual.setRubro("" + tabla["Rubro"]);
                    tesisActual.setTexto("" + tabla["texto"]);
                    tesisActual.setPrecedentes("" + tabla["precedentes"]);
                    tesisActual.setEpoca("" + tabla["epoca"]);
                    tesisActual.setSala("" + tabla["sala"]);
                    tesisActual.setFuente("" + tabla["fuente"]);
                    tesisActual.setVolumen("" + tabla["volumen"]);
                    tesisActual.setVolOrden("" + tabla["volOrden"]);
                    tesisActual.setInfAnexos("" + tabla["infAnexos"]);
                    tesisActual.setIdTCC("" + tabla["idTCC"]);
                    tesisActual.setNumLetra("" + tabla["numLetra"]);
                    tesisActual.setConsecLetra("" + tabla["consecLetra"]);
                    tesisActual.setInstancia("" + tabla["instancia"]);
                    tesisActual.setConsecInst("" + tabla["consecInst"]);
                    tesisActual.setTesis("" + tabla["tesis"]);
                    tesisActual.setPagina("" + tabla["pagina"]);
                    tesisActual.setLocAbr("" + tabla["LocAbr"]);
                    tesisActual.setConsec("" + tabla["ConsecIndx"]);
                    tesisActual.setTa_tj("" + tabla["ta_tj"]);
                    tesisActual.setMateria1("" + tabla["materia1"]);
                    tesisActual.setMateria2("" + tabla["materia2"]);
                    tesisActual.setMateria3("" + tabla["materia3"]);
                    tesisActual.setIdGenealogia("" + tabla["idGenealogia"]);
                    tesisActual.setTpoTesis("" + tabla["tpoTesis"]);
                    tesisActual.setImageWeb("" + tabla["imageWeb"]);
                    tesisActual.setDescTpoTesis("" + tabla["descTpoTesis"]);
                    tesisActual.setImageOther("" + tabla["imageOther"]);
                    tesisActual.setIus("" + tabla["ius"]);
                    lista.Add(tesisActual);
                }
                tabla.Close();
                if (lista.Count < 1)
                {
                    return new TesisTO();
                }
                conexion.Close();
                return lista[0];
            }
            catch (Exception e)
            {
                conexion.Close();
                //if (!EventLog.SourceExists("IUS"))
                //{
                //    EventLog.CreateEventSource("IUS", "IUS");
                //}
                //EventLog Logg = new EventLog("IUS");
                //Logg.Source = "IUS";
                //String mensaje = "TesisDAOImpl Exception at getTesisEliminada(Int32\n" + e.Message + e.StackTrace;
                //Logg.WriteEntry(mensaje);
                //Logg.Close();
                return new TesisTO();
            }
        }

        public TesisTO getTesisReferenciadas(Int32 ius)
        {
            DbConnection conexion = contextoBD.ContextConection;
            try
            {
                string selectQuery = " select Ius, SustituidaPor" +
                                     " from TesisRefsSup" +
                                     " where ius = " + ius.ToString();
                DataAdapter query = contextoBD.dataAdapter(selectQuery, conexion);
                DataSet datos = new DataSet();
                query.Fill(datos);
                List<TesisTO> lista = new List<TesisTO>();
                DataTableReader tabla = datos.Tables[0].CreateDataReader();

                //foreach (DataRow fila in tabla.Rows)
                while (tabla.Read())
                {
                    TesisTO tesisActual = new TesisTO();
                    tesisActual.setIus("" + tabla["SustituidaPor"]);
                    lista.Add(tesisActual);
                }
                tabla.Close();

                conexion.Close();
                if (lista.Count < 1)
                {
                    return new TesisTO();
                }
                return lista[0];
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
                String mensaje = "TesisDAOImpl Exception at getTesisReferenciadas(Int32\n" + e.Message + e.StackTrace;
                Logg.WriteEntry(mensaje);
                Logg.Close();
                return new TesisTO();
            }
        }

        public List<RelacionTO> getRelaciones(Int32 ius)
        {
            try
            {
                return new List<RelacionTO>();
            }
            catch (Exception e)
            {
                if (!EventLog.SourceExists("IUS"))
                {
                    EventLog.CreateEventSource("IUS", "IUS");
                }
                EventLog Logg = new EventLog("IUS");
                Logg.Source = "IUS";
                String mensaje = "TesisDAOImpl Exception at getRelaciones(Int32\n" + e.Message + e.StackTrace;
                Logg.WriteEntry(mensaje);
                Logg.Close();
                return new List<RelacionTO>();
            }
        }

        public List<RelacionTO> getRelacionesIUSSeccion(RelacionTO iusSeccion)
        {
            DbConnection conexion = contextoBD.ContextConection;
            try
            {
                String sqlQuery = " select IUS,IdRel,MiDescriptor,Seccion,Posicion,Tipo From relaciones " +
                                  " where IUS = " + iusSeccion.getIus() + " AND Seccion = " + iusSeccion.getSeccion() +
                                  " order by Posicion asc";

                DataAdapter query = contextoBD.dataAdapter(sqlQuery, conexion);
                DataSet datos = new DataSet();
                query.Fill(datos);
                List<RelacionTO> lista = new List<RelacionTO>();
                DataTable tabla = datos.Tables[0];

                foreach (DataRow fila in tabla.Rows)
                //while (tabla.Read())
                {
                    RelacionTO relacionActual = new RelacionTO();
                    relacionActual.setIdRel("" + fila["idRel"]);
                    relacionActual.setIus("" + fila["ius"]);
                    relacionActual.setMiDescriptor((String)fila["MiDescriptor"]);
                    relacionActual.setPosicion("" + fila["Posicion"]);
                    relacionActual.setSeccion("" + fila["Seccion"]);
                    relacionActual.setTipo("" + fila["Tipo"]);
                    lista.Add(relacionActual);
                }
                //tabla.Close();
                datos.Dispose();
                conexion.Close();
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
                String mensaje = "TesisDAOImpl Exception at getRelacionesIUSSeccion(RelacionTO\n" + e.Message + e.StackTrace;
                Logg.WriteEntry(mensaje);
                Logg.Close();
                return new List<RelacionTO>();
            }
        }

        public List<RelacionFraseArticulosTO> getRelacionFraseArticulos(
            RelacionFraseArticulosTO idRelIus)
        {
            DbConnection conexion = contextoBD.ContextConection;
            try
            {
                String sqlQuery = " select ius, idRel, idLey, idArt, idRef, consec from relfrasearts" +
                                  " where ius = " + idRelIus.getIus() + " AND idrel = " + idRelIus.getIdRel() + " order by consec asc";

                DataAdapter query = contextoBD.dataAdapter(sqlQuery, conexion);
                DataSet datos = new DataSet();
                query.Fill(datos);
                List<RelacionFraseArticulosTO> lista = new List<RelacionFraseArticulosTO>();
                DataTableReader tabla = datos.Tables[0].CreateDataReader();

                //foreach (DataRow fila in tabla.Rows)
                while (tabla.Read())
                {
                    RelacionFraseArticulosTO relacionActual = new RelacionFraseArticulosTO();
                    relacionActual.setIdRel("" + tabla["idRel"]);
                    relacionActual.setIus("" + tabla["ius"]);
                    relacionActual.setIdLey("" + tabla["idLey"]);
                    relacionActual.setIdArt("" + tabla["idArt"]);
                    relacionActual.setIdRef("" + tabla["idRef"]);
                    relacionActual.setConsec("" + tabla["consec"]);
                    lista.Add(relacionActual);
                }
                tabla.Close();
                conexion.Close();
                return lista;
            }
            catch (Exception e)
            {
                if (!EventLog.SourceExists("IUS"))
                {
                    EventLog.CreateEventSource("IUS", "IUS");
                }
                EventLog Logg = new EventLog("IUS");
                Logg.Source = "IUS";
                String mensaje = "TesisDAOImpl Exception at getRelacionFraseArticuo(RelacionFraseArticuloTO)\n" + e.Message + e.StackTrace;
                Logg.WriteEntry(mensaje);
                Logg.Close();
                conexion.Close();
                return new List<RelacionFraseArticulosTO>();
            }
        }

        public List<RelacionFraseArticulosTO> getRelacionFraseArticulosEst(
            RelacionFraseArticulosTO idRelIus)
        {
            DbConnection conexion = contextoBD.ContextConection;
            try
            {
                String sqlQuery = " select ius, idRel, idLey, idArt, idRef, consec from RelFraseArtsEstatal" +
                                  " where ius = " + idRelIus.getIus() + " AND idrel = " + idRelIus.getIdRel() + " order by consec asc";

                DataAdapter query = contextoBD.dataAdapter(sqlQuery, conexion);
                DataSet datos = new DataSet();
                query.Fill(datos);
                List<RelacionFraseArticulosTO> lista = new List<RelacionFraseArticulosTO>();
                DataTableReader tabla = datos.Tables[0].CreateDataReader();

                //foreach (DataRow fila in tabla.Rows)
                while (tabla.Read())
                {
                    RelacionFraseArticulosTO relacionActual = new RelacionFraseArticulosTO();
                    relacionActual.setIdRel("" + tabla["idRel"]);
                    relacionActual.setIus("" + tabla["ius"]);
                    relacionActual.setIdLey("" + tabla["idLey"]);
                    relacionActual.setIdArt("" + tabla["idArt"]);
                    relacionActual.setIdRef("" + tabla["idRef"]);
                    relacionActual.setConsec("" + tabla["consec"]);
                    lista.Add(relacionActual);
                }
                tabla.Close();
                conexion.Close();
                return lista;
            }
            catch (Exception e)
            {
                if (!EventLog.SourceExists("IUS"))
                {
                    EventLog.CreateEventSource("IUS", "IUS");
                }
                EventLog Logg = new EventLog("IUS");
                Logg.Source = "IUS";
                String mensaje = "TesisDAOImpl Exception at getRelacionFraseArticuo(RelacionFraseArticuloTO)\n" + e.Message + e.StackTrace;
                Logg.WriteEntry(mensaje);
                Logg.Close();
                conexion.Close();
                return new List<RelacionFraseArticulosTO>();
            }
        }

        public List<RelacionFraseTesisTO> getRelacionFraseTesis(
            RelacionFraseTesisTO idRelIus)
        {
            DbConnection conexion = contextoBD.ContextConection;
            try
            {
                String sqlQuery = " select tipo, ius, idRel, idLink, consec from relfrasetesis" +
                                  " where ius = " + idRelIus.getIus() + " AND idrel = " + idRelIus.getIdRel() + " order by consec asc";
                DataAdapter query = contextoBD.dataAdapter(sqlQuery, conexion);
                DataSet datos = new DataSet();
                query.Fill(datos);
                List<RelacionFraseTesisTO> lista = new List<RelacionFraseTesisTO>();
                DataTableReader tabla = datos.Tables[0].CreateDataReader();

                //foreach (DataRow fila in tabla.Rows)
                while (tabla.Read())
                {
                    RelacionFraseTesisTO relacionActual = new RelacionFraseTesisTO();
                    relacionActual.setIdRel("" + tabla["idRel"]);
                    relacionActual.setTipo("" + tabla["tipo"]);
                    relacionActual.setIus("" + tabla["ius"]);
                    relacionActual.setIdLink("" + tabla["idLink"]);
                    relacionActual.setConsec("" + tabla["consec"]);
                    lista.Add(relacionActual);
                }
                tabla.Close();

                conexion.Close();
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
                String mensaje = "TesisDAOImpl Exception at getRelacionFraseTesis(RelacionFraseTesisTO)\n" + e.Message + e.StackTrace;
                Logg.WriteEntry(mensaje);
                Logg.Close();
                return new List<RelacionFraseTesisTO>();
            }
        }

        public GenealogiaTO getGenealogia(String id)
        {
            DbConnection conexion = contextoBD.ContextConection;
            try
            {
                String sqlQuery = " select idGenealogia, txtGenealogia " +
                                  " from genealogia " +
                                  " where idGenealogia=" + id;
                DataAdapter query = contextoBD.dataAdapter(sqlQuery, conexion);
                DataSet datos = new DataSet();
                query.Fill(datos);
                List<GenealogiaTO> lista = new List<GenealogiaTO>();
                DataTableReader tabla = datos.Tables[0].CreateDataReader();

                //foreach (DataRow fila in tabla.Rows)
                while (tabla.Read())
                {
                    GenealogiaTO relacionActual = new GenealogiaTO();
                    relacionActual.setIdGenealogia("" + tabla["idGenealogia"]);
                    relacionActual.setTxtGenealogia((String)tabla["txtGenealogia"]);
                    lista.Add(relacionActual);
                }
                tabla.Close();

                conexion.Close();
                return lista[0];
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
                String mensaje = "TesisDAOImpl Exception at getGenealogia(String)\n" + e.Message + e.StackTrace;
                Logg.WriteEntry(mensaje);
                Logg.Close();
                return new GenealogiaTO();
            }
        }

        public /*List<TesisTO>*/ DataTableReader getTesis(MostrarPorIusTO identificadores)
        {
            DbConnection conexion = contextoBD.ContextConection;
            try
            {
                String sqlQuery = "        select A.Parte,   A.IUS,     A.Rubro," + //     B.descEpoca As epoca," +
                                  "               A.Tesis,     A.LocAbr, A.ConsecIndx, A.ta_tj,      A.tpoTesis, " +
                                  "               D.imageWeb,           D.descripcion As descTpoTesis, D.imageOther," +
                                  "               A.ordenInstancia, A.ordenTesis, A.ordenRubro, " +
                                  " A.TpoAsunto1, A.TpoAsunto2, A.TpoAsunto3, A.TpoAsunto4, A.TpoAsunto5, " +
                                  " A.tpoPonente1, A.tpoPonente2, A.tpoPonente3, A.tpoPonente4, A.tpoPonente5, " +
                                  " A.tpoPon1, A.tpoPon2, A.tpoPon3, A.tpoPon4, A.tpoPon5 " +
                                  " from Tesis As A, ctpoTesis As D where  " +
                                  "        A.tpoTesis = D.id " +
                                  "        AND ius IN (";
                foreach (int item in identificadores.getListado())
                {
                    sqlQuery += item + ",";
                }
                sqlQuery = sqlQuery.Substring(0, sqlQuery.Length - 1);
                sqlQuery += ") order by " + identificadores.getOrderBy() + " " + identificadores.getOrderType();
                DataAdapter query = contextoBD.dataAdapter(sqlQuery, conexion);
                DataSet datos = new DataSet();
                query.Fill(datos);
                DataTableReader tabla = datos.Tables[0].CreateDataReader();
                conexion.Close();
                return tabla;
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
                String mensaje = "TesisDAOImpl Exception at getTesis(MostrarPorIusTO)\n" + e.Message + e.StackTrace;
                Logg.WriteEntry(mensaje);
                Logg.Close();
                return (new DataTable()).CreateDataReader();// List<TesisTO>();
            }
        }

        public List<TesisTO> getIusTesis(MostrarPorIusTO identificadores)
        {
            DbConnection conexion = contextoBD.ContextConection;
            try
            {
                String sqlQuery = "select " +
                                  "IUS, ConsecIndx, OrdenInstancia, OrdenRubro, OrdenTesis, tpoTesis," +
                                  " TpoAsunto1, TpoAsunto2, TpoAsunto3, TpoAsunto4, TpoAsunto5, " +
                                  " tpoPonente1, tpoPonente2, tpoPonente3, tpoPonente4, tpoPonente5, " +
                                  " tpoPon1, tpoPon2, tpoPon3, tpoPon4, tpoPon5 " +
                                  identificadores.OrderBy + " from tesis where ius IN (";
                HashSet<String> listadocompleto = new HashSet<String>();
                foreach (int item in identificadores.Listado)
                {
                    listadocompleto.Add("" + item);
                }
                List<String> listaFinal = new List<string>();
                foreach (String item in listadocompleto)
                {
                    listaFinal.Add(item);
                }
                sqlQuery += String.Join(",", listaFinal.ToArray());
                sqlQuery += ") order by " + identificadores.getOrderBy() + " " + identificadores.getOrderType();
                DataAdapter query = contextoBD.dataAdapter(sqlQuery, conexion);
                DataSet datos = new DataSet();
                query.Fill(datos);
                DataTableReader tabla = datos.Tables[0].CreateDataReader();

                List<TesisTO> lista = new List<TesisTO>();
                //foreach (DataRow fila in tabla.Rows)
                while (tabla.Read())
                {
                    TesisTO tesisActual = new TesisTO();
                    tesisActual.setIus("" + tabla["ius"]);
                    tesisActual.ConsecIndx = "" + tabla["ConsecIndx"];
                    tesisActual.OrdenInstancia = (int)tabla["OrdenInstancia"];
                    tesisActual.OrdenTesis = (int)tabla["OrdenTesis"];
                    tesisActual.OrdenRubro = (int)tabla["OrdenRubro"];
                    tesisActual.TpoTesis = "" + tabla["TpoTesis"];
                    tesisActual.Ponentes = new int[5];
                    tesisActual.Ponentes[0] = (short)tabla["tpoPonente1"];
                    tesisActual.Ponentes[1] = (short)tabla["tpoPonente2"];
                    tesisActual.Ponentes[2] = (short)tabla["tpoPonente3"];
                    tesisActual.Ponentes[3] = (short)tabla["tpoPonente4"];
                    tesisActual.Ponentes[4] = (short)tabla["tpoPonente5"];
                    tesisActual.TipoPonente = new int[5];
                    tesisActual.TipoPonente[0] = (short)tabla["tpoPon1"];
                    tesisActual.TipoPonente[1] = (short)tabla["tpoPon2"];
                    tesisActual.TipoPonente[2] = (short)tabla["tpoPon3"];
                    tesisActual.TipoPonente[3] = (short)tabla["tpoPon4"];
                    tesisActual.TipoPonente[4] = (short)tabla["tpoPon5"];
                    tesisActual.TipoTesis = new int[5];
                    tesisActual.TipoTesis[0] = (short)tabla["TpoAsunto1"];
                    tesisActual.TipoTesis[1] = (short)tabla["TpoAsunto2"];
                    tesisActual.TipoTesis[2] = (short)tabla["TpoAsunto3"];
                    tesisActual.TipoTesis[3] = (short)tabla["TpoAsunto4"];
                    tesisActual.TipoTesis[4] = (short)tabla["TpoAsunto5"];
                    lista.Add(tesisActual);
                }
                tabla.Close();

                conexion.Close();
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
                String mensaje = "TesisDAOImpl Exception at getIusTesis(MostrarPorIusTO)\n" + e.Message + e.StackTrace;
                Logg.WriteEntry(mensaje);
                Logg.Close();
                return new List<TesisTO>();
            }
        }

        public List<OtrosTextosTO> getOtrosTextosPorIus(String ius)
        {
            DbConnection conexion = contextoBD.ContextConection;
            try
            {
                String sqlQuery = " select ius, tipoNota, textos from otrosTextos " +
                                  " where ius = " + ius;
                DataAdapter query = contextoBD.dataAdapter(sqlQuery, conexion);
                DataSet datos = new DataSet();
                query.Fill(datos);
                DataTableReader tabla = datos.Tables[0].CreateDataReader();

                List<OtrosTextosTO> lista = new List<OtrosTextosTO>();
                //foreach (DataRow fila in tabla.Rows)
                while (tabla.Read())
                {
                    OtrosTextosTO textosActual = new OtrosTextosTO();
                    textosActual.setIus("" + tabla["ius"]);
                    textosActual.setTipoNota("" + tabla["tipoNota"]);
                    textosActual.setTextos("" + tabla["textos"]);
                    lista.Add(textosActual);
                }
                tabla.Close();

                conexion.Close();
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
                String mensaje = "TesisDAOImpl Exception at getOtrosTextosPorIus(String)\n" + e.Message + e.StackTrace;
                Logg.WriteEntry(mensaje);
                Logg.Close();
                return new List<OtrosTextosTO>();
            }
        }

        public List<OtrosTextosTO> getNotasContradiccionesPorIus(String ius)
        {
            DbConnection conexion = contextoBD.ContextConection;
            try
            {
                String sqlQuery = " select ius, tipo, regRef, version, txtNota from zzNotasContradiccionTesis where ius = " + ius;
                DataAdapter query = contextoBD.dataAdapter(sqlQuery, conexion);
                DataSet datos = new DataSet();
                query.Fill(datos);
                DataTableReader tabla = datos.Tables[0].CreateDataReader();

                List<OtrosTextosTO> lista = new List<OtrosTextosTO>();
                //foreach (DataRow fila in tabla.Rows)
                while (tabla.Read())
                {
                    OtrosTextosTO textosActual = new OtrosTextosTO();
                    textosActual.setIus("" + tabla["ius"]);
                    textosActual.setTipoNota("" + tabla["tipo"]);
                    textosActual.setTextos("" + tabla["regRef"]);
                    textosActual.TxtNotas = DBNull.Value.Equals(tabla["txtNota"]) ? null : (String)tabla["txtNota"];
                    textosActual.version = (byte)tabla["version"];
                    lista.Add(textosActual);
                }
                tabla.Close();

                conexion.Close();
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
                String mensaje = "TesisDAOImpl Exception at getTesisContradiccionesPorIUS(String)\n" + e.Message + e.StackTrace;
                Logg.WriteEntry(mensaje);
                Logg.Close();
                return new List<OtrosTextosTO>();
            }
        }

        public List<RelDocumentoTesisTO> getRelEjecutorias(String ius)
        {
            DbConnection conexion = contextoBD.ContextConection;
            try
            {
                String sqlQuery = " select ius, idPte, cve from relPartes where ius=" + ius + " and cve=2 ";
                DataAdapter query = contextoBD.dataAdapter(sqlQuery, conexion);
                DataSet datos = new DataSet();
                query.Fill(datos);
                DataTableReader tabla = datos.Tables[0].CreateDataReader();

                List<RelDocumentoTesisTO> lista = new List<RelDocumentoTesisTO>();
                //foreach (DataRow fila in tabla.Rows)
                while (tabla.Read())
                {
                    RelDocumentoTesisTO textosActual = new RelDocumentoTesisTO();
                    textosActual.setIus("" + tabla["ius"]);
                    textosActual.setId("" + tabla["idPte"]);
                    textosActual.setTpoRel("" + tabla["cve"]);
                    lista.Add(textosActual);
                }
                tabla.Close();

                conexion.Close();
                return lista;
            }
            catch (Exception e)
            {
                if (!EventLog.SourceExists("IUS"))
                {
                    EventLog.CreateEventSource("IUS", "IUS");
                }
                EventLog Logg = new EventLog("IUS");
                Logg.Source = "IUS";
                String mensaje = "TesisDAOImpl Exception at getRelEjecutorias(String)\n" + e.Message + e.StackTrace;
                Logg.WriteEntry(mensaje);
                Logg.Close();
                return new List<RelDocumentoTesisTO>();
            }
        }

        public List<RelDocumentoTesisTO> getRelVotos(String ius)
        {
            DbConnection conexion = contextoBD.ContextConection;
            try
            {
                String sqlQuery = " select ius, idPte, cve from relPartes where ius=" + ius + " and cve=3 ";
                DataAdapter query = contextoBD.dataAdapter(sqlQuery, conexion);
                DataSet datos = new DataSet();
                query.Fill(datos);
                DataTableReader tabla = datos.Tables[0].CreateDataReader();

                List<RelDocumentoTesisTO> lista = new List<RelDocumentoTesisTO>();
                //foreach (DataRow fila in tabla.Rows)
                while (tabla.Read())
                {
                    RelDocumentoTesisTO textosActual = new RelDocumentoTesisTO();
                    textosActual.setIus("" + tabla["ius"]);
                    textosActual.setId("" + tabla["idPte"]);
                    textosActual.setTpoRel("" + tabla["cve"]);
                    lista.Add(textosActual);
                }
                tabla.Close();

                conexion.Close();
                return lista;
            }
            catch (Exception e)
            {
                if (!EventLog.SourceExists("IUS"))
                {
                    EventLog.CreateEventSource("IUS", "IUS");
                }
                EventLog Logg = new EventLog("IUS");
                Logg.Source = "IUS";
                String mensaje = "TesisDAOImpl Exception at getRelVotos(String)\n" + e.Message + e.StackTrace;
                Logg.WriteEntry(mensaje);
                Logg.Close();
                return new List<RelDocumentoTesisTO>();
            }
        }

        public List<TesisTO> getIusPorPalabra(BusquedaTO parametros)
        {
            BooleanQuery.SetMaxClauseCount(IUSConstants.CLAUSULAS);

            SpanishAnalyzer analyzer = new SpanishAnalyzer();
            List<String> querysEscritos = new List<string>();
            String queryEpocas = "";
            BooleanQuery QueryCompleto = new BooleanQuery();
            BooleanQuery queryGlobal = new BooleanQuery();
            IndexSearcher searcher = new IndexSearcher(IUSConstants.DIRECCION_INDEXER);
            if ((parametros.OrdenarPor == null) || (parametros.OrdenarPor.Equals(IUSConstants.ORDER_DEFAULT)))
            {
                parametros.OrdenarPor = IUSConstants.ORDER_RUBRO;
            }
            String ordenarPor = parametros.OrdenarPor == null ? IUSConstants.ORDER_DEFAULT : parametros.OrdenarPor;
            List<TesisTO> resultado = new List<TesisTO>();
            int[] partes = obtenPartes(parametros);
            String query = "";
            List<String> numeros = new List<string>();
            foreach (int item in partes)
            {
                query = "" + item;
                numeros.Add(query);
            }
            queryEpocas = String.Join(" OR ", numeros.ToArray());
            queryEpocas = "(parte: (" + queryEpocas + "))";
            List<BusquedaPalabraTO> listado = FuncionesGenerales.GeneralizaBusqueda(parametros.Palabra);
            foreach (BusquedaPalabraTO palabras in listado)
            {
                querysEscritos.Add(obtenQueryPalabras(palabras, queryEpocas));
            }
            String queryTotal = "(";
            int primero = 0;
            int excedentes = 0;
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
                    else if ((listado.ElementAt(primero).ValorLogico == IUSConstants.BUSQUEDA_PALABRA_OP_Y) ||
                             (listado.ElementAt(primero).ValorLogico == 0))
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
                    queryTotal = "((" + queryTotal + ")" + operador + "(" + item + ")";
                    excedentes++;
                }
                primero++;
            }
            for (int cerrarPar = 0; cerrarPar < (excedentes + 1); cerrarPar++)
            {
                queryTotal += ")";
            }

            QueryParser queryGeneral = new QueryParser("", analyzer);
            Hits hits = null;
            try
            {
                queryGlobal.Add(queryGeneral.Parse(queryTotal), BooleanClause.Occur.SHOULD);
                hits = searcher.Search(queryGlobal, new Sort(ordenarPor));
            }
            catch (Exception e)
            {
                BooleanClause[] clausulas = queryGlobal.GetClauses();
                if (!EventLog.SourceExists("IUS"))
                {
                    EventLog.CreateEventSource("IUS", "IUS");
                }
                EventLog Logg = new EventLog("IUS");
                Logg.Source = "IUS";
                String mensaje = "TesisDAOImpl Exception at getIusPorPalabra(BusquedaTO)\n" + e.Message + e.StackTrace + "\n Query:\n" + queryTotal;
                Logg.WriteEntry(mensaje);
                Logg.Close();
            }
            int itemActual = 0;
            for (itemActual = 0; itemActual < hits.Length(); itemActual++)
            {
                Document item = hits.Doc(itemActual);
                TesisTO itemVerdadero;
                //try
                //{
                //    itemVerdadero = tesisCompletas[Int32.Parse(item.Get("consecIndx"))];// new TesisTO();
                //}
                //catch (Exception e)
                //{
                itemVerdadero = new TesisTO();
                itemVerdadero.Ius = item.Get("ius");
                itemVerdadero.OrdenInstancia = long.Parse(item.Get("OrdenInstancia"));
                itemVerdadero.OrdenTesis = long.Parse(item.Get("OrdenTesis"));
                itemVerdadero.OrdenRubro = long.Parse(item.Get("OrdenRubro"));
                itemVerdadero.ConsecIndx = item.Get("consecIndx");
                itemVerdadero.ConsecIndxInt = Int32.Parse(item.Get("consecIndx"));
                itemVerdadero.TpoTesis = item.Get("tpoTesis");
                itemVerdadero.TipoTesis = new int[5];
                itemVerdadero.TipoTesis[0] = int.Parse(item.Get("tpoAsunto1"));
                itemVerdadero.TipoTesis[1] = int.Parse(item.Get("tpoAsunto2"));
                itemVerdadero.TipoTesis[2] = int.Parse(item.Get("tpoAsunto3"));
                itemVerdadero.TipoTesis[3] = int.Parse(item.Get("tpoAsunto4"));
                itemVerdadero.TipoTesis[4] = int.Parse(item.Get("tpoAsunto5"));
                itemVerdadero.Ponentes = new int[5];
                itemVerdadero.Ponentes[0] = int.Parse(item.Get("TpoPonente1"));
                itemVerdadero.Ponentes[1] = int.Parse(item.Get("TpoPonente2"));
                itemVerdadero.Ponentes[2] = int.Parse(item.Get("TpoPonente3"));
                itemVerdadero.Ponentes[3] = int.Parse(item.Get("TpoPonente4"));
                itemVerdadero.Ponentes[4] = int.Parse(item.Get("TpoPonente5"));
                itemVerdadero.TipoPonente = new int[5];
                itemVerdadero.TipoPonente[0] = int.Parse(item.Get("TpoPon1"));
                itemVerdadero.TipoPonente[1] = int.Parse(item.Get("TpoPon2"));
                itemVerdadero.TipoPonente[2] = int.Parse(item.Get("TpoPon3"));
                itemVerdadero.TipoPonente[3] = int.Parse(item.Get("TpoPon4"));
                itemVerdadero.TipoPonente[4] = int.Parse(item.Get("TpoPon5"));
                //}
                resultado.Add(itemVerdadero);
            }
            searcher.Close();
            return resultado;
        }

        public PaginadorTO getIusPorPalabraPaginador(BusquedaTO parametros, Dictionary<int, int[]> secciones)
        {
            BooleanQuery.SetMaxClauseCount(IUSConstants.CLAUSULAS);
            SpanishAnalyzer analyzer = new SpanishAnalyzer();
            List<String> querysEscritos = new List<string>();
            String queryEpocas = "";
            BooleanQuery QueryCompleto = new BooleanQuery();
            BooleanQuery queryGlobal = new BooleanQuery();
            IndexSearcher searcher = null;
            while (searcher == null)
            {
                try
                {
                    searcher = new IndexSearcher(IUSConstants.DIRECCION_INDEXER);
                }
                catch (Exception exc)
                {
                    if (!EventLog.SourceExists("IUS"))
                    {
                        EventLog.CreateEventSource("IUS", "IUS");
                    }
                    EventLog Logg = new EventLog("IUS");
                    Logg.Source = "IUS";
                    String mensaje = "TesisDAOImpl Exception at getIusPorPalabraPaginador(BusquedaTO)\nProblemas con los archivos temporales:\n" + exc.Message + exc.StackTrace;
                    Logg.WriteEntry(mensaje);
                    Logg.Close();
                }
            }
            if ((parametros.OrdenarPor == null) || (parametros.OrdenarPor.Equals(IUSConstants.ORDER_DEFAULT)))
            {
                parametros.OrdenarPor = "consecIndx";// IUSConstants.ORDER_RUBRO;
            }
            String ordenarPor = parametros.OrdenarPor == null ? IUSConstants.ORDER_DEFAULT : parametros.OrdenarPor;
            PaginadorTO resultado = new PaginadorTO();
            int[] partes = obtenPartes(parametros);
            String query = "";
            List<String> numeros = new List<string>();
            foreach (int item in partes)
            {
                query = "" + item;
                numeros.Add(query);
            }
            queryEpocas = String.Join(" OR ", numeros.ToArray());
            queryEpocas = "(parte: (" + queryEpocas + "))";
            List<BusquedaPalabraTO> listado = FuncionesGenerales.GeneralizaBusqueda(parametros.Palabra);
            foreach (BusquedaPalabraTO palabras in listado)
            {
                querysEscritos.Add(obtenQueryPalabras(palabras, queryEpocas));
            }
            String queryTotal = "(";
            int primero = 0;
            int excedentes = 0;
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
                    else if ((listado.ElementAt(primero).ValorLogico == IUSConstants.BUSQUEDA_PALABRA_OP_Y) ||
                             (listado.ElementAt(primero).ValorLogico == 0))
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
                    queryTotal = "((" + queryTotal + ")" + operador + "(" + item + ")";
                    excedentes++;
                }
                primero++;
            }
            for (int cerrarPar = 0; cerrarPar < (excedentes + 1); cerrarPar++)
            {
                queryTotal += ")";
            }

            QueryParser queryGeneral = new QueryParser("", analyzer);
            Hits hits = null;
            try
            {
                queryGlobal.Add(queryGeneral.Parse(queryTotal), BooleanClause.Occur.SHOULD);
                hits = searcher.Search(queryGlobal, new Sort(ordenarPor));
            }
            catch (Exception e)
            {
                BooleanClause[] clausulas = queryGlobal.GetClauses();
                if (!EventLog.SourceExists("IUS"))
                {
                    EventLog.CreateEventSource("IUS", "IUS");
                }
                EventLog Logg = new EventLog("IUS");
                Logg.Source = "IUS";
                String mensaje = "TesisDAOImpl Exception at getIusPorpalabraPaginador(BusquedaTO)\n" + e.Message + e.StackTrace;
                Logg.WriteEntry(mensaje);
                Logg.Close();
            }
            int itemActual = 0;
            List<Int32> ids = new List<int>();
            for (itemActual = 0; itemActual < hits.Length(); itemActual++)
            {
                Document item = hits.Doc(itemActual);
                Int32 itemVerdadero;
                itemVerdadero = Int32.Parse(item.Get("ius"));
                Int32 parte = Int32.Parse(item.Get("parte"));
                Int32 tribunal = Int32.Parse(item.Get("Instancia"));
                Int32 seccionTesis = Int32.Parse(item.Get("seccion"));
                if (((parte % 7) == 6) && (parte < 100))
                {
                    if ((parametros.Tribunales == null) || (parametros.Tribunales[0] == -1))
                    {
                        ids.Add(itemVerdadero);
                    }
                    else
                    {
                        if (parametros.Tribunales.Contains(tribunal))
                        {
                            ids.Add(itemVerdadero);
                        }
                    }
                }
                else if (parte > 139 && parte < 148)
                {
                    if (secciones.Keys.Contains(parte))
                    {
                        int[] seccionesAcomparar = secciones[parte];
                        if (seccionesAcomparar.Contains(seccionTesis) || seccionesAcomparar[1] == -1)
                            ids.Add(itemVerdadero);
                    }
                }
                else
                {
                    ids.Add(itemVerdadero);
                }
            }
            try
            {
                searcher.Close();
            }
            catch (Exception e)
            {
                /** Pueden ocurrir errores por el manejo de los archivos temporales*/
                if (!EventLog.SourceExists("IUS"))
                {
                    EventLog.CreateEventSource("IUS", "IUS");
                }
                EventLog Logg = new EventLog("IUS");
                Logg.Source = "IUS";
                String mensaje = "TesisDAOImpl Exception at getIusPorPalabraPaginador(BusquedaTO)\nProblema con los temporales:\n" + e.Message + e.StackTrace;
                Logg.WriteEntry(mensaje);
                Logg.Close();
            }
            resultado.TipoBusqueda = parametros.TipoBusqueda;
            resultado.TimeStamp = DateTime.Now;
            resultado.Activo = true;
            resultado.Largo = ids.Count;
            resultado.ResultadoIds = ids;
            #if RED_JUR
            GuardarPaginador(resultado);
            #else
            ConsultasActuales.Add(resultado.Id, resultado);
            #endif
            return resultado;
        }

        /// <summary>
        ///      Genera un query por cada expresión que se almacenó en la lista de palabras
        ///      para que todos estos al final sean ligados en uno solo
        /// </summary>
        /// <param name="palabras" type="mx.gob.scjn.ius_common.TO.BusquedaPalabraTO">
        ///     <para>
        ///         La expresión de búsqueda que se quiere encontrar
        ///     </para>
        /// </param>
        /// <returns>
        ///     El query de Lucene a buscar.
        /// </returns>
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
                                //if(queryPalabrasFrases.StartsWith(" AND (")
                                queryPalabrasFrases = "(" + queryPalabrasFrases;
                            }
                            queryPalabrasFrases = completaParentesis(queryPalabrasFrases);
                            break;
                    }
                    //queryPalabrasFrasesTemp = completaParentesis(queryPalabrasFrasesTemp);
                }
                queryPalabrasFrases = prefijo + queryPalabrasFrases + ")";
                //queryPalabrasFrases = queryPalabrasFrases.Substring(0, queryPalabrasFrases.Length - 5);
                queryPalabrasFrases = completaParentesis(queryPalabrasFrases);
                queryActual = queryActual + queryPalabrasFrases + ")";
                return queryActual;
            }
            else
            {
                ///Se trata de una búsqueda Almacenada que hay que obtener para integrar su
                ///propio query
                ///
                IUSApplicationContext contexto = new IUSApplicationContext();
                GuardarExpresionDAO guardarExpresion = (GuardarExpresionDAO)contexto.GetObject("GuardarExpresionDAO");
                BusquedaAlmacenadaTO busquedaAlmacenada = guardarExpresion.ObtenBusqueda(palabras.Campos[0]);
                if (
                    (busquedaAlmacenada.TipoBusqueda == IUSConstants.BUSQUEDA_TESIS_SIMPLE) ||
                    (busquedaAlmacenada.TipoBusqueda == IUSConstants.BUSQUEDA_PALABRA_TESIS))
                {
                    if (busquedaAlmacenada.Tribunales == null || busquedaAlmacenada.Tribunales.Length == 0)
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
                        int contador = 0;
                        foreach (ExpresionBusqueda itemBusquedaA in busquedaAlmacenada.Expresiones)
                        {
                            BusquedaPalabraTO expresionAObtener = new BusquedaPalabraTO();
                            expresionAObtener.Campos = itemBusquedaA.Campos;
                            expresionAObtener.Expresion = itemBusquedaA.Expresion;
                            expresionAObtener.Jurisprudencia = itemBusquedaA.IsJuris;
                            expresionAObtener.ValorLogico = itemBusquedaA.Operador;
                            if (contador > 0)
                            {
                                String Conector = String.Empty;
                                switch (expresionAObtener.ValorLogico)
                                {
                                    case IUSConstants.BUSQUEDA_PALABRA_OP_Y:
                                        Conector = " AND ";
                                        break;
                                    case IUSConstants.BUSQUEDA_PALABRA_OP_O:
                                        Conector = " OR ";
                                        break;
                                    case IUSConstants.BUSQUEDA_PALABRA_OP_NO:
                                        Conector = " NOT ";
                                        break;
                                }
                                query = "(" + query + Conector + obtenQueryPalabras(expresionAObtener, queryEpocas);
                            }
                            else
                            {
                                query = query + " " + obtenQueryPalabras(expresionAObtener, queryEpocas);
                            }
                            contador++;
                        }
                        query = completaParentesis(query);
                        return query;
                    }
                    else
                    {
                        /// Hay tribunales en la búsqueda, por lo que se complica esta
                        queryEpocas = "(";
                        List<int> epocas = busquedaAlmacenada.Epocas.ToList();
                        List<int> epocasConTribunales = new List<int>();
                        String queryT = String.Empty;
                        String query = String.Empty;
                        List<int> epocasSinTribunales = new List<int>();
                        foreach (int itemepocas in epocas)
                        {
                            if (itemepocas % 7 == 6 && itemepocas < 49)// TCC es la 7a columna, pero se empieza en 0, por eso es módulo 6s
                            {
                                epocasConTribunales.Add(itemepocas);
                            }
                            else
                            {
                                epocasSinTribunales.Add(itemepocas);
                            }
                        }
                        if (epocasSinTribunales.Count > 0)
                        {
                            foreach (int itemEpoca in epocasSinTribunales)
                            {
                                queryEpocas += itemEpoca + " OR ";
                            }
                            queryEpocas = queryEpocas.Substring(0, queryEpocas.Length - 4) + ")";
                            queryEpocas = "(parte: (" + queryEpocas + "))";

                            query = "";
                            int contador = 0;
                            foreach (ExpresionBusqueda itemBusquedaA in busquedaAlmacenada.Expresiones)
                            {
                                BusquedaPalabraTO expresionAObtener = new BusquedaPalabraTO();
                                expresionAObtener.Campos = itemBusquedaA.Campos;
                                expresionAObtener.Expresion = itemBusquedaA.Expresion;
                                expresionAObtener.Jurisprudencia = itemBusquedaA.IsJuris;
                                expresionAObtener.ValorLogico = itemBusquedaA.Operador;
                                if (contador > 0)
                                {
                                    String Conector = String.Empty;
                                    switch (expresionAObtener.ValorLogico)
                                    {
                                        case IUSConstants.BUSQUEDA_PALABRA_OP_Y:
                                            Conector = " AND ";
                                            break;
                                        case IUSConstants.BUSQUEDA_PALABRA_OP_O:
                                            Conector = " OR ";
                                            break;
                                        case IUSConstants.BUSQUEDA_PALABRA_OP_NO:
                                            Conector = " NOT ";
                                            break;
                                    }
                                    query = "(" + query + Conector + obtenQueryPalabras(expresionAObtener, queryEpocas);
                                }
                                else
                                {
                                    query = query + " " + obtenQueryPalabras(expresionAObtener, queryEpocas);
                                }
                                contador++;
                            }
                            query = completaParentesis(query);
                        }
                        if (epocasConTribunales.Count > 0)
                        {
                            // Ahora hay que añadir el pedazo correspondiente a lo que si tiene tribunales
                            String queryEpocasTribunales = "(";

                            foreach (int itemEpoca in epocasConTribunales)
                            {
                                queryEpocasTribunales += itemEpoca + " OR ";
                            }
                            queryEpocasTribunales = queryEpocasTribunales.Substring(0, queryEpocasTribunales.Length - 4) + ")";
                            queryEpocasTribunales = "(parte: (" + queryEpocasTribunales + "))";
                            String QTribunales = "(";
                            if (busquedaAlmacenada.Tribunales != null &&
                                busquedaAlmacenada.Tribunales.Length > 0 &&
                                busquedaAlmacenada.Tribunales[0] != -1)
                            {
                                foreach (int itemTribunal in busquedaAlmacenada.Tribunales)
                                {
                                    QTribunales += itemTribunal + " OR ";
                                }
                                QTribunales = QTribunales.Substring(0, QTribunales.Length - 4) + ")";
                                QTribunales = "(Instancia:(" + QTribunales + "))";
                                queryEpocasTribunales = "((" + queryEpocasTribunales + ") AND (" + QTribunales + "))";
                            }
                            
                            int contadorT = 0;
                            foreach (ExpresionBusqueda itemBusquedaA in busquedaAlmacenada.Expresiones)
                            {
                                BusquedaPalabraTO expresionAObtener = new BusquedaPalabraTO();
                                expresionAObtener.Campos = itemBusquedaA.Campos;
                                expresionAObtener.Expresion = itemBusquedaA.Expresion;
                                expresionAObtener.Jurisprudencia = itemBusquedaA.IsJuris;
                                expresionAObtener.ValorLogico = itemBusquedaA.Operador;
                                if (contadorT > 0)
                                {
                                    String Conector = String.Empty;
                                    switch (expresionAObtener.ValorLogico)
                                    {
                                        case IUSConstants.BUSQUEDA_PALABRA_OP_Y:
                                            Conector = " AND ";
                                            break;
                                        case IUSConstants.BUSQUEDA_PALABRA_OP_O:
                                            Conector = " OR ";
                                            break;
                                        case IUSConstants.BUSQUEDA_PALABRA_OP_NO:
                                            Conector = " NOT ";
                                            break;
                                    }
                                    queryT = "(" + queryT + Conector + obtenQueryPalabras(expresionAObtener, queryEpocasTribunales);
                                }
                                else
                                {
                                    queryT = queryT + " " + obtenQueryPalabras(expresionAObtener, queryEpocasTribunales);
                                }
                                contadorT++;
                            }
                            queryT = completaParentesis(queryT);
                        }
                        String resultado = String.Empty;
                        if ((!query.Equals(String.Empty)) && (!queryT.Equals(String.Empty)))
                        {
                            resultado = "((" + query + ")OR(" + queryT + "))";
                        }
                        else if (!query.Equals(String.Empty))
                            resultado = query;
                        else
                            resultado = queryT;
                        return resultado;
                    }
                }
                else
                {
                    return "(idProg: " + busquedaAlmacenada.ValorBusqueda.Replace(" &&& ", "") + ")";
                }
                return "";
            }
        }

        /// <summary>
        /// Verifica que todo paréntesis que se abra también se cierre
        /// </summary>
        /// <param name="queryPalabrasFrasesTemp">La expresión con paréntesis</param>
        /// <returns>La expresión con los paréntesis completos</returns>
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

        /// <summary>
        /// Genera un query con un determinado parámetro  de búsqueda para una expresión de la que se requiere buscar en Lucene
        /// </summary>
        /// <param name="p">palabras o expresión a buscar</param>
        /// <param name="campos">Los campos en los que se va a buscar</param>
        /// <returns>El query de Lucene para realizar la búsqueda</returns>
        private List<string> ObtenQuery(String p, BusquedaPalabraTO campos)
        {
            List<String> listaResultado = new List<string>();
            List<String> resultado = new List<string>();// obtenPalabras(p);
            String parametro = p;
            parametro = parametro.Trim();
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
                if ((!item.Equals(" NOT ")) && (!item.Equals(" AND ")) &&
                    (!item.Equals(" OR ")))
                {
                    if (regreso.Equals(""))
                    {
                        regreso = "(" + regreso + "( ";
                        if (campos.Campos.Contains(IUSConstants.BUSQUEDA_PALABRA_CAMPO_TEXTO))
                        {
                            regreso = regreso + "texto:(" + item + ")";
                        }
                        if (campos.Campos.Contains(IUSConstants.BUSQUEDA_PALABRA_CAMPO_RUBRO))
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
                        if (campos.Campos.Contains(IUSConstants.BUSQUEDA_PALABRA_CAMPO_PRECE))
                        {
                            if (regreso.EndsWith(")"))
                            {
                                regreso = regreso + " OR precedentes:(" + item + ")";
                            }
                            else
                            {
                                regreso = regreso + "precedentes:(" + item + ")";
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
                        if (campos.Campos.Contains(IUSConstants.BUSQUEDA_PALABRA_CAMPO_RUBRO))
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
                        if (campos.Campos.Contains(IUSConstants.BUSQUEDA_PALABRA_CAMPO_PRECE))
                        {
                            if (regreso.EndsWith(")"))
                            {
                                regreso = regreso + " OR precedentes:(" + item + ")";
                            }
                            else
                            {
                                regreso = regreso + "precedentes:(" + item + ")";
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
                    //regreso = regreso.Substring(0, regreso.Length - 5);
                    //regreso = "("+regreso + ") " + item + "(";
                    listaResultado.Add(item);
                }
            }
            //int cuantos = 0;
            //int finCuenta = 0;
            //String total = regreso;
            //while (!total.Equals(""))
            //{
            //    cuantos = total.Contains("(") ? cuantos + 1 : cuantos;
            //    if (cuantos == finCuenta)
            //    {
            //        total = "";
            //    }
            //    else
            //    {
            //        total = total.Substring(total.IndexOf('(') + 1);
            //    }
            //    finCuenta = cuantos;
            //}
            //total = regreso;
            //while (!total.Equals(""))
            //{
            //    cuantos = total.Contains(")") ? cuantos - 1 : cuantos;
            //    if (cuantos == finCuenta)
            //    {
            //        total = "";
            //    }
            //    else
            //    {
            //        total = total.Substring(total.IndexOf(')') + 1);
            //    }
            //    finCuenta = cuantos;
            //}

            //for (int agrega = 0; agrega < cuantos; agrega++)
            //{
            //    regreso += ")";
            //}
            return listaResultado;
        }

        /// <summary>
        /// Genera un string conteniendo las frases y palabras separadas en una expresión.
        /// </summary>
        /// <param name="p">Expresión a buscar</param>
        /// <returns>El token de las palabras o frases encontradas y normalizadas</returns>
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
        /// obtiene las frases que pueden haber en una expresión de búsqueda
        /// </summary>
        /// <param name="cadenaActual">La cadena de búsqueda</param>
        /// <returns>La lista con todas las frases que se buscarán</returns>
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

        /// <summary>
        /// Obtiene la lista de palabras a buscar dentro de una expresión
        /// </summary>
        /// <param name="cadenaActual">La cadena actual de búsqueda</param>
        /// <returns>La lista de palabras de dicha cadena</returns>
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
        /// Obtiene las materias de un IUS determinado
        /// </summary>
        /// <param name="ius">El ius de la tesis de la cual se quiere conocer sus materias</param>
        /// <returns>El string conteniendo todas las materias a las que pertenece la tesis</returns>
        public List<String> getMaterias(String ius)
        {
            try
            {
                DbConnection conexion = contextoBD.ContextConection;
                //String connectionString = "Server=\\dgcscthp01;database=iusServer;provider=sqloledb";
                //OdbcConnection conexion = new OdbcConnection(connectionString);
                String sqlQuery = " select A.descMat from cmats As A, tesis As B where B.ius= " + ius +
                                  "        AND A.idMat IN ( B.Materia1, B.Materia2, B.Materia3)";
                DataAdapter query = contextoBD.dataAdapter(sqlQuery, conexion);
                DataSet datos = new DataSet();
                query.Fill(datos);
                DataTableReader tabla = datos.Tables[0].CreateDataReader();

                List<String> lista = new List<String>();
                //foreach (DataRow fila in tabla.Rows)
                while (tabla.Read())
                {
                    String textosActual = "" + tabla["descMat"];
                    ;
                    lista.Add(textosActual);
                }
                tabla.Close();

                conexion.Close();
                return lista;
            }
            catch (Exception e)
            {
                if (!EventLog.SourceExists("IUS"))
                {
                    EventLog.CreateEventSource("IUS", "IUS");
                }
                EventLog Logg = new EventLog("IUS");
                Logg.Source = "IUS";
                String mensaje = "TesisDAOImpl Exception at getMaterias(String)\n" + e.Message + e.StackTrace;
                Logg.WriteEntry(mensaje);
                Logg.Close();
                return new List<String>();
            }
        }

        /// <summary>
        /// Obtiene el catálogo de las fuentes
        /// </summary>

        /// <returns>Las fuentes que generan el carálogo</returns>
        public List<String> getFuentes()
        {
            DbConnection conexion = contextoBD.ContextConection;
            try
            {
                //String connectionString = "Server=\\dgcscthp01;database=iusServer;provider=sqloledb";
                //OdbcConnection conexion = new OdbcConnection(connectionString);
                String sqlQuery = " select A.descFte from cFuentes As A";
                DataAdapter query = contextoBD.dataAdapter(sqlQuery, conexion);
                DataSet datos = new DataSet();
                query.Fill(datos);
                DataTableReader tabla = datos.Tables[0].CreateDataReader();

                List<String> lista = new List<String>();
                //foreach (DataRow fila in tabla.Rows)
                while (tabla.Read())
                {
                    String textosActual = "" + tabla["descFte"];
                    ;
                    lista.Add(textosActual);
                }
                tabla.Close();

                conexion.Close();
                return lista;
            }
            catch (Exception e)
            {
                if (!EventLog.SourceExists("IUS"))
                {
                    EventLog.CreateEventSource("IUS", "IUS");
                }
                EventLog Logg = new EventLog("IUS");
                Logg.Source = "IUS";
                String mensaje = "TesisDAOImpl Exception at getMaterias(String)\n" + e.Message + e.StackTrace;
                Logg.WriteEntry(mensaje);
                Logg.Close();
                conexion.Close();
                return new List<String>();
            }
        }

        /// <summary>
        /// Obtiene una lista de tesis filtrada de una manera en específico,
        /// obteniendo todos los datos y no solo el IUS. Trae las tesis desde
        /// las busquedas especiales.
        /// </summary>
        /// <param name="parte">Los datos de la búsqueda.</param>
        /// <returns> Las coincidencias de la Búsqueda.</returns>
        public /*List<TesisTO>*/ DataTableReader getTesisEspecialConFiltro(MostrarPorIusTO parte)
        {
            DbConnection conexion = contextoBD.ContextConection;
            try
            {
                //String connectionString = "Server=\\dgcscthp01;database=iusServer;provider=sqloledb";
                //OdbcConnection conexion = new OdbcConnection(connectionString);
                String sqlQuery = " select A.ius. B.ordenInstancia, B.ordenTesis, B.OrdenRubro from progsalternos As A, tesis As B" +
                                  " where A.ius=B.ius AND" +
                                  "       A.idProg = " + parte.getBusquedaEspecialValor() + " AND" +
                                  "       B." + parte.getFilterBy() + " = " + parte.getFilterValue() +
                                  " order by B." + parte.getOrderBy() + " " + parte.getOrderType();
                DataAdapter query = contextoBD.dataAdapter(sqlQuery, conexion);
                DataSet datos = new DataSet();
                query.Fill(datos);
                DataTableReader tabla = datos.Tables[0].CreateDataReader();
                conexion.Close();
                //List<TesisTO> lista = new List<TesisTO>();
                //foreach (DataRow fila in tabla.Rows)
                //{
                //    TesisTO tesisActual = new TesisTO();
                //    tesisActual.setIus("" + fila["ius"]);
                //    lista.Add(tesisActual);
                //}
                //conexion.Close();
                //return lista;
                return tabla;
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
                String mensaje = "TesisDAOImpl Exception at getTesisEspecialConFiltro(mostrarPorIusTO)\n" + e.Message + e.StackTrace;
                Logg.WriteEntry(mensaje);
                Logg.Close();
                return (new DataTable()).CreateDataReader();// List<TesisTO>();
            }
        }

        /// <summary>
        /// Obtiene una lista de tesis que coinciden con la 
        /// característica de ser jurisprudencia. Trae las tesis desde
        /// las busquedas especiales.
        /// </summary>
        /// <param name="parte"> Los parámetros de búsqueda.</param>
        /// <returns> La lista de resultados coincidentes.</returns>
        public /*List<TesisTO>*/ DataTableReader getTesisEspecialJurisprudencia(MostrarPorIusTO parte)
        {
            try
            {
                DbConnection conexion = contextoBD.ContextConection;
                //String connectionString = "Server=\\dgcscthp01;database=iusServer;provider=sqloledb";
                //OdbcConnection conexion = new OdbcConnection(connectionString);
                String sqlQuery = " select A.ius, B.Ordeninstancia, B.OrdenTesis, B.OrdenRubro from progsalternos As A, tesis As B" +
                                  " where A.ius=B.ius AND" +
                                  "       A.idProg = " + parte.getBusquedaEspecialValor() + " AND" +
                                  "                       B.ta_tj = 1" +
                                  "                 order by B." + parte.getOrderBy() + " " + parte.getOrderType();
                DataAdapter query = contextoBD.dataAdapter(sqlQuery, conexion);
                DataSet datos = new DataSet();
                query.Fill(datos);
                DataTableReader tabla = datos.Tables[0].CreateDataReader();
                conexion.Close();
                //List<TesisTO> lista = new List<TesisTO>();
                //foreach (DataRow fila in tabla.Rows)
                //{
                //    TesisTO tesisActual = new TesisTO();
                //    tesisActual.setIus("" + fila["ius"]);
                //    lista.Add(tesisActual);
                //}
                ////conexion.Close();
                //return lista;
                return tabla;
            }
            catch (Exception e)
            {
                if (!EventLog.SourceExists("IUS"))
                {
                    EventLog.CreateEventSource("IUS", "IUS");
                }
                EventLog Logg = new EventLog("IUS");
                Logg.Source = "IUS";
                String mensaje = "TesisDAOImpl Exception at getTesisEspecialJurisprudencia(MostrarPorIUSTO)\n" + e.Message + e.StackTrace;
                Logg.WriteEntry(mensaje);
                Logg.Close();
                return (new DataTable()).CreateDataReader();// List<TesisTO>();
            }
        }

        /// <summary>
        /// Obtiene las tesis que pertenecen a la lista de enteros dentro
        /// del objeto identificadores. Trae las tesis desde
        /// las busquedas especiales.</summary>
        /// <param name="identificadores"> el objeto con los parámetros de búsqueda.</param>
        /// <returns> la Lista de coincidencias.</returns>
        public /*List<TesisTO>*/ DataTableReader getTesisEspecial(MostrarPorIusTO identificadores)
        {
            try
            {
                DbConnection conexion = contextoBD.ContextConection;
                // String connectionString = "Server=\\dgcscthp01;database=iusServer;provider=sqloledb";
                //OdbcConnection conexion = contextoBD.contextConection;//new OdbcConnection(connectionString);
                String sqlQuery = " select A.ius, B.Consecindx, B.OrdenTesis, B.OrdenInstancia, B.ordenRubro, " +
                                  "        B.tpoTesis, tpoPonente1, tpoPonente2, tpoPonente3, tpoPonente4, tpoPonente5, " +
                                  "        tpoAsunto1, tpoAsunto2, tpoAsunto3, tpoAsunto4, tpoAsunto5, tpoPon1, tpoPon2, tpoPon3, tpoPon4, tpoPon5 " +
                                  "    from progsalternos As A, tesis As B" +
                                  "                 where A.ius=B.ius AND" +
                                  "                       A.idProg = " + identificadores.getBusquedaEspecialValor() +
                                  "                 order by B." + identificadores.getOrderBy() + " " + identificadores.getOrderType();
                DataAdapter query = contextoBD.dataAdapter(sqlQuery, conexion);
                DataSet datos = new DataSet();
                query.Fill(datos);
                DataTableReader tabla = datos.Tables[0].CreateDataReader();
                conexion.Close();
                //List<TesisTO> lista = new List<TesisTO>();
                //foreach (DataRow fila in tabla.Rows)
                //{
                //    TesisTO tesisActual = new TesisTO();
                //    tesisActual.setIus("" + fila["ius"]);
                //    lista.Add(tesisActual);
                //}
                ////conexion.Close();
                //return lista;
                return tabla;
            }
            catch (Exception e)
            {
                if (!EventLog.SourceExists("IUS"))
                {
                    EventLog.CreateEventSource("IUS", "IUS");
                }
                EventLog Logg = new EventLog("IUS");
                Logg.Source = "IUS";
                String mensaje = "TesisDAOImpl Exception at getTesisEspecial(MostrarPorIusTO)\n" + e.Message + e.StackTrace;
                Logg.WriteEntry(mensaje);
                Logg.Close();
                return (new DataTable()).CreateDataReader();// List<TesisTO>();
            }
        }

        /// <summary>
        /// Obtiene las tesis que pertenecen a la lista de enteros dentro
        /// del objeto identificadores. Trae las tesis desde
        /// las busquedas especiales.</summary>
        /// <param name="identificadores"> el objeto con los parámetros de búsqueda.</param>
        /// <returns> Un paginador con la lista de coincidencias.</returns>
        public PaginadorTO getTesisEspecialPaginador(MostrarPorIusTO identificadores)
        {
            DbConnection conexion = contextoBD.ContextConection;
            try
            {
                String sqlQuery = " select A.ius " +
                                  "        from progsalternos As A, tesis As B" +
                                  "                 where A.ius=B.ius AND" +
                                  "                       A.idProg = " + identificadores.getBusquedaEspecialValor() +
                                  "                 order by B." + identificadores.getOrderBy() + " " + identificadores.getOrderType();
                DataAdapter query = contextoBD.dataAdapter(sqlQuery, conexion);
                DataSet datos = new DataSet();
                query.Fill(datos);
                DataTableReader tabla = datos.Tables[0].CreateDataReader();

                List<int> lista = new List<int>();
                //foreach (DataRow fila in tabla.Rows)
                while (tabla.Read())
                {
                    lista.Add((int)tabla["ius"]);
                }
                tabla.Close();

                PaginadorTO paginadorActual = new PaginadorTO();
                paginadorActual.Largo = lista.Count;
                paginadorActual.ResultadoIds = lista;
                paginadorActual.TimeStamp = DateTime.Now;
                paginadorActual.TipoBusqueda = IUSConstants.BUSQUEDA_TESIS_SIMPLE;
                #if RED_JUR
                GuardarPaginador(paginadorActual);
                #else
                ConsultasActuales.Add(paginadorActual.Id, paginadorActual);
                #endif
                conexion.Close();
                return paginadorActual;
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
                String mensaje = "TesisDAOImpl Exception at getTesisEspecialPaginador(MostrarPorIUSTO)\n" + e.Message + e.StackTrace;
                Logg.WriteEntry(mensaje);
                Logg.Close();
                return null;// List<TesisTO>();
            }
        }

        ///<summary>
        /// Obtiene la lista de las tesis pertenecientes a una instancia en una época
        /// determinada.</summary>
        /// <param name="parametros">Los parámetros de la búsqueda.</param>
        /// <returns> Las tesis que cumplen con el criterio seleccionado.</returns>
        public List<TesisTO> getTesisPorInstancias(MostrarPorIusTO parametros)
        {
            DbConnection conexion = contextoBD.ContextConection;
            try
            {
                String sqlQuery = " select A.ius, A.OrdenTesis, A.OrdenInstancia, A.ordenRubro, A.ConsecIndx, A.TpoTesis," +
                                  " A.TpoAsunto1, A.TpoAsunto2, A.TpoAsunto3, A.TpoAsunto4, A.TpoAsunto5, " +
                                  " A.tpoPonente1, A.tpoPonente2, A.tpoPonente3, A.tpoPonente4, A.tpoPonente5  from tesis As A " +
                                  "       where A.Instancia = " + parametros.getBusquedaEspecialValor() +
                                  "       order by A." + parametros.getOrderBy() + " " + parametros.getOrderType();
                DataAdapter query = contextoBD.dataAdapter(sqlQuery, conexion);
                DataSet datos = new DataSet();
                query.Fill(datos);
                DataTableReader tabla = datos.Tables[0].CreateDataReader();

                List<TesisTO> lista = new List<TesisTO>();
                //foreach (DataRow fila in tabla.Rows)
                while (tabla.Read())
                {
                    TesisTO tesisActual = new TesisTO();
                    tesisActual.setIus("" + tabla["ius"]);
                    tesisActual.OrdenRubro = (int)tabla["OrdenRubro"];
                    tesisActual.ConsecIndx = "" + tabla["ConsecIndx"];
                    tesisActual.OrdenTesis = (int)tabla["OrdenTesis"];
                    tesisActual.OrdenInstancia = (int)tabla["OrdenInstancia"];
                    tesisActual.TpoTesis = "" + tabla["TpoTesis"];
                    tesisActual.Ponentes = new int[5];
                    tesisActual.Ponentes[0] = (short)tabla["tpoPonente1"];
                    tesisActual.Ponentes[1] = (short)tabla["tpoPonente2"];
                    tesisActual.Ponentes[2] = (short)tabla["tpoPonente3"];
                    tesisActual.Ponentes[3] = (short)tabla["tpoPonente4"];
                    tesisActual.Ponentes[4] = (short)tabla["tpoPonente5"];
                    tesisActual.TipoTesis = new int[5];
                    tesisActual.TipoTesis[0] = (short)tabla["TpoAsunto1"];
                    tesisActual.TipoTesis[1] = (short)tabla["TpoAsunto2"];
                    tesisActual.TipoTesis[2] = (short)tabla["TpoAsunto3"];
                    tesisActual.TipoTesis[3] = (short)tabla["TpoAsunto4"];
                    tesisActual.TipoTesis[4] = (short)tabla["TpoAsunto5"];
                    lista.Add(tesisActual);
                }
                tabla.Close();

                conexion.Close();
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
                String mensaje = "TesisDAOImpl Exception at getTesisPorInstancias(MostrarPorIusTO\n" + e.Message + e.StackTrace;
                Logg.WriteEntry(mensaje);
                Logg.Close();
                return new List<TesisTO>();
            }
        }

        ///<summary>
        /// Obtiene la lista de las tesis pertenecientes a una instancia en una época
        /// determinada.</summary>
        /// <param name="parametros">Los parámetros de la búsqueda.</param>
        /// <returns> Las tesis que cumplen con el criterio seleccionado.</returns>
        public PaginadorTO getTesisPorInstanciasPaginador(MostrarPorIusTO parametros)
        {
            DbConnection conexion = contextoBD.ContextConection;
            try
            {
                String sqlQuery = " select A.ius from tesis As A " +
                                  "       where A.Instancia = " + parametros.getBusquedaEspecialValor() +
                                  "       order by A." + parametros.getOrderBy() + " " + parametros.getOrderType();
                DataAdapter query = contextoBD.dataAdapter(sqlQuery, conexion);
                DataSet datos = new DataSet();
                query.Fill(datos);
                DataTableReader tabla = datos.Tables[0].CreateDataReader();

                List<int> lista = new List<int>();
                //foreach (DataRow fila in tabla.Rows)
                while (tabla.Read())
                {
                    lista.Add((int)tabla["ius"]);
                }
                tabla.Close();

                conexion.Close();
                PaginadorTO resultado = new PaginadorTO();
                resultado.Activo = true;
                resultado.Largo = lista.Count;
                resultado.ResultadoIds = lista;
                resultado.TimeStamp = DateTime.Now;
                resultado.TipoBusqueda = IUSConstants.BUSQUEDA_INDICES;
                #if RED_JUR
                GuardarPaginador(resultado);
                #else
                ConsultasActuales.Add(resultado.Id, resultado);
                #endif
                return resultado;
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
                String mensaje = "TesisDAOImpl Exception at getTesisPorInstanciasPaginador(mostrarPorIusTO\n" + e.Message + e.StackTrace;
                Logg.WriteEntry(mensaje);
                Logg.Close();
                return null;
            }
        }

        /// <summary>
        /// Obtiene una lista de tesis del índice solicitado con los filtros requeridos.</summary>
        /// <param name="parametros"> los parametros de la búsqueda a realizar.</param>
        /// <returns> Las tesis que deben estar en el índice seleccionado.</returns>
        public List<TesisTO> getIndicesJurisprudencia(MostrarPorIusTO parametros)
        {
            DbConnection conexion = contextoBD.ContextConection;
            try
            {
                String sqlQuery = " select A.ius. A.ConsecIndx, A.OrdenInstancia, A.OrdenTesis, A.ordenRubro, A.TpoTesis from tesis As A" +
                                  "       where A.Instancia = " + parametros.getBusquedaEspecialValor() +
                                  "          AND A.ta_tj = 1" +
                                  "       order by A." + parametros.getOrderBy() + " " + parametros.getOrderType();
                DataAdapter query = contextoBD.dataAdapter(sqlQuery, conexion);
                DataSet datos = new DataSet();
                query.Fill(datos);
                DataTableReader tabla = datos.Tables[0].CreateDataReader();

                List<TesisTO> lista = new List<TesisTO>();
                //foreach (DataRow fila in tabla.Rows)
                while (tabla.Read())
                {
                    TesisTO tesisActual = new TesisTO();
                    tesisActual.setIus((String)tabla["ius"]);
                    tesisActual.OrdenRubro = (int)tabla["OrdenRubro"];
                    tesisActual.ConsecIndx = "" + tabla["ConsecIndx"];
                    tesisActual.OrdenTesis = (int)tabla["OrdenTesis"];
                    tesisActual.OrdenInstancia = (int)tabla["OrdenInstancia"];
                    tesisActual.TpoTesis = "" + tabla["TpoTesis"];
                    lista.Add(tesisActual);
                }
                tabla.Close();

                conexion.Close();
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
                String mensaje = "TesisDAOImpl Exception at getIndicesJurisprudencia(MostrarPorIusTO\n" + e.Message + e.StackTrace;
                Logg.WriteEntry(mensaje);
                Logg.Close();
                return new List<TesisTO>();
            }
        }

        /// <summary>
        /// Obtiene una lista de tesis del índice solicitado con los filtros requeridos.</summary>
        /// <param name="parametros"> los parametros de la búsqueda a realizar.</param>
        /// <returns> Las tesis que deben estar en el índice seleccionado.</returns>
        public List<TesisTO> getIndicesConFiltro(MostrarPorIusTO parametros)
        {
            DbConnection conexion = contextoBD.ContextConection;
            try
            {
                String sqlQuery = " select A.ius, A.ConsecIndx, A.ordenTesis, A.OrdenInstancia, A.OrdenRubro from tesis As A" +
                                  "      where A.Instancia = " + parametros.getBusquedaEspecialValor() +
                                  "          AND A." + parametros.getFilterBy() + " = " + parametros.getFilterValue() +
                                  "       order by A." + parametros.getOrderBy() + " " + parametros.getOrderType();
                DataAdapter query = contextoBD.dataAdapter(sqlQuery, conexion);
                DataSet datos = new DataSet();
                query.Fill(datos);
                DataTableReader tabla = datos.Tables[0].CreateDataReader();

                List<TesisTO> lista = new List<TesisTO>();
                //foreach (DataRow fila in tabla.Rows)
                while (tabla.Read())
                {
                    TesisTO tesisActual = new TesisTO();
                    tesisActual.setIus("" + tabla["ius"]);
                    tesisActual.OrdenRubro = (int)tabla["OrdenRubro"];
                    tesisActual.ConsecIndx = "" + tabla["ConsecIndx"];
                    tesisActual.OrdenTesis = (int)tabla["OrdenTesis"];
                    tesisActual.OrdenInstancia = (int)tabla["OrdenInstancia"];
                    lista.Add(tesisActual);
                }
                tabla.Close();

                conexion.Close();
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
                String mensaje = "TesisDAOImpl Exception at getIndicesConFiltro(mostrarPorIusTO\n" + e.Message + e.StackTrace;
                Logg.WriteEntry(mensaje);
                Logg.Close();
                return new List<TesisTO>();
            }
        }

        ///<summary>
        /// Obtiene las tesis que pertenecen a una materia determinada
        /// de acuerdo a los parámetros de búsqueda por Indice.</summary>
        /// <param name="parametros"> Los parametros de la búsqueda</param>
        /// <returns> La lista con las tesis que cumplen con los parámetros de búsqueda.</returns>
        public List<TesisTO> getTesisPorMaterias(MostrarPorIusTO parametros)
        {
            DbConnection conexion = contextoBD.ContextConection;
            try
            {
                parametros.setTabla(obtenTablaMateria(parametros.getBusquedaEspecialValor()));
                String sqlQuery = "";
                if (!parametros.getTabla().Equals("cInd00Materias"))
                {
                    sqlQuery = " select A.ius4, B.ConsecIndx, B.tpotesis, B.OrdenInstancia," +
                               " B.OrdenRubro, B.OrdenTesis, B.tpoPonente1," +
                               " B.tpoPonente2, B.tpoPonente3, B.tpoPonente4," +
                               " B.tpoPonente5," +
                               " B.TpoAsunto1, B.TpoAsunto2, B.TpoAsunto3, " +
                               " B.TpoAsunto4, B.TpoAsunto5,  ius from " + parametros.getTabla() +
                               " As A, tesis As B" +
                               "     where B.IUS = A.IUS4" +
                               "         AND A.NumLetra = " + parametros.getLetra() +
                               "     order by B." + parametros.getOrderBy() + " " + parametros.getOrderType();
                }
                else
                {
                    sqlQuery = "select  A.ConsecIndx, A.tpotesis, A.OrdenInstancia," +
                               " A.OrdenRubro, A.OrdenTesis, A.tpoPonente1," +
                               " A.tpoPonente2, A.tpoPonente3, A.tpoPonente4," +
                               " A.tpoPonente5," +
                               " A.TpoAsunto1, A.TpoAsunto2, A.TpoAsunto3, " +
                               " A.TpoAsunto4, A.TpoAsunto5,  ius from tesis As A" +
                               " where A.NumLetra = " + parametros.getLetra() +
                               " order by A." + parametros.getOrderBy() + " " + parametros.getOrderType();
                }
                DataAdapter query = contextoBD.dataAdapter(sqlQuery, conexion);
                DataSet datos = new DataSet();
                query.Fill(datos);
                DataTableReader tabla = datos.Tables[0].CreateDataReader();

                List<TesisTO> lista = new List<TesisTO>();
                //foreach (DataRow fila in tabla.Rows)
                while (tabla.Read())
                {
                    TesisTO tesisActual = new TesisTO();
                    tesisActual.ConsecIndxInt = (int)tabla["ConsecIndx"];
                    tesisActual.ConsecIndx = "" + tabla["ConsecIndx"];
                    tesisActual.setIus("" + tabla["ius"]);
                    tesisActual.TpoTesis = "" + tabla["tpoTesis"];
                    tesisActual.OrdenInstancia = (int)tabla["OrdenInstancia"];
                    tesisActual.OrdenRubro = (int)tabla["OrdenRubro"];
                    tesisActual.OrdenTesis = (int)tabla["OrdenTesis"];
                    tesisActual.Ponentes = new int[5];
                    tesisActual.Ponentes[0] = (short)tabla["tpoPonente1"];
                    tesisActual.Ponentes[1] = (short)tabla["tpoPonente2"];
                    tesisActual.Ponentes[2] = (short)tabla["tpoPonente3"];
                    tesisActual.Ponentes[3] = (short)tabla["tpoPonente4"];
                    tesisActual.Ponentes[4] = (short)tabla["tpoPonente5"];
                    tesisActual.TipoTesis = new int[5];
                    tesisActual.TipoTesis[0] = (short)tabla["TpoAsunto1"];
                    tesisActual.TipoTesis[1] = (short)tabla["TpoAsunto2"];
                    tesisActual.TipoTesis[2] = (short)tabla["TpoAsunto3"];
                    tesisActual.TipoTesis[3] = (short)tabla["TpoAsunto4"];
                    tesisActual.TipoTesis[4] = (short)tabla["TpoAsunto5"];
                    //tesisActual.setIus("" + fila["ius"]);
                    //tesisActual.ConsecIndx = "" + fila["ConsecIndx"];
                    //tesisActual.OrdenTesis = (int)fila["OrdenTesis"];
                    //tesisActual.OrdenInstancia = (int)fila["OrdenInstancia"];
                    //tesisActual.OrdenRubro = (int)fila["OrdenRubro"];
                    //tesisActual.TpoTesis = "" + fila["TpoTesis"];
                    lista.Add(tesisActual);
                }
                tabla.Close();

                conexion.Close();
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
                String mensaje = "TesisDAOImpl Exception at getTesisPormaterias(MostrarPorIusTO\n" + e.Message + e.StackTrace;
                Logg.WriteEntry(mensaje);
                Logg.Close();
                return new List<TesisTO>();
            }
        }

        ///<summary>
        /// Obtiene las tesis que pertenecen a una materia determinada
        /// de acuerdo a los parámetros de búsqueda por Indice.</summary>
        /// <param name="parametros"> Los parametros de la búsqueda</param>
        /// <returns> La lista con las tesis que cumplen con los parámetros de búsqueda.</returns>
        public PaginadorTO getTesisPorMateriasPaginador(MostrarPorIusTO parametros)
        {
            DbConnection conexion = contextoBD.ContextConection;
            try
            {
                parametros.setTabla(obtenTablaMateria(parametros.getBusquedaEspecialValor()));
                String sqlQuery = "";
                if (!parametros.getTabla().Equals("cInd00Materias"))
                {
                    sqlQuery = " select A.ius4, ius from " + parametros.getTabla() +
                               " As A, tesis As B" +
                               "     where B.IUS = A.IUS4" +
                               "         AND A.NumLetra = " + parametros.getLetra() +
                               "     order by B." + parametros.getOrderBy() + " " + parametros.getOrderType();
                }
                else
                {
                    sqlQuery = "select ius from tesis As A" +
                               " where A.NumLetra = " + parametros.getLetra() +
                               " order by A." + parametros.getOrderBy() + " " + parametros.getOrderType();
                }
                DataAdapter query = contextoBD.dataAdapter(sqlQuery, conexion);
                DataSet datos = new DataSet();
                query.Fill(datos);
                DataTableReader tabla = datos.Tables[0].CreateDataReader();

                List<int> lista = new List<int>();
                //foreach (DataRow fila in tabla.Rows)
                while (tabla.Read())
                {
                    lista.Add((int)tabla["ius"]);
                }
                tabla.Close();

                conexion.Close();
                PaginadorTO resultado = new PaginadorTO();
                resultado.Largo = lista.Count;
                resultado.Activo = true;
                resultado.TimeStamp = DateTime.Now;
                resultado.ResultadoIds = lista;
                resultado.TipoBusqueda = IUSConstants.BUSQUEDA_INDICES;
                #if RED_JUR
                GuardarPaginador(resultado);
                #else
                ConsultasActuales.Add(resultado.Id, resultado);
                #endif
                return resultado;
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
                String mensaje = "TesisDAOImpl Exception at getTesisPorMateriasPaginador(MostrarporIusTO)\n" + e.Message + e.StackTrace;
                Logg.WriteEntry(mensaje);
                Logg.Close();
                return null;
            }
        }

        ///<summary>
        /// Obtiene una lista de tesis del índice de materias solicitado con los filtros requeridos.</summary>
        /// <param name="parametros"> los parametros de la búsqueda a realizar.</param>
        /// <returns> Las tesis que deben estar en el índice de la materia seleccionado.</returns>
        public List<TesisTO> getMateriasJurisprudencia(MostrarPorIusTO parametros)
        {
            DbConnection conexion = contextoBD.ContextConection;
            try
            {
                parametros.setTabla(obtenTablaMateria(parametros.getBusquedaEspecialValor()));
                String sqlQuery = "";
                if (!parametros.getTabla().Equals("cInd00Materias"))
                {
                    sqlQuery = "select A.ius4 ius from " + parametros.getTabla() + " As A, tesis As B" +
                               "          where B.IUS = A.IUS4" +
                               "              AND A.NumLetra = " + parametros.getLetra() +
                               "            AND B.ta_tj = 1" +
                               "          order by B." + parametros.getOrderBy() + " " + parametros.getOrderType();
                }
                else
                {
                    sqlQuery = "select A.ius from tesis As A" +
                               "    where A.NumLetra = " + parametros.getLetra() +
                               "      AND A.ta_tj = 1" +
                               " order by A." + parametros.getOrderBy() + " " + parametros.getOrderType();
                }
                DataAdapter query = contextoBD.dataAdapter(sqlQuery, conexion);
                DataSet datos = new DataSet();
                query.Fill(datos);
                DataTableReader tabla = datos.Tables[0].CreateDataReader();

                List<TesisTO> lista = new List<TesisTO>();
                //foreach (DataRow fila in tabla.Rows)
                while (tabla.Read())
                {
                    TesisTO tesisActual = new TesisTO();
                    tesisActual.setIus("" + tabla["ius"]);
                    lista.Add(tesisActual);
                }
                tabla.Close();

                conexion.Close();
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
                String mensaje = "TesisDAOImpl Exception at getMateriasJurisprudencia(PartesTO\n" + e.Message + e.StackTrace;
                Logg.WriteEntry(mensaje);
                Logg.Close();
                return new List<TesisTO>();
            }
        }

        ///<summary>
        /// Obtiene una lista de tesis del índice de materias solicitado con los filtros requeridos.</summary>
        /// <param name="parametros"> los parametros de la búsqueda a realizar.</param>
        /// <returns> Las tesis que deben estar en el índice de la materia seleccionado.</returns>
        public List<TesisTO> getMateriasConFiltro(MostrarPorIusTO parametros)
        {
            DbConnection conexion = contextoBD.ContextConection;
            try
            {
                parametros.setTabla(obtenTablaMateria(parametros.getBusquedaEspecialValor()));
                String sqlQuery = "";
                if (!parametros.getTabla().Equals("cInd00Materias"))
                {
                    sqlQuery = "select A.ius4 ius from " + parametros.getTabla() + " As A, tesis B" +
                               "          where B.IUS = A.IUS4" +
                               "              AND A.NumLetra = " + parametros.getLetra() +
                               "              AND B." + parametros.getFilterBy() + " = " + parametros.getFilterValue() +
                               "          order by B." + parametros.getOrderBy() + " " + parametros.getOrderType();
                }
                else
                {
                    sqlQuery = "select A.ius from tesis As A" +
                               "    where A.NumLetra = " + parametros.getLetra() +
                               "      AND A." + parametros.getFilterBy() + "= " + parametros.getFilterValue() +
                               "    order by A." + parametros.getOrderBy() + " " + parametros.getOrderType();
                }
                DataAdapter query = contextoBD.dataAdapter(sqlQuery, conexion);
                DataSet datos = new DataSet();
                query.Fill(datos);
                DataTableReader tabla = datos.Tables[0].CreateDataReader();

                List<TesisTO> lista = new List<TesisTO>();
                //foreach (DataRow fila in tabla.Rows)
                while (tabla.Read())
                {
                    TesisTO tesisActual = new TesisTO();
                    tesisActual.setIus((String)tabla["ius"]);
                    lista.Add(tesisActual);
                }
                tabla.Close();

                conexion.Close();
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
                String mensaje = "TesisDAOImpl Exception at getMateriasConFiltro(MostrarPorIusTO\n" + e.Message + e.StackTrace;
                Logg.WriteEntry(mensaje);
                Logg.Close();
                return new List<TesisTO>();
            }
        }

        /// <summary>
        /// Obtiene las tesis que pertenecen a un tribunal colegiado determinada
        /// de acuerdo a los parámetros de búsqueda por Indice.</summary>
        /// <param name="parametros"> Los parametros de la búsqueda</param>
        /// <returns> La lista con las tesis que cumplen con los parámetros de búsqueda.</returns>
        public List<TesisTO> getTesisPorTribunal(MostrarPorIusTO parametros)
        {
            TesisTO tesisActual = null;
            DbConnection conexion = contextoBD.ContextConection;
            try
            {
                String sqlQuery = " select ConsecIndx, ius, tpoTesis," +
                                  "OrdenInstancia, OrdenRubro, OrdenTesis," +
                                  "tpoPonente1, tpoPonente2, tpoPonente3, " +
                                  "tpoPonente4, tpoPonente5," +
                                  "TpoAsunto1, TpoAsunto2, TpoAsunto3, " +
                                  "TpoAsunto4, TpoAsunto5 from tesis As A" +
                                  "       where A.instancia = " + parametros.getBusquedaEspecialValor() +
                                  "       order by A." + parametros.getOrderBy() + " " + parametros.getOrderType();
                DataAdapter query = contextoBD.dataAdapter(sqlQuery, conexion);
                DataSet datos = new DataSet();
                query.Fill(datos);
                DataTableReader tabla = datos.Tables[0].CreateDataReader();

                List<TesisTO> lista = new List<TesisTO>();
                //foreach (DataRow fila in tabla.Rows)
                while (tabla.Read())
                {
                    tesisActual = new TesisTO();
                    tesisActual.ConsecIndxInt = (int)tabla["ConsecIndx"];
                    tesisActual.ConsecIndx = "" + tabla["ConsecIndx"];
                    tesisActual.setIus("" + tabla["ius"]);
                    tesisActual.TpoTesis = "" + tabla["tpoTesis"];
                    tesisActual.OrdenInstancia = (int)tabla["OrdenInstancia"];
                    tesisActual.OrdenRubro = (int)tabla["OrdenRubro"];
                    tesisActual.OrdenTesis = (int)tabla["OrdenTesis"];
                    tesisActual.Ponentes = new int[5];
                    tesisActual.Ponentes[0] = (short)tabla["tpoPonente1"];
                    tesisActual.Ponentes[1] = (short)tabla["tpoPonente2"];
                    tesisActual.Ponentes[2] = (short)tabla["tpoPonente3"];
                    tesisActual.Ponentes[3] = (short)tabla["tpoPonente4"];
                    tesisActual.Ponentes[4] = (short)tabla["tpoPonente5"];
                    tesisActual.TipoTesis = new int[5];
                    tesisActual.TipoTesis[0] = (short)tabla["TpoAsunto1"];
                    tesisActual.TipoTesis[1] = (short)tabla["TpoAsunto2"];
                    tesisActual.TipoTesis[2] = (short)tabla["TpoAsunto3"];
                    tesisActual.TipoTesis[3] = (short)tabla["TpoAsunto4"];
                    tesisActual.TipoTesis[4] = (short)tabla["TpoAsunto5"];
                    //tesisActual.setIus("" + fila["ius"]);
                    lista.Add(tesisActual);
                }
                tabla.Close();

                conexion.Close();
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
                String mensaje = "TesisDAOImpl Exception at getTesisPorTribunal(MostrarPorIusTO)\n" + e.Message + e.StackTrace;
                Logg.WriteEntry(mensaje);
                Logg.Close();
                return new List<TesisTO>();
            }
        }

        /// <summary>
        /// Obtiene las tesis que pertenecen a un tribunal colegiado determinada
        /// de acuerdo a los parámetros de búsqueda por Indice.</summary>
        /// <param name="parametros"> Los parametros de la búsqueda</param>
        /// <returns> La lista con las tesis que cumplen con los parámetros de búsqueda.</returns>
        public PaginadorTO getTesisPorTribunalPaginador(MostrarPorIusTO parametros)
        {
            //TesisTO tesisActual = null;
            DbConnection conexion = contextoBD.ContextConection;
            try
            {
                String sqlQuery = " select ius from tesis As A" +
                                  "       where A.instancia = " + parametros.getBusquedaEspecialValor() +
                                  "       order by A." + parametros.getOrderBy() + " " + parametros.getOrderType();
                DataAdapter query = contextoBD.dataAdapter(sqlQuery, conexion);
                DataSet datos = new DataSet();
                query.Fill(datos);
                DataTableReader tabla = datos.Tables[0].CreateDataReader();

                List<int> lista = new List<int>();
                //foreach (DataRow fila in tabla.Rows)
                while (tabla.Read())
                {
                    lista.Add((int)tabla["ius"]);
                }
                tabla.Close();

                conexion.Close();
                PaginadorTO resultado = new PaginadorTO();
                resultado.Activo = true;
                resultado.Largo = lista.Count;
                resultado.ResultadoIds = lista;
                resultado.TimeStamp = DateTime.Now;
                resultado.TipoBusqueda = IUSConstants.BUSQUEDA_INDICES;
                #if RED_JUR
                GuardarPaginador(resultado);
                #else
                ConsultasActuales.Add(resultado.Id, resultado);
                #endif
                return resultado;
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
                String mensaje = "TesisDaoImpl Exception at getTesisPorTribunalPaginador(MostrarPorIusTO)\n" + e.Message + e.StackTrace;
                Logg.WriteEntry(mensaje);
                Logg.Close();
                return null;
            }
        }

        ///
        /// Obtiene una lista de tesis del tribunal colegiado solicitado con los filtros requeridos.
        /// @param parametros los parametros de la búsqueda a realizar.
        /// @return Las tesis que deben estar en el tribunal colegiado seleccionado.
        ///
        public List<TesisTO> getTribunalJurisprudencia(MostrarPorIusTO parametros)
        {
            DbConnection conexion = contextoBD.ContextConection;
            try
            {
                String sqlQuery = " select A.ius from tesis As A" +
                                  "       where A.instancia = " + parametros.getBusquedaEspecialValor() +
                                  "          AND A.ta_tj = 1" +
                                  "       order by A." + parametros.getOrderBy() + " " + parametros.getOrderType();
                DataAdapter query = contextoBD.dataAdapter(sqlQuery, conexion);
                DataSet datos = new DataSet();
                query.Fill(datos);
                DataTableReader tabla = datos.Tables[0].CreateDataReader();

                List<TesisTO> lista = new List<TesisTO>();
                //foreach (DataRow fila in tabla.Rows)
                while (tabla.Read())
                {
                    TesisTO tesisActual = new TesisTO();
                    tesisActual.setIus("" + tabla["ius"]);
                    lista.Add(tesisActual);
                }
                tabla.Close();

                conexion.Close();
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
                String mensaje = "TesisDAOImpl Exception at getTribunalJurisprudencia(MostrarIusTO)\n" + e.Message + e.StackTrace;
                Logg.WriteEntry(mensaje);
                Logg.Close();
                return new List<TesisTO>();
            }
        }

        /// <summary>
        /// Obtiene una lista de tesis del índice de materias solicitado con los filtros requeridos.
        /// </summary>
        /// <param name="parametros"> los parametros de la búsqueda a realizar.</param>
        /// <returns> Las tesis que deben estar en el índice de la materia seleccionado.</returns>
        ///
        public List<TesisTO> getTribunalConFiltro(MostrarPorIusTO parametros)
        {
            DbConnection conexion = contextoBD.ContextConection;
            try
            {
                String sqlQuery = "select A.ius from tesis As A" +
                                  "       where A.instancia = " + parametros.getBusquedaEspecialValor() +
                                  "          AND A." + parametros.getFilterBy() + " = " + parametros.getFilterValue() +
                                  "       order by A." + parametros.getOrderBy() + " " + parametros.getOrderType();
                DataAdapter query = contextoBD.dataAdapter(sqlQuery, conexion);
                DataSet datos = new DataSet();
                query.Fill(datos);
                DataTableReader tabla = datos.Tables[0].CreateDataReader();
                conexion.Close();
                List<TesisTO> lista = new List<TesisTO>();
                //foreach (DataRow fila in tabla.Rows)
                while (tabla.Read())
                {
                    TesisTO tesisActual = new TesisTO();
                    tesisActual.setIus("" + tabla["ius"]);
                    lista.Add(tesisActual);
                }
                tabla.Close();

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
                String mensaje = "TesisDAOImpl Exception at getTribunalConFiltro\n" + e.Message + e.StackTrace;
                Logg.WriteEntry(mensaje);
                Logg.Close();
                return new List<TesisTO>();
            }
        }

        /// <summary>
        /// Obtiene la tabla de donde se obtendrán los datos de la materia
        /// </summary>
        /// <param name="idTabla">identificador mediante el cual se mapea la tabla</param>
        /// <returns></returns>
        private String obtenTablaMateria(String idTabla)
        {
            Int32 tipoTabla = Int32.Parse(idTabla);
            String resultado = "";
            switch (tipoTabla)
            {
                case 0://Todas las materias
                    resultado += "cInd00Materias";
                    break;
                case 1://Constitucional
                    resultado += "cInd01constitucional";
                    break;
                case 2://Penal
                    resultado += "cInd02Penal";
                    break;
                case 3://administrativa
                    resultado += "cInd03Administrativa";
                    break;
                case 4://Civil
                    resultado += "cInd04Civil";
                    break;
                case 5://Laboral
                    resultado += "cInd05Laboral";
                    break;
                case 6://Comun
                    resultado += "cInd06Comun";
                    break;
            }
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
            return epocasSalas.ToArray();
        }

        public List<RaizTO> getRaizTematica(string id)
        {
            DbConnection conexion = contextoBD.ContextConection;
            try
            {
                String sqlQuery = "select Id, Nivel, Descripcion,Tema_May, Consecutivo, " +
                                  " Posicion, Muestra, Cuantas, Desplazamiento, " +
                                  " Tabla, NoIus, OLE, Hyperl" +
                                  " From  raiz" +
                                  " Where Padre = " + id +
                                  " Order By Consecutivo, Id";
                DataAdapter query = contextoBD.dataAdapter(sqlQuery, conexion);
                DataSet datos = new DataSet();
                query.Fill(datos);
                DataTableReader tabla = datos.Tables[0].CreateDataReader();
                conexion.Close();
                List<RaizTO> lista = new List<RaizTO>();
                while (tabla.Read())
                //foreach (DataRow fila in tabla.Rows)
                {
                    RaizTO nodoActual = new RaizTO();
                    nodoActual.Id = "" + tabla["Id"];
                    nodoActual.Nivel = "" + tabla["Nivel"];
                    nodoActual.Descripcion = "" + tabla["Descripcion"];
                    nodoActual.TemaMay = "" + tabla["Tema_May"];
                    nodoActual.Consecutivo = (short)tabla["Consecutivo"];
                    nodoActual.Posicion = (short)tabla["Posicion"];
                    nodoActual.Muestra = (byte)tabla["Muestra"];
                    nodoActual.Cuantas = (int)tabla["Cuantas"];
                    nodoActual.Desplazamiento = (int)tabla["Desplazamiento"];
                    nodoActual.Tabla = "" + tabla["Tabla"];
                    nodoActual.NoIus = (int)tabla["NoIus"];
                    nodoActual.OLE = tabla["OLE"];
                    nodoActual.Hiperlink = "" + tabla["HyperL"];
                    lista.Add(nodoActual);
                }
                tabla.Close();

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
                String mensaje = "TesisDAOImpl Exception at getRaizTematica\n" + e.Message + e.StackTrace;
                Logg.WriteEntry(mensaje);
                Logg.Close();
                return new List<RaizTO>();
            }
        }

        public List<RaizTO> getSubtemas(String tablaOrigen)
        {
            DbConnection conexion = contextoBD.ContextConection;
            try
            {
                String sqlQuery = "select Id, Nivel, Padre, Descripcion,Tema_May, Consecutivo, " +
                                  " Posicion, Muestra, Cuantas, Desplazamiento, " +
                                  " Vease, NoIus" +
                                  " From  " + tablaOrigen +
                                  " Order By Posicion";
                DataAdapter query = contextoBD.dataAdapter(sqlQuery, conexion);
                DataSet datos = new DataSet();
                query.Fill(datos);
                DataTableReader tabla = datos.Tables[0].CreateDataReader();
                conexion.Close();
                List<RaizTO> lista = new List<RaizTO>();
                //foreach (DataRow fila in tabla.Rows)
                while (tabla.Read())
                {
                    RaizTO nodoActual = new RaizTO();
                    nodoActual.Id = "" + tabla["Id"];
                    nodoActual.Nivel = "" + tabla["Nivel"];
                    nodoActual.Descripcion = "" + tabla["Descripcion"];
                    nodoActual.TemaMay = "" + tabla["Tema_May"];
                    nodoActual.Consecutivo = (short)tabla["Consecutivo"];
                    nodoActual.Posicion = (short)tabla["Posicion"];
                    nodoActual.Muestra = (byte)tabla["Muestra"];
                    nodoActual.Cuantas = (int)tabla["Cuantas"];
                    nodoActual.Desplazamiento = (int)tabla["Desplazamiento"];
                    nodoActual.Tabla = "" + tabla["Vease"];
                    nodoActual.NoIus = (int)tabla["NoIus"];
                    nodoActual.Padre = "" + tabla["Padre"];
                    //nodoActual.Hiperlink = "" + fila["HyperL"];
                    lista.Add(nodoActual);
                }
                tabla.Close();

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
                String mensaje = "TesisDAOImpl Exception at getSubtemas\n" + e.Message + e.StackTrace;
                Logg.WriteEntry(mensaje);
                Logg.Close();
                return new List<RaizTO>();
            }
        }

        public List<RaizTO> getSubtemas(String tablaOrigen, String busqueda)
        {
            DbConnection conexion = contextoBD.ContextConection;
            try
            {
                String[] palabras = busqueda.Split();
                String condicion = "";
                foreach (String item in palabras)
                {
                    condicion += " Tema_May like '% " + item + " %' AND ";
                }
                condicion = condicion.Substring(0, condicion.Length - 4);
                String sqlQuery = "select Id, Nivel, Padre, Descripcion,Tema_May, Consecutivo, " +
                                  " Posicion, Muestra, Cuantas, Desplazamiento, " +
                                  " Vease, NoIus" +
                                  " From  " + tablaOrigen +
                                  " Where " + condicion +
                                  " Order By Posicion";
                DataAdapter query = contextoBD.dataAdapter(sqlQuery, conexion);
                DataSet datos = new DataSet();
                query.Fill(datos);
                DataTableReader tabla = datos.Tables[0].CreateDataReader();
                conexion.Close();
                List<RaizTO> lista = new List<RaizTO>();
                //foreach (DataRow fila in tabla.Rows)
                while (tabla.Read())
                {
                    RaizTO nodoActual = new RaizTO();
                    nodoActual.Id = "" + tabla["Id"];
                    nodoActual.Nivel = "" + tabla["Nivel"];
                    nodoActual.Descripcion = "" + tabla["Descripcion"];
                    nodoActual.TemaMay = "" + tabla["Tema_May"];
                    nodoActual.Consecutivo = (short)tabla["Consecutivo"];
                    nodoActual.Posicion = (short)tabla["Posicion"];
                    nodoActual.Muestra = (byte)tabla["Muestra"];
                    nodoActual.Cuantas = (int)tabla["Cuantas"];
                    nodoActual.Desplazamiento = (int)tabla["Desplazamiento"];
                    nodoActual.Tabla = "" + tabla["Vease"];
                    nodoActual.NoIus = (int)tabla["NoIus"];
                    nodoActual.Padre = "" + tabla["Padre"];
                    //nodoActual.Hiperlink = "" + fila["HyperL"];
                    lista.Add(nodoActual);
                }
                tabla.Close();

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
                String mensaje = "TesisDAOImpl Exception at getSubtemas(string, string)\n" + e.Message + e.StackTrace;
                Logg.WriteEntry(mensaje);
                Logg.Close();
                return new List<RaizTO>();
            }
        }

        public RaizTO getSubtema(string tablaOrigen, string valor)
        {
            DbConnection conexion = contextoBD.ContextConection;
            try
            {
                String sqlQuery = "select Id, Nivel, Padre, Descripcion,Tema_May, Consecutivo, " +
                                  " Posicion, Muestra, Cuantas, Desplazamiento, " +
                                  " Vease, NoIus" +
                                  " From  " + tablaOrigen +
                                  " Where Id=" + valor + "";
                DataAdapter query = contextoBD.dataAdapter(sqlQuery, conexion);
                DataSet datos = new DataSet();
                query.Fill(datos);
                DataTableReader tabla = datos.Tables[0].CreateDataReader();
                conexion.Close();
                List<RaizTO> lista = new List<RaizTO>();
                //foreach (DataRow fila in tabla.Rows)
                while (tabla.Read())
                {
                    RaizTO nodoActual = new RaizTO();
                    nodoActual.Id = "" + tabla["Id"];
                    nodoActual.Nivel = "" + tabla["Nivel"];
                    nodoActual.Descripcion = "" + tabla["Descripcion"];
                    nodoActual.TemaMay = "" + tabla["Tema_May"];
                    nodoActual.Consecutivo = (short)tabla["Consecutivo"];
                    nodoActual.Posicion = (short)tabla["Posicion"];
                    nodoActual.Muestra = (byte)tabla["Muestra"];
                    nodoActual.Cuantas = (int)tabla["Cuantas"];
                    nodoActual.Desplazamiento = (int)tabla["Desplazamiento"];
                    nodoActual.Tabla = "" + tabla["Vease"];
                    nodoActual.NoIus = (int)tabla["NoIus"];
                    nodoActual.Padre = "" + tabla["Padre"];
                    //nodoActual.Hiperlink = "" + fila["HyperL"];
                    lista.Add(nodoActual);
                }
                tabla.Close();

                //conexion.Close();
                return lista.ElementAt(0);
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
                String mensaje = "TesisDAOImpl Exception at getSubtema(string, string)n" + e.Message + e.StackTrace;
                Logg.WriteEntry(mensaje);
                Logg.Close();
                return new RaizTO();
            }
        }

        public RaizTO getSubtemaTesis(string tablaOrigen, string valor)
        {
            DbConnection conexion = contextoBD.ContextConection;
            try
            {
                String StringId = "Id";
                if (tablaOrigen.ToUpper().Contains("ALTERNO"))
                {
                    StringId += "_ALTERNO";
                }
                String sqlQuery = "select " + StringId + ",  Descripcion, " +
                                  "  Cuantas,  " +
                                  "  NoIus" +
                                  " From  " + tablaOrigen +
                                  " Where " + StringId + "=" + valor + "";
                if (tablaOrigen.ToUpper().Contains("THE"))
                {
                    sqlQuery = "select idTema, Descripcion, 0 as cuantas, 0 as noIus " +
                               "From The_temas where idTema = " + valor;
                    StringId = "IdTema";
                }
                DataAdapter query = contextoBD.dataAdapter(sqlQuery, conexion);
                DataSet datos = new DataSet();
                query.Fill(datos);
                DataTableReader tabla = datos.Tables[0].CreateDataReader();
                int filas = datos.Tables[0].Rows.Count;
                List<RaizTO> lista = new List<RaizTO>();
                if (tablaOrigen.ToUpper().Contains("THE") && filas == 0)
                {
                    sqlQuery = "select idTema, Descripcion, 0 as cuantas, 0 as noIus " +
                               "From The_Sinonimos where idTema = " + valor;
                    StringId = "IdTema";
                    query = contextoBD.dataAdapter(sqlQuery, conexion);
                    datos = new DataSet();
                    query.Fill(datos);
                    tabla = datos.Tables[0].CreateDataReader();
                    filas = datos.Tables[0].Rows.Count;
                    if (filas == 0)
                    {
                        sqlQuery = "select idTema, Descripcion, 0 as cuantas, 0 as noIus " +
                                   "From The_Ascendente where idTema = " + valor;
                        query = contextoBD.dataAdapter(sqlQuery, conexion);
                        datos = new DataSet();
                        query.Fill(datos);
                        tabla = datos.Tables[0].CreateDataReader();
                    }
                }
                conexion.Close();
                while (tabla.Read())
                //foreach (DataRow fila in tabla.Rows)
                {
                    RaizTO nodoActual = new RaizTO();
                    nodoActual.Id = "" + tabla[StringId];
                    nodoActual.Descripcion = "" + tabla["Descripcion"];
                    nodoActual.TemaMay = nodoActual.Descripcion;
                    nodoActual.Cuantas = (int)tabla["Cuantas"];
                    nodoActual.NoIus = (int)tabla["NoIus"];
                    //nodoActual.Hiperlink = "" + fila["HyperL"];
                    lista.Add(nodoActual);
                }
                tabla.Close();

                //conexion.Close();
                if (filas == 0)
                {
                    return null;
                }
                else
                {
                    return lista.ElementAt(0);
                }
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
                String mensaje = "TesisDAOImpl Exception at getSubtemaTesis(string, string)\n" + e.Message + e.StackTrace;
                Logg.WriteEntry(mensaje);
                Logg.Close();
                return new RaizTO();
            }
        }

        public List<RaizTO> getSinonimos(String tablaOrigen, String ID)
        {
            DbConnection conexion = contextoBD.ContextConection;
            try
            {
                String tablasTematico = "ELE_alternos IJA_alternos SAR_alternos DRH_alternos ";
                if (tablasTematico.Contains(tablaOrigen))
                {
                    return new List<RaizTO>();
                }
                String sqlQuery = "select Id_alterno, Descripcion, " +
                                  " Cuantas, Desplazamiento, " +
                                  " NoIus" +
                                  " From  " + tablaOrigen +
                                  " Where ID= " + ID;
                DataAdapter query = contextoBD.dataAdapter(sqlQuery, conexion);
                DataSet datos = new DataSet();
                query.Fill(datos);
                DataTableReader tabla = datos.Tables[0].CreateDataReader();
                conexion.Close();
                List<RaizTO> lista = new List<RaizTO>();
                //foreach (DataRow fila in tabla.Rows)
                while (tabla.Read())
                {
                    RaizTO nodoActual = new RaizTO();
                    nodoActual.Id = "" + tabla["Id_alterno"];
                    nodoActual.Descripcion = "" + tabla["Descripcion"];
                    nodoActual.Cuantas = (int)tabla["Cuantas"];
                    nodoActual.Desplazamiento = (int)tabla["Desplazamiento"];
                    nodoActual.NoIus = (int)tabla["NoIus"];
                    //nodoActual.Hiperlink = "" + fila["HyperL"];
                    lista.Add(nodoActual);
                }
                tabla.Close();

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
                String mensaje = "TesisDAOImpl Exception at getSinonimos\n" + e.Message + e.StackTrace;
                Logg.WriteEntry(mensaje);
                Logg.Close();
                return new List<RaizTO>();
            }
        }

        public List<TesisTO> getTesisSubtemas(String tablaOrigen, String valor)
        {
            String Path = "";
            String sqlQuery = "";
            String PathDirectorio = "Error al leer la clave, la variable no fue modificada.";
            List<RaizTO> lista = null;
            DbConnection conexion = contextoBD.ContextConection;

            try
            {
                sqlQuery = "select Id, Nivel, Padre, Descripcion,Tema_May, Consecutivo, " +
                           " Posicion, Muestra, Cuantas, Desplazamiento, " +
                           " Vease, NoIus" +
                           " From  " + tablaOrigen +
                           " Where id=" + valor +
                           " Order By Posicion";
                DataAdapter query = contextoBD.dataAdapter(sqlQuery, conexion);
                DataSet datos = new DataSet();
                query.Fill(datos);
                DataTableReader tabla = datos.Tables[0].CreateDataReader();
                conexion.Close();
                lista = new List<RaizTO>();
                //foreach (DataRow fila in tabla.Rows)
                while (tabla.Read())
                {
                    RaizTO nodoActual = new RaizTO();
                    nodoActual.Id = "" + tabla["Id"];
                    nodoActual.Nivel = "" + tabla["Nivel"];
                    nodoActual.Descripcion = "" + tabla["Descripcion"];
                    nodoActual.TemaMay = "" + tabla["Tema_May"];
                    nodoActual.Consecutivo = (short)tabla["Consecutivo"];
                    nodoActual.Posicion = (short)tabla["Posicion"];
                    nodoActual.Muestra = (byte)tabla["Muestra"];
                    nodoActual.Cuantas = (int)tabla["Cuantas"];
                    nodoActual.Desplazamiento = (int)tabla["Desplazamiento"];
                    nodoActual.Tabla = "" + tabla["Vease"];
                    nodoActual.NoIus = (int)tabla["NoIus"];
                    nodoActual.Padre = "" + tabla["Padre"];
                    //nodoActual.Hiperlink = "" + fila["HyperL"];
                    lista.Add(nodoActual);
                }
                tabla.Close();

                //conexion.Close();
                List<TesisTO> registros = new List<TesisTO>();
                if (IUSConstants.TEMATICA_MANUAL.Contains(tablaOrigen))
                {
                    conexion = contextoBD.ContextConection;
                    sqlQuery = "select A.IdProg, A.Id, A.IUS, A.ConsecIndx, " +
                               " B.tpoTesis,B.OrdenInstancia, B.OrdenRubro," +
                               " B.OrdenTesis, B.tpoPonente1, B.tpoPonente2," +
                               " B.tpoPonente3, B.tpoPonente4, B.tpoPonente5," +
                               " B.TpoAsunto1, B.TpoAsunto2, B.TpoAsunto3, " +
                               " B.TpoAsunto4, B.TpoAsunto5 " +
                               " from " + tablaOrigen + "_temas A, tesis B" +
                               "       where A.id=" + valor +
                               "             AND B.IUS = A.IUS" +
                               "       order by a.ConsecIndx";
                    List<TesisTO> resultadoTematicaManual = new List<TesisTO>();
                    query = contextoBD.dataAdapter(sqlQuery);
                    datos = new DataSet();
                    query.Fill(datos);
                    tabla = datos.Tables[0].CreateDataReader();
                    //foreach (DataRow fila in tabla.Rows)
                    while (tabla.Read())
                    {
                        TesisTO tesisActual = new TesisTO();
                        tesisActual.ConsecIndxInt = (int)tabla["ConsecIndx"];
                        tesisActual.ConsecIndx = "" + tabla["ConsecIndx"];
                        tesisActual.setIus("" + tabla["ius"]);
                        tesisActual.TpoTesis = "" + tabla["tpoTesis"];
                        tesisActual.OrdenInstancia = (int)tabla["OrdenInstancia"];
                        tesisActual.OrdenRubro = (int)tabla["OrdenRubro"];
                        tesisActual.OrdenTesis = (int)tabla["OrdenTesis"];
                        tesisActual.Ponentes = new int[5];
                        tesisActual.Ponentes[0] = (short)tabla["tpoPonente1"];
                        tesisActual.Ponentes[1] = (short)tabla["tpoPonente2"];
                        tesisActual.Ponentes[2] = (short)tabla["tpoPonente3"];
                        tesisActual.Ponentes[3] = (short)tabla["tpoPonente4"];
                        tesisActual.Ponentes[4] = (short)tabla["tpoPonente5"];
                        tesisActual.TipoTesis = new int[5];
                        tesisActual.TipoTesis[0] = (short)tabla["TpoAsunto1"];
                        tesisActual.TipoTesis[1] = (short)tabla["TpoAsunto2"];
                        tesisActual.TipoTesis[2] = (short)tabla["TpoAsunto3"];
                        tesisActual.TipoTesis[3] = (short)tabla["TpoAsunto4"];
                        tesisActual.TipoTesis[4] = (short)tabla["TpoAsunto5"];
                        registros.Add(tesisActual);
                    }
                    tabla.Close();

                    return registros;
                }
                RaizTO nodo = lista.ElementAt(0);
                RegistryKey clase = Registry.LocalMachine.OpenSubKey(IUSConstants.IUS_REGISTRY_ENTRY);
                PathDirectorio = (String)clase.GetValue("Ruta");
                if (PathDirectorio != null || !PathDirectorio.Equals(""))
                {
                    Path = PathDirectorio + IUSConstants.IUS_TEMATICA_FILE;
                }
                else
                {
                    throw new Exception("\n*******************************ERROR AL LEER LA LLAVE" + Path + sqlQuery);
                }
                FileStream file = new FileStream(Path, FileMode.Open, FileAccess.Read);
                BinaryReader readerDeInts = new BinaryReader(file);
                int lugar = nodo.Desplazamiento;
                for (int contador = 0; contador < nodo.Cuantas; contador++)
                {
                    TesisTO registroActual = new TesisTO();
                    readerDeInts.BaseStream.Seek(lugar, SeekOrigin.Begin);
                    lugar += 4;
                    byte[] registroFile = readerDeInts.ReadBytes(4);
                    long nValAux = registroFile[3];
                    nValAux = nValAux * 256;
                    nValAux = nValAux + registroFile[2];
                    nValAux = nValAux * 65536;
                    long nValor = registroFile[1];
                    nValor = nValor * 256;
                    nValor = nValAux + nValor + registroFile[0];
                    registroActual.ConsecIndxInt = nValor;
                    registroActual.ConsecIndx = "" + nValor;
                    //ObtenRegistro(registroActual);
                    registros.Add(registroActual);
                }
                conexion.Close();
                return ObtenRegistros(registros);
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
                String mensaje = "TesisDAOImpl Exception at getTesisSubtemas(PartesTO\n" + e.Message + e.StackTrace;
                Logg.WriteEntry(mensaje);
                Logg.Close();
                throw new Exception(e.Message + "\n******************************* Path:" + Path +
                                    "\n*************************** SQLquery: " + sqlQuery +
                                    "\n*************************** Registros: " + lista.Count +
                                    "\n*************************** Directorio: " + PathDirectorio);
                //return new List<TesisTO>();
            }
        }

        public List<TesisTO> getTesisSubtemasSinonimoPalabra(String tablaOrigen, String valor)
        {
            BooleanQuery.SetMaxClauseCount(IUSConstants.CLAUSULAS);

            SpanishAnalyzer analyzer = new SpanishAnalyzer();
            List<String> querysEscritos = new List<string>();
            BooleanQuery QueryCompleto = new BooleanQuery();
            BooleanQuery queryGlobal = new BooleanQuery();
            IndexSearcher searcher = new IndexSearcher(IUSConstants.DIRECCION_INDEXER);
            String ordenarPor = "consecIndx";
            List<TesisTO> resultado = new List<TesisTO>();
            QueryParser queryGeneral = new QueryParser("", analyzer);
            String queryTotal = "(idProg: " + tablaOrigen + valor + ")";
            Hits hits = null;
            try
            {
                queryGlobal.Add(queryGeneral.Parse(queryTotal), BooleanClause.Occur.SHOULD);
                hits = searcher.Search(queryGlobal, new Sort(ordenarPor));
            }
            catch (Exception e)
            {
                BooleanClause[] clausulas = queryGlobal.GetClauses();
                if (!EventLog.SourceExists("IUS"))
                {
                    EventLog.CreateEventSource("IUS", "IUS");
                }
                EventLog Logg = new EventLog("IUS");
                Logg.Source = "IUS";
                String mensaje = "TesisDAOImpl Exception at getTesisSubtemasSinonimoPalabra(String, String)\n" + e.Message + e.StackTrace;
                Logg.WriteEntry(mensaje);
                Logg.Close();
            }
            int itemActual = 0;
            for (itemActual = 0; itemActual < hits.Length(); itemActual++)
            {
                Document item = hits.Doc(itemActual);
                TesisTO itemVerdadero;
                //try
                //{
                //    itemVerdadero = tesisCompletas[Int32.Parse(item.Get("consecIndx"))];// new TesisTO();
                //}
                //catch (Exception e)
                //{
                itemVerdadero = new TesisTO();
                itemVerdadero.Ius = item.Get("ius");
                itemVerdadero.OrdenInstancia = long.Parse(item.Get("OrdenInstancia"));
                itemVerdadero.OrdenTesis = long.Parse(item.Get("OrdenTesis"));
                itemVerdadero.OrdenRubro = long.Parse(item.Get("OrdenRubro"));
                itemVerdadero.ConsecIndx = item.Get("consecIndx");
                itemVerdadero.ConsecIndxInt = Int32.Parse(item.Get("consecIndx"));
                itemVerdadero.TpoTesis = item.Get("tpoTesis");
                itemVerdadero.TipoTesis = new int[5];
                itemVerdadero.TipoTesis[0] = int.Parse(item.Get("tpoAsunto1"));
                itemVerdadero.TipoTesis[1] = int.Parse(item.Get("tpoAsunto2"));
                itemVerdadero.TipoTesis[2] = int.Parse(item.Get("tpoAsunto3"));
                itemVerdadero.TipoTesis[3] = int.Parse(item.Get("tpoAsunto4"));
                itemVerdadero.TipoTesis[4] = int.Parse(item.Get("tpoAsunto5"));
                itemVerdadero.Ponentes = new int[5];
                itemVerdadero.Ponentes[0] = int.Parse(item.Get("TpoPonente1"));
                itemVerdadero.Ponentes[1] = int.Parse(item.Get("TpoPonente2"));
                itemVerdadero.Ponentes[2] = int.Parse(item.Get("TpoPonente3"));
                itemVerdadero.Ponentes[3] = int.Parse(item.Get("TpoPonente4"));
                itemVerdadero.Ponentes[4] = int.Parse(item.Get("TpoPonente5"));
                itemVerdadero.TipoPonente = new int[5];
                itemVerdadero.TipoPonente[0] = int.Parse(item.Get("TpoPon1"));
                itemVerdadero.TipoPonente[1] = int.Parse(item.Get("TpoPon2"));
                itemVerdadero.TipoPonente[2] = int.Parse(item.Get("TpoPon3"));
                itemVerdadero.TipoPonente[3] = int.Parse(item.Get("TpoPon4"));
                itemVerdadero.TipoPonente[4] = int.Parse(item.Get("TpoPon5"));
                //}
                resultado.Add(itemVerdadero);
            }
            return resultado;
        }

        public PaginadorTO getTesisSubtemasSinonimoPalabraPaginador(String tablaOrigen, String valor)
        {
            BooleanQuery.SetMaxClauseCount(IUSConstants.CLAUSULAS);
            SpanishAnalyzer analyzer = new SpanishAnalyzer();
            List<String> querysEscritos = new List<string>();
            BooleanQuery QueryCompleto = new BooleanQuery();
            BooleanQuery queryGlobal = new BooleanQuery();
            IndexSearcher searcher = new IndexSearcher(IUSConstants.DIRECCION_INDEXER);
            String ordenarPor = "consecIndx";
            PaginadorTO resultado = new PaginadorTO();
            QueryParser queryGeneral = new QueryParser("", analyzer);
            String queryTotal = "(idProg: " + tablaOrigen + valor + ")";
            Hits hits = null;
            try
            {
                queryGlobal.Add(queryGeneral.Parse(queryTotal), BooleanClause.Occur.SHOULD);
                hits = searcher.Search(queryGlobal, new Sort(ordenarPor));
            }
            catch (Exception e)
            {
                BooleanClause[] clausulas = queryGlobal.GetClauses();
                if (!EventLog.SourceExists("IUS"))
                {
                    EventLog.CreateEventSource("IUS", "IUS");
                }
                EventLog Logg = new EventLog("IUS");
                Logg.Source = "IUS";
                String mensaje = "TesisDAOImpl Exception at getTesisSubtemasSinonimoPalabraPaginador(String, String)\n" + e.Message + e.StackTrace;
                Logg.WriteEntry(mensaje);
                Logg.Close();
            }
            int itemActual = 0;
            List<Int32> ids = new List<int>();
            for (itemActual = 0; itemActual < hits.Length(); itemActual++)
            {
                Document item = hits.Doc(itemActual);
                Int32 itemVerdadero;
                itemVerdadero = Int32.Parse(item.Get("ius"));
                ids.Add(itemVerdadero);
            }
            resultado.Activo = true;
            resultado.Largo = ids.Count;
            resultado.ResultadoIds = ids;
            resultado.TimeStamp = DateTime.Now;
            resultado.TipoBusqueda = IUSConstants.BUSQUEDA_TESIS_TEMATICA;
            #if RED_JUR
            GuardarPaginador(resultado);
            #else
            ConsultasActuales.Add(resultado.Id, resultado);
            #endif
            return resultado;
        }

        public List<TesisTO> getTesisSinonimos(string tablaOrigen, string valor)
        {
            DbConnection conexion = contextoBD.ContextConection;

            try
            {
                String sqlQuery = "select Id_alterno, Cuantas, Desplazamiento " +
                                  " From  " + tablaOrigen +
                                  " Where id_alterno=" + valor;
                DataAdapter query = contextoBD.dataAdapter(sqlQuery, conexion);
                DataSet datos = new DataSet();
                query.Fill(datos);
                DataTableReader tabla = datos.Tables[0].CreateDataReader();
                conexion.Close();
                List<RaizTO> lista = new List<RaizTO>();
                //foreach (DataRow fila in tabla.Rows)
                while (tabla.Read())
                {
                    RaizTO nodoActual = new RaizTO();
                    nodoActual.Id = "" + tabla["Id_alterno"];
                    nodoActual.Cuantas = (int)tabla["Cuantas"];
                    nodoActual.Desplazamiento = (int)tabla["Desplazamiento"];
                    lista.Add(nodoActual);
                }
                tabla.Close();

                conexion.Close();
                List<TesisTO> registros = new List<TesisTO>();
                if (lista.Count > 0)
                {
                    RaizTO nodo = lista.ElementAt(0);
                    RegistryKey clase = Registry.LocalMachine.OpenSubKey(IUSConstants.IUS_REGISTRY_ENTRY);
                    String PathDirectorio = (String)clase.GetValue("Ruta");
                    String Path = PathDirectorio + IUSConstants.IUS_TEMATICA_FILE;
                    FileStream file = new FileStream(Path, FileMode.Open, FileAccess.Read);
                    BinaryReader readerDeInts = new BinaryReader(file);
                    int lugar = nodo.Desplazamiento;
                    for (int contador = 0; contador < nodo.Cuantas; contador++)
                    {
                        TesisTO registroActual = new TesisTO();
                        readerDeInts.BaseStream.Seek(lugar, SeekOrigin.Begin);
                        lugar += 4;
                        byte[] registroFile = readerDeInts.ReadBytes(4);
                        long nValAux = registroFile[3];
                        nValAux = nValAux * 256;
                        nValAux = nValAux + registroFile[2];
                        nValAux = nValAux * 65536;
                        long nValor = registroFile[1];
                        nValor = nValor * 256;
                        nValor = nValAux + nValor + registroFile[0];
                        registroActual.ConsecIndxInt = nValor;
                        registroActual.ConsecIndx = "" + nValor;
                        //ObtenRegistro(registroActual);
                        registros.Add(registroActual);
                    }
                }
                return ObtenRegistros(registros);
            }
            catch (Exception exc)
            {
                conexion.Close();
                if (!EventLog.SourceExists("IUS"))
                {
                    EventLog.CreateEventSource("IUS", "IUS");
                }
                EventLog Logg = new EventLog("IUS");
                Logg.Source = "IUS";
                String mensaje = "TesisDAOImpl Exception at getTesisSinonimos(string, string)\n" + exc.Message + exc.StackTrace;
                Logg.WriteEntry(mensaje);
                Logg.Close();
                return new List<TesisTO>();
            }
        }

        public List<TesisTO> getTesisTesauro(string identificador)
        {
            DbConnection conexion = contextoBD.ContextConection;
            String sqlQuery = "select A.ius, A.ConsecIndx, A.OrdenInstancia, A.OrdenTesis, A.OrdenRubro, A.tpoTesis, " +
                              " A.TpoAsunto1, A.TpoAsunto2, A.TpoAsunto3, A.TpoAsunto4, A.TpoAsunto5, " +
                              " A.tpoPonente1, A.tpoPonente2, A.tpoPonente3, A.tpoPonente4, A.tpoPonente5  " +
                              "  from tesis As A" +
                              "  where A.ConsecIndx IN ( " +
                              "  SELECT ConsecIndx From  THE_TESIS Where IDTema = " + identificador + ") order by A.ConsecIndx";
            DataAdapter query = contextoBD.dataAdapter(sqlQuery, conexion);
            DataSet datos = new DataSet();
            query.Fill(datos);
            DataTableReader tabla = datos.Tables[0].CreateDataReader();
            conexion.Close();
            List<TesisTO> lista = new List<TesisTO>();
            //foreach (DataRow fila in tabla.Rows)
            while (tabla.Read())
            {
                TesisTO tesisActual = new TesisTO();
                tesisActual.setIus("" + tabla["ius"]);
                tesisActual.setConsecIndx("" + tabla["consecIndx"]);
                tesisActual.OrdenInstancia = (int)tabla["ordenInstancia"];
                tesisActual.OrdenRubro = (int)tabla["OrdenRubro"];
                tesisActual.OrdenTesis = (int)tabla["OrdenTesis"];
                tesisActual.TpoTesis = "" + tabla["tpoTesis"];
                tesisActual.Ponentes = new int[5];
                tesisActual.Ponentes[0] = (short)tabla["tpoPonente1"];
                tesisActual.Ponentes[1] = (short)tabla["tpoPonente2"];
                tesisActual.Ponentes[2] = (short)tabla["tpoPonente3"];
                tesisActual.Ponentes[3] = (short)tabla["tpoPonente4"];
                tesisActual.Ponentes[4] = (short)tabla["tpoPonente5"];
                tesisActual.TipoTesis = new int[5];
                tesisActual.TipoTesis[0] = (short)tabla["TpoAsunto1"];
                tesisActual.TipoTesis[1] = (short)tabla["TpoAsunto2"];
                tesisActual.TipoTesis[2] = (short)tabla["TpoAsunto3"];
                tesisActual.TipoTesis[3] = (short)tabla["TpoAsunto4"];
                tesisActual.TipoTesis[4] = (short)tabla["TpoAsunto5"];
                lista.Add(tesisActual);
            }
            tabla.Close();

            //conexion.Close();
            return lista;
        }

        private List<TesisTO> ObtenRegistros(List<TesisTO> registros)
        {
            int cuantosQuerys = (registros.Count / 1000) + 1;
            int contadorFinal = 0;
            int lugarFinal = 0;
            String[] ins = new String[cuantosQuerys];
            for (int queryActual = 0; queryActual < cuantosQuerys; queryActual++)
            {
                ins[queryActual] = "";
            }
            foreach (TesisTO itemTesis in registros)
            {
                if (contadorFinal > 999)
                {
                    lugarFinal++;
                    contadorFinal = 0;
                }
                ins[lugarFinal] += itemTesis.ConsecIndx + ",";
                contadorFinal++;
            }
            for (int queryActual = 0; queryActual < cuantosQuerys; queryActual++)
            {
                if (!ins[queryActual].Equals(""))
                {
                    ins[queryActual] = ins[queryActual].Substring(0, ins[queryActual].Length - 1);
                }
            }
            DbConnection conexion = contextoBD.ContextConection;
            List<TesisTO> lista = new List<TesisTO>();
            foreach (String inActual in ins)
            {
                if (!inActual.Equals(""))
                {
                    String sqlQuery = "select A.ius, A.ConsecIndx, A.OrdenInstancia, A.OrdenTesis, A.OrdenRubro, A.tpoTesis, " +
                                      " A.TpoAsunto1, A.TpoAsunto2, A.TpoAsunto3, A.TpoAsunto4, A.TpoAsunto5, " +
                                      " A.tpoPonente1, A.tpoPonente2, A.tpoPonente3, A.tpoPonente4, A.tpoPonente5  " +
                                      "  from tesis As A" +
                                      "  where A.ConsecIndx IN ( " + inActual +
                                      ")";
                    DataAdapter query = contextoBD.dataAdapter(sqlQuery, conexion);
                    DataSet datos = new DataSet();
                    query.Fill(datos);
                    DataTableReader tabla = datos.Tables[0].CreateDataReader();
                    conexion.Close();
                    //foreach (DataRow fila in tabla.Rows)
                    while (tabla.Read())
                    {
                        TesisTO tesisActual = new TesisTO();
                        tesisActual.setIus("" + tabla["ius"]);
                        tesisActual.setConsecIndx("" + tabla["consecIndx"]);
                        tesisActual.OrdenInstancia = (int)tabla["ordenInstancia"];
                        tesisActual.OrdenRubro = (int)tabla["OrdenRubro"];
                        tesisActual.OrdenTesis = (int)tabla["OrdenTesis"];
                        tesisActual.TpoTesis = "" + tabla["tpoTesis"];
                        tesisActual.Ponentes = new int[5];
                        tesisActual.Ponentes[0] = (short)tabla["tpoPonente1"];
                        tesisActual.Ponentes[1] = (short)tabla["tpoPonente2"];
                        tesisActual.Ponentes[2] = (short)tabla["tpoPonente3"];
                        tesisActual.Ponentes[3] = (short)tabla["tpoPonente4"];
                        tesisActual.Ponentes[4] = (short)tabla["tpoPonente5"];
                        tesisActual.TipoTesis = new int[5];
                        tesisActual.TipoTesis[0] = (short)tabla["TpoAsunto1"];
                        tesisActual.TipoTesis[1] = (short)tabla["TpoAsunto2"];
                        tesisActual.TipoTesis[2] = (short)tabla["TpoAsunto3"];
                        tesisActual.TipoTesis[3] = (short)tabla["TpoAsunto4"];
                        tesisActual.TipoTesis[4] = (short)tabla["TpoAsunto5"];
                        lista.Add(tesisActual);
                    }
                    tabla.Close();
                }
            }

            //conexion.Close();
            return lista;
        }

        //private void ObtenRegistro(TesisTO registroActual)
        //{
        //    //ConsecIndx = registroActual.ConsecIndxInt;
        //    TesisTO registroFinal = tesisCompletas[registroActual.ConsecIndxInt];
        //    registroActual.Ius = registroFinal.Ius;
        //    registroActual.TpoTesis = registroFinal.TpoTesis;
        //    registroActual.OrdenTesis = registroFinal.OrdenTesis;
        //    registroActual.OrdenRubro = registroFinal.OrdenRubro;
        //    registroActual.OrdenInstancia = registroFinal.OrdenInstancia;
        //    registroActual.TipoTesis = registroFinal.TipoTesis;
        //    registroActual.Ponentes = registroFinal.Ponentes;
        //}

        private static bool esConsecutivo(TesisTO item)
        {
            // Int32 comparar = Int32.Parse(item.ConsecIndx);
            return (item.ConsecIndxInt == TesisDAOImpl.ConsecIndx);
        }

        public List<RaizTO> getRaizConstitucional(string id)
        {
            DbConnection conexion = contextoBD.ContextConection;
            try
            {
                String sqlQuery = "select IdTema, Nivel, Descripcion,DescripcionStr, IDPadre," +
                                  " IDUser, Fecha, hora" +
                                  " From  The_Temas" +
                                  " Where IdPadre = " + id +
                                  " Order By DescripcionStr";
                DataAdapter query = contextoBD.dataAdapter(sqlQuery, conexion);
                DataSet datos = new DataSet();
                query.Fill(datos);
                DataTableReader tabla = datos.Tables[0].CreateDataReader();
                conexion.Close();
                List<RaizTO> lista = new List<RaizTO>();
                //foreach (DataRow fila in tabla.Rows)
                while (tabla.Read())
                {
                    RaizTO nodoActual = new RaizTO();
                    nodoActual.Id = "" + tabla["IdTema"];
                    if (!id.Equals("0"))
                    {
                        int cuantos = ObtenCuantosTemaTesauro(Int32.Parse(nodoActual.Id));
                        nodoActual.Descripcion = "ID " + tabla["Descripcion"] + " [" + cuantos + "]";
                    }
                    else
                    {
                        nodoActual.Descripcion = "" + tabla["Descripcion"];
                    }
                    nodoActual.Nivel = "" + tabla["Nivel"];
                    nodoActual.Padre = "" + tabla["IDPadre"];
                    nodoActual.TemaMay = "" + tabla["DescripcionStr"];
                    nodoActual.Tabla = "" + tabla["IdUser"];
                    lista.Add(nodoActual);
                }
                tabla.Close();

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
                String mensaje = "TesisDAOImpl Exception at getRaizconstitucional(string)\n" + e.Message + e.StackTrace;
                Logg.WriteEntry(mensaje);
                Logg.Close();
                return new List<RaizTO>();
            }
        }

        public List<RaizTO> getRaizConstitucional(string id, String Busqueda)
        {
            DbConnection conexion = contextoBD.ContextConection;
            try
            {
                String[] palabras = Busqueda.Split();
                String condicion = "";
                foreach (String item in palabras)
                {
                    condicion += " DescripcionStr like '% " + item + " %' AND ";
                }
                condicion = condicion.Substring(0, condicion.Length - 4);
                String sqlQuery = "select IdTema, Nivel, Descripcion,DescripcionStr, IDPadre," +
                                  " IDUser, Fecha, hora" +
                                  " From  The_Temas" +
                                  " Where "//IdPadre = " + id
                                  +
                                  condicion +
                                  " Order By DescripcionStr";
                DataAdapter query = contextoBD.dataAdapter(sqlQuery, conexion);
                DataSet datos = new DataSet();
                query.Fill(datos);
                DataTableReader tabla = datos.Tables[0].CreateDataReader();
                conexion.Close();
                List<RaizTO> lista = new List<RaizTO>();
                //foreach (DataRow fila in tabla.Rows)
                while (tabla.Read())
                {
                    RaizTO nodoActual = new RaizTO();
                    nodoActual.Id = "" + tabla["IdTema"];
                    if (!id.Equals("0"))
                    {
                        int cuantos = ObtenCuantosTemaTesauro(Int32.Parse(nodoActual.Id));
                        nodoActual.Descripcion = "ID " + tabla["Descripcion"] + " [" + cuantos + "]";
                    }
                    else
                    {
                        nodoActual.Descripcion = "" + tabla["Descripcion"];
                    }
                    nodoActual.Nivel = "" + tabla["Nivel"];
                    nodoActual.Padre = "" + tabla["IDPadre"];
                    nodoActual.TemaMay = "" + tabla["DescripcionStr"];
                    nodoActual.Tabla = "" + tabla["IdUser"];
                    lista.Add(nodoActual);
                }
                tabla.Close();

                conexion = contextoBD.ContextConection;
                sqlQuery = "select IdTema,IdAscendente, Descripcion,DescripcionStr" +
                           " From  The_Ascendente" +
                           " Where "//IdPadre = " + id
                           +
                           condicion +
                           " Order By DescripcionStr";
                query = contextoBD.dataAdapter(sqlQuery, conexion);
                datos = new DataSet();
                query.Fill(datos);
                tabla = datos.Tables[0].CreateDataReader();
                conexion.Close();
                //foreach (DataRow fila in tabla.Rows)
                while (tabla.Read())
                {
                    RaizTO nodoActual = new RaizTO();
                    nodoActual.Id = "" + tabla["IdTema"];
                    nodoActual.Padre = "" + tabla["IdAscendente"];
                    if (!id.Equals("0"))
                    {
                        int cuantos = ObtenCuantosTemaTesauro(Int32.Parse(nodoActual.Id));
                        nodoActual.Descripcion = "ID " + tabla["Descripcion"] + " [" + cuantos + "]";
                    }
                    else
                    {
                        nodoActual.Descripcion = "" + tabla["Descripcion"];
                    }
                    //nodoActual.Nivel = "" + fila["Nivel"];

                    nodoActual.TemaMay = "" + tabla["DescripcionStr"];
                    //nodoActual.Tabla = "" + fila["IdUser"];
                    lista.Add(nodoActual);
                }
                tabla.Close();

                conexion = contextoBD.ContextConection;
                sqlQuery = "select IdTema, Descripcion,DescripcionStr, IDPadre" +
                           " From  The_Sinonimos" +
                           " Where "//IdPadre = " + id
                           +
                           condicion +
                           " Order By DescripcionStr";
                query = contextoBD.dataAdapter(sqlQuery, conexion);
                datos = new DataSet();
                query.Fill(datos);
                tabla = datos.Tables[0].CreateDataReader();
                conexion.Close();
                //foreach (DataRow fila in tabla.Rows)
                while (tabla.Read())
                {
                    RaizTO nodoActual = new RaizTO();
                    nodoActual.Id = "" + tabla["IdTema"];
                    if (!id.Equals("0"))
                    {
                        int cuantos = ObtenCuantosTemaTesauro(Int32.Parse(nodoActual.Id));
                        nodoActual.Descripcion = "ID " + tabla["Descripcion"] + " [" + cuantos + "]";
                    }
                    else
                    {
                        nodoActual.Descripcion = "" + tabla["Descripcion"];
                    }
                    //nodoActual.Nivel = "" + fila["Nivel"];
                    nodoActual.Padre = "" + tabla["IDPadre"];
                    nodoActual.TemaMay = "" + tabla["DescripcionStr"];
                    //nodoActual.Tabla = "" + fila["IdUser"];
                    lista.Add(nodoActual);
                }
                tabla.Close();

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
                String mensaje = "TesisDAOImpl Exception at getRaizConstitucional(string, string)\n" + e.Message + e.StackTrace;
                Logg.WriteEntry(mensaje);
                Logg.Close();
                return new List<RaizTO>();
            }
        }

        public RaizTO getIdConstitucional(string id)
        {
            DbConnection conexion = contextoBD.ContextConection;
            try
            {
                String sqlQuery = "select IdTema, Nivel, Descripcion,DescripcionStr, IDPadre," +
                                  " IDUser, Fecha, hora" +
                                  " From  The_Temas" +
                                  " Where IdTema = " + id;
                DataAdapter query = contextoBD.dataAdapter(sqlQuery, conexion);
                DataSet datos = new DataSet();
                query.Fill(datos);
                DataTableReader tabla = datos.Tables[0].CreateDataReader();
                conexion.Close();
                List<RaizTO> lista = new List<RaizTO>();
                //foreach (DataRow fila in tabla.Rows)
                while (tabla.Read())
                {
                    RaizTO nodoActual = new RaizTO();
                    nodoActual.Id = "" + tabla["IdTema"];
                    nodoActual.Nivel = "" + tabla["Nivel"];
                    nodoActual.Padre = "" + tabla["IDPadre"];
                    int cuantos = ObtenCuantosTemaTesauro(Int32.Parse(nodoActual.Id));
                    nodoActual.Descripcion = "" + tabla["Descripcion"] + " [" + cuantos + "]";
                    nodoActual.TemaMay = "" + tabla["DescripcionStr"];
                    nodoActual.Tabla = "" + tabla["IdUser"];
                    lista.Add(nodoActual);
                }
                tabla.Close();

                //conexion.Close();
                return lista.ElementAt(0);
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
                String mensaje = "TesisDAOImpl Exception at getidConstitucional(string)\n" + e.Message + e.StackTrace;
                Logg.WriteEntry(mensaje);
                Logg.Close();
                return new RaizTO();
            }
        }

        public RaizTO getAscendenteConstitucional(string id)
        {
            DbConnection conexion = contextoBD.ContextConection;
            try
            {
                String sqlQuery = "select IdTema, IdAscendente, Descripcion,DescripcionStr " +
                                  " From  The_Ascendente" +
                                  " Where IdAscendente = " + id;
                DataAdapter query = contextoBD.dataAdapter(sqlQuery, conexion);
                DataSet datos = new DataSet();
                query.Fill(datos);
                DataTableReader tabla = datos.Tables[0].CreateDataReader();
                conexion.Close();
                List<RaizTO> lista = new List<RaizTO>();
                //foreach (DataRow fila in tabla.Rows)
                while (tabla.Read())
                {
                    RaizTO nodoActual = new RaizTO();
                    nodoActual.Id = "" + tabla["IdTema"];
                    int cuantos = ObtenCuantosTemaTesauro(Int32.Parse(nodoActual.Id));
                    nodoActual.Descripcion = "" + tabla["Descripcion"] + " [" + cuantos + "]";
                    nodoActual.TemaMay = "" + tabla["DescripcionStr"];
                    lista.Add(nodoActual);
                }
                tabla.Close();

                //conexion.Close();
                return lista.ElementAt(0);
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
                String mensaje = "TesisDAOImpl Exception at getAscendenteConstitucional(PartesTO\n" + e.Message + e.StackTrace;
                Logg.WriteEntry(mensaje);
                Logg.Close();
                return new RaizTO();
            }
        }

        public List<RaizTO> getSinonimoConstitucional(String Id, int tipo)
        {
            DbConnection conexion = contextoBD.ContextConection;
            try
            {
                String sqlQuery = "select IdTema, IdPadre, Descripcion,tipo, DescripcionStr " +
                                  " From  The_sinonimos" +
                                  " Where IdPadre = " + Id +
                                  "   AND tipo = " + tipo +
                                  " Order by DescripcionStr";
                DataAdapter query = contextoBD.dataAdapter(sqlQuery, conexion);
                DataSet datos = new DataSet();
                query.Fill(datos);
                DataTableReader tabla = datos.Tables[0].CreateDataReader();
                conexion.Close();
                List<RaizTO> lista = new List<RaizTO>();
                //foreach (DataRow fila in tabla.Rows)
                while (tabla.Read())
                {
                    RaizTO nodoActual = new RaizTO();
                    nodoActual.Id = "" + tabla["IdTema"];
                    int cuantos = 0;
                    cuantos = ObtenCuantosTemaTesauro(Int32.Parse(nodoActual.Id));
                    nodoActual.Descripcion = "" + tabla["Descripcion"] + " [" + cuantos + "]";
                    nodoActual.TemaMay = "" + tabla["DescripcionStr"];
                    lista.Add(nodoActual);
                }
                tabla.Close();

                conexion.Close();
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
                String mensaje = "TesisDAOImpl Exception at getSinonimoConstitucional\n" + e.Message + e.StackTrace;
                Logg.WriteEntry(mensaje);
                Logg.Close();
                return new List<RaizTO>();
            }
        }

        private int ObtenCuantosTemaTesauro(int id)
        {
            int cuantos = 0;
            DbConnection conexionauxiliar = contextoBD.ContextConection;
            String sqlQuery = "SELECT Count(ConsecIndx) As Cuantos From  THE_TESIS Where IDTema = " + id;
            DataAdapter queryAuxiliar = contextoBD.dataAdapter(sqlQuery, conexionauxiliar);
            DataSet datosAuxiliares = new DataSet();
            queryAuxiliar.Fill(datosAuxiliares);
            DataTableReader tablaAuxiliar = datosAuxiliares.Tables[0].CreateDataReader();
            //foreach (DataRow item in tablaAuxiliar.Rows)
            while (tablaAuxiliar.Read())
            {
                cuantos = (int)tablaAuxiliar["Cuantos"];
            }
            tablaAuxiliar.Close();
            conexionauxiliar.Close();
            return cuantos;
        }

        /// <summary>
        /// Devuelve la lista completa del catalogo de ponentes.
        /// </summary>
        /// <returns>La lista del catálogo de ponentes</returns>
        public List<PonenteTO> getCatalogoPonente()
        {
            DbConnection conexion = contextoBD.ContextConection;
            try
            {
                String sqlQuery = "select Activo, DescTipo, IdOrder, IdTipo, letraIni, Parte " +
                                  " From  clasifPonente where Activo=1 order by DescTipo";
                DataAdapter query = contextoBD.dataAdapter(sqlQuery, conexion);
                DataSet datos = new DataSet();
                query.Fill(datos);
                DataTableReader tabla = datos.Tables[0].CreateDataReader();
                conexion.Close();
                List<PonenteTO> lista = new List<PonenteTO>();
                //foreach (DataRow fila in tabla.Rows)
                while (tabla.Read())
                {
                    PonenteTO nodoActual = new PonenteTO();
                    nodoActual.Activo = (short)tabla["Activo"];
                    nodoActual.DescTipo = "" + tabla["DescTipo"];
                    nodoActual.IdOrder = (short)tabla["IdOrder"];
                    nodoActual.IdTipo = (short)tabla["IdTipo"];
                    nodoActual.LetraIni = "" + tabla["LetraIni"];
                    nodoActual.Parte = (short)tabla["Parte"];
                    lista.Add(nodoActual);
                }
                tabla.Close();

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
                String mensaje = "TesisDAOImpl Exception at getCatalogoPonente(PartesTO\n" + e.Message + e.StackTrace;
                Logg.WriteEntry(mensaje);
                Logg.Close();
                return new List<PonenteTO>();
            }
        }

        /// <summary>
        /// Devuelve la lista completa del catalogo de asuntos.
        /// </summary>
        /// <returns>La lista del catálogo de asuntos</returns>
        public List<AsuntoTO> getCatalogoAsunto()
        {
            DbConnection conexion = contextoBD.ContextConection;
            try
            {
                String sqlQuery = "select Activo, DescTipo, IdTipo, letraIni, Parte " +
                                  " From  clasifAsunto where Activo =1 order by DescTipo";
                DataAdapter query = contextoBD.dataAdapter(sqlQuery, conexion);
                DataSet datos = new DataSet();
                query.Fill(datos);
                DataTableReader tabla = datos.Tables[0].CreateDataReader();
                conexion.Close();
                List<AsuntoTO> lista = new List<AsuntoTO>();
                //foreach (DataRow fila in tabla.Rows)
                while (tabla.Read())
                {
                    AsuntoTO nodoActual = new AsuntoTO();
                    nodoActual.Activo = (short)tabla["Activo"];
                    nodoActual.DescTipo = "" + tabla["DescTipo"];
                    nodoActual.IdTipo = (short)tabla["IdTipo"];
                    nodoActual.LetraIni = "" + tabla["LetraIni"];
                    nodoActual.Parte = (short)tabla["Parte"];
                    lista.Add(nodoActual);
                }
                tabla.Close();

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
                String mensaje = "TesisDAOImpl Exception at getCatalogoAsunto()\n" + e.Message + e.StackTrace;
                Logg.WriteEntry(mensaje);
                Logg.Close();
                return new List<AsuntoTO>();
            }
        }

        /// <summary>
        /// Devuelve la lista completa del catalogo de asuntos.
        /// </summary>
        /// <returns>La lista del catálogo de asuntos</returns>
        public List<AsuntoTO> getCatalogoAsunto(int[] partes)
        {
            DbConnection conexion = contextoBD.ContextConection;
            try
            {
                String sqlQuery = "select Activo, DescTipo, IdTipo, letraIni, Parte " +
                                  " From  clasifAsunto where Activo =1 AND parte in( ";
                sqlQuery = sqlQuery + " order by DescTipo";
                DataAdapter query = contextoBD.dataAdapter(sqlQuery, conexion);
                DataSet datos = new DataSet();
                query.Fill(datos);
                DataTableReader tabla = datos.Tables[0].CreateDataReader();
                conexion.Close();
                List<AsuntoTO> lista = new List<AsuntoTO>();
                //foreach (DataRow fila in tabla.Rows)
                while (tabla.Read())
                {
                    AsuntoTO nodoActual = new AsuntoTO();
                    nodoActual.Activo = (short)tabla["Activo"];
                    nodoActual.DescTipo = "" + tabla["DescTipo"];
                    nodoActual.IdTipo = (short)tabla["IdTipo"];
                    nodoActual.LetraIni = "" + tabla["LetraIni"];
                    nodoActual.Parte = (short)tabla["Parte"];
                    lista.Add(nodoActual);
                }
                tabla.Close();

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
                String mensaje = "TesisDAOImpl Exception at getCatalogoasunto(Int[])\n" + e.Message + e.StackTrace;
                Logg.WriteEntry(mensaje);
                Logg.Close();
                return new List<AsuntoTO>();
            }
        }

        public List<TreeNodeDataTO> getNodosConstitucionales(List<String> ids)
        {
            DbConnection conexion = contextoBD.ContextConection;

            try
            {
                String sqlQuery = "select IdTema, Nivel, Descripcion,DescripcionStr, IDPadre," +
                                  " IDUser, Fecha, hora" +
                                  " From  The_Temas" +
                                  " Where IdTema in ( " + String.Join(",", ids.ToArray()) +
                                  ") Order By DescripcionStr";
                DataAdapter query = contextoBD.dataAdapter(sqlQuery, conexion);
                DataSet datos = new DataSet();
                query.Fill(datos);
                DataTableReader tabla = datos.Tables[0].CreateDataReader();
                conexion.Close();
                List<TreeNodeDataTO> lista = new List<TreeNodeDataTO>();
                //foreach (DataRow fila in tabla.Rows)
                while (tabla.Read())
                {
                    TreeNodeDataTO nodoActual = new TreeNodeDataTO();
                    nodoActual.Id = "" + tabla["IdTema"];
                    String IdPadre = "" + tabla["IDPadre"];
                    if (!IdPadre.Equals("0"))
                    {
                        int cuantos = ObtenCuantosTemaTesauro(Int32.Parse(nodoActual.Id));
                        nodoActual.Label = "ID " + tabla["Descripcion"] + " [" + cuantos + "]";
                        nodoActual.Label = nodoActual.Label.ToUpper();
                    }
                    else
                    {
                        nodoActual.Label = "" + tabla["Descripcion"];
                        nodoActual.Label = nodoActual.Label.ToUpper();
                    }
                    nodoActual.Padre = "" + tabla["IDPadre"];
                    nodoActual.Target = "" + tabla["IdUser"];
                    nodoActual.Href = "";
                    lista.Add(nodoActual);
                }
                tabla.Close();

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
                String mensaje = "TesisDAOImpl Exception at getNodosConstitucionales(List<String>)\n" + e.Message + e.StackTrace;
                Logg.WriteEntry(mensaje);
                Logg.Close();
                return new List<TreeNodeDataTO>();
            }
        }

        public TreeNodeDataTO getNodoConstitucional(string id)
        {
            DbConnection conexion = contextoBD.ContextConection;
            try
            {
                String sqlQuery = "select IdTema, Nivel, Descripcion,DescripcionStr, IDPadre," +
                                  " IDUser, Fecha, hora" +
                                  " From  The_Temas" +
                                  " Where IdTema = " + id +
                                  " Order By DescripcionStr";
                DataAdapter query = contextoBD.dataAdapter(sqlQuery, conexion);
                DataSet datos = new DataSet();
                query.Fill(datos);
                DataTableReader tabla = datos.Tables[0].CreateDataReader();
                conexion.Close();
                List<TreeNodeDataTO> lista = new List<TreeNodeDataTO>();
                //foreach (DataRow fila in tabla.Rows)
                while (tabla.Read())
                {
                    TreeNodeDataTO nodoActual = new TreeNodeDataTO();
                    nodoActual.Id = "" + tabla["IdTema"];
                    String IdPadre = "" + tabla["IDPadre"];
                    if (!IdPadre.Equals("0"))
                    {
                        int cuantos = ObtenCuantosTemaTesauro(Int32.Parse(nodoActual.Id));
                        nodoActual.Label = "ID " + tabla["Descripcion"] + " [" + cuantos + "]";
                        nodoActual.Label = nodoActual.Label.ToUpper();
                    }
                    else
                    {
                        nodoActual.Label = "" + tabla["Descripcion"];
                        nodoActual.Label = nodoActual.Label.ToUpper();
                    }
                    nodoActual.Padre = "" + tabla["IDPadre"];
                    nodoActual.Target = "" + tabla["IdUser"];
                    nodoActual.Href = "";
                    lista.Add(nodoActual);
                }
                tabla.Close();

                //conexion.Close();
                return lista.ElementAt(0);
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
                String mensaje = "TesisDAOImpl Exception at getNodoConstitucional(string)\n" + e.Message + e.StackTrace;
                Logg.WriteEntry(mensaje);
                Logg.Close();
                return new TreeNodeDataTO();
            }
        }

        ///<Summary>
        ///Devuelve de un determinado Id dentro de las consultas activas
        ///un listado de hasta IUSConstants.BLOQUE_PAGINADOR tesis a partir
        ///del registro de inicio
        ///</summary>
        public List<TesisTO> getTesisPaginadas(int IdConsulta, int PosicionPaginador)
        {
            #if !RED_JUR
            PaginadorTO listadoOriginal = ConsultasActuales[IdConsulta];
            listadoOriginal.TimeStamp = DateTime.Now;
            if ((listadoOriginal == null) ||
                (PosicionPaginador >= listadoOriginal.ResultadoIds.Count))
            {
                return null;
            }
            MostrarPorIusTO listaIds = new MostrarPorIusTO();
            List<int> solicitados = new List<int>();
            int contador = 0;
            for (contador = 0;
                 ((contador < IUSConstants.BLOQUE_PAGINADOR) &&
                  ((contador + PosicionPaginador) < listadoOriginal.ResultadoIds.Count));
                 contador++)
            {
                solicitados.Add(listadoOriginal.ResultadoIds.ElementAt(contador + PosicionPaginador));
            }
            listaIds.Listado = solicitados;
            listaIds.OrderBy = "consecindx";
            listaIds.OrderType = "asc";
            #else
            int final = PosicionPaginador+ IUSConstants.BLOQUE_PAGINADOR+1;
            DbConnection conexion = contextoBD.ContextConection;
            String SqlDatosPaginador = "select id, time, tipoBusqueda from "+IUSConstants.PAGINADOR_BD+"IdsActivos where Id = " + IdConsulta;
            DataAdapter adaptador = contextoBD.dataAdapter(SqlDatosPaginador, conexion);
            DataSet ds = new DataSet();
            adaptador.Fill(ds);
            DataTableReader TablaResultado = ds.Tables[0].CreateDataReader();
            conexion.Close();
            #endif
            #if !RED_JUR
            DataTableReader TablaResultado = getTesis(listaIds);
            int tipoBusqueda = 0;
            tipoBusqueda = listadoOriginal.TipoBusqueda;
            #else
            String sqlQuery = "select ius, parte, rubro,      epoca, " +
                                     "sala,  tesis,      locAbr, " +
                                     "ta_tj, imageOther, consecIndx, " +
                                     "OrdenInstancia, OrdenTesis, OrdenRubro, " +
                                     "tpoTesis, tpoPonente1, tpoPonente2, " +
                                     "tpoPonente3, tpoPonente4, tpoPonente5, " +
                                     "tpoPon1, tpoPon2, tpoPon3, tpoPon4, tpoPon5, " +
                                     "TpoAsunto1, TpoAsunto2, TpoAsunto3, TpoAsunto4, TpoAsunto5" +
                                " from Tesis  As A with(nolock), ctpoTesis  As D with(nolock) where  " +
                                  "        A.tpoTesis = D.id " +
                                  "        AND ius  in (select ius from " + IUSConstants.PAGINADOR_BD + "Paginador with(nolock) where Id =" + IdConsulta + " and " +
                                                    "Consec > " + PosicionPaginador + " AND " +
                                                    "Consec < " + final+ ")";
            int id = 0;
            DateTime caducidad = DateTime.Now;
            int tipoBusqueda = 0;
            while (TablaResultado.Read())
            {
                id = (int)TablaResultado["id"];
                caducidad = (DateTime)TablaResultado["time"];
                tipoBusqueda = (int)TablaResultado["tipoBusqueda"];
            }
            TablaResultado.Close();
            conexion = contextoBD.ContextConection;
            adaptador = contextoBD.dataAdapter(sqlQuery, conexion);
            ds = new DataSet();
            adaptador.Fill(ds);
            TablaResultado = ds.Tables[0].CreateDataReader(); //getTesis(listaIds);
            conexion.Close();
            #endif
            List<TesisTO> resultado = new List<TesisTO>();
            //foreach (DataRow fila in TablaResultado.Rows)
            while (TablaResultado.Read())
            {
                TesisTO tesisActual = new TesisTO();
                tesisActual.setIus("" + TablaResultado["ius"]);
                if (tipoBusqueda == IUSConstants.BUSQUEDA_ESPECIALES)
                {
                    tesisActual.setParte("" + TablaResultado["parte"]);
                    tesisActual.setRubro("" + TablaResultado["rubro"]);
                    tesisActual.setEpoca("" + TablaResultado["epoca"]);
                    tesisActual.setSala("" + TablaResultado["sala"]);
                    tesisActual.setTesis("" + TablaResultado["tesis"]);
                    tesisActual.setLocAbr("" + TablaResultado["locAbr"]);
                    tesisActual.setTa_tj("" + TablaResultado["ta_tj"]);
                    tesisActual.setImageOther("" + TablaResultado["imageOther"]);
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
                tesisActual.setConsecIndx("" + TablaResultado["consecIndx"]);
                tesisActual.OrdenInstancia = (int)TablaResultado["OrdenInstancia"];
                tesisActual.OrdenTesis = (int)TablaResultado["OrdenTesis"];
                tesisActual.OrdenRubro = (int)TablaResultado["OrdenRubro"];
                tesisActual.setTpoTesis("" + TablaResultado["tpoTesis"]);
                tesisActual.Ponentes = new int[5];
                tesisActual.Ponentes[0] = (short)TablaResultado["tpoPonente1"];
                tesisActual.Ponentes[1] = (short)TablaResultado["tpoPonente2"];
                tesisActual.Ponentes[2] = (short)TablaResultado["tpoPonente3"];
                tesisActual.Ponentes[3] = (short)TablaResultado["tpoPonente4"];
                tesisActual.Ponentes[4] = (short)TablaResultado["tpoPonente5"];
                tesisActual.TipoPonente = new int[5];
                tesisActual.TipoPonente[0] = (short)TablaResultado["tpoPon1"];
                tesisActual.TipoPonente[1] = (short)TablaResultado["tpoPon2"];
                tesisActual.TipoPonente[2] = (short)TablaResultado["tpoPon3"];
                tesisActual.TipoPonente[3] = (short)TablaResultado["tpoPon4"];
                tesisActual.TipoPonente[4] = (short)TablaResultado["tpoPon5"];
                tesisActual.TipoTesis = new int[5];
                tesisActual.TipoTesis[0] = (short)TablaResultado["TpoAsunto1"];
                tesisActual.TipoTesis[1] = (short)TablaResultado["TpoAsunto2"];
                tesisActual.TipoTesis[2] = (short)TablaResultado["TpoAsunto3"];
                tesisActual.TipoTesis[3] = (short)TablaResultado["TpoAsunto4"];
                tesisActual.TipoTesis[4] = (short)TablaResultado["TpoAsunto5"];
                //tesisActual.setImageWeb("" + fila["imageWeb"]);
                //tesisActual.setDescTpoTesis("" + fila["descTpoTesis"]);
                resultado.Add(tesisActual);
            }
            TablaResultado.Close();
            return resultado;
        }

        ///<Summary>
        ///Devuelve de un determinado Id dentro de las consultas activas
        ///un listado de hasta IUSConstants.BLOQUE_PAGINADOR tesis a partir
        ///del registro de inicio
        ///</summary>
        public List<TesisTO> getTesisPaginadasParaDespliegue(List<int> IdConsulta)
        {
            MostrarPorIusTO listaIds = new MostrarPorIusTO();
            List<int> solicitados = IdConsulta;
            listaIds.Listado = solicitados;
            listaIds.OrderBy = "consecindx";
            listaIds.OrderType = "asc";
            DataTableReader TablaResultado = getTesis(listaIds);
            List<TesisTO> resultado = new List<TesisTO>();
            //foreach (DataRow fila in TablaResultado.Rows)
            while (TablaResultado.Read())
            {
                TesisTO tesisActual = new TesisTO();
                tesisActual.setIus("" + TablaResultado["ius"]);
                tesisActual.setParte("" + TablaResultado["parte"]);
                tesisActual.setRubro("" + TablaResultado["rubro"]);
                tesisActual.setEpoca("" + TablaResultado["epoca"]);
                tesisActual.setSala("" + TablaResultado["sala"]);
                tesisActual.setTesis("" + TablaResultado["tesis"]);
                tesisActual.setLocAbr("" + TablaResultado["locAbr"]);
                tesisActual.setTa_tj("" + TablaResultado["ta_tj"]);
                tesisActual.setImageOther("" + TablaResultado["imageOther"]);
                tesisActual.setConsecIndx("" + TablaResultado["consecIndx"]);
                tesisActual.OrdenInstancia = (int)TablaResultado["OrdenInstancia"];
                tesisActual.OrdenTesis = (int)TablaResultado["OrdenTesis"];
                tesisActual.OrdenRubro = (int)TablaResultado["OrdenRubro"];
                tesisActual.setTpoTesis("" + TablaResultado["tpoTesis"]);
                tesisActual.Ponentes = new int[5];
                tesisActual.Ponentes[0] = (short)TablaResultado["tpoPonente1"];
                tesisActual.Ponentes[1] = (short)TablaResultado["tpoPonente2"];
                tesisActual.Ponentes[2] = (short)TablaResultado["tpoPonente3"];
                tesisActual.Ponentes[3] = (short)TablaResultado["tpoPonente4"];
                tesisActual.Ponentes[4] = (short)TablaResultado["tpoPonente5"];
                tesisActual.TipoPonente = new int[5];
                tesisActual.TipoPonente[0] = (short)TablaResultado["tpoPon1"];
                tesisActual.TipoPonente[1] = (short)TablaResultado["tpoPon2"];
                tesisActual.TipoPonente[2] = (short)TablaResultado["tpoPon3"];
                tesisActual.TipoPonente[3] = (short)TablaResultado["tpoPon4"];
                tesisActual.TipoPonente[4] = (short)TablaResultado["tpoPon5"];
                tesisActual.TipoTesis = new int[5];
                tesisActual.TipoTesis[0] = (short)TablaResultado["TpoAsunto1"];
                tesisActual.TipoTesis[1] = (short)TablaResultado["TpoAsunto2"];
                tesisActual.TipoTesis[2] = (short)TablaResultado["TpoAsunto3"];
                tesisActual.TipoTesis[3] = (short)TablaResultado["TpoAsunto4"];
                tesisActual.TipoTesis[4] = (short)TablaResultado["TpoAsunto5"];
                //tesisActual.setImageWeb("" + fila["imageWeb"]);
                //tesisActual.setDescTpoTesis("" + fila["descTpoTesis"]);
                resultado.Add(tesisActual);
            }
            TablaResultado.Close();
            return resultado;
        }

        public void BorraPaginador(Int32 Id)
        {
            #if RED_JUR
            BackgroundWorker workerBorraPag = new BackgroundWorker();
            workerBorraPag.DoWork += new DoWorkEventHandler(workerBorraPag_DoWork);
            workerBorraPag.RunWorkerAsync(Id);
            #else
            try
            {
                ConsultasActuales[Id].ResultadoIds = null;
                ConsultasActuales.Remove(Id);
            }
            catch (Exception e)
            {
                //La llave ya no se encontraba
                System.Console.WriteLine(e.Message);
            }
            #endif
        }

        void workerBorraPag_DoWork(object sender, DoWorkEventArgs workerArgs)
        {
            Int32 Id = (Int32)workerArgs.Argument;
            DbConnection conexion = contextoBD.ContextConection;
            bool faltan = true;
            while (faltan)
            {
                String encuentraFaltantes = "select count(Consec) as contador from " + IUSConstants.PAGINADOR_BD + "Paginador where Id=" + Id;
                DataAdapter da = contextoBD.dataAdapter(encuentraFaltantes, conexion);
                DataSet ds = new DataSet();
                try
                {
                    da.Fill(ds);
                }
                catch (Exception exc)
                {
                    System.Console.WriteLine(exc.Message);
                    return;
                }
                DataTableReader dr = ds.Tables[0].CreateDataReader();
                conexion.Close();
                dr.Read();
                int existen = (int)dr["contador"];
                conexion = contextoBD.ContextConection;
                encuentraFaltantes = "select cuantos from " + IUSConstants.PAGINADOR_BD + "IdsActivos where Id=" + Id;
                da = contextoBD.dataAdapter(encuentraFaltantes, conexion);
                ds = new DataSet();
                da.Fill(ds);
                dr.Close();
                dr = ds.Tables[0].CreateDataReader();
                conexion.Close();
                dr.Read();
                int largo = 0;
                try
                {
                    largo = (int)dr["cuantos"];
                }
                catch (Exception exc)
                {
                    if (!EventLog.SourceExists("IUS"))
                    {
                        EventLog.CreateEventSource("IUS", "IUS");
                    }
                    EventLog Logg = new EventLog("IUS");
                    Logg.Source = "IUS";
                    String mensaje = "TesisDAOImpl Exception at workerBorraPag_DoWork\n" + exc.Message + exc.StackTrace;
                    Logg.WriteEntry(mensaje);
                    Logg.Close();
                }
                faltan = (existen < largo);
                dr.Close();
                System.Threading.Thread.Sleep(10000);
            }
            String sqlquery = "Delete " + IUSConstants.PAGINADOR_BD + "Paginador where Id = " + Id;
            DbCommand command = contextoBD.CommandRegisterPag(sqlquery, conexion);
            if (command.Connection.State != ConnectionState.Open)
            {
                command.Connection.Open();
            }
            command.ExecuteNonQuery();
            conexion.Close();
            conexion = contextoBD.ContextConection;
            sqlquery = "Delete " + IUSConstants.PAGINADOR_BD + "IdsActivos where Id = " + Id;
            command = contextoBD.CommandRegisterPag(sqlquery, conexion);
            if (command.Connection.State != ConnectionState.Open)
            {
                command.Connection.Open();
            }
            command.ExecuteNonQuery();
            sqlquery = IUSConstants.CADENA_VACIA;
            conexion.Close();
        }

        private void worker_doWork(object sernder, ElapsedEventArgs args)
        {
            #if RED_JUR
            DbConnection conexion = contextoBD.ContextConection;
            String sqlquery = "Delete " + IUSConstants.PAGINADOR_BD + "Paginador where Id IN (Select id from "
                + IUSConstants.PAGINADOR_BD + "IdsActivos where time < (current_timestamp-0.005))";
            DbCommand command = contextoBD.CommandRegisterPag(sqlquery, conexion);
            if (command.Connection.State != ConnectionState.Open)
            {
                command.Connection.Open();
            }
            command.ExecuteNonQuery();
            conexion.Close();
            conexion = contextoBD.ContextConection;
            sqlquery = "Delete " + IUSConstants.PAGINADOR_BD
                + "IdsActivos where time < (current_timestamp-0.005) ";
            command = contextoBD.CommandRegisterPag(sqlquery, conexion);
            if (command.Connection.State != ConnectionState.Open)
            {
                command.Connection.Open();
            }
            command.ExecuteNonQuery();
            sqlquery = IUSConstants.CADENA_VACIA;
            conexion.Close();
            #else
            List<Int32> removibles = new List<int>();
            foreach (KeyValuePair<Int32, PaginadorTO> item in ConsultasActuales)
            {
                DateTime ahora = DateTime.Now;
                TimeSpan diferencia = ahora.Subtract(item.Value.TimeStamp);
                String TimeoutStr = ConfigurationManager.AppSettings["Timeout"];
                TimeSpan tiempo = new TimeSpan(Int64.Parse(TimeoutStr) * 100000);
                if (diferencia > tiempo)
                {
                    removibles.Add(item.Key);
                    if (!EventLog.SourceExists("IUS"))
                    {
                        EventLog.CreateEventSource("IUS", "IUS");
                    }
                    EventLog Logg = new EventLog("IUS");
                    Logg.Source = "IUS";
                    String mensaje = "Paginador borrado\n" + item.Key +
                                     "\n ticksdiferencia:" + diferencia.Ticks +
                                     "\n ticks tiempo:" + tiempo.Ticks;
                    Logg.WriteEntry(mensaje);
                    Logg.Close();
                }
            }
            foreach (Int32 itemInt32 in removibles)
            {
                ConsultasActuales.Remove(itemInt32);
            }
            #endif
        }

        private void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
        }

        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
        }

        public string getIdProg(string Id)
        {
            DbConnection conexion = contextoBD.ContextConection;
            try
            {
                String resultado = IUSConstants.CADENA_VACIA;
                String sqlQuery = "select IdProg " +
                                  " From  Tesis" +
                                  " Where Ius = " + Id;
                DataAdapter query = contextoBD.dataAdapter(sqlQuery, conexion);
                DataSet datos = new DataSet();
                query.Fill(datos);
                DataTableReader tabla = datos.Tables[0].CreateDataReader();
                conexion.Close();
                //foreach (DataRow fila in tabla.Rows)
                while (tabla.Read())
                {
                    resultado = (String)tabla["IdProg"];
                }
                tabla.Close();

                return resultado;
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
                String mensaje = "TesisDAOImpl Exception at getidProg(string)\n" + e.Message + e.StackTrace;
                Logg.WriteEntry(mensaje);
                Logg.Close();
                return IUSConstants.CADENA_VACIA;
            }
        }

        public List<TipoPonenteTO> getCatalogoTipoPonente()
        {
            DbConnection conexion = contextoBD.ContextConection;
            try
            {
                String sqlQuery = "select Activo, DescTipo, IdOrder, IdTipo " +
                                  " From  ClasifTpoPonente where Activo=1 order by DescTipo";
                DataAdapter query = contextoBD.dataAdapter(sqlQuery, conexion);
                DataSet datos = new DataSet();
                query.Fill(datos);
                DataTableReader tabla = datos.Tables[0].CreateDataReader();
                conexion.Close();
                List<TipoPonenteTO> lista = new List<TipoPonenteTO>();
                //foreach (DataRow fila in tabla.Rows)
                while (tabla.Read())
                {
                    TipoPonenteTO nodoActual = new TipoPonenteTO();
                    nodoActual.Activo = (short)tabla["Activo"];
                    nodoActual.DescTipo = "" + tabla["DescTipo"];
                    nodoActual.IdOrder = (short)tabla["IdOrder"];
                    nodoActual.IdTipo = (short)tabla["IdTipo"];
                    lista.Add(nodoActual);
                }
                tabla.Close();

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
                String mensaje = "TesisDAOImpl Exception at getCatalogoTipoPonente()\n" + e.Message + e.StackTrace;
                Logg.WriteEntry(mensaje);
                Logg.Close();
                return new List<TipoPonenteTO>();
            }
        }

        #region TesisDAO Members

        public List<CategoriaDocTO> GetCategorias()
        {
            DbConnection conexion = contextoBD.ContextConection;
            try
            {
                String sqlQuery = "select CategoriaDoc, Descripcion, Imagen" +
                                  " From  CategoriaDoc";
                DataAdapter query = contextoBD.dataAdapter(sqlQuery, conexion);
                DataSet datos = new DataSet();
                query.Fill(datos);
                DataTableReader tabla = datos.Tables[0].CreateDataReader();
                conexion.Close();
                List<CategoriaDocTO> lista = new List<CategoriaDocTO>();
                //foreach (DataRow fila in tabla.Rows)
                while (tabla.Read())
                {
                    CategoriaDocTO nodoActual = new CategoriaDocTO();
                    nodoActual.CategoriaDoc = (int)tabla["CategoriaDoc"];
                    nodoActual.Descripcion = "" + tabla["Descripcion"];
                    nodoActual.imagen = (String)tabla["Imagen"];
                    lista.Add(nodoActual);
                }
                tabla.Close();

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
                String mensaje = "TesisDAOImpl Exception at getCatalogoTipoPonente()\n" + e.Message + e.StackTrace;
                Logg.WriteEntry(mensaje);
                Logg.Close();
                return new List<CategoriaDocTO>();
            }
        }

        public List<DocumentoTO> GetDocumanto(int Categoria)
        {
            DbConnection conexion = contextoBD.ContextConection;
            try
            {
                String sqlQuery = "select TipoDocumento, Descripcion, " +
                                  " Clase, Ensamblado, ClaseTablaResultado, " +
                                  " EnsambladoTR, MetodoFachada, logo, CategoriaDoc, " +
                                  " EsPaginado, Propiedad, TipoBusqueda " +
                                  " From  Documento where CategoriaDoc = " + Categoria;
                DataAdapter query = contextoBD.dataAdapter(sqlQuery, conexion);
                DataSet datos = new DataSet();
                query.Fill(datos);
                DataTableReader tabla = datos.Tables[0].CreateDataReader();
                conexion.Close();
                List<DocumentoTO> lista = new List<DocumentoTO>();
                //foreach (DataRow fila in tabla.Rows)
                while (tabla.Read())
                {
                    DocumentoTO nodoActual = new DocumentoTO();
                    nodoActual.CategoriaDoc = (int)tabla["CategoriaDoc"];
                    nodoActual.Descripcion = "" + tabla["Descripcion"];
                    nodoActual.Clase = (String)tabla["Clase"];
                    nodoActual.ClaseTablaResultado = (String)tabla["ClaseTablaResultado"];
                    nodoActual.Ensamblado = (String)tabla["Ensamblado"];
                    nodoActual.EnsambladoTR = (String)tabla["EnsambladoTR"];
                    nodoActual.EsPaginado = (bool?)tabla["EsPaginado"];
                    nodoActual.Logo = (String)tabla["Logo"];
                    nodoActual.MetodoFachada = (String)tabla["MetodoFachada"];
                    nodoActual.Propiedad = (String)tabla["Propiedad"];
                    nodoActual.TipoDocumento = (int)tabla["TipoDocumento"];
                    nodoActual.TipoBusqueda = (int)tabla["TipoBusqueda"];
                    lista.Add(nodoActual);
                }
                tabla.Close();

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
                String mensaje = "TesisDAOImpl Exception at getCatalogoTipoPonente()\n" + e.Message + e.StackTrace;
                Logg.WriteEntry(mensaje);
                Logg.Close();
                return new List<DocumentoTO>();
            }
        }

        public TesisTO getTesisPaginadaPorPosicion(int paginador, int posicion)
        {
            PaginadorTO pag = ConsultasActuales[paginador];
            pag.TimeStamp = DateTime.Now;
            return getTesisPorIus(pag.ResultadoIds[posicion]);
        }
        
        public List<TomoTO> getTomosPrimerNivel()
        {
            DbConnection conexion = contextoBD.ContextConection;
            try
            {
                String sqlQuery = "select Id, Descripcion " +
                                  " From  Tomo where Grupo = 0 order by Consec";
                DataAdapter query = contextoBD.dataAdapter(sqlQuery, conexion);
                DataSet datos = new DataSet();
                query.Fill(datos);
                DataTableReader tabla = datos.Tables[0].CreateDataReader();
                conexion.Close();
                List<TomoTO> lista = new List<TomoTO>();
                //foreach (DataRow fila in tabla.Rows)
                while (tabla.Read())
                {
                    TomoTO nodoActual = new TomoTO();
                    nodoActual.Id = (int)tabla["Id"];
                    nodoActual.Descripcion = "" + tabla["Descripcion"];
                    lista.Add(nodoActual);
                }
                tabla.Close();

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
                String mensaje = "TesisDAOImpl Exception at getTomosPrimerNivel()\n" + e.Message + e.StackTrace;
                Logg.WriteEntry(mensaje);
                Logg.Close();
                return new List<TomoTO>();
            }
        }

        #endregion
        
        private List<TesisTO> filtraPorSecciones(List<TesisTO> original, BusquedaTO parametros)
        {
            List<TesisTO> Resultado = new List<TesisTO>();
            Dictionary<int, int[]> seccionesParte = new Dictionary<int, int[]>();
            foreach (int[] secciones in parametros.Secciones)
            {
                List<int> itemList = secciones.ToList();
                itemList.Remove(secciones[0]);
                seccionesParte.Add(secciones[0], itemList.ToArray());
            }
            if (parametros.Secciones == null || parametros.Secciones.Length == 0)
            {
                return original;
            }
            
            foreach (TesisTO item in original)
            {
                if (Int32.Parse(item.Parte) > 139 && Int32.Parse(item.Parte) < 150)
                {
                    int[] secciones = seccionesParte[Int32.Parse(item.Parte)];
                    if (secciones[0] == -1 || secciones.Contains(item.Seccion))
                    {
                        Resultado.Add(item);
                    }
                }
                else
                {
                    Resultado.Add(item);
                }
            }
            return Resultado;
        }
        
        public List<SeccionTO> getTomos(int p)
        {
            DbConnection conexion = contextoBD.ContextConection;
            try
            {
                String sqlQuery = "select Id, Descripcion " +
                                  " From  Tomo where Grupo = " + p +
                                  " AND id in (select distinct(tomo) from secciones where id>10000)" +
                                  " order by Consec";
                DataAdapter query = contextoBD.dataAdapter(sqlQuery, conexion);
                DataSet datos = new DataSet();
                query.Fill(datos);
                DataTableReader tabla = datos.Tables[0].CreateDataReader();
                conexion.Close();
                List<SeccionTO> lista = new List<SeccionTO>();
                //foreach (DataRow fila in tabla.Rows)
                while (tabla.Read())
                {
                    SeccionTO nodoActual = new SeccionTO();
                    nodoActual.Id = (int)tabla["Id"];
                    nodoActual.Descripcion = "" + tabla["Descripcion"];
                    nodoActual.Padre = p;
                    nodoActual.Tomo = p;
                    lista.Add(nodoActual);
                }
                tabla.Close();
                
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
                String mensaje = "TesisDAOImpl Exception at getTomos()\n" + e.Message + e.StackTrace;
                Logg.WriteEntry(mensaje);
                Logg.Close();
                return new List<SeccionTO>();
            }
        }
        
        public List<SeccionTO> getSecciones(int p)
        {
            DbConnection conexion = contextoBD.ContextConection;
            try
            {
                String sqlQuery = "select Id, Descripcion, SeccionPadre " +
                                  " From  Secciones where Id > 10000 and Tomo = " + p + " AND (" +
                                  " id in( select distinct(seccion) from volumen) OR id in (select seccionPadre from Secciones) " +
                                  " OR SeccionPadre = 0)" +
                                  " order by Id";
                DataAdapter query = contextoBD.dataAdapter(sqlQuery, conexion);
                DataSet datos = new DataSet();
                query.Fill(datos);
                DataTableReader tabla = datos.Tables[0].CreateDataReader();
                conexion.Close();
                List<SeccionTO> lista = new List<SeccionTO>();
                //foreach (DataRow fila in tabla.Rows)
                while (tabla.Read())
                {
                    SeccionTO nodoActual = new SeccionTO();
                    nodoActual.Id = (int)tabla["Id"];
                    nodoActual.Descripcion = "" + tabla["Descripcion"];
                    nodoActual.Padre = (int)tabla["SeccionPadre"];
                    nodoActual.Tomo = p;
                    lista.Add(nodoActual);
                }
                tabla.Close();
                
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
                String mensaje = "TesisDAOImpl Exception at getSecciones()\n" + e.Message + e.StackTrace;
                Logg.WriteEntry(mensaje);
                Logg.Close();
                return new List<SeccionTO>();
            }
        }
        
        public List<int> getNotasGenericas()
        {
            DbConnection conexion = contextoBD.ContextConection;
            try
            {
                String sqlQuery = "select Ius " +
                                  " From  conNotasGenericas" ;
                DataAdapter query = contextoBD.dataAdapter(sqlQuery, conexion);
                DataSet datos = new DataSet();
                query.Fill(datos);
                DataTableReader tabla = datos.Tables[0].CreateDataReader();
                conexion.Close();
                List<int> lista = new List<int>();
                //foreach (DataRow fila in tabla.Rows)
                while (tabla.Read())
                {
                    lista.Add((int)tabla["Ius"]);
                }
                tabla.Close();
                
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
                String mensaje = "TesisDAOImpl Exception at getSecciones()\n" + e.Message + e.StackTrace;
                Logg.WriteEntry(mensaje);
                Logg.Close();
                return new List<int>();
            }
        }
    }
}