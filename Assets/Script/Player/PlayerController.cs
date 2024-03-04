using System.Collections;
using UnityEngine;

public enum AnimateState
{
    Stay, Move, Battle
}

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [HideInInspector] public Vector3 moveVector
    {
        get { return new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0); }
    } 
    [HideInInspector] public float moveRotate = 0;
    /// <summary>
    /// 0(90), 1(45), 2(0), 3(315), 4(270), 5(225), 6(180), 7(135)
    /// </summary>
    [HideInInspector] public int moveIntRotate
    {
        get
        {
            if (moveRotate < 0) moveRotate += 360;

            if (moveRotate < 22.5) return 0;
            else if (moveRotate < 67.5) return 1;
            else if (moveRotate < 112.5) return 2;
            else if (moveRotate < 157.5) return 3;
            else if (moveRotate < 202.5) return 4;
            else if (moveRotate < 247.5) return 5;
            else if (moveRotate < 292.5) return 6;
            else if (moveRotate < 337.5) return 7;
            else return 0;
        }
    }

    //마우스
    [HideInInspector] public Vector3 mousePos
    {
        get { return Input.mousePosition; }
    }
    [HideInInspector] public float viewRotateZ
    {
        get
        {
            return Mathf.Atan2(Player.cam.ScreenToWorldPoint(mousePos).y - transform.position.y,
                Player.cam.ScreenToWorldPoint(mousePos).x - transform.position.x) * Mathf.Rad2Deg - 90;
        }
    }

    //좌클릭
    [HideInInspector] public bool mouse0Down
    {
        get { return Input.GetMouseButtonDown(0); }
    }
    [HideInInspector] public bool mouse0
    {
        get { return Input.GetMouseButton(0); }
    }
    [HideInInspector] public bool mouse0Up
    {
        get { return Input.GetMouseButtonUp(0); }
    }

    //우클릭
    [HideInInspector] public bool mouse1Down
        {
        get { return Input.GetMouseButtonDown(1); }
    }
    [HideInInspector] public bool mouse1
    {
        get { return Input.GetMouseButton(1); }
    }
    [HideInInspector] public bool mouse1Up
    {
        get { return Input.GetMouseButtonUp(1); }
    }

    //마우스 휠
    [HideInInspector] public float mouseWheelScroll
    {
        get { return Input.GetAxis("Mouse ScrollWheel"); }
    }
    [HideInInspector] public bool mouseWheelClickDown
    {
        get { return Input.GetMouseButtonDown(2); }
    }
    [HideInInspector] public bool mouseWheelClick
    {
        get { return Input.GetMouseButtonUp(2); }
    }
    [HideInInspector] public bool mouseWheelClickUp
    {
        get { return Input.GetMouseButtonDown(2); }
    }

    //기타
    bool attackInput;
    float attackInputAllowTime;
    Coroutine curAttackInputCoroutine;
    public float attackCool;
    [HideInInspector] public bool attack
    {
        get
        {
            if ((attackInput) && attackCool == 0)
            {
                attackInput = false;
                attackCool = Player.wCon.curWeapon.attackCooltime;
        
                return true;
            }
            else return false;
        }
    }
    public float moveSpeed = 1;

    void Awake()
    {
        Player.pCon = this;
        Player.transform = transform;
        Player.gameObject = gameObject;
        Player.cam = Camera.main;
    }
    void Update()
    {                                                                                                                                                                                                                                                                                                       
        Move();
        Attack();
    }
    void Move()
    {
        transform.position += moveVector.normalized * Time.deltaTime * moveSpeed;
        if (moveVector.magnitude > 0.1f) moveRotate = Mathf.Atan2(moveVector.y, moveVector.x) * Mathf.Rad2Deg;
    }
    void Attack()
    {
        if (mouse0Down)
        {
            if (curAttackInputCoroutine != null) StopCoroutine(curAttackInputCoroutine);
            curAttackInputCoroutine = StartCoroutine(AttackInput());
        }
        if (attackCool > 0) attackCool -= Time.deltaTime;
        else if (attackCool < 0) attackCool = 0;
    }
    IEnumerator AttackInput()
    {
        attackInput = true;
        yield return new WaitForSeconds(attackInputAllowTime);
        attackInput = false;
    }
}
