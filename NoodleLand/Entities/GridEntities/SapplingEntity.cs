using NoodleLand.Farming;
using UnityEngine;

namespace NoodleLand.Entities.GridEntities
{
    public class SapplingEntity : GridEntity, ITickable
    {
        [SerializeField] private GridEntity tree;
        private float t;


        [Range(100,3000)]
        public float maxGrowTime;

        private float randomgrowTime;

        public override void OnSpawn()
        {
            base.OnSpawn();

            t = 0;
            randomgrowTime = Random.Range(0, maxGrowTime);

        }

        public void OnTick(World world)
        {
            t++;
            if (t >= randomgrowTime)
            {
                GrowToTree(World);
                
            }

        }

        private void GrowToTree(World world)
        {
            OnObjectDeath(false);
            world.AddEntity(Instantiate(tree), transform.position);
           
        }
    }
}