using UnityEngine;

namespace Script.SaveSystem
{
    public interface ISaveable
    {
        string SaveUniqueID { get; set; }
        GameObjectSave GameObjectSave { get; set; }

        void SaveRegister();
        void SaveDeregister();
        void StoreScene(string sceneName);
        void RestoreScene(string sceneName);

    }
}