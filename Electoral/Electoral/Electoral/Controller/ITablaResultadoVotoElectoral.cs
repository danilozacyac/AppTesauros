using System;
using System.Linq;

namespace mx.gob.scjn.electoral.Controller
{
    public interface ITablaResultadoVotoElectoral
    {
        void InicioClic();

        void AnteriorClic();

        void SiguienteClic();

        void FinalClic();

        void IrTecla(System.Windows.Input.KeyEventArgs e);

        void IrClic();

        void ImprimirClic();

        void SalirClic();

        void GuardarClic();

        void VisualizarClic();

        void TacheClic();

        void ImprimePapelClic();

        void CambioRegistro();
    }
}
