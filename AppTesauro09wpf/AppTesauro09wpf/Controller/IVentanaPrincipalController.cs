using System;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;

namespace AppTesauro09wpf.Controller
{
    public interface IVentanaPrincipalController
    {
        WMain Ventana { get; set; }
        void LoadTemas();
        void ObtenDatos(TreeView arbol);
        void CambioTema(TreeView arbol);
        void CambioRP();
        void CambioSinonimos();
        void ActualizaExpresiones(int tipo);
        void ObtenDatos(int idTema);
        void EliminaTema();
        void EliminaRP();
        void EliminaSinonimo();
        void EliminaExpresion();
        void SeleccionaIA();
        void BuscarTema();
        void RestaurarTemas();
        void NuevoTema();
        void NuevoRP();
        void NuevoSinonimo();
        void NuevaExpresion();
        void ModificarExpresion();
        void ModificaTema();
        void ModificaRP();
        void ModificaSinonimo();
        void NuevaRelacionTemaTesis();
        void ImportaTema();

        void AplicaSeguridad();

        void EjecutaConsulta();

        void ObtenTesisPorRegistro();
        void MuestraEstadisticas();
        void GeneraListadoCertificacion();
        void TesisNoRelacionadas();

        #region MenuItems KeyBoardFastAccess

        void KeyBoardFastAccess(object sender, System.Windows.Input.KeyEventArgs e);
        void CtrlBFunction();
        void CtrlVFunction();
        void CtrlXFunction(Key key);
        void CtrlCFunction(Key key);

        void CtrlYFunction();
        #endregion


        #region Exporta Información

        void GetPdf();
        void GetExcelReport();

        #endregion

        mx.gob.scjn.ius_common.TO.BusquedaTO EjecutaConsulta(TesauroTO.TemaTO item);

        //mx.gob.scjn.ius_common.TO.TesisTO[] ObtenTesisPorRegistro(List<int> registros);

        #region RibbonView

        void CopiarTesisMarcadas();
        void TrasladaTesisMarcadas();
        void EliminaMarcacionTesis();

        #endregion
    }
}
