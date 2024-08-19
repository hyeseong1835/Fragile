using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(WeaponTriggerSkillData))]
public class WeaponTriggerSkillDataDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUILayout.LabelField("트리거 스킬 데이터");
        EditorGUILayout.PropertyField(
            property.FindPropertyRelative("executeBehaviorData"),
            true
        );
    }
}