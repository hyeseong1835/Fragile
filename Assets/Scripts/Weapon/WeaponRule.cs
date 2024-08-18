using UnityEngine;

public enum InputType
{
    Trigger, Hold
}
[CreateAssetMenu(fileName = "WeaponRule", menuName = "Data/Weapon/WeaponRule")]
public class WeaponRule : ScriptableObject
{
    [Header("좌클릭 공격")]
    [SerializeReference] public WeaponSkillInvokerData attackInvoker = new WeaponSkillTriggerInvokerData();

    [Header("우클릭 공격")]
    [SerializeReference] public WeaponSkillInvokerData specialInvoker = new WeaponSkillTriggerInvokerData();
}