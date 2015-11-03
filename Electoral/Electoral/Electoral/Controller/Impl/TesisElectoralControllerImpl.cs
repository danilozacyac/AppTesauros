using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mx.gob.scjn.electoral_common.TO;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows;
using System.Windows.Documents;
using mx.gob.scjn.ius_common.gui.impresion;
using mx.gob.scjn.ius_common.gui.utils;
using mx.gob.scjn.ius_common.TO;
using mx.gob.scjn.electoral_common.gui.utils;
using Xceed.Wpf.DataGrid;
using mx.gob.scjn.ius_common.fachade;
using mx.gob.scjn.ius_common.utils;
using System.Windows.Media;
using System.Windows.Input;
using System.Windows.Interop;
using mx.gob.scjn.ius_common.gui.gui.impresion;
using mx.gob.scjn.electoral.gui.impresion;

namespace mx.gob.scjn.electoral.Controller.Impl
{
    public class TesisElectoralControllerImpl : ITesisElectoralController
    {
        #region properties
        public TesisElectoral Ventana { get; set; }
        /// <summary>
        /// El control del datagrid propio de la clase, para ser tratado como
        /// proveedor de datos en el caso de el uso de las flechas de navegación.
        /// </summary>
        private Xceed.Wpf.DataGrid.DataGridControl ArregloTesis { get; set; }
        /// <summary>
        /// El identificador de la genealogía que tiene la tesis actual.
        /// </summary>
        private String genealogiaId { get; set; }
        /// <summary>
        /// La posición que tiene el documento dentro del proveedor de datos.
        /// </summary>
        private int posicion = 0;
        private FlowDocument documentoPrecedentes;
        private FlowDocument documentoRubro;
        public Boolean ImprimirCompleto { get; set; }
        public Object Parametro { get; set; }
        /// <summary>
        /// La relación con los documentos de ejecutoria que tiene la tesis.
        /// </summary>
#if STAND_ALONE
        private List<RelDocumentoTesisTO> documentosEjecutoria { get; set; }
#else
        private RelDocumentoTesisTO[] documentosEjecutoria { get; set; }
#endif
        /// <summary>
        /// La relación que tiene la tesis con los votos que la produjeron.
        /// </summary>
#if STAND_ALONE
        private List<RelDocumentoTesisTO> documentosVotos { get; set; }
#else
        private RelDocumentoTesisTO[] documentosVotos { get; set; }
#endif
        /// <summary>
        /// El documento de la tesis actual.
        /// </summary>
        private TesisTO DocumentoActual;
        /// <summary>
        /// Las observaciones realizadas a los  documentos de la tesis.
        /// </summary>
#if STAND_ALONE
        List<OtrosTextosTO> observaciones;
#else
        OtrosTextosTO[] observaciones;
#endif
        /// <summary>
        /// La lista del historial y sus ligas
        /// </summary>
        private List<IUSHyperlink> ArregloLigas;
        public List<List<RelacionFraseArticulosTO>> listaLeyes { get; set; }
        /// <summary>
        /// La busqueda solicitada desde el panel
        /// </summary>
        public BusquedaTO busquedaSolicitada { get; set; }
        /// <summary>
        ///     Registros que han sido marcados
        /// </summary>
        /// <remarks>
        ///     La llave del arreglo es el Identificador del documento
        /// </remarks>
        public static HashSet<int> marcados;
        private FlowDocument documentoCopia { get; set; }

        /// <summary>
        ///     Define si la frase fue localizada o no
        /// </summary>
        /// <value>
        ///     <para>
        ///         True si la frase se encontro, False en caso contrario
        ///     </para>
        /// </value>
        protected bool EncontradaFrase { get; set; }
        /// <summary>
        ///     Define si la contradicción existe, si lo es hay que pintar su botón
        /// </summary>
        protected bool contradiccionExiste;
        /// <summary>
        ///     Define si la ventana de rangos está abierta
        /// </summary>
        protected bool verVentanaRangos;
        /// <summary>
        ///     Define si la ventana emergente está abierta.
        /// </summary>
        protected bool verVentanaEmergente { get; set; }
        protected bool verVentanaListaEjecutorias { get; set; }
        protected bool verVentanaListaVotos { get; set; }
        protected bool verVentanaListadoLeyes { get; set; }
        protected bool verVentanitaLeyes { get; set; }
        protected bool verVentanaAnexos { get; set; }
        protected bool verVentanaHistorial { get; set; }
        protected bool verMaterias { get; set; }
        protected bool verFontMayor { get; set; }
        protected bool verFontMenor { get; set; }
        protected bool mostrarVP { get; set; }
        #endregion
        #region constructores
        public TesisElectoralControllerImpl()
        {
        }
        public TesisElectoralControllerImpl(TesisElectoral Origen)
        {
            this.Ventana = Origen;
            mostrarVP = true;
            Ventana.ventanitaLeyes.Padre = Ventana;
            Parametro = null;
            ArregloLigas = null;
            Ventana.contenidoTexto.SetValue(RichTextBox.FontSizeProperty, CalculosPropiedadesGlobales.FontSize);
            verFontMenor = true;
            verFontMayor = true;
        }
        public TesisElectoralControllerImpl(TesisElectoral Origen, TesisTO Documento)
        {
            Ventana = Origen;
            DocumentoActual = Documento;
            mostrarVP = true;
            Ventana.contenidoTexto.SetValue(RichTextBox.FontSizeProperty, CalculosPropiedadesGlobales.FontSize);
            verFontMenor = true;
            verFontMayor = true;
            ArregloLigas = new List<IUSHyperlink>();
            Ventana.ventanitaLeyes.Padre = Ventana;
#if STAND_ALONE
            FachadaBusquedaTradicional fachada = new FachadaBusquedaTradicional();
#else
            FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
#endif
            TesisTO documentoActual = Documento;
            MostrarDatos(documentoActual);
            fachada.Close();
            //Si entran a este constructor es por que vienen 
            //para ver solamente un registro
            Ventana.ejecutoria.Visibility=Visibility.Hidden;
            //Ventana.voto.Visibility=Visibility.Hidden;
            Ventana.ejecutoria.Visibility = Visibility.Hidden;
            Ventana.inicioLista.Visibility = Visibility.Hidden;
            Ventana.anteriorLista.Visibility = Visibility.Hidden;
            Ventana.siguienteLista.Visibility = Visibility.Hidden;
            Ventana.ultimoLista.Visibility = Visibility.Hidden;
            Ventana.regNum.Visibility = Visibility.Hidden;
            Ventana.RegNum.Visibility = Visibility.Hidden;
            Ventana.IrALabel.Visibility = Visibility.Hidden;
            Ventana.IrBoton.Visibility = Visibility.Hidden;
            Ventana.MarcarTodo.Visibility = Visibility.Hidden;
            Ventana.Marcar.Visibility = Visibility.Hidden;
            Ventana.Desmarcar.Visibility = Visibility.Hidden;
            Parametro = Ventana;
        }
        
        public TesisElectoralControllerImpl(TesisElectoral Origen, DataGridControl Grid, BusquedaTO Busqueda)
        {
            Ventana = Origen;
            ArregloTesis = Grid;
            mostrarVP = true;
            CalculosPropiedadesGlobales.FontSize = Ventana.FontSize;
            ArregloLigas = new List<IUSHyperlink>();
            marcados = new HashSet<int>();
            Ventana.ventanitaLeyes.Padre = Ventana;
            TesisSimplificadaElectoralTO tesisMostrar = (TesisSimplificadaElectoralTO)Grid.SelectedItem;
            posicion = Grid.SelectedIndex;
#if STAND_ALONE
            FachadaBusquedaTradicional fachada = new FachadaBusquedaTradicional();
#else
            FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
#endif
            TesisTO documentoActual = fachada.getTesisElectoralPorRegistroLiga(tesisMostrar.Ius);
            documentoActual.Ius = tesisMostrar.Ius;
            fachada.Close();
            //Si hay palabras que pintar
            if ((Busqueda != null))
            {
                this.busquedaSolicitada = Busqueda;
            }
            //fin de las palabras a pintar
            MostrarDatos(documentoActual);
            if (Grid.Items.Count == 1)
            {
                Ventana.inicioLista.Visibility = Visibility.Hidden;
                Ventana.anteriorLista.Visibility = Visibility.Hidden;
                Ventana.siguienteLista.Visibility = Visibility.Hidden;
                Ventana.ultimoLista.Visibility = Visibility.Hidden;
                Ventana.IrALabel.Visibility = Visibility.Hidden;
                Ventana.IrBoton.Visibility = Visibility.Hidden;
                Ventana.regNum.Visibility = Visibility.Hidden;
                Ventana.RegNum.Visibility = Visibility.Hidden;
                Ventana.Marcar.Visibility = Visibility.Hidden;
                Ventana.MarcarTodo.Visibility = Visibility.Hidden;
                Ventana.Desmarcar.Visibility = Visibility.Hidden;
            }
            Parametro = Ventana;
            Ventana.contenidoTexto.SetValue(RichTextBox.FontSizeProperty, CalculosPropiedadesGlobales.FontSize);
            verFontMenor = true;
            verFontMayor = true;      
        }
        #endregion
        #region ITesisElectoralController Members

        public void ActualizaRangoMarcado(int inicial, int FinRango)
        {
            for (int contador = inicial; contador <= FinRango; contador++)
            {
                TesisSimplificadaElectoralTO agregado = (TesisSimplificadaElectoralTO)ArregloTesis.Items.GetItemAt(contador - 1);
                marcados.Add(Int32.Parse(agregado.Ius));
            }
            if (marcados.Contains(Int32.Parse(DocumentoActual.Ius)))
            {
                marcados.Add(Int32.Parse(DocumentoActual.Ius));
                Image imagenNueva = new Image();
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri("/General;component/images/PALOMA1.png", UriKind.Relative);
                bitmap.EndInit();
                Ventana.Marcar.Source = bitmap;
            }
        }

        public void PortaPapelesClic()
        {
            mostrarVP = false;
            //Imprimir_MouseButtonDown(sender, e);
            DocumentoElectoralTesis documento = null;
            DocumentoElectoralTesis.ImprimirCompleto = true;
            //impresion.Document = documento.Documento;
            //FlowDocument documentoOriginal = this.contenidoTexto.Document;
            MessageBoxResult resultado = MessageBoxResult.No;
            if (marcados.Count > 0)
            {
                resultado = MessageBox.Show(Mensajes.MENSAJE_MARCADOS_ACCION, Mensajes.TITULO_MARCADOS_ACCION,
                    MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
            }
            if (resultado.Equals(MessageBoxResult.No))
            {
                documento = new DocumentoElectoralTesis(DocumentoActual);
            }
            else if (resultado.Equals(MessageBoxResult.Yes))
            {
                documento = new DocumentoElectoralTesis(marcados, true);
            }
            else
            {
                mostrarVP = true;
                return;
            }
            FlowDocument documentoImprimir = documento.DocumentoCopia;
            Ventana.RtbCopyPaste.Document = documentoImprimir;
            Ventana.RtbCopyPaste.SelectAll();
            Ventana.RtbCopyPaste.Copy();
            //MostrarDatos(DocumentoActual);
            //this.contenidoTexto.Document = documentoOriginal;
            EscondeVistaPrel();
            mostrarVP = true;
            MessageBox.Show(Mensajes.MENSAJE_ENVIADO_PORTAPAPELES,
                Mensajes.TITULO_ENVIADO_PORTAPAPELES, MessageBoxButton.OK,
                MessageBoxImage.Information);
        }

        private void EscondeVistaPrel()
        {
            if (verVentanaRangos)
                Ventana.ventanaRangos.Visibility = Visibility.Visible;
            if (verVentanaEmergente)
                Ventana.ventanaEmergente.Visibility = Visibility.Visible;
            if (verVentanaListaEjecutorias)
                Ventana.ventanaListaEjecutorias.Visibility = Visibility.Visible;
            if (verVentanaListaVotos)
                Ventana.ventanaListaVotos.Visibility = Visibility.Visible;
            if (verVentanaListadoLeyes)
                Ventana.ventanaListadoLeyes.Visibility = Visibility.Visible;
            if (verVentanitaLeyes)
                Ventana.ventanitaLeyes.Visibility = Visibility.Visible;

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
                Ventana.Marcar.Visibility = Visibility.Visible;
                Ventana.MarcarTodo.Visibility = Visibility.Visible;
                Ventana.Desmarcar.Visibility = Visibility.Visible;
            }
            if (!DocumentoActual.IdGenealogia.Equals("0"))
            {
                Ventana.genealogia.Visibility = Visibility.Visible;
            }
#if STAND_ALONE
            if (documentosEjecutoria.Count > 0)
#else
            if (documentosEjecutoria.Length > 0)
#endif
            {
                Ventana.ejecutoria.Visibility = Visibility.Visible;
            }
//#if STAND_ALONE
//            if (documentosVotos.Count>0)
//#else
//            if (documentosVotos.Length > 0)
//#endif
//            {
//                Ventana.voto.Visibility = Visibility.Visible;
//            }
            foreach (OtrosTextosTO item in observaciones)
            {
                if (item.TipoNota.Equals("2"))
                {
                    Ventana.observacionesBot.Visibility = Visibility.Visible;
                }
                else if (item.TipoNota.Equals("3"))
                {
                    Ventana.BtnConcordancia.Visibility = Visibility.Visible;
                }
            }
            Ventana.PortaPapeles.Visibility = Visibility.Visible;
            Ventana.Imprimir.Visibility = Visibility.Visible;
            Ventana.imprimePapel.Visibility = Visibility.Hidden;
            Ventana.BtnTache.Visibility = Visibility.Hidden;
            if (verFontMayor) Ventana.FontMayor.Visibility = Visibility.Visible;
            if (verFontMenor) Ventana.FontMenor.Visibility = Visibility.Visible;
            Ventana.Salir.Visibility = Visibility.Visible;
            if (verMaterias)
            {
                Ventana.AnuncioMaterias.Visibility = Visibility.Visible;
                Ventana.Materias.Visibility = Visibility.Visible;
            }
            Ventana.Guardar.Visibility = !BrowserInteropHelper.IsBrowserHosted ? Visibility.Visible : Visibility.Hidden;
            Ventana.TesisLabel.Visibility = Visibility.Visible;
            Ventana.TesisTesisLabel.Visibility = Visibility.Visible;
            Ventana.fuenteLabel.Visibility = Visibility.Visible;
            Ventana.EpocaLabel.Visibility = Visibility.Visible;
            Ventana.IUSLabel.Visibility = Visibility.Visible;
            Ventana.SalaLabel.Visibility = Visibility.Visible;
            //Ventana.FechaLabel.Visibility = Visibility.Visible;
            Ventana.PaginaLabel.Visibility = Visibility.Visible;
            Ventana.jurisLabel.Visibility = Visibility.Visible;
            if (contradiccionExiste)
            {
                Ventana.contradiccion.Visibility = Visibility.Visible;
            }
            Ventana.LblPalabraBuscar.Visibility = Visibility.Visible;
            Ventana.textoAbuscar.Visibility = Visibility.Visible;
            Ventana.Buscar.Visibility = Visibility.Visible;
            Ventana.Expresion.Visibility = Visibility.Visible;
            Ventana.tabControl1.Visibility = Visibility.Visible;
            Ventana.impresion.Visibility = Visibility.Hidden;
        }

        public void GuardarClic()
        {
            mostrarVP = false;
            if (!BrowserInteropHelper.IsBrowserHosted)
            {
                Microsoft.Win32.SaveFileDialog guardaEn = new Microsoft.Win32.SaveFileDialog();
                guardaEn.DefaultExt = ".rtf";
                guardaEn.Title = "Guardar una tesis";
                guardaEn.Filter = "Archivos de Texto Enriquecido|*.rtf";
                guardaEn.AddExtension = true;
                EscondeVistaPrel();
                if ((bool)guardaEn.ShowDialog())
                {
                    ImprimirCompleto = true;
                    ImprmirClic();
                    FlowDocument documentoImprimir = documentoCopia as FlowDocument;
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
                ImprmirClic();
                FlowDocument documentoImprimir = Ventana.impresion.Document as FlowDocument;
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
            }
            EscondeVistaPrel();
            mostrarVP = true;
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
            // this.contenidoTexto.SelectAll();
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
            int faltantes = 50 - marcados.Count;
            if (faltantes > ArregloTesis.Items.Count)
            {
                // El número de documentos en el arreglo cabe en los faltantes
                foreach (TesisSimplificadaElectoralTO item in ArregloTesis.Items)
                {
                    marcados.Add(Int32.Parse(item.Ius));
                }
                MessageBox.Show(Mensajes.MENSAJE_TODOS_PORTAPAPELES + ArregloTesis.Items.Count +
                    Mensajes.MENSAJE_TODOS_PROTAPAPELES2, Mensajes.TITULO_TODOS_PORTAPAPELES,
                    MessageBoxButton.OK, MessageBoxImage.Information);
                MarcarClic();
            }
            else
            {
                if (marcados.Count < 50)
                {
                    Ventana.ventanaRangos.Visibility = Visibility.Visible;
                    Ventana.ventanaRangos.InicioRango = 1;
                    Ventana.ventanaRangos.FinRango = 50;
                    Ventana.ventanaRangos.diferenciaRangos = faltantes;
                    Ventana.ventanaRangos.StrMensaje = Mensajes.MENSAJE_RANGO_MARCAR + faltantes;
                    Ventana.ventanaRangos.contenedor = Ventana;
                    Ventana.ventanaRangos.registroFinal = this.ArregloTesis.Items.Count;
                }
                else
                {
                    MessageBox.Show(Mensajes.MENSAJE_RANGO_YA_NO_HAY, Mensajes.TITULO_RANGO,
                        MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
            }
        }

        public void DesmarcarClic()
        {
            MessageBoxResult resultado = MessageBoxResult.Yes;
            if (marcados.Count > 0)
            {
                resultado = MessageBox.Show(Mensajes.MENSAJE_DESMARCAR_TODO,
                    Mensajes.TITULO_DESMARCAR_TODO, MessageBoxButton.YesNo, MessageBoxImage.Question);
            }
            if (resultado == MessageBoxResult.Yes)
            {
                //marcados.Remove(Int32.Parse(DocumentoActual.Ius));
                marcados.Clear();
                Image imagenNueva = new Image();
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri("/General;component/images/MARCAR1.png", UriKind.Relative);
                bitmap.EndInit();
                Ventana.Marcar.Source = bitmap;
                MessageBox.Show(Mensajes.MENSAJE_SIN_MARCAS, Mensajes.TITULO_SIN_MARCAS,
                    MessageBoxButton.OK, MessageBoxImage.Information);
                Ventana.Marcar.ToolTip = Mensajes.TOOLTIP_SIN_MARCAR;
            }
        }

        public void MarcarEnter()
        {
            if (!marcados.Contains(Int32.Parse(this.DocumentoActual.Ius)))
            {
                Image imagenNueva = new Image();
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri("/General;component/images/MARCAR2.png", UriKind.Relative);
                bitmap.EndInit();
                Ventana.Marcar.Source = bitmap;
                Ventana.Marcar.ToolTip = Mensajes.TOOLTIP_SIN_MARCAR;
            }
            else
            {
                Image imagenNueva = new Image();
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri("/General;component/images/PALOMA1.png", UriKind.Relative);
                bitmap.EndInit();
                Ventana.Marcar.Source = bitmap;
                Ventana.Marcar.ToolTip = Mensajes.TOOLTIP_MARCADO;
            }
        }

        public void MarcarSalir()
        {
            if (!marcados.Contains(Int32.Parse(this.DocumentoActual.Ius)))
            {
                Image imagenNueva = new Image();
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri("/General;component/images/MARCAR1.png", UriKind.Relative);
                bitmap.EndInit();
                Ventana.Marcar.Source = bitmap;
                Ventana.Marcar.ToolTip = Mensajes.TOOLTIP_SIN_MARCAR;
            }
            else
            {
                Image imagenNueva = new Image();
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri("/General;component/images/PALOMA1.png", UriKind.Relative);
                bitmap.EndInit();
                Ventana.Marcar.Source = bitmap;
                Ventana.Marcar.ToolTip = Mensajes.TOOLTIP_MARCADO;
            }
        }

        public void MarcarClic()
        {
            if (!marcados.Contains(Int32.Parse(DocumentoActual.Ius)))
            {
                if (marcados.Count <= 50)
                {
                    marcados.Add(Int32.Parse(DocumentoActual.Ius));
                    Image imagenNueva = new Image();
                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri("/General;component/images/PALOMA1.png", UriKind.Relative);
                    bitmap.EndInit();
                    Ventana.Marcar.Source = bitmap;
                    Ventana.Marcar.ToolTip = Mensajes.TOOLTIP_MARCADO;
                }
                else
                {
                    MessageBox.Show("El documento no se puede marcar ya que hay 50 documentos guardados", "Limite de marcados alcanzado", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                marcados.Remove(Int32.Parse(DocumentoActual.Ius));
                Image imagenNueva = new Image();
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri("/General;component/images/MARCAR1.png", UriKind.Relative);
                bitmap.EndInit();
                Ventana.Marcar.Source = bitmap;
                Ventana.Marcar.ToolTip = Mensajes.TOOLTIP_SIN_MARCAR;
            }
        }

        public void ConcordanciaClic()
        {
            FlowDocument documentoObservaciones = new FlowDocument();
            foreach (OtrosTextosTO item in observaciones)
            {
                if (Int32.Parse(item.TipoNota) == 3)
                {
                    Paragraph parrafoObservacion = new Paragraph(new Run(item.Textos));
                    parrafoObservacion.TextAlignment = TextAlignment.Justify;
                    documentoObservaciones.Blocks.Add(parrafoObservacion);
                }
            }
            Ventana.ventanaEmergente.contenido.Document = documentoObservaciones;
            Ventana.ventanaEmergente.titulo.Text = "Concordancia";
            Ventana.ventanaEmergente.Visibility = Visibility.Visible;
        }

        public void ImprmirClic()
        {
            DocumentoElectoralTesis documento;
            DocumentoElectoralTesis.ImprimirCompleto = ImprimirCompleto;
            if (Ventana.imprimePapel.Visibility == Visibility.Hidden)
            {
                //impresion.ToolTip = Constants.VISTA_PRELIMINAR_FUERA;
                Ventana.impresion.Document = null;
                MessageBoxResult resultadoMsgBox = MessageBoxResult.Cancel;
                if ((marcados != null) && (marcados.Count > 0))
                {
                    resultadoMsgBox = MessageBox.Show(Mensajes.MENSAJE_MARCADOS_ACCION,
                        Mensajes.TITULO_MARCADOS_ACCION, MessageBoxButton.YesNoCancel,
                        MessageBoxImage.Question);
                }
                else
                {
                    resultadoMsgBox = MessageBoxResult.No;
                }
                if (resultadoMsgBox.Equals(MessageBoxResult.No))
                {
                    documento = new DocumentoElectoralTesis(DocumentoActual);
                }
                else if (resultadoMsgBox.Equals(MessageBoxResult.Yes))
                {
                    documento = new DocumentoElectoralTesis(marcados, true);
                }
                else
                {
                    return;
                }
                Ventana.impresion.Document = documento.Documento; //(IDocumentPaginatorSource)documentoXps;
                documentoCopia = documento.DocumentoCopia;
                if (mostrarVP)
                {
                    MuestraVistaPrel();
                }
            }
            else
            {
                //  impresion.Document = documento.Documento; //(IDocumentPaginatorSource)documentoXps;
                //impresion.ToolTip = Constants.VISTA_PRELIMINAR;
                Ventana.impresion.Visibility = Visibility.Hidden;
                Ventana.tabControl1.Visibility = Visibility.Visible;
                Ventana.imprimePapel.Visibility = Visibility.Hidden;
                Ventana.textoAbuscar.Visibility = Visibility.Visible;
                Ventana.Buscar.Visibility = Visibility.Visible;
                Ventana.Expresion.Visibility = Visibility.Visible;
                EscondeVistaPrel();
            }
        }
        
        /// <summary>
        ///     Muestra los elementos que tienen que ver con la vista preliminar de
        ///     impresión y esconde los que no
        /// </summary>
        private void MuestraVistaPrel()
        {
            verVentanaRangos=(Ventana.ventanaRangos.Visibility.Equals(Visibility.Visible));
            verVentanaEmergente= (Ventana.ventanaEmergente.Visibility==Visibility.Visible);
            verVentanaListaEjecutorias= (Ventana.ventanaListaEjecutorias.Visibility==Visibility.Visible);
            verVentanaListaVotos=(Ventana.ventanaListaVotos.Visibility==Visibility.Visible);
            verVentanaListadoLeyes=Ventana.ventanaListadoLeyes.Visibility==Visibility.Visible;
            verVentanitaLeyes=Ventana.ventanitaLeyes.Visibility==Visibility.Visible;
            Ventana.ventanaRangos.Visibility=Visibility.Hidden;
            Ventana.ventanaEmergente.Visibility = Visibility.Hidden;;
            Ventana.ventanaListaEjecutorias.Visibility = Visibility.Hidden;
            Ventana.ventanaListaVotos.Visibility = Visibility.Hidden;
            Ventana.ventanaListadoLeyes.Visibility = Visibility.Hidden;
            Ventana.ventanitaLeyes.Visibility = Visibility.Hidden;
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
            Ventana.genealogia.Visibility = Visibility.Hidden;
            Ventana.ejecutoria.Visibility = Visibility.Hidden;
            //Ventana.voto.Visibility = Visibility.Hidden;
            Ventana.observacionesBot.Visibility = Visibility.Hidden;
            Ventana.BtnConcordancia.Visibility = Visibility.Hidden;
            Ventana.PortaPapeles.Visibility = Visibility.Hidden;
            Ventana.Imprimir.Visibility = Visibility.Hidden;
            Ventana.imprimePapel.Visibility = Visibility.Visible;
            Ventana.BtnTache.Visibility = Visibility.Visible;
            Ventana.FontMayor.Visibility = Visibility.Hidden;
            Ventana.FontMenor.Visibility = Visibility.Hidden;
            Ventana.Marcar.Visibility = Visibility.Hidden;
            Ventana.MarcarTodo.Visibility = Visibility.Hidden;
            Ventana.Desmarcar.Visibility = Visibility.Hidden;
            Ventana.Salir.Visibility = Visibility.Hidden;
            Ventana.AnuncioMaterias.Visibility = Visibility.Hidden;
            Ventana.Materias.Visibility = Visibility.Hidden;
            Ventana.Guardar.Visibility = Visibility.Hidden;
            Ventana.TesisLabel.Visibility = Visibility.Hidden;
            Ventana.TesisTesisLabel.Visibility = Visibility.Hidden;
            Ventana.fuenteLabel.Visibility = Visibility.Hidden;
            Ventana.EpocaLabel.Visibility = Visibility.Hidden;
            Ventana.IUSLabel.Visibility = Visibility.Hidden;
            Ventana.SalaLabel.Visibility = Visibility.Hidden;
            //Ventana.FechaLabel.Visibility = Visibility.Hidden;
            Ventana.PaginaLabel.Visibility = Visibility.Hidden;
            Ventana.jurisLabel.Visibility = Visibility.Hidden;
            Ventana.contradiccion.Visibility = Visibility.Hidden;
            Ventana.LblPalabraBuscar.Visibility = Visibility.Hidden;
            Ventana.textoAbuscar.Visibility = Visibility.Hidden;
            Ventana.Buscar.Visibility = Visibility.Hidden;
            Ventana.Expresion.Visibility = Visibility.Hidden;
        }

        public void EjecutoriaClic()
        {
#if STAND_ALONE
            if (documentosEjecutoria.Count == 1)
#else
            if (documentosEjecutoria.Length == 1)
#endif
            {
                try
                {
                    Ejecutoria ejecutoriaAsociada = new Ejecutoria(Int32.Parse(documentosEjecutoria[0].Id));
                    ejecutoriaAsociada.Back = Ventana;
                    Ventana.NavigationService.Navigate(ejecutoriaAsociada);
                }
                catch (Exception exc)
                {
                    MessageBox.Show("Dieron Click muy rápido" + exc.Message);
                }
            }
            else
            {
                List<int> identificadores = new List<int>();
                Ventana.ventanaListaEjecutorias.NavigationService = Ventana.NavigationService;
                foreach (RelDocumentoTesisTO item in documentosEjecutoria)
                {
                    identificadores.Add(Int32.Parse(item.Id));
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
                EjecutoriasTO[] ejecutoriasRelacionadas = fachada.getEjecutoriasElectoralPorIds(parametros);
#endif
                List<EjecutoriasTO> listaFinal = new List<EjecutoriasTO>();
                foreach (EjecutoriasTO item in ejecutoriasRelacionadas)
                {
                    listaFinal.Add(item);
                }
                Ventana.ventanaListaEjecutorias.ListaRelacion = listaFinal;
                Ventana.ventanaListaEjecutorias.Visibility = Visibility.Visible;
            }
        }

        public void VotoClic()
        {
#if STAND_ALONE
            if (documentosVotos.Count == 1)
#else
            if (documentosVotos.Length == 1)
#endif
            {
                Votos votoAsociado = new Votos(Int32.Parse(documentosVotos[0].Id));
                votoAsociado.Back = Ventana;
                Ventana.NavigationService.Navigate(votoAsociado);
            }
            else
            {
                List<int> identificadores = new List<int>();
                Ventana.ventanaListaVotos.NavigationService = Ventana;
                foreach (RelDocumentoTesisTO item in documentosVotos)
                {
                    identificadores.Add(Int32.Parse(item.Id));
                }
                MostrarPorIusTO parametros = new MostrarPorIusTO();
                parametros.OrderBy = "ConsecIndx";
                parametros.OrderType = "asc";
#if STAND_ALONE
                FachadaBusquedaTradicional fachada = new FachadaBusquedaTradicional();
                parametros.Listado = identificadores;
                List<VotosTO> votosRelacionadas = fachada.getVotosElectoralPorIds(parametros);
#else
            FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
                parametros.Listado = identificadores.ToArray();
                VotosTO[] votosRelacionadas = fachada.getVotosElectoralPorIds(parametros);
#endif
                List<VotoSimplificadoTO> listaFinal = new List<VotoSimplificadoTO>();
                foreach (VotosTO item in votosRelacionadas)
                {
                    VotoSimplificadoTO itemVerdadero = new VotoSimplificadoTO(item);
                    listaFinal.Add(itemVerdadero);
                }
                Ventana.ventanaListaVotos.ListaRelacion = listaFinal;
                Ventana.ventanaListaVotos.NavigationService = Ventana;
                Ventana.ventanaListaVotos.Visibility = Visibility.Visible;
            }
        }

        public void ObservacionesClic()
        {
            FlowDocument documentoObservaciones = new FlowDocument();
            foreach (OtrosTextosTO item in observaciones)
            {
                if (Int32.Parse(item.TipoNota) == 2)
                {
                    Paragraph parrafoObservacion = new Paragraph(new Run(item.Textos));
                    parrafoObservacion.TextAlignment = TextAlignment.Justify;
                    documentoObservaciones.Blocks.Add(parrafoObservacion);
                }
            }
            Ventana.ventanaEmergente.contenido.Document = documentoObservaciones;
            Ventana.ventanaEmergente.titulo.Text = "Observaciones";
            Ventana.ventanaEmergente.Visibility = Visibility.Visible;
        }

        public void GenealogiaClic()
        {
#if STAND_ALONE
            FachadaBusquedaTradicional fachada = new FachadaBusquedaTradicional();
#else
            FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
#endif
            GenealogiaTO genealogiaObj = fachada.getGenalogiaElectoral(DocumentoActual.IdGenealogia);
            fachada.Close();
            FlowDocument documentoGenealogia = new FlowDocument();
            Paragraph parrafoGenealogia = new Paragraph(new Run(genealogiaObj.TxtGenealogia));
            parrafoGenealogia.TextAlignment = TextAlignment.Justify;
            documentoGenealogia.Blocks.Add(parrafoGenealogia);
            Ventana.ventanaEmergente.titulo.Text = "Genealogía";
            Ventana.ventanaEmergente.contenido.Document = documentoGenealogia;
            Ventana.ventanaEmergente.Visibility = Visibility.Visible;
        }

        public void InicioListaClic()
        {
#if STAND_ALONE
            FachadaBusquedaTradicional fachada = new FachadaBusquedaTradicional();
#else
            FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
#endif
            List<TesisSimplificadaElectoralTO> presentadorDatos = (List<TesisSimplificadaElectoralTO>)this.ArregloTesis.ItemsSource;
            TesisSimplificadaElectoralTO tesisMostrar = presentadorDatos[0];
            posicion = 0;
            this.ArregloTesis.SelectedIndex = posicion;
            TesisTO documentoActual = fachada.getTesisElectoralPorRegistro(tesisMostrar.Ius);
            fachada.Close();
            //documentoActual.Ius=tesisMostrar.Ius;
            MostrarDatos(documentoActual);
        }

        public void AnteriorListaClic()
        {
            List<TesisSimplificadaElectoralTO> presentadorDatos = (List<TesisSimplificadaElectoralTO>)this.ArregloTesis.ItemsSource;
            TesisSimplificadaElectoralTO tesisMostrar = null;
            if (posicion == 0)
            {
                tesisMostrar = presentadorDatos[0];
            }
            else
            {
                posicion--;
                this.ArregloTesis.SelectedIndex = posicion;
                tesisMostrar = presentadorDatos[posicion];
            }
            TesisSimplificadaElectoralTO documentoActual = new TesisSimplificadaElectoralTO();
            documentoActual.Ius = tesisMostrar.Ius;
            while (MostrarDatos(documentoActual) == Constants.ERROR_FACHADA) { }
        }

        public void SiguienteListaClic()
        {
            List<TesisSimplificadaElectoralTO> presentadorDatos = (List<TesisSimplificadaElectoralTO>)this.ArregloTesis.ItemsSource;
            TesisSimplificadaElectoralTO tesisMostrar = null;
            if (posicion >= presentadorDatos.Count - 1)
            {
                posicion = presentadorDatos.Count - 1;
                tesisMostrar = presentadorDatos[posicion];
            }
            else
            {
                posicion++;
                this.ArregloTesis.SelectedIndex = posicion;
                tesisMostrar = presentadorDatos[posicion];
            }
            TesisSimplificadaElectoralTO documentoActual = new TesisSimplificadaElectoralTO();
            documentoActual.Ius = tesisMostrar.Ius;
            MostrarDatos(documentoActual);
        }

        public void UltimoListaClic()
        {
            List<TesisSimplificadaElectoralTO> presentadorDatos = (List<TesisSimplificadaElectoralTO>)this.ArregloTesis.ItemsSource;
            TesisSimplificadaElectoralTO tesisMostrar = null;
            posicion = presentadorDatos.Count - 1;
            this.ArregloTesis.SelectedIndex = posicion;
            tesisMostrar = presentadorDatos[posicion];
            TesisSimplificadaElectoralTO documentoActual = new TesisSimplificadaElectoralTO();
            documentoActual.Ius = tesisMostrar.Ius;
            MostrarDatos(documentoActual);
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

            else
            {
                int registro = Int32.Parse(Ventana.regNum.Text);
                List<TesisSimplificadaElectoralTO> arregloTesisActual = (List<TesisSimplificadaElectoralTO>)ArregloTesis.ItemsSource;
                if (registro > 0 && registro <= arregloTesisActual.Count)
                {
                    registro--;
                    TesisSimplificadaElectoralTO tesisActual = arregloTesisActual[registro];
                    TesisSimplificadaElectoralTO tesisCompleta = new TesisSimplificadaElectoralTO();
                    tesisCompleta.Ius = tesisActual.Ius;
                    posicion = registro;
                    ArregloTesis.SelectedIndex = registro;
                    MostrarDatos(tesisCompleta);
                    Ventana.regNum.Text = "";
                }
                else
                {
                    Ventana.regNum.Text = "";
                    MessageBox.Show(Mensajes.MENSAJE_CONSECUTIVO_NO_VALIDO, Mensajes.TITULO_CONSECUTIVO_NO_VALIDO,
                        MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
            }
        }

        public void SalirClic()
        {
            Ventana.NavigationService.Navigate(Ventana.Back);
        }

        public void RegNumTecla(KeyEventArgs e)
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

        public void ContenidoTextoCopia(object sender, DataObjectCopyingEventArgs e)
        {
            e.Handled = true;
            RichTextBox rtb = sender as RichTextBox;
            Ventana.TbxCopiar.Text = Ventana.contenidoTexto.Selection.Text;
            e.CancelCommand();
            Ventana.TbxCopiar.SelectAll();
            Ventana.TbxCopiar.Copy();
        }

        public void ContenidoTextoSeleccion()
        {
            
        }

        public void ContenidoTextoTecla(KeyEventArgs e)
        {
            if (e.Key.Equals(Key.Enter) && (!Ventana.textoAbuscar.Text.Equals("")))
            {
                BuscarClic();
            }
        }

        public void TextoBuscarCambio()
        {
            Ventana.contenidoTexto.Selection.Select(Ventana.contenidoTexto.Document.ContentStart,
                Ventana.contenidoTexto.Document.ContentStart);
            EncontradaFrase = false;
        }

        public void TextoBuscarTecla(KeyEventArgs e)
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
            String busquedaTexto = Ventana.textoAbuscar.Text;
            String[] respuesta = BuscarPalabra(busquedaTexto);
            if (respuesta != null)
            {
                MessageBox.Show(respuesta[0], respuesta[1],
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private String[] BuscarPalabra(String busquedaTexto)
        {
            String[] respuesta = null;
            TextPointer inicio = Ventana.contenidoTexto.Selection.Start;
            TextPointer final = Ventana.contenidoTexto.Selection.End;
            TextPointer lugarActual = null;
            if (inicio.CompareTo(final) == 0)
            {
                lugarActual = inicio;// this.contenidoTexto.Document.ContentStart;
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
                    respuesta = new String[2];
                    respuesta[0] = Mensajes.MENSAJE_NO_HAY_MAS_COINCIDENCIAS;
                    respuesta[1] = Mensajes.TITULO_NO_HAY_MAS_COINCIDENCIAS;
                    return respuesta;
                }
                else
                {
                    respuesta = new String[2];
                    respuesta[0] = Mensajes.MENSAJE_NO_HAY_COINCIDENCIAS;
                    respuesta[1] = Mensajes.TITULO_NO_HAY_COINCIDENCIAS;
                    return respuesta;
                }
                //TextRange rango = new TextRange(contenidoTexto.Document.ContentStart, contenidoTexto.Document.ContentStart);
                //rango.Select(contenidoTexto.Document.ContentStart, contenidoTexto.Document.ContentStart);
            }
            else
            {
                EncontradaFrase = true;
                inicio = Ventana.contenidoTexto.Selection.Start;
                final = Ventana.contenidoTexto.Selection.End;
                Ventana.contenidoTexto.Selection.Select(inicio, final);
                Ventana.contenidoTexto.Focus();
                return respuesta;
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
                    dialogoImpresion.PrintDocument(pgn, "Impresión de tesis");
                }
                catch (Exception exc)
                {
                    MessageBox.Show(Mensajes.MENSAJE_IMPRESORA, Mensajes.TITULO_ARCHIVO_ABIERTO,
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
                EscondeVistaPrel();
            }
        }

        public void BtnTacheClic()
        {
            EscondeVistaPrel();
        }

        #endregion
        #region funciones Generales
        private int MostrarDatos(TesisSimplificadaElectoralTO documentoActual)
        {
#if STAND_ALONE
            FachadaBusquedaTradicional fachada = new FachadaBusquedaTradicional();
#else
            FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
#endif
            TesisTO tesisVerdadera = fachada.getTesisElectoralPorRegistroLiga(documentoActual.Ius);
            fachada.Close();
            if (tesisVerdadera.Ius != null)
            {
                MostrarDatos(tesisVerdadera);
                return Constants.NO_ERROR;
            }
            else
            {
                return Constants.ERROR_FACHADA;
            }
        }

        private void MostrarDatos(TesisTO documentoActual)
        {
            try
            {
                ArregloLigas.Clear();
#if STAND_ALONE
                List<OtrosTextosTO> contradiccion;
#else
                OtrosTextosTO[] contradiccion;
#endif
                Ventana.TesisTesisLabel.Content = documentoActual.Tesis;
                String fuente = new String(documentoActual.LocAbr.ToCharArray());
                int lugarPuntoyComa = fuente.IndexOf(",");
                                documentoActual.Fuente = lugarPuntoyComa==-1?fuente:fuente.Substring(0,lugarPuntoyComa);
                //String juris = fuente.Substring(0, lugarPuntoyComa);
                //fuente = fuente.Substring(lugarPuntoyComa + 1, fuente.Length - lugarPuntoyComa - 1);
                //lugarPuntoyComa = fuente.IndexOf(";");
                //String epoca = fuente.Substring(0, lugarPuntoyComa);
                //fuente = fuente.Substring(lugarPuntoyComa + 1, fuente.Length - lugarPuntoyComa - 1);
                //lugarPuntoyComa = fuente.IndexOf(";");
                //String origen = fuente.Substring(0, lugarPuntoyComa);
                //fuente = fuente.Substring(lugarPuntoyComa + 1, fuente.Length - lugarPuntoyComa - 1);
                //lugarPuntoyComa = fuente.IndexOf(";");
                ////origen = fuente.Substring(0, lugarPuntoyComa);
                ////fuente = fuente.Substring(lugarPuntoyComa + 1, fuente.Length - lugarPuntoyComa - 1);
                ////lugarPuntoyComa = fuente.IndexOf(";");
                ////String fecha = fuente.Substring(0, lugarPuntoyComa);
                ////fuente = fuente.Substring(lugarPuntoyComa + 1, fuente.Length - lugarPuntoyComa - 1);
                //Ventana.FechaLabel.Content = "";//fecha;
                //lugarPuntoyComa = fuente.IndexOf(";");
                if (documentoActual.Pagina.Trim().Equals("0"))
                {
                    Ventana.PaginaLabel.Content = "Sin número de página";
                }
                else
                {
                    Ventana.PaginaLabel.Content = "Pág. " + documentoActual.Pagina;// fuente.Substring(0, lugarPuntoyComa);
                }
                if (documentoActual.Parte.Equals("99"))
                {
                    verMaterias = false;
                    Ventana.AnuncioMaterias.Visibility = Visibility.Hidden;
                    Ventana.Materias.Visibility = Visibility.Hidden;
                }
                else
                {
                    verMaterias = true;
                }
                Ventana.jurisLabel.Content = documentoActual.Ta_tj.Equals("1") ? "Jurisprudencia" : "Tesis Relevante";//juris.Trim().Equals("[J]") ? "Jurisprudencia" : "Tesis Aislada";
                Ventana.IUSLabel.Content = documentoActual.Ius;
                Ventana.EpocaLabel.Content = documentoActual.Epoca;
                Ventana.TblFuente.Text = documentoActual.Fuente;
                Ventana.SalaLabel.Content = documentoActual.Sala;
                documentoPrecedentes = new FlowDocument();
                documentoRubro = new FlowDocument();
                FlowDocument documentoTexto = new FlowDocument();
                Paragraph precedentesParrafo = obtenLigas(documentoActual.Precedentes, documentoActual.Ius, Constants.SECCION_LIGAS_PRECEDENTES);
                precedentesParrafo.TextAlignment = TextAlignment.Justify;
                precedentesParrafo.FontStyle = FontStyles.Italic;
                documentoPrecedentes.Blocks.Add(precedentesParrafo);
                Paragraph rubroParrafo = obtenLigas(documentoActual.Rubro, documentoActual.Ius, Constants.SECCION_LIGAS_RUBRO);
                Paragraph textoparrafo = obtenLigas(documentoActual.Texto, documentoActual.Ius, Constants.SECCION_LIGAS_TEXTO);
                rubroParrafo.FontWeight = FontWeights.Bold;
                rubroParrafo.TextAlignment = TextAlignment.Justify;
                textoparrafo.TextAlignment = TextAlignment.Justify;
                documentoRubro.Blocks.Add(rubroParrafo);
                documentoTexto.Blocks.Add(textoparrafo);
                Ventana.contenidoTexto.Document = documentoTexto;
                Ventana.contenidoTexto.IsReadOnly = true;
                documentoTexto.IsEnabled = true;
                Ventana.contenidoTexto.IsEnabled = true;
                if (documentoActual.IdGenealogia.Equals("0"))
                {
                    Ventana.genealogia.Visibility = Visibility.Hidden;
                }
                else
                {
                    Ventana.genealogia.Visibility = Visibility.Visible;
                }
#if STAND_ALONE
                FachadaBusquedaTradicional fachada = new FachadaBusquedaTradicional();
#else
            FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
#endif
                contradiccion = fachada.getNotasContradiccionPorIusElectoral(documentoActual.Ius);
                Ventana.Materias.Content = fachada.getMateriasTesisElectoral(documentoActual.Ius);
                this.documentosEjecutoria = fachada.getEjecutoriaTesisElectoral(documentoActual.Ius);
                this.documentosVotos = fachada.getVotosTesisElectoral(documentoActual.Ius);
                contradiccionExiste = false;
#if STAND_ALONE
                if (contradiccion.Count > 0)
#else
                if (contradiccion.Length > 0)
#endif
                {
                    contradiccionExiste = true;
                    Ventana.contradiccion.Visibility = Visibility.Visible;
                    FlowDocument contenidoContradiccion = new FlowDocument();
                    Paragraph parrafo = new Paragraph();
                    IUSHyperlink liga = new IUSHyperlink();
                    liga.Inlines.Add(new Run("Superada por Contradicción"));
                    liga.PaginaTarget = Ventana;
                    liga.FontSize = 13;
                    if (contradiccion[0].TipoNota.Equals("2"))
                    {
                        liga.Tag = "Ejecutoria(" + contradiccion[0].Textos + ")";
                    }
                    else
                    {
                        liga.Tag = "Tesis(" + contradiccion[0].Textos + ")";
                    }
                    parrafo.Inlines.Add(liga);

                    Ventana.contradiccion.Document = contenidoContradiccion;
                    ArregloLigas.Add(liga);
                    contenidoContradiccion.Blocks.Add(parrafo);
                    Ventana.contradiccion.IsDocumentEnabled = true;
                    Ventana.contradiccion.IsEnabled = true;
                    Ventana.contradiccion.IsReadOnly = true;
                }
                else
                {
                    Ventana.contradiccion.Visibility = Visibility.Hidden;
                }
//#if STAND_ALONE
//                if (this.documentosVotos.Count == 0)
//#else
//                if (this.documentosVotos.Length == 0)
//#endif
//                {
//                    Ventana.voto.Visibility = Visibility.Hidden;
//                }
//                else
//                {
//                    Ventana.voto.Visibility = Visibility.Visible;
//                }
#if STAND_ALONE
                if (this.documentosEjecutoria.Count == 0)
#else
                if (this.documentosEjecutoria.Length == 0)
#endif
                {
                    Ventana.ejecutoria.Visibility = Visibility.Hidden;
                }
                else
                {
                    Ventana.ejecutoria.Visibility = Visibility.Visible;
                }
                observaciones = fachada.getOtrosTextosPorIusElectoral(documentoActual.Ius);
#if STAND_ALONE
                if (observaciones.Count == 0)
#else
                if (observaciones.Length == 0)
#endif
                {
                    Ventana.observacionesBot.Visibility = Visibility.Hidden;
                    Ventana.BtnConcordancia.Visibility = Visibility.Hidden;
                }
                else
                {
                    int observacionesCont = 0;
                    int concordanciasCont = 0;
                    foreach (OtrosTextosTO item in observaciones)
                    {
                        switch (item.TipoNota)
                        {
                            case "2":
                                observacionesCont++;
                                break;
                            case "3":
                                concordanciasCont++;
                                break;
                        }
                    }
                    if (observacionesCont > 0)
                    {
                        Ventana.observacionesBot.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        Ventana.observacionesBot.Visibility = Visibility.Hidden;
                    }
                    if (concordanciasCont > 0)
                    {
                        Ventana.BtnConcordancia.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        Ventana.BtnConcordancia.Visibility = Visibility.Hidden;
                    }
                }
                fachada.Close();
                if (ArregloTesis != null)
                {
                    List<TesisSimplificadaElectoralTO> lista = (List<TesisSimplificadaElectoralTO>)(ArregloTesis.ItemsSource);
                    int posicionReal = posicion + 1;
                    Ventana.RegNum.Content = "" + posicionReal + " / " + lista.Count;
                    if (this.busquedaSolicitada != null)
                    {
                        Ventana.Expresion.Content = "Tesis Electorales";//CalculosGlobales.Expresion(this.busquedaSolicitada);
                    }
                }
                else
                {
                    Ventana.RegNum.Visibility = Visibility.Hidden;
                    Ventana.anteriorLista.Visibility = Visibility.Hidden;
                    Ventana.inicioLista.Visibility = Visibility.Hidden;
                    Ventana.siguienteLista.Visibility = Visibility.Hidden;
                    Ventana.ultimoLista.Visibility = Visibility.Hidden;
                    Ventana.BloqueTextoIrA.Visibility = Visibility.Hidden;
                    Ventana.IrALabel.Visibility = Visibility.Hidden;
                    Ventana.IrBoton.Visibility = Visibility.Hidden;
                    Ventana.regNum.Visibility = Visibility.Hidden;
                    Ventana.Expresion.Visibility = Visibility.Hidden;
                }
                DocumentoActual = documentoActual;
                Ventana.impresion.Visibility = Visibility.Hidden;
                Ventana.textoAbuscar.Text = Constants.CADENA_VACIA;
                Ventana.imprimePapel.Visibility = Visibility.Hidden;
                Ventana.tabControl1.Visibility = Visibility.Visible;
                Ventana.ventanaEmergente.Visibility = Visibility.Hidden;
                Ventana.ventanitaLeyes.Visibility = Visibility.Hidden;
                Ventana.ventanaListaEjecutorias.Visibility = Visibility.Hidden;
                Ventana.ventanaListaVotos.Visibility = Visibility.Hidden;
                Ventana.textoAbuscar.Visibility = Visibility.Visible;
                Ventana.Buscar.Visibility = Visibility.Visible;
                Ventana.Expresion.Visibility = Visibility.Visible;
                if ((this.busquedaSolicitada != null) && (busquedaSolicitada.Palabra != null))
                {
                    PintaTesis(busquedaSolicitada.Palabra);
                }
                documentoRubro.Blocks.Add(documentoTexto.Blocks.FirstBlock);
                Ventana.contenidoTexto.Document = documentoRubro;
                Ventana.contenidoTexto.Document.Blocks.Add(new Paragraph(new Run("")));
                Ventana.contenidoTexto.Document.Blocks.Add(documentoPrecedentes.Blocks.FirstBlock);
                if (marcados.Contains(Int32.Parse(DocumentoActual.Ius)))
                {
                    Image imagenNueva = new Image();
                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri("/General;component/images/PALOMA1.png", UriKind.Relative);
                    bitmap.EndInit();
                    Ventana.Marcar.Source = bitmap;
                }
                else
                {
                    Image imagenNueva = new Image();
                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri("/General;component/images/MARCAR1.png", UriKind.Relative);
                    bitmap.EndInit();
                    Ventana.Marcar.Source = bitmap;
                }
                List<object> items = new List<object>();
                foreach (TabItem item in Ventana.tabControl1.Items)
                {
                    items.Add(item);
                }
                foreach (object item in items)
                {
                    if (item != Ventana.TabTexto)
                    {
                        Ventana.tabControl1.Items.Remove(item);
                    }
                }
                if ((this.busquedaSolicitada != null) && (busquedaSolicitada.Palabra != null))
                {
                    FlowDocumentHighlight.UbicarPrimera(Ventana.contenidoTexto);
                }
            }
            catch (Exception exc)
            {
                DocumentoActual = documentoActual;
                System.Console.WriteLine("Verifique que la conexión y los servicios de IUS estén disponibles: \n"
                    + " MostrarDatosException on Tesis " + exc.Message);
            }
        }
        #endregion
        #region ligas
        /// <summary>
        /// Define la cadena de texto para generar las ligas del documento hacia
        /// leyes u otros objetos similares.
        /// </summary>
        /// <param name="texto"> El texto ue tendrá la liga</param>
        /// <param name="ius"> El Ius del documento </param>
        /// <param name="seccion"> La seccion donde estará la liga</param>
        /// <returns>El párrafo con la liga adecuada.</returns>
        protected Paragraph obtenLigas(String texto, String ius, int seccion)
        {
            Paragraph resultado = new Paragraph(new Run(texto));
#if STAND_ALONE
            FachadaBusquedaTradicional fachada = new FachadaBusquedaTradicional();
            List<RelacionTO> listaRelaciones = fachada.getRelaciones(Int32.Parse(ius), seccion);
#else
            FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
            RelacionTO[] listaRelaciones = fachada.getRelaciones(Int32.Parse(ius), seccion);
#endif
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
                foreach (RelacionTO item in listaRelaciones)
                {
                    posicionFinal = Int32.Parse(item.Posicion);
                    if ((posicionFinal < posicionInicial))//viene mal el dato
                    {
                        posicionInicial = texto.IndexOf(item.MiDescriptor);
                    }
                    cadenaNueva = texto.Substring(posicionInicial, posicionFinal - posicionInicial);
                    resultadosParciales.Add(cadenaNueva);
                    resultado.Inlines.Add(cadenaNueva);
                    resultado.Inlines.Add(creaLiga(item, item.MiDescriptor));
                    posicionInicial = posicionFinal + item.MiDescriptor.Length;
                }
                if (posicionInicial < texto.Length)
                {
                    resultado.Inlines.Add(texto.Substring(posicionInicial));
                }
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
        private IUSHyperlink creaLiga(RelacionTO liga, string contenido)
        {
            RelacionFraseTesisTO tesis = null;
            RelacionFraseArticulosTO articulos = null;
#if STAND_ALONE
            FachadaBusquedaTradicional fachadaBusqueda = new FachadaBusquedaTradicional();
#else
            FachadaBusquedaTradicionalClient fachadaBusqueda = new FachadaBusquedaTradicionalClient();
#endif
            IUSHyperlink ligaNueva = new IUSHyperlink(Ventana.ventanitaLeyes);
            ligaNueva.Inlines.Add(new Run(contenido));
            ligaNueva.IsEnabled = true;
            switch (liga.Tipo)
            {
                case "100":
                    ligaNueva = new IUSHyperlink(Ventana.tabControl1);
                    ligaNueva.Inlines.Add(new Run(contenido));
                    ligaNueva.IsEnabled = true;
                    ligaNueva.Tag = "PDF(T" + liga.Ius + liga.IdRel + ".pdf)";
                    ligaNueva.PaginaTarget = Ventana;
                    break;
                case "10":
#if STAND_ALONE
                    tesis = fachadaBusqueda.getRelacionesFraseTesis(Int32.Parse(liga.Ius),
                        Int32.Parse(liga.IdRel))[0];
#else
                    tesis = fachadaBusqueda.getRelacionesFrasesTesis(Int32.Parse(liga.Ius),
                        Int32.Parse(liga.IdRel))[0];
#endif
                    liga.Tipo = tesis.Tipo;
                    break;

            }
            switch (liga.Tipo)
            {
                case "0":
#if STAND_ALONE
                    List<RelacionFraseArticulosTO> articulosRelacionados = fachadaBusqueda.getRelacionesFraseArticulos(Int32.Parse(liga.Ius),
                        Int32.Parse(liga.IdRel),0);
                    if (articulosRelacionados.Count < 2)
#else
                    RelacionFraseArticulosTO[] articulosRelacionados = fachadaBusqueda.getRelacionesFrasesArticulos(Int32.Parse(liga.Ius),
                        Int32.Parse(liga.IdRel),0);
                    if (articulosRelacionados.Length < 2)
#endif
                    {
                        articulos = articulosRelacionados[0];
                        ligaNueva.Tag = "ventanaEmergente(" + articulos.IdLey +
                            "," + articulos.IdArt + "," + articulos.IdRef + ");";
                    }
                    else
                    {
                        ligaNueva = new IUSHyperlink(Ventana.ventanaListadoLeyes);
                        ligaNueva.Inlines.Add(new Run(contenido));
                        ligaNueva.IsEnabled = true;
                        if (listaLeyes == null)
                        {
                            listaLeyes = new List<List<RelacionFraseArticulosTO>>();
                        }
                        ligaNueva.Tag = "ventanaEmergente(" + 0 +
                            "," + 0 + "," + listaLeyes.Count + ")";
                        ligaNueva.ListaLeyes = new List<RelacionFraseArticulosTO>();
                        foreach (RelacionFraseArticulosTO item in articulosRelacionados)
                        {
                            ligaNueva.ListaLeyes.Add(item);
                        }
                        listaLeyes.Add(ligaNueva.ListaLeyes);
                        Ventana.ventanaListadoLeyes.VentanaLeyes = Ventana.ventanitaLeyes;
                    }
                    ligaNueva.PaginaTarget = Ventana;
                    break;
                case "1":
#if STAND_ALONE
                    List<RelacionFraseTesisTO> relsCompletas = fachadaBusqueda.getRelacionesFraseTesis(Int32.Parse(liga.Ius),
                        Int32.Parse(liga.IdRel));
#else
                    RelacionFraseTesisTO[] relsCompletas = fachadaBusqueda.getRelacionesFrasesTesis(Int32.Parse(liga.Ius),
                        Int32.Parse(liga.IdRel));
#endif
                    tesis = relsCompletas[0];
                    ligaNueva.PaginaTarget = Ventana;
                    ligaNueva.Tag = "Tesis(" + tesis.IdLink + ")";
                    ligaNueva.PaginaTarget = Ventana;
                    break;
                case "2":
#if STAND_ALONE
                    tesis = fachadaBusqueda.getRelacionesFraseTesis(Int32.Parse(liga.Ius),
                        Int32.Parse(liga.IdRel))[0];
#else
                    tesis = fachadaBusqueda.getRelacionesFrasesTesis(Int32.Parse(liga.Ius),
                        Int32.Parse(liga.IdRel))[0];
#endif
                    ligaNueva.PaginaTarget = Ventana;
                    ligaNueva.Tag = "Ejecutoria(" + tesis.IdLink + ")";
                    ligaNueva.PaginaTarget = Ventana;
                    break;
                case "3":
#if STAND_ALONE
                    tesis = fachadaBusqueda.getRelacionesFraseTesis(Int32.Parse(liga.Ius),
                        Int32.Parse(liga.IdRel))[0];
#else
                    tesis = fachadaBusqueda.getRelacionesFrasesTesis(Int32.Parse(liga.Ius),
                        Int32.Parse(liga.IdRel))[0];
#endif
                    ligaNueva.PaginaTarget = Ventana;
                    ligaNueva.Tag = "Votos(" + tesis.IdLink + ")";
                    ligaNueva.PaginaTarget = Ventana;
                    break;
                case "100":
                    break;
                default:
#if STAND_ALONE
                    relsCompletas = fachadaBusqueda.getRelacionesFraseTesis(Int32.Parse(liga.Ius),
                        Int32.Parse(liga.IdRel));
#else
                    relsCompletas=fachadaBusqueda.getRelacionesFrasesTesis(Int32.Parse(liga.Ius),
                        Int32.Parse(liga.IdRel));
#endif
                    tesis = relsCompletas[0];
                    ligaNueva.PaginaTarget = Ventana;
                    ligaNueva.Tag = "Acuerdos(" + tesis.IdLink + ")";
                    ligaNueva.PaginaTarget = Ventana;
                    break;
            }
            fachadaBusqueda.Close();
            ArregloLigas.Add(ligaNueva);
            return ligaNueva;
        }

#if STAND_ALONE
        internal void PintaTesis(List<BusquedaPalabraTO> palabras)
#else
        internal void PintaTesis(BusquedaPalabraTO[] palabras)
#endif
        {
            List<String> listaPalabras = new List<string>();
            List<String> frases = new List<string>();
            foreach (BusquedaPalabraTO item in palabras)
            {
                BusquedaPalabraTO palabrejasTO = new BusquedaPalabraTO();
                if (item.Jurisprudencia == Constants.BUSQUEDA_PALABRA_ALMACENADA)
                {
#if STAND_ALONE
                    FachadaBusquedaTradicional fachada = new FachadaBusquedaTradicional();
                    List<BusquedaAlmacenadaTO> busquedas = fachada.getBusquedasAlmacenadas(SeguridadUsuariosTO.UsuarioActual.Usuario);
#else
            FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
                    BusquedaAlmacenadaTO[] busquedas = fachada.getBusquedasAlmacenadas(SeguridadUsuariosTO.UsuarioActual.Usuario);
#endif
                    fachada.Close();
                    String palabrejas = "";
                    foreach (BusquedaAlmacenadaTO itemAlamacenada in busquedas)
                    {
                        if (itemAlamacenada.id == item.Campos[0])
                        {
                            foreach (ExpresionBusqueda itemExpresion in itemAlamacenada.Expresiones)
                            {
                                palabrejas += itemExpresion.Expresion;
                                palabrejas += " ";
                                palabrejasTO.Campos = itemExpresion.Campos;
                            }
                        }
                    }
                    palabrejasTO.Expresion = palabrejas;
                }
                else
                {
                    palabrejasTO = item;
                }

                List<String> anadeLista = FlowDocumentHighlight.obtenPalabras(palabrejasTO);
                foreach (String anadir in anadeLista)
                {
                    if ((!anadir.ToLower().Equals("n"))
                        && (!anadir.ToLower().Equals("y"))
                        && (!anadir.ToLower().Equals("o"))
                        && (!anadir.Trim().Equals(Constants.SEPARADOR_FRASES.Trim())
                        && (!anadir.Trim().Equals(Constants.CADENA_VACIA))))
                    {
                        listaPalabras.Add(anadir);
                    }
                }
                anadeLista = FlowDocumentHighlight.obtenFrases(palabrejasTO);
                foreach (String anadir in anadeLista)
                {
                    frases.Add(anadir);
                }
                List<int> campos = new List<int>();
                if (palabrejasTO.Campos != null)
                {
                    campos = new List<int>(palabrejasTO.Campos);
                }
                if (campos.Contains(Constants.BUSQUEDA_PALABRA_CAMPO_RUBRO))
                {
                    documentoRubro = FlowDocumentHighlight.imprimeToken(documentoRubro, listaPalabras, Brushes.Red);
                    documentoRubro = FlowDocumentHighlight.imprimeToken(documentoRubro, frases, Brushes.DarkGreen);
                }
                if (campos.Contains(Constants.BUSQUEDA_PALABRA_CAMPO_PRECE))
                {
                    documentoPrecedentes = FlowDocumentHighlight.imprimeToken(documentoPrecedentes, listaPalabras, Brushes.Red);
                    documentoPrecedentes = FlowDocumentHighlight.imprimeToken(documentoPrecedentes, frases, Brushes.DarkGreen);
                }
                if (campos.Contains(Constants.BUSQUEDA_PALABRA_CAMPO_TEXTO))
                {
                    Ventana.contenidoTexto.Document = FlowDocumentHighlight.imprimeToken(Ventana.contenidoTexto.Document, listaPalabras, Brushes.Red);
                    Ventana.contenidoTexto.Document = FlowDocumentHighlight.imprimeToken(Ventana.contenidoTexto.Document, frases, Brushes.DarkGreen);
                }
            }
        }

        #endregion
    }
}
