﻿#pragma checksum "..\..\..\apoyos\ListadoAsuntos.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "16D09B95A175AE67A6DD907ABF9F51C7"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.3603
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.Integration;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Xceed.Wpf.Controls;
using Xceed.Wpf.DataGrid;
using Xceed.Wpf.DataGrid.Converters;
using Xceed.Wpf.DataGrid.Markup;
using Xceed.Wpf.DataGrid.Print;
using Xceed.Wpf.DataGrid.Stats;
using Xceed.Wpf.DataGrid.ThemePack;
using Xceed.Wpf.DataGrid.Views;
using Xceed.Wpf.DataGrid.Views.Surfaces;


namespace mx.gob.scjn.ius_common.gui.apoyos {
    
    
    /// <summary>
    /// ListadoAsuntos
    /// </summary>
    public partial class ListadoAsuntos : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 15 "..\..\..\apoyos\ListadoAsuntos.xaml"
        internal System.Windows.Controls.Label label1;
        
        #line default
        #line hidden
        
        
        #line 22 "..\..\..\apoyos\ListadoAsuntos.xaml"
        internal System.Windows.Controls.Button Salir;
        
        #line default
        #line hidden
        
        
        #line 30 "..\..\..\apoyos\ListadoAsuntos.xaml"
        internal System.Windows.Shapes.Rectangle BarraMovimiento;
        
        #line default
        #line hidden
        
        
        #line 37 "..\..\..\apoyos\ListadoAsuntos.xaml"
        internal System.Windows.Controls.Button btnSeleccionar;
        
        #line default
        #line hidden
        
        
        #line 41 "..\..\..\apoyos\ListadoAsuntos.xaml"
        internal Xceed.Wpf.DataGrid.DataGridControl listado;
        
        #line default
        #line hidden
        
        
        #line 64 "..\..\..\apoyos\ListadoAsuntos.xaml"
        internal System.Windows.Controls.TextBox Busqueda;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/Presenacion;component/apoyos/listadoasuntos.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\apoyos\ListadoAsuntos.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.label1 = ((System.Windows.Controls.Label)(target));
            return;
            case 2:
            this.Salir = ((System.Windows.Controls.Button)(target));
            
            #line 27 "..\..\..\apoyos\ListadoAsuntos.xaml"
            this.Salir.Click += new System.Windows.RoutedEventHandler(this.Salir_MouseLeftButtonDown);
            
            #line default
            #line hidden
            return;
            case 3:
            this.BarraMovimiento = ((System.Windows.Shapes.Rectangle)(target));
            
            #line 33 "..\..\..\apoyos\ListadoAsuntos.xaml"
            this.BarraMovimiento.MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.BarraMovimiento_MouseLeftButtonDown);
            
            #line default
            #line hidden
            
            #line 34 "..\..\..\apoyos\ListadoAsuntos.xaml"
            this.BarraMovimiento.MouseLeftButtonUp += new System.Windows.Input.MouseButtonEventHandler(this.BarraMovimiento_MouseLeftButtonUp);
            
            #line default
            #line hidden
            
            #line 35 "..\..\..\apoyos\ListadoAsuntos.xaml"
            this.BarraMovimiento.MouseMove += new System.Windows.Input.MouseEventHandler(this.BarraMovimiento_MouseMove);
            
            #line default
            #line hidden
            return;
            case 4:
            this.btnSeleccionar = ((System.Windows.Controls.Button)(target));
            
            #line 38 "..\..\..\apoyos\ListadoAsuntos.xaml"
            this.btnSeleccionar.Click += new System.Windows.RoutedEventHandler(this.btnSeleccionar_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.listado = ((Xceed.Wpf.DataGrid.DataGridControl)(target));
            return;
            case 6:
            this.Busqueda = ((System.Windows.Controls.TextBox)(target));
            
            #line 63 "..\..\..\apoyos\ListadoAsuntos.xaml"
            this.Busqueda.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.Busqueda_TextChanged);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}
