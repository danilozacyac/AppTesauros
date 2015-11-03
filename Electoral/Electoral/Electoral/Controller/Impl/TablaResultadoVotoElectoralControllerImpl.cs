using System;
using System.Collections.Generic;
using System.Linq;
using mx.gob.scjn.ius_common.TO;
using System.Windows;
using mx.gob.scjn.ius_common.fachade;
using System.Windows.Interop;
using mx.gob.scjn.ius_common.utils;
using System.Windows.Input;
using mx.gob.scjn.ius_common.gui.utils;
using System.Windows.Controls;
using System.ComponentModel;
using System.Windows.Documents;
using System.Windows.Media;
using mx.gob.scjn.electoral_common.gui.impresion;
using mx.gob.scjn.ius_common.gui.gui.impresion;

namespace mx.gob.scjn.electoral.Controller.Impl
{
    public class TablaResultadoVotoElectoralControllerImpl:ITablaResultadoVotoElectoral
    {
        public TablaResultadoVotos Ventana { get; set; }
        private bool impreso;
        protected bool verFlechas { get; set; }
        /// <summary>
        /// Documento que se imprimirá
        /// </summary>
        DocumentoListadoVotoElectoral documentoImpresion { get; set; }
        BackgroundWorker worker;
        /// <summary>
        /// Los parámetros de la búsqueda.
        /// </summary>
        BusquedaTO busqueda { get; set; }
        /// <summary>
        /// La lista que se mostrará dentro del control de DataGrid
        /// </summary>
        List<VotoSimplificadoTO> resultadoVotos { get; set; }
        List<VotoSimplificadoTO> muestraActual { get; set; }

        public TablaResultadoVotoElectoralControllerImpl(TablaResultadoVotos ventana)
        {
            Ventana = ventana;
            verFlechas = false;
            Ventana.Title = "Resultado de la Búsqueda";
            Ventana.tablaResultado.ItemsSource = resultadoVotos;
            Ventana.RegistrosLabel.Content = "Registros: " + resultadoVotos.Count;
        }

        public TablaResultadoVotoElectoralControllerImpl(TablaResultadoVotos ventana,
            BusquedaTO buscar)
        {
            Ventana = ventana;
            Ventana.Title = "Buscando secuencialmente";
            busqueda = buscar;
            buscaTesis();
            Ventana.tablaResultado.ItemsSource = resultadoVotos;
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
            Ventana.RegistrosLabel.Content = "Registros: " + resultadoVotos.Count;
        }

        public TablaResultadoVotoElectoralControllerImpl(TablaResultadoVotos ventana,
            int[] identificadores)
        {
            Ventana = ventana;
            //Ventana.Clasificacion.NavigationService = Ventana;
            MostrarPorIusTO parametros = new MostrarPorIusTO();
            parametros.OrderBy = "Consec";
            parametros.OrderType = "asc";
            resultadoVotos = new List<VotoSimplificadoTO>();
            List<VotoSimplificadoTO> votosConcurrentes = new List<VotoSimplificadoTO>();
            List<VotoSimplificadoTO> votosMinoritario = new List<VotoSimplificadoTO>();
            List<VotoSimplificadoTO> votosParticular = new List<VotoSimplificadoTO>();
#if STAND_ALONE
            FachadaBusquedaTradicional fachada = new FachadaBusquedaTradicional();
            parametros.Listado = identificadores.ToList();
            List<VotosTO> arreglo = fachada.getVotosElectoralPorIds(parametros);
#else
            FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
            parametros.Listado = identificadores;
            VotosTO[] arreglo = fachada.getVotosElectoralPorIds(parametros);
#endif
            foreach (VotosTO item in arreglo)
            {
                VotoSimplificadoTO itemTemporal = new VotoSimplificadoTO(item);                
                    resultadoVotos.Add(itemTemporal);
            }
            Ventana.tablaResultado.ItemsSource = resultadoVotos;
            //resultadoOriginalOtros = resultadoVotos;            
            fachada.Close();
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
            Ventana.RegistrosLabel.Content = "Registros: " + resultadoVotos.Count;
        }

        private void buscaTesis()
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
                busqueda.Acuerdos = null;
                busqueda.Apendices = null;
                busqueda.Epocas = epocas;
                busqueda.OrdenarPor = "ConsecIndx";
            }
            //FachadaBusquedaTradicionalClient fachadaImpl = null;
            busqueda.TipoBusqueda = Constants.BUSQUEDA_VOTOS;
            Boolean browserHosted = BrowserInteropHelper.IsBrowserHosted;
#if STAND_ALONE
            FachadaBusquedaTradicional fachada = new FachadaBusquedaTradicional();
            List<VotosTO> votosObtenidos = null;
#else
            FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
            VotosTO[] votosObtenidos = null;
#endif
            try
            {
                votosObtenidos = fachada.getIdConsultaPanelElectoral(busqueda).Votos;
                resultadoVotos = new List<VotoSimplificadoTO>();
                foreach (VotosTO actual in votosObtenidos)
                {
                    VotoSimplificadoTO itemTemporal = new VotoSimplificadoTO(actual);
                    resultadoVotos.Add(itemTemporal);
                }
                List<int> listaIds = new List<int>();
                foreach (VotoSimplificadoTO item in resultadoVotos)
                {
                    listaIds.Add(Int32.Parse(item.Id));
                }
                MostrarPorIusTO ids = new MostrarPorIusTO();
                ids.OrderBy = "consecIndx";
                ids.OrderType = "asc";
#if STAND_ALONE
                ids.Listado = listaIds;
                List<VotosTO> arregloVotos = fachada.getVotosElectoralPorIds(ids);
#else
                ids.Listado=listaIds.ToArray();
                VotosTO[] arregloVotos = fachada.getVotosElectoralPorIds(ids);
#endif
                resultadoVotos.Clear();
                foreach (VotosTO actual in arregloVotos)
                {
                    VotoSimplificadoTO itemTemporal = new VotoSimplificadoTO(actual);
                    resultadoVotos.Add(itemTemporal);
                }
                fachada.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                resultadoVotos = new List<VotoSimplificadoTO>();
            }
        }

        #region ITablaResultadoVotoElectoral Members

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
                    FlowDocumentReader documentoAEscribir = new FlowDocumentReader();
                    //documentoListadoImpresion = null;
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

        public void SalirClic()
        {
            Ventana.NavigationService.Navigate(Ventana.Back);
        }

        public void GuardarClic()
        {
            if (!BrowserInteropHelper.IsBrowserHosted)
            {
                ImprimirClic();
                while (!impreso)
                {
                    System.Windows.Forms.Application.DoEvents();
                }
                Microsoft.Win32.SaveFileDialog guardaEn = new Microsoft.Win32.SaveFileDialog();
                guardaEn.DefaultExt = ".rtf";
                guardaEn.Title = "Guardar listado de votos";
                guardaEn.Filter = "Archivos de Texto Enriquecido|*.rtf";
                guardaEn.AddExtension = true;
                EsconderImpresionPrel();
                if ((bool)guardaEn.ShowDialog())
                {
                    FlowDocument documentoImprimir = DocumentoListadoVotoElectoral.generaDocumento(documentoImpresion.ListaImpresion);
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

        public void VisualizarClic()
        {
            Votos muestraVotos = new Votos(Ventana.tablaResultado, busqueda);
            muestraVotos.Back = Ventana;
            Ventana.NavigationService.Navigate(muestraVotos);
        }

        #endregion

        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            impreso = false;
            List<VotoSimplificadoTO> resultado = null;
            DocumentoListadoVotoElectoral generador = new DocumentoListadoVotoElectoral(Ventana.tablaResultado.Items, worker);
            documentoImpresion = generador;
            resultado = generador.ListaImpresion;
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
                FlowDocument resultado = DocumentoListadoVotoElectoral.generaDocumento((List<VotoSimplificadoTO>)e.Result);// (FlowDocument)XamlReader.Load(documento.GetStream());
                Ventana.impresionViewer.Document = resultado as IDocumentPaginatorSource;
                Ventana.impresionViewer.Visibility = Visibility.Visible;//Means everything went as expected 
                Ventana.impresionViewer.Background = Brushes.White;
                resultado.Background = Brushes.White;
                MostrarImpresionPrel();
            }
            impreso = true;
            Ventana.Esperar.Visibility = Visibility.Collapsed;
            Ventana.Cursor = Cursors.Arrow;
        }
        private void MostrarImpresionPrel()
        {
            Ventana.BtnDisminuyeAlto.Visibility = Visibility.Hidden;
            Ventana.BtnAumentaAlto.Visibility = Visibility.Hidden;
            Ventana.BtnGuardar.Visibility = Visibility.Hidden;
            Ventana.Titulo.Visibility = Visibility.Hidden;
            Ventana.RegistrosLabel.Visibility = Visibility.Hidden;
            //Expresion.Visibility = Visibility.Hidden;
            //idExpresion.Visibility = Visibility.Hidden;
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
            //Ventana.btnOrdenar.Visibility = Visibility.Hidden;
            Ventana.BtnVisualizar.Visibility = Visibility.Hidden;
        }
        private void EsconderImpresionPrel()
        {
            Ventana.BtnDisminuyeAlto.Visibility = Visibility.Visible;
            Ventana.BtnAumentaAlto.Visibility = Visibility.Visible;
            if (!BrowserInteropHelper.IsBrowserHosted) Ventana.BtnGuardar.Visibility = Visibility.Visible;
            Ventana.Titulo.Visibility = Visibility.Visible;
            Ventana.imprimir.ToolTip = Constants.VistaPreliminar;
            Ventana.imprimir.Visibility = Visibility.Visible;
            Ventana.salir.Visibility = Visibility.Visible;
            Ventana.BtnTache.Visibility = Visibility.Hidden;
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
            Ventana.tablaResultado.Visibility = Visibility.Visible;
            Ventana.impresionViewer.Visibility = Visibility.Hidden;
            Ventana.imprimePapel.Visibility = Visibility.Hidden;
            //Ventana.btnOrdenar.Visibility = Visibility.Visible;
            Ventana.BtnVisualizar.Visibility = Visibility.Visible;
        }


        #region ITablaResultadoVotoElectoral Members


        public void TacheClic()
        {
            EsconderImpresionPrel();
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

        public void CambioRegistro()
        {
            int registroActual = Ventana.tablaResultado.SelectedIndex + 1;
            Ventana.RegistrosLabel.Content = registroActual + "/" + Ventana.tablaResultado.Items.Count;
        }
        #endregion
    }
}
