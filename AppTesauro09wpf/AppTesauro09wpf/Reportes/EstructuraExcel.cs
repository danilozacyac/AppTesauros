using System;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.Office.Interop.Excel;
using TesauroTO;
using Excel = Microsoft.Office.Interop.Excel;

namespace AppTesauro09wpf.Reportes
{
    public class EstructuraExcel
    {


        private readonly Excel.Application app = new Excel.Application();
        Excel.Worksheet hoja;

        /// <summary>
        /// Indica hasta que nivel se generará el reporte
        /// </summary>
        private readonly int nivelDetalle = 0;
        private readonly TemaTO materiaDetalle;

        int fila = 3;

        public EstructuraExcel(int nivelDetalle, TemaTO materiaDetalle)
        {
            this.nivelDetalle = nivelDetalle;
            this.materiaDetalle = materiaDetalle;
        }

        public void ExportaExcel()
        {
            Workbook libro = app.Workbooks.Add();
            hoja = (Microsoft.Office.Interop.Excel.Worksheet)libro.Worksheets[1];
            app.Visible = true;
            PoneNodosPdf();

            //SaveFileDialog cuadroDialogo = new SaveFileDialog();
            //cuadroDialogo.DefaultExt = "xlsx";
            //cuadroDialogo.Filter = "xlsx file(*.xlsx)|*.xlsx";
            //cuadroDialogo.AddExtension = true;
            //cuadroDialogo.RestoreDirectory = true;
            //cuadroDialogo.Title = "Guardar";
            //cuadroDialogo.InitialDirectory = @"C:\DOCS\";

            //if (cuadroDialogo.ShowDialog() == DialogResult.OK)
            //{
            app.ActiveWorkbook.SaveCopyAs(@"C:\Users\lavega\Downloads\" + materiaDetalle.Descripcion + ".xlsx");
            app.ActiveWorkbook.Saved = true;
            app.Visible = true;
            app.ActiveWorkbook.Close();
            
            //}
            ///Hasta Aqui
            //try
            //{
            //    Process pr = new Process();
            //    pr.StartInfo.FileName = rutaExcel;
            //    pr.Start();
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show("Error ({0}) : {1}" + ex.Source + ex.Message, "Error Interno");
            //}
        }

        private void PoneNodosPdf()
        {

            materiaDetalle.IsExpanded = true;

            hoja.Cells[fila, 1] = materiaDetalle.IDTema;
            //hoja.Cells[fila, 2] = tema.Padre;
            hoja.Cells[fila, 2] = materiaDetalle.Descripcion;


            fila++;
            PoneNodosHijos(materiaDetalle.SubTemas, 3);
            fila += 2;


        }

        private void PoneNodosHijos(ObservableCollection<TemaTO> listaTemas, int columnaPadre)
        {
            foreach (TemaTO tema in listaTemas)
            {
                hoja.Cells[fila, columnaPadre] = tema.IDTema;
                //hoja.Cells[fila, columnaPadre + ] = tema.Padre;
                hoja.Cells[fila, columnaPadre + 1] = tema.Descripcion;


                if (tema.Nivel + 1 <= nivelDetalle)
                {
                    tema.IsExpanded = true;

                    fila++;
                    PoneNodosHijos(tema.SubTemas, columnaPadre + 1);
                    //fila +=  1;
                }
                else
                    fila = fila + 1;
            }
        }



    }
}
