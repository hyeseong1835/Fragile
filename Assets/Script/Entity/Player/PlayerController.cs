using Sirenix.OdinInspector;
using System;
using UnityEngine;

[ExecuteAlways]
public class PlayerController : Controller
{
    [Space(Editor.overrideSpace)]
    [BoxGroup("Object")]
    #region Override Box Object  - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -| 

    [Required][ChildGameObjectsOnly][PropertyOrder(0)]
        [LabelText("Player Grafic")][LabelWidth(Editor.propertyLabelWidth - Editor.childGameObjectOnlyWidth)]//-|
        public PlayerGrafic grafic;
                                                                                [BoxGroup("Object")]
        [Required][PropertyOrder(0)]
        [LabelText("Cam Controller")][LabelWidth(Editor.propertyLabelWidth)]
        public CameraController camCon;

    #endregion - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -| 

    [Space(Editor.overrideSpace)]
    [FoldoutGroup("Input")]
    #region Override Input  - - - - - - - - - - - - - - - - - - - - - - - - - - - -|
        [VerticalGroup("Input/Attack")]
        #region Override Foldout Attack - - - - - - - - - - - - - - - - - - - -|

            [SerializeField][ReadOnly]
            [LabelWidth(Editor.propertyLabelWidth)]
            bool attackInput = false;   
                                                                                [VerticalGroup("Input/Attack")]      
            [SerializeField] 
            [LabelText("Allow Time")][LabelWidth(Editor.propertyLabelWidth)]//-|
            float attackInputAllowTime = 1;

            Coroutine curAttackInputCoroutine;

    #endregion  - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -|

    #region Mouse

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

    #endregion  - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -|

    [HideInInspector]
    public Weapon nextWeapon;

    Weapon lastNotHandWeapon;


    void Awake()
    {

    }
    void Update()
    {
        if (Editor.GetType(Editor.StateType.IsPlay))
        {
            if (curWeapon != null)
            {
                Attack();

                if (attack) curWeapon.Attack();
                if (mouse1Down) curWeapon.Special();
            }

            Mouse();
            Move();

            WheelSelect();
            if (Input.GetKeyDown(KeyCode.P)) AddWeapon(Weapon.SpawnWeapon("WoodenSword"));
        }

        int curWeaponIndex = weapons.IndexOf(curWeapon);

        if (curWeaponIndex == weapons.Count - 1) //마지막 순서의 무기일 때
        {
            if (weapons.Count < 2) //무기가 1개 이하
            {
                if (defaultWeapon != null)
                {
                    if (curWeapon == defaultWeapon) nextWeapon = null;
                    else nextWeapon = defaultWeapon;
                }
            }
            else nextWeapon = weapons[0];
        }
        else
        {
            nextWeapon = weapons[curWeaponIndex + 1];
        }
    }
    void Mouse()
    {
        targetPos = mousePos;
    }
    void Move()
    {
        //입력이 있을 때
        if (Input.GetButton("Horizontal") || Input.GetButton("Vertical"))
        {
            //처음
            if (grafic.animationState == PlayerAnimationState.STAY)
            {
                grafic.animationState = PlayerAnimationState.WALK;
            }
            //계속
            else
            {
                moveVector = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
                moveRotate = Utility.Vector2ToDegree(moveVector);

                transform.position += (Vector3)moveVector.normalized * Time.deltaTime * moveSpeed;
            }
        }
        //입력이 없을 때
        else
        {
            //처음
            if (grafic.animationState == PlayerAnimationState.WALK)
            {
                grafic.animationState = PlayerAnimationState.STAY;

                lastMoveVector = moveVector;
                lastMoveRotate = Utility.Vector2ToDegree(moveVector);
                moveVector = Vector2.zero;

            }
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

            attackCool = curWeapon.attackFrontDelay + curWeapon.attackDelay + curWeapon.attackBackDelay;
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
