using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using mx.gob.scjn.ius_common.gui.gui.impresion;
using mx.gob.scjn.ius_common.utils;

//using System.Windows.Xps.Packaging;
//using System.Windows.Xps.Serialization;
namespace mx.gob.scjn.ius_common.gui.impresion
{
    public class DocumentoThesauro
    {
        public FlowDocument Documento { get; set; }
        public DocumentoThesauro(List<TreeViewItem> datos,
            List<TreeViewItem> Sinonimos, List<TreeViewItem> Proximidad, String Tema)
        {
            generaDocumento(datos, Sinonimos, Proximidad, Tema);
        }
        public DocumentoThesauro(List<TreeViewItem> datos,
            List<TreeViewItem> Sinonimos, List<TreeViewItem> Proximidad, String Tema, FlowDocument documento)
        {
            generaDocumento(datos, Sinonimos, Proximidad, Tema, documento);
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
            Paragraph TextoTema = new Paragraph(new Run("Descriptores de: " + Tema));
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
                            TextoImprimir = (String)SegundoNivel.Header;
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
                            if (SegundoNivel.ItemsSource != null)
                            {
                                foreach (TreeViewItem TercerNivel in DocumentoActual.Items)
                                {
                                    TextoImprimir = (String)TercerNivel.Header;
                                    if (!(TextoImprimir.StartsWith("IA") || TextoImprimir.StartsWith("CE")))
                                    {
                                        TextoImprimir = "\t\t\t" + TextoImprimir;
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
                                    if (TercerNivel.ItemsSource != null)
                                    {
                                        foreach (TreeViewItem CuartoNivel in DocumentoActual.Items)
                                        {
                                            TextoImprimir = (String)CuartoNivel.Header;
                                            if (!(TextoImprimir.StartsWith("IA") || TextoImprimir.StartsWith("CE")))
                                            {
                                                TextoImprimir = "\t\t\t\t" + TextoImprimir;
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
                                }
                            }
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

        /// <summary>
        /// Genera un documento a partir de un listado de tesis.
        /// </summary>
        /// <param name="todosLosDocumentos">El listado de tesis para el documento.</param>
        private void generaDocumento(List<TreeViewItem> todosLosDocumentos,
            List<TreeViewItem> Sinonimos, List<TreeViewItem> Proximidad,
            String Tema, FlowDocument documentoVariable)
        {
            
            Paragraph TextoTema = new Paragraph(new Run("Descriptores de: " + Tema));
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
                            TextoImprimir = (String)SegundoNivel.Header;
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
