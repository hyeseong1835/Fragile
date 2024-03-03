using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Skill_Swing : Skill
{
    public enum Curve
    {
        Linear, Quadratic
    }
    public IEnumerator Swing(TriggerObject triggerObj, float spread, float duration, Curve swingCurve, 
        UnityEvent<Transform, Collider2D> enterEvent = null, 
        UnityEvent<Transform, Collider2D> stayEvent = null, 
        UnityEvent<Transform, Collider2D> exitEvent = null)
    {
        //초기화
        triggerObj.gameObject.SetActive(true);
        triggerObj.SetEvent(enterEvent, stayEvent, exitEvent);
        float startRotateZ = Player.pCon.viewRotateZ;

        //스킬
        float time = 0;
        switch(swingCurve)
        {
            case Curve.Linear:
                while (time < 1)
                {
                    triggerObj.transform.rotation = Quaternion.Euler(0, 0, startRotateZ + spread * 0.5f - time * spread);

                    time += Time.deltaTime / duration;
                    yield return null;
                }
                break;
            case Curve.Quadratic:
                while (time < 1)
                {
                    triggerObj.transform.rotation = Quaternion.Euler(0, 0, startRotateZ + spread * 0.5f - time * time * spread);

                    time += Time.deltaTime / duration;
                    yield return null;
                }
                break;
        }
        triggerObj.transform.rotation = Quaternion.Euler(0, 0, startRotateZ - spread * 0.5f);

        yield return null;
        triggerObj.gameObject.SetActive(false);
    }
    public void SwingObjectTriggerEnter(Transform transform, Collider2D coll, UnityEvent<Transform, Collider2D> enterEvent)
    {
        enterEvent.Invoke(transform, coll);
    }
    public void SwingObjectTriggerStay(Transform transform, Collider2D coll, UnityEvent<Transform, Collider2D> stayEvent)
    {
        stayEvent.Invoke(transform, coll);
    }
    public void SwingObjectTriggerExit(Transform transform, Collider2D coll, UnityEvent<Transform, Collider2D> exitEvent)
    {
        exitEvent.Invoke(transform, coll);
    }
}