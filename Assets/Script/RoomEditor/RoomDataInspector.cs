using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(RoomData))]
public class RoomDataInspector : UnityEditor.Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        RoomData roomData = (RoomData)target;

        if (GUILayout.Button("Open Room Editor File View"))
        {
            RoomEditorFileView.ShowWindow();
        }
    }
}