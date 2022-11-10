using System;
using System.Collections;
using System.Collections.Generic;
using Lusitano.Input;
using NoodleLand.Data.Items.Tools;
using NoodleLand.Enums;
using NoodleLand.Events;
using NoodleLand.Farming;
using NoodleLand.Inventory;
using NoodleLand.Inventory.Items;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace NoodleLand.Entities.Living.Player
{
   
    public class PlayerEntity : LivingEntity
    {



        [FormerlySerializedAs("playerToolController")] [SerializeField] private PlayerWorldInteractionController playerWorldInteractionController;

        public const string AnimationCuttingTag = "Cutting";


        [SerializeField] private InventoryContainer mainInventory;
        [SerializeField] private InventoryContainer handInventory;

        private bool delaying;
        private bool canMove = false;
        private Coroutine c0;

        public float hunger;
        public Slider slider;

        public Vector2 FacingDirection => playerWorldInteractionController.lastDir;
        public Vector2 FacingDirectionPoint => playerWorldInteractionController.PointFacing;
        public StackableItem HandStackableItem => playerWorldInteractionController.InHand;

        public void SendToHand(StackableItem stackableItem)
        {
            playerWorldInteractionController.SetHandItem(stackableItem);
        }


        public int HandDamage => 5;

        public void Feed(int amount)
        {
            hunger += amount;
            slider.value = hunger;
        }

        public InventoryContainer MainInventory => mainInventory;
        public InventoryContainer HandInventory => handInventory;

      

        public void Start()
        {
            GameInput i = GameInput.Instance;
            i.OnJoystickTouch.AddListener(CancelInventoryOperation);
            i.OnJoystickTouch.AddListener(() =>
            {
                playerWorldInteractionController.ForceStopCurrentAction();
            });
            i.OnAnalogAPress.AddListener(CancelInventoryOperation);
            i.OnAnalogBPress.AddListener(CancelInventoryOperation);
            
            
            handInventory.OpenUI();
            InventoryEvent.InformInventoriesOpen(this,new List<InventoryContainer>(){handInventory});
        }





     
        private void CancelInventoryOperation()
        {
            InventoryEvent.CancelCurrentOperation();
        }

        IEnumerator StartDelay()
        {
            delaying = true;
            yield return new WaitForSeconds(0.1f);
            canMove = true;
           

        }



        public void NotifyOfHandChange()
        {
           
            playerWorldInteractionController.CheckHand();
        }
   
        private void Update()
        {

            GameInput gameInput = GameInput.Instance;
            
            float h = gameInput.HorizontalAnalogValue;
            float v = gameInput.VerticalAnalogValue;
            
            
            SetVelocity(Vector2.zero);
            // priority is left and right movemnet

            Vector2 direction = new Vector2(h, v);
            direction.Normalize();
          
            playerWorldInteractionController.CalculateWhatHits(this,direction);


            if (h != 0 && v != 0)
            {
            
                if (!delaying)
                {
                    c0 = StartCoroutine(StartDelay());
                }
                playerWorldInteractionController.AlertOfPlayerMovement(this);
                
            }

            if (canMove)
            {
                SetVelocity(direction, new Vector2(h,v).magnitude);
            }
            
            if (h >= 0.1)
            {
                SwitchFacing(Facing.Right);
                playerWorldInteractionController.ChangeWeaponSide(Facing.Right);
                PlayAnimation(AnimationWalkHorizontalTag);
                
            }
            else if (h <= -0.1f)
            {
               
                SwitchFacing(Facing.Left);
                playerWorldInteractionController.ChangeWeaponSide(Facing.Left);
                PlayAnimation(AnimationWalkHorizontalTag);
            }
            
             
            if (v >= 0.1f)
            {
                SwitchFacing(Facing.Top);
                PlayAnimation(AnimationWalkHorizontalTag);
                
            }
            else if (v <= -0.1f)
            {
               
                SwitchFacing(Facing.Bottom);
                PlayAnimation(AnimationWalkHorizontalTag);
            }

           
            if (v == 0 && h == 0)
            {
                PlayAnimation(AnimationIdleTag);
                StopCoroutine(StartDelay());
                canMove = false;
                delaying = false;
            }
            
          
        }


        public void AlertOfStackChange(StackableItem instance)
        {
            if (HandStackableItem != null && HandStackableItem == instance)
            {
                CheckIfStillExists(instance);
            }
            
        }

        private void CheckIfStillExists(StackableItem stackableItem)
        {
            if (stackableItem.Quantity <= 0)
            {
                playerWorldInteractionController.SetHandItem(null);
            }
        }

        public void InformOfItemBreak(StackableItem stackableItem)
        {
            if (HandStackableItem != null)
            {
                if (HandStackableItem == stackableItem)
                {
                    playerWorldInteractionController.SetHandItem(null);
                }
            }
        }
    }
}