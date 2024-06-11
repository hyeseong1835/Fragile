using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental;
using UnityEditor;
using UnityEngine;
using UnityEngine.XR;

public enum HandMode
{
    NONE, ToHand, ToTarget
}
[ExecuteAlways]
public class HandGrafic : MonoBehaviour
{
    [MenuItem("GameObject/Create Asset/HandGrafic", false, 0)]
    static void Create(MenuCommand menuCommand)
    {
        GameObject parentGameObject = (GameObject)menuCommand.context;
        HandGrafic hand = Instantiate(EditorResources.Load<GameObject>("Preset/HandGrafic.prefab"), parentGameObject.transform).GetComponent<HandGrafic>();
        hand.gameObject.name = "New HandGrafic";
        Controller con = parentGameObject.GetComponent<Controller>();
        con.hand = hand;
    }

    [HorizontalGroup("Horizontal")]
    
    #region Horizontal

        [VerticalGroup("Horizontal/Vertical", PaddingTop = 5, PaddingBottom = 2)]
        
        #region Vertical

            [ReadOnly]
            [LabelText("Target")]
            public Transform targetTransform;
                                                                      [VerticalGroup("Horizontal/Vertical")]
            [ReadOnly] 
            [LabelText("Handle")]
            public Transform handle;
                                                                      [VerticalGroup("Horizontal/Vertical")]
            [ReadOnly]
            [LabelText("Mode")]
            public HandMode handMode = HandMode.NONE;     

        #endregion

        /// <summary>
        /// ���� ������
        /// </summary>
        /// <param name="target"></param>
        /// <param name="_IK">����� [true: ����, false: ��]</param>
        [Button(Expanded = true)]                                                                                [HorizontalGroup("Horizontal")]
        public void HandLink(Transform target, HandMode mode)
        {
            switch (mode)
            {
                case HandMode.NONE:
                    targetTransform = null;
                    break;

                case HandMode.ToHand:
                    if (target == null) Debug.LogError("Parameter: {TargetTransform} is null");
                    targetTransform = target;
                    handle = target.Find("Handle");
                    break;

                case HandMode.ToTarget:
                    if (target == null) Debug.LogError("Parameter: {TargetTransform} is null");
                    targetTransform = target;
                    handle = target.Find("Handle");
                    break;
            }
            handMode = mode;
        }

    #endregion


    void Awake()
    {
        handle = transform.Find("Handle");
    }

    void Update()
    {
        Hand();
    }

    /// <summary>
    /// ���� ����
    /// </summary>
    /// <param name="target"></param>
    public void HandLink(Transform target)
    {
        if (target != null) Debug.LogWarning("Parameter: {target} must be null");

        handMode = HandMode.NONE;

        targetTransform = null;
    }
    public void Hand()
    {
        switch (handMode)
        {
            case HandMode.ToHand:
                if (targetTransform == null) { Debug.LogError("TargetTransform is null"); return; }
                
                handle = targetTransform.Find("Handle");
                if(handle == null) { Debug.LogError("Handle is null"); return; }

                targetTransform.position = transform.position - handle.localPosition;
                targetTransform.rotation = transform.rotation;
                break;

            case HandMode.ToTarget:
                if (targetTransform == null) { Debug.LogError("TargetTransform is null"); return; }

                handle = targetTransform.Find("Handle");
                if (handle == null) { Debug.LogError("Handle is null"); return; }

                transform.position = handle.position;
                transform.rotation = targetTransform.rotation;
                break;
        }
    }
    void OnDrawGizmosSelected()
    {
        switch (handMode)
        {
            case HandMode.NONE:
                Gizmos.color = new Color(0.25f, 0.25f, 0.25f);
                break;
            case HandMode.ToHand:
                Gizmos.color = Color.blue;
                break;
            case HandMode.ToTarget:
                Gizmos.color = Color.red;
                break;
            default:
                Gizmos.color = Color.black;
                break;
        }
        Gizmos.DrawWireSphere(transform.position, 0.25f);
    }
}
