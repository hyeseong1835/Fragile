using UnityEngine;

public class Item : MonoBehaviour
{
    Weapon weapon;

    public Item(Weapon _weapon)
    {
        weapon = _weapon;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            if (Player.wCon.transform.childCount > 11) return;

            string[] split = gameObject.name.Split('/');
            Player.wCon.AddWeapon(weapon, split[0], split[1]);

            Destroy(gameObject);
        }
    }
}
