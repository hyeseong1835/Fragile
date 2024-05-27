using Sirenix.OdinInspector;
using System;
using UnityEngine;

[Serializable]
public class Animation
{
    bool isRepeat = false;

    public float time = 0;                      
    public float length = 0.5f;

    public void Update()
    {
        time += Time.deltaTime;
        if (time > length)
        {
            time = 0;
            if (!isRepeat) 
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
    None, Stay, Walk, Run//, Crawl, Jump, Fall, Land, Dead,

    //Skill Prepare
    //Cast, 

    //Skill
    //Front, Back

    //Skill End

}
public abstract class Grafic : MonoBehaviour
{
    public Animation curAnimation;
    public Animation[] animations { get; private set; } =
    {

    };

    void Update()
    {
        curAnimation.Update();
    }
    public void Run(AnimationType type)
    {
        curAnimation = animations[(int)type];
    }
}
