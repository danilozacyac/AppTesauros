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
using mx.gob.scjn.ius_common.gui.gui.impresion;
using System.Windows.Media;
using mx.gob.scjn.ius_common.utils;

namespace mx.gob.scjn.ius_common.gui.impresion
{
    public class DocumentoVoto
    {
        public FlowDocument Documento { get; set; }
        public FlowDocument Copia { get; set; }
        protected int Parte { get; set; }
        public DocumentoVoto(VotoSimplificadoTO DocumentoActual, int NumeroParte)
        {
            Parte = NumeroParte == 0 ? -1 : NumeroParte;
            List<VotoSimplificadoTO> listado = new List<VotoSimplificadoTO>();
            listado.Add(DocumentoActual);
            generaDocumento(listado);
        }
        public DocumentoVoto(HashSet<int> listadoEnteros)
        {
            Parte = -1;
            List<VotoSimplificadoTO> listado = new List<VotoSimplificadoTO>();
            foreach (int item in listadoEnteros)
            {
                VotoSimplificadoTO itemTesis = new VotoSimplificadoTO();
                itemTesis.Id = "" + item;
                listado.Add(itemTesis);
            }
            generaDocumento(listado);
        }
        /// <summary>
        /// Genera un documento a partir de un listado de tesis.
        /// </summary>
        /// <param name="todosLosDocumentos">El listado de tesis para el documento.</param>
        private void generaDocumento(List<VotoSimplificadoTO> todosLosDocumentos)
        {
            FlowDocument documentoVariable = new IUSPaginatedDocument();
            FlowDocument documentoCopia = new FlowDocument();
            documentoCopia.FontSize = 15;
            documentoCopia.FontFamily = new FontFamily(Constants.FONT_USAR);
            foreach (VotoSimplificadoTO DocumentoActual in todosLosDocumentos)
            {
                FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
                String Materias = fachada.getMateriasTesis(DocumentoActual.Id);
                documentoVariable.ColumnWidth = 96 * 7;
                documentoCopia.ColumnWidth = 96 * 7;
                Section seccionActual = new Section();
                Section seccionActualCopia = new Section();
                seccionActual.BreakPageBefore = true;
                seccionActualCopia.BreakPageBefore = true;
                documentoVariable.Blocks.Add(seccionActual);
                documentoCopia.Blocks.Add(seccionActualCopia);
                documentoVariable.PagePadding = new Thickness(96 * 0.5, 96 * 1, 96 * 0.5, 96 * 0.5);
                documentoCopia.PagePadding = new Thickness(96 * 0.5, 96 * 1, 96 * 0.5, 96 * 0.5);
                Table datosDocumento = new Table();
                //Table datosDocumentoCopia = new Table();
                TableColumn columnaInicial = new TableColumn();
                //TableColumn columnaInicialCopia = new TableColumn();
                columnaInicial.Width = new GridLength(96 * 1.5);
                //columnaInicialCopia.Width = new GridLength(96 * 1);
                TableColumn columnaAncha = new TableColumn();
                //TableColumn columnaAnchaCopia = new TableColumn();
                columnaAncha.Width = new GridLength(96 * 3);
                //columnaAnchaCopia.Width = new GridLength(96 * 3);
                TableColumn columnaDatos = new TableColumn();
                //TableColumn columnaDatosCopia = new TableColumn();
                columnaDatos.Width = new GridLength(96 * 1);
                //columnaDatosCopia.Width = new GridLength(96 * 1);
                TableColumn columnaFinal = new TableColumn();
                //TableColumn columnaFinalCopia = new TableColumn();
                columnaFinal.Width = new GridLength(96 * 1);
                //columnaFinalCopia.Width = new GridLength(96 * 1);
                datosDocumento.Columns.Add(columnaInicial);
                //datosDocumentoCopia.Columns.Add(columnaInicialCopia);
                datosDocumento.Columns.Add(columnaAncha);
                //datosDocumentoCopia.Columns.Add(columnaAnchaCopia);
                datosDocumento.Columns.Add(columnaDatos);
                //datosDocumentoCopia.Columns.Add(columnaDatosCopia);
                datosDocumento.Columns.Add(columnaFinal);
                //datosDocumentoCopia.Columns.Add(columnaFinalCopia);

                datosDocumento.RowGroups.Add(new TableRowGroup());
                //datosDocumentoCopia.RowGroups.Add(new TableRowGroup());
                for (int i = 0; i < 6; i++)
                {
                    datosDocumento.RowGroups[0].Rows.Add(new TableRow());
                    //  datosDocumentoCopia.RowGroups[0].Rows.Add(new TableRow());
                }

                TableCell celdaActual = new TableCell(new Paragraph(new Run("No. de Registro:")));
                //celdaActual.ColumnSpan = 2;
                //TableCell celdaActualCopia = new TableCell(new Paragraph(new Run("No. de Registro:")));
                //celdaActualCopia.ColumnSpan = 2;
                datosDocumento.RowGroups[0].Rows[0].Cells.Add(celdaActual);
                //datosDocumentoCopia.RowGroups[0].Rows[0].Cells.Add(celdaActualCopia);
                celdaActual = new TableCell(new Paragraph(new Run(DocumentoActual.Id)));
                documentoCopia.Blocks.Add(new Paragraph(new Run("No. de Registro: "+DocumentoActual.Id)));
                datosDocumento.RowGroups[0].Rows[0].Cells.Add(celdaActual);
                //datosDocumentoCopia.RowGroups[0].Rows[0].Cells.Add(celdaActualCopia);

                Paragraph parrafoCelda = new Paragraph(new Run(DocumentoActual.Epoca));
                Paragraph parrafoCeldaCopia = new Paragraph(new Run(DocumentoActual.Epoca));
                celdaActual = new TableCell(parrafoCelda);
                celdaActual.ColumnSpan = 2;
                documentoCopia.Blocks.Add(parrafoCeldaCopia);
                //celdaActualCopia.ColumnSpan = 2;
                datosDocumento.RowGroups[0].Rows[1].Cells.Add(celdaActual);
                //datosDocumentoCopia.RowGroups[0].Rows[1].Cells.Add(celdaActualCopia);
                
                
                celdaActual = new TableCell(new Paragraph(new Run("Instancia:")));
                //celdaActualCopia = new TableCell(new Paragraph(new Run("Instancia:")));
                datosDocumento.RowGroups[0].Rows[2].Cells.Add(celdaActual);
                //datosDocumentoCopia.RowGroups[0].Rows[2].Cells.Add(celdaActualCopia);
                celdaActual = new TableCell(new Paragraph(new Run(DocumentoActual.Sala)));
                //celdaActual.ColumnSpan = 3;
                documentoCopia.Blocks.Add(new Paragraph(new Run("Instancia: " + DocumentoActual.Sala)));
                //celdaActualCopia.ColumnSpan = 3;
                datosDocumento.RowGroups[0].Rows[2].Cells.Add(celdaActual);
                //datosDocumentoCopia.RowGroups[0].Rows[2].Cells.Add(celdaActualCopia);

                celdaActual = new TableCell(new Paragraph(new Run("Fuente:")));
                //celdaActualCopia = new TableCell(new Paragraph(new Run("Fuente:")));
                datosDocumento.RowGroups[0].Rows[3].Cells.Add(celdaActual);
                //datosDocumentoCopia.RowGroups[0].Rows[3].Cells.Add(celdaActualCopia);
                celdaActual = new TableCell(new Paragraph(new Run(DocumentoActual.Fuente)));
                celdaActual.ColumnSpan = 3;
                documentoCopia.Blocks.Add(new Paragraph(new Run("Fuente: " + DocumentoActual.Fuente)));
                //celdaActualCopia.ColumnSpan = 3;
                datosDocumento.RowGroups[0].Rows[3].Cells.Add(celdaActual);
                //datosDocumentoCopia.RowGroups[0].Rows[3].Cells.Add(celdaActualCopia);

                celdaActual = new TableCell(new Paragraph(new Run("")));
                //celdaActualCopia = new TableCell(new Paragraph(new Run("Tomo:")));
                datosDocumento.RowGroups[0].Rows[4].Cells.Add(celdaActual);
                //datosDocumentoCopia.RowGroups[0].Rows[4].Cells.Add(celdaActualCopia);
                celdaActual = new TableCell(new Paragraph(new Run(DocumentoActual.Volumen)));
                celdaActual.ColumnSpan = 3;
                documentoCopia.Blocks.Add(new Paragraph(new Run(DocumentoActual.Volumen)));
                //celdaActualCopia.ColumnSpan = 3;
                datosDocumento.RowGroups[0].Rows[4].Cells.Add(celdaActual);
                //datosDocumentoCopia.RowGroups[0].Rows[4].Cells.Add(celdaActualCopia);

                celdaActual = new TableCell(new Paragraph(new Run("Página: ")));
                //celdaActualCopia = new TableCell(new Paragraph(new Run("Página: ")));
                datosDocumento.RowGroups[0].Rows[5].Cells.Add(celdaActual);
                //datosDocumentoCopia.RowGroups[0].Rows[5].Cells.Add(celdaActualCopia);
                celdaActual = new TableCell(new Paragraph(new Run(DocumentoActual.Pagina)));
                documentoCopia.Blocks.Add(new Paragraph(new Run("Página: " + DocumentoActual.Pagina)));
                datosDocumento.RowGroups[0].Rows[5].Cells.Add(celdaActual);
                //datosDocumentoCopia.RowGroups[0].Rows[5].Cells.Add(celdaActualCopia);
                celdaActual = new TableCell(new Paragraph(new Run("")));
                //celdaActualCopia = new TableCell(new Paragraph(new Run("")));
                datosDocumento.RowGroups[0].Rows[5].Cells.Add(celdaActual);
                //datosDocumentoCopia.RowGroups[0].Rows[5].Cells.Add(celdaActualCopia);
                celdaActual = new TableCell(new Paragraph(new Run("")));
                //celdaActualCopia = new TableCell(new Paragraph(new Run("")));
                datosDocumento.RowGroups[0].Rows[5].Cells.Add(celdaActual);
                //datosDocumentoCopia.RowGroups[0].Rows[5].Cells.Add(celdaActualCopia);
                documentoVariable.Blocks.Add(datosDocumento);
                //documentoCopia.Blocks.Add(datosDocumentoCopia);
                documentoCopia.Blocks.Add(new Paragraph(new Run("")));
                Paragraph parrafo = new Paragraph(new Run(DocumentoActual.Rubro));
                Paragraph parrafoCopia = new Paragraph(new Run(DocumentoActual.Rubro));
                parrafo.FontWeight = FontWeights.Bold;
                parrafoCopia.FontWeight = FontWeights.Bold;
                documentoVariable.Blocks.Add(parrafo);
                documentoCopia.Blocks.Add(parrafoCopia);
                documentoCopia.Blocks.Add(new Paragraph(new Run("")));
                documentoVariable.Blocks.Add(new Paragraph(new Run("")));
                VotosPartesTO[] partes= fachada.getVotosPartesPorId(Int32.Parse(DocumentoActual.Id));
                String Texto = "";
                foreach (VotosPartesTO item in partes)
                {
                    if ((item.Parte == Parte) || (Parte == -1))
                    {
                        char[] enter = new char[1];
                        enter[0] = '\n';
                        String[] Parrafos = item.TxtParte.Split(enter);
                        foreach (String parrafoAgrega in Parrafos)
                        {
                            if (!parrafoAgrega.Replace('\n', ' ').Trim().Equals(""))
                            {
                                parrafo = new Paragraph(new Run(parrafoAgrega));
                                parrafoCopia = new Paragraph(new Run(parrafoAgrega));
                                parrafo.FontWeight = FontWeights.Normal;
                                parrafoCopia.FontWeight = FontWeights.Normal;
                                parrafo.TextAlignment = TextAlignment.Justify;
                                documentoVariable.Blocks.Add(parrafo);
                                documentoCopia.Blocks.Add(parrafoCopia);
                            }
                        }
                    }
                }
                //parrafo = new Paragraph(new Run(Texto));
                //parrafoCopia = new Paragraph(new Run(Texto));
                //parrafo.FontWeight = FontWeights.Normal;
                //parrafoCopia.FontWeight=FontWeights.Normal;
                //documentoVariable.Blocks.Add(parrafo);
                //documentoCopia.Blocks.Add(parrafoCopia);
                //parrafo = new Paragraph(new Run(DocumentoActual.Precedentes));
                //parrafo.FontWeight = FontWeights.Normal;
                //documentoVariable.Blocks.Add(parrafo);
                //IDocumentPaginatorSource paginado = documentoVariable as IDocumentPaginatorSource;
                //MemoryStream ms = new MemoryStream();
                //Package pkg = Package.Open(ms, FileMode.Create, FileAccess.ReadWrite);
                //string pack = "pack://temp.xps";
                //PackageStore.RemovePackage(new Uri(pack));
                //PackageStore.AddPackage(new Uri(pack), pkg);
                //XpsDocument doc = new XpsDocument(pkg, CompressionOption.SuperFast, pack);
                //XpsSerializationManager rsm = new XpsSerializationManager(new XpsPackagingPolicy(doc), false);
                //DocumentPaginator pgn = paginado.DocumentPaginator;
                //pgn.PageSize = new Size(96 * 8, 96 * 11.5);
                //rsm.SaveAsXaml(pgn);
            }
            Documento = documentoVariable;// doc.GetFixedDocumentSequence();
            Copia = documentoCopia;
        }
    }
}
