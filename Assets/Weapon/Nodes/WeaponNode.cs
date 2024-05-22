using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class WeaponData : ScriptableObject
{
    //public string weaponName;
    public Sprite UISprite;
    public float damage;
    
    /*
        [DoNotSerialize] public ControlOutput Out;

        public ValueInput frontDelay;
        public ValueInput delay;
        public ValueInput backDelay;

        public ValueInput minDistance;
        public ValueInput maxDistance;

        protected override void Definition()
        {
            Out = ControlOutput(nameof(Out));

            frontDelay = ValueInput<float>("");
            delay = ValueInput<float>("delay");
            backDelay = ValueInput<float>("");

            minDistance = ValueInput<float>("MinDistance");
            maxDistance = ValueInput<float>("MaxDistance");
        }
    */
}
public class WeaponNode : Unit
{

    //Input
    [DoNotSerialize]
    public ControlInput input;

    //Output
    [DoNotSerialize] public ControlOutput Init;
    [DoNotSerialize] public ControlOutput OnUse;
    [DoNotSerialize] public ControlOutput DeUse;
    
    [DoNotSerialize] public ControlOutput OnBreak;
    [DoNotSerialize] public ControlOutput OnRemoved;
    [DoNotSerialize] public ControlOutput OnDestroyed;

    [DoNotSerialize] public ControlOutput Attack;
    [DoNotSerialize] public ControlOutput Special;

    protected override void Definition()
    {
        Init = ControlOutput(nameof(Init));
        OnUse = ControlOutput(nameof(OnUse));
        DeUse = ControlOutput(nameof(DeUse));
        
        OnBreak = ControlOutput(nameof(OnBreak));
        OnRemoved = ControlOutput(nameof(OnRemoved));
        OnDestroyed = ControlOutput(nameof(OnDestroyed));

        Attack = ControlOutput(nameof(Attack));
        Special = ControlOutput(nameof(Special));
    }
}


