using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface Input_Empty { public void Empty(); }
public interface Input_TriggerHit { public void TriggerHit(TriggerObject triggerObject, Collider2D coll); }
public interface Input_TriggerEvent { public void TriggerEvent(TriggerObject triggerObject); }

[ExecuteAlways]
public abstract class Module : MonoBehaviour
{
    [ReadOnly] public string moduleName;

    void Awake()
    {
        moduleName = GetType().Name;
        Debug.Log(moduleName);
        InitModule();
    }
    protected abstract void InitModule();
}

