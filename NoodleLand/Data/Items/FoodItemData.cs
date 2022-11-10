using Lusitano.Input;
using NoodleLand.Entities.Living.Player;
using NoodleLand.Events;
using NoodleLand.Inventory.Items;
using UnityEngine;

namespace NoodleLand.Data.Items
{
    [CreateAssetMenu(menuName = "NoodleLand/Data/Items/New Food Item", fileName = "FoodItemData", order = 0)]
    public class FoodItemData : BaseItemData
    {
        [Header("Food Properties")]
        [SerializeField] private int foodGivenAmount;

        public int FoodGivenAmount => foodGivenAmount;
        public override ActionMessage OnUse(OnUseEvent onUseEvent)
        {
          
            if (onUseEvent.AnalogType == AnalogType.B)
            {
                onUseEvent.PlayerEntity.Feed(foodGivenAmount);
                onUseEvent.Instance.RemoveFromStack(1);
                return ActionMessage.Sucess;
            }

            return ActionMessage.Failluire;
        }
    }
}