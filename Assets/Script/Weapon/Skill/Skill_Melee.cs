using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Skill_Melee : Skill
{
    [SerializeField]
    TriggerObject triggerObj;

    [SerializeField]
    protected BreakParticle breakParticle;

    [SerializeField]
    float duration;
    [SerializeField]
    float spread;
    [SerializeField]
    float spear;

    public enum SwingCurve { Linear, Quadratic }
    [SerializeField] SwingCurve curve;

    public UnityEvent<TriggerObject> startEvent;
    public UnityEvent<TriggerObject> updateEvent;
    public UnityEvent<TriggerObject> endEvent;

    public UnityEvent<TriggerObject, Collider2D> enterEvent;
    public UnityEvent<TriggerObject, Collider2D> stayEvent;
    public UnityEvent<TriggerObject, Collider2D> exitEvent;

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
        weapon.hand_obj.gameObject.SetActive(false);

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
                    , 0.75f, 0.5f) * (1 + spear * t);

            time += Time.deltaTime / duration;
            endEvent.Invoke(triggerObj);
            yield return null;
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
