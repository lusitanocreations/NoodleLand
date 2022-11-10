
using NoodleLand.Inventory.Items;

namespace NoodleLand.Events.Player
{
    public class ItemCollectedEvent : IEvent
    {
        public StackableItem Collected { get; }

        public ItemCollectedEvent(StackableItem stackableItem)
        {
            Collected = stackableItem;
        }
    }
}