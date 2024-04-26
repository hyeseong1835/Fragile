using UnityEditor;
using UnityEngine;

public struct ItemData
{
    public Vector2 pos;

    public WeaponData weaponData;

    public ItemData(Vector2 _pos, WeaponData _weaponData)
    {
        pos = _pos;
        weaponData = _weaponData;
    }
}
[ExecuteAlways]
public class Item : MonoBehaviour
{
    public Weapon weapon;

    void Update()
    {
        if (weapon == null 
            || weapon.transform.IsChildOf(transform) == false)
        {
            if (EditorApplication.isPlaying) Destroy(gameObject);
            else DestroyImmediate(gameObject);
        }
    }
    public string GetData()
    {
        return JsonUtility.ToJson(
            new ItemData
            (
                transform.position,
                weapon.GetData()
            )
        );
    }
    /*
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            Controller controller = collision.GetComponent<Controller>();
            
            if (controller.weapons.Count >= controller.inventorySize) return;

            controller.AddWeapon(weapon);

            Destroy(gameObject);
        }
    }
    */
}
