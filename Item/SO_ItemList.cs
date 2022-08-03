using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Script.Item
{
    [CreateAssetMenu(fileName = "so_ItemList",menuName = "Scriptable Objects/Item/Item_List")]
    public class SO_ItemList : ScriptableObject
    {
        [FormerlySerializedAs("ItemDetails")] [SerializeField]
        public List<ItemDetails> ItemDetailsList;
    }
}
