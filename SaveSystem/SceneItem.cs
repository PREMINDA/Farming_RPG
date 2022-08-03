using Script.Misc;

namespace Script.SaveSystem
{
    [System.Serializable]
    public class SceneItem
    {
        public int ItemCode;
        public Vector3Serializable Pos;
        public string ItemName;

        public SceneItem()
        {
            Pos = new Vector3Serializable();
        }
    }
}