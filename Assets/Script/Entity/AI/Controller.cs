using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    public Vector2 targetPos;
    public Vector3 moveVector;
    public float moveRotate;
    /// <summary>
    /// 0(90), 1(45), 2(0), 3(315), 4(270), 5(225), 6(180), 7(135)
    /// </summary>
    public int moveIntRotate
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
    //ют╥б
    public bool attack = false;
    public bool special = false;
}
