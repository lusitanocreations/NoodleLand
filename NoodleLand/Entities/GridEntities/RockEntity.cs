using Lusitano.Input;
using NoodleLand.Data.Items;
using NoodleLand.Entities.Item;
using NoodleLand.Events;
using NoodleLand.Inventory.Items;
using NoodleLand.Registeries;
using UnityEngine;

namespace NoodleLand.Entities.GridEntities
{
    public class RockEntity : GridEntity
    {
        public override void OnInteract(OnInteractEnterEvent onInteractEnterEvent)
        {
            base.OnInteract(onInteractEnterEvent);

            if (onInteractEnterEvent.analogUsed == AnalogType.A)
            {
                StackableItem handStackableItem = onInteractEnterEvent.player.HandStackableItem;
                if(handStackableItem != null)
                {

                    if (handStackableItem.BaseItemData.ItemTag == "Flint")
                    {
                       handStackableItem.RemoveFromStack(1);
                       
                       ItemEntity e0 = Instantiate(FindObjectOfType<ItemEntity>());
                       BaseItemData it = FindObjectOfType<RegisteredItems>().Get("FlintTip");

                       for (int i = 0; i < Random.Range(1,2); i++)
                       {
                           e0.Construct(new StackableItem(it,1));

                       }
                       World.AddEntity(e0, transform.position);
                       
                    }
                }
            }
         
        }
    }
}