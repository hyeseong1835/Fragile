using Unity.VisualScripting;

[UnitCategory("Math/RotationToRotate4X")]
public class RotationToRotate4XNode : Unit
{
    ValueInputPort<float> f;
    ValueOutputPort<int> rotate4X;

    protected override void Definition()
    {
        f = ValueInputPort<float>.Define(this, "f");
        rotate4X = ValueOutputPort<int>.Define(this, "4X", flow => Math.Rotate4X(-1, f.GetValue(flow)));
    }
}
