using UnityEngine;

public class ItemManager : MonoBehaviour
{
    [SerializeField] GameObject itemPrefab;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public static GameObject SpawnItem(Weapon weapon, string weaponData)
    {
        GameObject item = Instantiate(weapon.item);
        item.name = weaponData;
        return item;
    }
    //�߰� �ʿ�---------------------------------
    public void SaveItems()
    {
        string itemSaveData = "";
        for (int i = 0; i < transform.childCount; i++)
        {
            itemSaveData += transform.GetChild(i).GetComponent<Weapon>().name + transform.GetChild(i).gameObject.name;
        }
    }
    public void LoadItems()
    {
        
    }
}
