﻿#pragma checksum "..\..\..\General\QueImprimir.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "29FAC8544EB17EB2F782984FA1BE876E"
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


namespace mx.gob.scjn.directorio.General {
    
    
    /// <summary>
    /// QueImprimir
    /// </summary>
    public partial class QueImprimir : System.Windows.Controls.Page, System.Windows.Markup.IComponentConnector {
        
        
        #line 27 "..\..\..\General\QueImprimir.xaml"
        internal System.Windows.Controls.Label lblImpr;
        
        #line default
        #line hidden
        
        
        #line 28 "..\..\..\General\QueImprimir.xaml"
        internal System.Windows.Controls.Label lblWait;
        
        #line default
        #line hidden
        
        
        #line 29 "..\..\..\General\QueImprimir.xaml"
        internal System.Windows.Controls.RadioButton rbActual;
        
        #line default
        #line hidden
        
        
        #line 30 "..\..\..\General\QueImprimir.xaml"
        internal System.Windows.Controls.RadioButton rbIntegrantes;
        
        #line default
        #line hidden
        
        
        #line 31 "..\..\..\General\QueImprimir.xaml"
        internal System.Windows.Controls.Button btnOk;
        
        #line default
        #line hidden
        
        
        #line 32 "..\..\..\General\QueImprimir.xaml"
        internal System.Windows.Controls.Button btnCancel;
        
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
            System.Uri resourceLocater = new System.Uri("/DirectorioPJF;component/general/queimprimir.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\General\QueImprimir.xaml"
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
            this.lblImpr = ((System.Windows.Controls.Label)(target));
            return;
            case 2:
            this.lblWait = ((System.Windows.Controls.Label)(target));
            return;
            case 3:
            this.rbActual = ((System.Windows.Controls.RadioButton)(target));
            return;
            case 4:
            this.rbIntegrantes = ((System.Windows.Controls.RadioButton)(target));
            return;
            case 5:
            this.btnOk = ((System.Windows.Controls.Button)(target));
            return;
            case 6:
            this.btnCancel = ((System.Windows.Controls.Button)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}
