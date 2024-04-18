using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

public enum PlayerAnimationState
{
    NONE
}
interface OtherGrafic
{
    void OtherAnimation();
}
public class PlayerGrafic : Grafic, OtherGrafic
{
    public PlayerAnimationState playerState;

    public void OtherAnimation()
    {
        if (state != AnimationState.NONE) return;


    }
    [ShowIf("debug")][Button]
    public void SetState(PlayerAnimationState _playerState)
    {
        state = AnimationState.NONE;

        playerState = _playerState;
    }

}