using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[UnitTitle("Sample Animation")]
[UnitCategory("Animation")]
public class SampleAnimationNode : ActNode
{
    ValueInputPort<AnimationClip> clipPort;
    ValueInputPort<float> timePort;

    protected override void Definition()
    {
        base.Definition();
        clipPort = ValueInputPort<AnimationClip>.Define(this, "clip");
        timePort = ValueInputPort<float>.Define(this, "time");
    }
    protected override void Act(Flow flow)
    {
        AnimationClip animationClip = clipPort.GetValue(flow);
        float time = timePort.GetValue(flow);

        animationClip.SampleAnimation(flow.stack.gameObject, time);
    }
}
