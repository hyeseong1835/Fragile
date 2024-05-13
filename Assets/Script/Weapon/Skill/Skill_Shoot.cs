using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Events;

public class Skill_Shoot : Skill
{
    [SerializeField]
    GameObject prefab;

    [SerializeField]
    float spinSpeed;
    [SerializeField]
    float throwSpeed;
    [SerializeField]
    float duration;

    [SerializeField][ReadOnly(true)]
    Pool pool;
    [SerializeField]
    int poolStayCount = 0;
    [SerializeField]
    float poolDestroyDelay = 0;
    [SerializeField]
    int poolDefaultCount = 0;

    public UnityEvent<TriggerObject> startEvent;
    public UnityEvent<TriggerObject> updateEvent;
    public UnityEvent<TriggerObject> endEvent;

    public UnityEvent<TriggerObject, Collider2D> enterEvent;
    public UnityEvent<TriggerObject, Collider2D> stayEvent;
    public UnityEvent<TriggerObject, Collider2D> exitEvent;

    protected override void Init()
    {
        pool = new Pool(prefab, poolStayCount, poolDestroyDelay, poolDefaultCount);
    }
    public override void Execute()
    {
        StartCoroutine(Shoot());
    }
    IEnumerator Shoot()
    {
        TriggerObject triggerObj = pool.Use().GetComponent<TriggerObject>();

        triggerObj.gameObject.SetActive(true);
        triggerObj.SetEvent(enterEvent, stayEvent, exitEvent);
        triggerObj.transform.SetParent(null);
        triggerObj.transform.position = con.transform.position;

        Vector3 startVector = con.targetDir;
        float startAngleZ = Mathf.Atan2(startVector.y, startVector.x) * Mathf.Rad2Deg - 90;
        float time = 0;

        startEvent.Invoke(triggerObj);
        //실행 중
        while (time <= duration)
        {
            triggerObj.transform.position += startVector.normalized * throwSpeed * Time.deltaTime;

            float rotateZ = startAngleZ - time * spinSpeed;
            triggerObj.transform.rotation = Quaternion.Euler(0, 0, rotateZ);
            
            time += Time.deltaTime;

            updateEvent.Invoke(triggerObj);
            yield return null;
        }
        endEvent.Invoke(triggerObj);
    }
    
    public override void Break()
    {
        weapon.WeaponDestroy();
    }

    public override void Removed()
    {
        //풀 제거
    }

    public override void Destroyed()
    {

    }
}
