using Unity.VisualScripting;

[UnitTitle("Run Line")]
[UnitCategory("Animation")]
public class AnimationRun : ActNode
{
    public ValueInputPort<AnimationData> dataPort;

    protected override void Definition()
    {
        base.Definition();

        dataPort = ValueInputPort<AnimationData>.Define(this, "Data");
    }
    protected override void Act(Flow flow)
    {
        AnimationData data = dataPort.GetValue(flow);
    }
}