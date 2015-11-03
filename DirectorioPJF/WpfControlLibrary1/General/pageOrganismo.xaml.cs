
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

//using Xceed.Wpf.DataGrid;
//using Xceed.Wpf.DataGrid.Views;
//using Xceed.Wpf.DataGrid.Views.Surfaces;
//using Xceed.Wpf.Controls;
//using Xceed.Wpf.DataGrid.ThemePack;
namespace mx.gob.scjn.directorio.General
{

    /// <summary>
    /// Interaction logic for pageOrganismo.xaml
    /// </summary>
    public partial class pageOrganismo : Page
    {

        public Page Back { get; set; }
        public pageOrganismo()
        {
            InitializeComponent();
            llenaGrid();
        }

        public void llenaGrid()
        {
            List<String> lstAreas = new List<String>();
            clsListaAreas oAreas = new clsListaAreas();
            lstAreas = oAreas.TraeDatos();
            grdOrganismos.AutoCreateColumns = true;
            grdOrganismos.ItemsSource = lstAreas;
        }
    }
}
