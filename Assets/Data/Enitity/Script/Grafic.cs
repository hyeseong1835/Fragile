using Sirenix.OdinInspector;
using System;
using UnityEngine;
using System.Collections.Generic;

public enum GraficState
{
    //Default
    Idle, Move,

    //Attack
    ChargeAttack, StartAttack, Attack, EndAttack,

    //Special
    ChargeSpecial, StartSpecial, Special, EndSpecial
}
public enum GraficMoveStyle
{
    None, Walk, 
    Jump, Dash, Charge
}
public class Grafic : MonoBehaviour
{
    public GraficState state = GraficState.Idle;
    public GraficMoveStyle moveStyle = GraficMoveStyle.None;

    public int rotation = 0;

    public Vector2 view = new Vector2(0, 0);
    public Vector2 body = new Vector2(0, 0);

    public void Run(int index, float time, bool repeat)
    {

    }
}