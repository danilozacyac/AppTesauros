﻿#pragma checksum "..\..\..\apoyos\EjecutoriasOrdenarPor.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "B2443DF552B630E4A1F33B0B1E28282D"
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
using Xceed.Wpf.DataGrid;
using Xceed.Wpf.DataGrid.Views;


namespace mx.gob.scjn.ius_common.gui.apoyos {
    
    
    /// <summary>
    /// EjecutoriasOrdenarPor
    /// </summary>
    public partial class EjecutoriasOrdenarPor : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 17 "..\..\..\apoyos\EjecutoriasOrdenarPor.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label Titulo;
        
        #line default
        #line hidden
        
        
        #line 23 "..\..\..\apoyos\EjecutoriasOrdenarPor.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button Salir;
        
        #line default
        #line hidden
        
        
        #line 37 "..\..\..\apoyos\EjecutoriasOrdenarPor.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label LblLoc;
        
        #line default
        #line hidden
        
        
        #line 42 "..\..\..\apoyos\EjecutoriasOrdenarPor.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label LblIus;
        
        #line default
        #line hidden
        
        
        #line 50 "..\..\..\apoyos\EjecutoriasOrdenarPor.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Shapes.Rectangle BarraMovimiento;
        
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
            System.Uri resourceLocater = new System.Uri("/Presenacion;component/apoyos/ejecutoriasordenarpor.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\apoyos\EjecutoriasOrdenarPor.xaml"
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
            this.Titulo = ((System.Windows.Controls.Label)(target));
            return;
            case 2:
            this.Salir = ((System.Windows.Controls.Button)(target));
            
            #line 27 "..\..\..\apoyos\EjecutoriasOrdenarPor.xaml"
            this.Salir.Click += new System.Windows.RoutedEventHandler(this.Salir_MouseLeftButtonDown);
            
            #line default
            #line hidden
            return;
            case 3:
            this.LblLoc = ((System.Windows.Controls.Label)(target));
            
            #line 39 "..\..\..\apoyos\EjecutoriasOrdenarPor.xaml"
            this.LblLoc.MouseEnter += new System.Windows.Input.MouseEventHandler(this.LblLoc_MouseEnter);
            
            #line default
            #line hidden
            
            #line 40 "..\..\..\apoyos\EjecutoriasOrdenarPor.xaml"
            this.LblLoc.MouseLeave += new System.Windows.Input.MouseEventHandler(this.LblLoc_MouseLeave);
            
            #line default
            #line hidden
            
            #line 41 "..\..\..\apoyos\EjecutoriasOrdenarPor.xaml"
            this.LblLoc.MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.LblLoc_MouseLeftButtonDown);
            
            #line default
            #line hidden
            return;
            case 4:
            this.LblIus = ((System.Windows.Controls.Label)(target));
            
            #line 44 "..\..\..\apoyos\EjecutoriasOrdenarPor.xaml"
            this.LblIus.MouseEnter += new System.Windows.Input.MouseEventHandler(this.LblLoc_MouseEnter);
            
            #line default
            #line hidden
            
            #line 45 "..\..\..\apoyos\EjecutoriasOrdenarPor.xaml"
            this.LblIus.MouseLeave += new System.Windows.Input.MouseEventHandler(this.LblLoc_MouseLeave);
            
            #line default
            #line hidden
            
            #line 46 "..\..\..\apoyos\EjecutoriasOrdenarPor.xaml"
            this.LblIus.MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.LblIus_MouseLeftButtonDown);
            
            #line default
            #line hidden
            return;
            case 5:
            this.BarraMovimiento = ((System.Windows.Shapes.Rectangle)(target));
            
            #line 55 "..\..\..\apoyos\EjecutoriasOrdenarPor.xaml"
            this.BarraMovimiento.MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.BarraMovimiento_MouseLeftButtonDown);
            
            #line default
            #line hidden
            
            #line 56 "..\..\..\apoyos\EjecutoriasOrdenarPor.xaml"
            this.BarraMovimiento.MouseLeftButtonUp += new System.Windows.Input.MouseButtonEventHandler(this.BarraMovimiento_MouseLeftButtonUp);
            
            #line default
            #line hidden
            
            #line 57 "..\..\..\apoyos\EjecutoriasOrdenarPor.xaml"
            this.BarraMovimiento.MouseMove += new System.Windows.Input.MouseEventHandler(this.BarraMovimiento_MouseMove);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

