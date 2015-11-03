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
using mx.gob.scjn.ius_common.fachade;

namespace mx.gob.scjn.ius_common.gui.impresion
{
    public class DocumentoListadoEjecutoria
    {
        public FlowDocument Documento { get; set; }
        public List<EjecutoriasTO> ListaImpresion;
        public static bool cancelado { get; set; }
        public DocumentoListadoEjecutoria(ItemCollection listado, BackgroundWorker worker)
        {
           
            int contador = 0;
            float percent = 0;
            ListaImpresion = new List<EjecutoriasTO>();
            List<List<Int32>> ejecutorias = new List<List<Int32>>();
            List<Int32> ejecutoriaParcial = new List<int>();
            foreach (EjecutoriasSimplificadaTO item in listado)
            {
                if((contador%100)==99){
                    ejecutorias.Add(ejecutoriaParcial);
                    ejecutoriaParcial=new List<int>();
                }
                contador++;
                ejecutoriaParcial.Add(Int32.Parse(item.Id));
                //EjecutoriasTO itemVerdadero = new EjecutoriasTO();
                
                //itemVerdadero.Rubro = item.Rubro;
                //itemVerdadero.Id = item.Id;
                //itemVerdadero.Promovente = item.Promovente;
                //itemVerdadero.Loc = item.Loc;
                //ListaImpresion.Add(itemVerdadero);
            }
            ejecutorias.Add(ejecutoriaParcial);
            contador = 0;
            FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
            List<EjecutoriasTO> resultado = new List<EjecutoriasTO>();
            foreach (List<Int32> itemInt in ejecutorias)
            {
                if (worker.CancellationPending)
                {
                    return;
                }
                MostrarPorIusTO datos = new MostrarPorIusTO();
                datos.Listado = itemInt.ToArray();
                datos.OrderBy = "Consec";
                datos.OrderType = "asc";
                EjecutoriasTO[] parciales = fachada.getEjecutoriasPorIds(datos);
                contador++;
                percent = (float)((float)contador / ((float)listado.Count/100));
                worker.ReportProgress((int)(percent * 100));
                foreach (EjecutoriasTO itemEje in parciales)
                {
                    resultado.Add(itemEje);
                }
            }
            ListaImpresion = resultado;
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

        public static FlowDocument generaDocumento(List<EjecutoriasTO> listado)
        {
            cancelado = false;
            FlowDocument documentoVariable = new IUSPaginatedDocument();
            documentoVariable.FontFamily = new FontFamily("Arial");
            documentoVariable.FontSize = 10;
            documentoVariable.ColumnWidth = 96 * 7.5;
            documentoVariable.PageHeight = 96 * 11;
            documentoVariable.PageWidth = 96 * 8.5;
            documentoVariable.PagePadding = new Thickness(96 * 0.3, 96 * .5, 96 * 0.3, 96 * 0.5); 
            documentoVariable.ColumnWidth = 96 * 7.3;
            documentoVariable.PagePadding = new Thickness(96 * 0.3, 96 * .5, 96 * 0.3, 96 * 0.5);
            Table datosDocumento = new Table();
            datosDocumento.CellSpacing = 20;
            TableColumn columnaAsunto = new TableColumn();
            columnaAsunto.Width = new GridLength(96 * 2.7);
            TableColumn columnaPromovente = new TableColumn();
            columnaPromovente.Width = new GridLength(96 * 2);
            TableColumn columnaLoc = new TableColumn();
            columnaLoc.Width = new GridLength(96 * 1.5);
            TableColumn columnaId = new TableColumn();
            columnaId.Width = new GridLength(96 * 0.8);

            datosDocumento.Columns.Add(columnaAsunto);
            datosDocumento.Columns.Add(columnaPromovente);
            datosDocumento.Columns.Add(columnaLoc);
            datosDocumento.Columns.Add(columnaId);
            TableRow Linea = new TableRow();
            TableCell celdaActual = null;
            celdaActual = new TableCell(new Paragraph(new Run("Asunto")));
            celdaActual.FontSize = 11;
            celdaActual.FontFamily = new FontFamily(Constants.FONT_USAR);
            celdaActual.FontWeight = FontWeights.Bold;
            Linea.Cells.Add(celdaActual);
            celdaActual = new TableCell(new Paragraph(new Run("Promovente")));
            celdaActual.FontSize = 11;
            celdaActual.FontFamily = new FontFamily(Constants.FONT_USAR);
            celdaActual.FontWeight = FontWeights.Bold;
            Linea.Cells.Add(celdaActual);
            celdaActual = new TableCell(new Paragraph(new Run("Localización")));
            celdaActual.FontSize = 11;
            celdaActual.FontFamily = new FontFamily(Constants.FONT_USAR);
            celdaActual.FontWeight = FontWeights.Bold;
            Linea.Cells.Add(celdaActual);
            celdaActual = new TableCell(new Paragraph(new Run("Registro")));
            celdaActual.FontSize = 11;
            celdaActual.FontFamily = new FontFamily(Constants.FONT_USAR);
            celdaActual.FontWeight = FontWeights.Bold;
            Linea.Cells.Add(celdaActual);
            datosDocumento.RowGroups.Add(new TableRowGroup());
            datosDocumento.RowGroups[0].Rows.Add(Linea);

            foreach (EjecutoriasTO item in listado)
            {
                if (cancelado)
                {
                    return null;
                }
                System.Windows.Forms.Application.DoEvents();
                Linea = new TableRow();
                datosDocumento.RowGroups[0].Rows.Add(Linea);
                celdaActual = new TableCell(new Paragraph(new Run(item.TpoAsunto)));
                celdaActual.FontFamily = new FontFamily(Constants.FONT_USAR);
                celdaActual.FontSize = 10;
                Linea.Cells.Add(celdaActual);
                celdaActual = new TableCell(new Paragraph(new Run(item.Promovente)));
                celdaActual.FontSize = 10;
                celdaActual.FontFamily = new FontFamily(Constants.FONT_USAR);
                Linea.Cells.Add(celdaActual);
                celdaActual = new TableCell(new Paragraph(new Run(item.Loc)));
                celdaActual.FontSize = 10;
                celdaActual.FontFamily = new FontFamily(Constants.FONT_USAR);
                Linea.Cells.Add(celdaActual);
                celdaActual = new TableCell(new Paragraph(new Run(item.Id)));
                celdaActual.FontSize = 10;
                celdaActual.FontFamily = new FontFamily(Constants.FONT_USAR);
                Linea.Cells.Add(celdaActual);
            }
            documentoVariable.Blocks.Add(datosDocumento);
            return documentoVariable;
        }
    }
}
