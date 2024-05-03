using Sirenix.OdinInspector;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.UI;

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
    [ReadOnly] public Controller con;

    [Required] public string weaponName;

    public WeaponState state = WeaponState.NULL;

    [Title("Object")]
    [Required]
    public Sprite UI;
    
    [SerializeField] protected BreakParticle breakParticle;


    [Title("Stat")]
    public float damage = 1;
    public float attackCooltime = 0;

    [HorizontalGroup("Durability")] 
        public int durability = 1;
    
    [HorizontalGroup("Durability", width: 150)]
        [HideLabel] 
        public int maxDurability = 1;

    public float attackFrontDelay;
    public float attackDelay;
    public float attackBackDelay;

    public float specialFrontDelay;
    public float specialDelay;
    public float specialBackDelay;

    [HorizontalGroup("AttackRange", width: 50)]
    #region Horizontal Range

        [SerializeField][HideLabel][PropertyOrder(0)]
            public float attackMinDistance = 0;

        #if UNITY_EDITOR
        [ShowInInspector][HideLabel][PropertyOrder(1)]                      [HorizontalGroup("AttackRange")]
        [MinMaxSlider(0, nameof(attackMaxDistance))] Vector2 attackRange
        {
            get { return new Vector2(attackMinDistance, attackMaxDistance); }
        
            set 
            {
                attackMinDistance = value.x;
                attackMaxDistance = value.y; 
            }
        }
        #endif
    
        [SerializeField][HideLabel][PropertyOrder(2)]                       [HorizontalGroup("AttackRange", width: 50)]
        public float attackMaxDistance = 1;

    #endregion


#if UNITY_EDITOR
    [HideInInspector] public Transform parent = null;
    int prevChildIndex;
#endif
    
    void Awake()
    {
#if UNITY_EDITOR
        //������ ���� â�� �ƴ� ���
        if (Utility.GetEditorState(gameObject: gameObject) != Utility.EditorState.PREFAB)
        {
            if (state == WeaponState.PREFAB)
            {
                //���̾��Ű�� ��Ӵٿ� >> Controller�� �߰�
                if (transform.parent != null)
                {
                    //�θ� WeaponHolder
                    if (transform.parent.gameObject.name == Controller.weaponHolderName)
                    {
                        con = transform.parent.parent.GetComponent<Controller>();

                        if (con.weapons.Count >= con.inventorySize)
                        {
                            Debug.LogWarning("�κ��丮�� ������.");

                            Utility.Destroy(gameObject);

                            return;
                        }
                        con.AddWeapon(this);

                        if (parent == null) parent = transform.parent;
                        prevChildIndex = transform.GetSiblingIndex();
                    }
                    else
                    {
                        Debug.LogError("�������� �ƴ� ����� �׻� \"WeaponHolder\"�ȿ� �־�� �մϴ�.");
                    } //LogError: �������� �ƴ� ����� �׻� "WeaponHolder"�ȿ� �־�� �մϴ�.
                }
                //���信 �÷��� ���� >> ������ȭ
                else
                {
                    ItemManager.WrapWeaponInItem(this);
                }
            }
        } //��� �ٿ� ��ġ�� ���� ���� ����
#endif
        WeaponAwake();
    }
    void Start()
    {
        WeaponStart();
    }
    void LateUpdate()
    {
        #if UNITY_EDITOR

        if (Utility.GetEditorState(gameObject: gameObject) != Utility.EditorState.PREFAB)
        {
            switch (state)
            {
                case WeaponState.PREFAB:

                    if (transform.parent != null)
                    {
                        transform.parent.position = transform.position;
                        state = WeaponState.ITEM;
                    } //��Ӵٿ� >> �θ� ��ġ ����(1ȸ)

                    gameObject.name = weaponName + "[Prefab]";
                    break;
                case WeaponState.HOLD:
                    gameObject.name = "[Hold] ";
                    gameObject.name += weaponName + "(" + con.gameObject.name;
                    if (this == con.defaultWeapon) gameObject.name += "(Default)";
                    gameObject.name += ")";

                    WeaponUpdate();
                    break;
                case WeaponState.INVENTORY:
                    gameObject.name = weaponName + "(" + con.gameObject.name;
                    if (this == con.defaultWeapon) gameObject.name += "(Default)";
                    gameObject.name += ")";

                    WeaponBackGroundUpdate();
                    break;
                case WeaponState.ITEM:
                    gameObject.name = weaponName + "[Item]";
                    break;
                case WeaponState.REMOVED:
                    gameObject.name = weaponName + "[Removed(" + con.gameObject.name + ")]";
                    break;
                case WeaponState.NULL:
                    gameObject.name = weaponName + "[NULL]";
                    break;
            }

            AutoDebug();
        }
        #endif
    }
    void OnDrawGizmos()
    {
        WeaponOnDrawGizmos();
    }
    protected virtual void WeaponAwake() { }
    protected virtual void WeaponStart() { }
    protected virtual void WeaponUpdate() { }
    protected virtual void WeaponBackGroundUpdate() { }
    protected virtual void WeaponOnDrawGizmos() { }

    #region �Է�

    public abstract void Attack();

    public abstract void Special();

    #endregion

    #region ������
    
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

    #region ����
    /// <summary>
    /// true: INVENTORY -> HOLD //
    /// false: HOLD -> INVENTORY //
    /// </summary>
    /// <param name="use"></param>
    public void SetUse(bool use)
    {
        if (state == WeaponState.PREFAB)
        {
            Debug.LogError("���⸦ Ȱ��ȭ�� �� �����ϴ�.");
            return;
        }// LogError: ���⸦ Ȱ��ȭ�� �� �����ϴ�. >> return
        if (state == WeaponState.ITEM)
        {
            Debug.LogError("���⸦ Ȱ��ȭ�� �� �����ϴ�.");
            return;
        } //LogError: ���⸦ Ȱ��ȭ�� �� �����ϴ� >> return
        
        if (use)
        {
            if(state == WeaponState.REMOVED)
            {
                Debug.LogError("���⸦ Ȱ��ȭ�� �� �����ϴ�.");
                return;
            } //LogError: ���⸦ Ȱ��ȭ�� �� �����ϴ�. >> return

            if (state == WeaponState.HOLD)
            {
                Debug.LogWarning("�̹� Ȱ��ȭ�Ǿ����ϴ�.");
            } //LogWarning: �̹� Ȱ��ȭ�Ǿ����ϴ�.

            OnUse();
         
            state = WeaponState.HOLD;
        }
        else
        {
            if (state == WeaponState.INVENTORY)
            {
                Debug.LogWarning("�̹� ��Ȱ��ȭ�Ǿ����ϴ�.");
            } //LogWarning: �̹� ��Ȱ��ȭ�Ǿ����ϴ�.

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
    /// ���⸦ �ı��ϴ� �Լ�
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
            Utility.Destroy(transform.parent.gameObject);
            return;
        }

        Debug.LogError("���� ���°� ��ȿ���� �ʽ��ϴ�.");
    }

#endregion

    #region �̺�Ʈ

    protected virtual void OnUse() { }
    protected virtual void OnDeUse() { }
    protected virtual void Break() { }
    public virtual void OnWeaponRemoved() { }
    public virtual void OnWeaponDestroyed() { }

    #endregion

#if UNITY_EDITOR
    public void AutoDebug()
    {
        switch (state)
        {
            // �θ� �˻�
            case WeaponState.ITEM:
                if (transform.parent == null)
                {
                    Debug.LogError("�θ� �����ϴ�.");
                    break;
                }

                if (transform.parent.gameObject.name != "Item(" + weaponName + ")")
                {
                    Debug.LogError("�θ��� �̸��� �ùٸ��� �ʽ��ϴ�.");
                    break;
                }//LogError: �θ��� �̸��� �ùٸ��� �ʽ��ϴ�.

                transform.localPosition = Vector3.zero;
                break;

            //�θ� ���� ����
            case WeaponState.HOLD:
                //�θ� ���� ����
                if (transform.parent != parent)
                {
                    Debug.LogWarning("���̾��Ű���� ���⸦ �ٸ� ������Ʈ�� �ű� �� �����ϴ�.");

                    transform.parent = parent;
                    transform.SetSiblingIndex(prevChildIndex);
                } //LogWarning: ���̾��Ű���� ���⸦ �ٸ� ������Ʈ�� �ű� �� �����ϴ�.

                break;

            //�θ� ���� ����
            case WeaponState.INVENTORY:
                //�θ� ���� ����
                if (transform.parent != parent)
                {
                    Debug.LogWarning("���̾��Ű���� ���⸦ �ٸ� ������Ʈ�� �ű� �� �����ϴ�.");

                    transform.parent = parent;
                    transform.SetSiblingIndex(prevChildIndex);
                } //LogWarning: ���̾��Ű���� ���⸦ �ٸ� ������Ʈ�� �ű� �� �����ϴ�.

                break;
        }
     
        prevChildIndex = transform.GetSiblingIndex();
    }
#endif
}

/*

    switch (state)
    {
        case WeaponState.PREFAB:
                
            break;

        case WeaponState.ITEM:
                
            break;

        case WeaponState.HOLD:
                
            break;

        case WeaponState.INVENTORY:

            break;

        case WeaponState.REMOVED:
                
            break;


        default:
            Debug.LogError("���� ���°� ��ȿ���� �ʽ��ϴ�.");
            return; //LogError: ���� ���°� ��ȿ���� �ʽ��ϴ�.
    }

*/