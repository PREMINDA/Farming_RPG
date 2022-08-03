using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Script.Enums;
using Script.Events;
using Script.Misc;
using Script.SaveSystem;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Script.SceneMng
{
    public class SceneControllerManager : SingletoneMb<SceneControllerManager>
    {
        private bool _isFading;
        [SerializeField] private float fadeDuration = 1f;
        [SerializeField] private CanvasGroup faderCanvasGroup = null;
        [SerializeField] private Image faderImage = null;
        [SerializeField] private CinemachineVirtualCamera mainCamera;
        public SceneName startingSceneName;

        public void FadeAndLoadScene(string fromScene,string goToScene)
        {
            if (!_isFading)
            {
                StartCoroutine(FadeAndSwitchScenes(fromScene,goToScene));
            }
        }

        private IEnumerator FadeAndSwitchScenes(string fromScene, string goToScene)
        {
            EventHandler.CallBeforeSceneUnloadFadeOutEvent();

            yield return StartCoroutine(Fade(1f));
            
            SaveLoadManager.Instance.StoreCurrentSceneData();

            EventHandler.CallBeforeSceneUnloadEvent();

            yield return SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);

            yield return StartCoroutine(LoadSceneAndSetActive(goToScene));
            
            EventHandler.CallAfterSceneLoadEvent();

            Transform moveTransform = GetTeleportArray(fromScene).transform;
            Player.Player.Instance.transform.position = moveTransform.position;
            
            SaveLoadManager.Instance.RestoreCurrentSceneData();
            
            yield return StartCoroutine(Fade(0f));
            
            
            EventHandler.CallAfterSceneLoadFadeInEvent();

        }
        
        private IEnumerator Start()
        {
            faderImage.color = new Color(0f, 0f, 0f, 1f);
            faderCanvasGroup.alpha = 1f;

            // Start the first scene loading and wait for it to finish.
            yield return StartCoroutine(LoadSceneAndSetActive(startingSceneName.ToString()));
           
            EventHandler.CallAfterSceneLoadEvent();
            
            SaveLoadManager.Instance.RestoreCurrentSceneData();
            
            StartCoroutine(Fade(0f));
        }

        private GameObject GetTeleportArray(string fromScene)
        {
            GameObject toGameObject = new GameObject();
            GameObject[] objects = GameObject.FindGameObjectsWithTag(Tags.Teleport);
            if (objects.Length > 0)
            {
                foreach (var t in objects)
                {
                    string scenName = t.GetComponent<SceneTeleport>().GetGoToSceneName();
                    
                    if (scenName == fromScene)
                    {
                        toGameObject = t;
                    }
                }
                return toGameObject;
            }
            return null;
        }

        private IEnumerator Fade(float finalAlpha)
        {
            _isFading = true;
            faderCanvasGroup.blocksRaycasts = true;
            // Calculate how fast the CanvasGroup should fade based on it's current alpha, it's final alpha and how long it has to change between the two.
            float fadeSpeed = Mathf.Abs(faderCanvasGroup.alpha - finalAlpha) / fadeDuration;
            while (!Mathf.Approximately(faderCanvasGroup.alpha, finalAlpha))
            {
                // ... move the alpha towards it's target alpha.
                faderCanvasGroup.alpha = Mathf.MoveTowards(faderCanvasGroup.alpha, finalAlpha,
                    fadeSpeed * Time.deltaTime);

                yield return null;
            }
            
            _isFading = false;
            faderCanvasGroup.blocksRaycasts = false;
        }

        private IEnumerator LoadSceneAndSetActive(string sceneName)
        {
            yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

            // Find the scene that was most recently loaded (the one at the last index of the loaded scenes).
            Scene newlyLoadedScene = SceneManager.GetSceneAt(SceneManager.sceneCount - 1);
            SceneManager.SetActiveScene(newlyLoadedScene);
        }
    }
}