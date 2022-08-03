using System;
using System.Collections.Generic;
using System.Diagnostics;
using Script.Enums;
using Script.Item;
using Script.Misc;
using UnityEngine;
using Debug = UnityEngine.Debug;
using EventHandler = Script.Events.EventHandler;
namespace Script.Inventory
{
    public class InventoryManager : SingletoneMb<InventoryManager>
    {
        private Dictionary<int, ItemDetails> _itemDetailDictionary;
        private List<InventoryItem>[] _inventoryList;
        private int[] _selectedInventoryItem;
        [HideInInspector] public int[] inventoryListCapacityIntArray;
        [SerializeField] private SO_ItemList itemList = null;
        
        protected override void Awake()
        {
            base.Awake();
            CreateItemDetailDictionary();
            CreateInventoryList();
            _selectedInventoryItem = new int[(int) InventoryLocation.Count];

            for (int i = 0; i < _selectedInventoryItem.Length; i++)
            {
                _selectedInventoryItem[i] = -1;
            }
        }

        private void CreateInventoryList()
        {
            _inventoryList = new List<InventoryItem>[(int) InventoryLocation.Count];
            for (int i = 0; i < (int)InventoryLocation.Count; i++)
            {
                _inventoryList[i] = new List<InventoryItem>();
            }

            inventoryListCapacityIntArray = new int[(int) InventoryLocation.Count];
            inventoryListCapacityIntArray[(int) InventoryLocation.Player] = Settings.PlayerInitialInventoryCapacity;
        }
        
        public void AddItem(InventoryLocation inventoryLocation, Item.Item item, GameObject gameObjectToDelete)
        {
            AddItem(inventoryLocation, item);

            Destroy(gameObjectToDelete);
        }

        public void AddItem(InventoryLocation inventoryLocation,Item.Item item)
        {
            int itemCode = item.itemCode;
            List<InventoryItem> inventoryList = _inventoryList[(int) inventoryLocation];

            int itemPosition = FindItemInInventory(inventoryLocation, itemCode);
            if (itemPosition != -1)
            {
                AddItemAtPosition(inventoryList,itemCode,itemPosition);
            }
            else
            {
                AddItemAtPosition(inventoryList, itemCode);
            }
            EventHandler.CallInventoryUpdateEvent(inventoryLocation, _inventoryList[(int)inventoryLocation]);
        }

        private void AddItemAtPosition(List<InventoryItem> inventoryList, int itemCode)
        {
            InventoryItem tempInventoryItem = new InventoryItem();

            tempInventoryItem.itemQuantity = 1;
            tempInventoryItem.itemCode = itemCode;
            inventoryList.Add(tempInventoryItem);
            // DebugPrintInventoryList(inventoryList);
        }

        private void AddItemAtPosition(List<InventoryItem> inventoryList, int itemCode, int itemPosition)
        {
            InventoryItem tempInventoryItem = new InventoryItem();
            
            int quantity = inventoryList[itemPosition].itemQuantity + 1;
            tempInventoryItem.itemQuantity = quantity;
            tempInventoryItem.itemCode = itemCode;
            inventoryList[itemPosition] = tempInventoryItem;
            // DebugPrintInventoryList(inventoryList);
        }

        public int FindItemInInventory(InventoryLocation inventoryLocation, int itemCode)
        {
            List<InventoryItem> inventoryList = _inventoryList[(int) inventoryLocation];
            for (int i = 0; i < inventoryList.Count ; i++)
            {
                if (inventoryList[i].itemCode == itemCode)
                {
                    return i;
                }
            }
            return -1;
        }

        private void CreateItemDetailDictionary()
        {
            _itemDetailDictionary = new Dictionary<int, ItemDetails>();
            foreach (ItemDetails itemDetails in itemList.ItemDetailsList)
            {
                _itemDetailDictionary.Add(itemDetails.itemCode,itemDetails);
            }
        }

        public ItemDetails GetItemDetails(int code)
        {
            ItemDetails itemDetails;
            if (_itemDetailDictionary.TryGetValue(code, out itemDetails))
            {
                return itemDetails;
            }
            else
            {
                return null;
            }
        }
        
        // private void DebugPrintInventoryList(List<InventoryItem> inventoryList)
        // {
        //     foreach (InventoryItem inventoryItem in inventoryList)
        //     {
        //         Debug.Log("Item Description:" + InventoryManager.Instance.GetItemDetails(inventoryItem.itemCode).itemDescription + "    Item Quantity: " + inventoryItem.itemQuantity);
        //     }
        //     Debug.Log("******************************************************************************");
        // }
        public void RemoveFromList(InventoryLocation inventoryLocation, int itemCode)
        {
            List<InventoryItem> inventoryList = _inventoryList[(int)inventoryLocation];

            // Check if inventory already contains the item
            int itemPosition = FindItemInInventory(inventoryLocation, itemCode);

            if (itemPosition != -1)
            {
                RemoveItemAtPosition(inventoryList, itemCode, itemPosition);
            }

            //  Send event that inventory has been updated
            EventHandler.CallInventoryUpdateEvent(inventoryLocation, _inventoryList[(int)inventoryLocation]);
        }

        public string GetItemDescription(ItemTypes itemTypes)
        {
            string itemDescription;
            switch (itemTypes)
            {
                case ItemTypes.BreakingTol:
                    itemDescription = Settings.BreakingTool;
                    break;
                case ItemTypes.ChoppingTool:
                    itemDescription = Settings.BreakingTool;
                    break;
                case ItemTypes.HoeingTool:
                    itemDescription = Settings.BreakingTool;
                    break;
                case ItemTypes.ReapingTool:
                    itemDescription = Settings.BreakingTool;
                    break;
                case ItemTypes.WateringTool:
                    itemDescription = Settings.BreakingTool;
                    break;
                case ItemTypes.CollectingTool:
                    itemDescription = Settings.BreakingTool;
                    break;
                default:
                    itemDescription = itemTypes.ToString();
                    break;
            }
            return itemDescription;
        }

        private void RemoveItemAtPosition(List<InventoryItem> inventoryList, int itemCode, int itemPosition)
        {
            InventoryItem inventoryItem = new InventoryItem();

            int quantity = inventoryList[itemPosition].itemQuantity - 1;

            if (quantity > 0)
            {
                inventoryItem.itemQuantity = quantity;
                inventoryItem.itemCode = itemCode;
                inventoryList[itemPosition] = inventoryItem;
            }
            else
            {
                inventoryList.RemoveAt(itemPosition);
            }
        }

        public void SwapInventoryItem(InventoryLocation inventoryLocation, int slotNumber, int toSlotNumber)
        {
            if (slotNumber < _inventoryList[(int) inventoryLocation].Count &&
                toSlotNumber < _inventoryList[(int) inventoryLocation].Count && slotNumber != toSlotNumber &&
                slotNumber >= 0 && toSlotNumber >= 0)
            {
                InventoryItem dragItem = _inventoryList[(int) inventoryLocation][slotNumber];
                InventoryItem dropLocationItem = _inventoryList[(int) inventoryLocation][toSlotNumber];

                _inventoryList[(int) inventoryLocation][slotNumber] = dropLocationItem;
                _inventoryList[(int) inventoryLocation][toSlotNumber] = dragItem;
                
                EventHandler.CallInventoryUpdateEvent(inventoryLocation, _inventoryList[(int)inventoryLocation]);

            }
        }

        public void SelectedInventoryItem(InventoryLocation inventoryLocation,int itemCode)
        {
            _selectedInventoryItem[(int) inventoryLocation] = itemCode;
        }

        public void ClearInventoryLocation(InventoryLocation inventoryLocation)
        {
            _selectedInventoryItem[(int) inventoryLocation] = -1;
        }
    }
}