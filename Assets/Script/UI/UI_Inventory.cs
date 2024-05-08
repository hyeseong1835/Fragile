using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
public class UI_Inventory : MonoBehaviour
{
    [SerializeField][Required]
    PlayerController pCon;
    
    [SerializeField][Required][ChildGameObjectsOnly] 
    Image curWeaponImage;
    
    [SerializeField][Required][ChildGameObjectsOnly] 
    Image durabillity;

    [SerializeField][Required][ChildGameObjectsOnly] 
    Image nextWeaponImage;

    void Update()
    {
        curWeaponImage.sprite = pCon.curWeapon.UI;

        //�������� ���� ��
        if (pCon.curWeapon.durability == -1)
        {
            //���߿� ���ı� ���� �׵θ��� ��������
            durabillity.fillAmount = 0;
        }
        //�������� ���� ��
        else durabillity.fillAmount = ((float)pCon.curWeapon.durability) / pCon.curWeapon.maxDurability;

        if (pCon.nextWeapon == null)
        {
            nextWeaponImage.gameObject.SetActive(false);
        }
        else
        {
            nextWeaponImage.gameObject.SetActive(true);

            nextWeaponImage.sprite = pCon.nextWeapon.UI;
        }
        
    }
}