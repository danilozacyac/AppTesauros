using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using mx.gob.scjn.electoral_common.TO;
using mx.gob.scjn.ius_common.TO;
using mx.gob.scjn.ius_common.fachade;
using mx.gob.scjn.ius_common.gui.gui.impresion;
using mx.gob.scjn.ius_common.utils;

//using System.Windows.Xps.Packaging;
//using System.Windows.Xps.Serialization;
namespace mx.gob.scjn.electoral_common.gui.impresion
{
    public class DocumentoListadoEjecutoriaElectoral
    {
        public FlowDocument Documento { get; set; }
        public List<EjecutoriasTO> ListaImpresion;
        public static bool cancelado { get; set; }
        public DocumentoListadoEjecutoriaElectoral(ItemCollection listado, BackgroundWorker worker)
        {
           
            int contador = 0;
            float percent = 0;
            ListaImpresion = new List<EjecutoriasTO>();
            List<List<Int32>> ejecutorias = new List<List<Int32>>();
            List<Int32> ejecutoriaParcial = new List<int>();
            foreach (EjecutoriaSimplificadaElectoralTO item in listado)
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
#if STAND_ALONE
            FachadaBusquedaTradicional fachada = new FachadaBusquedaTradicional();
#else
            FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
#endif
            List<EjecutoriasTO> resultado = new List<EjecutoriasTO>();
            foreach (List<Int32> itemInt in ejecutorias)
            {
                if (worker.CancellationPending)
                {
                    return;
                }
                MostrarPorIusTO datos = new MostrarPorIusTO();
                datos.OrderBy = "Consec";
                datos.OrderType = "asc";
#if STAND_ALONE
                datos.Listado = itemInt;
                List<EjecutoriasTO> parciales = fachada.getEjecutoriasElectoralPorIds(datos);
#else
                datos.Listado = itemInt.ToArray();
                EjecutoriasTO[] parciales = fachada.getEjecutoriasElectoralPorIds(datos);
#endif
                contador++;
                percent = (float)((float)contador / ((float)listado.Count/100));
                worker.ReportProgress((int)(percent * 100));
                foreach (EjecutoriasTO itemEje in parciales)
                {
                    resultado.Add(itemEje);
                }
            }
            ListaImpresion = resultado;
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
            TableColumn columnaLoc = new TableColumn();
            columnaLoc.Width = new GridLength(96 * 1.5);
            TableColumn columnaId = new TableColumn();
            columnaId.Width = new GridLength(96 * 0.8);

            datosDocumento.Columns.Add(columnaAsunto);
            datosDocumento.Columns.Add(columnaLoc);
            datosDocumento.Columns.Add(columnaId);
            TableRow Linea = new TableRow();
            TableCell celdaActual = null;
            celdaActual = new TableCell(new Paragraph(new Run("Expediente")));
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
