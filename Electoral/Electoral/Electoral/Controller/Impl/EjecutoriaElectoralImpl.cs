using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using Xceed.Wpf.DataGrid;
using mx.gob.scjn.electoral_common.TO;
using mx.gob.scjn.electoral_common.gui.impresion;
using mx.gob.scjn.electoral_common.gui.utils;
using mx.gob.scjn.ius_common.TO;
using mx.gob.scjn.ius_common.fachade;
using mx.gob.scjn.ius_common.gui.gui.impresion;
using mx.gob.scjn.ius_common.gui.utils;
using mx.gob.scjn.ius_common.utils;

namespace mx.gob.scjn.electoral.Controller.Impl
{
    public class EjecutoriaElectoralImpl:IEjecutoriaElectoral
    {
        public Ejecutoria Ventana { get; set; }
        /// <summary>
        /// Los documentos de tesis que tiene relacionada la ejecutoria.
        /// </summary>
#if STAND_ALONE
        private List<RelDocumentoTesisTO> documentosTesis { get; set; }
#else
        private RelDocumentoTesisTO[] documentosTesis { get; set; }
#endif
        /// <summary>
        /// Los documentos de Votos relacionados con la tesis.
        /// </summary>
#if STAND_ALONE
        private List<RelVotoEjecutoriaTO> documentosVotos { get; set; }
#else
        private RelVotoEjecutoriaTO[] documentosVotos { get; set; }
#endif
        /// <summary>
        /// Cada una de las partes que componen la eecutoria de la tesis
        /// </summary>
#if STAND_ALONE
        private List<EjecutoriasPartesTO> partes { get; set; }
#else
        private EjecutoriasPartesTO[] partes { get; set; }
#endif
        /// <summary>
        /// Cuando se construye a partir de una lista que se encuentra en el
        /// dataGridControl esta queda aquí.
        /// </summary>
        private Xceed.Wpf.DataGrid.DataGridControl ArregloTesis { get; set; }
        /// <summary>
        ///     Define si la frase a encontrar fue ya o no localizada.
        /// </summary>
        /// 
        protected bool EncontradaFrase { get; set; }
        /// <summary>
        /// La posición en la que nos encontramos dentro de la selección del datagrid.
        /// </summary>
        private int posicion = 0;
        /// <summary>
        /// El parámetro con el que se mando llamar el constructor.
        /// </summary>
        /// 
        private Object ParametroConstruccion { get; set; }
        ///<summary>
        ///El objeto de la busqueda original cuando es secuencial o por palabras
        ///</summary>
        ///
        private BusquedaTO Busqueda { get; set; }
        /// <summary>
        /// La ejecutoria actual.
        /// </summary>
        private EjecutoriasTO DocumentoActual;
        /// <summary>
        /// El número de parte que se muestra en el momento.
        /// </summary>
        private int numeroParte { get; set; }
        private bool ExistenTablas { get; set; }
        protected bool verVentanaListadoVotos { get; set; }
        protected bool verVentanaListadoTesis { get; set; }
        protected bool verVentanaHistorial { get; set; }
        protected bool verVentanaRangos { get; set; }
        protected bool verVentanaAnexos { get; set; }
        protected bool verHistorial { get; set; }
        protected bool verFontMenor { get; set; }
        protected bool verFontMayor { get; set; }
        private FlowDocument DocumentoCopia { get; set; }

        public EjecutoriaElectoralImpl(Ejecutoria ventana)
        {
            Ventana = ventana;
        }
        public EjecutoriaElectoralImpl(Ejecutoria ventana, Int32 Id)
        {
            Ventana = ventana;
            Ventana.contenidoTexto.SetValue(RichTextBox.FontSizeProperty, CalculosPropiedadesGlobales.FontSize);
            verFontMenor = true;
            verFontMayor = true;
            this.ParametroConstruccion = this;
#if STAND_ALONE
            FachadaBusquedaTradicional fachada = new FachadaBusquedaTradicional();
#else
            FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
#endif
            EjecutoriasTO documentoActual = fachada.getEjecutoriaElectoralPorId(Id);
            MostrarDatos(documentoActual);
#if STAND_ALONE
            documentosVotos = new List<RelVotoEjecutoriaTO>();
            documentosTesis = new List<RelDocumentoTesisTO>();
#else
            documentosVotos = new RelVotoEjecutoriaTO[0];
            documentosTesis = new RelDocumentoTesisTO[0];
#endif
            Ventana.tesis.Visibility = Visibility.Hidden;
            fachada.Close();
            //Si entran a este constructor es por que vienen para ver solamente un registro
            Ventana.inicioLista.Visibility = Visibility.Hidden;
            Ventana.anteriorLista.Visibility = Visibility.Hidden;
            Ventana.siguienteLista.Visibility = Visibility.Hidden;
            Ventana.ultimoLista.Visibility = Visibility.Hidden;
            Ventana.regNum.Visibility = Visibility.Hidden;
            Ventana.RegNum.Visibility = Visibility.Hidden;
            Ventana.IrALabel.Visibility = Visibility.Hidden;
            Ventana.IrBoton.Visibility = Visibility.Hidden;
            Ventana.Marcar.Visibility = Visibility.Hidden;
            Ventana.MarcarTodo.Visibility = Visibility.Hidden;
            Ventana.Desmarcar.Visibility = Visibility.Hidden;
            Ventana.Expresion.Visibility = Visibility.Hidden;
        }
        public EjecutoriaElectoralImpl(Ejecutoria ventana, DataGridControl records, BusquedaTO parametrosBusqueda)
        {
            Ventana = ventana;
            Ventana.contenidoTexto.SetValue(RichTextBox.FontSizeProperty, CalculosPropiedadesGlobales.FontSize);
            verFontMenor = true;
            verFontMayor = true;
            CalculosPropiedadesGlobales.FontSize = Ventana.FontSize;
            if (records.Items.Count < 2)
            {
                Ventana.inicioLista.Visibility = Visibility.Hidden;
                Ventana.anteriorLista.Visibility = Visibility.Hidden;
                Ventana.siguienteLista.Visibility = Visibility.Hidden;
                Ventana.ultimoLista.Visibility = Visibility.Hidden;
                Ventana.regNum.Visibility = Visibility.Hidden;
                Ventana.RegNum.Visibility = Visibility.Hidden;
                Ventana.IrALabel.Visibility = Visibility.Hidden;
                Ventana.IrBoton.Visibility = Visibility.Hidden;
            }
            Busqueda = parametrosBusqueda;
            ParametroConstruccion = this;
            EjecutoriaSimplificadaElectoralTO ejecutoriaMostrar = (EjecutoriaSimplificadaElectoralTO)records.SelectedItem;
            posicion = records.SelectedIndex;
#if STAND_ALONE
            FachadaBusquedaTradicional fachada = new FachadaBusquedaTradicional();
#else
            FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
#endif
            EjecutoriasTO documentoActual = fachada.getEjecutoriaElectoralPorId(Int32.Parse(ejecutoriaMostrar.Id));
            fachada.Close();
            this.ArregloTesis = records;
            if (parametrosBusqueda != null)
            {
                Ventana.Expresion.Content = "Sentencias Electorales";// CalculosGlobales.Expresion(parametrosBusqueda);
            }
            else
            {
                List<int> registros = new List<int>();
                Ventana.Expresion.Content = CalculosGlobales.Expresion(registros);
            }
            MostrarDatos(documentoActual);
        }
        /// <summary>
        /// Muestra los datos de una ejecutoria determinada.
        /// </summary>
        private void MostrarDatos(EjecutoriasTO documentoActual){
            DocumentoActual = documentoActual;
            numeroParte = 0;
            if (documentoActual.Pagina.Trim().Equals("0"))
            {
                Ventana.PaginaLabel.Content = "Sin número de página";
            }
            else
            {
                Ventana.PaginaLabel.Content = "Página: " + documentoActual.Pagina;
            }
            Ventana.EpocaLabel.Content = documentoActual.Epoca;
            Ventana.fuenteLabel.Content = documentoActual.Fuente;
            Ventana.SalaLabel.Content = documentoActual.Sala;
            Ventana.IdLabel.Content = documentoActual.Id;
            Ventana.VolumenLabel.Content = documentoActual.Volumen;
            FlowDocument documentoPrecedentes = new FlowDocument();
            FlowDocument documentoRubro = new FlowDocument();
            Ventana.contenidoTexto.Document = documentoRubro;
#if STAND_ALONE
            FachadaBusquedaTradicional fachada = new FachadaBusquedaTradicional();
#else
            FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
#endif
            partes = fachada.getPartesElectoralPorId(Int32.Parse(documentoActual.Id));
            documentosTesis = fachada.getTesisPorEjecutoriaElectoral(documentoActual.Id);
            Paragraph textoParrafo;
            //textoParrafo = ObtenLigas(documentoActual.Rubro + "\n\n" + partes[0].TxtParte, documentoActual.Id, 1);
            textoParrafo = ObtenLigas(partes[0].TxtParte, documentoActual.Id, 1);
            textoParrafo.TextAlignment = TextAlignment.Justify;
            textoParrafo.FontWeight = FontWeights.Normal;
            documentoRubro.Blocks.Add(textoParrafo);
            this.documentosVotos = fachada.getVotoPorEjecutoria(documentoActual.Id);
#if STAND_ALONE
            if (this.documentosTesis.Count == 0)
#else
            if (this.documentosTesis.Length == 0)
#endif
            {
                Ventana.tesis.Visibility = Visibility.Hidden;
            }
            else
            {
                Ventana.tesis.Visibility = Visibility.Visible;
            }
#if STAND_ALONE
            if (this.documentosVotos.Count == 0)
#else
            if (this.documentosVotos.Length == 0)
#endif
            {
                Ventana.voto.Visibility = Visibility.Hidden;
            }
            else
            {
                Ventana.voto.Visibility = Visibility.Visible;
            }
#if STAND_ALONE
            if (partes.Count <= 1)
#else
            if (partes.Length <= 1)
#endif
            {
                Ventana.parteSiguiente.Visibility = Visibility.Hidden;
                Ventana.parteAnterior.Visibility = Visibility.Hidden;
                Ventana.parteFinal.Visibility = Visibility.Hidden;
                Ventana.parteInicio.Visibility = Visibility.Hidden;
                Ventana.NumeroPartes.Visibility = Visibility.Hidden;
                Ventana.docCompletoImage.Visibility = Visibility.Hidden;
            }
            else
            {
                Ventana.parteSiguiente.Visibility = Visibility.Visible;
                Ventana.parteAnterior.Visibility = Visibility.Visible;
                Ventana.parteFinal.Visibility = Visibility.Visible;
                Ventana.parteInicio.Visibility = Visibility.Visible;
                Ventana.NumeroPartes.Visibility = Visibility.Visible;
#if STAND_ALONE
                Ventana.NumeroPartes.Content = "Parte: 1 de " + partes.Count;
#else
                Ventana.NumeroPartes.Content = "Parte: 1 de " + partes.Length;
#endif
                Ventana.docCompletoImage.Visibility = Visibility.Visible;
            }
            Ventana.contenidoTexto.Document = documentoRubro;
            if ((Busqueda != null) && (Busqueda.Palabra != null))
            {
                foreach (BusquedaPalabraTO item in Busqueda.Palabra)
                {
                    BusquedaPalabraTO palabrejasTO = new BusquedaPalabraTO();
                        palabrejasTO = item;
                    List<String> listapalabras = FlowDocumentHighlight.obtenPalabras(palabrejasTO);
                    Ventana.contenidoTexto.Document = FlowDocumentHighlight.
                        imprimeToken(Ventana.contenidoTexto.Document,
                                     listapalabras, Brushes.Red);
                    List<String> frases = FlowDocumentHighlight.obtenFrases(palabrejasTO);
                    documentoRubro = FlowDocumentHighlight.imprimeToken(documentoRubro, frases, Brushes.DarkGreen);
                }
            }
            Ventana.contenidoTexto.IsReadOnly = true;
            Ventana.contenidoTexto.IsEnabled = true;
            fachada.Close();
            bool encontrado = false;
            bool encontradoverdadero = false;
            bool buscar = false;
            if ((Busqueda != null) && (Busqueda.Palabra != null))
            {
                foreach (BusquedaPalabraTO itemPalabra in Busqueda.Palabra)
                {
                    buscar = (itemPalabra.Campos.Contains(Constants.BUSQUEDA_PALABRA_CAMPO_TEXTO)
                        || itemPalabra.Campos.Contains(Constants.BUSQUEDA_PALABRA_CAMPO_RUBRO)
                        || itemPalabra.Campos.Contains(Constants.BUSQUEDA_PALABRA_CAMPO_ASUNTO));
                }
            }
            numeroParte = 0;
            while (!encontrado)
            {
                if (buscar)
                {
                    encontrado = FlowDocumentHighlight.UbicarPrimera(Ventana.contenidoTexto);
                    encontradoverdadero = encontrado;
                }
                else
                {
                    encontrado = true;
                }
#if STAND_ALONE
                if ((!encontrado) && (numeroParte < (partes.Count - 1)))
                {
                    if (numeroParte < (partes.Count - 1))
#else
                if ((!encontrado) && (numeroParte < (partes.Length-1)))
                {
                    if (numeroParte < (partes.Length - 1))
#endif
                    {
                        numeroParte++;
                    }
                    actualizaTexto();
                }
                else
                {
                    encontrado = true;
                }
            }
#if STAND_ALONE
            if (!encontradoverdadero && (numeroParte == (partes.Count - 1)))
#else
            if (!encontradoverdadero && (numeroParte == (partes.Length - 1)))
#endif
            {
                numeroParte = 0;
                actualizaTexto();
            }
            if (ArregloTesis != null)
            {
                List<EjecutoriaSimplificadaElectoralTO> lista = (List<EjecutoriaSimplificadaElectoralTO>)(ArregloTesis.ItemsSource);
                int posicionReal = posicion + 1;
                Ventana.RegNum.Content = "" + posicionReal + " / " + lista.Count;
                DocumentoActual = documentoActual;
                //impresion.Visibility = Visibility.Hidden;
                Ventana.tabControl1.Visibility = Visibility.Visible;
                //ventanaEmergente.Visibility = Visibility.Hidden;
            }
            else
            {
                posicion = 0;
                DocumentoActual = documentoActual;
                Ventana.tabControl1.Visibility = Visibility.Visible;
            }
            Ventana.TextoABuscar.Text = Constants.CADENA_VACIA;
            Ventana.ventanaListadoTesis.Visibility = Visibility.Hidden;
            Ventana.ventanaListadoVotos.Visibility = Visibility.Hidden;
            Ventana.impresion.Visibility = Visibility.Hidden;
            Ventana.imprimePapel.Visibility = Visibility.Hidden;
            Ventana.BuscarImage.Visibility = Visibility.Visible;
            Ventana.TextoABuscar.Visibility = Visibility.Visible;
            Ventana.Expresion.Visibility = Visibility.Visible;
            try
            {
                String archivo = documentoActual.Complemento.Substring(0, documentoActual.Complemento.Length - 4) + ".htm";
                Uri URL = new Uri(IUSConstants.IUS_RUTA_ANEXOS + "Electoral/Sent/" + archivo, UriKind.Absolute);
                Ventana.FrmDoc.Source = URL;
            }
            catch (Exception e)
            {
                MessageBox.Show("Hubo un problema al poner el archivo " + e.Message,
                    "Error al abrir sentencia", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            if (Ventana.TabDoc.IsEnabled)
            {
                TabControlChanged();
            }
        }

        #region IEjecutoriaElectoral Members

        public void ActualizaRango(int inicial, int FinRango)
        {
            throw new NotImplementedException();
        }

        public void IniciaPagina()
        {
            Ventana.contenidoTexto.FontSize = CalculosPropiedadesGlobales.FontSize;
        }

        public void GuardarClic()
        {
            if (!BrowserInteropHelper.IsBrowserHosted)
            {
                ImprimirClic();
                Microsoft.Win32.SaveFileDialog guardaEn = new Microsoft.Win32.SaveFileDialog();
                guardaEn.DefaultExt = ".rtf";
                guardaEn.Title = "Guardar una ejecutoria";
                guardaEn.Filter = "Archivos de Texto Enriquecido|*.rtf";
                guardaEn.AddExtension = true;
                EscondeVistaPrel();
                if ((bool)guardaEn.ShowDialog())
                {
                    FlowDocument documentoImprimir = DocumentoCopia;
                    Ventana.impresion.Document = null;
                    Ventana.RtbCopyPaste.Document = documentoImprimir;
                    try
                    {
                        System.IO.FileStream archivo = new System.IO.FileStream(guardaEn.FileName, System.IO.FileMode.Create);
                        Ventana.RtbCopyPaste.SelectAll();
                        Ventana.RtbCopyPaste.Selection.Save(archivo, System.Windows.DataFormats.Rtf);
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
                    MostrarDatos(DocumentoActual);
                }
            }
            else
            {
                ImprimirClic();
                FlowDocument documentoImprimir = DocumentoCopia;
                Ventana.impresion.Document = null;
                Ventana.RtbCopyPaste.Document = documentoImprimir;
                System.IO.IsolatedStorage.IsolatedStorageFileStream archivo = new System.IO.IsolatedStorage.
                    IsolatedStorageFileStream("texto.rtf", System.IO.FileMode.Create);
                Ventana.RtbCopyPaste.SelectAll();
                Ventana.RtbCopyPaste.Selection.Save(archivo, System.Windows.DataFormats.Text);
                archivo.Flush();
                archivo.Close();
                MostrarDatos(DocumentoActual);
                MessageBox.Show("El archivo fue guardado como: " + archivo.Name);
            }
        }

        private void EscondeVistaPrel()
        {
            if (verVentanaListadoVotos) Ventana.ventanaListadoVotos.Visibility = Visibility.Visible;
            if (verVentanaListadoTesis) Ventana.ventanaListadoTesis.Visibility = Visibility.Visible;
            if (verVentanaRangos) Ventana.ventanaRangos.Visibility = Visibility.Visible;

            if ((ArregloTesis != null) && (ArregloTesis.Items.Count > 1))
            {
                Ventana.anteriorLista.Visibility = Visibility.Visible;
                Ventana.inicioLista.Visibility = Visibility.Visible;
                Ventana.siguienteLista.Visibility = Visibility.Visible;
                Ventana.ultimoLista.Visibility = Visibility.Visible;
                Ventana.IrALabel.Visibility = Visibility.Visible;
                Ventana.IrBoton.Visibility = Visibility.Visible;
                Ventana.RegNum.Visibility = Visibility.Visible;
                Ventana.regNum.Visibility = Visibility.Visible;
                //Marcar.Visibility = Visibility.Visible;
                //MarcarTodo.Visibility = Visibility.Visible;
                //Desmarcar.Visibility = Visibility.Visible;
            }
#if STAND_ALONE
            if (documentosTesis.Count > 0)
#else
            if (documentosTesis.Length > 0)
#endif
            {
                Ventana.tesis.Visibility = Visibility.Visible;
            }
#if STAND_ALONE
            if (documentosVotos.Count > 0)
#else
            if (documentosVotos.Length > 0)
#endif
            {
                Ventana.voto.Visibility = Visibility.Visible;
            }
            Ventana.Portapapeles.Visibility = Visibility.Visible;
            Ventana.Imprimir.Visibility = Visibility.Visible;
            Ventana.imprimePapel.Visibility = Visibility.Hidden;
            Ventana.BtnTache.Visibility = Visibility.Hidden;
            if (verFontMayor) Ventana.FontMayor.Visibility = Visibility.Visible;
            if (verFontMenor) Ventana.FontMenor.Visibility = Visibility.Visible;
            Ventana.Salir.Visibility = Visibility.Visible;
            Ventana.Guardar.Visibility = !BrowserInteropHelper.IsBrowserHosted ? Visibility.Visible : Visibility.Hidden;
            Ventana.fuenteLabel.Visibility = Visibility.Visible;
            Ventana.EpocaLabel.Visibility = Visibility.Visible;
            Ventana.IdLabel.Visibility = Visibility.Visible;
            Ventana.SalaLabel.Visibility = Visibility.Visible;
            Ventana.VolumenLabel.Visibility = Visibility.Visible;
            Ventana.PaginaLabel.Visibility = Visibility.Visible;
            if (ExistenTablas)
            {
                Ventana.TablasAnexos.Visibility = Visibility.Visible;
            }
            Ventana.LblPalabraBuscar.Visibility = Visibility.Visible;
            Ventana.TextoABuscar.Visibility = Visibility.Visible;
            Ventana.BuscarImage.Visibility = Visibility.Visible;
            Ventana.Expresion.Visibility = Visibility.Visible;
            Ventana.tabControl1.Visibility = Visibility.Visible;
            Ventana.impresion.Visibility = Visibility.Hidden;
#if STAND_ALONE
            if ((partes.Count > 1) && (numeroParte > -1))
#else
            if ((partes.Length > 1) && (numeroParte > -1))
#endif
            {
                Ventana.NumeroPartes.Visibility = Visibility.Visible;
                Ventana.docCompletoImage.Visibility = Visibility.Visible;
                Ventana.parteInicio.Visibility = Visibility.Visible;
                Ventana.parteAnterior.Visibility = Visibility.Visible;
                Ventana.parteSiguiente.Visibility = Visibility.Visible;
                Ventana.parteFinal.Visibility = Visibility.Visible;
            }
        }

        public void PortapapelesClic()
        {
            ImprimirClic();
            FlowDocument documentoImprimir = DocumentoCopia;
            Ventana.impresion.Document = null;
            Ventana.RtbCopyPaste.Document = documentoImprimir;
            Ventana.RtbCopyPaste.SelectAll();
            Ventana.RtbCopyPaste.Copy();
            //MostrarDatos(DocumentoActual);
            //this.contenidoTexto.Document = documentoOriginal;
            EscondeVistaPrel();
            MessageBox.Show(Mensajes.MENSAJE_ENVIADO_PORTAPAPELES,
                Mensajes.TITULO_ENVIADO_PORTAPAPELES,
                MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public void FontMayorClic()
        {
            Ventana.FontMenor.Visibility = Visibility.Visible;
            verFontMenor = true;
            if (CalculosPropiedadesGlobales.FontSize < Constants.FONT_MAYOR)
            {
                //this.contenidoTexto.SelectAll();
                CalculosPropiedadesGlobales.FontSize++;
                Ventana.contenidoTexto.SetValue(RichTextBox.FontSizeProperty, CalculosPropiedadesGlobales.FontSize);
            }
            else
            {
                Ventana.FontMayor.Visibility = Visibility.Hidden;
                verFontMayor = false;
            }
        }

        public void FontMenorClic()
        {
            Ventana.FontMayor.Visibility = Visibility.Visible;
            verFontMayor = true;
            if (CalculosPropiedadesGlobales.FontSize > Constants.FONT_MENOR)
            {
                CalculosPropiedadesGlobales.FontSize--;
                Ventana.contenidoTexto.SetValue(RichTextBox.FontSizeProperty, CalculosPropiedadesGlobales.FontSize);
            }
            else
            {
                Ventana.FontMenor.Visibility = Visibility.Hidden;
                verFontMenor = false;
            }
        }

        public void MarcarTodoClic()
        {
            throw new NotImplementedException();
        }

        public void DesmarcarClic()
        {
            throw new NotImplementedException();
        }

        public void MarcarClic()
        {
            throw new NotImplementedException();
        }

        public void InicioListaClic()
        {
            List<EjecutoriaSimplificadaElectoralTO> presentadorDatos = (List<EjecutoriaSimplificadaElectoralTO>)this.ArregloTesis.ItemsSource;
            EjecutoriaSimplificadaElectoralTO ejecutoriasMostrar = presentadorDatos[0];
            posicion = 0;
            this.ArregloTesis.SelectedIndex = posicion;
            this.ArregloTesis.BringItemIntoView(this.ArregloTesis.SelectedItem);
#if STAND_ALONE
            FachadaBusquedaTradicional fachada = new FachadaBusquedaTradicional();
#else
            FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
#endif
            EjecutoriasTO documentoActual = fachada.getEjecutoriaElectoralPorId(Int32.Parse(ejecutoriasMostrar.Id));
            fachada.Close();
            MostrarDatos(documentoActual);
        }

        public void AnteriorListaClic()
        {
            List<EjecutoriaSimplificadaElectoralTO> presentadorDatos = (List<EjecutoriaSimplificadaElectoralTO>)this.ArregloTesis.ItemsSource;
            EjecutoriaSimplificadaElectoralTO ejecutoriaMostrar = null;
            if (posicion == 0)
            {
                ejecutoriaMostrar = presentadorDatos[0];
            }
            else
            {
                posicion--;
                ejecutoriaMostrar = presentadorDatos[posicion];
            }
#if STAND_ALONE
            FachadaBusquedaTradicional fachada = new FachadaBusquedaTradicional();
#else
            FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
#endif
            EjecutoriasTO documentoActual = fachada.getEjecutoriaElectoralPorId(Int32.Parse(ejecutoriaMostrar.Id));
            fachada.Close();
            this.ArregloTesis.SelectedIndex = posicion;
            this.ArregloTesis.BringItemIntoView(this.ArregloTesis.SelectedItem);
            MostrarDatos(documentoActual);
        }

        public void SiguienteListaClic()
        {
            List<EjecutoriaSimplificadaElectoralTO> presentadorDatos = (List<EjecutoriaSimplificadaElectoralTO>)this.ArregloTesis.ItemsSource;
            EjecutoriaSimplificadaElectoralTO ejecutoriaMostrar = null;
            if (posicion >= presentadorDatos.Count - 1)
            {
                posicion = presentadorDatos.Count - 1;
                ejecutoriaMostrar = presentadorDatos[posicion];
            }
            else
            {
                posicion++;
                ejecutoriaMostrar = presentadorDatos[posicion];
            }
#if STAND_ALONE
            FachadaBusquedaTradicional fachada = new FachadaBusquedaTradicional();
#else
            FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
#endif
            EjecutoriasTO documentoActual = fachada.getEjecutoriaElectoralPorId(Int32.Parse(ejecutoriaMostrar.Id));
            fachada.Close();
            this.ArregloTesis.SelectedIndex = posicion;
            this.ArregloTesis.BringItemIntoView(this.ArregloTesis.SelectedItem);
            MostrarDatos(documentoActual);
        }

        public void UltimoListaClic()
        {
            List<EjecutoriaSimplificadaElectoralTO> presentadorDatos = (List<EjecutoriaSimplificadaElectoralTO>)this.ArregloTesis.ItemsSource;
            EjecutoriaSimplificadaElectoralTO ejecutoriaMostrar = null;
            posicion = presentadorDatos.Count - 1;
            ejecutoriaMostrar = presentadorDatos[posicion];
#if STAND_ALONE
            FachadaBusquedaTradicional fachada = new FachadaBusquedaTradicional();
#else
            FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
#endif
            EjecutoriasTO documentoActual = fachada.getEjecutoriaElectoralPorId(Int32.Parse(ejecutoriaMostrar.Id));
            fachada.Close();
            this.ArregloTesis.SelectedIndex = posicion;
            this.ArregloTesis.BringItemIntoView(this.ArregloTesis.SelectedItem);
            MostrarDatos(documentoActual);
        }

        public void RegNumLetra(System.Windows.Input.KeyEventArgs e)
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
            if (Ventana.regNum.Text.Equals(""))
            {
                MessageBox.Show(Mensajes.MENSAJE_CAMPO_TEXTO_NUMERO_VACIO, Mensajes.TITULO_CAMPO_REQUERIDO,
                    MessageBoxButton.OK, MessageBoxImage.Error);
                Ventana.regNum.Focus();
                return;
            }
            int registro = Int32.Parse(Ventana.regNum.Text);
            List<EjecutoriaSimplificadaElectoralTO> arregloEjecutoriasActual = (List<EjecutoriaSimplificadaElectoralTO>)ArregloTesis.ItemsSource;
            if (registro > 0 && registro <= arregloEjecutoriasActual.Count)
            {
                registro--;
                EjecutoriaSimplificadaElectoralTO ejecutoriaActual = arregloEjecutoriasActual[registro];
#if STAND_ALONE
                FachadaBusquedaTradicional fachada = new FachadaBusquedaTradicional();
#else
            FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
#endif
                EjecutoriasTO ejecutoriaCompleta = fachada.getEjecutoriaElectoralPorId(Int32.Parse(ejecutoriaActual.Id));
                fachada.Close();
                posicion = registro;
                MostrarDatos(ejecutoriaCompleta);
                Ventana.regNum.Text = "";
            }
            else
            {
                Ventana.regNum.Text = "";
                MessageBox.Show(Mensajes.MENSAJE_CONSECUTIVO_NO_VALIDO, Mensajes.TITULO_CONSECUTIVO_NO_VALIDO,
                    MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        public void SalirClic()
        {
            Ventana.NavigationService.Navigate(Ventana.Back);
        }

        public void ImprimirClic()
        {
            if (Ventana.TablasAnexos.Visibility == Visibility.Visible)
            {
                MessageBox.Show(Mensajes.MENSAJE_TABLAS_EXISTENTES, Mensajes.TITULO_TABLAS_EXISTENTES,
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }

            if (Ventana.imprimePapel.Visibility == Visibility.Hidden)
            {
                DocumentoEjecutorias documento;
                MessageBoxResult resultadoMsgBox = MessageBoxResult.Cancel;
                resultadoMsgBox = MessageBoxResult.No;
                if (resultadoMsgBox.Equals(MessageBoxResult.No))
                {
                    EjecutoriaSimplificadaElectoralTO ejecutoriaImprimir = new EjecutoriaSimplificadaElectoralTO();
                    ejecutoriaImprimir.Id = DocumentoActual.Id;
                    documento = new DocumentoEjecutorias(ejecutoriaImprimir, numeroParte + 1);
                }
           
                else
                {
                    return;
                }
                Ventana.impresion.Document = documento.Documento; //(IDocumentPaginatorSource)documentoXps;
                DocumentoCopia = documento.Copia;
                Ventana.impresion.Visibility = Visibility.Visible;
                Ventana.impresion.Background = Brushes.White;
                Ventana.tabControl1.Visibility = Visibility.Hidden;
                Ventana.imprimePapel.Visibility = Visibility.Visible;
                Ventana.TextoABuscar.Visibility = Visibility.Hidden;
                Ventana.BuscarImage.Visibility = Visibility.Hidden;
                Ventana.Expresion.Visibility = Visibility.Hidden;
                MuestraVistaPrel();
            }
            else
            {
                Ventana.impresion.Visibility = Visibility.Hidden;
                //impresion.Background = Brushes.Transparent;
                Ventana.tabControl1.Visibility = Visibility.Visible;
                Ventana.imprimePapel.Visibility = Visibility.Hidden;
                Ventana.TextoABuscar.Visibility = Visibility.Visible;
                Ventana.BuscarImage.Visibility = Visibility.Visible;
                Ventana.Expresion.Visibility = Visibility.Visible;
                EscondeVistaPrel();
            }
        }

        private void MuestraVistaPrel()
        {
            verVentanaListadoVotos = Ventana.ventanaListadoVotos.Visibility == Visibility.Visible;
            verVentanaListadoTesis = Ventana.ventanaListadoTesis.Visibility == Visibility.Visible;
            verVentanaRangos = Ventana.ventanaRangos.Visibility == Visibility.Visible;
            verVentanaAnexos = Ventana.ventanaRangos.Visibility == Visibility.Visible;
            Ventana.ventanaListadoVotos.Visibility = Visibility.Hidden;
            Ventana.ventanaListadoTesis.Visibility = Visibility.Hidden;
            Ventana.ventanaRangos.Visibility = Visibility.Hidden;
            Ventana.impresion.Visibility = Visibility.Visible;
            Ventana.tabControl1.Visibility = Visibility.Hidden;
            Ventana.anteriorLista.Visibility = Visibility.Hidden;
            Ventana.inicioLista.Visibility = Visibility.Hidden;
            Ventana.siguienteLista.Visibility = Visibility.Hidden;
            Ventana.ultimoLista.Visibility = Visibility.Hidden;
            Ventana.IrALabel.Visibility = Visibility.Hidden;
            Ventana.IrBoton.Visibility = Visibility.Hidden;
            Ventana.RegNum.Visibility = Visibility.Hidden;
            Ventana.regNum.Visibility = Visibility.Hidden;
            Ventana.tesis.Visibility = Visibility.Hidden;
            Ventana.voto.Visibility = Visibility.Hidden;
            Ventana.TablasAnexos.Visibility = Visibility.Hidden;
            Ventana.Portapapeles.Visibility = Visibility.Hidden;
            Ventana.Imprimir.Visibility = Visibility.Hidden;
            Ventana.imprimePapel.Visibility = Visibility.Visible;
            Ventana.BtnTache.Visibility = Visibility.Visible;
            Ventana.FontMayor.Visibility = Visibility.Hidden;
            Ventana.FontMenor.Visibility = Visibility.Hidden;
            Ventana.Salir.Visibility = Visibility.Hidden;
            Ventana.Guardar.Visibility = Visibility.Hidden;
            Ventana.fuenteLabel.Visibility = Visibility.Hidden;
            Ventana.EpocaLabel.Visibility = Visibility.Hidden;
            Ventana.IdLabel.Visibility = Visibility.Hidden;
            Ventana.SalaLabel.Visibility = Visibility.Hidden;
            Ventana.VolumenLabel.Visibility = Visibility.Hidden;
            Ventana.PaginaLabel.Visibility = Visibility.Hidden;
            Ventana.LblPalabraBuscar.Visibility = Visibility.Hidden;
            Ventana.TextoABuscar.Visibility = Visibility.Hidden;
            Ventana.BuscarImage.Visibility = Visibility.Hidden;
            Ventana.Expresion.Visibility = Visibility.Hidden;
            Ventana.parteAnterior.Visibility = Visibility.Hidden;
            Ventana.parteInicio.Visibility = Visibility.Hidden;
            Ventana.parteSiguiente.Visibility = Visibility.Hidden;
            Ventana.parteFinal.Visibility = Visibility.Hidden;
            Ventana.docCompletoImage.Visibility = Visibility.Hidden;
            Ventana.NumeroPartes.Visibility = Visibility.Hidden;
        }

        #endregion

        #region IEjecutoriaElectoral Members


        public void DocumentoCompletoClic()
        {
            FlowDocument documentoRubro = new FlowDocument();
//#if STAND_ALONE
//            if (documentosTesis.Count == 0)
//#else
//            if (documentosTesis.Length == 0)
//#endif
//            {
//                documentoRubro.Blocks.Add(new Paragraph(new Run(DocumentoActual.Rubro)));
//            }
            foreach (EjecutoriasPartesTO item in partes)
            {
                Paragraph textoParrafo = ObtenLigas(item.TxtParte, "" + item.Id, item.Parte);
                textoParrafo.FontWeight = FontWeights.Normal;
                textoParrafo.TextAlignment = TextAlignment.Justify;
                documentoRubro.Blocks.Add(textoParrafo);
            }
            Ventana.contenidoTexto.Document = documentoRubro;
            if ((Busqueda != null) && (Busqueda.Palabra != null))
            {
                foreach (BusquedaPalabraTO item in Busqueda.Palabra)
                {
                    List<String> listapalabras = FlowDocumentHighlight.obtenPalabras(item);
                    Ventana.contenidoTexto.Document = FlowDocumentHighlight.
                        imprimeToken(Ventana.contenidoTexto.Document,
                                     listapalabras, Brushes.Red);
                    List<String> frases = FlowDocumentHighlight.obtenFrases(item);
                    documentoRubro = FlowDocumentHighlight.imprimeToken(documentoRubro, frases, Brushes.DarkGreen);
                }

            }
            Ventana.parteSiguiente.Visibility = Visibility.Hidden;
            Ventana.parteAnterior.Visibility = Visibility.Hidden;
            Ventana.parteFinal.Visibility = Visibility.Hidden;
            Ventana.parteInicio.Visibility = Visibility.Hidden;
            Ventana.NumeroPartes.Visibility = Visibility.Hidden;
            Ventana.docCompletoImage.Visibility = Visibility.Hidden;
            numeroParte = -2;
        }

        public void AnexosClic()
        {
            throw new NotImplementedException();
        }

        public void TesisClic()
        {

#if STAND_ALONE
            if (documentosTesis.Count == 1)
#else
            if (documentosTesis.Length == 1)
#endif
            {
                try
                {
#if STAND_ALONE
                    FachadaBusquedaTradicional fachada = new FachadaBusquedaTradicional();
#else
            FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
#endif
                    TesisTO tesisCompleta = fachada.getTesisElectoralPorRegistro(documentosTesis[0].Ius);
                    TesisElectoral tesisAsociada = new TesisElectoral(tesisCompleta);
                    tesisAsociada.Back = Ventana;
                    Ventana.NavigationService.Navigate(tesisAsociada);
                }
                catch (Exception exc)
                {
                    System.Console.WriteLine("Dieron Click muy rápido" + exc.Message);
                }
            }
            else
            {
                List<int> identificadores = new List<int>();
                Ventana.ventanaListadoTesis.NavigationService = Ventana;
                foreach (RelDocumentoTesisTO item in documentosTesis)
                {
                    identificadores.Add(Int32.Parse(item.Ius));
                }
                MostrarPorIusTO parametros = new MostrarPorIusTO();
                parametros.OrderBy = "ConsecIndx";
                parametros.OrderType = "asc";
#if STAND_ALONE
                FachadaBusquedaTradicional fachada = new FachadaBusquedaTradicional();
                parametros.Listado = identificadores;
                List<TesisTO> ejecutoriasRelacionadas = fachada.getTesisElectoralPorIus(parametros);
#else
            FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
                parametros.Listado = identificadores.ToArray();
                TesisTO[] ejecutoriasRelacionadas = fachada.getTesisElectoralPorIus(parametros);
#endif
                List<TesisSimplificadaElectoralTO> listaFinal = new List<TesisSimplificadaElectoralTO>();
                foreach (TesisTO item in ejecutoriasRelacionadas)
                {
                    TesisSimplificadaElectoralTO itemCompleto = new TesisSimplificadaElectoralTO();
                    itemCompleto.Ius = item.Ius;
                    itemCompleto.ConsecIndx = item.ConsecIndx;
                    listaFinal.Add(itemCompleto);
                }
                Ventana.ventanaListadoTesis.TesisMostrar = listaFinal;
                Ventana.ventanaListadoTesis.Visibility = Visibility.Visible;
                //MessageBox.Show("hola");
                fachada.Close();
            }
        }

        public void VotoClic()
        {
            throw new NotImplementedException();
        }

        public void ParteInicioClic()
        {
            numeroParte = 0;
            actualizaTexto();
        }

        public void ParteAnteriorClic()
        {
            if (numeroParte > 0)
            {
                numeroParte--;
            }
            actualizaTexto();
        }

        public void ParteSiguienteClic()
        {
#if STAND_ALONE
            if (numeroParte < (partes.Count - 1))
#else
            if (numeroParte < (partes.Length-1))
#endif
            {
                numeroParte++;
            }
            actualizaTexto();
        }

        public void ParteFinalClic()
        {
#if STAND_ALONE
            numeroParte = partes.Count - 1;
#else
            numeroParte = partes.Length-1;
#endif
            actualizaTexto();
        }


        private void actualizaTexto()
        {
            FlowDocument documentoRubro = new FlowDocument();
            Paragraph textoParrafo = null;
#if STAND_ALONE
            if ((documentosTesis.Count == 0) && (numeroParte == 0))
#else
            if ((documentosTesis.Length == 0) && (numeroParte == 0))
#endif
            {
                textoParrafo = ObtenLigas(/*DocumentoActual.Rubro + "\n\n"
                    +*/ partes[numeroParte].TxtParte, DocumentoActual.Id, numeroParte + 1);
            }
            else
            {
                textoParrafo = ObtenLigas(partes[numeroParte].TxtParte, DocumentoActual.Id, numeroParte + 1);
            }
            textoParrafo.FontWeight = FontWeights.Normal;
            textoParrafo.TextAlignment = TextAlignment.Justify;
            documentoRubro.Blocks.Add(textoParrafo);
            int parteReal = numeroParte + 1;
#if STAND_ALONE
            Ventana.NumeroPartes.Content = "Parte: " + parteReal + " / " + partes.Count;
#else
            Ventana.NumeroPartes.Content = "Parte: " + parteReal + " / " + partes.Length;
#endif
            Ventana.contenidoTexto.Document = documentoRubro;
            if ((Busqueda != null) && (Busqueda.Palabra != null))
            {
                foreach (BusquedaPalabraTO item in Busqueda.Palabra)
                {
                    BusquedaPalabraTO palabrejasTO = new BusquedaPalabraTO();
                    palabrejasTO = item;
                    List<String> listapalabras = FlowDocumentHighlight.obtenPalabras(palabrejasTO);
                    documentoRubro = FlowDocumentHighlight.
                        imprimeToken(documentoRubro,
                                     listapalabras, Brushes.Red);
                    List<String> frases = FlowDocumentHighlight.obtenFrases(palabrejasTO);
                    documentoRubro = FlowDocumentHighlight.imprimeToken(documentoRubro, frases, Brushes.DarkGreen);
                }
            }
            Ventana.contenidoTexto.Document = documentoRubro;
        }

        public void ImprimePapelClic()
        {
            PrintDialog dialogoImpresion = new PrintDialog();
            IDocumentPaginatorSource paginado = Ventana.impresion.Document as IDocumentPaginatorSource;
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
                    dialogoImpresion.PrintDocument(pgn, "Impresión de votos");
                }
                catch (Exception exc)
                {
                    MessageBox.Show(Mensajes.MENSAJE_IMPRESORA, Mensajes.TITULO_ARCHIVO_ABIERTO,
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
                EscondeVistaPrel();
            }
        }

        public void TacheClic()
        {
            EscondeVistaPrel();
        }

        public void ContenidoTextoCopy(System.Windows.DataObjectCopyingEventArgs e)
        {
            e.Handled = true;
            RichTextBox rtb = Ventana.contenidoTexto;
            Ventana.TbxCopiar.Text = Ventana.contenidoTexto.Selection.Text;
            e.CancelCommand();
            Ventana.TbxCopiar.SelectAll();
            Ventana.TbxCopiar.Copy();
        }

        public void TextoABuscarTecla(System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                BuscarClic();
            }
        }

        public void BuscarClic()
        {
            String validar = Validadores.BusquedaPalabraDocumento(Ventana.TextoABuscar);
            
            if (!validar.Equals(Constants.CADENA_VACIA))
            {
                return;
            }
            if (!Ventana.TabTexto.IsSelected)
            {
                Ventana.TabTexto.IsSelected = true;
            }
            String busquedaTexto = Ventana.TextoABuscar.Text;
            TextPointer inicio = Ventana.contenidoTexto.Selection.Start;
            TextPointer final = Ventana.contenidoTexto.Selection.End;
            TextPointer lugarActual = null;
            if (inicio.CompareTo(final) == 0)
            {
                lugarActual = inicio;//this.contenidoTexto.Document.ContentStart;
            }
            else
            {
                lugarActual = Ventana.contenidoTexto.Selection.End;
            }
            lugarActual = FlowDocumentHighlight.Find(busquedaTexto, Ventana.contenidoTexto, lugarActual);
            if (lugarActual.CompareTo(Ventana.contenidoTexto.Document.ContentEnd) == 0)
            {
                if (EncontradaFrase)
                {
                    MessageBox.Show(Mensajes.MENSAJE_NO_HAY_MAS_COINCIDENCIAS, Mensajes.TITULO_NO_HAY_MAS_COINCIDENCIAS,
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show(Mensajes.MENSAJE_NO_HAY_COINCIDENCIAS, Mensajes.TITULO_NO_HAY_COINCIDENCIAS,
                        MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
                TextRange rango = new TextRange(Ventana.contenidoTexto.Document.ContentStart, Ventana.contenidoTexto.Document.ContentStart);
                rango.Select(Ventana.contenidoTexto.Document.ContentStart, Ventana.contenidoTexto.Document.ContentStart);
            }
            else
            {
                EncontradaFrase = true;
                inicio = Ventana.contenidoTexto.Selection.Start;
                final = Ventana.contenidoTexto.Selection.End;
                Ventana.contenidoTexto.Selection.Select(inicio, final);
                Ventana.contenidoTexto.Focus();
            }
        }

        public void ContenidoTextoTecla()
        {
            
        }

        public void TextoBuscarTecla(System.Windows.Controls.TextChangedEventArgs e)
        {
            Ventana.contenidoTexto.Selection.Select(Ventana.contenidoTexto.Document.ContentStart,
                Ventana.contenidoTexto.Document.ContentStart);
            EncontradaFrase = false;
        }

        #endregion
        /// <summary>
        /// Define la cadena de texto para generar las ligas del documento hacia
        /// leyes u otros objetos similares.
        /// </summary>
        /// <param name="texto"> El texto ue tendrá la liga</param>
        /// <param name="ius"> El Ius del documento </param>
        /// <param name="seccion"> La seccion donde estará la liga</param>
        /// <returns>El párrafo con la liga adecuada.</returns>
        protected Paragraph ObtenLigas(String texto, String id, int parte)
        {
            Paragraph resultado = new Paragraph(new Run(texto));
#if STAND_ALONE
            FachadaBusquedaTradicional fachada = new FachadaBusquedaTradicional();
            List<TablaPartesTO> listaRelaciones = fachada.getTablaEjecutoriasElectoral(Int32.Parse(id));
            //Ventana.ventanaAnexos.listado.ItemsSource = listaRelaciones;
            if (listaRelaciones.Count == 0)
#else
            FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
            TablaPartesTO[] listaRelaciones = fachada.getTablaEjecutoriasElectoral(Int32.Parse(id));
            //Ventana.Anexos.listado.ItemsSource = listaRelaciones.ToList();
            if (listaRelaciones.Length == 0)
#endif
            {
                Ventana.TablasAnexos.Visibility = Visibility.Hidden;
                ExistenTablas = false;
            }
            else
            {
                ExistenTablas = true;
                Ventana.TablasAnexos.Visibility = Visibility.Visible;
            }
            fachada.Close();
            List<String> resultadosParciales = new List<String>();
            String cadenaNueva;
            int posicionInicial = 0, posicionFinal = 0;
#if STAND_ALONE
            if (listaRelaciones.Count == 0)
#else
            if (listaRelaciones.Length == 0)
#endif
            {
                return resultado;
            }
            else
            {
                resultado = new Paragraph();
                foreach (TablaPartesTO item in listaRelaciones)
                {
                    if (parte == -1)
                    {
                        posicionFinal = item.Posicion;
                    }
                    else if (parte == item.Parte)
                    {
                        posicionFinal = item.PosicionParte;
                    }
                    else
                    {
                        posicionFinal = -1;
                    }
                    if (posicionFinal != -1)
                    {
                        if (posicionFinal < posicionInicial) posicionFinal = posicionInicial;
                        cadenaNueva = texto.Substring(posicionInicial, posicionFinal - posicionInicial);
                        String resto = texto.Substring(posicionInicial + (posicionFinal - posicionInicial));
                        if (!resto.StartsWith(item.Frase))
                        {
                            int restoPos = resto.IndexOf(item.Frase);
                            if (restoPos != -1)
                            {
                                posicionFinal = posicionFinal + restoPos;
                            }
                            else
                            {
                                posicionFinal = texto.IndexOf(item.Frase);
                            }
                            cadenaNueva = texto.Substring(posicionInicial, posicionFinal - posicionInicial);
                        }
                        resultadosParciales.Add(cadenaNueva);
                        resultado.Inlines.Add(cadenaNueva);
                        resultado.Inlines.Add(creaLiga(item, item.Frase));
                        posicionInicial = posicionFinal + item.Frase.Length;
                    }
                }
                resultado.Inlines.Add(texto.Substring(posicionInicial));
                resultado.IsEnabled = true;
                return resultado;
            }
        }
        /// <summary>
        /// Crea la liga necesaria de acuerdo a los datos solicitados haciendo
        /// una verificación del tipo de liga y llenando el campo TAG
        /// con los valores necesarios para que el click funcione adecuadamente.
        /// </summary>
        /// <param name="liga">La liga que se pintará</param>
        /// <param name="contenido">El contenido de la liga</param>
        /// <returns>La liga ya lista para añadirse al documento</returns>
        private IUSHyperlink creaLiga(TablaPartesTO liga, string contenido)
        {
            //RelacionFraseTesisTO tesis = null;
            //RelacionFraseArticulosTO articulos = null;
            //FachadaBusquedaTradicionalClient fachadaBusqueda = new FachadaBusquedaTradicionalClient();
            IUSHyperlink ligaNueva = new IUSHyperlink(Ventana.tabControl1);
            ligaNueva.Inlines.Add(new Run(contenido));
            ligaNueva.IsEnabled = true;
            ligaNueva.Tag = "PDF(" + liga.Archivo + ")";
            ligaNueva.PaginaTarget = Ventana;
            //fachadaBusqueda.Close();
            return ligaNueva;
        }
        public void TabControlChanged()
        {
            if (Ventana.TabDoc.IsSelected)
            {
                Ventana.FontMayor.Visibility = Visibility.Hidden;
                Ventana.FontMenor.Visibility = Visibility.Hidden;
                Ventana.parteInicio.Visibility = Visibility.Hidden;
                Ventana.parteAnterior.Visibility = Visibility.Hidden;
                Ventana.parteSiguiente.Visibility = Visibility.Hidden;
                Ventana.parteFinal.Visibility = Visibility.Hidden;
                Ventana.docCompletoImage.Visibility = Visibility.Hidden;
                Ventana.tesis.Visibility = Visibility.Hidden;
                Ventana.voto.Visibility = Visibility.Hidden;
                Ventana.LblPalabraBuscar.Visibility = Visibility.Hidden;
                Ventana.BuscarImage.Visibility = Visibility.Hidden;
                Ventana.TextoABuscar.Visibility = Visibility.Hidden;

            }
            else
            {
                Ventana.FontMayor.Visibility = Visibility.Visible;
                Ventana.FontMenor.Visibility = Visibility.Visible;
                Ventana.LblPalabraBuscar.Visibility = Visibility.Visible;
                Ventana.BuscarImage.Visibility = Visibility.Visible;
                Ventana.TextoABuscar.Visibility = Visibility.Visible;

#if STAND_ALONE
                if (partes.Count > 1)
#else
                if (partes.Length > 1)
#endif
                {
                    Ventana.docCompletoImage.Visibility = Visibility.Visible;
                    Ventana.FontMenor.Visibility = Visibility.Visible;
                    Ventana.parteInicio.Visibility = Visibility.Visible;
                    Ventana.parteAnterior.Visibility = Visibility.Visible;
                    Ventana.parteSiguiente.Visibility = Visibility.Visible;
                    Ventana.parteFinal.Visibility = Visibility.Visible;
                }
#if STAND_ALONE
                if (documentosTesis.Count > 0)
#else
                if(documentosTesis.Length > 0)
#endif
                {
                    Ventana.tesis.Visibility = Visibility.Visible;
                }
#if STAND_ALONE
                if (documentosVotos.Count > 0)
#else
                if(documentosVotos.Length > 0)
#endif
                {
                    Ventana.voto.Visibility = Visibility.Visible;
                }
            }

        }

    }
}
