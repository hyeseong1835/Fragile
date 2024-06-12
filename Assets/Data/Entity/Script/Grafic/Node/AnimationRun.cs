using Unity.VisualScripting;

[UnitTitle("Run Line")]
[UnitCategory("Animation")]
public class AnimationRun : ActNode
{
    [Serialize, Inspectable ,UnitHeaderInspectable]
    public ReadLineType readType;

    protected Grafic grafic;

    public ValueInputPort<AnimationData> dataPort;
    public ValueInputPort<int> lineOffsetPort;
    ValueOutputPort<int> indexOutputPort;

    protected override void Definition()
    {
        base.Definition();

        dataPort = ValueInputPort<AnimationData>.Define(this, "Data");
        lineOffsetPort = ValueInputPort<int>.Define(this, "Offset");

        if (readType == ReadLineType.Rotation4X)
        {
            indexOutputPort = ValueOutputPort<int>.Define(this, "Index");
        }
    }
    protected override void Act(Flow flow)
    {
        if (grafic == null) grafic = flow.stack.gameObject.GetComponent<Grafic>();
        AnimationData data = dataPort.GetValue(flow);


        grafic.curAnimation = dataPort.GetValue(flow);
        
        grafic.lineOffset = lineOffsetPort.GetValue(flow);

        if (readType == ReadLineType.Rotation4X)
        {
            int index = grafic.GetIndex(data.count);
            grafic.Set(index, data.line + grafic.con.moveRotate4);
            indexOutputPort.SetValue(flow, index);
        }
    }
}