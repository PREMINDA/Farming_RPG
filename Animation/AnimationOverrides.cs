using System;
using System.Collections.Generic;
using UnityEngine;

namespace Script.Animation
{
    public class AnimationOverrides : MonoBehaviour
    {
        [SerializeField] private GameObject character = null;
        [SerializeField] private SO_AnimationType[] _soAnimationTypeArray;

        private Dictionary<AnimationClip, SO_AnimationType> _animationTypeDictionaryByAnimation;
        private Dictionary<string, SO_AnimationType> _animationTypeDictionaryByCompositeAttributeKey;

        private void Start()
        {
            _animationTypeDictionaryByAnimation = new Dictionary<AnimationClip, SO_AnimationType>();
            _animationTypeDictionaryByCompositeAttributeKey = new Dictionary<string, SO_AnimationType>();

            foreach (SO_AnimationType item in _soAnimationTypeArray)
            {
                string key = item.CharacterPartAnimator.ToString() + item.PartVariantColor.ToString() +
                             item.PartVariantType.ToString() + item.AnimationName.ToString();
                _animationTypeDictionaryByAnimation.Add(item.AnimationClip,item);
                _animationTypeDictionaryByCompositeAttributeKey.Add(key,item);
            }
        }
        
         public void ApplyCharacterCustomisationParameters(List<CharacterAttribute> characterAttributesList)
        {
            //Stopwatch s1 = Stopwatch.StartNew();

            // Loop through all character attributes and set the animation override controller for each
            foreach (CharacterAttribute characterAttribute in characterAttributesList)
            {
                Animator currentAnimator = null;
                List<KeyValuePair<AnimationClip, AnimationClip>> animsKeyValuePairList = new List<KeyValuePair<AnimationClip, AnimationClip>>();

                string animatorSOAssetName = characterAttribute.CharacterPartAnimator.ToString();

                // Find animators in scene that match scriptable object animator type
                Animator[] animatorsArray = character.GetComponentsInChildren<Animator>();

                foreach (Animator animator in animatorsArray)
                {
                    if (animator.name == animatorSOAssetName)
                    {
                        currentAnimator = animator;
                        break;
                    }
                }

                // Get base current animations for animator
                AnimatorOverrideController aoc = new AnimatorOverrideController(currentAnimator.runtimeAnimatorController);
                List<AnimationClip> animationsList = new List<AnimationClip>(aoc.animationClips);

                foreach (AnimationClip animationClip in animationsList)
                {
                    // find animation in dictionary
                    SO_AnimationType so_AnimationType;
                    bool foundAnimation = _animationTypeDictionaryByAnimation.TryGetValue(animationClip, out so_AnimationType);

                    if (foundAnimation)
                    {
                        string key = characterAttribute.CharacterPartAnimator.ToString() 
                                     + characterAttribute.PartVariantColor.ToString() 
                                     + characterAttribute.PartVariantType.ToString() 
                                     + so_AnimationType.AnimationName.ToString();

                        SO_AnimationType swapSO_AnimationType;
                        bool foundSwapAnimation = _animationTypeDictionaryByCompositeAttributeKey.TryGetValue(key, out swapSO_AnimationType);

                        if (foundSwapAnimation)
                        {
                            AnimationClip swapAnimationClip = swapSO_AnimationType.AnimationClip;

                            animsKeyValuePairList.Add(new KeyValuePair<AnimationClip, AnimationClip>(animationClip, swapAnimationClip));
                        }
                    }
                }

                // Apply animation updates to animation override controller and then update animator with the new controller
                aoc.ApplyOverrides(animsKeyValuePairList);
                currentAnimator.runtimeAnimatorController = aoc;
            }

            // s1.Stop();
            // UnityEngine.Debug.Log("Time to apply character customisation : " + s1.Elapsed + "   elapsed seconds");
        }
    }
}