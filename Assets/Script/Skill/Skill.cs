using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skill : MonoBehaviour
{
    protected Transform pTransform;

    public Weapon weapon;

    void Awake()
    {
        pTransform = transform.parent.parent;
    }
}