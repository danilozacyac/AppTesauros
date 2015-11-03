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
    public class DirectorioCatalogosDAOImpl : DirectorioCatalogosDAO
    {
        private DBContext contextoBD;

        public DirectorioCatalogosDAOImpl()
        {
            IUSApplicationContext contexto = new IUSApplicationContext();
            contextoBD = (DBContext)contexto.getInitialContext().GetObject("dataSource");
        }

        #region Catalogos Members

        public List<DirectorioCatalogosTO> getDirCatalogo(int nTipoCatalogo)
        {
            String sqlQuery = " ";

            switch (nTipoCatalogo)
            {

                case (1): sqlQuery = "SELECT IdCto as Id, DescCto as NombreElemento, Orden FROM __CJF_Circuitos WHERE IdCto <> 50 ORDER BY IdCto ASC";
                    break;

                case (2): sqlQuery = "SELECT IdMat as Id, DescMat as NombreElemento, nPos as Orden  FROM __CJF_Materias WHERE IdMat <> 50  ORDER BY nPos ASC";
                    break;

                case (3): sqlQuery = "SELECT IdOrd as Id, DescOrd as NombreElemento, IdOrd as Orden  FROM __CJF_Ordinal WHERE IdOrd > 0 AND IdOrd <> 50 ORDER BY IdOrd ASC";
                    break;

                case (4): sqlQuery = "SELECT IdCA as Id, DescCA as NombreElemento, Orden FROM __CJF_CentrosAuxiliares WHERE IdCA > 0  ORDER BY Orden ASC";
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
                List<DirectorioCatalogosTO> lista = new List<DirectorioCatalogosTO>();
                DataTable tabla = datos.Tables[0];

                foreach (DataRow fila in tabla.Rows)
                {
                    DirectorioCatalogosTO relacionActual = new DirectorioCatalogosTO();
                    relacionActual.IdElemento = (byte)fila["Id"];
                    relacionActual.NombreElemento = "" + fila["NombreElemento"];

                    if ((nTipoCatalogo == 1) || (nTipoCatalogo == 4)) { relacionActual.Orden = (int)fila["Orden"]; }
                    else { relacionActual.Orden = (byte)fila["Orden"]; }
                    lista.Add(relacionActual);
                }
                conexion.Close();
                return lista;
            }

            catch (Exception e) { return new List<DirectorioCatalogosTO>(); }
        }

        public List<DirectorioCatalogosTO> getDirCatalogoXTipo(int nTipoCatalogo, int TpoOJ)
        {

            String sqlQuery = " ";
            String strTablaOJ = "";
            String strTablaOJ_REG = "";

            switch (TpoOJ)
            {
                case (1): strTablaOJ = "_TUC";
                    strTablaOJ_REG = "__CJF_CA_REG_TUC";
                    break;
                case (2): strTablaOJ = "_juz";
                    strTablaOJ_REG = "__CJF_CA_REG_JUZ";
                    break;
                case (3): strTablaOJ = "_TCC";
                    strTablaOJ_REG = "__CJF_CA_REG_TCC";
                    break;
                case (4): strTablaOJ = "";
                    break;
                default: strTablaOJ = "";
                    break;
            }

            switch (nTipoCatalogo)
            {
                case (1): sqlQuery = "SELECT IdCto as Id, DescCto as NombreElemento, Orden " +
                    " FROM __CJF_Circuitos" + strTablaOJ + " WHERE  IdCto > 0 AND IdCto <> 50 ORDER BY IdCto ASC";
                    break;
                //case (2): sqlQuery = "SELECT IdMat as Id, DescMat as NombreElemento, nPos as Orden  " +
                //    "  FROM __CJF_Materias" + strTablaOJ + " WHERE IdMat > 0 AND IdMat <> 50  ORDER BY nPos ASC";
                case (2): sqlQuery = "SELECT IdMat as Id, DescMat as NombreElemento, nPos as Orden  " +
                    "  FROM __CJF_Materias" + strTablaOJ + " WHERE  IdMat <> 50  ORDER BY nPos ASC";
                    break;
                case (3): sqlQuery = "SELECT IdOrd as Id, DescOrd as NombreElemento, IdOrd as Orden  " +
                    "  FROM __CJF_Ordinal" + strTablaOJ + " WHERE IdOrd > 0 AND IdOrd <> 50 ORDER BY IdOrd ASC";
                    break;
                case (4): sqlQuery = "SELECT IdCA as Id, DescCA as NombreElemento, Orden  " +
                    "  FROM " + strTablaOJ_REG + " WHERE IdCA > 0  ORDER BY Orden ASC";
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
                List<DirectorioCatalogosTO> lista = new List<DirectorioCatalogosTO>();
                DataTable tabla = datos.Tables[0];

                foreach (DataRow fila in tabla.Rows)
                {
                    DirectorioCatalogosTO relacionActual = new DirectorioCatalogosTO();

                    relacionActual.IdElemento = (byte)fila["Id"];
                    relacionActual.NombreElemento = "" + fila["NombreElemento"];
                    if ((nTipoCatalogo == 1) || (nTipoCatalogo == 4)) { relacionActual.Orden = (int)fila["Orden"]; }
                    else { relacionActual.Orden = (byte)fila["Orden"]; }
                    lista.Add(relacionActual);
                }
                conexion.Close();
                return lista;
            }
            catch (Exception e) { return new List<DirectorioCatalogosTO>(); }

        }

        #endregion Catalogos Members

    }
}
