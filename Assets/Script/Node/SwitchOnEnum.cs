using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public abstract class CloseSwitchOnEnum<TEnum> : CloseNode where TEnum : Enum
{
    public static TEnum[] types = (TEnum[])Enum.GetValues(typeof(TEnum));
    public ControlOutput[] typeOut = new ControlOutput[types.Length];

    public abstract TEnum Value { get; }

    protected override void Definition()
    {
        base.Definition();

        for (int i = 0; i < typeOut.Length; i++)
        {
            typeOut[i] = ControlOutput(Enum.GetName(typeof(TEnum), types[i]));
        }
    }
    protected override ControlOutput Control(Flow flow)
    {
        Act(flow);

        for (int i = 0; i < typeOut.Length; i++)
        {
            if (Value.Equals(types[i])) return typeOut[i];
        }

        Debug.LogError("No type found");
        return default;
    }
}
public abstract class SwitchOnEnum<TEnum> : Node<TEnum> where TEnum : Enum
{
    public static TEnum[] types = (TEnum[])Enum.GetValues(typeof(TEnum));
    public ControlOutput[] typeOut = new ControlOutput[types.Length];

    protected override void Definition()
    {
        base.Definition();

        for (int i = 0; i < typeOut.Length; i++)
        {
            typeOut[i] = ControlOutput(Enum.GetName(typeof(TEnum), types[i]));
        }
    }
    protected override ControlOutput Control(Flow flow)
    {
        Act(flow);

        TEnum type = flow.GetValue<TEnum>(value);
        for (int i = 0; i < typeOut.Length; i++)
        {
            if (type.Equals(types[i])) return typeOut[i];
        }

        Debug.LogError("No type found");
        return default;
    }
}