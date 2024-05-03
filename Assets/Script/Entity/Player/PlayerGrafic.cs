using Sirenix.OdinInspector;
using System.Diagnostics;
using UnityEngine;

public enum PlayerAnimationState
{
    STAY, WALK,
    NULL, NONE
}
public class PlayerGrafic : Grafic
{
    public PlayerAnimationState animationState;

    const int stayIndex = (int)PlayerAnimationState.STAY;
    const int walkIndex = (int)PlayerAnimationState.WALK;

    void Update()
    {
        Animation();
    }
    void Animation()
    {
        //���¿� ���� �ִϸ��̼�
        switch (animationState)
        {
            case PlayerAnimationState.STAY:
                
                body.sprite = animations[stayIndex].GetSprite(ref texture, Utility.FloorRotateToInt(con.lastMoveRotate, 8));

                animations[stayIndex].AnimationUpdate();

                //Hand
                if (hand.handMode == HandMode.NONE || hand.handMode == HandMode.ToHand)
                {
                    hand.transform.localPosition = Utility.Vector2TransformToEllipse(con.lastMoveVector.normalized, 0.75f, 0.5f) + con.center;
                    hand.transform.localRotation = Quaternion.identity;
                }
                break;
            
            case PlayerAnimationState.WALK:

                body.sprite = animations[walkIndex].GetSprite(ref texture, Utility.FloorRotateToInt(con.moveRotate, 8));
                
                animations[walkIndex].AnimationUpdate();

                //Hand
                if (hand.handMode == HandMode.NONE || hand.handMode == HandMode.ToHand)
                {
                    hand.transform.localPosition = Utility.Vector2TransformToEllipse(con.moveVector.normalized, 0.75f, 0.5f) + con.center;
                    hand.transform.localRotation = Quaternion.identity;
                }
                break;
        }
    }
}