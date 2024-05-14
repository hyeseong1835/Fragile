using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class Skill_Melee : Skill
{
    [SerializeField]
    TriggerObject triggerObj;

    #if UNITY_EDITOR
    bool isWeaponHasDurability { 
        get {
            if (weapon == null) weapon = GetComponent<Weapon>();
            return weapon.maxDurability != -1; 
        } 
    }
    #endif
    [SerializeField][Required][ShowIf(nameof(isWeaponHasDurability))]
    protected BreakParticle breakParticle;

    [SerializeField]
    float duration;
    [SerializeField]
    float spread;
    [SerializeField]
    float startSpear;
    [SerializeField]
    float spear;
     
    public enum SwingCurve { Linear, Quadratic }
    [SerializeField] SwingCurve curve;

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

        UnityEvent<TriggerObject, Collider2D> _enterEvent;//-|

    protected override void Init()
    {
        triggerObj.SetEvent(enterEvent, stayEvent, exitEvent);
    }
    public override void Execute()
    {
        StartCoroutine(Swing());
    }
    IEnumerator Swing()
    {
        if(weapon.hand_obj != null) weapon.hand_obj.gameObject.SetActive(false);

        triggerObj.gameObject.SetActive(true);
        con.hand.HandLink(triggerObj.transform, HandMode.ToTarget);

        triggerObj.transform.position = con.transform.position + (Vector3)con.center;

        float startRotateZ = Utility.Vector2ToDegree(con.targetDir);

        float time = 0;
        float t = 0;
        while (t < 1)
        {
            switch (curve)
            {
                case SwingCurve.Linear:
                    t = time;
                    break;
                case SwingCurve.Quadratic:
                    t = time * time;
                    break;
            }
            float rotateZ = startRotateZ + spread * 0.5f - t * spread - 90;
            triggerObj.transform.rotation = Quaternion.Euler(0, 0, rotateZ);
            triggerObj.transform.position = (Vector2)con.transform.position + con.center
                + Utility.Vector2TransformToEllipse(
                    Utility.RadianToVector2((rotateZ + 90) * Mathf.Deg2Rad)
                    , 0.75f, 0.5f) * (startSpear + spear * t);

            time += Time.deltaTime / duration;
            endEvent.Invoke(triggerObj);
            yield return null;
        }
        if (weapon.hand_obj != null) weapon.hand_obj.gameObject.SetActive(false);

        triggerObj.gameObject.SetActive(false);
        
        if (weapon.hand_obj != null)
        {
            weapon.hand_obj.gameObject.SetActive(true);
            con.hand.HandLink(weapon.hand_obj, HandMode.ToHand);
        }
        else
        {
            con.hand.HandLink(null);
        }

    }
    public override void Break()
    {
        if (triggerObj.gameObject.activeInHierarchy)
        {
            breakParticle.SpawnParticle(triggerObj.transform.position, triggerObj.transform.rotation);
        }
    }

    public override void Removed()
    {
        Destroy(triggerObj.gameObject);
    }

    public override void Destroyed()
    {

    }
}
