using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New AnimationData", menuName = "AnimationData", order = 0)]
public class AnimationData : ScriptableObject
{
    public float length = 1;
    public int count = 1;
    public int line = 0;
    public bool repeat = true;
 
    public ReadLineType readLineType = ReadLineType.Rotation4X;
}
