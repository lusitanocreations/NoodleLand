using NoodleLand.Data.Databases;
using NoodleLand.Data.Entities;
using NoodleLand.Entities.Living.Player;
using NoodleLand.Events;
using NoodleLand.Inventory.Items;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace NoodleLand.Data.Items
{

    public enum ActionMessage
    {
        Sucess,
        Failluire
    }
    
    
    [CreateAssetMenu(menuName = "NoodleLand/Data/Items/New Base Item", fileName = "BaseItemData", order = 0)]
    public class BaseItemData : ScriptableObject, IData
    {
        [Header("Basic Properties")]
        [SerializeField] private Sprite _sprite;
        [SerializeField] private bool isStackable;
        [FormerlySerializedAs("itemName")] [SerializeField] private string itemTag;
        [Header("Complex Properties")]
        [SerializeField] private float actionTime;

        [Header("UI Properties")] [SerializeField]
        private Color _colorMultiplier = Color.white;

        public Color ColorMultiplier => _colorMultiplier;

        [Header("Debug")] [SerializeField] private bool ForceRegister;
        public virtual MaterialType GetMaterialType()
        {
            return MaterialType.None;
        }
        
        

        public virtual void OnStackChange(StackableItem instance, PlayerEntity playerEntity)
        {
            if (instance.Quantity == 0)
            {
                playerEntity.AlertOfStackChange(instance);
            }
        }
        
        public bool IsOfType<T>(T t) where T : BaseItemData
        {
            return GetType() == typeof(T);
        }

        public virtual int GetItemDamage()
        {
            return 5;
        }
        public virtual ActionMessage OnUse(OnUseEvent onUseEvent)
        {

            return ActionMessage.Failluire;

        }

        private void OnValidate()
        {
            if (ForceRegister)
            {
                ForceRegister = false;
                RegisterSelf();
            }

            if (_colorMultiplier.a == 0)
            {
                _colorMultiplier = Color.white;
            }
        }

        protected virtual void Awake()
        {
            RegisterSelf();
        }

        private void RegisterSelf()
        {
            var objs = AssetDatabase.LoadAllAssetsAtPath(ItemDatabase.FolderPath);

            if (objs != null && objs.Length != 0)
            {
                foreach (var o in objs)
                {
                    if (o is ItemDatabase entityDatabase)
                    {
                        if(! entityDatabase.Has(this))
                            entityDatabase.Register(this);
                        return;
                    }    
                   
                }
            }
        }

        public float ActionDuration => actionTime;
        public Sprite Icon => _sprite;
        public bool IsStackable => isStackable;
        public string ItemTag => itemTag;
        public string GetTag()
        {
            return itemTag;
            
        }
    }
}