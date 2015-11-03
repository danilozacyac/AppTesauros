using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Login.Utils;
using TesauroMiddleTier;
using TesauroTO;
using TesauroUtilities;

namespace AppTesauro09wpf.Observaciones
{
    /// <summary>
    /// Interaction logic for Observaciones.xaml
    /// </summary>
    public partial class VentanaObservaciones : Window
    {
        /// <summary>
        /// El tema que se va a observar
        /// </summary>
        public TemaTO TemaActual { get; set; }
        /// <summary>
        /// El resultado que dió el usuario.
        /// </summary>
        public int Resultado { get; set; }

        public VentanaObservaciones()
        {
            InitializeComponent();
        }

        public VentanaObservaciones(TemaTO tema)
        {
            InitializeComponent();
            TemaActual = tema;
            ObtenDatos();
            if ((TemaActual.Status == Constants.STATUS_NUEVO) || (TemaActual.Status == Constants.STATUS_ATENDIDO))
            {
                BtnAtender.Visibility = Visibility.Collapsed;
            }
            if (TemaActual.Status == Constants.STATUS_OBSERVADO)
            {
                BtnAceptar.Visibility = Visibility.Collapsed;
                BtnRevisar.Visibility = Visibility.Collapsed;
            }
            if (TemaActual.Status == Constants.STATUS_ACEPTADO)
            {
                TbxObservaciones.Visibility = Visibility.Collapsed;
                Botones.Visibility = Visibility.Collapsed;
                this.Height = 350;
            }
        }

        private void ObtenDatos()
        {
            IFachadaTesauro fac = new FachadaTesauro();
            List<ObservacionTO> obs = fac.ObtenObservaciones(TemaActual.IDTema);
            DgdHistorial.ItemsSource = obs;
        }
        private void CambiarStatus(object sender, RoutedEventArgs e)
        {
            if (sender == BtnAceptar)
            {
                TemaActual.Status = Constants.STATUS_ACEPTADO;
                IFachadaTesauro fac = new FachadaTesauro();
                fac.ActualizaTema(TemaActual);
                Resultado = Constants.ACEPTAR;
                ObservacionTO obs = new ObservacionTO();
                obs.Hora = DateTime.Now;
                obs.Id = -1;
                obs.IdTema = TemaActual.IDTema;
                obs.Texto = TbxObservaciones.Text;
                obs.Tipo = Constants.ACEPTAR;
                obs.UserId = UserStatus.IdActivo;
                obs.Usuario = String.Empty;
                fac.InsertaObservacion(obs);
                this.Close();
            }
            else if (sender == BtnAtender)
            {
                TemaActual.Status = Constants.STATUS_ATENDIDO;
                IFachadaTesauro fac = new FachadaTesauro();
                fac.ActualizaTema(TemaActual);
                Resultado = Constants.ATENDER;
                ObservacionTO obs = new ObservacionTO();
                obs.Hora = DateTime.Now;
                obs.Id = -1;
                obs.IdTema = TemaActual.IDTema;
                obs.Texto = TbxObservaciones.Text;
                obs.Tipo = Constants.ATENDER;
                obs.UserId = UserStatus.IdActivo;
                obs.Usuario = String.Empty;
                fac.InsertaObservacion(obs);
                this.Close();
            }
            else if (sender == BtnRevisar)
            {
                TemaActual.Status = Constants.STATUS_OBSERVADO;
                IFachadaTesauro fac = new FachadaTesauro();
                fac.ActualizaTema(TemaActual);
                Resultado = Constants.REVISAR;
                ObservacionTO obs = new ObservacionTO();
                obs.Hora = DateTime.Now;
                obs.Id = -1;
                obs.IdTema = TemaActual.IDTema;
                obs.Texto = TbxObservaciones.Text;
                obs.Tipo = Constants.REVISAR;
                obs.UserId = UserStatus.IdActivo;
                obs.Usuario = String.Empty;
                fac.InsertaObservacion(obs);
                this.Close();
            }
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void DgdHistorial_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            ObservacionTO obs = (ObservacionTO)DgdHistorial.SelectedItem;
            if (obs != null)
            {
                TblHistorial.Text = obs.Texto;
                LblFecha.Content = obs.Hora;
                LblUsuario.Content = obs.Usuario;
            }
        }
    }
}
