using System;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace mx.gob.scjn.directorio
{

    public class DocumentoAcuerdoDirec
    {

        public FlowDocument Documento { get; set; }
        public FlowDocument Copia { get; set; }
        public DocumentoAcuerdoDirec(String strDocImpr)
        {
            generaDocumento(strDocImpr);
        }

        private void generaDocumento(String strDocImpr)
        {
            FlowDocument documentoVariable = new IUSPaginatedDocumentDirec();
            FlowDocument documentoCopia = new FlowDocument();
#if STAND_ALONE
#else
            FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
#endif

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
            Table datosCopia = new Table();
            TableColumn columnaInicial = new TableColumn();
            TableColumn columnaInicialCopia = new TableColumn();
            columnaInicial.Width = new GridLength(96 * 1);
            columnaInicialCopia.Width = new GridLength(96 * 1);
            TableColumn columnaAncha = new TableColumn();
            TableColumn columnaAnchaCopia = new TableColumn();
            columnaAncha.Width = new GridLength(96 * 3);
            columnaAnchaCopia.Width = new GridLength(96 * 3);
            TableColumn columnaDatos = new TableColumn();
            TableColumn columnaDatosCopia = new TableColumn();
            columnaDatos.Width = new GridLength(96 * 1);
            columnaDatosCopia.Width = new GridLength(96 * 1);
            TableColumn columnaFinal = new TableColumn();
            TableColumn columnaFinalCopia = new TableColumn();
            columnaFinal.Width = new GridLength(96 * 1);
            columnaFinalCopia.Width = new GridLength();
            datosDocumento.Columns.Add(columnaInicial);
            datosCopia.Columns.Add(columnaInicialCopia);
            datosDocumento.Columns.Add(columnaAncha);
            datosCopia.Columns.Add(columnaAnchaCopia);
            datosDocumento.Columns.Add(columnaDatos);
            datosCopia.Columns.Add(columnaDatosCopia);
            datosDocumento.Columns.Add(columnaFinal);
            datosCopia.Columns.Add(columnaFinalCopia);
            Paragraph parrafo = new Paragraph(new Run(strDocImpr));
            parrafo = new Paragraph(new Run(strDocImpr));
            parrafo.FontWeight = FontWeights.Normal;

            documentoVariable.Blocks.Add(parrafo);

            Documento = documentoVariable;
            Copia = documentoCopia;
        }
    }
}
