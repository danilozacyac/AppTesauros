using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using iTextSharp.text;
using iTextSharp.text.pdf;
using TesauroTO;
using System.Collections.ObjectModel;

namespace AppTesauro09wpf.Listado.PDF
{
    public class TreeSchemaToPdf
    {
        private iTextSharp.text.Document myDocument;

        private ObservableCollection<TemaTO> temasMateria;

        public TreeSchemaToPdf(ObservableCollection<TemaTO> temasMateria)
        {
            this.temasMateria = temasMateria;
        }

        public void GeneraPDF()
        {

            myDocument = new iTextSharp.text.Document(PageSize.A4, 50, 50, 50, 50);

            string documento = Path.GetTempFileName() + ".PDF";// @"C:\IUSThesa\TesauroIUS" + TematicoConst.number + ".PDF";

            try
            {
                PdfWriter writer = PdfWriter.GetInstance(myDocument, new FileStream(documento, FileMode.Create));
                myDocument.Open();

                iTextSharp.text.Paragraph head = new iTextSharp.text.Paragraph("Temático IUS ");
                head.Alignment = Element.ALIGN_CENTER;
                myDocument.Add(head);

                this.PoneNodosPdf(temasMateria, 0);
               
                myDocument.Close();
                System.Diagnostics.Process.Start(documento);
            }
            catch (DocumentException de)
            {
                Console.Error.WriteLine(de.Message);
            }
            catch (IOException ioe)
            {
                Console.Error.WriteLine(ioe.Message);
                MessageBox.Show("Antes de continuar cierre el reporte que tiene abierto", "ERROR:", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }

        }


        private void PoneNodosPdf(ObservableCollection<TemaTO> temas,int indent)
        {
            foreach (TemaTO tema in temas)
            {
                iTextSharp.text.Paragraph par;
                par = new iTextSharp.text.Paragraph(tema.Descripcion);
                par.IndentationLeft = indent;
                myDocument.Add(par);

                if (tema.SubTemas.Count > 0)
                    this.PoneNodosPdf(tema.SubTemas, indent + 15);

            }
        }


    }
}
