using UnityEngine;

public enum InputType
{
    Trigger, Hold
}
[CreateAssetMenu(fileName = "WeaponRule", menuName = "Data/Weapon/WeaponRule")]
public class WeaponRule : ScriptableObject
{
    [SerializeReference] public WeaponSkillInvokerData attackInvoker = new WeaponSkillTriggerInvokerData();

    [SerializeReference] public WeaponSkillInvokerData specialInvoker = new WeaponSkillTriggerInvokerData();
}