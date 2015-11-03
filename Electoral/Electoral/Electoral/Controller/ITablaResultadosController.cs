using System;
using System.ComponentModel;
using System.Linq;

namespace mx.gob.scjn.electoral.Controller
{
    public interface ITablaResultadosController
    {
        /// <summary>
        /// Thread para realizar algunas búsquedas y generar los documentos de imrpesion
        /// </summary>
        BackgroundWorker worker { get; set; }

        TablaResultados Ventana { get; set; }

        void Habilita();

        void AnteriorClic();

        void SiguienteClic();

        void FinalClic();

        void ImprimirClic();

        void GuardarClic();

        void SalirClic();

        void CambioSeleccion();

        void TablaDobleClic();

        void ImprimePapelClic();

        void BtnTacheClic();

        void InicioClic();

        void IrAClic();
    }
}
