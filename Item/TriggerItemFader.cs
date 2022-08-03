using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Script.Item
{
    public class TriggerItemFader : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D col)
        {
            ItemFader[] itemFaders = col.gameObject.GetComponentsInChildren<ItemFader>();

            if (itemFaders.Length > 0)
            {
                foreach (ItemFader itemFader in itemFaders)
                {
                    itemFader.FadeOut();
                }
            }
        }
        
        private void OnTriggerExit2D(Collider2D col)
        {
            ItemFader[] itemFaders = col.gameObject.GetComponentsInChildren<ItemFader>();

            if (itemFaders.Length > 0)
            {
                foreach (ItemFader itemFader in itemFaders)
                {
                    itemFader.FadeIn();
                }
            }
        }
    }

}