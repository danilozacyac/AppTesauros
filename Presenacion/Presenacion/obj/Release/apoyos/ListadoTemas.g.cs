﻿#pragma checksum "..\..\..\apoyos\ListadoTemas.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "A039AC64B0AB8CF84AF4F703578FC59B"
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
    /// ListadoTemas
    /// </summary>
    public partial class ListadoTemas : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 15 "..\..\..\apoyos\ListadoTemas.xaml"
        internal System.Windows.Controls.Label label1;
        
        #line default
        #line hidden
        
        
        #line 22 "..\..\..\apoyos\ListadoTemas.xaml"
        internal System.Windows.Controls.Button Salir;
        
        #line default
        #line hidden
        
        
        #line 30 "..\..\..\apoyos\ListadoTemas.xaml"
        internal System.Windows.Shapes.Rectangle BarraMovimiento;
        
        #line default
        #line hidden
        
        
        #line 37 "..\..\..\apoyos\ListadoTemas.xaml"
        internal System.Windows.Controls.Button btnSeleccionar;
        
        #line default
        #line hidden
        
        
        #line 43 "..\..\..\apoyos\ListadoTemas.xaml"
        internal Xceed.Wpf.DataGrid.DataGridControl listado;
        
        #line default
        #line hidden
        
        
        #line 67 "..\..\..\apoyos\ListadoTemas.xaml"
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
            System.Uri resourceLocater = new System.Uri("/Presenacion;component/apoyos/listadotemas.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\apoyos\ListadoTemas.xaml"
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
            
            #line 27 "..\..\..\apoyos\ListadoTemas.xaml"
            this.Salir.Click += new System.Windows.RoutedEventHandler(this.Salir_MouseLeftButtonDown);
            
            #line default
            #line hidden
            return;
            case 3:
            this.BarraMovimiento = ((System.Windows.Shapes.Rectangle)(target));
            
            #line 33 "..\..\..\apoyos\ListadoTemas.xaml"
            this.BarraMovimiento.MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.BarraMovimiento_MouseLeftButtonDown);
            
            #line default
            #line hidden
            
            #line 34 "..\..\..\apoyos\ListadoTemas.xaml"
            this.BarraMovimiento.MouseLeftButtonUp += new System.Windows.Input.MouseButtonEventHandler(this.BarraMovimiento_MouseLeftButtonUp);
            
            #line default
            #line hidden
            
            #line 35 "..\..\..\apoyos\ListadoTemas.xaml"
            this.BarraMovimiento.MouseMove += new System.Windows.Input.MouseEventHandler(this.BarraMovimiento_MouseMove);
            
            #line default
            #line hidden
            return;
            case 4:
            this.btnSeleccionar = ((System.Windows.Controls.Button)(target));
            
            #line 39 "..\..\..\apoyos\ListadoTemas.xaml"
            this.btnSeleccionar.Click += new System.Windows.RoutedEventHandler(this.btnSeleccionar_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.listado = ((Xceed.Wpf.DataGrid.DataGridControl)(target));
            return;
            case 6:
            this.Busqueda = ((System.Windows.Controls.TextBox)(target));
            
            #line 66 "..\..\..\apoyos\ListadoTemas.xaml"
            this.Busqueda.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.Busqueda_TextChanged);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}
