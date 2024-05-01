using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : Controller
{
    enum BehaviorState
    {
        NULL, None, Idle, Move, Recoil
    }
    enum AnimateState
    {
        NULL, None, Stay, Move
    }
    public EnemyGrafic grafic;

    [SerializeField] BehaviorState behaviorState;
    [SerializeField] Transform target;

    [SerializeField] float attackMinDistance;
    [SerializeField] float attackMaxDistance;

    //[SerializeField] float attackCoolTime;
    [SerializeField] float attackFrontDelay;
    [SerializeField] float attackDelay;
    [SerializeField] float attackBackDelay;

    void Update()
    {
        targetPos = target.position;

        if (Utility.GetEditorStateByType(Utility.StateType.ISPLAY) == false) return;

        Behavior();
    }
    void Move()
    {
        moveVector = target.position - transform.position;
        moveRotate = Utility.Vector2ToDegree(moveVector);

        transform.position += (Vector3)moveVector.normalized * Time.deltaTime * moveSpeed;
    }
    void Recoil()
    {

    }
    void Behavior()
    {
        float distanceBetweenTarget = Vector2.Distance(target.position, transform.position);

        switch (behaviorState)
        {
            case BehaviorState.Idle:
                behaviorState = BehaviorState.Move;
                break;

            case BehaviorState.Move:

                //최대 거리 만족
                if (distanceBetweenTarget < attackMaxDistance)
                {
                    //최소 거리 만족
                    if (distanceBetweenTarget > attackMinDistance)
                    {
                        StartCoroutine(Attack());
                    }
                    //최소 거리 미만 >> 도주
                    else Recoil();
                }
                // 최대 거리 초과 >> 추적
                else Move();

                break;
        }
    }
    IEnumerator Attack()
    {
        behaviorState = BehaviorState.None;
        grafic.animationState = EnemyAnimationState.NONE;
        lastMoveVector = moveVector;
        moveVector = Vector2.zero;

        yield return new WaitForSeconds(attackFrontDelay);

        curWeapon.Attack();
        yield return new WaitForSeconds(attackDelay);

        yield return new WaitForSeconds(attackBackDelay);

        behaviorState = BehaviorState.Idle;
    }
}
