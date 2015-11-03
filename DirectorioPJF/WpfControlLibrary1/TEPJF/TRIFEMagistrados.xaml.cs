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

namespace mx.gob.scjn.directorio.TEPJF
{

    /// <summary>
    /// Interaction logic for TRIFEMagistrados.xaml
    /// </summary>
    public partial class TRIFEMagistrados : Page
    {
        String enterKey = "\n";
        /*Para la impresión ***********************************************/

        private FlowDocument DocumentoParaCopiar { get; set; }
        private AcuerdosTO DocumentoActual;
        String strMinImpr = "";
        /******************************************************************/
        List<DirectorioMinistrosTO> lstResImpr = new List<DirectorioMinistrosTO>();

        public TRIFEMagistrados()
        {
            InitializeComponent();

            if (!BrowserInteropHelper.IsBrowserHosted)
            { Guardar_.Visibility = Visibility.Visible; }
            else { Guardar_.Visibility = Visibility.Hidden; }
            lstResImpr = LlenaGridMagistrados();
        }

        public Page Back { get; set; }

        private List<DirectorioMinistrosTO> LlenaGridMagistrados()
        {
            List<DirectorioMinistrosTO> lstRes = new List<DirectorioMinistrosTO>();
#if STAND_ALONE
            List<DirectorioMinistrosTO> R = new List<DirectorioMinistrosTO>();
            FachadaBusquedaTradicional fachada = new FachadaBusquedaTradicional();
#else
            DirectorioMinistrosTO[] R = new DirectorioMinistrosTO[50];
            FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
#endif
            R = fachada.getDirMagistradosXFiltro("0");
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
            this.Magistrados.ItemsSource = lstRes;
            fachada.Close();
            return lstRes;
        }

        private void Ver_Semblanza(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (Magistrados.CurrentItem!=null)
                {
                    Semblanza TRIFESemblanza = new Semblanza(Magistrados.CurrentItem);
                    TRIFESemblanza.Back = this;
                    this.NavigationService.Navigate(TRIFESemblanza);   
                }

                
            }
            catch (System.Exception error)
            {
                //Handle exception here
            }
        }

        private void Salir_MouseButtonDown(object sender, MouseButtonEventArgs e)
        {

            //if (Back == null)
            //{
            //    this.NavigationService.GoBack();
            //}
            //else
            //{
            //    this.NavigationService.Navigate(Back);
            //}
        }

        private void Guardar(object sender, MouseButtonEventArgs e)
        {
            //ImprimeListado(true, 1);
        }

        private void PortaPapeles(object sender, MouseButtonEventArgs e)
        {
            //Boolean bOK = false;

            //BloqueaControles(true);
            //bOK =  ImprimeListado(true, 0);

            //BloqueaControles(bOK);
        }

        private void ArreglaDoc(RichTextBox rtbTexto)
        {
            rtbTexto.Width = 950;
            rtbTexto.Height = 1000;
        }

        private void Imprimir(object sender, MouseButtonEventArgs e)
        {
            //ImprimeListado(true, 2);
        }

        public Boolean ImprimeListado(Boolean bTodo, Int32 nSelSalida)
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
                    GuardarEnArchivo(strText);
                    break;

                case 2: //Imprimir
                    ImprimeStr(strText);
                    break;
            }

            return true;
        }

        private void ImprimeStr(String strTexto)
        {

            try
            {

                DocumentoAcuerdoDirec documento;

                documento = new DocumentoAcuerdoDirec(strTexto);

                DocumentoParaCopiar = documento.Copia;
                
                
                
                impresion.Document = documento.Documento; //(IDocumentPaginatorSource)documentoXps;
                impresion.Visibility = Visibility.Hidden;
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
            Aviso.BringIntoView(); 
            Aviso.Background = Brushes.White;

            Aviso.TomaFondo(FTransparente);
        }

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

        private void image1_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
        }

        private void TRIFE_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            if (BrowserInteropHelper.IsBrowserHosted)
            {
                MessageBox.Show(MensajesDirectorio.MENSAJE_NO_SE_PUEDE_VER_CORTE, MensajesDirectorio.TITULO_NO_SE_PUEDE_VER_CORTE,
                    MessageBoxButton.OK, MessageBoxImage.Information);
                //Uri lugar = new Uri("pack://siteofOrigin:,,,/Anexos/SCJN.htm", UriKind.RelativeOrAbsolute);
                //NavigationService.Navigate(lugar);
            }
            else
            {
                LlamaTRIFE();
            }
        }

        private void LlamaTRIFE()
        {
            Process.Start("http://www.trife.gob.mx");
        }

        private void txtTituloSemblanza_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            if (BrowserInteropHelper.IsBrowserHosted)
            {
                MessageBox.Show(MensajesDirectorio.MENSAJE_NO_SE_PUEDE_VER_CORTE, MensajesDirectorio.TITULO_NO_SE_PUEDE_VER_CORTE,
                    MessageBoxButton.OK, MessageBoxImage.Information);
                //Uri lugar = new Uri("pack://siteofOrigin:,,,/Anexos/SCJN.htm", UriKind.RelativeOrAbsolute);
                //NavigationService.Navigate(lugar);
            }
            else
            {
                LlamaTRIFE();
            }
        }

        private void BloqueaControles(Boolean bMostrar) {
            if (bMostrar)
            {
                ////Height="510" Width="737.5"
                //CuadroSombra.Height = 510;// this.Height;
                //CuadroSombra.Width = 750;// this.Width;
                //CuadroSombra.Visibility = Visibility.Visible;
                //CuadroSombra.BringIntoView();
                ////CuadroSombra. Background = Brushes.Black;
                //CuadroSombra.Opacity = 0.30;
            }
            else {

                //CuadroSombra.Height = 1;
                //CuadroSombra.Width = 1;
                //CuadroSombra.Visibility = Visibility.Hidden;
                 
            
            }



        }

        private void Imprimir_Click(object sender, RoutedEventArgs e)
        {
            ImprimeListado(true, 2);

        }

        private void Guardar_Click(object sender, RoutedEventArgs e)
        {
            ImprimeListado(true, 1);

        }

        private void PortaPapeles_Click(object sender, RoutedEventArgs e)
        {
            Boolean bOK = false;

            BloqueaControles(true);
            bOK = ImprimeListado(true, 0);

            BloqueaControles(bOK);

        }

        private void Salir_MouseButtonDown(object sender, RoutedEventArgs e)
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

       


    }
}
