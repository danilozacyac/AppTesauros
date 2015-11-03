using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace mx.gob.scjn.electoral.Controller
{
    public interface IMenuElectoral
    {
        UIElement Back { get; set; }
        MenuElectoral Ventana { get; set; }
        void InicializaPaneles();
        void CbxTipoDocChanged();
        void BtnSecuencialClic();
        void BtnRegistroClic();
        void BtnPalabraClic(MouseButtonEventArgs e);
        void BtnSalirClic();
    }
}
