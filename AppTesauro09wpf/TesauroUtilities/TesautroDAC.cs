using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using ScjnUtilities;
using TesauroTO;

namespace TesauroUtilities
{
    public class TesauroDAC
    {
        public static int UsuarioActual;
        public static TemaTO TemaImporta;

        private SqlConnection GetConnection()
        {
            String tipoAplicacion = ConfigurationManager.AppSettings.Get("tipoAplicacion");

            String bdStringSql;

            if (tipoAplicacion.Equals("PRUEBA"))
                bdStringSql = ConfigurationManager.ConnectionStrings["TematicoPrueba"].ConnectionString;
            else
                bdStringSql = ConfigurationManager.ConnectionStrings["Tematico"].ConnectionString;
            SqlConnection realConnection = new SqlConnection(bdStringSql);
            
            return realConnection;
        }

        public DbCommand GetCommand(String sql, DbConnection con)
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

        public List<TemaTO> GetAllTemasOrder(string orderBy)
        {
            List<TemaTO> temas = new List<TemaTO>();
            DbConnection connection = GetConnection();
            string sqlString;

            try
            {
                connection.Open();

                sqlString = "SELECT Temas.IDTema, Temas.Descripcion, Temas.DescripcionStr," +
                            "Temas.Nivel, Temas.IDPadre, Temas.IDUser, Temas.Fecha, Temas.Hora, " +
                            " Temas.Notas, Temas.Observaciones, Temas.Materia, Temas.Status,Temas.idOrigen " +
                            " FROM Temas" +
                            " ORDER BY " + orderBy;
                //Create Command
                DbCommand command = GetCommand(sqlString, connection);

                DbDataReader reader = command.ExecuteReader();

                // Add each object to the objects list.
                while (reader.Read())
                {
                    TemaTO tema = new TemaTO(reader.GetInt32(0), reader.GetString(1),
                        reader.GetString(2), reader.GetInt16(3), reader.GetInt32(4),
                        reader.GetInt16(5), reader.GetDateTime(6), reader.GetDateTime(7),
                        reader.GetString(8), reader.GetString(9), reader.GetInt32(10),
                        reader.GetInt32(11), reader.GetInt32(12));

                    temas.Add(tema);
                }
            }
            finally
            {
                connection.Close();
            }
            return temas;
        }

        /// <summary>
        /// Devuelve una lista de temas por materia en relación al usuario
        /// </summary>
        /// <param name="orderBy">Criterio por el que se ordenara el listado de temas </param>
        /// <param name="materias">Materias asignadas a cada usuario</param>
        /// <returns></returns>
        public List<TemaTO> GetAllTemasOrder(string orderBy, List<int> materias)
        {
            List<TemaTO> temas = new List<TemaTO>();
            DbConnection connection = GetConnection();
            string sqlString;

            String qMaterias = "";

            foreach (int materia in materias)
            {
                qMaterias += " OR Materia = " + materia;
            }

            qMaterias = qMaterias.Substring(3);

            try
            {
                connection.Open();

                sqlString = "SELECT Temas.IDTema, Temas.Descripcion, Temas.DescripcionStr," +
                            "Temas.Nivel, Temas.IDPadre, Temas.IDUser, Temas.Fecha, Temas.Hora, " +
                            " Temas.Notas, Temas.Observaciones, Temas.Materia, Temas.Status,Temas.idOrigen, " +
                            " (SELECT COUNT(idTEma) FROM TemasTesis T WHERE T.idTema = Temas.Idtema and T.idMateria = Temas.Materia ) Total " +
                            " FROM Temas " +
                            ((materias.Count > 0) ? "WHERE " + qMaterias : "") +
                            " ORDER BY " + orderBy;
                //Create Command
                DbCommand command = GetCommand(sqlString, connection);

                DbDataReader reader = command.ExecuteReader();

                // Add each object to the objects list.
                while (reader.Read())
                {
                    TemaTO tema = new TemaTO(reader.GetInt32(0), reader.GetString(1),
                        reader.GetString(2), reader.GetInt16(3), reader.GetInt32(4),
                        reader.GetInt16(5), reader.GetDateTime(6), reader.GetDateTime(7),
                        reader.GetString(8), reader.GetString(9), reader.GetInt32(10),
                        reader.GetInt32(11), reader.GetInt32(12), reader.GetInt32(12), reader.GetInt32(13));

                    temas.Add(tema);
                }
            }
            finally
            {
                connection.Close();
            }
            return temas;
        }

        public List<TemaTO> GetAllSubTemasOrder(int idPadre, string orderBy)
        {
            List<TemaTO> temas = new List<TemaTO>();
            DbConnection connection = GetConnection();
            string sqlString;

            try
            {
                connection.Open();

                sqlString = "SELECT Temas.IDTema, Temas.Descripcion, Temas.DescripcionStr," +
                            " Temas.Nivel, Temas.IDPadre, Temas.IDUser, Temas.Fecha, Temas.Hora" +
                            " Temas.Notas, Temas.Observaciones, Temas.Materia, Temas.Status " +
                            " FROM Temas" +
                            " WHERE idPadre = " + idPadre +
                            " ORDER BY " + orderBy;
                //Create Command
                DbCommand command = GetCommand(sqlString, connection);

                DbDataReader reader = command.ExecuteReader();

                // Add each object to the objects list.
                while (reader.Read())
                {
                    TemaTO tema = new TemaTO(reader.GetInt32(0), reader.GetString(1),
                        reader.GetString(2), reader.GetInt16(3), reader.GetInt32(4),
                        reader.GetInt16(5), reader.GetDateTime(6), reader.GetDateTime(7),
                        reader.GetString(8), reader.GetString(9), reader.GetInt16(10),
                        reader.GetInt32(11), reader.GetInt32(12));

                    temas.Add(tema);
                }
            }
            finally
            {
                connection.Close();
            }
            return temas;
        }

        /// <summary>
        /// Verifica que el tema que se va a ingresar no exista
        /// </summary>
        /// <param name="texto"></param>
        /// <param name="idpadre"></param>
        /// <returns></returns>
        public bool VerifyExistence(String texto, int idpadre, int idMateria)
        {
            //List<TemaTO> temas = new List<TemaTO>();
            DbConnection connection = connection = GetConnection();
            string sqlString;

            bool exist = false;

            try
            {
                connection.Open();

                sqlString = "SELECT * FROM Temas WHERE DescripcionStr = '" + texto +
                            "' and idpadre = " + idpadre + " and materia = " + idMateria;// +" ORDER BY " + orderBy;
                //Create Command
                DbCommand command = GetCommand(sqlString, connection);

                DbDataReader reader = command.ExecuteReader();

                // Add each object to the objects list.
                while (reader.Read())
                {
                    exist = true;
                }
            }
            finally
            {
                connection.Close();
            }
            return exist;
        }

        public List<int> GetTesisPorTemaOrder(TemaTO tema, string orderBy)
        {
            List<int> tesis = new List<int>();
            DbConnection connection = GetConnection();
            string sqlString;

            try
            {
                connection.Open();

                sqlString = "SELECT IUS  FROM TemasTesis WHERE idMateria = " + tema.Materia + " AND idTema = " + tema.IDTema +
                            " ORDER BY " + orderBy;
                //Create Command
                DbCommand command = GetCommand(sqlString, connection);

                DbDataReader reader = command.ExecuteReader();

                // Add each object to the objects list.
                while (reader.Read())
                {
                    tesis.Add(reader.GetInt32(0));
                }
            }
            finally
            {
                connection.Close();
            }
            return tesis;
        }

        #region Expresiones

        public List<ExpresionTO> GetAllExpresionForTema(int idTema, string orderBy)
        {
            List<ExpresionTO> lista = new List<ExpresionTO>();
            DbConnection connection = GetConnection();
            string sqlString;

            try
            {
                connection.Open();

                sqlString = "SELECT Expresion.IDTema, Expresion.Descripcion, Expresion.IDUser," +
                            "Expresion.Fecha, Expresion.Hora, Expresion.IdExpresion, " +
                            "Expresion.Operador " +
                            " FROM Expresion" +
                            " WHERE idTema = " + idTema +
                            " ORDER BY " + orderBy;

                //Create Command
                DbCommand command = GetCommand(sqlString, connection);

                DbDataReader reader = command.ExecuteReader();

                // Add each object to the objects list.
                while (reader.Read())
                {
                    int IdTema = reader.GetInt32(0);
                    String descripcion = reader.GetString(1);
                    int usuario = reader.GetInt16(2);
                    DateTime fecha = reader.GetDateTime(3);
                    DateTime hora = reader.GetDateTime(4);
                    int idExpresion = reader.GetInt32(5);
                    int operador = reader.GetInt32(6);
                    sqlString = "Select Campo, IdExpresion from CamposExpresion where IdExpresion = " + idExpresion;
                    DbCommand comando2 = GetCommand(sqlString, connection);
                    DbDataReader lector = comando2.ExecuteReader();
                    List<int> campos = new List<int>();
                    while (lector.Read())
                    {
                        campos.Add(lector.GetInt32(0));
                    }
                    ExpresionTO objeto = new ExpresionTO(IdTema, descripcion,
                        usuario,fecha,hora,idExpresion,operador,campos.ToArray());

                    lista.Add(objeto);
                }
            }
            finally
            {
                connection.Close();
            }
            return lista;
        }

        public List<ExpresionTO> ObtenExpresiones(int IdTema, int idMateria)
        {
            List<ExpresionTO> lista = new List<ExpresionTO>();
            DbConnection connection = null;
            string sqlString;

            try
            {
                connection = GetConnection();
                connection.Open();
                sqlString = "SELECT Expresion.IDTema, Expresion.Descripcion, " +
                            " Expresion.IDUser, Expresion.Fecha, Expresion.Hora, " +
                            " Expresion.IDExpresion, Expresion.Operador " +
                            " FROM Expresion " +
                            " WHERE idTema = " + IdTema + " AND idMateria = " + idMateria +
                            " ORDER BY  IdExpresion";
                //Create Command
                DbCommand command = GetCommand(sqlString, connection);
                DbDataReader reader = command.ExecuteReader();

                // Add each object to the objects list.
                while (reader.Read())
                {
                    Int32 id = reader.GetInt32(0);
                    String desc = reader.GetString(1);
                    Int16 user = 0;// reader.GetInt16(5) == null ? 0 : reader.GetInt16(5);
                    DateTime fecha = DateTime.Now; // reader.GetDateTime(6);
                    DateTime hora = DateTime.Now;  // reader.GetDateTime(7);
                    Int32 idExpresion = reader.GetInt32(5);
                    Int32 operador = reader.GetByte(6);
                    sqlString = "Select Campo, IdExpresion from CamposExpresion where IdExpresion = " + idExpresion;
                    DbConnection conexion2 = GetConnection();
                    if (conexion2.State != ConnectionState.Open)
                        conexion2.Open();
                    DbCommand comando2 = GetCommand(sqlString, conexion2);
                    DbDataReader lector = comando2.ExecuteReader();
                    List<int> campos = new List<int>();
                    while (lector.Read())
                    {
                        campos.Add(lector.GetInt32(0));
                    }
                    conexion2.Close();
                    ExpresionTO objeto = new ExpresionTO(id, desc, user, fecha,
                        hora, idExpresion, operador, campos.ToArray());
                    lista.Add(objeto);
                }
            }
            finally
            {
                connection.Close();
            }
            return lista;
        }

        public void ActualizaExpresion(ExpresionTO expresion, int idMateria)
        {
            DbConnection connection = null;
            string sqlString;

            try
            {
                connection = GetConnection();
                connection.Open();

                sqlString = "UPDATE Expresion SET Descripcion = '" + expresion.Descripcion +
                            "', Operador = " + expresion.Operador +
                            " WHERE IdExpresion = " + expresion.Id + " AND idMateria = " + idMateria;

                //Create Command
                DbCommand command = GetCommand(sqlString, connection);
                int reader = command.ExecuteNonQuery();
                sqlString = "Delete from CamposExpresion where idExpresion = " + expresion.Id;
                DbCommand comando = GetCommand(sqlString, connection);
                reader += comando.ExecuteNonQuery();
                connection.Close();
                connection.Open();
                foreach (int item in expresion.Campos)
                {
                    sqlString = "Insert into CamposExpresion (IDExpresion, Campo) VALUES (" +
                                expresion.Id + ", " + item + ")";
                    DbCommand comando2 = GetCommand(sqlString, connection);
                    reader += comando2.ExecuteNonQuery();
                }
                // Add each object to the objects list.
                connection.Close();
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,TesauroDAC", "Tesauro");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,TesauroDAC", "Tesauro");
            }
            finally
            {
                connection.Close();
            }
        }

        public void EliminarExpresioni(int idExpresion)
        {
            DbConnection connection = null;
            string sqlString;

            try
            {
                connection = GetConnection();
                connection.Open();

                sqlString = "DELETE FROM Expresion " +
                            " WHERE IdExpresion = " + idExpresion;

                //Create Command
                DbCommand command = GetCommand(sqlString, connection);
                command.ExecuteNonQuery();
            }
            finally
            {
                connection.Close();
            }
        }

        public void InsertaExpresion(ExpresionTO ExpresionActual, int idMateria)
        {
            DbConnection connection = null;
            string sqlString;

            try
            {
                connection = GetConnection();
                connection.Open();
                int idExpresion = ObtenMaximoIDExpresion();
                sqlString = "Insert into Expresion (IDTema, Descripcion, IDUser, Fecha, Hora, IdExpresion, Operador,idMateria) VALUES (" +
                            ExpresionActual.IDTema + ", '" + ExpresionActual.Descripcion + "', " + UsuarioActual + ", SYSDATETIME() , SYSDATETIME() ," +
                            +idExpresion + ", " + ExpresionActual.Operador + "," + idMateria + ")";
                DbCommand command = GetCommand(sqlString, connection);
                int reader = command.ExecuteNonQuery();
                foreach (int item in ExpresionActual.Campos)
                {
                    sqlString = "Insert into CamposExpresion (IDExpresion, Campo) VALUES (" +
                                idExpresion + ", " + item + ")";
                    command = GetCommand(sqlString, connection);
                    reader = command.ExecuteNonQuery();
                }
                // Add each object to the objects list.
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,TesauroDAC", "Tesauro");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,TesauroDAC", "Tesauro");
            }
            finally
            {
                connection.Close();
            }
        }

        public int ObtenMaximoIDExpresion()
        {
            DbConnection connection = GetConnection();
            string sqlString;
            int objeto = 0;
            int resultado = 0;
            
            try
            {
                connection.Open();

                sqlString = "SELECT Max(IDExpresion) + 1 FROM Expresion ";
                //Create Command
                DbCommand command = GetCommand(sqlString, connection);

                DbDataReader reader = command.ExecuteReader();

                // Add each object to the objects list.
                while (reader.Read())
                {
                    objeto = reader.GetInt32(0);
                }
                resultado = objeto;
            }
            catch (InvalidCastException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,TesauroDAC", "Tesauro");
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,TesauroDAC", "Tesauro");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,TesauroDAC", "Tesauro");
            }
            finally
            {
                connection.Close();
            }
            return resultado;
        }

        #endregion

        #region Sinonimos

        public List<SinonimoTO> GetSinonimosForTema(int idTema, string orderBy, int idMateria)
        {
            List<SinonimoTO> lista = new List<SinonimoTO>();
            DbConnection connection = GetConnection();
            string sqlString;

            try
            {
                connection.Open();

                sqlString = "SELECT Sinonimos.IDTema, Sinonimos.IDPadre, Sinonimos.Descripcion, Sinonimos.Tipo, Sinonimos.DescripcionStr, " +
                            " Sinonimos.IDUser, Sinonimos.Fecha, Sinonimos.Hora, Sinonimos.Notas, Sinonimos.Observaciones" +
                            " FROM Sinonimos" +
                            " WHERE idPadre = " + idTema +
                            " AND idMateria = " + idMateria +
                            " AND Tipo = 1 " +
                            " ORDER BY " + orderBy;

                //Create Command
                DbCommand command = GetCommand(sqlString, connection);

                DbDataReader reader = command.ExecuteReader();

                // Add each object to the objects list.
                while (reader.Read())
                {
                    Int32 id = reader.GetInt32(0);
                    Int32 idPadre = reader.GetInt32(1);
                    String desc = reader.GetString(2);
                    Byte tipo = reader.GetByte(3);
                    String descStr = reader.GetString(4);
                    Int16 user = 0;// reader.GetInt16(5) == null ? 0 : reader.GetInt16(5);
                    DateTime fecha = DateTime.Now; // reader.GetDateTime(6);
                    DateTime hora = DateTime.Now;  // reader.GetDateTime(7);
                    String nota = reader.GetString(8);
                    String observaciones = reader.GetString(9);
                    SinonimoTO objeto = new SinonimoTO(id, idPadre, desc, tipo,
                        descStr, user, fecha, hora, nota, observaciones);

                    lista.Add(objeto);
                }
            }
            finally
            {
                connection.Close();
            }
            return lista;
        }

        public void GeneraNuevoSinonimo(SinonimoTO sinonimoActual, int idMateria)
        {
            DbConnection connection = null;
            string sqlString;

            try
            {
                connection = GetConnection();
                connection.Open();

                sqlString = "INSERT INTO Sinonimos(IDTema, IDPadre, Descripcion, " +
                            " DescripcionStr, Tipo, IDUser, Fecha, Hora, Notas, Observaciones,idMateria) " +
                            " VALUES (" + sinonimoActual.IDTema + ", " + sinonimoActual.IDPadre + ", " +
                            " '" + sinonimoActual.Descripcion.Trim() + "', " +
                            " '" + sinonimoActual.DescripcionStr + "', " +
                            sinonimoActual.Tipo + ", " + UsuarioActual + ", SYSDATETIME() , SYSDATETIME() , '" +
                            sinonimoActual.Nota + "', '" + sinonimoActual.Observaciones + "'," + idMateria + " )";

                //Create Command
                DbCommand command = GetCommand(sqlString, connection);
                int reader = command.ExecuteNonQuery();
                // Add each object to the objects list.
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,TesauroDAC", "Tesauro");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,TesauroDAC", "Tesauro");
            }
            finally
            {
                connection.Close();
            }
        }

        public void ActualizaSinonimo(SinonimoTO sinonimoActual, int idMateria)
        {
            DbConnection connection = GetConnection();
            string sqlString;

            try
            {
                connection.Open();

                sqlString = "UPDATE Sinonimos SET IDPadre = " + sinonimoActual.IDPadre + ", " +
                            " Descripcion = '" + sinonimoActual.Descripcion.Trim() + "', " +
                            " DescripcionStr = '" + sinonimoActual.DescripcionStr + "', " +
                            " Notas = '" + sinonimoActual.Nota + "', " +
                            " Observaciones = '" + sinonimoActual.Observaciones + "'" +
                            " WHERE IDTema = " + sinonimoActual.IDTema + " AND idMateria = " + idMateria;

                //Create Command
                DbCommand command = GetCommand(sqlString, connection);
                int reader = command.ExecuteNonQuery();
                // Add each object to the objects list.
                connection.Close();
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,TesauroDAC", "Tesauro");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,TesauroDAC", "Tesauro");
            }
            finally
            {
                connection.Close();
            }
        }

        public void ActualizaTipoSinonimo(int id, int tipo, int idMateria)
        {
            DbConnection connection = GetConnection();
            string sqlString;

            try
            {
                connection.Open();

                sqlString = "UPDATE Sinonimos SET Tipo = " + tipo +
                            " WHERE IDTema = " + id + " AND idMateria = " + idMateria;

                //Create Command
                DbCommand command = GetCommand(sqlString, connection);
                int reader = command.ExecuteNonQuery();
                // Add each object to the objects list.
                connection.Close();
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,TesauroDAC", "Tesauro");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,TesauroDAC", "Tesauro");
            }
            finally
            {
                connection.Close();
            }
        }

        #endregion
        
        #region Relaciones Próximas
        
        public List<SinonimoTO> GetRPForTema(int idTema, string orderBy, int idMateria)
        {
            List<SinonimoTO> lista = new List<SinonimoTO>();
            DbConnection connection = GetConnection();
            string sqlString;
            
            try
            {
                connection.Open();
                            
                sqlString = "SELECT Sinonimos.IDTema, Sinonimos.IDPadre, Sinonimos.Descripcion, Sinonimos.Tipo, Sinonimos.DescripcionStr, " +
                            " Sinonimos.IDUser, Sinonimos.Fecha, Sinonimos.Hora, " +
                            " Sinonimos.Notas, Sinonimos.Observaciones " +
                            " FROM Sinonimos" +
                            " WHERE idPadre = " + idTema +
                            " AND idMateria = " + idMateria +
                            " AND Tipo = 0 " +
                            " ORDER BY " + orderBy;
                
                //Create Command
                DbCommand command = GetCommand(sqlString, connection);

                DbDataReader reader = command.ExecuteReader();
                
                // Add each object to the objects list.
                while (reader.Read())
                {
                    Int32 id = reader.GetInt32(0);
                    Int32 idPadre = reader.GetInt32(1);
                    String desc = reader.GetString(2);
                    Byte tipo = reader.GetByte(3);
                    String descStr = reader.GetString(4);
                    Int16 user = 0;// reader.GetInt16(5) == null ? 0 : reader.GetInt16(5);
                    DateTime fecha = DateTime.Now; // reader.GetDateTime(6);
                    DateTime hora = DateTime.Now;  // reader.GetDateTime(7);
                    String nota = reader.GetString(8);
                    String observaciones = reader.GetString(9);
                    SinonimoTO objeto = new SinonimoTO(id,idPadre, desc, tipo,
                        descStr, user, fecha, hora, nota, observaciones);
                    
                    //int IDTema, int IDPadre, string Descripcion, int Tipo,
                    //string DescripcionStr, int IDUser, DateTime Fecha, DateTime Hora
                
                    lista.Add(objeto);
                }
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,TesauroDAC", "Tesauro");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,TesauroDAC", "Tesauro");
            }
            finally
            {
                connection.Close();
            }
            return lista;
        }
        
        #endregion
        
        #region IA
            
        public TemaTO GetIAForTema(int idTema)
        {
            List<SinonimoTO> lista = new List<SinonimoTO>();
            DbConnection connection = GetConnection();
            string sqlString;
            TemaTO objeto = null;
            try
            {
                connection = GetConnection();
                connection.Open();

                sqlString = "SELECT IDTema, IDAscendente, Descripcion, DescripcionStr, IDUser, Fecha, hora, Notas, Observaciones, Status " +
                            " FROM Ascendente " +
                            " WHERE IDAscendente = " + idTema;
                
                //Create Command
                DbCommand command = GetCommand(sqlString, connection);
                
                DbDataReader reader = command.ExecuteReader();
                    
                // Add each object to the objects list.
                while (reader.Read())
                {
                    objeto = new TemaTO(reader.GetInt32(0),reader.GetString(2), reader.GetString(3),
                        0, reader.GetInt32(1), reader.GetInt32(4), reader.GetDateTime(5), reader.GetDateTime(6),
                        reader.GetString(7), reader.GetString(8), 0, reader.GetInt32(9), -1);
                }
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,TesauroDAC", "Tesauro");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,TesauroDAC", "Tesauro");
            }
            finally
            {
                connection.Close();
            }
            return objeto;
        }

        public void InsertaIA(String ascendente, int idTema)
        {
            DbConnection connection = GetConnection();
            string sqlString;

            try
            {
                connection.Open();
                int idAscendente = ObtenMaximoIdRelaciones("Ascendente");
                sqlString = "Insert into Ascendente (IDTema, IDAscendente, Descripcion, DescripcionStr, iduser, fecha, hora, notas, observaciones, status) VALUES (" +
                            idAscendente + ", " + idTema + ", '" + ascendente.Trim() + "',' " + ascendente.Trim() + " X', " + UsuarioActual + ", " +
                            "SYSDATETIME() , SYSDATETIME(), ' ', ' ',1)";
                DbCommand command = GetCommand(sqlString, connection);
                int reader = command.ExecuteNonQuery();
                // Add each object to the objects list.
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,TesauroDAC", "Tesauro");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,TesauroDAC", "Tesauro");
            }
            finally
            {
                connection.Close();
            }
        }

        public void ActualizaIA(TemaTO datosIA)
        {
            DbConnection connection = GetConnection();
            string sqlString;

            try
            {
                connection.Open();
                //int idAscendente = ObtenMaximoIDtema() + 1;
                sqlString = "Update Ascendente SET Descripcion= '" + datosIA.Descripcion.Trim() + "'," +
                            "DescripcionStr= '" + datosIA.DescripcionStr + "'" +
                            "Where IdTema = " + datosIA.IDTema;
                DbCommand command = GetCommand(sqlString, connection);
                int reader = command.ExecuteNonQuery();
                // Add each object to the objects list.
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,TesauroDAC", "Tesauro");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,TesauroDAC", "Tesauro");
            }
            finally
            {
                connection.Close();
            }
        }
        
        #endregion
            
        public int ActualizaPadreTema(TemaTO temaTO)
        {
            DbConnection connection = null;
            string sqlString;
                
            try
            {
                connection = GetConnection();
                connection.Open();

                sqlString = "UPDATE Temas SET IDPadre = " + temaTO.IDPadre +
                            " WHERE IDTema = " + temaTO.IDTema + " AND Materia = " + temaTO.Materia;
                
                //Create Command
                DbCommand command = GetCommand(sqlString, connection);
                
                int reader = command.ExecuteNonQuery();

                connection.Close();
                // Add each object to the objects list.
                return 0;
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,TesauroDAC", "Tesauro");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,TesauroDAC", "Tesauro");
            }
            finally
            {
                connection.Close();
            }
            return -1;
        }
            
        public void ActualizaTema(TemaTO datos)
        {
            DbConnection connection = GetConnection();
            string sqlString;
                
            try
            {
                connection.Open();
                            
                sqlString = "UPDATE Temas SET IDPadre = " + datos.IDPadre + ", " +
                            " Descripcion = '" + datos.Descripcion.Trim() + "', " +
                            " DescripcionStr = '" + datos.DescripcionStr + "', " +
                            " Notas = '" + datos.Nota + "', " +
                            " Observaciones = '" + datos.Observaciones + "', " +
                            " Status = " + datos.Status +
                            " WHERE IDTema = " + datos.IDTema + " AND Materia = " + datos.Materia;
                
                //Create Command
                DbCommand command = GetCommand(sqlString, connection);
                command.ExecuteNonQuery();
                // Add each object to the objects list.
                connection.Close();
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,TesauroDAC", "Tesauro");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,TesauroDAC", "Tesauro");
            }
            finally
            {
                connection.Close();
            }
        }
            
        //public int ObtenMaximoIDtema()
        //{
        //    //List<SinonimoTO> lista = new List<SinonimoTO>();
        //    DbConnection connection = null;
        //    string sqlString;
        //    int objeto = 0 ;
        //    int resultado = 0;
        //    try
        //    {
        //        connection = GetConnection();
        //        connection.Open();
                
        //        sqlString = "SELECT Max(IDTema)" +
        //                    " FROM Ascendente";
        //        //Create Command
        //        DbCommand command = GetCommand(sqlString, connection);
                
        //        DbDataReader reader = command.ExecuteReader();
                    
        //        // Add each object to the objects list.
        //        try
        //        {
        //            while (reader.Read())
        //            {
        //                objeto = reader.GetInt32(0);
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            objeto = 0;
                    
        //        }
        //        int comparar = 0;
        //        sqlString = "SELECT Max(IDTema)" +
        //                    " FROM Temas ";
        //        //Create Command
        //        connection.Close();
        //        connection.Open();
        //        command = GetCommand(sqlString, connection);
                
        //        reader = command.ExecuteReader();
                    
        //        // Add each object to the objects list.
        //        try
        //        {
        //            while (reader.Read())
        //            {
        //                comparar = reader.GetInt32(0);
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            comparar = 0;
                    
        //        }
        //        int comparar2 = 0;
        //        sqlString = "SELECT Max(IDTema)" +
        //                    " FROM Sinonimos ";
        //        //Create Command
        //        connection.Close();
        //        connection.Open();
                
        //        command = GetCommand(sqlString, connection);
                    
        //        reader = command.ExecuteReader();
        //        try
        //        {
        //            // Add each object to the objects list.
        //            while (reader.Read())
        //            {
        //                comparar2 = reader.GetInt32(0);
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            comparar2 = 0;
                    
        //        }
        //        resultado = objeto > comparar ? objeto : comparar;
        //        resultado = resultado > comparar2 ? resultado : comparar2;
        //    }
        //    finally
        //    {
        //        if (connection != null)
        //        {
        //            connection.Close();
        //        }
        //    }
        //    return resultado;
        //}

        /// <summary>
        /// Obtienen  el siguiente identificador a utilizar dentro de la tabla de temas,
        /// observando el rango de valores asignados para cada familia (Materia)
        /// </summary>
        /// <param name="materia">Materia de la cual se solicita el siguiente ID</param>
        /// <returns></returns>
        public int ObtenMaximoIDtema(int materia)
        {
            SqlCommand cmd;
            SqlDataReader reader = null;
            DbConnection connection = GetConnection();

            int idForUse = 0;

            try
            {
                connection.Open();

                string sqlCadena = "SELECT MAX(IDTema) + 1 AS Id FROM Temas WHERE Materia = @Materia";

                cmd = new SqlCommand(sqlCadena, (SqlConnection)connection);
                cmd.Parameters.AddWithValue("@Materia", materia);
                reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    reader.Read();

                    idForUse = Convert.ToInt32(reader["Id"]);
                }

                if (materia == 2 && idForUse > 59999)
                    idForUse = -1;
                else if (materia == 4 && idForUse > 89999)
                    idForUse = -1;
                else if (materia == 8 && idForUse > 119999)
                    idForUse = -1;
                else if (materia == 16 && idForUse > 149999)
                    idForUse = -1;
                else if (materia == 32 && idForUse > 179999)
                    idForUse = -1;

                if (idForUse == -1)
                    MessageBox.Show("Se ha alcanzado el límite de temas permitidos para esta materia, favor de contactar con el administrador");
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,TesauroDAC", "Tesauro");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,TesauroDAC", "Tesauro");
            }
            finally
            {
                reader.Close();
                connection.Close();
            }

            return idForUse;
        }

        /// <summary>
        /// Obtiene el siguiente ID a utilizar dentro de la tabla de Ascendente y Sinonimos, la cual incluye:
        /// Sinónimos, Relaciones Próximas, Inclusión Ascendente entre otras.
        /// En estas tablas la columna IdTema representa el identificador clave para la información contenida en estas tablas, 
        /// mientras que la columna IdPadre e IdAscendente hacen referencia al tema (tabla TEMAS) con el cual esta relacionado
        /// </summary>
        /// <param name="tabla"></param>
        /// <returns></returns>
        public int ObtenMaximoIdRelaciones(string tabla)
        {
            SqlCommand cmd;
            SqlDataReader reader = null;
            DbConnection connection = GetConnection();

            int idForUse = 0;

            try
            {
                connection.Open();

                string sqlCadena = "SELECT MAX(IDTema) + 1 AS Id FROM " + tabla;

                cmd = new SqlCommand(sqlCadena, (SqlConnection)connection);
                reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    reader.Read();

                    idForUse = Convert.ToInt32(reader["Id"]);
                }
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,TesauroDAC", "Tesauro");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,TesauroDAC", "Tesauro");
            }
            finally
            {
                reader.Close();
                connection.Close();
            }

            return idForUse;
        }

        public void GeneraNuevoTema(TemaTO Datos)
        {
            DbConnection connection = null;
            string sqlString;

            try
            {
                connection = GetConnection();
                connection.Open();

                sqlString = "INSERT INTO Temas(IDTema, IDPadre, Descripcion, " +
                            " DescripcionStr, Nivel, IDUser, Fecha, Hora, Notas, Observaciones, Materia, Status,idOrigen,idTemaOrigen) " +
                            " VALUES (" + Datos.IDTema + ", " + Datos.IDPadre + ", " +
                            " '" + Datos.Descripcion.Trim() + "', " +
                            " '" + Datos.DescripcionStr.TrimStart() + "', 0," + UsuarioActual + ", SYSDATETIME() , SYSDATETIME() , " +
                            " '" + Datos.Nota + "', '" + Datos.Observaciones + "', " +
                            Datos.Materia + ", 1," + Datos.IdOrigen + "," + Datos.IdTemaOrigen + ")";

                //Create Command
                DbCommand command = GetCommand(sqlString, connection);
                command.ExecuteNonQuery();

                connection.Close();
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,TesauroDAC", "Tesauro");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,TesauroDAC", "Tesauro");
            }
            finally
            {
                connection.Close();
            }
        }

        /// <summary>
        /// Actualiza la materia del tema seleccionado
        /// </summary>
        /// <param name="temaQueActualizar">Tema al cual se hara la actulización</param>
        /// <param name="materia">Nueva materia</param>
        public void ActualizaMateria(TemaTO temaQueActualizar, int materia)
        {
            DbConnection connection = null;
            string sqlString;
            int reader = 0;
            try
            {
                connection = GetConnection();
                connection.Open();
                sqlString = "Update Temas SET Materia = " + materia +
                            " Where IdTema = " + temaQueActualizar.IDTema + " AND Materia = " + temaQueActualizar.Materia;
                DbCommand command = GetCommand(sqlString, connection);
                reader = command.ExecuteNonQuery();
                // Add each object to the objects list.
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,TesauroDAC", "Tesauro");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,TesauroDAC", "Tesauro");
            }
            finally
            {
                connection.Close();
            }
        }
            
        public List<int> ObtenHijos(int id)
        {
            DbConnection connection = GetConnection();
            string sqlString;
            int objeto = 0;
            List<int> resultado = new List<int>();
            try
            {
                connection.Open();
                
                sqlString = "SELECT idTema " +
                            " FROM Temas where idPadre = " + id;
                //Create Command
                DbCommand command = GetCommand(sqlString, connection);
                
                DbDataReader reader = command.ExecuteReader();
                    
                // Add each object to the objects list.
                while (reader.Read())
                {
                    objeto = reader.GetInt32(0);
                    resultado.Add(objeto);
                }
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,TesauroDAC", "Tesauro");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,TesauroDAC", "Tesauro");
            }
            finally
            {
                connection.Close();
            }
            return resultado;
        }
            
        public TemaTO ObtenTema(int id)
        {
            DbConnection connection = GetConnection();
            string sqlString;
            TemaTO objeto = null;
            List<TemaTO> resultado = new List<TemaTO>();
            try
            {
                connection.Open();
                
                sqlString = "SELECT idTema, Descripcion, DescripcionStr, Nivel, IdPadre, IdUser, " +
                            " fecha, hora, notas, observaciones, materia, status,idOrigen " +
                            " FROM Temas where idTema = " + id;
                //Create Command
                DbCommand command = GetCommand(sqlString, connection);
                
                DbDataReader reader = command.ExecuteReader();
                    
                // Add each object to the objects list.
                while (reader.Read())
                {
                    int idTema = reader.GetInt32(0);
                    String descripcion = reader.GetString(1);
                    String descripcionStr = reader.GetString(2);
                    int nivel = reader.GetInt16(3);
                    int idPadre = reader.GetInt32(4);
                    int idUser = reader.GetInt16(5);
                    DateTime fecha = reader.GetDateTime(6);
                    DateTime hora = reader.GetDateTime(7);
                    String notas = reader.GetString(8);
                    String observaciones = reader.GetString(9);
                    int materia = reader.GetInt32(10);
                    int status = reader.GetInt32(11);
                    int idOrigen = reader.GetInt32(12);
                    TemaTO temaActual = new TemaTO(idTema, descripcion,descripcionStr,nivel,
                        idPadre, idUser,fecha,hora, notas,observaciones,materia, status,idOrigen);
                    resultado.Add(temaActual);
                }
                objeto = resultado[0];
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,TesauroDAC", "Tesauro");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,TesauroDAC", "Tesauro");
            }
            finally
            {
                connection.Close();
            }
            return objeto;
        }
            
        public List<TemaTO> ObtenTemas(String busqueda)
        {
            DbConnection connection = GetConnection();
            string sqlString;
            //TemaTO objeto = null;
            List<TemaTO> resultado = new List<TemaTO>();
            try
            {
                connection.Open();
                
                sqlString = "SELECT idTema, Descripcion, DescripcionStr, Nivel, IdPadre, IdUser, " +
                            " fecha, hora, notas, observaciones, materia, status,idOrigen " +
                            " FROM Temas where DescripcionStr LIKE '% " + busqueda + " %'";
                //Create Command
                DbCommand command = GetCommand(sqlString, connection);
                
                DbDataReader reader = command.ExecuteReader();
                    
                // Add each object to the objects list.
                while (reader.Read())
                {
                    int idTema = reader.GetInt32(0);
                    String descripcion = reader.GetString(1);
                    String descripcionStr = reader.GetString(2);
                    int nivel = reader.GetInt16(3);
                    int idPadre = reader.GetInt32(4);
                    int idUser = reader.GetInt16(5);
                    DateTime fecha = reader.GetDateTime(6);
                    DateTime hora = reader.GetDateTime(7);
                    String notas = reader.GetString(8);
                    String observaciones = reader.GetString(9);
                    int materia = reader.GetInt32(10);
                    int status = reader.GetInt32(11);
                    int idOrigen = reader.GetInt32(12);
                    TemaTO temaActual = new TemaTO(idTema, descripcion, descripcionStr, nivel,
                        idPadre, idUser, fecha, hora, notas, observaciones, materia, status,idOrigen);
                    resultado.Add(temaActual);
                }
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,TesauroDAC", "Tesauro");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,TesauroDAC", "Tesauro");
            }
            finally
            {
                connection.Close();
            }
            return resultado;
        }
            
        public String ObtenRuta(TemaTO tema)
        {
            DbConnection connection = GetConnection();
            string sqlString;
            //TemaTO objeto = null;
            List<TemaTO> resultado = new List<TemaTO>();
            try
            {
                connection.Open();
                
                sqlString = "SELECT idTema, Descripcion, DescripcionStr, Nivel, IdPadre, IdUser, " +
                            " fecha, hora, notas, observaciones, materia, status,idOrigen " +
                            " FROM Temas where IdTema = " + tema.IDPadre;
                //Create Command
                DbCommand command = GetCommand(sqlString, connection);
                
                DbDataReader reader = command.ExecuteReader();
                    
                // Add each object to the objects list.
                while (reader.Read())
                {
                    int idTema = reader.GetInt32(0);
                    String descripcion = reader.GetString(1);
                    String descripcionStr = reader.GetString(2);
                    int nivel = reader.GetInt16(3);
                    int idPadre = reader.GetInt32(4);
                    int idUser = reader.GetInt16(5);
                    DateTime fecha = reader.GetDateTime(6);
                    DateTime hora = reader.GetDateTime(7);
                    String notas = reader.GetString(8);
                    String observaciones = reader.GetString(9);
                    int materia = reader.GetInt32(10);
                    int status = reader.GetInt32(11);
                    int idOrigen = reader.GetInt32(12);
                    TemaTO temaActual = new TemaTO(idTema, descripcion, descripcionStr, nivel,
                        idPadre, idUser, fecha, hora, notas, observaciones, materia, status,idOrigen);
                    resultado.Add(temaActual);
                }
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,TesauroDAC", "Tesauro");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,TesauroDAC", "Tesauro");
            }
            finally
            {
                connection.Close();
            }
            if (resultado.Count == 0)
            {
                return String.Empty;
            }
            else
            {
                return ObtenRuta(resultado[0]) + "\\" + resultado[0].Descripcion;
            }
        }

        /// <summary>
        /// Devuelve la descripcion de la materia cuyo identificador se recibe
        /// </summary>
        /// <param name="idMateria">Identificador de la materia</param>
        /// <returns></returns>
        public string ObtenMateria(int idMateria)
        {
            SqlConnection connection = GetConnection();
            String materia = null;
            
            try
            {
                connection.Open();
                
                string sqlString = "SELECT A.idMateria, A.Descripcion " +
                                   " FROM Materias As A where A.idMateria = @IdMateria";
                
                SqlCommand cmd = new SqlCommand(sqlString, connection);
                cmd.Parameters.AddWithValue("@IdMateria", idMateria);
                SqlDataReader reader = cmd.ExecuteReader();
                
                while (reader.Read())
                {
                    materia = reader["Descripcion"].ToString();
                }
            }
            catch (InvalidCastException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,TesauroDAC", "Tesauro");
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,TesauroDAC", "Tesauro");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,TesauroDAC", "Tesauro");
            }
            finally
            {
                connection.Close();
            }
            return materia;
        }
            
        public List<StatusTO> GetStatus()
        {
            DbConnection connection = GetConnection();
            string sqlString;
            List<StatusTO> resultado = new List<StatusTO>();
            try
            {
                connection.Open();
                
                sqlString = "SELECT Id, Descripcion, imagen, Rol " +
                            " FROM Status order by Id ";
                //Create Command
                DbCommand command = GetCommand(sqlString, connection);
                
                DbDataReader reader = command.ExecuteReader();
                    
                // Add each object to the objects list.
                while (reader.Read())
                {
                    StatusTO registro = new StatusTO();
                    int id = reader.GetInt32(0);
                    String descripcion = reader.GetString(1);
                    String imagen = reader.GetString(2);
                    Int32 rol = reader.GetInt32(3);
                    registro.Id = id;
                    registro.Descripcion = descripcion;
                    registro.Imagen = imagen;
                    registro.Rol = rol;
                    resultado.Add(registro);
                }
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,TesauroDAC", "Tesauro");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,TesauroDAC", "Tesauro");
            }
            finally
            {
                connection.Close();
            }
            return resultado;
        }

        #region Observaciones
        
        public long ObtenMaximoObservacion()
        {
            DbConnection connection = GetConnection();
            string sqlString;
            long objeto = 0;
            long resultado = 0;
            try
            {
                connection.Open();
                
                sqlString = "SELECT Max(ID)+1" +
                            " FROM Observaciones ";
                //Create Command
                DbCommand command = GetCommand(sqlString, connection);
                DbDataReader reader = command.ExecuteReader();
                // Add each object to the objects list.
                while (reader.Read())
                {
                    if (reader.IsDBNull(0))
                    {
                        objeto = 0;
                    }
                    else
                    {
                        objeto = reader.GetInt64(0);
                    }
                }
                resultado = objeto;
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,TesauroDAC", "Tesauro");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,TesauroDAC", "Tesauro");
            }
            finally
            {
                connection.Close();
            }
            return resultado;
        }
        
        public void InsertaObservacion(ObservacionTO obs)
        {
            DbConnection connection = null;
            string sqlString;
            
            try
            {
                connection = GetConnection();
                connection.Open();
                
                sqlString = "Insert into Observaciones (ID, Tipo, Texto, IdUser, Hora, IdTema) VALUES (" +
                            obs.Id + ", " + obs.Tipo + ", '" + obs.Texto + "', " + UsuarioActual + ", SYSDATETIME() ," +
                            + obs.IdTema + ")";
                DbCommand command = GetCommand(sqlString, connection);
                command.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,TesauroDAC", "Tesauro");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,TesauroDAC", "Tesauro");
            }
            finally
            {
                connection.Close();
            }
        }
        
        public List<ObservacionTO> ObtenObservaciones(int id)
        {
            DbConnection connection = GetConnection();
            string sqlString;
            List<ObservacionTO> resultado = new List<ObservacionTO>();
            try
            {
                connection.Open();
                
                sqlString = "SELECT A.Id, A.Tipo, A.Texto, A.IdUser, " +
                            " B.Nombre, A.Hora, A.IdTema " +
                            " FROM Observaciones As A, acceso As B Where A.IdUser = B.Id AND A.IdTema = " + id;
                //Create Command
                DbCommand command = GetCommand(sqlString, connection);
                
                DbDataReader reader = command.ExecuteReader();
                
                // Add each object to the objects list.
                while (reader.Read())
                {
                    ObservacionTO registro = new ObservacionTO();
                    Int64 idOb = reader.GetInt64(0);
                    int tipo = reader.GetInt32(1);
                    String descripcion = reader.GetString(2);
                    int idUser = reader.GetInt16(3);
                    String usuario = reader.GetString(4);
                    DateTime hora = reader.GetDateTime(5);
                    Int32 idTema = reader.GetInt32(6);
                    registro.Id = idOb;
                    registro.Tipo = tipo;
                    registro.Texto = descripcion;
                    registro.IdTema = idTema;
                    registro.UserId = idUser;
                    registro.Usuario = usuario;
                    registro.Hora = hora;
                    
                    resultado.Add(registro);
                }
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,TesauroDAC", "Tesauro");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,TesauroDAC", "Tesauro");
            }
            finally
            {
                connection.Close();
            }
            return resultado;
        }
        
        #endregion
        
        #region TematicoIUS
        
        public void SetTemaQueImporta(int idTema, int idMateria, int idTemaQueImporta)
        {
            SqlConnection sqlConne = (SqlConnection)GetConnection();
            SqlDataAdapter dataAdapter;
            SqlCommand cmd;
            cmd = sqlConne.CreateCommand();
            cmd.Connection = sqlConne;
            
            try
            {
                sqlConne.Open();
                
                DataSet dataSet = new DataSet();
                DataRow dr;
                
                string sqlCadena = "SELECT * FROM TematicosIUS WHERE idTema = " + idTema + " AND idMateria = " + idMateria;
                
                dataAdapter = new SqlDataAdapter();
                dataAdapter.SelectCommand = new SqlCommand(sqlCadena, sqlConne);
                
                dataAdapter.Fill(dataSet, "Modulos");
                
                dr = dataSet.Tables[0].Rows[0];
                dr.BeginEdit();
                dr["TemaQueImporta"] = idTemaQueImporta;
                dr["IdTema"] = idTema;
                dr["IdMateria"] = idMateria;
                
                dr.EndEdit();
                
                dataAdapter.UpdateCommand = sqlConne.CreateCommand();
                
                string sSql = "UPDATE TematicosIUS " +
                              "SET TemaQueImporta = @TemaQueImporta " +
                              " WHERE IdTema = @IdTema AND IdMateria = @IdMateria";
                
                dataAdapter.UpdateCommand.CommandText = sSql;
                
                dataAdapter.UpdateCommand.Parameters.Add("@TemaQueImporta", SqlDbType.Int, 0, "TemaQueImporta");
                dataAdapter.UpdateCommand.Parameters.Add("@IdTema", SqlDbType.Int, 0, "IdTema");
                dataAdapter.UpdateCommand.Parameters.Add("@IdMateria", SqlDbType.Int, 0, "IdMateria");
                
                dataAdapter.Update(dataSet, "Modulos");
                dataSet.Dispose();
                dataAdapter.Dispose();
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,TesauroDAC", "Tesauro");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,TesauroDAC", "Tesauro");
            }
            finally
            {
                sqlConne.Close();
            }
        }
        
        #endregion
        
        #region Bitacora
        
        /// <summary>
        /// Ingresa un registro a la base de datos con la actividad que cada usuario reliza
        /// </summary>
        /// <param name="idTema">Tema al que esta relacionada la acción</param>
        /// <param name="idSeccion">Indica en que apartado se realizo la accion, Temas, tesis,expresiones de consulta ...</param>
        /// <param name="idMovimiento">Indica si se agrego, modifico, elimino o importo un elemento</param>
        /// <param name="idUsuario">Usuario que genera la accion</param>
        /// <param name="edoAnterior">Estado del elemento antes de la accion realizada</param>
        /// <param name="edoActual">Estado del elemento despues de realizada la accion</param>
        public void SetBitacora(int idTema, int idSeccion, int idMovimiento, int idUsuario, String edoAnterior, String edoActual, int idMateria)
        {
            SqlConnection connection = GetConnection();
            SqlDataAdapter dataAdapter;
            SqlCommand cmd;
            cmd = connection.CreateCommand();
            cmd.Connection = connection;
            
            try
            {
                connection.Open();
                
                DataSet dataSet = new DataSet();
                DataRow dr;
                
                string sqlCadena = "SELECT * FROM Bitacora WHERE id = 0";
                
                dataAdapter = new SqlDataAdapter();
                dataAdapter.SelectCommand = new SqlCommand(sqlCadena, connection);
                
                dataAdapter.Fill(dataSet, "Bitacora");
                
                dr = dataSet.Tables["Bitacora"].NewRow();
                dr["idTema"] = idTema;
                dr["idSeccion"] = idSeccion;
                dr["idMovimiento"] = idMovimiento;
                dr["idusuario"] = idUsuario;
                dr["edoAnterior"] = edoAnterior;
                dr["edoActual"] = edoActual;
                dr["usuario"] = Environment.MachineName;
                dr["idMateria"] = idMateria;
                
                dataSet.Tables["Bitacora"].Rows.Add(dr);
                
                dataAdapter.InsertCommand = connection.CreateCommand();
                
                string sSql = "INSERT INTO Bitacora (id,idTema,idSeccion,idMovimiento,idUsuario,edoAnterior,edoActual,fecha,usuario,idMateria) VALUES((SELECT MAX(id) + 1 FROM Bitacora),@idTema,@idSeccion,@idMovimiento,@idUsuario,@edoAnterior,@edoActual,SYSDATETIME(),@Usuario,@idMateria)";
                
                dataAdapter.InsertCommand.CommandText = sSql;
                
                dataAdapter.InsertCommand.Parameters.Add("@idTema", SqlDbType.Int, 0, "idTema");
                dataAdapter.InsertCommand.Parameters.Add("@idSeccion", SqlDbType.Int, 0, "idSeccion");
                dataAdapter.InsertCommand.Parameters.Add("@idMovimiento", SqlDbType.Int, 0, "idMovimiento");
                dataAdapter.InsertCommand.Parameters.Add("@idUsuario", SqlDbType.Int, 0, "idUsuario");
                dataAdapter.InsertCommand.Parameters.Add("@edoAnterior", SqlDbType.NVarChar, 0, "edoAnterior");
                dataAdapter.InsertCommand.Parameters.Add("@edoActual", SqlDbType.NVarChar, 0, "edoActual");
                dataAdapter.InsertCommand.Parameters.Add("@Usuario", SqlDbType.NVarChar, 0, "Usuario");
                dataAdapter.InsertCommand.Parameters.Add("@idMateria", SqlDbType.Int, 0, "idMateria");
                
                dataAdapter.Update(dataSet, "Bitacora");
                dataSet.Dispose();
                dataAdapter.Dispose();
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,TesauroDAC", "Tesauro");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,TesauroDAC", "Tesauro");
            }
            finally
            {
                connection.Close();
            }
        }
    
        #endregion
    }
}