﻿#pragma checksum "..\..\..\CJF\CJFPrincipal.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "A18D6084A27F180B9FC9BE432E9CA637"
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


namespace mx.gob.scjn.directorio.CJF {
    
    
    /// <summary>
    /// CJFPrincipal
    /// </summary>
    public partial class CJFPrincipal : System.Windows.Controls.Page, System.Windows.Markup.IComponentConnector {
        
        
        #line 11 "..\..\..\CJF\CJFPrincipal.xaml"
        internal System.Windows.Controls.Border Borde;
        
        #line default
        #line hidden
        
        
        #line 32 "..\..\..\CJF\CJFPrincipal.xaml"
        internal System.Windows.Controls.Button Salir;
        
        #line default
        #line hidden
        
        
        #line 41 "..\..\..\CJF\CJFPrincipal.xaml"
        internal System.Windows.Controls.Frame framContenedor;
        
        #line default
        #line hidden
        
        
        #line 42 "..\..\..\CJF\CJFPrincipal.xaml"
        internal System.Windows.Controls.Grid grdMenu;
        
        #line default
        #line hidden
        
        
        #line 70 "..\..\..\CJF\CJFPrincipal.xaml"
        internal System.Windows.Controls.Label lblAAOA;
        
        #line default
        #line hidden
        
        
        #line 71 "..\..\..\CJF\CJFPrincipal.xaml"
        internal System.Windows.Controls.Label lblCJF;
        
        #line default
        #line hidden
        
        
        #line 72 "..\..\..\CJF\CJFPrincipal.xaml"
        internal System.Windows.Controls.Label lblFuncAdmin;
        
        #line default
        #line hidden
        
        
        #line 73 "..\..\..\CJF\CJFPrincipal.xaml"
        internal System.Windows.Controls.Label lblJuecMag;
        
        #line default
        #line hidden
        
        
        #line 74 "..\..\..\CJF\CJFPrincipal.xaml"
        internal System.Windows.Controls.Label lblAA;
        
        #line default
        #line hidden
        
        
        #line 75 "..\..\..\CJF\CJFPrincipal.xaml"
        internal System.Windows.Controls.Label lblTUC;
        
        #line default
        #line hidden
        
        
        #line 76 "..\..\..\CJF\CJFPrincipal.xaml"
        internal System.Windows.Controls.Label lblTCC;
        
        #line default
        #line hidden
        
        
        #line 77 "..\..\..\CJF\CJFPrincipal.xaml"
        internal System.Windows.Controls.Label lblJUZ;
        
        #line default
        #line hidden
        
        
        #line 78 "..\..\..\CJF\CJFPrincipal.xaml"
        internal System.Windows.Controls.Label lblCON;
        
        #line default
        #line hidden
        
        
        #line 79 "..\..\..\CJF\CJFPrincipal.xaml"
        internal System.Windows.Controls.Label lblOfC;
        
        #line default
        #line hidden
        
        
        #line 80 "..\..\..\CJF\CJFPrincipal.xaml"
        internal System.Windows.Controls.Label lblCircuitos;
        
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
            System.Uri resourceLocater = new System.Uri("/DirectorioPJF;component/cjf/cjfprincipal.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\CJF\CJFPrincipal.xaml"
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
            this.Borde = ((System.Windows.Controls.Border)(target));
            return;
            case 2:
            this.Salir = ((System.Windows.Controls.Button)(target));
            
            #line 34 "..\..\..\CJF\CJFPrincipal.xaml"
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
            this.lblAAOA = ((System.Windows.Controls.Label)(target));
            
            #line 70 "..\..\..\CJF\CJFPrincipal.xaml"
            this.lblAAOA.MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.VerOA);
            
            #line default
            #line hidden
            
            #line 70 "..\..\..\CJF\CJFPrincipal.xaml"
            this.lblAAOA.MouseEnter += new System.Windows.Input.MouseEventHandler(this.lblAAOA_MouseEnter);
            
            #line default
            #line hidden
            
            #line 70 "..\..\..\CJF\CJFPrincipal.xaml"
            this.lblAAOA.MouseLeave += new System.Windows.Input.MouseEventHandler(this.lblAAOA_MouseLeave);
            
            #line default
            #line hidden
            return;
            case 6:
            this.lblCJF = ((System.Windows.Controls.Label)(target));
            
            #line 71 "..\..\..\CJF\CJFPrincipal.xaml"
            this.lblCJF.MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.VerCJF);
            
            #line default
            #line hidden
            
            #line 71 "..\..\..\CJF\CJFPrincipal.xaml"
            this.lblCJF.MouseEnter += new System.Windows.Input.MouseEventHandler(this.lblCJF_MouseEnter);
            
            #line default
            #line hidden
            
            #line 71 "..\..\..\CJF\CJFPrincipal.xaml"
            this.lblCJF.MouseLeave += new System.Windows.Input.MouseEventHandler(this.lblCJF_MouseLeave);
            
            #line default
            #line hidden
            return;
            case 7:
            this.lblFuncAdmin = ((System.Windows.Controls.Label)(target));
            
            #line 72 "..\..\..\CJF\CJFPrincipal.xaml"
            this.lblFuncAdmin.MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.VerFuncAdmin);
            
            #line default
            #line hidden
            
            #line 72 "..\..\..\CJF\CJFPrincipal.xaml"
            this.lblFuncAdmin.MouseEnter += new System.Windows.Input.MouseEventHandler(this.lblFuncAdmin_MouseEnter);
            
            #line default
            #line hidden
            
            #line 72 "..\..\..\CJF\CJFPrincipal.xaml"
            this.lblFuncAdmin.MouseLeave += new System.Windows.Input.MouseEventHandler(this.lblFuncAdmin_MouseLeave);
            
            #line default
            #line hidden
            return;
            case 8:
            this.lblJuecMag = ((System.Windows.Controls.Label)(target));
            
            #line 73 "..\..\..\CJF\CJFPrincipal.xaml"
            this.lblJuecMag.MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.VerJuecesMag);
            
            #line default
            #line hidden
            
            #line 73 "..\..\..\CJF\CJFPrincipal.xaml"
            this.lblJuecMag.MouseEnter += new System.Windows.Input.MouseEventHandler(this.lblJuecMag_MouseEnter);
            
            #line default
            #line hidden
            
            #line 73 "..\..\..\CJF\CJFPrincipal.xaml"
            this.lblJuecMag.MouseLeave += new System.Windows.Input.MouseEventHandler(this.lblJuecMag_MouseLeave);
            
            #line default
            #line hidden
            return;
            case 9:
            this.lblAA = ((System.Windows.Controls.Label)(target));
            
            #line 74 "..\..\..\CJF\CJFPrincipal.xaml"
            this.lblAA.MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.VerAA);
            
            #line default
            #line hidden
            
            #line 74 "..\..\..\CJF\CJFPrincipal.xaml"
            this.lblAA.MouseEnter += new System.Windows.Input.MouseEventHandler(this.lblAA_MouseEnter);
            
            #line default
            #line hidden
            
            #line 74 "..\..\..\CJF\CJFPrincipal.xaml"
            this.lblAA.MouseLeave += new System.Windows.Input.MouseEventHandler(this.lblAA_MouseLeave);
            
            #line default
            #line hidden
            return;
            case 10:
            this.lblTUC = ((System.Windows.Controls.Label)(target));
            
            #line 75 "..\..\..\CJF\CJFPrincipal.xaml"
            this.lblTUC.MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.VERTUC);
            
            #line default
            #line hidden
            
            #line 75 "..\..\..\CJF\CJFPrincipal.xaml"
            this.lblTUC.MouseEnter += new System.Windows.Input.MouseEventHandler(this.lblTUC_MouseEnter);
            
            #line default
            #line hidden
            
            #line 75 "..\..\..\CJF\CJFPrincipal.xaml"
            this.lblTUC.MouseLeave += new System.Windows.Input.MouseEventHandler(this.lblTUC_MouseLeave);
            
            #line default
            #line hidden
            return;
            case 11:
            this.lblTCC = ((System.Windows.Controls.Label)(target));
            
            #line 76 "..\..\..\CJF\CJFPrincipal.xaml"
            this.lblTCC.MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.VERTCC);
            
            #line default
            #line hidden
            
            #line 76 "..\..\..\CJF\CJFPrincipal.xaml"
            this.lblTCC.MouseEnter += new System.Windows.Input.MouseEventHandler(this.lblTCC_MouseEnter);
            
            #line default
            #line hidden
            
            #line 76 "..\..\..\CJF\CJFPrincipal.xaml"
            this.lblTCC.MouseLeave += new System.Windows.Input.MouseEventHandler(this.lblTCC_MouseLeave);
            
            #line default
            #line hidden
            return;
            case 12:
            this.lblJUZ = ((System.Windows.Controls.Label)(target));
            
            #line 77 "..\..\..\CJF\CJFPrincipal.xaml"
            this.lblJUZ.MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.VERJUZ);
            
            #line default
            #line hidden
            
            #line 77 "..\..\..\CJF\CJFPrincipal.xaml"
            this.lblJUZ.MouseEnter += new System.Windows.Input.MouseEventHandler(this.lblJUZ_MouseEnter);
            
            #line default
            #line hidden
            
            #line 77 "..\..\..\CJF\CJFPrincipal.xaml"
            this.lblJUZ.MouseLeave += new System.Windows.Input.MouseEventHandler(this.lblJUZ_MouseLeave);
            
            #line default
            #line hidden
            return;
            case 13:
            this.lblCON = ((System.Windows.Controls.Label)(target));
            
            #line 78 "..\..\..\CJF\CJFPrincipal.xaml"
            this.lblCON.MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.VERCONS);
            
            #line default
            #line hidden
            
            #line 78 "..\..\..\CJF\CJFPrincipal.xaml"
            this.lblCON.MouseEnter += new System.Windows.Input.MouseEventHandler(this.lblCON_MouseEnter);
            
            #line default
            #line hidden
            
            #line 78 "..\..\..\CJF\CJFPrincipal.xaml"
            this.lblCON.MouseLeave += new System.Windows.Input.MouseEventHandler(this.lblCON_MouseLeave);
            
            #line default
            #line hidden
            return;
            case 14:
            this.lblOfC = ((System.Windows.Controls.Label)(target));
            
            #line 79 "..\..\..\CJF\CJFPrincipal.xaml"
            this.lblOfC.MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.VEROC);
            
            #line default
            #line hidden
            
            #line 79 "..\..\..\CJF\CJFPrincipal.xaml"
            this.lblOfC.MouseEnter += new System.Windows.Input.MouseEventHandler(this.lblOC_MouseEnter);
            
            #line default
            #line hidden
            
            #line 79 "..\..\..\CJF\CJFPrincipal.xaml"
            this.lblOfC.MouseLeave += new System.Windows.Input.MouseEventHandler(this.lblOC_MouseLeave);
            
            #line default
            #line hidden
            return;
            case 15:
            this.lblCircuitos = ((System.Windows.Controls.Label)(target));
            
            #line 80 "..\..\..\CJF\CJFPrincipal.xaml"
            this.lblCircuitos.MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.VERCT);
            
            #line default
            #line hidden
            
            #line 80 "..\..\..\CJF\CJFPrincipal.xaml"
            this.lblCircuitos.MouseEnter += new System.Windows.Input.MouseEventHandler(this.lblCircuitos_MouseEnter);
            
            #line default
            #line hidden
            
            #line 80 "..\..\..\CJF\CJFPrincipal.xaml"
            this.lblCircuitos.MouseLeave += new System.Windows.Input.MouseEventHandler(this.lblCircuitos_MouseLeave);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}
