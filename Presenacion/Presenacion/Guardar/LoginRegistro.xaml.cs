using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using mx.gob.scjn.ius_common.TO;
using mx.gob.scjn.ius_common.fachade;
using mx.gob.scjn.ius_common.gui.utils;
using mx.gob.scjn.ius_common.utils;

namespace mx.gob.scjn.ius_common.gui.Guardar
{
    /// <summary>
    /// Interaction logic for LoginRegistro.xaml
    /// </summary>
    public partial class LoginRegistro : Page
    {
        public Page Back { get; set; }
        public LoginRegistro()
        {
            InitializeComponent();
        }

        private void BtnAceptarPwd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
#if STAND_ALONE
                FachadaBusquedaTradicional fachada = new FachadaBusquedaTradicional();
#else
                FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
#endif
                bool LoginCorrecto = fachada.VerificaUsuario(this.TbxUsuario.Text, this.PsbPassword.Password);
                if (LoginCorrecto)
                {
                    SeguridadUsuariosTO.UsuarioActual = fachada.ObtenDatosUsuario(this.TbxUsuario.Text);
                    this.NavigationService.Navigate(Back);
                }
                else
                {
                    MessageBox.Show(Mensajes.MENSAJE_USUARIO_PASS_EQUIVOCADO, Mensajes.TITULO_USUARIO_PASS_EQUIVOCADO, MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
                fachada.Close();
            }
            catch (Exception exc)
            {
                MessageBox.Show(Mensajes.MENSAJE_PROBLEMAS_FACHADA+ exc.Message, Mensajes.TITULO_PROBLEMAS_FACHADA,
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnAceptarReg_Click(object sender, RoutedEventArgs e)
        {
            //String campoFaltante = "";
            UsuarioTO usuarioARegistrar = new UsuarioTO();
#if STAND_ALONE
                FachadaBusquedaTradicional fachada = new FachadaBusquedaTradicional();
#else
                FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
#endif
            int resultado = 0;
            if (TbxApellidos.Text.Trim().Equals("")||TbxCorreo.Text.Trim().Equals("")||
                TbxNombre.Text.Equals("")||TbxUsuarioReg.Text.Equals("")||PsbPasswdReg.Password.Equals(""))
            {
                MessageBox.Show(Mensajes.MENSAJE_CAMPO_TEXTO_REGISTRO_VACIO, Mensajes.TITULO_ADVERTENCIA,
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (!PsbPasswdReg.Password.Equals(PsbPasswdRegConf.Password))
            {
                MessageBox.Show(Mensajes.MENSAJE_NO_COINCIDEN_CONTRASENAS, Mensajes.TITULO_NO_COINCIDEN_CONTRASENAS,
                    MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            if (PsbPasswdReg.Password.Trim().Length<5)
            {
                MessageBox.Show(Mensajes.MENSAJE_PASSWORD_VACIO, Mensajes.TITULO_PASSWORD_VACIO,
                    MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            usuarioARegistrar.Usuario = TbxUsuarioReg.Text;
            usuarioARegistrar.Apellidos = TbxApellidos.Text;
            usuarioARegistrar.Enviar = (bool)CbxEnvio.IsChecked;
            usuarioARegistrar.Nombre = TbxNombre.Text;
            usuarioARegistrar.Passwd = PsbPasswdReg.Password;
            usuarioARegistrar.Correo = TbxCorreo.Text;
            try
            {
                resultado = fachada.RegistrarUsuario(usuarioARegistrar);
            }
            catch (Exception exc)
            {
                MessageBox.Show(Mensajes.MENSAJE_PROBLEMAS_FACHADA + ": \n" + exc.Message, Mensajes.TITULO_PROBLEMAS_FACHADA,
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            fachada.Close();
            switch (resultado)
            {
                case Constants.USUARIO_EXISTENTE:
                    MessageBox.Show(Mensajes.MENSAJE_USUARIO_EXISTENTE, Mensajes.TITULO_USUARIO_EXISTENTE,
                        MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    TbxUsuario.Focus();
                    TbxUsuario.SelectAll();
                    break;
                case Constants.NO_ERROR:
                    MessageBox.Show(Mensajes.MENSAJE_USUARIO_ALTA_EXITOSA, Mensajes.TITULO_USUARIO_ALTA_EXITOSA,
                        MessageBoxButton.OK, MessageBoxImage.Information);
                    SeguridadUsuariosTO.UsuarioActual = usuarioARegistrar;
                    if (Back != null)
                    {
                        this.NavigationService.Navigate(Back);
                    }
                    else
                    {
                        this.NavigationService.GoBack();
                    }
                    break;
                default:
                    break;
            }
        }

        private void Regresar_Click(object sender, RoutedEventArgs e)
        {
            if (Back != null)
            {
                this.NavigationService.Navigate(Back);
            }
            else
            {
                this.NavigationService.GoBack();
            }
        }

        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.StkRegistrarse.Visibility = Visibility.Visible;
            this.StkIngreso.Visibility = Visibility.Collapsed;
            Titulo.Content = "Registro de usuario";
        }

        private void TblRecuperelo_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.StkRecuperar.Visibility = Visibility.Visible;
            this.StkIngreso.Visibility = Visibility.Collapsed;
            Titulo.Content = "Recuperación de contraseña";

        }

        private void BtnRecuperar_Click(object sender, RoutedEventArgs e)
        {
            Cursor = Cursors.Wait;
            if (CalculosGlobales.VerificaCorreo(TbxCorreoRecuperar.Text))
            {
#if STAND_ALONE
                FachadaBusquedaTradicional fachada = new FachadaBusquedaTradicional();
#else
                FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
#endif
                fachada.RecuperaUsuario(TbxCorreoRecuperar.Text);
                TblMensaje.Visibility = Visibility.Visible;
                StkDatosRecuperar.Visibility = Visibility.Collapsed;
            }
            Cursor = Cursors.Arrow;
        }
    }
}
