﻿#pragma checksum "..\..\..\..\Properties\Busqueda\BusquedaCopia.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "65F02CA87025C2D683BB31CF9C2F68AF"
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
using System.Windows.Shell;


namespace AppTesauro09wpf.Busqueda {
    
    
    /// <summary>
    /// BusquedaCopia
    /// </summary>
    public partial class BusquedaCopia : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 22 "..\..\..\..\Properties\Busqueda\BusquedaCopia.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock lblRuta;
        
        #line default
        #line hidden
        
        
        #line 27 "..\..\..\..\Properties\Busqueda\BusquedaCopia.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox CbxTodos;
        
        #line default
        #line hidden
        
        
        #line 28 "..\..\..\..\Properties\Busqueda\BusquedaCopia.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TreeView TviResultado;
        
        #line default
        #line hidden
        
        
        #line 37 "..\..\..\..\Properties\Busqueda\BusquedaCopia.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button BtnCopiar;
        
        #line default
        #line hidden
        
        
        #line 38 "..\..\..\..\Properties\Busqueda\BusquedaCopia.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button BtnIgnorar;
        
        #line default
        #line hidden
        
        
        #line 39 "..\..\..\..\Properties\Busqueda\BusquedaCopia.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button BtnCancelar;
        
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
            System.Uri resourceLocater = new System.Uri("/AppTesauro09wpf;component/properties/busqueda/busquedacopia.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\Properties\Busqueda\BusquedaCopia.xaml"
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
            this.lblRuta = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 2:
            this.CbxTodos = ((System.Windows.Controls.CheckBox)(target));
            
            #line 27 "..\..\..\..\Properties\Busqueda\BusquedaCopia.xaml"
            this.CbxTodos.Click += new System.Windows.RoutedEventHandler(this.CbxTodos_Click);
            
            #line default
            #line hidden
            return;
            case 3:
            this.TviResultado = ((System.Windows.Controls.TreeView)(target));
            return;
            case 4:
            this.BtnCopiar = ((System.Windows.Controls.Button)(target));
            
            #line 37 "..\..\..\..\Properties\Busqueda\BusquedaCopia.xaml"
            this.BtnCopiar.Click += new System.Windows.RoutedEventHandler(this.BtnCopiar_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.BtnIgnorar = ((System.Windows.Controls.Button)(target));
            
            #line 38 "..\..\..\..\Properties\Busqueda\BusquedaCopia.xaml"
            this.BtnIgnorar.Click += new System.Windows.RoutedEventHandler(this.BtnIgnorar_Click);
            
            #line default
            #line hidden
            return;
            case 6:
            this.BtnCancelar = ((System.Windows.Controls.Button)(target));
            
            #line 39 "..\..\..\..\Properties\Busqueda\BusquedaCopia.xaml"
            this.BtnCancelar.Click += new System.Windows.RoutedEventHandler(this.BtnCancelar_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

