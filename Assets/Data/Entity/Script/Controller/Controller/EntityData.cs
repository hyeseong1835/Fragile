using Sirenix.OdinInspector;
using UnityEngine;

public abstract class EntityData : ScriptableObject
{
    /// <summary> 
    /// 엔티티 폴더 경로
    /// </summary>
    public const string entityFolderPath = "Assets/Data/Entity";
    /// <summary> 
    /// 리소스 폴더 경로
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
