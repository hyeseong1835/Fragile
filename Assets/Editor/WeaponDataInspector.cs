using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.TextCore.Text;

[CustomEditor(typeof(WeaponData))]
public class WeaponDataInspector : Editor
{
    WeaponData weaponData = null;

    void OnEnable()
    {
        weaponData = (WeaponData)target;
    }
    public override void OnInspectorGUI()
    {
        EditorGUILayout.BeginVertical();
        {
            DrawDescription();
        }
        EditorGUILayout.EndVertical();


        void DrawDescription()
        {
            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.BeginVertical();
                {
                    /*
                    Rect iconPreviewRect = GUILayoutUtility.GetAspectRect(1,
                        GUILayout.MinWidth(100),
                            GUILayout.MaxWidth(200),
                            GUILayout.MinHeight(100),
                            GUILayout.MaxHeight(200)
                    );
                    */
                    weaponData.icon = (Sprite)EditorGUILayout.ObjectField(
                        weaponData.icon, 
                        typeof(Sprite), 
                        false, 
                        GUILayout.MinWidth(100),
                            GUILayout.MaxWidth(200),
                            GUILayout.MinHeight(100),
                            GUILayout.MaxHeight(200)
                    );

                    //Handles.color = Color.white;
                    //Handles.DrawWireCube(iconPreviewRect.center, iconPreviewRect.size);
                }
                EditorGUILayout.EndVertical();

                EditorGUILayout.BeginVertical();
                {
                    weaponData.displayedWeaponName = EditorGUILayout.TextField("Name", weaponData.name);
                    weaponData.description = EditorGUILayout.TextField("Description", weaponData.description);
                }
                EditorGUILayout.EndVertical();
            }
        }
    }
    
}