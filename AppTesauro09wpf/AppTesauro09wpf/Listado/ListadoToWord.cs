using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using SeguridadTesauro.TO;
using mx.gob.scjn.ius_common.TO;
using mx.gob.scjn.ius_common.fachade;
using mx.gob.scjn.ius_common.gui.utils;
using Word = Microsoft.Office.Interop.Word;
using ScjnUtilities;

namespace AppTesauro09wpf.Listado
{
    public class ListadoToWord
    {
        readonly string filepath = Path.GetTempFileName() + ".docx";

        int fila = 1;

        private List<ListadoCertificacion> listaRelaciones = new List<ListadoCertificacion>();
        private List<ListadoCertificacion> listaEliminadas = new List<ListadoCertificacion>();
        Word.Application oWord;
        Word.Document oDoc;
        object oMissing = System.Reflection.Missing.Value;
        object oEndOfDoc = "\\endofdoc";
        readonly MateriaTO materia;

        Microsoft.Office.Interop.Word.Table oTable;

        public ListadoToWord(MateriaTO materia)
        {
            this.materia = materia;
        }

        public void GeneraWord()
        {
            oWord = new Microsoft.Office.Interop.Word.Application();
            oDoc = oWord.Documents.Add(ref oMissing, ref oMissing, ref oMissing, ref oMissing);
            oDoc.PageSetup.Orientation = Word.WdOrientation.wdOrientLandscape;

            //Insert a paragraph at the beginning of the document.
            Microsoft.Office.Interop.Word.Paragraph oPara1;
            oPara1 = oDoc.Content.Paragraphs.Add(ref oMissing);
            oPara1.Range.Text = "Tesis relacionadas a los temas del proyecto Tematico IUS";
            oPara1.Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
            oPara1.Range.Font.Bold = 1;
            oPara1.Format.SpaceAfter = 24;    //24 pt spacing after paragraph.
            oPara1.Range.InsertParagraphAfter();

            Microsoft.Office.Interop.Word.Range wrdRng = oDoc.Bookmarks.get_Item(ref oEndOfDoc).Range;

            Word.Paragraph oPara2 = oDoc.Content.Paragraphs.Add(ref oMissing);
            oPara2.Range.Text = materia.Descripcion;
            oPara2.Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphLeft;
            oPara2.Range.Font.Bold = 1;
            oPara2.Format.SpaceAfter = 24;    //24 pt spacing after paragraph.
            oPara2.Range.InsertParagraphAfter();

            this.listaRelaciones = new ListadoCertificacionViewModel().GetTemasTesisPorUsuario(1, materia.Id);
            this.listaEliminadas = new ListadoCertificacionViewModel().GetTemasTesisPorUsuario(4, materia.Id);

            foreach (ListadoCertificacion item in listaEliminadas)
            {
                if (listaRelaciones.Contains(item))
                    listaRelaciones.Remove(item);
            }

            Microsoft.Office.Interop.Word.Table oTable = oDoc.Tables.Add(wrdRng, (listaRelaciones.Count + 1), 4, ref oMissing, ref oMissing);
            oTable.Range.ParagraphFormat.SpaceAfter = 6;
            oTable.Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphJustify;
            oTable.Range.Font.Size = 10;
            oTable.Range.Font.Bold = 1;

            oTable.Columns[1].SetWidth(40, Word.WdRulerStyle.wdAdjustSameWidth);
            oTable.Columns[2].SetWidth(70, Word.WdRulerStyle.wdAdjustSameWidth);
            oTable.Columns[3].SetWidth(300, Word.WdRulerStyle.wdAdjustSameWidth);
            oTable.Columns[4].SetWidth(150, Word.WdRulerStyle.wdAdjustSameWidth);

            oTable.Cell(fila, 1).Range.Text = "#";
            oTable.Cell(fila, 2).Range.Text = "IUS";
            oTable.Cell(fila, 3).Range.Text = "Rubro";
            oTable.Cell(fila, 4).Range.Text = "Tema";

            for (int x = 1; x < 5; x++)
            {
                oTable.Cell(fila, x).Borders.Enable = 1;
            }

            oTable.Range.Font.Size = 9;
            oTable.Range.Font.Bold = 0;

            fila++;
            ImprimeDocumento(oTable);

            oTable = null;
            
            try
            {
                //ImprimeDocumento();
                foreach (Word.Section wordSection in oDoc.Sections)
                {
                    object pagealign = Word.WdPageNumberAlignment.wdAlignPageNumberRight;
                    object firstpage = true;
                    wordSection.Footers[Word.WdHeaderFooterIndex.wdHeaderFooterPrimary].PageNumbers.Add(ref pagealign, ref firstpage);
                }

                oWord.ActiveDocument.SaveAs(filepath);
                oWord.ActiveDocument.Saved = true;
            }
            catch (Exception)
            {
            }
            finally
            {
                oWord.Visible = true;
                
                oDoc.Close();

                Process.Start(filepath);
            }
        }

        private void ImprimeDocumento(Microsoft.Office.Interop.Word.Table oTable)
        {
            List<ListadoCertificacion> completas = new List<ListadoCertificacion>();

            foreach (ListadoCertificacion elemento in listaRelaciones)
            {
                ListadoCertificacion nuevo = elemento;
                FachadaBusquedaTradicional fac = new FachadaBusquedaTradicional();
                TesisTO tesis = fac.getTesisPorRegistro(elemento.Ius.ToString());
                nuevo.Rubro = tesis.Rubro;

                completas.Add(nuevo);
            }

            completas.Sort(delegate(ListadoCertificacion p1, ListadoCertificacion p2)
            {
                return StringUtilities.QuitaCarOrden(p1.Rubro).CompareTo(StringUtilities.QuitaCarOrden(p2.Rubro));
            });

            foreach (ListadoCertificacion elemento in completas)
            {
                oTable.Cell(fila, 1).Range.Text = (fila - 1).ToString();
                oTable.Cell(fila, 2).Range.Text = elemento.Ius.ToString();

                FachadaBusquedaTradicional fac = new FachadaBusquedaTradicional();
                TesisTO tesis = fac.getTesisPorRegistro(elemento.Ius.ToString());

                oTable.Cell(fila, 3).Range.Text = tesis.Rubro;
                oTable.Cell(fila, 4).Range.Text = elemento.Tema;

                for (int x = 1; x < 5; x++)
                {
                    oTable.Cell(fila, x).Borders.Enable = 1;
                }

                fila++;
            }
        }
    }
}