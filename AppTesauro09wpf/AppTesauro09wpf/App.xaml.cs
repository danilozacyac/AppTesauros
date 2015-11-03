using System;
using System.Windows;
using Login;
using Login.Utils;
using Microsoft.Win32;
using mx.gob.scjn.ius_common.utils;
using System.Configuration;

namespace AppTesauro09wpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void inicio(object sender, StartupEventArgs e)
        {
            Xceed.Wpf.DataGrid.Licenser.LicenseKey = "DGP20-PR3LZ-9Y30W-FW2A";
            DialogoUsuario usuario = new DialogoUsuario();
            RegistryKey reg = Registry.LocalMachine.OpenSubKey("Software\\SCJN\\Tesauro\\General");
            if (reg == null)
            {
                reg = Registry.LocalMachine.CreateSubKey("Software\\SCJN\\Tesauro\\General");
                reg.SetValue("SQL", "mxctdb1.scjn.pjf.gob.mx");
            }
            reg.Close();
            reg = Registry.LocalMachine.OpenSubKey(IUSConstants.IUS_REGISTRY_ENTRY);
            if (reg == null)
            {
                reg = Registry.LocalMachine.CreateSubKey(IUSConstants.IUS_REGISTRY_ENTRY);
                
                reg.SetValue("IUS_DATABASE_SQL", ConfigurationManager.ConnectionStrings["IusProduccion"].ToString());
                reg.SetValue("DIRECCION_INDEXER", ConfigurationManager.AppSettings["DIRECCION_INDEXER"].ToString());
                reg.SetValue("BLOQUE_PAGINADOR", ConfigurationManager.AppSettings["BLOQUE_PAGINADOR"].ToString());
                reg.SetValue("PAGINADOR_BD", ConfigurationManager.AppSettings["PAGINADOR_BD"].ToString());
                reg.SetValue("RUTAANEXOS", ConfigurationManager.AppSettings["RUTAANEXOS"].ToString());
                reg.SetValue("USUARIOS_DATABASE_ACCESS", Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\BusquedasAlmacenadas.mdb");
            }
            reg.Close();
            
            WMain inicio = new WMain();
            this.MainWindow = inicio;

            if (ConfigurationManager.AppSettings.Get("tipoAplicacion").Equals("PRUEBA"))
            {
                MessageBox.Show(ConfigurationManager.AppSettings.Get("MensajeAppPrueba"));
            }

            usuario.ShowDialog();
            if (UserStatus.IdActivo != -1)
            {
                inicio.Controlador.AplicaSeguridad();
                inicio.Controlador.LoadTemas();
                inicio.Show();
            }
            else
            {
                inicio.Close();
            }
        }
    }
}