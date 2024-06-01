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
        public Texture2D breakTexture;
        [Required] public Transform hand_obj;
        
        public float damage = 1;

        public int durability = 1;
        public int maxDurability = 1;

        [ShowInInspector]
        public ActiveSkill attack = new ActiveSkill();

        [ShowInInspector]
        public ActiveSkill special = new ActiveSkill();

        public string weaponName { get { return gameObject.name; } }

    void Awake()
    {
            /*
        #if UNITY_EDITOR
            //최초 생성
            if (con == null)
            {
                DropDown();
            }
            #else
            if (con != null) state = WeaponState.Inventory;
            //수정 필요
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
                } //드롭다운 >> 부모 위치 수정(1회)
                StateAffix();
                AutoDebug();
            }

        #endif
            */
    }

    #region 이벤트

        //무기
        protected virtual void OnUse()
        {
            if (hand_obj != null)
            {
                hand_obj.gameObject.SetActive(true);
                con.grafic.hand.HandLink(hand_obj, HandMode.ToHand);
            }
        }
        protected virtual void OnDeUse()
        {
            if (hand_obj != null)
            {
                hand_obj.gameObject.SetActive(false);
                con.grafic.hand.HandLink(null);
            }
        }
        protected virtual void Break(Vector2 breakPos)
        {
            BreakParticle breakParticle = new GameObject("BreakParticle").AddComponent<BreakParticle>();
            breakParticle.AddComponent<ParticleSystem>();
            breakParticle.transform.parent = transform;

            WeaponDestroy();
        }
        public virtual void OnWeaponRemoved()
        {
            Destroy(hand_obj.gameObject);
            con.grafic.hand.HandLink(null);
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

        public void AddDurability(int add, Vector2 breakPos)
        {
            durability += add;
            if (durability <= 0)
            {
                Break(breakPos);
                return;
            }
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

            Debug.LogError("무기 상태가 유효하지 않습니다.");
        }

        #endregion

        #if UNITY_EDITOR
        #region 에디터 편의

        void DropDown()
        {
            if (Editor.GetObjectState(gameObject) == Editor.ObjectState.PrefabEdit)
            {
                //state = WeaponState.Prefab;
            }
            else
            {
                //씬뷰에 플로팅 시작 >> 아이템화
                if (transform.parent == null) ItemManager.WrapWeaponInItem(this);

                //하이어라키에 드롭다운 >> Controller에 추가
                else
                {
                    //위치 자동 수정-----------------------------------------------필요
                    con = transform.parent.parent.GetComponent<Controller>();

                    if (con.weapons.Count >= con.conData.inventorySize)
                    {
                        Debug.LogWarning("인벤토리가 가득참.");

                        Utility.AutoDestroy(gameObject);
                    } //LogWarning: 인벤토리가 가득참. >> 제거

                    con.AddWeapon(this);

                    //parent = transform.parent;
                    //prevChildIndex = transform.GetSiblingIndex();
                }
            }
        }
        #endregion
        #endif
    }
