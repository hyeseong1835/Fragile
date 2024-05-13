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

        [HorizontalGroup("Pool/Info", width: Editor.propertyHeight * 3)]
        #region Horizontal Info - - - - - - - - - - - - - - - - - - - -|

            [HideLabel]
            [PreviewField(height: Editor.propertyHeight * 3)]
            public GameObject prefab;

            [VerticalGroup("Pool/Info/Vertical")]
            #region Vertical - - - - - - - - - - - - -|

            [LabelWidth(Editor.propertyLabelWidth)]//-|
            public GameObject holder;
                                                       [VerticalGroup("Pool/Info/Vertical")]
            [LabelWidth(Editor.propertyLabelWidth)]
            public float stayCount;
                                                       [VerticalGroup("Pool/Info/Vertical")]
            [LabelWidth(Editor.propertyLabelWidth)]
            public float destroyDelay;

            #endregion - - - - - - - - - - - - - - - -|

        #endregion  - - - - - - - - - - - - - - - - - - - - - - - - - -|

        [HorizontalGroup("Pool/Object")]
        #region Horizontal Object  - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -|
    
            public List<GameObject> pool = new List<GameObject>();
                                                                                                                     [HorizontalGroup("Pool/Object")]
            [ShowInInspector]
            public List<(GameObject obj, Coroutine coroutine)> waitDestroy = new List<(GameObject, Coroutine)>();//-|

        #endregion - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -|

    #endregion - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -|



    public Pool(GameObject _prefab, int _stayCount, float _destroyDelay, int defaultCount = 0)
    {
        //_prefab
        holder = new GameObject(_prefab.name);
        holder.transform.SetParent(poolHolder);
        prefab = _prefab;

        //_stayCount
        stayCount = _stayCount;

        //destroyDelay
        destroyDelay = _destroyDelay;

        //defaultCount
        for (int i = 0; i < defaultCount; i++)
        {
            Add();
        }

        PoolManager.pools.Add(this);
    }
    ~Pool()
    {
        Debug.Log($"Pool({prefab.name}) 파괴됨");
        PoolManager.pools.Remove(this);
        Utility.AutoDestroy(holder.gameObject);
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

[ExecuteAlways]
public class PoolManager : MonoBehaviour
{
    public static PoolManager instance;

    [ShowInInspector]
    [TableList(HideToolbar = true, AlwaysExpanded = true, ShowPaging = false)]
    public static List<Pool> pools = new List<Pool>();
    
    #if UNITY_EDITOR
    [Button]
    void NewPool(GameObject prefab, int stayCount, float destroyDelay, int defaultCount)
    {
        new Pool(prefab, stayCount, destroyDelay, defaultCount);
    }
    #endif

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
