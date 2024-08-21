#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System;
using UnityEngine.UIElements;

public static class CustomGUI
{
    static Event e => Event.current;

    public static List<ObjectT> ListField<ObjectT>(
        Rect rect, List<ObjectT> list, 
        ref int holdedIndex, ref float holdOffset, 
        float indexWidth, float elementHeight,
        float buttonFontSize = 20, float buttonWidth = 40, float buttonHeight = 20
    ) where ObjectT : UnityEngine.Object
    {
        switch (e.type)
        {
            case EventType.MouseDown:
                for (int i = 0; i < list.Count; i++)
                {
                    Rect palleteIndexRect = new Rect(
                        rect.x,
                        rect.y + i * elementHeight,
                        indexWidth, 
                        elementHeight
                    );
                    if (palleteIndexRect.Contains(e.mousePosition))
                    {
                        holdedIndex = i;
                        holdOffset = e.mousePosition.y - palleteIndexRect.y;
                        break;
                    }
                }
                break;
            case EventType.MouseDrag:
                if (holdedIndex != -1)
                {
                    int moveIndex = Mathf.RoundToInt((e.mousePosition.y - rect.y) / elementHeight);

                    if (moveIndex != holdedIndex)
                    {
                        ObjectT obj = list[holdedIndex];
                        list.RemoveAt(holdedIndex);
                        list.Insert(moveIndex, obj);
                        holdedIndex = moveIndex;
                    }
                }
                break;
            case EventType.MouseUp:
                if (holdedIndex != -1)
                {
                    holdedIndex = -1;
                }
                break;
        }
        for (int i = 0; i < list.Count; i++)
        {
            if (i == holdedIndex)
            {
                EditorGUILayout.Space(elementHeight);
                continue;
            }
            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.LabelField(i.ToString(), GUILayout.Width(25), GUILayout.Height(elementHeight));

                list[i] = (ObjectT)EditorGUILayout.ObjectField(
                    string.Empty, 
                    list[i], 
                    typeof(ObjectT), 
                    true
                );
            }
            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.BeginHorizontal();
        {
            GUILayout.FlexibleSpace();

            GUI.skin.button.alignment = TextAnchor.MiddleCenter;
            GUI.skin.button.richText = true;
            bool addButtonDown = GUILayout.Button(
                "<size=20><b>+</b></size>",
                GUILayout.Width(buttonWidth),
                    GUILayout.Height(buttonHeight)
            );
            bool removeButtonDown = GUILayout.Button(
                "<size=20><b>-</b></size>",
                GUILayout.Width(buttonWidth),
                    GUILayout.Height(buttonHeight)
            );
            GUI.skin.button.richText = false;
            GUI.skin.button.alignment = TextAnchor.MiddleLeft;
        }
        EditorGUILayout.EndHorizontal();
        return list;
    }
    public static void DrawSquare(Rect rect, SquareColor color)
    {
        Handles.DrawSolidRectangleWithOutline(rect, color.face, color.outline);
    }
    public static void DrawSquare(Rect rect, Color color)
    {
        Handles.DrawSolidRectangleWithOutline(rect, color, color);
    }
    public static void DrawArrow(Vector2 start, Vector2 end, float angle, float length)
    {
        Handles.DrawLine(start, end);

        Vector2 reverseDir = (start - end).normalized;

        Vector2 a = end + reverseDir.Rotate(angle) * length;
        Vector2 b = end + reverseDir.Rotate(-angle) * length;

        Handles.DrawLine(end, a);
        Handles.DrawLine(end, b);
    }
    public static void DrawOpenGrid(Vector2 pos, Vector2Int cellCount, float cellSize, Color color)
    {
        Handles.color = color;
        Vector2 end = pos + cellSize * (Vector2)cellCount;
        for (int i = 1; i < cellCount.x; i++)
        {
            float x = pos.x + i * cellSize;
            Vector3 p1 = new Vector3(x, pos.y);
            Vector3 p2 = new Vector3(x, end.y);

            Handles.DrawLine(p1, p2);
        }
        for (int i = 1; i < cellCount.y; i++)
        {
            float y = pos.y + i * cellSize;
            Vector3 p1 = new Vector3(pos.x, y);
            Vector3 p2 = new Vector3(end.x, y);

            Handles.DrawLine(p1, p2);
        }
    }
    public static void DrawCloseGrid(Vector2 pos, Vector2Int cellCount, float cellSize, Color color)
    {
        Handles.color = color;
        Vector2 end = pos + cellSize * (Vector2)cellCount;
        for (int i = 0; i <= cellCount.x; i++)
        {
            float x = pos.x + i * cellSize;
            Vector3 p1 = new Vector3(x, pos.y);
            Vector3 p2 = new Vector3(x, end.y);

            Handles.DrawLine(p1, p2);
        }
        for (int i = 0; i <= cellCount.y; i++)
        {
            float y = pos.y + i * cellSize;
            Vector3 p1 = new Vector3(pos.x, y);
            Vector3 p2 = new Vector3(end.x, y);

            Handles.DrawLine(p1, p2);
        }
    }
    public static void HorizontalLine(Vector2 pos, float width)
    {
        if (e.type != EventType.Repaint) return;

        Vector3 p1 = new Vector3(pos.x, pos.y);
        Vector3 p2 = new Vector3(pos.x + width, pos.y);
        Handles.DrawLine(p1, p2);
    }
    public static void UnderBarTitleText(Rect rect, string label)
    {
        EditorGUI.LabelField(rect, label, EditorStyles.boldLabel);

        HorizontalLine(rect.position.SetY(rect.yMax), rect.width);
    }

    public static void TitleHeaderLabel(Rect rect, string title)
    {
        CustomGUI.UnderBarTitleText(rect, title);
    }
    public static ObjectT InteractionObjectField<ObjectT>(Rect rect, ObjectT obj, Action interaction, bool allowSceneObject) where ObjectT : UnityEngine.Object
    {
        interaction.Invoke();

        return (ObjectT)EditorGUI.ObjectField(rect, obj, typeof(ObjectT), allowSceneObject);
    }
}
#endif