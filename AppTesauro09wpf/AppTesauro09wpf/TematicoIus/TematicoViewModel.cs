using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace AppTesauro09wpf.TematicoIus
{
    public class TematicoViewModel
    {
        static readonly TematicoViewModel DummyChild = new TematicoViewModel();

        TematicoViewModel parent;// {get;set;};

        bool isExpanded;
        bool isSelected;

        private readonly TematicoViewModel parentItem;

        private int idTema;

        

        public int IDTema
        {
            get
            {
                return idTema;
            }
            set
            {
                idTema = value;
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

        private string descripcionStr;

        public string DescripcionStr
        {
            get
            {
                return descripcionStr;
            }
            set
            {
                descripcionStr = value;
            }
        }

        private int nivel;

        public int Nivel
        {
            get
            {
                return nivel;
            }
            set
            {
                nivel = value;
            }
        }

        private int idPadre;

        public int IDPadre
        {
            get
            {
                return idPadre;
            }
            set
            {
                idPadre = value;
            }
        }

        private int idUser;

        public int IDUser
        {
            get
            {
                return idUser;
            }
            set
            {
                idUser = value;
            }
        }

        private DateTime fecha;

        public DateTime Fecha
        {
            get
            {
                return fecha;
            }
            set
            {
                fecha = value;
            }
        }

        private DateTime hora;

        public DateTime Hora
        {
            get
            {
                return hora;
            }
            set
            {
                hora = value;
            }
        }

        private String nota;

        public String Nota
        {
            get
            {
                return nota;
            }
            set
            {
                nota = value;
            }
        }

        private String observaciones;

        public String Observaciones
        {
            get
            {
                return observaciones;
            }
            set
            {
                observaciones = value;
            }
        }
        
        private int materia;

        public int Materia
        {
            get
            {
                return materia;
            }
            set
            {
                materia = value;
            }
        }


        private int temaQueImporta = 0;

        public int TemaQueImporta
        {
            get
            {
                return temaQueImporta;
            }
            set
            {
                temaQueImporta = value;
            }
        }

        private ObservableCollection<TematicoViewModel> subtemas;

        public ObservableCollection<TematicoViewModel> SubTemas
        {
            get
            {
                if (this.subtemas == null)
                {
                    this.subtemas = new ObservableCollection<TematicoViewModel>();
                }
                return this.subtemas;
            }
            set
            {
                this.subtemas = value;
            }
        }

        public void AddSubTema(TematicoViewModel tema)
        {
            subtemas.Add(tema);
        }

        

        #region Constructores

        public TematicoViewModel()
        {
        }

        public TematicoViewModel(TematicoViewModel parent)
        {
            this.parentItem = parent;
        }

        public TematicoViewModel(TematicoViewModel parent, bool lazyLoadChildren)
        {
            this.parent = parent;
            
            subtemas = new ObservableCollection<TematicoViewModel>();

            if (lazyLoadChildren)
                subtemas.Add(DummyChild);
        }

        public TematicoViewModel(int idTema, string descripcion, string descripcionStr,
            int nivel, int idPadre, int iduser, DateTime fecha, DateTime hora,
            String nota, String observaciones, int materia,  int temaQueImporta)
        {
            this.idTema = idTema;
            this.descripcion = descripcion;
            this.descripcionStr = descripcionStr;
            this.nivel = nivel;
            this.idPadre = idPadre;
            this.idUser = iduser;
            this.fecha = fecha;
            this.hora = hora;
            this.nota = nota;
            this.observaciones = observaciones;
            this.materia = materia;
            this.temaQueImporta = temaQueImporta;
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

        public TematicoViewModel Parent
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
            foreach (var item in new MainViewModel(this.materia).GetTemas(this, this.Materia))
                SubTemas.Add(item);
        }

        #endregion // LoadChildren

        #region AddRemove Children

        protected virtual void AddSubtema(TematicoViewModel child)
        {
            child.Parent = this;
            SubTemas.Add(child);
        }

        public virtual void RemoveSubTema(TematicoViewModel child)
        {
            SubTemas.Remove(child);
        }

        #endregion
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
        


    }
}
