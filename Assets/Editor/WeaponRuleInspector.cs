using UnityEditor;

[CustomEditor(typeof(WeaponRule))]
public class WeaponRuleInspector : Editor
{
    WeaponRule weaponRule;

    void OnEnable()
    {
        weaponRule = (WeaponRule)target;

        if (weaponRule.attackInvoker == null)
        {
            weaponRule.attackInvoker = WeaponSkillInvoker.CreateDefault();
        }
        if (weaponRule.specialInvoker == null)
        {
            weaponRule.specialInvoker = WeaponSkillInvoker.CreateDefault();
        }
    }
    public override void OnInspectorGUI()
    {
        WeaponComponent.floatingManager.EventListen();

        CustomGUILayout.TitleHeaderLabel("컨테이너");
        weaponRule.controllerValueContainerLength = EditorGUILayout.IntField("Controller", weaponRule.controllerValueContainerLength);
        weaponRule.intValueContainerLength = EditorGUILayout.IntField("Int", weaponRule.intValueContainerLength);
        weaponRule.floatValueContainerLength = EditorGUILayout.IntField("Float", weaponRule.floatValueContainerLength);
        
        EditorGUILayout.Space(10);

        weaponRule.attackInvoker.WeaponComponentOnGUI(ref weaponRule.attackInvoker, "기본 공격");
        weaponRule.specialInvoker.WeaponComponentOnGUI(ref weaponRule.specialInvoker, "특수 공격");

        WeaponComponent.floatingManager.Draw();
    }
}