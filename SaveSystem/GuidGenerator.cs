using System;
using UnityEngine;


namespace Script.SaveSystem
{
    [ExecuteAlways]
    public class GuidGenerator: MonoBehaviour
    {
        [SerializeField] private string _gUID = "";
        public string GUID
        {
            get => _gUID;
            set=>_gUID = value;
        }

        private void Awake()
        {
            if (!Application.IsPlaying(gameObject))
            {
                if (_gUID == "")
                {
                    _gUID = System.Guid.NewGuid().ToString();
                }
            }
        }
    }
}