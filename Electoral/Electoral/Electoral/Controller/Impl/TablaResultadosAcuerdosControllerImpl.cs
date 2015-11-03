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
using mx.gob.scjn.ius_common.TO;
using mx.gob.scjn.ius_common.fachade;
using mx.gob.scjn.ius_common.gui.gui.impresion;
using mx.gob.scjn.ius_common.gui.impresion;
using mx.gob.scjn.ius_common.gui.utils;
using mx.gob.scjn.ius_common.utils;

namespace mx.gob.scjn.electoral.Controller.Impl
{
    
    public class TablaResultadosAcuerdosControllerImpl:ITablaResultadosAcuerdosController
    {
        public TablaResultadoAcuerdos Ventana { get; set; }
        /// <summary>
        /// La lista original de los resultados.
        /// </summary>
        List<AcuerdoSimplificadoTO> resultadoAcuerdos;
        /// <summary>
        /// Los parámetros de la búsqueda.
        /// </summary>
        BusquedaTO busqueda { get; set; }
        /// <summary>
        /// Documento que se imprimirá
        /// </summary>
        DocumentoListadoAcuerdo documentoImpresion { get; set; }
        protected bool mostrarIP = true;
        private bool impreso { get; set; }
        protected bool verFlechas { get; set; }
        protected BackgroundWorker worker;
        public TablaResultadosAcuerdosControllerImpl(TablaResultadoAcuerdos ventana)
        {
            Ventana = ventana;
            buscaAcuerdos();
            Ventana.Title = "Resultado de la Búsqueda";
            Ventana.tablaResultado.ItemsSource = resultadoAcuerdos;
            Ventana.tablaResultado.SelectedIndex = 0;
            Ventana.RegistrosLabel.Content = "Registros: " + resultadoAcuerdos.Count;
        }
        public TablaResultadosAcuerdosControllerImpl(TablaResultadoAcuerdos ventana,
            int[] identificadores)
        {
            Ventana = ventana;
            MostrarPorIusTO parametros = new MostrarPorIusTO();
            parametros.OrderBy = "Consec";
            parametros.OrderType = "asc";
#if STAND_ALONE
            FachadaBusquedaTradicional fachada = new FachadaBusquedaTradicional();
            parametros.Listado = identificadores.ToList();
            List<AcuerdosTO> arreglo = fachada.getAcuerdosElectoralPorIds(parametros);
#else
            FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
            parametros.Listado = identificadores;
            AcuerdosTO[] arreglo = fachada.getAcuerdos(parametros);
#endif
            resultadoAcuerdos = new List<AcuerdoSimplificadoTO>();
            foreach (AcuerdosTO item in arreglo)
            {
                resultadoAcuerdos.Add(new AcuerdoSimplificadoTO(item));
            }
            Ventana.tablaResultado.ItemsSource = resultadoAcuerdos;
            if (identificadores.Length < 2)
            {
                verFlechas = false;
                Ventana.IrANum.Visibility = Visibility.Hidden;
                Ventana.btnIrA.Visibility = Visibility.Hidden;
                Ventana.RegistrosLabel.Visibility = Visibility.Hidden;
                Ventana.lblIrA.Visibility = Visibility.Hidden;
                Ventana.inicio.Visibility = Visibility.Hidden;
                Ventana.anterior.Visibility = Visibility.Hidden;
                Ventana.siguiente.Visibility = Visibility.Hidden;
                Ventana.final.Visibility = Visibility.Hidden;
            }
            else
            {
                verFlechas = true;
            }
            Ventana.RegistrosLabel.Content = "Registros: " + resultadoAcuerdos.Count;
            Ventana.tablaResultado.SelectedIndex = 0;
            fachada.Close();
        }

        private void buscaAcuerdos()
        {
            busqueda.TipoBusqueda = Constants.BUSQUEDA_ACUERDO;
            Boolean browserHosted = BrowserInteropHelper.IsBrowserHosted;
#if STAND_ALONE
            FachadaBusquedaTradicional fachada = new FachadaBusquedaTradicional();
            List<AcuerdosTO> acuerdosObtenidos = null;
#else
            FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
            AcuerdosTO[] acuerdosObtenidos = null;
#endif
            try
            {
                acuerdosObtenidos = fachada.getIdConsultaPanelElectoral(busqueda).Acuerdos;
                resultadoAcuerdos = new List<AcuerdoSimplificadoTO>();
                foreach (AcuerdosTO actual in acuerdosObtenidos)
                {
                    resultadoAcuerdos.Add(new AcuerdoSimplificadoTO(actual));
                }
                List<int> listaIds = new List<int>();
                foreach (AcuerdoSimplificadoTO item in resultadoAcuerdos)
                {
                    listaIds.Add(Int32.Parse(item.Id));
                }
                MostrarPorIusTO ids = new MostrarPorIusTO();
                ids.OrderBy = "consecIndx";
                ids.OrderType = "asc";
#if STAND_ALONE
                ids.Listado = listaIds;
                List<AcuerdosTO> arregloAcuerdos = fachada.getAcuerdosElectoralPorIds(ids);
#else
                ids.Listado=listaIds.ToArray();
                AcuerdosTO[] arregloAcuerdos = fachada.getAcuerdos(ids);
#endif
                resultadoAcuerdos.Clear();
                foreach (AcuerdosTO actual in arregloAcuerdos)
                {
                    resultadoAcuerdos.Add(new AcuerdoSimplificadoTO(actual));
                }
                fachada.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                resultadoAcuerdos = new List<AcuerdoSimplificadoTO>();
            }
        }

        public TablaResultadosAcuerdosControllerImpl(TablaResultadoAcuerdos ventana,
            BusquedaTO busca)
        {
            Ventana = ventana;
            busqueda = busca;
            Ventana.Title = "Buscando secuencialmente";
            buscaAcuerdos();
            Ventana.tablaResultado.ItemsSource = resultadoAcuerdos;
            if (Ventana.tablaResultado.Items.Count < 2)
            {
                verFlechas = false;
                Ventana.IrANum.Visibility = Visibility.Hidden;
                Ventana.btnIrA.Visibility = Visibility.Hidden;
                Ventana.RegistrosLabel.Visibility = Visibility.Hidden;
                Ventana.lblIrA.Visibility = Visibility.Hidden;
                Ventana.inicio.Visibility = Visibility.Hidden;
                Ventana.anterior.Visibility = Visibility.Hidden;
                Ventana.siguiente.Visibility = Visibility.Hidden;
                Ventana.final.Visibility = Visibility.Hidden;
            }
            else
            {
                verFlechas = true;
            }
            Ventana.WindowTitle = "Visualizar registros " + ((String)Ventana.TituloAcuerdo.Title)+"s";
            Ventana.tablaResultado.SelectedIndex = 0;
            Ventana.Expresion.Text = "Acuerdos electorales";
            Ventana.RegistrosLabel.Content = "Registros: " + resultadoAcuerdos.Count;

        }

        #region ITablaResultadosAcuerdosController Members

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

        public void GuardarClic()
        {
            mostrarIP = false;
            ImprimirClic();
            while (!impreso)
            {
                System.Windows.Forms.Application.DoEvents();
            }
            EsconderImpresionPrel();
            Microsoft.Win32.SaveFileDialog guardaEn = new Microsoft.Win32.SaveFileDialog();
            guardaEn.DefaultExt = ".rtf";
            guardaEn.Title = "Guardar listado de " + Ventana.TituloAcuerdo.Title.ToString().ToLower();
            guardaEn.Filter = "Archivos de Texto Enriquecido|*.rtf";
            guardaEn.AddExtension = true;
            if ((bool)guardaEn.ShowDialog())
            {
                Ventana.DocumentoGuardar.Document = documentoImpresion.DocumentoImpresion;
                try
                {
                    System.IO.FileStream archivo = new System.IO.FileStream(guardaEn.FileName, System.IO.FileMode.Create);
                    Ventana.DocumentoGuardar.SelectAll();
                    Ventana.DocumentoGuardar.Selection.Save(archivo, System.Windows.DataFormats.Rtf);
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

        /// <summary>
        ///     Muestra los componentes relacionados con la impresión
        ///     preliminar y esconde los demás
        /// </summary>
        private void MostrarImpresionPrel()
        {
            Ventana.BtnDisminuyeAlto.Visibility = Visibility.Hidden;
            Ventana.BtnAumentaAlto.Visibility = Visibility.Hidden;
            Ventana.BtnGuardar.Visibility = Visibility.Hidden;
            Ventana.BtnVisualizar.Visibility = Visibility.Hidden;
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
        }
        /// <summary>
        ///     Esconde los componentes relacionados con la impresión preliminar y
        ///     muestra los que no.
        /// </summary>
        private void EsconderImpresionPrel()
        {
            Ventana.BtnDisminuyeAlto.Visibility = Visibility.Visible;
            Ventana.BtnAumentaAlto.Visibility = Visibility.Visible;
            if (!BrowserInteropHelper.IsBrowserHosted) Ventana.BtnGuardar.Visibility = Visibility.Visible;
            Ventana.BtnVisualizar.Visibility = Visibility.Visible;
            Ventana.Titulo.Visibility = Visibility.Visible;
            Ventana.RegistrosLabel.Visibility = Visibility.Visible;
            Ventana.Expresion.Visibility = Visibility.Visible;
            Ventana.imprimir.ToolTip = Constants.VistaPreliminar;
            Ventana.imprimir.Visibility = Visibility.Visible;
            Ventana.salir.Visibility = Visibility.Visible;
            Ventana.BtnTache.Visibility = Visibility.Hidden;
            Ventana.btnIrA.Visibility = Visibility.Visible;
            Ventana.tablaResultado.Visibility = Visibility.Visible;
            Ventana.impresionViewer.Visibility = Visibility.Hidden;
            Ventana.imprimePapel.Visibility = Visibility.Hidden;
            Ventana.IrANum.Visibility = Visibility.Visible;
            Ventana.lblIrA.Visibility = Visibility.Visible;
            Ventana.inicio.Visibility = Visibility.Visible;
            Ventana.anterior.Visibility = Visibility.Visible;
            Ventana.siguiente.Visibility = Visibility.Visible;
            Ventana.final.Visibility = Visibility.Visible;
            if (!verFlechas)
            {
                verFlechas = false;
                Ventana.IrANum.Visibility = Visibility.Hidden;
                Ventana.btnIrA.Visibility = Visibility.Hidden;
                Ventana.RegistrosLabel.Visibility = Visibility.Hidden;
                Ventana.lblIrA.Visibility = Visibility.Hidden;
                Ventana.inicio.Visibility = Visibility.Hidden;
                Ventana.anterior.Visibility = Visibility.Hidden;
                Ventana.siguiente.Visibility = Visibility.Hidden;
                Ventana.final.Visibility = Visibility.Hidden;
            }
        }

        public void OrdenarPor()
        {
            throw new NotImplementedException();
        }

        public void IrTecla(KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                IrClic();
            }
            if ((e.Key == Key.Decimal) || (e.Key == Key.OemPeriod))
            {
                e.Handled = true;
            }
        }

        public void IrClic()
        {
            if(Ventana.IrANum.Text.Equals("")){
                MessageBox.Show(Mensajes.MENSAJE_CAMPO_TEXTO_NUMERO_VACIO,
                    Mensajes.TITULO_CAMPO_TEXTO_VACIO,
                    MessageBoxButton.OK, MessageBoxImage.Error);
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

        public void VisualizarClic()
        {
            Acuerdos muestraAcuerdo = new Acuerdos(Ventana.tablaResultado, busqueda);
            Page pagina = (Page)Application.Current.MainWindow.Content;
            muestraAcuerdo.Back = pagina;
            pagina.NavigationService.Navigate(muestraAcuerdo);
        }

        public void SalirClic()
        {
            if (Ventana.Back == null)
            {
                Ventana.NavigationService.GoBack();
            }
            else
            {
                Ventana.NavigationService.Navigate(Ventana.Back);
            }
        }

        public void TablaResultadoPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            int registroActual = Ventana.tablaResultado.SelectedIndex + 1;
            Ventana.RegistrosLabel.Content = registroActual + " de " + Ventana.tablaResultado.Items.Count;
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

        #endregion


        #region BackgroundWorker Events

        //This event is fired on the background thread, and is where you would do all your work 
        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            impreso = false;
            List<AcuerdoSimplificadoTO> resultado = null;
            DocumentoListadoAcuerdo generador = new DocumentoListadoAcuerdo(Ventana.tablaResultado.Items, worker);
            resultado = generador.ListaImpresion;
            documentoImpresion = generador;
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
                Ventana.tablaResultado.Visibility = Visibility.Visible;
                Ventana.impresionViewer.Visibility = Visibility.Hidden;
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
                //documentoImpresion = new DocumentoListadoAcuerdo();
                FlowDocument resultado = documentoImpresion.generaDocumento((List<AcuerdoSimplificadoTO>)e.Result);// (FlowDocument)XamlReader.Load(documento.GetStream());
                Ventana.impresionViewer.Document = resultado;
                Ventana.impresionViewer.Visibility = Visibility.Visible;//Means everything went as expected 
                Ventana.impresionViewer.Background = Brushes.White;
                resultado.Background = Brushes.White;
                if (mostrarIP)
                {
                    MostrarImpresionPrel();
                }
                mostrarIP = true;
            }
            impreso = true;
            Ventana.Esperar.Visibility = Visibility.Collapsed;
            Ventana.Cursor = Cursors.Arrow;
        }
        #endregion 

    }
}
