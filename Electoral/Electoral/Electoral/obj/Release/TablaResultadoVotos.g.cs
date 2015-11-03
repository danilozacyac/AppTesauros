﻿#pragma checksum "..\..\TablaResultadoVotos.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "489370C566AE5181B382E09C16766249"
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
using Xceed.Wpf.Controls;
using Xceed.Wpf.DataGrid;
using Xceed.Wpf.DataGrid.Converters;
using Xceed.Wpf.DataGrid.Markup;
using Xceed.Wpf.DataGrid.Print;
using Xceed.Wpf.DataGrid.Stats;
using Xceed.Wpf.DataGrid.ThemePack;
using Xceed.Wpf.DataGrid.Views;
using Xceed.Wpf.DataGrid.Views.Surfaces;
using mx.gob.scjn.electoral;
using mx.gob.scjn.electoral_common.gui.apoyos;
using mx.gob.scjn.ius_common.gui.gui.utilities;


namespace mx.gob.scjn.electoral {
    
    
    /// <summary>
    /// TablaResultadoVotos
    /// </summary>
    public partial class TablaResultadoVotos : System.Windows.Controls.Page, System.Windows.Markup.IComponentConnector, System.Windows.Markup.IStyleConnector {
        
        
        #line 43 "..\..\TablaResultadoVotos.xaml"
        internal System.Windows.Controls.Label Titulo;
        
        #line default
        #line hidden
        
        
        #line 48 "..\..\TablaResultadoVotos.xaml"
        internal System.Windows.Controls.Button inicio;
        
        #line default
        #line hidden
        
        
        #line 53 "..\..\TablaResultadoVotos.xaml"
        internal System.Windows.Controls.Button anterior;
        
        #line default
        #line hidden
        
        
        #line 58 "..\..\TablaResultadoVotos.xaml"
        internal System.Windows.Controls.Button siguiente;
        
        #line default
        #line hidden
        
        
        #line 63 "..\..\TablaResultadoVotos.xaml"
        internal System.Windows.Controls.Button final;
        
        #line default
        #line hidden
        
        
        #line 68 "..\..\TablaResultadoVotos.xaml"
        internal System.Windows.Controls.Label RegistrosLabel;
        
        #line default
        #line hidden
        
        
        #line 75 "..\..\TablaResultadoVotos.xaml"
        internal System.Windows.Controls.Label lblIrA;
        
        #line default
        #line hidden
        
        
        #line 80 "..\..\TablaResultadoVotos.xaml"
        internal Xceed.Wpf.Controls.NumericTextBox IrANum;
        
        #line default
        #line hidden
        
        
        #line 88 "..\..\TablaResultadoVotos.xaml"
        internal System.Windows.Controls.Button btnIrA;
        
        #line default
        #line hidden
        
        
        #line 94 "..\..\TablaResultadoVotos.xaml"
        internal System.Windows.Controls.Button imprimir;
        
        #line default
        #line hidden
        
        
        #line 99 "..\..\TablaResultadoVotos.xaml"
        internal System.Windows.Controls.Button salir;
        
        #line default
        #line hidden
        
        
        #line 103 "..\..\TablaResultadoVotos.xaml"
        internal System.Windows.Controls.RichTextBox Contenido;
        
        #line default
        #line hidden
        
        
        #line 105 "..\..\TablaResultadoVotos.xaml"
        internal System.Windows.Controls.Button BtnGuardar;
        
        #line default
        #line hidden
        
        
        #line 110 "..\..\TablaResultadoVotos.xaml"
        internal System.Windows.Controls.Button BtnVisualizar;
        
        #line default
        #line hidden
        
        
        #line 113 "..\..\TablaResultadoVotos.xaml"
        internal System.Windows.Controls.Button BtnAumentaAlto;
        
        #line default
        #line hidden
        
        
        #line 120 "..\..\TablaResultadoVotos.xaml"
        internal System.Windows.Controls.Button BtnDisminuyeAlto;
        
        #line default
        #line hidden
        
        
        #line 127 "..\..\TablaResultadoVotos.xaml"
        internal Xceed.Wpf.DataGrid.DataGridControl tablaResultado;
        
        #line default
        #line hidden
        
        
        #line 174 "..\..\TablaResultadoVotos.xaml"
        internal System.Windows.Controls.FlowDocumentPageViewer impresionViewer;
        
        #line default
        #line hidden
        
        
        #line 181 "..\..\TablaResultadoVotos.xaml"
        internal System.Windows.Controls.Button imprimePapel;
        
        #line default
        #line hidden
        
        
        #line 188 "..\..\TablaResultadoVotos.xaml"
        internal System.Windows.Controls.Button BtnTache;
        
        #line default
        #line hidden
        
        
        #line 197 "..\..\TablaResultadoVotos.xaml"
        internal System.Windows.Controls.Canvas Esperar;
        
        #line default
        #line hidden
        
        
        #line 207 "..\..\TablaResultadoVotos.xaml"
        internal System.Windows.Controls.StackPanel EsperarStack;
        
        #line default
        #line hidden
        
        
        #line 211 "..\..\TablaResultadoVotos.xaml"
        internal System.Windows.Controls.ProgressBar EsperaBarra;
        
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
            System.Uri resourceLocater = new System.Uri("/Electoral;component/tablaresultadovotos.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\TablaResultadoVotos.xaml"
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
            
            #line 9 "..\..\TablaResultadoVotos.xaml"
            ((mx.gob.scjn.electoral.TablaResultadoVotos)(target)).Loaded += new System.Windows.RoutedEventHandler(this.Page_Loaded);
            
            #line default
            #line hidden
            return;
            case 2:
            this.Titulo = ((System.Windows.Controls.Label)(target));
            return;
            case 3:
            this.inicio = ((System.Windows.Controls.Button)(target));
            
            #line 49 "..\..\TablaResultadoVotos.xaml"
            this.inicio.Click += new System.Windows.RoutedEventHandler(this.inicio_MouseLeftButtonDown);
            
            #line default
            #line hidden
            return;
            case 4:
            this.anterior = ((System.Windows.Controls.Button)(target));
            
            #line 55 "..\..\TablaResultadoVotos.xaml"
            this.anterior.Click += new System.Windows.RoutedEventHandler(this.anterior_MouseLeftButtonDown);
            
            #line default
            #line hidden
            return;
            case 5:
            this.siguiente = ((System.Windows.Controls.Button)(target));
            
            #line 60 "..\..\TablaResultadoVotos.xaml"
            this.siguiente.Click += new System.Windows.RoutedEventHandler(this.siguiente_MouseLeftButtonDown);
            
            #line default
            #line hidden
            return;
            case 6:
            this.final = ((System.Windows.Controls.Button)(target));
            
            #line 65 "..\..\TablaResultadoVotos.xaml"
            this.final.Click += new System.Windows.RoutedEventHandler(this.final_MouseLeftButtonDown);
            
            #line default
            #line hidden
            return;
            case 7:
            this.RegistrosLabel = ((System.Windows.Controls.Label)(target));
            return;
            case 8:
            this.lblIrA = ((System.Windows.Controls.Label)(target));
            return;
            case 9:
            this.IrANum = ((Xceed.Wpf.Controls.NumericTextBox)(target));
            
            #line 81 "..\..\TablaResultadoVotos.xaml"
            this.IrANum.KeyDown += new System.Windows.Input.KeyEventHandler(this.IrANum_KeyDown);
            
            #line default
            #line hidden
            return;
            case 10:
            this.btnIrA = ((System.Windows.Controls.Button)(target));
            
            #line 89 "..\..\TablaResultadoVotos.xaml"
            this.btnIrA.Click += new System.Windows.RoutedEventHandler(this.btnIrA_Click);
            
            #line default
            #line hidden
            return;
            case 11:
            this.imprimir = ((System.Windows.Controls.Button)(target));
            
            #line 97 "..\..\TablaResultadoVotos.xaml"
            this.imprimir.Click += new System.Windows.RoutedEventHandler(this.imprimir_onClick);
            
            #line default
            #line hidden
            return;
            case 12:
            this.salir = ((System.Windows.Controls.Button)(target));
            
            #line 102 "..\..\TablaResultadoVotos.xaml"
            this.salir.Click += new System.Windows.RoutedEventHandler(this.salir_onClick);
            
            #line default
            #line hidden
            return;
            case 13:
            this.Contenido = ((System.Windows.Controls.RichTextBox)(target));
            return;
            case 14:
            this.BtnGuardar = ((System.Windows.Controls.Button)(target));
            
            #line 108 "..\..\TablaResultadoVotos.xaml"
            this.BtnGuardar.Click += new System.Windows.RoutedEventHandler(this.BtnGuardar_Click);
            
            #line default
            #line hidden
            return;
            case 15:
            this.BtnVisualizar = ((System.Windows.Controls.Button)(target));
            
            #line 112 "..\..\TablaResultadoVotos.xaml"
            this.BtnVisualizar.Click += new System.Windows.RoutedEventHandler(this.BtnVisualizar_Click);
            
            #line default
            #line hidden
            return;
            case 16:
            this.BtnAumentaAlto = ((System.Windows.Controls.Button)(target));
            
            #line 119 "..\..\TablaResultadoVotos.xaml"
            this.BtnAumentaAlto.Click += new System.Windows.RoutedEventHandler(this.BtnAumentaAlto_Click);
            
            #line default
            #line hidden
            return;
            case 17:
            this.BtnDisminuyeAlto = ((System.Windows.Controls.Button)(target));
            
            #line 126 "..\..\TablaResultadoVotos.xaml"
            this.BtnDisminuyeAlto.Click += new System.Windows.RoutedEventHandler(this.BtnDisminuyeAlto_Click);
            
            #line default
            #line hidden
            return;
            case 18:
            this.tablaResultado = ((Xceed.Wpf.DataGrid.DataGridControl)(target));
            
            #line 134 "..\..\TablaResultadoVotos.xaml"
            this.tablaResultado.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(this.tablaResultados_PropertyChanged);
            
            #line default
            #line hidden
            return;
            case 20:
            this.impresionViewer = ((System.Windows.Controls.FlowDocumentPageViewer)(target));
            return;
            case 21:
            this.imprimePapel = ((System.Windows.Controls.Button)(target));
            
            #line 187 "..\..\TablaResultadoVotos.xaml"
            this.imprimePapel.Click += new System.Windows.RoutedEventHandler(this.imprimePapel_MouseLeftButtonDown);
            
            #line default
            #line hidden
            return;
            case 22:
            this.BtnTache = ((System.Windows.Controls.Button)(target));
            
            #line 196 "..\..\TablaResultadoVotos.xaml"
            this.BtnTache.Click += new System.Windows.RoutedEventHandler(this.BtnTache_Click);
            
            #line default
            #line hidden
            return;
            case 23:
            this.Esperar = ((System.Windows.Controls.Canvas)(target));
            return;
            case 24:
            this.EsperarStack = ((System.Windows.Controls.StackPanel)(target));
            return;
            case 25:
            this.EsperaBarra = ((System.Windows.Controls.ProgressBar)(target));
            return;
            }
            this._contentLoaded = true;
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        void System.Windows.Markup.IStyleConnector.Connect(int connectionId, object target) {
            System.Windows.EventSetter eventSetter;
            switch (connectionId)
            {
            case 19:
            eventSetter = new System.Windows.EventSetter();
            eventSetter.Event = System.Windows.Controls.Control.MouseDoubleClickEvent;
            
            #line 170 "..\..\TablaResultadoVotos.xaml"
            eventSetter.Handler = new System.Windows.Input.MouseButtonEventHandler(this.tablaResultado_MouseDoubleClick);
            
            #line default
            #line hidden
            ((System.Windows.Style)(target)).Setters.Add(eventSetter);
            break;
            }
        }
    }
}