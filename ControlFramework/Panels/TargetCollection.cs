using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace JaStDev.ControlFramework.Controls
{
   /// <summary>
   /// Collection used by <see cref="DistributionPanel"/> to manage it's <see cref="DistributionTarget"/>s.
   /// </summary>
   /// <remarks>
   /// Lets the <see cref="DistributionTarget"/> know about changes in the list so it can update it's logical tree.
   /// This allows the target object's bindings to work properly.
   /// </remarks>
   public class TargetCollection: Collection<DistributionTarget>, INotifyCollectionChanged
   {
      protected override void ClearItems()
      {
         base.ClearItems();
         int iCounter = this.Count - 1;
         foreach (DistributionTarget i in this)
         {
            RaiseCollectionChanged(NotifyCollectionChangedAction.Remove, i, iCounter);
            iCounter--;
         }
      }

      protected override void InsertItem(int index, DistributionTarget item)
      {
         base.InsertItem(index, item);
         RaiseCollectionChanged(NotifyCollectionChangedAction.Add, item, index);
      }

      protected override void RemoveItem(int index)
      {
         base.RemoveItem(index);
         RaiseCollectionChanged(NotifyCollectionChangedAction.Remove, this[index], index);
      }

      protected override void SetItem(int index, DistributionTarget item)
      {
         base.SetItem(index, item);
         RaiseCollectionChanged(NotifyCollectionChangedAction.Remove, this[index], index);
         RaiseCollectionChanged(NotifyCollectionChangedAction.Add, item, index);
      }


      #region INotifyCollectionChanged Members

      public event NotifyCollectionChangedEventHandler CollectionChanged;

      private void RaiseCollectionChanged(NotifyCollectionChangedAction action, object changedItem, int index)
      {
         if (CollectionChanged != null)
            CollectionChanged(this, new NotifyCollectionChangedEventArgs(action, changedItem, index));
      }

      #endregion
   }
}
