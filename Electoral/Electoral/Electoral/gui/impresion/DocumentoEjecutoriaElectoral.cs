using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using mx.gob.scjn.electoral_common.TO;
using mx.gob.scjn.ius_common.TO;
using mx.gob.scjn.ius_common.fachade;
using mx.gob.scjn.ius_common.gui.gui.impresion;
using mx.gob.scjn.ius_common.utils;

namespace mx.gob.scjn.electoral_common.gui.impresion
{
    public class DocumentoEjecutorias
    {
        public FlowDocument Documento { get; set; }
        public FlowDocument Copia { get; set; }
        private int NumeroParte { get; set; }
        public DocumentoEjecutorias(EjecutoriaSimplificadaElectoralTO DocumentoActual, int numeroParte)
        {
            NumeroParte = numeroParte;
            List<EjecutoriaSimplificadaElectoralTO> listado = new List<EjecutoriaSimplificadaElectoralTO>();
            listado.Add(DocumentoActual);
            generaDocumento(listado);
        }

        public DocumentoEjecutorias(HashSet<int> listadoEnteros)
        {
            List<EjecutoriaSimplificadaElectoralTO> listado = new List<EjecutoriaSimplificadaElectoralTO>();
            foreach (int item in listadoEnteros)
            {
                EjecutoriaSimplificadaElectoralTO itemTesis = new EjecutoriaSimplificadaElectoralTO();
                itemTesis.Id = "" + item;
                listado.Add(itemTesis);
            }
            generaDocumento(listado);
        }

        /// <summary>
        /// Genera un documento a partir de un listado de tesis.
        /// </summary>
        /// <param name="todosLosDocumentos">El listado de tesis para el documento.</param>
        private void generaDocumento(List<EjecutoriaSimplificadaElectoralTO> todosLosDocumentos)
        {
            FlowDocument documentoVariable = new IUSPaginatedDocument();
            FlowDocument documentoCopia = new FlowDocument();
            documentoCopia.FontSize = 15;
            documentoCopia.FontFamily = new FontFamily(Constants.FONT_USAR);
            foreach (EjecutoriaSimplificadaElectoralTO DocumentoActual in todosLosDocumentos)
            {
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
                TableColumn columnaInicial = new TableColumn();
                columnaInicial.Width = new GridLength(96 * 1.5);
                TableColumn columnaAncha = new TableColumn();
                columnaAncha.Width = new GridLength(96 * 3);
                TableColumn columnaDatos = new TableColumn();
                columnaDatos.Width = new GridLength(96 * 1);
                TableColumn columnaFinal = new TableColumn();
                columnaFinal.Width = new GridLength(96 * 1);
                datosDocumento.Columns.Add(columnaInicial);
                datosDocumento.Columns.Add(columnaAncha);
                datosDocumento.Columns.Add(columnaDatos);
                datosDocumento.Columns.Add(columnaFinal);

                datosDocumento.RowGroups.Add(new TableRowGroup());
                for (int i = 0; i < 6; i++)
                {
                    datosDocumento.RowGroups[0].Rows.Add(new TableRow());
                }


                TableCell celdaActual = new TableCell(new Paragraph(new Run("No. de registro:")));
                datosDocumento.RowGroups[0].Rows[0].Cells.Add(celdaActual);
                celdaActual = new TableCell(new Paragraph(new Run(DocumentoActual.Id)));
                documentoCopia.Blocks.Add(new Paragraph(new Run("No. de registro: " + DocumentoActual.Id)));
                datosDocumento.RowGroups[0].Rows[0].Cells.Add(celdaActual);

                Paragraph parrafoCelda = new Paragraph(new Run(DocumentoActual.Epoca));
                Paragraph parrafoCeldaCopia = new Paragraph(new Run(DocumentoActual.Epoca));
                celdaActual = new TableCell(parrafoCelda);
                celdaActual.ColumnSpan = 2;
                documentoCopia.Blocks.Add(parrafoCeldaCopia);
                datosDocumento.RowGroups[0].Rows[1].Cells.Add(celdaActual);

                celdaActual = new TableCell(new Paragraph(new Run("Instancia:")));
                datosDocumento.RowGroups[0].Rows[2].Cells.Add(celdaActual);
                celdaActual = new TableCell(new Paragraph(new Run(DocumentoActual.Sala)));
                documentoCopia.Blocks.Add(new Paragraph(new Run("Instancia: " + DocumentoActual.Sala)));
                datosDocumento.RowGroups[0].Rows[2].Cells.Add(celdaActual);

                celdaActual = new TableCell(new Paragraph(new Run("Fuente:")));
                datosDocumento.RowGroups[0].Rows[3].Cells.Add(celdaActual);
                celdaActual = new TableCell(new Paragraph(new Run(DocumentoActual.Fuente)));
                celdaActual.ColumnSpan = 3;
                documentoCopia.Blocks.Add(new Paragraph(new Run("Fuente: " + DocumentoActual.Fuente)));
                datosDocumento.RowGroups[0].Rows[3].Cells.Add(celdaActual);


                celdaActual = new TableCell(new Paragraph(new Run("")));
                datosDocumento.RowGroups[0].Rows[4].Cells.Add(celdaActual);
                celdaActual = new TableCell(new Paragraph(new Run(DocumentoActual.Volumen)));
                celdaActual.ColumnSpan = 3;
                documentoCopia.Blocks.Add(new Paragraph(new Run(DocumentoActual.Volumen)));
                celdaActual.ColumnSpan = 3;
                datosDocumento.RowGroups[0].Rows[4].Cells.Add(celdaActual);

                if (DocumentoActual.Pagina.Trim().Equals("0"))
                {
                    celdaActual = new TableCell(new Paragraph(new Run("Página: ")));
                    datosDocumento.RowGroups[0].Rows[5].Cells.Add(celdaActual);
                    celdaActual = new TableCell(new Paragraph(new Run(DocumentoActual.Pagina)));
                    documentoCopia.Blocks.Add(new Paragraph(new Run("Página: " + DocumentoActual.Pagina)));
                }
                else
                {
                    celdaActual = new TableCell(new Paragraph(new Run("Sin número de página")));
                    datosDocumento.RowGroups[0].Rows[5].Cells.Add(celdaActual);
                    celdaActual = new TableCell(new Paragraph(new Run("")));
                    documentoCopia.Blocks.Add(new Paragraph(new Run("Sin número de página ")));
                }
                datosDocumento.RowGroups[0].Rows[5].Cells.Add(celdaActual);
                celdaActual = new TableCell(new Paragraph(new Run("")));
                datosDocumento.RowGroups[0].Rows[5].Cells.Add(celdaActual);
                celdaActual = new TableCell(new Paragraph(new Run("")));
                datosDocumento.RowGroups[0].Rows[5].Cells.Add(celdaActual);
                documentoVariable.Blocks.Add(datosDocumento);
                documentoCopia.Blocks.Add(new Paragraph(new Run("")));
                Paragraph parrafo = null;
                Paragraph parrafoCopia = null;
#if STAND_ALONE
                FachadaBusquedaTradicional fachada = new FachadaBusquedaTradicional();
                List<RelDocumentoTesisTO> tesisRel = fachada.getTesisPorEjecutoriaElectoral(DocumentoActual.Id);
                if ((tesisRel == null) || (tesisRel.Count == 0))
#else
                FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
                RelDocumentoTesisTO[] tesisRel = fachada.getTesisPorEjecutoriaElectoral(DocumentoActual.Id);
                if ((tesisRel==null)||(tesisRel.Length == 0))
#endif
                {
                    parrafo = new Paragraph(new Run(DocumentoActual.Rubro));
                    parrafoCopia = new Paragraph(new Run(DocumentoActual.Rubro));
                    parrafo.FontWeight = FontWeights.Bold;
                    parrafoCopia.FontWeight = FontWeights.Bold;
                    documentoVariable.Blocks.Add(parrafo);
                    documentoCopia.Blocks.Add(parrafoCopia);
                    documentoCopia.Blocks.Add(new Paragraph(new Run("")));
                    documentoVariable.Blocks.Add(new Paragraph(new Run("")));
                }
                //FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
#if STAND_ALONE
                List<EjecutoriasPartesTO> partes = fachada.getPartesElectoralPorId(Int32.Parse(DocumentoActual.Id));
#else
                EjecutoriasPartesTO[] partes = fachada.getPartesElectoralPorId(Int32.Parse(DocumentoActual.Id));
#endif
                fachada.Close();
                foreach (EjecutoriasPartesTO itemParte in partes)
                {
                    if ((NumeroParte == -1) || (NumeroParte == itemParte.Parte))
                    {
                        char[] enter = new char[1];
                        enter[0] = '\n';
                        String[] Parrafos = itemParte.TxtParte.Split(enter);
                        foreach (String parrafoAgrega in Parrafos)
                        {
                            if (!parrafoAgrega.Replace('\n',' ').Trim().Equals(""))
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
            }
            Documento = documentoVariable;
            Copia = documentoCopia;
        }
    }
}
