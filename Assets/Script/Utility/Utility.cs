using System;
using UnityEditor;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public static class Utility 
{
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
                    new Vector2(0.5f, 0));
            }
        }
        return frames;
    }
    /// <summary>
    /// {count}개의 각도 범위 중 {rotate}가 있는 범위
    /// </summary>
    /// <param name="rotate">(0 <= rotate <= 360)</param>
    /// <param name="count"></param>
    /// <returns></returns>

    public static Weapon SpawnWeapon(string weaponName, UnityEngine.Transform parent = null)
    {
        GameObject weaponObj;
        if (parent != null)
        {
            weaponObj = UnityEngine.Object.Instantiate(
            Resources.Load<GameObject>("WeaponObjPrefab/" + weaponName),
                parent
            );
        }
        else
        {
            weaponObj = UnityEngine.Object.Instantiate(
            Resources.Load<GameObject>("WeaponObjPrefab/" + weaponName)
            );
        }
        
        Weapon weapon = weaponObj.GetComponent<Weapon>();
        weapon.state = WeaponState.NULL;

        return weapon;
    }
    public static Weapon LoadWeapon(WeaponData data)
    {
        Weapon weapon = SpawnWeapon(data.name);
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
    /// {vector}를 각도로 변환
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
    public static void Destroy(UnityEngine.Object obj)
    {
#if UNITY_EDITOR
        if (EditorApplication.isPlaying) Destroy(obj);
        else UnityEngine.Object.DestroyImmediate(obj);
#else
            Destroy(obj);
#endif
    }
    public enum EditorState
    {
        UNKNOWN, BUILDPLAY, EDITORPLAY, EDITORPLAYPAUSE, EDITORPLAYCOMPILING, PREFAB
    }
    public enum StateType
    {
        ISPLAY, ISEDITOR
    }
    public static EditorState GetEditorState(GameObject gameObject = null)
    {
#if UNITY_EDITOR
        if (EditorApplication.isPlaying) return EditorState.EDITORPLAY;
        else if (gameObject != null && PrefabUtility.GetPrefabInstanceStatus(gameObject) == PrefabInstanceStatus.NotAPrefab) return EditorState.PREFAB;

        if (EditorApplication.isPaused) return EditorState.EDITORPLAYPAUSE;

        if (EditorApplication.isCompiling) return EditorState.EDITORPLAYCOMPILING;


        return EditorState.UNKNOWN;
#else
        return EditorState.BUILDPLAY;
#endif
    }
    public static bool GetEditorStateByType(StateType type)
    {
        EditorState state = GetEditorState();
        switch (type)
        {
            case StateType.ISPLAY:
                return (state == EditorState.EDITORPLAY 
                    || state == EditorState.BUILDPLAY);
            case StateType.ISEDITOR:
#if UNITY_EDITOR
                return true;
#else
                return false;
#endif
            default:
                return false;
        }
    }
}
