using System.Collections.Generic;
using NoodleLand.Data.Databases;
using NoodleLand.Data.Entities;
using NoodleLand.Data.Items;
using NoodleLand.Entities.GridEntities;
using UnityEditor;
using UnityEngine;

namespace NoodleLand.Data.Drops
{
   
    [System.Serializable]
    public class DropProperty
    {
        [SerializeField] private BaseItemData drop;
        [Range(1,10)]
        [SerializeField] private int quantity;

        [Header("If Random")] public bool isRandom;
        [SerializeField]private int minQuantity;
        [SerializeField] private int maxQuantity;
        [Header("Override if has conditions")] 
        private bool hasConditions;
        private DropCondition condition;
        private BaseItemData conditionDrop;

        public BaseItemData ConditionDrop => conditionDrop;

        public bool HasCondition => hasConditions;
        public DropCondition DropCondition => condition;


        
        
        
        public bool IsRandom => isRandom;
        public int MinQuantity => minQuantity;
        public int MaxQuantity => maxQuantity;
        public BaseItemData Drop => drop;
        public int Quantity => quantity;

    }
    
    [CreateAssetMenu(menuName = "NoodleLand/Data/Drops/New Drop", fileName = "Drop", order = 0)]
    public class DropData : ScriptableObject,IData
    {
        [SerializeField] private List<DropProperty> drop;
        [SerializeField] private string tag;

        [Header("debug")] public bool forceRegister;

        public List<DropProperty> Drops => drop;
        public string Tag => tag;


        private void OnDestroy()
        {
            Debug.Log("called");
        }


        private void OnValidate()
        {
            if (string.IsNullOrEmpty(tag))
            {
                tag = name;
            }

            if (forceRegister)
            {
                forceRegister = false;
                RegisterSelf();
            }
        }

        private void RegisterSelf()
        {
            var objs = AssetDatabase.LoadAllAssetsAtPath(DropDatabase.FolderPath);
            tag = name;

            if (objs != null && objs.Length != 0)
            {
                foreach (var o in objs)
                {
                    if (o is DropDatabase dropDatabase)
                    {
                        if(! dropDatabase.Has(this))
                            dropDatabase.Register(this);
                        return;
                    }    
                   
                }
            }
        }
        private void Awake()
        {
            RegisterSelf();

           
         
        }

        public string GetTag()
        {
            return tag;
        }
    }
}