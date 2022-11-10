using NoodleLand.Entities.Living.Player;
using NoodleLand.Inventory.Items;
using UnityEngine;

namespace NoodleLand.Events
{
    public class OnItemPickedEvent
    {
        [SerializeField] private PlayerEntity _playerEntity;
        [SerializeField] private StackableItem _stackableItem;
        
        public  OnItemPickedEvent(PlayerEntity playerEntity, StackableItem stackableItemPicked)
        {
            
        }
        
    }
}