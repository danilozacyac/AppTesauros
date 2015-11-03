using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using AppTesauro09wpf.Verificar;
using Microsoft.Win32;
using TesauroMiddleTier;
using Excel = Microsoft.Office.Interop.Excel;

namespace AppTesauro09wpf
{
    /// <summary>
    /// Interaction logic for TesisNoIngresadas.xaml
    /// </summary>
    public partial class TesisNoIngresadas : Window
    {
        private String filePath = "";

        public TesisNoIngresadas()
        {
            InitializeComponent();
        }

        private delegate void UpdateProgressBarDelegate(
        System.Windows.DependencyProperty dp, Object value);

        private void BtnVerificar_Click(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            System.Array myvalues = null;

            LstNoIngresadas.Items.Clear();
            int desde = Convert.ToInt32(TxtDesde.Text);
            int hasta = Convert.ToInt32(TxtHasta.Text);

            int totalRevisa = hasta - desde;

            //if ((hasta - desde) > 5000)
            //{
            //    MessageBox.Show("Seleccione un rango menor a 5000 registros");
            //    return;
            //}
             
            // Show open file dialog box

            if (!String.IsNullOrEmpty(filePath))
            {
                Excel.Application xlApp;
                Excel.Workbook xlWorkBook;
                Excel.Worksheet xlWorkSheet;
                Excel.Range range;

                //xlApp = new Excel.ApplicationClass();
                xlApp = new Excel.Application();
                xlWorkBook = xlApp.Workbooks.Open(filePath, 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
                xlWorkSheet = (Excel.Worksheet)xlWorkBook.Sheets[1];

                range = xlWorkSheet.get_Range(TxtColumna.Text + TxtDesde.Text, TxtColumna.Text + TxtHasta.Text);
                myvalues = (System.Array)range.Cells.Value;

                xlWorkBook.Close(false, null, null);
                xlApp.Quit();

                ReleaseObject(xlWorkSheet);
                ReleaseObject(xlWorkBook);
                ReleaseObject(xlApp);
            }

            int materiaSelect = Convert.ToInt32(((ComboBoxItem)CbxMaterias.SelectedItem).Uid);

            Progress.Minimum = 0;
            Progress.Maximum = totalRevisa;
            Progress.Value = 0;

            double value = 0;

            Progress.Visibility = Visibility.Visible;

            UpdateProgressBarDelegate updatePbDelegate =
                new UpdateProgressBarDelegate(Progress.SetValue);

            FachadaTesauro fac = new FachadaTesauro();

            for (int i = 1; i <= myvalues.Length; i++)
            {
                if (myvalues.GetValue(i, 1) == null)
                {
                }
                else
                {
                    int rowCount = fac.VerificaExistenciaRelacion((string)myvalues.GetValue(i, 1).ToString(), materiaSelect);

                    if (rowCount == 0)
                        LstNoIngresadas.Items.Add((string)myvalues.GetValue(i, 1).ToString());
                }

                value += 1;

                Dispatcher.Invoke(updatePbDelegate,
                   System.Windows.Threading.DispatcherPriority.Background,
                   new object[] { ProgressBar.ValueProperty, value });
            }
            Progress.Visibility = Visibility.Collapsed;
            Cursor = Cursors.Arrow;
            LblTotal.Content = "Total de tesis no ingresadas:   " + LstNoIngresadas.Items.Count;
        }

        string[] ConvertToStringArray(System.Array values)
        {
            // create a new string array
            string[] theArray = new string[values.Length];

            // loop through the 2-D System.Array and populate the 1-D String Array
            for (int i = 1; i <= values.Length; i++)
            {
                if (values.GetValue(i, 1) == null)
                    theArray[i - 1] = "";
                else
                    theArray[i - 1] = (string)values.GetValue(i, 1).ToString();
            }

            return theArray;
        }

        /// <summary>
        /// Obtiene la ruta del documento que se va a utilizar 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSelect_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.Filter = "Excel Files|*.xls;*.xlsx";
            // Show open file dialog box
            bool? result = openFileDialog1.ShowDialog();

            if (result == true)
            {
                filePath = openFileDialog1.FileName;
                TxtFilePath.Text = filePath;
            }
            else
            {
                filePath = String.Empty;
                TxtFilePath.Text = filePath;
            }
        }

        private void BtnExportarListado_Click(object sender, RoutedEventArgs e)
        {
            Excel.Application xlApp;
            Excel.Workbook xlWorkBook;
            Excel.Worksheet xlWorkSheet;
            object misValue = System.Reflection.Missing.Value;

            xlApp = new Excel.Application();
            xlWorkBook = xlApp.Workbooks.Add(misValue);

            xlWorkSheet = (Excel.Worksheet)xlWorkBook.Sheets[1];

            int row = 1;
            foreach (var item in LstNoIngresadas.Items)
            {
                xlWorkSheet.Cells[row, 1] = item.ToString();
                row++;
            }
            String tempName = Path.GetTempFileName() + ".xls";

            xlWorkBook.SaveAs(tempName, Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
            xlWorkBook.Close(true, misValue, misValue);
            xlApp.Quit();

            ReleaseObject(xlWorkSheet);
            ReleaseObject(xlWorkBook);
            ReleaseObject(xlApp);

            Process.Start(tempName);
            //MessageBox.Show("Excel file created , you can find the file " + tempName);
        }

       

        private void ReleaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                obj = null;
                MessageBox.Show("Unable to release the Object " + ex.ToString());
            }
            finally
            {
                GC.Collect();
            }
        }

        private void BtnNuevoLimpio_Click(object sender, RoutedEventArgs e)
        {
            List<DatosExcel> tesisFaltantes = new List<DatosExcel>();

            this.Cursor = Cursors.Wait;
            System.Array myvalues = null;

            if (!String.IsNullOrEmpty(filePath))
            {
                Excel.Application xlApp;
                Excel.Workbook xlWorkBook;
                Excel.Worksheet xlWorkSheet;
                Excel.Range range;

                xlApp = new Excel.Application();
                xlWorkBook = xlApp.Workbooks.Open(filePath, 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
                xlWorkSheet = (Excel.Worksheet)xlWorkBook.Sheets[1];// .get_Item(0);

                range = xlWorkSheet.get_Range("A15401", "H42898");
                myvalues = (System.Array)range.Cells.Value;

                xlWorkBook.Close(false, null, null);
                xlApp.Quit();

                ReleaseObject(xlWorkSheet);
                ReleaseObject(xlWorkBook);
                ReleaseObject(xlApp);
            }

            int materiaSelect = Convert.ToInt32(((ComboBoxItem)CbxMaterias.SelectedItem).Uid);

            FachadaTesauro fac = new FachadaTesauro();

            for (int i = 1; i < 27499; i++)
            {
                if (myvalues.GetValue(i, 1) == null)
                {
                }
                else
                {
                    int rowCount = fac.VerificaExistenciaRelacion((string)myvalues.GetValue(i, 1).ToString(), materiaSelect);

                    if (rowCount == 0)
                    {
                        DatosExcel datos = new DatosExcel();
                        datos.Ius = myvalues.GetValue(i, 1).ToString();
                        datos.Rubro = myvalues.GetValue(i, 2).ToString();
                        datos.Tesis = myvalues.GetValue(i, 3) == null ? "" : myvalues.GetValue(i, 3).ToString();
                        datos.Localizacion = myvalues.GetValue(i, 4).ToString();
                        datos.Materia1 = myvalues.GetValue(i, 5).ToString();
                        datos.Materia2 = myvalues.GetValue(i, 6).ToString();
                        datos.Materia3 = myvalues.GetValue(i, 7).ToString();
                        datos.Descripcion = myvalues.GetValue(i, 8).ToString();

                        tesisFaltantes.Add(datos);
                    }
                }
            }

            this.CreateNewList(tesisFaltantes);

            Cursor = Cursors.Arrow;
        }

        private void CreateNewList(List<DatosExcel> listaTesis)
        {
            Excel.Application xlApp;
            Excel.Workbook xlWorkBook;
            Excel.Worksheet xlWorkSheet;
            object misValue = System.Reflection.Missing.Value;

            xlApp = new Excel.Application();
            xlWorkBook = xlApp.Workbooks.Add(misValue);

            xlWorkSheet = (Excel.Worksheet)xlWorkBook.Sheets[1];

            int row = 1;
            foreach (DatosExcel item in listaTesis)
            {
                xlWorkSheet.Cells[row, 1] = item.Ius.ToString();
                xlWorkSheet.Cells[row, 2] = item.Rubro.ToString();
                xlWorkSheet.Cells[row, 3] = item.Tesis.ToString();
                xlWorkSheet.Cells[row, 4] = item.Localizacion.ToString();
                xlWorkSheet.Cells[row, 5] = item.Materia1.ToString();
                xlWorkSheet.Cells[row, 6] = item.Materia2.ToString();
                xlWorkSheet.Cells[row, 7] = item.Materia3.ToString();
                xlWorkSheet.Cells[row, 8] = item.Descripcion.ToString();
                row++;
            }
            String tempName = Path.GetTempFileName() + ".xls";

            xlWorkBook.SaveAs(tempName, Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
            xlWorkBook.Close(true, misValue, misValue);
            xlApp.Quit();

            ReleaseObject(xlWorkSheet);
            ReleaseObject(xlWorkBook);
            ReleaseObject(xlApp);

            Process.Start(tempName);
            //MessageBox.Show("Excel file created , you can find the file " + tempName);
        }
    }
}