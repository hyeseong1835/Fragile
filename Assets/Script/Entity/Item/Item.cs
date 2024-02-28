using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Item : MonoBehaviour
{
    public GameObject weapon;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            WeaponController weaponCon = collision.GetComponent<WeaponController>();
            if (weaponCon.weaponHolder.childCount > 10) return;

            Debug.Log(weapon.GetComponent<Weapon>());
            weaponCon.TakeItem(weapon.GetComponent<Weapon>());

            Destroy(gameObject);
        }
    }
}
