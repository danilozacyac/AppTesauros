﻿#pragma checksum "..\..\..\apoyos\ListadoTesis.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "D938A9110A8D84ADD26B870914D49DC1"
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
    /// ListadoTesis
    /// </summary>
    public partial class ListadoTesis : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 12 "..\..\..\apoyos\ListadoTesis.xaml"
        internal System.Windows.Controls.Border degradado;
        
        #line default
        #line hidden
        
        
        #line 16 "..\..\..\apoyos\ListadoTesis.xaml"
        internal System.Windows.Controls.Label label1;
        
        #line default
        #line hidden
        
        
        #line 20 "..\..\..\apoyos\ListadoTesis.xaml"
        internal System.Windows.Controls.TextBlock Titulo;
        
        #line default
        #line hidden
        
        
        #line 23 "..\..\..\apoyos\ListadoTesis.xaml"
        internal System.Windows.Controls.Button Salir;
        
        #line default
        #line hidden
        
        
        #line 30 "..\..\..\apoyos\ListadoTesis.xaml"
        internal System.Windows.Shapes.Rectangle BarraMovimiento;
        
        #line default
        #line hidden
        
        
        #line 37 "..\..\..\apoyos\ListadoTesis.xaml"
        internal Xceed.Wpf.DataGrid.DataGridControl listado;
        
        #line default
        #line hidden
        
        
        #line 62 "..\..\..\apoyos\ListadoTesis.xaml"
        internal System.Windows.Controls.Label Registros;
        
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
            System.Uri resourceLocater = new System.Uri("/Presenacion;component/apoyos/listadotesis.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\apoyos\ListadoTesis.xaml"
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
            this.degradado = ((System.Windows.Controls.Border)(target));
            return;
            case 2:
            this.label1 = ((System.Windows.Controls.Label)(target));
            return;
            case 3:
            this.Titulo = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 4:
            this.Salir = ((System.Windows.Controls.Button)(target));
            
            #line 27 "..\..\..\apoyos\ListadoTesis.xaml"
            this.Salir.Click += new System.Windows.RoutedEventHandler(this.Salir_MouseLeftButtonDown);
            
            #line default
            #line hidden
            return;
            case 5:
            this.BarraMovimiento = ((System.Windows.Shapes.Rectangle)(target));
            
            #line 33 "..\..\..\apoyos\ListadoTesis.xaml"
            this.BarraMovimiento.MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.BarraMovimiento_MouseLeftButtonDown);
            
            #line default
            #line hidden
            
            #line 34 "..\..\..\apoyos\ListadoTesis.xaml"
            this.BarraMovimiento.MouseRightButtonUp += new System.Windows.Input.MouseButtonEventHandler(this.BarraMovimiento_MouseRightButtonUp);
            
            #line default
            #line hidden
            
            #line 35 "..\..\..\apoyos\ListadoTesis.xaml"
            this.BarraMovimiento.MouseMove += new System.Windows.Input.MouseEventHandler(this.BarraMovimiento_MouseMove);
            
            #line default
            #line hidden
            return;
            case 6:
            this.listado = ((Xceed.Wpf.DataGrid.DataGridControl)(target));
            
            #line 41 "..\..\..\apoyos\ListadoTesis.xaml"
            this.listado.MouseDoubleClick += new System.Windows.Input.MouseButtonEventHandler(this.listado_MouseDoubleClick);
            
            #line default
            #line hidden
            return;
            case 7:
            this.Registros = ((System.Windows.Controls.Label)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}
