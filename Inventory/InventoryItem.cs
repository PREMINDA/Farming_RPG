using UnityEngine.Serialization;

namespace Script.Inventory
{
    [System.Serializable]
    public struct InventoryItem
    {
        public int itemCode;
        public int itemQuantity;
    }

}