using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Cinemachine;
using Script.Events;
using Script.Misc;

namespace Script.SceneMng
{
    public class SwitchConfinerBoundingShape : MonoBehaviour
    {

        private void OnEnable()
        {
            EventHandler.AfterSceneLoadEvent += SwitchBoundingShape;
        }

        private void OnDisable()
        {
            EventHandler.AfterSceneLoadEvent -= SwitchBoundingShape;

        }

        private void SwitchBoundingShape()
        {
            PolygonCollider2D polygonCollider2D =
                GameObject.FindGameObjectWithTag(Tags.BoundsConfiner).GetComponent<PolygonCollider2D>();

            CinemachineConfiner cinemachineConfiner = GetComponent<CinemachineConfiner>();

            cinemachineConfiner.m_BoundingShape2D = polygonCollider2D;
            
            cinemachineConfiner.InvalidatePathCache();
        }

    }

}