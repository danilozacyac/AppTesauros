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
using System.Windows.Controls;
using mx.gob.scjn.ius_common.utils;

namespace mx.gob.scjn.ius_common.gui.impresion
{
    public class DocumentoThesauro
    {
        public FlowDocument Documento { get; set; }
        public DocumentoThesauro(List<TreeViewItem> datos,
            List<TreeViewItem> Sinonimos, List<TreeViewItem> Proximidad, String Tema)
        {
            generaDocumento(datos, Sinonimos,Proximidad, Tema);
        }
        /// <summary>
        /// Genera un documento a partir de un listado de tesis.
        /// </summary>
        /// <param name="todosLosDocumentos">El listado de tesis para el documento.</param>
        private void generaDocumento(List<TreeViewItem> todosLosDocumentos,
            List<TreeViewItem> Sinonimos, List<TreeViewItem> Proximidad,
            String Tema)
        {
            IUSPaginatedDocument documentoVariable = new IUSPaginatedDocument();
            Paragraph TextoTema = new Paragraph(new Run("Descriptores de: "+Tema));
            TextoTema.FontSize = 14;
            TextoTema.FontFamily = new FontFamily(Constants.FONT_USAR);
            TextoTema.FontWeight = FontWeights.Bold;
            TextoTema.FontStyle = FontStyles.Italic;
            documentoVariable.Blocks.Add(TextoTema);
            TextoTema = new Paragraph(new Run("Estructura"));
            TextoTema.FontSize = 12;
            TextoTema.FontFamily = new FontFamily(Constants.FONT_USAR);
            documentoVariable.Blocks.Add(TextoTema);
            bool CEImpreso = false;
            foreach (TreeViewItem DocumentoActual in todosLosDocumentos)
            {
                String TextoImprimir = (String)DocumentoActual.Header;
                if (!(CEImpreso && TextoImprimir.StartsWith("CE")))
                {
                    if (!(TextoImprimir.StartsWith("IA") || TextoImprimir.StartsWith("CE")))
                    {
                        TextoImprimir = "\t" + TextoImprimir;
                    }
                    Paragraph TextoItem = new Paragraph(new Run(TextoImprimir));
                    TextoItem.FontSize = 10;
                    TextoItem.FontFamily = new FontFamily(Constants.FONT_USAR);
                    if (DocumentoActual.IsSelected)
                    {
                        TextoItem.FontWeight = FontWeights.Bold;
                        TextoItem.FontStyle = FontStyles.Italic;
                    }
                    documentoVariable.Blocks.Add(TextoItem);
                    if (DocumentoActual.ItemsSource != null)
                    {
                        foreach (TreeViewItem SegundoNivel in DocumentoActual.Items)
                        {
                            TextoImprimir = (String)DocumentoActual.Header;
                            if (!(TextoImprimir.StartsWith("IA") || TextoImprimir.StartsWith("CE")))
                            {
                                TextoImprimir = "\t\t" + TextoImprimir;
                            }
                            TextoItem = new Paragraph(new Run(TextoImprimir));
                            TextoItem.FontSize = 10;
                            TextoItem.FontFamily = new FontFamily(Constants.FONT_USAR);
                            if (DocumentoActual.IsSelected)
                            {
                                TextoItem.FontWeight = FontWeights.Bold;
                                TextoItem.FontStyle = FontStyles.Italic;
                            }
                            documentoVariable.Blocks.Add(TextoItem);
                        }
                    }
                    CEImpreso = TextoImprimir.StartsWith("CE");
                }
            }
            TextoTema = new Paragraph(new Run("Sinonimos"));
            TextoTema.FontSize = 12;
            TextoTema.FontFamily = new FontFamily(Constants.FONT_USAR);
            documentoVariable.Blocks.Add(TextoTema);
            foreach (TreeViewItem DocumentoActual in Sinonimos)
            {
                Paragraph TextoItem = new Paragraph(new Run((String)DocumentoActual.Header));
                TextoItem.FontSize = 10;
                TextoItem.FontFamily = new FontFamily(Constants.FONT_USAR);
                if (DocumentoActual.IsSelected)
                {
                    TextoItem.FontWeight = FontWeights.Bold;
                    TextoItem.FontStyle = FontStyles.Italic;
                }
                documentoVariable.Blocks.Add(TextoItem);
            }
            TextoTema = new Paragraph(new Run("Relación Próxima"));
            TextoTema.FontSize = 12;
            TextoTema.FontFamily = new FontFamily(Constants.FONT_USAR);
            documentoVariable.Blocks.Add(TextoTema);
            foreach (TreeViewItem DocumentoActual in Proximidad)
            {
                Paragraph TextoItem = new Paragraph(new Run((String)DocumentoActual.Header));
                TextoItem.FontSize = 10;
                TextoItem.FontFamily = new FontFamily(Constants.FONT_USAR);
                if (DocumentoActual.IsSelected)
                {
                    TextoItem.FontWeight = FontWeights.Bold;
                    TextoItem.FontStyle = FontStyles.Italic;
                }
                documentoVariable.Blocks.Add(TextoItem);
            }

            Documento = documentoVariable;// doc.GetFixedDocumentSequence();
        }
    }
}
