using UnityEngine;
using UnityEngine.Serialization;

namespace NoodleLand.Data.Items.Tools
{
    public class ToolData : BaseItemData
    {
       [SerializeField] private MaterialType materialType;


        public override MaterialType GetMaterialType()
        {
            return materialType;
        }

        public int damage;

        public override int GetItemDamage()
        {
            Debug.Log(damage);
            return damage;
        }
    }
}