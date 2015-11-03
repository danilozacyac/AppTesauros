using System;
using System.Linq;
using System.Windows;
using TesauroMiddleTier;
using TesauroTO;
using TesauroUtilities;
using mx.gob.scjn.ius_common.gui.utils;
using Login.Utils;
using AppTesauro09wpf.UserControl;
using ScjnUtilities;

namespace AppTesauro09wpf.TematicoIus
{
    /// <summary>
    /// Interaction logic for Tematico.xaml
    /// </summary>
    public partial class Tematico : Window
    {
        public WMain VentanaPadre { get; set; }

        private MainViewModel main;
        private TematicoViewModel temaSeleccionado = null;
        private TemaTO temaMateria;

        private readonly bool isSearchEnable = false;
        
        private readonly TemaTO tema;

        public Tematico(TemaTO tema, TemaTO temaMateria, bool isSearchEnable)
        {
            InitializeComponent();
            this.tema = tema;
            this.temaMateria = temaMateria;
            this.isSearchEnable = isSearchEnable;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            main = new MainViewModel(tema.Materia);
            TreeTematico.DataContext = main;
        }

        private void TreeTematico_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            try
            {
                temaSeleccionado = (TematicoViewModel)TreeTematico.SelectedItem;
                TxtDescripcion.Text = temaSeleccionado.Descripcion;
            }
            catch (NullReferenceException)
            {
            }
        }

        private void BtnAceptar_Click(object sender, RoutedEventArgs e)
        {
            IFachadaTesauro fac = new FachadaTesauro();
            bool exist = false;
            if ((bool)RadYes.IsChecked)
            {
                exist = fac.VerifyExistence(BusquedaUtilities.Normaliza(TxtDescripcion.Text.ToUpper()), temaMateria.IDTema, temaMateria.Materia);
            }
            else
            {
                exist = fac.VerifyExistence(BusquedaUtilities.Normaliza(TxtDescripcion.Text.ToUpper()), tema.IDTema, temaMateria.Materia);
            }


            if (exist)
            {
                MessageBox.Show("Ese tema ya existe, favor de verificar");
                return;
            }

            if (temaSeleccionado == null)
            {
                MessageBox.Show("Seleccione el tema que desea importar", Constants.TITULO_GENERAL,
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (RadYes.IsChecked == true)
            {
                if (tema.Materia == 4 || tema.Materia == 8)
                {
                    temaSeleccionado.IDPadre = temaMateria.IDTema;
                    temaSeleccionado.Nivel = 2;
                }
                else
                {
                    temaSeleccionado.IDPadre = temaMateria.IDTema;
                    temaSeleccionado.Nivel = 1;
                }

            }
            else
            {
                temaSeleccionado.IDPadre = tema.IDTema;
                temaSeleccionado.Nivel = tema.Nivel + 1;
            }

            temaSeleccionado.Nota = TxtNotas.Text;
            temaSeleccionado.Observaciones = TxtObservaciones.Text;
            temaSeleccionado.DescripcionStr = BusquedaUtilities.Normaliza(TxtDescripcion.Text);
            

            if (RadNo.IsChecked == true)
                temaSeleccionado.Descripcion = temaSeleccionado.Descripcion.Substring(0, 1).ToUpper() + temaSeleccionado.Descripcion.Substring(1).ToLower();

            TemaTO temaTo = new TemaTO(0, temaSeleccionado.Descripcion, temaSeleccionado.DescripcionStr, temaSeleccionado.Nivel,
                                    temaSeleccionado.IDPadre, 0, new DateTime(), new DateTime(), temaSeleccionado.Nota, 
                                    temaSeleccionado.Observaciones, tema.Materia, 1,2,temaSeleccionado.IDTema,0);
            if (RadNo.IsChecked == true)
            {
                tema.IsExpanded = true;
                temaTo.Parent = tema;
                tema.AddSubTema(temaTo);
            }
            else
            {
                temaTo.Parent = temaMateria;
                temaMateria.AddSubTema(temaTo);

                if (isSearchEnable)
                    new TemaToViewModel().SearchParentAddSon(TemaToViewModel.Tematico, temaTo);

            }

            temaSeleccionado.TemaQueImporta = tema.IDTema;

            FachadaTesauro fach = new FachadaTesauro();
            int idTemaNuevo = fac.GeneraNuevoTema(temaTo);
            fach.SetTemaQueImporta(temaSeleccionado.IDTema, temaSeleccionado.Materia, temaSeleccionado.IDPadre);
            fac.SetBitacora(idTemaNuevo, 1, 8, UserStatus.IdActivo, " ", temaSeleccionado.Descripcion,tema.Materia);

            VentanaPadre.Controlador.LoadTemas();

            this.Close();
        }

        private void BtnBuscar_Click(object sender, RoutedEventArgs e)
        {
            String stringResult = Validadores.BusquedaPalabraTexto(TxtBuscar);

            if (stringResult.Equals(""))
            {
                MainViewModel search = new MainViewModel(tema.Materia, TxtBuscar.Text);
                TreeTematico.DataContext = search;
            }
        }

        private void BtnRestaurar_Click(object sender, RoutedEventArgs e)
        {
            TreeTematico.DataContext = main;
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}