using System;
using System.Linq;

namespace mx.gob.scjn.electoral.Controller
{
    public interface IAcuerdosController
    {
        void GuardarClic();

        void PortapapelesClic();

        void FontMenorClic();

        void FontMayorClic();

        void InicioListaClic();

        void AnteriorListaClic();

        void SiguienteListaClic();

        void UltimoListaClic();

        void RegNumTecla(System.Windows.Input.KeyEventArgs e);

        void ImprimirClic();

        void DocumentoCompletoClic();

        void ParteInicioClic();

        void ParteAnteriorClic();

        void ParteSiguienteClic();

        void ParteFinalClic();

        void IrClic();

        void TextoBuscarCambio();

        void TextoBuscarTecla(System.Windows.Input.KeyEventArgs e);

        void BuscarClic();

        void ImprimePapelClic();

        void TacheClic();

        void SalirClic();

        void ContenidoTextoTecla(System.Windows.Input.KeyEventArgs e);

        void ContenidoTextoCopia(object sender, System.Windows.DataObjectCopyingEventArgs e);

        void TabControlChanged();
    }
}
