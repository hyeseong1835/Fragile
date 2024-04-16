using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utility
{
    public static float GetTargetAngle(Vector2 origin, Vector2 target)
    {
        Vector2 dir = target - origin;
        return Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="spriteSheet"></param>
    /// <param name="startPos">왼쪽 상단 기준 스프라이트 좌표</param>
    /// <param name="endPos">왼쪽 상단 기준 스프라이트 좌표</param>
    /// <param name="spritePixelWidth"></param>
    /// <param name="spritePixelHeight"></param>
    /// <returns></returns>
    public static Sprite[,] GetSpriteArray2DFromSpriteSheet(Texture2D spriteSheet, Vector2Int startPos, Vector2Int endPos, int spritePixelWidth, int spritePixelHeight)
    {
        Sprite[,] frames = new Sprite[endPos.x - startPos.x + 1, endPos.y - startPos.y + 1];
        for (int x = startPos.x; x <= endPos.x; x++)
        {
            for (int y = startPos.y; y <= endPos.y; y++)
            {
                Texture2D tex = new Texture2D(spritePixelWidth, spritePixelHeight);
                tex.SetPixels(spriteSheet.GetPixels(x * spritePixelWidth, spriteSheet.height - ((y + 1) * spritePixelHeight), spritePixelWidth, spritePixelHeight));
                tex.filterMode = FilterMode.Point;
                tex.Apply();

                frames[x - startPos.x, y - startPos.y] = Sprite.Create(tex, new Rect(0, 0, spritePixelWidth, spritePixelHeight), Vector2.one * 0.5f);
            }
        }
        return frames;
    }
    public static int FloorRotateToInt(float rotate, int count)
    {   
        return Mathf.FloorToInt(((rotate % 360) / 360) * count); 
    }
}
