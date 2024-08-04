using Sirenix.OdinInspector;
using Sirenix.Utilities;
using System;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : Controller
{
    public static PlayerController instance { get; private set; }
    
    [HideInInspector]
    public PlayerControllerData data;
    public override ControllerData ControllerData
    {
        get => data; 
        set => data = (PlayerControllerData)value;
    }
    public override Type DataType => typeof(PlayerControllerData);
    public CameraController camCon => CameraController.instance;

    public enum BehaviorState
    {
        //Default
        Idle, Move,

        //Attack
        ChargeAttack, Attack,

        //Special
        ChargeSpecial, Special
    }
    public BehaviorState behaviorState;

    #region Mouse

        public Vector2 mousePos => camCon.cam.ScreenToWorldPoint(Input.mousePosition);

        public Vector2 playerToMouse => mousePos - (Vector2)transform.position; 

        float viewRotateZ => Mathf.Atan2(playerToMouse.y, playerToMouse.x) * Mathf.Rad2Deg; 

        #region 좌클릭

            bool mouse0Down => UnityEngine.Input.GetMouseButtonDown(0); 

            bool mouse0Stay => UnityEngine.Input.GetMouseButton(0);

            bool mouse0Up => UnityEngine.Input.GetMouseButtonUp(0);

        #endregion

        #region 우클릭

            public bool mouse1Down => UnityEngine.Input.GetMouseButtonDown(1);

            [ShowInInspector]
            public bool mouse1Stay => UnityEngine.Input.GetMouseButton(1);

            public bool mouse1Up => UnityEngine.Input.GetMouseButtonUp(1);

        #endregion

        #region 마우스 휠

            public float mouseWheelScroll => UnityEngine.Input.GetAxis("Mouse ScrollWheel");

            public bool mouseWheelClickDown=> UnityEngine.Input.GetMouseButtonDown(2);

            [ShowInInspector]
            public bool mouseWheelClick => UnityEngine.Input.GetMouseButtonUp(2);

            public bool mouseWheelClickUp => UnityEngine.Input.GetMouseButtonDown(2);

        #endregion

    #endregion

    [NonSerialized]
    public Weapon nextWeapon;

    [NonSerialized]
    public Weapon lastNotHandWeapon;

    public GameObject dummyPrefab;

    protected void Awake()
    {
        instance = this;
    }
    new protected void Update()
    {
        base.Update();
        
        if (preventAttackInput == false) attack = mouse0Stay;
        else if (mouse0Stay == false) attack = false;

        if (preventSpecialInput == false) special = mouse1Stay;
        else if (mouse1Stay == false) special = false;

        InputEventTrigger();
        
        Mouse();
        Move();

        WheelSelect();
        if (Input.GetKeyDown(KeyCode.P)) Instantiate(dummyPrefab);

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
        if (UnityEngine.Input.GetButton("Horizontal") || UnityEngine.Input.GetButton("Vertical"))
        {
            moveVector = new Vector3(UnityEngine.Input.GetAxis("Horizontal"), UnityEngine.Input.GetAxis("Vertical"), 0);

            transform.position += (Vector3)moveVector.normalized * Time.deltaTime * data.moveSpeed;

            animationType = AnimationType.Move;
            moveAnimationType = MoveAnimationType.Walk;
        }
        //입력이 없을 때
        else
        {
            moveVector = Vector2.zero;

            animationType = AnimationType.Move;
            moveAnimationType = MoveAnimationType.Stay;
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
}
