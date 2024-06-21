using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System.Reflection.Emit;
using UnityEngine.AI;
using System;

public class EnemyController : Controller
{
    [HideInInspector]
    public EnemyControllerData data;
    public override ControllerData ControllerData {
        get => data;
        set => data = (EnemyControllerData)value; 
    }
    public override Type DataType => typeof(EnemyControllerData);
    public Controller target => PlayerController.instance;
    public enum BehaviorState
    {
        Move, Recoil,
        ChargeAttack, Attack
    }
    public BehaviorState behaviorState;

    //[FoldoutGroup("NavMeshAgent")]
    #region Foldout NavMeshAgent

        //public const int agentTypeID = 0;

        //NavMeshAgent agent;

    #endregion
    /*
    void Awake()
    {
        if (Editor.GetApplicationType(Editor.StateType.IsPlay))
        {
            //agent = gameObject.AddComponent<NavMeshAgent>();
            //agent.speed = data.moveSpeed;
        }
    }
    void Start()
    {
        return;
        if (target == null) return;

        if (curWeapon == null)
        {
            Recoil();
            return;
        }
        Move();
        return;
    }
    new void Update()
    {
        base.Update();

        return;

        targetPos = (Vector2)target.transform.position + target.ControllerData.center;

        if (Editor.GetApplicationType(Editor.StateType.IsEditor))
        {
            /*
            if (TryGetComponent<NavMeshAgent>(out agent))
            {
                Debug.LogWarning("\"NavMeshAgent\"�� �÷��� ���� �� �ڵ����� �߰��˴ϴ�");
                DestroyImmediate(agent);
                agent = null;
            }
            //
        }
    }
    void Move()
    {
        behaviorState = BehaviorState.Move;

        moveVector = target.transform.position - transform.position;

        transform.position += (Vector3)moveVector.normalized * Time.deltaTime * data.moveSpeed;

        float distanceBetweenTarget = Vector2.Distance(targetPos, (Vector2)transform.position + data.center);


        //�ִ� �Ÿ� ����
        if (distanceBetweenTarget < curWeapon.attack.maxDistance)
        {
            //�ּ� �Ÿ� ����
            if (distanceBetweenTarget > curWeapon.attack.minDistance)
            {
                if (curWeapon != null)
                {
                    StartCoroutine(Attack());
                    return;
                }
                //���� �������� CancelCoroutine
            }
            //�ּ� �Ÿ� �̸� >> ����
            else
            {
                Recoil();
                return;
            }
        }
        // �ִ� �Ÿ� �ʰ� >> ����
        else
        {
            Move();
            return;
        }
    }
    void Recoil()
    {
        behaviorState = BehaviorState.Recoil;

        moveVector = -(target.transform.position - transform.position);

        transform.position += (Vector3)moveVector.normalized * Time.deltaTime * data.moveSpeed * 0.5f;
        
        float distanceBetweenTarget = Vector2.Distance(targetPos, (Vector2)transform.position + data.center);

        //�ִ� �Ÿ� ����
        if (distanceBetweenTarget < curWeapon.attack.maxDistance)
        {
            //�ּ� �Ÿ� ����
            if (distanceBetweenTarget > curWeapon.attack.minDistance)
            {
                if (curWeapon != null)
                {
                    StartCoroutine(Attack());
                    return;
                }
                //���� �������� CancelCoroutine
            }
            //�ּ� �Ÿ� �̸� >> ����
            else
            {
                Recoil();
                return;
            }
        }
    }
    IEnumerator Attack()
    {
        behaviorState = BehaviorState.Attack;

        yield return new WaitForSeconds(curWeapon.attack.frontDelay);

        attack = true;

        yield return new WaitForSeconds(curWeapon.attack.delay);


        Move();
    }
    new void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();
        if (curWeapon != null)
        {
            //AttackRange
            Gizmos.DrawWireSphere(transform.position + (Vector3)data.center, curWeapon.attack.minDistance);
            Gizmos.DrawWireSphere(transform.position + (Vector3)data.center, curWeapon.attack.maxDistance);
        }
    }
    */
}