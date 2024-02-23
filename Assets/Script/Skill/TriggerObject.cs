using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerObject : MonoBehaviour
{
    [SerializeField] UnityEvent<GameObject, Collider2D> OnTriggerEnter;
    [SerializeField] UnityEvent<GameObject, Collider2D> OnTriggerStay;
    [SerializeField] UnityEvent<GameObject, Collider2D> OnTriggerExit;

    void OnTriggerEnter2D(Collider2D collider)
    {
        OnTriggerEnter.Invoke(gameObject, collider);
    }
    void OnTriggerStay2D(Collider2D collider)
    {
        OnTriggerStay.Invoke(gameObject, collider);
    }
    void OnTriggerExit2D(Collider2D collider)
    {
        OnTriggerExit.Invoke(gameObject, collider);
    }
}
