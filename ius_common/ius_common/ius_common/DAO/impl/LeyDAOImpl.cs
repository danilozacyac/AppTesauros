using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mx.gob.scjn.ius_common.DAO;
using mx.gob.scjn.ius_common.TO;
using System.Data;
using System.Data.Odbc;
using mx.gob.scjn.ius_common.context;
using log4net;
using System.Data.Common;
using System.Diagnostics;
namespace mx.gob.scjn.ius_common.DAO.impl
{
    class LeyDAOImpl : LeyDAO
    {
       public static ILog log = LogManager.GetLogger("mx.gob.scjn.iuscommon.DAO.impl.TesisDAOImpl");
       private  DBContext contextoBD;

       public LeyDAOImpl()
       {
           IUSApplicationContext contexto = new IUSApplicationContext();
           contextoBD = (DBContext)contexto.getInitialContext().GetObject("dataSource");
       }

        #region LeyDAO Members

        public mx.gob.scjn.ius_common.TO.LeyTO getLeyPorId(int id)
        {
            DbConnection conexion = contextoBD.ContextConection;
            try
            {
                //string connectionString = "Server=\\dgcscthp01;database=iusServer;provider=sqloledb";
                String sqlQuery = " select idLey,   descLey,   consec,   descAbr, decreto, " +
                                  "           visible, tieneRefs, nombreStr from leyes"+
                                  "    where idLey =" + id + " order by consec asc";

                DataAdapter query = contextoBD.dataAdapter(sqlQuery, conexion);
                DataSet datos = new DataSet();
                query.Fill(datos);
                List<LeyTO> lista = new List<LeyTO>();
                DataTableReader tabla = datos.Tables[0].CreateDataReader();
                conexion.Close();
                //foreach (DataRow fila in tabla.Rows)
                while(tabla.Read())
                {
                    LeyTO relacionActual = new LeyTO();
                    relacionActual.Consec = "" + tabla["consec"];
                    relacionActual.Decreto = "" + tabla["decreto"];
                    relacionActual.DescAbr = "" + tabla["descAbr"];
                    relacionActual.DescLey = "" + tabla["descLey"];
                    relacionActual.Visible = "" + tabla["visible"];
                    relacionActual.TieneRefs = "" + tabla["tieneRefs"];
                    relacionActual.IdLey = "" + tabla["idLey"];
                    relacionActual.NombrStr = "" + tabla["nombreStr"];
                    lista.Add(relacionActual);
                }
                return lista[0];
            }
            catch (Exception e)
            {
                conexion.Close();
                log.Debug(e.StackTrace);
                if (!EventLog.SourceExists("IUS"))
                {
                    EventLog.CreateEventSource("IUS", "IUS");
                }
                EventLog Logg = new EventLog("IUS");
                Logg.Source = "IUS";
                String mensaje = "LeyDAOImpl Exception at GetLeyPorId(int)\n" + e.Message + e.StackTrace;
                Logg.WriteEntry(mensaje);
                Logg.Close();
                return new LeyTO();
            }
        }

        public List<ArticulosTO> getArticulos(ArticulosTO articuloIdLey)
        {
            DbConnection conexion = contextoBD.ContextConection;
            try
            {
                //string connectionString = "Server=\\dgcscthp01;database=iusServer;provider=sqloledb";
                String sqlQuery = " select idLey, idRef, idArt, renglon, consec, tipo," +
                                  "        numArt, info, infoT from articulos"+
                                  "   where idLey ="+ articuloIdLey.IdLey+" AND idArt ="+
                                  articuloIdLey.IdArt+" AND idRef="+
                                  articuloIdLey.IdRef+" order by consec asc";
                DataAdapter query = contextoBD.dataAdapter(sqlQuery, conexion);
                DataSet datos = new DataSet();
                query.Fill(datos);
                List<ArticulosTO> lista = new List<ArticulosTO>();
                DataTableReader tabla = datos.Tables[0].CreateDataReader();
                conexion.Close();
                //foreach (DataRow fila in tabla.Rows)
                while(tabla.Read())
                {
                    ArticulosTO relacionActual = new ArticulosTO();
                    relacionActual.Consec = "" + tabla["consec"];
                    relacionActual.IdArt = "" + tabla["IdArt"];
                    relacionActual.Info = "" + tabla["info"];
                    relacionActual.Renglon = "" + tabla["renglon"];
                    relacionActual.IdRef = "" + tabla["idRef"];
                    relacionActual.Tipo = "" + tabla["tipo"];
                    relacionActual.NumArt = "" + tabla["numArt"];
                    relacionActual.InfoT = "" + tabla["infoT"];
                    sqlQuery = "SELECT IDLEY, IDREF, IDART, PARTE, INFOT FROM ARTICULOSPARTE "
                        + "WHERE IDLEY = " + articuloIdLey.IdLey + " AND IDART = " + articuloIdLey.IdArt
                        + " AND IDREF = " + articuloIdLey.IdRef + " ORDER BY PARTE";
                    DbConnection conexion2 = contextoBD.ContextConection;
                    DataAdapter queryPartes = contextoBD.dataAdapter(sqlQuery, conexion2);
                    DataSet datosPartes = new DataSet();
                    queryPartes.Fill(datosPartes);
                    DataTableReader tablaPartes = datosPartes.Tables[0].CreateDataReader();
                    conexion2.Close();
                    //foreach (DataRow filaPartes in tablaPartes.Rows)
                    while(tablaPartes.Read())
                    {
                        relacionActual.Info += tablaPartes["INFOT"];
                        relacionActual.InfoT += tablaPartes["INFOT"];
                    }
                    relacionActual.IdLey = "" + tabla["idLey"];
                    lista.Add(relacionActual);
                }
                //conexion.Close();
                return lista;
            }
            catch (Exception e)
            {
                conexion.Close();
                log.Debug(e.StackTrace);
                if (!EventLog.SourceExists("IUS"))
                {
                    EventLog.CreateEventSource("IUS", "IUS");
                }
                EventLog Logg = new EventLog("IUS");
                Logg.Source = "IUS";
                String mensaje = "LeyDAOImpl Exception at GetArticulos(AtirculoTO)\n" + e.Message + e.StackTrace;
                Logg.WriteEntry(mensaje);
                Logg.Close();

                return new List<ArticulosTO>();
            }
        }
        public List<String> getArchivos(ArticulosTO articulo)
        {
            List<String> resultado = new List<string>();
            DbConnection conexion = contextoBD.ContextConection;
            try
            {
                String sqlQuery = " select idLey, idRef, idArt, NombreArchivo" +
                                  "        from articulosArchivos" +
                                  "   where idLey =" + articulo.IdLey + " AND idArt =" +
                                  articulo.IdArt + " AND idRef=" +
                                  articulo.IdRef;
                DataAdapter query = contextoBD.dataAdapter(sqlQuery, conexion);
                DataSet datos = new DataSet();
                query.Fill(datos);
                DataTableReader tabla = datos.Tables[0].CreateDataReader();
                conexion.Close();
                //foreach (DataRow fila in tabla.Rows)
                while(tabla.Read())
                {
                    String relacionActual = "" + tabla["NombreArchivo"];
                    resultado.Add(relacionActual);
                }
            }
            catch (Exception e)
            {
                if (!EventLog.SourceExists("IUS"))
                {
                    EventLog.CreateEventSource("IUS", "IUS");
                }
                EventLog Logg = new EventLog("IUS");
                Logg.Source = "IUS";
                String mensaje = "LeyDAOImpl Exception at GetArchivos(ArticulosTO)\n" + e.Message + e.StackTrace;
                Logg.WriteEntry(mensaje);
                Logg.Close();

            }
            finally
            {
                conexion.Close();
            }
            return resultado;
        }

        public List<ArticulosTO> getArticulosPorIUS(long tesis)
        {
            List<ArticulosTO> resultado = new List<ArticulosTO>();
            DbConnection conexion = contextoBD.ContextConection;
            try
            {
                String sqlQuery = " SELECT        A.IdLey, A.IdArt, A.Info, A.idRef,  C.IdLey AS Expr1, C.DescLey " +
                                           " FROM      Articulos AS A INNER JOIN " +
                                           "           RelFraseArts AS B ON A.IdLey = B.IdLey AND " +
                                           "           A.IdArt = B.IdArt AND A.IdRef = B.IdRef INNER JOIN " +
                                           "           Leyes AS C ON B.IdLey = C.IdLey " +
                                           " WHERE        (B.IUS = " +
                                  tesis + ")";
                DataAdapter query = contextoBD.dataAdapter(sqlQuery, conexion);
                DataSet datos = new DataSet();
                query.Fill(datos);
                DataTableReader tabla = datos.Tables[0].CreateDataReader();
                conexion.Close();

                //foreach (DataRow fila in tabla.Rows)
                while (tabla.Read())
                {
                    ArticulosTO relacionActual = new ArticulosTO();
                    relacionActual.IdArt = "" + tabla["IdArt"];
                    relacionActual.IdLey = "" + tabla["idLey"];
                    relacionActual.IdRef = ""+tabla["IdRef"];
                    relacionActual.Info = "" + tabla["Info"];
                    relacionActual.InfoT = "" + tabla["DescLey"];
                    sqlQuery = "SELECT IDLEY, IDREF, IDART, PARTE, INFOT FROM ARTICULOSPARTE "
                        + "WHERE IDLEY = " + relacionActual.IdLey + " AND IDART = " + relacionActual.IdArt
                        + " AND IDREF = " + relacionActual.IdRef + " ORDER BY PARTE";
                    DbConnection conexion2 = contextoBD.ContextConection;
                    DataAdapter queryPartes = contextoBD.dataAdapter(sqlQuery, conexion2);
                    DataSet datosPartes = new DataSet();
                    queryPartes.Fill(datosPartes);
                    DataTableReader tablaPartes = datosPartes.Tables[0].CreateDataReader();
                    conexion2.Close();
                    //foreach (DataRow filaPartes in tablaPartes.Rows)
                    while (tablaPartes.Read())
                    {
                        relacionActual.Info += tablaPartes["INFOT"];
                        //relacionActual.InfoT += tablaPartes["INFOT"];
                    }
                    resultado.Add(relacionActual);
                }

                ///ESTATALES///

                conexion.Open();
                sqlQuery = " SELECT        A.IdLey, A.IdArt, A.idRef, A.Info, C.IdLey AS Expr1, C.DescLey " +
                " FROM      Articulos AS A INNER JOIN " +
                "           RelFraseArtsEstatal AS B ON A.IdLey = B.IdLey AND " +
                "           A.IdArt = B.IdArt AND A.IdRef = B.IdRef INNER JOIN " +
                "           Leyes AS C ON B.IdLey = C.IdLey " +
                " WHERE        (B.IUS = " +
       tesis + ")";
                query = contextoBD.dataAdapter(sqlQuery, conexion);
                datos = new DataSet();
                query.Fill(datos);
                tabla = datos.Tables[0].CreateDataReader();
                conexion.Close();

                //foreach (DataRow fila in tabla.Rows)
                while (tabla.Read())
                {
                    ArticulosTO relacionActual = new ArticulosTO();
                    relacionActual.IdArt = "" + tabla["IdArt"];
                    relacionActual.IdLey = "" + tabla["idLey"];
                    relacionActual.Info = "" + tabla["Info"];
                    relacionActual.IdRef = ""+ tabla["IdRef"];
                    relacionActual.InfoT = "" + tabla["DescLey"];
                    sqlQuery = "SELECT IDLEY, IDREF, IDART, PARTE, INFOT FROM ARTICULOSPARTE "
                        + "WHERE IDLEY = " + relacionActual.IdLey + " AND IDART = " + relacionActual.IdArt
                        + " AND IDREF = " + relacionActual.IdRef + " ORDER BY PARTE";
                    DbConnection conexion2 = contextoBD.ContextConection;
                    DataAdapter queryPartes = contextoBD.dataAdapter(sqlQuery, conexion2);
                    DataSet datosPartes = new DataSet();
                    queryPartes.Fill(datosPartes);
                    DataTableReader tablaPartes = datosPartes.Tables[0].CreateDataReader();
                    conexion2.Close();
                    //foreach (DataRow filaPartes in tablaPartes.Rows)
                    while (tablaPartes.Read())
                    {
                        relacionActual.Info += tablaPartes["INFOT"];
                        //relacionActual.InfoT += tablaPartes["INFOT"];
                    }
                    resultado.Add(relacionActual);
                }

            }
            catch (Exception e)
            {
                if (!EventLog.SourceExists("IUS"))
                {
                    EventLog.CreateEventSource("IUS", "IUS");
                }
                EventLog Logg = new EventLog("IUS");
                Logg.Source = "IUS";
                String mensaje = "LeyDAOImpl Exception at GetArchivosPorIUS(int)\n" + e.Message + e.StackTrace;
                Logg.WriteEntry(mensaje);
                Logg.Close();

            }
            finally
            {
                conexion.Close();
            }
            return resultado;
        }

        #endregion

        #region LeyDAO Members


        public List<ArticulosTO> getArticulosEst(ArticulosTO articuloIdLey)
        {
            DbConnection conexion = contextoBD.ContextConection;
            try
            {
                //string connectionString = "Server=\\dgcscthp01;database=iusServer;provider=sqloledb";
                String sqlQuery = " select idLey, idRef, idArt, renglon, consec, tipo," +
                                  "        numArt, info, infoT from articulos" +
                                  "   where idLey =" + articuloIdLey.IdLey + " AND idArt =" +
                                  articuloIdLey.IdArt + " AND idRef=" +
                                  articuloIdLey.IdRef + " order by consec asc";
                DataAdapter query = contextoBD.dataAdapter(sqlQuery, conexion);
                DataSet datos = new DataSet();
                query.Fill(datos);
                List<ArticulosTO> lista = new List<ArticulosTO>();
                DataTableReader tabla = datos.Tables[0].CreateDataReader();
                conexion.Close();
                //foreach (DataRow fila in tabla.Rows)
                while (tabla.Read())
                {
                    ArticulosTO relacionActual = new ArticulosTO();
                    relacionActual.Consec = "" + tabla["consec"];
                    relacionActual.IdArt = "" + tabla["IdArt"];
                    relacionActual.Info = "" + tabla["info"];
                    relacionActual.Renglon = "" + tabla["renglon"];
                    relacionActual.IdRef = "" + tabla["idRef"];
                    relacionActual.Tipo = "" + tabla["tipo"];
                    relacionActual.NumArt = "" + tabla["numArt"];
                    relacionActual.InfoT = "" + tabla["infoT"];
                    sqlQuery = "SELECT IDLEY, IDREF, IDART, PARTE, INFOT FROM ARTICULOSPARTE "
                        + "WHERE IDLEY = " + articuloIdLey.IdLey + " AND IDART = " + articuloIdLey.IdArt
                        + " AND IDREF = " + articuloIdLey.IdRef + " ORDER BY PARTE";
                    DbConnection conexion2 = contextoBD.ContextConection;
                    DataAdapter queryPartes = contextoBD.dataAdapter(sqlQuery, conexion2);
                    DataSet datosPartes = new DataSet();
                    queryPartes.Fill(datosPartes);
                    DataTableReader tablaPartes = datosPartes.Tables[0].CreateDataReader();
                    conexion2.Close();
                    //foreach (DataRow filaPartes in tablaPartes.Rows)
                    while (tablaPartes.Read())
                    {
                        relacionActual.Info += tablaPartes["INFOT"];
                        relacionActual.InfoT += tablaPartes["INFOT"];
                    }
                    relacionActual.IdLey = "" + tabla["idLey"];
                    lista.Add(relacionActual);
                }
                //conexion.Close();
                return lista;
            }
            catch (Exception e)
            {
                conexion.Close();
                log.Debug(e.StackTrace);
                if (!EventLog.SourceExists("IUS"))
                {
                    EventLog.CreateEventSource("IUS", "IUS");
                }
                EventLog Logg = new EventLog("IUS");
                Logg.Source = "IUS";
                String mensaje = "LeyDAOImpl Exception at GetArticulosEst(AtirculoTO)\n" + e.Message + e.StackTrace;
                Logg.WriteEntry(mensaje);
                Logg.Close();

                return new List<ArticulosTO>();
            }
        }

        #endregion
    }
}
