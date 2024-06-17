using Unity.VisualScripting;
using UnityEngine;

public struct AnimationInfo
{
    public AnimatorStateInfo stateInfo;

    /// <summary>
    /// -1: 반복 없음
    /// </summary>
    public int repeatLayer;

    /// <summary>
    /// The full path hash for this state.
    /// </summary>
    public int fullPathHash => stateInfo.fullPathHash;

    /// <summary>
    ///     the name of the parent layer.
    /// </summary>
    public int shortNameHash => stateInfo.shortNameHash;

    /// <summary>
    /// Normalized time of the State.
    /// </summary>
    public float normalizedTime => stateInfo.normalizedTime;

    /// <summary>
    /// Current duration of the state.
    /// </summary>
    public float length => stateInfo.length;

    /// <summary>
    /// The playback speed of the animation. 1 is the normal playback speed.
    /// </summary>
    public float speed => stateInfo.speed;

    /// <summary>
    /// The speed multiplier for this state.
    /// </summary>
    public float speedMultiplier => stateInfo.speedMultiplier;

    /// <summary>
    /// The Tag of the State.
    /// </summary>
    public int tagHash => stateInfo.tagHash;

    /// <summary>
    /// Is the state looping.
    /// </summary>
    public bool loop => stateInfo.loop;

    /// <summary>
    /// Does name match the name of the active state in the statemachine?
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public bool IsName(string name) => stateInfo.IsName(name);

    /// <summary>
    /// Does tag match the tag of the active state in the statemachine.
    /// </summary>
    /// <param name="tag"></param>
    /// <returns></returns>
    public bool IsTag(string tag) => stateInfo.IsTag(tag);
}
public abstract class AnimationPlayNodeBase : GetComponentActNode<Animator>
{
    protected AnimationInfo animationInfo;

    public ValueInputPort<string> namePort;
    public ValueInputPort<int> animationLayerPort;

    public ValueOutputPort<AnimationInfo> animationInfoPort;

    protected AnimationInfo GetAnimationInfo(int animationLayer, int repeatLayer)
    {
        AnimationInfo info = new AnimationInfo();
        info.stateInfo = component.GetCurrentAnimatorStateInfo(animationLayer);
        info.repeatLayer = repeatLayer;

        return info;
    }
}
[UnitTitle("Animation Play")]
[UnitCategory("Animation")]
public class AnimationPlayNode : AnimationPlayNodeBase
{
    public ValueInputPort<int> repeatLayerPort;

    public ControlOutputPort newAnimationPort;
    public ValueOutputPort<float> normalizedTimePort;
    protected override void Definition()
    {
        base.Definition();

        namePort = ValueInputPort<string>.Define(this, "Name");
        animationLayerPort = ValueInputPort<int>.Define(this, "Animation Layer");
        repeatLayerPort = ValueInputPort<int>.Define(this, "Repeat Layer");

        newAnimationPort = ControlOutputPort.Define(this, "NewAnimation");
        animationInfoPort = ValueOutputPort<AnimationInfo>.Define(this, "Animation Info", (flow) => animationInfo);
        normalizedTimePort = ValueOutputPort<float>.Define(this, "NormalizedTime", (flow) => animationInfo.normalizedTime % 1);
    }
    protected override void Act(Flow flow)
    {
        base.Act(flow);

        string name = namePort.GetValue(flow);
        int animationLayer = animationLayerPort.GetValue(flow);
        int repeatLayer = repeatLayerPort.GetValue(flow);

        animationInfo = GetAnimationInfo(animationLayer, repeatLayer);

        if (animationInfo.IsUnityNull() || animationInfo.IsName("None"))
        {
            AnimationPlay(name, animationLayer, repeatLayer);
            return;
        }

        if (animationInfo.IsName(name) == false)
        {
            if (animationInfo.repeatLayer == animationLayer)
            {
                float normalTime = animationInfo.normalizedTime % 1;

                AnimationPlay(name, animationLayer, repeatLayer, normalTime);
                return;
            }
            else
            {
                AnimationPlay(name, animationLayer, repeatLayer);
                return;
            }
        }

        void AnimationPlay(string name, int animationLayer, int repeatLayer, float normalizeTime = 0)
        {
            component.Play(name, animationLayer, normalizeTime);
            animationInfo = GetAnimationInfo(animationLayer, repeatLayer);

            newAnimationPort.Run(reference);
        }
    }
}

[UnitTitle("Animation Play With Start TIme")]
[UnitCategory("Animation")]
public class AnimationPlayWithStartTime : AnimationPlayNodeBase
{
    public ValueInputPort<int> repeatLayerPort;
    public ValueInputPort<float> startTimePort;

    public ControlOutputPort newAnimationPort;
    public ValueOutputPort<float> normalizedTimePort;
    protected override void Definition()
    {
        base.Definition();

        namePort = ValueInputPort<string>.Define(this, "Name");
        animationLayerPort = ValueInputPort<int>.Define(this, "Animation Layer");
        repeatLayerPort = ValueInputPort<int>.Define(this, "Repeat Layer");

        newAnimationPort = ControlOutputPort.Define(this, "NewAnimation");
        animationInfoPort = ValueOutputPort<AnimationInfo>.Define(this, "Animation Info", (flow) => animationInfo);
        normalizedTimePort = ValueOutputPort<float>.Define(this, "NormalizedTime", (flow) => animationInfo.normalizedTime % 1);

        startTimePort = ValueInputPort<float>.Define(this, "Normalized Time");
    }

    protected override void Act(Flow flow)
    {
        base.Act(flow);

        string name = namePort.GetValue(flow);
        int animationLayer = animationLayerPort.GetValue(flow);
        int repeatLayer = repeatLayerPort.GetValue(flow);
        float startTime = startTimePort.GetValue(flow);

        
        if (animationInfo.IsName(name) == false)
        {
            component.Play(name, animationLayer, startTime);
            animationInfo = GetAnimationInfo(animationLayer, repeatLayer);

            newAnimationPort.Run(reference);
            return;
        }
    }
}

[UnitTitle("Linked Animation Play")]
[UnitCategory("Animation")]
public class LinkedAnimationPlayNode : AnimationPlayNodeBase
{
    public ValueInputPort<AnimationInfo> infoPort;

    protected override void Definition()
    {
        base.Definition();

        infoPort = ValueInputPort<AnimationInfo>.Define(this, "Animation Info");
        namePort = ValueInputPort<string>.Define(this, "Name");
        animationLayerPort = ValueInputPort<int>.Define(this, "Animation Layer");

        animationInfoPort = ValueOutputPort<AnimationInfo>.Define(this, "Animation Info", (flow) => animationInfo);

    }
    protected override void Act(Flow flow)
    {
        base.Act(flow);

        AnimationInfo info = infoPort.GetValue(flow);
        string name = namePort.GetValue(flow);
        int animationLayer = animationLayerPort.GetValue(flow);

        component.Play(name, animationLayer, info.normalizedTime);
        animationInfo = GetAnimationInfo(animationLayer, info.repeatLayer);
    }
}

[UnitTitle("Empty Animation Play")]
[UnitCategory("Animation")]
public class EmptyAnimationPlayNode : GetComponentActNode<Animator>
{
    public ValueInputPort<int> animationLayerPort;

    protected override void Definition()
    {
        base.Definition();

        animationLayerPort = ValueInputPort<int>.Define(this, "Animation Layer");
    }
    protected override void Act(Flow flow)
    {
        base.Act(flow); 

        int animationLayer = animationLayerPort.GetValue(flow);

        if (animationLayer == -1) 
        {
            for (int i = 0; i < component.layerCount; i++)
            {
                component.Play("None", i);
            }
        }
        else
        {
            component.Play("None", animationLayer);
        }
    }
}
