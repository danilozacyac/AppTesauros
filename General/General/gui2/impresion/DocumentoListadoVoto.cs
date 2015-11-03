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
    public class DocumentoListadoVoto
    {
        public FlowDocument Documento { get; set; }
        public List<VotoSimplificadoTO> ListaImpresion { get; set; }
        public DocumentoListadoVoto(ItemCollection listado, BackgroundWorker worker)
        {
           
            int contador = 0;
            float percent = 0;
            ListaImpresion = new List<VotoSimplificadoTO>();
            foreach (VotoSimplificadoTO item in listado)
            {
                //VotosTO itemVerdadero = new VotosTO();
                contador++;
                percent = (float)((float)contador / (float)listado.Count);
                worker.ReportProgress((int)(percent * 100));
                //itemVerdadero.Rubro = item.Rubro;
                //itemVerdadero.Id = item.Id;
                //itemVerdadero.Promovente = item.Promovente;
                //itemVerdadero.Complemento = item.Loc;
                ListaImpresion.Add(item);
            }
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

        public static FlowDocument generaDocumento(List<VotoSimplificadoTO> listado)
        {
            FlowDocument documentoVariable = new IUSPaginatedDocument();
            documentoVariable.Background = Brushes.White;
            documentoVariable.FontFamily = new FontFamily("Arial");
            documentoVariable.FontSize = 10;
            documentoVariable.ColumnWidth = 96 * 7.5;
            documentoVariable.PageHeight = 96 * 11;
            documentoVariable.PageWidth = 96 * 8.5;
            documentoVariable.PagePadding = new Thickness(96 * 0.3, 96 * .5, 96 * 0.3, 96 * 0.5); 
            documentoVariable.ColumnWidth = 96 * 7;
            documentoVariable.PagePadding = new Thickness(96 * 0.3, 96 * .5, 96 * 0.3, 96 * 0.5);
            Table datosDocumento = new Table();
            datosDocumento.CellSpacing = 0.1*96;
            TableColumn columnaAsunto = new TableColumn();
            columnaAsunto.Width = new GridLength(96 * 3);
            TableColumn columnaPromovente = new TableColumn();
            columnaPromovente.Width = new GridLength(96 * 2);
            TableColumn columnaLoc = new TableColumn();
            columnaLoc.Width = new GridLength(96 * 1);
            TableColumn columnaId = new TableColumn();
            columnaId.Width = new GridLength(96 * 0.5);

            datosDocumento.Columns.Add(columnaId);
            datosDocumento.Columns.Add(columnaAsunto);
            datosDocumento.Columns.Add(columnaPromovente);
            datosDocumento.Columns.Add(columnaLoc);
            TableRow Linea = new TableRow();
            TableCell celdaActual = null;
            celdaActual = new TableCell(new Paragraph(new Run("Registro")));
            celdaActual.FontSize = 11;
            celdaActual.FontFamily = new FontFamily(Constants.FONT_USAR);
            celdaActual.FontWeight = FontWeights.Bold;
            Linea.Cells.Add(celdaActual);
            celdaActual = new TableCell(new Paragraph(new Run("Asunto")));
            celdaActual.FontWeight = FontWeights.Bold;
            celdaActual.FontSize = 11;
            celdaActual.FontFamily = new FontFamily(Constants.FONT_USAR);
            Linea.Cells.Add(celdaActual);
            celdaActual = new TableCell(new Paragraph(new Run("Emisor")));
            celdaActual.FontSize = 11;
            celdaActual.FontFamily = new FontFamily(Constants.FONT_USAR);
            celdaActual.FontWeight = FontWeights.Bold;
            Linea.Cells.Add(celdaActual);
            celdaActual = new TableCell(new Paragraph(new Run("Localización")));
            celdaActual.FontSize = 11;
            celdaActual.FontFamily = new FontFamily(Constants.FONT_USAR);
            celdaActual.FontWeight = FontWeights.Bold;
            Linea.Cells.Add(celdaActual);
            datosDocumento.RowGroups.Add(new TableRowGroup());
            datosDocumento.RowGroups[0].Rows.Add(Linea);

            foreach (VotoSimplificadoTO item in listado)
            {
                Linea = new TableRow();
                datosDocumento.RowGroups[0].Rows.Add(Linea);
                celdaActual = new TableCell(new Paragraph(new Run(item.Id)));
                celdaActual.FontFamily = new FontFamily(Constants.FONT_USAR);
                celdaActual.FontSize = 10;
                Linea.Cells.Add(celdaActual);
                celdaActual = new TableCell(new Paragraph(new Run(item.TpoAsunto)));
                celdaActual.FontFamily = new FontFamily(Constants.FONT_USAR);
                celdaActual.FontSize = 10;
                Linea.Cells.Add(celdaActual);
                celdaActual = new TableCell(new Paragraph(new Run(item.Complemento)));
                celdaActual.FontFamily = new FontFamily(Constants.FONT_USAR);
                celdaActual.FontSize = 10;
                Linea.Cells.Add(celdaActual);
                celdaActual = new TableCell(new Paragraph(new Run(item.Loc)));
                celdaActual.FontFamily = new FontFamily(Constants.FONT_USAR);
                celdaActual.FontSize = 10;
                Linea.Cells.Add(celdaActual);
            }
            documentoVariable.Blocks.Add(datosDocumento);
            return documentoVariable;
        }
    }
}
