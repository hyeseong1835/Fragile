using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.XR;
using static UnityEngine.Rendering.DebugUI;

public enum AnimationType
{
    Move, Attack, Special
}
public enum MoveAnimationType
{
    Stay, Walk, Jump, Dash, Charge
}
public enum SkillAnimationType
{
    Swing, Shoot, Throw, Cast, Summon
}
public enum ReadLineType
{
    Custom, One, Rotation4X
}
public struct ControllerSave
{
    //좌표
    public Vector2 pos;
    //기본 무기(데이터)
    public WeaponSaveData defaultWeaponData;
    //현재 무기(인덱스)
    public int curWeaponIndex;
    //인벤토리(무기(데이터))
    public WeaponSaveData[] weaponDatas;

    public ControllerSave(
        Vector2 _pos,
        WeaponSaveData _defaultWeaponData,
        WeaponSaveData[] _weaponDatas,
        int _curWeaponIndex
        )
    {
        pos = _pos;
        defaultWeaponData = _defaultWeaponData;
        weaponDatas = _weaponDatas;
        curWeaponIndex = _curWeaponIndex;
    }
}
public abstract class Controller : Entity
{
    public const string WEAPONHOLDER_NAME = "WeaponHolder";

    [FoldoutGroup("Stat")]
    #region Foldout Stat - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -|                                         

    [ShowInInspector]
    [LabelWidth(Editor.labelWidth)]
    public abstract Vector2 Center { get; set; }
                                                                                        [FoldoutGroup("Stat")]
    [ShowInInspector]
    [LabelWidth(Editor.labelWidth)]
    public abstract int InventorySize { get; set; }

    #endregion

    [BoxGroup("Object")]
    #region Foldout Object - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -|

        [HorizontalGroup("Object/Hand", Order = 1)]
        #region Horizontal Hand

            [NonSerialized][ReadOnly][ShowInInspector][PropertyOrder(0)]
            [LabelText("Hand")][LabelWidth(Editor.labelWidth)]//-|
            public HandGrafic hand;

            #if UNITY_EDITOR
            [HorizontalGroup("Object/Hand", Width = Editor.shortButtonWidth)]
            [Button("Create")]
            public void CreateHand() => HandGrafic.SetHandGrafic(this);
            #endif

        #endregion

        [HorizontalGroup("Object/WeaponHolder", Order = 2)]
        #region Horizontal WeaponHolder

            [NonSerialized][ReadOnly][ShowInInspector][PropertyOrder(0)]
            [LabelWidth(Editor.labelWidth)]
            public Transform weaponHolder;

            #if UNITY_EDITOR
            [HorizontalGroup("Object/WeaponHolder", Width = Editor.shortButtonWidth)]
            [Button("Create")]
            public void CreateWeaponHolder()
            {
                Transform weaponHolder = transform.Find(WEAPONHOLDER_NAME);
                if(weaponHolder != null)
                {
                    if (weaponHolder.transform.parent != transform)
                    {
                        weaponHolder.transform.parent = transform;
                        Debug.LogWarning("WeaponHolder parent must be this");
                    }
                    if (weaponHolder.localPosition != Vector3.zero)
                    {
                        weaponHolder.localPosition = Vector3.zero;
                        Debug.LogWarning("WeaponHolder localPosition must be zero");
                    }
                    Debug.Log("Set WeaponHolder");
                    return;
                }
                weaponHolder = new GameObject("WeaponHolder").transform;
                weaponHolder.parent = transform;
                weaponHolder.localPosition = Vector3.zero;
                Debug.Log("Create WeaponHolder");
            }
            #endif

        #endregion

    #endregion - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -|

    [FoldoutGroup("Input")]
    #region Foldout Input  - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -|

    [VerticalGroup("Input/Move")]
    #region Vertical Move - - - - - - - - - - - - - - - - - - - - - - - - - - - -|

        [ReadOnly][ShowInInspector]
        [LabelWidth(Editor.labelWidth)]
        public Vector2 lastMoveVector { get; private set; } = new Vector2(0.5f, 0);
                                                                                                  [VerticalGroup("Input/Move")]
        [NonSerialized]
        public Vector2 moveVector = new Vector2(0.5f, 0);

        [ReadOnly][ShowInInspector]
        [LabelWidth(Editor.labelWidth)]
        public float moveRotate { get; private set; }

        public int moveRotate4 { get; private set; }

        public int moveRotate8 { get; private set; }

    #endregion  - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -|

        [VerticalGroup("Input/Target")]
        #region Vertical Target  - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -|

            [NonSerialized]
            [LabelWidth(Editor.labelWidth)]
            public Vector2 targetPos;
                                                                                                        [VerticalGroup("Input/Target")]
            [ShowInInspector]
            [LabelWidth(Editor.labelWidth)]
            public Vector2 targetDir => targetPos - ((Vector2)transform.position + Center).normalized; 

        #endregion - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -|

        [VerticalGroup("Input/Attack")]
        #region Vertical Attack - - - - - - - - - - - - - - - - -|


            [ReadOnly] public bool attack = false;
            [NonSerialized] public bool prevAttack = false;

            [NonSerialized] float attackCharge;
            [NonSerialized] public bool preventAttackInput = false;
            [NonSerialized] public bool readyPreventAttackInput = false;


        #endregion  - - - - - - - - - - - - - - - - - - - - - - -|

        [VerticalGroup("Input/Special")]
        #region Vertical Special - - - - - - - - - - - - - - - - -|

            [ReadOnly] public bool special;
            [NonSerialized] public bool prevSpecial = false;

            [NonSerialized] float specialCharge;
            [HideInInspector] public bool isSpecial { get; private set; } = false;
            [NonSerialized] public bool preventSpecialInput = false;
            [NonSerialized] public bool readyPreventSpecialInput = false;

        #endregion  - - - - - - - - - - - - - - - - - - - - - - -|   

    #endregion - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -|

    [FoldoutGroup("Weapon")]
    #region Foldout Weapon - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -|     

        [VerticalGroup("Weapon/DefaultWeapon")]
        [HorizontalGroup("Weapon/DefaultWeapon/Horizontal")]
        #region Horizontal DefaultWeapon - - - - - - - - - - - - - - - - - - - - - - - - - - -|

            [VerticalGroup("Weapon/DefaultWeapon/Horizontal/Vertical", PaddingBottom = 25)]//-|
            [ShowInInspector][ReadOnly][NonSerialized]
            [LabelWidth(Editor.labelWidth)]
            public Weapon defaultWeapon;

            #if UNITY_EDITOR
                                                                                                    [HorizontalGroup("Weapon/DefaultWeapon/Horizontal", width: Editor.shortButtonWidth)]
            [DisableInEditorMode]
            [Button(name: "Set", Expanded = true)]
            void SetDefaultWeapon(int index)
            {
                // 제거(-1)
                if (index == -1)
                {
                    if (curWeapon == defaultWeapon)
                    {
                        curWeapon = null;
                    }
                    weaponList.Add(defaultWeapon);
                    defaultWeapon = null;
                }
                else
                {
                    Weapon weapon = weaponHolder.GetChild(index).GetComponent<Weapon>();

                    //현재 무기가 없으면 자동 선택
                    if (curWeapon == null)
                    {
                        curWeapon = weapon;
                        SelectWeapon(weapon);
                    }

                    //인벤토리에서 자동 제거
                    if (weaponList.Contains(weapon))
                    {
                        weaponList.Remove(weapon);
                    }
                    defaultWeapon = weapon;
                    defaultWeapon.transform.SetAsFirstSibling();
                }
            }

            #endif

        #endregion - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -|

        [HorizontalGroup("Weapon/CurWeapon")]
        #region Horizontal CurWeapon  - - - - - - - - - - - - - - - - - - - - -|

            [VerticalGroup("Weapon/CurWeapon/Vertical", PaddingBottom = 25)]//-|
            [ReadOnly][NonSerialized]
            [LabelWidth(Editor.labelWidth)]
            public Weapon curWeapon;

            #if UNITY_EDITOR

            [HorizontalGroup("Weapon/CurWeapon", width: Editor.shortButtonWidth)]
            [DisableInEditorMode]
            [Button(name: "Set", Expanded = true)]
            void SetCurWeapon(int index)
            {
                //무기 없음
                if (transform.childCount == 0) return;

                //선택 해제
                if (index == -1)
                {
                    curWeapon.state = WeaponState.Inventory;
                    curWeapon = null;
                }
                else SelectWeapon(weaponHolder.GetChild(index).GetComponent<Weapon>());
            }

            #endif

        #endregion  - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -|

        [VerticalGroup("Weapon/Inventory")]
        [HorizontalGroup("Weapon/Inventory/Horizontal")]
        #region Horizontal Inventory - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -|

            [ReadOnly]
            [LabelWidth(Editor.labelWidth)]
            public List<Weapon> weaponList = new List<Weapon>();

            #if UNITY_EDITOR

            [HorizontalGroup("Weapon/Inventory/Horizontal", width: Editor.shortNoLabelPropertyWidth)]
            [VerticalGroup("Weapon/Inventory/Horizontal/Manage")]
            #region Vertical Manage  - - - - - - - - - - - - - - - - - - - - - - - - - - - -|       
    
                [ShowInInspector][DelayedProperty]
                [HideLabel]
                public int showInventorySize {
                    get => InventorySize; 
                    set {
                        if (value < weaponList.Count) InventorySize = weaponList.Count; 
                        else InventorySize = value; 
                    }
                } 
                                                                                                    [VerticalGroup("Weapon/Inventory/Horizontal/Manage")]
                [Button(name: "Clear", Expanded = true)]
                void ClearInventory()
                {
                    //기본 무기 제거
                    if (defaultWeapon != null)
                    {
                        defaultWeapon.gameObject.AutoDestroy();//-|
                    }

                    //인벤토리 무기 제거
                    for (int i = weaponList.Count - 1; i >= 0; i--)
                    {
                        if (weaponList[i] == null) continue;

                        weaponList[i].gameObject.AutoDestroy();
                    }

                    defaultWeapon = null;
                    curWeapon = null;
                    weaponList.Clear();
                }


            #endregion - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -|


            [HorizontalGroup("Weapon/Inventory/Manage")]
            #region Horizontal Manage  - - - - - - - - - - - - - - - - - - - - - - - - - - - -|

                [Button(name: "Add")]
                void AddWeaponInInspector(string name)
                {
                    AddWeapon(Weapon.PrefabLinkSpawn(name, weaponHolder));
                    if (defaultWeapon == null) SetDefaultWeapon(weaponHolder.childCount - 1);
                }
                [HorizontalGroup("Weapon/Inventory/Manage")]
                [Button(name: "Destroy")]
                void DestroyWeaponInInspector(int index)
                {
                    if (index == -1)
                    {
                        if (defaultWeapon == null) return;

                        DestroyImmediate(defaultWeapon.gameObject);

                        defaultWeapon = null;
                    }
                    else
                    {
                        if (weaponList[index] == null)
                        {
                            weaponList.RemoveAt(index);
                            return;
                        }

                        weaponList[index].WeaponDestroy();
                    }
                }

            #endregion - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -|

            #endif

        #endregion - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -|

    #endregion

    [LabelWidth(Editor.labelWidth)]
    public Vector2 view = new Vector2(0, 0);

    [LabelWidth(Editor.labelWidth)]
    public Vector2 bodyPos = new Vector2(0, 0);

    [NonSerialized] 
    public AnimationType animationType;
    
    [NonSerialized] 
    public MoveAnimationType moveAnimationType;
    
    [NonSerialized] 
    public SkillAnimationType skillAnimationType;

    #region 무기 관리

    /// <summary>
    /// 무기 추가 >> INVENTORY
    /// </summary>
    /// <param name="weapon">무기 상태에 대해 안전하지 않음</param>
    public Weapon AddWeapon(Weapon weapon)
    {
        if (weapon.state == WeaponState.Hold
            || weapon.state == WeaponState.Inventory)
        {
            Debug.LogWarning("먼저 인벤토리에서 제거된 후 추가해야함");
            return weapon;
        } //LogWarning: 먼저 인벤토리에서 제거된 후 추가해야함 >> return

        if (weaponList.Contains(weapon))
        {
            Debug.LogWarning("이미 인벤토리에 있음");
            return weapon;
        } //LogWarning: 이미 인벤토리에 있음 >> return

        if (weaponList.Count > InventorySize)
        {
            Debug.LogWarning("인벤토리가 꽉 참");
            weapon.gameObject.AutoDestroy();
            return weapon;
        } //LogWarning: 인벤토리가 꽉 참 >> return

        weapon.con = this;

        weapon.transform.parent = weaponHolder;

        if (curWeapon == null) weapon.SetUse(true);
        else weapon.SetUse(false);

        weaponList.Add(weapon);
        return weapon;
    }

    /// <summary>
    /// 무기 드랍
    /// HOLD -> INVENTORY -> REMOVED -> ITEM >>  //
    /// </summary>
    /// <param name="weapon"></param>
    public void DropWeapon(Weapon weapon)
    {
        Item item = null;

        if (weapon.state == WeaponState.Item)
        {
            item = weapon.transform.parent.GetComponent<Item>();
        }
        else
        {
            RemoveWeapon(weapon);
            item = ItemManager.WrapWeaponInItem(weapon);
        }

        item.transform.position = transform.position;

        //던지는 효과----------------------------------------------------
    }

    /// <summary>
    /// Hold -> Inventory >> 인벤토리에서 무기 제거 >> weapon.OnWeaponRemoved() >> 부모 제거, weapon.state = REMOVED >> 새 무기 선택 //
    /// </summary>
    /// <param name="weapon"></param>
    public void RemoveWeapon(Weapon weapon)
    {
        weapon.OnWeaponRemoved();

        if (weapon == defaultWeapon)
        {
            Debug.LogError("기본 무기는 제거할 수 없음");
            return;
        } //LogError: 기본 무기는 제거할 수 없음 >> return

        if (weaponList.Contains(weapon) == false)
        {
            Debug.LogError("{" + weapon.name + "}을(를) 인벤토리에서 찾을 수 없어 제거하지 못함.");
            return;
        } //LogError: {weapon.name}을(를) 인벤토리에서 찾을 수 없어 제거하지 못함. >> return

        if (weapon.state == WeaponState.Hold)
        {
            weapon.SetUse(false);
        } //HOLD -> INVENTORY

        else if (weapon.state == WeaponState.Inventory)
        {

        } //

        int index = weaponList.IndexOf(weapon);
        weaponList.Remove(weapon);

        weapon.transform.parent = null;
        weapon.state = WeaponState.Removed;

        //무기 선택
        if (index == weaponList.Count) //마지막 순서의 무기였을 때
        {
            if (index == 0)
            {
                if (defaultWeapon != null) SelectWeapon(defaultWeapon);
            }
            else SelectWeapon(weaponList[0]);
        }
        else
        {
            if (weaponList[index] != null) SelectWeapon(weaponList[index]);
            else if (defaultWeapon != null) SelectWeapon(defaultWeapon);
        }
    }

    protected void SelectWeapon(Weapon weapon)
    {
        if (weapon != defaultWeapon && weaponList.Contains(weapon) == false)
        {
            Debug.LogError("{" + weapon.name + "}을(를) 인벤토리에서 찾을 수 없어 선택하지 못함.");
            return;
        } //LogError: {weapon.name}을(를) 인벤토리에서 찾을 수 없어 선택하지 못함. >> return

        if (curWeapon != null && curWeapon.state == WeaponState.Hold) curWeapon.SetUse(false); //현재 무기 비활성화

        curWeapon = weapon;
        if (curWeapon.state == WeaponState.Inventory) weapon.SetUse(true); //선택한 무기 활성화
    }

    #endregion

    protected void Awake()
    {
        hand = transform.Find(HandGrafic.HAND_NAME).GetComponent<HandGrafic>();
        weaponHolder = transform.Find(WEAPONHOLDER_NAME);
        LoadWeapon();
    }
    protected void Update()
    {
        //Move
        if (moveVector != Vector2.zero && lastMoveVector != moveVector)
        {
            moveRotate = Math.Vector2ToDegree(moveVector);
            moveRotate4 = Math.Rotate4X(moveRotate4, moveRotate);//-|
            moveRotate8 = Math.FloorRotateToInt(moveRotate, 45, 8);
            lastMoveVector = moveVector;
        }
    }
    protected void LoadWeapon()
    {
        defaultWeapon = weaponHolder.GetChild(0).GetComponent<Weapon>();
        defaultWeapon.con = this;
        defaultWeapon.transform.parent = weaponHolder; 
        defaultWeapon.SetUse(false);
        SelectWeapon(defaultWeapon);

        for (int i = 1; i < weaponHolder.childCount; i++)
        {
            Weapon weapon = weaponHolder.GetChild(i).GetComponent<Weapon>();
            defaultWeapon.con = this;
            defaultWeapon.transform.parent = weaponHolder;
            weapon.SetUse(false);
            weaponList.Add(weapon);
        }
    }
    protected void InputEventTrigger()
    {
        //Attack
        if (attack)
        {
            if (attack != prevAttack) //Down
            {
                AttackDownEventNode.Trigger(curWeapon.gameObject);

                prevAttack = attack;
            }

            //Update
            AttackHoldEventNode.Trigger(curWeapon.gameObject, attackCharge);
            attackCharge += Time.deltaTime;
        }
        else
        {
            if (attack != prevAttack) //Up
            {
                AttackUpEventNode.Trigger(curWeapon.gameObject, attackCharge);
                attackCharge = 0;

                if (readyPreventAttackInput) preventAttackInput = true;
                else preventAttackInput = false;

                prevAttack = attack;
            }
        }

        //Special
        if (special)
        {
            if (special != prevSpecial) //Down
            {
                SpecialDownEventNode.Trigger(curWeapon.gameObject);

                prevSpecial = special;
            }

            //Update
            SpecialHoldEventNode.Trigger(curWeapon.gameObject, specialCharge);
            specialCharge += Time.deltaTime;
        }
        else
        {
            if (special != prevSpecial) //Up
            {
                SpecialUpEventNode.Trigger(curWeapon.gameObject, specialCharge);
                specialCharge = 0;

                if (readyPreventSpecialInput) preventSpecialInput = true;
                else preventSpecialInput = false;

                prevSpecial = special;
            }
        }
    }

    public ControllerSave GetData()
    {
        WeaponSaveData[] weaponDatas = new WeaponSaveData[weaponList.Count];

        for (int weaponIndex = 0; weaponIndex < weaponList.Count; weaponIndex++)
        {
            for (int weaponDataIndex = 0; weaponDataIndex < weaponList.Count; weaponDataIndex++)
            {
                weaponDatas[weaponDataIndex] = weaponList[weaponIndex].GetData();
            }
        }

        return new ControllerSave
            (
                (Vector2)transform.position,
                defaultWeapon.GetData(),
                weaponDatas,
                weaponList.IndexOf(curWeapon)
            );

    }
    public void SetData(ControllerSave data)
    {
        transform.position = data.pos;
        defaultWeapon = Weapon.LoadWeapon(data.defaultWeaponData);
        foreach (WeaponSaveData weaponData in data.weaponDatas)
        {
            AddWeapon(Weapon.LoadWeapon(weaponData));
        }
        SelectWeapon(weaponList[data.curWeaponIndex]);
    }
    protected void OnDrawGizmosSelected()
    {
        //LastMoveVector
        if (moveVector == Vector2.zero)
        {
            Gizmos.color = new Color(0, 1, 0, 0.25f);
            Gizmos.DrawRay(transform.position, lastMoveVector.normalized);
        }

        //MoveVector
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, moveVector.normalized);

        //Target
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position + (Vector3)Center, targetPos);
        Gizmos.DrawWireSphere(targetPos, 0.1f);
    }
}