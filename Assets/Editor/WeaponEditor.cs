using System.IO;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class WeaponEditor : EditorWindow
{
    string loadWeaponFolderPath = "Assets/Weapon/Resources/WeaponObjPrefab/";
    [SerializeField]
    private VisualTreeAsset m_VisualTreeAsset = default;

    [MenuItem("Window/UI Toolkit/WeaponEditor")]
    public static void ShowExample()
    {
        WeaponEditor wnd = GetWindow<WeaponEditor>();
        wnd.titleContent = new GUIContent("WeaponEditor");
    }

    public void CreateGUI()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;
        /*
        // VisualElements objects can contain other VisualElement following a tree hierarchy.
        VisualElement label = new Label("Hello World! From C#");
        root.Add(label);
        */


        var toolbarPopupSearchField = new ToolbarPopupSearchField();
        toolbarPopupSearchField.value = loadWeaponFolderPath;

        //string[] weaponFiles = Directory.GetFiles(loadWeaponFolderPath, "*.prefab", SearchOption.TopDirectoryOnly);

        //for (int i = 0; i < weaponFiles.Length; i++)
        //{
        //    toolbarPopupSearchField.menu.AppendAction(weaponFiles[i], action => LoadWeapon(weaponFiles[i]));
        //}
        //root.Add(toolbarPopupSearchField);
        //toolbarPopupSearchField.RegisterValueChangedCallback(evt => Debug.Log("New search value: " + evt.newValue));

        // Instantiate UXML
        VisualElement labelFromUXML = m_VisualTreeAsset.Instantiate();
        root.Add(labelFromUXML);
    }
    void LoadWeapon(string path)
    {
        Debug.Log($"Load Weapon from {path}");
    }
    private UnityEditor.Editor componentEditor;
    private Skill targetComponent;

    private void OnGUI()
    {
        GUILayout.Label("Component Inspector", EditorStyles.boldLabel);

        targetComponent = (Skill)EditorGUILayout.ObjectField("Component", targetComponent, typeof(Skill), true);

        if (targetComponent != null)
        {
            if (componentEditor == null || componentEditor.target != targetComponent)
            {
                componentEditor = UnityEditor.Editor.CreateEditor(targetComponent);
            }

            if (componentEditor != null)
            {
                componentEditor.OnInspectorGUI();
            }
        }
    }

    private void OnDisable()
    {
        if (componentEditor != null)
        {
            DestroyImmediate(componentEditor);
        }
    }
}
