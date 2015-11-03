using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using ScjnUtilities;
using TesauroTO;
using mx.gob.scjn.ius_common.TO;

namespace TesauroUtilities
{
    public class RelTemasTesisDac
    {

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


        /// <summary>
        /// Establece una nueva relación entre un tema y una tesis
        /// </summary>
        /// <param name="tema"></param>
        /// <param name="tesis"></param>
        public void NuevaRelacionTemaTesis(TemaTO tema, TesisTO tesis)
        {
            SqlConnection connection = new SqlConnection();
            SqlDataAdapter dataAdapter;

            DataSet dataSet = new DataSet();
            DataRow dr;

            string sqlCadena = "SELECT * FROM TemasTesis WHERE IdMateria = 0";

            connection = GetConnection();

            try
            {

                dataAdapter = new SqlDataAdapter();
                dataAdapter.SelectCommand = new SqlCommand(sqlCadena, connection);

                dataAdapter.Fill(dataSet, "Relacion");

                dr = dataSet.Tables["Relacion"].NewRow();
                dr["idMateria"] = tema.Materia;
                dr["IUS"] = tesis.Ius;
                dr["IdTema"] = tema.IDTema;
                dr["ConsecIndx"] = tesis.Consec;

                dataSet.Tables["Relacion"].Rows.Add(dr);

                dataAdapter.InsertCommand = connection.CreateCommand();
                dataAdapter.InsertCommand.CommandText = "INSERT INTO TemasTesis(IdMateria,IUS,IdTema,ConsecIndx)" +
                                                        " VALUES(@IdMateria,@IUS,@IdTema,@ConsecIndx)";

                dataAdapter.InsertCommand.Parameters.Add("@IdMateria", SqlDbType.Int, 0, "IdMateria");
                dataAdapter.InsertCommand.Parameters.Add("@IUS", SqlDbType.Int, 0, "IUS");
                dataAdapter.InsertCommand.Parameters.Add("@IdTema", SqlDbType.Int, 0, "IdTema");
                dataAdapter.InsertCommand.Parameters.Add("@ConsecIndx", SqlDbType.Int, 0, "ConsecIndx");

                dataAdapter.Update(dataSet, "Relacion");

                dataSet.Dispose();
                dataAdapter.Dispose();
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,RelTemasTesisDac", "Tesauro");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,RelTemasTesisDac", "Tesauro");
            }
            finally
            {
                connection.Close();
            }
        }

        /// <summary>
        /// Devuelve las tesis relacionadas al tema seleccionado
        /// </summary>
        /// <param name="tema">Tema seleccionado en el árbol de materias</param>
        /// <returns></returns>
        public List<int> ObtenTesisRelacionadasPorTema(TemaTO tema)
        {
            SqlConnection connection = GetConnection();
            List<int> relaciones = new List<int>();

            try
            {
                connection.Open();

                string sqlCadena = "SELECT * FROM TemasTesis WHERE idTema = @idTema AND idMateria = @idMateria order by ius";
                SqlCommand cmd = new SqlCommand(sqlCadena, connection);
                cmd.Parameters.AddWithValue("@idTema", tema.IDTema);
                cmd.Parameters.AddWithValue("@idMateria", tema.Materia);
                
                
                //SqlParameter idTema = cmd.Parameters.Add("@idTema", SqlDbType.Int, 0);
                //idTema.Value = tema.IDTema;
                //SqlParameter idMateria = cmd.Parameters.Add("@idMateria", SqlDbType.Int, 0);
                //idMateria.Value = tema.Materia;

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                        relaciones.Add(reader.GetInt32(1));
                }
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,RelTemasTesisDac", "Tesauro");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,RelTemasTesisDac", "Tesauro");
            }
            finally
            {
                connection.Close();
            }

            return relaciones;
        }

        /// <summary>
        /// Elimina una relación existente entre tema y tesis
        /// </summary>
        /// <param name="tema"></param>
        /// <param name="tesis">Número de registro ius cuya relación con este tema será eliminada</param>
        public void RemueveRelacionTemaTesis(TemaTO tema, int tesis)
        {
            SqlConnection connection = GetConnection();
            SqlCommand cmd;

            cmd = connection.CreateCommand();
            cmd.Connection = connection;

            try
            {
                connection.Open();

                cmd.CommandText = "DELETE FROM TemasTesis WHERE idTema = " + tema.IDTema + " AND IUS = " + tesis + " AND idMateria = " + tema.Materia;
                cmd.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,RelTemasTesisDac", "Tesauro");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,RelTemasTesisDac", "Tesauro");
            }
            finally
            {
                connection.Close();
            }
        }

        /// <summary>
        /// Elimina todas las relaciones con números de registros IUS de un tema
        /// </summary>
        /// <param name="temas"></param>
        public void RemueveRelacionTemaTesis(TemaTO tema)
        {
            SqlConnection connection = GetConnection();
            SqlCommand cmd;

            cmd = connection.CreateCommand();
            cmd.Connection = connection;

            try
            {
                connection.Open();

                cmd.CommandText = "DELETE FROM TemasTesis WHERE idTema = " + tema.IDTema + " AND idMateria = " + tema.Materia;
                cmd.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,RelTemasTesisDac", "Tesauro");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,RelTemasTesisDac", "Tesauro");
            }
            finally
            {
                connection.Close();
            }
        }

        /// <summary>
        /// Verifica que la tesis señalada haya sido relacionada al menos con un tema dentro de la 
        /// materia que se esta verificando
        /// </summary>
        /// <param name="registroIus">Registro digital de la tesis</param>
        /// <param name="idMateria">MAteria en la que se busca la relación</param>
        /// <returns></returns>
        public int VerificaExistenciaRelacion(String registroIus, int idMateria)
        {
            SqlConnection connection = GetConnection();

            int rowCount = 0;
            try
            {
                connection.Open();

                string sqlCadena = "SELECT * FROM TemasTesis WHERE IUS = @IUS and idMateria = @idMateria";
                SqlCommand cmd = new SqlCommand(sqlCadena, connection);
                SqlParameter ius = cmd.Parameters.Add("@IUS", SqlDbType.Int, 0);
                ius.Value = Convert.ToInt32(registroIus);
                SqlParameter materia = cmd.Parameters.Add("@idMateria", SqlDbType.Int, 0);
                materia.Value = idMateria;

                SqlDataReader reader = cmd.ExecuteReader();



                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        rowCount++;
                    }
                }
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,RelTemasTesisDac", "Tesauro");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,RelTemasTesisDac", "Tesauro");
            }
            finally
            {
                connection.Close();
            }

            return rowCount;
        }



    }
}
