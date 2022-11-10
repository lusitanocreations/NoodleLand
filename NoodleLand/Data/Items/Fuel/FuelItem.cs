using UnityEngine;

namespace NoodleLand.Data.Items.Fuel
{
    [CreateAssetMenu(menuName = "NoodleLand/Data/Items/New Fuel Item", fileName = "Fuel Item", order = 0)]
    public class FuelItem : BaseItemData
    {
        [SerializeField] private float burnTime;


        public float BurnTimer => burnTime;

    }
}