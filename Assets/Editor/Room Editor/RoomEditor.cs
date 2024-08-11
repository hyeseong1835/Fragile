using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Experimental;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

public class RoomEditor : EditorWindow, IHasCustomMenu
{
    public static RoomEditorData data;
    public float scroll;

    public void AddItemsToMenu(GenericMenu menu)
    {
        menu.AddItem(
            new GUIContent("Refresh"),
            false,
            Refresh
        );
    }
    [MenuItem("Window/RoomEditor")]
    public static void ShowWindow()
    {
        RoomEditor window = GetWindow<RoomEditor>();
        window.Show();
    }
    protected void OnEnable()
    {
        SceneView.duringSceneGui += OnSceneGUI;
        Refresh();
        data.scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        SceneManager.SetActiveScene(data.scene);
    }
    protected void OnDisable()
    {
        SceneView.duringSceneGui -= OnSceneGUI;
        if (data != null) EditorUtility.SetDirty(data);
        if (data.scene != null) EditorSceneManager.CloseScene(data.scene, true);
    }

    public void Refresh()
    {
        data = (RoomEditorData)EditorGUIUtility.Load("Room Editor/Data.asset");
    }
    
    void OnGUI()
    {
        if (data == null)
        {
            CustomGUILayout.WarningLabel("Editor Data is Null");
            return;
        }
        DrawPhaseList();
    }

    void DrawPhaseList()
    {
        CustomGUILayout.TitleHeaderLabel("Phase");
        if (data.room == null)
        {
            CustomGUILayout.WarningLabel("Room Data is Null");
            return;
        }
        if (data.room.phaseDataList == null)
        {
            CustomGUILayout.WarningLabel("Phase Data List is Null");
            return;
        }
        CustomGUILayout.CustomList(
            data.room.phaseDataList,
            25,
            ref scroll,
            (i) => {
                EditorPhaseData phase = data.room.phaseDataList[i];
            }
        );
    }
    void OnSceneGUI(SceneView view)
    {
        if (data == null) return;

        if (data.room == null) return;
        if (data.room.phaseDataList == null) return;

        foreach (EditorPhaseData phase in data.room.phaseDataList)
        {
            if (data.curPhaseIndex == -1)
            {
                foreach (EditorSpawn spawnData in phase.spawnDataList)
                {
                    spawnData.OnSceneGUI(view);
                }
            }
        }
    }
}
