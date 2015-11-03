using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Runtime.Serialization;
using System.Windows;
using System.Windows.Threading;

namespace JaStDev.ControlFramework.Controls
{

   #region Exceptions
   /// <summary>
   /// Exception incountered or caused during distribution of elements.
   /// </summary>
   [global::System.Serializable]
   public class DistributionException : Exception
   {
      public DistributionException() { }
      public DistributionException(string message) : base(message) { }
      public DistributionException(string message, Exception inner) : base(message, inner) { }
      protected DistributionException(SerializationInfo info, StreamingContext context) : base(info, context) { }
   } 
   #endregion

   #region Delegates and event args
   /// <summary>
   /// Event arguments for the <see cref="DistributionPanel.Redistributed"/> event.
   /// </summary>
   public class DistributionEventArgs : RoutedEventArgs
   {
      /// <summary>
      /// The default constructor.
      /// </summary>
      /// <param name="item">The item being redistributed.</param>
      /// <param name="oldTarget">The <see cref="DistributionTarget"/> that currently has the item (for the preview event) or had (for the normal event).</param>
      /// <param name="newTarget">The <see cref="DistributionTarget"/> that will receive the item (for the preview event) or now has (for the normal event).</param>
      public DistributionEventArgs(UIElement item, DistributionTarget oldTarget, DistributionTarget newTarget)
      {
         Item = item;
         OldTarget = oldTarget;
         newTarget = newTarget;
      }
      /// <summary>
      /// The item being redistributed.
      /// </summary>
      public UIElement Item { get; set; }
      /// <summary>
      /// The <see cref="DistributionTarget"/> that currently has the item (for the preview event) or had (for the normal event).
      /// </summary>
      public DistributionTarget OldTarget { get; set; }
      /// <summary>
      /// The <see cref="DistributionTarget"/> that will receive the item (for the preview event) or now has (for the normal event).
      /// </summary>
      public DistributionTarget NewTarget { get; set; }
   }

   public delegate void DistributionEventHandler(object sender, DistributionEventArgs e); 
   #endregion

   /// <summary>
   /// A <see cref="System.Windows.Controls.Panel"/> descendent able to put all of it's children on other panels (or general lists).
   /// </summary>
   /// <remarks>
   /// <para>
   /// Although theoretically possible, a DistributionPanel doesn't allow it's children to be owned by more than 1 DistributionPanel. So
   /// you can't send a child to another DistributionPanel, not even indirectly. This is done to allow for proper internal workings.
   /// </para>
   /// <para>
   /// Note: when you use a DistributionPanel in an ItemsControl as it's itemsPanel, the <see cref="System.Windows.Controls.ItemsControl.ItemsControlFromItemContainer"/>
   /// function no longer works. This is because it uses the logical tree to find the parent items control, which is broken by ConceptualPanel.
   /// </para>
   /// </remarks>
   public class DistributionPanel : ConceptualPanel
   {

      #region Fields
      TargetCollection fTargets = new TargetCollection(); 
      #endregion

      #region ctor
      /// <summary>
      /// Default constructor.
      /// </summary>
      public DistributionPanel()
      {
         fTargets.CollectionChanged += new NotifyCollectionChangedEventHandler(fTargets_CollectionChanged);
         //the initial distribution  after loading is triggered by the Loaded event handler of ConceptualPanel
      }


      #endregion


      #region Prop
      #region Targets
      /// <summary>
      /// gets the list of rules that determin how distribution should be done.
      /// </summary>
      /// <remarks>
      /// A <see cref="DistributionTarget"/> has a <see cref="DistributionTarget.Target"/> and <see cref="DistributionRule.Value"/> property.
      /// This list of targets is applied to all the children of the panel.  The first one that evaluates to true is used (When the binding 
      /// equals the value). An evaluation is done by comparing the value declared in the <see cref="DistributionTarget.Value"/> and that one found 
      /// on the item to distribute for the <see cref="DistributionPanel.Key"/> attached property.The child is than sent to the 
      /// <see cref="DistributionRule.Target"/>.
      /// </remarks>
      public TargetCollection Targets
      {
         get
         {
            return fTargets;
         }
      } 
      #endregion

      #region Key

      /// <summary>
      /// Key Attached Dependency Property
      /// </summary>
      public static readonly DependencyProperty KeyProperty =
          DependencyProperty.RegisterAttached("Key", typeof(object), typeof(DistributionPanel),
              new FrameworkPropertyMetadata((object)null,
                  FrameworkPropertyMetadataOptions.SubPropertiesDoNotAffectRender,
                  new PropertyChangedCallback(OnKeyChanged)));

      /// <summary>
      /// Gets the Key property.  This attached property 
      /// indicates the value that determins which <see cref="DistributionTarget.Target"/> to use.
      /// </summary>
      /// <remarks>
      /// Assign this attached property to the object managed by the <see cref="DistributionPanel"/> to indicate which
      /// <see cref="DistributionTarget"/>'s <see cref="DistributionTarget.Target"/> to add the object to. This value
      /// is compared with each <see cref="DistributionTarget.Value"/> property of the DistributionTargets.  The first
      /// match is used as the final target.
      /// </remarks>
      public static object GetKey(DependencyObject d)
      {
         return (object)d.GetValue(KeyProperty);
      }

      /// <summary>
      /// Sets the Key property.  This attached property 
      /// indicates the value that determins which Target to use.
      /// </summary>
      /// <remarks>
      /// Assign this attached property to the object managed by the <see cref="DistributionPanel"/> to indicate which
      /// <see cref="DistributionTarget"/>'s <see cref="DistributionTarget.Target"/> to add the object to. This value
      /// is compared with each <see cref="DistributionTarget.Value"/> property of the DistributionTargets.  The first
      /// match is used as the final target.
      /// </remarks>
      public static void SetKey(DependencyObject d, object value)
      {
         d.SetValue(KeyProperty, value);
      }

      /// <summary>
      /// Handles changes to the Key property.
      /// </summary>
      private static void OnKeyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
      {
         DistributionPanel iPanel = GetPanel(sender);
         UIElement iSender = sender as UIElement;
         if (iPanel != null && iSender != null)
            iPanel.Dispatcher.BeginInvoke(DispatcherPriority.Loaded, 
               new Action<UIElement, object, object>(iPanel.Redistribute), 
               iSender, new object[] { e.OldValue, e.NewValue });                               //we call this async to make certain that all objects are loaded.  This is first called when values are loaded from xaml, or at a time when not all objects are loaded already, so the targets might not yet be there, hence we wait to do the distribution untill all is loaded.
      }

      #endregion


      #region Panel

      /// <summary>
      /// Panel Attached Dependency Property
      /// </summary>
      internal static readonly DependencyProperty PanelProperty =
          DependencyProperty.RegisterAttached("Panel", typeof(DistributionPanel), typeof(DistributionPanel),
              new FrameworkPropertyMetadata(null, null, new CoerceValueCallback(CoercePanelValue)));

      /// <summary>
      /// Gets the Panel property.  This attached property 
      /// indicates which DistributionPanel manages the object..
      /// </summary>
      /// <remarks>
      /// Used to find out if an item has already been added to a distribution panel or not.
      /// </remarks>
      internal static DistributionPanel GetPanel(DependencyObject d)
      {
         return (DistributionPanel)d.GetValue(PanelProperty);
      }

      /// <summary>
      /// Sets the Panel property.  This attached property 
      /// indicates which DistributionPanel manages the object..
      /// </summary>
      internal static void SetPanel(DependencyObject d, DistributionPanel value)
      {
         d.SetValue(PanelProperty, value);
      }

      /// <summary>
      /// Coerces the Panel value.
      /// </summary>
      private static object CoercePanelValue(DependencyObject d, object value)
      {
         object iLocalVal = d.ReadLocalValue(DistributionPanel.PanelProperty);
         if (iLocalVal != DependencyProperty.UnsetValue && value != null)
            throw new DistributionException("The object is already managed by a DistributionPanel. A chid of a DistributionPanel can't be handled by multiple DistributionPanels.");
         return value;
      }

      #endregion




      #endregion


      #region Events

      #region PreviewDistributed

      /// <summary>
      /// PreviewDistributed Routed Event
      /// </summary>
      public static readonly RoutedEvent PreviewDistributedEvent = EventManager.RegisterRoutedEvent("PreviewDistributed",
          RoutingStrategy.Tunnel, typeof(DistributionEventHandler), typeof(DistributionPanel));

      /// <summary>
      /// Occurs when the DistributionPanel adds/removes or moves an item from one list to another.
      /// </summary>
      public event DistributionEventHandler PreviewDistributed
      {
         add { AddHandler(PreviewDistributedEvent, value); }
         remove { RemoveHandler(PreviewDistributedEvent, value); }
      }

      /// <summary>
      /// A helper method to raise the PreviewDistributed event.
      /// </summary>
      /// <param name="element">The item being distributed.</param>
      /// <param name="oldTarget">The DistributionTarget it is currently in.</param>
      /// <param name="newTarget">The distributionTarget it will be moved to.</param>
      protected DistributionEventArgs RaisePreviewDistributedEvent(UIElement element, DistributionTarget oldTarget, DistributionTarget newTarget)
      {
         return RaisePreviewDistributedEvent(this, element, oldTarget, newTarget);
      }

      /// <summary>
      /// A static helper method to raise the PreviewDistributed event on a target element.
      /// </summary>
      /// <param name="target">UIElement or ContentElement on which to raise the event</param>
      /// <param name="element">The item being distributed.</param>
      /// <param name="oldTarget">The DistributionTarget it is currently in.</param>
      /// <param name="newTarget">The distributionTarget it will be moved to.</param>
      internal static DistributionEventArgs RaisePreviewDistributedEvent(UIElement target, UIElement element, DistributionTarget oldTarget, DistributionTarget newTarget)
      {
         if (target == null) return null;

         DistributionEventArgs args = new DistributionEventArgs(element, oldTarget, newTarget);
         args.RoutedEvent = PreviewDistributedEvent;
         RaiseEvent(target, args);
         return args;
      }  


      #endregion


      #region Distributed

      /// <summary>
      /// Distributed Routed Event
      /// </summary>
      public static readonly RoutedEvent DistributedEvent = EventManager.RegisterRoutedEvent("Distributed",
          RoutingStrategy.Bubble, typeof(DistributionEventHandler), typeof(DistributionPanel));

      /// <summary>
      /// Occurs when an element is added/removed or moved from a panel and/or to another one.
      /// </summary>
      public event DistributionEventHandler Distributed
      {
         add { AddHandler(DistributedEvent, value); }
         remove { RemoveHandler(DistributedEvent, value); }
      }

      /// <summary>
      /// A helper method to raise the Distributed event.
      /// </summary>
      /// <param name="element">The item being moved/added/removed</param>
      /// <param name="oldTarget">The DistributionTarget it was in.</param>
      /// <param name="newTarget">The DistributionTarger it has moved to.</param>
      protected DistributionEventArgs RaiseDistributedEvent(UIElement element, DistributionTarget oldTarget, DistributionTarget newTarget)
      {
         return RaiseDistributedEvent(this, element, oldTarget, newTarget);
      }

      /// <summary>
      /// A helper method to raise the Distributed event.
      /// </summary>
      /// <remarks>
      /// Used in a combo <see cref="DistributionTarget.PreviewDistributed"/> and <see cref="DistributionTarget.Distributed"/> so that the
      /// event args of the first can be passed along to the second.
      /// </remarks>
      /// <param name="arg">The arguments for the event.</param>
      protected DistributionEventArgs RaiseDistributedEvent(DistributionEventArgs arg)
      {
         return RaiseDistributedEvent(this, arg);
      }

      /// <summary>
      /// A static helper method to raise the Distributed event on a target element.
      /// </summary>
      /// <param name="target">UIElement or ContentElement on which to raise the event</param>
      /// <param name="element">The item being moved/added/removed</param>
      /// <param name="oldTarget">The DistributionTarget it was in.</param>
      /// <param name="newTarget">The DistributionTarget it has moved to.</param>
      internal static DistributionEventArgs RaiseDistributedEvent(UIElement target, UIElement element, DistributionTarget oldTarget, DistributionTarget newTarget)
      {
         if (target == null) return null;

         DistributionEventArgs args = new DistributionEventArgs(element, oldTarget, newTarget);
         args.RoutedEvent = DistributedEvent;
         RaiseEvent(target, args);
         return args;
      }

      /// <summary>
      /// A static helper method to raise the Distributed event on a target element.
      /// </summary>
      /// <param name="target">UIElement or ContentElement on which to raise the event</param>
      /// <param name="arg">The arguments for the event.</param>
      internal static DistributionEventArgs RaiseDistributedEvent(UIElement target, DistributionEventArgs arg)
      {
         if (target == null) return null;

         arg.RoutedEvent = DistributedEvent;
         RaiseEvent(target, arg);
         return arg;
      }


      #endregion

      #region RoutedEvent Helper Methods

      /// <summary>
      /// A static helper method to raise a routed event on a target UIElement or ContentElement.
      /// </summary>
      /// <param name="target">UIElement or ContentElement on which to raise the event</param>
      /// <param name="args">RoutedEventArgs to use when raising the event</param>
      private static void RaiseEvent(DependencyObject target, RoutedEventArgs args)
      {
         if (target is UIElement)
            (target as UIElement).RaiseEvent(args);
         else if (target is ContentElement)
            (target as ContentElement).RaiseEvent(args);
      }

      /// <summary>
      /// A static helper method that adds a handler for a routed event 
      /// to a target UIElement or ContentElement.
      /// </summary>
      /// <param name="element">UIElement or ContentElement that listens to the event</param>
      /// <param name="routedEvent">Event that will be handled</param>
      /// <param name="handler">Event handler to be added</param>
      private static void AddHandler(DependencyObject element, RoutedEvent routedEvent, Delegate handler)
      {
         UIElement uie = element as UIElement;
         if (uie != null)
            uie.AddHandler(routedEvent, handler);
         else
         {
            ContentElement ce = element as ContentElement;
            if (ce != null)
               ce.AddHandler(routedEvent, handler);
         }
      }

      /// <summary>
      /// A static helper method that removes a handler for a routed event 
      /// from a target UIElement or ContentElement.
      /// </summary>
      /// <param name="element">UIElement or ContentElement that listens to the event</param>
      /// <param name="routedEvent">Event that will no longer be handled</param>
      /// <param name="handler">Event handler to be removed</param>
      private static void RemoveHandler(DependencyObject element, RoutedEvent routedEvent, Delegate handler)
      {
         UIElement uie = element as UIElement;
         if (uie != null)
         {
            uie.RemoveHandler(routedEvent, handler);
         }
         else
         {
            ContentElement ce = element as ContentElement;
            if (ce != null)
            {
               ce.RemoveHandler(routedEvent, handler);
            }
         }
      }

      #endregion
        
  

        

        

      #endregion

      #region Distribution

      /// <summary>
      /// Cleans out the previous distribution, reassesses each item again and assigns it to the correct list.
      /// </summary>
      /// <remarks>
      /// Use this function if, for any reason the distribution has gotten out of sync.
      /// </remarks>
      public virtual void RebuildDistribution()
      {
         try
         {
            if (IsLoaded == true)
            {
               foreach (DistributionTarget i in Targets)
                  ClearDistributionFor(i);
               foreach (UIElement i in InternalChildren)
                  AddChild(i);
            }
         }
         catch (Exception e)
         {
            throw new DistributionException("Failed to rebuild part of distributed list!", e);
         }
      }

      /// <summary>
      /// Relocates the specified object from the distribution target located at oldKey to the target found at newKey.
      /// </summary>
      /// <remarks>
      /// This function is called whenever the distribution key value changes for an item.
      /// </remarks>
      /// <param name="item">The item to be moved.  This must be in the <see cref="DistributionPanel.Children"/> collection.</param>
      /// <param name="oldKey">The key that determines the old location.</param>
      /// <param name="newKey">The new location of the item.</param>
      protected virtual void Redistribute(UIElement item, object oldKey, object newKey)
      {
         DistributionTarget iOld = null;
         DistributionTarget iNew = null;
         foreach (DistributionTarget i in Targets)
         {
            if (iOld == null && i.Value.Equals(oldKey))
               iOld = i;
            if (iNew == null && i.Value.Equals(newKey))
               iNew = i;
            if (iOld != null && iNew != null)
               break;
         }
         Distribute(item, iOld, iNew);
      }

      

      /// <summary>
      /// Puts the specified object from the old target to the new target.
      /// </summary>
      /// <remarks>
      /// Overwrite this function if you need to do some extra processing during a transition from 1 target to another one.
      /// You can achieve similar results overwriting <see cref="DistributionTarget.AddChildToTarget"/> and
      /// <see cref="DistributionTarget.RemoveChildFromTarget"/>, which are called by this function.  Overwriting at this level
      /// gives you the ability to check old and new target at the same time.  This function is always called, also during an
      /// add or remove (in which case one of the params is empty).    
      /// </remarks>
      /// <param name="item">The item to move.</param>
      /// <param name="oldTarget">The old distribution target.</param>
      /// <param name="newTarget">The new distribution target.</param>
      protected virtual void Distribute(UIElement item, DistributionTarget oldTarget, DistributionTarget newTarget)
      {
         DistributionEventArgs iArgs = RaisePreviewDistributedEvent(item, oldTarget, newTarget);               //raise the preview event
         if (oldTarget != null)
         {
            if (oldTarget.Items.Contains(item) == true)                                                        //do some sanity check: if there is an oldTarget, it should be in it's list, otherwise we shouldn't be able to remove it.
               oldTarget.RemoveChildFromTarget(item);                                                          //Also, if there is an oldTarget, ask it to remove the item.
            else
               throw new DistributionException("Invalid old key!", new ArgumentOutOfRangeException("oldTarget"));
         }
         if (newTarget != null)                                                                                //if there is a new target, ask it to add the item.
            newTarget.AddChildToTarget(item);
         RaiseDistributedEvent(iArgs);                                                                         //and finally raise the event indicating that the distribution has completed.
      }


      /// <summary>
      /// Rechecks the key for each item in the specified distribution target, thereby effectivelly redistributing part of the 
      /// items list of the DistributionPanel.
      /// </summary>
      /// <param name="target"></param>
      internal void RebuildDistributionFor(DistributionTarget target)
      {
         try
         {
            List<UIElement> iTemp = new List<UIElement>(target.Items);
            ClearDistributionFor(target);
            foreach (UIElement i in iTemp)
               AddChild(i);
         }
         catch (Exception e)
         {
            throw new DistributionException("Failed to rebuild part of distributed list!", e);
         }
      }

      /// <summary>
      /// Tries to remove each item found in <see cref="DistributionTarget.Items"/> from the Target.
      /// </summary>
      /// <param name="target">The <see cref="DistributionTarget"/> to clean out.</param>
      protected virtual void ClearDistributionFor(DistributionTarget target)
      {
         IList iTarget = target.Target;
         if (iTarget != null)
         {
            foreach (UIElement i in target.Items)
            {
               DistributionEventArgs iArgs = DistributionPanel.RaisePreviewDistributedEvent(this, i, target, null);
               iTarget.Remove(i);
               DistributionPanel.RaiseDistributedEvent(this, iArgs);
            }
            target.Items.Clear();
         }
      }

      protected override void OnChildAdded(UIElement child)
      {
         if (child != null)
            AddChild(child);
      }

      protected override void OnChildRemoved(UIElement child)
      {
         if (child != null)
            RemoveChild(child);
      }

      /// <summary>
      /// Tries to find a <see cref="DistributionTarget"/> who's <see cref="DistributionTarget.Value"/> matches that of the
      /// child's <see cref="DistributionPanel.GetKey"/> attached property and adds it to the <see cref="DistributionTarget.Target"/>.
      /// </summary>
      /// <remarks>
      /// The first target in the list that matches is used.
      /// </remarks>
      /// <param name="child">The child to add to a list</param>
      private void AddChild(UIElement child)
      {
         try
         {
            SetPanel(child, this);
            object iValToCheck = GetKey(child);
            foreach (DistributionTarget i in Targets)
            {
               if (i.Value.Equals(iValToCheck) == true)
               {
                  Distribute(child, null, i);
                  return;
               }
            }
         }
         catch (Exception e)
         {
            throw new DistributionException("Failed to distribute child to list!", e);
         }
      }

      /// <summary>
      /// Tries to find a <see cref="DistributionTarget"/> who's <see cref="DistributionTarget.Value"/> matches that of the
      /// child's <see cref="DistributionPanel.GetKey"/> attached property and removes it from the <see cref="DistributionTarget.Target"/>.
      /// </summary>
      /// <remarks>
      /// The first target in the list that matches is used. If the remove failed somehow, a new exception is thrown using
      /// </remarks>
      /// <param name="child">The child to add to a list</param>
      private void RemoveChild(UIElement child)
      {
         try
         {
            child.ClearValue(DistributionPanel.PanelProperty);
            foreach (DistributionTarget i in Targets)
            {
               if (i.Items.Contains(child) == true)                                             //we check the internal list of the target instead of comparing with the value of 'Key", cause this is more secure, we are certain that the specified target contains the list.
               {
                  Distribute(child, i, null);
                  return;
               }
            }
         }
         catch (Exception e)
         {
            throw new DistributionException("Failed to remove distributed child from list!", e);
         }
      }

      #endregion

      #region Target management
      /// <summary>
      /// Called by <see cref="TargetCollection"/> wenever a target is added or removed.
      /// </summary>
      /// <remarks>
      /// We always rebuild the intire distribibution again.  This allows us to keep the logical tree in sync.
      /// </remarks>
      /// <param name="target">The target added to or removed from the list.</param>
      void fTargets_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
      {
         switch (e.Action)
         {
            case NotifyCollectionChangedAction.Add:
               AddLogicalChild(e.NewItems[0]);
               break;
            case NotifyCollectionChangedAction.Remove:
               RemoveLogicalChild(e.OldItems[0]);
               break;
            default:
               break;
         }
         if (IsLoaded == true)
            RebuildDistribution();
      } 
      #endregion
   }
}
