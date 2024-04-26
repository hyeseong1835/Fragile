using Sirenix.OdinInspector;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.UI;
using static UnityEngine.UIElements.UxmlAttributeDescription;
using System.Collections.Generic;
using Unity.VisualScripting;

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

    public bool dropable = true;

    [Title("Object")]
    [ShowIf("dropable", true)]
    [Required]
        public Sprite item;
    [Required]
    public Sprite UI;

    [Title("Stat")]
    public float damage = 1;
    public float attackCooltime = 0;

    [HorizontalGroup("Durability")] 
        public int durability = 1;
    
    [HorizontalGroup("Durability", width: 150)]
        [HideLabel] 
        public int maxDurability = 1;

    [SerializeField] protected BreakParticle breakParticle = null;

    #if UNITY_EDITOR
    public Transform parent = null;
    int prevChildIndex;
    #endif

    void Awake()
    {
        #if UNITY_EDITOR
        //������ ���� â�� �ƴ� ���
        if (PrefabUtility.GetPrefabInstanceStatus(gameObject) != PrefabInstanceStatus.NotAPrefab
            || EditorApplication.isPlaying)
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

                            if (EditorApplication.isPlaying) Destroy(gameObject);
                            else DestroyImmediate(gameObject);

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
    void Update()
    {
        #if UNITY_EDITOR

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

        #endif

    }
    void LateUpdate()
    {
        #if UNITY_EDITOR
        
        #region �ڵ� �����
         
        AutoDebug();

        #endregion
        
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
    [DisableInEditorMode]
    [Button(ButtonStyle.Box)]
    public virtual void Attack() { }
    [DisableInEditorMode]
    [HorizontalGroup("Mouse0"), Button(ButtonStyle.Box)]
    public virtual void Mouse0Down() { }
    [DisableInEditorMode]
    [HorizontalGroup("Mouse0"), Button(ButtonStyle.Box)]
    public virtual void Mouse0() { }
    [DisableInEditorMode]
    [HorizontalGroup("Mouse0"), Button(ButtonStyle.Box)]
    public virtual void Mouse0Up() { }
    [DisableInEditorMode]
    [HorizontalGroup("Mouse1"), Button(ButtonStyle.Box)]
    public virtual void Mouse1Down() { }
    [DisableInEditorMode]
    [HorizontalGroup("Mouse1"), Button(ButtonStyle.Box)]
    public virtual void Mouse1() { }
    [DisableInEditorMode]
    [HorizontalGroup("Mouse1"), Button(ButtonStyle.Box)]
    public virtual void Mouse1Up() { }
    #endregion

    #region ������
    
    public abstract WeaponData GetData();
    public abstract void SetData(WeaponData data);

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
            if (EditorApplication.isPlaying) DestroyImmediate(gameObject);
            else DestroyImmediate(gameObject);
            return;
        }

        //ITEM >> Destroy(parent) >> return
        if (state == WeaponState.ITEM)
        {
            if (EditorApplication.isPlaying) Destroy(transform.parent.gameObject);
            else DestroyImmediate(transform.parent.gameObject);
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
            if (EditorApplication.isPlaying) Destroy(gameObject);
            else DestroyImmediate(gameObject);
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