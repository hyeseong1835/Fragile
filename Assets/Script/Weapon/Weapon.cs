using Sirenix.OdinInspector;
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
/// <summary>
/// NULL, PREFAB, ITEM, HOLD, INVENTORY, REMOVED
/// </summary>
public enum WeaponState
{
    NULL, PREFAB, ITEM, HOLD, INVENTORY, REMOVED
}
[ExecuteAlways]
public abstract class Weapon : MonoBehaviour
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

    [Required] public string weaponName;

    [ReadOnly]
    public WeaponState state = WeaponState.NULL;

    [FoldoutGroup("Object")]
    #region Foldout Object  - - - - - - - - - - - - - - - - - - - - - - - - - -|

        [Required]
        [LabelWidth(Utility.propertyLabelWidth)]
        public Sprite UI;
                                                                                [FoldoutGroup("Object")]
        #if UNITY_EDITOR
        [SerializeField][ChildGameObjectsOnly][HideIf(nameof(noDurability))]//-|
        #endif
        [LabelWidth(Utility.propertyLabelWidth)]
        protected BreakParticle breakParticle;
                                                                                [FoldoutGroup("Object")]
        [SerializeField][ChildGameObjectsOnly]
        [LabelWidth(Utility.propertyLabelWidth)]
        protected Transform hand_obj;

    #endregion  - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -|

    [FoldoutGroup("Stat")]
    #region Foldout Stat  - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -|

        [LabelWidth(Utility.propertyLabelWidth)]
        public float damage = 1;

        [HorizontalGroup("Stat/Durability")]
        #region Horizontal Durability - - - - - - - - - - - - - - - - - - - - - - - - - - - -|

            #if UNITY_EDITOR
            [DisableIf(nameof(noDurability))]
            #endif
            [LabelWidth(Utility.propertyLabelWidth)]
            [ProgressBar(0, nameof(maxDurability), Segmented = true, R = 1, G = 1, B = 1)]//-|
            public int durability = 1;
                                                                                              [HorizontalGroup("Stat/Durability", width: Utility.shortNoLabelPropertyWidth)]
            [HideLabel][LabelWidth(Utility.shortNoLabelPropertyWidth)]
            public int maxDurability = 1;

            #if UNITY_EDITOR
            bool noDurability { get { return maxDurability == -1; } }
            #endif

        #endregion  - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -|

    #endregion  - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -|

    [FoldoutGroup("Attack")]
    #region Foldout Attack  - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -|

        [LabelWidth(Utility.propertyLabelWidth)]
        public float attackFrontDelay;
                                                                                                                          [FoldoutGroup("Attack")]
        [LabelWidth(Utility.propertyLabelWidth)]
        public float attackDelay;
                                                                                                                          [FoldoutGroup("Attack")]
        [LabelWidth(Utility.propertyLabelWidth)] 
        public float attackBackDelay;
                                                                                                                          [FoldoutGroup("Attack")]
        [HorizontalGroup("Attack/AttackRange", width: Utility.shortNoLabelPropertyWidth + Utility.propertyLabelWidth)]//-|
        #region Horizontal Range - - - - - - - - - - - - - - - - - - - - - - - - - -|

            [SerializeField][PropertyOrder(0)]
            [LabelText("AttackRange")][LabelWidth(Utility.propertyLabelWidth)]                                      
            public float attackMinDistance = 0;                                                     
                                                                                                
            #if UNITY_EDITOR                                                                                
                                                                                     [HorizontalGroup("Attack/AttackRange")]                                              
            [ShowInInspector][PropertyOrder(1)]
            [HideLabel]            
            [MinMaxSlider(0, nameof(attackMaxDistance))]
            Vector2 attackRange {                                                                               
                get { return new Vector2(attackMinDistance, attackMaxDistance); }//-|
                set {
                    attackMinDistance = value.x;
                    attackMaxDistance = value.y; 
                }
            }
                                                                                                
            #endif                                                                      
                                                                                     [HorizontalGroup("Attack/AttackRange", width: 50)]                                   
            [SerializeField][HideLabel][PropertyOrder(2)]
            public float attackMaxDistance = 1;

        #endregion - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -|

    #endregion  - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -|

    [FoldoutGroup("Special")]
    #region Foldout Special - - - - - - - - - -|

    [LabelWidth(Utility.propertyLabelWidth)]//-|
    public float specialFrontDelay;
                                                [FoldoutGroup("Special")]
    [LabelWidth(Utility.propertyLabelWidth)]
    public float specialDelay;
                                                [FoldoutGroup("Special")]
    [LabelWidth(Utility.propertyLabelWidth)]
    public float specialBackDelay;

    #endregion  - - - - - - - - - - - - - - - -|

#if UNITY_EDITOR

    [HideInInspector] 
    public Transform parent = null;
    
    int prevChildIndex;
    
    #endif
    
    void Awake()
    {
#if UNITY_EDITOR
        //최초 생성
        if (con == null)
        {
            DropDown();
        }
    
    #endif

        WeaponAwake();
    }
    void Start()
    {
        WeaponStart();
    }
    void Update()
    {
    #if UNITY_EDITOR

        if (Utility.GetObjectState(gameObject) == Utility.ObjectState.PrefabEdit)
        {
            if (state != WeaponState.PREFAB)
            {
                state = WeaponState.PREFAB;
                Debug.LogWarning("상태를 변경할 수 없습니다.");
            } //LogWarning: 상태를 변경할 수 없습니다.
        }
        else
        {
            switch (state)
            {
                case WeaponState.PREFAB:
                    if (transform.parent != null)
                    {
                        transform.parent.position = transform.position;
                        state = WeaponState.ITEM;
                    } //드롭다운 >> 부모 위치 수정(1회)
                    break;
                case WeaponState.HOLD:
                    WeaponUpdate();
                    break;
                case WeaponState.INVENTORY:
                    WeaponBackGroundUpdate();
                    break;
            }
            StateAffix();
            AutoDebug();
        }
    #endif
    }
    void OnDrawGizmos()
    {
        WeaponOnDrawGizmos();
    }

    #region 이벤트
    
        //기본
        protected virtual void WeaponAwake() { }
        protected virtual void WeaponStart() { }
        protected virtual void WeaponUpdate() { }
        protected virtual void WeaponBackGroundUpdate() { }
        protected virtual void WeaponOnDrawGizmos() { }

        //무기
        protected virtual void OnUse() { }
        protected virtual void OnDeUse() { }
        protected virtual void Break() { }
        public virtual void OnWeaponRemoved() { }
        public virtual void OnWeaponDestroyed() { }

        //입력
        public abstract void Attack();
        public abstract void Special();

    #endregion

    #region 데이터
    
    public virtual WeaponData GetData()
    {
        return new WeaponData
            (
                weaponName,
                durability,
                null
            );
    }
    public virtual void SetData(WeaponData data)
    {
        weaponName = data.name;
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
            if (state == WeaponState.PREFAB)
            {
                Debug.LogError("무기를 활성화할 수 없습니다.");
                return;
            }// LogError: 무기를 활성화할 수 없습니다. >> return
            if (state == WeaponState.ITEM)
            {
                Debug.LogError("무기를 활성화할 수 없습니다.");
                return;
            } //LogError: 무기를 활성화할 수 없습니다 >> return
        
            if (use)
            {
                if(state == WeaponState.REMOVED)
                {
                    Debug.LogError("무기를 활성화할 수 없습니다.");
                    return;
                } //LogError: 무기를 활성화할 수 없습니다. >> return

                if (state == WeaponState.HOLD)
                {
                    Debug.LogWarning("이미 활성화되었습니다.");
                } //LogWarning: 이미 활성화되었습니다.

                OnUse();
         
                state = WeaponState.HOLD;
            }
            else
            {
                if (state == WeaponState.INVENTORY)
                {
                    Debug.LogWarning("이미 비활성화되었습니다.");
                } //LogWarning: 이미 비활성화되었습니다.

                OnDeUse();
        
                state = WeaponState.INVENTORY;
            }
        }

        public void AddDurability(int add)
        {
            if (durability == -1) return;

            durability += add;
            if (durability == 0)
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
        public void Destroy()
        {
            //PREFAB >> Destroy(gameObject) >> return
            if (state == WeaponState.PREFAB)
            {   
                DestroyImmediate(gameObject);
                return;
            }

            //ITEM >> Destroy(parent) >> return
            if (state == WeaponState.ITEM)
            {
                Utility.Destroy(transform.parent.gameObject);
                return;
            }

            //HOLD -> INVENTORY -> REMOVED ->
            if (state == WeaponState.HOLD
                || state == WeaponState.INVENTORY)
            {
                con.RemoveWeapon(this);
            }

            // REMOVED >> OnWeaponDestroy(item) >> Destroy(gameObject) >> return
            if (state == WeaponState.REMOVED)
            {
                OnWeaponDestroyed();
                Utility.Destroy(gameObject);
                return;
            }

            Debug.LogError("무기 상태가 유효하지 않습니다.");
        }

    #endregion

    #if UNITY_EDITOR
    #region 에디터 편의

        void DropDown()
        {
            if (Utility.GetObjectState(gameObject) == Utility.ObjectState.PrefabEdit)
            {
                state = WeaponState.PREFAB;
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

                        Utility.Destroy(gameObject);
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
                case WeaponState.PREFAB:
                    gameObject.name = weaponName + "[Prefab]";
                    break;
                case WeaponState.HOLD:
                    if (this == con.defaultWeapon) gameObject.name = $"[Hold] {weaponName}({con.gameObject.name}[Default])";
                    else gameObject.name = $"[Hold] {weaponName}({con.gameObject.name})";
                    break;
                case WeaponState.INVENTORY:
                    if (this == con.defaultWeapon) gameObject.name = $"[Hold] {weaponName}({con.gameObject.name}[Default])";
                    else gameObject.name = $"{weaponName}({con.gameObject.name})"; 
                    break;
                case WeaponState.ITEM:
                    gameObject.name = $"[Item] {weaponName}";
                    break;
                case WeaponState.REMOVED:
                    gameObject.name = $"[Removed({con.gameObject.name})] {weaponName}";
                    break;
                case WeaponState.NULL:
                    gameObject.name = $"[NULL] {weaponName}";
                    break;
            }
        }
        public void AutoDebug()
        {
            switch (state)
            {
                case WeaponState.ITEM:
                    // 부모 검사
                    if (transform.parent == null || transform.parent.gameObject.name != "Item(" + weaponName + ")")
                    {
                        Debug.LogError("부모가 유효하지 않습니다.");
                        break;
                    } //LogError: 부모의 이름이 올바르지 않습니다.

                    //위치 고정
                    if( transform.localPosition != Vector3.zero)
                    {
                        transform.localPosition = Vector3.zero;

                        Debug.LogWarning("무기의 위치는 변경될 수 없습니다.");
                        return;
                    } //LogWarning: 무기의 위치는 변경될 수 없습니다.

                    break;

                case WeaponState.HOLD:
                    //부모 변경 억제
                    if (transform.parent != parent)
                    {
                        Debug.LogWarning("하이어라키에서 무기를 다른 오브젝트로 옮길 수 없습니다.");

                        transform.parent = parent;
                        transform.SetSiblingIndex(prevChildIndex);
                    } //LogWarning: 하이어라키에서 무기를 다른 오브젝트로 옮길 수 없습니다.

                    prevChildIndex = transform.GetSiblingIndex();

                    break;

                case WeaponState.INVENTORY:
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