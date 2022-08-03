﻿namespace Script.Misc
{
    [System.Serializable]
    public class Vector3Serializable
    {
        public float x, y, z;

        public Vector3Serializable()
        {
        }

        public Vector3Serializable(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
    }
}