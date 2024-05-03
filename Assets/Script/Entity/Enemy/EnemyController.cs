using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System.Reflection.Emit;

[ExecuteAlways]
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
    [Required][PropertyOrder(0)]
        public EnemyGrafic grafic;

    [SerializeField] BehaviorState behaviorState;
    [SerializeField] Controller target;

    //[SerializeField] float attackCoolTime;


    void Update()
    {
        targetPos = (Vector2)target.transform.position + target.center;

        if (Utility.GetEditorStateByType(Utility.StateType.ISPLAY) == false) return;

        Behavior();
    }
    void Move()
    {
        moveVector = target.transform.position - transform.position;
        moveRotate = Utility.Vector2ToDegree(moveVector);

        transform.position += (Vector3)moveVector.normalized * Time.deltaTime * moveSpeed;
    }
    void Recoil()
    {

    }
    void Behavior()
    {
        switch (behaviorState)
        {
            case BehaviorState.Idle:
                behaviorState = BehaviorState.Move;
                break;

            case BehaviorState.Move:

                grafic.animationState = EnemyAnimationState.WALK;

                float distanceBetweenTarget = Vector2.Distance(targetPos, (Vector2)transform.position + center);
                
                //최대 거리 만족
                if (distanceBetweenTarget < curWeapon.attackMaxDistance)
                {
                    //최소 거리 만족
                    if (distanceBetweenTarget > curWeapon.attackMinDistance)
                    {
                        if(curWeapon != null) StartCoroutine(Attack());
                        //무기 없어지면 CancelCoroutine
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

        yield return new WaitForSeconds(curWeapon.attackFrontDelay);

        curWeapon.Attack();
        yield return new WaitForSeconds(curWeapon.attackDelay);

        yield return new WaitForSeconds(curWeapon.attackBackDelay);

        behaviorState = BehaviorState.Idle;
    }
    protected override void ControllerOnDrawGizmosSelected()
    {
        if (curWeapon != null)
        {
            //AttackRange
            Gizmos.DrawWireSphere(transform.position + (Vector3)center, curWeapon.attackMinDistance);
            Gizmos.DrawWireSphere(transform.position + (Vector3)center, curWeapon.attackMaxDistance);
        }
    }
}
