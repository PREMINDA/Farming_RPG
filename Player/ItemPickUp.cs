using System;
using System.Collections;
using System.Collections.Generic;
using Script.Enums;
using Script.Inventory;
using UnityEngine;
using Script.Item;

namespace Script.Player
{
    public class ItemPickUp : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D col)
        {
            Item.Item item = col.GetComponent<Item.Item>();
            if (item != null)
            {
                ItemDetails itemDetails = InventoryManager.Instance.GetItemDetails(item.itemCode);

                if (itemDetails.canBePickedUp)
                {
                    InventoryManager.Instance.AddItem(InventoryLocation.Player,item,col.gameObject);
                }
            }
        }
    }

}