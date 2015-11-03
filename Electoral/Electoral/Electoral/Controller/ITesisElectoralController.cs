using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using mx.gob.scjn.ius_common.TO;

namespace mx.gob.scjn.electoral.Controller
{
    public interface ITesisElectoralController
    {
        TesisElectoral Ventana { get; set; }
        List<List<RelacionFraseArticulosTO>> listaLeyes { get; set; }

        void ActualizaRangoMarcado(int inicial, int FinRango);

        void PortaPapelesClic();

        void GuardarClic();

        void FontMayorClic();

        void FontMenorClic();

        void MarcarTodoClic();

        void DesmarcarClic();

        void MarcarEnter();

        void MarcarSalir();

        void MarcarClic();

        void ConcordanciaClic();

        void ImprmirClic();

        void EjecutoriaClic();

        void VotoClic();

        void ObservacionesClic();

        void GenealogiaClic();

        void InicioListaClic();

        void AnteriorListaClic();

        void SiguienteListaClic();

        void UltimoListaClic();

        void IrClic();

        void SalirClic();

        void RegNumTecla(KeyEventArgs e);

        void ContenidoTextoCopia(object sender, DataObjectCopyingEventArgs e);

        void ContenidoTextoSeleccion();

        void ContenidoTextoTecla(KeyEventArgs e);

        void TextoBuscarCambio();

        void TextoBuscarTecla(KeyEventArgs e);

        void BuscarClic();

        void ImprimePapelClic();

        void BtnTacheClic();
    }
}
