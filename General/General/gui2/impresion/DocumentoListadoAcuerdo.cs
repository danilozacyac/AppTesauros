using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mx.gob.scjn.ius_common.TO;
//using System.Windows.Xps.Packaging;
using System.Windows.Documents;
using System.IO;
using System.IO.Packaging;
//using System.Windows.Xps.Serialization;
using System.Windows;
using System.Windows.Controls;
using System.ComponentModel;
using System.Windows.Media;
using mx.gob.scjn.ius_common.gui.gui.impresion;
using mx.gob.scjn.ius_common.utils;

namespace mx.gob.scjn.ius_common.gui.impresion
{
    public class DocumentoListadoAcuerdo
    {
        public FlowDocument Documento { get; set; }
        public FlowDocument DocumentoImpresion { get; set; }
        public List<AcuerdoSimplificadoTO> ListaImpresion;
        
        public DocumentoListadoAcuerdo(ItemCollection listado, BackgroundWorker worker)
        {
           
            int contador = 0;
            float percent = 0;
            ListaImpresion = new List<AcuerdoSimplificadoTO>();
            foreach (AcuerdoSimplificadoTO item in listado)
            {
                AcuerdoSimplificadoTO itemVerdadero = new AcuerdoSimplificadoTO();
                contador++;
                percent = (float)((float)contador / (float)listado.Count);
                worker.ReportProgress((int)(percent * 100));
                itemVerdadero.Tesis = item.Tesis;
                itemVerdadero.Rubro = item.Rubro;
                itemVerdadero.Id = item.Id;
                itemVerdadero.Promovente = item.Promovente;
                itemVerdadero.Epoca = item.Epoca;
                itemVerdadero.Sala = item.Sala;
                itemVerdadero.Fuente = item.Fuente;
                itemVerdadero.Volumen = item.Volumen;
                itemVerdadero.Pagina = item.Pagina;
                itemVerdadero.ParteT = item.ParteT;
                ListaImpresion.Add(itemVerdadero);
            }
        }

        public FlowDocument generaDocumento(List<AcuerdoSimplificadoTO> listado)
        {
            FlowDocument documentoVariable = new IUSPaginatedDocument();
            FlowDocument documentoImpresion = new FlowDocument();
            documentoVariable.FontFamily = new FontFamily(Constants.FONT_USAR);
            documentoImpresion.FontFamily = new FontFamily(Constants.FONT_USAR);
            documentoVariable.FontSize = 10;
            documentoImpresion.FontSize = 10;
            documentoVariable.ColumnWidth = 96 * 7.5;
            documentoImpresion.ColumnWidth = 96 * 7.5;
            documentoVariable.PageHeight = 96 * 11;
            documentoImpresion.PageHeight = 96 * 11;
            documentoVariable.PageWidth = 96 * 8.5;
            documentoImpresion.PageWidth = 96 * 8.5;
            documentoVariable.PagePadding = new Thickness(96 * 0.3, 96 * .5, 96 * 0.3, 96 * 0.5);
            documentoImpresion.PagePadding = new Thickness(96 * 0.3, 96 * .5, 96 * 0.3, 96 * 0.5);
            documentoVariable.ColumnWidth = 96 * 7;
            documentoImpresion.ColumnWidth = 96 * 7;
            documentoVariable.PagePadding = new Thickness(96 * 0.3, 96 * .5, 96 * 0.3, 96 * 0.5);
            documentoImpresion.PagePadding = new Thickness(96 * 0.3, 96 * .5, 96 * 0.3, 96 * 0.5);
            Table datosDocumento = new Table();
            Table datosDocumentoImpresion = new Table();
            datosDocumento.CellSpacing = 0.2;
            datosDocumentoImpresion.CellSpacing = 0.2;
            TableColumn columnaAcuerdo = new TableColumn();
            TableColumn columnaAcuerdoImpr = new TableColumn();
            columnaAcuerdo.Width = new GridLength(96 * 1.5);
            columnaAcuerdoImpr.Width = new GridLength(96 * 1.5);
            TableColumn columnaTema = new TableColumn();
            TableColumn columnaTemaImpr = new TableColumn();
            columnaTema.Width = new GridLength(96 * 3.5);
            columnaTemaImpr.Width = new GridLength(96 * 3.5);
            TableColumn columnaLoc = new TableColumn();
            TableColumn columnaLocImpr = new TableColumn();
            columnaLoc.Width = new GridLength(96 * 1);
            columnaLocImpr.Width = new GridLength(96 * 1);
            TableColumn columnaId = new TableColumn();
            TableColumn columnaIdImpr = new TableColumn();
            columnaId.Width = new GridLength(96 * 0.5);
            columnaIdImpr.Width = new GridLength(96 * 0.5);

            datosDocumento.Columns.Add(columnaAcuerdo);
            datosDocumentoImpresion.Columns.Add(columnaAcuerdoImpr);
            datosDocumento.Columns.Add(columnaTema);
            datosDocumentoImpresion.Columns.Add(columnaTemaImpr);
            datosDocumento.Columns.Add(columnaLoc);
            datosDocumentoImpresion.Columns.Add(columnaLocImpr);
            datosDocumento.Columns.Add(columnaId);
            datosDocumentoImpresion.Columns.Add(columnaIdImpr);
            TableRow Linea = new TableRow();
            TableRow LineaImpr = new TableRow();
            TableCell celdaActual = null;
            TableCell celdaActualImpr = null;
            String acuerdoTitulo = "Acuerdo";
            if (listado.ElementAt(0).ParteT.Equals("156") || listado.ElementAt(0).ParteT.Equals("163"))
            {
                acuerdoTitulo = "Otros";
                // PnlOrdenar.LblTipo.Content = "Tipo";
            }
            celdaActual = new TableCell(new Paragraph(new Run(acuerdoTitulo)));
            celdaActualImpr = new TableCell(new Paragraph(new Run(acuerdoTitulo)));
            celdaActual.FontFamily = new FontFamily(Constants.FONT_USAR);
            celdaActualImpr.FontFamily = new FontFamily(Constants.FONT_USAR);
            celdaActual.FontWeight = FontWeights.Bold;
            celdaActualImpr.FontWeight = FontWeights.Bold;
            celdaActual.FontSize = 11;
            celdaActualImpr.FontSize = 11;
            Linea.Cells.Add(celdaActual);
            LineaImpr.Cells.Add(celdaActualImpr);
            celdaActual = new TableCell(new Paragraph(new Run("Tema")));
            celdaActualImpr = new TableCell(new Paragraph(new Run("Tema")));
            celdaActual.FontFamily = new FontFamily(Constants.FONT_USAR);
            celdaActualImpr.FontFamily = new FontFamily(Constants.FONT_USAR);
            celdaActual.FontSize = 11;
            celdaActualImpr.FontSize = 11;
            celdaActual.FontWeight = FontWeights.Bold;
            celdaActualImpr.FontWeight = FontWeights.Bold;
            Linea.Cells.Add(celdaActual);
            LineaImpr.Cells.Add(celdaActualImpr);
            celdaActual = new TableCell(new Paragraph(new Run("Localización")));
            celdaActualImpr = new TableCell(new Paragraph(new Run("Localización")));
            celdaActual.FontFamily = new FontFamily(Constants.FONT_USAR);
            celdaActualImpr.FontFamily = new FontFamily(Constants.FONT_USAR);
            celdaActual.FontWeight = FontWeights.Bold;
            celdaActualImpr.FontWeight = FontWeights.Bold;
            celdaActual.FontSize = 11;
            celdaActualImpr.FontSize = 11;
            Linea.Cells.Add(celdaActual);
            LineaImpr.Cells.Add(celdaActualImpr);
            celdaActual = new TableCell(new Paragraph(new Run("Registro")));
            celdaActualImpr = new TableCell(new Paragraph(new Run("Registro")));
            celdaActual.FontWeight = FontWeights.Bold;
            celdaActualImpr.FontWeight = FontWeights.Bold;
            celdaActual.FontFamily = new FontFamily(Constants.FONT_USAR);
            celdaActualImpr.FontFamily = new FontFamily(Constants.FONT_USAR);
            celdaActual.FontSize = 11;
            celdaActualImpr.FontSize = 11;
            Linea.Cells.Add(celdaActual);
            LineaImpr.Cells.Add(celdaActualImpr);
            datosDocumento.RowGroups.Add(new TableRowGroup());
            datosDocumentoImpresion.RowGroups.Add(new TableRowGroup());
            datosDocumento.RowGroups[0].Rows.Add(Linea);
            datosDocumentoImpresion.RowGroups[0].Rows.Add(LineaImpr);
            foreach (AcuerdoSimplificadoTO item in listado)
            {
                Linea = new TableRow();
                LineaImpr = new TableRow();
                datosDocumento.RowGroups[0].Rows.Add(Linea);
                datosDocumentoImpresion.RowGroups[0].Rows.Add(LineaImpr);
                celdaActual = new TableCell(new Paragraph(new Run(item.Tesis)));
                celdaActualImpr = new TableCell(new Paragraph(new Run(item.Tesis)));
                celdaActual.FontSize = 10;
                celdaActualImpr.FontSize = 10;
                celdaActual.FontFamily = new FontFamily(Constants.FONT_USAR);
                celdaActualImpr.FontFamily = new FontFamily(Constants.FONT_USAR);
                celdaActual.Padding = new Thickness(10);
                celdaActualImpr.Padding = new Thickness(10);
                Linea.Cells.Add(celdaActual);
                LineaImpr.Cells.Add(celdaActualImpr);
                celdaActual = new TableCell(new Paragraph(new Run(item.Rubro)));
                celdaActualImpr = new TableCell(new Paragraph(new Run(item.Rubro)));
                celdaActual.FontFamily = new FontFamily(Constants.FONT_USAR);
                celdaActualImpr.FontFamily = new FontFamily(Constants.FONT_USAR);
                celdaActual.FontSize = 10;
                celdaActualImpr.FontSize = 10;
                celdaActual.Padding = new Thickness(10);
                celdaActualImpr.Padding = new Thickness(10);
                Linea.Cells.Add(celdaActual);
                LineaImpr.Cells.Add(celdaActualImpr);
                celdaActual = new TableCell(new Paragraph(new Run(item.Loc)));
                celdaActualImpr = new TableCell(new Paragraph(new Run(item.Loc)));
                celdaActual.FontFamily = new FontFamily(Constants.FONT_USAR);
                celdaActualImpr.FontFamily = new FontFamily(Constants.FONT_USAR);
                celdaActual.FontSize = 10;
                celdaActualImpr.FontSize = 10;
                celdaActual.Padding = new Thickness(10);
                celdaActualImpr.Padding = new Thickness(10);
                Linea.Cells.Add(celdaActual);
                LineaImpr.Cells.Add(celdaActualImpr);
                celdaActual = new TableCell(new Paragraph(new Run(item.Id)));
                celdaActualImpr = new TableCell(new Paragraph(new Run(item.Id)));
                celdaActual.FontFamily = new FontFamily(Constants.FONT_USAR);
                celdaActualImpr.FontFamily = new FontFamily(Constants.FONT_USAR);
                celdaActual.FontSize = 10;
                celdaActualImpr.FontSize = 10;
                celdaActual.FontFamily = new FontFamily(Constants.FONT_USAR);
                celdaActualImpr.FontFamily = new FontFamily(Constants.FONT_USAR);
                celdaActual.Padding = new Thickness(10);
                celdaActualImpr.Padding = new Thickness(10);
                Linea.Cells.Add(celdaActual);
                LineaImpr.Cells.Add(celdaActualImpr);
            }
            documentoVariable.Blocks.Add(datosDocumento);
            documentoImpresion.Blocks.Add(datosDocumentoImpresion);
            DocumentoImpresion = documentoImpresion;

            return documentoVariable;
        }
    }
}
