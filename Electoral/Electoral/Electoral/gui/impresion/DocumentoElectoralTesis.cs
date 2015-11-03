using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Documents;
using mx.gob.scjn.ius_common.TO;
//using System.Windows.Xps.Packaging;
//using System.Windows.Xps.Serialization;
using System.IO;
using System.IO.Packaging;
using System.Windows;
using mx.gob.scjn.ius_common.fachade;
using System.Windows.Media;
using mx.gob.scjn.ius_common.gui.gui.impresion;
using mx.gob.scjn.ius_common.utils;

namespace mx.gob.scjn.electoral.gui.impresion
{
    public class DocumentoElectoralTesis
    {
        public FlowDocument Documento { get; set; }
        public FlowDocument DocumentoCopia { get; set; }
        public static  Boolean ImprimirCompleto { get; set; }
        public DocumentoElectoralTesis(TesisTO DocumentoActual)
        {
            List<TesisTO> listado = new List<TesisTO>();
            listado.Add(DocumentoActual);
            generaDocumento(listado);
        }

        public DocumentoElectoralTesis(HashSet<int> listadoEnteros, bool Electoral)
        {
#if STAND_ALONE
            FachadaBusquedaTradicional fachada = new FachadaBusquedaTradicional();
#else
            FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
#endif
            List<TesisTO> listado = new List<TesisTO>();
            foreach (int item in listadoEnteros)
            {
                TesisTO itemTesis = null;
                if (Electoral)
                {
                    itemTesis = fachada.getTesisElectoralPorRegistro("" + item);
                }
                else
                {
                    itemTesis = fachada.getTesisPorRegistro("" + item);
                }
                itemTesis.Ius = "" + item;
                listado.Add(itemTesis);
            }
            fachada.Close();
            generaDocumento(listado);
        }
        /// <summary>
        /// Genera un documento a partir de un listado de tesis.
        /// </summary>
        /// <param name="todosLosDocumentos">El listado de tesis para el documento.</param>
        private void generaDocumento(List<TesisTO> todosLosDocumentos)
        {
            IUSPaginatedDocument documentoVariable = new IUSPaginatedDocument();
            FlowDocument documentoCopia = new FlowDocument();
            documentoVariable.FontFamily = new FontFamily(Constants.FONT_USAR);
            documentoCopia.FontFamily = new FontFamily(Constants.FONT_USAR);
            documentoCopia.FontSize = 15;// 96/72 Es la equivalencia de px a pt
#if STAND_ALONE
            FachadaBusquedaTradicional fachada = new FachadaBusquedaTradicional();
#else
            FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
#endif
            foreach (TesisTO DocumentoActual in todosLosDocumentos)
            {
                Section seccionActual = new Section();
                seccionActual.FontFamily = new FontFamily(Constants.FONT_USAR);
                Section seccionActualCopia = new Section();
                seccionActualCopia.FontFamily = new FontFamily(Constants.FONT_USAR);
                seccionActual.BreakPageBefore = true;
                seccionActualCopia.BreakPageBefore = true;
                documentoVariable.Blocks.Add(seccionActual);
                documentoCopia.Blocks.Add(seccionActualCopia);
                if (ImprimirCompleto)
                {
                    String Materias = fachada.getMateriasTesisElectoral(DocumentoActual.Ius);
                    documentoVariable.ColumnWidth = 96 * 8.5;
                    documentoCopia.ColumnWidth = 96 * 8.5;
                    documentoVariable.PagePadding = new Thickness(96 * 0.5, 96 * 1, 96 * 0.5, 96 * 1.5);
                    documentoCopia.PagePadding = new Thickness(96 * 0.5, 96 * 1, 96 * 0.5, 96 * 1.5);
                    documentoVariable.PageHeight = 96 * 10;
                    documentoCopia.PageHeight = 96 * 10;
                    documentoVariable.Background = Brushes.White;
                    documentoCopia.Background = Brushes.White;
                    Table datosDocumento = new Table();
                    datosDocumento.FontFamily = new FontFamily(Constants.FONT_USAR);
                    //Table datosDocumentoCopia = new Table();
                    TableColumn columnaInicial = new TableColumn();
                    //TableColumn columnaInicialCopia = new TableColumn();
                    columnaInicial.Width = new GridLength(96 * 1);
                    //columnaInicialCopia.Width = new GridLength(96 * 1);
                    TableColumn columnaAncha = new TableColumn();
                    //TableColumn columnaAnchaCopia = new TableColumn();
                    columnaAncha.Width = new GridLength(96 * 3);
                    //columnaAnchaCopia.Width = new GridLength(96 * 3);
                    TableColumn columnaDatos = new TableColumn();
                    //TableColumn columnaDatoscopia = new TableColumn();
                    columnaDatos.Width = new GridLength(96 * 1);
                    TableColumn columnaAjuste = new TableColumn();
                    columnaAjuste.Width = new GridLength(96 * 1);
                    //columnaDatoscopia.Width = new GridLength(96 * 1.5);
                    TableColumn columnaFinal = new TableColumn();
                    //TableColumn columnaFinalCopia = new TableColumn();
                    columnaFinal.Width = new GridLength(96 * 1.33);
                    //columnaFinalCopia.Width = new GridLength(96 * 2);
                    datosDocumento.Columns.Add(columnaInicial);
                    //datosDocumentoCopia.Columns.Add(columnaInicialCopia);
                    datosDocumento.Columns.Add(columnaAncha);
                    //datosDocumentoCopia.Columns.Add(columnaAnchaCopia);
                    datosDocumento.Columns.Add(columnaDatos);
                    datosDocumento.Columns.Add(columnaAjuste);
                    //datosDocumentoCopia.Columns.Add(columnaDatoscopia);
                    datosDocumento.Columns.Add(columnaFinal);
                    //datosDocumentoCopia.Columns.Add(columnaFinalCopia);
                    Paragraph parrafoCelda = new Paragraph(new Run(DocumentoActual.Epoca));
                    parrafoCelda.FontFamily = new FontFamily(Constants.FONT_USAR);
                    Paragraph parrafoCeldaCopia = new Paragraph(new Run(DocumentoActual.Epoca));
                    parrafoCeldaCopia.FontFamily = new FontFamily(Constants.FONT_USAR);
                    TableCell celdaActual = new TableCell(parrafoCelda);
                    documentoCopia.Blocks.Add(parrafoCeldaCopia);
                    celdaActual.ColumnSpan = 2;
                    //celdaActualcopia.ColumnSpan = 2;
                    celdaActual.FontWeight = FontWeights.Bold;
                    //celdaActualcopia.FontWeight = FontWeights.Bold;
                    datosDocumento.RowGroups.Add(new TableRowGroup());
                    //datosDocumentoCopia.RowGroups.Add(new TableRowGroup());
                    for (int i = 0; i < 6; i++)
                    {
                        datosDocumento.RowGroups[0].Rows.Add(new TableRow());
                        //  datosDocumentoCopia.RowGroups[0].Rows.Add(new TableRow());
                    }
                    datosDocumento.RowGroups[0].Rows[0].Cells.Add(celdaActual);
                    //datosDocumentoCopia.RowGroups[0].Rows[0].Cells.Add(celdaActualcopia);
                    celdaActual = new TableCell(new Paragraph(new Run("")));
                    datosDocumento.RowGroups[0].Rows[0].Cells.Add(celdaActual);
                    celdaActual = new TableCell(new Paragraph(new Run("Registro:")));
                    //celdaActualcopia = new TableCell(new Paragraph(new Run("No. de Registro:")));
                    celdaActual.FontWeight = FontWeights.Bold;
                    celdaActual.TextAlignment = TextAlignment.Left;
                    //celdaActualcopia.FontWeight = FontWeights.Bold;
                    datosDocumento.RowGroups[0].Rows[0].Cells.Add(celdaActual);
                    //datosDocumentoCopia.RowGroups[0].Rows[0].Cells.Add(celdaActualcopia);
                    celdaActual = new TableCell(new Paragraph(new Run(DocumentoActual.Ius)));
                    documentoCopia.Blocks.Add(new Paragraph(new Run("Registro: " + DocumentoActual.Ius)));
                    celdaActual.FontWeight = FontWeights.Bold;
                    //celdaActualcopia.FontWeight = FontWeights.Bold;
                    datosDocumento.RowGroups[0].Rows[0].Cells.Add(celdaActual);
                    //datosDocumentoCopia.RowGroups[0].Rows[0].Cells.Add(celdaActualcopia);
                    celdaActual = new TableCell(new Paragraph(new Run("Instancia:")));
                    //celdaActualcopia= new TableCell(new Paragraph(new Run("Instancia")));
                    celdaActual.FontWeight = FontWeights.Bold;
                    //celdaActualcopia.FontWeight=FontWeights.Bold;
                    datosDocumento.RowGroups[0].Rows[1].Cells.Add(celdaActual);
                    //datosDocumentoCopia.RowGroups[0].Rows[1].Cells.Add(celdaActualcopia);
                    celdaActual = new TableCell(new Paragraph(new Run(DocumentoActual.Sala)));
                    documentoCopia.Blocks.Add(new Paragraph(new Run("Instancia: " + DocumentoActual.Sala)));
                    celdaActual.FontWeight = FontWeights.Bold;
                    celdaActual.ColumnSpan = 2;
                    //celdaActualcopia.FontWeight=FontWeights.Bold;
                    datosDocumento.RowGroups[0].Rows[1].Cells.Add(celdaActual);
                    //datosDocumentoCopia.RowGroups[0].Rows[1].Cells.Add(celdaActualcopia);
                    celdaActual = new TableCell(new Paragraph(new Run("")));
                    //celdaActualcopia=new TableCell(new Paragraph(new Run("")));
                    datosDocumento.RowGroups[0].Rows[1].Cells.Add(celdaActual);
                    //datosDocumentoCopia.RowGroups[0].Rows[1].Cells.Add(celdaActualcopia);
                    celdaActual = new TableCell(new Paragraph(new Run(DocumentoActual.Ta_tj == "1" ? "Jurisprudencia" : "Tesis Relevante")));
                    documentoCopia.Blocks.Add(new Paragraph(new Run(DocumentoActual.Ta_tj == "1" ? "Jurisprudencia" : "Tesis Relevante")));
                    celdaActual.FontWeight = FontWeights.Bold;
                    //celdaActualcopia.FontWeight = FontWeights.Bold;
                    datosDocumento.RowGroups[0].Rows[1].Cells.Add(celdaActual);
                    //datosDocumentoCopia.RowGroups[0].Rows[1].Cells.Add(celdaActualcopia);
                    celdaActual = new TableCell(new Paragraph(new Run("Fuente:")));
                    //celdaActualcopia = new TableCell(new Paragraph(new Run("Fuente:")));
                    celdaActual.FontWeight = FontWeights.Bold;
                    //celdaActualcopia.FontWeight = FontWeights.Bold;
                    datosDocumento.RowGroups[0].Rows[2].Cells.Add(celdaActual);
                    //datosDocumentoCopia.RowGroups[0].Rows[2].Cells.Add(celdaActualcopia);
                    celdaActual = new TableCell(new Paragraph(new Run(DocumentoActual.Fuente)));
                    documentoCopia.Blocks.Add(new Paragraph(new Run("Fuente: " + DocumentoActual.Fuente)));
                    celdaActual.FontWeight = FontWeights.Bold;
                    //celdaActualcopia.FontWeight = FontWeights.Bold;
                    celdaActual.ColumnSpan = 4;
                    //celdaActualcopia.ColumnSpan = 3;
                    datosDocumento.RowGroups[0].Rows[2].Cells.Add(celdaActual);
                    //datosDocumentoCopia.RowGroups[0].Rows[2].Cells.Add(celdaActualcopia);
                    //celdaActual = new TableCell(new Paragraph(new Run("")));
                    //datosDocumento.RowGroups[0].Rows[2].Cells.Add(celdaActual);
                    //celdaActual = new TableCell(new Paragraph(new Run("")));
                    //datosDocumento.RowGroups[0].Rows[2].Cells.Add(celdaActual);
                    String valorVolumen = "";
                    if (DocumentoActual.VolumenPrefijo != null) valorVolumen = DocumentoActual.VolumenPrefijo;
                    //if ((Int32.Parse(DocumentoActual.Parte) > 14)
                    //    && (Int32.Parse(DocumentoActual.Parte) < 29))
                    //{
                    //    celdaActual = new TableCell(new Paragraph(new Run("Volumen:")));
                    //    valorVolumen = "Volumen: ";
                    //}
                    //else
                    //{
                    //    celdaActual = new TableCell(new Paragraph(new Run("Tomo:")));
                    //    valorVolumen = "Tomo: ";
                    //}
                    celdaActual = new TableCell(new Paragraph(new Run(valorVolumen)));
                    celdaActual.FontWeight = FontWeights.Bold;
                    //celdaActualcopia.FontWeight = FontWeights.Bold;
                    datosDocumento.RowGroups[0].Rows[3].Cells.Add(celdaActual);
                    //datosDocumentoCopia.RowGroups[0].Rows[3].Cells.Add(celdaActualcopia);
                    celdaActual = new TableCell(new Paragraph(new Run(DocumentoActual.Volumen)));
                    documentoCopia.Blocks.Add(new Paragraph(new Run(valorVolumen + " " + DocumentoActual.Volumen)));
                    celdaActual.FontWeight = FontWeights.Bold;
                    celdaActual.ColumnSpan = 2;
                    //celdaActualcopia.FontWeight = FontWeights.Bold;
                    datosDocumento.RowGroups[0].Rows[3].Cells.Add(celdaActual);
                    //datosDocumentoCopia.RowGroups[0].Rows[3].Cells.Add(celdaActualcopia);
                    celdaActual = new TableCell(new Paragraph(new Run("Materia(s):")));
                    //celdaActualcopia = new TableCell(new Paragraph(new Run("Materia(s):")));
                    celdaActual.FontWeight = FontWeights.Bold;
                    //celdaActualcopia.FontWeight = FontWeights.Bold;
                    datosDocumento.RowGroups[0].Rows[3].Cells.Add(celdaActual);
                    //datosDocumentoCopia.RowGroups[0].Rows[3].Cells.Add(celdaActualcopia);
                    celdaActual = new TableCell(new Paragraph(new Run(Materias)));
                    documentoCopia.Blocks.Add(new Paragraph(new Run("Materia(s): " + Materias)));
                    celdaActual.FontWeight = FontWeights.Bold;
                    //celdaActualcopia.FontWeight = FontWeights.Bold;
                    datosDocumento.RowGroups[0].Rows[3].Cells.Add(celdaActual);
                    //datosDocumentoCopia.RowGroups[0].Rows[3].Cells.Add(celdaActualcopia);
                    if ((DocumentoActual.Tesis != null) && (!DocumentoActual.Tesis.Equals("")))
                    {
                        celdaActual = new TableCell(new Paragraph(new Run("Tesis:")));
                    }
                    else
                    {
                        celdaActual = new TableCell(new Paragraph(new Run("")));
                    }
                    //celdaActualcopia = new TableCell(new Paragraph(new Run("Tesis:")));
                    celdaActual.FontWeight = FontWeights.Bold;
                    //celdaActualcopia.FontWeight = FontWeights.Bold;
                    datosDocumento.RowGroups[0].Rows[4].Cells.Add(celdaActual);
                    //datosDocumentoCopia.RowGroups[0].Rows[4].Cells.Add(celdaActualcopia);
                    celdaActual = new TableCell(new Paragraph(new Run(DocumentoActual.Tesis)));
                    documentoCopia.Blocks.Add(new Paragraph(new Run("Tesis: " + DocumentoActual.Tesis)));
                    celdaActual.FontWeight = FontWeights.Bold;
                    //celdaActualcopia.FontWeight = FontWeights.Bold;
                    datosDocumento.RowGroups[0].Rows[4].Cells.Add(celdaActual);
                    //datosDocumentoCopia.RowGroups[0].Rows[4].Cells.Add(celdaActualcopia);
                    celdaActual = new TableCell(new Paragraph(new Run("")));
                    //celdaActualcopia = new TableCell(new Paragraph(new Run("")));
                    celdaActual.FontWeight = FontWeights.Bold;
                    //celdaActualcopia.FontWeight = FontWeights.Bold;
                    datosDocumento.RowGroups[0].Rows[4].Cells.Add(celdaActual);
                    //datosDocumentoCopia.RowGroups[0].Rows[4].Cells.Add(celdaActualcopia);
                    celdaActual = new TableCell(new Paragraph(new Run("")));
                    //celdaActualcopia = new TableCell(new Paragraph(new Run("")));
                    datosDocumento.RowGroups[0].Rows[4].Cells.Add(celdaActual);
                    //datosDocumentoCopia.RowGroups[0].Rows[4].Cells.Add(celdaActualcopia);
                    celdaActual = new TableCell(new Paragraph(new Run("Página: ")));
                    //celdaActualcopia = new TableCell(new Paragraph(new Run("Página: ")));
                    celdaActual.FontWeight = FontWeights.Bold;
                    //celdaActualcopia.FontWeight = FontWeights.Bold;
                    datosDocumento.RowGroups[0].Rows[5].Cells.Add(celdaActual);
                    //datosDocumentoCopia.RowGroups[0].Rows[5].Cells.Add(celdaActualcopia);
                    if (DocumentoActual.Pagina.Trim().Equals("0"))
                    {
                        celdaActual = new TableCell(new Paragraph(new Run("")));
                        documentoCopia.Blocks.Add(new Paragraph(new Run("")));
                    }
                    else
                    {
                        celdaActual = new TableCell(new Paragraph(new Run(DocumentoActual.Pagina)));
                        documentoCopia.Blocks.Add(new Paragraph(new Run("Página: " + DocumentoActual.Pagina)));
                    }
                    celdaActual.FontWeight = FontWeights.Bold;
                    //celdaActualcopia.FontWeight = FontWeights.Bold;
                    datosDocumento.RowGroups[0].Rows[5].Cells.Add(celdaActual);
                    //datosDocumentoCopia.RowGroups[0].Rows[5].Cells.Add(celdaActualcopia);
                    celdaActual = new TableCell(new Paragraph(new Run("")));
                    //celdaActualcopia = new TableCell(new Paragraph(new Run("")));
                    datosDocumento.RowGroups[0].Rows[5].Cells.Add(celdaActual);
                    //datosDocumentoCopia.RowGroups[0].Rows[5].Cells.Add(celdaActualcopia);
                    celdaActual = new TableCell(new Paragraph(new Run("")));
                    //celdaActualcopia = new TableCell(new Paragraph(new Run("")));
                    datosDocumento.RowGroups[0].Rows[5].Cells.Add(celdaActual);
                    //datosDocumentoCopia.RowGroups[0].Rows[5].Cells.Add(celdaActualcopia);
                    documentoVariable.Blocks.Add(datosDocumento);
                    //documentoCopia.Blocks.Add(datosDocumentoCopia);
                    datosDocumento = new Table();
                    //datosDocumentoCopia = new Table();
                    columnaInicial = new TableColumn();
                    //columnaInicialCopia = new TableColumn();
                    columnaInicial.Width = new GridLength(96 * 1);
                    //columnaInicialCopia.Width = new GridLength(96 * 3);
                    columnaAncha = new TableColumn();
                    //columnaAnchaCopia = new TableColumn();
                    columnaAncha.Width = new GridLength(96 * 7);

                    //columnaAnchaCopia.Width = new GridLength(96 * 6);
                    datosDocumento.Columns.Add(columnaInicial);
                    //datosDocumentoCopia.Columns.Add(columnaInicialCopia);
                    datosDocumento.Columns.Add(columnaAncha);
                    //datosDocumentoCopia.Columns.Add(columnaAnchaCopia);
                    datosDocumento.RowGroups.Add(new TableRowGroup());
                    //datosDocumentoCopia.RowGroups.Add(new TableRowGroup());
                    datosDocumento.RowGroups[0].Rows.Add(new TableRow());
                    //datosDocumentoCopia.RowGroups[0].Rows.Add(new TableRow());
                    Paragraph parrafoGenealogia = null;
                    if ((DocumentoActual.IdGenealogia != null) && (Int32.Parse(DocumentoActual.IdGenealogia) != 0))
                    {
                        celdaActual = new TableCell(new Paragraph(new Run("Genealogía:")));
                        parrafoGenealogia = new Paragraph();
                        documentoCopia.Blocks.Add(parrafoGenealogia);
                        parrafoGenealogia = new Paragraph(new Run("Genealogía:"));
                        celdaActual.FontWeight = FontWeights.Bold;
                        parrafoGenealogia.FontWeight = FontWeights.Bold;
                        datosDocumento.RowGroups[0].Rows[0].Cells.Add(celdaActual);
                        documentoCopia.Blocks.Add(parrafoGenealogia);
                        GenealogiaTO genealogiaObj = fachada.getGenalogiaElectoral(DocumentoActual.IdGenealogia);
                        parrafoGenealogia = new Paragraph(new Run(genealogiaObj.TxtGenealogia));
                        Paragraph parrafoGenealogiaCopia = new Paragraph(new Run(genealogiaObj.TxtGenealogia));
                        parrafoGenealogia.TextAlignment = TextAlignment.Justify;
                        parrafoGenealogiaCopia.TextAlignment = TextAlignment.Justify;
                        celdaActual = new TableCell(parrafoGenealogia);
                        celdaActual.TextAlignment = TextAlignment.Justify;
                        // celdaActual.ColumnSpan=3;
                        datosDocumento.RowGroups[0].Rows[0].Cells.Add(celdaActual);
                        documentoCopia.Blocks.Add(parrafoGenealogiaCopia);
                    }
                    documentoVariable.Blocks.Add(datosDocumento);
                    documentoCopia.Blocks.Add(new Paragraph(new Run("")));
                }
                Paragraph parrafo = new Paragraph(new Run(DocumentoActual.Rubro));
                Paragraph parrafoCopia = new Paragraph(new Run(DocumentoActual.Rubro));
                //parrafoCelda.FontFamily = new FontFamily(Constants.FONT_USAR);
                //parrafoCeldaCopia.FontFamily = new FontFamily(Constants.FONT_USAR);
                parrafo.FontWeight = FontWeights.Bold;
                parrafoCopia.FontWeight = FontWeights.Bold;
                documentoVariable.Blocks.Add(parrafo);
                documentoCopia.Blocks.Add(parrafoCopia);
                char[] enter = new char[1];
                enter[0] = '\n';
                String[] TextoCuerpo = DocumentoActual.Texto.Split(enter);
                foreach (String textoItem in TextoCuerpo)
                {
                    if (!textoItem.Trim().Equals(""))
                    {
                        parrafo = new Paragraph(new Run(textoItem));
                        parrafoCopia = new Paragraph(new Run(textoItem));
                        //parrafoCelda.FontFamily = new FontFamily(Constants.FONT_USAR);
                        //parrafoCeldaCopia.FontFamily = new FontFamily(Constants.FONT_USAR);
                        parrafo.FontWeight = FontWeights.Normal;
                        parrafoCopia.FontWeight = FontWeights.Normal;
                        documentoVariable.Blocks.Add(parrafo);
                        documentoCopia.Blocks.Add(parrafoCopia);
                    }
                }
                documentoCopia.Blocks.Add(new Paragraph(new Run("")));
                TextoCuerpo = DocumentoActual.Precedentes.Split(enter);
                foreach (String textoItem in TextoCuerpo)
                {
                    if (!textoItem.Trim().Equals(""))
                    {
                        parrafo = new Paragraph(new Run(textoItem));
                        parrafoCopia = new Paragraph(new Run(textoItem));
                        //parrafoCelda.FontFamily = new FontFamily(Constants.FONT_USAR);
                        //parrafoCeldaCopia.FontFamily = new FontFamily(Constants.FONT_USAR);
                        parrafo.FontWeight = FontWeights.Normal;
                        parrafoCopia.FontWeight = FontWeights.Normal;
                        documentoVariable.Blocks.Add(parrafo);
                        documentoCopia.Blocks.Add(parrafoCopia);
                        documentoCopia.Blocks.Add(new Paragraph(new Run("")));
                    }
                }
#if STAND_ALONE
                List<OtrosTextosTO> observaciones = fachada.getOtrosTextosPorIusElectoral(DocumentoActual.Ius);
#else
                OtrosTextosTO[] observaciones= fachada.getOtrosTextosPorIusElectoral(DocumentoActual.Ius);
#endif
                documentoCopia.Blocks.Add(new Paragraph(new Run("")));
                foreach (OtrosTextosTO itemObs in observaciones)
                {
                    if (itemObs.TipoNota.Equals("2"))
                    {
                        parrafo= new Paragraph(new Run("Observaciones\n"));
                        parrafoCopia = new Paragraph(new Run("Observaciones\n"));
                        //parrafoCelda.FontFamily = new FontFamily(Constants.FONT_USAR);
                        //parrafoCeldaCopia.FontFamily = new FontFamily(Constants.FONT_USAR);
                        documentoVariable.Blocks.Add(parrafo);
                        documentoCopia.Blocks.Add(parrafoCopia);
                        parrafo = new Paragraph(new Run(itemObs.Textos));
                        parrafoCopia = new Paragraph(new Run(itemObs.Textos));
                        //parrafoCelda.FontFamily = new FontFamily(Constants.FONT_USAR);
                        //parrafoCeldaCopia.FontFamily = new FontFamily(Constants.FONT_USAR);
                        documentoVariable.Blocks.Add(parrafo);
                        documentoCopia.Blocks.Add(parrafoCopia);
                        documentoCopia.Blocks.Add(new Paragraph(new Run("")));

                    }
                    else if (itemObs.TipoNota.Equals("3"))
                    {
                        parrafo = new Paragraph(new Run("Concordancia: \n"));
                        parrafoCopia=new Paragraph(new Run("Concordancia: \n"));
                        //parrafoCelda.FontFamily = new FontFamily(Constants.FONT_USAR);
                        //parrafoCeldaCopia.FontFamily = new FontFamily(Constants.FONT_USAR);
                        documentoVariable.Blocks.Add(parrafo);
                        documentoCopia.Blocks.Add(parrafoCopia);
                        parrafo = new Paragraph(new Run(itemObs.Textos));
                        parrafoCopia = new Paragraph(new Run(itemObs.Textos));
                        //parrafoCelda.FontFamily = new FontFamily(Constants.FONT_USAR);
                        //parrafoCeldaCopia.FontFamily = new FontFamily(Constants.FONT_USAR);
                        documentoVariable.Blocks.Add(parrafo);
                        documentoCopia.Blocks.Add(parrafoCopia);
                        documentoCopia.Blocks.Add(new Paragraph(new Run("")));

                    }
                }
            }
            documentoCopia.Blocks.Add(new Paragraph(new Run("")));
            documentoCopia.Blocks.Add(new Paragraph(new Run("")));
            Documento = documentoVariable;// doc.GetFixedDocumentSequence();
            DocumentoCopia = documentoCopia;
            fachada.Close(); 
        }
    }
}
