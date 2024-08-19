using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(WeaponSkillTriggerInvokerData))]
public class WeaponSkillTriggerInvokerDataDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUILayout.LabelField("실행기 (트리거)");
        EditorGUILayout.PropertyField(
            property.FindPropertyRelative("onTrigger"),
            true
        );
    }
}