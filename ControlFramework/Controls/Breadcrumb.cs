using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace JaStDev.ControlFramework.Controls
{
   /// <summary>
   /// Represents 1 item in a <see cref="BreadcrumbsControl"/>
   /// </summary>
   /// <remarks>
   /// <para>
   /// When changing the template, don't forget to put a button called 'BtnBreadcrumb' somewhere.  This is used to clip the
   /// path up to the selected breadcrumb.
   /// </para>
   /// </remarks>
   [StyleTypedProperty(Property = "ItemContainerStyle", StyleTargetType = typeof(Breadcrumb))]
   [TemplatePartAttribute(Name = "BtnBreadcrumb", Type = typeof(ButtonBase))]
   public class Breadcrumb : HeaderedItemsControl
   {

      static Breadcrumb()
      {
         DefaultStyleKeyProperty.OverrideMetadata(typeof(Breadcrumb), new FrameworkPropertyMetadata(typeof(Breadcrumb)));
      }

      #region Prop
      
      #endregion


      #region Functions

      /// <summary>
      /// Determines if the specified item is (or is eligible to be) its own container.
      /// </summary>
      /// <param name="item">The item to check.</param>
      /// <returns>
      /// true if the item is (or is eligible to be) its own container; otherwise, false.
      /// </returns>
      protected override bool IsItemItsOwnContainerOverride(object item)
      {
         return item is Breadcrumb;
      }

      /// <summary>
      /// Creates or identifies the element that is used to display the given item.
      /// </summary>
      /// <returns>
      /// The element that is used to display the given item.
      /// </returns>
      protected override DependencyObject GetContainerForItemOverride()
      {
         return new Breadcrumb();
      }


      #endregion

   }
}
