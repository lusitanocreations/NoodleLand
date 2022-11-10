using System;
using System.Collections;
using Lusitano.Input;
using Lusofinn.Audio;
using NoodleLand.Enums;
using NoodleLand.Events;
using NoodleLand.Farming;
using NoodleLand.Interfaces;
using NoodleLand.Inventory.Items;
using UnityEngine;
using UnityEngine.UI;

namespace NoodleLand.Entities.Living.Player
{
public class PlayerWorldInteractionController : MonoBehaviour
{
    [SerializeField] private int health;




    public Slider slider;

    [SerializeField] private Transform spotA;
    [SerializeField] private Transform spotB;
    public GameObject test;

   public GameObject itemStackHandShower;
     
    public GameObject marker;
    
    public Transform lazer;
    public Vector2 lastDir;
    public Vector2 PointFacing { get; private set; }



    private void Awake()
    {
        canUseTool = true;  
    }

    private void Start()
    {
       RemoveStack();
    }
    
    public void CheckHand()
    {
        
            RemoveStack();
      
            
    }
    
    public StackableItem InHand { get; private set; }
    public void SetHandItem(StackableItem stackableItem)
    {
        // if(itemStack != null)
        //     itemStack.onSlotLeave = RemoveStack;
      
        this.InHand = stackableItem;

        Sprite s0 = null;
        if (InHand != null)
        {
            s0 = InHand.BaseItemData.Icon;
        }

        itemStackHandShower.GetComponent<SpriteRenderer>().sprite = s0;
    }

  
    private void RemoveStack()
    {
        Debug.Log("removed stack called");
        itemStackHandShower.GetComponent<SpriteRenderer>().sprite = null;
        InHand = null;
    }
 

    private IMarkerOn lastMarkerOn;


    public void AlertOfPlayerMovement(PlayerEntity entity)
    {
        currentInteracting?.OnLeave(new OnInteractLeaveEvent(entity));
        currentInteracting = null;
    }


    private bool isInteractingB;

    IEnumerator DelayInteractingB()
    {
        yield return new WaitForSeconds(0.5f);
        isInteractingB = false;
    }
    
    public float attackTimer = 1f;
    public void CalculateWhatHits(PlayerEntity playerEntity,Vector2 direction)
    {
        if (direction != Vector2.zero)
        {

            lastDir = direction/2f;

        }
        Debug.DrawRay(lazer.position,lastDir);

        LayerMask mask = LayerMask.NameToLayer("Player");
        RaycastHit2D hit =Physics2D.Raycast(lazer.position ,lastDir, lastDir.magnitude,~(1 << mask));
           
        GameInput gameInput = GameInput.Instance;

        PointFacing = (Vector2) lazer.position + lastDir;

        if (hit.collider != null )
        {
            CheckForMarkerInteractable(hit.collider);
     
        }
        else
        {
            if (lastMarkerOn != null)
            {
                lastMarkerOn.OnMarkerLeave();
                lastMarkerOn = null;
            }
            marker.transform.position = PointFacing;
        }
           
         
        if (gameInput.IsPressing(AnalogType.A) && canUseTool)
        {

           
            StartCoroutine(StartCooldown(attackTimer));


            float animT01 = attackTimer / 5;
            float animT02 = attackTimer / 10;
            float animT03 = attackTimer - animT01 - animT02;

            if (itemStackHandShower != null)
            {
                LeanTween.rotateZ(itemStackHandShower.gameObject, -60 * rotateModifier, animT01).setOnComplete(() =>
                {
                    LeanTween.rotateZ(itemStackHandShower.gameObject, 60 * rotateModifier, animT02).setOnComplete(() =>
                    {
                        if (hit.collider != null)
                        {
                            CheckForInteractability(hit.collider, playerEntity,AnalogType.A);

                        }
                        
                        LeanTween.rotateZ(itemStackHandShower.gameObject, 0, animT03).setOnComplete(() =>
                        {
                          
                        });
                    });
                });

            }
           
            
        }
        
        
        if (gameInput.IsPressing(AnalogType.B))
        {
            
            if (!isInteractingB && hit.collider != null && GetInteractableFrom(hit.collider, out IWorldInteractable interactable))
            {
             
                if (interactable != null)
                {
                    isInteractingB = true;
                    StartCoroutine(DelayInteractingB());
                    InteractWithObject(playerEntity, AnalogType.B, interactable);
                 

                }
            }

            else
            {
                Update_Tool_Action(playerEntity);

            }
            

          
        }
        else if (gameInput.HasReleased(AnalogType.B))
        {
            ForceStopCurrentAction();

        }
    
           
    }
    
    public void ForceStopCurrentAction()
    {
        slider.gameObject.SetActive(false);
        isPressing = false;
        if (currentAction != null) StopCoroutine(currentAction);


    }
    private Coroutine currentAction;
    private bool isPressing;
    private void Update_Tool_Action(PlayerEntity entity)
    {
        if (isPressing)
        {
            return;
        }

        isPressing = true;
        StackableItem a0 = InHand;

        

        if (InHand != null && a0.BaseItemData.ActionDuration != 0 && currentInteracting == null)
        {
            currentAction = StartCoroutine(ActionDoing(() =>
            {
                OnUseEvent onUseEvent =
                    new OnUseEvent(entity, FindObjectOfType<World>(), AnalogType.B, a0, PointFacing);
                a0.BaseItemData.OnUse(onUseEvent);
                
            }, a0.BaseItemData.ActionDuration));
        }
       
         
    }

    IEnumerator ActionDoing(Action action,float timer,bool forceSkip = false)
    {
        slider.gameObject.SetActive(true);
        slider.value = 0;
            
        while (true)
        {
            if (forceSkip) break;
            slider.value++;
            if (slider.value >= 100) break;
            yield return new WaitForSeconds(timer/100);

        }
        action?.Invoke();   
        slider.gameObject.SetActive(false);
        isPressing = false;

    }


    private bool canUseTool;

    IEnumerator StartCooldown(float timer)
    {
        canUseTool = false;
        yield return new WaitForSeconds(timer);
        canUseTool = true;

    }

   

    private IWorldInteractable currentInteracting;

    public bool IsMarking() => lastMarkerOn != null;

    private void CheckForInteractability(Collider2D collider,PlayerEntity playerEntity,AnalogType used)
    {
        IWorldInteractable interactable = collider.GetComponent<IWorldInteractable>();

        if (interactable != null)
        {
            InteractWithObject(playerEntity, used, interactable);
            FindObjectOfType<SoundSystem>().PlaySound("WeaponBeam");
        }
    }

    private void InteractWithObject(PlayerEntity playerEntity, AnalogType used, IWorldInteractable interactable)
    {
        OnInteractEnterEvent onInteractEnterEvent = new OnInteractEnterEvent(playerEntity, used);
        interactable.OnInteract(onInteractEnterEvent);
        currentInteracting = interactable;
    }

    private bool GetInteractableFrom(Collider2D c0 , out IWorldInteractable worldInteractable)
    {
        IWorldInteractable interactable = c0.GetComponent<IWorldInteractable>();

        if (interactable != null)
        {

            worldInteractable = interactable;
            return true;
        }

        worldInteractable = null;
        return false;
    }

    private void CheckForMarkerInteractable(Collider2D collider)
    {
        IMarkerOn markerOn = collider.GetComponent<IMarkerOn>();

        if (markerOn != null)
        {
            marker.transform.position =  collider.transform.position;
            
            if(lastMarkerOn != null && lastMarkerOn == markerOn) return;
             if (lastMarkerOn != null && lastMarkerOn != markerOn)
            {
                lastMarkerOn.OnMarkerLeave();
                lastMarkerOn = markerOn;
                lastMarkerOn.OnMarkerEnter();
                

            }
             else
             {
                 lastMarkerOn = markerOn;
                 lastMarkerOn.OnMarkerEnter();
                
             }
             
            
        }
        else
        {
            if (lastMarkerOn != null)
            {
                lastMarkerOn.OnMarkerLeave();
                lastMarkerOn = null;
            }
        }

    }

    private float rotateModifier = 1;
    public void ChangeWeaponSide(Facing facing)
    {
        
        if(itemStackHandShower == null) return;
        
        switch (facing)
        {
            case Facing.Left:
                rotateModifier = 1;
                itemStackHandShower.transform.localPosition = spotA.localPosition;
                itemStackHandShower.GetComponent<SpriteRenderer>().flipX = false;
                break;
            case Facing.Right:
                rotateModifier = -1;
                itemStackHandShower.transform.localPosition = spotB.localPosition;
                itemStackHandShower.GetComponent<SpriteRenderer>().flipX = true;
                break;
        }
    }
}
}