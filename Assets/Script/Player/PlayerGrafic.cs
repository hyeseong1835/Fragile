using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

public enum PlayerAnimationState
{
    NONE, test1, test2, test3
}
public class PlayerGrafic : Grafic
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

    protected override void StateSetToNONE()
    {
        playerState = PlayerAnimationState.NONE;
    }
    protected override void OtherAnimation()
    {
        if (animationState != AnimationState.NONE) return;


    }
}