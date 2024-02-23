using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum Curve
{
    Linear, Quadratic
}

public abstract class Skill : MonoBehaviour
{
    protected Transform pTransform;
    protected Player player;
    protected PlayerController pCon;
    protected WeaponController wCon;

    [SerializeField] protected Weapon weapon;

    void Awake()
    {
        //변수 세팅
        pTransform = transform.parent.parent;
        player = pTransform.GetComponent<Player>();
        pCon = pTransform.GetComponent<PlayerController>();
        wCon = pTransform.GetComponent<WeaponController>();
    }

    /*
    protected Weapon weapon;
    protected Transform playerTransform;
    protected Player player;
    protected PlayerController playerCon;

    void Awake()
    {
        weapon = GetComponent<Weapon>();
        playerTransform = transform.parent.parent;
        player = playerTransform.GetComponent<Player>();
        playerCon = playerTransform.GetComponent<PlayerController>();
    }
    void Update()
    {
        if (!weapon.isUsing) return;

        //기본
        if (playerCon.mouse0Down) Mouse0Down();
        if (playerCon.mouse0) Mouse0();
        if (playerCon.mouse0Up) Mouse0Up();
        if (playerCon.mouse1Down) Mouse1Down();
        if (playerCon.mouse1) Mouse1();
        if (playerCon.mouse1Up) Mouse1Up();

        if (playerCon.attackDown) AttackDown();
        if (playerCon.attack) Attack();
        if (playerCon.attackUp) AttackUp();
    }

    public virtual void AttackDown() { }
    public virtual void Attack() { }
    public virtual void AttackUp() { }
    public virtual void Mouse0Down() { }
    public virtual void Mouse0() { }
    public virtual void Mouse0Up() { }
    public virtual void Mouse1Down() { }
    public virtual void Mouse1() { }
    public virtual void Mouse1Up() { }
    */
}