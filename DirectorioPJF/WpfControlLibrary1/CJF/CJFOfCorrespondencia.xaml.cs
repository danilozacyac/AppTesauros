using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using mx.gob.scjn.ius_common.TO;
using mx.gob.scjn.ius_common.fachade;

namespace mx.gob.scjn.directorio.CJF
{

    /// <summary>
    /// Interaction logic for CJFOfCorrespondencia.xaml
    /// </summary>
    public partial class CJFOfCorrespondencia : Page
    {

        public Page Back { get; set; }
        public Page contenedor { get; set; }
        String enterKey = "\n";
        List<DirectorioOrgJurTO> lstRes = new List<DirectorioOrgJurTO>();
        String strOCActual = "";

        public CJFOfCorrespondencia()
        {
            InitializeComponent();
            TraeOC();

            if (!BrowserInteropHelper.IsBrowserHosted)
            { Guardar_.Visibility = Visibility.Visible; }
            else { Guardar_.Visibility = Visibility.Hidden; }
        }

        private List<DirectorioOrgJurTO> TraeOC()
        {
            List<String> aCto = new List<String>();
            aCto.Add(" ");
            aCto.Add("PRIMER CIRCUITO");
            aCto.Add("SEGUNDO CIRCUITO");
            aCto.Add("TERCER CIRCUITO");
            aCto.Add("CUARTO CIRCUITO");
            aCto.Add("QUINTO CIRCUITO");
            aCto.Add("SEXTO CIRCUITO");
            aCto.Add("SÉPTIMO CIRCUITO");
            aCto.Add("OCTAVO CIRCUITO");
            aCto.Add("NOVENO CIRCUITO");
            aCto.Add("DÉCIMO CIRCUITO");
            aCto.Add("DÉCIMO PRIMER CIRCUITO");
            aCto.Add("DÉCIMO SEGUNDO CIRCUITO");
            aCto.Add("DÉCIMO TERCER CIRCUITO");
            aCto.Add("DÉCIMO CUARTO CIRCUITO");
            aCto.Add("DÉCIMO QUINTO CIRCUITO");
            aCto.Add("DÉCIMO SEXTO CIRCUITO");
            aCto.Add("DÉCIMO SÉPTIMO CIRCUITO");
            aCto.Add("DÉCIMO OCTAVO CIRCUITO");
            aCto.Add("DÉCIMO NOVENO CIRCUITO");
            aCto.Add("VIGÉSIMO CIRCUITO");
            aCto.Add("VIGÉSIMO PRIMER CIRCUITO");
            aCto.Add("VIGÉSIMO SEGUNDO CIRCUITO");
            aCto.Add("VIGÉSIMO TERCER CIRCUITO");
            aCto.Add("VIGÉSIMO CUARTO CIRCUITO");
            aCto.Add("VIGÉSIMO QUINTO CIRCUITO");
            aCto.Add("VIGÉSIMO SEXTO CIRCUITO");
            aCto.Add("VIGÉSIMO SÉPTIMO CIRCUITO");
            aCto.Add("VIGÉSIMO OCTAVO CIRCUITO");
            aCto.Add("VIGÉSIMO NOVENO CIRCUITO");
            aCto.Add("TRIGÉSIMO CIRCUITO");
            //List<DirectorioOrgJurTO> lstRes = new List<DirectorioOrgJurTO>();
#if STAND_ALONE
            List<DirectorioOrgJurTO> R = null;
            FachadaBusquedaTradicional fachada = new FachadaBusquedaTradicional();
#else
            DirectorioOrgJurTO[] R = new DirectorioOrgJurTO[50];
            FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
#endif
            R = fachada.getDirOfCorrespondencia();
#if STAND_ALONE
            for (int i = 0; i < R.Count; i++)
#else
            for (int i = 0; i < R.Length; i++)
#endif
            {
                DirectorioOrgJurTO Campo = new DirectorioOrgJurTO();
                Campo.IdOrganoJur = R[i].IdOrganoJur;
                Campo.NombreOrganoJur = R[i].NombreOrganoJur;
                Campo.DomOrganoJur = R[i].DomOrganoJur;
                Campo.TelOrganoJur = R[i].TelOrganoJur;
                Campo.NombreStrOrganoJur = R[i].NombreStrOrganoJur;
                Campo.TitularSolo = R[i].TitularSolo;
                Campo.TelOrganoJur = aCto[R[i].IdCto];
                lstRes.Add(Campo);
                //vamos a usar esta variable para poder ubicar los nombres en el texto de acceso rapido
                string tmp = R[i].NombreStrOrganoJur;
                //tmp = tmp.Replace(" x ", "");
                //tmp = tmp.Trim();
                //strArrNC.Add(tmp);
                Campo = null;
            }
            this.AreasAdmin.ItemsSource = lstRes;
            this.AreasAdmin.SelectedIndex = 0;
            //textCuantos.Text = this.AreasAdmin.Items.Count.ToString();   // strArrNC.Count.ToString();
            //CargaDetalleAA((DirectorioOrgJurTO)AreasAdmin.SelectedItem);
            fachada.Close();
            return lstRes;
        }

        private void CargaDetalleOC(DirectorioOrgJurTO Campo)
        {

            try
            {
                strOCActual = Campo.NombreOrganoJur + enterKey;
                strOCActual = strOCActual + Campo.DomOrganoJur;
                Campo = null;
            }

            catch { }
        }

        private void CargaDetalle(object sender, MouseButtonEventArgs e)
        {

            try
            {
                CargaDetalleOC((DirectorioOrgJurTO)AreasAdmin.SelectedItem);
            }

            catch { }
        }

        private void txtAccesoRapido_GotFocus(object sender, RoutedEventArgs e)
        {
        }

        private void txtAccesoRapido_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
        }

        private void txtAccesoRapido_TextChanged(object sender, TextChangedEventArgs e)
        {
        }

        private void Imprimir(object sender, MouseButtonEventArgs e)
        {
        }

        private void Salir(object sender, MouseButtonEventArgs e)
        {
        }

        private void Guardar(object sender, MouseButtonEventArgs e)
        {
            //OpcionesImprimir opMinistros = new OpcionesImprimir();
            //opMinistros.Visibility = Visibility.Visible;
            //OpImprimir.StrCabecera = "Opciones de almacenamiento";
            //OpImprimir.StrMensaje = "Seleccione qué es lo que se va a guardar en disco ";
            //OpImprimir.StrActual = "La oficina actual";
            //OpImprimir.StrOpcionTodos = "Todas las oficinas";
            //OpImprimir.OptSalida = 1;
            //OpImprimir.BringIntoView();
            //this.OpImprimir.contenedor = this;
            //OpImprimir.Visibility = Visibility.Visible;
        }

        private void Guardar__MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //OpcionesImprimir opMinistros = new OpcionesImprimir();
            //opMinistros.Visibility = Visibility.Visible;

            //FTransparente.Visibility = Visibility.Visible;
            //OpImprimir.TomaFondo(FTransparente);

            
            //OpImprimir.StrCabecera = "Opciones de almacenamiento";
            //OpImprimir.StrMensaje = "Seleccione lo que se va a guardar en disco ";
            //OpImprimir.StrActual = "La oficina actual";
            //OpImprimir.StrOpcionTodos = "Todas las oficinas";
            //OpImprimir.OptSalida = 1;
            //OpImprimir.BringIntoView();
            //this.OpImprimir.contenedor = this;
            //OpImprimir.Visibility = Visibility.Visible;
        }

        private void Imprimir__MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //String strCabecera = "Opciones de impresión";
            //String strMensaje = "Seleccione lo que se va a imprimir";
            //String strActual = "La oficina actual";
            //String strTodos = "Todas las oficinas";

            //FTransparente.Visibility = Visibility.Visible;
            //OpImprimir.TomaFondo(FTransparente);

            
            //OpImprimir.StrCabecera = strCabecera;
            //OpImprimir.StrMensaje = strMensaje;
            //OpImprimir.StrActual = strActual;
            //OpImprimir.StrOpcionTodos = strTodos;
            //OpImprimir.OptSalida = 2;
            //OpImprimir.BringIntoView();
            //this.OpImprimir.contenedor = this;
            //OpImprimir.Visibility = Visibility.Visible;
        }

        private void PortaPapeles__MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //OpcionesImprimir opMinistros = new OpcionesImprimir();
            //opMinistros.Visibility = Visibility.Visible;

            //FTransparente.Visibility = Visibility.Visible;
            //OpImprimir.TomaFondo(FTransparente);


            //OpImprimir.StrCabecera = "Opciones de almacenamiento";
            //OpImprimir.StrMensaje = "Seleccione lo que se va a enviar al portapapeles ";
            //OpImprimir.StrActual = "La oficina actual";
            //OpImprimir.StrOpcionTodos = "Todas las oficinas";
            //OpImprimir.OptSalida = 0;
            //OpImprimir.BringIntoView();
            //this.OpImprimir.contenedor = this;
            //OpImprimir.Visibility = Visibility.Visible;
        }

        private void Salir__MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
        }

        public void ImprimeListado(Boolean bTodos, Int32 nSelSalida)
        {
            String strText = "";

            if (bTodos == false) //Imprimir sólo el actual
            {
                strText = strText + strOCActual + enterKey;
            }
            else  //Imprimir todos
            {

                foreach (DirectorioOrgJurTO ItemIntegrante in lstRes)
                {
                    //strText = strText + ItemIntegrante.TelOrganoJur + enterKey;
                    strText = strText + ItemIntegrante.NombreOrganoJur + enterKey;
                    strText = strText + ItemIntegrante.DomOrganoJur + enterKey;
                    strText = strText + enterKey;
                }
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
        }

        private void PortaPapStr(String strTexto)
        {
            contenidoTexto = new RichTextBox();
            this.contenidoTexto.AppendText(strTexto);
            this.contenidoTexto.SelectAll();
            contenidoTexto.Copy();
            FTransparente.Visibility = Visibility.Visible;
            Aviso.TomaFondo(FTransparente);

            Aviso.StrCabecera = mx.gob.scjn.directorio.MensajesDirectorio.TITULO_PORTAPAPELES_DIR;
            Aviso.StrMensaje = mx.gob.scjn.directorio.MensajesDirectorio.MENSAJE_PORTAPAPELES_DIR;
            Aviso.Visibility = Visibility.Visible;
            Aviso.Background = Brushes.White;
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

        private void ImprimeStr(String strTexto)
        {

            try
            {

                DocumentoAcuerdoDirec documento;

                documento = new DocumentoAcuerdoDirec(strTexto);
                //DocumentoParaCopiar = documento.Copia;
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

        private void PortaPapeles__ImageFailed(object sender, ExceptionRoutedEventArgs e)
        {

        }

        private void Guardar_Click(object sender, RoutedEventArgs e)
        {
            OpcionesImprimir opMinistros = new OpcionesImprimir();
            opMinistros.Visibility = Visibility.Visible;

            FTransparente.Visibility = Visibility.Visible;
            OpImprimir.TomaFondo(FTransparente);


            OpImprimir.StrCabecera = "Opciones de almacenamiento";
            OpImprimir.StrMensaje = "Seleccione lo que se va a guardar en disco ";
            OpImprimir.StrActual = "La oficina actual";
            OpImprimir.StrOpcionTodos = "Todas las oficinas";
            OpImprimir.OptSalida = 1;
            OpImprimir.BringIntoView();
            this.OpImprimir.contenedor = this;
            OpImprimir.Visibility = Visibility.Visible;
        }

        private void Imprimir_Click(object sender, RoutedEventArgs e)
        {
            String strCabecera = "Opciones de impresión";
            String strMensaje = "Seleccione lo que se va a imprimir";
            String strActual = "La oficina actual";
            String strTodos = "Todas las oficinas";

            FTransparente.Visibility = Visibility.Visible;
            OpImprimir.TomaFondo(FTransparente);


            OpImprimir.StrCabecera = strCabecera;
            OpImprimir.StrMensaje = strMensaje;
            OpImprimir.StrActual = strActual;
            OpImprimir.StrOpcionTodos = strTodos;
            OpImprimir.OptSalida = 2;
            OpImprimir.BringIntoView();
            this.OpImprimir.contenedor = this;
            OpImprimir.Visibility = Visibility.Visible;

        }

        private void PortaPapeles_Click(object sender, RoutedEventArgs e)
        {
            OpcionesImprimir opMinistros = new OpcionesImprimir();
            opMinistros.Visibility = Visibility.Visible;

            FTransparente.Visibility = Visibility.Visible;
            OpImprimir.TomaFondo(FTransparente);


            OpImprimir.StrCabecera = "Opciones de almacenamiento";
            OpImprimir.StrMensaje = "Seleccione lo que se va a enviar al portapapeles ";
            OpImprimir.StrActual = "La oficina actual";
            OpImprimir.StrOpcionTodos = "Todas las oficinas";
            OpImprimir.OptSalida = 0;
            OpImprimir.BringIntoView();
            this.OpImprimir.contenedor = this;
            OpImprimir.Visibility = Visibility.Visible;
        }
    }
}
