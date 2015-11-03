using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using mx.gob.scjn.ius_common.context;
using System.Data;
using mx.gob.scjn.ius_common.TO;
using System.Diagnostics;

namespace mx.gob.scjn.ius_common.DAO.impl
{
    public class InformesDAOImpl : InformesDAO
    {
        /// <summary>
        ///     El contexto que maneja la Base de Datos.
        /// </summary>
        private DBContext contextoBD;

        public InformesDAOImpl()
        {
            IUSApplicationContext contexto = new IUSApplicationContext();
            contextoBD = (DBContext)contexto.getInitialContext().GetObject("dataSource");

        }

        public List<InformesTO> getInformes()
        {
            DbConnection conexion = contextoBD.ContextConection;
            try
            {
                string selectQuery = " select A.Id, A.descripcion, A.Orden, A.URL " +
                                     " from Informes As A order by A.Orden";
                if (conexion.State != ConnectionState.Open) conexion.Open();
                DataAdapter query = contextoBD.dataAdapter(selectQuery, conexion);
                DataSet datos = new DataSet();
                query.Fill(datos);
                List<InformesTO> lista = new List<InformesTO>();
                DataTableReader tabla = datos.Tables[0].CreateDataReader();

               while (tabla.Read())
                {
                    InformesTO InformeActual = new InformesTO();
                    InformeActual.Id = (int)tabla["Id"];
                    InformeActual.Descripcion = (String)tabla["Descripcion"];
                    InformeActual.Orden = (int)tabla["orden"];
                    InformeActual.URl = (String)tabla["URL"];
                    lista.Add(InformeActual);
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
                String mensaje = "TesisDAOImpl Exception at getTesisPorIus(Int32\n" + e.Message + e.StackTrace;
                Logg.WriteEntry(mensaje);
                Logg.Close();
                return new List<InformesTO>();
            }
        }
    }
}
