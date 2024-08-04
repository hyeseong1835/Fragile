using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental;
using UnityEditor;
using UnityEngine;
using UnityEngine.XR;
using Unity.VisualScripting;

public enum HandMode
{
    NONE, ToHand, ToTarget
}
public class HandGrafic : MonoBehaviour
{
    public const string HAND_NAME = "Hand";
    [Required][ReadOnly] 
    public Controller con;

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

    #if UNITY_EDITOR
    public static HandGrafic SetHandGrafic(Controller con)
    {
        HandGrafic hand;
        Transform handTransform = con.transform.Find(HAND_NAME);
        if (handTransform == null)
        {
            hand = con.transform.GetComponentInChildren<HandGrafic>();
            if (hand != null)
            {
                hand.gameObject.name = HAND_NAME;
                Debug.LogWarning($"Hand name must be \"{HAND_NAME}\"");
            }
        }
        else
        {
            hand = handTransform.GetComponent<HandGrafic>();
            if (hand == null)
            {
                hand = handTransform.gameObject.AddComponent<HandGrafic>();
                hand.con = con;
                Debug.Log("AddComponent Hand");
                return hand;
            }
        }

        hand = new GameObject(HAND_NAME).AddComponent<HandGrafic>();
        hand.transform.parent = con.transform;
        hand.con = con;
        Debug.Log("Create Hand");
        return hand;
    }
    #endif

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
                if (targetTransform == null) 
                {
                    targetTransform = con.curWeapon.hand_obj;
                }
                if (handle == null)
                {
                    handle = targetTransform.Find("Handle");
                }
                
                targetTransform.position = transform.position + (targetTransform.position - handle.position);
                targetTransform.rotation = transform.rotation;
                break;

            case HandMode.ToTarget:
                if (targetTransform == null)
                {
                    targetTransform = con.curWeapon.hand_obj;
                }
                if (handle == null)
                {
                    handle = targetTransform.Find("Handle");
                }
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
