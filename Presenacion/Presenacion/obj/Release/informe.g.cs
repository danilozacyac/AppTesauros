﻿#pragma checksum "..\..\informe.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "83288D0B09E306DFB9C7693A6938C02D"
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
using mx.gob.scjn.ius_common.gui.apoyos;


namespace IUS {
    
    
    /// <summary>
    /// Informe
    /// </summary>
    public partial class Informe : System.Windows.Controls.Page, System.Windows.Markup.IComponentConnector {
        
        
        #line 28 "..\..\informe.xaml"
        internal System.Windows.Controls.Button Imprimir;
        
        #line default
        #line hidden
        
        
        #line 36 "..\..\informe.xaml"
        internal System.Windows.Controls.Button PortaPapeles;
        
        #line default
        #line hidden
        
        
        #line 41 "..\..\informe.xaml"
        internal System.Windows.Controls.Button Salir;
        
        #line default
        #line hidden
        
        
        #line 46 "..\..\informe.xaml"
        internal System.Windows.Controls.Frame contenido;
        
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
            System.Uri resourceLocater = new System.Uri("/Presenacion;component/informe.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\informe.xaml"
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
            this.Imprimir = ((System.Windows.Controls.Button)(target));
            
            #line 30 "..\..\informe.xaml"
            this.Imprimir.Click += new System.Windows.RoutedEventHandler(this.Imprimir_MouseLeftButtonDown);
            
            #line default
            #line hidden
            return;
            case 2:
            this.PortaPapeles = ((System.Windows.Controls.Button)(target));
            
            #line 37 "..\..\informe.xaml"
            this.PortaPapeles.Click += new System.Windows.RoutedEventHandler(this.PortaPapeles_MouseLeftButtonDown);
            
            #line default
            #line hidden
            return;
            case 3:
            this.Salir = ((System.Windows.Controls.Button)(target));
            
            #line 44 "..\..\informe.xaml"
            this.Salir.Click += new System.Windows.RoutedEventHandler(this.Salir_MouseLeftButtonDown);
            
            #line default
            #line hidden
            return;
            case 4:
            this.contenido = ((System.Windows.Controls.Frame)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}
