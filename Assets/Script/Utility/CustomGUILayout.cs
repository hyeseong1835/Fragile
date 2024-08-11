using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class CustomGUILayout
{
    public static float UnderBarTitleTextHeight => EditorGUIUtility.singleLineHeight;
    public static float WarningLabelHeight => EditorStyles.centeredGreyMiniLabel.lineHeight + 20;
    public static float TitleHeaderLabelHeight => UnderBarTitleTextHeight + 5;

    static Event e => Event.current;
    public static List<ObjectT> ListField<ObjectT>(
        List<ObjectT> list,
        ref int holdedIndex, ref float holdOffset,
        float indexWidth = 25, float elementHeight = 25
    ) where ObjectT : UnityEngine.Object
    {
        Rect rect = GUILayoutUtility.GetRect(indexWidth, list.Count * elementHeight);

        return CustomGUI.ListField(
            rect, list, 
            ref holdedIndex, ref holdOffset, 
            indexWidth, elementHeight
        );
    }
    public static ObjectT InteractionObjectField<ObjectT>(ObjectT obj, Action<Rect> interaction, float width = -1, float height = -1) where ObjectT : UnityEngine.Object
    {
        if (width == -1) width = GUILayoutUtility.GetLastRect().width;
        if (height == -1) height = EditorStyles.objectField.lineHeight;
        Rect rect = GUILayoutUtility.GetRect(width, height);

        interaction.Invoke(rect);

        return (ObjectT)EditorGUI.ObjectField(rect, obj, typeof(ObjectT), false);
    }
    public static void PreventMouseObjectField<ObjectT>(ObjectT obj, float width = -1, float height = -1) where ObjectT : UnityEngine.Object
    {
        if (width == -1) width = GUILayoutUtility.GetLastRect().width;
        if (height == -1) height = EditorStyles.objectField.lineHeight;
        Rect rect = GUILayoutUtility.GetRect(width, height);

        if (e.isMouse && rect.Contains(e.mousePosition)) e.Use();

        EditorGUI.ObjectField(rect, obj, typeof(ObjectT), false);
    }
    public static void UnderBarTitleText(string label)
    {
        EditorGUILayout.LabelField(label, EditorStyles.boldLabel);
        Rect labelRect = GUILayoutUtility.GetLastRect();
        float lineY = labelRect.position.y + EditorStyles.boldLabel.lineHeight + 3;
        Vector3 p1 = new Vector3(labelRect.x, lineY);
        Vector3 p2 = new Vector3(labelRect.x + labelRect.width, lineY);
        Handles.DrawLine(p1, p2);
    }
    public static void WarningLabel(string message)
    {
        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField(message, EditorStyles.centeredGreyMiniLabel);
        EditorGUILayout.Space(10);
    }
    public static void TitleHeaderLabel(string title)
    {
        EditorGUILayout.Space(5);
        CustomGUILayout.UnderBarTitleText(title);
    }
    public static void BeginTab(float space = 10)
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.Space(space, false);
        EditorGUILayout.BeginVertical();
    }
    public static void EndTab()
    {
        EditorGUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();
    }
    public static List<ElementT> CustomList<ElementT>(
        List<ElementT> list, 
        float elementHeight, ref float scroll, 
        Action<int> drawElement, 
        bool alwaysShowScrollBar = false, GUIStyle verticalScrollBarStyle = null
    )
    {
        Rect rect = GUILayoutUtility.GetRect(0, elementHeight * list.Count);
        return CustomGUI.CustomList(rect, list, ref scroll, drawElement, alwaysShowScrollBar, verticalScrollBarStyle);
    }
}