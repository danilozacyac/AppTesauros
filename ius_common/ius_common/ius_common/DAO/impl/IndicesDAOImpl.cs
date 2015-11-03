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
    /// <summary>
    /// Implementa la interfaz para la obtención de datos
    /// referentes a las búsquedas por Indices.
    /// </summary>
    /// <remarks author="Carlos de Luna Sáenz"></remarks>
    /// 

    public class IndicesDAOImpl : IndicesDAO
    {
        private ILog log = LogManager.GetLogger("mx.gob.scjn.ius_common.DAO.impl.IndicesDAOImpl");
        ///
        /// <summary>
        /// El conector hacia la BD
        /// </summary>
        /// 

        public DBContext contextoBD;

        ///
        /// <summary>
        /// El constructor por omisión, inicia las conexiones a la BD
        /// </summary>
        /// 

        public IndicesDAOImpl()
        {
            IUSApplicationContext contexto = new IUSApplicationContext();
            contextoBD = (DBContext)contexto.getInitialContext().GetObject("dataSource");
        }

        ///
        /// <summary>
        /// Obtiene los hijos de una rama en el arvol de índices
        /// </summary>
        /// <param name="idPadre">El padre del que se quieren las ramas u hojas hijas</param>
        /// <returns>La lista de hijos.</returns>
        /// 
        public List<CIndicesTO> obtenHijos(int idPadre)
        {
            DbConnection conexion = contextoBD.ContextConection;
            try
            {
                DataAdapter query = contextoBD.dataAdapter
                                ("Select IdInd, nNivel, cDesc, cTag, cKey, nIdPadre, cImagen"
                                + "     from CIndices "
                                + " where nIdPadre=" + idPadre + " order by idind", conexion);
                DataSet datos = new DataSet();
                query.Fill(datos);
                List<CIndicesTO> lista = new List<CIndicesTO>();
                DataTableReader tabla = datos.Tables[0].CreateDataReader();
                conexion.Close();
                //foreach (DataRow fila in tabla.Rows)
                while(tabla.Read())
                {
                    CIndicesTO indiceActual = new CIndicesTO();
                    indiceActual.setIdInd((short)tabla["idInd"]);
                    indiceActual.setNumeroNivel((byte)tabla["nNivel"]);
                    indiceActual.setCadenaDesc("" + tabla["cDesc"]);
                    indiceActual.setCadenaTag(("" + tabla["cTag"]));
                    indiceActual.setCadenaKey("" + tabla["cKey"]);
                    indiceActual.setNumeroIdPadre((int)tabla["nIdPadre"]);
                    indiceActual.setCadenaImagen("" + tabla["cImagen"]);
                    lista.Add(indiceActual);
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
                String mensaje = "IndicesDAOImpl Exception at ObtenHijos(int)\n" + e.Message + e.StackTrace;
                Logg.WriteEntry(mensaje);
                Logg.Close();
                return new List<CIndicesTO>();
            }
        }

        /// 
        /// <summary>
        /// Obtiene los hijos del arbol si es que este es padre
        /// </summary>
        /// <param name="idPadre">El identificador del Tribunal que es el padre en el arbol</param>
        /// <returns>La lista de los tribunales hijos</returns>
        /// 

        public List<TCCTO> getTribunal(int idPadre)
        {
            DbConnection conexion = contextoBD.ContextConection;
            try
            {
                DataAdapter query = contextoBD.dataAdapter
                                ("Select IdTCC, DescTCC, CTO, MAT, ORD, DESC_MAY, Contador"
                                +"     from tribcol"
                                +"     where CTO="+idPadre
                                +"           order by mat, ord");
                DataSet datos = new DataSet();
                query.Fill(datos);
                List<TCCTO> lista = new List<TCCTO>();
                DataTableReader tabla = datos.Tables[0].CreateDataReader();
                conexion.Close();
                //foreach (DataRow fila in tabla.Rows)
                while(tabla.Read())
                {
                    TCCTO indiceActual = new TCCTO();
                    indiceActual.setIdTribunal((short)tabla["IdTCC"]);
                    indiceActual.setDescripcion("" + tabla["DescTCC"]);
                    indiceActual.setCircuito((short)tabla["CTO"]);
                    indiceActual.setOrd(((short)tabla["ORD"]));
                    indiceActual.setDescripcionMayusculas("" + tabla["DESC_MAY"]);
                    indiceActual.setContador((int)tabla["Contador"]);
                    lista.Add(indiceActual);
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
                String mensaje = "IndicesDAOImpl Exception at GetTribunal(int)\n" + e.Message + e.StackTrace;
                Logg.WriteEntry(mensaje);
                Logg.Close();
                return new List<TCCTO>();
            }
        }
    }
}