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
using SeguridadTesauro;
using SeguridadTesauro.TO;

namespace AdministracionTesauro.Usuarios
{
    /// <summary>
    /// Interaction logic for Usuarios.xaml
    /// </summary>
    public partial class UsuariosPanel : UserControl
    {
        public UsuariosPanel()
        {
            InitializeComponent();
            FachadaST fachada = new FachadaST();
            List<ComboBoxItem> listado = new List<ComboBoxItem>();
            List<PerfilTO> Periles = fachada.ObtenPerfiles();
            foreach (PerfilTO item in Periles)
            {
                ComboBoxItem itemCbx = new ComboBoxItem();
                itemCbx.Content = item.Descripcion;
                itemCbx.Tag = item;
                listado.Add(itemCbx);
            }
            CbxPerfilUsuario.ItemsSource = listado;
            CbxPerfilUsuario.SelectedIndex = 0;
            listado = new List<ComboBoxItem>();           
                ComboBoxItem itemCbx1 = new ComboBoxItem();
                itemCbx1.Content = "Generador";
                itemCbx1.Tag = 1;
                listado.Add(itemCbx1);
                ComboBoxItem itemCbx2 = new ComboBoxItem();
                itemCbx2.Content = "Revisor";
                itemCbx2.Tag = 2;
                listado.Add(itemCbx2);
                CbxRolUsuario.ItemsSource = listado;
                listado = new List<ComboBoxItem>();
                CbxRolUsuario.SelectedIndex = 0;


            List<UsuarioTO> Usuarios = fachada.ObtenTodosUsuarios();

            foreach (UsuarioTO item in Usuarios)
            {
                ComboBoxItem itemCbx = new ComboBoxItem();
                itemCbx.Content = item.UserName;
                itemCbx.Tag = item;
                listado.Add(itemCbx);
            }
            CbxNombreUsuario.ItemsSource = listado;
            CbxNombreUsuario.SelectedIndex = 0;

        }
    }
}
