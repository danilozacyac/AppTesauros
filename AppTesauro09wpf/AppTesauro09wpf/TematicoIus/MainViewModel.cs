using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Windows;
using TesauroUtilities;

namespace AppTesauro09wpf.TematicoIus
{
    public class MainViewModel
    {
        private readonly int idMateria;
        private readonly string textoBuscado;
        private List<int> temasEnLista = new List<int>();
        public ObservableCollection<TematicoViewModel> Categories { get; set; }
        private static ObservableCollection<TematicoViewModel> administrativa;
        private static ObservableCollection<TematicoViewModel> penal;
        private static ObservableCollection<TematicoViewModel> civil;
        private static ObservableCollection<TematicoViewModel> laboral;
        private static ObservableCollection<TematicoViewModel> comun;

        public MainViewModel(int idMateria)
        {
            this.idMateria = idMateria;
            Categories = this.Inter();
        }

        public MainViewModel(int idMateria, String textoBuscado)
        {
            this.idMateria = idMateria;
            this.textoBuscado = textoBuscado;
            Categories = this.GetTemasBusqueda(null);
        }

        public ObservableCollection<TematicoViewModel> Inter()
        {
            switch (idMateria)
            {
                case 2:
                    if (penal == null)
                        penal = this.GetTemas(null,idMateria);

                    Categories = penal;

                    break;
                case 4:
                    if (civil == null)
                        civil = this.GetTemas(null, idMateria);

                    Categories = civil;

                    break;
                case 8:
                    if (administrativa == null)
                        administrativa = this.GetTemas(null, idMateria);

                    Categories = administrativa;

                    break;
                case 16:
                    if (laboral == null)
                        laboral = this.GetTemas(null, idMateria);

                    Categories = laboral;

                    break;
                case 32:
                    if (comun == null)
                        comun = this.GetTemas(null, idMateria);

                    Categories = comun;

                    break;
            }



            return Categories;
        }

        public ObservableCollection<TematicoViewModel> GetTemas(TematicoViewModel parentModule, int idMateria)
        {
            SqlConnection sqlConne = (SqlConnection)this.GetConnection();

            ObservableCollection<TematicoViewModel> modulos = new ObservableCollection<TematicoViewModel>();

            try
            {
                sqlConne.Open();

                string sqlCadena = "SELECT * FROM TematicosIus WHERE padre = @padre AND IdMateria = @IdMateria ORDER BY DescripcionStr ";
                SqlCommand cmd = new SqlCommand(sqlCadena, sqlConne);
                SqlParameter name = cmd.Parameters.Add("@padre", SqlDbType.Int, 0);
                name.Value = (parentModule != null) ? parentModule.IDTema : 0;
                SqlParameter materia = cmd.Parameters.Add("@IdMateria", SqlDbType.Int, 0);
                materia.Value = idMateria;
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        TematicoViewModel tema;

                        if (parentModule != null)
                            tema = new TematicoViewModel(parentModule, true);
                        else
                            tema = new TematicoViewModel(null, true);

                        tema.Materia = Convert.ToInt32(reader["idMateria"]);
                        tema.IDTema = Convert.ToInt32(reader["idTema"]);
                        tema.IDPadre = Convert.ToInt32(reader["Padre"]);
                        tema.Nivel = Convert.ToInt32(reader["Nivel"]);
                        tema.Descripcion = reader["Descripcion"].ToString();
                        tema.TemaQueImporta = Convert.ToInt32(reader["TemaQueImporta"]);

                        modulos.Add(tema);
                    }
                }
            }
            catch (SqlException ex)
            {
                //MessageBox.Show("Error ({0}) : {1}" + sql.Source + sql.Message, "Error Interno");
            }
            catch (Exception ex)
            {
                // MessageBox.Show("Error ({0}) : {1}" + ex.Source + ex.Message, "Error Interno");
            }
            finally
            {
                sqlConne.Close();
            }
            return modulos;
        }

        private ObservableCollection<TematicoViewModel> GetTemasBusqueda(TematicoViewModel parentModule)
        {
            
            SqlConnection sqlConne = (SqlConnection)this.GetConnection();

            ObservableCollection<TematicoViewModel> modulos = new ObservableCollection<TematicoViewModel>();

            try
            {

                sqlConne.Open();

                string sqlCadena = "SELECT * FROM TematicosIus WHERE " + this.ArmaCadenaBusqueda(textoBuscado) + " AND IdMateria = @IdMateria  ORDER BY DescripcionStr ";
                SqlCommand cmd = new SqlCommand(sqlCadena, sqlConne);
                SqlParameter materia = cmd.Parameters.Add("@IdMateria", SqlDbType.Int, 0);
                materia.Value = idMateria;
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {

                        TematicoViewModel tema;

                        if (parentModule != null)
                            tema = new TematicoViewModel(parentModule);
                        else
                            tema = new TematicoViewModel(null);

                        tema.Materia = Convert.ToInt32(reader["idMateria"]);
                        tema.IDTema = Convert.ToInt32(reader["idTema"]);
                        tema.IDPadre = Convert.ToInt32(reader["Padre"]);
                        tema.Nivel = Convert.ToInt32(reader["Nivel"]);
                        tema.Descripcion = reader["Descripcion"].ToString();
                        tema.TemaQueImporta = Convert.ToInt32(reader["TemaQueImporta"]);
                        //tema.SubCategories = this.GetModulos(tema);

                        if (tema.IDPadre == 0)
                        {
                            modulos.Add(tema);
                            temasEnLista.Add(tema.IDTema);
                        }
                        else
                        {
                            if (temasEnLista.Contains(tema.IDPadre))
                            {
                                foreach (TematicoViewModel tematico in modulos)
                                {
                                    if (tematico.IDTema == tema.IDPadre)
                                    {
                                        if (tematico.SubTemas == null)
                                            tema.SubTemas = new ObservableCollection<TematicoViewModel>();
                                        tematico.SubTemas.Add(tema);
                                        temasEnLista.Add(tema.IDTema);

                                    }
                                }
                            }
                            else
                            {
                                TematicoViewModel parentTema = this.GetTemaByIdTema(tema.IDPadre);
                                temasEnLista.Add(parentTema.IDTema);
                                parentTema.SubTemas = new ObservableCollection<TematicoViewModel>();
                                parentTema.SubTemas.Add(tema);
                                temasEnLista.Add(tema.IDTema);
                                modulos.Add(parentTema);

                            }
                        }
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

        private TematicoViewModel GetTemaByIdTema(int idTema)
        {
            SqlConnection sqlConne = (SqlConnection)this.GetConnection();

            TematicoViewModel tema = new TematicoViewModel();
            try
            {

                sqlConne.Open();

                string sqlCadena = "SELECT * FROM TematicosIus WHERE  IdTema = @IdTema  ORDER BY DescripcionStr ";
                SqlCommand cmd = new SqlCommand(sqlCadena, sqlConne);
                SqlParameter materia = cmd.Parameters.Add("@IdTema", SqlDbType.Int, 0);
                materia.Value = idTema;
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                       
                        tema.Materia = Convert.ToInt32(reader["idMateria"]);
                        tema.IDTema = Convert.ToInt32(reader["idTema"]);
                        tema.IDPadre = Convert.ToInt32(reader["Padre"]);
                        tema.Nivel = Convert.ToInt32(reader["Nivel"]);
                        tema.Descripcion = reader["Descripcion"].ToString();
                        tema.TemaQueImporta= Convert.ToInt32(reader["TemaQueImporta"]);
                        
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
            return tema;
        }

        private String ArmaCadenaBusqueda(String textoBuscado)
        {
            String resultString = "";

            foreach (string palabra in textoBuscado.Split(' '))
            {
                if(!Constants.STOPERS.Contains(palabra.Trim().ToLower()))
                    resultString = "OR Descripcion LIKE '%" + palabra.Trim() + "%' ";
            }

            return resultString.Substring(2);
        }
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


    }
}
