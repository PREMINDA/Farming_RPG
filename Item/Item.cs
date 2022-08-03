using System;
using System.Collections;
using System.Collections.Generic;
using Script.Enums;
using Script.Inventory;
using Script.Utilities.Property_Drawers;
using UnityEngine;

namespace Script.Item
{
    public class Item : MonoBehaviour
    {
        [ItemCodeDes]
        public int itemCode;

        private SpriteRenderer _spriteRenderer;

        private void Awake()
        {
            _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        }

        private void Start()
        {
            if (itemCode != 0)
            {
                Init(itemCode);
            }
        }

        public void Init(int itemCode)
        {
            
            if (itemCode != 0)
            {
                
                ItemDetails itemDetails = InventoryManager.Instance.GetItemDetails(itemCode);
                _spriteRenderer.sprite = itemDetails.itemSprite;
                if (itemDetails.itemType == ItemTypes.ReapableScenary)
                {
                    gameObject.AddComponent<ItemNudge>();
                }
            }
        }
    }

}