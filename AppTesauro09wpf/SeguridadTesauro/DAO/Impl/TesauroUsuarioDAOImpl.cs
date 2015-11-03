using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using SeguridadTesauro.TO;

namespace SeguridadTesauro.DAO.Impl
{
    class TesauroUsuarioDAOImpl:UsuarioDAO
    {
        private DbConnection GetConnection()
        {
            String tipoAplicacion = ConfigurationManager.AppSettings.Get("tipoAplicacion");

            String bdStringSql;

            if (tipoAplicacion.Equals("PRUEBA"))
            {
                bdStringSql = ConfigurationManager.ConnectionStrings["TematicoPrueba"].ConnectionString;
                //MessageBox.Show(ConfigurationManager.AppSettings.Get("MensajeAppPrueba"));
            }
            else
                bdStringSql = ConfigurationManager.ConnectionStrings["Tematico"].ConnectionString;
            DbConnection realConnection = new SqlConnection(bdStringSql);
            return realConnection;


        }

        public DbCommand getCommand(String sql, DbConnection con)
        {
            if (con.GetType() == typeof(OleDbConnection))
            {
                return new OleDbCommand(sql, (OleDbConnection)con);
            }
            else
            {
                return new SqlCommand(sql, (SqlConnection)con);
            }
        }


        #region UsuarioDAO Members

        public List<UsuarioTO> ObtenUsuarios()
        {
            List<UsuarioTO> temas = new List<UsuarioTO>();
            DbConnection connection = null;
            string sqlString;

            try
            {
                connection = GetConnection();
                connection.Open();

                sqlString = "SELECT Acceso.Id, Acceso.Nombre, Acceso.pwd, Acceso.Tipo, Acceso.Rol" +
                            " FROM Acceso ";
                //Create Command
                DbCommand command = getCommand(sqlString, connection);

                DbDataReader reader = command.ExecuteReader();

                // Add each object to the objects list.
                while (reader.Read())
                {
                    UsuarioTO usuario= new UsuarioTO();
                    usuario.Id = reader.GetInt32(0);
                    usuario.UserName = reader.GetString(1);
                    usuario.TipoUsuario = reader.GetInt32(2);
                    temas.Add(usuario);
                }

            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                }
            }
            return temas;
        }

        public bool VerificaUsuario(string usuario, string Pass)
        {
            DbConnection connection = null;
            string sqlString;
            UsuarioTO Usuario = new UsuarioTO();
            Usuario.Id = -1;
            try
            {
                connection = GetConnection();
                connection.Open();

                sqlString = "SELECT Acceso.Id" +
                            " FROM Acceso  WHERE Nombre = '" + usuario + "' AND pwd ='" + Pass + "'";
                //Create Command
                DbCommand command = getCommand(sqlString, connection);

                DbDataReader reader = command.ExecuteReader();

                // Add each object to the objects list.
                while (reader.Read())
                {
                    Usuario.Id = reader.GetInt16(0);
                }
            }
            catch (InvalidCastException exc)
            {
                Usuario.Id = -1;
                if (!EventLog.SourceExists("Tesauro"))
                {
                    EventLog.CreateEventSource("Tesauro", "Tesauro");
                }
                EventLog Logg = new EventLog("Tesauro");
                Logg.Source = "Tesauro";
                String mensaje = "VentanaController Exception at loadTemas()\n" + exc.Message + exc.StackTrace;
                Logg.WriteEntry(mensaje);
                Logg.Close();
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                }
            }
            return !(Usuario.Id==-1);
        }

        public int ObtenerId(string usuario)
        {
            DbConnection connection = null;
            string sqlString;
            UsuarioTO Usuario = new UsuarioTO();
            Usuario.Id = -1;
            try
            {
                connection = GetConnection();
                connection.Open();

                sqlString = "SELECT Acceso.Id" +
                            " FROM Acceso  WHERE Nombre = '" + usuario + "'";
                //Create Command
                DbCommand command = getCommand(sqlString, connection);

                DbDataReader reader = command.ExecuteReader();

                // Add each object to the objects list.
                while (reader.Read())
                {
                    Usuario.Id = reader.GetInt16(0);
                }
            }
            catch (InvalidCastException exc)
            {
                Usuario.Id = -1;
                if (!EventLog.SourceExists("Tesauro"))
                {
                    EventLog.CreateEventSource("Tesauro", "Tesauro");
                }
                EventLog Logg = new EventLog("Tesauro");
                Logg.Source = "Tesauro";
                String mensaje = "VentanaController Exception at loadTemas()\n" + exc.Message + exc.StackTrace;
                Logg.WriteEntry(mensaje);
                Logg.Close();
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                }
            }
            return Usuario.Id;
        }

        public int ObtenRol(String usuario)
        {
            DbConnection connection = null;
            string sqlString;
            UsuarioTO Usuario = new UsuarioTO();
            Usuario.Id = -1;
            try
            {
                connection = GetConnection();
                connection.Open();

                sqlString = "SELECT Acceso.Rol" +
                            " FROM Acceso  WHERE Nombre = '" + usuario + "'";
                //Create Command
                DbCommand command = getCommand(sqlString, connection);

                DbDataReader reader = command.ExecuteReader();

                // Add each object to the objects list.
                while (reader.Read())
                {
                    Usuario.Rol = reader.GetInt32(0);
                }
            }
            catch (InvalidCastException exc)
            {
                Usuario.Rol= -1;
                if (!EventLog.SourceExists("Tesauro"))
                {
                    EventLog.CreateEventSource("Tesauro", "Tesauro");
                }
                EventLog Logg = new EventLog("Tesauro");
                Logg.Source = "Tesauro";
                String mensaje = "VentanaController Exception at loadTemas()\n" + exc.Message + exc.StackTrace;
                Logg.WriteEntry(mensaje);
                Logg.Close();
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                }
            }
            return Usuario.Rol;
        }

        public List<MateriaTO> ObtenMaterias(int Usuario)
        {
            DbConnection connection = null;
            string sqlString;
            List<MateriaTO> materias = new List<MateriaTO>();

            if (Usuario == -1)
                return materias;

            try
            {
                connection = GetConnection();
                connection.Open();

                sqlString = " SELECT DISTINCT Materias.IdMateria, Materias.Descripcion, MateriaUsuario.Acceso " +
                            " FROM Materias, MateriaUsuario  WHERE Materias.IdMateria = MateriaUsuario.IdMateria AND "+
                            " MateriaUsuario.IdUsuario = " + Usuario ;
                //Create Command
                DbCommand command = getCommand(sqlString, connection);

                DbDataReader reader = command.ExecuteReader();

                // Add each object to the objects list.
                while (reader.Read())
                {
                    MateriaTO mat = new MateriaTO();
                    mat.Id = reader.GetInt32(0);
                    mat.Descripcion = reader.GetString(1);
                    mat.Acceso = reader.GetInt32(2);
                    materias.Add(mat);
                }
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                }
            }
            return materias;
        }

        public List<PermisoTO> ObtenPermisos(int Usuario)
        {
            DbConnection connection = null;
            string sqlString;
            List<PermisoTO> Permisos = new List<PermisoTO>();
            try
            {
                connection = GetConnection();
                connection.Open();

                sqlString = "SELECT Permiso.IdPermiso, Permiso.Descripcion, Permiso.Categoria" +
                            " FROM Permiso, PerfilPermisos, Acceso  WHERE Permiso.IdPermiso = PerfilPermisos.IdPermiso AND " +
                            " PerfilPermisos.IdPerfil = Acceso.IdPerfil AND Acceso.Id = " + Usuario;
                //Create Command
                DbCommand command = getCommand(sqlString, connection);

                DbDataReader reader = command.ExecuteReader();

                // Add each object to the objects list.
                while (reader.Read())
                {
                    PermisoTO per = new PermisoTO();
                    per.Id = reader.GetInt32(0);
                    per.Descripción = reader.GetString(1);
                    Permisos.Add(per);
                }
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                }
            }
            return Permisos;
        }

        public List<UsuarioTO> ObtenTodosUsuarios()
        {
            List<UsuarioTO> Resultado = new List<UsuarioTO>();
            DbConnection connection = null;
            string sqlString;
            try
            {
                connection = GetConnection();
                sqlString = "Select Id, nombre, pwd, Tipo, IdPerfil, Rol from acceso order by Nombre";
                connection.Open();
                DbCommand comando = getCommand(sqlString, connection);
                DbDataReader lector = comando.ExecuteReader();
                while (lector.Read())
                {
                    UsuarioTO item = new UsuarioTO();
                    item.Id = lector.GetInt16(0);
                    item.UserName = lector.GetString(1);
                    item.Pwd = lector.GetString(2);
                    item.TipoUsuario = lector.GetInt16(3);
                    item.Perfil = lector.GetInt32(4);
                    item.Rol = lector.GetInt32(5);
                    Resultado.Add(item);
                }
            }
            finally
            {
                connection.Close();
            }
            return Resultado;
        }

        public List<PerfilTO> ObtenPerfiles()
        {
            List<PerfilTO> Resultado = new List<PerfilTO>();
            DbConnection connection = null;
            string sqlString;
            try
            {
                connection = GetConnection();
                sqlString = "Select Id, Descripcion from Perfil order by Descripcion";
                connection.Open();
                DbCommand comando = getCommand(sqlString, connection);
                DbDataReader lector = comando.ExecuteReader();
                while (lector.Read())
                {
                    PerfilTO item = new PerfilTO();
                    item.Id = lector.GetInt32(0);
                    item.Descripcion = lector.GetString(1);
                    
                    Resultado.Add(item);
                }
            }
            finally
            {
                connection.Close();
            }
            return Resultado;
        }

        #endregion
    }
}
