using UnityEngine;
using UnityEngine.Events;

public class TriggerObject : MonoBehaviour
{
    System.Action<TriggerObject, Collider2D> enter;
    System.Action<TriggerObject, Collider2D> stay;
    System.Action<TriggerObject, Collider2D> exit;

    public void SetEvent(
        System.Action<TriggerObject, Collider2D> _enter,
        System.Action<TriggerObject, Collider2D> _stay,
        System.Action<TriggerObject, Collider2D> _exit)
    {
        enter = _enter;
        stay = _stay;
        exit = _exit;
    }
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (enter == null) return;

        enter(this, collider);
    }
    void OnTriggerStay2D(Collider2D collider)
    {
        if (stay == null) return;

        stay(this, collider);
    }
    void OnTriggerExit2D(Collider2D collider)
    {
        if (exit == null) return;

        exit.Invoke(this, collider);
    }
}
