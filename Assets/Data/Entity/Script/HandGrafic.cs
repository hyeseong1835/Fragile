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
        /// 손을 연결함
        /// </summary>
        /// <param name="target"></param>
        /// <param name="_IK">제어권 [true: 무기, false: 손]</param>
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
                    if (handle == null) { Debug.LogError("Handle is null"); return; }

                    break;

                case HandMode.ToTarget:
                    if (target == null) Debug.LogError("Parameter: {TargetTransform} is null");
                    targetTransform = target;
                    handle = target.Find("Handle");
                    if (handle == null) { Debug.LogError("Handle is null"); return; }
                    
                    break;
            }
            handMode = mode;
        }

    #endregion


    void Awake()
    {

    }

    void LateUpdate()
    {
        Hand();
    }

    /// <summary>
    /// 연결 해제
    /// </summary>
    /// <param name="target"></param>
    public void HandLink(Transform target)
    {
        if (target != null) Debug.LogWarning("Parameter: {target} must be null");

        handMode = HandMode.NONE;

        targetTransform = null;
        handle = null;
    }
    void Hand()
    {
        switch (handMode)
        {
            case HandMode.ToHand:
                if (targetTransform == null) { Debug.LogError("TargetTransform is null"); return; }
                
                targetTransform.position = transform.position + (targetTransform.position - handle.position);
                targetTransform.rotation = transform.rotation;
                break;

            case HandMode.ToTarget:
                if (targetTransform == null) { Debug.LogError("TargetTransform is null"); return; }

                transform.position = handle.position;
                transform.rotation = handle.rotation;
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
