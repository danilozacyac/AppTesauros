using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data.Common;
using System.Data.Odbc;
using System.Data.OleDb;
using mx.gob.scjn.electoral_common.utils;
using System.Data;

namespace mx.gob.scjn.electoral_common.context
{
    public class DBContext
    {
        private static DbProviderFactory fabricaDeConexiones; //usar si se vuelven a presentar inconsistencias en las conexiones.
        public DbConnection ContextConection { get { return this.contextConection; } }
        private DbConnection contextConection { get { return GetDBConection(); } set { SetDBConnection(); } }
        public DbConnection ContextConectionEVA { get { return this.contextConectionEVA; } }
        private DbConnection contextConectionEVA { get { return GetDBConectionEVA(); } set { SetDBConnectionEVA(); } }
        public DbConnection ContextConectionELE { get { return this.contextConectionELE; } }
        private DbConnection contextConectionELE { get { return GetDBConectionELE(); } set { SetDBConnectionELE(); } }
        private static DbConnection RealDBConection;
        private static DbConnection RealDBConectionEVA;

        private static void SetDBConnection()
        {
            throw new NotImplementedException();
        }

        private static void SetDBConnectionEVA()
        {
            throw new NotImplementedException();
        }
        private static void SetDBConnectionELE()
        {
            throw new NotImplementedException();
        }

        private DbConnection GetDBConection()
        {
            //if ((RealDBConection == null)||(RealDBConection.State!=ConnectionState.Open))
            //{
            if ((IUSConstants.IUS_DATABASE_ACCESS_ELE != null) && (!IUSConstants.IUS_DATABASE_ACCESS_ELE.Equals(IUSConstants.CADENA_VACIA)))
            {
                RealDBConection = new OleDbConnection(IUSConstants.IUS_DATABASE_ACCESS_ELE);
                return RealDBConection;
            }
            else
            {
                RealDBConection = new SqlConnection(IUSConstants.IUS_DATABASE_SQL);
                return RealDBConection;
            }
            //}
            //else
            //{
            //    return RealDBConection;
            //}
        }
        private DbConnection GetDBConectionEVA()
        {
            //if ((RealDBConection == null)||(RealDBConection.State!=ConnectionState.Open))
            //{
            if ((IUSConstants.IUS_DATABASE_ACCESS_ELE != null) && (!IUSConstants.IUS_DATABASE_ACCESS_ELE.Equals(IUSConstants.CADENA_VACIA)))
            {
                RealDBConection = new OleDbConnection(IUSConstants.IUS_DATABASE_ACCESS_ELE);
                return RealDBConection;
            }
            else
            {
                RealDBConection = new SqlConnection(IUSConstants.IUS_DATABASE_SQL);
                return RealDBConection;
            }
        }
        private DbConnection GetDBConectionELE()
        {
            //if ((RealDBConection == null)||(RealDBConection.State!=ConnectionState.Open))
            //{
            if ((IUSConstants.IUS_DATABASE_ACCESS_ELE != null) && (!IUSConstants.IUS_DATABASE_ACCESS_ELE.Equals(IUSConstants.CADENA_VACIA)))
            {
                RealDBConection = new OleDbConnection(IUSConstants.IUS_DATABASE_ACCESS_ELE);
                return RealDBConection;
            }
            else
            {
                RealDBConection = new SqlConnection(IUSConstants.IUS_DATABASE_SQL);
                return RealDBConection;
            }
        }

        public DbConnection RegistroUsuariosConnection { get { return getRegistroUsuariosConnection(); } set { } }
        private DbConnection registroUsuariosConnection { get; set; }
        private DbConnection getRegistroUsuariosConnection()
        {
            if (IUSConstants.USUARIOS_DATABASE_ACCESS != null)
            {
                return new OleDbConnection(IUSConstants.USUARIOS_DATABASE_ACCESS);
            }
            else
            {
                return new SqlConnection (IUSConstants.USUARIOS_DATABASE_SQL);
            }
        }

        public DataAdapter dataAdapter(String sql)
        {
            if (this.contextConection.GetType() == typeof(SqlConnection))
            {
                SqlConnection sqlDA = (SqlConnection)this.contextConection;
                return new SqlDataAdapter(sql, sqlDA);
            }
            else
            {
                OleDbConnection oleDA = (OleDbConnection)this.contextConection;
                return new OleDbDataAdapter(sql, oleDA);
            }
            //return new SqlDataAdapter(sql, this.contextConection);
            //return new OdbcDataAdapter(sql, this.contextConection);
        }

        public DataAdapter dataAdapter(String sql, DbConnection conexion)
        {
            if (conexion.GetType() == typeof(SqlConnection))
            {
                SqlConnection sqlDA = (SqlConnection)conexion;
                return new SqlDataAdapter(sql, sqlDA);
            }
            else
            {
                OleDbConnection oleDA = (OleDbConnection)conexion;
                return new OleDbDataAdapter(sql, oleDA);
            }
            //return new SqlDataAdapter(sql, this.contextConection);
            //return new OdbcDataAdapter(sql, this.contextConection);
        }

        
        public DataAdapter RegistroDataAdapter(String sql, DbConnection conexion )
        {
            if (IUSConstants.USUARIOS_DATABASE_ACCESS != null)
            {
                return new OleDbDataAdapter(sql, (OleDbConnection)conexion);
            }
            else
            {
                return new SqlDataAdapter(sql, (SqlConnection)conexion);
            }
        }
        public DbCommand CommandRegister(String sql, DbConnection conexion)
        {
            if (IUSConstants.USUARIOS_DATABASE_ACCESS != null)
            {
                return new OleDbCommand(sql, (OleDbConnection)conexion);
            }
            else
            {
                return new SqlCommand(sql, (SqlConnection)conexion);
            }
        }
    }
}