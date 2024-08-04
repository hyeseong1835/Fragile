using Sirenix.OdinInspector;
using UnityEngine;

public abstract class EntityData : ScriptableObject
{
    /// <summary> 
    /// ��ƼƼ ���� ���
    /// </summary>
    public const string entityFolderPath = "Assets/Data/Entity";
    /// <summary> 
    /// ���ҽ� ���� ���
    /// </summary>
    public const string entityResourceFolderPath = EntityData.entityFolderPath + "/Resources";

    [HorizontalGroup("Name")]
    #region Horizontal Name  - - - - - - - - - - -|  

        [ReadOnly]
        [LabelWidth(Editor.propertyLabelWidth)]//-|
        new public string name;
        
        #if UNITY_EDITOR
        [HorizontalGroup("Name", width: Editor.shortButtonWidth)]
        [Button("Reset")]
        public abstract void ResetName();
        #endif
    
    #endregion - - - - - - - - - - - - - - - - - -|

    [LabelWidth(Editor.propertyLabelWidth)]
    public float maxHp = 1;
}
