using UnityEditor;
using UnityEngine;
using WeaponSystem;

[CustomEditor(typeof(WeaponData))]
public class WeaponDataInspector : Editor
{
    const float labelHeight = 20;
    const float textHeight = 25;

    WeaponData weaponData;
    SerializedProperty rule;
    Editor ruleEditor;

    void OnEnable()
    {
        weaponData = (WeaponData)target;
        rule = serializedObject.FindProperty("rule");
    }
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        //EditorGUILayout.BeginVertical();
        {
            DrawDescription();

            EditorGUILayout.Space(10);
            EditorGUILayout.PropertyField(
                rule, 
                label: new GUIContent("규칙")
            );
            Editor.CreateCachedEditor(
                rule.objectReferenceValue, 
                null, 
                ref ruleEditor
            );
            ruleEditor?.OnInspectorGUI();
        }
        //EditorGUILayout.EndVertical();

        serializedObject.ApplyModifiedProperties();

        //===============================================================================================

        void DrawDescription()
        {
            EditorGUILayout.BeginHorizontal();
            {
                Rect iconPreviewRect = GUILayoutUtility.GetAspectRect(1,
                    GUILayout.MinWidth(100),
                        GUILayout.MaxWidth(200),
                        GUILayout.MinHeight(100),
                        GUILayout.MaxHeight(200)
                );

                weaponData.icon = (Sprite)EditorGUI.ObjectField(
                    iconPreviewRect,
                    weaponData.icon,
                    typeof(Sprite),
                    false
                );

                if (Event.current.type == EventType.Repaint)
                {
                    Handles.color = Color.white;
                    Handles.DrawWireCube(iconPreviewRect.center, iconPreviewRect.size);
                }

                EditorGUILayout.BeginVertical();
                {
                    EditorGUILayout.LabelField(
                        "무기 이름", 
                        GUILayout.Height(labelHeight),
                            GUILayout.ExpandHeight(false)
                    );
                    weaponData.displayedWeaponName = EditorGUILayout.TextField(
                        weaponData.displayedWeaponName,
                        GUILayout.Height(textHeight),
                            GUILayout.ExpandHeight(false)
                    );
                    EditorGUILayout.LabelField(
                        "무기 설명",
                        GUILayout.Height(labelHeight),
                            GUILayout.ExpandHeight(false)
                    );
                    weaponData.description = EditorGUI.TextArea(
                        new Rect(
                            iconPreviewRect.xMax + 3,
                            iconPreviewRect.y + (2 * labelHeight + textHeight) + 6,
                            EditorGUIUtility.currentViewWidth - iconPreviewRect.width - 25,
                            iconPreviewRect.height - (2 * labelHeight + textHeight) - 2
                        ),
                        weaponData.description
                    );
                }
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndHorizontal();
        }
    }
}