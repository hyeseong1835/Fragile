using Sirenix.OdinInspector;
using System;
using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

public enum GraficState
{
    //Default
    Idle, Move,

    //Attack
    ChargeAttack, Attack, 

    //Special
    ChargeSpecial, Special
}
public enum GraficMoveStyle
{
    None, Walk, 
    Jump, Dash, Charge
}
public class Grafic : MonoBehaviour
{
    public Controller con;
    public Texture2D spriteSheet { get; private set; }
    public HandGrafic hand;

    public GraficState state = GraficState.Idle;
    public GraficMoveStyle moveStyle = GraficMoveStyle.None;

    public int rotation = 0;

    public Vector2 view = new Vector2(0, 0);
    public Vector2 body = new Vector2(0, 0);

    public int currentAnimationOrder = int.MinValue;

    void Awake()
    {
        con = transform.parent.GetComponent<Controller>();
        spriteSheet = Resources.Load<Texture2D>(con.data.name);
    }
    public void Run(int index, float time, bool repeat)
    {

    }
}
[UnitTitle("Run Line")]
[UnitCategory("Animation")]
public class AnimationRun : Node
{
    public ValueInput Iv_line;
    public ValueInput Iv_time;
    public ValueInput Iv_repeat;

    Grafic grafic;

    protected override void Definition()
    {
        base.Definition();

        Iv_line = ValueInput<int>("Line");
        Iv_time = ValueInput<float>("Time");
        Iv_repeat = ValueInput<bool>("Repeat");
    }
    protected override void Act(Flow flow)
    {
        grafic.Run(flow.GetValue<int>(Iv_line), flow.GetValue<float>(Iv_time), flow.GetValue<bool>(Iv_repeat));
    }
}
[UnitTitle("Run Line By Rotation")]
[UnitCategory("Animation")]
public class AnimationRunByRotation : Node
{
    public ValueInput Iv_startLine;
    public ValueInput Iv_time;
    public ValueInput Iv_repeat;
    public ValueInput Iv_order;

    Grafic grafic;

    protected override void Definition()
    {
        base.Definition();

        Iv_startLine = ValueInput<int>("StartLine", 0);
        Iv_time = ValueInput<float>("Time", 1);
        Iv_repeat = ValueInput<bool>("Repeat", false);
        Iv_order = ValueInput<int>("Order", 1);
    }
    protected override void Act(Flow flow)
    {
        if (grafic == null) grafic = flow.stack.gameObject.GetComponent<Grafic>();

        int startLine = flow.GetValue<int>(Iv_startLine);
        float time = flow.GetValue<float>(Iv_time);
        bool repeat = flow.GetValue<bool>(Iv_repeat);
        int order = flow.GetValue<int>(Iv_order);

        if(order > grafic.currentAnimationOrder)
        {
            grafic.Run(startLine + grafic.rotation, time, repeat);
        }
    }
}