﻿#pragma checksum "..\..\ConsultasEspeciales.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "3D66FC1D706B3172A68E5A9BD96F9525"
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


namespace IUS {
    
    
    /// <summary>
    /// ConsultasEspeciales
    /// </summary>
    public partial class ConsultasEspeciales : System.Windows.Controls.Page, System.Windows.Markup.IComponentConnector {
        
        
        #line 8 "..\..\ConsultasEspeciales.xaml"
        internal System.Windows.Controls.Border Borde;
        
        #line default
        #line hidden
        
        
        #line 11 "..\..\ConsultasEspeciales.xaml"
        internal System.Windows.Controls.TreeView treeView1;
        
        #line default
        #line hidden
        
        
        #line 32 "..\..\ConsultasEspeciales.xaml"
        internal System.Windows.Controls.Label label1;
        
        #line default
        #line hidden
        
        
        #line 38 "..\..\ConsultasEspeciales.xaml"
        internal System.Windows.Controls.Button Salir;
        
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
            System.Uri resourceLocater = new System.Uri("/Presenacion;component/consultasespeciales.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\ConsultasEspeciales.xaml"
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
            
            #line 4 "..\..\ConsultasEspeciales.xaml"
            ((IUS.ConsultasEspeciales)(target)).Loaded += new System.Windows.RoutedEventHandler(this.Page_Loaded);
            
            #line default
            #line hidden
            return;
            case 2:
            this.Borde = ((System.Windows.Controls.Border)(target));
            return;
            case 3:
            this.treeView1 = ((System.Windows.Controls.TreeView)(target));
            
            #line 11 "..\..\ConsultasEspeciales.xaml"
            this.treeView1.MouseDoubleClick += new System.Windows.Input.MouseButtonEventHandler(this.treeView1_MouseDoubleClick);
            
            #line default
            #line hidden
            
            #line 12 "..\..\ConsultasEspeciales.xaml"
            this.treeView1.SelectedItemChanged += new System.Windows.RoutedPropertyChangedEventHandler<object>(this.treeView1_SelectedItemChanged);
            
            #line default
            #line hidden
            return;
            case 4:
            
            #line 13 "..\..\ConsultasEspeciales.xaml"
            ((System.Windows.Controls.TreeViewItem)(target)).Selected += new System.Windows.RoutedEventHandler(this.TreeViewItem_Selected);
            
            #line default
            #line hidden
            return;
            case 5:
            this.label1 = ((System.Windows.Controls.Label)(target));
            return;
            case 6:
            this.Salir = ((System.Windows.Controls.Button)(target));
            
            #line 42 "..\..\ConsultasEspeciales.xaml"
            this.Salir.Click += new System.Windows.RoutedEventHandler(this.Salir_MouseLeftButtonDown);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}