using System;
using System.Linq;

namespace mx.gob.scjn.electoral.Controller
{
    public interface ITablaResultadoEjecutoriaController
    {
        String OrdenarPor { get; set; }

        void PaginaCargada();

        void InicioClic();

        void AnteriorClic();

        void SiguienteClic();

        void FinalClic();

        void OrdenaClic();

        void IrAClic();

        void ImprimirClic();

        void GuardarClic();

        void TablaResultadoDobleClic();

        void SalirClic();

        void TablaResultadoCambio();

        void ImprimePapelClic();

        void TacheClic();

        void CancelarClic();
    }
}
