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
    /// 시작 스프라이트 좌표에서 배열 크기만큼 오른쪽 아래로 불러오는 함수
    /// </summary>
    /// <param name="spriteSheet"></param>
    /// <param name="startPos">왼쪽 상단 기준 스프라이트 좌표</param>
    /// <param name="size">배열 크기</param>
    /// <param name="spritePixelSize">스프라이트 픽셀 크기</param>
    /// <returns>스프라이트 배열</returns>
    public static Sprite[,] GetSpriteArray2DFromSpriteSheet(Texture2D spriteSheet, Vector2Int startPos, Vector2Int size, Vector2Int spritePixelSize)
    {
        return GetSpriteArray2DFromSpriteSheet(spriteSheet, startPos, size, spritePixelSize.x, spritePixelSize.y);
    }
    /// <summary>
    /// 시작 스프라이트 좌표에서 배열 크기만큼 오른쪽 아래로 불러오는 함수
    /// </summary>
    /// <param name="spriteSheet"></param>
    /// <param name="startPos">왼쪽 상단 기준 스프라이트 좌표</param>
    /// <param name="size">배열 크기</param>
    /// <param name="spritePixelWidth">스프라이트 가로 픽셀 크기</param>
    /// <param name="spritePixelHeight">스프라이트 세로 픽셀 크기(기본 1 : 1 비율)</param>
    /// <returns>스프라이트 배열</returns>
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
    public static int FloorRotateToInt(float rotate, int count)
    {   
        return Mathf.FloorToInt(((rotate % 360) / 360) * count);
    }
}
