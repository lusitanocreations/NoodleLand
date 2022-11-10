using NoodleLand.Inventory.Items;

namespace NoodleLand.Events.Item
{
    public class ItemStackModifiedEvent: IEvent
    {
        public StackableItem Modified { get; }
        public ItemStackModifiedEvent(StackableItem modified)
        {
            Modified = modified;

        }
        
    }
}