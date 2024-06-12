using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "New AnimationData", menuName = "AnimationData", order = 0)]
public class AnimationData : ScriptableObject
{
    public Sprite[,] frame;
    public int repeatLayer;
}
