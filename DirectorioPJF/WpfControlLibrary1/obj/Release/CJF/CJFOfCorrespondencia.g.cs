﻿#pragma checksum "..\..\..\CJF\CJFOfCorrespondencia.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "218216A7DF55272CE9DA9FD0CD043C5A"
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
using mx.gob.scjn.directorio;


namespace mx.gob.scjn.directorio.CJF {
    
    
    /// <summary>
    /// CJFOfCorrespondencia
    /// </summary>
    public partial class CJFOfCorrespondencia : System.Windows.Controls.Page, System.Windows.Markup.IComponentConnector {
        
        
        #line 14 "..\..\..\CJF\CJFOfCorrespondencia.xaml"
        internal System.Windows.Controls.Border Borde;
        
        #line default
        #line hidden
        
        
        #line 26 "..\..\..\CJF\CJFOfCorrespondencia.xaml"
        internal System.Windows.Controls.Grid grdOC;
        
        #line default
        #line hidden
        
        
        #line 36 "..\..\..\CJF\CJFOfCorrespondencia.xaml"
        internal System.Windows.Shapes.Rectangle rectangleGrid;
        
        #line default
        #line hidden
        
        
        #line 42 "..\..\..\CJF\CJFOfCorrespondencia.xaml"
        internal Xceed.Wpf.DataGrid.DataGridControl AreasAdmin;
        
        #line default
        #line hidden
        
        
        #line 62 "..\..\..\CJF\CJFOfCorrespondencia.xaml"
        internal mx.gob.scjn.directorio.FondoTransparente FTransparente;
        
        #line default
        #line hidden
        
        
        #line 63 "..\..\..\CJF\CJFOfCorrespondencia.xaml"
        internal mx.gob.scjn.directorio.MensajesAvisos Aviso;
        
        #line default
        #line hidden
        
        
        #line 64 "..\..\..\CJF\CJFOfCorrespondencia.xaml"
        internal mx.gob.scjn.directorio.OpcionesImprimir OpImprimir;
        
        #line default
        #line hidden
        
        
        #line 66 "..\..\..\CJF\CJFOfCorrespondencia.xaml"
        internal System.Windows.Controls.Grid grid1;
        
        #line default
        #line hidden
        
        
        #line 72 "..\..\..\CJF\CJFOfCorrespondencia.xaml"
        internal System.Windows.Controls.Grid grdBotones;
        
        #line default
        #line hidden
        
        
        #line 80 "..\..\..\CJF\CJFOfCorrespondencia.xaml"
        internal System.Windows.Controls.Button Guardar_;
        
        #line default
        #line hidden
        
        
        #line 85 "..\..\..\CJF\CJFOfCorrespondencia.xaml"
        internal System.Windows.Controls.Button Imprimir_;
        
        #line default
        #line hidden
        
        
        #line 90 "..\..\..\CJF\CJFOfCorrespondencia.xaml"
        internal System.Windows.Controls.Button PortaPapeles_;
        
        #line default
        #line hidden
        
        
        #line 101 "..\..\..\CJF\CJFOfCorrespondencia.xaml"
        internal System.Windows.Controls.FlowDocumentPageViewer impresion;
        
        #line default
        #line hidden
        
        
        #line 103 "..\..\..\CJF\CJFOfCorrespondencia.xaml"
        internal System.Windows.Controls.RichTextBox contenidoTexto;
        
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
            System.Uri resourceLocater = new System.Uri("/DirectorioPJF;component/cjf/cjfofcorrespondencia.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\CJF\CJFOfCorrespondencia.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal System.Delegate _CreateDelegate(System.Type delegateType, string handler) {
            return System.Delegate.CreateDelegate(delegateType, this, handler);
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.Borde = ((System.Windows.Controls.Border)(target));
            return;
            case 2:
            this.grdOC = ((System.Windows.Controls.Grid)(target));
            return;
            case 3:
            this.rectangleGrid = ((System.Windows.Shapes.Rectangle)(target));
            return;
            case 4:
            this.AreasAdmin = ((Xceed.Wpf.DataGrid.DataGridControl)(target));
            
            #line 42 "..\..\..\CJF\CJFOfCorrespondencia.xaml"
            this.AreasAdmin.MouseLeftButtonUp += new System.Windows.Input.MouseButtonEventHandler(this.CargaDetalle);
            
            #line default
            #line hidden
            return;
            case 5:
            this.FTransparente = ((mx.gob.scjn.directorio.FondoTransparente)(target));
            return;
            case 6:
            this.Aviso = ((mx.gob.scjn.directorio.MensajesAvisos)(target));
            return;
            case 7:
            this.OpImprimir = ((mx.gob.scjn.directorio.OpcionesImprimir)(target));
            return;
            case 8:
            this.grid1 = ((System.Windows.Controls.Grid)(target));
            return;
            case 9:
            this.grdBotones = ((System.Windows.Controls.Grid)(target));
            return;
            case 10:
            this.Guardar_ = ((System.Windows.Controls.Button)(target));
            
            #line 80 "..\..\..\CJF\CJFOfCorrespondencia.xaml"
            this.Guardar_.Click += new System.Windows.RoutedEventHandler(this.Guardar_Click);
            
            #line default
            #line hidden
            return;
            case 11:
            this.Imprimir_ = ((System.Windows.Controls.Button)(target));
            
            #line 85 "..\..\..\CJF\CJFOfCorrespondencia.xaml"
            this.Imprimir_.Click += new System.Windows.RoutedEventHandler(this.Imprimir_Click);
            
            #line default
            #line hidden
            return;
            case 12:
            this.PortaPapeles_ = ((System.Windows.Controls.Button)(target));
            
            #line 90 "..\..\..\CJF\CJFOfCorrespondencia.xaml"
            this.PortaPapeles_.Click += new System.Windows.RoutedEventHandler(this.PortaPapeles_Click);
            
            #line default
            #line hidden
            return;
            case 13:
            this.impresion = ((System.Windows.Controls.FlowDocumentPageViewer)(target));
            return;
            case 14:
            this.contenidoTexto = ((System.Windows.Controls.RichTextBox)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}
