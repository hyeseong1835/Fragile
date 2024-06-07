using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System.Reflection.Emit;
using UnityEngine.AI;
using System;

public class EnemyController : Controller
{
    public EnemyControllerData data;
    public override ControllerData ControllerData { 
        get => data;
        set => data = (EnemyControllerData)value; 
    }
    public override Type DataType => typeof(EnemyControllerData);

    public enum BehaviorState
    {
        Move, Recoil,
        ChargeAttack, Attack
    }
    public BehaviorState behaviorState;

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

    void Awake()
    {
        if (Editor.GetApplicationType(Editor.StateType.IsPlay))
        {
            agent = gameObject.AddComponent<NavMeshAgent>();
            agent.speed = data.moveSpeed;
        }
    }
    void Start()
    {
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

        targetPos = (Vector2)target.transform.position + target.ControllerData.center;

        if (Editor.GetApplicationType(Editor.StateType.IsEditor))
        {
            if (TryGetComponent<NavMeshAgent>(out agent))
            {
                Debug.LogWarning("\"NavMeshAgent\"는 플레이 시작 시 자동으로 추가됩니다");
                DestroyImmediate(agent);
                agent = null;
            }
        }
    }
    void Move()
    {
        behaviorState = BehaviorState.Move;

        moveVector = target.transform.position - transform.position;

        transform.position += (Vector3)moveVector.normalized * Time.deltaTime * data.moveSpeed;

        float distanceBetweenTarget = Vector2.Distance(targetPos, (Vector2)transform.position + data.center);


        //최대 거리 만족
        if (distanceBetweenTarget < curWeapon.attack.maxDistance)
        {
            //최소 거리 만족
            if (distanceBetweenTarget > curWeapon.attack.minDistance)
            {
                if (curWeapon != null)
                {
                    StartCoroutine(Attack());
                    return;
                }
                //무기 없어지면 CancelCoroutine
            }
            //최소 거리 미만 >> 도주
            else
            {
                Recoil();
                return;
            }
        }
        // 최대 거리 초과 >> 추적
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

        //최대 거리 만족
        if (distanceBetweenTarget < curWeapon.attack.maxDistance)
        {
            //최소 거리 만족
            if (distanceBetweenTarget > curWeapon.attack.minDistance)
            {
                if (curWeapon != null)
                {
                    StartCoroutine(Attack());
                    return;
                }
                //무기 없어지면 CancelCoroutine
            }
            //최소 거리 미만 >> 도주
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

        yield return new WaitForSeconds(curWeapon.attack.backDelay);

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
}