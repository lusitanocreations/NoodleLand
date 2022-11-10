using System.Collections.Generic;
using NoodleLand.Data.Crops;
using NoodleLand.Farming;
using UnityEngine;

namespace NoodleLand.Entities.GridEntities.Farming.Crops
{
    public class CropEntity : GridEntity, ITickable
    {
        [SerializeField] private CropData _cropData;
        
    
        private FarmingAge _currentFarmingAge;
        private Dictionary<int, Sprite> age_to_sprite;
        private int currentAge = 0;
        private int maxAgeFase = 0;


        protected override void Awake()
        {
            base.Awake();
            
            age_to_sprite = new Dictionary<int, Sprite>();
          
            for (var i = 0; i < _cropData.FarmingAgeCount; i++)
            {
                FarmingAge farmingAge = _cropData.FarmingFarmingAges[i];
             
                int age = farmingAge.Age;
                
                age_to_sprite.Add(age,farmingAge.Sprite);
                
                if (maxAgeFase <= age)
                {
                    maxAgeFase = age;

                }

            }
            currentAge = 0;
            SetSpriteFromAge(0);

        }

        public override void OnSpawn()
        {
            base.OnSpawn();
            Construct();
        }

        public void Construct()
        {
            
            currentAge = 0;
            SetSpriteFromAge(0);


        }

        private float ticks;


        private bool ReachedMaxAge()
        {
            return currentAge == maxAgeFase;
        }

        public void OnTick(World world)
        {
            if (ReachedMaxAge())
            {
                ChangeDropCondition(DropCondition.IsGrown,true);
                world.RemoveTickableObject(this);
                return;
            }

            ticks++;
            if (ticks >= 100)
            {
                ticks = 0;
                if(CanGrow())
                    AgeUp();
            }
        }

        public bool CanGrow()
        {
            return !ReachedMaxAge();
        }

        private void SetSpriteFromAge(int ageFase)
        {
            _spriteRenderer.sprite = age_to_sprite[ageFase];
            

        }
        
        public void AgeUp()
        {
            int next = currentAge + 1;
            
            if (next > maxAgeFase)
            {
                
                return;
            }

            // Debug.Log("age up");
            currentAge++;
            SetSpriteFromAge(currentAge);

        }

        protected override void OnConditionsInitialise(Dictionary<DropCondition, bool> drops)
        {
            base.OnConditionsInitialise(drops);
            drops.Add(DropCondition.IsGrown,false);
        }
    }
    }
