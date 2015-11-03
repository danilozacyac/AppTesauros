using System;
using System.Linq;
using System.Windows;
using Login.Utils;
using SeguridadTesauro.Fachada;

namespace Login
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class DialogoUsuario : Window
    {
        public DialogoUsuario()
        {
            InitializeComponent();
            UserStatus.IdActivo = -1;
            TbxUsuario.Focus();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            FachadaST fachada = new FachadaST();
            bool correcto = fachada.ValidaUsuario(TbxUsuario.Text, TbxPass.Password);
            if (correcto)
            {
                UserStatus.IdActivo = fachada.ObtenIdUsuario(TbxUsuario.Text);
                UserStatus.RolActivo = fachada.ObtenRol(TbxUsuario.Text);
                UserStatus.MateriasUser = fachada.ObtenMaterias(UserStatus.IdActivo);
                this.Close();
            }
            else
            {
                MessageBox.Show("Usuario o contraseña incorrectos, por favor intente de nuevo", "Seguridad",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                TbxUsuario.Focus();
                UserStatus.IdActivo = -1;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
