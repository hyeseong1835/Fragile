using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
public class Skill_Melee : Skill, IVoid
{
    [SerializeField] IVoid[] start, update, end;

    [SerializeField] IController[] selfEnter, selfStay, selfExit;
    [SerializeField] IController[] friendEnter, friendStay, friendExit;
    [SerializeField] IController[] enemyEnter, enemyStay, enemyExit;
    [SerializeField] IController[] objectEnter, objectStay, objectExit;


    [ReadOnly][Required]
    TriggerObject triggerObj;

    [HorizontalGroup("BreakParticle")]
    #region BreakParticle - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    
    [SerializeField] 
    protected BreakParticle breakParticle;//-|
                                              [HorizontalGroup("BreakParticle")]
    [Button("Add")]
    void AddBreakParticle()
    {
        //프리팹으로 변경
        breakParticle = new GameObject("BreakParticle").AddComponent<BreakParticle>();
        breakParticle.AddComponent<ParticleSystem>();
        breakParticle.transform.parent = transform;
    }

    #endregion - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    public float duration;
    [SerializeField] float spread;
    [SerializeField] float startSpear;
    [SerializeField] float spear;
     
    public enum SwingCurve { Linear, Quadratic }
    [SerializeField] SwingCurve curve;

    public override void SpawnModule()
    {
        triggerObj = Instantiate(triggerObj, transform);
        triggerObj.Init
        (
            con,
            selfEnter, selfStay, selfExit,
            friendEnter, friendStay, friendExit,
            enemyEnter, enemyStay, enemyExit,
            objectEnter, objectStay, objectExit
        );
    }
    protected override void InitSkill()
    {
        
    }
    public void InputVoid()
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
        IVoid.Invoke(start);

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
            IVoid.Invoke(update);

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
        IVoid.Invoke(end);
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
