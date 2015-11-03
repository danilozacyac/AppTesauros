using System;
using System.Linq;

namespace mx.gob.scjn.electoral.Controller
{
    public interface ITablaResultadosAcuerdosController
    {
        void InicioClic();

        void AnteriorClic();

        void SiguienteClic();

        void FinalClic();

        void GuardarClic();

        void OrdenarPor();

        void IrTecla(System.Windows.Input.KeyEventArgs e);

        void IrClic();

        void ImprimirClic();

        void VisualizarClic();

        void SalirClic();

        void TablaResultadoPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e);

        void ImprimePapelClic();

        void TacheClic();
    }
}
