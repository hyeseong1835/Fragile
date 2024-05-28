using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System.Reflection.Emit;
using UnityEngine.AI;

[ExecuteAlways]
public class EnemyController : Controller
{
    [Space(Editor.overrideSpace)]
    [BoxGroup("Object")]
    #region Override Box Object  - - - - - - - - - - - - - - - - - - - - - - - - - -|
    
        [SerializeField] 
        [LabelWidth(Editor.propertyLabelWidth)]
        Controller target;

    #endregion - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -|

    [FoldoutGroup("NavMeshAgent")]
    #region Foldout NavMeshAgent

        //public const int agentTypeID = 0;

        NavMeshAgent agent;

    #endregion

    enum BehaviorState
    {
        NULL, None, Idle, Move, Recoil
    }
    [SerializeField] BehaviorState behaviorState;

    enum AnimateState
    {
        NULL, None, Stay, Move
    }
    void Awake()
    {
        if (Editor.GetType(Editor.StateType.IsPlay))
        {
            agent = gameObject.AddComponent<NavMeshAgent>();
            agent.speed = moveSpeed;
        }
    }
    new void Update()
    {
        base.Update();

        if (target == null) return;

        targetPos = (Vector2)target.transform.position + target.center;

        if (Editor.GetType(Editor.StateType.IsPlay))
        {
            Behavior();
        }
        else
        {
            if(TryGetComponent<NavMeshAgent>(out agent))
            {
                Debug.LogWarning("\"NavMeshAgent\"�� �÷��� ���� �� �ڵ����� �߰��˴ϴ�");
                DestroyImmediate(agent);
                agent = null;
            }
        }

    }
    void Move()
    {
        moveVector = target.transform.position - transform.position;

        transform.position += (Vector3)moveVector.normalized * Time.deltaTime * moveSpeed;
    }
    void Recoil()
    {
        moveVector = -(target.transform.position - transform.position);

        transform.position += (Vector3)moveVector.normalized * Time.deltaTime * moveSpeed * 0.5f;
    }
    void Behavior()
    {
        switch (behaviorState)
        {
            case BehaviorState.Idle:
                behaviorState = BehaviorState.Move;
                break;

            case BehaviorState.Move:

                if (curWeapon == null)
                {
                    Recoil();
                    return;
                }
                float distanceBetweenTarget = Vector2.Distance(targetPos, (Vector2)transform.position + center);
                
                //�ִ� �Ÿ� ����
                if (distanceBetweenTarget < curWeapon.attack.maxDistance)
                {
                    //�ּ� �Ÿ� ����
                    if (distanceBetweenTarget > curWeapon.attack.minDistance)
                    {
                        if(curWeapon != null) StartCoroutine(Attack());
                        //���� �������� CancelCoroutine
                    }
                    //�ּ� �Ÿ� �̸� >> ����
                    else Recoil();
                }
                // �ִ� �Ÿ� �ʰ� >> ����
                else Move();

                break;
        }
    }
    IEnumerator Attack()
    {
        behaviorState = BehaviorState.None;

        yield return new WaitForSeconds(curWeapon.attack.frontDelay);

        attack = true;

        yield return new WaitForSeconds(curWeapon.attack.delay);

        yield return new WaitForSeconds(curWeapon.attack.backDelay);

        behaviorState = BehaviorState.Idle;
    }
    new void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();
        if (curWeapon != null)
        {
            //AttackRange
            Gizmos.DrawWireSphere(transform.position + (Vector3)center, curWeapon.attack.minDistance);
            Gizmos.DrawWireSphere(transform.position + (Vector3)center, curWeapon.attack.maxDistance);
        }
    }
}