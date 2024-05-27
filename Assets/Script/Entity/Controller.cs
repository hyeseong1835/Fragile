using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.Rendering.DebugUI;

public struct ControllerData
{
    //좌표
    public Vector2 pos;
    //기본 무기(데이터)
    public WeaponSaveData defaultWeaponData;
    //현재 무기(인덱스)
    public int curWeaponIndex;
    //인벤토리(무기(데이터))
    public WeaponSaveData[] weaponDatas;

    public ControllerData(
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
[ExecuteAlways]
public abstract class Controller : MonoBehaviour
{
    #region 정적 멤버 - - - - - - - - - - - - - - - - - - - -|

    public const string weaponHolderName = "WeaponHolder";//-|

    #endregion  - - - - - - - - - - - - - - - - - - - - - - -|

    [LabelWidth(Editor.propertyLabelWidth)]
    public Vector2 center = new Vector2(0, 0.5f);

    [BoxGroup("Object")]
    #region Foldout Object - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -|

        [Required][ChildGameObjectsOnly][PropertyOrder(0)]
        [LabelText("Hand Grafic")][LabelWidth(Editor.propertyLabelWidth - Editor.childGameObjectOnlyWidth)]//-|
        public HandGrafic hand;
                                                                                                               [BoxGroup("Object")]
        [Required][ChildGameObjectsOnly][PropertyOrder(0)]
        [LabelWidth(Editor.propertyLabelWidth - Editor.childGameObjectOnlyWidth)]
        public Transform weaponHolder;

    #endregion - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -|

    [FoldoutGroup("Stat")]
    #region Foldout Stat - - - - - - - - - - - - - - - -|                                         

        [HorizontalGroup("Stat/HP")]
        #region Horizontal HP - - - - - - - - - - - - - - - - - - - - - - - - -|

            [LabelWidth(Editor.propertyLabelWidth)]
            #if UNITY_EDITOR
            [ProgressBar(0, nameof(_maxHp), ColorGetter = nameof(_hpColor))]
            #endif
            public float hp;

            [HideInInspector]
            public float maxHp;

            #if UNITY_EDITOR
                                                                                [HorizontalGroup("Stat/HP", Width = Editor.shortNoLabelPropertyWidth)]
            [ShowInInspector][HideLabel]
            [DelayedProperty]
            float _maxHp{
                get { return maxHp; }
                set {
                    if (hp == maxHp || hp > value) hp = value;
                    maxHp = value;
                }
            }

            Color _hpColor {
                get {
                    Gradient gradient = new Gradient();
                    gradient.SetKeys(
                        new GradientColorKey[] {
                            new GradientColorKey(Color.yellow, 0),
                            new GradientColorKey(Color.red, 1)
                        },
                        new GradientAlphaKey[] { new GradientAlphaKey(1, 0) }//-|
                    );
                    return gradient.Evaluate(hp / maxHp);
                }
            }

            #endif

        #endregion  - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -|    

        [LabelWidth(Editor.propertyLabelWidth)]
        public float moveSpeed = 1;

    #endregion - - - - - - - - - - - - - - - - - - - - -|

    #region Input  - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -|

    [VerticalGroup("Input/Move")]
    #region Vertical Move - - - - - - - - - - - - - - - - - - - - -|
    
    #if UNITY_EDITOR
    [DisableInEditorMode][HideIf(nameof(inspectorShowLastMoveVector))]
    #endif
    [LabelWidth(Editor.propertyLabelWidth)]
    public Vector2 moveVector = new Vector2(0.5f, 0);
                                                                        [VerticalGroup("Input/Move")]
    #if UNITY_EDITOR
    [DisableInEditorMode][ShowIf(nameof(inspectorShowLastMoveVector))]
    #endif
    [LabelWidth(Editor.propertyLabelWidth)]
    public Vector2 lastMoveVector = new Vector2(0.5f, 0);//-|

    [VerticalGroup("Input/Move")]
    #if UNITY_EDITOR
    [ShowInInspector][ReadOnly][HideIf(nameof(inspectorShowLastMoveVector))]
    [LabelWidth(Editor.propertyLabelWidth)]
    [MinMaxSlider(0, 7)]
    Vector2Int _moveRotate {
        get {
            int intRotate = Utility.FloorRotateToInt(moveRotate, 8);

            if (intRotate == 7) return new Vector2Int(intRotate, intRotate);
            else return new Vector2Int(intRotate, intRotate + 1);
        }
    }
    #endif

    [HideInInspector]
    public float moveRotate = 0;
    #if UNITY_EDITOR
                                                                                    [VerticalGroup("Input/Move")]
    [ShowInInspector][ReadOnly][ShowIf(nameof(inspectorShowLastMoveVector))]
    [LabelWidth(Editor.propertyLabelWidth)]
    [MinMaxSlider(0, 7)]
    Vector2Int _lastMoveRotate {
        get {
            int intRotate = Utility.FloorRotateToInt(lastMoveRotate, 8);

            if (intRotate == 7) return new Vector2Int(intRotate, intRotate);
            else return new Vector2Int(intRotate, intRotate + 1);
        }
    }
    #endif
                                                                                    [VerticalGroup("Input/Move")]
    public float lastMoveRotate = 0;

#if UNITY_EDITOR

    bool inspectorShowLastMoveVector
    {
        get { return (moveVector == Vector2.zero); }
    }

#endif

    #endregion  - - - - - - - - - - - - - - - - - - - - - - - - - -|

    [VerticalGroup("Input/Target")]
    #region Vertical Target  - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -|

    [LabelWidth(Editor.propertyLabelWidth)]
    public Vector2 targetPos;
    [VerticalGroup("Input/Target")]
    [ShowInInspector]
    [LabelWidth(Editor.propertyLabelWidth)]
    public Vector2 targetDir
    {
        get { return (targetPos - ((Vector2)transform.position + center)).normalized; }//-|
    }

    #endregion - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -|

    [VerticalGroup("Input/Attack")]
    #region Vertical Attack - - - - - - - - - - - - - - - - -|

    public bool attack = false;
    public bool prevAttack = false;

    float attackCharge;
    public bool preventAttackInput = false;
    public bool readyPreventAttackInput = false;


    #endregion  - - - - - - - - - - - - - - - - - - - - - - -|

    [VerticalGroup("Input/Special")]
    #region Vertical Special - - - - - - - - - - - - - - - - -|

    public bool special;
    public bool prevSpecial = false;

    float specialCharge;
    public bool isSpecial { get; private set; } = false;
    public bool preventSpecialInput = false;
    public bool readyPreventSpecialInput = false;


    #endregion  - - - - - - - - - - - - - - - - - - - - - - -|   

    #endregion - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -|

    [FoldoutGroup("Weapon")]
    #region Foldout Weapon - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -|     

    [VerticalGroup("Weapon/DefaultWeapon")]
    [HorizontalGroup("Weapon/DefaultWeapon/Horizontal")]
    #region Horizontal DefaultWeapon - - - - - - - - - - - - - - - - - - - - - - - - - - -|

    [VerticalGroup("Weapon/DefaultWeapon/Horizontal/Vertical", PaddingBottom = 25)]//-|
    [ShowInInspector]
    [ReadOnly]
    [Required]
    [LabelWidth(Editor.propertyLabelWidth)]
    public Weapon defaultWeapon;

#if UNITY_EDITOR
    [HorizontalGroup("Weapon/DefaultWeapon/Horizontal", width: Editor.shortFunctionButtonWidth)]
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
    [ReadOnly]
    [Required]
    [LabelWidth(Editor.propertyLabelWidth)]
    public Weapon curWeapon;

#if UNITY_EDITOR
    [HorizontalGroup("Weapon/CurWeapon", width: Editor.shortFunctionButtonWidth)]
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

    [HorizontalGroup("Weapon/Inventory/Horizontal", width: Editor.shortNoLabelPropertyWidth)]
    [VerticalGroup("Weapon/Inventory/Horizontal/Manage")]
    #region Vertical Manage  - - - - - - - - - - - - - - - - - - - - - - - - - - - -|       

    [DelayedProperty]
    [HideLabel]
    public int inventorySize = 0;

#if UNITY_EDITOR
    [VerticalGroup("Weapon/Inventory/Horizontal/Manage")]
    [Button(name: "Clear")]
    void ClearInventory()
    {
        //기본 무기 제거
        if (defaultWeapon != null) Utility.AutoDestroy(defaultWeapon.gameObject);//-|

        //인벤토리 무기 제거
        for (int i = weapons.Count - 1; i >= 0; i--)
        {
            if (weapons[i] == null) continue;

            Utility.AutoDestroy(weapons[i].gameObject);
        }

        defaultWeapon = null;
        curWeapon = null;
        weapons.Clear();
    }

#endif

    #endregion - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -|

#if UNITY_EDITOR

    [HorizontalGroup("Weapon/Inventory/Manage")]
    #region Horizontal Manage  - - - - - - - - - - - - - - - - - - - - - - - - - - - -|

    [Button(name: "Add")]
    void AddWeaponInInspector(string name)
    {
        AddWeapon(Weapon.Spawn(name, weaponHolder));
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

    #endregion - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -|     

    #endregion - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -|



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

        if (weapons.Count > inventorySize)
        {
            Debug.LogWarning("인벤토리가 꽉 참");
            Utility.AutoDestroy(weapon.gameObject);
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

    public void TakeDamage(float damage)
    {
        hp -= damage;
        if (hp <= 0) Die();
    }
    void Die()
    {
        Destroy(gameObject);
    }
    protected void Update()
    {
        //Attack
        if (attack != prevAttack)
        {
            if (attack) //Down
            {
                AttackDownEventNode.Trigger(curWeapon.gameObject);
            }
            else //Up
            {
                AttackUpEventNode.Trigger(curWeapon.gameObject, attackCharge);
                attackCharge = 0;
                if (readyPreventAttackInput) preventAttackInput = true;
                else preventAttackInput = false;
            }
            prevAttack = attack;
        }
        if (attack)
        {
            AttackHoldEventNode.Trigger(curWeapon.gameObject, attackCharge);
            attackCharge += Time.deltaTime;
        }

        //Special
        if (special != prevSpecial)
        {
            if (special)
            {
                SpecialDownEventNode.Trigger(curWeapon.gameObject);
            }
            else
            {
                SpecialUpEventNode.Trigger(curWeapon.gameObject, specialCharge);
                specialCharge = 0;
                if (readyPreventSpecialInput) preventSpecialInput = true;
                else preventSpecialInput = false;
            }
            prevSpecial = special;
        }
        if (special)
        {
            SpecialHoldEventNode.Trigger(curWeapon.gameObject, specialCharge);
            specialCharge += Time.deltaTime;
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
    public ControllerData GetData()
    {
        WeaponSaveData[] weaponDatas = new WeaponSaveData[weapons.Count];

        for (int weaponIndex = 0; weaponIndex < weapons.Count; weaponIndex++)
        {
            for (int weaponDataIndex = 0; weaponDataIndex < weapons.Count; weaponDataIndex++)
            {
                weaponDatas[weaponDataIndex] = weapons[weaponIndex].GetData();
            }
        }

        return new ControllerData
            (
                (Vector2)transform.position,
                defaultWeapon.GetData(),
                weaponDatas,
                weapons.IndexOf(curWeapon)
            );

    }
    public void SetData(ControllerData data)
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
        Gizmos.DrawLine(transform.position + (Vector3)center, targetPos);
        Gizmos.DrawWireSphere(targetPos, 0.1f);
    }
}
[UnitTitle("Prevent Attack Input")]
[UnitCategory("Events/Weapon/Action")]
public class PreventAttackInput : Node
{
    Weapon weapon;
    protected override void Act(Flow flow)
    {
        if (weapon == null) weapon = flow.stack.gameObject.GetComponent<Weapon>();

        weapon.con.readyPreventAttackInput = true;
    }
}
[UnitTitle("Allow Attack Input")]
[UnitCategory("Events/Weapon/Action")]
public class AllowAttackInput : Node
{
    Weapon weapon;
    protected override void Act(Flow flow)
    {
        if (weapon == null) weapon = flow.stack.gameObject.GetComponent<Weapon>();

        weapon.con.readyPreventAttackInput = false;
        weapon.con.preventAttackInput = false;
    }
}
[UnitTitle("Prevent Special Input")]
[UnitCategory("Events/Weapon/Action")]
public class PreventSpecialInput : Node
{
    Weapon weapon;

    protected override void Act(Flow flow)
    {
        if (weapon == null) weapon = flow.stack.gameObject.GetComponent<Weapon>();

        weapon.con.readyPreventSpecialInput = true;
    }
}
[UnitTitle("Allow Special Input")]
[UnitCategory("Events/Weapon/Action")]
public class AllowSpecialInput : Node
{
    Weapon weapon;
    protected override void Act(Flow flow)
    {
        if (weapon == null) weapon = flow.stack.gameObject.GetComponent<Weapon>();

        weapon.con.readyPreventSpecialInput = false;
        weapon.con.preventSpecialInput = false;
    }
}
