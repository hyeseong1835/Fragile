using UnityEngine;

public static class Texture2DUtillity
{
    public static Sprite GetSprite(this Texture2D spriteSheet, int posX, int posY, int spritePixelWidth, int spritePixelHeight)
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
    public static Sprite[,] GetSpriteArray2DFromSpriteSheet(this Texture2D spriteSheet, Vector2Int startPos, Vector2Int size, Vector2Int spritePixelSize)
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
    public static Sprite[,] GetSpriteArray2DFromSpriteSheet(this Texture2D spriteSheet, Vector2Int startPos, Vector2Int size, int spritePixelWidth, int spritePixelHeight = -1)
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

}
