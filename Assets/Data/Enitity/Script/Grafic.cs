using Sirenix.OdinInspector;
using System;
using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.UIElements;

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
    public int spritePixelWidth = 16;
    public int spritePixelHeight = 16;
    public HandGrafic hand;
    public SpriteRenderer body;

    public GraficState state = GraficState.Idle;
    public GraficMoveStyle moveStyle = GraficMoveStyle.None;

    public int rotation = 0;

    public Vector2 view = new Vector2(0, 0);
    public Vector2 bodyPos = new Vector2(0, 0);

    public int curAnimationOrder = int.MinValue;
    public float t = 0;
    public float curAnimationlength = 1;
    public int curAnimationSize = 1;
    public int curAnimationLine = 0;
    public bool curAnimationRepeat = true;

    void Awake()
    {
        spriteSheet = Resources.Load<Texture2D>($"{con.data.FilePath}/BodySpriteSheet");
        body = transform.Find("Body").Find("Sprite").GetComponent<SpriteRenderer>();
    }
    void Update()
    {
        t += Time.deltaTime / curAnimationlength;
        if (t >= 1) t--;
        body.sprite = Utility.GetSprite(
            spriteSheet, 
            Mathf.FloorToInt(t * curAnimationSize) * curAnimationSize, 
            curAnimationLine, 
            spritePixelWidth, spritePixelHeight
        );
    }
}
[UnitTitle("Run Line")]
[UnitCategory("Animation")]
public class AnimationRun : Node
{
    public ValueInput Iv_line;
    public ValueInput Iv_count;
    public ValueInput Iv_time;
    public ValueInput Iv_repeat;
    public ValueInput Iv_order;

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
        if (flow.GetValue<int>(Iv_order) > grafic.curAnimationOrder)
        {
            grafic.curAnimationLine = flow.GetValue<int>(Iv_line);
            grafic.curAnimationSize = flow.GetValue<int>(Iv_count);
            grafic.curAnimationlength = flow.GetValue<float>(Iv_time);
            grafic.curAnimationRepeat = flow.GetValue<bool>(Iv_repeat);
        }
    }
}
[UnitTitle("Run Line By Rotation")]
[UnitCategory("Animation")]
public class AnimationRunByRotation : Node
{
    public ValueInput Iv_startLine;
    public ValueInput Iv_count;
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

        if(flow.GetValue<int>(Iv_order) > grafic.curAnimationOrder)
        {
            grafic.curAnimationLine = flow.GetValue<int>(Iv_startLine) + grafic.rotation;
            grafic.curAnimationSize = flow.GetValue<int>(Iv_count);
            grafic.curAnimationlength = flow.GetValue<float>(Iv_time);
            grafic.curAnimationRepeat = flow.GetValue<bool>(Iv_repeat);
        }
    }
}