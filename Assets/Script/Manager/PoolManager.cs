using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class Pool
{
    #region 정적 필드  - - - - - - - - - -|

    public static Transform poolHolder;//-|

    #endregion - - - - - - - - - - - - - -|


    [VerticalGroup("Pool")]
    #region Pool - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -|

    #if UNITY_EDITOR
    [SerializeField]
    string name;
#endif

    [HorizontalGroup("Pool/Info")]
    #region Horizontal Info

    public GameObject prefab;
                                                        [HorizontalGroup("Pool/Info", width: 130)]
    [VerticalGroup("Pool/Info/Vertical")]
    #region Vertical

    [LabelWidth(100)]
    public float stayCount;
                                                    [VerticalGroup("Pool/Info/Vertical")]
    [LabelWidth(100)]
    public float destroyDelay;

    #endregion
                                                                    [VerticalGroup("Pool")]
    public List<GameObject> pool = new List<GameObject>();
                                                                                        [VerticalGroup("Pool")]
    [ShowInInspector]
    public Dictionary<GameObject, Coroutine> waitDestroy = new Dictionary<GameObject, Coroutine>();

    [HideInInspector]
    public GameObject lastWaitDestroyObj;

    #endregion


    Transform holder;
    public int count { get { return pool.Count; } }

    #endregion

    public Pool(string _name, GameObject _prefab, float defaultCount, float _destroyDelay)
    {
        //_name
        #if UNITY_EDITOR
        name = _name;
        #endif
        GameObject holderObj = new GameObject(_name);
        holderObj.transform.SetParent(poolHolder);

        //_prefab
        prefab = _prefab;
        
        //defaultCount
        holder = holderObj.transform;
        for (int i = 0; i < defaultCount; i++)
        {
            GameObject obj = UnityEngine.Object.Instantiate(prefab, holder);
            pool.Add(obj);
        }
        
        //destroyDelay
        destroyDelay = _destroyDelay;

        #if UNITY_EDITOR
        PoolManager.pools.Add(this);
        #endif
    }
    ~Pool()
    {
        #if UNITY_EDITOR
        PoolManager.pools.Remove(this);
        #endif
        Utility.Destroy(holder.gameObject);
    }
    #if UNITY_EDITOR
    public void Destroy() 
    {
        PoolManager.pools.Remove(this);
        Utility.Destroy(holder.gameObject);
    }
    #endif
    public GameObject Use()
    {
        if (waitDestroy.Count > 0) CancelInvokeDestroy(lastWaitDestroyObj);
        foreach(GameObject gameObject in pool)
        {
            if(gameObject.activeInHierarchy == false)
            {
                gameObject.SetActive(true);
                return gameObject;
            }
        }
        GameObject obj = UnityEngine.Object.Instantiate(prefab, holder);
        pool.Add(obj);
        return obj;
    }
    public GameObject DeUse(ref GameObject obj)
    {
        //초과 상태일 때
        if (count > stayCount)
        {
            InvokeDestroy(obj, destroyDelay);

            return null;
        }
        else
        {
            obj.SetActive(false);
            obj = prefab;

            return obj;
        }
    }
    public void InvokeDestroy(GameObject obj, float time)
    {
        waitDestroy.Add(obj, PoolManager.instance.StartCoroutine(DelayDestroy(obj, time)));
        lastWaitDestroyObj = obj;
    }
    IEnumerator DelayDestroy(GameObject obj, float time)
    {
        yield return new WaitForSeconds(time);
        UnityEngine.Object.Destroy(obj);
    }
    public void CancelInvokeDestroy(GameObject obj)
    {
        PoolManager.instance.StopCoroutine(waitDestroy[obj]);
        waitDestroy.Remove(obj);
    }
}

[ExecuteAlways]
public class PoolManager : MonoBehaviour
{
    public static PoolManager instance;

    #if UNITY_EDITOR
    [ShowInInspector]
    [TableList(HideToolbar = true, AlwaysExpanded = true, ShowPaging = false)]
    public static List<Pool> pools = new List<Pool>();
    
    [Button]
    void NewPool(string name, GameObject prefab, float defaultCount, float destroyDelay)
    {
        new Pool(name, prefab, defaultCount, destroyDelay);
    }
    [Button]
    void DeletePool(int index)
    {
        pools[index].Destroy();
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
