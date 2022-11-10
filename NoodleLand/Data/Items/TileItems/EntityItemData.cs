using NoodleLand.Entities.GridEntities;
using NoodleLand.Events;
using NoodleLand.Farming;
using NoodleLand.MessageHandling.World;
using UnityEngine;
using UnityEngine.Serialization;

namespace NoodleLand.Data.Items.TileItems
{
    [CreateAssetMenu(menuName = "NoodleLand/Data/Items/New NonLiving Entity Data", fileName = "NonLivingEntityItemData", order = 0)]
    public class EntityItemData : BaseItemData
    {

        [FormerlySerializedAs("nonLivingEntity")] [SerializeField] private GridEntity gridEntity;

        public override ActionMessage OnUse(OnUseEvent eOnUseEvent)
        {

            World world = eOnUseEvent.world;

            if (world.CanPlaceAt(eOnUseEvent.location))
            {
                GridEntity e0 = Instantiate(gridEntity);
                world.AddEntity(e0, eOnUseEvent.location);
                eOnUseEvent.Instance.RemoveFromStack(1);
                OnStackChange(eOnUseEvent.Instance,eOnUseEvent.PlayerEntity);
                return ActionMessage.Sucess;

            }

            return ActionMessage.Failluire;

        }
    }
}