using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Skill_Swing : Skill
{
    public enum Curve
    {
        Linear, Quadratic
    }
    public IEnumerator Swing(TriggerObject trigger, float spread, float duration, Curve swingCurve, 
        UnityEvent<GameObject, Collider2D> enterEvent = null, 
        UnityEvent<GameObject, Collider2D> stayEvent = null, 
        UnityEvent<GameObject, Collider2D> exitEvent = null,
        UnityEvent<GameObject> endEvent = null)
    {
        //초기화
        trigger.gameObject.SetActive(true);
        trigger.SetEvent(enterEvent, stayEvent, exitEvent);
        float startRotateZ = Player.pCon.viewRotateZ;

        //스킬
        float time = 0;
        switch(swingCurve)
        {
            case Curve.Linear:
                while (time < 1)
                {
                    trigger.transform.rotation = Quaternion.Euler(0, 0, startRotateZ + spread * 0.5f - time * spread);

                    time += Time.deltaTime / duration;
                    yield return null;
                }
                break;
            case Curve.Quadratic:
                while (time < 1)
                {
                    trigger.transform.rotation = Quaternion.Euler(0, 0, startRotateZ + spread * 0.5f - time * time * spread);

                    time += Time.deltaTime / duration;
                    yield return null;
                }
                break;
        }
        trigger.transform.rotation = Quaternion.Euler(0, 0, startRotateZ - spread * 0.5f);

        yield return null;

        trigger.gameObject.SetActive(false);
        if (endEvent != null) endEvent.Invoke(trigger.gameObject);
    }
    public void SwingObjectTriggerEnter(GameObject triggerObj, Collider2D coll, UnityEvent<GameObject, Collider2D> enterEvent)
    {
        enterEvent.Invoke(triggerObj, coll);
    }
    public void SwingObjectTriggerStay(GameObject triggerObj, Collider2D coll, UnityEvent<GameObject, Collider2D> stayEvent)
    {
        stayEvent.Invoke(triggerObj, coll);
    }
    public void SwingObjectTriggerExit(GameObject triggerObj, Collider2D coll, UnityEvent<GameObject, Collider2D> exitEvent)
    {
        exitEvent.Invoke(triggerObj, coll);
    }
}