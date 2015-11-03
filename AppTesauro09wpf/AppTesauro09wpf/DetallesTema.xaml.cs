using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using AppTesauro09wpf.Busqueda;
using Login.Utils;
using TesauroMiddleTier;
using TesauroTO;
using TesauroUtilities;
using mx.gob.scjn.ius_common.gui.utils;
using AppTesauro09wpf.UserControl;
using ScjnUtilities;

namespace AppTesauro09wpf
{
    /// <summary>
    /// Interaction logic for DetallesTema.xaml
    /// </summary>
    public partial class DetallesTema : Window
    {
        private int TipoActualiza { get; set; }

        private TemaTO Datos { get; set; }

        private TemaTO DatosIA { get; set; }

        public WMain VentanaPadre { get; set; }

        

        /// <summary>
        /// En las materia que estan subdivididas es necesario saber cual de los temas de primer nivel, es decir, su subdivisión de materia, es el padre
        /// del que se va a agregar en caso de que sea de primer nivel
        /// </summary>
        private TemaTO TemaMateria { get; set; }

        private TemaTO TemaPadre { get; set; }

        private readonly bool isSearchEnable;

       

        private List<TemaTO> TemasCopiar { get; set; }

        /// <summary>
        /// Constructor por omision
        /// </summary>
        public DetallesTema()
        {
            InitializeComponent();
            TbxDescripcion.Focus();
        }

        /// <summary>
        /// Se busca crear un nuevo nodo, puede tener un padre,
        /// si el padre es 0 es de primer nivel.
        /// </summary>
        /// <param name="Padre"></param>
        public DetallesTema(TemaTO temaPadre, TemaTO temaMateria, bool isSearchEnable)
        {
            InitializeComponent();

            this.isSearchEnable = isSearchEnable;
            TbxDescripcion.Focus();
            TipoActualiza = 1;
            this.TemaPadre = temaPadre;
            TemaMateria = temaMateria;
            
            if (temaPadre.IDTema != 0)
            {
                TbxDescStr.Visibility = Visibility.Hidden;
                lblDescSTtr.Visibility = Visibility.Hidden;
            }
            else
            {
                TbxDescStr.Visibility = Visibility.Visible;
                lblDescSTtr.Visibility = Visibility.Visible;
                rdbSi.IsChecked = true;
                rdbNo.Visibility = Visibility.Hidden;
                rdbSi.Visibility = Visibility.Hidden;
                lblPregunta.Visibility = Visibility.Hidden;
            }
        }

        public DetallesTema(TemaTO aModificar)
        {
            InitializeComponent();
            TbxDescripcion.Focus();
            GrdPregunta.Visibility = Visibility.Collapsed;
            lblPregunta.Visibility = Visibility.Collapsed;
            TbxDescripcion.Text = aModificar.Descripcion;
            TbxNotas.Text = aModificar.Nota;
            TbxObservaciones.Text = aModificar.Observaciones;
            //TbxDescStr.Text = AModificar.DescripcionStr;
            Datos = aModificar;
            if (aModificar.IDPadre != 0)
            {
                TbxDescStr.Visibility = Visibility.Hidden;
                lblDescSTtr.Visibility = Visibility.Hidden;
                rdbNo.IsChecked = true;
            }
            else
            {
                IFachadaTesauro fac = new FachadaTesauro();
                TemaTO ia = fac.GetIA(Datos.IDTema);
                if (ia != null)
                {
                    DatosIA = ia;
                    TbxDescStr.Text = ia.Descripcion;
                }
                else
                {
                    DatosIA = new TemaTO(-1, String.Empty, String.Empty, 0,
                        Datos.IDTema, 0, DateTime.Now, DateTime.Now,
                        String.Empty, String.Empty, 0, Constants.STATUS_NUEVO,0);
                }
                rdbSi.IsChecked = true;
            }
        }

        private void BtnOK_Click(object sender, RoutedEventArgs e)
        {
            if ((TbxDescripcion.Text == null) || (TbxDescripcion.Text.Equals(String.Empty)))
            {
                MessageBox.Show(Mensajes.MENSAJE_CAMPO_TEXTO_VACIO, Constants.TITULO_GENERAL,
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            bool permitidos = true;
            foreach (String item in Constants.NO_PERMITIDOS)
            {
                permitidos = permitidos && (!TbxDescripcion.Text.Contains(item));
            }
            if (!permitidos)
            {
                MessageBox.Show(Mensajes.MENSAJE_NO_PERMITIDOS, Constants.TITULO_GENERAL,
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            int accion = 4;// VerificaExisteTema(FlowDocumentHighlight.Normaliza(TbxDescripcion.Text));
            if (accion == Constants.CANCELAR)
            {
                return;
            }
            else if (accion == Constants.COPIAR)
            {
                CopiaParciales();
                VentanaPadre.Controlador.LoadTemas();
                this.Close();
                return;
            }
            else if (accion == Constants.IGNORAR)
            {
                IgnoraAccion();
                return;
            }
        }

        /// <summary>
        /// Cuando se encontró coincidencias pero se pide ignorar la ventana
        /// de búsqueda, lo que se hace es generar efectivamente la modificación o
        /// alta de tena.
        /// </summary>
        private void IgnoraAccion()
        {
            IFachadaTesauro fac = new FachadaTesauro();
            if (Datos != null)
            {
                String tempDesc = Datos.Descripcion;
                Datos.Descripcion = (rdbSi.IsChecked == true) ? TbxDescripcion.Text.ToUpper() : TbxDescripcion.Text;
                Datos.DescripcionStr = BusquedaUtilities.Normaliza(TbxDescripcion.Text) + " X";
                Datos.Nota = TbxNotas.Text;
                Datos.Observaciones = TbxObservaciones.Text;
                fac.ActualizaTema(Datos);
                fac.SetBitacora(Datos.IDTema, 1, 2, UserStatus.IdActivo, tempDesc, Datos.Descripcion, ((TemaTO)VentanaPadre.treeView.SelectedItem).Materia);
                if (Datos.IDPadre == 0)
                {
                    DatosIA.Descripcion = TbxDescStr.Text.ToUpper();
                    DatosIA.Status = Constants.STATUS_NUEVO;
                    DatosIA.DescripcionStr = " " + BusquedaUtilities.Normaliza(TbxDescStr.Text.ToUpper()) + " X";
                    if (DatosIA.IDTema != -1)
                    {
                        fac.actualizaIA(DatosIA);
                    }
                    else
                    {
                        fac.InsertaIA(DatosIA.Descripcion, Datos.IDTema,TemaMateria.IDTema);
                    }
                }
                
                if (!isSearchEnable)
                    VentanaPadre.Controlador.LoadTemas();
                this.Close();
            }
            else
            {
                bool exist = false;
                if ((bool)rdbSi.IsChecked)
                {
                    exist = fac.VerifyExistence(BusquedaUtilities.Normaliza(TbxDescripcion.Text.ToUpper()), TemaMateria.IDTema, TemaMateria.Materia);
                }
                else
                {
                    exist = fac.VerifyExistence(BusquedaUtilities.Normaliza(TbxDescripcion.Text.ToUpper()), TemaPadre.IDTema, TemaMateria.Materia);
                }

                                
                if (exist)
                {
                    MessageBox.Show("Ese tema ya existe, favor de verificar");
                    return;
                }

                Datos = new TemaTO(-1, (rdbSi.IsChecked == true) ? TbxDescripcion.Text.ToUpper() : TbxDescripcion.Text,
                     BusquedaUtilities.Normaliza(TbxDescripcion.Text.ToUpper()) + " X",
                    0, TemaPadre.IDTema, 0, DateTime.Now, DateTime.Now,
                    TbxNotas.Text, TbxObservaciones.Text, TemaPadre.Materia, Constants.STATUS_NUEVO,0);
                
                
                if ((bool)rdbSi.IsChecked)
                {
                    if (TemaPadre.Materia == 4 || TemaPadre.Materia == 8)
                    {
                        Datos.IDPadre = TemaMateria.IDTema;
                        Datos.Nivel = 2;
                    }
                    else
                    {
                        Datos.IDPadre = TemaMateria.IDTema;
                        Datos.Nivel = 1;
                    }
                }
                else
                {
                    Datos.IDPadre = TemaPadre.IDTema;
                }
                String inclusionAscendente = TbxDescStr.Text.ToUpper();
                
                int idTemaNuevo = fac.GeneraNuevoTema(Datos);
                Datos.IDTema = idTemaNuevo;
                fac.SetBitacora(idTemaNuevo, 1, 1, UserStatus.IdActivo, " ", Datos.Descripcion, Datos.Materia);

                if (Datos.IDPadre == 0)
                {
                    fac.InsertaIA(inclusionAscendente, Datos.IDTema,Datos.Materia);
                }

                TemaTO temaSelected = (TemaTO)VentanaPadre.treeView.SelectedItem;

                if ((bool)rdbSi.IsChecked)
                {
                    Datos.Parent = TemaMateria;
                    TemaMateria.AddSubTema(Datos);

                    //if (isSearchEnable)
                    //    new TemaToViewModel().SearchParentAddSon(TemaToViewModel.Tematico, Datos);
                }
                else
                {
                    if (temaSelected.IsExpanded)
                    {
                        Datos.Parent = temaSelected;
                        //temaSelected.AddSubTema(Datos);
                        //if (isSearchEnable)
                            new TemaToViewModel().SearchParentAddSon(TemaToViewModel.Tematico, Datos);
                        //((TemaTO)VentanaPadre.treeView.SelectedItem).AddSubTema(temaSelected);
                    }
                    else
                    {
                        temaSelected.IsExpanded = true;
                    }
                }

                this.Close();
            }
        }

        private void CopiaParciales()
        {
            //IFachadaTesauro fac = new FachadaTesauro();
            //if ((bool)rdbSi.IsChecked)
            //{
            //    IDPadre = 0;
            //}
            //fac.CopiaTema(TemasCopiar, IDPadre, IDMateria);
        }

        private int VerificaExisteTema(string buscar)
        {
            BusquedaCopia auxiliar = new BusquedaCopia(buscar);
            int res = 0;
            if ((auxiliar.TemasSeleccioandos.Count > 0) && (Datos != null))
            {
                List<TemaTO> temporal = new List<TemaTO>();
                foreach (TemaTO item in auxiliar.TemasSeleccioandos)
                {
                    if (item.IDTema != Datos.IDTema)
                    {
                        temporal.Add(item);
                    }
                }
                auxiliar.TemasSeleccioandos = temporal;
            }
            if (auxiliar.TemasSeleccioandos.Count > 0)
            {
                auxiliar.ShowDialog();
                res = auxiliar.Resultado;
                if (res == Constants.COPIAR)
                {
                    TemasCopiar = auxiliar.ListaCopiar;
                }
            }
            else
            {
                res = Constants.IGNORAR;
            }
            return res;
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void rdbSi_Checked(object sender, RoutedEventArgs e)
        {
            TbxDescStr.Visibility = Visibility.Visible;
            lblDescSTtr.Visibility = Visibility.Visible;
        }

        private void rdbNo_Checked(object sender, RoutedEventArgs e)
        {
            TbxDescStr.Visibility = Visibility.Hidden;
            lblDescSTtr.Visibility = Visibility.Hidden;
        }
    }
}