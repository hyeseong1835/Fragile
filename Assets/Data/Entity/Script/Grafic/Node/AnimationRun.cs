using Unity.VisualScripting;

[UnitTitle("Run Line")]
[UnitCategory("Animation")]
public class AnimationRun : ActNode
{
    protected Grafic grafic;

    public ValueInputPort<AnimationData> dataPort;

    protected override void Definition()
    {
        base.Definition();

        dataPort = ValueInputPort<AnimationData>.Define(this, "Data");
    }
    protected override void Act(Flow flow)
    {
        if (grafic == null) grafic = flow.stack.gameObject.GetComponent<Grafic>();
        AnimationData data = dataPort.GetValue(flow);

        grafic.curAnimation = dataPort.GetValue(flow);
    }
}