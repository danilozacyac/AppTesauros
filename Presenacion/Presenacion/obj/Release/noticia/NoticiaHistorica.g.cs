﻿#pragma checksum "..\..\..\noticia\NoticiaHistorica.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "33788B2F9343A89B78C4BAD3042DEE03"
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


namespace mx.gob.scjn.ius_common.gui.noticia {
    
    
    /// <summary>
    /// NoticiaHistorica
    /// </summary>
    public partial class NoticiaHistorica : System.Windows.Controls.Page, System.Windows.Markup.IComponentConnector {
        
        
        #line 12 "..\..\..\noticia\NoticiaHistorica.xaml"
        internal System.Windows.Controls.Button Ministros;
        
        #line default
        #line hidden
        
        
        #line 17 "..\..\..\noticia\NoticiaHistorica.xaml"
        internal System.Windows.Controls.Button Marco;
        
        #line default
        #line hidden
        
        
        #line 22 "..\..\..\noticia\NoticiaHistorica.xaml"
        internal System.Windows.Controls.Frame Contenido;
        
        #line default
        #line hidden
        
        
        #line 27 "..\..\..\noticia\NoticiaHistorica.xaml"
        internal System.Windows.Controls.Button Salir;
        
        #line default
        #line hidden
        
        
        #line 35 "..\..\..\noticia\NoticiaHistorica.xaml"
        internal System.Windows.Controls.Button Copiar;
        
        #line default
        #line hidden
        
        
        #line 40 "..\..\..\noticia\NoticiaHistorica.xaml"
        internal System.Windows.Controls.Button Imprimir;
        
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
            System.Uri resourceLocater = new System.Uri("/Presenacion;component/noticia/noticiahistorica.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\noticia\NoticiaHistorica.xaml"
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
            this.Ministros = ((System.Windows.Controls.Button)(target));
            
            #line 14 "..\..\..\noticia\NoticiaHistorica.xaml"
            this.Ministros.Click += new System.Windows.RoutedEventHandler(this.Ministros_Click);
            
            #line default
            #line hidden
            return;
            case 2:
            this.Marco = ((System.Windows.Controls.Button)(target));
            
            #line 20 "..\..\..\noticia\NoticiaHistorica.xaml"
            this.Marco.Click += new System.Windows.RoutedEventHandler(this.Marco_Click);
            
            #line default
            #line hidden
            return;
            case 3:
            this.Contenido = ((System.Windows.Controls.Frame)(target));
            return;
            case 4:
            this.Salir = ((System.Windows.Controls.Button)(target));
            
            #line 31 "..\..\..\noticia\NoticiaHistorica.xaml"
            this.Salir.Click += new System.Windows.RoutedEventHandler(this.Salir_MouseLeftButtonDown);
            
            #line default
            #line hidden
            return;
            case 5:
            this.Copiar = ((System.Windows.Controls.Button)(target));
            
            #line 36 "..\..\..\noticia\NoticiaHistorica.xaml"
            this.Copiar.Click += new System.Windows.RoutedEventHandler(this.Copiar_MouseLeftButtonDown);
            
            #line default
            #line hidden
            return;
            case 6:
            this.Imprimir = ((System.Windows.Controls.Button)(target));
            
            #line 41 "..\..\..\noticia\NoticiaHistorica.xaml"
            this.Imprimir.Click += new System.Windows.RoutedEventHandler(this.Imprimir_MouseLeftButtonDown);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}
