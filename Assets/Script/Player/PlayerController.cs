using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;

public enum AnimateState
{
    Stay, Move, Battle
}

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : Controller
{
    [HideInInspector] public Vector3 prevMoveVector;
    //마우스
    [HideInInspector] public Vector2 mousePos { get { return Input.mousePosition; } }
    [HideInInspector] public Vector2 playerToMouse { get { return (Player.camCon.cam.ScreenToWorldPoint(mousePos) - transform.position); } }
    [HideInInspector] public float viewRotateZ { get { return Mathf.Atan2(playerToMouse.y, playerToMouse.x) * Mathf.Rad2Deg; } }

    //좌클릭
    [HideInInspector] public bool mouse0Down { get { return Input.GetMouseButtonDown(0); } }
    [HideInInspector] public bool mouse0Stay { get { return Input.GetMouseButton(0); } }
    [HideInInspector] public bool mouse0Up { get { return Input.GetMouseButtonUp(0); } }

    //우클릭
    [HideInInspector] public bool mouse1Down { get { return Input.GetMouseButtonDown(1); } }
    [HideInInspector] public bool mouse1 { get { return Input.GetMouseButton(1); } }
    [HideInInspector] public bool mouse1Up { get { return Input.GetMouseButtonUp(1); } }

    //마우스 휠
    [HideInInspector] public float mouseWheelScroll { get { return Input.GetAxis("Mouse ScrollWheel"); } }
    [HideInInspector] public bool mouseWheelClickDown { get { return Input.GetMouseButtonDown(2); } }
    [HideInInspector] public bool mouseWheelClick{ get { return Input.GetMouseButtonUp(2); } }
    [HideInInspector] public bool mouseWheelClickUp { get { return Input.GetMouseButtonDown(2); } }

    //기타
    [SerializeField][ReadOnly] bool attackInput = false;
    float attackInputAllowTime = 1;
    Coroutine curAttackInputCoroutine;
    public float attackCool = 0;
    public float moveSpeed = 1;

    void Awake()
    {
        Player.pCon = this;
        Player.transform = transform;
        Player.gameObject = gameObject;
    }
    void Update()
    {                         
        Mouse();
        Move();
        Attack();
    }
    void Mouse()
    {
        targetPos = Player.camCon.cam.ScreenToWorldPoint(mousePos);
    }
    void Move()
    {
        prevMoveVector = moveVector;
        moveVector = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);

        transform.position += moveVector.normalized * Time.deltaTime * moveSpeed;
        if (moveVector.magnitude >= Mathf.Epsilon) moveRotate = Mathf.Atan2(moveVector.y, moveVector.x) * Mathf.Rad2Deg;
    }
    void Attack()
    {
        if (mouse0Down)
        {
            CancelInvoke(nameof(AttackInputCancel));
            attackInput = true;
            Invoke(nameof(AttackInputCancel), attackInputAllowTime);
        }

        if ((attackInput) && attackCool == 0)
        {
            attackInput = false;
            attackCool = Player.wCon.curWeapon.attackCooltime;

            attack = true;
        }
        else attack = false;
        
        if (attackCool > 0) attackCool -= Time.deltaTime;
        else if (attackCool < 0) attackCool = 0;
    }
    void AttackInputCancel()
    {
        attackInput = false;
    }
}
