using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public enum AnimateState
{
    Stay, Move, Battle
}

[RequireComponent(typeof(Player))]
public class PlayerController : MonoBehaviour
{
    [HideInInspector] public Vector3 moveVector
    {
        get { return new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0}; }
        set { if (moveVector.magnitude > 0.1f) moveRotate = Mathf.Atan2(moveVector.y, moveVector.x) * Mathf.Rad2Deg; }
    }
    [HideInInspector] public float curMoveSpeed
    {
        get { moveVector.magnitude * moveSpeed }
    }
    [HideInInspector] public float moveRotate;
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
    [HideInInspector]
    public Vector3 mousePos
    {
        get { return Input.mousePosition; }
    }
    [HideInInspector]
    public float viewRotateZ
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
    [HideInInspector]
    public float mouseWheelScroll
    {
        get { return Input.GetAxis("Mouse ScrollWheel"); }
    }
    [HideInInspector]
    public bool mouseWheelClickDown
    {
        get { return Input.GetMouseButtonDown(2); }
    }
    [HideInInspector]
    public bool mouseWheelClick
    {
        get { return Input.GetMouseButtonUp(2); }
    }
    [HideInInspector]
    public bool mouseWheelClickUp
    {
        get { return Input.GetMouseButtonDown(2); }
    }

    //기타
    [HideInInspector] 
    public bool attackDown
    {
        get
        {
            if (mouse0Down)
            {
               if (attackStack == 0) attackStack++;
            }
            if ((attackStack > 0) && attackCool == 0)
            {
                attackStack--;
                attackCool = attackCooltime;
        
                StartCoroutine(AttackDown());
            }
        }
    }
    public float moveSpeed = 1;
    
    void Update()
    {                                                                                                                                                                                                                                                                                                       
        Move();
    }
    IEnumerator AttackDown()
    {
        attackDown = true;
        yield return null;
        attackDown = false;
    }
    IEnumerator AttackInput
    {
        
    }
    void Move()
    {
        transform.position += moveVector.normalized * Time.deltaTime * moveSpeed;
        curMoveSpeed = moveVector.magnitude * moveSpeed;
    }
}
