using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace JaStDev.ControlFramework.Utility
{
   /// <summary>
   /// A general purpose class that can be used to declare TreeViewItems as resources that can be reused as sub items through an ItemsSource binding.
   /// </summary>
   /// <remarks>
   /// This object should be put in a list that can be used as the ItemsSource of a treeViewItem that uses <see cref="TreeItemObjectTemplateSelector"/>
   /// as it's ItemTemplateSelector.
   /// </remarks>
   /// <example>
   /// <code lang="xml">
   /// <![CDATA[
   /// <DataTemplate DataType="{x:Type a:TreeItemObject}">
   ///     <TextBlock Text="{Binding Path=ItemsSourcePath}"/>
   /// </DataTemplate>
   /// <HierarchicalDataTemplate x:Key="DevciesTreeItemObject"
   ///                          ItemsSource="{Binding Path=Devices}">         
   /// </HierarchicalDataTemplate>
   /// 
   /// <HierarchicalDataTemplate x:Key="EventsTreeItemObject"
   ///                          ItemsSource="{Binding Path=Events}">         
   /// </HierarchicalDataTemplate>
   /// 
   /// <a:TreeItemObjectTemplateSelector x:Key="SiteTypesSelector"/>
   /// 
   /// <col:ArrayList x:Key="SiteTypes">
   ///      <a:TreeItemObject ItemsSourcePath="Devices"/>
   ///      <a:TreeItemObject ItemsSourcePath="Events"/>
   /// </col:ArrayList>
   /// <TreeView ItemsSource="{Binding Source={StaticResource SiteTypes}}"/>
   /// ]]>
   /// </code>
   /// </example>
   public class TreeItemObject
   {
      /// <summary>
      /// Gets/sets the string that is put in front of 'TreeItemObject' for searching the hiearchical template to use. 
      /// </summary>
      public string ItemsSourcePath { get; set; }
   }



   /// <summary>
   /// Selects the correct DataTemplate for <see cref="TreeItemObject"/>s.
   /// </summary>
   /// <remarks>
   /// This selector searches for templats that start with the ItemsSourcePath defined in the <see cref="TreeItemObject"/>,
   /// followed by 'TreeItemObject'.  So for instance, if the ItemsSourcePath is 'x', a Template called 'xTreeItemObject' is
   /// searched for.
   /// </remarks>
   public class TreeItemObjectTemplateSelector : DataTemplateSelector
   {
      public override DataTemplate SelectTemplate(object item, DependencyObject container)
      {
         TreeItemObject iItem = item as TreeItemObject;
         FrameworkElement iCont = container as FrameworkElement;
         if (iItem != null && iCont != null)
         {
            DependencyObject iTemp = VisualTreeHelper.GetParent(iCont);
            while(iTemp != null && !(iTemp is FrameworkElement))                                         //we need to copy over the datacontext so that the bindings can work correctly.
               iTemp = VisualTreeHelper.GetParent(iTemp);
            if (iTemp != null)
            {
               iCont.DataContext = ((FrameworkElement)iTemp).DataContext;
               return iCont.TryFindResource(iItem.ItemsSourcePath + "TreeItemObject") as DataTemplate;
            }
         }
         return null;
      }

   }
}
