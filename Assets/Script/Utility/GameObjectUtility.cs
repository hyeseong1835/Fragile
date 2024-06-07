using UnityEngine;

public static class GameObjectUtility
{
    public static void AutoDestroy(this GameObject obj)
    {
        if (Editor.GetApplicationType(Editor.StateType.IsPlay)) Object.Destroy(obj);
        else Object.DestroyImmediate(obj);
    }
}