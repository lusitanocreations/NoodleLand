using UnityEngine;

namespace NoodleLand.Entities.Living
{
    [System.Serializable]
    public struct LivingEntityProperties
    {
        [SerializeField] private float speed;
        public float Speed => speed;

    }
}