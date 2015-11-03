using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls.DataVisualization.Charting;

namespace AppTesauro09wpf.Estadisticas
{
    /// <summary>
    /// Interaction logic for StatBarGraph.xaml
    /// </summary>
    public partial class StatBarGraph : Window
    {
        List<EstadisticaViewModel> stats;
        List<KeyValuePair<String, int>> pieData;
        
        public StatBarGraph()
        {
            InitializeComponent();
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            stats = new EstadisticaMainViewModel().GetStatsGenerales();
            
            mcChart.Title = "Número de tesis ingresadas por abogado" + "          " + DateTime.Now.ToShortDateString();//.ToString();

            List<KeyValuePair<String, int>> tesisAgregadas = new List<KeyValuePair<string, int>>();

            foreach (EstadisticaViewModel stat in stats)
            {
                tesisAgregadas.Add(new KeyValuePair<String, int>(stat.Usuario, stat.Totales[1].Agregados));

            }

            ((BarSeries)mcChart.Series[0]).ItemsSource = tesisAgregadas;

            List<KeyValuePair<String, int>> tesisElim = new List<KeyValuePair<string, int>>();

            foreach (EstadisticaViewModel stat in stats)
            {
                tesisElim.Add(new KeyValuePair<String, int>(stat.Usuario, stat.Totales[1].Eliminados));

            }

            ((BarSeries)mcChart.Series[1]).ItemsSource = tesisElim;
            
        }

        private void BtnVerDetalle_Click(object sender, RoutedEventArgs e)
        {
            StatsWindow statsWin = new StatsWindow(stats);
            statsWin.Show();
        }

        private void BtnTesisPorMateria_Click(object sender, RoutedEventArgs e)
        {
            pieData = new EstadisticaMainViewModel().GetTesisPorMateria();
            ((PieSeries)labeledPieChart.Series[0]).ItemsSource = pieData;

            labeledPieChart.Title = "Total de tesis ingresadas por materia" + "          " + DateTime.Now.ToShortDateString();

            labeledPieChart.Height = SystemParameters.FullPrimaryScreenHeight - 30;// System.Windows.SystemParameters.PrimaryScreenHeight
            pieGrid.Visibility = Visibility.Visible;
            barGrid.Visibility = Visibility.Collapsed;
            
        }

        private void BtnTesisAbogado_Click(object sender, RoutedEventArgs e)
        {
            pieGrid.Visibility = Visibility.Collapsed;
            barGrid.Visibility = Visibility.Visible;
        }
    }
}
