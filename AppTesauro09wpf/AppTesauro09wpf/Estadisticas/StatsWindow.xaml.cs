using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Login.Utils;

namespace AppTesauro09wpf.Estadisticas
{
    /// <summary>
    /// Interaction logic for StatsWindow.xaml
    /// </summary>
    public partial class StatsWindow : Window
    {
        private int actualIndex = 1;
        private int userIndex = 0;
        private EstadisticaViewModel stat;
        private List<EstadisticaViewModel> generales;
        
        public StatsWindow()
        {
            InitializeComponent();
            stat = new EstadisticaMainViewModel().GetStatsPorUsuario();
        }

        public StatsWindow(List<EstadisticaViewModel> generales)
        {
            InitializeComponent();
            this.generales = generales;
            stat = generales[userIndex];
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (UserStatus.IdActivo == 210 || UserStatus.IdActivo == 50 || UserStatus.IdActivo == 300)
            {
                ChiefPanel.Visibility = System.Windows.Visibility.Visible;
                //generales = new EstadisticaMainViewModel().GetStatsGenerales();
                //stat = generales[userIndex];
                LblUsuario.Content = stat.Usuario;
            }
            else
            {
                ChiefPanel.Visibility = System.Windows.Visibility.Collapsed;
                //stat = new EstadisticaMainViewModel().GetStatsPorUsuario();
            }
            lblElemento.Content = stat.Estadisticas[actualIndex].Elemento;
            GDia.DataContext = stat.Estadisticas[actualIndex];
            GTotal.DataContext = stat.Totales[actualIndex];

            BtnAnterior.Content = stat.Estadisticas[actualIndex - 1].Elemento;
            BtnSiguiente.Content = stat.Estadisticas[actualIndex + 1].Elemento;

            //StatBarGraph graph = new StatBarGraph();
            //graph.Show();
        }

        private void BtnAnterior_Click(object sender, RoutedEventArgs e)
        {
            if (actualIndex > 0)
            {
                actualIndex = actualIndex - 1;
                lblElemento.Content = stat.Estadisticas[actualIndex].Elemento;
                GDia.DataContext = stat.Estadisticas[actualIndex];
                GTotal.DataContext = stat.Totales[actualIndex];

                if (actualIndex == 0)
                    BtnAnterior.Content = stat.Estadisticas[stat.Estadisticas.Count - 1].Elemento;
                else
                    BtnAnterior.Content = stat.Estadisticas[actualIndex - 1].Elemento;

                if (actualIndex == stat.Estadisticas.Count - 1)
                    BtnSiguiente.Content = stat.Estadisticas[0].Elemento;
                else
                    BtnSiguiente.Content = stat.Estadisticas[actualIndex + 1].Elemento;
            }
            else
            {
                actualIndex = stat.Estadisticas.Count - 1;
                lblElemento.Content = stat.Estadisticas[actualIndex].Elemento;
                GDia.DataContext = stat.Estadisticas[actualIndex];
                GTotal.DataContext = stat.Totales[actualIndex];

                if (actualIndex == 0)
                    BtnAnterior.Content = stat.Estadisticas[stat.Estadisticas.Count - 1].Elemento;
                else
                    BtnAnterior.Content = stat.Estadisticas[actualIndex - 1].Elemento;

                if (actualIndex == stat.Estadisticas.Count - 1)
                    BtnSiguiente.Content = stat.Estadisticas[0].Elemento;
                else
                    BtnSiguiente.Content = stat.Estadisticas[actualIndex + 1].Elemento;
            }
        }

        private void BtnSiguiente_Click(object sender, RoutedEventArgs e)
        {
            if (actualIndex < stat.Estadisticas.Count - 1)
            {
                actualIndex = actualIndex + 1;
                lblElemento.Content = stat.Estadisticas[actualIndex].Elemento;
                GDia.DataContext = stat.Estadisticas[actualIndex];
                GTotal.DataContext = stat.Totales[actualIndex];

                if (actualIndex == 0)
                    BtnAnterior.Content = stat.Estadisticas[stat.Estadisticas.Count - 1].Elemento;
                else
                    BtnAnterior.Content = stat.Estadisticas[actualIndex - 1].Elemento;

                if (actualIndex == stat.Estadisticas.Count - 1)
                    BtnSiguiente.Content = stat.Estadisticas[0].Elemento;
                else
                    BtnSiguiente.Content = stat.Estadisticas[actualIndex + 1].Elemento;
            }
            else
            {
                actualIndex = 0;
                lblElemento.Content = stat.Estadisticas[actualIndex].Elemento;
                GDia.DataContext = stat.Estadisticas[actualIndex];
                GTotal.DataContext = stat.Totales[actualIndex];

                if (actualIndex == 0)
                    BtnAnterior.Content = stat.Estadisticas[stat.Estadisticas.Count - 1].Elemento;
                else
                    BtnAnterior.Content = stat.Estadisticas[actualIndex - 1].Elemento;

                if (actualIndex == stat.Estadisticas.Count - 1)
                    BtnSiguiente.Content = stat.Estadisticas[0].Elemento;
                else
                    BtnSiguiente.Content = stat.Estadisticas[actualIndex + 1].Elemento;
            }
        }

        private void BtnSalir_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnBackUser_Click(object sender, RoutedEventArgs e)
        {
            if (userIndex > 0)
            {
                stat = generales[userIndex - 1];
                userIndex -= 1;
                LblUsuario.Content = stat.Usuario;
            }
            else
            {
                stat = generales[generales.Count - 1];
                userIndex = generales.Count - 1;
                LblUsuario.Content = stat.Usuario;
            }

            actualIndex = 1;
            lblElemento.Content = stat.Estadisticas[actualIndex].Elemento;
            GDia.DataContext = stat.Estadisticas[actualIndex];
            GTotal.DataContext = stat.Totales[actualIndex];

            BtnAnterior.Content = stat.Estadisticas[actualIndex - 1].Elemento;
            BtnSiguiente.Content = stat.Estadisticas[actualIndex + 1].Elemento;
        }

        private void BtnFowardUser_Click(object sender, RoutedEventArgs e)
        {
            if (userIndex < generales.Count - 1)
            {
                stat = generales[userIndex + 1];
                userIndex += 1;
                LblUsuario.Content = stat.Usuario;
            }
            else
            {
                stat = generales[0];
                userIndex = 0;
                LblUsuario.Content = stat.Usuario;
            }

            actualIndex = 1;
            lblElemento.Content = stat.Estadisticas[actualIndex].Elemento;
            GDia.DataContext = stat.Estadisticas[actualIndex];
            GTotal.DataContext = stat.Totales[actualIndex];

            BtnAnterior.Content = stat.Estadisticas[actualIndex - 1].Elemento;
            BtnSiguiente.Content = stat.Estadisticas[actualIndex + 1].Elemento;
        }
    }
}
