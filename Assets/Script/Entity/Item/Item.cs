using UnityEngine;

public struct ItemData
{
    public string name;
    public string[] weaponData;
    
    public Vector2 pos;

    public ItemData(string _name, string[] _weaponData, Vector2 _pos)
    {
        name = _name;
        weaponData = _weaponData;
        pos = _pos;
    }
}
public class Item : MonoBehaviour
{
    public string weaponName;
    public Weapon weapon;

    void Update()
    {
 
    }
    public string GetData()
    {
        return JsonUtility.ToJson(
            new ItemData
            (
                weapon.name,
                weapon.GetData(),
                transform.position
            )
        );

    }
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
}
