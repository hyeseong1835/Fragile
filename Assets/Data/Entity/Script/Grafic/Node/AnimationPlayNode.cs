using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[UnitTitle("Animation Play")]
[UnitCategory("Animation")]
public class AnimationPlayNode : GetComponentActNode<Animator>
{
    ValueInputPort<string> namePort;
    ValueInputPort<int> layerPort;
    ValueOutputPort<bool> IsNewAnimationPort;

    AnimatorClipInfo[] clipInfoArray;

    protected override void Definition()
    {
        base.Definition();

        namePort = ValueInputPort<string>.Define(this, "Name");
        layerPort = ValueInputPort<int>.Define(this, "Layer");
        IsNewAnimationPort = ValueOutputPort<bool>.Define(this, "IsNewAnimation");
    }
    protected override void Act(Flow flow)
    {
        base.Act(flow);

        string name = namePort.GetValue(flow);
        int layer = layerPort.GetValue(flow);
        
        clipInfoArray = component.GetCurrentAnimatorClipInfo(layer);

        if (clipInfoArray.IsUnityNull()) { AnimationPlay(flow, name, layer); return; }

        foreach(AnimatorClipInfo info in clipInfoArray)
        {
            if (info.clip.name == name) 
            {
                AnimationPlay(flow, name, layer, info.); 
                return; 
            }
        }

        IsNewAnimationPort.SetValue(flow, false);
    }
    void AnimationPlay(Flow flow, string name, int layer, float normalizeTime = 0)
    {
        Debug.Log(normalizeTime);
        component.Play(name, layer, normalizeTime);
        IsNewAnimationPort.SetValue(flow, true);
    }
}
