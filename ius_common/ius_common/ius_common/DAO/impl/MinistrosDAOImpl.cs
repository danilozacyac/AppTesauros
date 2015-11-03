using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mx.gob.scjn.ius_common.TO;
using System.Data.Common;
using mx.gob.scjn.ius_common.context;
using System.Data;
using log4net;
using System.Diagnostics;

namespace mx.gob.scjn.ius_common.DAO.impl
{
    public class MinistrosDAOImpl:MinistrosDAO
    {
        /// <summary>
        /// El contexto y manejador de la BD
        /// </summary>
        private DBContext contextoBD;
        /// <summary>
        /// El manejador de la bitácora
        /// </summary>
        private ILog log = LogManager.GetLogger("mx.gob.scjn.iuscommon.DAO.impl.MinistrosDAOImpl");

       public MinistrosDAOImpl()
       {
           IUSApplicationContext contexto = new IUSApplicationContext();
           contextoBD = (DBContext)contexto.getInitialContext().GetObject("dataSource");
       }
       #region MinistrosDAO Members

       public String getActualizadoA()
       {
           DbConnection conexion = contextoBD.ContextConection;
           try
           {
               //string connectionString = "Server=\\dgcscthp01;database=iusServer;provider=sqloledb";
               String sqlQuery = " Select ActualizadoA " +
                                 "       from ActualizadoA";

               DataAdapter query = contextoBD.dataAdapter(sqlQuery, conexion);
               DataSet datos = new DataSet();
               query.Fill(datos);
               String resultado = String.Empty;
               List<EpocaMagistradoTO> lista = new List<EpocaMagistradoTO>();
               DataTableReader tabla = datos.Tables[0].CreateDataReader();
               conexion.Close();
               //foreach (DataRow fila in tabla.Rows)
               while(tabla.Read())
               {
                   resultado = (String) tabla["ActualizadoA"];
               }
               //conexion.Close();
               return resultado;
           }
           catch (Exception e)
           {
               conexion.Close();
               log.Debug(e.Message);
               //if (!EventLog.SourceExists("IUS"))
               //{
               //    EventLog.CreateEventSource("IUS", "IUS");
               //}
               //EventLog Logg = new EventLog("IUS");
               //Logg.Source = "IUS";
               //String mensaje = "MinistrosDAOImpl Exception at GetActualizadoA()\n" + e.Message + e.StackTrace;
               //Logg.WriteEntry(mensaje);
               //Logg.Close();

               return String.Empty;
           }
       }

       public List<EpocaMagistradoTO> getFechasMagistrados(EpocaMagistradoTO parametros)
       {
           DbConnection conexion = contextoBD.ContextConection;
           try
           {
               //string connectionString = "Server=\\dgcscthp01;database=iusServer;provider=sqloledb";
               String sqlQuery = " Select id, epoca, sala, fecha " +
                                 "       from xacep" +
                                 " where  epoca = " + parametros.Epoca +
                                 " AND sala = " + parametros.Sala;

               DataAdapter query = contextoBD.dataAdapter(sqlQuery, conexion);
               DataSet datos = new DataSet();
               query.Fill(datos);
               List<EpocaMagistradoTO> lista = new List<EpocaMagistradoTO>();
               DataTableReader tabla = datos.Tables[0].CreateDataReader();
               conexion.Close();
               //foreach (DataRow fila in tabla.Rows)
               while(tabla.Read())
               {
                   EpocaMagistradoTO relacionActual = new EpocaMagistradoTO();
                   relacionActual.Id = "" + tabla["id"];
                   relacionActual.Epoca = "" + tabla["epoca"];
                   relacionActual.Sala = "" + tabla["sala"];
                   relacionActual.Fecha = "" + tabla["fecha"];
                   lista.Add(relacionActual);
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
               String mensaje = "MinistrosDAOImpl Exception at GetAFechaMagistrados(EpocasMagistradoTO)\n" + e.Message + e.StackTrace;
               Logg.WriteEntry(mensaje);
               Logg.Close();
               return new List<EpocaMagistradoTO>();
           }
       }

        public List<FuncionariosTO> getFuncionarios(string id)
        {
            DbConnection conexion = contextoBD.ContextConection;
            try
            {
                //string connectionString = "Server=\\dgcscthp01;database=iusServer;provider=sqloledb";
                String sqlQuery = " Select id, funcionario, consec" +
                                  " from xfuncep"+
                                  " where id="+id+
                                  " order by consec";

                DataAdapter query = contextoBD.dataAdapter(sqlQuery, conexion);
                DataSet datos = new DataSet();
                query.Fill(datos);
                List<FuncionariosTO> lista = new List<FuncionariosTO>();
                DataTableReader tabla = datos.Tables[0].CreateDataReader();
                conexion.Close();
                //foreach (DataRow fila in tabla.Rows)
                while(tabla.Read())
                {
                    FuncionariosTO relacionActual = new FuncionariosTO();
                    relacionActual.Consec = "" + tabla["consec"];
                    relacionActual.Id = "" + tabla["id"];
                    relacionActual.Funcionario = "" + tabla["funcionario"];
                    lista.Add(relacionActual);
                }
                conexion.Close();
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
                String mensaje = "MinistrosDAOImpl Exception at GetAFuncionarios(Strig)\n" + e.Message + e.StackTrace;
                Logg.WriteEntry(mensaje);
                Logg.Close();
                return new List<FuncionariosTO>();
            }
        }

        #endregion
    }
}
