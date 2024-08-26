using UnityEngine;


namespace WeaponSystem
{
    public abstract class WeaponData : ScriptableObject
    {
        public abstract WeaponRule Rule { get; }

        public Sprite icon;
        public string displayedWeaponName;
        public string description;
    }
}