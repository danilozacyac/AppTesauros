using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using mx.gob.scjn.ius_common.TO;
using mx.gob.scjn.ius_common.fachade;
using mx.gob.scjn.ius_common.gui.gui.impresion;
using mx.gob.scjn.ius_common.utils;

//using System.Windows.Xps.Packaging;
//using System.Windows.Xps.Serialization;
namespace mx.gob.scjn.electoral_common.gui.impresion
{
    public class DocumentoAcuerdoElectoral
    {
        public FlowDocument Documento { get; set; }
        public FlowDocument Copia { get; set; }
        protected int Parte { get; set; }
        public DocumentoAcuerdoElectoral(AcuerdosTO DocumentoActual, int NumeroParte)
        {
            Parte = NumeroParte;
            List<AcuerdosTO> listado = new List<AcuerdosTO>();
            listado.Add(DocumentoActual);
            generaDocumento(listado);
        }
        public DocumentoAcuerdoElectoral(HashSet<int> listadoEnteros)
        {
            Parte = -1;
            List<AcuerdosTO> listado = new List<AcuerdosTO>();
            foreach (int item in listadoEnteros)
            {
                AcuerdosTO itemTesis = new AcuerdosTO();
                itemTesis.Id = "" + item;
                listado.Add(itemTesis);
            }
            generaDocumento(listado);
        }
        /// <summary>
        /// Genera un documento a partir de un listado de tesis.
        /// </summary>
        /// <param name="todosLosDocumentos">El listado de tesis para el documento.</param>
        private void generaDocumento(List<AcuerdosTO> todosLosDocumentos)
        {
            FlowDocument documentoVariable = new IUSPaginatedDocument();
            FlowDocument documentoCopia = new FlowDocument();
            documentoCopia.FontSize = 15;
            documentoCopia.FontFamily = new FontFamily(Constants.FONT_USAR);
            foreach (AcuerdosTO DocumentoActual in todosLosDocumentos)
            {
#if STAND_ALONE
                FachadaBusquedaTradicional fachada = new FachadaBusquedaTradicional();
#else
                FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
#endif
                String Materias = fachada.getMateriasTesisElectoral(DocumentoActual.Id);
                documentoVariable.ColumnWidth = 96 * 7;
                documentoCopia.ColumnWidth = 96 * 7;
                Section seccionActual = new Section();
                Section seccionCopia = new Section();
                seccionActual.BreakPageBefore = true;
                seccionCopia.BreakPageBefore = true;
                documentoVariable.Blocks.Add(seccionActual);
                documentoCopia.Blocks.Add(seccionCopia);
                documentoVariable.PageHeight = 96 * 11;
                documentoCopia.PageHeight = 96 * 11;
                documentoVariable.PageWidth = 96 * 8.5;
                documentoCopia.PageWidth = 96 * 8.5;
                documentoVariable.Background = Brushes.White;
                documentoCopia.Background = Brushes.White;
                documentoVariable.PagePadding = new Thickness(96 * 0.5, 96 * 1, 96 * 0.5, 96 * 0.5);
                documentoCopia.PagePadding = new Thickness(96 * 0.5, 96 * 1, 96 * 0.5, 96 * 0.5);
                Table datosDocumento = new Table();
                //Table datosCopia = new Table();
                TableColumn columnaInicial = new TableColumn();
                //TableColumn columnaInicialCopia = new TableColumn();
                columnaInicial.Width = new GridLength(96 * 1.5);
                //columnaInicialCopia.Width = new GridLength( 96 * 1);
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
                //columnaFinalCopia.Width = new GridLength();
                datosDocumento.Columns.Add(columnaInicial);
                //datosCopia.Columns.Add(columnaInicialCopia);
                datosDocumento.Columns.Add(columnaAncha);
                //datosCopia.Columns.Add(columnaAnchaCopia);
                datosDocumento.Columns.Add(columnaDatos);
                //datosCopia.Columns.Add(columnaDatosCopia);
                datosDocumento.Columns.Add(columnaFinal);
                //datosCopia.Columns.Add(columnaFinalCopia);

                datosDocumento.RowGroups.Add(new TableRowGroup());
                //datosCopia.RowGroups.Add(new TableRowGroup());
                for (int i = 0; i < 6; i++)
                {
                    datosDocumento.RowGroups[0].Rows.Add(new TableRow());
                    //  datosCopia.RowGroups[0].Rows.Add(new TableRow());
                }

                TableCell celdaActual = new TableCell(new Paragraph(new Run("No. de Registro:")));
                //celdaActual.ColumnSpan = 2;
                //TableCell celdaActualCopia = new TableCell(new Paragraph(new Run("No. de Registro:")));
                //celdaActualCopia.ColumnSpan = 2;
                datosDocumento.RowGroups[0].Rows[0].Cells.Add(celdaActual);
                //datosCopia.RowGroups[0].Rows[0].Cells.Add(celdaActualCopia);
                celdaActual = new TableCell(new Paragraph(new Run(DocumentoActual.Id)));
                documentoCopia.Blocks.Add(new Paragraph(new Run("No. de registro: " + DocumentoActual.Id)));
                datosDocumento.RowGroups[0].Rows[0].Cells.Add(celdaActual);
                //datosCopia.RowGroups[0].Rows[0].Cells.Add(celdaActualCopia);

                Paragraph parrafoCelda = new Paragraph(new Run(DocumentoActual.Epoca));
                Paragraph parrafoCeldaCopia = new Paragraph(new Run(DocumentoActual.Epoca));
                celdaActual = new TableCell(parrafoCelda);
                celdaActual.ColumnSpan = 3;
                //celdaActualCopia = new TableCell(parrafoCeldaCopia);
                //celdaActualCopia.ColumnSpan = 3;
                datosDocumento.RowGroups[0].Rows[1].Cells.Add(celdaActual);
                //datosCopia.RowGroups[0].Rows[1].Cells.Add(celdaActualCopia);

                celdaActual = new TableCell(new Paragraph(new Run("Instancia:")));
                //celdaActualCopia = new TableCell(new Paragraph(new Run("Instancia:")));
                datosDocumento.RowGroups[0].Rows[2].Cells.Add(celdaActual);
                //datosCopia.RowGroups[0].Rows[2].Cells.Add(celdaActualCopia);
                celdaActual = new TableCell(new Paragraph(new Run(DocumentoActual.Sala)));
                celdaActual.ColumnSpan = 3;
                documentoCopia.Blocks.Add(new Paragraph(new Run("Instancia: " + DocumentoActual.Sala)));
                //celdaActualCopia.ColumnSpan = 3;
                datosDocumento.RowGroups[0].Rows[2].Cells.Add(celdaActual);
                //datosCopia.RowGroups[0].Rows[2].Cells.Add(celdaActualCopia);

                celdaActual = new TableCell(new Paragraph(new Run("Fuente:")));
                //celdaActualCopia = new TableCell(new Paragraph(new Run("Fuente:")));
                datosDocumento.RowGroups[0].Rows[3].Cells.Add(celdaActual);
                //datosCopia.RowGroups[0].Rows[3].Cells.Add(celdaActualCopia);
                celdaActual = new TableCell(new Paragraph(new Run(DocumentoActual.Fuente)));
                celdaActual.ColumnSpan = 3;
                documentoCopia.Blocks.Add(new Paragraph(new Run("Fuente: " + DocumentoActual.Fuente)));
                //celdaActualCopia.ColumnSpan = 3;
                datosDocumento.RowGroups[0].Rows[3].Cells.Add(celdaActual);
                //datosCopia.RowGroups[0].Rows[3].Cells.Add(celdaActualCopia);

                celdaActual = new TableCell(new Paragraph(new Run("")));
                //celdaActualCopia = new TableCell(new Paragraph(new Run("Tomo:")));
                datosDocumento.RowGroups[0].Rows[4].Cells.Add(celdaActual);
                //datosCopia.RowGroups[0].Rows[4].Cells.Add(celdaActualCopia);
                celdaActual = new TableCell(new Paragraph(new Run(DocumentoActual.Volumen)));
                celdaActual.ColumnSpan = 3;
                documentoCopia.Blocks.Add(new Paragraph(new Run(DocumentoActual.Volumen)));
                //celdaActualCopia.ColumnSpan = 3;
                datosDocumento.RowGroups[0].Rows[4].Cells.Add(celdaActual);
                //datosCopia.RowGroups[0].Rows[4].Cells.Add(celdaActualCopia);

                celdaActual = new TableCell(new Paragraph(new Run("Página: ")));
                //celdaActualCopia = new TableCell(new Paragraph(new Run("Página: ")));
                datosDocumento.RowGroups[0].Rows[5].Cells.Add(celdaActual);
                //datosCopia.RowGroups[0].Rows[5].Cells.Add(celdaActualCopia);
                celdaActual = new TableCell(new Paragraph(new Run(DocumentoActual.Pagina)));
                documentoCopia.Blocks.Add(new Paragraph(new Run("Página: " + DocumentoActual.Pagina)));
                datosDocumento.RowGroups[0].Rows[5].Cells.Add(celdaActual);
                //datosCopia.RowGroups[0].Rows[5].Cells.Add(celdaActualCopia);
                celdaActual = new TableCell(new Paragraph(new Run("")));
                //celdaActualCopia = new TableCell(new Paragraph(new Run("")));
                datosDocumento.RowGroups[0].Rows[5].Cells.Add(celdaActual);
                //datosCopia.RowGroups[0].Rows[5].Cells.Add(celdaActualCopia);
                celdaActual = new TableCell(new Paragraph(new Run("")));
                //celdaActualCopia = new TableCell(new Paragraph(new Run("")));
                datosDocumento.RowGroups[0].Rows[5].Cells.Add(celdaActual);
                //datosCopia.RowGroups[0].Rows[5].Cells.Add(celdaActualCopia);
                documentoVariable.Blocks.Add(datosDocumento);
                //documentoCopia.Blocks.Add(datosCopia);
                documentoCopia.Blocks.Add(new Paragraph(new Run("")));
                Paragraph parrafo = new Paragraph(new Run(DocumentoActual.Rubro+"\n"));
                Paragraph parrafoCopia = new Paragraph(new Run(DocumentoActual.Rubro+"\n"));
                parrafo.FontWeight = FontWeights.Bold;
                parrafoCopia.FontWeight = FontWeights.Bold;
                documentoVariable.Blocks.Add(parrafo);
                documentoCopia.Blocks.Add(parrafoCopia);
                documentoCopia.Blocks.Add(new Paragraph(new Run("")));
                documentoVariable.Blocks.Add(new Paragraph(new Run("")));
#if STAND_ALONE
                List<AcuerdosPartesTO> partes = fachada.getAcuerdoPartesElectoralPorId(Int32.Parse(DocumentoActual.Id));
#else
                AcuerdosPartesTO[] partes= fachada.getAcuerdoPartesElectoralPorId(Int32.Parse(DocumentoActual.Id));
#endif
                foreach (AcuerdosPartesTO item in partes)
                {
                    if ((Parte == -1) || (Parte == item.Parte))
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
                //parrafoCopia.FontWeight = FontWeights.Normal;
                //documentoVariable.Blocks.Add(parrafo);
                //documentoCopia.Blocks.Add(parrafoCopia);
            }
            Documento = documentoVariable;// doc.GetFixedDocumentSequence();
            Copia = documentoCopia;
        }
    }
}
