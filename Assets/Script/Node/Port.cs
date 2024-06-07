using System;
using Unity.VisualScripting;

public abstract class Port
{
    public Unit unit;
    public string argumentName;
    protected abstract void EnsureUnique(string key);
    public abstract void Define();
}
public abstract class InputPort : Port
{
    protected override void EnsureUnique(string key)
    {
        if (unit.controlInputs.Contains(key) || unit.valueInputs.Contains(key) || unit.invalidInputs.Contains(key))
        {
            throw new ArgumentException($"Duplicate input for '{key}' in {GetType()}.");
        }
    }
}
public abstract class OutputPort : Port
{
    protected override void EnsureUnique(string key)
    {
        if (unit.controlOutputs.Contains(key) || unit.valueOutputs.Contains(key) || unit.invalidOutputs.Contains(key))
        {
            throw new ArgumentException($"Duplicate output for '{key}' in {GetType()}.");
        }
    }
}

public class ControlInputPort : InputPort
{
    public ControlInput port;
    public Func<Flow, ControlOutput> action;

    public ControlInputPort(Unit _unit, Func<Flow, ControlOutput> _action, string _argumentName = "In")
    {
        unit = _unit;
        action = _action;
        argumentName = _argumentName;
    }
    public override void Define()
    {
        port = ControlInput(argumentName, action);
    }
    protected ControlInput ControlInput(string key, Func<Flow, ControlOutput> action)
    {
        EnsureUnique(key);
        var port = new ControlInput(key, action);
        unit.controlInputs.Add(port);
        return port;
    }
}

public class ValueInputPort<ValueT> : InputPort
{
    public ValueInput port;
    public bool showField;
    public ValueT @default;

    public ValueInputPort(Unit _unit, string _argumentName = "typeof(ValueT).Name", bool _showField = false, ValueT _default = default)
    {
        unit = _unit;

        if (_argumentName == "typeof(ValueT).Name") argumentName = typeof(ValueT).Name;
        else argumentName = _argumentName;

        showField = _showField;
        @default = _default;
    }
    public override void Define()
    {
        port = ValueInput(argumentName);
    }
    ValueInput ValueInput(string key)
    {
        EnsureUnique(key);
        var port = new ValueInput(key, typeof(ValueT));
        unit.valueInputs.Add(port);
        if (showField) port.SetDefaultValue(@default);
        return port;
    }
    public ValueT GetValue(Flow flow) => flow.GetValue<ValueT>(port);
}


public class ControlOutputPort : OutputPort
{
    public ControlOutput port;

    public override void Define()
    {
        port = ControlOutput(argumentName);
    }
    protected ControlOutput ControlOutput(string key)
    {
        EnsureUnique(key);
        var port = new ControlOutput(key);
        unit.controlOutputs.Add(port);
        return port;
    }
}
public class ValueOutputPort<ValueT> : OutputPort
{
    public ValueOutput port;

    public override void Define()
    {
        port = ValueOutput(argumentName);
    }
    protected ValueOutput ValueOutput(string key)
    {
        EnsureUnique(key);
        var port = new ValueOutput(key, typeof(ValueT));
        unit.valueOutputs.Add(port);
        return port;
    }
    public void SetValue(Flow flow, ValueT value) => flow.SetValue(port, value);
}   