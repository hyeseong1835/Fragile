using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class Skill_Melee : Skill, Input_Empty
{
    [SerializeField] Input_TriggerHit enter, stay, exit;
    [SerializeField] Input_TriggerEvent start, update, end;

    [SerializeField] TriggerObject triggerObj;

    protected BreakParticle breakParticle;

    public float duration;
    float spread;
    float startSpear;
    float spear;
     
    public enum SwingCurve { Linear, Quadratic }
    [SerializeField] SwingCurve curve;

    protected override void InitSkill()
    {
        triggerObj.SetEvent
        (
            enter.TriggerHit,
            stay.TriggerHit,
            exit.TriggerHit
        );
    }
    public void Empty()
    {
        StartCoroutine(Swing());
    }
    IEnumerator Swing()
    {
        #region Start  - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -|

            if (weapon.hand_obj != null) weapon.hand_obj.gameObject.SetActive(false);

            triggerObj.gameObject.SetActive(true);
            con.hand.HandLink(triggerObj.transform, HandMode.ToTarget);

            triggerObj.transform.position = con.transform.position + (Vector3)con.center;//-|

            float startRotateZ = Utility.Vector2ToDegree(con.targetDir);

        #endregion - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -|
        start.TriggerEvent(triggerObj);

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

            #region Update  - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -|
            
                float rotateZ = startRotateZ + spread * 0.5f - t * spread - 90;
                triggerObj.transform.rotation = Quaternion.Euler(0, 0, rotateZ);
                triggerObj.transform.position = (Vector2)con.transform.position + con.center//-|
                    + Utility.Vector2TransformToEllipse(
                        Utility.RadianToVector2((rotateZ + 90) * Mathf.Deg2Rad)
                        , 0.75f, 0.5f) * (startSpear + spear * t);

            #endregion  - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -| 
            update.TriggerEvent(triggerObj);

            time += Time.deltaTime / duration;
            yield return null;
        }

        #region End  - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -|

            if (weapon.hand_obj != null) weapon.hand_obj.gameObject.SetActive(false);//-|

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

        #endregion - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -|
        end.TriggerEvent(triggerObj);
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
