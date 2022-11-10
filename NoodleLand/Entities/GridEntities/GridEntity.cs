using System.Collections;
using System.Collections.Generic;
using Lusitano.Input;
using Lusitano.UI;
using NoodleLand.Entities.Item;
using NoodleLand.Entities.Living.Player;
using NoodleLand.Events;
using NoodleLand.Farming;
using NoodleLand.Interfaces;
using NoodleLand.Inventory.Items;
using NoodleLand.Registeries;
using UnityEngine;

namespace NoodleLand.Entities.GridEntities
{

    public enum DropCondition
    {
        IsGrown,
        IsAge0,
        IsAge1,
        IsAge2,
        IsAge3,
        IsAge4,
        
    }
    

    
    [RequireComponent(typeof(SpriteRenderer))]
    public class GridEntity : Entity, IMarkerOn, IGridObject,IWorldInteractable
    {
        
        
        [Header("On Grid Placement")]
        [SerializeField] private Vector2 offset;

      
        [SerializeField] private int hardened;
        [SerializeField] private MaterialType _materialType = MaterialType.Stone;
        [Header("Entity Properties")]
        [SerializeField] private Bar healthBar;
        [SerializeField] private int health;
        
        protected SpriteRenderer _spriteRenderer;
        
        private Coroutine HUD_Dissapear;

        private int maxHealth;

        private Dictionary<DropCondition, bool> dropConditions = new Dictionary<DropCondition, bool>();


      

        protected virtual void OnConditionsInitialise(Dictionary<DropCondition, bool> drops)
        {
            
        }

        public void ChangeDropCondition(DropCondition dropCondition, bool value)
        {
            if(dropConditions.ContainsKey(dropCondition))
            {
                dropConditions[dropCondition] = value;

            }
        }
        public virtual void OnSpawn()
        {
            
        }
       
     

        protected override void Awake()
        {
            base.Awake();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            dropConditions = new Dictionary<DropCondition, bool>();
            OnConditionsInitialise(dropConditions);
           
        }

        protected override void Start()
        {
            base.Start();
            lastScale = transform.localScale;
            maxHealth = health;
            
          
        }

        protected virtual void OnObjectDeath(bool shouldDropItems = true)
        {
            if (shouldDropItems)
            {
                var dropProperties = RegisteredDrops.Instance.Get(EntityTag);
            
            if (dropProperties != null)
            {
                foreach (var dropProperty in dropProperties.Drops)
                {
                    if (dropProperty.HasCondition)
                    {
                        DropCondition condition = dropProperty.DropCondition;

                        if (dropConditions.TryGetValue(condition, out bool hasFullfiled))
                        {

                          
                            if (hasFullfiled)
                            {
                                //   TODO MAKE AN OBJECT POOL INSTEAD
                                ItemEntity g0 = Instantiate(FindObjectOfType<ItemEntity>(), transform.position,
                                    Quaternion.identity);
                                g0.Construct(new StackableItem(dropProperty.ConditionDrop,1));
                                
                                Recycle();
                                return;
                            }
                        }
                        else
                        {

                        }
                    
                    }

                    int quantityToSpawn = 0;
                    if (dropProperty.IsRandom)
                    {
                        quantityToSpawn = UnityEngine.Random.Range(dropProperty.MinQuantity, dropProperty.MaxQuantity);
                    }
                    else
                    {
                        quantityToSpawn = dropProperty.Quantity;
                    }

                    for (int i = 0; i < quantityToSpawn; i++)
                    {
                        ItemEntity g0 = Instantiate(FindObjectOfType<ItemEntity>(), transform.position,
                            Quaternion.identity);
                        g0.Construct(new StackableItem(dropProperty.Drop, 1));
                    }
                }
            }
            }
            
            
            Recycle();
          
            
        }
        IEnumerator DisapearKappa()
        {
            yield return new WaitForSeconds(4f);
            healthBar.gameObject.SetActive(false);
        }

        private void UpdateUI()
        {
            if (healthBar != null)
            {
                healthBar.Maximum = maxHealth;
                healthBar.Minimum = 0;
                healthBar.ForceSetCurrent(0); 
            }
        }


        protected virtual void OnDamageTaken(GameObject source)
        {
            
        }
        public void TakeDamage(GameObject source,int amount)
        {

            DamagePopUp d0 = FindObjectOfType<DamagePopUp>();
            Instantiate(d0, transform.position + new Vector3(0,0.4f,0), Quaternion.identity).Set(amount);
            UpdateUI();
            StartHidingHUD();
            health -= amount;
            OnDamageTaken(source);
          
            if (healthBar != null)
            {
                healthBar.Remove(amount);
                UpdateUI();
            }

            if (health <= 1)
            {
                OnObjectDeath();
            }

        }

        
        private void StartHidingHUD()
        {
            if(healthBar.gameObject == null) return;
            
            
            healthBar.gameObject.SetActive(true);
            if(HUD_Dissapear == null)
                HUD_Dissapear = StartCoroutine(DisapearKappa());
            else
            {
                StopCoroutine(HUD_Dissapear);
                HUD_Dissapear = StartCoroutine(DisapearKappa());
            }
        }
        public void Recycle()
        {
            World.RemoveEntity(this);
           
        }


        private bool hasReached;
        private Vector3 lastScale;
        public virtual void OnMarkerEnter()
        {


            float multiplier = 0.125f;
            LeanTween.scale(gameObject,lastScale + new Vector3(multiplier, multiplier, multiplier),0.2f);

        }

     

        public virtual void OnMarkerLeave()
        {
            LeanTween.scale(gameObject,lastScale,0.2f);
        }

        public GameObject Get()
        {
            return gameObject;
        }

    
        public Vector2 Offset()
        {
            return offset;

        }


        public virtual void OnInteract(OnInteractEnterEvent onInteractEnterEvent)
        {
            if (onInteractEnterEvent.analogUsed == AnalogType.A)
            {
                StackableItem stackableItem = onInteractEnterEvent.player.HandStackableItem;
                
                if (stackableItem == null )
                {
                    if (_materialType == MaterialType.None)
                    {
                        TakeDamage(onInteractEnterEvent.player.gameObject,onInteractEnterEvent.player.HandDamage);

                    }
                    else
                    {
                        TakeDamageCalculated(onInteractEnterEvent.player,onInteractEnterEvent.player.HandDamage);
                    }
            
                }
                else
                {
                    int damage = stackableItem.BaseItemData.GetItemDamage();
                    
                    if (_materialType == MaterialType.None)
                    {
                        TakeDamage(onInteractEnterEvent.player.gameObject,damage);
                      
                    }
                    else if (_materialType != stackableItem.BaseItemData.GetMaterialType())
                    {
                        TakeDamageCalculated(onInteractEnterEvent.player,damage);
                    }
                    else
                    {
                        TakeDamage(onInteractEnterEvent.player.gameObject,Mathf.RoundToInt(damage * 1.25f));
                    }
                 

                }
            }
        }

        private void TakeDamageCalculated(PlayerEntity  playerEntity,int damage)
        {
            if (hardened <= 1)
            {
                TakeDamage(playerEntity.gameObject,Mathf.RoundToInt(damage));
            }
            else
            {
                int dm = Mathf.RoundToInt(damage * 0.50f / hardened);
                if (dm <= 1)
                {
                    dm = 0;
                }

                TakeDamage(playerEntity.gameObject,dm);
            }
        }
       
        public virtual void OnLeave(OnInteractLeaveEvent onInteractLeaveEvent)
        {
            
        }
    }
}