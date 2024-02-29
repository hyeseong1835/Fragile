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
            if (WeaponController.weaponHolder.childCount > 11) return;

            Debug.Log(weapon.GetComponent<Weapon>());
            Player.wCon.TakeItem(weapon.GetComponent<Weapon>(), gameObject.name);

            Destroy(gameObject);
        }
    }
}
