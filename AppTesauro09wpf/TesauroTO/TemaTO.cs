using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;

namespace TesauroTO
{
    public class TemaTO : INotifyPropertyChanged
    {
        static readonly TemaTO DummyChild = new TemaTO();

        TemaTO parent;// {get;set;};

        bool isExpanded;
        bool isSelected;

        private TemaTO parentItem;

        private int _IDTema;

        public int IDTema
        {
            get
            {
                return _IDTema;
            }
            set
            {
                _IDTema = value;
            }
        }

        private string descripcion;

        public string Descripcion
        {
            get
            {
                return descripcion;
            }
            set
            {
                descripcion = value;
                this.OnPropertyChanged("Descripcion");
            }

            
        }

        private string _DescripcionStr = "";

        public string DescripcionStr
        {
            get
            {
                return _DescripcionStr;
            }
            set
            {
                _DescripcionStr = value;
            }
        }

        private int _Nivel;

        public int Nivel
        {
            get
            {
                return _Nivel;
            }
            set
            {
                _Nivel = value;
            }
        }

        private int _IDPadre;

        public int IDPadre
        {
            get
            {
                return _IDPadre;
            }
            set
            {
                _IDPadre = value;
            }
        }

        private int _IDUser;

        public int IDUser
        {
            get
            {
                return _IDUser;
            }
            set
            {
                _IDUser = value;
            }
        }

        private DateTime _Fecha;

        public DateTime Fecha
        {
            get
            {
                return _Fecha;
            }
            set
            {
                _Fecha = value;
            }
        }

        private DateTime _Hora;

        public DateTime Hora
        {
            get
            {
                return _Hora;
            }
            set
            {
                _Hora = value;
            }
        }

        private String _Nota = "";

        public String Nota
        {
            get
            {
                return _Nota;
            }
            set
            {
                _Nota = value;
            }
        }

        private String _Observaciones = "";

        public String Observaciones
        {
            get
            {
                return _Observaciones;
            }
            set
            {
                _Observaciones = value;
            }
        }
        
        private int _Materia;

        public int Materia
        {
            get
            {
                return _Materia;
            }
            set
            {
                _Materia = value;
            }
        }

        private int _Status;

        public int Status
        {
            get
            {
                return _Status;
            }
            set
            {
                _Status = value;
            }
        }

        private int idOrigen;

        public int IdOrigen
        {
            get
            {
                return idOrigen;
            }
            set
            {
                idOrigen = value;
            }
        }

        private int idTemaOrigen = 0;

        public int IdTemaOrigen
        {
            get
            {
                return idTemaOrigen;
            }
            set
            {
                idTemaOrigen = value;
            }
        }

        private int tesisRelacionadas;

        public int TesisRelacionadas
        {
            get
            {
                return tesisRelacionadas;
            }
            set
            {
                if (value != null)
                {
                    tesisRelacionadas = value;
                    this.OnPropertyChanged("TesisRelacionadas");
                }
                else
                {
                    tesisRelacionadas = 0;
                }
            }
        }

        private ObservableCollection<TemaTO> subtemas;

        public ObservableCollection<TemaTO> SubTemas
        {
            get
            {
                if (this.subtemas == null)
                {
                    this.subtemas = new ObservableCollection<TemaTO>();
                }
                return this.subtemas;
            }
            set
            {
                this.subtemas = value;
            }
        }

        public void AddSubTema(TemaTO tema)
        {
            subtemas.Add(tema);
        }

        public TemaTO(TemaTO parent)
        {
            this.parentItem = parent;
        }

        #region Constructores

        public TemaTO()
        {
        }

        public TemaTO(TemaTO parent, bool lazyLoadChildren)
        {
            this.parent = parent;
            
            subtemas = new ObservableCollection<TemaTO>();

            if (lazyLoadChildren)
                subtemas.Add(DummyChild);
        }

        public TemaTO(int idTema, string descripcion, string descripcionStr,
            int nivel, int idPadre, int idUser, DateTime fecha, DateTime hora,
            String nota, String observaciones, int materia, int status, int idOrigen)
        {
            _IDTema = idTema;
            this.descripcion = descripcion;
            _DescripcionStr = descripcionStr;
            _Nivel = nivel;
            _IDPadre = idPadre;
            _IDUser = idUser;
            _Fecha = fecha;
            _Hora = hora;
            _Nota = nota;
            _Observaciones = observaciones;
            _Materia = materia;
            _Status = status;
            this.idOrigen = idOrigen;
        }

        public TemaTO(int idTema, string descripcion, string descripcionStr,
            int nivel, int idPadre, int idUser, DateTime fecha, DateTime hora,
            String nota, String observaciones, int materia, int status, int idOrigen, int idTemaOrigen, int tesisRelacionadas)
        {
            _IDTema = idTema;
            this.descripcion = descripcion;
            _DescripcionStr = descripcionStr;
            _Nivel = nivel;
            _IDPadre = idPadre;
            _IDUser = idUser;
            _Fecha = fecha;
            _Hora = hora;
            _Nota = nota;
            _Observaciones = observaciones;
            _Materia = materia;
            _Status = status;
            this.idOrigen = idOrigen;
            this.idTemaOrigen = idTemaOrigen;
            this.tesisRelacionadas = tesisRelacionadas;
        }

        #endregion

        #region HasLoadedChildren

        /// <summary>
        /// Returns true if this object's Children have not yet been populated.
        /// </summary>
        public bool HasDummyChild
        {
            get
            {
                return this.SubTemas.Count == 1 && this.SubTemas[0] == DummyChild;
            }
        }

        #endregion // HasLoadedChildren

        #region Parent

        public TemaTO Parent
        {
            get
            {
                return parent;
            }
            set
            {
                parent = value;
            }
        }

        #endregion

        #region IsExpanded

        /// <summary>
        /// Gets/sets whether the TreeViewItem 
        /// associated with this object is expanded.
        /// </summary>
        public bool IsExpanded
        {
            get
            {
                return isExpanded;
            }
            set
            {
                if (value != isExpanded)
                {
                    isExpanded = value;
                    this.OnPropertyChanged("IsExpanded");
                }
                // Expand all the way up to the root.
                if (isExpanded && parent != null)
                    parent.IsExpanded = true;
                // Lazy load the child items, if necessary.
                if (this.HasDummyChild)
                {
                    this.SubTemas.Remove(DummyChild);
                    this.LoadChildren();
                }
            }
        }

        #endregion // IsExpanded

        #region LoadChildren

        /// <summary>
        /// Invoked when the child items need to be loaded on demand.
        /// Subclasses can override this to populate the Children collection.
        /// </summary>
        protected virtual void LoadChildren()
        {
            //SubTemas = this.GetTemas(this, this.Materia);
            foreach (var item in this.GetTemas(this, this.Materia))
                SubTemas.Add(item);
        }

        #endregion // LoadChildren

        #region AddRemove Children

        

        protected virtual void AddSubtema(TemaTO child)
        {
            child.Parent = this;
            SubTemas.Add(child);
        }

        #endregion

        public virtual void RemoveSubTema(TemaTO child)
        {
            SubTemas.Remove(child);
        }

        #region IsSelected

        /// <summary>
        /// Gets/sets whether the TreeViewItem 
        /// associated with this object is selected.
        /// </summary>
        public bool IsSelected
        {
            get
            {
                return isSelected;
            }
            set
            {
                if (value != isSelected)
                {
                    isSelected = value;
                    this.OnPropertyChanged("IsSelected");
                }
            }
        }

        #endregion // IsSelected


        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        
        #endregion // INotifyPropertyChanged Members
        
        public ObservableCollection<TemaTO> GetTemas(TemaTO parentModule, int idMateria)
        {
            SqlConnection sqlConne = (SqlConnection)this.GetConnection();

            ObservableCollection<TemaTO> modulos = new ObservableCollection<TemaTO>();

            try
            {
                sqlConne.Open();

                string sqlCadena = "SELECT *, (SELECT COUNT(idTEma) FROM TemasTesis T WHERE T.idTema = Temas.Idtema and T.idMateria = Temas.Materia ) Total " +
                                   "FROM Temas WHERE IdPadre = @padre AND Materia = @IdMateria ORDER BY DescripcionStr ";
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
            catch (SqlException )
            {
                //MessageBox.Show("Error ({0}) : {1}" + sql.Source + sql.Message, "Error Interno");
            }
            catch (Exception )
            {
                // MessageBox.Show("Error ({0}) : {1}" + ex.Source + ex.Message, "Error Interno");
            }
            finally
            {
                sqlConne.Close();
            }
            return modulos;
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