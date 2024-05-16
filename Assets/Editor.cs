using UnityEngine;
using Sirenix.OdinInspector;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

public class Editor : MonoBehaviour
{
    #region Inspector  - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -|

    [ShowInInspector]
    public const float propertyHeight = 20;

    [ShowInInspector]
    public const int propertyFontSize = 12;

    [ShowInInspector]
    public const int titleFontSize = 20;

    [ShowInInspector]
    public const float titleHeight = 30;

    [ShowInInspector]
    public const float propertyLabelWidth = 125;

    [ShowInInspector]
    public const float shortNoLabelPropertyWidth = 50;
    
    [ShowInInspector]
    public const float overrideSpace = 10;
    
    [ShowInInspector]
    public const float shortFunctionButtonWidth = 150;

    [ShowInInspector]
    public const float shortPropertyWidth = 150;

    [ShowInInspector]
    public const float childGameObjectOnlyWidth = 16.5f;



    #endregion - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -|

    #region Editor - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -|

    public enum EditorState
    {
        Unknown, BuildPlay, Editor, EditorPlay, EditorPlayPaused, EditorPlayCompiling
    }
    [ShowInInspector]
    public static EditorState state { 
        get { 
            #if UNITY_EDITOR
        
                //플레이
                if (EditorApplication.isPlaying) return EditorState.EditorPlay;

                //정지
                if (EditorApplication.isPaused) return EditorState.EditorPlayPaused;

                //컴파일 중
                if (EditorApplication.isCompiling) return EditorState.EditorPlayCompiling;

                return EditorState.Editor;

            #else
                //빌드
                return EditorState.BuildPlay;
            #endif 
        } 
    }

   
    public enum StateType
    {
        IsPlay, IsEditor, IsBuild
    }
    public enum ObjectState
    {
        Null, PrefabEdit, Prefab, NotPrefab
    }
    public static bool GetType(StateType type)
    {
        switch (type)
        {
            case StateType.IsPlay:
                #if UNITY_EDITOR
                    EditorState editorState = state;

                    return (editorState == EditorState.EditorPlay 
                        || editorState == EditorState.BuildPlay);
                #else 
                    return true;
                #endif
            case StateType.IsEditor:
                #if UNITY_EDITOR
                    return true;
                #else
                    return false;
                #endif
            case StateType.IsBuild:
                #if UNITY_EDITOR
                    return false;
                #else
                    return true;
                #endif
            default:
                return false;
        }
    }

    public static ObjectState GetObjectState(GameObject gameObject)
    {
        #if UNITY_EDITOR

            if (gameObject == null) return ObjectState.Null;
        
            if (StageUtility.GetStage(gameObject) != StageUtility.GetMainStage()) return ObjectState.PrefabEdit;
            
            return ObjectState.NotPrefab;

        #else
        
            return ObjectState.NotPrefab;
        
        #endif
    }
    

    #endregion - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -|
}
