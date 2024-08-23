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

        CustomGUILayout.TitleHeaderLabel("�����̳�");
        weaponRule.controllerValueContainerLength = EditorGUILayout.IntField("Controller", weaponRule.controllerValueContainerLength);
        weaponRule.intValueContainerLength = EditorGUILayout.IntField("Int", weaponRule.intValueContainerLength);
        weaponRule.floatValueContainerLength = EditorGUILayout.IntField("Float", weaponRule.floatValueContainerLength);
        
        EditorGUILayout.Space(10);

        weaponRule.attackInvoker.WeaponComponentOnGUI(ref weaponRule.attackInvoker, "�⺻ ����");
        weaponRule.specialInvoker.WeaponComponentOnGUI(ref weaponRule.specialInvoker, "Ư�� ����");

        WeaponComponent.floatingManager.Draw();
    }
}