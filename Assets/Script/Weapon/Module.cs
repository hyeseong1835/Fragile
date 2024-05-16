using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Module : MonoBehaviour
{
    [ShowInInspector]
    public string moduleName { get { return GetModuleName(); } }
    protected abstract string GetModuleName();
}
