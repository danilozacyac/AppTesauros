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
    public class DirectorioOrganosJurDAOImpl : DirectorioOrgJurDAO
    {
      private DBContext contextoBD;

      public DirectorioOrganosJurDAOImpl()
       {
           IUSApplicationContext contexto = new IUSApplicationContext();
           contextoBD = (DBContext)contexto.getInitialContext().GetObject("dataSource");
       }

      public List<DirectorioOrgJurTO> getDirOrganosJur(string Filtro)
        {
            int TpoOrg = -1;
            switch (Filtro) 
            {
                case "TCC": TpoOrg = 1;       break;
                case "TUC": TpoOrg = 2;       break;
                case "JUZ": TpoOrg = 3;       break;
            }

            try
            {
                String sqlQuery = "  SELECT [__CJF_Organismos].IdentOJ, [__CJF_Organismos].TpoOrg, " +
                    " [__CJF_Organismos].Nombre, [__CJF_Organismos].Direccion  " +
                    " FROM [__CJF_Organismos] " +
                    " WHERE [__CJF_Organismos].TpoOrg =" + TpoOrg.ToString() + " " +
                    " ORDER BY [__CJF_Organismos].Cto,  " +
                    " [__CJF_Organismos].Mat,  " +
                    " [__CJF_Organismos].Ordinal,  " + 
                    " [__CJF_Organismos].Orden ;";

                DbConnection conexion = contextoBD.ContextConection;
                DataAdapter query = contextoBD.dataAdapter(sqlQuery, conexion);


                //DataAdapter query = contextoBD.dataAdapter(sqlQuery);
                DataSet datos = new DataSet();
                query.Fill(datos);
                List<DirectorioOrgJurTO> lista = new List<DirectorioOrgJurTO>();
                DataTable tabla = datos.Tables[0];
                foreach (DataRow fila in tabla.Rows)
                {
                    int tmp;
                    DirectorioOrgJurTO relacionActual = new DirectorioOrgJurTO();
                    tmp = (short)fila["IdentOJ"];
                    relacionActual.IdOrganoJur = tmp;
                    relacionActual.NombreOrganoJur = "" + fila["Nombre"];
                    //relacionActual.TelOrganoJur = "" + fila["Tels"];
                    relacionActual.DomOrganoJur = "" + fila["Direccion"];
                    //relacionActual.TelOrganoJur = "" + fila["Tels"];
                    //relacionActual.Titulares = DaTitulares(relacionActual.IdOrganoJur);
                    

                    lista.Add(relacionActual);
                }
                conexion.Close();
                return lista;
            }
            catch (Exception e){ return new List<DirectorioOrgJurTO>();   }

        }

      public List<DirectorioOrgJurTO> getDirOrganosJurXFiltro(string Filtro, int nOrd , int nMat, int nCto, int Region)
      {
          int TpoOrg = -1;
          switch (Filtro)
          {
              case "TCC": TpoOrg = 1; break;
              case "TUC": TpoOrg = 2; break;
              case "JUZ": TpoOrg = 3; break;
          }

       try
        {
            String sqlQuery = "  SELECT [__CJF_Organismos].IdentOJ, [__CJF_Organismos].TpoOrg, " + 
                            "           [__CJF_Organismos].Cto, [__CJF_Organismos].Mat,  " + 
                            "           [__CJF_Organismos].Ordinal, [__CJF_Organismos].Nombre, [__CJF_Organismos].Direccion  " + 
                            "  FROM __CJF_Organismos " +
                            "  WHERE (([__CJF_Organismos].TpoOrg= " + TpoOrg + " )";

            //Por Circuito
            if (nCto > 0) { sqlQuery = sqlQuery + " AND ([__CJF_Organismos].Cto= " + nCto + " )"; }

            //Por Materia
            if (nMat > 0) { sqlQuery = sqlQuery +  " AND ([__CJF_Organismos].Mat= " + nMat + " )"; }

            //Por Ordinal
            if (nOrd > 0) { sqlQuery = sqlQuery + " AND ([__CJF_Organismos].Ordinal=" + nOrd + " )"; }
            
           //Por Region
            if (Region > 0) { sqlQuery = sqlQuery + " AND ([__CJF_Organismos].Region=" + Region + " )"; }

            sqlQuery = sqlQuery + ") Order by  [__CJF_Organismos].Cto, " +
                                            " [__CJF_Organismos].Mat, " +
                                            " [__CJF_Organismos].Ordinal, " +
                                            "  [__CJF_Organismos].Orden;";

            DbConnection conexion = contextoBD.ContextConection;
            DataAdapter query = contextoBD.dataAdapter(sqlQuery, conexion);

            //DataAdapter query = contextoBD.dataAdapter(sqlQuery);
            DataSet datos = new DataSet();
            query.Fill(datos);
            List<DirectorioOrgJurTO> lista = new List<DirectorioOrgJurTO>();
            DataTable tabla = datos.Tables[0];
            foreach (DataRow fila in tabla.Rows)
            {
                int tmp;
                DirectorioOrgJurTO relacionActual = new DirectorioOrgJurTO();
                tmp = (short)fila["IdentOJ"];
                relacionActual.IdOrganoJur = tmp;
                relacionActual.NombreOrganoJur = "" + fila["Nombre"];
                relacionActual.DomOrganoJur = "" + fila["Direccion"];
                lista.Add(relacionActual);
            }
            conexion.Close();
            return lista;
       }
        catch (Exception e) { return new List<DirectorioOrgJurTO>(); }

      }

      public List<DirectorioOrgJurTO> getDirOrganosJurXId(int nIdOrgJud)
      {
          
          try
          {
              String sqlQuery = " SELECT [__CJF_Organismos].IdentOJ, [__CJF_Organismos].Nombre,  " +
                                  "     [__CJF_Organismos].TpoOrg, [__CJF_Organismos].Cto, " +
                                  "     [__CJF_Organismos].Mat,  [__CJF_Organismos].Ordinal," +
                                  "     [__CJF_Organismos].Direccion,  [__CJF_Organismos].Ciudad " +
                                  " FROM [__CJF_Organismos] " +
                                  " WHERE [__CJF_Organismos].IdentOJ= " + nIdOrgJud +   
                                  " Order by  [__CJF_Organismos].Cto, " +
                                  "     [__CJF_Organismos].Mat, " +
                                  "     [__CJF_Organismos].Ordinal, " +
                                  "     [__CJF_Organismos].Orden;";


              DbConnection conexion = contextoBD.ContextConection;
              DataAdapter query = contextoBD.dataAdapter(sqlQuery, conexion);
              //DataAdapter query = contextoBD.dataAdapter(sqlQuery);
              DataSet datos = new DataSet();
              query.Fill(datos);
              List<DirectorioOrgJurTO> lista = new List<DirectorioOrgJurTO>();
              DataTable tabla = datos.Tables[0];
              foreach (DataRow fila in tabla.Rows)
              {
                  int tmp;
                  DirectorioOrgJurTO relacionActual = new DirectorioOrgJurTO();
                  tmp = (short)fila["IdentOJ"];
                  relacionActual.IdOrganoJur = tmp;
                  relacionActual.NombreOrganoJur = "" + fila["Nombre"];
                  relacionActual.DomOrganoJur = "" + fila["Direccion"];
                  lista.Add(relacionActual);
              }
              conexion.Close();
              return lista;
          }
          catch (Exception e) { return new List<DirectorioOrgJurTO>(); }

      }


      public List<DirectorioOrgJurTO> getDirOfCorrespondencia()
      {

          try
          {
              //String sqlQuery = " Select * from __CJF_OCC Order by Cto,Id";
              //String sqlQuery = " Select * from __CJF_OCC Order by Cto, Orden";
              //String sqlQuery = "  SELECT [__CJF_OCC].*, [__CJF_Ciudad].Des_Ciudad, [__CJF_Estados].Des_Estado " +
              //                   " FROM __CJF_OCC " +
              //                   " INNER JOIN (__CJF_Ciudad " +
              //                   " INNER JOIN __CJF_Estados " +
              //                   " ON [__CJF_Ciudad].Cve_Estado = [__CJF_Estados].Cve_Estado) " +
              //                   " ON [__CJF_OCC].IdCiudad = [__CJF_Ciudad].Cve_Ciudad;";

              String sqlQuery = "   SELECT [__CJF_OCC].*, [__CJF_Ciudad].Des_Ciudad, [__CJF_Estados].Des_Estado, " +
                                  " [__CJF_Circuitos].DescCto, [__CJF_Circuitos].Orden " +
                                  " FROM (__CJF_OCC INNER JOIN (__CJF_Ciudad " +
                                  " INNER JOIN __CJF_Estados ON [__CJF_Ciudad].Cve_Estado = [__CJF_Estados].Cve_Estado) " +
                                  " ON [__CJF_OCC].IdCiudad = [__CJF_Ciudad].Cve_Ciudad) " +
                                  " INNER JOIN __CJF_Circuitos ON [__CJF_OCC].Cto = [__CJF_Circuitos].IdCto " + 
                                  " ORDER BY [__CJF_OCC].Orden;";




              DbConnection conexion = contextoBD.ContextConection;
              DataAdapter query = contextoBD.dataAdapter(sqlQuery, conexion);
              //DataAdapter query = contextoBD.dataAdapter(sqlQuery);
              DataSet datos = new DataSet();
              query.Fill(datos);
              List<DirectorioOrgJurTO> lista = new List<DirectorioOrgJurTO>();
              DataTable tabla = datos.Tables[0];
              foreach (DataRow fila in tabla.Rows)
              {
                  int tmp;
                  double dlTmp;
                  DirectorioOrgJurTO relacionActual = new DirectorioOrgJurTO();
                 // tmp = (short)fila["Id"];
                  dlTmp = (Int16)fila["Id"];
                  tmp = (int)dlTmp;
                  relacionActual.IdOrganoJur = tmp;
                  relacionActual.NombreOrganoJur = "" + fila["Oficina"];
                  relacionActual.DomOrganoJur = "" + fila["Direccion"];
                  relacionActual.TitularSolo =  "" + fila["Des_Ciudad"]+ ", " + fila["Des_Estado"];
                  //tmp = (Int32)fila["IdCiudad"];
                  //tmp = (Int32)fila["IdCiudad"];
                  dlTmp = (Int32)fila["IdCiudad"];
                  tmp = (int)dlTmp;
                  relacionActual.IdTipoOrgJ = tmp; // tomamos este valor para guardar el idCIudad
                  //tmp = (Byte)fila["Cto"];
                  //dlTmp = (Byte)fila["Cto"];
                  dlTmp = (Byte)fila["Cto"];
                  tmp = (int)dlTmp;
                  relacionActual.IdCto = tmp; // tomamos este valor para guardar el idCIudad
                  relacionActual.ExtOrganoJur  = "" + fila["DescCto"];
                  //relacionActual.IdCto = tmp;

                  lista.Add(relacionActual);
              }
              conexion.Close();
              return lista;
          }
          catch (Exception e) { return new List<DirectorioOrgJurTO>(); }

      }

      public List<DirectorioOrgJurTO> getDirOrganosJurXIdTitular(int nIdOrgTitular)
      {

          try
          {
              String sqlQuery = "   SELECT [__CJF_Organismos].IdentOJ, [__CJF_Organismos].Nombre, " +
                                "  [__CJF_Organismos].TpoOrg, [__CJF_Organismos].Cto, [__CJF_Organismos].Mat,  " +
                                "[__CJF_Organismos].Ordinal, [__CJF_Organismos].Direccion, " +
                                " [__CJF_Organismos].Ciudad, [__CJF_Funcionarios].IdentFunc, " +
                                " [__CJF_Rel_OJ_Func].Tels, [__CJF_Rel_OJ_Func].Funcion " +
                                " FROM (__CJF_Organismos " +
                                " INNER JOIN __CJF_Rel_OJ_Func  " +
                                " ON [__CJF_Organismos].IdentOJ = [__CJF_Rel_OJ_Func].IdentOJ) " +
                                " INNER JOIN __CJF_Funcionarios  " +
                                " ON [__CJF_Rel_OJ_Func].IdentFunc = [__CJF_Funcionarios].IdentFunc  " +
                                " WHERE ((([__CJF_Funcionarios].IdentFunc)=" + nIdOrgTitular +" ))  " +
                                "ORDER BY [__CJF_Organismos].Cto,  " +
                                "[__CJF_Organismos].Mat, " +
                                " [__CJF_Organismos].Ordinal,  " +
                                "[__CJF_Organismos].Orden;";

              DbConnection conexion = contextoBD.ContextConection;
              DataAdapter query = contextoBD.dataAdapter(sqlQuery, conexion);
              //DataAdapter query = contextoBD.dataAdapter(sqlQuery);
              DataSet datos = new DataSet();
              query.Fill(datos);
              List<DirectorioOrgJurTO> lista = new List<DirectorioOrgJurTO>();
              DataTable tabla = datos.Tables[0];
              foreach (DataRow fila in tabla.Rows)
              {
                  int tmp;
                  DirectorioOrgJurTO relacionActual = new DirectorioOrgJurTO();
                  tmp = (short)fila["IdentOJ"];
                  relacionActual.IdOrganoJur = tmp;
                  relacionActual.NombreOrganoJur = "" + fila["Nombre"];
                  relacionActual.DomOrganoJur = "" + fila["Direccion"];
                  relacionActual.TelOrganoJur = "" + fila["Tels"];
                  lista.Add(relacionActual);
              }
              conexion.Close();
              return lista;
          }
          catch (Exception e) { return new List<DirectorioOrgJurTO>(); }

      }

      private List<String> DaTitulares(int IdOrg) 
      {
         

          try {
                String sqlQuery = "   SELECT [__CJF_Organismos].IdentOJ, [__CJF_Organismos].Nombre, " + 
                    "  [__CJF_Organismos].Direccion  " + 
                    "  FROM __CJF_Organismos  " + 
                    "  WHERE [__CJF_Organismos].IdentOJ = " + IdOrg.ToString() + ";";

                DbConnection conexion = contextoBD.ContextConection;
                DataAdapter query = contextoBD.dataAdapter(sqlQuery, conexion);
                //DataAdapter query = contextoBD.dataAdapter(sqlQuery);
                DataSet datos = new DataSet();
                query.Fill(datos);
                List<String> lstTit = new List<String>();
                DataTable tabla = datos.Tables[0];
                foreach (DataRow fila in tabla.Rows)
                {
                    lstTit.Add("" + fila["Nombre"]);
                }
                conexion.Close();
                return lstTit;
          }
          catch (Exception e) {   return new List<String>();   }


          
      }


      public List<DirectorioOrgJurTO> getDirComisiones()
      {
          
          try
          {
              String sqlQuery = " SELECT [__CJF_Comisiones].IdentOJ, [__CJF_Comisiones].Nombre, " + 
                  "  [__CJF_Comisiones].NombreStr " +
                    " FROM [__CJF_Comisiones];";
              
              DbConnection conexion = contextoBD.ContextConection;
              DataAdapter query = contextoBD.dataAdapter(sqlQuery, conexion);

              //DataAdapter query = contextoBD.dataAdapter(sqlQuery);
              DataSet datos = new DataSet();
              query.Fill(datos);
              List<DirectorioOrgJurTO> lista = new List<DirectorioOrgJurTO>();
              DataTable tabla = datos.Tables[0];
              foreach (DataRow fila in tabla.Rows)
              {
                  int tmp;
                  DirectorioOrgJurTO relacionActual = new DirectorioOrgJurTO();
                  tmp = (byte)fila["IdentOJ"];
                  relacionActual.IdOrganoJur = tmp;
                  relacionActual.NombreOrganoJur = "" + fila["Nombre"];
                  lista.Add(relacionActual);
              }
              conexion.Close();
              return lista;
          }
          catch (Exception e) { return new List<DirectorioOrgJurTO>(); }

      }

      public List<DirectorioOrgJurTO> getDirAreasAdmin()
      {
         

          try
          {
              String sqlQuery = "    SELECT [__SCJN_DepSCJN].IdentOJ, [__SCJN_DepSCJN].Nombre,  [__SCJN_DepSCJN].Direccion, " +
                  " [__SCJN_DepSCJN].Tels, [__SCJN_DepSCJN].NombreStr,  [__SCJN_DepSCJN].IdTit," +
                  " [__SCJN_FuncSCJN].Nombre AS NombreTit,  [__SCJN_FuncSCJN].Apellidos, [_Titulo].DescTitulo" +
                  "  FROM (__SCJN_FuncSCJN " +
                  " INNER JOIN (__SCJN_DepSCJN " +
                  " INNER JOIN __SCJN_RelDepFuncSCJN " +
                  " ON [__SCJN_DepSCJN].IdentOJ = [__SCJN_RelDepFuncSCJN].IdentOJ) " +
                  " ON [__SCJN_FuncSCJN].IdentFunc = [__SCJN_RelDepFuncSCJN].IdentFunc) " +
                  " INNER JOIN _Titulo ON [__SCJN_FuncSCJN].IdTitulo = [_Titulo].IdTitulo " +
                  " Where ((([__SCJN_DepSCJN].IdentOJ) > 0))" +
                  " ORDER BY [__SCJN_DepSCJN].Orden;";

              DbConnection conexion = contextoBD.ContextConection;
              DataAdapter query = contextoBD.dataAdapter(sqlQuery, conexion);
              //DataAdapter query = contextoBD.dataAdapter(sqlQuery);
              DataSet datos = new DataSet();
              query.Fill(datos);
              List<DirectorioOrgJurTO> lista = new List<DirectorioOrgJurTO>();
              DataTable tabla = datos.Tables[0];
              foreach (DataRow fila in tabla.Rows)
              {
                  int tmp;
                  DirectorioOrgJurTO relacionActual = new DirectorioOrgJurTO();
                  tmp = (short)fila["IdentOJ"];
                  relacionActual.IdOrganoJur = tmp;
                  relacionActual.NombreOrganoJur = "" + fila["Nombre"];
                  relacionActual.DomOrganoJur = "" + fila["Direccion"];
                  relacionActual.TelOrganoJur = "" + fila["Tels"];
                  relacionActual.NombreStrOrganoJur = "" + fila["NombreStr"];
                  relacionActual.TitularSolo = "" + fila["DescTitulo"] + " " + fila["NombreTit"] + " " + fila["Apellidos"];

                  lista.Add(relacionActual);
              }
              conexion.Close();
              return lista;
          }
          catch (Exception e) { return new List<DirectorioOrgJurTO>(); }

      }


      public List<DirectorioOrgJurTO> getDirAreasAdminCJF( int nTipo)
      {

           
          try
          {
              String sqlQuery = " ";

              if (nTipo == 0) {
                  sqlQuery = "   SELECT [__CJF_DepCJF].IdentOJ, [__CJF_DepCJF].Nombre, " +
                " [__CJF_DepCJF].Direccion, [__CJF_DepCJF].Tels, [__CJF_DepCJF].NombreStr, " +
                " [__CJF_FuncCJF].Nombre AS Titular, [__CJF_DepCJF].orden, [_Titulo].DescTitulo, " +
                " [__CJF_FuncCJF].Apellidos , [__CJF_FuncCJF].PosFijo " +
                " FROM (__CJF_FuncCJF INNER JOIN _Titulo ON [__CJF_FuncCJF].IdTitulo = [_Titulo].IdTitulo)" +
                "  INNER JOIN (__CJF_DepCJF INNER JOIN __CJF_RelDepFuncCJF ON [__CJF_DepCJF].IdentOJ = [__CJF_RelDepFuncCJF].IdentOJ)" +
                "  ON [__CJF_FuncCJF].IdentFunc = [__CJF_RelDepFuncCJF].IdentFunc  Where ((([__CJF_DepCJF].nTipo) = 0)) " +
                " ORDER BY [__CJF_DepCJF].orden; ";
              
              }
              else{

                  sqlQuery = "    SELECT [__CJF_DepCJF].IdentOJ, [__CJF_DepCJF].Nombre, " +
                 " [__CJF_DepCJF].Direccion, [__CJF_DepCJF].Tels, [__CJF_DepCJF].NombreStr, " +
                 " [__CJF_FuncCJF].Nombre AS Titular, [__CJF_DepCJF].orden, [_Titulo].DescTitulo,  [__CJF_FuncCJF].Apellidos " +
                 " FROM (__CJF_FuncCJF INNER JOIN _Titulo ON [__CJF_FuncCJF].IdTitulo = [_Titulo].IdTitulo)" +
                 "  INNER JOIN (__CJF_DepCJF INNER JOIN __CJF_RelDepFuncCJF ON [__CJF_DepCJF].IdentOJ = [__CJF_RelDepFuncCJF].IdentOJ)" +
                 "  ON [__CJF_FuncCJF].IdentFunc = [__CJF_RelDepFuncCJF].IdentFunc  Where ((([__CJF_DepCJF].nTipo) = 1)) " +
                 " ORDER BY [__CJF_DepCJF].orden; ";
              
              }



              DbConnection conexion = contextoBD.ContextConection;
              DataAdapter query = contextoBD.dataAdapter(sqlQuery, conexion);
              //DataAdapter query = contextoBD.dataAdapter(sqlQuery);
              DataSet datos = new DataSet();
              query.Fill(datos);
              List<DirectorioOrgJurTO> lista = new List<DirectorioOrgJurTO>();
              DataTable tabla = datos.Tables[0];
              foreach (DataRow fila in tabla.Rows)
              {
                  int tmp;
                  DirectorioOrgJurTO relacionActual = new DirectorioOrgJurTO();
                  tmp = (short)fila["IdentOJ"];
                  relacionActual.IdOrganoJur = tmp;
                    relacionActual.NombreOrganoJur = "" + fila["Nombre"];
                  relacionActual.DomOrganoJur = "" + fila["Direccion"];
                  relacionActual.TelOrganoJur = "" + fila["Tels"];
                  relacionActual.NombreStrOrganoJur = "" + fila["NombreStr"];

                  if (nTipo == 0)
                  {
                      relacionActual.TitularSolo = "" + fila["DescTitulo"] + " " +
                          fila["Titular"] + " " + fila["Apellidos"] + " " + fila["Posfijo"];
                  }

                  else
                  {
                      relacionActual.TitularSolo = "" + fila["DescTitulo"] + " " + fila["Titular"] +
                                              " " + fila["Apellidos"];
                  }


                  lista.Add(relacionActual);
              }
              conexion.Close();
              return lista;
          }
          catch (Exception e) { return new List<DirectorioOrgJurTO>(); }

      }
 


      
    }
}

