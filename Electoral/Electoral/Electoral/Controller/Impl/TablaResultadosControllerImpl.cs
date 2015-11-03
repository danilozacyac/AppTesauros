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
using System.Windows.Navigation;
using mx.gob.scjn.electoral_common.TO;
using mx.gob.scjn.electoral_common.TO.Comparador;
using mx.gob.scjn.electoral_common.gui.impresion;
using mx.gob.scjn.ius_common.TO;
using mx.gob.scjn.ius_common.fachade;
using mx.gob.scjn.ius_common.gui.gui.impresion;
using mx.gob.scjn.ius_common.gui.utils;
using mx.gob.scjn.ius_common.utils;

namespace mx.gob.scjn.electoral.Controller.Impl
{
    public class TablaResultadosControllerImpl:ITablaResultadosController
    {
        public TablaResultados Ventana { get; set; }
        public BusquedaTO Busqueda { get; set; }
        private bool Terminado { get; set; }
        private int PosicionActual { get; set; }
        protected List<TesisSimplificadaElectoralTO> resultadoTesis { get; set; }
        protected List<TesisSimplificadaElectoralTO> MuestraActual { get; set; }
        protected PaginadorTO PaginadorActual { get; set; }
        public bool FlechasMuestra { get; set; }
        public BackgroundWorker worker { get; set; }
        public String OrdenarPor { get { return this.ordenarPor; } set { this.setOrdenarPor(value); } }
        private String ordenarPor { get; set; }
        DocumentoListadoTesisElectoral documentoImpresion { get; set; }
        private FlowDocument documentoListadoimpresion { get; set; }
        protected bool Impreso { get; set; }
        protected MostrarPorIusTO BuscaEspecial { get; set; }
        public bool MostrarVP { get; set; }
        /// <summary>
        /// La fachada que se usa para traer de bloque en bloque las tesis a mostrar.
        /// </summary>
#if STAND_ALONE
        protected FachadaBusquedaTradicional fachadaThread;
#else
        protected FachadaBusquedaTradicionalClient fachadaThread;
#endif
        public TablaResultadosControllerImpl(TablaResultados ventana)
        {
            this.Ventana = ventana;
            MostrarVP = true;
        }

        public TablaResultadosControllerImpl(TablaResultados ventana, BusquedaTO busqueda)
        {
            this.Ventana = ventana;
            this.Busqueda = busqueda;
            MostrarVP = true;
            BuscaTesis();
            Ventana.Expresion.Text = "Tesis Electorales";
        }

        public TablaResultadosControllerImpl(TablaResultados ventana, List<int> Identificadores)
        {
            this.Ventana = ventana;
            MostrarVP = true;
            MostrarVP = true;
            Ventana.Expresion.Text = "Tesis Electorales por registro";
            BuscaEspecial = new MostrarPorIusTO();
            BuscaEspecial.BusquedaEspecialValor = null;
            BuscaEspecial.FilterBy = null;
            BuscaEspecial.FilterValue = null;
            BuscaEspecial.Letra = 0;
#if STAND_ALONE
            BuscaEspecial.Listado = Identificadores;
#else
            BuscaEspecial.Listado = Identificadores.ToArray();
#endif
            BuscaEspecial.OrderBy = Constants.ORDER_DEFAULT;
            BuscaEspecial.OrderType = Constants.ORDER_TYPE_DEFAULT;
            BuscaEspecial.Tabla = null;
            Busqueda = null;
            //Ventana.Filtros.ItemsSource = new List<String>();
            resultadoTesis = new List<TesisSimplificadaElectoralTO>();
#if STAND_ALONE
            FachadaBusquedaTradicional fachada = new FachadaBusquedaTradicional();
            List<TesisTO> resultadoFachada = fachada.getTesisElectoralPorIus(BuscaEspecial);
            fachada.Close();
            if (resultadoFachada.Count > 1)
            {
#else
            FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
            TesisTO[] resultadoFachada = fachada.getTesisElectoralPorIus(BuscaEspecial);
            fachada.Close();
            if (resultadoFachada.Length > 1)
            {
#endif
                FlechasMuestra = true;
            }
            else
            {
                FlechasMuestra = false;
                Ventana.inicio.Visibility = Visibility.Hidden;
                Ventana.siguiente.Visibility = Visibility.Hidden;
                Ventana.anterior.Visibility = Visibility.Hidden;
                Ventana.final.Visibility = Visibility.Hidden;
                Ventana.IrANum.Visibility = Visibility.Hidden;
                Ventana.btnIrA.Visibility = Visibility.Hidden;
                Ventana.lblIrA.Visibility = Visibility.Hidden;
            }
            List<int> idPonentes = new List<int>();
            List<int> idAsuntos = new List<int>();
            foreach (TesisTO item in resultadoFachada)
            {
                TesisSimplificadaElectoralTO newItemSource = new TesisSimplificadaElectoralTO();
                newItemSource.Ius = item.Ius;
                newItemSource.ConsecIndx = item.ConsecIndx;
                newItemSource.OrdenaInstancia = (int)item.OrdenInstancia;
                newItemSource.OrdenaTesis = (int)item.OrdenTesis;
                newItemSource.OrdenaRubro = (int)item.OrdenRubro;
                newItemSource.TipoTesis = item.TipoTesis;
                resultadoTesis.Add(newItemSource);
            }
            Ventana.tablaResultados.ItemsSource = resultadoTesis;
            Ventana.RegistrosLabel.Content = "Registros: " + resultadoTesis.Count;
            MuestraActual = resultadoTesis;
        }

        private void BuscaTesis()
        {
            if ((Busqueda.TipoBusqueda == Constants.BUSQUEDA_TESIS_SIMPLE) ||
                (Busqueda.TipoBusqueda == Constants.BUSQUEDA_TESIS_TEMATICA))
            {
                LlenaTabla();
            }            
        }

        protected void LlenaTabla()//PaginadorTO Paginador)
        {
            Terminado = false;
            Inhabilita();
            PosicionActual = 0;
            BackgroundWorker workerLlenado = new BackgroundWorker() { WorkerReportsProgress = true, WorkerSupportsCancellation = true };
            workerLlenado.ProgressChanged += new ProgressChangedEventHandler(worker_llenadoTabla_ProgressChanged);
            workerLlenado.RunWorkerCompleted += new RunWorkerCompletedEventHandler(worker_llenadoTabla_RunWorkerCompleted);
            workerLlenado.DoWork += new DoWorkEventHandler(worker_llenadoTabla_doWork);
            try
            {
#if STAND_ALONE
                fachadaThread = new FachadaBusquedaTradicional();
#else
                fachadaThread = new FachadaBusquedaTradicionalClient();
#endif
                PaginadorTO tesisObtenidas = fachadaThread.getIdTesisConsultaElectoralPanel(Busqueda);
                //fachada.Close();
                if (tesisObtenidas.Largo == 0)
                {
                    resultadoTesis = new List<TesisSimplificadaElectoralTO>();
                    Ventana.tablaResultados.ItemsSource = resultadoTesis;
                    return;
                }
                PaginadorActual = tesisObtenidas;
            }

            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            PosicionActual = 0;
            TesisSimplificadaElectoralTO tesisDummy = new TesisSimplificadaElectoralTO();
            List<TesisSimplificadaElectoralTO> itemsSource = new List<TesisSimplificadaElectoralTO>();
            itemsSource.Add(tesisDummy);
            resultadoTesis = itemsSource;
            Ventana.tablaResultados.ItemsSource = resultadoTesis;
            MuestraActual = resultadoTesis;
            Ventana.PgbTotalTesis.Value = 0;
            workerLlenado.RunWorkerAsync(workerLlenado);
        }

        private void Inhabilita()
        {
            //throw new NotImplementedException();
        }

        private void worker_llenadoTabla_doWork(object sernder, DoWorkEventArgs args)
         {
            BackgroundWorker BgwArgs = (BackgroundWorker)args.Argument;
            TesisTO[] tesisObtenidas = null;
            //fachadaThread = null;
            //fachadaThread = new FachadaBusquedaTradicionalClient();
            try
            {
#if STAND_ALONE
                tesisObtenidas = fachadaThread.getTesisElePaginadas(PaginadorActual.Id, PosicionActual).ToArray();
#else
                tesisObtenidas = fachadaThread.getTesisElePaginadas(PaginadorActual.Id, PosicionActual);
#endif
                //resultadoTesis = new List<TesisSimplificadaTO>();
                int Contador = 0;
                foreach (TesisTO actual in tesisObtenidas)
                {
                    if (PosicionActual == 0)
                    {
                        resultadoTesis.Clear();
                    }
                    Contador++;
                    PosicionActual++;
                    float Porcentaje = (float)((float)PosicionActual / (float)PaginadorActual.Largo);
                    BgwArgs.ReportProgress((int)(Porcentaje * 100.0));
                    TesisSimplificadaElectoralTO item2 = new TesisSimplificadaElectoralTO();
                    if (actual.Ius != null)
                    {
                        item2.Ius = actual.Ius;
                        item2.ConsecIndx = actual.ConsecIndx;
                        item2.OrdenaInstancia = (int)actual.OrdenInstancia;
                        item2.OrdenaTesis = (int)actual.OrdenTesis;
                        item2.OrdenaRubro = (int)actual.OrdenRubro;
                        item2.TpoTesis = actual.TpoTesis;
                        
                        item2.TipoTesis = actual.TipoTesis;
                        item2.Ponentes = actual.Ponentes;
                        
                    }
                    else
                    {
                        item2.ConsecIndx = actual.ConsecIndx;
                    }
                    resultadoTesis.Add(item2);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void worker_llenadoTabla_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            Ventana.PgbTotalTesis.Value = e.ProgressPercentage;
        }

        private void worker_llenadoTabla_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            System.Windows.Forms.Application.DoEvents();
            if (Ventana.tablaResultados.SelectedIndex == -1)
            {
                Ventana.tablaResultados.SelectedIndex = 0;
            }
            Ventana.tablaResultados.Items.Refresh();
            Ventana.RegistrosLabel.Content = "Registros: " + resultadoTesis.Count
                + " de " + PaginadorActual.Largo;
            Habilita();
            if ((resultadoTesis.Count > 1)
                    && ((PaginadorActual != null) && (PaginadorActual.Largo > 1)))
            {
                FlechasMuestra = true;
                Ventana.inicio.Visibility = Visibility.Visible;
                Ventana.siguiente.Visibility = Visibility.Visible;
                Ventana.anterior.Visibility = Visibility.Visible;
                Ventana.final.Visibility = Visibility.Visible;
                Ventana.IrANum.Visibility = Visibility.Visible;
                Ventana.btnIrA.Visibility = Visibility.Visible;
                Ventana.lblIrA.Visibility = Visibility.Visible;
            }
            else
            {
                FlechasMuestra = false;
                Ventana.inicio.Visibility = Visibility.Hidden;
                Ventana.siguiente.Visibility = Visibility.Hidden;
                Ventana.anterior.Visibility = Visibility.Hidden;
                Ventana.final.Visibility = Visibility.Hidden;
                Ventana.IrANum.Visibility = Visibility.Hidden;
                Ventana.btnIrA.Visibility = Visibility.Hidden;
                Ventana.lblIrA.Visibility = Visibility.Hidden;
            }
            if (resultadoTesis.Count < PaginadorActual.Largo)
            {
                BackgroundWorker workerLlenado = new BackgroundWorker() { WorkerReportsProgress = true, WorkerSupportsCancellation = true };
                workerLlenado.ProgressChanged += new ProgressChangedEventHandler(worker_llenadoTabla_ProgressChanged);
                workerLlenado.RunWorkerCompleted += new RunWorkerCompletedEventHandler(worker_llenadoTabla_RunWorkerCompleted);
                workerLlenado.DoWork += new DoWorkEventHandler(worker_llenadoTabla_doWork);
                workerLlenado.RunWorkerAsync(workerLlenado);
            }
            else
            {
                //fachada = new FachadaBusquedaTradicionalClient();
                //fachadaThread.BorraPaginador(PaginadorActual.Id);
                Ventana.PgbTotalTesis.Visibility = Visibility.Hidden;
                Terminado = true;
                fachadaThread.Close();
                fachadaThread = null;
            }
        }
        #region ITablaResultadosController Members

        public void Habilita()
        {
            //throw new NotImplementedException();
        }

        public void AnteriorClic()
        {
            if (Ventana.tablaResultados.SelectedIndex > 0)
            {
                Ventana.tablaResultados.SelectedIndex--;
            }
            Ventana.tablaResultados.BringItemIntoView(Ventana.tablaResultados.SelectedItem);
        }

        public void SiguienteClic()
        {
            if (Ventana.tablaResultados.Items.Count > Ventana.tablaResultados.SelectedIndex)
            {
                Ventana.tablaResultados.SelectedIndex++;
            }
            Ventana.tablaResultados.BringItemIntoView(Ventana.tablaResultados.SelectedItem);
        }

        public void FinalClic()
        {
            Ventana.tablaResultados.SelectedIndex = Ventana.tablaResultados.Items.Count - 1;
            Ventana.tablaResultados.BringItemIntoView(Ventana.tablaResultados.SelectedItem);
        }

        public void ImprimirClic()
        {
            if ((PaginadorActual != null))
            {
                if (!Terminado)
                {
                    VerificaActualización();
                    return;
                }
            }
            Ventana.imprimir.ToolTip = Constants.VISTA_PRELIMINAR_FUERA;
            MessageBoxResult resultadoAdv = MessageBoxResult.Yes;
            if (Ventana.tablaResultados.Items.Count > 2000)
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
                Ventana.IrANum.Visibility = Visibility.Hidden;
                Ventana.tablaResultados.Visibility = Visibility.Visible;
                Ventana.Esperar.Focus();
                FlowDocumentReader documentoAEscribir = new FlowDocumentReader();
                documentoListadoimpresion = null;
                worker = new BackgroundWorker() { WorkerReportsProgress = true, WorkerSupportsCancellation = true };
                worker.ProgressChanged += new ProgressChangedEventHandler(worker_ProgressChanged);
                worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(worker_RunWorkerCompleted);
                worker.DoWork += new DoWorkEventHandler(worker_DoWork);
                worker.RunWorkerAsync();
                Ventana.Cursor = Cursors.Wait;
            }
        }

        private void VerificaActualización()
        {
            throw new NotImplementedException();
        }

        public void GuardarClic()
        {
            if ((PaginadorActual != null))
            {
                if (!Terminado)
                {
                    VerificaActualización();
                    return;
                }
            }
            MostrarVP = false;
            Impreso = false;
            //PaginaActual.BtnDisminuyeAlto.Visibility = Visibility.Hidden;
            //PaginaActual.BtnAumentaAlto.Visibility = Visibility.Hidden;
            if (Ventana.impresionViewer.Visibility != Visibility.Visible)
            {
                ImprimirClic();
            }

            if (!BrowserInteropHelper.IsBrowserHosted)
            {
                while (!Impreso)
                {
                    System.Windows.Forms.Application.DoEvents();
                }
                MostrarVP = true;
                Microsoft.Win32.SaveFileDialog guardaEn = new Microsoft.Win32.SaveFileDialog();
                guardaEn.DefaultExt = ".rtf";
                guardaEn.Title = "Guardar listado de tesis";
                guardaEn.Filter = "Archivos de Texto Enriquecido|*.rtf";
                guardaEn.AddExtension = true;
                //EsconderImpresionPrel();
                Ventana.IrANum.Visibility = Visibility.Visible;
                if ((bool)guardaEn.ShowDialog())
                {
                    FlowDocument documentoImprimir = DocumentoListadoTesisElectoral.generaDocumento(documentoImpresion.ListaImpresion);
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

        public void SalirClic()
        {
            Ventana.NavigationService.Navigate(Ventana.Back);
        }

        public void CambioSeleccion()
        {
            int registroActual = Ventana.tablaResultados.SelectedIndex + 1;
            Ventana.RegistrosLabel.Content = registroActual + " de " + Ventana.tablaResultados.Items.Count;
        }

        public void TablaDobleClic()
        {
            TesisSimplificadaElectoralTO tesisSeleccionada = (TesisSimplificadaElectoralTO)Ventana.tablaResultados.SelectedItem;
            if (tesisSeleccionada.Ius == null)
            {
                return;
            }
            TesisElectoral paginaTesis = new TesisElectoral(Ventana.tablaResultados, Busqueda);
            NavigationWindow ventanaInicial = (NavigationWindow)Application.Current.MainWindow;
            if (Application.Current.MainWindow.Content == this)
            {
                paginaTesis.Back = Ventana;
            }
            else
            {
                paginaTesis.Back = (Page)Application.Current.MainWindow.Content;
            }
            ventanaInicial.NavigationService.Navigate(paginaTesis);
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

        public void BtnTacheClic()
        {
            EsconderImpresionPrel();
        }

        public void InicioClic()
        {
            Ventana.tablaResultados.SelectedIndex = 0;
            Ventana.tablaResultados.BringItemIntoView(Ventana.tablaResultados.SelectedItem);
        }

        public void IrAClic()
        {
            if (Ventana.IrANum.Text.Equals(""))
            {
                MessageBox.Show(Mensajes.MENSAJE_CONSECUTIVO_NO_VALIDO,
                    Mensajes.TITULO_REGISTRO_INVALIDO,
                    MessageBoxButton.OK, MessageBoxImage.Exclamation);
                Ventana.IrANum.Focus();
            }
            else
            {
                int registroSeleccionado = Int32.Parse(Ventana.IrANum.Text);
                registroSeleccionado--;
                if ((Ventana.tablaResultados.Items.Count > registroSeleccionado)
                    && (registroSeleccionado >= 0))
                {
                    Ventana.tablaResultados.SelectedIndex = registroSeleccionado;
                    Ventana.IrANum.Text = "";
                }
                else
                {
                    MessageBox.Show(Mensajes.MENSAJE_REGISTRO_INVALIDO,
                        Mensajes.TITULO_REGISTRO_INVALIDO,
                        MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
                Ventana.tablaResultados.BringItemIntoView(Ventana.tablaResultados.SelectedItem);
            }
        }
        /// <summary>
        /// Que hacer cuando se termine el trabajo, en este caso la generación de los datos de impresión.
        /// </summary>
        /// <param name="sender">Quien lo envía</param>
        /// <param name="e">Datos de como se completó.</param>
        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                EsconderImpresionPrel();
            }
            else if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message, "Error al generar el documento", MessageBoxButton.OK, MessageBoxImage.Error);
                MostrarImpresionPrel();
            }
            else
            {
                Ventana.TxbEsperar.Text = Mensajes.GENERANDO_DOCUMENTO;
                List<TesisTO> listaImprimir = (List<TesisTO>)e.Result;
                String ordenacion = OrdenarPor == null ? "consecindx" : ordenarPor.ToLower();
                switch (ordenacion.ToLower())
                {
                    case "tesis":
                        IComparer<TesisTO> comparador = new TesisElectoralNSTOComp();
                        listaImprimir.Sort(comparador);
                        break;
                    case "locabr":
                        comparador = new TesisElectoralConsecIndxNSTOComp();
                        listaImprimir.Sort(comparador);
                        break;
                    case "ius":
                        comparador = new TesisElectoralIUSTONSComp();
                        listaImprimir.Sort(comparador);
                        break;
                    case "rubro":
                        comparador = new TesisElectoralRubroNSTOComp();
                        listaImprimir.Sort(comparador);
                        break;
                    case "sala":
                        comparador = new TesisElectoralInstanciaNSTOComp();
                        listaImprimir.Sort(comparador);
                        break;
                }
                FlowDocument resultado = DocumentoListadoTesisElectoral.generaDocumento((List<TesisTO>)e.Result);
                if (!DocumentoListadoTesisElectoral.cancelado)
                {
                    Ventana.impresionViewer.Document = resultado;
                    Ventana.impresionViewer.Background = Brushes.White;
                    resultado.Background = Brushes.White;
                    if (MostrarVP)
                    {
                        MostrarImpresionPrel();
                    }
                    Impreso = true;
                }
            }
            Ventana.Esperar.Visibility = Visibility.Collapsed;
            Ventana.Cursor = Cursors.Arrow;
        }

        private void EsconderImpresionPrel()
        {
            Ventana.BtnVisualizar.Visibility = Visibility.Visible;
            if (!BrowserInteropHelper.IsBrowserHosted)
            {
                Ventana.Guardar.Visibility = Visibility.Visible;
            }
            Ventana.Titulo.Visibility = Visibility.Visible;
            Ventana.Expresion.Visibility = Visibility.Visible;
            Ventana.LblExpresion.Visibility = Visibility.Visible;
            Ventana.idExpresion.Visibility = Visibility.Visible;
            Ventana.imprimir.ToolTip = Constants.VistaPreliminar;
            Ventana.imprimir.Visibility = Visibility.Visible;
            //Ventana.Filtros.Visibility = Visibility.Visible;
            //Ventana.LblFiltros.Visibility = Visibility.Visible;
            Ventana.salir.Visibility = Visibility.Visible;
            Ventana.BtnTache.Visibility = Visibility.Hidden;
            if (FlechasMuestra)
            {
                Ventana.RegistrosLabel.Visibility = Visibility.Visible;
                Ventana.btnIrA.Visibility = Visibility.Visible;
                Ventana.IrANum.Visibility = Visibility.Visible;
                Ventana.lblIrA.Visibility = Visibility.Visible;
                Ventana.inicio.Visibility = Visibility.Visible;
                Ventana.anterior.Visibility = Visibility.Visible;
                Ventana.siguiente.Visibility = Visibility.Visible;
                Ventana.final.Visibility = Visibility.Visible;
            }
            Ventana.tablaResultados.Visibility = Visibility.Visible;
            Ventana.impresionViewer.Visibility = Visibility.Hidden;
            Ventana.imprimePapel.Visibility = Visibility.Hidden;
            Ventana.BtnDisminuyeAlto.Visibility = Visibility.Visible;
            Ventana.BtnAumentaAlto.Visibility = Visibility.Visible;
        }

        private void MostrarImpresionPrel()
        {
            Ventana.BtnDisminuyeAlto.Visibility = Visibility.Hidden;
            Ventana.BtnAumentaAlto.Visibility = Visibility.Hidden;
            Ventana.BtnVisualizar.Visibility = Visibility.Hidden;
            Ventana.Titulo.Visibility = Visibility.Hidden;
            Ventana.RegistrosLabel.Visibility = Visibility.Hidden;
            Ventana.Guardar.Visibility = Visibility.Hidden;
            Ventana.LblExpresion.Visibility = Visibility.Hidden;
            Ventana.Expresion.Visibility = Visibility.Hidden;
            Ventana.idExpresion.Visibility = Visibility.Hidden;
            Ventana.tablaResultados.Visibility = Visibility.Hidden;
            Ventana.impresionViewer.Visibility = Visibility.Visible;
            Ventana.imprimePapel.Visibility = Visibility.Visible;
            //Ventana.IrANum.Visibility = Visibility.Hidden;
            Ventana.lblIrA.Visibility = Visibility.Hidden;
            Ventana.btnIrA.Visibility = Visibility.Hidden;
            Ventana.imprimir.ToolTip = Constants.VISTA_PRELIMINAR_FUERA;
            Ventana.imprimir.Visibility = Visibility.Hidden;
            //Ventana.Filtros.Visibility = Visibility.Hidden;
            //Ventana.LblFiltros.Visibility = Visibility.Hidden;
            Ventana.salir.Visibility = Visibility.Hidden;
            Ventana.BtnTache.Visibility = Visibility.Visible;
            Ventana.imprimePapel.Visibility = Visibility.Visible;
            Ventana.inicio.Visibility = Visibility.Hidden;
            Ventana.anterior.Visibility = Visibility.Hidden;
            Ventana.siguiente.Visibility = Visibility.Hidden;
            Ventana.final.Visibility = Visibility.Hidden;
        }

        //This event is fired on the background thread, and is where you would do all your work 
        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            Impreso = false;
            List<TesisTO> resultado = null;
            DocumentoListadoTesisElectoral generador = new DocumentoListadoTesisElectoral(Ventana.tablaResultados.Items, worker);
            resultado = generador.ListaImpresion;
            documentoImpresion = generador;
            if (worker.CancellationPending)
            {
                e.Cancel = true;
            }
            e.Result = resultado;
        }
        /// <summary>
        /// Define que hacer cuando va avanzando el progreso del trabajador.
        /// </summary>
        /// <param name="sender">Quien lo envia</param>
        /// <param name="e">Datos del progreso</param>
        private void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            Ventana.EsperaBarra.Value = e.ProgressPercentage;
            System.Windows.Forms.Application.DoEvents();
        }
        #endregion
        private void setOrdenarPor(string value)
        {
            this.ordenarPor = value;
            switch (value.ToLower())
            {
                case "tesis":
                    IComparer<TesisSimplificadaElectoralTO> comparador = new TesisElectoralTOComp();
                    MuestraActual.Sort(comparador);
                    Ventana.tablaResultados.ItemsSource = null;
                    Ventana.tablaResultados.ItemsSource = MuestraActual;
                    break;
                case "locabr":
                    comparador = new TesisElectoralConsecIndxTOComp();
                    MuestraActual.Sort(comparador);
                    Ventana.tablaResultados.ItemsSource = null;
                    Ventana.tablaResultados.ItemsSource = MuestraActual;
                    break;
                case "ius":
                    comparador = new TesisElectoralIUSTOComp();
                    MuestraActual.Sort(comparador);
                    Ventana.tablaResultados.ItemsSource = null;
                    Ventana.tablaResultados.ItemsSource = MuestraActual;
                    break;
                case "rubro":
                    comparador = new TesisElectoralRubroTOComp();
                    MuestraActual.Sort(comparador);
                    Ventana.tablaResultados.ItemsSource = null;
                    Ventana.tablaResultados.ItemsSource = MuestraActual;
                    break;
                case "sala":
                    comparador = new TesisElectoralInstanciaTOComp();
                    MuestraActual.Sort(comparador);
                    Ventana.tablaResultados.ItemsSource = null;
                    Ventana.tablaResultados.ItemsSource = MuestraActual;
                    break;
            }
            Ventana.tablaResultados.SelectedIndex = 0;
        }
    }
}
