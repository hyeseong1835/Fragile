using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
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
    public override EntityData EntityData
    {
        get => ControllerData;
        set { ControllerData = (ControllerData)value; }
    }
    [BoxGroup("Object")]
    #region Foldout Object - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -|

        [HorizontalGroup("Object/ControllerData", Order = 0)]
        #region Horizontal ControllerData
        
            [ShowInInspector]
            [LabelWidth(Editor.propertyLabelWidth)]
            public abstract ControllerData ControllerData { get; set; }
    
            #if UNITY_EDITOR
            [HorizontalGroup("Object/ControllerData", Width = Editor.shortButtonWidth)]
            [Button("Create")]
            public void CreateData()
            {
                string currentPath = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(gameObject);
                string path = $"{currentPath.Substring(0, currentPath.LastIndexOf('/'))}/{DataType.Name}.asset";
                ControllerData dataInstance = (ControllerData)ScriptableObject.CreateInstance(DataType);
                ControllerData = dataInstance;
                AssetDatabase.CreateAsset(dataInstance, path);
            }
            #endif

        #endregion

        [HorizontalGroup("Object/Hand", Order = 1)]
        #region Horizontal Hand

            [PropertyOrder(0)]
            [LabelText("Hand")][LabelWidth(Editor.propertyLabelWidth)]//-|
            public HandGrafic hand;

            #if UNITY_EDITOR
            [HorizontalGroup("Object/Hand", Width = Editor.shortButtonWidth)]
            [Button("Create")]
            public void CreateHand()
            {
                HandGrafic hand = new GameObject("Hand").AddComponent<HandGrafic>();
                hand.transform.parent = transform;

                Controller con = gameObject.GetComponent<Controller>();
                con.hand = hand;
            }
            #endif

        #endregion

        [HorizontalGroup("Object/WeaponHolder", Order = 2)]
        #region Horizontal WeaponHolder

            [PropertyOrder(0)]
            [LabelWidth(Editor.propertyLabelWidth)]
            public Transform weaponHolder;

            #if UNITY_EDITOR
            [HorizontalGroup("Object/WeaponHolder", Width = Editor.shortButtonWidth)]
            [Button("Create")]
            public void CreateWeaponHolder()
            {
                weaponHolder = new GameObject("WeaponHolder").transform;
                weaponHolder.parent = transform;
                weaponHolder.localPosition = Vector3.zero;
            }
            #endif

        #endregion

    #endregion - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -|

    [FoldoutGroup("Input")]
    #region Foldout Input  - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -|

        [VerticalGroup("Input/Move")]
        #region Vertical Move - - - - - - - - - - - - - - - - - - - - - - - - - - - -|
                                                                                                    [VerticalGroup("Input/Move")]
        [ShowInInspector]
        [LabelWidth(Editor.propertyLabelWidth)]
        public Vector2 lastMoveVector { get { return _lastMoveVector; }
            private set {
                if (_lastMoveVector != value) 
                {
                    moveRotate = Math.Vector2ToDegree(moveVector);
                    moveRotate4 = Math.Rotate4X(moveRotate4, moveRotate);//-|
                    moveRotate8 = Math.FloorRotateToInt(moveRotate, 45, 8);
                    _lastMoveVector = value;
                }
            } 
        } Vector2 _lastMoveVector = new Vector2(0.5f, 0);
                                                                                                  [VerticalGroup("Input/Move")]
        [LabelWidth(Editor.propertyLabelWidth)]
        public float moveRotate { get; private set; }

//      HideInInspector__________________________________________________________|

        [HideInInspector]
        public Vector2 moveVector = new Vector2(0.5f, 0);
    
        [HideInInspector]
        public int moveRotate4 { get; private set; }

        [HideInInspector]
        public int moveRotate8 { get; private set; }

    #endregion  - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -|

        [VerticalGroup("Input/Target")]
        #region Vertical Target  - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -|

            [LabelWidth(Editor.propertyLabelWidth)]
            public Vector2 targetPos;
                                                                                                        [VerticalGroup("Input/Target")]
            [ShowInInspector]
            [LabelWidth(Editor.propertyLabelWidth)]
            public Vector2 targetDir{
                get {
                    if (ControllerData == null) return default;
                    return (targetPos - ((Vector2)transform.position + ControllerData.center)).normalized; 
                }
            }

        #endregion - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -|

        [VerticalGroup("Input/Attack")]
        #region Vertical Attack - - - - - - - - - - - - - - - - -|


            [ReadOnly] public bool attack = false;
            [HideInInspector] public bool prevAttack = false;

            [HideInInspector] float attackCharge;
            [HideInInspector] public bool preventAttackInput = false;
            [HideInInspector] public bool readyPreventAttackInput = false;


        #endregion  - - - - - - - - - - - - - - - - - - - - - - -|

        [VerticalGroup("Input/Special")]
        #region Vertical Special - - - - - - - - - - - - - - - - -|

            [ReadOnly] public bool special;
            [HideInInspector] public bool prevSpecial = false;

            [HideInInspector] float specialCharge;
            [HideInInspector] public bool isSpecial { get; private set; } = false;
            [HideInInspector] public bool preventSpecialInput = false;
            [HideInInspector] public bool readyPreventSpecialInput = false;

        #endregion  - - - - - - - - - - - - - - - - - - - - - - -|   

    #endregion - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -|

    [FoldoutGroup("Weapon")]
    #region Foldout Weapon - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -|     

        [VerticalGroup("Weapon/DefaultWeapon")]
        [HorizontalGroup("Weapon/DefaultWeapon/Horizontal")]
        #region Horizontal DefaultWeapon - - - - - - - - - - - - - - - - - - - - - - - - - - -|

            [VerticalGroup("Weapon/DefaultWeapon/Horizontal/Vertical", PaddingBottom = 25)]//-|
            [ShowInInspector][ReadOnly][Required]
            [LabelWidth(Editor.propertyLabelWidth)]
            public Weapon defaultWeapon;

            #if UNITY_EDITOR
                                                                                                    [HorizontalGroup("Weapon/DefaultWeapon/Horizontal", width: Editor.shortButtonWidth)]
            [Button(name: "Set")]
            void SetDefaultWeapon(int index)
            {
                // 제거(-1)
                if (index == -1)
                {
                    if (curWeapon == defaultWeapon)
                    {
                        curWeapon = null;
                    }
                    weapons.Add(defaultWeapon);
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
                    if (weapons.Contains(weapon))
                    {
                        weapons.Remove(weapon);
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
            [ReadOnly][Required]
            [LabelWidth(Editor.propertyLabelWidth)]
            public Weapon curWeapon;

            #if UNITY_EDITOR

            [HorizontalGroup("Weapon/CurWeapon", width: Editor.shortButtonWidth)]
            [Button(name: "Set")]
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
            [LabelWidth(Editor.propertyLabelWidth)]
            public List<Weapon> weapons = new List<Weapon>();

            #if UNITY_EDITOR

            [HorizontalGroup("Weapon/Inventory/Horizontal", width: Editor.shortNoLabelPropertyWidth)]
            [VerticalGroup("Weapon/Inventory/Horizontal/Manage")]
            #region Vertical Manage  - - - - - - - - - - - - - - - - - - - - - - - - - - - -|       
    
    
                [ShowInInspector][DelayedProperty]
                [HideLabel]
                public int showInventorySize { 
                    get { 
                        if (ControllerData == null) return default; 
                        return ControllerData.inventorySize; 
                    } 
                    set { ControllerData.inventorySize = value; } 
                }
                                                                                                    [VerticalGroup("Weapon/Inventory/Horizontal/Manage")]
                [Button(name: "Clear")]
                void ClearInventory()
                {
                    //기본 무기 제거
                    if (defaultWeapon != null)
                    {
                        defaultWeapon.gameObject.AutoDestroy();//-|
                    }

                    //인벤토리 무기 제거
                    for (int i = weapons.Count - 1; i >= 0; i--)
                    {
                        if (weapons[i] == null) continue;

                        weapons[i].gameObject.AutoDestroy();
                    }

                    defaultWeapon = null;
                    curWeapon = null;
                    weapons.Clear();
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
                        if (weapons[index] == null)
                        {
                            weapons.RemoveAt(index);
                            return;
                        }

                        weapons[index].WeaponDestroy();
                    }
                }

            #endregion - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -|

            #endif

        #endregion - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -|

    #endregion

    [LabelWidth(Editor.propertyLabelWidth)]
    public Vector2 view = new Vector2(0, 0);

    [LabelWidth(Editor.propertyLabelWidth)]
    public Vector2 bodyPos = new Vector2(0, 0);

    public AnimationType animationType;
    public MoveAnimationType moveAnimationType;
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

        if (weapons.Contains(weapon))
        {
            Debug.LogWarning("이미 인벤토리에 있음");
            return weapon;
        } //LogWarning: 이미 인벤토리에 있음 >> return

        if (weapons.Count > ControllerData.inventorySize)
        {
            Debug.LogWarning("인벤토리가 꽉 참");
            weapon.gameObject.AutoDestroy();
            return weapon;
        } //LogWarning: 인벤토리가 꽉 참 >> return

        weapon.con = this;

        weapon.transform.parent = weaponHolder;

        if (curWeapon == null) weapon.SetUse(true);
        else weapon.SetUse(false);

        weapons.Add(weapon);
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

        if (weapons.Contains(weapon) == false)
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

        int index = weapons.IndexOf(weapon);
        weapons.Remove(weapon);

        weapon.transform.parent = null;
        weapon.state = WeaponState.Removed;

        //무기 선택
        if (index == weapons.Count) //마지막 순서의 무기였을 때
        {
            if (index == 0)
            {
                if (defaultWeapon != null) SelectWeapon(defaultWeapon);
            }
            else SelectWeapon(weapons[0]);
        }
        else
        {
            if (weapons[index] != null) SelectWeapon(weapons[index]);
            else if (defaultWeapon != null) SelectWeapon(defaultWeapon);
        }
    }

    protected void SelectWeapon(Weapon weapon)
    {
        if (weapon != defaultWeapon && weapons.Contains(weapon) == false)
        {
            Debug.LogError("{" + weapon.name + "}을(를) 인벤토리에서 찾을 수 없어 선택하지 못함.");
            return;
        } //LogError: {weapon.name}을(를) 인벤토리에서 찾을 수 없어 선택하지 못함. >> return

        if (curWeapon != null && curWeapon.state == WeaponState.Hold) curWeapon.SetUse(false); //현재 무기 비활성화

        curWeapon = weapon;
        if (curWeapon.state == WeaponState.Inventory) weapon.SetUse(true); //선택한 무기 활성화
    }

    #endregion

    protected void Update()
    {
        //Move
        if (moveVector != Vector2.zero) lastMoveVector = moveVector;

    }
    protected void InputEvent()
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

    public void AutoDebug()
    {
        //개수 비교 >? 초기화
        if (weaponHolder.childCount != weapons.Count + Convert.ToInt32(defaultWeapon != null))
        {
            if (defaultWeapon != null) defaultWeapon.transform.SetAsFirstSibling();

            weapons.Clear();
            for (int i = Convert.ToInt32(defaultWeapon != null); i < weaponHolder.childCount; i++)
            {
                weapons.Add(weaponHolder.GetChild(i).GetComponent<Weapon>());
            }
            return;
        }

        //무기 검사
        for (int weaponIndex = weapons.Count - 1; weaponIndex >= 0; weaponIndex--)
        {
            //공란 삭제
            if (weapons[weaponIndex] == null) weapons.RemoveAt(weaponIndex);

            //기본 무기 최상단
            if (weapons[weaponIndex] == defaultWeapon) defaultWeapon.transform.SetAsFirstSibling();

            //다르면 초기화
            if (transform.GetSiblingIndex() != weaponIndex + Convert.ToInt32(defaultWeapon != null))
            {
                if (defaultWeapon != null) defaultWeapon.transform.SetAsFirstSibling();

                weapons.Clear();
                for (int i = Convert.ToInt32(defaultWeapon != null); i < weaponHolder.childCount; i++)
                {
                    weapons.Add(weaponHolder.GetChild(i).GetComponent<Weapon>());
                }
                return;
            }
        }
    }
    public ControllerSave GetData()
    {
        WeaponSaveData[] weaponDatas = new WeaponSaveData[weapons.Count];

        for (int weaponIndex = 0; weaponIndex < weapons.Count; weaponIndex++)
        {
            for (int weaponDataIndex = 0; weaponDataIndex < weapons.Count; weaponDataIndex++)
            {
                weaponDatas[weaponDataIndex] = weapons[weaponIndex].GetData();
            }
        }

        return new ControllerSave
            (
                (Vector2)transform.position,
                defaultWeapon.GetData(),
                weaponDatas,
                weapons.IndexOf(curWeapon)
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
        SelectWeapon(weapons[data.curWeaponIndex]);
    }
    protected void OnDrawGizmosSelected()
    {
        if (ControllerData == null) return;

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
        Gizmos.DrawLine(transform.position + (Vector3)ControllerData.center, targetPos);
        Gizmos.DrawWireSphere(targetPos, 0.1f);
    }
}