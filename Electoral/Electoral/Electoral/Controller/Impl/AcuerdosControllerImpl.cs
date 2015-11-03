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
using mx.gob.scjn.electoral_common.gui.impresion;
using mx.gob.scjn.electoral_common.gui.utils;
using mx.gob.scjn.ius_common.TO;
using mx.gob.scjn.ius_common.fachade;
using mx.gob.scjn.ius_common.gui.gui.impresion;
using mx.gob.scjn.ius_common.gui.utils;
using mx.gob.scjn.ius_common.utils;

namespace mx.gob.scjn.electoral.Controller.Impl
{
    public class AcuerdosControllerImpl:IAcuerdosController
    {
        private Xceed.Wpf.DataGrid.DataGridControl ArregloTesis { get; set; }
        private String genealogiaId { get; set; }
#if STAND_ALONE
        private List<AcuerdosPartesTO> partes { get; set; }
#else
        private AcuerdosPartesTO[] partes { get; set; }
#endif
        private int posicion = 0;
        private int numeroParte { get; set; }
        public Acuerdos Ventana { get; set; }
        private AcuerdosTO DocumentoActual { get; set; }
        private FlowDocument DocumentoParaCopiar { get; set; }
        private BusquedaTO Busqueda { get; set; }
        protected bool EncontradaFrase { get; set; }
        protected bool ExistenTablas { get; set; }
        protected bool verVentanaRangos { get; set; }
        protected bool verVentanaListadoAnexos { get; set; }
        protected bool verFlechas { get; set; }
        protected bool verFontMayor { get; set; }
        protected bool verFontMenor { get; set; }

        public AcuerdosControllerImpl(Acuerdos ventana)
        {
            Ventana = ventana;
        }

        public AcuerdosControllerImpl(Acuerdos ventana, DataGridControl records, BusquedaTO busqueda)
        {
            Ventana = ventana;
            CalculosPropiedadesGlobales.FontSize = Ventana.FontSize;
            Busqueda = busqueda;
            Ventana.contenidoTexto.SetValue(RichTextBox.FontSizeProperty, CalculosPropiedadesGlobales.FontSize);
            AcuerdoSimplificadoTO acuerdosMostrar = (AcuerdoSimplificadoTO)records.SelectedItem;
            posicion = records.SelectedIndex;
#if STAND_ALONE
            FachadaBusquedaTradicional fachada = new FachadaBusquedaTradicional();
#else
            FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
#endif
            AcuerdosTO documentoActual = fachada.getAcuerdoElectoralPorId(Int32.Parse(acuerdosMostrar.Id));
            fachada.Close();
            if (busqueda != null)
            {
                Ventana.Expresion.Content = "Acuerdos electorales";
            }
            else
            {
                List<int> registros = new List<int>();
                Ventana.Expresion.Content = "Acuerdos electorales por registros";
            }
            this.ArregloTesis = records;
            if (ArregloTesis.Items.Count < 2)
            {
                verFlechas = false;
                Ventana.inicioLista.Visibility = Visibility.Hidden;
                Ventana.anteriorLista.Visibility = Visibility.Hidden;
                Ventana.siguienteLista.Visibility = Visibility.Hidden;
                Ventana.ultimoLista.Visibility = Visibility.Hidden;
                Ventana.regNum.Visibility = Visibility.Hidden;
                Ventana.RegNum.Visibility = Visibility.Hidden;
                Ventana.IrALabel.Visibility = Visibility.Hidden;
                Ventana.IrBoton.Visibility = Visibility.Hidden;

            }
            MostrarDatos(documentoActual);
        }

        public AcuerdosControllerImpl(Acuerdos ventana, int Id)
        {
            Ventana = ventana;
            Ventana.contenidoTexto.SetValue(RichTextBox.FontSizeProperty, CalculosPropiedadesGlobales.FontSize);
#if STAND_ALONE
            FachadaBusquedaTradicional fachada = new FachadaBusquedaTradicional();
#else
            FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
#endif
            AcuerdosTO documentoActual = fachada.getAcuerdoElectoralPorId(Id);
            MostrarDatos(documentoActual);
            DocumentoActual = documentoActual;
            fachada.Close();
            Ventana.Expresion.Visibility = Visibility.Hidden;
            //Si entran a este constructor es por que vienen para ver solamente un registro
            verFlechas = false;
            Ventana.inicioLista.Visibility = Visibility.Hidden;
            Ventana.anteriorLista.Visibility = Visibility.Hidden;
            Ventana.siguienteLista.Visibility = Visibility.Hidden;
            Ventana.ultimoLista.Visibility = Visibility.Hidden;
            Ventana.regNum.Visibility = Visibility.Hidden;
            Ventana.RegNum.Visibility = Visibility.Hidden;
            Ventana.IrALabel.Visibility = Visibility.Hidden;
            Ventana.IrBoton.Visibility = Visibility.Hidden;
        }


        /// <summary>
        /// Muestra los datos de una ejecutoria determinada.
        /// </summary>
        private void MostrarDatos(AcuerdosTO documentoActual)
        {
            DocumentoActual = documentoActual;
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
            Ventana.SalaLabel.Text = documentoActual.Sala;
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
            partes = fachada.getAcuerdoPartesElectoralPorId(Int32.Parse(documentoActual.Id));
            Paragraph textoParrafo = ObtenLigas(documentoActual.Rubro + "\n\n" + partes[0].TxtParte, documentoActual.Id, 1);
            textoParrafo.TextAlignment = TextAlignment.Justify;
            textoParrafo.FontWeight = FontWeights.Normal;
            documentoRubro.Blocks.Add(textoParrafo);
            numeroParte = 0;
            //textoParrafo=ObtenLigas(partes[0].TxtParte, documentoActual.Id, 1);
            //textoParrafo = new Paragraph(new Run(partes[0].TxtParte));
            //documentoRubro.Blocks.Add(textoParrafo);
            //this.documentosVotos = fachada.getVotoPorEjecutoria(documentoActual.Id);
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
                Ventana.NumeroPartes.Content = "Parte: 1 / " + partes.Count;
#else
                Ventana.NumeroPartes.Content = "Parte: 1 / " + partes.Length;
#endif
                Ventana.docCompletoImage.Visibility = Visibility.Visible;
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
            Ventana.contenidoTexto.IsReadOnly = true;
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
                    {
#else
                if ((!encontrado) && (numeroParte < (partes.Length - 1)))
                {
                    if (numeroParte < (partes.Length - 1))
                    {
#endif
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
                List<AcuerdoSimplificadoTO> lista = (List<AcuerdoSimplificadoTO>)(ArregloTesis.ItemsSource);
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
                Ventana.tabControl1.Visibility = Visibility.Visible;
            }
            Ventana.textoAbuscar.Text = Constants.CADENA_VACIA;
            Ventana.Buscar.Visibility = Visibility.Visible;
            Ventana.textoAbuscar.Visibility = Visibility.Visible;
            Ventana.imprimePapel.Visibility = Visibility.Hidden;
            Ventana.impresion.Visibility = Visibility.Hidden;
            try
            {
                String archivo = documentoActual.Complemento.Substring(0, documentoActual.Complemento.Length - 4) + ".htm";
                Uri URL = new Uri(IUSConstants.IUS_RUTA_ANEXOS + "Electoral/Ac/" + archivo, UriKind.Absolute);
                Ventana.FrmDocumento.Source = URL;
            }
            catch (Exception e)
            {
            }
            if (Ventana.TabDocumento.IsSelected)
            {
                TabControlChanged();
            }
        }

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
            List<TablaPartesTO> listaRelaciones = fachada.getTablaAcuerdos(Int32.Parse(id));
#else
            FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
            TablaPartesTO[] listaRelaciones = fachada.getTablaAcuerdo(Int32.Parse(id));
#endif
#if STAND_ALONE
            if (listaRelaciones.Count == 0)
#else
            if (listaRelaciones.Length == 0)
#endif
            {
                ExistenTablas = false;
            }
            else
            {
                ExistenTablas = true;
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

        private void actualizaTexto()
        {
            String textoImpreso = null;
            if (numeroParte == 0)
            {
                textoImpreso = DocumentoActual.Rubro + partes[numeroParte].TxtParte;
            }
            else
            {
                textoImpreso = partes[numeroParte].TxtParte;
            }
            FlowDocument documentoRubro = new FlowDocument();
            Paragraph textoParrafo = ObtenLigas(textoImpreso, DocumentoActual.Id, numeroParte + 1);
            textoParrafo.FontWeight = FontWeights.Normal;
            textoParrafo.TextAlignment = TextAlignment.Justify;
            documentoRubro.Blocks.Add(textoParrafo);
            int parteReal = numeroParte + 1;
#if STAND_ALONE
            Ventana.NumeroPartes.Content = "Parte: " + parteReal + " / " + partes.Count;
#else
            Ventana.NumeroPartes.Content = "Parte: " + parteReal + " / " + partes.Length;
#endif
            if ((Busqueda != null) && (Busqueda.Palabra != null))
            {
                foreach (BusquedaPalabraTO item in Busqueda.Palabra)
                {
                    List<String> listapalabras = FlowDocumentHighlight.obtenPalabras(item);
                    documentoRubro = FlowDocumentHighlight.
                        imprimeToken(documentoRubro,
                                     listapalabras, Brushes.Red);
                    List<String> frases = FlowDocumentHighlight.obtenFrases(item);
                    documentoRubro = FlowDocumentHighlight.imprimeToken(documentoRubro, frases, Brushes.DarkGreen);
                }

            }
            Ventana.contenidoTexto.Document = documentoRubro;
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
#if STAND_ALONE
            FachadaBusquedaTradicional fachadaBusqueda = new FachadaBusquedaTradicional();
#else
            FachadaBusquedaTradicionalClient fachadaBusqueda = new FachadaBusquedaTradicionalClient();
#endif
            IUSHyperlink ligaNueva = new IUSHyperlink(Ventana.tabControl1);
            ligaNueva.Inlines.Add(new Run(contenido));
            ligaNueva.IsEnabled = true;
            ligaNueva.Tag = "PDF(" + liga.Archivo + ")";
            ligaNueva.PaginaTarget = Ventana;
            fachadaBusqueda.Close();
            return ligaNueva;
        }

        private void EscondeVistaPrel()
        {
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
            }
            Ventana.PortaPapeles.Visibility = Visibility.Visible;
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
            Ventana.LblSala.Visibility = Visibility.Visible;
            Ventana.VolumenLabel.Visibility = Visibility.Visible;
            Ventana.PaginaLabel.Visibility = Visibility.Visible;
            //if (ExistenTablas)
            //{
            //    Ventana.BtnTablas.Visibility = Visibility.Visible;
            //}
            Ventana.LblPalabraBuscar.Visibility = Visibility.Visible;
            Ventana.textoAbuscar.Visibility = Visibility.Visible;
            Ventana.Buscar.Visibility = Visibility.Visible;
            Ventana.Expresion.Visibility = Visibility.Visible;
            Ventana.tabControl1.Visibility = Visibility.Visible;
            Ventana.impresion.Visibility = Visibility.Hidden;
#if STAND_ALONE
            if ((partes.Count > 1) && (numeroParte > -1))
#else
            if ((partes.Length > 1) && (numeroParte > -1))
#endif
            {
                Ventana.docCompletoImage.Visibility = Visibility.Visible;
                Ventana.parteInicio.Visibility = Visibility.Visible;
                Ventana.parteAnterior.Visibility = Visibility.Visible;
                Ventana.parteSiguiente.Visibility = Visibility.Visible;
                Ventana.parteFinal.Visibility = Visibility.Visible;
                Ventana.NumeroPartes.Visibility = Visibility.Visible;
            }
        }
        private void MuestraVistaPrel()
        {
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
            //tesis.Visibility = Visibility.Hidden;
            //ejecutoria.Visibility = Visibility.Hidden;
            //Ventana.BtnTablas.Visibility = Visibility.Hidden;
            Ventana.PortaPapeles.Visibility = Visibility.Hidden;
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
            Ventana.LblSala.Visibility = Visibility.Hidden;
            Ventana.VolumenLabel.Visibility = Visibility.Hidden;
            Ventana.PaginaLabel.Visibility = Visibility.Hidden;
            Ventana.LblPalabraBuscar.Visibility = Visibility.Hidden;
            Ventana.textoAbuscar.Visibility = Visibility.Hidden;
            Ventana.Buscar.Visibility = Visibility.Hidden;
            Ventana.Expresion.Visibility = Visibility.Hidden;
            Ventana.parteAnterior.Visibility = Visibility.Hidden;
            Ventana.parteInicio.Visibility = Visibility.Hidden;
            Ventana.parteSiguiente.Visibility = Visibility.Hidden;
            Ventana.parteFinal.Visibility = Visibility.Hidden;
            Ventana.docCompletoImage.Visibility = Visibility.Hidden;
            Ventana.NumeroPartes.Visibility = Visibility.Hidden;
        }

        #region IAcuerdosController Members

        public void GuardarClic()
        {
            if (!BrowserInteropHelper.IsBrowserHosted)
            {
                ImprimirClic();
                Microsoft.Win32.SaveFileDialog guardaEn = new Microsoft.Win32.SaveFileDialog();
                guardaEn.DefaultExt = ".rtf";
                guardaEn.Title = "Guardar un acuerdo";
                guardaEn.Filter = "Archivos de Texto Enriquecido|*.rtf";
                guardaEn.AddExtension = true;
                EscondeVistaPrel();
                if ((bool)guardaEn.ShowDialog())
                {
                    FlowDocument documentoImprimir = DocumentoParaCopiar;
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
                FlowDocument documentoImprimir = DocumentoParaCopiar;
                Ventana.impresion.Document = null;
                Ventana.contenidoTexto.Document = documentoImprimir;
                System.IO.IsolatedStorage.IsolatedStorageFileStream archivo = new System.IO.IsolatedStorage.
                    IsolatedStorageFileStream("texto.rtf", System.IO.FileMode.Create);
                Ventana.contenidoTexto.SelectAll();
                Ventana.contenidoTexto.Selection.Save(archivo, System.Windows.DataFormats.Text);
                archivo.Flush();
                archivo.Close();
                MostrarDatos(DocumentoActual);
                MessageBox.Show(Mensajes.MENSAJE_GUARDADO + archivo.Name,
                    Mensajes.TITULO_GUARDADO,
                    MessageBoxButton.OK,
                    MessageBoxImage.Exclamation);
            }
        }

        public void PortapapelesClic()
        {
            ImprimirClic();
            FlowDocument documentoImprimir = DocumentoParaCopiar;
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

        public void FontMenorClic()
        {
            Ventana.FontMayor.Visibility = Visibility.Visible;
            verFontMayor = true;
            if (CalculosPropiedadesGlobales.FontSize > Constants.FONT_MENOR)
            {
                //this.contenidoTexto.SelectAll();
                CalculosPropiedadesGlobales.FontSize--;
                Ventana.contenidoTexto.SetValue(RichTextBox.FontSizeProperty, CalculosPropiedadesGlobales.FontSize);
            }
            else
            {
                Ventana.FontMenor.Visibility = Visibility.Hidden;
                verFontMenor = false;
            }
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
                verFontMayor = true;
            }
        }

        public void InicioListaClic()
        {
            List<AcuerdoSimplificadoTO> presentadorDatos = (List<AcuerdoSimplificadoTO>)this.ArregloTesis.ItemsSource;
            AcuerdosTO acuerdoMostrar = new AcuerdosTO();
            acuerdoMostrar.Id = presentadorDatos[0].Id;
            posicion = 0;
            this.ArregloTesis.SelectedIndex = posicion;
            this.ArregloTesis.BringItemIntoView(this.ArregloTesis.SelectedItem);
#if STAND_ALONE
            FachadaBusquedaTradicional fachada = new FachadaBusquedaTradicional();
#else
            FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
#endif
            AcuerdosTO documentoActual = fachada.getAcuerdoElectoralPorId(Int32.Parse(acuerdoMostrar.Id));
            fachada.Close();
            MostrarDatos(documentoActual);
        }

        public void AnteriorListaClic()
        {
            List<AcuerdoSimplificadoTO> presentadorDatos = (List<AcuerdoSimplificadoTO>)this.ArregloTesis.ItemsSource;
            AcuerdosTO acuerdoMostrar = null;
            if (posicion == 0)
            {
                acuerdoMostrar = new AcuerdosTO();
                acuerdoMostrar.Id = presentadorDatos[0].Id;
            }
            else
            {
                posicion--;
                acuerdoMostrar = new AcuerdosTO();
                acuerdoMostrar.Id = presentadorDatos[posicion].Id;
            }
#if STAND_ALONE
            FachadaBusquedaTradicional fachada = new FachadaBusquedaTradicional();
#else
            FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
#endif
            AcuerdosTO documentoActual = fachada.getAcuerdoElectoralPorId(Int32.Parse(acuerdoMostrar.Id));
            fachada.Close();
            this.ArregloTesis.SelectedIndex = posicion;
            this.ArregloTesis.BringItemIntoView(this.ArregloTesis.SelectedItem);
            MostrarDatos(documentoActual);
        }

        public void SiguienteListaClic()
        {
            List<AcuerdoSimplificadoTO> presentadorDatos = (List<AcuerdoSimplificadoTO>)this.ArregloTesis.ItemsSource;
            AcuerdosTO acuerdoMostrar = null;

            if (posicion >= presentadorDatos.Count - 1)
            {
                posicion = presentadorDatos.Count - 1;
                acuerdoMostrar = new AcuerdosTO();
                acuerdoMostrar.Id = presentadorDatos[posicion].Id;
            }
            else
            {
                posicion++;
                acuerdoMostrar = new AcuerdosTO();
                acuerdoMostrar.Id = presentadorDatos[posicion].Id;
            }
#if STAND_ALONE
            FachadaBusquedaTradicional fachada = new FachadaBusquedaTradicional();
#else
            FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
#endif
            AcuerdosTO documentoActual = fachada.getAcuerdoElectoralPorId(Int32.Parse(acuerdoMostrar.Id));
            fachada.Close();
            this.ArregloTesis.SelectedIndex = posicion;
            this.ArregloTesis.BringItemIntoView(this.ArregloTesis.SelectedItem);
            MostrarDatos(documentoActual);
        }

        public void UltimoListaClic()
        {
            List<AcuerdoSimplificadoTO> presentadorDatos = (List<AcuerdoSimplificadoTO>)this.ArregloTesis.ItemsSource;
            AcuerdosTO acuerdoMostrar = null;
            posicion = presentadorDatos.Count - 1;
            this.ArregloTesis.SelectedIndex = posicion;
            this.ArregloTesis.BringItemIntoView(this.ArregloTesis.SelectedItem);
            acuerdoMostrar = new AcuerdosTO();
            acuerdoMostrar.Id = presentadorDatos[posicion].Id;
#if STAND_ALONE
            FachadaBusquedaTradicional fachada = new FachadaBusquedaTradicional();
#else
            FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
#endif
            AcuerdosTO documentoActual = fachada.getAcuerdoElectoralPorId(Int32.Parse(acuerdoMostrar.Id));
            fachada.Close();
            MostrarDatos(documentoActual);
        }

        public void RegNumTecla(System.Windows.Input.KeyEventArgs e)
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

        public void ImprimirClic()
        {
            //if (Ventana.BtnTablas.Visibility == Visibility.Visible)
            //{
            //    MessageBox.Show(Mensajes.MENSAJE_TABLAS_EXISTENTES, Mensajes.TITULO_TABLAS_EXISTENTES,
            //        MessageBoxButton.OK, MessageBoxImage.Information);
            //}
            if (Ventana.imprimePapel.Visibility == Visibility.Hidden)
            {
                DocumentoAcuerdoElectoral documento;
                MessageBoxResult resultadoMsgBox = MessageBoxResult.Cancel;
                documento = new DocumentoAcuerdoElectoral(DocumentoActual, numeroParte + 1);
                //FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
                //object documentoXps = fachada.getDocumentoTesis(this.DocumentoActual.Ius);
                DocumentoParaCopiar = documento.Copia;
                Ventana.impresion.Document = documento.Documento; //(IDocumentPaginatorSource)documentoXps;
                Ventana.impresion.Visibility = Visibility.Visible;
                Ventana.impresion.Background = Brushes.White;
                Ventana.tabControl1.Visibility = Visibility.Hidden;
                Ventana.imprimePapel.Visibility = Visibility.Visible;
                Ventana.textoAbuscar.Visibility = Visibility.Hidden;
                Ventana.Buscar.Visibility = Visibility.Hidden;
                Ventana.Expresion.Visibility = Visibility.Hidden;
                MuestraVistaPrel();
            }
            else
            {
                Ventana.impresion.Visibility = Visibility.Hidden;
                //impresion.Background = Brushes.Transparent;
                Ventana.tabControl1.Visibility = Visibility.Visible;
                Ventana.imprimePapel.Visibility = Visibility.Hidden;
                Ventana.textoAbuscar.Visibility = Visibility.Visible;
                Ventana.Buscar.Visibility = Visibility.Visible;
                Ventana.Expresion.Visibility = Visibility.Visible;
                EscondeVistaPrel();
            }
        }

        public void DocumentoCompletoClic()
        {
            FlowDocument documentoRubro = new FlowDocument();
            //if (documentosTesis.Length == 0)
            //{
            documentoRubro.Blocks.Add(new Paragraph(new Run(DocumentoActual.Rubro)));
            //}
            int contador = 1;
            String TextoCompleto = "";
            foreach (AcuerdosPartesTO item in partes)
            {
                TextoCompleto += item.TxtParte;
                contador++;
            }
            Paragraph textoParrafo = ObtenLigas(TextoCompleto, DocumentoActual.Id, -1);
            contador++;
            textoParrafo.FontWeight = FontWeights.Normal;
            textoParrafo.TextAlignment = TextAlignment.Justify;
            documentoRubro.Blocks.Add(textoParrafo);
            if ((Busqueda != null) && (Busqueda.Palabra != null))
            {
                foreach (BusquedaPalabraTO item in Busqueda.Palabra)
                {
                    List<String> listapalabras = FlowDocumentHighlight.obtenPalabras(item);
                    documentoRubro = FlowDocumentHighlight.
                        imprimeToken(documentoRubro,
                                     listapalabras, Brushes.Red);
                    List<String> frases = FlowDocumentHighlight.obtenFrases(item);
                    documentoRubro = FlowDocumentHighlight.imprimeToken(documentoRubro, frases, Brushes.DarkGreen);
                }
            }
            numeroParte = -2;
            Ventana.contenidoTexto.Document = documentoRubro;
            Ventana.parteSiguiente.Visibility = Visibility.Hidden;
            Ventana.parteAnterior.Visibility = Visibility.Hidden;
            Ventana.parteFinal.Visibility = Visibility.Hidden;
            Ventana.parteInicio.Visibility = Visibility.Hidden;
            Ventana.NumeroPartes.Visibility = Visibility.Hidden;
            Ventana.docCompletoImage.Visibility = Visibility.Hidden;
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
            {
#else
            if (numeroParte < (partes.Length-1))
            {
#endif
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
            List<AcuerdoSimplificadoTO> arregloAcuerdosActual = (List<AcuerdoSimplificadoTO>)ArregloTesis.ItemsSource;
            if (registro > 0 && registro <= arregloAcuerdosActual.Count)
            {
                registro--;
                AcuerdoSimplificadoTO acuerdoActual = arregloAcuerdosActual[registro];
#if STAND_ALONE
                FachadaBusquedaTradicional fachada = new FachadaBusquedaTradicional();
#else
            FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
#endif
                AcuerdosTO acuerdoCompleta = fachada.getAcuerdoPorId(Int32.Parse(acuerdoActual.Id));
                fachada.Close();
                posicion = registro;
                MostrarDatos(acuerdoCompleta);
                Ventana.regNum.Text = "";
            }
            else
            {
                Ventana.regNum.Text = "";
                MessageBox.Show(Mensajes.MENSAJE_CONSECUTIVO_NO_VALIDO, Mensajes.TITULO_CONSECUTIVO_NO_VALIDO,
                    MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        public void TextoBuscarCambio()
        {
            EncontradaFrase = false;
            Ventana.contenidoTexto.Selection.Select(Ventana.contenidoTexto.Document.ContentStart,
                Ventana.contenidoTexto.Document.ContentStart);
        }

        public void TextoBuscarTecla(System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                BuscarClic();
            }
        }

        public void BuscarClic()
        {
            String validar = Validadores.BusquedaPalabraDocumento(Ventana.textoAbuscar);
            if (!validar.Equals(Constants.CADENA_VACIA))
            {
                return;
            }
            if (!Ventana.TabTexto.IsSelected)
            {
                Ventana.TabTexto.IsSelected = true;
            }
            String busquedaTexto = Ventana.textoAbuscar.Text;
            TextPointer inicio = Ventana.contenidoTexto.Selection.Start;
            TextPointer final = Ventana.contenidoTexto.Selection.End;
            TextPointer lugarActual = null;
            if (inicio.CompareTo(final) == 0)
            {
                lugarActual = Ventana.contenidoTexto.Document.ContentStart;
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
                    dialogoImpresion.PrintDocument(pgn, "Impresión de acuerdos/otros");

                    EscondeVistaPrel();
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
            EscondeVistaPrel();
        }

        public void SalirClic()
        {
            Ventana.NavigationService.Navigate(Ventana.Back);
        }

        public void ContenidoTextoTecla(System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key.Equals(Key.Enter) && (!Ventana.textoAbuscar.Text.Equals("")))
            {
                BuscarClic();
            }
        }

        public void ContenidoTextoCopia(object sender, DataObjectCopyingEventArgs e)
        {
            e.Handled = true;
            RichTextBox rtb = sender as RichTextBox;
            Ventana.TbxCopiar.Text = Ventana.contenidoTexto.Selection.Text;
            e.CancelCommand();
            Ventana.TbxCopiar.SelectAll();
            Ventana.TbxCopiar.Copy();
        }

        #endregion

        #region IAcuerdosController Members


        public void TabControlChanged()
        {
            if (Ventana.TabDocumento.IsSelected)
            {
                Ventana.FontMayor.Visibility = Visibility.Hidden;
                Ventana.FontMenor.Visibility = Visibility.Hidden;
                Ventana.parteInicio.Visibility = Visibility.Hidden;
                Ventana.parteAnterior.Visibility = Visibility.Hidden;
                Ventana.parteSiguiente.Visibility = Visibility.Hidden;
                Ventana.parteFinal.Visibility = Visibility.Hidden;
                Ventana.LblPalabraBuscar.Visibility = Visibility.Hidden;
                Ventana.Buscar.Visibility = Visibility.Hidden;
                Ventana.textoAbuscar.Visibility = Visibility.Hidden;
            }
            else
            {
                Ventana.FontMayor.Visibility = Visibility.Visible;
                Ventana.FontMenor.Visibility = Visibility.Visible;
                Ventana.LblPalabraBuscar.Visibility = Visibility.Visible;
                Ventana.Buscar.Visibility = Visibility.Visible;
                Ventana.textoAbuscar.Visibility = Visibility.Visible;
#if STAND_ALONE
                if (partes.Count > 1)
#else
                if(partes.Length>1)
#endif
                {
                    Ventana.FontMenor.Visibility = Visibility.Visible;
                    Ventana.parteInicio.Visibility = Visibility.Visible;
                    Ventana.parteAnterior.Visibility = Visibility.Visible;
                    Ventana.parteSiguiente.Visibility = Visibility.Visible;
                    Ventana.parteFinal.Visibility = Visibility.Visible;
                }
            }

        }

        #endregion
    }
}
