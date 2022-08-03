using System;
using System.Collections;
using System.Collections.Generic;
using Script.Enums;
using Script.Inventory;
using Script.Item;
using UnityEngine;
using EventHandler = Script.Events.EventHandler;

namespace Script.UI.UIInventory
{
    public class UIInventoryBar : MonoBehaviour
    {
        public GameObject inventoryBarDraggedItem;
        public GameObject inventoryTextBoxPrefab;
        
        [SerializeField] private Sprite _blankSprite = null;
        [SerializeField] private UIInventorySlot[] _inventorySlots = null;

        [HideInInspector] public GameObject inventoryTextBoxGameObject;
        
        // Start is called before the first frame update
        void Start()
        {
        
        }

        private void OnEnable()
        {
            EventHandler.InventoryUpdateEvent += InventoryUpdate;
        }
        
        // Update is called once per frame
        void Update()
        {
        
        }
        
        private void InventoryUpdate(InventoryLocation inventoryLocation, List<InventoryItem> inventoryItems)
        {
            if (inventoryLocation == InventoryLocation.Player)
            {
                ClearInventorySlot();
                if (_inventorySlots.Length > 0 && inventoryItems.Count > 0)
                {
                    for (int i = 0; i < _inventorySlots.Length; i++)
                    {
                        if (i<inventoryItems.Count)
                        {
                            int itemCode = inventoryItems[i].itemCode;
                            ItemDetails itemDetails = InventoryManager.Instance.GetItemDetails(itemCode);

                            if (itemDetails != null)
                            {
                                _inventorySlots[i].inventorySlotImage.sprite = itemDetails.itemSprite;
                                _inventorySlots[i].slotText.text = inventoryItems[i].itemQuantity.ToString();
                                _inventorySlots[i].itemDetails = itemDetails;
                                _inventorySlots[i].itemQuantity = inventoryItems[i].itemQuantity;
                                SetHighlightedInventorySlot(i);
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
        }

        private void ClearInventorySlot()
        {
            if (_inventorySlots.Length > 0)
            {
                for (int i = 0; i < _inventorySlots.Length; i++)
                {
                    _inventorySlots[i].inventorySlotImage.sprite = _blankSprite;
                    _inventorySlots[i].slotText.text = "";
                    _inventorySlots[i].itemDetails = null;
                    _inventorySlots[i].itemQuantity = 0;
                }
            }
        }

        public void ClearHighlightInventorySlot()
        {
            if (_inventorySlots.Length > 0)
            {
                for (int i = 0; i < _inventorySlots.Length; i++)
                {
                    if (_inventorySlots[i].isSelected)
                    {
                        _inventorySlots[i].isSelected = false;
                        _inventorySlots[i].inventorySlotHighLight.color = new Color(0f, 0f, 0f, 0f);
                        
                        InventoryManager.Instance.ClearInventoryLocation(InventoryLocation.Player);
                    }
                }
            }
        }

        public void SetHighlightedInventorySlot()
        {
            if (_inventorySlots.Length > 0)
            {
                for (int i = 0; i < _inventorySlots.Length; i++)
                {
                    SetHighlightedInventorySlot(i);
                }
            }
        }

        public void SetHighlightedInventorySlot(int itemPosition)
        {
            if (_inventorySlots.Length > 0 && _inventorySlots[itemPosition].itemDetails != null)
            {
                if (_inventorySlots[itemPosition].isSelected)
                {
                    _inventorySlots[itemPosition].inventorySlotHighLight.color = new Color(1f, 1f, 1f, 1f);
                    
                    InventoryManager.Instance.SelectedInventoryItem(InventoryLocation.Player,_inventorySlots[itemPosition].itemDetails.itemCode);
                }
            }
        }

    }

}