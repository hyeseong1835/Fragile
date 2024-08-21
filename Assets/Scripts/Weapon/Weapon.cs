using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeaponEventHandler
{

}
public class Weapon : MonoBehaviour
{
    public WeaponData data;

    [NonSerialized] public Controller owner;

    [NonSerialized] public Controller[] controllerValueContainer;
    [NonSerialized] public int[] intValueContainer;
    [NonSerialized] public float[] floatValueContainer;

    void Awake()
    {
        gameObject.SetActive(false);
        controllerValueContainer = new Controller[data.rule.controllerValueContainerLength];
        intValueContainer = new int[data.rule.intValueContainerLength];
        floatValueContainer = new float[data.rule.floatValueContainerLength];
    }
    void Update()
    {

    }
}
