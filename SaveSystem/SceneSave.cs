using System.Collections.Generic;

namespace Script.SaveSystem
{
    [System.Serializable]
    public class SceneSave
    {
        public Dictionary<string, List<SceneItem>> ListSceneItemDictionary;
    }
}