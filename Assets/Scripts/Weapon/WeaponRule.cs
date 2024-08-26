using System;
using UnityEngine;


namespace WeaponSystem
{
    public abstract class WeaponRule : ScriptableObject
    {
        public int intValueContainerLength = 0;
        public int floatValueContainerLength = 0;
        public int controllerValueContainerLength = 0;

#if UNITY_EDITOR
        public abstract void OnGUI();
#endif
    }
}