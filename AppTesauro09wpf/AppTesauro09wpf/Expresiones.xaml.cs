using System;
using System.Linq;
using System.Windows;
using Login.Utils;
using TesauroMiddleTier;
using TesauroTO;
using TesauroUtilities;
using mx.gob.scjn.ius_common.gui.utils;

namespace AppTesauro09wpf
{
    /// <summary>
    /// Interaction logic for Expresiones.xaml
    /// </summary>
    public partial class Expresiones : Window
    {
        /// <summary>
        /// La ventana que mando llamar a este diálogo
        /// </summary>
        public WMain VentanaPadre { get; set; }
        /// <summary>
        /// La expresión a actualizar
        /// </summary>
        private ExpresionTO ExpresionActual { get; set; }
        /// <summary>
        /// Cuando se trata de insertar una expresión, el tema al que pertenece.
        /// </summary>
        private int Tema { get; set; }
        /// <summary>
        /// Constructor por omisión
        /// </summary>
        public Expresiones()
        {
            InitializeComponent();
            tbxExpresion.Focus();
        }
        /// <summary>
        /// Constructor para insertar un nuevo tema.
        /// </summary>
        /// <param name="tema"></param>
        public Expresiones(int tema)
        {
            InitializeComponent();
            Tema = tema;
            
            tbxExpresion.Focus();
        }
        private readonly String expresionModificada = "";
        public Expresiones(ExpresionTO expresion)
        {
            InitializeComponent();
            ExpresionActual = expresion;
            expresionModificada = ExpresionActual.Descripcion;
            tbxExpresion.Text = expresion.Descripcion;
            tbxExpresion.Focus();
            CbxTexto.IsChecked = false;
            CbxRubro.IsChecked = false;
            CbxPrec.IsChecked = false;
            CbxLoc.IsChecked = false;
            switch (expresion.Operador)
            {
                case Constants.BUSQUEDA_PALABRA_OP_Y:
                    cmbOperador.SelectedIndex = 0;
                    break;
                case Constants.BUSQUEDA_PALABRA_OP_O:
                    cmbOperador.SelectedIndex = 1;
                    break;
                case Constants.BUSQUEDA_PALABRA_OP_NO:
                    cmbOperador.SelectedIndex = 2;
                    break;
            }
            foreach (int item in expresion.Campos)
            {
                switch (item)
                {
                    case Constants.BUSQUEDA_PALABRA_CAMPO_LOC:
                        CbxLoc.IsChecked = true;
                        break;
                    case Constants.BUSQUEDA_PALABRA_CAMPO_PRECE:
                        CbxPrec.IsChecked = true;
                        break;
                    case Constants.BUSQUEDA_PALABRA_CAMPO_RUBRO:
                        CbxRubro.IsChecked = true;
                        break;
                    case Constants.BUSQUEDA_PALABRA_CAMPO_TEXTO:
                        CbxTexto.IsChecked = true;
                        break;
                }
            }
        }

        private void btnAceptar_Click(object sender, RoutedEventArgs e)
        {
            if (Validadores.BusquedaPalabraTexto(tbxExpresion).Equals(Constants.CADENA_VACIA))
            {
                IFachadaTesauro fac = new FachadaTesauro();
                Int32 Operador = 0;
                int contadorCampos = 0;
                if ((bool)CbxTexto.IsChecked) contadorCampos++;
                if ((bool)CbxPrec.IsChecked) contadorCampos++;
                if ((bool)CbxRubro.IsChecked) contadorCampos++;
                if ((bool)CbxLoc.IsChecked) contadorCampos++;
                if (contadorCampos == 0)
                {
                    MessageBox.Show(Mensajes.MENSAJE_CAMPO_REQUERIDO, Mensajes.TITULO_ADVERTENCIA,
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                int[] camposSeleccionados = new int[contadorCampos];
                contadorCampos = 0;
                if ((bool)CbxTexto.IsChecked) { camposSeleccionados[contadorCampos] = Constants.BUSQUEDA_PALABRA_CAMPO_TEXTO; contadorCampos++; }
                if ((bool)CbxPrec.IsChecked) { camposSeleccionados[contadorCampos] = Constants.BUSQUEDA_PALABRA_CAMPO_PRECE; contadorCampos++; }
                if ((bool)CbxRubro.IsChecked) { camposSeleccionados[contadorCampos] = Constants.BUSQUEDA_PALABRA_CAMPO_RUBRO; contadorCampos++; }
                if ((bool)CbxLoc.IsChecked) { camposSeleccionados[contadorCampos] = Constants.BUSQUEDA_PALABRA_CAMPO_LOC; contadorCampos++; }
                switch (cmbOperador.SelectedIndex)
                {
                    case 0:
                        Operador = Constants.BUSQUEDA_PALABRA_OP_Y;
                        break;
                    case 1:
                        Operador = Constants.BUSQUEDA_PALABRA_OP_O;
                        break;
                    case 2:
                        Operador = Constants.BUSQUEDA_PALABRA_OP_NO;
                        break;
                }

                FachadaTesauro tas = new FachadaTesauro();
                if (ExpresionActual != null)
                {
                    ExpresionActual.Descripcion = tbxExpresion.Text;
                    ExpresionActual.Operador = Operador;
                    ExpresionActual.Campos = camposSeleccionados;
                    fac.actualizaExpresion(ExpresionActual,((TemaTO)VentanaPadre.treeView.SelectedItem).Materia);

                    tas.SetBitacora(ExpresionActual.IDTema, 3, 2, UserStatus.IdActivo, expresionModificada, ExpresionActual.Descripcion, ((TemaTO)VentanaPadre.treeView.SelectedItem).Materia);
                }
                else
                {
                    ExpresionActual = new ExpresionTO(Tema, tbxExpresion.Text,
                        0, DateTime.Now, DateTime.Now, 0, Operador, camposSeleccionados);
                    fac.InsertaExpresion(ExpresionActual, ((TemaTO)VentanaPadre.treeView.SelectedItem).Materia);

                    tas.SetBitacora(Tema, 3, 1, UserStatus.IdActivo, " ", ExpresionActual.Descripcion, ((TemaTO)VentanaPadre.treeView.SelectedItem).Materia);
                }
                VentanaPadre.Controlador.ActualizaExpresiones(VentanaPadre.TipoSeleccion);
                this.Close();
            }
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
