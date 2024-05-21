using JetBrains.Annotations;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

    public struct WeaponData
    {
        public string name;
        public int durability;
        public string weaponData;

        public WeaponData(string _name, int _durability)
        {
            name = _name;
            durability = _durability;
            weaponData = null;
        }
        public WeaponData(string _name, int _durability, string _weaponData)
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

        public Skill[] skills = new Skill[0];
        public bool[,] skillSet = new bool[1, 0];
    }
    /// <summary>
    /// NULL, PREFAB, ITEM, HOLD, INVENTORY, REMOVED
    /// </summary>
    public enum WeaponState
    {
        NULL, Prefab, Item, Hold, Inventory, Removed
    }
    [ExecuteAlways]
    public class Weapon : Module
    {
        #region 정적 멤버 - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -|
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
        public static Weapon LoadWeapon(WeaponData data)
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

#if UNITY_EDITOR

        [HideInInspector]
        public Transform parent = null;

        int prevChildIndex;

#endif

        public override void SpawnModule()
        {
            Transform weaponEvent = Spawn("Event", transform);
                Spawn("Passive", weaponEvent);
                Spawn("Attack", weaponEvent);
                Spawn("Special", weaponEvent);
                Spawn("NULL", weaponEvent);

            static Transform Spawn(string name, Transform parent)
            {
                Transform spawn = new GameObject(name).transform;
                spawn.parent = parent;
                return spawn;
            }
        }

        protected override void InitModule()
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
            WeaponAwake();
        }
        void Start()
        {
            WeaponStart();
        }
        void Update()
        {
#if UNITY_EDITOR

            if (Editor.GetObjectState(gameObject) == Editor.ObjectState.PrefabEdit)
            {
                moduleName = gameObject.name;
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
        void OnDrawGizmos()
        {
            WeaponOnDrawGizmos();
        }

        #region 이벤트

        //기본
        protected virtual void WeaponAwake() { }
        protected virtual void WeaponStart() { }
        protected virtual void WeaponUpdate()
        {
            /*
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
            */
        }
        protected virtual void OnUseUpdate()
        {
            foreach (Skill skill in attack.skills)
            {
                skill.OnUseUpdate();
            }
            foreach (Skill skill in special.skills)
            {
                skill.OnUseUpdate();
            }
        }
        protected virtual void DeUseUpdate()
        {
            foreach (Skill skill in attack.skills)
            {
                skill.DeUseUpdate();
            }
            foreach (Skill skill in special.skills)
            {
                skill.DeUseUpdate();
            }
        }
        protected virtual void WeaponOnDrawGizmos() { }

        //무기
        protected virtual void OnUse()
        {
            if (hand_obj != null)
            {
                hand_obj.gameObject.SetActive(true);
                con.hand.HandLink(hand_obj, HandMode.ToHand);
            }
            foreach (Skill skill in attack.skills)
            {
                skill.OnUse();
            }
            foreach (Skill skill in special.skills)
            {
                skill.OnUse();
            }
        }
        protected virtual void OnDeUse()
        {
            if (hand_obj != null)
            {
                hand_obj.gameObject.SetActive(false);
                con.hand.HandLink(null);
            }
            foreach (Skill skill in attack.skills)
            {
                skill.DeUse();
            }
            foreach (Skill skill in special.skills)
            {
                skill.DeUse();
            }
        }
        protected virtual void Break()
        {
            foreach (Skill skill in attack.skills)
            {
                skill.Break();
            }
            foreach (Skill skill in special.skills)
            {
                skill.Break();
            }
            WeaponDestroy();
        }
        public virtual void OnWeaponRemoved()
        {
            foreach (Skill skill in attack.skills)
            {
                skill.Removed();
            }
            foreach (Skill skill in special.skills)
            {
                skill.Removed();
            }

            Destroy(hand_obj.gameObject);
            con.hand.HandLink(null);
        }
        protected virtual void OnWeaponDestroyed()
        {
            foreach (Skill skill in attack.skills)
            {
                skill.Destroyed();
            }
            foreach (Skill skill in special.skills)
            {
                skill.Destroyed();
            }
        }

        //스킬
        public virtual void Attack() { }
        public virtual void Special() { }


        #endregion

        #region 데이터

        public virtual WeaponData GetData()
        {
            return new WeaponData
                (
                    moduleName,
                    durability,
                    null
                );
        }
        public virtual void SetData(WeaponData data)
        {
            moduleName = data.name;
            durability = data.durability;
        }

        #endregion

        #region 무기 관리

        /// <summary>
        /// true: INVENTORY -> HOLD //
        /// false: HOLD -> INVENTORY //
        /// </summary>
        /// <param name="use"></param>
        public void SetUse(bool use)
        {
            if (state == WeaponState.Prefab)
            {
                Debug.LogError("무기를 활성화할 수 없습니다.");
                return;
            }// LogError: 무기를 활성화할 수 없습니다. >> return
            if (state == WeaponState.Item)
            {
                Debug.LogError("무기를 활성화할 수 없습니다.");
                return;
            } //LogError: 무기를 활성화할 수 없습니다 >> return

            if (use)
            {
                if (state == WeaponState.Removed)
                {
                    Debug.LogError("무기를 활성화할 수 없습니다.");
                    return;
                } //LogError: 무기를 활성화할 수 없습니다. >> return

                if (state == WeaponState.Hold)
                {
                    Debug.LogWarning("이미 활성화되었습니다.");
                } //LogWarning: 이미 활성화되었습니다.

                OnUse();

                state = WeaponState.Hold;
            }
            else
            {
                if (state == WeaponState.Inventory)
                {
                    Debug.LogWarning("이미 비활성화되었습니다.");
                } //LogWarning: 이미 비활성화되었습니다.

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
        /// 무기를 파괴하는 함수
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

            Debug.LogError("무기 상태가 유효하지 않습니다.");
        }

        #endregion

#if UNITY_EDITOR
        #region 에디터 편의

        void DropDown()
        {
            if (Editor.GetObjectState(gameObject) == Editor.ObjectState.PrefabEdit)
            {
                state = WeaponState.Prefab;
            }
            else
            {
                //씬뷰에 플로팅 시작 >> 아이템화
                if (transform.parent == null) ItemManager.WrapWeaponInItem(this);

                //하이어라키에 드롭다운 >> Controller에 추가
                else
                {
                    if (transform.parent.gameObject.name != Controller.weaponHolderName)
                    {
                        Debug.LogError("아이템이 아닌 무기는 항상 \"" + Controller.weaponHolderName + "\" 안에 있어야 합니다.");
                        return;
                    } //LogError: 아이템이 아닌 무기는 항상 "{Controller.weaponHolderName}" 안에 있어야 합니다. >> return


                    con = transform.parent.parent.GetComponent<Controller>();

                    if (con.weapons.Count >= con.inventorySize)
                    {
                        Debug.LogWarning("인벤토리가 가득참.");

                        Utility.AutoDestroy(gameObject);
                    } //LogWarning: 인벤토리가 가득참. >> 제거

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
                    // 부모 검사
                    if (transform.parent == null || transform.parent.gameObject.name != "Item(" + moduleName + ")")
                    {
                        Debug.LogError("부모가 유효하지 않습니다.");
                        break;
                    } //LogError: 부모의 이름이 올바르지 않습니다.

                    //위치 고정
                    if (transform.localPosition != Vector3.zero)
                    {
                        transform.localPosition = Vector3.zero;

                        Debug.LogWarning("무기의 위치는 변경될 수 없습니다.");
                        return;
                    } //LogWarning: 무기의 위치는 변경될 수 없습니다.

                    break;

                case WeaponState.Hold:
                    //부모 변경 억제
                    if (transform.parent != parent)
                    {
                        Debug.LogWarning("하이어라키에서 무기를 다른 오브젝트로 옮길 수 없습니다.");

                        transform.parent = parent;
                        transform.SetSiblingIndex(prevChildIndex);
                    } //LogWarning: 하이어라키에서 무기를 다른 오브젝트로 옮길 수 없습니다.

                    prevChildIndex = transform.GetSiblingIndex();

                    break;

                case WeaponState.Inventory:
                    //부모 변경 억제
                    if (transform.parent != parent)
                    {
                        Debug.LogWarning("하이어라키에서 무기를 다른 오브젝트로 옮길 수 없습니다.");

                        transform.parent = parent;
                        transform.SetSiblingIndex(prevChildIndex);
                    } //LogWarning: 하이어라키에서 무기를 다른 오브젝트로 옮길 수 없습니다.

                    prevChildIndex = transform.GetSiblingIndex();

                    break;
            }
        }

        #endregion
#endif
    }
