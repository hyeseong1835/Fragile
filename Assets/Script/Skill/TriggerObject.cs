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
        enterEvent.Invoke(transform, collider);
    }
    void OnTriggerStay2D(Collider2D collider)
    {
        stayEvent.Invoke(transform, collider);
    }
    void OnTriggerExit2D(Collider2D collider)
    {
        exitEvent.Invoke(transform, collider);
    }
}
