using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using mx.gob.scjn.electoral_common.TO;
using mx.gob.scjn.electoral_common.TO.Comparador;
using mx.gob.scjn.electoral_common.gui.impresion;
using mx.gob.scjn.ius_common.TO;
using mx.gob.scjn.ius_common.TO.Comparador;
using mx.gob.scjn.ius_common.fachade;
using mx.gob.scjn.ius_common.gui.gui.impresion;
using mx.gob.scjn.ius_common.gui.utils;
using mx.gob.scjn.ius_common.utils;

namespace mx.gob.scjn.electoral.Controller.Impl
{
    public class TablaResultadosEjecutoriaControllerImpl:ITablaResultadoEjecutoriaController
    {
        public TablaResultadoEjecutoria Ventana { get; set; }
        /// <summary>
        /// La lista que se mostrará dentro del control de DataGrid
        /// </summary>
        List<EjecutoriaSimplificadaElectoralTO> resultadoEjecutorias;
        /// <summary>
        /// La lista que se mostrará dentro del control de DataGrid, una vez ordenada.
        /// </summary>
        List<EjecutoriaSimplificadaElectoralTO> muestraActual;
        protected bool mostrarVP { get; set; }
        protected bool verFlechas { get; set; }
        private bool impreso { get; set; }
        protected bool impresionCancelada { get; set; }
        protected bool verEjecutoriasOrdenarPor { get; set; }
        public String OrdenarPor { get { return this.getOrdernarPor(); } set { this.setOrdenarPor(value); } }
        private String ordenarPor;
        
        /// <summary>
        /// Los parámetros de la búsqueda.
        /// </summary>
        BusquedaTO busqueda;
        /// <summary>
        /// Documento que se imprimirá
        /// </summary>
        DocumentoListadoEjecutoriaElectoral documentoImpresion { get; set; }
        //private FlowDocument documentoListadoimpresion;
        BackgroundWorker worker;
        
        public TablaResultadosEjecutoriaControllerImpl(TablaResultadoEjecutoria ventana)
        {
            Ventana = ventana;
            mostrarVP = true;
            Ventana.ListaOrdenar.Padre = Ventana;
            buscaTesis();
            Ventana.Title = "Resultado de la Búsqueda";
            Ventana.tablaResultado.ItemsSource = resultadoEjecutorias;
            muestraActual = resultadoEjecutorias;
            Ventana.RegistrosLabel.Content = "Registros: " + resultadoEjecutorias.Count;
        }

        
        /// <summary>
        /// Constructor para la búsqueda por registros.
        /// </summary>
        /// <param name="identificadores">Los registros a desplegar</param>
        public TablaResultadosEjecutoriaControllerImpl(TablaResultadoEjecutoria ventana,int[] identificadores)
        {
            Ventana=ventana;
            mostrarVP = true;
            Ventana.ListaOrdenar.Padre = Ventana;
            Ventana.Expresion.Text = "Busqueda por Registros";
            MostrarPorIusTO parametros = new MostrarPorIusTO();
            parametros.OrderBy = "Consec";
            parametros.OrderType = "asc";
            resultadoEjecutorias = new List<EjecutoriaSimplificadaElectoralTO>();
#if STAND_ALONE
            FachadaBusquedaTradicional fachada = new FachadaBusquedaTradicional();
            parametros.Listado = identificadores.ToList();
            List<EjecutoriasTO> arreglo = fachada.getEjecutoriasElectoralPorIds(parametros);
#else
            FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
            parametros.Listado = identificadores;
            EjecutoriasTO[] arreglo = fachada.getEjecutoriasElectoralPorIds(parametros);
#endif
            foreach (EjecutoriasTO item in arreglo)
            {
                EjecutoriaSimplificadaElectoralTO ejecutoriaActual = new EjecutoriaSimplificadaElectoralTO();
                ejecutoriaActual.Id = item.Id;
                resultadoEjecutorias.Add(ejecutoriaActual);
            }
            Ventana.tablaResultado.ItemsSource = resultadoEjecutorias;
            muestraActual = resultadoEjecutorias;
            fachada.Close();
            if (Ventana.tablaResultado.Items.Count > 1)
            {
                verFlechas = true;
            }
            else
            {
                verFlechas = false;
                Ventana.RegistrosLabel.Visibility = Visibility.Hidden;
                Ventana.btnIrA.Visibility = Visibility.Hidden;
                Ventana.IrANum.Visibility = Visibility.Hidden;
                Ventana.lblIrA.Visibility = Visibility.Hidden;
                Ventana.inicio.Visibility = Visibility.Hidden;
                Ventana.anterior.Visibility = Visibility.Hidden;
                Ventana.siguiente.Visibility = Visibility.Hidden;
                Ventana.final.Visibility = Visibility.Hidden;
            }
            Ventana.RegistrosLabel.Content = "Registros: " + resultadoEjecutorias.Count;
        }

        /// <summary>
        /// Constructor para la búsqueda ordinaria de secuenciales por panel
        /// </summary>
        /// <param name="buscar">Los parámetros del panel de búsqueda</param>
        public TablaResultadosEjecutoriaControllerImpl(TablaResultadoEjecutoria ventana,BusquedaTO buscar)
        {
            Ventana = ventana;
            mostrarVP = true;
            Ventana.ListaOrdenar.Padre = Ventana;
            Ventana.Title = "Buscando secuencialmente";
            Ventana.Expresion.Text = "Sentencias electorales";
            Ventana.Expresion.IsEnabled = false;
            busqueda = buscar;
            buscaTesis();
            Ventana.tablaResultado.ItemsSource = resultadoEjecutorias;
            muestraActual = resultadoEjecutorias;
            Ventana.RegistrosLabel.Content = "Registros: " + resultadoEjecutorias.Count;
        }
        /// <summary>
        /// Busca las tesis por omisión de las pruebas
        /// </summary>
        /// <returns>La lista de las ejecutorias que coinciden 
        /// con los parámetros de la búsqueda</returns>
        protected List<EjecutoriaSimplificadaElectoralTO> buscaTesis()
        {
            if (busqueda == null)
            {
                busqueda = new BusquedaTO();
                Boolean[][] epocas = new Boolean[7][];
                for (int i = 0; i < 7; i++)
                {
                    epocas[i] = new Boolean[6];
                    for (int j = 0; j < 6; j++)
                    {
                        if (i == 0 && j == 1)
                        {
                            epocas[i][j] = true;
                        }
                        else
                        {
                            epocas[i][j] = false;
                        }
                    }
                }
                busqueda.Epocas = epocas;
                busqueda.OrdenarPor = "ConsecIndx";
            }
            busqueda.TipoBusqueda = Constants.BUSQUEDA_EJECUTORIAS;
            Boolean browserHosted = BrowserInteropHelper.IsBrowserHosted;
#if STAND_ALONE
            FachadaBusquedaTradicional fachada = new FachadaBusquedaTradicional();
            List<EjecutoriasTO> ejecutoriasObtenidas = null;
#else
            FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
            EjecutoriasTO[] ejecutoriasObtenidas = null;
#endif
            try
            {
                ejecutoriasObtenidas = fachada.getIdConsultaPanelElectoral(busqueda).Ejecutorias;
                resultadoEjecutorias = new List<EjecutoriaSimplificadaElectoralTO>();
                foreach (EjecutoriasTO actual in ejecutoriasObtenidas)
                {
                    EjecutoriaSimplificadaElectoralTO ejecutoriaActual = new EjecutoriaSimplificadaElectoralTO();
                    ejecutoriaActual.Id = actual.Id;
                    ejecutoriaActual.OrdenarAsunto = actual.OrdenarAsunto;
                    ejecutoriaActual.OrdenarPromovente = actual.OrdenarPromovente;
                    ejecutoriaActual.ConsecIndx = actual.ConsecIndx;
                   resultadoEjecutorias.Add(ejecutoriaActual);
                }
                fachada.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                resultadoEjecutorias = new List<EjecutoriaSimplificadaElectoralTO>();
            }

            return resultadoEjecutorias;
        }
        #region ITablaResultadoEjecutoriaController Members

        public void PaginaCargada()
        {
            if (Ventana.tablaResultado.SelectedItem != null)
            {
                Ventana.tablaResultado.BringItemIntoView(Ventana.tablaResultado.SelectedItem);
            }
            else
            {
                Ventana.tablaResultado.SelectedIndex = 0;
                Ventana.tablaResultado.BringItemIntoView(Ventana.tablaResultado.SelectedItem);
            }
        }

        public void InicioClic()
        {
            Ventana.tablaResultado.SelectedIndex = 0;
            Ventana.tablaResultado.BringItemIntoView(Ventana.tablaResultado.SelectedItem);
        }

        public void AnteriorClic()
        {
            if (Ventana.tablaResultado.SelectedIndex > 0)
            {
                Ventana.tablaResultado.SelectedIndex--;
            }
            Ventana.tablaResultado.BringItemIntoView(Ventana.tablaResultado.SelectedItem);
        }

        public void SiguienteClic()
        {
            if (Ventana.tablaResultado.Items.Count > Ventana.tablaResultado.SelectedIndex)
            {
                Ventana.tablaResultado.SelectedIndex++;
            }
            Ventana.tablaResultado.BringItemIntoView(Ventana.tablaResultado.SelectedItem);
        }

        public void FinalClic()
        {
            Ventana.tablaResultado.SelectedIndex = Ventana.tablaResultado.Items.Count - 1;
            Ventana.tablaResultado.BringItemIntoView(Ventana.tablaResultado.SelectedItem);
        }

        public void OrdenaClic()
        {
            Ventana.ListaOrdenar.Visibility = Visibility.Visible;
        }

        public void IrAClic()
        {
            if (Ventana.IrANum.Text.Equals(""))
            {
                MessageBox.Show(Mensajes.MENSAJE_CAMPO_TEXTO_NUMERO_VACIO, Mensajes.TITULO_CAMPO_REQUERIDO,
                    MessageBoxButton.OK, MessageBoxImage.Error);
                Ventana.IrANum.Focus();
                return;
            }
            int registroSeleccionado = Int32.Parse(Ventana.IrANum.Text);
            registroSeleccionado--;
            if ((Ventana.tablaResultado.Items.Count > registroSeleccionado)
                || (registroSeleccionado < 0))
            {
                Ventana.tablaResultado.SelectedIndex = registroSeleccionado;
                Ventana.IrANum.Text = "";
            }
            else
            {
                MessageBox.Show(Mensajes.MENSAJE_CONSECUTIVO_NO_VALIDO,
                    Mensajes.TITULO_CONSECUTIVO_NO_VALIDO, MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
            Ventana.tablaResultado.BringItemIntoView(Ventana.tablaResultado.SelectedItem);
        }

        public void ImprimirClic()
        {
            if (Ventana.impresionViewer.Visibility.Equals(Visibility.Hidden))
            {
                Ventana.imprimir.ToolTip = Constants.VISTA_PRELIMINAR_FUERA;
                MessageBoxResult resultadoAdv = MessageBoxResult.Yes;
                if (Ventana.tablaResultado.Items.Count > 2000)
                {
                    resultadoAdv = MessageBox.Show(Mensajes.MENSAJE_MUCHOS_REGISTROS,
                         Mensajes.TITULO_MUCHOS_REGISTROS, MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
                }
                if (resultadoAdv == MessageBoxResult.Yes)
                {
                    documentoImpresion = null;
                    Ventana.impresionViewer.Document = null;
                    //EsperaBarra.Maximum = tablaResultados.Items.Count;
                    Ventana.EsperaBarra.Value = 0;
                    Ventana.Esperar.Visibility = Visibility.Visible;
                    //FlowDocumentReader documentoAEscribir = new FlowDocumentReader();
                    //documentoListadoimpresion = null;
                    worker = new BackgroundWorker() { WorkerReportsProgress = true, WorkerSupportsCancellation = true };
                    worker.ProgressChanged += new ProgressChangedEventHandler(worker_ProgressChanged);
                    worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(worker_RunWorkerCompleted);
                    worker.DoWork += new DoWorkEventHandler(worker_DoWork);
                    worker.RunWorkerAsync();
                    Ventana.Cursor = Cursors.Wait;
                }
            }
            else
            {
                Ventana.imprimir.ToolTip = Constants.VistaPreliminar;
                Ventana.tablaResultado.Visibility = Visibility.Visible;
                Ventana.impresionViewer.Visibility = Visibility.Hidden;
                Ventana.imprimePapel.Visibility = Visibility.Hidden;
            }
        }

        public void GuardarClic()
        {
            mostrarVP = false;
            if (!BrowserInteropHelper.IsBrowserHosted)
            {
                ImprimirClic();
                while (!impreso)
                {
                    System.Windows.Forms.Application.DoEvents();
                }
                if (!impresionCancelada)
                {
                    Microsoft.Win32.SaveFileDialog guardaEn = new Microsoft.Win32.SaveFileDialog();
                    guardaEn.DefaultExt = ".rtf";
                    guardaEn.Title = "Guardar listado de ejecutorias";
                    guardaEn.Filter = "Archivos de Texto Enriquecido|*.rtf";
                    guardaEn.AddExtension = true;
                    EsconderImpresionPrel();
                    mostrarVP = true;
                    if ((bool)guardaEn.ShowDialog())
                    {
                        FlowDocument documentoImprimir = DocumentoListadoEjecutoriaElectoral.generaDocumento(documentoImpresion.ListaImpresion);
                        Ventana.impresionViewer.Document = null;
                        Ventana.Contenido.Document = documentoImprimir;
                        try
                        {
                            System.IO.FileStream archivo = new System.IO.FileStream(guardaEn.FileName, System.IO.FileMode.Create);
                            Ventana.Contenido.SelectAll();
                            Ventana.Contenido.Selection.Save(archivo, System.Windows.DataFormats.Rtf);
                            archivo.Flush();
                            archivo.Close();
                            MessageBox.Show(Mensajes.MENSAJE_ARCHIVO_GUARDADO + archivo.Name,
                                Mensajes.TITULO_ARCHIVO_GUARDADO, MessageBoxButton.OK,
                                MessageBoxImage.Information);
                        }
                        catch (Exception exc)
                        {
                            MessageBox.Show(Mensajes.MENSAJE_ARCHIVO_ABIERTO, Mensajes.TITULO_ARCHIVO_ABIERTO,
                               MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
            }
        }

        public void TablaResultadoDobleClic()
        {
            Ejecutoria muestraEjecutoria = new Ejecutoria(Ventana.tablaResultado, busqueda);
            Page pagina = (Page)Application.Current.MainWindow.Content;
            muestraEjecutoria.Back = pagina;
            pagina.NavigationService.Navigate(muestraEjecutoria);
        }

        public void SalirClic()
        {
            Ventana.NavigationService.Navigate(Ventana.Back);
        }

        public void TablaResultadoCambio()
        {
            int registroActual = Ventana.tablaResultado.SelectedIndex + 1;
            Ventana.RegistrosLabel.Content = registroActual + "/" + Ventana.tablaResultado.Items.Count;            
        }

        public void ImprimePapelClic()
        {
            PrintDialog dialogoImpresion = new PrintDialog();
            IDocumentPaginatorSource paginado = Ventana.impresionViewer.Document as IDocumentPaginatorSource;
            DocumentPaginator pgn = paginado.DocumentPaginator;
            dialogoImpresion.UserPageRangeEnabled = true;
            dialogoImpresion.MinPage = 1;
            dialogoImpresion.MaxPage = (uint)pgn.PageCount;
            Nullable<Boolean> respuesta = dialogoImpresion.ShowDialog();
            //Debe mandar a la impresora con las opciones del usuario
            if (respuesta == true)
            {
                try
                {
                    if (dialogoImpresion.PageRangeSelection == PageRangeSelection.UserPages)
                    {
                        ((DocumentPaginatorWrapper)pgn).PaginaInicial = dialogoImpresion.PageRange.PageFrom;
                        ((DocumentPaginatorWrapper)pgn).PaginaFinal = dialogoImpresion.PageRange.PageTo;
                    }
                    dialogoImpresion.PrintDocument(pgn, "Impresión de la tabla de resultados");

                    EsconderImpresionPrel();
                }
                catch (Exception exc)
                {
                    MessageBox.Show(Mensajes.MENSAJE_IMPRESORA, Mensajes.TITULO_ARCHIVO_ABIERTO,
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        public void TacheClic()
        {
            EsconderImpresionPrel();
        }

        public void CancelarClic()
        {
            MessageBoxResult res = MessageBox.Show(Mensajes.MENSAJE_CANCELAR, Mensajes.TITULO_CANCELAR,
                        MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
            if (res.Equals(MessageBoxResult.Yes))
            {
                worker.CancelAsync();
                DocumentoListadoEjecutoriaElectoral.cancelado = true;
            }
        }

        #endregion


        #region BackgroundWorker Events

        //This event is fired on the background thread, and is where you would do all your work 
        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            impreso = false;
            // List<EjecutoriasTO> resultado = null;
            DocumentoListadoEjecutoriaElectoral generador = new DocumentoListadoEjecutoriaElectoral(Ventana.tablaResultado.Items, worker);
            //resultado = generador.ListaImpresion;
            if (worker.CancellationPending)
            {
                e.Cancel = true;
            }
            e.Result = generador;
        }
        /// <summary>
        /// Define que hacer cuando va avanzando el progreso del trabajador.
        /// </summary>
        /// <param name="sender">Quien lo envia</param>
        /// <param name="e">Datos del progreso</param>
        private void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            Ventana.EsperaBarra.Value = e.ProgressPercentage;
        }
        /// <summary>
        /// Que hacer cuando se termine el trabajo, en este caso la generación de los datos de impresión.
        /// </summary>
        /// <param name="sender">Quien lo envía</param>
        /// <param name="e">Datos de como se completó.</param>
        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            impresionCancelada = false;
            if (e.Cancelled)
            {
                Ventana.tablaResultado.Visibility = Visibility.Visible;
                Ventana.impresionViewer.Visibility = Visibility.Hidden;
                this.impresionCancelada = true;
            }
            else if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message, "Error al generar el documento", MessageBoxButton.OK, MessageBoxImage.Error);
                Ventana.tablaResultado.Visibility = Visibility.Visible;
                Ventana.impresionViewer.Visibility = Visibility.Hidden;
                Ventana.imprimePapel.Visibility = Visibility.Hidden;
            }
            else
            {
                //Package pack = PackageStore.GetPackage(new Uri("pack://temp.xps"));
                //PackagePart documento =  pack.GetPart(new Uri("/temp.xps", UriKind.Relative));
                DocumentoListadoEjecutoriaElectoral generador = (DocumentoListadoEjecutoriaElectoral)e.Result;
                List<EjecutoriasTO> resultado = generador.ListaImpresion;
                documentoImpresion = generador;
                Ventana.TxbEspere.Text = Mensajes.GENERANDO_DOCUMENTO;
                //Ordenar la lista antes de generar el documento
                String ordenacion = ordenarPor == null ? "consecindx" : ordenarPor;
                switch (ordenacion.ToLower())
                {
                    case Constants.ORDENAR_CONSEC:
                        IComparer<EjecutoriasTO> comparador = new EjecutoriasConsecIndxNSTOComp();
                        resultado.Sort(comparador);
                        break;
                    case Constants.ORDENAR_ID:
                        comparador = new EjecutoriasIdNSTOComp();
                        resultado.Sort(comparador);
                        break;
                }

                Ventana.impresionViewer.Document = DocumentoListadoEjecutoriaElectoral.generaDocumento(resultado);
                if (!DocumentoListadoEjecutoriaElectoral.cancelado)
                {
                    Ventana.impresionViewer.Background = Brushes.White;
                    //resultado.Background = Brushes.White;
                    if (mostrarVP)
                    {
                        MostrarImpresionPrel();
                    }
                }
            }
            impreso = true;
            Ventana.Esperar.Visibility = Visibility.Collapsed;
            Ventana.Cursor = Cursors.Arrow;
        }
        #endregion
        #region vistapreliminar
        private void MostrarImpresionPrel()
        {
            verFlechas = (Ventana.tablaResultado.Items.Count > 0);
            Ventana.BtnDisminuyeAlto.Visibility = Visibility.Hidden;
            Ventana.BtnAumentaAlto.Visibility = Visibility.Hidden;
            Ventana.BtnGuardar.Visibility = Visibility.Hidden;
            Ventana.BtnVisualizar.Visibility = Visibility.Hidden;
            verEjecutoriasOrdenarPor = Ventana.ListaOrdenar.Visibility == Visibility.Visible;
            Ventana.ListaOrdenar.Visibility = Visibility.Hidden;
            Ventana.Titulo.Visibility = Visibility.Hidden;
            Ventana.RegistrosLabel.Visibility = Visibility.Hidden;
            Ventana.Expresion.Visibility = Visibility.Hidden;
            Ventana.tablaResultado.Visibility = Visibility.Hidden;
            Ventana.impresionViewer.Visibility = Visibility.Visible;
            Ventana.imprimePapel.Visibility = Visibility.Visible;
            Ventana.IrANum.Visibility = Visibility.Hidden;
            Ventana.lblIrA.Visibility = Visibility.Hidden;
            Ventana.btnIrA.Visibility = Visibility.Hidden;
            Ventana.imprimir.ToolTip = Constants.VISTA_PRELIMINAR_FUERA;
            Ventana.imprimir.Visibility = Visibility.Hidden;
            Ventana.salir.Visibility = Visibility.Hidden;
            Ventana.BtnTache.Visibility = Visibility.Visible;
            Ventana.imprimePapel.Visibility = Visibility.Visible;
            Ventana.inicio.Visibility = Visibility.Hidden;
            Ventana.anterior.Visibility = Visibility.Hidden;
            Ventana.siguiente.Visibility = Visibility.Hidden;
            Ventana.final.Visibility = Visibility.Hidden;
            //Ventana.Ordena.Visibility = Visibility.Hidden;
        }
        private void EsconderImpresionPrel()
        {
            Ventana.BtnDisminuyeAlto.Visibility = Visibility.Visible;
            Ventana.BtnAumentaAlto.Visibility = Visibility.Visible;
            if (!BrowserInteropHelper.IsBrowserHosted) Ventana.BtnGuardar.Visibility = Visibility.Visible;
            Ventana.BtnVisualizar.Visibility = Visibility.Visible;
            if (verEjecutoriasOrdenarPor) Ventana.ListaOrdenar.Visibility = Visibility.Visible;
            Ventana.Titulo.Visibility = Visibility.Visible;
            Ventana.Expresion.Visibility = Visibility.Visible;
            Ventana.imprimir.ToolTip = Constants.VistaPreliminar;
            Ventana.imprimir.Visibility = Visibility.Visible;
            Ventana.salir.Visibility = Visibility.Visible;
            Ventana.BtnTache.Visibility = Visibility.Hidden;
            Ventana.tablaResultado.Visibility = Visibility.Visible;
            Ventana.impresionViewer.Visibility = Visibility.Hidden;
            Ventana.imprimePapel.Visibility = Visibility.Hidden;
            if (verFlechas)
            {
                Ventana.IrANum.Visibility = Visibility.Visible;
                Ventana.btnIrA.Visibility = Visibility.Visible;
                Ventana.RegistrosLabel.Visibility = Visibility.Visible;
                Ventana.lblIrA.Visibility = Visibility.Visible;
                Ventana.inicio.Visibility = Visibility.Visible;
                Ventana.anterior.Visibility = Visibility.Visible;
                Ventana.siguiente.Visibility = Visibility.Visible;
                Ventana.final.Visibility = Visibility.Visible;
            }
            //Ventana.Ordena.Visibility = Visibility.Visible;
        }

        #endregion
        /// <summary>
        /// Define por que campo se ordenará la lista de resultados
        /// </summary>
        /// <param name="value">El valor del campo de ordenamiento</param>
        private void setOrdenarPor(string value)
        {

            switch (value.ToLower())
            {
                case Constants.ORDENAR_PROMOVENTE:
                    IComparer<EjecutoriaSimplificadaElectoralTO> comparador = new EjecutoriaElectoralTOComp();
                    muestraActual.Sort(comparador);
                    Ventana.tablaResultado.ItemsSource = null;
                    Ventana.tablaResultado.ItemsSource = muestraActual;
                    break;
                case Constants.ORDENAR_CONSEC:
                    comparador = new EjecutoriasConsecIndxElectoralTOComp();
                    this.muestraActual.Sort(comparador);
                    Ventana.tablaResultado.ItemsSource = null;
                    Ventana.tablaResultado.ItemsSource = muestraActual;
                    break;
                case Constants.ORDENAR_ID:
                    comparador = new EjecutoriasIdElectoralTOComp();
                    this.muestraActual.Sort(comparador);
                    Ventana.tablaResultado.ItemsSource = null;
                    Ventana.tablaResultado.ItemsSource = muestraActual;
                    break;
                case Constants.ORDENAR_ASUNTO:
                    comparador = new EjecutoriasRubroElectoralTOComp();
                    this.muestraActual.Sort(comparador);
                    Ventana.tablaResultado.ItemsSource = null;
                    Ventana.tablaResultado.ItemsSource = muestraActual;
                    break;
            }
            Ventana.tablaResultado.SelectedIndex = 0;
            ordenarPor = value;
        }

        private string getOrdernarPor()
        {
            return this.ordenarPor;
        }

    }
}
