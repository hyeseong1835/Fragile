using UnityEngine;
using UnityEngine.Events;

public class TriggerObject : MonoBehaviour
{
    UnityEvent<Transform, Collider2D> enterEvent;
    UnityEvent<Transform, Collider2D> stayEvent;
    UnityEvent<Transform, Collider2D> exitEvent;

    public void SetEvent(UnityEvent<Transform, Collider2D> _enterEvent, 
        UnityEvent<Transform, Collider2D> _stayEvent, 
        UnityEvent<Transform, Collider2D> _exitEvent)
    {
        enterEvent = _enterEvent;
        stayEvent = _stayEvent;
        exitEvent = _exitEvent;
    }
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (enterEvent == null) return;

        enterEvent.Invoke(transform, collider);
    }
    void OnTriggerStay2D(Collider2D collider)
    {
        if (stayEvent == null) return;

        stayEvent.Invoke(transform, collider);
    }
    void OnTriggerExit2D(Collider2D collider)
    {
        if (exitEvent == null) return;

        exitEvent.Invoke(transform, collider);
    }
}
