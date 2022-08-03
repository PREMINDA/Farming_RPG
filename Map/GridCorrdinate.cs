using UnityEngine;

namespace Script.Map
{
    public class GridCoordinate
    {
        public int X;
        public int Y;

        public GridCoordinate(int x, int y)
        {
            X = x;
            Y = y;
        }
        
        public static explicit operator Vector2(GridCoordinate gridCoordinate)
        {
            return new Vector2((float)gridCoordinate.X, (float)gridCoordinate.Y);
        }

        public static explicit operator Vector2Int(GridCoordinate gridCoordinate)
        {
            return new Vector2Int(gridCoordinate.X, gridCoordinate.Y);
        }

        public static explicit operator Vector3(GridCoordinate gridCoordinate)
        {
            return new Vector3((float)gridCoordinate.X, (float)gridCoordinate.Y, 0f);
        }

        public static explicit operator Vector3Int(GridCoordinate gridCoordinate)
        {
            return new Vector3Int(gridCoordinate.X, gridCoordinate.Y, 0);
        }
        
    }
}