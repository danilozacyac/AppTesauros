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
    public class VotoElectoralControllerImpl:IVotoElectoralController
    {
        private Xceed.Wpf.DataGrid.DataGridControl ArregloVotos { get; set; }
#if STAND_ALONE
        private List<RelDocumentoTesisTO> documentosTesis { get; set; }
        private List<RelVotoEjecutoriaTO> documentosEjecutorias { get; set; }
        private List<VotosPartesTO> partes { get; set; }
#else
        private RelDocumentoTesisTO[] documentosTesis { get; set; }
        private RelVotoEjecutoriaTO[] documentosEjecutorias { get; set; }
        private VotosPartesTO[] partes { get; set; }
#endif
        private int numeroParte { get; set; }
        private int posicion = 0;
        private BusquedaTO Busqueda { get; set; }
        protected bool ExistenTablas;
        private FlowDocument DocumentoCopia { get; set; }
        protected bool EncontradaFrase { get; set; }

        private Object ParametroConstruccion { get; set; }
        protected bool verVentanaListadoEjecutorias;
        protected bool verVentanaListadoTesis;
        protected bool verFontMayor;
        protected bool verFontMenor;
        private VotoSimplificadoTO DocumentoActual;
        public Votos Ventana { get; set; }

        public VotoElectoralControllerImpl(Votos ventana)
        {
            Ventana = ventana;
            ParametroConstruccion = this;
            Ventana.contenidoTexto.SetValue(RichTextBox.FontSizeProperty, CalculosPropiedadesGlobales.FontSize);
            verFontMenor = true;
            verFontMayor = true;
        }

        public VotoElectoralControllerImpl(Votos ventana, Int32 Id)
        {
            Ventana = ventana;
            ParametroConstruccion = this;
#if STAND_ALONE
            FachadaBusquedaTradicional fachada = new FachadaBusquedaTradicional();
#else
            FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
#endif
            VotoSimplificadoTO documentoActual = new VotoSimplificadoTO(fachada.getVotosElectoralPorId(Id));
            MostrarDatos(documentoActual);
            fachada.Close();
            Ventana.contenidoTexto.SetValue(RichTextBox.FontSizeProperty, CalculosPropiedadesGlobales.FontSize);
            verFontMenor = true;
            verFontMayor = true;
            Ventana.Expresion.Visibility = Visibility.Hidden;
            //Si entran a este constructor es por que vienen para ver solamente un registro
            Ventana.inicioLista.Visibility = Visibility.Hidden;
            Ventana.anteriorLista.Visibility = Visibility.Hidden;
            Ventana.siguienteLista.Visibility = Visibility.Hidden;
            Ventana.ultimoLista.Visibility = Visibility.Hidden;
            Ventana.regNum.Visibility = Visibility.Hidden;
            Ventana.RegNum.Visibility = Visibility.Hidden;
            Ventana.IrALabel.Visibility = Visibility.Hidden;
            Ventana.IrBoton.Visibility = Visibility.Hidden;
        }

        private void MostrarDatos(VotoSimplificadoTO documentoActual)
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
            Ventana.SalaLabel.Content = documentoActual.Sala;
            Ventana.IdLabel.Content = documentoActual.Id;
            Ventana.VolumenLabel.Content = documentoActual.Volumen;
            FlowDocument documentoPrecedentes = new FlowDocument();
            FlowDocument documentoRubro = new FlowDocument();
            Ventana.contenidoTexto.Document = documentoRubro;
#if STAND_ALONE
            FachadaBusquedaTradicional fachada = new FachadaBusquedaTradicional();
            documentosEjecutorias = fachada.getEjecutoriaElectoralPorVoto(documentoActual.Id);
#else
            FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
            documentosEjecutorias = fachada.getEjecutoriasPorVoto(documentoActual.Id);
#endif
            documentosTesis = fachada.getTesisElectoralPorVoto(documentoActual.Id);
            partes = fachada.getVotosPartesElectoralPorId(Int32.Parse(documentoActual.Id));
            Paragraph textoParrafo;
            textoParrafo = ObtenLigas(documentoActual.Rubro + "\n\n" + partes[0].TxtParte, documentoActual.Id, 1);

#if STAND_ALONE
            if (documentosTesis.Count == 0)
#else
            if (documentosTesis.Length == 0)
#endif
            {
                Ventana.tesis.Visibility = Visibility.Hidden;
            }
            else
            {
                Ventana.tesis.Visibility = Visibility.Visible;
            }
#if STAND_ALONE
            if (documentosEjecutorias.Count == 0)
#else
            if (documentosEjecutorias.Length == 0)
#endif
            {
                Ventana.ejecutoria.Visibility = Visibility.Hidden;
            }
            else
            {
                Ventana.ejecutoria.Visibility = Visibility.Visible;
            }
            textoParrafo.TextAlignment = TextAlignment.Justify;
            textoParrafo.FontWeight = FontWeights.Normal;
            documentoRubro.Blocks.Add(textoParrafo);
            numeroParte = 0;
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
#else
                if ((!encontrado) && (numeroParte < (partes.Length - 1)))
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
            if (ArregloVotos != null)
            {
                List<VotoSimplificadoTO> lista = (List<VotoSimplificadoTO>)(ArregloVotos.ItemsSource);
                int posicionReal = posicion + 1;
                Ventana.RegNum.Content = "" + posicionReal + " / " + lista.Count;
                DocumentoActual = documentoActual;
                Ventana.tabControl1.Visibility = Visibility.Visible;
                //Si es un solo registro en los arreglos esconder los de navegacion
            }
            else
            {
                posicion = 0;
                DocumentoActual = documentoActual;
                Ventana.tabControl1.Visibility = Visibility.Visible;
            }
            Ventana.textoAbuscar.Text = Constants.CADENA_VACIA;
            Ventana.impresion.Visibility = Visibility.Hidden;
            Ventana.ventanaListadoEjecutorias.Visibility = Visibility.Hidden;
            Ventana.ventanaListadoTesis.Visibility = Visibility.Hidden;
            Ventana.tabControl1.Visibility = Visibility.Visible;
            Ventana.imprimePapel.Visibility = Visibility.Hidden;
            Ventana.textoAbuscar.Visibility = Visibility.Visible;
            Ventana.Buscar.Visibility = Visibility.Visible;
            Ventana.Expresion.Visibility = Visibility.Visible;
            List<object> items = new List<object>();
            try
            {
                String archivo = documentoActual.Complemento.Substring(0, documentoActual.Complemento.Length - 4) + ".htm";
                Uri URL = new Uri(IUSConstants.IUS_RUTA_ANEXOS + "Electoral/Voto/" + archivo, UriKind.Absolute);
                Ventana.FrmDocumento.Source = URL;
            }
            catch (Exception e)
            {
            }
            if (Ventana.TbiDocumento.IsSelected)
            {
                TabControlChanged();
            }
        }

        public VotoElectoralControllerImpl(Votos ventana, DataGridControl records, BusquedaTO parametros)
        {
            Ventana = ventana;
            Busqueda = parametros;
            CalculosPropiedadesGlobales.FontSize = Ventana.FontSize;
            Ventana.contenidoTexto.SetValue(RichTextBox.FontSizeProperty, CalculosPropiedadesGlobales.FontSize);
            verFontMenor = true;
            verFontMayor = true;
            ParametroConstruccion = this;
            VotoSimplificadoTO votosMostrar = (VotoSimplificadoTO)records.SelectedItem;
#if STAND_ALONE
            FachadaBusquedaTradicional fachada = new FachadaBusquedaTradicional();
#else
            FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
#endif
            VotoSimplificadoTO documentoActual = new VotoSimplificadoTO(fachada.getVotosElectoralPorId(Int32.Parse(votosMostrar.Id)));
            this.ArregloVotos = records;
            posicion = records.SelectedIndex;
            if (parametros != null)
            {
                Ventana.Expresion.Content = "Votos electorales";
            }
            else
            {
                List<int> registros = new List<int>();
                Ventana.Expresion.Content = "Votos electorales";
            }
            MostrarDatos(documentoActual);
            fachada.Close();
            if (this.ArregloVotos.Items.Count < 2)
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
            List<TablaPartesTO> listaRelaciones = fachada.getTablaVoto(Int32.Parse(id));
#else
            FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
            TablaPartesTO[] listaRelaciones = fachada.getTablaVoto(Int32.Parse(id));
#endif
            //ventanaListadoAnexos.listado.ItemsSource = listaRelaciones.ToList();
#if STAND_ALONE
            if (listaRelaciones.Count == 0)
#else
            if (listaRelaciones.Length == 0)
#endif
            {
                //Ventana.BtnTablas.Visibility = Visibility.Hidden;
                ExistenTablas = false;
            }
            else
            {
                ExistenTablas = true;
                //Ventana.BtnTablas.Visibility = Visibility.Visible;
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

        private void actualizaTexto()
        {
            FlowDocument documentoRubro = new FlowDocument();
            Paragraph textoParrafo = null;
#if STAND_ALONE
            if ((documentosTesis.Count == 0) && (numeroParte == 0))
#else
            if ((documentosTesis.Length == 0) &&(numeroParte==0))
#endif
            {
                textoParrafo = new Paragraph(new Run(DocumentoActual.Rubro + "\n\n" + partes[numeroParte].TxtParte));
            }
            else
            {
                textoParrafo = new Paragraph(new Run(partes[numeroParte].TxtParte));
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



        #region IVotoElectoralController Members

        public void GuardarClic()
        {
            if (!BrowserInteropHelper.IsBrowserHosted)
            {
                ImprimirClic();
                Microsoft.Win32.SaveFileDialog guardaEn = new Microsoft.Win32.SaveFileDialog();
                guardaEn.DefaultExt = ".rtf";
                guardaEn.Title = "Guardar un voto";
                guardaEn.Filter = "Archivos de Texto Enriquecido|*.rtf";
                guardaEn.AddExtension = true;
                EscondeVistaPrel();
                if ((bool)guardaEn.ShowDialog())
                {
                    FlowDocument documentoImprimir = DocumentoCopia;
                    Ventana.impresion.Document = null;
                    EscondeVistaPrel();
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
                Ventana.contenidoTexto.Document = documentoImprimir;
                System.IO.IsolatedStorage.IsolatedStorageFileStream archivo = new System.IO.IsolatedStorage.
                    IsolatedStorageFileStream("texto.rtf", System.IO.FileMode.Create);
                Ventana.contenidoTexto.SelectAll();
                Ventana.contenidoTexto.Selection.Save(archivo, System.Windows.DataFormats.Text);
                archivo.Flush();
                archivo.Close();
                MostrarDatos(DocumentoActual);
                MessageBox.Show("El archivo fue guardado como: " + archivo.Name);
                EscondeVistaPrel();
            }
        }

        private void EscondeVistaPrel()
        {
            if (verVentanaListadoEjecutorias) Ventana.ventanaListadoEjecutorias.Visibility = Visibility.Visible;
            if (verVentanaListadoTesis) Ventana.ventanaListadoTesis.Visibility = Visibility.Visible;

            if ((ArregloVotos != null) && (ArregloVotos.Items.Count > 1))
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
            if (documentosEjecutorias.Count > 0)
#else
            if ( documentosEjecutorias.Length > 0)
#endif
            {
                Ventana.ejecutoria.Visibility = Visibility.Visible;
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
            Ventana.SalaLabel.Visibility = Visibility.Visible;
            Ventana.VolumenLabel.Visibility = Visibility.Visible;
            Ventana.PaginaLabel.Visibility = Visibility.Visible;
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

        public void PortaPapelesClic()
        {
            ImprimirClic();
            FlowDocument documentoImprimir = DocumentoCopia;
            Ventana.impresion.Document = null;
            Ventana.RtbCopyPaste.Document = documentoImprimir;
            Ventana.RtbCopyPaste.SelectAll();
            Ventana.RtbCopyPaste.Copy();
            EscondeVistaPrel();
            MessageBox.Show(Mensajes.MENSAJE_ENVIADO_PORTAPAPELES,
                Mensajes.TITULO_ENVIADO_PORTAPAPELES, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public void AumentaFontClic()
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
            Ventana.contenidoTexto.SetValue(RichTextBox.FontSizeProperty,
                CalculosPropiedadesGlobales.FontSize);
        }

        public void DisminuyeFontClic()
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

        public void ImprimirClic()
        {
            if (Ventana.imprimePapel.Visibility == Visibility.Hidden)
            {
                DocumentoVotoElectoral documento;
                documento = new DocumentoVotoElectoral(DocumentoActual, numeroParte + 1);
                //FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
                //object documentoXps = fachada.getDocumentoTesis(this.DocumentoActual.Ius);
                Ventana.impresion.Document = documento.Documento; //(IDocumentPaginatorSource)documentoXps;
                //fachada.Close();
                DocumentoCopia = documento.Copia;
                Ventana.impresion.Visibility = Visibility.Visible;
                Ventana.impresion.Background = Brushes.White;
                Ventana.tabControl1.Visibility = Visibility.Hidden;
                Ventana.imprimePapel.Visibility = Visibility.Visible;
                Ventana.textoAbuscar.Visibility = Visibility.Hidden;
                Ventana.Buscar.Visibility = Visibility.Hidden;
                Ventana.Expresion.Visibility = Visibility.Hidden;
                Ventana.Guardar.Visibility = Visibility.Hidden;
                Ventana.PortaPapeles.Visibility = Visibility.Hidden;
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

        private void MuestraVistaPrel()
        {
            verVentanaListadoEjecutorias = Ventana.ventanaListadoEjecutorias.Visibility == Visibility.Visible;
            verVentanaListadoTesis = Ventana.ventanaListadoTesis.Visibility == Visibility.Visible;
            Ventana.ventanaListadoEjecutorias.Visibility = Visibility.Hidden;
            Ventana.ventanaListadoTesis.Visibility = Visibility.Hidden;
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
            Ventana.ejecutoria.Visibility = Visibility.Hidden;
            Ventana.PortaPapeles.Visibility = Visibility.Hidden;
            Ventana.Imprimir.Visibility = Visibility.Hidden;
            Ventana.imprimePapel.Visibility = Visibility.Visible;
            Ventana.BtnTache.Visibility = Visibility.Visible;
            Ventana.FontMayor.Visibility = Visibility.Hidden;
            Ventana.FontMenor.Visibility = Visibility.Hidden;
            //Marcar.Visibility = Visibility.Hidden;
            //MarcarTodo.Visibility = Visibility.Hidden;
            //Desmarcar.Visibility = Visibility.Hidden;
            Ventana.Salir.Visibility = Visibility.Hidden;
            Ventana.Guardar.Visibility = Visibility.Hidden;
            Ventana.fuenteLabel.Visibility = Visibility.Hidden;
            Ventana.EpocaLabel.Visibility = Visibility.Hidden;
            Ventana.IdLabel.Visibility = Visibility.Hidden;
            Ventana.SalaLabel.Visibility = Visibility.Hidden;
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
                    TesisTO tesisVerdadera = fachada.getTesisElectoralPorRegistro(documentosTesis[0].Ius);
                    TesisElectoral tesisAsociada = new TesisElectoral(tesisVerdadera);
                    Ventana.NavigationService.Navigate(tesisAsociada);
                }
                catch (Exception exc)
                {

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
                List<TesisTO> tesisRelacionadas = fachada.getTesisElectoralPorIus(parametros);
#else
            FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
                parametros.Listado = identificadores.ToArray();
                TesisTO[] tesisRelacionadas = fachada.getTesisPorIus(parametros);
#endif
                List<TesisSimplificadaElectoralTO> listaFinal = new List<TesisSimplificadaElectoralTO>();
                foreach (TesisTO item in tesisRelacionadas)
                {
                    TesisSimplificadaElectoralTO itemVerdadero = new TesisSimplificadaElectoralTO();
                    itemVerdadero.ConsecIndx = item.ConsecIndx;
                    itemVerdadero.Ius = item.Ius;

                    listaFinal.Add(itemVerdadero);
                }
                Ventana.ventanaListadoTesis.TesisMostrar = listaFinal;
                Ventana.ventanaListadoTesis.NavigationService = Ventana;
                Ventana.ventanaListadoTesis.Visibility = Visibility.Visible;
            }
        }

        public void EjecutoriaClic()
        {
#if STAND_ALONE
            if (documentosEjecutorias.Count == 1)
#else
            if (documentosEjecutorias.Length == 1)
#endif
            {
                try
                {
                    Ejecutoria ejecutoriaAsociada = new Ejecutoria(Int32.Parse(documentosEjecutorias[0].Ejecutoria));
                    ejecutoriaAsociada.Back = Ventana;
                    Ventana.NavigationService.Navigate(ejecutoriaAsociada);
                }
                catch (Exception exc)
                {
                    MessageBox.Show(Mensajes.MENSAJE_PROBLEMAS_FACHADA + exc.Message, Mensajes.TITULO_PROBLEMAS_FACHADA,
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                List<int> identificadores = new List<int>();
                Ventana.ventanaListadoEjecutorias.NavigationService = Ventana.NavigationService;
                foreach (RelVotoEjecutoriaTO item in documentosEjecutorias)
                {
                    identificadores.Add(Int32.Parse(item.Ejecutoria));
                }
                MostrarPorIusTO parametros = new MostrarPorIusTO();
                parametros.OrderBy = "ConsecIndx";
                parametros.OrderType = "asc";
#if STAND_ALONE
                FachadaBusquedaTradicional fachada = new FachadaBusquedaTradicional();
                parametros.Listado = identificadores;
                List<EjecutoriasTO> ejecutoriasRelacionadas = fachada.getEjecutoriasElectoralPorIds(parametros);
#else
                FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
                parametros.Listado = identificadores.ToArray();
                EjecutoriasTO[] ejecutoriasRelacionadas = fachada.getEjecutoriasPorIds(parametros);
#endif
                List<EjecutoriasTO> listaFinal = new List<EjecutoriasTO>();
                foreach (EjecutoriasTO item in ejecutoriasRelacionadas)
                {
                    listaFinal.Add(item);
                }
                fachada.Close();
                Ventana.ventanaListadoEjecutorias.ListaRelacion = listaFinal;
                Ventana.ventanaListadoEjecutorias.NavigationService = Ventana.NavigationService;

                Ventana.ventanaListadoEjecutorias.Visibility = Visibility.Visible;
            }
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
            List<VotoSimplificadoTO> arregloVotosActual = (List<VotoSimplificadoTO>)ArregloVotos.ItemsSource;
            if (registro > 0 && registro <= arregloVotosActual.Count)
            {
                registro--;
                VotoSimplificadoTO votoActual = arregloVotosActual[registro];
#if STAND_ALONE
                FachadaBusquedaTradicional fachada = new FachadaBusquedaTradicional();
#else
            FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
#endif
                VotoSimplificadoTO votoCompleto = new VotoSimplificadoTO(fachada.getVotosPorId(Int32.Parse(votoActual.Id)));
                fachada.Close();
                posicion = registro;
                MostrarDatos(votoCompleto);
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
            if (Ventana.Back == null)
            {
                Ventana.NavigationService.GoBack();
            }
            else
            {
                Ventana.NavigationService.Navigate(Ventana.Back);
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

        public void TextoBuscarTecla(System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                BuscaClic();
            }
        }

        public void TextoBuscarCambio(TextChangedEventArgs e)
        {
            Ventana.contenidoTexto.Selection.Select(Ventana.contenidoTexto.Document.ContentStart,
                Ventana.contenidoTexto.Document.ContentStart);
            EncontradaFrase = false;
        }

        public void BuscaClic()
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

        public void DocumentoCompletoClic()
        {
            FlowDocument documentoRubro = new FlowDocument();
#if STAND_ALONE
            if (documentosTesis.Count == 0)
#else
            if (documentosTesis.Length == 0)
#endif
            {
                documentoRubro.Blocks.Add(new Paragraph(new Run(DocumentoActual.Rubro)));
            }
            int contador = 1;
            foreach (VotosPartesTO item in partes)
            {
                Paragraph textoParrafo = ObtenLigas(item.TxtParte, DocumentoActual.Id, contador);
                contador++;
                textoParrafo.FontWeight = FontWeights.Normal;
                textoParrafo.TextAlignment = TextAlignment.Justify;
                documentoRubro.Blocks.Add(textoParrafo);
            }
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
            numeroParte = -1;
            Ventana.contenidoTexto.Document = documentoRubro;
            Ventana.parteSiguiente.Visibility = Visibility.Hidden;
            Ventana.parteAnterior.Visibility = Visibility.Hidden;
            Ventana.parteFinal.Visibility = Visibility.Hidden;
            Ventana.parteInicio.Visibility = Visibility.Hidden;
            Ventana.NumeroPartes.Visibility = Visibility.Hidden;
            Ventana.docCompletoImage.Visibility = Visibility.Hidden;
        }

        public void InicioListaClic()
        {
            List<VotoSimplificadoTO> presentadorDatos = (List<VotoSimplificadoTO>)this.ArregloVotos.ItemsSource;
            VotoSimplificadoTO ejecutoriasMostrar = presentadorDatos[0];
            posicion = 0;
#if STAND_ALONE
            FachadaBusquedaTradicional fachada = new FachadaBusquedaTradicional();
#else
            FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
#endif
            VotoSimplificadoTO documentoActual = new VotoSimplificadoTO(fachada.getVotosElectoralPorId(Int32.Parse(ejecutoriasMostrar.Id)));
            fachada.Close();
            this.ArregloVotos.SelectedIndex = posicion;
            this.ArregloVotos.BringItemIntoView(this.ArregloVotos.SelectedItem);
            MostrarDatos(documentoActual);
        }

        public void AnteriorListaClic()
        {
            List<VotoSimplificadoTO> presentadorDatos = (List<VotoSimplificadoTO>)this.ArregloVotos.ItemsSource;
            VotoSimplificadoTO votoMostrar = null;
            if (posicion == 0)
            {
                votoMostrar = presentadorDatos[0];
            }
            else
            {
                posicion--;
                votoMostrar = presentadorDatos[posicion];
            }
#if STAND_ALONE
            FachadaBusquedaTradicional fachada = new FachadaBusquedaTradicional();
#else
            FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
#endif
            VotoSimplificadoTO documentoActual = new VotoSimplificadoTO(fachada.getVotosElectoralPorId(Int32.Parse(votoMostrar.Id)));
            fachada.Close();
            this.ArregloVotos.SelectedIndex = posicion;
            this.ArregloVotos.BringItemIntoView(this.ArregloVotos.SelectedItem);
            MostrarDatos(documentoActual);
        }

        public void SiguienteListaClic()
        {
            List<VotoSimplificadoTO> presentadorDatos = (List<VotoSimplificadoTO>)this.ArregloVotos.ItemsSource;
            VotoSimplificadoTO votosMostrar = null;
            if (posicion >= presentadorDatos.Count - 1)
            {
                posicion = presentadorDatos.Count - 1;
                votosMostrar = presentadorDatos[posicion];
            }
            else
            {
                posicion++;
                votosMostrar = presentadorDatos[posicion];
            }
#if STAND_ALONE
            FachadaBusquedaTradicional fachada = new FachadaBusquedaTradicional();
#else
            FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
#endif
            VotoSimplificadoTO documentoActual = new VotoSimplificadoTO(fachada.getVotosElectoralPorId(Int32.Parse(votosMostrar.Id)));
            fachada.Close();
            this.ArregloVotos.SelectedIndex = posicion;
            this.ArregloVotos.BringItemIntoView(this.ArregloVotos.SelectedItem);
            MostrarDatos(documentoActual);
        }

        public void UltimoListaClic()
        {
            List<VotoSimplificadoTO> presentadorDatos = (List<VotoSimplificadoTO>)this.ArregloVotos.ItemsSource;
            VotoSimplificadoTO votoMostrar = null;
            posicion = presentadorDatos.Count - 1;
            votoMostrar = presentadorDatos[posicion];
#if STAND_ALONE
            FachadaBusquedaTradicional fachada = new FachadaBusquedaTradicional();
#else
            FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
#endif
            VotoSimplificadoTO documentoActual = new VotoSimplificadoTO(fachada.getVotosElectoralPorId(Int32.Parse(votoMostrar.Id)));
            fachada.Close();
            this.ArregloVotos.SelectedIndex = posicion;
            this.ArregloVotos.BringItemIntoView(this.ArregloVotos.SelectedItem);
            MostrarDatos(documentoActual);
        }

        public void ContenidoTextoTecla(System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key.Equals(Key.Enter) && (!Ventana.textoAbuscar.Text.Equals("")))
            {
                BuscaClic();
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
        public void TabControlChanged()
        {
            if (Ventana.TbiDocumento.IsSelected)
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
                if (partes.Length > 1)
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
