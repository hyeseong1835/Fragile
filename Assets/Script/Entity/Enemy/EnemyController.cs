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

        [Required][ChildGameObjectsOnly][PropertyOrder(0)]
        [LabelWidth(Editor.propertyLabelWidth - Editor.childGameObjectOnlyWidth)]//-|
        public EnemyGrafic grafic;
                                                                                     [BoxGroup("Object")]
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
        agent = gameObject.AddComponent<NavMeshAgent>();
        agent.speed = moveSpeed;
    }
    void Update()
    {
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
        moveRotate = Utility.Vector2ToDegree(moveVector);

        transform.position += (Vector3)moveVector.normalized * Time.deltaTime * moveSpeed;
    }
    void Recoil()
    {
        moveVector = -(target.transform.position - transform.position);
        moveRotate = Utility.Vector2ToDegree(moveVector);

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

                grafic.animationState = EnemyAnimationState.WALK;

                if (curWeapon == null)
                {
                    Recoil();
                    return;
                }
                float distanceBetweenTarget = Vector2.Distance(targetPos, (Vector2)transform.position + center);
                
                //�ִ� �Ÿ� ����
                if (distanceBetweenTarget < curWeapon.attackMaxDistance)
                {
                    //�ּ� �Ÿ� ����
                    if (distanceBetweenTarget > curWeapon.attackMinDistance)
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
        grafic.animationState = EnemyAnimationState.NONE;

        yield return new WaitForSeconds(curWeapon.attackFrontDelay);

        foreach (Skill skill in curWeapon.attackSkills)
        {
            skill.Execute();
        }

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
