using System;
using System.Collections;
using JetBrains.Annotations;
using NoodleLand.Data.Items;
using NoodleLand.Entities.Living.Player;
using NoodleLand.Events;
using NoodleLand.Events.Item;
using NoodleLand.Inventory.Items;
using NoodleLand.MessageHandling.Inventory;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;


namespace NoodleLand.Inventory
{
    public class InventorySlot : MonoBehaviour, IPointerDownHandler, IEquatable<InventorySlot>,IPointerUpHandler
    {

        [Header("UI Dependencies")] 
        [SerializeField] private Image itemIcon;
        [SerializeField] private Slider _slider;
        [SerializeField] private GameObject marker;
        [SerializeField] private TextMeshProUGUI itemQuantityText;
        private StackableItem _stackableItem;


        public StackableItem StackableItem => _stackableItem;
        
        private Guid _guid;

        private void Awake()
        {
            _guid = Guid.NewGuid();
        }

        

        public UnityEvent OnSlotPress;
        public UnityEvent<float> OnSlotHold;

        public bool IsSameItem(BaseItemData itemData)
        {
            if (_stackableItem == null) return false;

            return _stackableItem.BaseItemData == itemData;

        }

        public bool SplitItemStack(out StackableItem stackableItem)
        {
            if (this._stackableItem.Split(out stackableItem))
            {
                UpdateUI();
                return true;
            }

            return false;
        }
        public bool IsSameItem(StackableItem stackableItem)
        {
            if (_stackableItem == null || stackableItem == null) return false;

            return IsSameItem(stackableItem.BaseItemData);

        }

        public bool IsEmpty()
        {
            return _stackableItem == null;
        }
        public void AddToStack(int amount)
        {
            if (_stackableItem != null)
            {
                _stackableItem.AddToStack(amount);
                CheckIfStackStillExists();
                UpdateUI();
                
            }
        }

        private void CheckIfStackStillExists()
        {
            if (_stackableItem is {Quantity: 0})
            {
                _stackableItem = null;
            }
        }
        public SlotMessage Select()
        {
            if (_stackableItem == null)
            {
                return SlotMessage.SlotEmpty;
            }
            SelectMarker(true);
            UpdateUI();
            return SlotMessage.ItemExists;
        }
        public void Deselect()
        {
            SelectMarker(false);
        }

        public void RemoveStack()
        {
            //_itemStack?.onSlotLeave?.Invoke();
            this.itemIcon.color = Color.white;
            _stackableItem = null;
            UpdateUI();
            
            
        }

        private void SetUi(Sprite sprite, int qunt)
        {
            this.itemIcon.sprite = sprite;
            this.itemQuantityText.text = qunt.ToString();
            if(_stackableItem != null) this.itemIcon.color = _stackableItem.BaseItemData.ColorMultiplier;
      

        }
        public void UpdateUI()
        {
            if (_stackableItem == null)
            {
               ActivateUI(false);
               return;
            }

            itemIcon.color = _stackableItem.BaseItemData.ColorMultiplier;
            ActivateUI(true);
            this.itemQuantityText.text = _stackableItem.Quantity.ToString();



        }

        private void ActivateUI(bool enable)
        {
            itemIcon.gameObject.SetActive(enable);
            itemQuantityText.gameObject.SetActive(enable);
        }

      
        
        public SlotMessage ForceAddNewStack(StackableItem stackableItem)
        {
            _stackableItem = stackableItem;
            SetUi(_stackableItem.BaseItemData.Icon, stackableItem.Quantity);

            _stackableItem.UpdateUI = () =>
            {
                CheckIfStackStillExists();
                UpdateUI();
                FindObjectOfType<PlayerEntity>().InformOfItemBreak(stackableItem);
            };
            
            // _itemStack.onStackModified = () =>
            // {
            //     CheckIfStackStillExists();
            //     UpdateUI();
            //     FindObjectOfType<PlayerEntity>().InformOfItemBreak(itemStack);
            //
            // };
            
            UpdateUI();
            return SlotMessage.SetSuccessful;
        }
        
        
        public void OnPointerDown(PointerEventData eventData)
        {
            timer = 0;
            OnSlotPress?.Invoke();
            timerCorotine = StartCoroutine(PressingTimer());
        }
        
        private void SelectMarker(bool active)
        {
            if(marker != null) marker.SetActive(active);
        }

        public bool Equals(InventorySlot other)
        {
            return other != null && _guid.Equals(other._guid);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((InventorySlot) obj);
        }

        private float timer = 0;
        private Coroutine timerCorotine;
        IEnumerator PressingTimer()
        {
            yield return new WaitForSeconds(0.2f);
            _slider.gameObject.SetActive(true);
            while (timer < 1)
            {
                _slider.value = timer;
                timer += Time.deltaTime;
                yield return null;

            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (timerCorotine != null)
            {
                _slider.gameObject.SetActive(false);
                OnSlotHold?.Invoke(timer);
                StopCoroutine(timerCorotine);
            }
        }

    }
}
