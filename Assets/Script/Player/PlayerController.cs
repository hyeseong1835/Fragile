using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;

public enum AnimateState
{
    Stay, Move, Battle
}

public class PlayerController : Controller
{
    CameraController camCon;

    #region 입력

    //마우스
    [HideInInspector] public Vector2 mousePos { get { return Input.mousePosition; } }
    [HideInInspector] public Vector2 playerToMouse { get { return (camCon.cam.ScreenToWorldPoint(mousePos) - transform.position); } }
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

    #endregion


    //int lastWeaponIndex;

    [SerializeField][ReadOnly] bool attackInput = false;
    float attackInputAllowTime = 1;
    Coroutine curAttackInputCoroutine;
    public float attackCool = 0;
    public float moveSpeed = 1;

    void Update()
    {
        if (mouse0Down) curWeapon.Mouse0Down();
        if (mouse0Stay) curWeapon.Mouse0();
        if (mouse0Up) curWeapon.Mouse0Up();
        if (mouse1Down) curWeapon.Mouse1Down();
        if (mouse1) curWeapon.Mouse1();
        if (mouse1Up) curWeapon.Mouse1Up();

        if (attack) Attack();

        Mouse();
        Move();
        Attack();

        WheelSelect();
        if (Input.GetKeyDown(KeyCode.P)) AddWeapon(Utility.LoadWeapon("WoodenSword"));
    }
    void Mouse()
    {
        targetPos = camCon.cam.ScreenToWorldPoint(mousePos);
    }
    void Move()
    {
        prevMoveVector = moveVector;
        moveVector = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);

        transform.position += moveVector.normalized * Time.deltaTime * moveSpeed;
        if (moveVector.magnitude > Mathf.Epsilon) moveRotate = Utility.Vector2ToRotate(moveVector);
               
    }
    void WheelSelect()
    {
        if (mouseWheelClickDown) SelectWeapon(0);

        //입력 없음 >> return
        if (mouseWheelScroll == 0) return;

        //맨손일 때 >> 마지막 무기 선택 >> return
        /*if (curWeapon.index == 0) 
        {
            if (transform.childCount == 1) //무기가 없을 때
            {
                if (curWeapon.index != 0) SelectWeapon(0);
                return;
            }
            if (lastWeaponIndex != 0) //마지막으로 들었던 무기가 있을 때
            {
                SelectWeapon(lastWeaponIndex); //마지막으로 들었던 무기
                return;
            }
        }
        */

        // 휠+
        if (mouseWheelScroll > 0) 
        {
            if (curWeapon.index != 0 && curWeapon.index == transform.childCount - 1) //마지막 순서의 무기일 때
            {
                SelectWeapon(1); //첫 번째 무기
            }
            else SelectWeapon(curWeapon.index + 1); // +1

            return;
        }
        // 휠-
        else
        {
            if (curWeapon.index <= 1) //맨손이거나 첫 번째 순서의 무기일 때
            {
                SelectWeapon(transform.childCount - 1);
            }
            else SelectWeapon(curWeapon.index - 1); // -1

            return;
        }
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
            attackCool = curWeapon.attackCooltime;

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
