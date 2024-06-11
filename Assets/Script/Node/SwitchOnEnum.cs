using System;
using Unity.VisualScripting;
using UnityEngine;

public abstract class SwitchOnEnum<TEnum> : Unit where TEnum : Enum
{
    protected static TEnum[] types = (TEnum[])Enum.GetValues(typeof(TEnum));

    protected ControlInputPort inputPort;
    protected ControlOutputPort[] typePort;

    protected abstract TEnum GetValue(Flow flow);

    protected override void Definition()
    {
        inputPort = ControlInputPort.Define(this, "In", Act);

        typePort = new ControlOutputPort[types.Length];
        for (int i = 0; i < typePort.Length; i++)
        {
            typePort[i] = ControlOutputPort.Define(this, Enum.GetName(typeof(TEnum), types[i]));
        }
    }
    ControlOutput Act(Flow flow)
    {
        TEnum value = GetValue(flow);
        for (int i = 0; i < typePort.Length; i++)
        {
            if (value.Equals(types[i])) return typePort[i].port;
        }

        Debug.LogError("No type found");
        return default;
    }
}