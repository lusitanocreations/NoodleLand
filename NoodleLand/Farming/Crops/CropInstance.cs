using System;
using System.Collections.Generic;
using NoodleLand.Data;
using NoodleLand.Data.Crops;
using NoodleLand.Enums;
using Unity.VisualScripting;
using UnityEngine;


namespace NoodleLand.Farming.Crops
{
    
    [RequireComponent(typeof(SpriteRenderer))]
    public class CropInstance : MonoBehaviour
    {
        [SerializeField] private CropData _cropData;
        
        private SpriteRenderer _spriteRenderer;
        private FarmingAge _currentFarmingAge;
        private Dictionary<int, Sprite> age_to_sprite;
        private int currentAge = 0;
        private int maxAgeFase = 0;
        
        private void Awake()
        {
            Construct(_cropData);
        }

        public CropInstance Construct(CropData cropData)
        {

            _cropData = cropData;
            age_to_sprite = new Dictionary<int, Sprite>();
            currentAge = 0;
            
       
            for (var i = 0; i < cropData.FarmingAgeCount; i++)
            {
                FarmingAge farmingAge = cropData.FarmingFarmingAges[i];
             
                int age = farmingAge.Age;
                
                age_to_sprite.Add(age,farmingAge.Sprite);
                
                if (maxAgeFase < age)
                {
                    maxAgeFase = age;

                }

            }
            
        
            SetSpriteFromAge(0);
            
            return this;

        }

        private float k0;
        private void OnTick(World world)
        {
            k0 += Time.deltaTime;
            if (k0 >= 1)
            {
                int rn = UnityEngine.Random.Range(0, 100);
                if (rn <= 15)
                {
                    AgeUp();
                }
                k0 = 0;
            }

        }

        private void SetSpriteFromAge(int ageFase)
        {
            _spriteRenderer.sprite = age_to_sprite[ageFase];

        }
        public void AgeUp()
        {
            int next = currentAge + 1;
            
            if (next >= maxAgeFase)
            {
                // remove from tickers
                return;
            }

           // Debug.Log("age up");
            currentAge = next;
            SetSpriteFromAge(currentAge);

        }      
    }
}