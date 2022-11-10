using Lusitano.Input;
using NoodleLand.Entities.Living.Player;
using NoodleLand.Farming;
using NoodleLand.Inventory.Items;
using UnityEngine;

namespace NoodleLand.Events
{
    public class OnUseEvent
    {
        public PlayerEntity PlayerEntity { get; }
        public AnalogType AnalogType { get; }
        public StackableItem Instance { get; }
        public World world { get; }

        public Vector2 location { get; }

        public OnUseEvent(PlayerEntity playerEntity, World world,AnalogType analogType, StackableItem instance,Vector2 location)
        {
            this.PlayerEntity = playerEntity;
            this.AnalogType = analogType;
            this.Instance = instance;
            this.world = world;
            this.location = location;

        }
    }
}