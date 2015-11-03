#region USING
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using mx.gob.scjn.directorio.General;
using mx.gob.scjn.ius_common.TO;
using mx.gob.scjn.ius_common.fachade;

#endregion USING

namespace mx.gob.scjn.directorio.CJF
{

    /// <summary>
    /// Interaction logic for CJFConsejeros.xaml
    /// </summary>
    public partial class CJFConsejeros : Page
    {
        String enterKey = "\n";

        public Page contenedor { get; set; }
        public Page Back { get; set; }
        /*Para la impresión ***********************************************/

        private FlowDocument DocumentoParaCopiar { get; set; }
        private AcuerdosTO DocumentoActual;
        String strMinImpr = "";
        /******************************************************************/
        List<DirectorioMinistrosTO> lstResImpr = new List<DirectorioMinistrosTO>();

        private double margenTextoEnter = 1.0;
        private int margenTextoLeave = 0;

        public CJFConsejeros()
        {
            InitializeComponent();
            lstResImpr = LlenaGridConsejeros();

            if (!BrowserInteropHelper.IsBrowserHosted)
            { Guardar_.Visibility = Visibility.Visible; }
            else { Guardar_.Visibility = Visibility.Hidden; }
        }

        private List<DirectorioMinistrosTO> LlenaGridConsejeros()
        {
            List<DirectorioMinistrosTO> lstRes = new List<DirectorioMinistrosTO>();
#if STAND_ALONE
            List<DirectorioMinistrosTO> R = null;
            FachadaBusquedaTradicional fachada = new FachadaBusquedaTradicional();
#else
            DirectorioMinistrosTO[] R = new DirectorioMinistrosTO[50];
            FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
#endif
            R = fachada.getDirConsejerosXFiltro("0");
#if STAND_ALONE
            for (int i = 0; i < R.Count; i++)
#else
            for (int i = 0; i < R.Length; i++)
#endif
            {
                DirectorioMinistrosTO Campo = new DirectorioMinistrosTO();
                Campo.IdPersona = R[i].IdPersona;
                Campo.IdTitulo = R[i].IdTitulo;
                Campo.IdPuesto = R[i].IdPuesto;
                Campo.IdPonencia = R[i].IdPonencia;
                Campo.NombrePersona = R[i].NombrePersona;
                Campo.NombreCompleto = R[i].Cargo + " " + R[i].NombreCompleto + R[i].Posfijo;
                Campo.ApellidosPersona = R[i].ApellidosPersona;
                Campo.Orden = R[i].Orden;
                Campo.OrdenSala = R[i].OrdenSala;
                Campo.Sala = R[i].Sala;
                Campo.Prefijo = R[i].Prefijo;
                Campo.Posfijo = R[i].Posfijo;
                Campo.DomPersona = R[i].DomPersona;
                Campo.TelPersona = R[i].TelPersona;
                Campo.ExtPersona = R[i].ExtPersona;
                Campo.TituloPersona = R[i].TituloPersona;
                Campo.TitSemblanza = R[i].TitSemblanza;
                Campo.ArchivoSemblanza = R[i].ArchivoSemblanza;
                Campo.Cargo = R[i].Cargo;
                lstRes.Add(Campo);
                Campo = null;
            }
            this.Consejeros.ItemsSource = lstRes;
            fachada.Close();
            return lstRes;
        }

        private void lblJuecMag_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        { }

        private void Salir_ImageFailed(object sender, ExceptionRoutedEventArgs e)
        { }

        private void Salir_MouseButtonDown(object sender, MouseButtonEventArgs e)
        {

            if (Back == null)
            {
                this.NavigationService.GoBack();
            }
            else
            {
                this.NavigationService.Navigate(Back);
            }
        }

        #region Opciones

        private void VerSemblanza(object sender, MouseButtonEventArgs e)
        {

            try
            {

                if (Consejeros.SelectedIndex >= 0)
                {
                    //if (typeof(CJFPrincipal) == contenedor.GetType())
                    //{
                    //contenedor.IconoRegresarVisible(true);

                    if (Consejeros.SelectedIndex != 2) 
                    {
                        Semblanza CJFSemblanza = new Semblanza(Consejeros.CurrentItem);
                        CJFSemblanza.contenedor = this;
                        CJFSemblanza.Back = this;
                        this.NavigationService.Navigate(CJFSemblanza);
                    }
                    else {
                        //por peticion del consejero, no quiere mostrar sus datos, jul, 2009
                        FTransparente.Visibility = Visibility.Visible;
                        Aviso.StrCabecera = mx.gob.scjn.directorio.MensajesDirectorio.TITULO_AVISO;
                        Aviso.StrMensaje = mx.gob.scjn.directorio.MensajesDirectorio.MENSAJE_SIN_DATOS;
                        Aviso.Visibility = Visibility.Visible;
                        Aviso.Background = Brushes.White;
                        Aviso.TomaFondo(FTransparente);
            
                    
                    }

                    //contenedor.IconoRegresarVisible(false);
                    //}
                }
            }

            catch (System.Exception error)
            {
                //Handle exception here
            }
        }

        public void VerConsejeros()
        {
            //if (typeof(CJFPrincipal) == contenedor.GetType())
            //{
            //    CJFPrincipal ventanaSCJN = (CJFPrincipal)contenedor;
            //    ventanaSCJN.VerConsejeros();
            //}
        }

        private void FuncAdmin_Open(object sender, MouseButtonEventArgs e)
        {
            CJFFuncionariosAdmin CJF_FuncAdmin = new CJFFuncionariosAdmin();
            CJF_FuncAdmin.Back = this;
            this.NavigationService.Navigate(CJF_FuncAdmin);
        }

        private void VERTCC(object sender, MouseButtonEventArgs e)
        {
            pageOrganoJur CJF_TCC = new pageOrganoJur("TCC");
            CJF_TCC.Back = this;
            this.NavigationService.Navigate(CJF_TCC);
        }

        private void VERJUZ(object sender, MouseButtonEventArgs e)
        {
            pageOrganoJur CJF_JUZ = new pageOrganoJur("JUZ");
            CJF_JUZ.Back = this;
            this.NavigationService.Navigate(CJF_JUZ);
        }

        private void VERTUC(object sender, MouseButtonEventArgs e)
        {
            pageOrganoJur CJF_TUC = new pageOrganoJur("TUC");
            CJF_TUC.Back = this;
            this.NavigationService.Navigate(CJF_TUC);
        }

        private void VerJuecesMag(object sender, MouseButtonEventArgs e)
        {
            CJFFuncionarios CJF_JUEMAG = new CJFFuncionarios();
            CJF_JUEMAG.Back = this;
            this.NavigationService.Navigate(CJF_JUEMAG);
        }

        private void VerAA(object sender, MouseButtonEventArgs e)
        {
            CJFAreasAdmin CJF_AA = new CJFAreasAdmin(0);
            CJF_AA.Back = this;
            this.NavigationService.Navigate(CJF_AA);
        }

        private void VerOA(object sender, MouseButtonEventArgs e)
        {
            CJFAreasAdmin CJF_AA = new CJFAreasAdmin(1);
            CJF_AA.Back = this;
            this.NavigationService.Navigate(CJF_AA);
        }

        private void lblCJF_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            CJFComisiones CJF_COM = new CJFComisiones();
            CJF_COM.Back = this;
            this.NavigationService.Navigate(CJF_COM);
        }

        private void lblMenu_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            CJFPrincipal CJF_Menu = new CJFPrincipal();
            CJF_Menu.Back = this;
            this.NavigationService.Navigate(CJF_Menu);
        }

        #endregion Opciones

        #region Eventos Etiquetas Menu

        private void lblCJF_MouseEnter(object sender, MouseEventArgs e)
        {
            lblCJF.BorderThickness = new Thickness(margenTextoEnter);
        }

        private void lblCJF_MouseLeave(object sender, MouseEventArgs e)
        {
            lblCJF.BorderThickness = new Thickness(margenTextoLeave);
        }

        private void lblJUZ_MouseEnter(object sender, MouseEventArgs e)
        {
            lblJUZ.BorderThickness = new Thickness(margenTextoEnter);
        }

        private void lblJUZ_MouseLeave(object sender, MouseEventArgs e)
        {
            lblJUZ.BorderThickness = new Thickness(margenTextoLeave);
        }

        private void lblTCC_MouseEnter(object sender, MouseEventArgs e)
        {
            lblTCC.BorderThickness = new Thickness(margenTextoEnter);
        }

        private void lblTCC_MouseLeave(object sender, MouseEventArgs e)
        {
            lblTCC.BorderThickness = new Thickness(margenTextoLeave);
        }

        private void lblFuncAdmin_MouseLeave(object sender, MouseEventArgs e)
        {
            lblFuncAdmin.BorderThickness = new Thickness(margenTextoLeave);
        }

        private void lblTUC_MouseEnter(object sender, MouseEventArgs e)
        {
            lblTUC.BorderThickness = new Thickness(margenTextoEnter);
        }

        private void lblTUC_MouseLeave(object sender, MouseEventArgs e)
        {
            lblTUC.BorderThickness = new Thickness(margenTextoLeave);
        }

        private void lblJuecMag_MouseEnter(object sender, MouseEventArgs e)
        {
            lblJuecMag.BorderThickness = new Thickness(margenTextoEnter);
        }

        private void lblJuecMag_MouseLeave(object sender, MouseEventArgs e)
        {
            lblJuecMag.BorderThickness = new Thickness(margenTextoLeave);
        }

        private void lblMenu_MouseEnter(object sender, MouseEventArgs e)
        {
            lblMenu.BorderThickness = new Thickness(margenTextoEnter);
        }

        private void lblMenu_MouseLeave(object sender, MouseEventArgs e)
        {
            lblMenu.BorderThickness = new Thickness(margenTextoLeave);
        }

        private void lblAA_MouseEnter(object sender, MouseEventArgs e)
        {
            lblAA.BorderThickness = new Thickness(margenTextoEnter);
        }

        private void lblAA_MouseLeave(object sender, MouseEventArgs e)
        {
            lblAA.BorderThickness = new Thickness(margenTextoLeave);
        }

        private void lblAAOA_MouseEnter(object sender, MouseEventArgs e)
        {
            lblAAOA.BorderThickness = new Thickness(margenTextoEnter);
        }

        private void lblAAOA_MouseLeave(object sender, MouseEventArgs e)
        {
            lblAAOA.BorderThickness = new Thickness(margenTextoLeave);
        }

        private void lblFuncAdmin_MouseEnter(object sender, MouseEventArgs e)
        {
            lblFuncAdmin.BorderThickness = new Thickness(margenTextoEnter);
        }

        #endregion Eventos Etiquetas Menu

        private void Imprimir_Click(object sender, RoutedEventArgs e)
        {
            ImprimeListado(true, 2);
        }


        //private void Imprimir(object sender, RoutedEventArgs e)
        //{
        //    ImprimeListado(true, 2);
        //}


        //private void Imprimir(object sender, MouseButtonEventArgs e)
        //{
          
        //}

        public void ImprimeListado(Boolean bTodo, Int32 nSelSalida)
        {
            String strText = "";

            if (bTodo == true) //Imprimir todos 
            {

                foreach (DirectorioMinistrosTO ItemIntegrante in lstResImpr)
                {
                    strText = strText + ItemIntegrante.NombreCompleto + enterKey;
                    strText = strText + ItemIntegrante.DomPersona + enterKey;
                    strText = strText + "Teléfono: " + ItemIntegrante.TelPersona + enterKey;
                    strText = strText + "Extensión: " + ItemIntegrante.ExtPersona + enterKey;
                    strText = strText + enterKey;
                }
            }
            else   //Imprimir sólo el actual
            {
                strText = strMinImpr + enterKey;
            }

            switch (nSelSalida)
            {
                case 0: //Papelera
                    PortaPapStr(strText);
                    break;

                case 1: //Archivo
                    Guardar(strText);
                    break;

                case 2: //Imprimir
                    ImprimeStr(strText);
                    break;
            }
        }

        private void ImprimeStr(String strTexto)
        {

            try
            {

                DocumentoAcuerdoDirec documento;

                documento = new DocumentoAcuerdoDirec(strTexto);

                DocumentoParaCopiar = documento.Copia;
                impresion.Document = documento.Documento; //(IDocumentPaginatorSource)documentoXps;
                impresion.Background = Brushes.White;
                PrintDialog pd = new PrintDialog();

                if ((pd.ShowDialog() == true))
                {
                    IDocumentPaginatorSource pag = impresion.Document as IDocumentPaginatorSource;
                    pd.PrintDocument(pag.DocumentPaginator, "Directorio");
                }
                impresion.Visibility = Visibility.Hidden;
            }

            catch (System.Exception error)
            {
                MessageBox.Show(MensajesDirectorio.MENSAJE_Y_LA_IMPRESORA, MensajesDirectorio.TITULO_MENSAJES,
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void PortaPapStr(String strTexto)
        {
            contenidoTexto = new RichTextBox();
            this.contenidoTexto.AppendText(strTexto);
            this.contenidoTexto.SelectAll();
            contenidoTexto.Copy();
            FTransparente.Visibility = Visibility.Visible;
            Aviso.StrCabecera = mx.gob.scjn.directorio.MensajesDirectorio.TITULO_PORTAPAPELES_DIR;
            Aviso.StrMensaje = mx.gob.scjn.directorio.MensajesDirectorio.MENSAJE_PORTAPAPELES_DIR;
            Aviso.Visibility = Visibility.Visible;
            Aviso.Background = Brushes.White;
            Aviso.TomaFondo(FTransparente);
        }

        private void Guardar(String strTexto)
        {

            try
            {

                if (!BrowserInteropHelper.IsBrowserHosted)
                {
                    Microsoft.Win32.SaveFileDialog guardaEn = new Microsoft.Win32.SaveFileDialog();
                    guardaEn.DefaultExt = ".rtf";
                    guardaEn.FileName = "Consejeros";
                    guardaEn.Title = "Guardar";
                    guardaEn.Filter = "Archivos de Texto Enriquecido|*.rtf";
                    guardaEn.AddExtension = true;

                    if ((bool)guardaEn.ShowDialog())
                    {
                        contenidoTexto = new RichTextBox();
                        contenidoTexto.AppendText(strTexto);
                        System.IO.FileStream archivo = new System.IO.FileStream(guardaEn.FileName, System.IO.FileMode.Create);
                        this.contenidoTexto.SelectAll();
                        this.contenidoTexto.Selection.Save(archivo, System.Windows.DataFormats.Rtf);
                        archivo.Flush();
                        archivo.Close();
                        MessageBox.Show("El archivo fue guardado como: " + archivo.Name, MensajesDirectorio.TITULO_MENSAJES);
                    }
                }
                else
                {
                    contenidoTexto = new RichTextBox();
                    contenidoTexto.AppendText(strTexto);
                    System.IO.IsolatedStorage.IsolatedStorageFileStream archivo = new System.IO.IsolatedStorage.
                        IsolatedStorageFileStream("texto.rtf", System.IO.FileMode.Create);
                    this.contenidoTexto.SelectAll();
                    this.contenidoTexto.Selection.Save(archivo, System.Windows.DataFormats.Text);
                    archivo.Flush();
                    archivo.Close();
                    MessageBox.Show("El archivo fue guardado como: " + archivo.Name, MensajesDirectorio.TITULO_MENSAJES,
                        MessageBoxButton.OK,
                        MessageBoxImage.Exclamation);
                }
                impresion.Visibility = Visibility.Hidden;
            }

            catch (System.Exception error)
            {
                MessageBox.Show(MensajesDirectorio.MENSAJE_ARCHIVO_ABIERTO, MensajesDirectorio.TITULO_ARCHIVO_ABIERTO,
                   MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void PortaPapeles_Click(object sender, RoutedEventArgs e)
        {
            ImprimeListado(true, 0);
        }


        //private void PortaPapeles(object sender, MouseButtonEventArgs e)
        //{
        //    ImprimeListado(true, 0);
        //}

        private void PortaPapeles__ImageFailed(object sender, ExceptionRoutedEventArgs e)
        {
        }

        private void Guardar_Click(object sender, RoutedEventArgs e)
        {
            ImprimeListado(true, 1);

        }

        //private void Guardar__MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        //{
        //    ImprimeListado(true, 1);
        //}

        private void GuardarEnArchivo(String strTexto)
        {

            try
            {

                if (!BrowserInteropHelper.IsBrowserHosted)
                {
                    Microsoft.Win32.SaveFileDialog guardaEn = new Microsoft.Win32.SaveFileDialog();
                    guardaEn.DefaultExt = ".rtf";
                    guardaEn.FileName = "DirectorioPJF";
                    guardaEn.Title = "Guardar";
                    guardaEn.Filter = "Archivos de Texto Enriquecido|*.rtf";
                    guardaEn.AddExtension = true;

                    if ((bool)guardaEn.ShowDialog())
                    {
                        contenidoTexto = new RichTextBox();
                        contenidoTexto.AppendText(strTexto);
                        System.IO.FileStream archivo = new System.IO.FileStream(guardaEn.FileName, System.IO.FileMode.Create);
                        this.contenidoTexto.SelectAll();
                        this.contenidoTexto.Selection.Save(archivo, System.Windows.DataFormats.Rtf);
                        archivo.Flush();
                        archivo.Close();
                        MessageBox.Show("El archivo fue guardado como: " + archivo.Name, MensajesDirectorio.TITULO_MENSAJES);
                    }
                }
                else
                {
                    contenidoTexto = new RichTextBox();
                    contenidoTexto.AppendText(strTexto);
                    System.IO.IsolatedStorage.IsolatedStorageFileStream archivo = new System.IO.IsolatedStorage.
                        IsolatedStorageFileStream("texto.rtf", System.IO.FileMode.Create);
                    this.contenidoTexto.SelectAll();
                    this.contenidoTexto.Selection.Save(archivo, System.Windows.DataFormats.Text);
                    archivo.Flush();
                    archivo.Close();
                    MessageBox.Show("El archivo fue guardado como: " + archivo.Name, MensajesDirectorio.TITULO_MENSAJES,
                        MessageBoxButton.OK,
                        MessageBoxImage.Exclamation);
                }
                impresion.Visibility = Visibility.Hidden;
            }

            catch (System.Exception error)
            {
                MessageBox.Show(MensajesDirectorio.MENSAJE_ARCHIVO_ABIERTO, MensajesDirectorio.TITULO_ARCHIVO_ABIERTO,
                   MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Salir_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
        }

        private void image1_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
        }

        private void CJF_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            if (BrowserInteropHelper.IsBrowserHosted)
            {
                MessageBox.Show(MensajesDirectorio.MENSAJE_NO_SE_PUEDE_VER_CORTE, MensajesDirectorio.TITULO_NO_SE_PUEDE_VER_CORTE,
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                LlamaCJF();
            }
        }

        private void LlamaCJF()
        {
            Process.Start("http://www.cjf.gob.mx");
        }



 
    }
}
