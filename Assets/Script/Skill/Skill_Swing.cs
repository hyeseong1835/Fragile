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
        trigger.SetEvent(enterEvent, stayEvent, exitEvent);
        float startRotateZ = Player.pCon.viewRotateZ;

        //스킬
        Player.grafic.stateHandAnimation = false;
        trigger.transform.position = transform.position + (Vector3) Player.pCon.playerToMouse.normalized * 0.5f;

        float time = 0;
        switch(swingCurve)
        {
            case Curve.Linear:
                while (time < 1)
                {
                    trigger.transform.rotation = Quaternion.Euler(0, 0, startRotateZ + spread * 0.5f - time * spread - 90);
                    trigger.transform.localPosition = new Vector3(1, Mathf.Tan(startRotateZ + spread * 0.5f - time * spread), 0).normalized * 0.25f;

                    time += Time.deltaTime / duration;
                    yield return null;
                }
                break;
            case Curve.Quadratic:
                while (time < 1)
                {
                    trigger.transform.rotation = Quaternion.Euler(0, 0, startRotateZ + spread * 0.5f - time * time * spread);
                    trigger.transform.localPosition = new Vector3(
                        Mathf.Cos((startRotateZ + spread * 0.5f - time * time * spread) * Mathf.Deg2Rad), 
                        Mathf.Sin((startRotateZ + spread * 0.5f - time * time * spread) * Mathf.Deg2Rad) * 0.5f, 0).normalized * 0.5f;
                    Debug.Log(startRotateZ + spread * 0.5f - time * time * spread + ": " + new Vector3(Mathf.Cos(startRotateZ + spread * 0.5f - time * time * spread),
                        Mathf.Sin(startRotateZ + spread * 0.5f - time * time * spread), 0));
                    time += Time.deltaTime / duration;
                    
                    yield return null;
                }
                break;
        }
        trigger.transform.rotation = Quaternion.Euler(0, 0, startRotateZ - spread * 0.5f);

        yield return null;

        trigger.gameObject.SetActive(false);
        Player.grafic.stateHandAnimation = true;
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