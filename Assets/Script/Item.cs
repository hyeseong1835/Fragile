using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Item : MonoBehaviour
{
    public GameObject weapon;
    public int durability;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            WeaponController weaponCon = collision.GetComponent<WeaponController>();
            if (weaponCon.weaponHolder.childCount > 10) return;

            Debug.Log(weapon.GetComponent<Weapon>());
            weaponCon.TakeItem(weapon.GetComponent<Weapon>(), durability);

            Destroy(gameObject);
        }
    }
}
