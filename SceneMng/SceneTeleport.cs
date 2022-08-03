using System;
using System.Collections;
using System.Collections.Generic;
using Script.Enums;
using UnityEngine;

namespace Script.SceneMng
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class SceneTeleport : MonoBehaviour
    {
        [SerializeField] private SceneName _fromScene;
        [SerializeField] private SceneName _goToScene;

        private void OnTriggerEnter2D(Collider2D col)
        {
            Player.Player player = col.GetComponent<Player.Player>();
            if (player != null)
            {
                SceneControllerManager.Instance.FadeAndLoadScene(_fromScene.ToString(),_goToScene.ToString());
            }
        }

        public String GetGoToSceneName()
        {
            return this._goToScene.ToString();
        }
    }
}
