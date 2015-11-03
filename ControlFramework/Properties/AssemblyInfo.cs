﻿using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;
using System.Windows;
using System.Windows.Markup;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("JaStDev ControlFramework")]
[assembly: AssemblyDescription("A free WPF control library.")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("JaStDev")]
[assembly: AssemblyProduct("ControlFramework")]
[assembly: AssemblyCopyright("Copyright ©  2007-2008")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

//In order to begin building localizable applications, set 
//<UICulture>CultureYouAreCodingWith</UICulture> in your .csproj file
//inside a <PropertyGroup>.  For example, if you are using US english
//in your source files, set the <UICulture> to en-US.  Then uncomment
//the NeutralResourceLanguage attribute below.  Update the "en-US" in
//the line below to match the UICulture setting in the project file.

//[assembly: NeutralResourcesLanguage("en-US", UltimateResourceFallbackLocation.Satellite)]

[assembly: ThemeInfo(
    ResourceDictionaryLocation.None, //where theme specific resource dictionaries are located
   //(used if a resource is not found in the page, 
   // or application resource dictionaries)
    ResourceDictionaryLocation.SourceAssembly //where the generic resource dictionary is located
   //(used if a resource is not found in the page, 
   // app, or any theme specific resource dictionaries)
)]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers 
// by using the '*' as shown below:
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion("1.5.0.0")]
[assembly: AssemblyFileVersion("1.5")]

//added myself
[assembly: CLSCompliant(true)]
[assembly: AllowPartiallyTrustedCallers]

[assembly: XmlnsPrefix("http://schemas.jastdev.ControlFramework/winfx/2007/xaml/presentation", "jf")]
[assembly: XmlnsDefinition("http://schemas.jastdev.ControlFramework/winfx/2007/xaml/presentation", "JaStDev.ControlFramework.Controls")]
[assembly: XmlnsDefinition("http://schemas.jastdev.ControlFramework/winfx/2007/xaml/presentation", "JaStDev.ControlFramework.Controls.Primitives")]
[assembly: XmlnsDefinition("http://schemas.jastdev.ControlFramework/winfx/2007/xaml/presentation", "JaStDev.ControlFramework.Utility")]
[assembly: XmlnsDefinition("http://schemas.jastdev.ControlFramework/winfx/2007/xaml/presentation", "JaStDev.ControlFramework.Input")]
