using UnityEditor;
using WeaponSystem;
using WeaponSystem.Component;

[CustomEditor(typeof(WeaponRule), true)]
public class WeaponRuleInspector : Editor
{
    WeaponRule weaponRule;

    void OnEnable()
    {
        weaponRule = (WeaponRule)target;

    }
    public override void OnInspectorGUI()
    {
        CustomGUILayout.TitleHeaderLabel("�����̳�");
        weaponRule.controllerValueContainerLength = EditorGUILayout.IntField("Controller", weaponRule.controllerValueContainerLength);
        weaponRule.intValueContainerLength = EditorGUILayout.IntField("Int", weaponRule.intValueContainerLength);
        weaponRule.floatValueContainerLength = EditorGUILayout.IntField("Float", weaponRule.floatValueContainerLength);
        
        EditorGUILayout.Space(10);

        weaponRule.OnGUI();
    }
}