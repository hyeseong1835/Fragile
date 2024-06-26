﻿using JetBrains.Annotations;
using Sirenix.OdinInspector;
using System;
using System.Linq.Expressions;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public struct WeaponSaveData
{
    public string name;
    public int durability;
    public string weaponData;

    public WeaponSaveData(string _name, int _durability, string _weaponData = null)
    {
        name = _name;
        durability = _durability;
        weaponData = _weaponData;
    }
}
[Serializable]
public class ActiveSkill
{
    public float frontDelay = 0;
    public float delay = 1;
    public float backDelay = 0;

    public float minDistance = 0;
    public float maxDistance = 1;
}
/// <summary>
/// NULL, PREFAB, ITEM, HOLD, INVENTORY, REMOVED
/// </summary>
public enum WeaponState
{
    NULL, Item, Hold, Inventory, Removed
}
//[ExecuteAlways]
public class Weapon : MonoBehaviour
{
    #region 정적 멤버 - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -|
    public static Weapon Spawn(string weaponName, Transform parent = null)//-|
    {
        GameObject weaponObj;
        if (parent != null)
        {
            weaponObj = Instantiate(
            Resources.Load<GameObject>($"{weaponName}/{weaponName}"),
                parent
            );
        }
        else
        {
            weaponObj = Instantiate(
            Resources.Load<GameObject>($"{weaponName}/{weaponName}")
            );
        }
        weaponObj.name = weaponName;

        Weapon weapon = weaponObj.GetComponent<Weapon>();
        weapon.state = WeaponState.NULL;

        return weapon;
    }
#if UNITY_EDITOR
    public static Weapon PrefabLinkSpawn(string weaponName, Transform parent = null)//-|
    {
        GameObject weaponObj;
        if (parent != null)
        {
            weaponObj = (GameObject)PrefabUtility.InstantiatePrefab(
                Resources.Load<UnityEngine.Object>($"{weaponName}/{weaponName}")
                , parent
            );
        }
        else
        {
            weaponObj = (GameObject)PrefabUtility.InstantiatePrefab(
                Resources.Load<GameObject>($"{weaponName}/{weaponName}")
            );
        }
        weaponObj.name = weaponName;

        Weapon weapon = weaponObj.GetComponent<Weapon>();

        return weapon;
    }
#endif
    public static Weapon LoadWeapon(WeaponSaveData data)
    {
        Weapon weapon = Spawn(data.name);
        weapon.SetData(data);

        return weapon;
    }

    #endregion  - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -|

    [Required] public WeaponData data;
    
    [FoldoutGroup("Stat")]
    #region Foldout Stat - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -|                                         

        [HorizontalGroup("Stat/Durability")]
        #region Horizontal Durability

            [LabelWidth(Editor.propertyLabelWidth)]
            #if UNITY_EDITOR
            [ProgressBar(0, nameof(maxDurability), ColorGetter = nameof(durabilityColor))]
            #endif
            public int durability = 1;

            #if UNITY_EDITOR
                                                                                 [HorizontalGroup("Stat/Durability", Width = Editor.shortNoLabelPropertyWidth)]
            [ShowInInspector][HideLabel]
            [DelayedProperty]
            int maxDurability{
                get { 
                    if(data == null) return default;
                    return data.maxDurability; 
                }
                set {
                    if (durability == data.maxDurability || durability > value) durability = value;
                    data.maxDurability = value;
                }
            }

//          HideInInspector_____________________________________________________|
            Color durabilityColor {
                get {
                    if(data == null) return new Color(0.5f, 0.5f, 0.5f);

                    return new Color(1, 1, 1);
                }
            }

            #endif

        #endregion  - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -|    

    #endregion - - - - - - - - - - - - - - - - - - - - -|

    public Controller con;

    public WeaponState state = WeaponState.NULL;

    public Sprite UISprite;
    [Required] public Transform hand_obj;


    public string weaponName { get { return gameObject.name; } }

    void Awake()
    {

    }
    void Start()
    {

    }

    void Update()
    {
        UpdateEventNode.Trigger(gameObject);
    }

    #region 이벤트

    //무기
    protected virtual void OnUse()
    {
        if (hand_obj != null)
        {
            hand_obj.gameObject.SetActive(true);
            con.hand.HandLink(hand_obj, HandMode.ToHand);
        }
    }
    protected virtual void OnDeUse()
    {
        if (hand_obj != null)
        {
            hand_obj.gameObject.SetActive(false);
            con.hand.HandLink(null);
        }
    }
    public virtual void OnWeaponRemoved()
    {
        Destroy(hand_obj.gameObject);
        con.hand.HandLink(null);
    }
    protected virtual void OnWeaponDestroyed() { }

    #endregion

    #region 데이터

    public virtual WeaponSaveData GetData()
    {
        return new WeaponSaveData
            (
                weaponName,
                durability,
                null
            );
    }
    public virtual void SetData(WeaponSaveData data)
    {
        durability = data.durability;
    }

    #endregion

    #region 무기 관리

    public void SetUse(bool use)
    {
        if (use)
        {
            if (state != WeaponState.Hold)
            {
                OnUse();
                state = WeaponState.Hold;
            }
        }
        else
        {
            OnDeUse();
            state = WeaponState.Inventory;
        }
    }

    public bool AddDurability(int add)
    {
        durability += add;
        if (durability <= 0)
        {
            return true;
        }
        return false;
    }
    /// <summary>
    /// 무기를 파괴하는 함수
    /// INVENTORY -> HOLD -> REMOVED >> OnWeaponDestroy() >> Destroy(gameObject) //
    /// ITEM >> OnWeaponDestroy(item) >> Destroy //
    /// PREFAB >> DestroyImmediate(gameObject) //
    /// </summary>
    public void WeaponDestroy()
    {
        //PREFAB >> Destroy(gameObject) >> return
        if (state == WeaponState.NULL)
        {
            DestroyImmediate(gameObject);
            return;
        }

        //ITEM >> Destroy(parent) >> return
        if (state == WeaponState.Item)
        {
            transform.parent.gameObject.AutoDestroy();
            return;
        }

        //HOLD -> INVENTORY -> REMOVED ->
        if (state == WeaponState.Hold
            || state == WeaponState.Inventory)
        {
            con.RemoveWeapon(this);
        }

        // REMOVED >> OnWeaponDestroy(item) >> Destroy(gameObject) >> return
        if (state == WeaponState.Removed)
        {
            OnWeaponDestroyed();
            gameObject.AutoDestroy();
            return;
        }

        Debug.LogError("무기 상태가 유효하지 않습니다.");
    }

    #endregion
}
