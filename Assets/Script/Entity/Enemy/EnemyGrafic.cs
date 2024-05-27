using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyAnimationState
{
    STAY, WALK,
    NULL, NONE
}
public class EnemyGrafic : Grafic
{
    public EnemyAnimationState animationState;
    const int stayIndex = (int)EnemyAnimationState.STAY;
    const int walkIndex = (int)EnemyAnimationState.WALK;

    void Update()
    {
        Animation();
    }
    void Animation()
    {
        //상태에 따른 애니메이션
        switch (animationState)
        {
            case EnemyAnimationState.STAY:

                body.sprite = animations[stayIndex].GetSprite(ref texture, Utility.FloorRotateToInt(con.lastMoveRotate, 8));
                animations[stayIndex].Update();

                //Hand
                if (hand.handMode == HandMode.NONE || hand.handMode == HandMode.ToHand)
                {
                    hand.transform.localPosition = Utility.Vector2TransformToEllipse(con.lastMoveVector.normalized, 0.75f, 0.5f) + con.center;
                    hand.transform.localRotation = Quaternion.identity;
                }
                break;

            case EnemyAnimationState.WALK:

                body.sprite = animations[walkIndex].GetSprite(ref texture, Utility.FloorRotateToInt(con.moveRotate, 8));
                animations[walkIndex].Update();

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
