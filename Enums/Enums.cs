
namespace Script.Enums
{
    public enum InventoryLocation
    {
        Player,
        Chest,
        Count
    }

    public enum ToolEffect 
    {
        None,
        Watering
    }

    public enum Direction
    {
        Up,
        Down,
        Left,
        Right,
        None
    }

    public enum Season
    {
        Spring,
        Summer,
        Autumn,
        Winter,
        None,
        Count
    }

    public enum ItemTypes
    {
        Seed,
        Commodity,
        WateringTool,
        HoeingTool,
        ChoppingTool,
        BreakingTol,
        ReapingTool,
        CollectingTool,
        ReapableScenary,
        Furniture,
        None,
        Count
    }

    public enum GridBoolProperty
    {
        Diggable,
        CanDropItem,
        CanPlaceFurniture,
        IsPath,
        IsNpcObstacle
        
    }

    public enum SceneName
    {
        Scene_Farm1,
        Scene_Field1,
        Scene_Cabin1
    }
    
    public enum AnimationName
    {
        idleDown,
        idleUp,
        idleRight,
        idleLeft,
        walkUp,
        walkDown,
        walkRight,
        walkLeft,
        runUp,
        runDown,
        runRight,
        runLeft,
        useToolUp,
        useToolDown,
        useToolRight,
        useToolLeft,
        swingToolUp,
        swingToolDown,
        swingToolRight,
        swingToolLeft,
        liftToolUp,
        liftToolDown,
        liftToolRight,
        liftToolLeft,
        holdToolUp,
        holdToolDown,
        holdToolRight,
        holdToolLeft,
        pickDown,
        pickUp,
        pickRight,
        pickLeft,
        count
    }

    public enum CharacterPartAnimator
    {
        body,
        arms,
        hair,
        tool,
        hat,
        count
    }
    public enum PartVariantColor
    {
        none,
        count
    }

    public enum PartVariantType
    {
        none,
        carry,
        hoe,
        pickaxe,
        axe,
        scythe,
        wateringCan,
        count
    }
    
}
