using System;
using UnityEngine;

public enum InputType
{
    Trigger, Hold
}
[CreateAssetMenu(fileName = "WeaponRule", menuName = "Data/Weapon/WeaponRule")]
public class WeaponRule : ScriptableObject
{
    public int intValueContainerLength = 0;
    public int floatValueContainerLength = 0;
    public int controllerValueContainerLength = 0;

    [SerializeReference] public WeaponSkillInvoker attackInvoker = WeaponSkillInvoker.CreateDefault();

    [SerializeReference] public WeaponSkillInvoker specialInvoker = WeaponSkillInvoker.CreateDefault();

#if UNITY_EDITOR
    [NonSerialized] public int selectedAttackInvokerIndex = 0;
    [NonSerialized] public int selectedSpecialInvokerIndex = 0;
#endif
}