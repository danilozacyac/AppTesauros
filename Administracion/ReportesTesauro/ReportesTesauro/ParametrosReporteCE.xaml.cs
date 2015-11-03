using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SqlClient;
using Microsoft.Win32;
using System.IO;

namespace ReportesTesauro
{
    /// <summary>
    /// Interaction logic for ParametrosReporteCE.xaml
    /// </summary>
    public partial class ParametrosReporteCE : UserControl
    {
        public ParametrosReporteCE()
        {
            InitializeComponent();
        }

        private void btnGeneraReporteCE_Click(object sender, RoutedEventArgs e)
        {
            String sqlMateria = "";
            if (!(((ComboBoxItem)CbxMateria.SelectedItem)).Tag.Equals("0"))
            {
                sqlMateria = " AND A.Materia = "+((ComboBoxItem)CbxMateria.SelectedItem).Tag;
            }
            String sqlString1 = "select 'CE '+ A.Descripcion as CE, B.Descripcion, A.idtema as materia" +
                                " from Temas As A, Materias as b" +
                                " where a.IDPadre=0 "+sqlMateria+" and A.Materia=B.IdMateria" +
                                " group by a.idtema, b.Descripcion, A.Descripcion";
            SqlConnection conn = new SqlConnection("server=CT9BD1;uid=4cc3s01nf0;pwd=Pr0gr4m4d0r3s;database=Tesauro");
            conn.Open();
            SqlCommand com = new SqlCommand(sqlString1, conn);
            SqlDataReader reader = com.ExecuteReader();
            FileDialog fd = new SaveFileDialog();
            fd.AddExtension = true;
            Int32 tipoArchivo = Int32.Parse((String)((ComboBoxItem)CbxTipoReporte.SelectedItem).Tag);
            String Separador = String.Empty;
            if (tipoArchivo == 1)
            {
                fd.Filter = "Texto separado por comas | *.csv";
                Separador = ",";
            }
            else
            {
                fd.Filter = "Texto separado por tabuladores | *.txt";
                Separador = "\t";
            }
            fd.ShowDialog();
            StreamWriter fs = new StreamWriter(fd.FileName,false);
            while (reader.Read())
            {
                String CE = reader.GetString(0);
                Int32 Materia = reader.GetInt32(2);
                String MateriaStr = reader.GetString(1);

                if ((bool)CbxNumeroHijos.IsChecked)
                {
                    String sqlString2 = "select Count(*) "+
                               " from Temas " +
                               " where IDPadre= " + Materia ;
                    SqlConnection conn2 = new SqlConnection("server=CT9BD1;uid=4cc3s01nf0;pwd=Pr0gr4m4d0r3s;database=Tesauro");
                    conn2.Open();
                    SqlCommand com2 = new SqlCommand(sqlString2, conn2);
                    SqlDataReader reader2 = com2.ExecuteReader();
                    reader2.Read();
                    String strTemp = "["+reader2.GetInt32(0)+"]";
                    CE += strTemp;
                    conn2.Close();
                }
                String Escribir = CE + Separador + MateriaStr;
                fs.WriteLine(Escribir);
            }
            fs.Close();
            conn.Close();
        }
    }
}
