using System;
using System.Linq;

namespace mx.gob.scjn.electoral.Controller
{
    public interface IEjecutoriaElectoral
    {
        void ActualizaRango(int inicial, int FinRango);

        void IniciaPagina();

        void GuardarClic();

        void PortapapelesClic();

        void FontMayorClic();

        void FontMenorClic();

        void MarcarTodoClic();

        void DesmarcarClic();

        void MarcarClic();

        void InicioListaClic();

        void AnteriorListaClic();

        void SiguienteListaClic();

        void UltimoListaClic();

        void RegNumLetra(System.Windows.Input.KeyEventArgs e);

        void IrClic();

        void SalirClic();

        void ImprimirClic();

        void DocumentoCompletoClic();

        void AnexosClic();

        void TesisClic();

        void VotoClic();

        void ParteInicioClic();

        void ParteAnteriorClic();

        void ParteSiguienteClic();

        void ParteFinalClic();

        void ImprimePapelClic();

        void TacheClic();

        void ContenidoTextoCopy(System.Windows.DataObjectCopyingEventArgs e);

        void TextoABuscarTecla(System.Windows.Input.KeyEventArgs e);

        void BuscarClic();

        void ContenidoTextoTecla();

        void TextoBuscarTecla(System.Windows.Controls.TextChangedEventArgs e);

        void TabControlChanged();
    }
}
