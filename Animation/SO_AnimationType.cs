using Script.Enums;
using UnityEngine;

namespace Script.Animation
{
    [CreateAssetMenu(fileName = "so_ItemList",menuName = "Scriptable Objects/Animation/Animation Type")]
    public class SO_AnimationType : ScriptableObject
    {
        public AnimationClip AnimationClip;
        public AnimationName AnimationName;
        public CharacterPartAnimator CharacterPartAnimator;
        public PartVariantColor PartVariantColor;
        public PartVariantType PartVariantType;
    }
}