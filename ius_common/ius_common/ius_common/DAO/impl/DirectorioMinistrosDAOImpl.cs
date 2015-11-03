using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using mx.gob.scjn.ius_common.context;
using mx.gob.scjn.ius_common.TO;

namespace mx.gob.scjn.ius_common.DAO.impl
    {
    public class DirectorioMinistrosDAOImpl : DirectorioMinistrosDAO
        {
        private DBContext contextoBD;

        public DirectorioMinistrosDAOImpl()
            {
            IUSApplicationContext contexto = new IUSApplicationContext();
            contextoBD = (DBContext)contexto.getInitialContext().GetObject("dataSource");
            }

        #region MinistrosDAO Members

        public List<DirectorioMinistrosTO> getDirTodosLosMinistros()
            {
            String strTmp = "";

            try
                {
                //String sqlQuery = " SELECT * FROM __SCJN_Ministros ";
                String sqlQuery = "  SELECT [_Titulo].DescTitulo, *  " +
                                  "  FROM __SCJN_Ministros " +
                                  "  INNER JOIN _Titulo ON [__SCJN_Ministros].IdTitulo = [_Titulo].IdTitulo " +
                                  "  ORDER BY [__SCJN_Ministros].orden";

                DbConnection conexion = contextoBD.ContextConection;
                DataAdapter query = contextoBD.dataAdapter(sqlQuery, conexion);
                DataSet datos = new DataSet();

                //DataAdapter query = contextoBD.dataAdapter(sqlQuery);
                //DataSet datos = new DataSet();
                query.Fill(datos);

                List<DirectorioMinistrosTO> lista = new List<DirectorioMinistrosTO>();
                DataTable tabla = datos.Tables[0];
                foreach(DataRow fila in tabla.Rows)
                    {
                    DirectorioMinistrosTO relacionActual = new DirectorioMinistrosTO();
                    relacionActual.IdPersona = (byte)fila["Id"];
                    //relacionActual.IdTitulo = (byte)fila["IdTitulo"];
                    relacionActual.IdPuesto = (int)fila["IdPuesto"];

                    float lnIdPonencia = 0;
                    lnIdPonencia = (float)fila["IdPonencia"];
                    relacionActual.IdPonencia = (int)lnIdPonencia;

                    relacionActual.NombrePersona = (String)fila["Nombre"];
                    relacionActual.Cargo = (String)fila["DescTitulo"];
                    relacionActual.NombreCompleto = (String)fila["NombreCompleto"];
                    relacionActual.ApellidosPersona = (String)fila["Apellidos"];
                    relacionActual.Orden = (short)fila["Orden"];
                    relacionActual.OrdenSala = (int)fila["OrdenSala"];
                    relacionActual.Sala = (byte)fila["Sala"];

                    if(fila["prefijo"].Equals(System.DBNull.Value))
                        {
                        }
                    else
                        relacionActual.Prefijo = (String)fila["prefijo"];

                    if(fila["posfijo"].Equals(System.DBNull.Value))
                        {
                        }
                    else
                        relacionActual.Posfijo = (String)fila["posfijo"];

                    //strTmp = (String)fila["prefijo"];

                    //if (strTmp.Contains("xxxx")) { }
                    //else
                    //{
                    //    if (strTmp.Length < 1) strTmp = " ";
                    //    relacionActual.Prefijo = strTmp;// (String)fila["prefijo"];
                    //}
                    //strTmp = (String)fila["posfijo"];
                    //if (strTmp.Contains("xxxx")) { }
                    //else
                    //{
                    //    if (strTmp.Length < 1) strTmp = " ";

                    //    relacionActual.Posfijo = strTmp;// (String)fila["posfijo"];
                    //}
                    relacionActual.DomPersona = (String)fila["Domicilio"];
                    relacionActual.TelPersona = (String)fila["Telefono"];
                    relacionActual.ExtPersona = (String)fila["Ext"];
                    relacionActual.TituloPersona = (String)fila["DescTitulo"];
                    relacionActual.TitSemblanza = (String)fila["TituloSemblanza"];
                    relacionActual.ArchivoSemblanza = (String)fila["Archivo"];

                    lista.Add(relacionActual);
                    }
                conexion.Close();
                return lista;
                }
            catch(Exception e)
                {
                // log.Debug(e.StackTrace);
                return new List<DirectorioMinistrosTO>();
                }
            }

        public List<DirectorioMinistrosTO> getDirMinistrosXFiltro(int idFiltro)
            {
            String sqlQuery = " ";
            String strFiltro = idFiltro.ToString();
            try
                {
                switch(idFiltro)
                    {
                    case 0://Presidencia
                        sqlQuery = "  SELECT [_Titulo].DescTitulo, *  " +
                                    "  FROM __SCJN_Ministros " +
                                    "  INNER JOIN _Titulo " +
                                    " ON [__SCJN_Ministros].IdTitulo = [_Titulo].IdTitulo " +
                                    " WHERE Sala = " + strFiltro +
                                    "  ORDER BY [__SCJN_Ministros].Orden";
                        break;
                    case 1://PS
                        sqlQuery = "  SELECT [_Titulo].DescTitulo, *  " +
                                           "  FROM __SCJN_Ministros " +
                                           "  INNER JOIN _Titulo " +
                                           " ON [__SCJN_Ministros].IdTitulo = [_Titulo].IdTitulo " +
                                           " WHERE Sala = " + strFiltro +
                                           "  ORDER BY [__SCJN_Ministros].OrdenSala";
                        break;
                    case 2: //SS
                        sqlQuery = "  SELECT [_Titulo].DescTitulo, *  " +
                                    "  FROM __SCJN_Ministros " +
                                    "  INNER JOIN _Titulo " +
                                    " ON [__SCJN_Ministros].IdTitulo = [_Titulo].IdTitulo " +
                                    " WHERE Sala = " + strFiltro +
                                    "  ORDER BY [__SCJN_Ministros].OrdenSala";
                        break;
                    case 3: //PLENO
                        sqlQuery = "  SELECT [_Titulo].DescTitulo, *  " +
                                    "  FROM __SCJN_Ministros " +
                                    "  INNER JOIN _Titulo " +
                                    " ON [__SCJN_Ministros].IdTitulo = [_Titulo].IdTitulo " +
                                    " WHERE Sala = 0 Or Sala = 1 Or Sala = 2 " +
                                    "  ORDER BY [__SCJN_Ministros].Orden";
                        break;
                    }

                DbConnection conexion = contextoBD.ContextConection;
                DataAdapter query = contextoBD.dataAdapter(sqlQuery, conexion);
                DataSet datos = new DataSet();

                //DataAdapter query = contextoBD.dataAdapter(sqlQuery);
                //DataSet datos = new DataSet();
                query.Fill(datos);
                List<DirectorioMinistrosTO> lista = new List<DirectorioMinistrosTO>();
                DataTable tabla = datos.Tables[0];

                int IdTemp = 0;

                foreach(DataRow fila in tabla.Rows)
                    {
                    DirectorioMinistrosTO relacionActual = new DirectorioMinistrosTO();
                    relacionActual.IdPersona = (byte)fila["Id"];
                    relacionActual.IdPuesto = (int)fila["IdPuesto"];

                    float lnIdPonencia = 0;
                    lnIdPonencia = (float)fila["IdPonencia"];

                    //if (fila["DescTitulo"].Equals(System.DBNull.Value))
                    //{
                    //}
                    //else
                    //    relacionActual.Prefijo = (String)fila["DescTitulo"];
                    
                    relacionActual.IdPonencia = (int)lnIdPonencia;

                    relacionActual.NombrePersona = (String)fila["Nombre"];
                    relacionActual.Cargo = (String)fila["DescTitulo"];
                    relacionActual.NombreCompleto = (String)fila["NombreCompleto"];
                    relacionActual.ApellidosPersona = (String)fila["Apellidos"];
                    relacionActual.Orden = (short)fila["Orden"];
                    relacionActual.OrdenSala = (int)fila["OrdenSala"];
                    relacionActual.Sala = (byte)fila["Sala"];

                    if (fila["Prefijo"].Equals(System.DBNull.Value))
                    {
                        relacionActual.Prefijo = "xxxx";
                    }
                    else relacionActual.Prefijo = (String)fila["Prefijo"];


                    if (fila["Posfijo"].Equals(System.DBNull.Value))
                    {
                        relacionActual.Posfijo = "xxxx";
                    }
                    else relacionActual.Posfijo = (String)fila["Posfijo"];
                    
                    
                    
                    relacionActual.DomPersona = (String)fila["Domicilio"];
                    relacionActual.TelPersona = (String)fila["Telefono"];
                    relacionActual.ExtPersona = (String)fila["Ext"];
                    relacionActual.TituloPersona = (String)fila["DescTitulo"];
                    relacionActual.TitSemblanza = (String)fila["TituloSemblanza"];
                    relacionActual.ArchivoSemblanza = (String)fila["Archivo"];

                    lista.Add(relacionActual);
                    }
                conexion.Close();
                return lista;
                }
            catch(Exception e)
                {
                //log.Debug(e.StackTrace); ;
                return new List<DirectorioMinistrosTO>();
                }
            }

        public List<DirectorioMinistrosTO> getDirMinistro(string id)
            {
            try
                {
                //string connectionString = "Server=\\dgcscthp01;database=iusServer;provider=sqloledb";
                //OdbcConnection conexion = contextoBD.contextConection;//new OdbcConnection(connectionString);
                /* String sqlQuery = " SELECT * " +
                                   " FROM __SCJN_IntegrantesPonencias " +
                                    " where IdPonencia=" + id +
                                   " order by Orden";
                 */

                String sqlQuery = " SELECT __SCJN_IntegrantesPonencias.*, " +
                    " [_Titulo].DescTitulo, [_Puestos].DescPuesto  " +
                    " FROM (__SCJN_IntegrantesPonencias  " +
                    " INNER JOIN _Puestos ON [__SCJN_IntegrantesPonencias].IdPuesto = [_Puestos].IdPuesto)  " +
                    " INNER JOIN _Titulo ON [__SCJN_IntegrantesPonencias].IdTitulo = [_Titulo].IdTitulo  " +
                    " WHERE (([__SCJN_IntegrantesPonencias].IdPonencia = " + id + " )  " +
                    " AND (([__SCJN_IntegrantesPonencias].IdPuesto)<>16  " +
                    " And ([__SCJN_IntegrantesPonencias].IdPuesto)<>23  " +
                    " And ([__SCJN_IntegrantesPonencias].IdPuesto)<>22))  " +
                    "  ORDER BY [__SCJN_IntegrantesPonencias].Orden; ";

                /*

                   SELECT [__SCJN_IntegrantesPonencias].*, [_Titulo].DescTitulo, [_Puestos].DescPuesto  FROM (__SCJN_IntegrantesPonencias  INNER JOIN _Puestos ON [__SCJN_IntegrantesPonencias].IdPuesto = [_Puestos].IdPuesto)  INNER JOIN _Titulo ON [__SCJN_IntegrantesPonencias].IdTitulo = [_Titulo].IdTitulo  WHERE (([__SCJN_IntegrantesPonencias].IdPonencia = 9)  AND (([__SCJN_IntegrantesPonencias].IdPuesto)<>16 And ([__SCJN_IntegrantesPonencias].IdPuesto)<>23 And ([__SCJN_IntegrantesPonencias].IdPuesto)<>22))   ORDER BY [__SCJN_IntegrantesPonencias].Orden;

                 */
                DbConnection conexion = contextoBD.ContextConection;
                DataAdapter query = contextoBD.dataAdapter(sqlQuery, conexion);

                //DataAdapter query = contextoBD.dataAdapter(sqlQuery);
                DataSet datos = new DataSet();
                query.Fill(datos);
                List<DirectorioMinistrosTO> lista = new List<DirectorioMinistrosTO>();
                DataTable tabla = datos.Tables[0];

                int IdTemp = 0;

                foreach(DataRow fila in tabla.Rows)
                    {
                    DirectorioMinistrosTO relacionActual = new DirectorioMinistrosTO();

                    IdTemp = (short)fila["IdentFunc"];

                    relacionActual.IdPersona = IdTemp;//fila["IdentFunc"];

                    lista.Add(relacionActual);
                    }
                conexion.Close();
                return lista;
                }
            catch(Exception e)
                {
                //log.Debug(e.StackTrace); ;
                return new List<DirectorioMinistrosTO>();
                }
            }

        public List<DirectorioMinistrosTO> getDirConsejerosXFiltro(string idFiltro)
            {
            String sqlQuery = " ";
            //int iFiltro = (int)idFiltro;

            try
                {
                if(idFiltro == "0")
                    {
                    sqlQuery = "  SELECT [_Titulo].DescTitulo, *  " +
                              "  FROM __CJF_Consejeros " +
                              "  INNER JOIN _Titulo ON [__CJF_Consejeros].IdTitulo = [_Titulo].IdTitulo " +
                              "  ORDER BY [__CJF_Consejeros].Orden";
                    }
                else
                    {
                    sqlQuery = " SELECT [__CJF_RelComCons].IdComision, [__CJF_RelComCons].idTipoRel, " +
                                "    [__CJF_Puestos].DescPuesto, [__CJF_Consejeros].* " +
                                "    FROM __CJF_RelComCons " +
                                " INNER JOIN (__CJF_Consejeros " +
                                " INNER JOIN __CJF_Puestos ON [__CJF_Consejeros].IdPuesto = [__CJF_Puestos].IdPuesto) " +
                                "  ON [__CJF_RelComCons].IdConsejero = [__CJF_Consejeros].Id " +
                                "  WHERE ((([__CJF_RelComCons].IdComision) =  " + idFiltro + " ) " +
                                "  ORDER BY [__CJF_RelComCons].idTipoRel";
                    }
                DbConnection conexion = contextoBD.ContextConection;
                DataAdapter query = contextoBD.dataAdapter(sqlQuery, conexion);

                //DataAdapter query = contextoBD.dataAdapter(sqlQuery);
                DataSet datos = new DataSet();
                query.Fill(datos);
                List<DirectorioMinistrosTO> lista = new List<DirectorioMinistrosTO>();
                DataTable tabla = datos.Tables[0];

                int IdTemp = 0;

                foreach(DataRow fila in tabla.Rows)
                    {
                    String strDomTmp = "";
                    String strTelTmp = "";
                    String strExtTmp = "";
                    String strTmp = "";

                    DirectorioMinistrosTO relacionActual = new DirectorioMinistrosTO();
                    relacionActual.IdPersona = (byte)fila["Id"];
                    relacionActual.IdPuesto = (int)fila["IdPuesto"];

                    float lnIdPonencia = 0;
                    lnIdPonencia = -1; //(float)fila["IdPonencia"];
                    relacionActual.IdPonencia = (int)lnIdPonencia;

                    relacionActual.NombrePersona = (String)fila["Nombre"];

                    if (fila["DescTitulo"].Equals(System.DBNull.Value))
                    {
                    }
                    else
                        relacionActual.Prefijo = (String)fila["DescTitulo"];
                    
                    
                    relacionActual.NombreCompleto = (String)fila["NombreCompleto"];
                    relacionActual.ApellidosPersona = (String)fila["Apellidos"];
                    relacionActual.Orden = (short)fila["Orden"];
                    relacionActual.OrdenSala = (int)fila["OrdenSala"];
                    relacionActual.Sala = (byte)fila["Sala"];

                    if(fila["prefijo"].Equals(System.DBNull.Value))
                        {
                        }
                    else
                        relacionActual.Prefijo = (String)fila["prefijo"];

                    if(fila["posfijo"].Equals(System.DBNull.Value))
                        {
                        }
                    else
                        relacionActual.Posfijo = (String)fila["posfijo"];

                    strDomTmp = (String)fila["Domicilio"];
                    relacionActual.DomPersona = strDomTmp;
                    strTelTmp = (String)fila["Telefono"];
                    relacionActual.TelPersona = strTelTmp;
                    strExtTmp = (String)fila["Ext"];
                    relacionActual.ExtPersona = strExtTmp;
                    relacionActual.TituloPersona = (String)fila["DescTitulo"];
                    relacionActual.Cargo = relacionActual.TituloPersona;
                    relacionActual.TitSemblanza = (String)fila["TituloSemblanza"];
                    relacionActual.ArchivoSemblanza = (String)fila["Archivo"];

                    lista.Add(relacionActual);
                    }
                conexion.Close();
                return lista;
                }
            catch(Exception e)
                {
                //log.Debug(e.StackTrace); ;
                return new List<DirectorioMinistrosTO>();
                }
            }

        public List<DirectorioMinistrosTO> getDirMagistradosXFiltro(string idFiltro)
            {
            String sqlQuery = " ";

            try
                {
                if(idFiltro == "0")
                    {
                    sqlQuery = "  SELECT [_TFE_Puestos].DescPuesto,  * " +
                                "   FROM __TFE_MagistradosTFE " +
                                " INNER JOIN _TFE_Puestos  " +
                                " ON [__TFE_MagistradosTFE].IdPuesto = [_TFE_Puestos].IdPuesto " +
                                " ORDER BY [__TFE_MagistradosTFE].orden; ";
                    }
                else
                    {//Aqui sólo aplica traer todos
                    sqlQuery = " Select * from [__TFE_MagistradosTFE] Order by Orden ";
                    }
                //contextConection
                DbConnection conexion = contextoBD.ContextConection;

                //DbConnection conexion = contextoBD.ContextConection;
                DataAdapter query = contextoBD.dataAdapter(sqlQuery, conexion);

                //DataAdapter query = contextoBD.dataAdapter(sqlQuery);
                DataSet datos = new DataSet();
                query.Fill(datos);
                List<DirectorioMinistrosTO> lista = new List<DirectorioMinistrosTO>();
                DataTable tabla = datos.Tables[0];

                int IdTemp = 0;

                foreach(DataRow fila in tabla.Rows)
                    {
                    DirectorioMinistrosTO relacionActual = new DirectorioMinistrosTO();
                    relacionActual.IdPersona = (byte)fila["Id"];
                    //relacionActual.IdPuesto = (int)fila["IdPuestoMag"];

                    float lnIdPonencia = 0;
                    lnIdPonencia = -1; //(float)fila["IdPonencia"];
                    relacionActual.IdPonencia = (int)lnIdPonencia;

                    relacionActual.NombrePersona = (String)fila["Nombre"];
                    relacionActual.Cargo = (String)fila["DescPuesto"];
                    relacionActual.NombreCompleto = (String)fila["NombreCompleto"];
                    relacionActual.ApellidosPersona = (String)fila["Apellidos"];
                    //relacionActual.Orden = (short)fila["Orden"];
                    //relacionActual.OrdenSala = (int)fila["OrdenSala"];
                    relacionActual.Sala = (byte)fila["Sala"];
                    relacionActual.Prefijo = (String)fila["Prefijo"];
                    relacionActual.Posfijo = (String)fila["Posfijo"];
                    relacionActual.DomPersona = (String)fila["Domicilio"];
                    relacionActual.TelPersona = (String)fila["Telefono"];
                    relacionActual.ExtPersona = (String)fila["Ext"];
                    relacionActual.TituloPersona = (String)fila["DescPuesto"];
                    relacionActual.TitSemblanza = (String)fila["TituloSemblanza"];
                    relacionActual.ArchivoSemblanza = (String)fila["Archivo"];

                    lista.Add(relacionActual);
                    }
                conexion.Close();
                return lista;
                }
            catch(Exception e)
                {
                //log.Debug(e.StackTrace); ;
                return new List<DirectorioMinistrosTO>();
                }
            }

        #endregion MinistrosDAO Members
        }
    }