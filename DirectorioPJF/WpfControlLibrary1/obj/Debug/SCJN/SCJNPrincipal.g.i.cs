﻿#pragma checksum "..\..\..\SCJN\SCJNPrincipal.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "22BF61E053C4FE3C2BA073B28EFABDE5"
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


namespace mx.gob.scjn.directorio.SCJN {
    
    
    /// <summary>
    /// SCJNPrincipal
    /// </summary>
    public partial class SCJNPrincipal : System.Windows.Controls.Page, System.Windows.Markup.IComponentConnector {
        
        
        #line 11 "..\..\..\SCJN\SCJNPrincipal.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Border Borde;
        
        #line default
        #line hidden
        
        
        #line 30 "..\..\..\SCJN\SCJNPrincipal.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button Salir;
        
        #line default
        #line hidden
        
        
        #line 39 "..\..\..\SCJN\SCJNPrincipal.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Frame framContenedor;
        
        #line default
        #line hidden
        
        
        #line 42 "..\..\..\SCJN\SCJNPrincipal.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid grdMenu;
        
        #line default
        #line hidden
        
        
        #line 71 "..\..\..\SCJN\SCJNPrincipal.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label lblMIN;
        
        #line default
        #line hidden
        
        
        #line 74 "..\..\..\SCJN\SCJNPrincipal.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label lblSCJN;
        
        #line default
        #line hidden
        
        
        #line 77 "..\..\..\SCJN\SCJNPrincipal.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label lblAA;
        
        #line default
        #line hidden
        
        
        #line 80 "..\..\..\SCJN\SCJNPrincipal.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label lblFP;
        
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
            System.Uri resourceLocater = new System.Uri("/DirectorioPJF;component/scjn/scjnprincipal.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\SCJN\SCJNPrincipal.xaml"
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
            this.Borde = ((System.Windows.Controls.Border)(target));
            return;
            case 2:
            this.Salir = ((System.Windows.Controls.Button)(target));
            
            #line 32 "..\..\..\SCJN\SCJNPrincipal.xaml"
            this.Salir.Click += new System.Windows.RoutedEventHandler(this.Salir_MouseButtonDown);
            
            #line default
            #line hidden
            return;
            case 3:
            this.framContenedor = ((System.Windows.Controls.Frame)(target));
            return;
            case 4:
            this.grdMenu = ((System.Windows.Controls.Grid)(target));
            return;
            case 5:
            this.lblMIN = ((System.Windows.Controls.Label)(target));
            
            #line 72 "..\..\..\SCJN\SCJNPrincipal.xaml"
            this.lblMIN.MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.VERMIN);
            
            #line default
            #line hidden
            
            #line 72 "..\..\..\SCJN\SCJNPrincipal.xaml"
            this.lblMIN.MouseEnter += new System.Windows.Input.MouseEventHandler(this.lblMIN_MouseEnter);
            
            #line default
            #line hidden
            
            #line 72 "..\..\..\SCJN\SCJNPrincipal.xaml"
            this.lblMIN.MouseLeave += new System.Windows.Input.MouseEventHandler(this.lblMIN_MouseLeave);
            
            #line default
            #line hidden
            return;
            case 6:
            this.lblSCJN = ((System.Windows.Controls.Label)(target));
            
            #line 75 "..\..\..\SCJN\SCJNPrincipal.xaml"
            this.lblSCJN.MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.VERSCJN);
            
            #line default
            #line hidden
            
            #line 75 "..\..\..\SCJN\SCJNPrincipal.xaml"
            this.lblSCJN.MouseEnter += new System.Windows.Input.MouseEventHandler(this.lblSCJN_MouseEnter);
            
            #line default
            #line hidden
            
            #line 75 "..\..\..\SCJN\SCJNPrincipal.xaml"
            this.lblSCJN.MouseLeave += new System.Windows.Input.MouseEventHandler(this.lblSCJN_MouseLeave);
            
            #line default
            #line hidden
            return;
            case 7:
            this.lblAA = ((System.Windows.Controls.Label)(target));
            
            #line 78 "..\..\..\SCJN\SCJNPrincipal.xaml"
            this.lblAA.MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.VERAA);
            
            #line default
            #line hidden
            
            #line 78 "..\..\..\SCJN\SCJNPrincipal.xaml"
            this.lblAA.MouseEnter += new System.Windows.Input.MouseEventHandler(this.lblAA_MouseEnter);
            
            #line default
            #line hidden
            
            #line 78 "..\..\..\SCJN\SCJNPrincipal.xaml"
            this.lblAA.MouseLeave += new System.Windows.Input.MouseEventHandler(this.lblAA_MouseLeave);
            
            #line default
            #line hidden
            return;
            case 8:
            this.lblFP = ((System.Windows.Controls.Label)(target));
            
            #line 81 "..\..\..\SCJN\SCJNPrincipal.xaml"
            this.lblFP.MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.VERFP);
            
            #line default
            #line hidden
            
            #line 81 "..\..\..\SCJN\SCJNPrincipal.xaml"
            this.lblFP.MouseEnter += new System.Windows.Input.MouseEventHandler(this.lblFP_MouseEnter);
            
            #line default
            #line hidden
            
            #line 81 "..\..\..\SCJN\SCJNPrincipal.xaml"
            this.lblFP.MouseLeave += new System.Windows.Input.MouseEventHandler(this.lblFP_MouseLeave);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

