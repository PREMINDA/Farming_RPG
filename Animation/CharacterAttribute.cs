using Script.Enums;

namespace Script.Animation
{
    public struct CharacterAttribute
    {
        public CharacterPartAnimator CharacterPartAnimator;
        public PartVariantColor PartVariantColor;
        public PartVariantType PartVariantType;

        public CharacterAttribute(CharacterPartAnimator characterPartAnimator, PartVariantColor partVariantColor, PartVariantType partVariantType)
        {
            this.CharacterPartAnimator = characterPartAnimator;
            this.PartVariantColor = partVariantColor;
            this.PartVariantType = partVariantType;
        }
        
        
    }
}