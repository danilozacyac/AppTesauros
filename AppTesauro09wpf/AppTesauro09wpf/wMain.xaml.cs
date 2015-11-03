using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using AppTesauro09wpf.Controller;
using AppTesauro09wpf.Controller.Impl;
using AppTesauro09wpf.Reportes;
using Telerik.Windows.Controls;
using TesauroTO;
using TesauroUtilities;

namespace AppTesauro09wpf
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class WMain : Window
    {
        public IVentanaPrincipalController Controlador { get; set; }
        public int TipoSeleccion;

        public WMain()
        {
            InitializeComponent();
            Controlador = new VentanaPrincipalControlerImpl(this);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            StyleManager.ApplicationTheme = new Windows8Theme();
            Controlador.LoadTemas();
        }
        
        private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            TreeView arbol = (TreeView)sender;
            Controlador.CambioTema(arbol);
        }


        private void DgRelProx_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            Controlador.CambioRP();
        }

        private void DgSinonimos_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (DgSinonimos.SelectedItem != null)
            {
                SinonimoTO seleccionado = (SinonimoTO)DgSinonimos.SelectedItem;
                lblTemaSelec.Text = "Sinónimo seleccionado: " + seleccionado.Descripcion;
                TipoSeleccion = Constants.TIPO_SELECCION_SINONIMO;
            }
        }

        private void Eliminar_Click(object sender, RoutedEventArgs e)
        {
            Controlador.EliminaTema();
        }

        private void Modificar_Click(object sender, RoutedEventArgs e)
        {
            Controlador.ModificaTema();
        }

        private void Nuevo_Click(object sender, RoutedEventArgs e)
        {
            Controlador.NuevoTema();
        }

        private void BtnSinonimoNvo_Click(object sender, RoutedEventArgs e)
        {
            Controlador.NuevoSinonimo();
        }

        private void BtnRelacionespNvo_Click(object sender, RoutedEventArgs e)
        {
            Controlador.NuevoRP();
        }

        private void BtnSinonimoModif_Click(object sender, RoutedEventArgs e)
        {
            Controlador.ModificaSinonimo();
        }

        private void BtnRelacionespModif_Click(object sender, RoutedEventArgs e)
        {
            Controlador.ModificaRP();
        }

        private void BtnRelacionespElim_Click(object sender, RoutedEventArgs e)
        {
            Controlador.EliminaRP();
        }

        private void LblResultadoIA_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Controlador.SeleccionaIA();
        }

        ////private void MateriaChecked(object sender, RoutedEventArgs e)
        ////{
        ////    if (Controlador != null)
        ////    {
        ////        Controlador.CambioMateria(sender as MenuItem);
        ////    }
        ////}

        private void BtnSinonimoElim_Click(object sender, RoutedEventArgs e)
        {
            Controlador.EliminaSinonimo();
        }

        private void BtnVerTesis_Click(object sender, RoutedEventArgs e)
        {
            Cursor = Cursors.Wait;
            mx.gob.scjn.ius_common.utils.Globals.Marcados = new System.Collections.Generic.HashSet<int>();
            Controlador.ObtenTesisPorRegistro();
            Cursor = Cursors.Arrow;
        }

        private void BtnAgregarRelacionTesis_Click(object sender, RoutedEventArgs e)
        {
            Controlador.NuevaRelacionTemaTesis();
        }

        private void BtnImportarTema_Click(object sender, RoutedEventArgs e)
        {
            Controlador.ImportaTema();
        }

        

        private void Stats_Click(object sender, RoutedEventArgs e)
        {
            Controlador.MuestraEstadisticas();
        }

        private void Certificacion_Click(object sender, RoutedEventArgs e)
        {
            Controlador.GeneraListadoCertificacion();
        }

        private void treeView_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            Controlador.KeyBoardFastAccess( sender, e);
        }

        private void Window_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            Controlador.KeyBoardFastAccess(sender, e);
        }

        private void MenuBuscar_Click(object sender, RoutedEventArgs e)
        {
            Controlador.CtrlBFunction();
        }

        private void MenuPegar_Click(object sender, RoutedEventArgs e)
        {
            Controlador.CtrlVFunction();
        }

        private void MenuCortar_Click(object sender, RoutedEventArgs e)
        {
            Controlador.CtrlXFunction(Key.X);
        }

        private void MenuCopiar_Click(object sender, RoutedEventArgs e)
        {
            Controlador.CtrlCFunction(Key.C);
        }

        private void MenuCortarTesis_Click(object sender, RoutedEventArgs e)
        {
            Controlador.CtrlXFunction(Key.U);
        }

        private void MenuCopiarTesis_Click(object sender, RoutedEventArgs e)
        {
            Controlador.CtrlCFunction(Key.T);
        }

        private void MenuPegarTesis_Click(object sender, RoutedEventArgs e)
        {
            Controlador.CtrlYFunction();
        }

        

        private void NoIngresadas_Click(object sender, RoutedEventArgs e)
        {
            Controlador.TesisNoRelacionadas();
        }

        private void TemasToPdf_Click(object sender, RoutedEventArgs e)
        {
            Controlador.GetPdf();
        }

        private void RBtnCopiaMarc_Click(object sender, RoutedEventArgs e)
        {
            Controlador.CopiarTesisMarcadas();
        }

        private void RBtnTraslMarc_Click(object sender, RoutedEventArgs e)
        {
            Controlador.TrasladaTesisMarcadas();
        }

        private void RBtnDelMarc_Click(object sender, RoutedEventArgs e)
        {
            Controlador.EliminaMarcacionTesis();
        }

        private void RBtnEjecutaExp_Click(object sender, RoutedEventArgs e)
        {
            Cursor = Cursors.Wait;
            Controlador.EjecutaConsulta();
            Cursor = Cursors.Arrow;
        }

        private void RBtnNewExp_Click(object sender, RoutedEventArgs e)
        {
            Controlador.NuevaExpresion();
        }

        private void RBtnEditExp_Click(object sender, RoutedEventArgs e)
        {
            Controlador.ModificarExpresion();
        }

        private void RBtnDelExp_Click(object sender, RoutedEventArgs e)
        {
            Controlador.EliminaExpresion();
        }

        private void SearchTextBox_Search(object sender, RoutedEventArgs e)
        {
            String tempString = ((TextBox)sender).Text.ToUpper();

            if (String.IsNullOrEmpty(tempString) || String.IsNullOrWhiteSpace(tempString))
                Controlador.RestaurarTemas();
            else
                Controlador.BuscarTema();
        }

        private void RBtnExportExcel_Click(object sender, RoutedEventArgs e)
        {
            Controlador.GetExcelReport();
        }

        
    }
}
