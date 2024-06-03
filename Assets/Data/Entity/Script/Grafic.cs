using Sirenix.OdinInspector;
using System;
using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.UIElements;
using UnityEngine.UI;
using UnityEngine.Windows;

public enum GraficMoveStyle
{
    Walk, Jump, Dash, Charge
}
public enum GraficSkillStyle
{

}
public class Grafic : MonoBehaviour
{
    public Controller con;
    public HandGrafic hand;
    public SpriteRenderer body;
    public Texture2D spriteSheet;

    [Button]
    void Refresh()
    {
        con = transform.parent.GetComponent<Controller>();
        if (spriteSheet == null) spriteSheet = Resources.Load<Texture2D>($"{con.ControllerData.name}/BodySpriteSheet");
            //Hand
            Transform handTransform = transform.Find("Hand");
            if (handTransform == null)
            { 
                GameObject handObj = new GameObject("Hand");
                handObj.transform.parent = transform;
                handTransform = handObj.transform; 
            }
            if (handTransform.TryGetComponent<HandGrafic>(out hand) == false)
            {
                hand = handTransform.AddComponent<HandGrafic>();
            }
            hand.transform.parent = transform;
            //Body
            Transform bodyTransform = transform.Find("Body");
            if (bodyTransform == null)
            { 
                GameObject bodyObj = new GameObject("Body"); 
                bodyObj.transform.parent = transform;
                bodyTransform = bodyObj.transform; 
            }
                //Sprite
                Transform bodySpriteTransform = bodyTransform.Find("Body");
                if (bodySpriteTransform == null)
                {        
                    GameObject bodySpriteObj = new GameObject("Sprite");
                    bodySpriteObj.transform.parent = bodyTransform;
                    bodySpriteTransform = bodySpriteObj.transform;
                }
                if (bodySpriteTransform.TryGetComponent<SpriteRenderer>(out body) == false)
                {
                    body = bodySpriteTransform.AddComponent<SpriteRenderer>();
                }
    }

    [ShowInInspector][DisableInEditorMode]
    [TableMatrix(IsReadOnly = true, SquareCells = true, HorizontalTitle = "Frame", VerticalTitle = "Index")]
    public Sprite[,] sprites;

    public int spritePixelWidth = 16;
    public int spritePixelHeight = 16;
    
    public GraficMoveStyle moveStyle = GraficMoveStyle.Walk;

    public int rotation = 0;

    public Vector2 view = new Vector2(0, 0);
    public Vector2 bodyPos = new Vector2(0, 0);

    public int curAnimationOrder = int.MinValue;
    public float t = 0;
    public float curAnimationTime = 1;
    public int curAnimationCount = 1;
    public int curAnimationLine = 0;
    public bool curAnimationRepeat = true;

    public void CreateAsset()
    {
        transform.parent = con.transform;
        string graphDataPath = $"{ControllerData.resourceFolderPath}/{con.ControllerData.name}.asset";
        if (Directory.Exists(graphDataPath))
        {
            gameObject.AddComponent<ScriptMachine>().graphData = (IGraphData)Resources.Load(graphDataPath);
        }

        if (Directory.Exists($"{ControllerData.resourceFolderPath}/{con.ControllerData.name}/BodySpriteSheet.png"))
        {
            spriteSheet = Resources.Load<Texture2D>($"{con.ControllerData.name}/BodySpriteSheet");
        }
            //Hand
            hand = new GameObject("Hand").AddComponent<HandGrafic>();
            hand.transform.parent = con.transform;

            //Body
            GameObject bodyObj = new GameObject("Body");
            bodyObj.transform.parent = con.transform;
                //Sprite
                GameObject bodySpriteObj = new GameObject("Sprite");
                bodySpriteObj.transform.parent = body.transform;
                body = bodySpriteObj.AddComponent<SpriteRenderer>();
    }

    void Awake()
    {
        sprites = new Sprite[spriteSheet.width / spritePixelWidth, spriteSheet.height / spritePixelHeight];
        for(int x = 0; x < sprites.GetLength(0); x++)
        {
            for(int y = 0; y < sprites.GetLength(1); y++)
            {
                sprites[x, y] = Utility.GetSprite(spriteSheet, x, y, spritePixelWidth, spritePixelHeight);
            }
        }
    }
    void Update()
    {
        if (curAnimationTime * (1 - t) > Time.deltaTime)
        {
            t += Time.deltaTime / curAnimationTime;
            if (t >= 1) t--;
        }
        else t = 0;

        int frameIndex = Mathf.FloorToInt(t * curAnimationCount);
      
        body.sprite = sprites[frameIndex, curAnimationLine];
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

        Iv_line = ValueInput<int>("Line", 0);
        Iv_count = ValueInput<int>("Count", 4);
        Iv_time = ValueInput<float>("Time", 1);
        Iv_repeat = ValueInput<bool>("Repeat", false);
        Iv_order = ValueInput<int>("Order", 1);
    }
    protected override void Act(Flow flow)
    {
        if (grafic == null) grafic = flow.stack.gameObject.GetComponent<Grafic>();

        //if (flow.GetValue<int>(Iv_order) >= grafic.curAnimationOrder)
        //{
            grafic.curAnimationOrder = flow.GetValue<int>(Iv_order);
            grafic.curAnimationLine = flow.GetValue<int>(Iv_line);
            grafic.curAnimationCount = flow.GetValue<int>(Iv_count);
            grafic.curAnimationTime = flow.GetValue<float>(Iv_time);
            grafic.curAnimationRepeat = flow.GetValue<bool>(Iv_repeat);
        //}
    }
}