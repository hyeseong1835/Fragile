using System;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

[CustomPropertyDrawer(typeof(SquareColor))]
public class SquareColorDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return 0;
    }
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        SerializedProperty faceColor = property.FindPropertyRelative("face");
        SerializedProperty outlineColor = property.FindPropertyRelative("outline");

        EditorGUILayout.LabelField(label, EditorStyles.boldLabel);
        Rect labelRect = GUILayoutUtility.GetLastRect();
        float lineY = labelRect.position.y + EditorStyles.boldLabel.lineHeight + 3;
        Vector3 p1 = new Vector3(labelRect.x, lineY);
        Vector3 p2 = new Vector3(labelRect.x + labelRect.width, lineY);
        Handles.DrawLine(p1, p2);

        EditorGUILayout.BeginHorizontal();

        EditorGUILayout.LabelField("Face", GUILayout.Width(50));
        faceColor.colorValue = EditorGUILayout.ColorField(faceColor.colorValue);
        
        EditorGUILayout.LabelField("Outline", GUILayout.Width(50));
        outlineColor.colorValue = EditorGUILayout.ColorField(outlineColor.colorValue);

        EditorGUILayout.EndHorizontal();
    }
}
#endif

[Serializable]
public struct SquareColor
{
    public Color face;
    public Color outline;
    public SquareColor(Color face, Color outline)
    {
        this.face = face;
        this.outline = outline;
    }
}