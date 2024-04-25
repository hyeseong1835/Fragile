using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

public enum PlayerAnimationState
{
    NONE, test1, test2, test3
}
interface OtherGrafic
{
    public void StateSetToNONE();
    public void OtherAnimation();
}
public class PlayerGrafic : Grafic, OtherGrafic
{
    [PropertySpace(10)]

    [BoxGroup("Animation/State")][ShowInInspector]
        [LabelText("Player")]
        public PlayerAnimationState playerState
        {
            get { return _playerState; }
            set
            {
                if (value != PlayerAnimationState.NONE)
                {
                    animationState = AnimationState.NONE;
                }

                _playerState = value;
            }
        } PlayerAnimationState _playerState;

    public void StateSetToNONE()
    {
        playerState = PlayerAnimationState.NONE;
    }
    public void OtherAnimation()
    {
        if (animationState != AnimationState.NONE) return;


    }
}