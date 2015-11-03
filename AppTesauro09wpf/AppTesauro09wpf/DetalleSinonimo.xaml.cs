using System;
using System.Linq;
using System.Windows;
using Login.Utils;
using TesauroMiddleTier;
using TesauroTO;
using TesauroUtilities;
using mx.gob.scjn.ius_common.gui.utils;
using ScjnUtilities;

namespace AppTesauro09wpf
{
    /// <summary>
    /// Interaction logic for DetalleSinonimo.xaml
    /// </summary>
    public partial class DetalleSinonimo : Window
    {
        private int tipoSinonimo = 0;
        private SinonimoTO sinonimoActual { get; set; }
        private int PadreSinonimo { get; set; }
        public WMain ventanaPadre { get; set; }

        public DetalleSinonimo()
        {
            InitializeComponent();
            TbxDescripcion.Focus();
        }
        public DetalleSinonimo(int Padre, int TIPO)
        {
            InitializeComponent();
            TbxDescripcion.Focus();
            tipoSinonimo = TIPO;
            PadreSinonimo = Padre;
            if (tipoSinonimo == Constants.TIPO_RP)
            {
                lblTitulo.Content = "Detalle de la relación próxima";
            }
        }

        public DetalleSinonimo(int tipo, SinonimoTO sinonimo)
        {
            InitializeComponent();
            TbxDescripcion.Focus();
            tipoSinonimo = tipo;
            if (tipoSinonimo == Constants.TIPO_RP)
            {
                lblTitulo.Content = "Detalle de la relación próxima";
            }
            TbxDescripcion.Text = sinonimo.Descripcion;
            tbxNotas.Text = sinonimo.Nota;
            tbxObservaciones.Text = sinonimo.Observaciones;
            //TbxDescStr.Text = sinonimo.DescripcionStr;
            sinonimoActual = sinonimo;
        }

        private void BtnOK_Click(object sender, RoutedEventArgs e)
        {
            if ((TbxDescripcion.Text == null) || (TbxDescripcion.Text.Equals(String.Empty)))
            {
                MessageBox.Show(Mensajes.MENSAJE_CAMPO_TEXTO_VACIO, Constants.TITULO_GENERAL,
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            bool Permitidos = true;
            foreach (String item in Constants.NO_PERMITIDOS)
            {
                Permitidos = Permitidos && (!TbxDescripcion.Text.Contains(item));
            }
            if (!Permitidos)
            {
                MessageBox.Show(Mensajes.MENSAJE_NO_PERMITIDOS, Constants.TITULO_GENERAL,
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            IFachadaTesauro fac = new FachadaTesauro();
            if (sinonimoActual != null)
            {
                String tempDesc = sinonimoActual.Descripcion;
                sinonimoActual.Descripcion = BusquedaUtilities.Normaliza(TbxDescripcion.Text);
                sinonimoActual.DescripcionStr = " " + sinonimoActual.Descripcion + " X";
                sinonimoActual.Nota = BusquedaUtilities.Normaliza(tbxNotas.Text);
                sinonimoActual.Observaciones = BusquedaUtilities.Normaliza(tbxObservaciones.Text);
                fac.ActualizaSinonimo(sinonimoActual, ((TemaTO)ventanaPadre.treeView.SelectedItem).Materia);
                fac.SetBitacora(sinonimoActual.IDTema, (tipoSinonimo == 1) ? 4 : 5, 2, UserStatus.IdActivo, tempDesc, sinonimoActual.Descripcion, ((TemaTO)ventanaPadre.treeView.SelectedItem).Materia);
                ventanaPadre.Controlador.LoadTemas();
                this.Close();
            }
            else
            {
                String DescStr = " " + BusquedaUtilities.Normaliza(TbxDescripcion.Text) + " X";
                sinonimoActual = new SinonimoTO(0, -1, TbxDescripcion.Text.ToUpper(), tipoSinonimo, DescStr,
                    0, DateTime.Now, DateTime.Now, tbxNotas.Text, tbxObservaciones.Text);
                TemaTO Datos = (TemaTO)ventanaPadre.treeView.SelectedItem;
                sinonimoActual.IDPadre = Datos.IDTema;
                sinonimoActual.IDTema = fac.GeneraNuevoSinonimo(sinonimoActual, ((TemaTO)ventanaPadre.treeView.SelectedItem).Materia);
                fac.SetBitacora(sinonimoActual.IDTema, (tipoSinonimo == 1) ? 4 : 5, 1, UserStatus.IdActivo, " ", sinonimoActual.Descripcion, ((TemaTO)ventanaPadre.treeView.SelectedItem).Materia);
               //ventanaPadre.LoadTemas();
                this.Close();
            }
            ventanaPadre.Controlador.ObtenDatos(ventanaPadre.treeView);
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
