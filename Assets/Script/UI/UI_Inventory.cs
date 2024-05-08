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

        //내구도가 없을 때
        if (pCon.curWeapon.durability == -1)
        {
            //나중에 비파괴 전용 테두리도 만들어줘용
            durabillity.fillAmount = 0;
        }
        //내구도가 있을 때
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