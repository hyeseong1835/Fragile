using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;
using System.IO;

public static class Utility
{
    public static float rotateMax = 360f - Mathf.Epsilon;
    public static float GetTargetAngle(Vector2 origin, Vector2 target)
    {
        Vector2 dir = target - origin;
        return Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
    }
    /// <summary>
    /// ���� ��������Ʈ ��ǥ���� �迭 ũ�⸸ŭ ������ �Ʒ��� �ҷ����� �Լ�
    /// </summary>
    /// <param name="spriteSheet"></param>
    /// <param name="startPos">���� ��� ���� ��������Ʈ ��ǥ</param>
    /// <param name="size">�迭 ũ��</param>
    /// <param name="spritePixelSize">��������Ʈ �ȼ� ũ��</param>
    /// <returns>��������Ʈ �迭</returns>
    public static Sprite[,] GetSpriteArray2DFromSpriteSheet(Texture2D spriteSheet, Vector2Int startPos, Vector2Int size, Vector2Int spritePixelSize)
    {
        return GetSpriteArray2DFromSpriteSheet(spriteSheet, startPos, size, spritePixelSize.x, spritePixelSize.y);
    }
    /// <summary>
    /// ���� ��������Ʈ ��ǥ���� �迭 ũ�⸸ŭ ������ �Ʒ��� �ҷ����� �Լ�
    /// </summary>
    /// <param name="spriteSheet"></param>
    /// <param name="startPos">���� ��� ���� ��������Ʈ ��ǥ</param>
    /// <param name="size">�迭 ũ��</param>
    /// <param name="spritePixelWidth">��������Ʈ ���� �ȼ� ũ��</param>
    /// <param name="spritePixelHeight">��������Ʈ ���� �ȼ� ũ��(�⺻ 1 : 1 ����)</param>
    /// <returns>��������Ʈ �迭</returns>
    public static Sprite[,] GetSpriteArray2DFromSpriteSheet(Texture2D spriteSheet, Vector2Int startPos, Vector2Int size, int spritePixelWidth, int spritePixelHeight = -1)
    {
        if (spritePixelHeight == -1) spritePixelHeight = spritePixelWidth;

        Sprite[,] frames = new Sprite[size.x, size.y];
        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                Texture2D tex = new Texture2D(spritePixelWidth, spritePixelHeight);
                tex.SetPixels(
                    spriteSheet.GetPixels(
                        (startPos.x + x) * spritePixelWidth, 
                        spriteSheet.height - ((startPos.y + (y + 1)) * spritePixelHeight), 
                        spritePixelWidth, spritePixelHeight)
                    );
                tex.filterMode = FilterMode.Point;
                tex.Apply();

                frames[x, y] = Sprite.Create(
                    tex, 
                    new Rect(0, 0, spritePixelWidth, spritePixelHeight), 
                    Vector2.one * 0.5f);
            }
        }
        return frames;
    }
    /// <summary>
    /// {count}���� ���� ���� �� {rotate}�� �ִ� ����
    /// </summary>
    /// <param name="rotate">(0 <= rotate <= 360)</param>
    /// <param name="count"></param>
    /// <returns></returns>

    public static Weapon LoadWeapon(string name)
    {
        Debug.Log("LoadWeapon: {\"" + "WeaponObjPrefab/" + name + "\"}");
        
        GameObject weaponObj = Object.Instantiate(
            Resources.Load<GameObject>("WeaponObjPrefab/" + name)
        );
        Weapon weapon = weaponObj.GetComponent<Weapon>();

        return weapon;
    }
    public static Weapon LoadWeapon(string name, string[] data)
    {
        Weapon weapon = LoadWeapon(name);
        weapon.SetData(data);

        return weapon;
    }


    public static int FloorRotateToInt(float rotate, int count)
    {   
        int intRotate = Mathf.FloorToInt((rotate / 360) * count);

        if (intRotate == count) return 0;

        return intRotate;
    }
    /// <summary>
    /// {vector}�� ������ ��ȯ
    /// </summary>
    /// <param name="vector"></param>
    /// <returns>0 <= return < 360</returns>
    public static float Vector2ToRotate(Vector2 vector)
    {
        float rotate = Mathf.Atan2(vector.y, vector.x) * Mathf.Rad2Deg;
        
        if (rotate < 0) return rotate + 360;
        return rotate;
    }
}
