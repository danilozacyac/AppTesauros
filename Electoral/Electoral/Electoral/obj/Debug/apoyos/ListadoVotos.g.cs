﻿#pragma checksum "..\..\..\apoyos\ListadoVotos.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "5B8620217D8948561886EE9910366157"
//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.34209
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
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
using Xceed.Wpf.DataGrid;
using Xceed.Wpf.DataGrid.Views;


namespace mx.gob.scjn.electoral_common.gui.apoyos {
    
    
    /// <summary>
    /// ListadoVotos
    /// </summary>
    public partial class ListadoVotos : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 12 "..\..\..\apoyos\ListadoVotos.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Border degradado;
        
        #line default
        #line hidden
        
        
        #line 15 "..\..\..\apoyos\ListadoVotos.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Xceed.Wpf.DataGrid.DataGridControl listado;
        
        #line default
        #line hidden
        
        
        #line 50 "..\..\..\apoyos\ListadoVotos.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label label1;
        
        #line default
        #line hidden
        
        
        #line 56 "..\..\..\apoyos\ListadoVotos.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button Salir;
        
        #line default
        #line hidden
        
        
        #line 63 "..\..\..\apoyos\ListadoVotos.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Shapes.Rectangle BarraMovimiento;
        
        #line default
        #line hidden
        
        
        #line 71 "..\..\..\apoyos\ListadoVotos.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label Registros;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/Electoral;component/apoyos/listadovotos.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\apoyos\ListadoVotos.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.degradado = ((System.Windows.Controls.Border)(target));
            return;
            case 2:
            this.listado = ((Xceed.Wpf.DataGrid.DataGridControl)(target));
            
            #line 20 "..\..\..\apoyos\ListadoVotos.xaml"
            this.listado.MouseDoubleClick += new System.Windows.Input.MouseButtonEventHandler(this.listado_MouseDoubleClick);
            
            #line default
            #line hidden
            return;
            case 3:
            this.label1 = ((System.Windows.Controls.Label)(target));
            return;
            case 4:
            this.Salir = ((System.Windows.Controls.Button)(target));
            
            #line 60 "..\..\..\apoyos\ListadoVotos.xaml"
            this.Salir.Click += new System.Windows.RoutedEventHandler(this.Salir_MouseLeftButtonDown);
            
            #line default
            #line hidden
            return;
            case 5:
            this.BarraMovimiento = ((System.Windows.Shapes.Rectangle)(target));
            
            #line 66 "..\..\..\apoyos\ListadoVotos.xaml"
            this.BarraMovimiento.MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.BarraMovimiento_MouseLeftButtonDown);
            
            #line default
            #line hidden
            
            #line 67 "..\..\..\apoyos\ListadoVotos.xaml"
            this.BarraMovimiento.MouseLeftButtonUp += new System.Windows.Input.MouseButtonEventHandler(this.BarraMovimiento_MouseLeftButtonUp);
            
            #line default
            #line hidden
            
            #line 68 "..\..\..\apoyos\ListadoVotos.xaml"
            this.BarraMovimiento.MouseMove += new System.Windows.Input.MouseEventHandler(this.BarraMovimiento_MouseMove);
            
            #line default
            #line hidden
            return;
            case 6:
            this.Registros = ((System.Windows.Controls.Label)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

