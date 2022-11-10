using System.Collections.Generic;
using NoodleLand.Farming;
using UnityEngine;

namespace NoodleLand.Data.Crops
{
    [CreateAssetMenu(menuName = "NoodleLand/Data/Crops/New Crop", fileName = "New Crop", order = 0)]
    public class CropData : ScriptableObject
    {
        [SerializeField] private List<FarmingAge> farmingAges;
        [SerializeField] private string cropName;
        public List<FarmingAge> FarmingFarmingAges => farmingAges;
        public string CropName => cropName;

        public int FarmingAgeCount => FarmingFarmingAges.Count;
        
       


    }
}