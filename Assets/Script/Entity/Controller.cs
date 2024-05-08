using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.Rendering.DebugUI;

public struct ControllerData
{
    //��ǥ
    public Vector2 pos;
    //�⺻ ����(������)
    public WeaponData defaultWeaponData;
    //���� ����(�ε���)
    public int curWeaponIndex;
    //�κ��丮(����(������))
    public WeaponData[] weaponDatas;

    public ControllerData(
        Vector2 _pos, 
        WeaponData _defaultWeaponData, 
        WeaponData[] _weaponDatas, 
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



    [Required][PropertyOrder(0)]
    public HandGrafic hand;

    [Required][PropertyOrder(0)]
    public Transform weaponHolder;

    public Vector2 center = new Vector2(0, 0.5f);

    [FoldoutGroup("Stat")]
    #region Foldout Stat - - - - - - - - - - - - - - - -|                                         

        [HorizontalGroup("Stat/HP")]
        #region Horizontal HP  - - - - - - - - - - -|
            #if UNITY_EDITOR
            [ProgressBar(0, nameof(_maxHp), 
                ColorGetter = nameof(_hpColor))]//-|
            #endif
            public float hp;

            [HideInInspector]
            public float maxHp;
            
            #if UNITY_EDITOR
                                                     [HorizontalGroup("Stat/HP", width: 30)]
            [ShowInInspector]
            [HideLabel][DelayedProperty]
            float _maxHp { 
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
                        new GradientAlphaKey[] { new GradientAlphaKey(1,0) }
                    );
                    return gradient.Evaluate(hp / maxHp);
                }
            }
            
            #endif

        #endregion - - - - - - - - - - - -|                         
                                               [FoldoutGroup("Stat")]
    public float moveSpeed = 1;

    #endregion - - - - - - - - - - - - - - - -|

    [FoldoutGroup("Input")]
    #region Foldout Input  - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -|

        [VerticalGroup("Input/Move")]
        #region Vertical Move - - - - - - - - - - - - - - - - - - - - -|
            #if UNITY_EDITOR
            [HideIf(nameof(inspectorShowLastMoveVector))] 
            #endif
            public Vector2 moveVector = new Vector3(0.5f, 0, 0);                                             
                                                                        [VerticalGroup("Input/Move")]
            #if UNITY_EDITOR
            [ShowIf(nameof(inspectorShowLastMoveVector))]
            #endif
            public Vector2 lastMoveVector = new Vector3(0.5f, 0, 0);//-|
                 
            [VerticalGroup("Input/Move")]                 
            #if UNITY_EDITOR
            [HideIf(nameof(inspectorShowLastMoveVector))]
            #endif
            [Range(0f, 360f)]
            public float moveRotate = 0;
                                                                        [VerticalGroup("Input/Move")]
            #if UNITY_EDITOR
            [ShowIf(nameof(inspectorShowLastMoveVector))] 
            #endif
            [Range(0f, 360f)]                                                                              
            public float lastMoveRotate = 0;
        
        #endregion  - - - - - - - - - - - - - - - - - - - - - - - - - -|
    
        [VerticalGroup("Input/Target")]
        #region Vertical Target  - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -|
    
            public Vector2 targetPos;                                                                        
                                                                                                   [VerticalGroup("Input/Target")]
            [ShowInInspector]
            public Vector2 targetDir { 
                get { return (targetPos - ((Vector2)transform.position + center)).normalized; }//-|
            }

        #endregion - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -|

        [VerticalGroup("Input/Skill")]
        #region Vertical Skill  - - - - - - - - - - - - - - -|
            #if UNITY_EDITOR
            [PropertyRange(0, nameof(attackCoolMax))]
            #endif
            public float attackCool = 0;
        
            [HideInInspector] 
            public bool attack = false;
    
            [HideInInspector] public bool special = false;//-|

        #endregion  - - - - - - - - - - - - - - - - - - - - -|

        #if UNITY_EDITOR
        
        bool inspectorShowLastMoveVector{ 
            get { return (moveVector == Vector2.zero); } 
        }
        
        float attackCoolMax {
            get {
                if (curWeapon == null) return 0;
                else return curWeapon.attackFrontDelay + curWeapon.attackDelay + curWeapon.attackBackDelay;
            }
        }

        #endif

    #endregion - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -|     

    [FoldoutGroup("Weapon")]
    #region Foldout Weapon - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -|     

        [VerticalGroup("Weapon/DefaultWeapon")]
        [HorizontalGroup("Weapon/DefaultWeapon/Horizontal")]
        #region Horizontal DefaultWeapon - - - - - - - - - - - - - - - - - - - - - - - - - - -|
                
            [VerticalGroup("Weapon/DefaultWeapon/Horizontal/Vertical", PaddingBottom = 25)]//-|
            [ShowInInspector][ReadOnly][Required]
            public Weapon defaultWeapon;

            #if UNITY_EDITOR
                                                                                               [HorizontalGroup("Weapon/DefaultWeapon/Horizontal", width:150)]
            [Button(name:"Set")]
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
            [ReadOnly][Required]
            public Weapon curWeapon;
            
            #if UNITY_EDITOR
                                                                                [HorizontalGroup("Weapon/CurWeapon", width:150)]
            [Button(name:"Set")]
            void SetCurWeapon(int index)
            {
                //���� ����
                if (transform.childCount == 0) return;

                //���� ����
                if (index == -1)
                {
                    curWeapon.state = WeaponState.INVENTORY;
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
            public List<Weapon> weapons = new List<Weapon>();

            [HorizontalGroup("Weapon/Inventory/Horizontal", width: 30)]
            [VerticalGroup("Weapon/Inventory/Horizontal/Manage")]
            #region Vertical Manage  - - - - - - - - - - - - - - - - - - - - - - - - - - - -|       
            
                [DelayedProperty][HideLabel]
                public int inventorySize = 0;

                #if UNITY_EDITOR
                                                                                             [VerticalGroup("Weapon/Inventory/Horizontal/Manage")]
                [HideLabel]
                [Button(name: "Clear", Stretch = true)]
                void ClearInventory()
                {
                    //�⺻ ���� ����
                    if (defaultWeapon != null) Utility.Destroy(defaultWeapon.gameObject);//-|

                    //�κ��丮 ���� ����
                    for (int i = weapons.Count - 1; i >= 0; i--)
                    {
                        if (weapons[i] == null) continue;
                            
                        Utility.Destroy(weapons[i].gameObject);
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
                    AddWeapon(Weapon.SpawnWeapon(name));
                }
                                                                                               [HorizontalGroup("Weapon/Inventory/Manage")]
                [Button(name: "Destroy")]
                void DestroyWeaponInInspector(int index)
                {
                    if (index == -1)
                    {
                        if (defaultWeapon == null) return;

                        if (EditorApplication.isPlaying) Destroy(defaultWeapon.gameObject);//-|
                        else DestroyImmediate(defaultWeapon.gameObject);

                        defaultWeapon = null;
                    }
                    else
                    {
                        if (weapons[index] == null)
                        {
                            weapons.RemoveAt(index);
                            return;
                        }

                        weapons[index].Destroy();
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
        public void AddWeapon(Weapon weapon)
        {
            if (weapon.state == WeaponState.HOLD
                || weapon.state == WeaponState.INVENTORY)
            {
                Debug.LogWarning("���� �κ��丮���� ���ŵ� �� �߰��ؾ���");
                return;
            } //LogWarning: ���� �κ��丮���� ���ŵ� �� �߰��ؾ��� >> return

            if (weapons.Contains(weapon))
            {
                Debug.LogWarning("�̹� �κ��丮�� ����");
                return;
            } //LogWarning: �̹� �κ��丮�� ���� >> return

            if (weapons.Count > inventorySize)
            {
                Debug.LogWarning("�κ��丮�� �� ��");
                if (weapon.state == WeaponState.PREFAB) weapon.Destroy();
                return;
            } //LogWarning: �κ��丮�� �� �� >> return


            weapon.con = this;

            weapon.transform.parent = weaponHolder;
        #if UNITY_EDITOR
            weapon.parent = weaponHolder;
        #endif
            weapon.state = WeaponState.INVENTORY;

            weapons.Add(weapon);
        }

        /// <summary>
        /// ���� ���
        /// HOLD -> INVENTORY -> REMOVED -> ITEM >>  //
        /// </summary>
        /// <param name="weapon"></param>
        public void DropWeapon(Weapon weapon)
        {
            Item item = null;

            if (weapon.state == WeaponState.ITEM)
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

            if (weapon.state == WeaponState.HOLD)
            {
                weapon.SetUse(false);

            } //HOLD -> INVENTORY

            else if (weapon.state == WeaponState.INVENTORY)
            {

            } //

            int index = weapons.IndexOf(weapon);
            weapon.OnWeaponRemoved();

            weapons.Remove(weapon);

            weapon.transform.parent = null;
            weapon.state = WeaponState.REMOVED;

            //���� ����
            if (index == weapons.Count) //������ ������ ������ ��
            {
                if (weapons.Count == 0)
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

            if (curWeapon != null && curWeapon.state == WeaponState.HOLD) curWeapon.SetUse(false); //���� ���� ��Ȱ��ȭ

            curWeapon = weapon;
            if (curWeapon.state == WeaponState.INVENTORY) weapon.SetUse(true); //������ ���� Ȱ��ȭ
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
    void LateUpdate()
    {
        AutoDebug();
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
        WeaponData[] weaponDatas = new WeaponData[weapons.Count];

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
        foreach (WeaponData weaponData in data.weaponDatas)
        {
            AddWeapon(Weapon.LoadWeapon(weaponData));
        }
        SelectWeapon(weapons[data.curWeaponIndex]);
    }

    
    private void OnDrawGizmosSelected()
    {
        //LastMoveVector
        if(moveVector == Vector2.zero)
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

        ControllerOnDrawGizmosSelected();
    }
    protected virtual void ControllerOnDrawGizmosSelected() { }
}