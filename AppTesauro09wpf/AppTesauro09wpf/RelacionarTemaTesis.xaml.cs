using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using TesauroMiddleTier;
using TesauroTO;
using mx.gob.scjn.ius_common.TO;
using mx.gob.scjn.ius_common.fachade;
using Login.Utils;

namespace AppTesauro09wpf
{
    /// <summary>
    /// Interaction logic for RelacionarTemaTesis.xaml
    /// </summary>
    public partial class RelacionarTemaTesis : Window
    {
        private readonly TemaTO tema;
        private int sumaResta = 0;

        public RelacionarTemaTesis(TemaTO tema)
        {
            InitializeComponent();
            this.tema = tema;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            FachadaTesauro fac = new FachadaTesauro();
            LstRegistros.ItemsSource = fac.ObtenTesisRelacionadasPorTema(tema);

            BtnQuitar.IsEnabled = (LstRegistros.Items.Count > 0) ? true : false;
            BtnTodos.IsEnabled = (LstRegistros.Items.Count > 0) ? true : false;
        }

        private void BtnAgregar_Click(object sender, RoutedEventArgs e)
        {
            FachadaBusquedaTradicional fac = new FachadaBusquedaTradicional();
                    
            if (TxtRegIus.Text.Equals(String.Empty))
                return;

            int regNuevo = Convert.ToInt32(TxtRegIus.Text);

            IEnumerable<int> items = (IEnumerable<int>)LstRegistros.ItemsSource;

            if (items.Contains(regNuevo))
            {
                MessageBox.Show("El número de registro que intenta ingresar ya se encuentra relacionado con el tema actual");
                return;
            }

            try
            {
                TesisTO tesis = fac.getTesisPorRegistro(TxtRegIus.Text);
                fac.Close();
                if ((tesis != null) && (tesis.Ius != null))
                {
                    FachadaTesauro tes = new FachadaTesauro();
                    tes.NuevaRelacionTemaTesis(tema, tesis);
                    tes.SetBitacora(tema.IDTema, 2, 1, UserStatus.IdActivo, " ", tesis.Ius.ToString(), tema.Materia);

                    LstRegistros.ItemsSource = tes.ObtenTesisRelacionadasPorTema(tema);
                    BtnTodos.IsEnabled = true;
                    TxtRegIus.Text = String.Empty;
                    tema.TesisRelacionadas += 1;
                }
                else
                {
                    MessageBox.Show("Ingrese un número de registro válido");
                }
            }
            catch (SqlException sql)
            {
                if (sql.Message.Contains("PRIMARY"))
                    MessageBox.Show("El número de registro que intenta ingresar ya se encuentra relacionado con el tema actual");
                else
                    MessageBox.Show("Error ({0}) : {1}" + sql.Source + sql.Message, "Error Interno");
            }
        }

        private void BtnQuitar_Click(object sender, RoutedEventArgs e)
        {
            if (LstRegistros.SelectedItem == null)
            {
                MessageBox.Show("Debes seleccionar el registro que deseas eliminar");
            }
            else
            {
                FachadaTesauro tes = new FachadaTesauro();
                tes.RemueveRelacionTemaTesis(tema, Convert.ToInt32(LstRegistros.SelectedItem));
                tes.SetBitacora(tema.IDTema, 2, 4, UserStatus.IdActivo, LstRegistros.SelectedItem.ToString(), " ", tema.Materia);

                LstRegistros.ItemsSource = tes.ObtenTesisRelacionadasPorTema(tema);
                tema.TesisRelacionadas += -1;
            }
        }

        private void BtnTodos_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("¿Estás seguro de eliminar todas las relaciones existentes?", "Atención:",
                MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                FachadaTesauro tes = new FachadaTesauro();
                tes.RemueveRelacionTemaTesis(tema);

                LstRegistros.ItemsSource = new List<int>();
            }
        }

        private void TxtRegIus_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = IsTextAllowed(e.Text);
        }

        private static bool IsTextAllowed(string text)
        {
            Regex regex = new Regex("[^0-9.-]+");  
            return regex.IsMatch(text);
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void LstRegistros_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            BtnQuitar.IsEnabled = true;
        }
    }
}