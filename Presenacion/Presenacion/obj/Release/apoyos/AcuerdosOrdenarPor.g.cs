﻿#pragma checksum "..\..\..\apoyos\AcuerdosOrdenarPor.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "2B950F9A6459752B71FD5C5829372F90"
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
using Xceed.Wpf.DataGrid;
using Xceed.Wpf.DataGrid.Views;


namespace mx.gob.scjn.ius_common.gui.apoyos {
    
    
    /// <summary>
    /// AcuerdosOrdenarPor
    /// </summary>
    public partial class AcuerdosOrdenarPor : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 12 "..\..\..\apoyos\AcuerdosOrdenarPor.xaml"
        internal System.Windows.Controls.Border degradado;
        
        #line default
        #line hidden
        
        
        #line 15 "..\..\..\apoyos\AcuerdosOrdenarPor.xaml"
        internal System.Windows.Controls.Label Titulo;
        
        #line default
        #line hidden
        
        
        #line 20 "..\..\..\apoyos\AcuerdosOrdenarPor.xaml"
        internal System.Windows.Controls.Button Salir;
        
        #line default
        #line hidden
        
        
        #line 35 "..\..\..\apoyos\AcuerdosOrdenarPor.xaml"
        internal System.Windows.Controls.Label LblLoc;
        
        #line default
        #line hidden
        
        
        #line 40 "..\..\..\apoyos\AcuerdosOrdenarPor.xaml"
        internal System.Windows.Controls.Label LblRubro;
        
        #line default
        #line hidden
        
        
        #line 45 "..\..\..\apoyos\AcuerdosOrdenarPor.xaml"
        internal System.Windows.Controls.Label LblIus;
        
        #line default
        #line hidden
        
        
        #line 53 "..\..\..\apoyos\AcuerdosOrdenarPor.xaml"
        internal System.Windows.Shapes.Rectangle BarraMovimiento;
        
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
            System.Uri resourceLocater = new System.Uri("/Presenacion;component/apoyos/acuerdosordenarpor.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\apoyos\AcuerdosOrdenarPor.xaml"
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
            this.degradado = ((System.Windows.Controls.Border)(target));
            return;
            case 2:
            this.Titulo = ((System.Windows.Controls.Label)(target));
            return;
            case 3:
            this.Salir = ((System.Windows.Controls.Button)(target));
            
            #line 25 "..\..\..\apoyos\AcuerdosOrdenarPor.xaml"
            this.Salir.Click += new System.Windows.RoutedEventHandler(this.Salir_MouseLeftButtonDown);
            
            #line default
            #line hidden
            return;
            case 4:
            this.LblLoc = ((System.Windows.Controls.Label)(target));
            
            #line 37 "..\..\..\apoyos\AcuerdosOrdenarPor.xaml"
            this.LblLoc.MouseEnter += new System.Windows.Input.MouseEventHandler(this.LblLoc_MouseEnter);
            
            #line default
            #line hidden
            
            #line 38 "..\..\..\apoyos\AcuerdosOrdenarPor.xaml"
            this.LblLoc.MouseLeave += new System.Windows.Input.MouseEventHandler(this.LblLoc_MouseLeave);
            
            #line default
            #line hidden
            
            #line 39 "..\..\..\apoyos\AcuerdosOrdenarPor.xaml"
            this.LblLoc.MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.LblLoc_MouseLeftButtonDown);
            
            #line default
            #line hidden
            return;
            case 5:
            this.LblRubro = ((System.Windows.Controls.Label)(target));
            
            #line 42 "..\..\..\apoyos\AcuerdosOrdenarPor.xaml"
            this.LblRubro.MouseEnter += new System.Windows.Input.MouseEventHandler(this.LblLoc_MouseEnter);
            
            #line default
            #line hidden
            
            #line 43 "..\..\..\apoyos\AcuerdosOrdenarPor.xaml"
            this.LblRubro.MouseLeave += new System.Windows.Input.MouseEventHandler(this.LblLoc_MouseLeave);
            
            #line default
            #line hidden
            
            #line 44 "..\..\..\apoyos\AcuerdosOrdenarPor.xaml"
            this.LblRubro.MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.LblRubro_MouseLeftButtonDown);
            
            #line default
            #line hidden
            return;
            case 6:
            this.LblIus = ((System.Windows.Controls.Label)(target));
            
            #line 47 "..\..\..\apoyos\AcuerdosOrdenarPor.xaml"
            this.LblIus.MouseEnter += new System.Windows.Input.MouseEventHandler(this.LblLoc_MouseEnter);
            
            #line default
            #line hidden
            
            #line 48 "..\..\..\apoyos\AcuerdosOrdenarPor.xaml"
            this.LblIus.MouseLeave += new System.Windows.Input.MouseEventHandler(this.LblLoc_MouseLeave);
            
            #line default
            #line hidden
            
            #line 49 "..\..\..\apoyos\AcuerdosOrdenarPor.xaml"
            this.LblIus.MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.LblIus_MouseLeftButtonDown);
            
            #line default
            #line hidden
            return;
            case 7:
            this.BarraMovimiento = ((System.Windows.Shapes.Rectangle)(target));
            
            #line 58 "..\..\..\apoyos\AcuerdosOrdenarPor.xaml"
            this.BarraMovimiento.MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.BarraMovimiento_MouseLeftButtonDown);
            
            #line default
            #line hidden
            
            #line 59 "..\..\..\apoyos\AcuerdosOrdenarPor.xaml"
            this.BarraMovimiento.MouseLeftButtonUp += new System.Windows.Input.MouseButtonEventHandler(this.BarraMovimiento_MouseLeftButtonUp);
            
            #line default
            #line hidden
            
            #line 60 "..\..\..\apoyos\AcuerdosOrdenarPor.xaml"
            this.BarraMovimiento.MouseMove += new System.Windows.Input.MouseEventHandler(this.BarraMovimiento_MouseMove);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}
