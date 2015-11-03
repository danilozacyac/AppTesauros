using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using mx.gob.scjn.ius_common.TO;
using mx.gob.scjn.ius_common.fachade;

namespace mx.gob.scjn.ius_common.gui.noticia
{
    /// <summary>
    /// Interaction logic for FramesResultados.xaml
    /// </summary>
    public partial class FramesResultados : Page
    {
        public Page Back { get; set; }
        public FramesResultados()
        {
            InitializeComponent();
        }
        public FramesResultados(int cual)
        {
            InitializeComponent();
#if STAND_ALONE
            FachadaBusquedaTradicional fachada = new FachadaBusquedaTradicional();
#else
            FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
#endif
            EpocaMagistradoTO parametro = new EpocaMagistradoTO();
            parametro.Id = ""+cual;
            parametro.Epoca = parametro.Id.Substring(0, 1);
            parametro.Sala = parametro.Id.Substring(1, 1);
            if (parametro.Sala.Equals("6"))
            {
                parametro.Sala = "7";
            }
            this.Periodos.ItemsSource = fachada.getFechasMagistrados(parametro);
            fachada.Close();
            this.Periodos.SelectedIndex = 0;
            Periodos_MouseDoubleClick(null, null);
        }

        private void Periodos_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
#if STAND_ALONE
            FachadaBusquedaTradicional fachada = new FachadaBusquedaTradicional();
#else
            FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
#endif
            EpocaMagistradoTO seleccionado = (EpocaMagistradoTO) this.Periodos.SelectedItem;
            this.Funcionarios.ItemsSource=fachada.getFuncionarios(seleccionado.Id);
            fachada.Close();
        }

        private void Salir_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            if (this.Back == null)
            {
                this.NavigationService.GoBack();
            }
            else
            {
                this.NavigationService.Navigate(Back);
            }
        }

        private void Periodos_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (Periodos.SelectedItem != null)
            {
                Periodos_MouseDoubleClick(sender, null);
            }
        }
    }
}
