using System.Collections.Generic;
using Lusitano.Input;
using NoodleLand.Data.Items;
using NoodleLand.Entities.GridEntities;
using NoodleLand.Entities.Item;
using NoodleLand.Entities.Living.Player;
using NoodleLand.Events;
using NoodleLand.Inventory;
using NoodleLand.Inventory.Items;
using NoodleLand.Registeries;
using NoodleLand.Serialization.BDS;
using UnityEngine;
using UnityEngine.Serialization;

public enum MaterialType
{
    None,
    Wood,
    Stone
}
public class ContainerEntity : GridEntity
{
    [FormerlySerializedAs("containerUI")] [SerializeField] protected InventoryContainer container;

    private bool isOpen;

    private void DropContainerToWorld()
    {
        for (var i = 0; i < container.Slots.Length; i++)
        {
            InventorySlot slot = container.Slots[i];
            if (!slot.IsEmpty())
            {
                ItemEntity g0 = Instantiate(FindObjectOfType<ItemEntity>(), transform.position,
                    Quaternion.identity);
                g0.Construct(slot.StackableItem);
            }

            slot.RemoveStack();
        }
    }

    protected override void CustomSaveEntity(LDSDictionary ldsDictionary)
    {
       

        List<int> quantities = new List<int>();
        List<string> itemTags = new List<string>();
        List<int> positionAt = new List<int>();

        for (var i = 0; i < container.Slots.Length; i++)
        {
            if (!container.Slots[i].IsEmpty())
            {
                StackableItem stackableItem = container.Slots[i].StackableItem;
                quantities.Add(stackableItem.Quantity);
                itemTags.Add(stackableItem.BaseItemData.ItemTag);
                positionAt.Add(i);

            }
          
        }
        
        ldsDictionary.SetList("quantities",quantities);
        ldsDictionary.SetList("position",positionAt);
        ldsDictionary.SetStringList("itemTags",itemTags);
    }

    protected override void CustomLoadEntity(LDSDictionary ldsDictionary)
    {

        List<int> quantities = ldsDictionary.GetList<int>("quantities");
        List<string> itemTags = ldsDictionary.GetStringList("itemTags");
        List<int> positionsAt = ldsDictionary.GetList<int>("position");
        
        for (var i = 0; i < quantities.Count; i++)
        {
            BaseItemData itemData = FindObjectOfType<RegisteredItems>().Get(itemTags[i]);
            StackableItem instance = new StackableItem( itemData,quantities[i]);

            int positionAt = positionsAt[i];

            container.ForceAddInSlot(instance, positionAt);
        }



    }

    protected override void OnDamageTaken(GameObject source)
    {
        PlayerEntity player = source.GetComponent<PlayerEntity>();
        if (player != null)
        {
            OnLeave(new OnInteractLeaveEvent(player));
        }
    }

    protected override void OnObjectDeath(bool shouldDropItems = true)
    {
        base.OnObjectDeath(shouldDropItems);
        
        DropContainerToWorld();

        
        
    }
    
    

    public override void OnInteract(OnInteractEnterEvent onInteractEnterEvent)
    {
        base.OnInteract(onInteractEnterEvent);
        
        
        if (onInteractEnterEvent.analogUsed == AnalogType.B)
        {
            
            Debug.Log("b was used");
            if (isOpen)
            {
                OnLeave(new OnInteractLeaveEvent(onInteractEnterEvent.player));
                return;
            }

            InventoryEvent.InformInventoriesOpen(onInteractEnterEvent.player,new List<InventoryContainer>(){container,onInteractEnterEvent.player.HandInventory});
            isOpen = true;
            container.OpenUI();
        }

    }


    public override void OnLeave(OnInteractLeaveEvent onInteractEnterEvent)
    {
        InventoryEvent.InformInventoryCancel();
        InventoryEvent.InformInventoriesOpen(onInteractEnterEvent.player,new List<InventoryContainer>(){onInteractEnterEvent.player.HandInventory});
        isOpen = false;
        container.CloseUI();
    }
}
