using Sirenix.OdinInspector;
using System;
using UnityEngine;
using System.Collections.Generic;

[Serializable]
public class Animation
{
    bool isRepeat = false;

    public float time = 0;                      
    public float length = 0.5f;

    public void Update()
    {
        time -= Time.deltaTime;

        if (time < 0)
        {
            if (isRepeat) time = length;
            else time = 0;
        }
    }
    public Animation(bool _isRepeat)
    {
        isRepeat = _isRepeat;
    }
}
public enum AnimationType
{
    //Default
    None, Stay, Walk, Run, Crawl, Jump, Fall, Land, Dead, 

    //Skill Prepare
    //Cast, Charge, Aim, Ready, Hold, Draw, Sheath, Load, Unload, Equip, UnEquip,

    //Skill
    //Front, Back,

    //Skill End

}
public abstract class Grafic : MonoBehaviour
{
    public Animation curAnimation;
    public Animation[] animations { get; private set; } = 
    {
        
    };
    #if UNITY_EDITOR
    public string[] animationName;
    #endif

    public int[] animationIndex;

    void Update()
    {
        curAnimation.Update();
        if (curAnimation.time == 0) Run(AnimationType.None, 0);
    }
    public void Run(AnimationType type, float time)
    {
        curAnimation = animations[(int)type];
    }
}
