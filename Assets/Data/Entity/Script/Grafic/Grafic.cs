using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UI;
using UnityEngine.Windows;
using UnityEditor;
using UnityEditor.Experimental;
using System.Collections.Generic;

public enum AnimationType
{
    Move, Attack, Special
}
public enum MoveAnimationType
{
    Stay, Walk, Jump, Dash, Charge
}
public enum SkillAnimationType
{
    Swing, Shoot, Throw, Cast, Summon
}
public enum ReadLineType
{
    Custom, One, Rotation4X
}
public class RepeatLayer
{
#if UNITY_EDITOR
    public string label;
#endif
    public float time;

    public void Update(float length)
    {
        if (length * (1 - time) > Time.deltaTime)
        {
            time += Time.deltaTime / length;
            if (time >= 1) time--;
        }
        else time = 0;
    }
    public RepeatLayer(string label)
    {
        this.label = label;
    }
}
public class Grafic : MonoBehaviour
{
    [MenuItem("GameObject/Create Asset/Grafic", false, 0)]
    static void Create(MenuCommand menuCommand)
    {
        GameObject parentGameObject = (GameObject)menuCommand.context;
        Grafic grafic = Instantiate(EditorResources.Load<GameObject>("Preset/Grafic.prefab"), parentGameObject.transform).GetComponent<Grafic>();
        grafic.gameObject.name = "New Grafic";
        grafic.con = parentGameObject.GetComponent<Controller>();
        grafic.renderer = grafic.GetComponent<SpriteRenderer>();
    }

    [Required]
    [LabelWidth(Editor.propertyLabelWidth)] 
    public Controller con;

    [ReadOnly]
    [LabelWidth(Editor.propertyLabelWidth)] 
    new public SpriteRenderer renderer;

    [Required]
    [LabelWidth(Editor.propertyLabelWidth)]
    public AnimationData curAnimation;

    [ShowInInspector]
    public RepeatLayer[] layer = new RepeatLayer[1] { new RepeatLayer("NoRepeat") };
}