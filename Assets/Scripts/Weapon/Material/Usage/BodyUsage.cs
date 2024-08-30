using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace WeaponSystem.Material.Usage
{
    [MaterialUsageInfo("¸öÃ¼", "¸öÃ¼ÀÔ´Ï´Ù.", false)]
    public class BodyUsage : WeaponMaterialUsage
    {
        public string glgl;

        public override void OnGUI()
        {
            glgl = EditorGUILayout.TextField("È÷È÷", glgl);
        }
    }
}
