
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mx.gob.scjn.ius_common.TO;
using System.Data.Common;
using mx.gob.scjn.ius_common.context;
using System.Data;

namespace mx.gob.scjn.ius_common.DAO.impl
{

    public class DirectorioPersonasDAOImpl : DirectorioPersonasDAO
    {

        private DBContext contextoBD;

        public DirectorioPersonasDAOImpl()
        {
            IUSApplicationContext contexto = new IUSApplicationContext();
            contextoBD = (DBContext)contexto.getInitialContext().GetObject("dataSource");
        }

        #region MinistrosDAO Members

        public List<DirectorioPersonasTO> getDirTodasLasPersonas()
        {

            try
            {
                //string connectionString = "Server=\\dgcscthp01;database=iusServer;provider=sqloledb";
                //OdbcConnection conexion = contextoBD.contextConection;//new OdbcConnection(connectionString);
                String sqlQuery = " SELECT * " +
                                  " FROM __SCJN_IntegrantesPonencias ";

                DbConnection conexion = contextoBD.ContextConection;
                DataAdapter query = contextoBD.dataAdapter(sqlQuery, conexion);
                //DataAdapter query = contextoBD.dataAdapter(sqlQuery);
                DataSet datos = new DataSet();
                query.Fill(datos);
                List<DirectorioPersonasTO> lista = new List<DirectorioPersonasTO>();
                DataTable tabla = datos.Tables[0];

                foreach (DataRow fila in tabla.Rows)
                {
                    DirectorioPersonasTO relacionActual = new DirectorioPersonasTO();
                    relacionActual.IdPersona = (int)fila["IdentFunc"];
                    relacionActual.NombrePersona = "" + fila["idNombre"];
                    relacionActual.TelPersona = "" + fila["Tels"];
                    lista.Add(relacionActual);
                }
                conexion.Close();
                return lista;
            }

            catch (Exception e)
            {
                //log.Debug(e.StackTrace); ;
                return new List<DirectorioPersonasTO>();
            }
        }

        public List<DirectorioPersonasTO> getDirPersonas(string id)
        {

            try
            {

                String sqlQuery = " SELECT __SCJN_IntegrantesPonencias.*, " +
                    " [_Titulo].DescTitulo, [_Puestos].DescPuesto  " +
                    " FROM (__SCJN_IntegrantesPonencias  " +
                    " INNER JOIN _Puestos ON [__SCJN_IntegrantesPonencias].IdPuesto = [_Puestos].IdPuesto)  " +
                    " INNER JOIN _Titulo ON [__SCJN_IntegrantesPonencias].IdTitulo = [_Titulo].IdTitulo  " +
                    " WHERE (([__SCJN_IntegrantesPonencias].IdPonencia = " + id + " )  " +
                    " AND (([__SCJN_IntegrantesPonencias].IdPuesto)<>16  " +
                    " And ([__SCJN_IntegrantesPonencias].IdPuesto)<>23  " +
                    " And ([__SCJN_IntegrantesPonencias].IdPuesto)<>11  " +
                    " And ([__SCJN_IntegrantesPonencias].IdPuesto)<>6  " +
                    " And ([__SCJN_IntegrantesPonencias].IdPuesto)<>22))  " +
                    "  ORDER BY [__SCJN_IntegrantesPonencias].Orden; ";

                DbConnection conexion = contextoBD.ContextConection;
                DataAdapter query = contextoBD.dataAdapter(sqlQuery, conexion);
                //DataAdapter query = contextoBD.dataAdapter(sqlQuery);
                DataSet datos = new DataSet();
                query.Fill(datos);
                List<DirectorioPersonasTO> lista = new List<DirectorioPersonasTO>();
                DataTable tabla = datos.Tables[0];

                int IdTemp = 0;

                foreach (DataRow fila in tabla.Rows)
                {
                    DirectorioPersonasTO relacionActual = new DirectorioPersonasTO();

                    IdTemp = (short)fila["IdentFunc"];

                    relacionActual.IdPersona = IdTemp;//fila["IdentFunc"];
                    relacionActual.NombrePersona = "" + fila["NombreCompleto"].ToString();
                    relacionActual.ApellidosPersona = "" + fila["Apellidos"].ToString();
                    relacionActual.NombreStrPersona = "" + fila["NombreStr"].ToString();
                    relacionActual.TelPersona = "" + fila["Tels"].ToString();
                    relacionActual.ExtPersona = "" + fila["Ext"].ToString();
                    relacionActual.CargoPersona = "" + fila["DescPuesto"].ToString();
                    relacionActual.TituloPersona = "" + fila["DescTitulo"].ToString();

                    lista.Add(relacionActual);
                }
                conexion.Close();
                return lista;
            }

            catch (Exception e)
            {
                return new List<DirectorioPersonasTO>();
            }
        }

        public List<DirectorioPersonasTO> getDirPersonasXPuesto(string idPuesto)
        {

            try
            {

                String sqlQuery = " SELECT __SCJN_IntegrantesPonencias.*, " +
                    " [_Titulo].DescTitulo, [_Puestos].DescPuesto  " +
                    " FROM (__SCJN_IntegrantesPonencias  " +
                    " INNER JOIN _Puestos ON [__SCJN_IntegrantesPonencias].IdPuesto = [_Puestos].IdPuesto)  " +
                    " INNER JOIN _Titulo ON [__SCJN_IntegrantesPonencias].IdTitulo = [_Titulo].IdTitulo  " +
                    " WHERE (([__SCJN_IntegrantesPonencias].IdPuesto = " + idPuesto + " )  " +
                    " ORDER BY [__SCJN_IntegrantesPonencias].Orden; ";

                DbConnection conexion = contextoBD.ContextConection;
                DataAdapter query = contextoBD.dataAdapter(sqlQuery, conexion);
                //DataAdapter query = contextoBD.dataAdapter(sqlQuery);
                DataSet datos = new DataSet();
                query.Fill(datos);
                List<DirectorioPersonasTO> lista = new List<DirectorioPersonasTO>();
                DataTable tabla = datos.Tables[0];

                int IdTemp = 0;

                foreach (DataRow fila in tabla.Rows)
                {
                    DirectorioPersonasTO relacionActual = new DirectorioPersonasTO();

                    IdTemp = (short)fila["IdentFunc"];

                    relacionActual.IdPersona = IdTemp;//fila["IdentFunc"];
                    relacionActual.NombrePersona = "" + fila["NombreStr"].ToString();
                    relacionActual.TelPersona = "" + fila["Tels"].ToString();
                    relacionActual.ExtPersona = "" + fila["Ext"].ToString();
                    relacionActual.CargoPersona = "" + fila["DescPuesto"].ToString();
                    relacionActual.TituloPersona = "" + fila["DescTitulo"].ToString();

                    lista.Add(relacionActual);
                }
                conexion.Close();
                return lista;
            }

            catch (Exception e)
            {
                return new List<DirectorioPersonasTO>();
            }
        }

        public List<DirectorioPersonasTO> getDirPersonasXPuestoYSala(string idPuesto, string idSala)
        {

            try
            {

                String sqlQuery = " SELECT __SCJN_IntegrantesPonencias.*, " +
                    " [_Titulo].DescTitulo, [_Puestos].DescPuesto  " +
                    " FROM (__SCJN_IntegrantesPonencias  " +
                    " INNER JOIN _Puestos ON [__SCJN_IntegrantesPonencias].IdPuesto = [_Puestos].IdPuesto)  " +
                    " INNER JOIN _Titulo ON [__SCJN_IntegrantesPonencias].IdTitulo = [_Titulo].IdTitulo  " +
                    " WHERE ([__SCJN_IntegrantesPonencias].IdPuesto = " + idPuesto + " )  " +
                           " AND ([__SCJN_IntegrantesPonencias].IdSala  = " + idSala + " ) " +
             " ORDER BY [__SCJN_IntegrantesPonencias].Orden; ";

                DbConnection conexion = contextoBD.ContextConection;
                DataAdapter query = contextoBD.dataAdapter(sqlQuery, conexion);
                //DataAdapter query = contextoBD.dataAdapter(sqlQuery);
                DataSet datos = new DataSet();
                query.Fill(datos);
                List<DirectorioPersonasTO> lista = new List<DirectorioPersonasTO>();
                DataTable tabla = datos.Tables[0];

                int IdTemp = 0;

                foreach (DataRow fila in tabla.Rows)
                {
                    DirectorioPersonasTO relacionActual = new DirectorioPersonasTO();

                    IdTemp = (short)fila["IdentFunc"];

                    //relacionActual.IdPersona = IdTemp;//fila["IdentFunc"];
                    //relacionActual.NombrePersona = "" + fila["NombreStr"].ToString();
                    //relacionActual.TelPersona = "" + fila["Tels"].ToString();
                    //relacionActual.ExtPersona = "" + fila["Ext"].ToString();
                    //relacionActual.CargoPersona = "" + fila["DescPuesto"].ToString();
                    //relacionActual.TituloPersona = "" + fila["DescTitulo"].ToString();
                    relacionActual.IdPersona = IdTemp;//fila["IdentFunc"];
                    relacionActual.NombrePersona = "" + fila["NombreCompleto"].ToString();
                    relacionActual.ApellidosPersona = "" + fila["Apellidos"].ToString();
                    relacionActual.NombreStrPersona = "" + fila["NombreStr"].ToString();
                    relacionActual.TelPersona = "" + fila["Tels"].ToString();
                    relacionActual.ExtPersona = "" + fila["Ext"].ToString();
                    relacionActual.CargoPersona = "" + fila["DescPuesto"].ToString();
                    relacionActual.TituloPersona = "" + fila["DescTitulo"].ToString();

                    lista.Add(relacionActual);
                }
                conexion.Close();
                return lista;
            }

            catch (Exception e)
            {
                return new List<DirectorioPersonasTO>();
            }
        }

        public List<DirectorioPersonasTO> getDirPersonasFuncAdmin(string Filtro)
        {

            try
            {
                String sqlQuery = "";

                switch (Filtro)
                {

                    case "SCJN":
                        sqlQuery = " SELECT [__SCJN_FuncSCJN].Nombre AS NombTit, [__SCJN_FuncSCJN].Apellidos,  [__SCJN_FuncSCJN].NombreStr, " +
                            "   [__SCJN_DepSCJN].Nombre AS NombreDep, [__SCJN_DepSCJN].Direccion, " +
                            " [__SCJN_DepSCJN].Tels, [__SCJN_FuncSCJN].IdentFunc, [__SCJN_FuncSCJN].orden, " +
                            " [_Titulo].DescTitulo, [__SCJN_DepSCJN].Direccion " +
                            " FROM (__SCJN_FuncSCJN " +
                            " INNER JOIN _Titulo ON [__SCJN_FuncSCJN].IdTitulo = [_Titulo].IdTitulo) " +
                            " INNER JOIN (__SCJN_DepSCJN " +
                            " INNER JOIN __SCJN_RelDepFuncSCJN ON [__SCJN_DepSCJN].IdentOJ = [__SCJN_RelDepFuncSCJN].IdentOJ) " +
                            "  ON [__SCJN_FuncSCJN].IdentFunc = [__SCJN_RelDepFuncSCJN].IdentFunc " +
                            "   ORDER BY [__SCJN_FuncSCJN].orden";

                        break;

                    case "CJF":

                        #region

                        sqlQuery = "SELECT [__CJF_FuncCJF].IdentFunc, [__CJF_FuncCJF].Nombre AS NombTit, [__CJF_FuncCJF].Apellidos, " +
                            "   [__CJF_FuncCJF].Posfijo,  [__CJF_FuncCJF].NombreStr, [__CJF_FuncCJF].Activo, " +
                            " [__CJF_DepCJF].Nombre AS NombreDep,   [__CJF_DepCJF].IdentOJ, [_Titulo].DescTitulo," +
                            "  [_Titulo].DescTitulo As Puesto, [__CJF_DepCJF].Tels, [__CJF_DepCJF].Direccion " +
                            "  FROM (__CJF_FuncCJF " +
                            "   INNER JOIN (__CJF_DepCJF " +
                            "   INNER JOIN __CJF_RelDepFuncCJF   ON [__CJF_DepCJF].IdentOJ = [__CJF_RelDepFuncCJF].IdentOJ)" +
                            "    ON [__CJF_FuncCJF].IdentFunc = [__CJF_RelDepFuncCJF].IdentFunc) " +
                            "   INNER JOIN _Titulo ON [__CJF_FuncCJF].IdTitulo = [_Titulo].IdTitulo " +
                            "   Where ((([__CJF_FuncCJF].IdentFunc) > 0  " +
                            "  And ([__CJF_FuncCJF].IdentFunc) <> 40) " +
                            "   And (([__CJF_DepCJF].IdentOJ) > 0)) " +
                            "   ORDER BY [__CJF_FuncCJF].orden";
                        break;

                        #endregion

                }

                DbConnection conexion = contextoBD.ContextConection;
                DataAdapter query = contextoBD.dataAdapter(sqlQuery, conexion);
                //DataAdapter query = contextoBD.dataAdapter(sqlQuery);
                DataSet datos = new DataSet();
                query.Fill(datos);
                List<DirectorioPersonasTO> lista = new List<DirectorioPersonasTO>();
                DataTable tabla = datos.Tables[0];

                int IdTemp = 0;

                foreach (DataRow fila in tabla.Rows)
                {
                    DirectorioPersonasTO relacionActual = new DirectorioPersonasTO();

                    IdTemp = (short)fila["IdentFunc"];
                    String strTemp = "";
                    relacionActual.IdPersona = IdTemp;//fila["IdentFunc"];
                    relacionActual.NombrePersona = fila["DescTitulo"].ToString() + " " + fila["NombTit"].ToString() + " " + fila["Apellidos"].ToString();
                    relacionActual.TelPersona = "" + fila["Tels"].ToString();
                    relacionActual.NombreStrPersona = fila["NombreStr"].ToString();
                    relacionActual.TituloPersona = "" + fila["DescTitulo"].ToString();
                    relacionActual.AdscripcionPersona = "" + fila["NombreDep"].ToString();
                    relacionActual.DomPersona = "" + fila["Direccion"].ToString();
                    if (Filtro == "CJF")
                    {
                        strTemp = " " + fila["Posfijo"].ToString();
                        relacionActual.Posfijo = strTemp;
                    }

                    lista.Add(relacionActual);
                }
                conexion.Close();
                return lista;
            }

            catch (Exception e)
            {
                return new List<DirectorioPersonasTO>();
            }
        }

        public List<DirectorioPersonasTO> getDirPersonasFuncAdminFiltro(string Filtro, Boolean bInicio, String strCadena)
        {

            try
            {
                String sqlQuery = "";

                String sqlQFiltroSCJN = "";
                String sqlQFiltroCJF = "";

                if (strCadena.Length > 1)
                {

                    if (bInicio == true)
                    {
                        sqlQFiltroSCJN = " WHERE   [__SCJN_FuncSCJN].NombreStr like ' x " + strCadena + "%'";
                        sqlQFiltroCJF = " AND   [__CJF_FuncCJF].NombreStr like ' x " + strCadena + "%'";
                    }

                    else
                    {
                        sqlQFiltroSCJN = "  WHERE [__SCJN_FuncSCJN].NombreStr Like '%" + strCadena + "%'";
                        sqlQFiltroCJF = "  AND [__CJF_FuncCJF].NombreStr Like '%" + strCadena + "%'";
                    }
                }

                else
                {
                    sqlQFiltroSCJN = " ";
                    sqlQFiltroCJF = " ";
                }

                switch (Filtro)
                {

                    case "SCJN":
                        sqlQuery = " SELECT [__SCJN_FuncSCJN].Nombre AS NombTit, [__SCJN_FuncSCJN].Apellidos,  [__SCJN_FuncSCJN].NombreStr, " +
                            "   [__SCJN_DepSCJN].Nombre AS NombreDep, [__SCJN_DepSCJN].Direccion, " +
                            " [__SCJN_DepSCJN].Tels, [__SCJN_FuncSCJN].IdentFunc, [__SCJN_FuncSCJN].orden, " +
                            " [_Titulo].DescTitulo, [__SCJN_DepSCJN].Direccion " +
                            " FROM (__SCJN_FuncSCJN " +
                            " INNER JOIN _Titulo ON [__SCJN_FuncSCJN].IdTitulo = [_Titulo].IdTitulo) " +
                            " INNER JOIN (__SCJN_DepSCJN " +
                            " INNER JOIN __SCJN_RelDepFuncSCJN ON [__SCJN_DepSCJN].IdentOJ = [__SCJN_RelDepFuncSCJN].IdentOJ) " +
                            "  ON [__SCJN_FuncSCJN].IdentFunc = [__SCJN_RelDepFuncSCJN].IdentFunc " +
                             sqlQFiltroSCJN +
                             "   ORDER BY [__SCJN_FuncSCJN].orden";

                        break;

                    case "CJF":

                        #region

                        sqlQuery = "SELECT [__CJF_FuncCJF].IdentFunc, [__CJF_FuncCJF].Nombre AS NombTit, [__CJF_FuncCJF].Apellidos, " +
                            "   [__CJF_FuncCJF].Posfijo,  [__CJF_FuncCJF].NombreStr, [__CJF_FuncCJF].Activo, " +
                            " [__CJF_DepCJF].Nombre AS NombreDep,   [__CJF_DepCJF].IdentOJ, [_Titulo].DescTitulo," +
                            "  [_Titulo].DescTitulo As Puesto, [__CJF_DepCJF].Tels, [__CJF_DepCJF].Direccion " +
                            "  FROM (__CJF_FuncCJF " +
                            "   INNER JOIN (__CJF_DepCJF " +
                            "   INNER JOIN __CJF_RelDepFuncCJF   ON [__CJF_DepCJF].IdentOJ = [__CJF_RelDepFuncCJF].IdentOJ)" +
                            "    ON [__CJF_FuncCJF].IdentFunc = [__CJF_RelDepFuncCJF].IdentFunc) " +
                            "   INNER JOIN _Titulo ON [__CJF_FuncCJF].IdTitulo = [_Titulo].IdTitulo " +
                            "   Where ((([__CJF_FuncCJF].IdentFunc) > 0  " +
                            "  And ([__CJF_FuncCJF].IdentFunc) <> 40) " +
                            "   And (([__CJF_DepCJF].IdentOJ) > 0)) " +
                             sqlQFiltroCJF +
                             "   ORDER BY [__CJF_FuncCJF].orden";
                        break;

                        #endregion

                }

                DbConnection conexion = contextoBD.ContextConection;
                DataAdapter query = contextoBD.dataAdapter(sqlQuery, conexion);
                //DataAdapter query = contextoBD.dataAdapter(sqlQuery);
                DataSet datos = new DataSet();
                query.Fill(datos);
                List<DirectorioPersonasTO> lista = new List<DirectorioPersonasTO>();
                DataTable tabla = datos.Tables[0];

                int IdTemp = 0;

                foreach (DataRow fila in tabla.Rows)
                {
                    DirectorioPersonasTO relacionActual = new DirectorioPersonasTO();

                    IdTemp = (short)fila["IdentFunc"];

                    relacionActual.IdPersona = IdTemp;//fila["IdentFunc"];
                    relacionActual.NombrePersona = fila["DescTitulo"].ToString() + " " + fila["NombTit"].ToString() + " " + fila["Apellidos"].ToString();
                    relacionActual.TelPersona = "" + fila["Tels"].ToString();
                    relacionActual.NombreStrPersona = fila["NombreStr"].ToString();
                    relacionActual.TituloPersona = "" + fila["DescTitulo"].ToString();
                    relacionActual.AdscripcionPersona = "" + fila["NombreDep"].ToString();
                    relacionActual.DomPersona = "" + fila["Direccion"].ToString();
                    lista.Add(relacionActual);
                }
                conexion.Close();
                return lista;
            }

            catch (Exception e)
            {
                return new List<DirectorioPersonasTO>();
            }
        }

        public List<DirectorioPersonasTO> getDirPersonasConQuery(string sqlQFiltro, int nTipoPersona)
        {
            String sqlQuery = "";

            try
            {

                switch (nTipoPersona)
                {

                    case 1: //SCJN 
                        sqlQuery = " SELECT [__SCJN_FuncSCJN].Nombre AS NombTit, [__SCJN_FuncSCJN].Apellidos,  [__SCJN_FuncSCJN].NombreStr, " +
                            "   [__SCJN_DepSCJN].Nombre AS NombreDep, [__SCJN_DepSCJN].Direccion, " +
                            " [__SCJN_DepSCJN].Tels, [__SCJN_FuncSCJN].IdentFunc, [__SCJN_FuncSCJN].orden, " +
                            " [_Titulo].DescTitulo, [__SCJN_DepSCJN].Direccion " +
                            " FROM (__SCJN_FuncSCJN " +
                            " INNER JOIN _Titulo ON [__SCJN_FuncSCJN].IdTitulo = [_Titulo].IdTitulo) " +
                            " INNER JOIN (__SCJN_DepSCJN " +
                            " INNER JOIN __SCJN_RelDepFuncSCJN ON [__SCJN_DepSCJN].IdentOJ = [__SCJN_RelDepFuncSCJN].IdentOJ) " +
                            "  ON [__SCJN_FuncSCJN].IdentFunc = [__SCJN_RelDepFuncSCJN].IdentFunc " +
                             sqlQFiltro +
                             "   ORDER BY [__SCJN_FuncSCJN].orden";

                        break;

                    case 2: //CJF Admin

                        sqlQuery = "SELECT [__CJF_FuncCJF].IdentFunc, [__CJF_FuncCJF].Nombre AS NombTit, [__CJF_FuncCJF].Apellidos, " +
                            "   [__CJF_FuncCJF].Posfijo,  [__CJF_FuncCJF].NombreStr, [__CJF_FuncCJF].Activo, " +
                            " [__CJF_DepCJF].Nombre AS NombreDep,   [__CJF_DepCJF].IdentOJ, [_Titulo].DescTitulo," +
                            "  [_Titulo].DescTitulo As Puesto, [__CJF_DepCJF].Tels, [__CJF_DepCJF].Direccion " +
                            "  FROM (__CJF_FuncCJF " +
                            "   INNER JOIN (__CJF_DepCJF " +
                            "   INNER JOIN __CJF_RelDepFuncCJF   ON [__CJF_DepCJF].IdentOJ = [__CJF_RelDepFuncCJF].IdentOJ)" +
                            "    ON [__CJF_FuncCJF].IdentFunc = [__CJF_RelDepFuncCJF].IdentFunc) " +
                            "   INNER JOIN _Titulo ON [__CJF_FuncCJF].IdTitulo = [_Titulo].IdTitulo " +
                             sqlQFiltro +
                            "   AND ((([__CJF_FuncCJF].IdentFunc) > 0  " +
                            "  And ([__CJF_FuncCJF].IdentFunc) <> 40) " +
                            "   And (([__CJF_DepCJF].IdentOJ) > 0)) " +
                             "   ORDER BY [__CJF_FuncCJF].orden";
                        break;

                    case 3: //Jueces y Mag
                        sqlQuery = " SELECT [__CJF_Funcionarios].IdentFunc, [__CJF_Funcionarios].Puesto, [__CJF_Funcionarios].Nombre, " +
                                " [__CJF_Funcionarios].Apellidos, [__CJF_Funcionarios].prefijo,  [__CJF_Funcionarios].posfijo,  " +
                                " [__CJF_Rel_OJ_Func].IdentOJ, [__CJF_Funcionarios].NombreStr, [__CJF_Funcionarios].Activo " +
                                " FROM __CJF_Rel_OJ_Func " +
                                " INNER JOIN __CJF_Funcionarios ON [__CJF_Rel_OJ_Func].IdentFunc = [__CJF_Funcionarios].IdentFunc " +
                                 sqlQFiltro +
                                " ORDER BY [__CJF_Funcionarios].NombreStr ASC;";

                         break;

                }

                DbConnection conexion = contextoBD.ContextConection;
                DataAdapter query = contextoBD.dataAdapter(sqlQuery, conexion);
                //DataAdapter query = contextoBD.dataAdapter(sqlQuery);
                DataSet datos = new DataSet();
                query.Fill(datos);
                List<DirectorioPersonasTO> lista = new List<DirectorioPersonasTO>();
                DataTable tabla = datos.Tables[0];

                int IdTemp = 0;

                switch (nTipoPersona)
                {

                    case 1:

                    case 2:
                        {
                            foreach (DataRow fila in tabla.Rows)
                            {
                                DirectorioPersonasTO relacionActual = new DirectorioPersonasTO();

                                IdTemp = (short)fila["IdentFunc"];

                                relacionActual.IdPersona = IdTemp;
                                relacionActual.NombrePersona = fila["DescTitulo"].ToString() + " " + fila["NombTit"].ToString() + " " + fila["Apellidos"].ToString();
                                relacionActual.TelPersona = "" + fila["Tels"].ToString();
                                relacionActual.NombreStrPersona = fila["NombreStr"].ToString();
                                relacionActual.TituloPersona = "" + fila["DescTitulo"].ToString();
                                relacionActual.AdscripcionPersona = "" + fila["NombreDep"].ToString();
                                relacionActual.DomPersona = "" + fila["Direccion"].ToString();
                                //relacionActual.Posfijo = "" + fila["Posfijo"].ToString();
                                lista.Add(relacionActual);
                            }
                        }
                        break;

                    case 3:
                        {

                            foreach (DataRow fila in tabla.Rows)
                            {
                                DirectorioPersonasTO relacionActual = new DirectorioPersonasTO();

                                IdTemp = (short)fila["IdentFunc"];

                                relacionActual.IdPersona = IdTemp;//fila["IdentFunc"];
                                relacionActual.NombrePersona = fila["Nombre"].ToString() + " " + fila["Apellidos"].ToString();
                                relacionActual.Prefijo = fila["prefijo"].ToString();
                                relacionActual.Posfijo = fila["posfijo"].ToString();
                                relacionActual.CargoPersona = fila["Puesto"].ToString();
                                relacionActual.NombreStrPersona = fila["NombreStr"].ToString();
                                lista.Add(relacionActual);
                            }
                        }
                        break;

                }
                conexion.Close();
                return lista;
            }

            catch (Exception e)
            {
                return new List<DirectorioPersonasTO>();
            }
        }

        public List<DirectorioPersonasTO> getDirJuecesMag(string Filtro)
        {

            try
            {
                String sqlQuery = "";

                switch (Filtro)
                {

                    case "JUEZ":
                        sqlQuery = " SELECT [__CJF_Funcionarios].IdentFunc, [__CJF_Funcionarios].Puesto, [__CJF_Funcionarios].Nombre, " +
                            "    [__CJF_Funcionarios].Apellidos, [__CJF_Funcionarios].posfijo, [__CJF_Funcionarios].prefijo,  " +
                            " [__CJF_Rel_OJ_Func].IdentOJ, [__CJF_Funcionarios].NombreStr, [__CJF_Funcionarios].Activo, " +
                            "  [__CJF_Rel_OJ_Func].Funcion, [_Funciones].DescFunc, [__CJF_Organismos].Nombre,  " +
                            " [__CJF_Organismos].Direccion, [__CJF_Rel_OJ_Func].Tels " +
                            "  FROM (_Funciones  " +
                            " INNER JOIN (__CJF_Rel_OJ_Func  " +
                            " INNER JOIN __CJF_Funcionarios ON [__CJF_Rel_OJ_Func].IdentFunc = [__CJF_Funcionarios].IdentFunc) " +
                            "  ON [_Funciones].IdFunc = [__CJF_Rel_OJ_Func].Funcion)  " +
                            " INNER JOIN __CJF_Organismos ON [__CJF_Rel_OJ_Func].IdentOJ = [__CJF_Organismos].IdentOJ  " +
                            " WHERE ((([__CJF_Funcionarios].IdentFunc)<>40)) " +
                            " ORDER BY [__CJF_Funcionarios].NombreStr;";

                        break;

                    case "MAG":



                        sqlQuery = "SELECT [__CJF_FuncCJF].IdentFunc, [__CJF_FuncCJF].Nombre AS NombTit, [__CJF_FuncCJF].Apellidos, " +
                            "   [__CJF_FuncCJF].Posfijo,  [__CJF_FuncCJF].NombreStr, [__CJF_FuncCJF].Activo, " +
                            " [__CJF_DepCJF].Nombre AS NombreDep,   [__CJF_DepCJF].IdentOJ, [_Titulo].DescTitulo," +
                            "  [_Titulo].DescTitulo As Puesto, [__CJF_DepCJF].Tels, [__CJF_DepCJF].Direccion " +
                            "  FROM (__CJF_FuncCJF " +
                            "   INNER JOIN (__CJF_DepCJF " +
                            "   INNER JOIN __CJF_RelDepFuncCJF   ON [__CJF_DepCJF].IdentOJ = [__CJF_RelDepFuncCJF].IdentOJ)" +
                            "    ON [__CJF_FuncCJF].IdentFunc = [__CJF_RelDepFuncCJF].IdentFunc) " +
                            "   INNER JOIN _Titulo ON [__CJF_FuncCJF].IdTitulo = [_Titulo].IdTitulo " +
                            "   Where ((([__CJF_FuncCJF].IdentFunc) > 0  " +
                            "  And ([__CJF_FuncCJF].IdentFunc) <> 40) " +
                            "   And (([__CJF_DepCJF].IdentOJ) > 0)) " +
                            "   ORDER BY [__CJF_FuncCJF].orden";
                        break;


                }

                DbConnection conexion = contextoBD.ContextConection;
                DataAdapter query = contextoBD.dataAdapter(sqlQuery, conexion);
                //DataAdapter query = contextoBD.dataAdapter(sqlQuery);
                DataSet datos = new DataSet();
                query.Fill(datos);
                List<DirectorioPersonasTO> lista = new List<DirectorioPersonasTO>();
                DataTable tabla = datos.Tables[0];

                int IdTemp = 0;

                foreach (DataRow fila in tabla.Rows)
                {
                    DirectorioPersonasTO relacionActual = new DirectorioPersonasTO();

                    IdTemp = (short)fila["IdentFunc"];

                    relacionActual.IdPersona = IdTemp;//fila["IdentFunc"];
                    relacionActual.NombrePersona = fila["DescTitulo"].ToString() + " " + fila["NombTit"].ToString() + " " + fila["Apellidos"].ToString();
                    relacionActual.TelPersona = "" + fila["Tels"].ToString();
                    relacionActual.TituloPersona = "" + fila["DescTitulo"].ToString();
                    relacionActual.AdscripcionPersona = "" + fila["NombreDep"].ToString();
                    relacionActual.DomPersona = "" + fila["Direccion"].ToString();
                    lista.Add(relacionActual);
                }
                conexion.Close();
                return lista;
            }

            catch (Exception e)
            {
                return new List<DirectorioPersonasTO>();
            }
        }

        public List<DirectorioPersonasTO> getDirJuecesMag(string Filtro, Boolean bInicio, String strCadena)
        {

            try
            {
                String sqlQuery = "";
                String sqlQFiltro = "";

                if (strCadena.Length > 1)
                {

                    if (bInicio == true)
                    {
                        sqlQFiltro = " WHERE   NombreStr like ' x  " + strCadena + "%'";
                    }

                    else
                    {
                        sqlQFiltro = "  WHERE [__CJF_Funcionarios].NombreStr Like '%" + strCadena + "%'";
                    }
                }

                else
                {
                    sqlQFiltro = " ";
                }

                //En este query tomamos el puesto directamente de la tabla de Funcionarios
                //sqlQuery = " SELECT [__CJF_Funcionarios].IdentFunc, [__CJF_Funcionarios].Puesto, [__CJF_Funcionarios].Nombre, " +
                //        " [__CJF_Funcionarios].Apellidos, [__CJF_Funcionarios].prefijo,  [__CJF_Funcionarios].posfijo,  " +
                //        " [__CJF_Rel_OJ_Func].IdentOJ, [__CJF_Funcionarios].NombreStr, [__CJF_Funcionarios].Activo " +
                //        " FROM __CJF_Rel_OJ_Func " +
                //        " INNER JOIN __CJF_Funcionarios ON [__CJF_Rel_OJ_Func].IdentFunc = [__CJF_Funcionarios].IdentFunc " +
                //         sqlQFiltro +
                //        " ORDER BY [__CJF_Funcionarios].NombreStr ASC;";

                // Ahora hacemos un Join con el catálogo de puestos y de ahí los tomamos.
                sqlQuery = "   SELECT [__CJF_Funcionarios].IdentFunc, [__CJF_Funcionarios].Puesto, [__CJF_Funcionarios].Nombre, " +
                         " [__CJF_Funcionarios].Apellidos, [__CJF_Funcionarios].prefijo, [__CJF_Funcionarios].posfijo, " +
                         " [__CJF_Rel_OJ_Func].IdentOJ, [__CJF_Funcionarios].NombreStr, [__CJF_Funcionarios].Activo, " +
                         " [__CJF_Puestos].DescPuesto " +
                         " FROM (__CJF_Rel_OJ_Func " +
                         " INNER JOIN __CJF_Funcionarios ON [__CJF_Rel_OJ_Func].IdentFunc = [__CJF_Funcionarios].IdentFunc) " +
                         " INNER JOIN __CJF_Puestos ON [__CJF_Funcionarios].IdPuesto = [__CJF_Puestos].IdPuesto " +
                             sqlQFiltro +
                       " ORDER BY [__CJF_Funcionarios].NombreStr; ";






                DbConnection conexion = contextoBD.ContextConection;
                DataAdapter query = contextoBD.dataAdapter(sqlQuery, conexion);
                //DataAdapter query = contextoBD.dataAdapter(sqlQuery);
                DataSet datos = new DataSet();
                query.Fill(datos);
                List<DirectorioPersonasTO> lista = new List<DirectorioPersonasTO>();
                DataTable tabla = datos.Tables[0];

                int IdTemp = 0;

                foreach (DataRow fila in tabla.Rows)
                {
                    DirectorioPersonasTO relacionActual = new DirectorioPersonasTO();

                    IdTemp = (short)fila["IdentFunc"];

                    relacionActual.IdPersona = IdTemp;//fila["IdentFunc"];
                    relacionActual.NombrePersona = fila["Nombre"].ToString() + " " + fila["Apellidos"].ToString();
                    relacionActual.Prefijo = fila["prefijo"].ToString();
                    relacionActual.Posfijo = fila["posfijo"].ToString();
                    relacionActual.CargoPersona = fila["DescPuesto"].ToString();
                    relacionActual.NombreStrPersona = fila["NombreStr"].ToString();
                    lista.Add(relacionActual);
                }
                conexion.Close();
                return lista;
            }

            catch (Exception e)
            {
                return new List<DirectorioPersonasTO>();
            }
        }

        public List<DirectorioPersonasTO> getDirTitularesXOJ(string Filtro)
        {

            try
            {
                /*
                String sqlQuery = "  SELECT [__CJF_Rel_OJ_Func].IdentOJ, [__CJF_Rel_OJ_Func].IdentFunc, " +
                    " [__CJF_Rel_OJ_Func].Tels, [__CJF_Funcionarios].IdentFunc as IdFuncionario, [__CJF_Funcionarios].Puesto,  " +
                    "[__CJF_Funcionarios].Nombre as NombTit, [__CJF_Funcionarios].Apellidos,  " +
                    "[__CJF_Funcionarios].prefijo, [__CJF_Funcionarios].posfijo,   " +
                    "         [_Funciones].DescFunc, [__CJF_Rel_OJ_Func].Funcion  FROM   (__CJF_Rel_OJ_Func  " +
                    " INNER JOIN __CJF_Funcionarios  " +
                    "ON [__CJF_Rel_OJ_Func].IdentFunc = [__CJF_Funcionarios].IdentFunc)  " +
                    " INNER JOIN _Funciones ON [__CJF_Rel_OJ_Func].Funcion = [_Funciones].IdFunc   " +
                    "WHERE ([__CJF_Rel_OJ_Func].IdentOJ)=" + Filtro + "  ORDER BY [__CJF_Rel_OJ_Func].Funcion DESC;";
                */


                String sqlQuery = "  SELECT [__CJF_Rel_OJ_Func].IdentOJ, [__CJF_Rel_OJ_Func].IdentFunc, " +
                         " [__CJF_Rel_OJ_Func].Tels, [__CJF_Funcionarios].IdentFunc AS IdFuncionario, [__CJF_Funcionarios].Puesto, " +
                         " [__CJF_Funcionarios].Nombre AS NombTit, [__CJF_Funcionarios].Apellidos, " +
                         " [__CJF_Funcionarios].prefijo, [__CJF_Funcionarios].posfijo, [_Funciones].DescFunc, " +
                         " [__CJF_Rel_OJ_Func].Funcion, [__CJF_Puestos].DescPuesto " +
                         " FROM ((__CJF_Rel_OJ_Func INNER JOIN __CJF_Funcionarios " +
                         " ON [__CJF_Rel_OJ_Func].IdentFunc = [__CJF_Funcionarios].IdentFunc) " +
                         " INNER JOIN _Funciones ON [__CJF_Rel_OJ_Func].Funcion = [_Funciones].IdFunc) " +
                         " INNER JOIN __CJF_Puestos ON [__CJF_Funcionarios].IdPuesto = [__CJF_Puestos].IdPuesto " +
                         " WHERE ((([__CJF_Rel_OJ_Func].IdentOJ)=" + Filtro + "  )) " +
                         " ORDER BY [__CJF_Rel_OJ_Func].Funcion DESC; ";



                DbConnection conexion = contextoBD.ContextConection;
                DataAdapter query = contextoBD.dataAdapter(sqlQuery, conexion);
                //DataAdapter query = contextoBD.dataAdapter(sqlQuery);
                DataSet datos = new DataSet();
                List<DirectorioPersonasTO> lista = new List<DirectorioPersonasTO>();
                query.Fill(datos);
                DataTable tabla = datos.Tables[0];

                int IdTemp = 0;

                foreach (DataRow fila in tabla.Rows)
                {
                    DirectorioPersonasTO relacionActual = new DirectorioPersonasTO();

                    IdTemp = (short)fila["IdFuncionario"];

                    string temp = fila["DescFunc"].ToString();

                    if (temp.Length > 0) { temp = "(" + temp + ")"; }

                    relacionActual.IdPersona = IdTemp;//fila["IdentFunc"];
                    relacionActual.NombrePersona = fila["DescPuesto"].ToString() + " " + fila["NombTit"].ToString() + " " + fila["Apellidos"].ToString() + " " + temp;
                    relacionActual.TelPersona = "" + fila["Tels"].ToString();
                    lista.Add(relacionActual);
                }
                conexion.Close();
                return lista;
            }

            catch (Exception e) { return new List<DirectorioPersonasTO>(); }

        }

        public List<DirectorioPersonasTO> getDirConsejerosIntComisiones(int IdCom)
        {

            try
            {
                String sqlQuery = "  SELECT [__CJF_RelComCons].IdComision, [__CJF_RelComCons].idTipoRel, " +
                               "  [__CJF_Puestos].DescPuesto, [__CJF_Consejeros].*  " +
                               "  FROM __CJF_RelComCons INNER JOIN (__CJF_Consejeros INNER JOIN __CJF_Puestos  " +
                               " ON [__CJF_Consejeros].IdPuesto = [__CJF_Puestos].IdPuesto) ON [__CJF_RelComCons].IdConsejero = [__CJF_Consejeros].Id " +
                               " Where ((([__CJF_RelComCons].IdComision) = " + IdCom + " )) " +
                               " ORDER BY [__CJF_RelComCons].idTipoRel DESC;";

                DbConnection conexion = contextoBD.ContextConection;
                DataAdapter query = contextoBD.dataAdapter(sqlQuery, conexion);
                //DataAdapter query = contextoBD.dataAdapter(sqlQuery);
                DataSet datos = new DataSet();
                List<DirectorioPersonasTO> lista = new List<DirectorioPersonasTO>();
                query.Fill(datos);
                DataTable tabla = datos.Tables[0];

                int IdTemp = 0;

                foreach (DataRow fila in tabla.Rows)
                {
                    DirectorioPersonasTO relacionActual = new DirectorioPersonasTO();

                    IdTemp = (byte)fila["IdTipoRel"];

                    string temp = "";

                    if (IdTemp == 1) { temp = "(Presidente)"; }

                    else { temp = ""; }

                    relacionActual.IdPersona = (byte)fila["Id"];

                    relacionActual.IdAdsc = (byte)fila["IdComision"];

                    relacionActual.NombrePersona = fila["Nombre"].ToString() + " " + fila["Apellidos"].ToString() + " " + temp;
                    relacionActual.CargoPersona = "" + fila["DescPuesto"].ToString();
                    relacionActual.TelPersona = "" + fila["Telefono"].ToString();
                    relacionActual.ExtPersona = "" + fila["Ext"].ToString();

                    relacionActual.DomPersona = "" + fila["Domicilio"].ToString();
                    lista.Add(relacionActual);
                }
                conexion.Close();
                return lista;
            }

            catch (Exception e) { return new List<DirectorioPersonasTO>(); }

        }

        public List<DirectorioPersonasTO> getDirSTCPComision(int IdCom)
        {

            try
            {
                String sqlQuery = "    SELECT [__CJF_RelComCons].IdComision, " +
                                    "  [__CJF_RelComCons].idTipoRel, [__CJF_Puestos].DescPuesto,  " +
                                    "  [__CJF_Personal].IdPuesto, [__CJF_Personal].*, [_Titulo].DescTitulo  " +
                                    " FROM (__CJF_RelComCons INNER JOIN  (__CJF_Puestos  " +
                                    " INNER JOIN __CJF_Personal " +
                                    "  ON [__CJF_Puestos].IdPuesto = [__CJF_Personal].IdPuesto)  " +
                                    " ON [__CJF_RelComCons].IdConsejero = [__CJF_Personal].IdentFunc) " +
                                    "  INNER JOIN _Titulo ON [__CJF_Personal].IdTitulo = [_Titulo].IdTitulo  " +
                                    " WHERE ((([__CJF_RelComCons].IdComision)= " + IdCom + ")  " +
                                    "  AND (([__CJF_Personal].IdPuesto)=10)) ORDER BY [__CJF_RelComCons].idTipoRel;";

                DbConnection conexion = contextoBD.ContextConection;
                DataAdapter query = contextoBD.dataAdapter(sqlQuery, conexion);
                //DataAdapter query = contextoBD.dataAdapter(sqlQuery);
                DataSet datos = new DataSet();
                List<DirectorioPersonasTO> lista = new List<DirectorioPersonasTO>();
                query.Fill(datos);
                DataTable tabla = datos.Tables[0];

                foreach (DataRow fila in tabla.Rows)
                {
                    DirectorioPersonasTO relacionActual = new DirectorioPersonasTO();

                    relacionActual.IdPersona = (short)fila["IdentFunc"];
                    relacionActual.IdAdsc = (byte)fila["IdComision"];
                    relacionActual.NombrePersona = fila["NombreCompleto"].ToString();
                    relacionActual.TelPersona = "" + fila["Tels"].ToString();
                    relacionActual.DomPersona = "" + fila["Domicilio"].ToString();
                    relacionActual.ExtPersona = "" + fila["Ext"].ToString();
                    relacionActual.CargoPersona = "" + fila["DescPuesto"].ToString();
                    relacionActual.TituloPersona = "" + fila["DescTitulo"].ToString();

                    lista.Add(relacionActual);
                }
                conexion.Close();
                return lista;
            }

            catch (Exception e) { return new List<DirectorioPersonasTO>(); }
        }

        public List<DirectorioPersonasTO> getDirCJFIntPonencias(int idConsejero)
        {

            try
            {

                String sqlQuery = " SELECT [__CJF_Puestos].DescPuesto, [_Titulo].DescTitulo, * " +
                         " FROM (__CJF_Personal " +
                         "   INNER JOIN __CJF_Puestos " +
                         " ON [__CJF_Personal].IdPuesto = [__CJF_Puestos].IdPuesto) " +
                         " INNER JOIN _Titulo ON [__CJF_Personal].IdTitulo = [_Titulo].IdTitulo " +
                         " WHERE ((([__CJF_Personal].IdPonencia)= " + idConsejero + ")) " +
                         " ORDER BY [__CJF_Personal].Orden; ";

                DbConnection conexion = contextoBD.ContextConection;
                DataAdapter query = contextoBD.dataAdapter(sqlQuery, conexion);
                //DataAdapter query = contextoBD.dataAdapter(sqlQuery);
                DataSet datos = new DataSet();
                query.Fill(datos);
                List<DirectorioPersonasTO> lista = new List<DirectorioPersonasTO>();
                DataTable tabla = datos.Tables[0];

                int IdTemp = 0;

                foreach (DataRow fila in tabla.Rows)
                {
                    DirectorioPersonasTO relacionActual = new DirectorioPersonasTO();

                    relacionActual.IdPersona = (short)fila["IdentFunc"];
                    relacionActual.NombrePersona = "" + fila["Nombre"].ToString() + " " + fila["Apellidos"].ToString();
                    relacionActual.ApellidosPersona = "" + fila["Apellidos"].ToString();
                    relacionActual.TelPersona = "" + fila["Tels"].ToString();
                    relacionActual.ExtPersona = "" + fila["Ext"].ToString();
                    relacionActual.DomPersona = "" + fila["Domicilio"].ToString();
                    relacionActual.IdAdsc = (short)fila["IdPonencia"];
                    relacionActual.CargoPersona = "" + fila["DescPuesto"].ToString();
                    relacionActual.TituloPersona = "" + fila["DescTitulo"].ToString();

                    lista.Add(relacionActual);
                }
                conexion.Close();
                return lista;
            }

            catch (Exception e)
            {
                return new List<DirectorioPersonasTO>();
            }
        }


    }
}
        #endregion
