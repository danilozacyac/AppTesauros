using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Windows;
using Login.Utils;

namespace AppTesauro09wpf.Estadisticas
{
    public class EstadisticaMainViewModel
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

        /// <summary>
        /// Devuelve las estadisticas de el usuario que esta loggeado
        /// </summary>
        /// <returns></returns>
        public EstadisticaViewModel GetStatsPorUsuario()
        {
            EstadisticaViewModel estadistica = new EstadisticaViewModel();
            estadistica.IdUsuario = UserStatus.IdActivo;

            SqlConnection sqlConne = (SqlConnection)this.GetConnection();

            try
            {
                sqlConne.Open();

                string sqlCadena = "select * from acceso where id = @idUsuario ";

                SqlCommand cmd = new SqlCommand(sqlCadena, sqlConne);
                SqlParameter name = cmd.Parameters.Add("@idUsuario", SqlDbType.Int, 0);
                name.Value = UserStatus.IdActivo;
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        estadistica.Usuario = reader["nombre"].ToString();
                    }
                }
                estadistica.Estadisticas = this.GetEstadisticas(1, UserStatus.IdActivo);
                estadistica.Totales = this.GetEstadisticas(2, UserStatus.IdActivo);
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
            
            return estadistica;
        }


        /// <summary>
        /// Devuelve las estadisticas de cada uno de los usuarios que colaboran en el proyecto
        /// </summary>
        /// <returns></returns>
        public List<EstadisticaViewModel> GetStatsGenerales()
        {
            List<EstadisticaViewModel> estadisticas = new List<EstadisticaViewModel>();
            

            SqlConnection sqlConne = (SqlConnection)this.GetConnection();

            try
            {
                sqlConne.Open();

                string sqlCadena = "select * from acceso where rol > 0 order by id";

                SqlCommand cmd = new SqlCommand(sqlCadena, sqlConne);
                SqlParameter name = cmd.Parameters.Add("@idUsuario", SqlDbType.Int, 0);
                name.Value = UserStatus.IdActivo;
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        EstadisticaViewModel estadistica = new EstadisticaViewModel();
                        estadistica.IdUsuario = Convert.ToInt32(reader["id"]);
                        estadistica.Usuario = reader["nombre"].ToString();
                        estadistica.Estadisticas = this.GetEstadisticas(1, estadistica.IdUsuario);
                        estadistica.Totales = this.GetEstadisticas(2, estadistica.IdUsuario);

                        estadisticas.Add(estadistica);
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

            return estadisticas;
        }


        /// <summary>
        /// Devuelve una lista con las estadísticas de cada seccion por usuario
        /// </summary>
        /// <param name="tipo">Periodo de tiempo de las estadisticas solicitadas. 1- Diarias   2- Totales</param>
        /// <returns></returns>
        private List<EstadisticasBitacora> GetEstadisticas( int tipo,int idUsuario)
        {
            List<EstadisticasBitacora> estadisticas = new List<EstadisticasBitacora>();
            SqlConnection sqlConne = (SqlConnection)this.GetConnection();

            int idSeccion = 1;
            while (idSeccion < 6)
            {
                try
                {
                    sqlConne.Open();

                    string dateCondition = "";

                    if (tipo == 1)
                        dateCondition += " and fecha >= '" + DateTime.Now.Year + ((DateTime.Now.Month < 10) ? "-0" : "-") + DateTime.Now.Month + ((DateTime.Now.Day < 10) ? "-0" : "-") + DateTime.Now.Day + "T00:00:00.000' " +
                                         " AND fecha <= '" + DateTime.Now.Year + ((DateTime.Now.Month < 10) ? "-0" : "-") + DateTime.Now.Month + ((DateTime.Now.Day < 10) ? "-0" : "-") + DateTime.Now.Day + "T23:59:59.000' ";
                    else if (tipo == 2)
                        dateCondition += "and fecha >= '2013-06-11T00:00:00.000' " +
                                         "AND fecha <= '" + DateTime.Now.Year + ((DateTime.Now.Month < 10) ? "-0" : "-") + DateTime.Now.Month + ((DateTime.Now.Day < 10) ? "-0" : "-") + DateTime.Now.Day + "T23:59:59.000' ";

                    EstadisticasBitacora stat = new EstadisticasBitacora();

                    stat.Agregados = this.GetQueryNumberOfRowsCount("SELECT COUNT(edoActual) FROM Bitacora Where idUsuario = @idUsuario and idSeccion = @idSeccion and idMovimiento = @idMovimiento " + dateCondition, idSeccion, idUsuario,1);
                    stat.Modificados = this.GetQueryNumberOfRowsCount("SELECT COUNT(idUsuario) FROM Bitacora Where idUsuario = @idUsuario and idSeccion = @idSeccion and idMovimiento = @idMovimiento " + dateCondition, idSeccion, idUsuario,2);
                    stat.Eliminados = this.GetQueryNumberOfRowsCount("SELECT COUNT(edoAnterior) FROM Bitacora Where idUsuario = @idUsuario and idSeccion = @idSeccion and idMovimiento = @idMovimiento " + dateCondition, idSeccion, idUsuario,4);
                    stat.Importados = this.GetQueryNumberOfRowsCount("SELECT COUNT(idUsuario) FROM Bitacora Where idUsuario = @idUsuario and idSeccion = @idSeccion and idMovimiento = @idMovimiento " + dateCondition, idSeccion, idUsuario,8);

                    switch (idSeccion)
                    {
                        case 1:
                            stat.Elemento = "Temas";
                            break;
                        case 2:
                            stat.Elemento = "Tesis";
                            break;
                        case 3:
                            stat.Elemento = "Expresiones de Consulta";
                            break;
                        case 4:
                            stat.Elemento = "Sinónimos";
                            break;
                        case 5:
                            stat.Elemento = "Relaciones Proximas";
                            break;
                    }

                    if (tipo == 2)
                        stat.Promedio = stat.Agregados / this.GetDiasTrabajados(idUsuario);

                    estadisticas.Add(stat);
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
                idSeccion++;
            }
            return estadisticas;
        }

        private int GetDiasTrabajados(int idUsuario)
        {
            SqlConnection sqlConne = (SqlConnection)this.GetConnection();

            int rowNumber = 0;

            try
            {
                sqlConne.Open();

                string sqlCadena = "select distinct CONVERT(VARCHAR(10),fecha,111),COUNT(CONVERT(VARCHAR(10),fecha,111)) Dias " +
                                   " from bitacora where idusuario = @idUsuario group by idusuario,fecha ";

                SqlCommand cmd = new SqlCommand(sqlCadena, sqlConne);
                SqlParameter user = cmd.Parameters.Add("@idUsuario", SqlDbType.Int, 0);
                user.Value = idUsuario;
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    rowNumber = reader.Cast<object>().Count(); 
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
            
            return rowNumber;
        }

        private int GetQueryNumberOfRowsCount(String sqlCadena,int idSeccion,int idUsuario,int idmovimiento)
        {
            SqlConnection sqlConne = (SqlConnection)this.GetConnection();

            if (idSeccion == 1 || idSeccion == 2)
                if (idmovimiento == 4)
                    sqlCadena += " GROUP BY edoAnterior";
                else
                    sqlCadena += " GROUP BY edoActual";
            else
                sqlCadena += " group by fecha,idUsuario";
            int rowNumber = 0;

            try
            {
                sqlConne.Open();

                SqlCommand cmd = new SqlCommand(sqlCadena, sqlConne);
                
                SqlParameter user = cmd.Parameters.Add("@idUsuario", SqlDbType.Int, 0);
                user.Value = idUsuario;
                SqlParameter seccion = cmd.Parameters.Add("@idSeccion", SqlDbType.Int, 0);
                seccion.Value = idSeccion;
                SqlParameter movimiento = cmd.Parameters.Add("@idMovimiento", SqlDbType.Int, 0);
                movimiento.Value = idmovimiento;
                
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        rowNumber++;
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

            return rowNumber;
        }


        public List<KeyValuePair<String,int>> GetTesisPorMateria()
        {
            SqlConnection sqlConne = (SqlConnection)this.GetConnection();

            List<KeyValuePair<String, int>> valuesPairs = new List<KeyValuePair<string, int>>();

            int penal = 0;
            int civil = 0;
            int admin = 0;
            int labor = 0;
            int comun = 0;

            try
            {
                sqlConne.Open();

                string sqlCadena = "select edoActual,COUNT(edoActual),idMateria FROM Bitacora " +
                                " WHERE idMovimiento = 1 and idSeccion = 2 " +
                                " GROUP BY edoActual,idMateria " +
                                " ORDER BY COUNT(edoActual) desc";

                SqlCommand cmd = new SqlCommand(sqlCadena, sqlConne);
                
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        if (reader["idMateria"] != DBNull.Value)
                        {
                            switch (Convert.ToInt32(reader["idMateria"]))
                            {
                                case 2:
                                    penal++;
                                    break;
                                case 4:
                                    civil++;
                                    break;
                                case 8:
                                    admin++;
                                    break;
                                case 16:
                                    labor++;
                                    break;
                                case 32:
                                    comun++;
                                    break;
                            }
                        }
                        
                    }

                    valuesPairs.Add(new KeyValuePair<string,int>("Penal",penal));
                    valuesPairs.Add(new KeyValuePair<string,int>("Civil",civil));
                    valuesPairs.Add(new KeyValuePair<string,int>("Administrativa",admin));
                    valuesPairs.Add(new KeyValuePair<string,int>("Laboral",labor));
                    valuesPairs.Add(new KeyValuePair<string,int>("Común",comun));
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

            return valuesPairs;
        }


    }
}