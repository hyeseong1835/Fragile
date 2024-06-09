using Unity.VisualScripting;

[UnitTitle("Run Line")]
[UnitCategory("Animation")]
public class AnimationRun : Unit
{
    protected Grafic grafic;

    public ControlInputPort inputPort;
    public ControlOutputPort outputPort;
    public ValueInputPort<AnimationData> dataPort;
    public ValueInputPort<ReadLineType> readType;
    public ValueInputPort<int> lineOffsetPort;

    protected override void Definition()
    {
        inputPort.Define(this, Act);
        outputPort.Define(this);
        dataPort.Define(this, "Data");
        readType.Define(this, "Read");
        lineOffsetPort.Define(this, "Offset");
    }
    ControlOutput Act(Flow flow)
    {
        if (grafic == null) grafic = flow.stack.gameObject.GetComponent<Grafic>();

        grafic.curAnimation = dataPort.GetValue(flow);
        
        grafic.lineOffset = lineOffsetPort.GetValue(flow);

        return outputPort.port;
    }
}