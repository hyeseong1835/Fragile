using System;
using Unity.VisualScripting;

public class ControlInputPort
{
    [DoNotSerialize]
    public ControlInput port;

    public static ControlInputPort Define(Unit unit, string argumentName, Func<Flow, ControlOutput> action)
    {
        ControlInputPort controlInputPort = new ControlInputPort();

        if (unit.controlInputs.Contains(argumentName)) throw new ArgumentException($"Duplicate input for '{argumentName}' in {unit}.");

        controlInputPort.port = new ControlInput(argumentName, action);
        unit.controlInputs.Add(controlInputPort.port);

        return controlInputPort;
    }
}
public class ControlOutputPort
{
    [DoNotSerialize]
    public ControlOutput port;

    public static ControlOutputPort Define(Unit unit, string argumentName)
    {
        ControlOutputPort outputPort = new ControlOutputPort();

        if (unit.controlOutputs.Contains(argumentName)) throw new ArgumentException($"Duplicate input for '{argumentName}' in {unit}.");

        outputPort.port = new ControlOutput(argumentName);
        unit.controlOutputs.Add(outputPort.port);

        return outputPort;
    }
    public void Run(Flow flow) => flow.Run(port);
    public void Run(GraphReference reference) => Flow.New(reference).Run(port);
}
public class ValueInputPort<ValueT>
{
    [DoNotSerialize]
    public ValueInput port;

    public static ValueInputPort<ValueT> Define(Unit unit, string argumentName = "typeof(ValueT).Name", bool showField = true, ValueT @default = default)
    {
        ValueInputPort<ValueT> valueInputPort = new ValueInputPort<ValueT>();

        if (argumentName == "typeof(ValueT).Name") argumentName = typeof(ValueT).Name;
        
        if (unit.valueInputs.Contains(argumentName)) throw new ArgumentException($"Duplicate ValueInput for '{argumentName}' in {unit}.");

        valueInputPort.port = new ValueInput(argumentName, typeof(ValueT));
        unit.valueInputs.Add(valueInputPort.port);
        if (showField) valueInputPort.port.SetDefaultValue(@default);

        return valueInputPort;
    }
    public ValueT GetValue(Flow flow) => flow.GetValue<ValueT>(port);
}

public class ValueOutputPort<ValueT>
{
    [DoNotSerialize]
    public ValueOutput port; 

    public static ValueOutputPort<ValueT> Define(Unit unit, string argumentName = "typeof(ValueT).Name", Func<Flow, object> getValue = null)
    {
        ValueOutputPort<ValueT> valueOutputPort = new ValueOutputPort<ValueT>();

        if (argumentName == "typeof(ValueT).Name") argumentName = typeof(ValueT).Name;

        if (unit.valueOutputs.Contains(argumentName)) throw new ArgumentException($"Duplicate ValueInput for '{argumentName}' in {unit}.");

        if (getValue == null) valueOutputPort.port = new ValueOutput(argumentName, typeof(ValueT));
        else valueOutputPort.port = new ValueOutput(argumentName, typeof(ValueT), getValue);

        unit.valueOutputs.Add(valueOutputPort.port);

        return valueOutputPort;
    }
    public void SetValue(Flow flow, ValueT value) => flow.SetValue(port, value);
}