using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System.Reflection.Emit;
using UnityEngine.AI;
using System;

public enum EnemyType
{
    None, Friend, Hostile, Neutral
}
public abstract class EnemyController : Controller
{
    [HideInInspector]
    public EnemyControllerData data;
    public EnemyType enemyType;

    public override ControllerData ControllerData {
        get => data;
        set => data = (EnemyControllerData)value; 
    }
    public Controller target => PlayerController.instance;
}