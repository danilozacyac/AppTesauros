using System;
using System.Linq;

namespace mx.gob.scjn.electoral.Controller
{
    public interface IVotoElectoralController
    {
        void GuardarClic();

        void PortaPapelesClic();

        void AumentaFontClic();

        void DisminuyeFontClic();

        void ImprimirClic();

        void TesisClic();

        void EjecutoriaClic();

        void ParteInicioClic();

        void ParteAnteriorClic();

        void ParteSiguienteClic();

        void ParteFinalClic();

        void RegNumTecla(System.Windows.Input.KeyEventArgs e);

        void IrClic();

        void SalirClic();

        void ImprimePapelClic();

        void TacheClic();

        void TextoBuscarTecla(System.Windows.Input.KeyEventArgs e);

        void TextoBuscarCambio(System.Windows.Controls.TextChangedEventArgs e);

        void BuscaClic();

        void DocumentoCompletoClic();

        void InicioListaClic();

        void AnteriorListaClic();

        void SiguienteListaClic();

        void UltimoListaClic();

        void ContenidoTextoTecla(System.Windows.Input.KeyEventArgs e);

        void ContenidoTextoCopia(object sender, System.Windows.DataObjectCopyingEventArgs e);

        void TabControlChanged();
    }
}
