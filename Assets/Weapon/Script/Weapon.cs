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
        NULL, Item, Hold, Inventory, Removed
    }
    //[ExecuteAlways]
    public class Weapon : MonoBehaviour
    {
        #region ���� ��� - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -|
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

            Weapon weapon = weaponObj.GetComponent<Weapon>();
            weapon.state = WeaponState.NULL;

            return weapon;
        }
        public static Weapon LoadWeapon(WeaponSaveData data)
        {
            Weapon weapon = Spawn(data.name);
            weapon.SetData(data);

            return weapon;
        }

        #endregion  - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -|

        public Controller con;

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

        public string Name { get { return gameObject.name; } }

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
        UpdateEventNode.Trigger(gameObject, state);

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
    }

    #region �̺�Ʈ

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

    #endregion

    #region ������

    public virtual WeaponSaveData GetData()
        {
            return new WeaponSaveData
                (
                    Name,
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
            if (state == WeaponState.NULL)
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
                //state = WeaponState.Prefab;
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

                    //parent = transform.parent;
                    //prevChildIndex = transform.GetSiblingIndex();
                }
            }
        }
        #endregion
        #endif
    }
