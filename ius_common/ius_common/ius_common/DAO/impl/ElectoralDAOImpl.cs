using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mx.gob.scjn.ius_common.TO;
using System.Data;
using mx.gob.scjn.ius_common.utils;
using System.Data.Common;
using mx.gob.scjn.ius_common.context;
using System.Timers;
using System.ComponentModel;
using System.Diagnostics;

namespace mx.gob.scjn.ius_common.DAO.impl
{
    public class ElectoralDAOImpl:ElectoralDAO
    {
        private static Dictionary<int, PaginadorTO> ConsultasActuales { get; set; }
        private DBContext contextoBD;
        private Timer workerBorraConsultas;

        public ElectoralDAOImpl()
        {
            IUSApplicationContext contexto = new IUSApplicationContext();
            contextoBD = (DBContext)contexto.getInitialContext().GetObject("dataSource");
            ConsultasActuales = new Dictionary<int, PaginadorTO>();
            workerBorraConsultas = new Timer();
            workerBorraConsultas.Interval = 600000;
            workerBorraConsultas.Elapsed += worker_doWork;
            workerBorraConsultas.Start();
        }
        #region ElectoralDAO Members

        public List<TesisTO> getTesisPaginadas(int IdPaginador, int PosicionPaginador)
        {
            PaginadorTO listadoOriginal = ConsultasActuales[IdPaginador];
            listadoOriginal.TimeStamp = DateTime.Now;
            if ((listadoOriginal == null)
                || (PosicionPaginador >= listadoOriginal.ResultadoIds.Count))
            {
                return null;
            }
            MostrarPorIusTO listaIds = new MostrarPorIusTO();
            List<int> solicitados = new List<int>();
            int contador = 0;
            for (contador = 0;
                ((contador < IUSConstants.BLOQUE_PAGINADOR)
                && ((contador + PosicionPaginador) < listadoOriginal.ResultadoIds.Count));
                contador++)
            {
                solicitados.Add(listadoOriginal.ResultadoIds.ElementAt(contador + PosicionPaginador));
            }
            listaIds.Listado = solicitados;
            listaIds.OrderBy = "consecindx";
            listaIds.OrderType = "asc";
            DataTableReader TablaResultado = getTesis(listaIds);
            List<TesisTO> resultado = new List<TesisTO>();
            //foreach (DataRow fila in TablaResultado.Rows)
            while(TablaResultado.Read())
            {
                TesisTO tesisActual = new TesisTO();
                tesisActual.setIus("" + TablaResultado["ius"]);
                if (listadoOriginal.TipoBusqueda == IUSConstants.BUSQUEDA_ESPECIALES)
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
            return resultado;
        }        
        #endregion
        private DataTableReader getTesis(MostrarPorIusTO identificadores)
        {
            DbConnection conexion = contextoBD.ContextConectionELE;
            try
            {
                String sqlQuery = "        select A.Parte,   A.IUS,     A.Rubro,     B.descEpoca As epoca," +
                                  "               C.descInst As sala,      A.Tesis,     A.LocAbr, A.ConsecIndx, A.ta_tj,      A.tpoTesis, " +
                                  "               D.imageWeb,           D.descripcion As descTpoTesis, D.imageOther, A.ordenInstancia, A.ordenTesis, A.ordenRubro, " +
                    " A.TpoAsunto1, A.TpoAsunto2, A.TpoAsunto3, A.TpoAsunto4, A.TpoAsunto5, " +
                    " A.tpoPonente1, A.tpoPonente2, A.tpoPonente3, A.tpoPonente4, A.tpoPonente5 " +
                                  " from Tesis As A, cepocas As B, cinsts As C, ctpoTesis As D where A.epoca = B.idEpoca " +
                                  "        AND A.sala = C.idInst " +
                                  "        AND A.tpoTesis = D.id " +
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
                String mensaje = "EjecutoriasDAOImpl Exception at GetTesis(MostrarPorIusTO)\n" + e.Message + e.StackTrace;
                Logg.WriteEntry(mensaje);
                Logg.Close();
                return (new DataTable()).CreateDataReader();// List<TesisTO>();
            }
        }

        private void worker_doWork(object sernder, ElapsedEventArgs args)
        {
            List<Int32> removibles = new List<int>();
            foreach (KeyValuePair<Int32, PaginadorTO> item in ConsultasActuales)
            {
                DateTime ahora = DateTime.Now;
                TimeSpan diferencia = ahora - item.Value.TimeStamp;
                if (diferencia.Minutes > 9)
                {
                    removibles.Add(item.Key);
                }
            }
            foreach (Int32 itemInt32 in removibles)
            {
                ConsultasActuales.Remove(itemInt32);
            }
        }

        private void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
        }

        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
        }

    }
}
