using System;
using UnityEngine;
using System.Linq;
using UnityEngine.Events;
using WeaponSystem;

public class TriggerObject : MonoBehaviour
{
    public Controller con;
    IController[] selfEnter,   selfStay,   selfExit;
    IController[] friendEnter, friendStay, friendExit;
    IController[] enemyEnter,  enemyStay,  enemyExit;
    IController[] objectEnter, objectStay, objectExit;

    public void Init(
        Controller _con,
        IController[] _selfEnter,   IController[] _selfStay,   IController[] _selfExit,
        IController[] _friendEnter, IController[] _friendStay, IController[] _friendExit,
        IController[] _enemyEnter,  IController[] _enemyStay,  IController[] _enemyExit,
        IController[] _objectEnter, IController[] _objectStay, IController[] _objectExit)
    {
        con = _con;
        selfEnter = _selfEnter;     selfStay = _selfStay;     selfExit = _selfExit;
        friendEnter = _friendEnter; friendStay = _friendStay; friendExit = _friendExit;
        enemyEnter = _enemyEnter;   enemyStay = _enemyStay;   enemyExit = _enemyExit;
        objectEnter = _objectEnter; objectStay = _objectStay; objectExit = _objectExit;
    }
    void OnTriggerEnter2D(Collider2D coll)
    {
        SendEvent(coll, selfEnter, friendEnter, enemyEnter, objectEnter);

    }
    void OnTriggerStay2D(Collider2D coll)
    {
        SendEvent(coll, selfStay, friendStay, enemyStay, objectStay);
    }
    void OnTriggerExit2D(Collider2D coll)
    {
        SendEvent(coll, selfExit, friendExit, enemyExit, objectExit);
    }
    void SendEvent(Collider2D coll, IController[] selfEvent, IController[] friendEvent, IController[] enemyEvent, IController[] objectEvent)
    {
        Controller hitCon = coll.GetComponent<Controller>();
        if (hitCon == null)
        {
            IController.Invoke(objectEvent, hitCon);
        }
        else if (hitCon == con)
        {
            IController.Invoke(selfEvent, hitCon);
        }
        else if (hitCon.gameObject.layer == con.gameObject.layer)
        {
            IController.Invoke(friendEvent, hitCon);
        }
        else
        {
            IController.Invoke(enemyEvent, hitCon);
        }
    }
}
