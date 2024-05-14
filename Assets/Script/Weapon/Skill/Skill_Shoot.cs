using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Skill_Shoot : Skill
{
    [SerializeField]
    float spinSpeed;
    [SerializeField]
    float throwSpeed;
    [SerializeField]
    float duration;

    [SerializeField]
    Pool pool;

    [FoldoutGroup("Event")]
    #region Foldout Event - - - - - - - - - - - - - - - - - - - - -|

        public UnityEvent<TriggerObject> startEvent;
                                                                    [FoldoutGroup("Event")]
        public UnityEvent<TriggerObject> updateEvent;
                                                                    [FoldoutGroup("Event")]
        public UnityEvent<TriggerObject> endEvent;
                                                                    [FoldoutGroup("Event")]
        public UnityEvent<TriggerObject, Collider2D> enterEvent;//-|
                                                                    [FoldoutGroup("Event")]
        public UnityEvent<TriggerObject, Collider2D> stayEvent;
                                                                    [FoldoutGroup("Event")]
        public UnityEvent<TriggerObject, Collider2D> exitEvent;

    #endregion  - - - - - - - - - - - - - - - - - - - - - - - - - -|

    protected override void Init()
    {
        if (Editor.GetType(Editor.StateType.IsPlay))
        {
            pool.Init();
        }
    }
    public override void Execute()
    {
        StartCoroutine(Shoot());
    }
    IEnumerator Shoot()
    {
        TriggerObject triggerObj = pool.Use().GetComponent<TriggerObject>();

        triggerObj.SetEvent(enterEvent, stayEvent, exitEvent);
        triggerObj.gameObject.SetActive(true);
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
