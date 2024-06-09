using System;
using Unity.VisualScripting;

public class ControlInputPort
{
    public ControlInput port;

    public void Define(Unit unit, Func<Flow, ControlOutput> action, string argumentName = "")
    {
        if (unit.controlInputs.Contains(argumentName)) throw new ArgumentException($"Duplicate input for '{argumentName}' in {unit}.");

        port = new ControlInput(argumentName, action);
        unit.controlInputs.Add(port);
    }
}

public class ControlOutputPort
{
    public ControlOutput port;

    public void Define(Unit unit, string argumentName = "")
    {
        if (unit.controlInputs.Contains(argumentName)) throw new ArgumentException($"Duplicate input for '{argumentName}' in {unit}.");

        port = new ControlOutput(argumentName);
        unit.controlOutputs.Add(port);
    }
}
public class ValueInputPort<ValueT>
{
    public ValueInput port;

    public void Define(Unit unit, string argumentName = "typeof(ValueT).Name", bool showField = true, ValueT @default = default)
    {
        if (argumentName == "typeof(ValueT).Name") argumentName = typeof(ValueT).Name;
        
        if (unit.valueInputs.Contains(argumentName)) throw new ArgumentException($"Duplicate ValueInput for '{argumentName}' in {unit}.");

        port = new ValueInput(argumentName, typeof(ValueT));
        unit.valueInputs.Add(port);
        if (showField) port.SetDefaultValue(@default);
    }
    public ValueT GetValue(Flow flow) => flow.GetValue<ValueT>(port);
}

public class ValueOutputPort<ValueT>
{
    public ValueOutput port; 

    public void Define(Unit unit, string argumentName = "typeof(ValueT).Name", Func<Flow, object> getValue = null)
    {
        if (argumentName == "typeof(ValueT).Name") argumentName = typeof(ValueT).Name;

        if (unit.valueOutputs.Contains(argumentName)) throw new ArgumentException($"Duplicate ValueInput for '{argumentName}' in {unit}.");

        port = new ValueOutput(argumentName, typeof(ValueT), getValue);
        unit.valueOutputs.Add(port);
    }
    public void SetValue(Flow flow, ValueT value) => flow.SetValue(port, value);
}   