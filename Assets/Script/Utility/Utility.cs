using System;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public static class Utility 
{
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
        UNKNOWN, BUILDPLAY, EDITORPLAY, EDITORPLAYPAUSE, EDITORPLAYCOMPILING, PREFABEDIT
    }
    public enum StateType
    {
        ISPLAY, ISEDITOR, ISBUILD
    }
    public static EditorState GetEditorState(GameObject gameObject)
    {
#if UNITY_EDITOR
        //�÷���
        if (EditorApplication.isPlaying) return EditorState.EDITORPLAY;
        
        //������ ����
        if (gameObject != null && StageUtility.GetStage(gameObject) != StageUtility.GetMainStage()) return EditorState.PREFABEDIT;

        //����
        if (EditorApplication.isPaused) return EditorState.EDITORPLAYPAUSE;

        //������ ��
        if (EditorApplication.isCompiling) return EditorState.EDITORPLAYCOMPILING;


        return EditorState.UNKNOWN;
#else
        //����
        return EditorState.BUILDPLAY;
#endif
    }
    public static bool GetEditorStateByType(StateType type)
    {
        EditorState state = GetEditorState(null);
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
            case StateType.ISBUILD:
                #if UNITY_EDITOR
                    return false;
                #else
                    return true;
                #endif
            default:
                return false;
        }
    }
}
