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
    //�Է�
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
        //���� ����
        if (index + 1 == transform.childCount) //������ ������ ������ ��
        {
            if (transform.childCount == 2) //���Ⱑ �ϳ��� ��
            {
                SelectWeapon(0);
            }
            else SelectWeapon(index - 1); //���Ⱑ �� ���� ��
        }
        else SelectWeapon(index + 1);

        //����(�κ��丮����)
        weapons[index].transform.parent = null;

    }
    [DisableInEditorMode]
    [Button(ButtonStyle.Box)]
    protected void SelectWeapon(int index)
    {
        if (index < 0 || transform.childCount - 1 < index)
        {
            Debug.LogWarning("index�� ������ �ʰ���: (" + index + "/" + (transform.childCount - 1) + " )");
            index = 0;
        } //LogWarning: "index�� ������ �ʰ���"

        for (int i = 0; i < transform.childCount; i++) //���� ��� ��Ȱ��ȭ
        {
            if (weapons[i].isUsing)
            {
                weapons[i].SetUse(false);
            }
        }
        if (weapons[index] == null)
        {
            Debug.LogWarning("ȣ���� �ε����� ���Ⱑ ����");
            index = 0;
        } //LogWarning: "ȣ���� �ε����� ���Ⱑ ����"

        curWeapon = weapons[index];

        curWeapon.SetUse(true); //������ ���� Ȱ��ȭ
    }
}
