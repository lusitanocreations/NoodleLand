using System.Collections.Generic;
using NoodleLand.Data.Items;
using NoodleLand.Serialization.BDS;
using UnityEngine;

namespace NoodleLand.Entities.GridEntities.Storage
{
    [System.Serializable]
    public class ItemStackTest
    {
        [SerializeField] private BaseItemData _itemData;
        [SerializeField] private int quantity;

        public BaseItemData ItemData => _itemData;
        public int Quantity => quantity;
    }
    public class TestContainerEntity : ContainerEntity
    {
        public List<ItemStackTest> toAddToChest;

        protected override void Awake()
        {
            foreach (var itemStackTest in toAddToChest)
            {
                container.TryAdd(itemStackTest.ItemData,itemStackTest.Quantity);
            }
        }

        protected override void CustomSaveEntity(LDSDictionary ldsDictionary)
        {
           
        }

        protected override void CustomLoadEntity(LDSDictionary ldsDictionary)
        {
            
        }
    }
}