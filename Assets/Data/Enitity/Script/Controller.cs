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
    //��ǥ
    public Vector2 pos;
    //�⺻ ����(������)
    public WeaponSaveData defaultWeaponData;
    //���� ����(�ε���)
    public int curWeaponIndex;
    //�κ��丮(����(������))
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
    #region ���� ��� - - - - - - - - - - - - - - - - - - - -|

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
        [LabelText("Player Grafic")][LabelWidth(Editor.propertyLabelWidth - Editor.childGameObjectOnlyWidth)]//-|
        public Grafic grafic;
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
            public float hp = 1;

            [HideInInspector]
            public float maxHp = 1;

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
    #region Vertical Move - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -|

        [ShowInInspector]
        [LabelWidth(Editor.propertyLabelWidth)]
        public Vector2 lastMoveVector { get { return _lastMoveVector; }
            private set {
                if (_lastMoveVector != value) 
                {
                    moveRotate = Utility.Vector2ToDegree(moveVector);
                    moveRotate4 = Utility.FloorRotateToInt(moveRotate, 45, 4);
                    moveRotate8 = Utility.FloorRotateToInt(moveRotate, 45, 8);
                    _lastMoveVector = value;
                }
            } 
        } Vector2 _lastMoveVector = new Vector2(0.5f, 0);
                                                                                                  [VerticalGroup("Input/Move")]
        [LabelWidth(Editor.propertyLabelWidth)]
        public float moveRotate { get; private set; }


//      �ν����Ϳ� ǥ������ ����_________________________________________________________________|

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
        // ����(-1)
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

            //���� ���Ⱑ ������ �ڵ� ����
            if (curWeapon == null)
            {
                curWeapon = weapon;
                SelectWeapon(weapon);
            }

            //�κ��丮���� �ڵ� ����
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
        //���� ����
        if (transform.childCount == 0) return;

        //���� ����
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
        //�⺻ ���� ����
        if (defaultWeapon != null) Utility.AutoDestroy(defaultWeapon.gameObject);//-|

        //�κ��丮 ���� ����
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



    #region ���� ����

    /// <summary>
    /// ���� �߰� >> INVENTORY
    /// </summary>
    /// <param name="weapon">���� ���¿� ���� �������� ����</param>
    public Weapon AddWeapon(Weapon weapon)
    {
        if (weapon.state == WeaponState.Hold
            || weapon.state == WeaponState.Inventory)
        {
            Debug.LogWarning("���� �κ��丮���� ���ŵ� �� �߰��ؾ���");
            return weapon;
        } //LogWarning: ���� �κ��丮���� ���ŵ� �� �߰��ؾ��� >> return

        if (weapons.Contains(weapon))
        {
            Debug.LogWarning("�̹� �κ��丮�� ����");
            return weapon;
        } //LogWarning: �̹� �κ��丮�� ���� >> return

        if (weapons.Count > inventorySize)
        {
            Debug.LogWarning("�κ��丮�� �� ��");
            Utility.AutoDestroy(weapon.gameObject);
            return weapon;
        } //LogWarning: �κ��丮�� �� �� >> return

        weapon.con = this;

        weapon.transform.parent = weaponHolder;

        if (curWeapon == null) weapon.SetUse(true);
        else weapon.SetUse(false);

        weapons.Add(weapon);
        return weapon;
    }

    /// <summary>
    /// ���� ���
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

        //������ ȿ��----------------------------------------------------
    }

    /// <summary>
    /// Hold -> Inventory >> �κ��丮���� ���� ���� >> weapon.OnWeaponRemoved() >> �θ� ����, weapon.state = REMOVED >> �� ���� ���� //
    /// </summary>
    /// <param name="weapon"></param>
    public void RemoveWeapon(Weapon weapon)
    {
        weapon.OnWeaponRemoved();

        if (weapon == defaultWeapon)
        {
            Debug.LogError("�⺻ ����� ������ �� ����");
            return;
        } //LogError: �⺻ ����� ������ �� ���� >> return

        if (weapons.Contains(weapon) == false)
        {
            Debug.LogError("{" + weapon.name + "}��(��) �κ��丮���� ã�� �� ���� �������� ����.");
            return;
        } //LogError: {weapon.name}��(��) �κ��丮���� ã�� �� ���� �������� ����. >> return

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

        //���� ����
        if (index == weapons.Count) //������ ������ ���⿴�� ��
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
            Debug.LogError("{" + weapon.name + "}��(��) �κ��丮���� ã�� �� ���� �������� ����.");
            return;
        } //LogError: {weapon.name}��(��) �κ��丮���� ã�� �� ���� �������� ����. >> return

        if (curWeapon != null && curWeapon.state == WeaponState.Hold) curWeapon.SetUse(false); //���� ���� ��Ȱ��ȭ

        curWeapon = weapon;
        if (curWeapon.state == WeaponState.Inventory) weapon.SetUse(true); //������ ���� Ȱ��ȭ
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
        //Move
        if (moveVector != Vector2.zero) lastMoveVector = moveVector;

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
        //���� �� >? �ʱ�ȭ
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

        //���� �˻�
        for (int weaponIndex = weapons.Count - 1; weaponIndex >= 0; weaponIndex--)
        {
            //���� ����
            if (weapons[weaponIndex] == null) weapons.RemoveAt(weaponIndex);

            //�⺻ ���� �ֻ��
            if (weapons[weaponIndex] == defaultWeapon) defaultWeapon.transform.SetAsFirstSibling();

            //�ٸ��� �ʱ�ȭ
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