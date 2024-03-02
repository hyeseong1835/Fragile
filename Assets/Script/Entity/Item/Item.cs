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
            if (Player.wCon.weaponCount > 11) return;

            Player.wCon.TakeItem(weapon, gameObject.name);

            Destroy(gameObject);
        }
    }
}
