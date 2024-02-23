using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stat : MonoBehaviour
{
    public float hp;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnDamage(float damage)
    {
        hp -= damage;
        if (hp <= 0) Destroy(gameObject);
    }
}
