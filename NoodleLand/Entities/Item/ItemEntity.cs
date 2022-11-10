using System.Collections;
using NoodleLand.Entities.Living.Player;
using NoodleLand.Events;
using NoodleLand.Events.Player;
using NoodleLand.Inventory.Items;
using NoodleLand.MessageHandling.Inventory;
using UnityEngine;

namespace NoodleLand.Entities.Item
{
    
    [RequireComponent(typeof(SpriteRenderer),typeof(Rigidbody2D))]
    public class ItemEntity : Entity
    {
        private SpriteRenderer _spriteRenderer;
        private Rigidbody2D _rigidbody2D;
        private StackableItem _stackableItem;
       

        private bool canBePicked;
        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _rigidbody2D = GetComponent<Rigidbody2D>();

        }

        private void Start()
        {
            _rigidbody2D.gravityScale = 0;
        }


        private float k0;
        private byte modifier;
        private bool sh;

        private bool canPick;
        
        IEnumerator InvFrame()
        {
            canPick = false;
            yield return new WaitForSeconds(0.6f);
            canPick = true;
        }
        
        IEnumerator Magnt()
        {
            yield return new WaitForSeconds(2f);


            while (true)
            {
                // TODO MAKE THIS BETTER, CURRENTLY ITS REALLY HEAVY. 
                Collider2D[] c0 = Physics2D.OverlapCircleAll(transform.position, 3f);
            for (var i = 0; i < c0.Length; i++)
            {

                PlayerEntity p0 = c0[i].transform.GetComponent<PlayerEntity>();
                if (p0 != null && p0.HandInventory != null && p0.HandInventory.HasSpace())
                {
                    LeanTween.move(gameObject, c0[i].gameObject.transform, 0.2f);
                    break;
                }
                yield return null;

            }

            yield return new WaitForSeconds(3f);
            }
        }
        private void Update()
        {
            Vector2 p = new Vector2(transform.position.x, transform.position.y);
            
            k0 += Time.deltaTime;
            
            if (sh)
            {
                p.y += Time.deltaTime / 6;
               
            }
            else
            {
                p.y -= Time.deltaTime / 6;
            }
      

            if (k0 >= 1f)
            {
                sh = !sh;
                k0 = 0;
            }
           
            transform.position = p;
            
        }

        private void Animate()
        {
            int rhX = 1;
            int rhY = 1;


            StartCoroutine(InvFrame());
            StartCoroutine(Magnt());
            
            float forceRX = UnityEngine.Random.Range(1, 2.5f);
            float forceRY = UnityEngine.Random.Range(1, 2.5f);
            
            int rnX= UnityEngine.Random.Range(0, 100);
            int rnY= UnityEngine.Random.Range(0, 100);
            if (rnX <= 50)
            {
                rhX = -1;
            }
            
            if (rnY <= 50)
            {
                rhY = -1;
            }
            _rigidbody2D.AddForce(new Vector2(forceRX * rhX, forceRY * rhY),ForceMode2D.Impulse);
        }
        private void OnTriggerEnter2D(Collider2D col)
        {
            if(!canPick) return;
            PlayerEntity playerEntity = col.transform.GetComponent<PlayerEntity>();
            if (playerEntity != null)
            {
                var b = playerEntity.HandInventory.TryAdd(_stackableItem);
                Debug.Log(b);
                var a = b == InventoryMessage.AddedToInventory;
                if (a)
                {
                    FakeEventBus.InvokeEvent(new ItemCollectedEvent(_stackableItem));
                    Destroy(gameObject);

                };
            }
        }

       

        public void Construct(StackableItem stackableItem)
        {
          

            this._stackableItem = stackableItem; _spriteRenderer.sprite = stackableItem.BaseItemData.Icon;
            _spriteRenderer.color *= stackableItem.BaseItemData.ColorMultiplier;
            Animate();
          
        }
    
    }
}