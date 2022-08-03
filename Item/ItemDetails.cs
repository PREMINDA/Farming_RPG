using System;
using System.Collections;
using System.Collections.Generic;
using Script.Enums;
using UnityEngine;

namespace Script.Item
{
    [System.Serializable]
    public class ItemDetails 
    {
        public int itemCode;
        public ItemTypes itemType;
        public string itemDescription;
        public Sprite itemSprite;
        public string itemLongDescription;
        public short itemUseGridRadius;
        public float itemUseRadius;
        public bool isStartingItem;
        public bool canBePickedUp;
        public bool canBeDropped;
        public bool canBeEaten;
        public bool canBeCarried;
    }
}
