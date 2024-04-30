using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class HandGrafic : MonoBehaviour
{
    [HorizontalGroup("Horizontal")]
    
    #region Horizontal

        [VerticalGroup("Horizontal/Vertical", PaddingTop = 25, PaddingBottom = 2)]
        [ReadOnly]
        #region Vertical

            [LabelText("Target")]
            public Transform targetTransform;
            
            [LabelText("Mode")]                      [VerticalGroup("Horizontal/Vertical")][ReadOnly]
            public HandMode handMode = HandMode.NONE;     

        #endregion

        /// <summary>
        /// 손을 연결함
        /// </summary>
        /// <param name="target"></param>
        /// <param name="_IK">제어권 [true: 무기, false: 손]</param>
        
        [Button]                                                                                [HorizontalGroup("Horizontal")]
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
                    break;

                case HandMode.ToTarget:
                    if (target == null) Debug.LogError("Parameter: {TargetTransform} is null");
                    targetTransform = target;
                    break;
            }
            handMode = mode;
        }
    
    #endregion

    /// <summary>
    /// 연결 해제
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
                if (targetTransform == null) Debug.LogError("{TargetTransform} is null");

                targetTransform.position = transform.position;
                targetTransform.rotation = transform.rotation;
                break;

            case HandMode.ToTarget:
                if (targetTransform == null) Debug.LogError("{TargetTransform} is null");

                transform.position = targetTransform.position;
                transform.rotation = targetTransform.rotation;
                break;
        }
    }
}
