using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;

public class Utility_Loop : Unit
{
    [DoNotSerialize] public ControlInput In;
    [DoNotSerialize] public ValueInput duration;

    [DoNotSerialize] public ControlOutput Out;
    [DoNotSerialize] public ControlOutput Exit;
    [DoNotSerialize] public ControlOutput Loop;

    [DoNotSerialize] public ValueOutput time;
    bool isLoop = true;
    bool isUse = false;

    protected override void Definition()
    {
        In = ControlInput(string.Empty, 
            (flow) => 
            {
                if (isLoop)
                {
                    return Loop;
                }
                else if (isUse)
                {
                    isUse = false;
                    return Exit;
                }
                else return Out;
            }
        );

        duration = ValueInput<float>("Duration");

        Loop = ControlOutput("Loop");
        Exit = ControlOutput("Exit");

        time = ValueOutput<float>("Time");
    }
    
}
