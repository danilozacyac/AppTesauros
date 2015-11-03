using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows;
using Login.Utils;
using ScjnUtilities;
using SeguridadTesauro.TO;
using TesauroTO;
using TesauroUtilities;

namespace AppTesauro09wpf.UserControl
{
    public class TemaToViewModel
    {
        public ObservableCollection<TemaTO> Temas { get; set; }

        private readonly string textoBuscado;

        private static ObservableCollection<TemaTO> tematico;

        private ObservableCollection<TemaTO> temasEnLista = new ObservableCollection<TemaTO>();

        public static ObservableCollection<TemaTO> Tematico
        {
            get
            {
                if (tematico == null)
                    tematico = new TemaToViewModel().GetTemasRaiz(null);

                return tematico;
            }
        }

        public TemaToViewModel()
        {
            Temas = this.GetTemasRaiz(null);
        }

        public TemaToViewModel(String textoBuscado)
        {
            this.textoBuscado = textoBuscado;
            Temas = this.GetTemasBusqueda(null);
        }

        public ObservableCollection<TemaTO> GetTemasRaiz(TemaTO parentModule)
        {
            SqlConnection sqlConne = this.GetConnection();

            ObservableCollection<TemaTO> modulos = new ObservableCollection<TemaTO>();

            foreach (MateriaTO idMateria in UserStatus.MateriasUser)
            {
                try
                {
                    sqlConne.Open();

                    string sqlCadena = "SELECT *, (SELECT COUNT(idTEma) FROM TemasTesis T WHERE T.idTema = Temas.Idtema and T.idMateria = Temas.Materia ) Total " +
                                       "FROM Temas WHERE IdPadre = 0 AND IdTema <> -4 AND Materia = @Materia ORDER BY DescripcionStr ";
                    SqlCommand cmd = new SqlCommand(sqlCadena, sqlConne);
                    SqlParameter materia = cmd.Parameters.Add("@Materia", SqlDbType.Int, 0);
                    materia.Value = idMateria.Id;
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            TemaTO tema;

                            if (parentModule != null)
                                tema = new TemaTO(parentModule, true);
                            else
                                tema = new TemaTO(null, true);

                            tema.Materia = Convert.ToInt32(reader["Materia"]);
                            tema.IDTema = Convert.ToInt32(reader["idTema"]);
                            tema.IDPadre = Convert.ToInt32(reader["IDPadre"]);
                            tema.Nivel = Convert.ToInt32(reader["Nivel"]);
                            tema.Descripcion = reader["Descripcion"].ToString();
                            tema.IdOrigen = Convert.ToInt32(reader["IdOrigen"]);
                            tema.IdTemaOrigen = Convert.ToInt32(reader["IdTemaOrigen"]);
                            tema.TesisRelacionadas = Convert.ToInt32(reader["Total"]);

                            modulos.Add(tema);
                        }
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
                    sqlConne.Close();
                }
            }
            return modulos;
        }

        //public ObservableCollection<TemaTO> GetTemas(TemaTO parentModule, int idMateria)
        //{
        //    SqlConnection sqlConne = (SqlConnection)this.GetConnection();

        //    ObservableCollection<TemaTO> modulos = new ObservableCollection<TemaTO>();
            
        //    try
        //    {
        //        sqlConne.Open();

        //        string sqlCadena = "SELECT *, (SELECT COUNT(idTEma) FROM TemasTesis T WHERE T.idTema = Temas.Idtema and T.idMateria = Temas.Materia ) Total " +
        //                           "FROM Temas WHERE IdPadre = @padre AND Materia = @IdMateria ORDER BY DescripcionStr ";
        //        SqlCommand cmd = new SqlCommand(sqlCadena, sqlConne);
        //        SqlParameter name = cmd.Parameters.Add("@padre", SqlDbType.Int, 0);
        //        name.Value = (parentModule != null) ? parentModule.IDTema : 0;
        //        SqlParameter materia = cmd.Parameters.Add("@IdMateria", SqlDbType.Int, 0);
        //        materia.Value = idMateria;
        //        SqlDataReader reader = cmd.ExecuteReader();

        //        if (reader.HasRows)
        //        {
        //            while (reader.Read())
        //            {
        //                TemaTO tema;

        //                if (parentModule != null)
        //                    tema = new TemaTO(parentModule,true);
        //                else
        //                    tema = new TemaTO(null,true);

        //                tema.Materia = Convert.ToInt32(reader["Materia"]);
        //                tema.IDTema = Convert.ToInt32(reader["idTema"]);
        //                tema.IDPadre = Convert.ToInt32(reader["IDPadre"]);
        //                tema.Nivel = Convert.ToInt32(reader["Nivel"]);
        //                tema.Descripcion = reader["Descripcion"].ToString();
        //                tema.IdOrigen = Convert.ToInt32(reader["IdOrigen"]);
        //                tema.IdTemaOrigen = Convert.ToInt32(reader["IdTemaOrigen"]);
        //                tema.TesisRelacionadas = Convert.ToInt32(reader["Total"]);

        //                modulos.Add(tema);
        //            }
        //        }
        //    }
        //    catch (SqlException sql)
        //    {
        //        MessageBox.Show("Error ({0}) : {1}" + sql.Source + sql.Message, "Error Interno");
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Error ({0}) : {1}" + ex.Source + ex.Message, "Error Interno");
        //    }
        //    finally
        //    {
        //        sqlConne.Close();
        //    }
        //    return modulos;
        //}

        public ObservableCollection<TemaTO> GetTemasBusqueda(TemaTO parentModule)
        {
            SqlConnection sqlConne = this.GetConnection();

            ObservableCollection<TemaTO> modulos = new ObservableCollection<TemaTO>();

            foreach (MateriaTO idMateria in UserStatus.MateriasUser)
            {
                try
                {
                    sqlConne.Open();

                    string sqlCadena = "SELECT *, (SELECT COUNT(idTEma) FROM TemasTesis T WHERE T.idTema = Temas.Idtema and T.idMateria = Temas.Materia ) Total " +
                                       "FROM Temas WHERE (" + this.ArmaCadenaBusqueda(textoBuscado) + ")  AND Materia = @IdMateria  AND idtema >= 0 and idPadre <> -1 ORDER BY DescripcionStr ";
                    SqlCommand cmd = new SqlCommand(sqlCadena, sqlConne);
                    SqlParameter materia = cmd.Parameters.Add("@IdMateria", SqlDbType.Int, 0);
                    materia.Value = idMateria.Id;
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            TemaTO tema = new TemaTO(null, true);
                            
                            tema.Materia = Convert.ToInt32(reader["Materia"]);
                            tema.IDTema = Convert.ToInt32(reader["idTema"]);
                            tema.IDPadre = Convert.ToInt32(reader["IDPadre"]);
                            tema.Nivel = Convert.ToInt32(reader["Nivel"]);
                            tema.Descripcion = reader["Descripcion"].ToString();
                            tema.IdOrigen = 99;
                            tema.TesisRelacionadas = Convert.ToInt32(reader["Total"]);
                            
                            List<TemaTO> buscaExistaTema = (from n in temasEnLista
                                                            where n.IDTema == tema.IDTema && n.Materia == tema.Materia
                                                            select n).ToList() ;
                            
                            //if (temasEnLista.Contains(tema.IDTema))
                            if (buscaExistaTema != null && buscaExistaTema.Count > 0)
                            {
                            }
                            else
                            {
                                if (tema.IDPadre == 0)
                                {
                                    modulos.Add(tema);
                                    temasEnLista.Add(tema);
                                }
                                else
                                {
                                    List<TemaTO> buscaExistaPadre = (from n in temasEnLista
                                                                     where n.IDTema == tema.IDPadre && n.Materia == tema.Materia
                                                                     select n).ToList();

                                    if (buscaExistaPadre != null && buscaExistaPadre.Count > 0)
                                    //if (temasEnLista.Contains(tema.IDPadre) )
                                    {
                                        foreach (TemaTO tematico in modulos)
                                        {
                                            if (tematico.IDTema == tema.IDPadre)
                                            {
                                                if (tematico.SubTemas == null)
                                                    tema.SubTemas = new ObservableCollection<TemaTO>();
                                                tema.Parent = tematico;

                                                tematico.SubTemas.Add(tema);
                                                temasEnLista.Add(tema);
                                            }
                                            else
                                            {
                                                this.SearchExistingParent(tema, tematico);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        this.SearchParent(tema, modulos);
                                    }
                                }
                            }
                        }
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
                    sqlConne.Close();
                }
            }
            return SortSearch(this.SetParents(modulos));
        }

        private ObservableCollection<TemaTO> GetTemasBusqueda(TemaTO parentModule, String sqlCadenaComplement)
        {
            SqlConnection sqlConne = this.GetConnection();
            ObservableCollection<TemaTO> modulos = new ObservableCollection<TemaTO>();
            foreach (MateriaTO idMateria in UserStatus.MateriasUser)
            {
                try
                {
                    sqlConne.Open();
                    string sqlCadena = "SELECT *, (SELECT COUNT(idTEma) FROM TemasTesis T WHERE T.idTema = Temas.Idtema and T.idMateria = Temas.Materia ) Total " +
                                       "FROM Temas WHERE (" + sqlCadenaComplement + ")  and idPadre <> -1 ORDER BY DescripcionStr ";
                    SqlCommand cmd = new SqlCommand(sqlCadena, sqlConne);
                    SqlParameter materia = cmd.Parameters.Add("@IdMateria", SqlDbType.Int, 0);
                    materia.Value = idMateria.Id;
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            TemaTO tema = new TemaTO(null, true);
                            tema.Materia = Convert.ToInt32(reader["Materia"]);
                            tema.IDTema = Convert.ToInt32(reader["idTema"]);
                            tema.IDPadre = Convert.ToInt32(reader["IDPadre"]);
                            tema.Nivel = Convert.ToInt32(reader["Nivel"]);
                            tema.Descripcion = reader["Descripcion"].ToString();
                            tema.IdOrigen = 99;
                            tema.TesisRelacionadas = Convert.ToInt32(reader["Total"]);

                            List<TemaTO> buscaExistaTema = (from n in temasEnLista
                                                            where n.IDTema == tema.IDTema && n.Materia == tema.Materia
                                                            select n).ToList();

                            //if (temasEnLista.Contains(tema.IDTema))
                            if (buscaExistaTema != null && buscaExistaTema.Count > 0)
                            {
                            }
                            else
                            {
                                if (tema.IDPadre == 0)
                                {
                                    modulos.Add(tema);
                                    temasEnLista.Add(tema);
                                }
                                else
                                {
                                    List<TemaTO> buscaExistaPadre = (from n in temasEnLista
                                                                     where n.IDTema == tema.IDPadre && n.Materia == tema.Materia
                                                                     select n).ToList();

                                    if (buscaExistaPadre != null && buscaExistaPadre.Count > 0)
                                    //if (temasEnLista.Contains(tema.IDPadre) )
                                    {
                                        foreach (TemaTO tematico in modulos)
                                        {
                                            if (tematico.IDTema == tema.IDPadre)
                                            {
                                                if (tematico.SubTemas == null)
                                                    tema.SubTemas = new ObservableCollection<TemaTO>();
                                                tema.Parent = tematico;

                                                tematico.SubTemas.Add(tema);
                                                temasEnLista.Add(tema);
                                            }
                                            else
                                            {
                                                this.SearchExistingParent(tema, tematico);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        this.SearchParent(tema, modulos);
                                    }
                                }
                            }
                        }
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
                    sqlConne.Close();
                }
            }
            return SortSearch(this.SetParents(modulos));
        }

        public ObservableCollection<TemaTO> GetTemasPorIus(TemaTO parentModule)
        {
            SqlConnection sqlConne = this.GetConnection();

            ObservableCollection<TemaTO> modulos = new ObservableCollection<TemaTO>();

            String sqlIusMateria = "";

            foreach (MateriaTO idMateria in UserStatus.MateriasUser)
            {
                try
                {
                    sqlConne.Open();

                    string sqlCadena = "SELECT idMateria,idTema FROM TemasTesis WHERE IUS = @IUS AND " +
                                       " IdMateria = @IdMateria  ORDER BY IUS Asc";
                    SqlCommand cmd = new SqlCommand(sqlCadena, sqlConne);
                    SqlParameter ius = cmd.Parameters.Add("@IUS", SqlDbType.Int, 0);
                    ius.Value = textoBuscado;
                    SqlParameter materia = cmd.Parameters.Add("@IdMateria", SqlDbType.Int, 0);
                    materia.Value = idMateria.Id;
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            sqlIusMateria += " OR (idTema = " + reader["idTema"].ToString() + " AND Materia = " + reader["idMateria"].ToString() + ")";
                        }
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
                    sqlConne.Close();
                }
            }

            if (sqlIusMateria.Length > 2)
            {
                sqlIusMateria = sqlIusMateria.Substring(4);

                modulos = this.GetTemasBusqueda(null, sqlIusMateria);

                return SortSearch(this.SetParents(modulos));
            }
            else
            {
                MessageBox.Show("No existen temas relacionados al número de tesis ingresada");
                return new ObservableCollection<TemaTO>();
            }
        }

        /// <summary>
        /// Ordena alfabéticamente los temas obtenidos
        /// </summary>
        /// <param name="moduloToSort">Listado de Temas a ordenar</param>
        /// <returns></returns>
        private ObservableCollection<TemaTO> SortSearch(ObservableCollection<TemaTO> moduloToSort)
        {

            ObservableCollection<TemaTO> moduloOrdenado = new ObservableCollection<TemaTO>(moduloToSort.OrderBy(i => i.Descripcion));

            foreach (TemaTO tema in moduloOrdenado)
            {
                tema.SubTemas = this.SortSearch(tema.SubTemas);
            }

            return moduloOrdenado;
        }

        /// <summary>
        /// Busca el tema padre del nodo actual dentro de la estructura existente
        /// </summary>
        /// <param name="temaHijo">Subtema</param>
        /// <param name="temaPadre">Tema que se esta buscando</param>
        private void SearchExistingParent(TemaTO temaHijo, TemaTO temaPadre)
        {
            foreach (TemaTO tema in temaPadre.SubTemas)
            {
                if (temaHijo.IDPadre == tema.IDTema)
                {
                    if (tema.SubTemas == null)
                        tema.SubTemas = new ObservableCollection<TemaTO>();
                    temaHijo.Parent = tema;
                    tema.SubTemas.Add(temaHijo);
                    temasEnLista.Add(temaHijo);
                    
                }
                else
                {
                    this.SearchExistingParent(temaHijo, tema);
                }
            }
        }


        /// <summary>
        /// Busca el padre del nodo actual dentro de la base de datos y forma 
        /// la estrucutra que se adicionará a la ya existente
        /// </summary>
        /// <param name="temaHijo">Tema del cual se busca el padre</param>
        /// <param name="modulos">Estructura del arbol existente</param>
        private void SearchParent(TemaTO temaHijo, ObservableCollection<TemaTO> modulos)
        {
            TemaTO temaPadre = this.GetTemaByIdTema(temaHijo.IDPadre, temaHijo.Materia);
            temaPadre.SubTemas = new ObservableCollection<TemaTO>();
            temaHijo.Parent = temaPadre;
            temaPadre.SubTemas.Add(temaHijo);

            temasEnLista.Add(temaHijo);

            List<TemaTO> buscaExistaAbuelo = (from n in temasEnLista
                                             where n.IDTema == temaPadre.IDPadre && n.Materia == temaPadre.Materia
                                             select n).ToList();

            //if (temasEnLista.Contains(temaPadre.IDPadre))
            if (buscaExistaAbuelo != null && buscaExistaAbuelo.Count > 0 && temaPadre.IDPadre != 0)
            {
                foreach (TemaTO tematico in modulos)
                {
                    if (tematico.IDTema == temaPadre.IDPadre)
                    {
                        temaPadre.Parent = tematico;
                        tematico.SubTemas.Add(temaPadre);
                        temasEnLista.Add(temaPadre);

                        break;
                    }
                    //else
                    //{
                    //    this.SearchExistingParent(temaPadre, tematico);
                    //}

                    else
                        GetRecursiveParents(temaPadre, tematico.SubTemas);
                }
            }
            else if (temaPadre.IDPadre == 0)
            {
                modulos.Add(temaPadre);
                temasEnLista.Add(temaPadre);
            }
            else if (temaPadre.IDPadre == -1)
            {
                //Si tiene como padre menos uno no lo agrega porque quiere decir que fue eliminado
            }
            else
                this.SearchParent(temaPadre, modulos);
        }

        /// <summary>
        /// Busqueda top-down del padre del tema, es decir, busca verifica desde el nodo más general hacia el
        /// más particular buscando el nodo padre del tema activo
        /// </summary>
        /// <param name="tema"></param>
        /// <param name="subtemas"></param>
        private void GetRecursiveParents(TemaTO tema, ObservableCollection<TemaTO> subtemas)
        {

            foreach (TemaTO tematico in subtemas)
            {
                if (tematico.IDTema == tema.IDPadre)
                {
                    if (tematico.SubTemas == null)
                        tema.SubTemas = new ObservableCollection<TemaTO>();
                    tema.Parent = tematico;
                    tematico.SubTemas.Add(tema);
                    temasEnLista.Add(tema);
                    break;
                }
                else 
                    GetRecursiveParents(tema, tematico.SubTemas);
            }
        }

        private ObservableCollection<TemaTO> SetParents(ObservableCollection<TemaTO> modulos)
        {
            foreach (TemaTO tema in modulos)
            {
                foreach (TemaTO subtema in tema.SubTemas)
                {
                    subtema.Parent = tema;
                    this.SetParents(subtema.SubTemas);
                }
            }
            return modulos;
        }

        /// <summary>
        /// Obtiene la información general de un tema a partir de su identificador y la materia a la cual pertenece
        /// </summary>
        /// <param name="idTema">Identificador del tema</param>
        /// <param name="idMateria">Materia a la que pertenece el tema</param>
        /// <returns></returns>
        private TemaTO GetTemaByIdTema(int idTema, int idMateria)
        {
            SqlConnection sqlConne = this.GetConnection();

            TemaTO tema = new TemaTO();
            try
            {
                sqlConne.Open();

                string sqlCadena = "SELECT *, (SELECT COUNT(idTEma) FROM TemasTesis T WHERE T.idTema = Temas.Idtema and T.idMateria = Temas.Materia ) Total " +
                                   "FROM Temas WHERE  IdTema = @IdTema AND Materia = @Materia  ORDER BY DescripcionStr ";
                SqlCommand cmd = new SqlCommand(sqlCadena, sqlConne);
                cmd.Parameters.AddWithValue("@IdTema", idTema);
                cmd.Parameters.AddWithValue("@Materia", idMateria);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        tema.Materia = Convert.ToInt32(reader["Materia"]);
                        tema.IDTema = Convert.ToInt32(reader["idTema"]);
                        tema.IDPadre = Convert.ToInt32(reader["IDPadre"]);
                        tema.Nivel = Convert.ToInt32(reader["Nivel"]);
                        tema.Descripcion = reader["Descripcion"].ToString();
                        tema.IdTemaOrigen = Convert.ToInt32(reader["IdTemaOrigen"]);
                        tema.TesisRelacionadas = Convert.ToInt32(reader["Total"]);
                    }
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
                sqlConne.Close();
            }
            return tema;
        }

        private String ArmaCadenaBusqueda(String textoBuscado)
        {
            String[] listadoPalabras = textoBuscado.Split(' ');

            String resultString1 = "'%";
            foreach (string palabra in listadoPalabras)
            {
                if (!Constants.STOPERS.Contains(palabra.Trim().ToLower()))
                    resultString1 += BusquedaUtilities.Normaliza(palabra.Trim()) + "%";
                //resultString += "OR DescripcionStr LIKE '%" + FlowDocumentHighlight.Normaliza( palabra.Trim() ) + "%' ";
            }
            resultString1 += "'";

            String resultString2 = "'%";
            foreach (string palabra in listadoPalabras.Reverse())
            {
                if (!Constants.STOPERS.Contains(palabra.Trim().ToLower()))
                    resultString2 += BusquedaUtilities.Normaliza(palabra.Trim()) + "%";
                //resultString += "OR DescripcionStr LIKE '%" + FlowDocumentHighlight.Normaliza( palabra.Trim() ) + "%' ";
            }
            resultString2 += "'";

            return "DescripcionStr LIKE " + resultString1 + " OR DescripcionStr LIKE " + resultString2; 
        }

        private bool findParent = false;

        public void SearchParentAddSon(ObservableCollection<TemaTO> listaTemas, TemaTO temaHijo)
        {
            if (!findParent)
                foreach (TemaTO temaBuscado in listaTemas)
                {
                    if (temaBuscado.IDTema == temaHijo.Parent.IDTema)
                    {
                        temaBuscado.AddSubTema(temaHijo);
                        findParent = true;
                    }
                    else
                    {
                        this.SearchParentAddSon(temaBuscado.SubTemas, temaHijo);
                    }

                    if (findParent)
                        break;
                }
        }

        public void SearchParentDeleteSon(ObservableCollection<TemaTO> listaTemas, TemaTO temaHijo)
        {
            if (!findParent)
                foreach (TemaTO temaBuscado in listaTemas)
                {
                    if (temaBuscado.IDTema == temaHijo.Parent.IDTema)
                    {
                        temaBuscado.RemoveSubTema(temaHijo);
                        findParent = true;
                    }
                    else
                    {
                        this.SearchParentAddSon(temaBuscado.SubTemas, temaHijo);
                    }

                    if (findParent)
                        break;
                }
        }

        private SqlConnection GetConnection()
        {
            String tipoAplicacion = ConfigurationManager.AppSettings.Get("tipoAplicacion");

            String bdStringSql;

            if (tipoAplicacion.Equals("PRUEBA"))
            {
                bdStringSql = ConfigurationManager.ConnectionStrings["TematicoPrueba"].ConnectionString;
            }
            else
                bdStringSql = ConfigurationManager.ConnectionStrings["Tematico"].ConnectionString;
            SqlConnection realConnection = new SqlConnection(bdStringSql);
            return realConnection;
        }
    }
}