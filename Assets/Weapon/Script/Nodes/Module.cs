using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface IInput
{
    public Module GetModule();
}
public interface IVoid : IInput
{
    public static void Invoke(IVoid[] inputs)
    {
        foreach (IVoid input in inputs) { input.InputVoid(); }
    }
    public void InputVoid();
}
public interface IGameObject : IInput
{
    public static void Invoke(IGameObject[] inputs, GameObject obj)
    {
        foreach (IGameObject input in inputs) { input.InputGameObject(obj); }
    }
    public void InputGameObject(GameObject obj);
}
public interface IController : IInput
{
    public static void Invoke(IController[] inputs, Controller con)
    {
        foreach (IController input in inputs) { input.InputController(con); }
    }
    public void InputController(Controller con);
}
public interface IWeapon : IInput
{
    public static void Invoke(IWeapon[] inputs, Weapon weapon)
    {
        foreach (IWeapon input in inputs) { input.InputWeapon(weapon); }
    }
    public void InputWeapon(Weapon weapon);
}


[ExecuteAlways]
public abstract class Module : MonoBehaviour, IInput
{
    [ReadOnly] public string moduleName;

    void Awake()
    {
        if (this is Weapon)
        {

        }
        else
        {
            string scriptName = GetType().Name;
            int underBarIndex = scriptName.IndexOf('_');
            moduleName = scriptName.Substring(underBarIndex + 1, scriptName.Length - (underBarIndex + 1));
        }

        InitModule();
    }
    public Module GetModule()
    {
        return this;
    }
    public virtual void SpawnModule() { }
    protected abstract void InitModule();
}
