using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    public Vector2 targetPos;
    public Vector3 moveVector;
    [HideInInspector] public Vector3 prevMoveVector;

    public float moveRotate = 0;
    //ют╥б
    public bool attack = false;
    public bool special = false;
}
