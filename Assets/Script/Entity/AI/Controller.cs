using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    public Grafic grafic;
    public Vector2 targetPos;
    public Vector3 moveVector;
    [HideInInspector] public Vector3 prevMoveVector;

    [Range(0f, 360f)] public float moveRotate = 0;
    //입력
    public bool attack = false;
    public bool special = false;

    public Weapon curWeapon;
    protected int curWeponIndex;
    public Weapon[] weapons;
    public int inventorySize;

    public void AddWeapon(Weapon weapon)
    {
        weapon.con = this;
        weapon.index = transform.childCount - 1;
        weapons[weapon.index] = weapon;
    }
    [DisableInEditorMode]
    [Button(ButtonStyle.Box)]
    public void RemoveWeapon(int index)
    {
        //무기 선택
        if (index + 1 == transform.childCount) //마지막 순서의 무기일 때
        {
            if (transform.childCount == 2) //무기가 하나일 때
            {
                SelectWeapon(0);
            }
            else SelectWeapon(index - 1); //무기가 더 있을 때
        }
        else SelectWeapon(index + 1);

        //제거(인벤토리에서)
        weapons[index].transform.parent = null;

    }
    [DisableInEditorMode]
    [Button(ButtonStyle.Box)]
    protected void SelectWeapon(int index)
    {
        if (index < 0 || transform.childCount - 1 < index)
        {
            Debug.LogWarning("index가 범위를 초과함: (" + index + "/" + (transform.childCount - 1) + " )");
            index = 0;
        } //LogWarning: "index가 범위를 초과함"

        for (int i = 0; i < transform.childCount; i++) //무기 모두 비활성화
        {
            if (weapons[i].isUsing)
            {
                weapons[i].SetUse(false);
            }
        }
        if (weapons[index] == null)
        {
            Debug.LogWarning("호출한 인덱스에 무기가 없음");
            index = 0;
        } //LogWarning: "호출한 인덱스에 무기가 없음"

        curWeapon = weapons[index];

        curWeapon.SetUse(true); //선택한 무기 활성화
    }
}
