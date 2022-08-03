using System.Collections.Generic;
using Script.Misc;
using UnityEngine.SceneManagement;

namespace Script.SaveSystem
{
    
    public class SaveLoadManager : SingletoneMb<SaveLoadManager>
    {
        public List<ISaveable> SaveableObjrctList;

        protected override void Awake()
        {
            base.Awake();
            SaveableObjrctList = new List<ISaveable>();
        }

        public void StoreCurrentSceneData()
        {
            foreach (ISaveable saveableObject in SaveableObjrctList)
            {
                saveableObject.StoreScene(SceneManager.GetActiveScene().name);
            }
        }
        
        public void RestoreCurrentSceneData()
        {
            foreach (ISaveable saveableObject in SaveableObjrctList)
            {
                saveableObject.RestoreScene(SceneManager.GetActiveScene().name);
            }
        }
    }
}