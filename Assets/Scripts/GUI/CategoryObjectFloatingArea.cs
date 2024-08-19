/*
#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;

public class CategoryObjectFloatingArea : FloatingArea
{
    Action<int, int> selectEvent;
    string[] categoryNameArray;
    UnityEngine.Object[][] objectArray;

    Rect[] objectRectArray;
    float height;
    
    public CategoryObjectFloatingArea(string[] categoryNameArray, UnityEngine.Object[][] objectArray, Action<int, int> selectEvent = null)
    {
        this.categoryNameArray = categoryNameArray;
        this.objectArray = objectArray;
        this.selectEvent = selectEvent;
    }
    public override void EventListen(Event e)
    {
        if (objectRectArray != null)
        {
            if (EventUtility.MouseDown(0))
            {
                for (int i = 0; i < objectRectArray.Length; i++)
                {
                    Rect rect = objectRectArray[i];

                    if (rect.Contains(e.mousePosition))
                    {
                        if (ArrayUtility.TryTransformSingleToDoubleArrayIndex(objectArray, i, out int i1, out int i2))
                        {
                            selectEvent?.Invoke(i1, i2);
                        }
                        else Debug.LogWarning("Index not found");
                        break;
                    }
                }
            }
        }
    }
    public override void Draw()
    {
        height = 0;
        if (Event.current.type == EventType.Repaint)
        {
            objectRectArray = new Rect[objectArray.GetSingleLength()];
        }
        GUILayout.BeginArea(manager.rect);
        {
            int index = 0;
            for (int categoryIndex = 0; categoryIndex < categoryNameArray.Length; categoryIndex++)
            {
                UnityEngine.Object[] array = objectArray[categoryIndex];
                string categoryName = categoryNameArray[categoryIndex];

                CustomGUILayout.UnderBarTitleText(categoryName);
                height += CustomGUILayout.UnderBarTitleTextHeight;
                if (array.Length == 0)
                {
                    CustomGUILayout.WarningLabel("Empty List");
                    height += CustomGUILayout.WarningLabelHeight;
                    continue;
                }

                for (int elementIndex = 0; elementIndex < array.Length; elementIndex++)
                {
                    EditorGUILayout.ObjectField(array[elementIndex], typeof(UnityEngine.Object), true);
                    if (Event.current.type == EventType.Repaint)
                    {
                        objectRectArray[index++] = GUILayoutUtility.GetLastRect().AddPos(manager.rect.position);
                    }
                    height += EditorGUIUtility.singleLineHeight;
                }
            }
            height += 25;
        }
        GUILayout.EndArea();
    }
    public override float GetHeight()
    {
        return height;
    }
}
#endif
*/