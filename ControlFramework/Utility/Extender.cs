using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media.Animation;
using JaStDev.ControlFramework.Controls;

namespace JaStDev.ControlFramework.Utility
{
   /// <summary>
   /// The event arguments for <see cref="ExtentionHandler"/>
   /// </summary>
   public class ExtentionEventArgs : EventArgs
   {
      DependencyObject fOriginalSource;

      /// <summary>
      /// Gets/sets the object that triggered the event.
      /// </summary>
      public DependencyObject OriginalSource
      {
         get { return fOriginalSource; }
         set { fOriginalSource = value; }
      }
   }

   /// <summary>
   /// The event handler definition for events triggered by an <see cref="Extention"/>.
   /// </summary>
   /// <param name="sender">The <see cref="Extention"/> that triggered the event.</param>
   /// <param name="e">The parameters for the event.</param>
   public delegate void ExtentionHandler(object sender, ExtentionEventArgs e);

   /// <summary>
   /// This class defines which extentions can be used in a <see cref="Extender"/>.
   /// </summary>
   /// <remarks>
   /// <para>
   /// <see cref="Extender"/> has a dictionary (<see cref="Extender.Extentions"/>) containing Extentions, the key value for each extention is the
   /// name that is used to identify in in xaml.
   /// </para>
   /// <para>
   /// There is an event that is raised when the extention is assigned to an object: <see cref="Extention.Register"/>.  This usually assigns events
   /// to the object that the extention is assigned too.  <see cref="Extention.UnRegister"/> is called when extention is removed from an object.  
   /// usually this event handler removes the previously registered events from the object.
   /// </para>
   /// </remarks>
   public class Extention
   {
      /// <summary>
      /// Called when this extention is assigned to an object.
      /// </summary>
      public event ExtentionHandler Register;
      /// <summary>
      /// Called when this extention is removed from an object.
      /// </summary>
      public event ExtentionHandler UnRegister;


      internal void OnUnRegister(DependencyObject sender)
      {
         if (UnRegister != null)
         {
            ExtentionEventArgs iAgrs = new ExtentionEventArgs();
            if (iAgrs != null)
            {
               iAgrs.OriginalSource = sender;
               UnRegister(this, iAgrs);
            }
         }
      }

      internal void OnRegister(DependencyObject sender)
      {
         if (Register != null)
         {
            ExtentionEventArgs iAgrs = new ExtentionEventArgs();
            if (iAgrs != null)
            {
               iAgrs.OriginalSource = sender;
               Register(this, iAgrs);
            }
         }
      }
   }

   /// <summary>
   /// Provides functionality for extending the functionality of an object.
   /// </summary>
   /// <remarks>
   /// <para>
   /// This class provides an attached property that can be used to assign extra/specific 'functionality' to a WPF element through xaml.
   /// This is usefull in situations where you can't use events (code behind) like when reading uncompiled xaml files.
   /// </para>
   /// <para>
   /// The object contains a dictionary of <see cref="Extention"/> objects which you assign to it.  The key of each Extention object defines a value that
   /// can be used in the attached property <see cref="Extender.SetFunctionality"/>.
   /// </para>
   /// <para>
   /// Default available extentions:
   /// <list type="bullet">
   ///   <item>
   ///      DockSplitterToExpander: Apply this functionality to a button on a <see cref="DockSplitter"/>'s Template to let it function as an Expander.
   ///      By clicking on the button, the user can expand/collaps the object just in front of the splitter.  Side effect: this functionality stores
   ///      a value in the Tag property of the button used to expand/collaps.
   ///   </item> 
   ///   <item>
   ///      ListBoxSelectOnFocus: Apply this to a control that contains ListBoxItems (usually a ListBox, or a control that contains a listbox). 
   ///      When applied, listbox items will be selected as soon as they get focus.  This is usefull in keyboard only situations or when the
   ///      control key needs to be pressed while still making selections.
   ///   </item>
   /// </list>
   /// </para>
   /// </remarks>
   public class Extender : DependencyObject
   {
      #region fields
      static Dictionary<string, Extention> fExtentions = new Dictionary<string, Extention>();


      static RoutedEventHandler fDockSplitterToExpanderButtonClick;
      static DragStartedEventHandler fDockSplittervalueReset;
      static RoutedEventHandler fListBoxSelectOnFocusGotFocus;

      #endregion

      #region const

      static Extender()
      {
         RegisterDockSplitterToExpander();
         RegisterListBoxSelectOnFocus();
         
      }

      private static void RegisterListBoxSelectOnFocus()
      {
         Extention iNew = new Extention();

         iNew.Register += new ExtentionHandler(ListBoxSelectOnFocus_Register);
         iNew.UnRegister += new ExtentionHandler(ListBoxSelectOnFocus_UnRegister);
         fExtentions.Add("ListBoxSelectOnFocus", iNew);
         fListBoxSelectOnFocusGotFocus = new RoutedEventHandler(ListBoxSelectOnFocus_GotFocus);
      }

      private static void RegisterDockSplitterToExpander()
      {
         Extention iNew = new Extention();

         iNew.Register += new ExtentionHandler(DockSplitterToExpander_Register);
         iNew.UnRegister += new ExtentionHandler(DockSplitterToExpander_UnRegister);
         fExtentions.Add("DockSplitterToExpander", iNew);
         fDockSplitterToExpanderButtonClick = new RoutedEventHandler(DockSplitterToExpanderButton_Click);
         fDockSplittervalueReset = new DragStartedEventHandler(DockSplitter_DragStarted);
      }


      #endregion

      #region props

      /// <summary>
      /// Gets the dictionary of Extentions available.  The Key 
      /// </summary>
      public static Dictionary<string, Extention> Extentions
      {
         get { return Extender.fExtentions; }
      }


      #region Functionality

      /// <summary>
      /// Identifies the Functionality attached property.
      /// </summary>
      public static readonly DependencyProperty FunctionalityProperty =
            DependencyProperty.RegisterAttached("Functionality", typeof(string), typeof(Extender), new UIPropertyMetadata(null, new PropertyChangedCallback(OnFunctionalityChanged)));

      /// <summary>
      /// Sets the custom functionality key that is assigned to the object.
      /// </summary>
      public static void SetFunctionality(UIElement element, string value)
      {
         element.SetValue(FunctionalityProperty, value);
      }

      /// <summary>
      /// Gets the custom functionality key that is assigned to the object.
      /// </summary>
      public static string GetFunctionality(UIElement element)
      {
         return (string)element.GetValue(FunctionalityProperty);
      }

      /// <summary>
      /// Called when the Functionality property has been changed on an object.
      /// </summary>
      static void OnFunctionalityChanged(DependencyObject aSender, DependencyPropertyChangedEventArgs e)
      {
         Extention iFound;

         string iVal = (string)e.OldValue;
         if (iVal != null)
         {
            if (Extender.Extentions.TryGetValue(iVal, out iFound) == true)
            {
               iFound.OnUnRegister(aSender);
            }
         }

         iVal = (string)e.NewValue;
         if (iVal != null)
         {
            if (Extender.Extentions.TryGetValue(iVal, out iFound) == true)
            {
               iFound.OnRegister(aSender);
            }
         }
      }


      #endregion

      #endregion

      #region event handlers

      #region DockSplitterToExpander

      static void DockSplitterToExpander_UnRegister(object sender, ExtentionEventArgs e)
      {
         Button iButton = e.OriginalSource as Button;

         if (iButton != null)
         {
            iButton.Click -= fDockSplitterToExpanderButtonClick;
         }
      }

      static void DockSplitterToExpander_Register(object sender, ExtentionEventArgs e)
      {
         Button iButton = e.OriginalSource as Button;

         if (iButton != null)
         {
            iButton.Click += fDockSplitterToExpanderButtonClick;
         }
      }

      /// <summary>
      /// Event handler for the splitters collaps/expand button.
      /// collapses the associated object by setting it's Value to 0.0, or restoring it to it's default value stored in the Tag property.
      /// </summary>
      static void DockSplitterToExpanderButton_Click(object sender, RoutedEventArgs e)
      {
         try
         {
            Button iOrigin = sender as Button;

            if (iOrigin != null)
            {
               DockSplitter iSender = iOrigin.TemplatedParent as DockSplitter;

               if (iSender != null)
               {
                  if (iSender.Value == 0.0)
                  {
                     if (iOrigin.Tag is double)
                     {
                        iSender.DragStarted += fDockSplittervalueReset;
                        double iTo = (double)iOrigin.Tag;
                        DoubleAnimation iValueAnimation = new DoubleAnimation(iTo, new TimeSpan(0, 0, 0, 0, 70));
                        iValueAnimation.AutoReverse = false;
                        iValueAnimation.FillBehavior = FillBehavior.HoldEnd;
                        AnimationClock iMyControllableClock = iValueAnimation.CreateClock();
                       
                        iSender.ApplyAnimationClock(DockSplitter.ValueProperty, iMyControllableClock);
                        //iSender.Value = iTo;
                     }
                  }
                  else
                  {
                     iOrigin.Tag = iSender.Value;

                     iSender.DragStarted += fDockSplittervalueReset;
                     DoubleAnimation iValueAnimation = new DoubleAnimation(0.0, new TimeSpan(0, 0, 0, 0, 120));
                     iValueAnimation.AutoReverse = false;
                     iValueAnimation.FillBehavior = FillBehavior.HoldEnd;
                     AnimationClock iMyControllableClock = iValueAnimation.CreateClock();
                     iSender.ApplyAnimationClock(DockSplitter.ValueProperty, iMyControllableClock);
                     //iSender.Value = 0.0;
                  }
               }
            }
         }
         catch (Exception ex)
         {
            MessageBox.Show(ex.Message);
         }
      }

      static void DockSplitter_DragStarted(object sender, System.Windows.Controls.Primitives.DragStartedEventArgs e)
      {
         DockSplitter iSender = sender as DockSplitter;
         if (iSender != null)
         {
            iSender.DragStarted -= fDockSplittervalueReset;
            double iVal = iSender.Value;
            iSender.ApplyAnimationClock(DockSplitter.ValueProperty, null);
            iSender.Value = iVal;
         }
      }


      #endregion

      #region ListBoxSelectOnFocus

      static void ListBoxSelectOnFocus_UnRegister(object sender, ExtentionEventArgs e)
      {
         UIElement iItem = e.OriginalSource as UIElement;

         if (iItem != null)
         {

            iItem.RemoveHandler(ListBoxItem.GotFocusEvent, fListBoxSelectOnFocusGotFocus);
         }
      }

      static void ListBoxSelectOnFocus_Register(object sender, ExtentionEventArgs e)
      {
         UIElement iItem = e.OriginalSource as UIElement;

         if (iItem != null)
         {
            iItem.AddHandler(ListBoxItem.GotFocusEvent, fListBoxSelectOnFocusGotFocus);
         }
      }

      static void ListBoxSelectOnFocus_GotFocus(object sender, RoutedEventArgs e)
      {
         ListBox iList = sender as ListBox;
         if (iList != null)
         {
            ListBoxItem iItem = iList.ContainerFromElement(e.OriginalSource as DependencyObject) as ListBoxItem;

            if (iItem != null && !iItem.IsSelected)
               iItem.IsSelected = true;
         }
      }


      #endregion

      #endregion

   }
}
