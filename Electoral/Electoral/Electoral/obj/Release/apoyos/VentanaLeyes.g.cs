﻿#pragma checksum "..\..\..\apoyos\VentanaLeyes.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "9A7374442330E0A326E9DACC46239B93"
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
using mx.gob.scjn.ius_common.gui.gui.utilities;


namespace mx.gob.scjn.electoral_common.gui.apoyos {
    
    
    /// <summary>
    /// VentanaLeyes
    /// </summary>
    public partial class VentanaLeyes : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 18 "..\..\..\apoyos\VentanaLeyes.xaml"
        internal System.Windows.Controls.Label Titulo;
        
        #line default
        #line hidden
        
        
        #line 23 "..\..\..\apoyos\VentanaLeyes.xaml"
        internal System.Windows.Controls.TextBlock contenidoReal;
        
        #line default
        #line hidden
        
        
        #line 27 "..\..\..\apoyos\VentanaLeyes.xaml"
        internal System.Windows.Controls.Button Salir;
        
        #line default
        #line hidden
        
        
        #line 34 "..\..\..\apoyos\VentanaLeyes.xaml"
        internal System.Windows.Controls.RichTextBox contenidoLey;
        
        #line default
        #line hidden
        
        
        #line 42 "..\..\..\apoyos\VentanaLeyes.xaml"
        internal System.Windows.Controls.Button imprimir;
        
        #line default
        #line hidden
        
        
        #line 51 "..\..\..\apoyos\VentanaLeyes.xaml"
        internal System.Windows.Controls.Button Copiar;
        
        #line default
        #line hidden
        
        
        #line 60 "..\..\..\apoyos\VentanaLeyes.xaml"
        internal System.Windows.Shapes.Rectangle BarraMovimiento;
        
        #line default
        #line hidden
        
        
        #line 72 "..\..\..\apoyos\VentanaLeyes.xaml"
        internal System.Windows.Controls.Button Guardar;
        
        #line default
        #line hidden
        
        
        #line 77 "..\..\..\apoyos\VentanaLeyes.xaml"
        internal System.Windows.Controls.Button Anexos;
        
        #line default
        #line hidden
        
        
        #line 85 "..\..\..\apoyos\VentanaLeyes.xaml"
        internal System.Windows.Controls.FlowDocumentPageViewer docImpresion;
        
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
            System.Uri resourceLocater = new System.Uri("/Electoral;component/apoyos/ventanaleyes.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\apoyos\VentanaLeyes.xaml"
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
            this.Titulo = ((System.Windows.Controls.Label)(target));
            return;
            case 2:
            this.contenidoReal = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 3:
            this.Salir = ((System.Windows.Controls.Button)(target));
            
            #line 28 "..\..\..\apoyos\VentanaLeyes.xaml"
            this.Salir.Click += new System.Windows.RoutedEventHandler(this.Salir_MouseLeftButtonUp);
            
            #line default
            #line hidden
            return;
            case 4:
            this.contenidoLey = ((System.Windows.Controls.RichTextBox)(target));
            return;
            case 5:
            this.imprimir = ((System.Windows.Controls.Button)(target));
            
            #line 46 "..\..\..\apoyos\VentanaLeyes.xaml"
            this.imprimir.Click += new System.Windows.RoutedEventHandler(this.imprimir_MouseLeftButtonUp);
            
            #line default
            #line hidden
            return;
            case 6:
            this.Copiar = ((System.Windows.Controls.Button)(target));
            
            #line 56 "..\..\..\apoyos\VentanaLeyes.xaml"
            this.Copiar.Click += new System.Windows.RoutedEventHandler(this.Copiar_MouseLeftButtonUp);
            
            #line default
            #line hidden
            return;
            case 7:
            this.BarraMovimiento = ((System.Windows.Shapes.Rectangle)(target));
            
            #line 64 "..\..\..\apoyos\VentanaLeyes.xaml"
            this.BarraMovimiento.MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.BarraMovimiento_DragEnter);
            
            #line default
            #line hidden
            
            #line 65 "..\..\..\apoyos\VentanaLeyes.xaml"
            this.BarraMovimiento.MouseMove += new System.Windows.Input.MouseEventHandler(this.BarraMovimiento_DragLeave);
            
            #line default
            #line hidden
            
            #line 66 "..\..\..\apoyos\VentanaLeyes.xaml"
            this.BarraMovimiento.MouseLeftButtonUp += new System.Windows.Input.MouseButtonEventHandler(this.BarraMovimiento_MouseLeftButtonUp);
            
            #line default
            #line hidden
            return;
            case 8:
            this.Guardar = ((System.Windows.Controls.Button)(target));
            
            #line 74 "..\..\..\apoyos\VentanaLeyes.xaml"
            this.Guardar.Click += new System.Windows.RoutedEventHandler(this.Guardar_MouseLeftButtonDown);
            
            #line default
            #line hidden
            return;
            case 9:
            this.Anexos = ((System.Windows.Controls.Button)(target));
            
            #line 83 "..\..\..\apoyos\VentanaLeyes.xaml"
            this.Anexos.Click += new System.Windows.RoutedEventHandler(this.Anexos_Click);
            
            #line default
            #line hidden
            return;
            case 10:
            this.docImpresion = ((System.Windows.Controls.FlowDocumentPageViewer)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}