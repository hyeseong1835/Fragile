using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace WeaponSystem.Material.Usage
{
    [MaterialUsageInfo("��ü", "��ü�Դϴ�.", false)]
    public class BodyUsage : WeaponMaterialUsage
    {
        public string glgl;

        public override void OnGUI()
        {
            glgl = EditorGUILayout.TextField("����", glgl);
        }
    }
}
