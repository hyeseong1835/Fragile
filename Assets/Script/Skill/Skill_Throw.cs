using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Skill_Throw : Skill
{
    public IEnumerator Throw(TriggerObject trigger, float spinSpeed, float throwSpeed, float duration,
        UnityEvent<GameObject, Collider2D> enterEvent = null, 
        UnityEvent<GameObject, Collider2D> stayEvent = null, 
        UnityEvent<GameObject, Collider2D> exitEvent = null)
    {
        weapon.Remove();
        trigger.gameObject.SetActive(true);
        trigger.SetEvent(enterEvent, stayEvent, exitEvent);
        transform.SetParent(null);
        float startAngleZ = Mathf.Atan2(Player.cam.ScreenToWorldPoint(Player.pCon.mousePos).y - Player.transform.position.y,
            Player.cam.ScreenToWorldPoint(Player.pCon.mousePos).x - Player.transform.position.x) * Mathf.Rad2Deg - 90;
        Vector3 startVector = Player.cam.ScreenToWorldPoint(Player.pCon.mousePos) + new Vector3(0, 0, 10) - Player.transform.position;
        float time = 0;
        while (time <= duration)
        {
            trigger.transform.position += startVector.normalized * throwSpeed;
            trigger.transform.rotation = Quaternion.Euler(0, 0, startAngleZ - time * spinSpeed);
            time += Time.deltaTime;
            yield return null;
        }
        Destroy(trigger.gameObject);
        weapon.Remove();
    }
    public void SpinObjectTriggerEnter(Transform transform, Collider2D coll, UnityEvent<Transform, Collider2D> enterEvent)
    {
        enterEvent.Invoke(transform, coll);
    }
    public void SpinObjectTriggerStay(Transform transform, Collider2D coll, UnityEvent<Transform, Collider2D> stayEvent)
    {
        stayEvent.Invoke(transform, coll);
    }
    public void SpinObjectTriggerExit(Transform transform, Collider2D coll, UnityEvent<Transform, Collider2D> exitEvent)
    {
        exitEvent.Invoke(transform, coll);
    }
}
