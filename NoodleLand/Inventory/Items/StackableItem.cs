using System;
using Lusitano.Input;
using NoodleLand.Data.Items;
using NoodleLand.Entities.Living.Player;
using NoodleLand.Events;
using NoodleLand.Events.Item;
using UnityEngine.Events;

namespace NoodleLand.Inventory.Items
{
    public class StackableItem
    {
        private BaseItemData _baseItemData;
        private int quantity;
        private bool isStackable;

       // public Action onSlotLeave;

       public Action UpdateUI;
     
        public BaseItemData BaseItemData => _baseItemData;
        public int Quantity => quantity;
        public bool IsStackable => isStackable;
       
        public void AddToStack(int amount)
        {
            quantity += amount;
            FakeEventBus.InvokeEvent(new ItemStackModifiedEvent(this));
            UpdateUI?.Invoke();
           
        }

       
        public void RemoveFromStack(int amount)
        {
            quantity -= amount;
            FakeEventBus.InvokeEvent(new ItemStackModifiedEvent(this));
            UpdateUI?.Invoke();
            // onStackModified?.Invoke();
           
            
        }

        public bool Split(out StackableItem stackableItem)
        {
            int toGive;

            if (quantity == 1)
            {
                stackableItem = null;
                return false;
            }

            toGive = quantity / 2;

            StackableItem st = new StackableItem(_baseItemData, toGive);
            stackableItem = st;
            quantity /= 2;
            UpdateUI?.Invoke();
            return true;
        }

        public StackableItem Copy()
        {
            return new StackableItem(_baseItemData, quantity);
        }
        public StackableItem(BaseItemData itemData, int quantity)
        {
            _baseItemData = itemData;
            isStackable = _baseItemData.IsStackable;
            this.quantity = quantity;
            
        }


    }
}