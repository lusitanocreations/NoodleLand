using NoodleLand.Enums;
using UnityEngine;

namespace NoodleLand.Farming
{
    [System.Serializable]
    public class FarmingAge
    {
        [SerializeField] private Sprite sprite;
        [SerializeField] private int age;

        public int Age => age;
        public Sprite Sprite => sprite;

    }
}