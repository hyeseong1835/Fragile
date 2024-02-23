using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public enum AnimateState
{
    Stay, Move, Battle
}

public class PlayerController : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] WeaponController WCon;
    float hMove = 0;
    float vMove = 0;
    [HideInInspector] public Vector3 moveVector = new Vector3(0, 0, 0);
    [HideInInspector] public float curMoveSpeed = 0;
    public float moveRotate;
    /// <summary>
    /// 0(90), 1(45), 2(0), 3(315), 4(270), 5(225), 6(180), 7(135)
    /// </summary>
    public int moveIntRotate;

    //마우스
    [HideInInspector] public Vector3 mousePos = Vector3.zero;
    [HideInInspector] public float viewRotateZ = 0;

    [HideInInspector] public float mouseWheelScroll = 0;
    [HideInInspector] public bool mouseWheelClickDown = false;
    [HideInInspector] public bool mouseWheelClick = false;
    [HideInInspector] public bool mouseWheelClickUp = false;

    [HideInInspector] public bool mouse0Down = false;
    [HideInInspector] public bool mouse0 = false;
    [HideInInspector] public bool mouse0Up = false;

    [HideInInspector] public bool mouse1Down = false;
    [HideInInspector] public bool mouse1 = false;
    [HideInInspector] public bool mouse1Up = false;

    //기타
    [HideInInspector] public bool isAttack = false;
    [HideInInspector] public bool attackDown = false;
    [HideInInspector] public bool attack = false;
    [HideInInspector] public bool attackUp = false;

    public float moveSpeed = 1;
    // Start is called before the first frame update
    void Awake()
    {

    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Key();
        Move();
    }
    void Key()
    {
        //이동
        hMove = Input.GetAxis("Horizontal");
        vMove = Input.GetAxis("Vertical");
        moveVector = new Vector3(hMove,vMove);
        if (moveVector.magnitude > 0.1f)
        {
            moveRotate = Mathf.Atan2(moveVector.y, moveVector.x) * Mathf.Rad2Deg;
            if (moveRotate < 0) moveRotate += 360;

            if (moveRotate < 22.5) moveIntRotate = 0;
            else if (moveRotate < 67.5) moveIntRotate = 1;
            else if (moveRotate < 112.5) moveIntRotate = 2;
            else if (moveRotate < 157.5) moveIntRotate = 3;
            else if (moveRotate < 202.5) moveIntRotate = 4;
            else if (moveRotate < 247.5) moveIntRotate = 5;
            else if (moveRotate < 292.5) moveIntRotate = 6;
            else if (moveRotate < 337.5) moveIntRotate = 7;
            else moveIntRotate = 0;
        }

        //마우스
        mousePos = Input.mousePosition;
        viewRotateZ = Mathf.Atan2(player.cam.ScreenToWorldPoint(mousePos).y - player.transform.position.y,
            player.cam.ScreenToWorldPoint(mousePos).x - player.transform.position.x) * Mathf.Rad2Deg - 90;

        mouseWheelScroll = Input.GetAxis("Mouse ScrollWheel");
        mouseWheelClickDown = Input.GetMouseButtonDown(2);
        mouseWheelClick = Input.GetMouseButton(2);
        mouseWheelClickUp = Input.GetMouseButtonUp(2);

        mouse0Down = Input.GetMouseButtonDown(0);
        mouse0 = Input.GetMouseButton(0);
        mouse0Up = Input.GetMouseButtonUp(0);

        mouse1Down = Input.GetMouseButtonDown(1);
        mouse1 = Input.GetMouseButton(1);
        mouse1Up = Input.GetMouseButtonUp(1);

        //기타

        //Attack
        if (mouse0Down)
        {
            if (WCon.curWeapon.attackCool < 0.5f && WCon.curWeapon.attackStack == 0) WCon.curWeapon.attackStack++;
        }
        if ((mouse0Down || WCon.curWeapon.attackStack > 0) && WCon.curWeapon.attackCool == 0)
        {
            StartCoroutine(AttackDown());
        }
    }
    IEnumerator AttackDown()
    {
        WCon.curWeapon.attackStack--;
        WCon.curWeapon.attackCool = WCon.curWeapon.attackCooltime;
        
        attackDown = true;
        yield return null;
        attackDown = false;
        
        attack = true;
        while (isAttack)
        {
            yield return null;
        }
        attack = false;

        attackUp = true;
        yield return null;
        attackUp = false;
    }
    void Move()
    {
        transform.position += moveVector.normalized * Time.deltaTime * moveSpeed;
        curMoveSpeed = moveVector.magnitude * moveSpeed;
    }
}
