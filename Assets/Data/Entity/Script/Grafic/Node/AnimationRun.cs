using Unity.VisualScripting;

[UnitTitle("Run Line")]
[UnitCategory("Animation")]
public class AnimationRun : Node
{
    protected Grafic grafic;

    public ValueInput data;

    protected override void Definition()
    {
        base.Definition();

        data = ValueInput<AnimationData>("Data", null);
    }
    protected override void Act(Flow flow)
    {
        if (grafic == null) grafic = flow.stack.gameObject.GetComponent<Grafic>();

        grafic.curAnimation = flow.GetValue<AnimationData>(data);
    }
}

[UnitTitle("Custom Run Line")]
[UnitCategory("Animation")]
public class AnimationCustomRun : AnimationRun
{
    public ValueInput lineOffset;

    protected override void Definition()
    {
        base.Definition();

        lineOffset = ValueInput<int>("Offset", 0);
    }
    protected override void Act(Flow flow)
    {
        base.Act(flow);

        grafic.lineOffset = flow.GetValue<int>(lineOffset);
    }
}