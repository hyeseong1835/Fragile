using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class Pool
{
    #region 정적 필드  - - - - - - - - - - - -|

        public static Transform poolHolder;//-|

    #endregion - - - - - - - - - - - - - - - -|




    public int count { get { return pool.Count; } }
    
    [VerticalGroup("Pool", order: 0)]
    #region Pool - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -|

        [ShowInInspector][PropertyOrder(0)]
        [HideLabel]
        string name {
            get {
                if (prefab == null) return "";
                return prefab.name;
            } 
        }    

        [HorizontalGroup("Pool/Info", width: Editor.propertyHeight * 3, marginLeft: 5 , order: 1)]
        #region Horizontal Info - - - - - - - - - - - - - - - - - - - -|

            [HideLabel]
            [PreviewField(height: Editor.propertyHeight * 3)]
            public GameObject prefab;

            [VerticalGroup("Pool/Info/Vertical")]
            #region Vertical - - - - - - - - - - - - -|

            [ReadOnly]
            [LabelWidth(Editor.labelWidth)]//-|
            public GameObject holder;
                                                       [VerticalGroup("Pool/Info/Vertical")]
            [LabelWidth(Editor.labelWidth)]
            public float stayCount;
                                                       [VerticalGroup("Pool/Info/Vertical")]
            [LabelWidth(Editor.labelWidth)]
            public float destroyDelay;

            #endregion - - - - - - - - - - - - - - - -|

        #endregion  - - - - - - - - - - - - - - - - - - - - - - - - - -|

        [HorizontalGroup("Pool/Object", marginLeft: 5, order: 2)]
        #region Horizontal Object  - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -|
    
            public List<GameObject> pool = new List<GameObject>();
                                                                                                                     [HorizontalGroup("Pool/Object")]
            [ShowInInspector]
            public List<(GameObject obj, Coroutine coroutine)> waitDestroy = new List<(GameObject, Coroutine)>();//-|

        #endregion - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -|

    #endregion - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -|



    public void Init()
    {
        //_prefab
        holder = new GameObject(prefab.name);
        holder.transform.SetParent(poolHolder);

        PoolManager.pools.Add(this);
    }
    ~Pool()
    {
        Debug.Log($"Pool({prefab.name}) 파괴됨");
        PoolManager.pools.Remove(this);
        holder.gameObject.AutoDestroy();
    }

    public GameObject Use()
    {
        //비활성화 오브젝트 찾기
        foreach(GameObject gameObject in pool)
        {
            if(gameObject.activeInHierarchy == false)
            {
                gameObject.SetActive(true);
                return gameObject;
            }
        }
        //파괴 예정 오브젝트가 있을 때
        if (waitDestroy.Count > 0)
        {
            return CancelInvokeDestroy(waitDestroy[0]);
        }
        //모두 사용 중일 때
        return Add();
    }
    public GameObject DeUse(ref GameObject obj)
    {
        //초과 상태일 때
        if (count > stayCount)
        {
            InvokeDestroy(obj);

            return null;//?
        }
        else
        {
            obj.SetActive(false);
            obj = prefab;

            return obj;
        }
    }
    public GameObject Add()
    {
        GameObject obj = UnityEngine.Object.Instantiate(prefab, holder.transform);
        pool.Add(obj);
        return obj;
    }
    public void InvokeDestroy(GameObject obj)
    {
        pool.Remove(obj);
        waitDestroy.Add((obj, PoolManager.instance.StartCoroutine(DelayDestroy(obj))));
    }
    IEnumerator DelayDestroy(GameObject obj)
    {
        yield return new WaitForSeconds(destroyDelay);
        UnityEngine.Object.Destroy(obj);
    }
    public GameObject CancelInvokeDestroy((GameObject obj, Coroutine coroutine) invokeDestroy)
    {
        PoolManager.instance.StopCoroutine(invokeDestroy.coroutine);
        waitDestroy.Remove(invokeDestroy);
        pool.Add(invokeDestroy.obj);
        return invokeDestroy.obj;
    }
}
public class PoolManager : MonoBehaviour
{
    public static PoolManager instance;

    [ShowInInspector]
    [TableList(HideToolbar = true, AlwaysExpanded = true, ShowPaging = false)]
    public static List<Pool> pools = new List<Pool>();

    void Awake()
    {
        instance = this;
        Pool.poolHolder = transform;
    }

    void Update()
    {
        #if UNITY_EDITOR
        if(instance == null) instance = this;
        if(Pool.poolHolder == null) Pool.poolHolder = transform;
        #endif        
    }


}
