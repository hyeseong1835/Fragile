using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SpriteSheet
{
    public static Sprite[,] GetSpriteArray2DFromSpriteSheet(Texture2D spriteSheet, Vector2Int startPos, Vector2Int endPos, int spritePixelWidth, int spritePixelHeight)
    {
        Sprite[,] frames = new Sprite[endPos.x - startPos.x + 1, endPos.y - startPos.y + 1];
        for (int x = startPos.x; x <= endPos.x; x++)
        {
            for (int y = startPos.y; y <= endPos.y; y++)
            {
                Texture2D tex = new Texture2D(spritePixelWidth, spritePixelHeight);
                tex.SetPixels(spriteSheet.GetPixels(x * spritePixelWidth, y * spritePixelHeight, spritePixelWidth, spritePixelHeight));
                tex.filterMode = FilterMode.Point;
                tex.Apply();

                Debug.Log("X: (" + (x - startPos.x) + ", " + (y - startPos.y) + " )");
                frames[x - startPos.x, y - startPos.y] = Sprite.Create(tex, new Rect(0, 0, spritePixelWidth, spritePixelHeight), Vector2.one * 0.5f);
            }
        }
        return frames;
    }
}
