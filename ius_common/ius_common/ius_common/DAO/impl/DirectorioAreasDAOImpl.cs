//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace mx.gob.scjn.ius_common.DAO.impl
//{
//    class DirectorioAreasDAOImpl
//    {
//    }
//}
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
    public class DirectorioAreasDAOImpl
    {
        private DBContext contextoBD;

        public DirectorioAreasDAOImpl()
        {
            IUSApplicationContext contexto = new IUSApplicationContext();
            contextoBD = (DBContext)contexto.getInitialContext().GetObject("dataSource");
        }

        #region MinistrosDAO Members

        public List<DirectorioAreasTO> getDirOrganosJur(string Filtro)
        {
            int TpoOrg = -1;

            String sqlQuery = "";
            switch (Filtro)
            {
                case "SCJN": sqlQuery = "    SELECT [__SCJN_DepSCJN].IdentOJ, [__SCJN_DepSCJN].Nombre as NombreArea, " +
                    "  [__SCJN_DepSCJN].Direccion, [__SCJN_DepSCJN].Tels, [__SCJN_DepSCJN].NombreStr, " +
                    "  [__SCJN_DepSCJN].IdTit, [__SCJN_FuncSCJN].Nombre AS NombreTit, " +
                    "  [__SCJN_FuncSCJN].Apellidos, [_Titulo].DescTitulo " +
                    "  FROM (__SCJN_FuncSCJN " +
                    " INNER JOIN (__SCJN_DepSCJN  " +
                    " INNER JOIN __SCJN_RelDepFuncSCJN " +
                    " ON [__SCJN_DepSCJN].IdentOJ = [__SCJN_RelDepFuncSCJN].IdentOJ) " +
                    "  ON [__SCJN_FuncSCJN].IdentFunc = [__SCJN_RelDepFuncSCJN].IdentFunc) " +
                    "  INNER JOIN _Titulo ON [__SCJN_FuncSCJN].IdTitulo = [_Titulo].IdTitulo " + 
                    "  Where ((([__SCJN_DepSCJN].IdentOJ) > 0)) ORDER BY [__SCJN_DepSCJN].Orden;"; 
                            break;
                case "CJF": sqlQuery = "  SELECT [__SCJN_DepSCJN].IdentOJ, [__SCJN_DepSCJN].Nombre as NombreArea, " +
                    "  [__SCJN_DepSCJN].Direccion, [__SCJN_DepSCJN].Tels, [__SCJN_DepSCJN].NombreStr, " +
                    "  [__SCJN_DepSCJN].IdTit, [__SCJN_FuncSCJN].Nombre AS NombreTit,  [__SCJN_FuncSCJN].Apellidos," +
                    "  [_Titulo].DescTitulo " +
                    "  FROM (__SCJN_FuncSCJN " +
                    " INNER JOIN (__SCJN_DepSCJN " +
                    "  INNER JOIN __SCJN_RelDepFuncSCJN ON [__SCJN_DepSCJN].IdentOJ = [__SCJN_RelDepFuncSCJN].IdentOJ) " +
                    "  ON [__SCJN_FuncSCJN].IdentFunc = [__SCJN_RelDepFuncSCJN].IdentFunc) " +
                    "  INNER JOIN _Titulo ON [__SCJN_FuncSCJN].IdTitulo = [_Titulo].IdTitulo " +
                    "  Where ((([__SCJN_DepSCJN].IdentOJ) > 0))" +
                    "  ORDER BY [__SCJN_DepSCJN].Orden; "; 
                    break;
                case "CJFAUX": sqlQuery = "   SELECT [__CJF_DepCJF].IdentOJ, [__CJF_DepCJF].Nombre as NombreArea, " +
                    "  [__CJF_DepCJF].Direccion, [__CJF_DepCJF].Tels, [__CJF_DepCJF].NombreStr, " +
                    "  [__CJF_FuncCJF].Nombre AS Titular, [__CJF_DepCJF].orden, [_Titulo].DescTitulo,  " +
                    " [__CJF_FuncCJF].Apellidos " +
                    "  FROM (__CJF_FuncCJF " +
                    " INNER JOIN _Titulo ON [__CJF_FuncCJF].IdTitulo = [_Titulo].IdTitulo) " +
                    "  INNER JOIN (__CJF_DepCJF " +
                    " INNER JOIN __CJF_RelDepFuncCJF ON [__CJF_DepCJF].IdentOJ = [__CJF_RelDepFuncCJF].IdentOJ) " +
                    "  ON [__CJF_FuncCJF].IdentFunc = [__CJF_RelDepFuncCJF].IdentFunc " +
                    "  Where ((([__CJF_DepCJF].nTipo) = 1))  " +
                    " ORDER BY [__CJF_DepCJF].orden; "; 
                    break;
            }

            try
            {
                DbConnection conexion = contextoBD.ContextConection;
                DataAdapter query = contextoBD.dataAdapter(sqlQuery, conexion);
                DataSet datos = new DataSet();
                query.Fill(datos);
 

                //DataAdapter query = contextoBD.dataAdapter(sqlQuery);
                //DataSet datos = new DataSet();
                //query.Fill(datos);
                List<DirectorioAreasTO> lista = new List<DirectorioAreasTO>();
                DataTable tabla = datos.Tables[0];
                foreach (DataRow fila in tabla.Rows)
                {
                    DirectorioAreasTO relacionActual = new DirectorioAreasTO();
                    relacionActual.IdArea = (int)fila["IdentOJ"];
                    relacionActual.NombreArea = "" + fila["NombreArea"];
                    relacionActual.DomArea = "" + fila["Direccion"];
                    relacionActual.TelArea = "" + fila["Tels"];
                    relacionActual.Titulares = DaTitulares(relacionActual.IdArea);

                    lista.Add(relacionActual);
                }
                conexion.Close();
                return lista;
            }
            catch (Exception e) { return new List<DirectorioAreasTO>(); }

        }

        private List<String> DaTitulares(int IdOrg)
        {


            try
            {
                String sqlQuery = "   SELECT [__CJF_Organismos].IdentOJ, [__CJF_Organismos].Nombre, " +
                    "  [__CJF_Organismos].Direccion  " +
                    "  FROM __CJF_Organismos  " +
                    "  WHERE [__CJF_Organismos].IdentOJ = " + IdOrg.ToString() + ";";

                DbConnection conexion = contextoBD.ContextConection;
                DataAdapter query = contextoBD.dataAdapter(sqlQuery, conexion);
                DataSet datos = new DataSet();
                query.Fill(datos);


                //DataAdapter query = contextoBD.dataAdapter(sqlQuery);
                //DataSet datos = new DataSet();
                //query.Fill(datos);
                List<String> lstTit = new List<String>();
                DataTable tabla = datos.Tables[0];
                foreach (DataRow fila in tabla.Rows)
                {
                    lstTit.Add("" + fila["Nombre"]);
                }
                conexion.Close(); 
                return lstTit;
            }
            catch (Exception e) { return new List<String>(); }



        }

        #endregion MinistrosDAO Members

    }
}
