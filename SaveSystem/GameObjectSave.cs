using System.Collections.Generic;

namespace Script.SaveSystem
{
    public class GameObjectSave
    {
        public Dictionary<string, SceneSave> SceneData;

        public GameObjectSave()
        {
            SceneData = new Dictionary<string, SceneSave>();
        }

        public GameObjectSave(Dictionary<string, SceneSave> sceneData)
        {
            SceneData = sceneData;
        }
    }
}