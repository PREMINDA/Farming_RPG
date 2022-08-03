using System;
using System.Collections;
using System.Collections.Generic;
using Script.Misc;
using UnityEngine;

namespace Script.Item
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class ItemFader : MonoBehaviour
    {
        private SpriteRenderer _spriteRenderer; 
        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void FadeOut()
        {
            StartCoroutine(FadeOutRoutine()); 
        }
        
        public void FadeIn()
        {
            StartCoroutine(FadeInRoutine());
        }

        private IEnumerator FadeInRoutine()
        {
            float currentAlpha = _spriteRenderer.color.a;
            float distance = 1f - currentAlpha;
            while (1f - currentAlpha > 0.01f)
            {
                currentAlpha += distance / Settings.FadeInSecond * Time.deltaTime;
                _spriteRenderer.color = new Color(1f, 1f, 1f, currentAlpha);
                yield return null;
            }
            _spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
        }

        private IEnumerator FadeOutRoutine()
        {
            var currentAlpha = _spriteRenderer.color.a;
            float distance = currentAlpha - Settings.TargetAlpha;
            while (currentAlpha - Settings.TargetAlpha > 0.01f)
            {
                currentAlpha -= distance / Settings.FadeOutSecond * Time.deltaTime;
                _spriteRenderer.color = new Color(1f, 1f, 1f, currentAlpha);
                yield return null;
            }
            _spriteRenderer.color = new Color(1f,1f,1f,Settings.TargetAlpha);
        }
    }
}
