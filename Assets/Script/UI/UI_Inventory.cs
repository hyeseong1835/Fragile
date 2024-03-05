using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class UI_Inventory : MonoBehaviour
{
    [SerializeField]
    [ChildGameObjectsOnly] Image UI;
    [SerializeField]
    [ChildGameObjectsOnly] Image durabillity;

    [SerializeField]
    [HorizontalGroup(110)]
    [PreviewField(ObjectFieldAlignment.Left)] Sprite[] UISprite;

    public void ChangeWeaponUI(int index)
    {
        if (UISprite[index] != null) UI.sprite = UISprite[index];
        else Debug.LogWarning("UISprite["+ index + "] is null."); //LogWarning: UISprite[{index}] is null."

        ResetDurabilityUI();
    }
    public void AddToInventory(Weapon weapon)
    {
        if(weapon.UI == null)
        {
            Debug.LogWarning(weapon + ".UI is null");
            ResetInventoryUI();
        }//LogError: {name}(Weapon).UI is null
        UISprite[weapon.index] = weapon.UI;
    }
    public void ResetInventoryUI()
    {
        UISprite = new Sprite[11];
        for (int i = 0; i < Player.wCon.transform.childCount; i++)
        {
            UISprite[i] = Player.wCon.weapons[i].UI;
        }
        UI.sprite = UISprite[Player.wCon.curWeapon.index];
        ResetDurabilityUI();
    }
    public void ResetDurabilityUI()
    {
        if (Player.wCon.curWeapon.breakable)
        {
            if (Player.wCon.curWeapon.maxDurability == 0)
            {
                Debug.LogWarning(Player.wCon.curWeapon.name + "의 maxDurabillity는 0일 수 없습니다");
                Player.wCon.curWeapon.maxDurability = 1;
            } //LogWarning: {name}의 maxDurabillity는 0일 수 없습니다
            durabillity.fillAmount = ((float) Player.wCon.curWeapon.durability) / Player.wCon.curWeapon.maxDurability;
        }
        else
        {
            //나중에 비파괴 전용 테두리도 만들어줘용
            durabillity.fillAmount = 0;
        }

    }
}