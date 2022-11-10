using System;
using System.Collections.Generic;
using NoodleLand.Entities.Living.Player;
using NoodleLand.Inventory.Items;
using NoodleLand.MessageHandling.Inventory;
using UnityEngine;

namespace NoodleLand.Inventory
{
    /// <summary>
    /// I dont know how I feel about this one haha.
    /// </summary>
    public static class InventoryEvent
    {

        private static InventorySlot currentSlot;
        private static List<InventoryContainer> currentInventoriesOpen;

    
            
        /// <summary>
        /// Inform that an inventory trade is happening between the inventories
        /// </summary>
        public static void InformInventoriesOpen(PlayerEntity playerEntity,List<InventoryContainer> inventoryContainerUis)
        {
            InformInventoryCancel();

            currentInventoriesOpen = inventoryContainerUis;
            
            
            for (var i = 0; i < inventoryContainerUis.Count; i++)
            {
                InventoryContainer container = inventoryContainerUis[i];
                for (var j = 0; j < container.Slots.Length; j++)
                {
                    InventorySlot sl = container.Slots[j];
                    

                    sl.OnSlotHold.AddListener((f0 => SelectHold(sl,container,f0)));
                    sl.OnSlotPress.AddListener(() =>SelectInventorySlot(playerEntity,sl));
                }
                container.OpenUI();
            }
            
        
        }
        

        private static void SelectHold(InventorySlot inventorySlot, InventoryContainer inv,float amount)
        {
          
            if (inv.HasSpace())
            {  

                if ( amount >= 0.5f &&  inventorySlot.SplitItemStack(out StackableItem item))
                {
                   
                    inv.TryAdd(item,false);
                    RemoveCurrentSlot();
                }

            }
                

              

        }
        public static void CancelCurrentOperation()
        {
            if (currentSlot != null)
            {
                currentSlot.Deselect();
                currentSlot = null;

            }
        }
    
        /// <summary>
        /// Cancels all trading inventory operations
        /// </summary>

        public static void InformInventoryCancel()
        {
            if(currentInventoriesOpen == null) return;
            
            
            for (var i = 0; i < currentInventoriesOpen.Count; i++)
            {
                InventoryContainer container = currentInventoriesOpen[i];
                for (var j = 0; j < container.Slots.Length; j++)
                {
                    InventorySlot sl = container.Slots[j];
                    
                    sl.OnSlotHold.RemoveAllListeners();
                    sl.OnSlotPress.RemoveAllListeners();
                }
                container.CloseUI();
            }

            if (currentSlot != null)
            {
                currentSlot.Deselect();
            }
          
            currentSlot = null;
            currentInventoriesOpen = null;
            
        }
        
        private static void SelectInventorySlot(PlayerEntity playerEntity,InventorySlot inventorySlot)
        {

            if (IsFirstPressOnSlot(playerEntity,inventorySlot)) return;
            
            if (IsSameSlot(inventorySlot)) return;

            if(IsMovingToEmptySlot(playerEntity,inventorySlot)) return;
            
            if(IsMovingToSameItem(playerEntity,inventorySlot)) return;

            if(IsSwitchingSlots(playerEntity,inventorySlot)) return;


        }
        
        private static bool IsSwitchingSlots(PlayerEntity p,InventorySlot inventorySlot)
        {
            // why am I checking this
            
            if (!inventorySlot.IsEmpty())
            {

            
              // currentSlot.ItemStack.onSlotLeave?.Invoke();
                StackableItem s0 = currentSlot.StackableItem.Copy();
                StackableItem s1 = inventorySlot.StackableItem.Copy();

                currentSlot.ForceAddNewStack(s1);
                inventorySlot.ForceAddNewStack(s0);
                
                inventorySlot.Deselect();
              
                RemoveCurrentSlot();
                return true;
            }

            return false;
        }

        private static bool IsMovingToSameItem(PlayerEntity p,InventorySlot inventorySlot)
        {
            if (inventorySlot.IsSameItem(currentSlot.StackableItem))
            {
              //  currentSlot.ItemStack.onSlotLeave?.Invoke();
                p.NotifyOfHandChange();
                inventorySlot.Deselect();
                inventorySlot.AddToStack(currentSlot.StackableItem.Quantity);
                currentSlot.RemoveStack();
                RemoveCurrentSlot();
                return true;
            }

            return false;
        }

        private static bool IsMovingToEmptySlot(PlayerEntity p,InventorySlot inventorySlot)
        {
            if (inventorySlot.IsEmpty())
            {

                inventorySlot.Deselect();
                if (currentSlot.StackableItem != null)
           
                {
                   
                    inventorySlot.ForceAddNewStack(currentSlot.StackableItem.Copy());
                }
                currentSlot.RemoveStack();
                p.NotifyOfHandChange();
                RemoveCurrentSlot();
               
                return true;
            }

            return false;
        }

        private static bool IsSameSlot(InventorySlot inventorySlot)
        {
            if (inventorySlot == currentSlot)
            {
             
                return true;
            }

            return false;
        }

        private static bool IsFirstPressOnSlot(PlayerEntity e,InventorySlot inventorySlot)
        {
            if (currentSlot != null && currentSlot == inventorySlot)
            {
                currentSlot.Deselect();
                currentSlot = null;
                return true;
            }
                if (currentSlot == null)
            {
                SlotMessage a = inventorySlot.Select();

                if (a == SlotMessage.ItemExists)
                {
                    if (e.HandInventory.HasSlot(inventorySlot))
                    {
                       
                        e.SendToHand(inventorySlot.StackableItem);
                    }
                    currentSlot = inventorySlot;
                }
                else
                {
                    e.SendToHand(null);
                }

                return true;
            }

            return false;
        }

        private static void RemoveCurrentSlot()
        {
            currentSlot.Deselect();
            currentSlot = null;
        }
    }
}