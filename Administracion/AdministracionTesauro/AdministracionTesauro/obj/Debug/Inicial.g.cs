﻿#pragma checksum "..\..\Inicial.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "1F0B54648B7AEBBA91930C8563778B60"
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


namespace AdministracionTesauro {
    
    
    /// <summary>
    /// Inicial
    /// </summary>
    public partial class Inicial : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 14 "..\..\Inicial.xaml"
        internal System.Windows.Controls.Button Permisos;
        
        #line default
        #line hidden
        
        
        #line 15 "..\..\Inicial.xaml"
        internal System.Windows.Controls.Button Usuarios;
        
        #line default
        #line hidden
        
        
        #line 16 "..\..\Inicial.xaml"
        internal System.Windows.Controls.Grid Contenido;
        
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
            System.Uri resourceLocater = new System.Uri("/AdministracionTesauro;component/inicial.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\Inicial.xaml"
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
            this.Permisos = ((System.Windows.Controls.Button)(target));
            return;
            case 2:
            this.Usuarios = ((System.Windows.Controls.Button)(target));
            
            #line 15 "..\..\Inicial.xaml"
            this.Usuarios.Click += new System.Windows.RoutedEventHandler(this.Usuarios_Click);
            
            #line default
            #line hidden
            return;
            case 3:
            this.Contenido = ((System.Windows.Controls.Grid)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}