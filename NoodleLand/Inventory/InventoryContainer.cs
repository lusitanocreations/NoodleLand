using System;
using NoodleLand.Data.Items;
using NoodleLand.Inventory.Items;
using NoodleLand.MessageHandling.Inventory;
using UnityEngine;

namespace NoodleLand.Inventory
{
    public class InventoryContainer : MonoBehaviour
    {
        [SerializeField] private InventorySlot[] _inventorySlots;


        public GameObject[] UIObjectS;
        public InventorySlot[] Slots => _inventorySlots;
        private InventorySlot currentSelected;


        public bool HasSlot(InventorySlot invSlot)
        {
            for (var i = 0; i < _inventorySlots.Length; i++)
            {
                if (_inventorySlots[i] == invSlot)
                {
                    return true;
                }
                
            }

            return false;
        }

        public bool HasSpace()
        {
            for (var i = 0; i < _inventorySlots.Length; i++)
            {
                if (_inventorySlots[i].IsEmpty())
                {
                    return true;
                }
                
            }

            return false;
        }

        public InventoryMessage ForceAddInSlot(StackableItem stackableItem,int slot)
        {
            _inventorySlots[slot].ForceAddNewStack(stackableItem);
            return InventoryMessage.InventoryFull;
        }
        public InventoryMessage TryAdd(BaseItemData baseItem, int amount,bool substitute = true)
        {
            InventorySlot freeInventory = null;
            foreach (var inventorySlot in _inventorySlots)
            {
                if (inventorySlot.IsEmpty())
                {
                    if(freeInventory == null)
                      freeInventory = inventorySlot;
                    continue;
                }
                if (inventorySlot.IsSameItem(baseItem) && substitute)
                {
                    inventorySlot.AddToStack(amount);
                    return InventoryMessage.AddedToInventory;
                }
            }

            if (freeInventory != null)
            {
                freeInventory.ForceAddNewStack(new StackableItem(baseItem, amount));
                return InventoryMessage.AddedToInventory;

            }

            return InventoryMessage.InventoryFull;
        }

        public InventoryMessage TryAdd(StackableItem stackableItem, bool substitute = true)
        {
            return TryAdd(stackableItem.BaseItemData,stackableItem.Quantity, substitute);
        }
        private void UpdateAllSlotsUI()
        {
            for (var i = 0; i < _inventorySlots.Length; i++)
            {
                _inventorySlots[i].UpdateUI();
            }
        }
        public void OpenUI()
        {
            foreach (var o in UIObjectS)
            {
                o.SetActive(true);
                
            }
           
            UpdateAllSlotsUI();
        }

      
        public void CloseUI()
        {
            foreach (var o in UIObjectS)
            {
                o.SetActive(false);
                
            }
         
        }


        private void OnDestroy()
        {
           
        }

    }
}