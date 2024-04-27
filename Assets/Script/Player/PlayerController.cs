using Sirenix.OdinInspector;
using System.Collections;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public enum AnimateState
{
    Stay, Move, Battle
}

public class PlayerController : Controller
{
    [SerializeField]
        [Required]
        CameraController camCon;

    #region 입력

        #region 마우스
    
            [HideInInspector] 
                public Vector2 mousePos { get { return camCon.cam.ScreenToWorldPoint(Input.mousePosition); } }
    
            [HideInInspector] 
                public Vector2 playerToMouse { get { return mousePos - (Vector2)transform.position; } }
    
            [HideInInspector] 
                public float viewRotateZ { get { return Mathf.Atan2(playerToMouse.y, playerToMouse.x) * Mathf.Rad2Deg; } }

            #region 좌클릭

                [HideInInspector] 
                    public bool mouse0Down { get { return Input.GetMouseButtonDown(0); } }
    
                [HideInInspector] 
                    public bool mouse0Stay { get { return Input.GetMouseButton(0); } }
    
                [HideInInspector] 
                    public bool mouse0Up { get { return Input.GetMouseButtonUp(0); } }
    
            #endregion
    
            #region 우클릭

                [HideInInspector] 
                    public bool mouse1Down { get { return Input.GetMouseButtonDown(1); } }
    
                [HideInInspector] 
                    public bool mouse1 { get { return Input.GetMouseButton(1); } }
    
                [HideInInspector] 
                    public bool mouse1Up { get { return Input.GetMouseButtonUp(1); } }
    
            #endregion
    
            #region 마우스 휠

                [HideInInspector] 
                    public float mouseWheelScroll { get { return Input.GetAxis("Mouse ScrollWheel"); } }
    
                [HideInInspector] 
                    public bool mouseWheelClickDown { get { return Input.GetMouseButtonDown(2); } }
    
                [HideInInspector] 
                    public bool mouseWheelClick{ get { return Input.GetMouseButtonUp(2); } }
    
                [HideInInspector] 
                    public bool mouseWheelClickUp { get { return Input.GetMouseButtonDown(2); } }

            #endregion

        #endregion
    
    #endregion

    Weapon lastNotHandWeapon;

    [SerializeField][ReadOnly] bool attackInput = false;
    [SerializeField] float attackInputAllowTime = 1;
    Coroutine curAttackInputCoroutine;

    #if UNITY_EDITOR
    float attackCoolMax
    {
        get
        {
            if (curWeapon == null)
            {
                return 0;
            }
            else return curWeapon.attackCooltime;
        }
    }
    #endif
        [PropertyRange(0, "attackCoolMax")] public float attackCool = 0;

    public float moveSpeed = 1;

    void Awake()
    {

    }
    void Update()
    {
        if (EditorApplication.isPlaying == false) return;

        if (curWeapon != null)
        {
            if (mouse0Down) curWeapon.Mouse0Down();
            if (mouse0Stay) curWeapon.Mouse0();
            if (mouse0Up) curWeapon.Mouse0Up();
            if (mouse1Down) curWeapon.Mouse1Down();
            if (mouse1) curWeapon.Mouse1();
            if (mouse1Up) curWeapon.Mouse1Up();

            Attack();
            if (attack) curWeapon.Attack();
        }

        Mouse();
        Move();

        WheelSelect();
        if (Input.GetKeyDown(KeyCode.P)) AddWeapon(Utility.SpawnWeapon("WoodenSword"));
    }
    void Mouse()
    {
        targetPos = mousePos;
    }
    void Move()
    {
        moveVector = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);

        transform.position += (Vector3)moveVector.normalized * Time.deltaTime * moveSpeed;
        if (moveVector.magnitude > Mathf.Epsilon)
        {
            lastMoveVector = moveVector;
            moveRotate = Utility.Vector2ToRotate(moveVector);
        }      
    }
    void WheelSelect()
    {
        // 휠 클릭 >> 현재 무기 선택
        if (mouseWheelClickDown) SelectWeapon(defaultWeapon);

        // 입력 없음 >> return
        if (mouseWheelScroll == 0) return;

        // 맨손일 때 >> 마지막 무기 선택 >> return
        if (curWeapon == defaultWeapon)
        {
            //마지막으로 들었던 무기가 남아있으면 >> 선택 >> return
            if (weapons.Contains(lastNotHandWeapon))
            {
                SelectWeapon(lastNotHandWeapon);
                return;
            }
            //없으면 >> 첫 번째 무기 선택 >> return
            else
            {
                if (weapons.Count != 0) SelectWeapon(weapons[0]);
                return;
            }
        }

        int curWeaponIndex = weapons.IndexOf(curWeapon);
        // 휠+
        if (mouseWheelScroll > 0)
        {
            if (curWeaponIndex == weapons.Count - 1) // 마지막 순서의 무기일 때
            {
                SelectWeapon(weapons[0]); // 첫 번째 무기
            }
            else SelectWeapon(weapons[curWeaponIndex + 1]); // +1
        }
        // 휠-
        else
        {
            if (weapons.IndexOf(curWeapon) == 0) // 첫 번째 순서의 무기일 때
            {
                SelectWeapon(weapons[weapons.Count - 1]);
            }
            else SelectWeapon(weapons[curWeaponIndex - 1]); // -1
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
            attack = true;

            attackCool = curWeapon.attackCooltime;
            attackInput = false;
        }
        else attack = false;
        
        if (attackCool > 0) attackCool -= Time.deltaTime;
        if (attackCool < 0) attackCool = 0;
    }
    void AttackInputCancel()
    {
        attackInput = false;
    }
}
