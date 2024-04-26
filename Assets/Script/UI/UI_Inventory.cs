using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class UI_Inventory : MonoBehaviour
{
    [SerializeField] PlayerController pCon;
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
        //UISprite[weapon.index] = weapon.UI;
    }
    public void ResetInventoryUI()
    {
        UISprite = new Sprite[11];
        for (int i = 0; i < pCon.transform.childCount; i++)
        {
            UISprite[i] = pCon.weapons[i].UI;
        }
        //UI.sprite = UISprite[pCon.curWeapon.index];
        ResetDurabilityUI();
    }
    public void ResetDurabilityUI()
    {
        //�������� ���� ��
        if (pCon.curWeapon.durability == -1)
        {
            //���߿� ���ı� ���� �׵θ��� ��������
            durabillity.fillAmount = 0;
        }
        //�������� ���� ��
        else
        {
            if (pCon.curWeapon.maxDurability == 0)
            {
                Debug.LogWarning(pCon.curWeapon.name + "�� maxDurabillity�� 0�� �� �����ϴ�");
                pCon.curWeapon.maxDurability = 1;
            } //LogWarning: {name}�� maxDurabillity�� 0�� �� �����ϴ�
            durabillity.fillAmount = ((float)pCon.curWeapon.durability) / pCon.curWeapon.maxDurability;
        }

    }
}