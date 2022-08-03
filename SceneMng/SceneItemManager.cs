using System;
using System.Collections.Generic;
using Script.Misc;
using Script.SaveSystem;
using Unity.VisualScripting;
using UnityEngine;
using EventHandler = Script.Events.EventHandler;

namespace Script.SceneMng
{
    [RequireComponent(typeof(GuidGenerator))]
    public class SceneItemManager: SingletoneMb<SceneItemManager>, ISaveable
    {

        private Transform _parentItem;
        [SerializeField] private GameObject itemPrefab = null;
        
        public string SaveUniqueID { get; set; }
        public GameObjectSave GameObjectSave { get; set; }

        

        protected override void Awake()
        {
            base.Awake();
            SaveUniqueID = GetComponent<GuidGenerator>().GUID;
            GameObjectSave = new GameObjectSave();
        }

        private void OnDisable()
        {
            SaveDeregister();
            EventHandler.AfterSceneLoadEvent -= AfterSceneLoad;
        }

        private void OnEnable()
        {
            SaveRegister();
            EventHandler.AfterSceneLoadEvent += AfterSceneLoad;
        }

        private void AfterSceneLoad()
        {
            _parentItem = GameObject.FindGameObjectWithTag(Tags.ItemParentTransform).transform;
        }

        public void SaveRegister()
        {
            SaveLoadManager.Instance.SaveableObjrctList.Add(this);
        }

        public void SaveDeregister()
        {
            SaveLoadManager.Instance.SaveableObjrctList.Remove(this);
        }

        public void StoreScene(string sceneName)
        {
            GameObjectSave.SceneData.Remove(sceneName);

            List<SceneItem> sceneItemsList = new List<SceneItem>();
            Item.Item[] itemsInScene = FindObjectsOfType<Item.Item>();

            foreach (Item.Item item in itemsInScene)
            {
                SceneItem sceneItem = new SceneItem();
                sceneItem.ItemCode = item.itemCode;
                var position = item.transform.position;
                sceneItem.Pos = new Vector3Serializable(position.x, position.y, position.z);
                sceneItem.ItemName = item.name;
                
                sceneItemsList.Add(sceneItem);
            }

            SceneSave sceneSave = new SceneSave
            {
                ListSceneItemDictionary = new Dictionary<string, List<SceneItem>> {{"sceneItemList", sceneItemsList}}
            };

            GameObjectSave.SceneData.Add(sceneName,sceneSave);

        }

        public void RestoreScene(string sceneName)
        {
            if (GameObjectSave.SceneData.TryGetValue(sceneName, out SceneSave sceneSave))
            {
                if (sceneSave.ListSceneItemDictionary != null &&
                    sceneSave.ListSceneItemDictionary.TryGetValue("sceneItemList", out List<SceneItem> sceneItems))
                {
                    DestroySceneItems();
                    InstantiateSceneItem(sceneItems);
                }
            }
        }

        private void DestroySceneItems()
        {
            Item.Item[] itemsInScene = FindObjectsOfType<Item.Item>();
            Debug.Log(itemsInScene.Length);

            for (int i = itemsInScene.Length-1; i > -1; i--)
            {
                Destroy(itemsInScene[i].gameObject);
            }
            
        }

        private void InstantiateSceneItem(List<SceneItem> sceneItems)
        {
            GameObject itemGameObject;

            foreach (SceneItem sceneItem in sceneItems)
            {
                var instLocation = new Vector3(sceneItem.Pos.x, sceneItem.Pos.y, sceneItem.Pos.z);
                itemGameObject = Instantiate(itemPrefab, instLocation, Quaternion.identity, _parentItem);

                Item.Item item = itemGameObject.GetComponent<Item.Item>();
                item.itemCode = sceneItem.ItemCode;
                item.name = sceneItem.ItemName;
            }
        }
    }
}