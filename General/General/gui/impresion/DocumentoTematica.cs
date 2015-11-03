using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using mx.gob.scjn.ius_common.gui.gui.impresion;
using mx.gob.scjn.ius_common.utils;

//using System.Windows.Xps.Packaging;
//using System.Windows.Xps.Serialization;
namespace mx.gob.scjn.ius_common.gui.impresion
{
    public class DocumentoTematica
    {
        public FlowDocument Documento { get; set; }
        public DocumentoTematica(List<TreeViewItem> datos, String[] Tema)
        {
            generaDocumento(datos, Tema);
        }
        /// <summary>
        /// Genera un documento a partir de un listado de tesis.
        /// </summary>
        /// <param name="todosLosDocumentos">El listado de tesis para el documento.</param>
        private void generaDocumento(List<TreeViewItem> todosLosDocumentos, String[] Tema)
        {
            IUSPaginatedDocument documentoVariable = new IUSPaginatedDocument();
            Paragraph EncabezadoTema = new Paragraph(new Run("Materia: "+Tema[2]));
            EncabezadoTema.FontSize = 14;
            EncabezadoTema.FontWeight = FontWeights.Bold;
            EncabezadoTema.FontStyle = FontStyles.Italic;
            documentoVariable.Blocks.Add(EncabezadoTema);
            if ((Tema[1] != null) && (!Tema[1].Equals(Constants.CADENA_VACIA)))
            {
                EncabezadoTema = new Paragraph(new Run("Submateria: "+Tema[1]));
                EncabezadoTema.FontSize = 14;
                EncabezadoTema.FontWeight = FontWeights.Bold;
                EncabezadoTema.FontStyle = FontStyles.Italic;
                documentoVariable.Blocks.Add(EncabezadoTema);
            }
            if ((!Tema[0].Equals(Tema[1])) && (!Tema[0].Equals(Tema[2])))
            {
                Paragraph TextoTema = new Paragraph(new Run(Tema[0]));
                TextoTema.FontSize = 14;
                TextoTema.FontWeight = FontWeights.Bold;
                TextoTema.FontStyle = FontStyles.Italic;
                documentoVariable.Blocks.Add(TextoTema);
            }
            else
            {
                documentoVariable.Blocks.Add(new Paragraph(new Run("")));
            }
            foreach (TreeViewItem DocumentoActual in todosLosDocumentos)
            {
                Paragraph TextoItem = new Paragraph(new Run((String) DocumentoActual.Header));
                TextoItem.FontSize = 12;
                documentoVariable.Blocks.Add(TextoItem);
            }
            Documento = documentoVariable;// doc.GetFixedDocumentSequence();
        }
    }
}
