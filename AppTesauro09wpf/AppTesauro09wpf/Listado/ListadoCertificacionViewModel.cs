using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Windows;
using Login.Utils;

namespace AppTesauro09wpf.Listado
{
    public class ListadoCertificacionViewModel
    {

        private DbConnection GetConnection()
        {
            String tipoAplicacion = ConfigurationManager.AppSettings.Get("tipoAplicacion");

            String bdStringSql;

            if (tipoAplicacion.Equals("PRUEBA"))
            {
                bdStringSql = ConfigurationManager.ConnectionStrings["TematicoPrueba"].ConnectionString;
            }
            else
                bdStringSql = ConfigurationManager.ConnectionStrings["Tematico"].ConnectionString;
            DbConnection realConnection = new SqlConnection(bdStringSql);
            return realConnection;


        }

        public List<ListadoCertificacion> GetTemasTesisPorUsuario(int idMovimiento,int idMateria)
        {
            SqlConnection sqlConne = (SqlConnection)this.GetConnection();

            List<ListadoCertificacion> modulos = new List<ListadoCertificacion>();

            try
            {
                sqlConne.Open();

                string sqlCadena = "SELECT B.idTema,B.edoActual,T.descripcion FROM bitacora B INNER JOIN Temas T ON T.idTema = B.idtema " +
                    " WHERE (idseccion = 2 and idmovimiento = @movimiento AND T.Materia = @materia) AND T.Materia = B.idMateria AND idUsuario = @usuario  ";
                SqlCommand cmd = new SqlCommand(sqlCadena, sqlConne);
                SqlParameter tipoMovimiento = cmd.Parameters.Add("@movimiento", SqlDbType.Int, 0);
                tipoMovimiento.Value = idMovimiento;
                SqlParameter materia = cmd.Parameters.Add("@materia", SqlDbType.Int, 0);
                materia.Value = idMateria;
                SqlParameter idUsuario = cmd.Parameters.Add("@usuario", SqlDbType.Int, 0);
                idUsuario.Value = UserStatus.IdActivo;
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ListadoCertificacion cert = new ListadoCertificacion();
                        cert.Ius = Convert.ToInt32(reader["edoActual"]);
                        cert.IdTema = Convert.ToInt32(reader["idTema"]);
                        cert.Tema = reader["Descripcion"].ToString();

                        modulos.Add(cert);
                    }
                }
            }
            catch (SqlException sql)
            {
                MessageBox.Show("Error ({0}) : {1}" + sql.Source + sql.Message, "Error Interno");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error ({0}) : {1}" + ex.Source + ex.Message, "Error Interno");
            }
            finally
            {
                sqlConne.Close();
            }
            return modulos;
        }


    }
}
