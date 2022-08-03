using Script.Enums;

namespace Script.Map
{
    [System.Serializable]
    public class GridProperty
    {
        public GridCoordinate GridCoordinate;
        public GridBoolProperty gridBoolProperty;
        public bool gridBoolValue;

        public GridProperty(GridCoordinate gridCoordinate, GridBoolProperty gridBoolProperty, bool gridBoolValue)
        {
            GridCoordinate = gridCoordinate;
            this.gridBoolProperty = gridBoolProperty;
            this.gridBoolValue = gridBoolValue;
        }
    }
}