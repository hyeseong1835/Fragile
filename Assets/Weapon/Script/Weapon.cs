using JetBrains.Annotations;
using Sirenix.OdinInspector;
using System;
using Unity.VisualScripting;
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
        NULL, Prefab, Item, Hold, Inventory, Removed
    }
    [ExecuteAlways]
    public class Weapon : MonoBehaviour
    {
        #region ���� ��� - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -|
        public static Weapon SpawnWeapon(string weaponName, Transform parent = null)//-|
        {
            GameObject weaponObj;
            if (parent != null)
            {
                weaponObj = Instantiate(
                Resources.Load<GameObject>("WeaponObjPrefab/" + weaponName),
                    parent
                );
            }
            else
            {
                weaponObj = Instantiate(
                Resources.Load<GameObject>("WeaponObjPrefab/" + weaponName)
                );
            }

            Weapon weapon = weaponObj.GetComponent<Weapon>();
            weapon.state = WeaponState.NULL;

            return weapon;
        }
        public static Weapon LoadWeapon(WeaponSaveData data)
        {
            Weapon weapon = SpawnWeapon(data.name);
            weapon.SetData(data);

            return weapon;
        }

        #endregion  - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -|

        [ReadOnly] public Controller con;

        public WeaponState state = WeaponState.NULL;

        public Sprite UISprite;
        [Required] public Transform hand_obj;
        
        public float damage = 1;

        public int durability = 1;
        public int maxDurability = 1;

        [ShowInInspector]
        public ActiveSkill attack = new ActiveSkill();

        [ShowInInspector]
        public ActiveSkill special = new ActiveSkill();

        public string moduleName { get { return gameObject.name; } }

#if UNITY_EDITOR

    [HideInInspector]
        public Transform parent = null;

        int prevChildIndex;

#endif

    void Awake()
    {
            /*
        #if UNITY_EDITOR
            //���� ����
            if (con == null)
            {
                DropDown();
            }
            #else
            if (con != null) state = WeaponState.Inventory;
            //���� �ʿ�
        #endif
            */
    }
    void Start()
    {

    }

    void Update()
    {
        Attack();

        /*
#if UNITY_EDITOR

            if (Editor.GetObjectState(gameObject) == Editor.ObjectState.PrefabEdit)
            {
                if (state != WeaponState.Prefab) state = WeaponState.Prefab;
            }
            else
            {
                if (state == WeaponState.Prefab && transform.parent != null)
                {
                    transform.parent.position = transform.position;
                    state = WeaponState.Item;
                } //��Ӵٿ� >> �θ� ��ġ ����(1ȸ)
                StateAffix();
                AutoDebug();
            }

#endif
            */
        WeaponUpdate();

        switch (state)
        {
            case WeaponState.Hold:
                OnUseUpdate();
                break;
            case WeaponState.Inventory:
                DeUseUpdate();
                break;
        }
    }

        #region �̺�Ʈ

        //�⺻
        protected virtual void WeaponUpdate() { /*
            if (Editor.GetType(Editor.StateType.IsEditor))
            {
                //AttackSet Resize
                if (attack.skillSet.GetLength(1) != attack.skills.Length)
                {
                    bool[,] temp = attack.skillSet;
                    attack.skillSet = new bool[attack.skillSet.GetLength(0), attack.skills.Length];

                    for (int x = 0; x < attack.skillSet.GetLength(0); x++)
                    {
                        for (int y = 0; y < Utility.Smaller(attack.skillSet.GetLength(1), temp.GetLength(1)); y++)
                        {
                            attack.skillSet[x, y] = temp[x, y];
                        }
                    }
                }

                //SpecialSet Resize
                if (special.skillSet.GetLength(1) != special.skills.Length)
                {
                    bool[,] temp = special.skillSet;
                    special.skillSet = new bool[special.skillSet.GetLength(0), special.skills.Length];

                    for (int x = 0; x < special.skillSet.GetLength(0); x++)
                    {
                        for (int y = 0; y < Utility.Smaller(special.skillSet.GetLength(1), temp.GetLength(1)); y++)
                        {
                            special.skillSet[x, y] = temp[x, y];
                        }
                    }
                }
            }
            */ }
        protected virtual void OnUseUpdate() { }
        protected virtual void DeUseUpdate() { }

        //����
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
        protected virtual void Break()
        {
            WeaponDestroy();
        }
        public virtual void OnWeaponRemoved()
        {
            Destroy(hand_obj.gameObject);
            con.hand.HandLink(null);
        }
        protected virtual void OnWeaponDestroyed() { }

    //��ų
    public void Attack()
    {
        Debug.Log($"{gameObject}: Attack");
        CustomEvent.Trigger(gameObject, "Attack");
    }
    public virtual void Special() 
    { 
        CustomEvent.Trigger(gameObject, "Special");
    }

    #endregion

    #region ������

    public virtual WeaponSaveData GetData()
        {
            return new WeaponSaveData
                (
                    moduleName,
                    durability,
                    null
                );
        }
        public virtual void SetData(WeaponSaveData data)
        {
            durability = data.durability;
        }

        #endregion

        #region ���� ����

        /// <summary>
        /// true: INVENTORY -> HOLD //
        /// false: HOLD -> INVENTORY //
        /// </summary>
        /// <param name="use"></param>
        public void SetUse(bool use)
        {
            if (state == WeaponState.Prefab)
            {
                Debug.LogError("���⸦ Ȱ��ȭ�� �� �����ϴ�.");
                return;
            }// LogError: ���⸦ Ȱ��ȭ�� �� �����ϴ�. >> return
            if (state == WeaponState.Item)
            {
                Debug.LogError("���⸦ Ȱ��ȭ�� �� �����ϴ�.");
                return;
            } //LogError: ���⸦ Ȱ��ȭ�� �� �����ϴ� >> return

            if (use)
            {
                if (state == WeaponState.Removed)
                {
                    Debug.LogError("���⸦ Ȱ��ȭ�� �� �����ϴ�.");
                    return;
                } //LogError: ���⸦ Ȱ��ȭ�� �� �����ϴ�. >> return

                if (state == WeaponState.Hold)
                {
                    Debug.LogWarning("�̹� Ȱ��ȭ�Ǿ����ϴ�.");
                } //LogWarning: �̹� Ȱ��ȭ�Ǿ����ϴ�.

                OnUse();

                state = WeaponState.Hold;
            }
            else
            {
                if (state == WeaponState.Inventory)
                {
                    Debug.LogWarning("�̹� ��Ȱ��ȭ�Ǿ����ϴ�.");
                } //LogWarning: �̹� ��Ȱ��ȭ�Ǿ����ϴ�.

                OnDeUse();

                state = WeaponState.Inventory;
            }
        }

        public void AddDurability(int add)
        {
            durability += add;
            if (durability <= 0)
            {
                Break();
                return;
            }
        }
        /// <summary>
        /// ���⸦ �ı��ϴ� �Լ�
        /// INVENTORY -> HOLD -> REMOVED >> OnWeaponDestroy() >> Destroy(gameObject) //
        /// ITEM >> OnWeaponDestroy(item) >> Destroy //
        /// PREFAB >> DestroyImmediate(gameObject) //
        /// </summary>
        public void WeaponDestroy()
        {
            //PREFAB >> Destroy(gameObject) >> return
            if (state == WeaponState.Prefab)
            {
                DestroyImmediate(gameObject);
                return;
            }

            //ITEM >> Destroy(parent) >> return
            if (state == WeaponState.Item)
            {
                Utility.AutoDestroy(transform.parent.gameObject);
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
                Utility.AutoDestroy(gameObject);
                return;
            }

            Debug.LogError("���� ���°� ��ȿ���� �ʽ��ϴ�.");
        }

        #endregion

#if UNITY_EDITOR
        #region ������ ����

        void DropDown()
        {
            if (Editor.GetObjectState(gameObject) == Editor.ObjectState.PrefabEdit)
            {
                state = WeaponState.Prefab;
            }
            else
            {
                //���信 �÷��� ���� >> ������ȭ
                if (transform.parent == null) ItemManager.WrapWeaponInItem(this);

                //���̾��Ű�� ��Ӵٿ� >> Controller�� �߰�
                else
                {
                    if (transform.parent.gameObject.name != Controller.weaponHolderName)
                    {
                        Debug.LogError("�������� �ƴ� ����� �׻� \"" + Controller.weaponHolderName + "\" �ȿ� �־�� �մϴ�.");
                        return;
                    } //LogError: �������� �ƴ� ����� �׻� "{Controller.weaponHolderName}" �ȿ� �־�� �մϴ�. >> return


                    con = transform.parent.parent.GetComponent<Controller>();

                    if (con.weapons.Count >= con.inventorySize)
                    {
                        Debug.LogWarning("�κ��丮�� ������.");

                        Utility.AutoDestroy(gameObject);
                    } //LogWarning: �κ��丮�� ������. >> ����

                    con.AddWeapon(this);

                    parent = transform.parent;
                    prevChildIndex = transform.GetSiblingIndex();
                }
            }
        }
        void StateAffix()
        {
            switch (state)
            {
                case WeaponState.Prefab:
                    gameObject.name = moduleName + "[Prefab]";
                    break;
                case WeaponState.Hold:
                    if (this == con.defaultWeapon) gameObject.name = $"[Hold] {moduleName}({con.gameObject.name}[Default])";
                    else gameObject.name = $"[Hold] {moduleName}({con.gameObject.name})";
                    break;
                case WeaponState.Inventory:
                    if (this == con.defaultWeapon) gameObject.name = $"{moduleName}({con.gameObject.name}[Default])";
                    else gameObject.name = $"{moduleName}({con.gameObject.name})";
                    break;
                case WeaponState.Item:
                    gameObject.name = $"[Item] {moduleName}";
                    break;
                case WeaponState.Removed:
                    gameObject.name = $"[Removed({con.gameObject.name})] {moduleName}";
                    break;
                case WeaponState.NULL:
                    gameObject.name = $"[NULL] {moduleName}";
                    break;
            }
        }
        public void AutoDebug()
        {
            switch (state)
            {
                case WeaponState.Item:
                    // �θ� �˻�
                    if (transform.parent == null || transform.parent.gameObject.name != "Item(" + moduleName + ")")
                    {
                        Debug.LogError("�θ� ��ȿ���� �ʽ��ϴ�.");
                        break;
                    } //LogError: �θ��� �̸��� �ùٸ��� �ʽ��ϴ�.

                    //��ġ ����
                    if (transform.localPosition != Vector3.zero)
                    {
                        transform.localPosition = Vector3.zero;

                        Debug.LogWarning("������ ��ġ�� ����� �� �����ϴ�.");
                        return;
                    } //LogWarning: ������ ��ġ�� ����� �� �����ϴ�.

                    break;

                case WeaponState.Hold:
                    //�θ� ���� ����
                    if (transform.parent != parent)
                    {
                        Debug.LogWarning("���̾��Ű���� ���⸦ �ٸ� ������Ʈ�� �ű� �� �����ϴ�.");

                        transform.parent = parent;
                        transform.SetSiblingIndex(prevChildIndex);
                    } //LogWarning: ���̾��Ű���� ���⸦ �ٸ� ������Ʈ�� �ű� �� �����ϴ�.

                    prevChildIndex = transform.GetSiblingIndex();

                    break;

                case WeaponState.Inventory:
                    //�θ� ���� ����
                    if (transform.parent != parent)
                    {
                        Debug.LogWarning("���̾��Ű���� ���⸦ �ٸ� ������Ʈ�� �ű� �� �����ϴ�.");

                        transform.parent = parent;
                        transform.SetSiblingIndex(prevChildIndex);
                    } //LogWarning: ���̾��Ű���� ���⸦ �ٸ� ������Ʈ�� �ű� �� �����ϴ�.

                    prevChildIndex = transform.GetSiblingIndex();

                    break;
            }
        }

        #endregion
#endif
    }
