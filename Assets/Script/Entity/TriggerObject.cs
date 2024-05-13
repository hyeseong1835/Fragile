using UnityEngine;
using UnityEngine.Events;

public class TriggerObject : MonoBehaviour
{
    UnityEvent<TriggerObject, Collider2D> enterEvent;
    UnityEvent<TriggerObject, Collider2D> stayEvent;
    UnityEvent<TriggerObject, Collider2D> exitEvent;

    public void SetEvent(UnityEvent<TriggerObject, Collider2D> _enterEvent, 
        UnityEvent<TriggerObject, Collider2D> _stayEvent, 
        UnityEvent<TriggerObject, Collider2D> _exitEvent)
    {
        enterEvent = _enterEvent;
        stayEvent = _stayEvent;
        exitEvent = _exitEvent;
    }
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (enterEvent == null) return;

        enterEvent.Invoke(this, collider);
    }
    void OnTriggerStay2D(Collider2D collider)
    {
        if (stayEvent == null) return;

        stayEvent.Invoke(this, collider);
    }
    void OnTriggerExit2D(Collider2D collider)
    {
        if (exitEvent == null) return;

        exitEvent.Invoke(this, collider);
    }
}
