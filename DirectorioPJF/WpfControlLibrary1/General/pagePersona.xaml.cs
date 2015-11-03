using System;
using System.Linq;
using System.Windows.Controls;

namespace mx.gob.scjn.directorio.General
{

    /// <summary>
    /// Interaction logic for FuncCJF.xaml
    /// </summary>
    public partial class pagePersona : Page
    {

        public Page Back { get; set; }

        public pagePersona()
        {
            InitializeComponent();

            clsPersona oPersonaActual = new clsPersona();
            llenaDetalle(oPersonaActual);
        }

        private void llenaDetalle(clsPersona oPersona)
        {
            this.NombreFunc.Content = oPersona.NombreCompleto();
            this.lblNombreOrgJud.Content = oPersona.OrganoAlQuePertenece();
            this.lblDirFunc.Content = oPersona.Domicilio();
        }

    }
}
