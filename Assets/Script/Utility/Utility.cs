using System;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif
using UnityEngine;

public static class Utility
{
    #region Load - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -|

    public static Sprite GetSprite(Texture2D spriteSheet, int posX, int posY, int spritePixelWidth, int spritePixelHeight)
    {
        Texture2D tex = new Texture2D(spritePixelWidth, spritePixelHeight);
        tex.SetPixels(
            spriteSheet.GetPixels(posX * spritePixelWidth, (spriteSheet.height - spritePixelHeight) - posY * spritePixelHeight, spritePixelWidth, spritePixelHeight)
        );
        tex.filterMode = FilterMode.Point;
        tex.Apply();

        return Sprite.Create(
            tex,
            new Rect(0, 0, spritePixelWidth, spritePixelHeight),
            new Vector2(0.5f, 0),
            16
        );
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
                    new Vector2(0.5f, 0));
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

    #endregion - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -|

    #region Math - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -|

    public static int FloorRotateToInt(float rotate, float offset, int count)
    {
        rotate += offset;
        int intRotate = Mathf.FloorToInt((rotate / 360) * count);

        return intRotate % count;
    }
    /// <summary>
    /// {vector}�� ������ ��ȯ
    /// </summary>
    /// <param name="vector"></param>
    /// <returns>0 <= return < 360</returns>
    public static float Vector2ToDegree(Vector2 vector)
    {
        float rotate = Mathf.Atan2(vector.y, vector.x) * Mathf.Rad2Deg;
        
        if (rotate < 0) return rotate + 360;
        return rotate;
    }
    public static Vector2 Vector2TransformToEllipse(Vector2 vector, float x, float y)
    {
        return new Vector2(vector.x * x, vector.y * y);
    }
    public static Vector2 RadianToVector2(float radian)
    {
        return new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));
    }
    public static int Bigger(int a, int b)
    {
        if (a >= b) return a;
        else return b;
    }
    public static int Smaller(int a, int b)
    {
        if (a >= b) return b;
        else return a;
    }

    #endregion - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -|

    #region Other  - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -|

    public static void AutoDestroy(UnityEngine.Object obj)
    {
        if (Editor.GetType(Editor.StateType.IsPlay)) UnityEngine.Object.Destroy(obj);
        else UnityEngine.Object.DestroyImmediate(obj);
    }

    #endregion - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -|
}
