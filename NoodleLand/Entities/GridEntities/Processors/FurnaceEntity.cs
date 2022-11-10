using NoodleLand.Data.Items.Fuel;
using NoodleLand.Events;
using NoodleLand.Farming;
using NoodleLand.Inventory;
using UnityEngine;
using UnityEngine.UI;

namespace NoodleLand.Entities.GridEntities.Processors
{
    public class FurnaceEntity : ContainerEntity, ITickable
    {
        [SerializeField] private Image progressImage;

        private InventorySlot input;
        private InventorySlot output;
        private InventorySlot fuel;

        protected override void Awake()
        {
            base.Awake();

            input = container.Slots[0];
            output = container.Slots[1];
            fuel = container.Slots[2];
        }

        private bool isOpened;
        public override void OnInteract(OnInteractEnterEvent onInteractEnterEvent)
        {
            base.OnInteract(onInteractEnterEvent);
            isOpened = !isOpened;
            if (isOpened)
            {
                progressImage.gameObject.SetActive(true);
            }
            else
            {
                progressImage.gameObject.SetActive(false);
            }
        }

        private float amountToTick;
        private bool isBurning;

        //TODO MELHORAR ESTA PARTE
        public void OnTick(World world)
        {
            Debug.Log(!fuel.IsEmpty() && fuel.StackableItem.BaseItemData is FuelItem kappa);
          
            if (!isBurning && !fuel.IsEmpty() && fuel.StackableItem.BaseItemData is FuelItem fuelItem)
            {
                Debug.Log("found fuel");
                float burnTimer = fuelItem.BurnTimer;
                amountToTick = burnTimer;
                fuel.StackableItem.RemoveFromStack(1);
                isBurning = true;

            }


            if (amountToTick == 0)
            {
                isBurning = false;
                return;
            }


            if (isOpened)
            {
                progressImage.gameObject.SetActive(true);
                progressImage.fillAmount += 1f / 100f;
            
                if(progressImage.fillAmount >= 1)
                {
                    progressImage.fillAmount = 0;
                }

            }
    


        }
    }
}
