using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

public enum AnimationState
{
    NONE, STAY, WALK, BATTLE
}
public enum HandMode
{
    NONE, ToHand, ToTarget
}
[ExecuteAlways]
public class Grafic : MonoBehaviour
{
    #if UNITY_EDITOR
    
        [SerializeField] protected bool debug = false;
    
    #endif

    [VerticalGroup("Base")]
    #region Base
        
        [VerticalGroup("Base")][SerializeField] 
            protected Controller con;
    
        [VerticalGroup("Base")][SerializeField] 
            protected Texture2D texture;

        [VerticalGroup("Base")][SerializeField]
            [ChildGameObjectsOnly]
            protected SpriteRenderer body;
    
    #endregion

    [FoldoutGroup("State")]
    #region State

        [ShowInInspector]
            public AnimationState animationState
            {
                get { return _animationState; }
                set
                {
                    if (value != AnimationState.NONE)
                    {
                        if (this is OtherGrafic) ((OtherGrafic)this).StateSetToNONE();
                    }

                    _animationState = value;
                } 
            } AnimationState _animationState;

    #endregion

    [FoldoutGroup("Hand")]
    #region Hand

        [HorizontalGroup("Hand/Horizontal")]
        #region Horizontal
            
            [SerializeField]
                [Required][ChildGameObjectsOnly]
                protected Transform hand;

            [VerticalGroup("Hand/Horizontal/Vertical")]
            #region Vertical

                    [LabelText("Mode")]
                    public HandMode handMode = HandMode.NONE;

                [VerticalGroup("Hand/Horizontal/Vertical")]
                    [ShowIf("debug")][LabelText("Target")]
                    public Transform targetTransform;

            #endregion

        #endregion
    #endregion

    [PropertySpace(10)]

    [FoldoutGroup("Animation")]
    #region Animation

        [FoldoutGroup("Animation/Stay")]
        #region Stay

            [BoxGroup("Animation/Stay/Texture")]
            #region Texture

                [ShowInInspector]
                    [ShowIf("debug")]
                    [TableMatrix(IsReadOnly = true, SquareCells = true, HorizontalTitle = "Rotation", VerticalTitle = "Frame")] Sprite[,] stayFrame;
                
                [PropertySpace(10)]
            
                [BoxGroup("Animation/Stay/Texture")][SerializeField]
                    [LabelText("Start Sprite")] Vector2Int stayFrameTextureAnchor = new Vector2Int(0, 0);
            
                [BoxGroup("Animation/Stay/Texture")][SerializeField]
                    [LabelText("Array Length")] Vector2Int stayFrameTextureSize = new Vector2Int(8, 2);
    
                [BoxGroup("Animation/Stay/Texture")][SerializeField]
                    [LabelText("Sprite Size ")]Vector2Int stayFrameSpriteSize = new Vector2Int(16, 16);

            #endregion

            [BoxGroup("Animation/Stay/Time")]
            #region Time        

                [HorizontalGroup("Animation/Stay/Time/Horizontal")]
                #region Horizontal

                    [ShowInInspector]
                        [ShowIf("debug")]
                        [Range(0, 0.9999f)] protected float stayTime = 0;

                    [HorizontalGroup("Animation/Stay/Time/Horizontal", width: 10, marginRight: 5, marginLeft: 2)][ShowInInspector]
                        [HideLabel]
                        protected bool staySimulate
                        {
                            get { return _staySimulate; }
                            set
                            {
                                if (value == true) stayTime = Random.Range(0f, 0.9999f);

                                _staySimulate = value;
                            }
                        }protected bool _staySimulate;

                #endregion

                [BoxGroup("Animation/Stay/Time")][SerializeField]
                    [LabelText("Time")]
                    float stayTimeScale = 0.5f;

            #endregion
        
        #endregion
        
        [PropertySpace(10)]

        [FoldoutGroup("Animation/Walk")]
        #region Walk

            [BoxGroup("Animation/Walk/Texture")]    
            #region Texture
                
                [ShowInInspector]
                    [ShowIf("debug")]
                    [TableMatrix(IsReadOnly = true, SquareCells = true, HorizontalTitle = "Rotation", VerticalTitle = "Frame")] Sprite[,] walkFrame;
                
                [PropertySpace(10)]
            
                [BoxGroup("Animation/Walk/Texture")][SerializeField]
                    [LabelText("Start Sprite")] 
                    Vector2Int walkFrameTextureAnchor = new Vector2Int(0, 2);

                [BoxGroup("Animation/Walk/Texture")][SerializeField]
                    [LabelText("Array Length")] 
                    Vector2Int walkFrameTextureSize = new Vector2Int(8, 4);

                [BoxGroup("Animation/Walk/Texture")][SerializeField]
                    [LabelText("Sprite Size ")] 
                    Vector2Int walkFrameSpriteSize = new Vector2Int(16, 16);

            #endregion

            [BoxGroup("Animation/Walk/Time")]
            #region Time

                [HorizontalGroup("Animation/Walk/Time/Horizontal")]
                #region Horizontal

                    [SerializeField]
                        [ShowIf("debug")]
                        [Range(0, 0.9999f)] protected float walkTime = 0;

                    [HorizontalGroup("Animation/Walk/Time/Horizontal", width: 10, marginRight: 5, marginLeft: 2)][ShowInInspector]
                        [ShowIf("debug")][HideLabel] 
                        protected bool walkSimulate
                                        {
                                            get { return _walkSimulate; }
                                            set
                                            {
                                                if (value == true) stayTime = Random.Range(0f, 0.9999f);

                                                _walkSimulate = value;
                                            }
                                        }protected bool _walkSimulate;
                    
                #endregion

                [BoxGroup("Animation/Walk/Time")][SerializeField]
                    [LabelText("Time")]
                    float walkTimeScale = 0.1f;

            #endregion
        
        #endregion
    
    #endregion

    void Awake()
    {

    }
    void Start()
    {
        staySimulate = true;
        walkSimulate = true;
    }
    void Update()
    {
        if (Selection.Contains(this) == false)
        {
            staySimulate = false;
            walkSimulate = false;
        }

        if (staySimulate) AnimationUpdate(ref stayTime, stayTimeScale);
        if (walkSimulate) AnimationUpdate(ref walkTime, walkTimeScale);

        Animation();
        if (this is OtherGrafic) ((OtherGrafic)this).OtherAnimation();

        if (EditorApplication.isPlaying)
        {
            StateUpdate();
            Hand();
        }
        else
        {
            if (Selection.Contains(gameObject) || Selection.Contains(transform.parent.gameObject))
            {
                if (stayFrame == null)
                {
                    stayFrame = Utility.GetSpriteArray2DFromSpriteSheet(texture, stayFrameTextureAnchor, stayFrameTextureSize, stayFrameSpriteSize);
                    body.sprite = stayFrame[0, 0];
                }
                if (walkFrame == null)
                {
                    walkFrame = Utility.GetSpriteArray2DFromSpriteSheet(texture, walkFrameTextureAnchor, walkFrameTextureSize, walkFrameSpriteSize);
                }
            }
            else
            {
                if (debug)
                {
                    staySimulate = true;
                    walkSimulate = true;
                }
                else
                {
                    staySimulate = false;
                    walkSimulate = false;
                }
            }
        }
    }
    public virtual void StateUpdate()
    {
        //Stay => Walk
        if (animationState == AnimationState.STAY &&
            con.moveVector.magnitude > Mathf.Epsilon) animationState = AnimationState.WALK;

        //Walk => Stay
        if (animationState == AnimationState.WALK &&
            con.moveVector.magnitude <= Mathf.Epsilon) animationState = AnimationState.STAY;
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
    }
    /// <summary>
    /// 손을 연결함
    /// </summary>
    /// <param name="target"></param>
    /// <param name="_IK">제어권 [true: 무기, false: 손]</param>
    [HorizontalGroup("Hand/Hand")]
    [ShowIf("debug")]
    [Button]
    public void HandLink(HandMode mode, Transform target)
    {
        target.gameObject.SetActive(true);

        handMode = mode;

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
    }
    void Hand()
    {
        switch (handMode)
        {
            case HandMode.NONE:
                break;

            case HandMode.ToHand:
                if (targetTransform == null) Debug.LogError("{TargetTransform} is null");
                
                targetTransform.position = hand.position;
                targetTransform.rotation = hand.rotation;
                break;

            case HandMode.ToTarget:
                if (targetTransform == null) Debug.LogError("{TargetTransform} is null");
                
                hand.position = targetTransform.position;
                hand.rotation = targetTransform.rotation;
                break;
        }
    }
    void Animation()
    {
        //상태에 따른 애니메이션
        switch (animationState)
        {
            case AnimationState.NONE:
                if (this is OtherGrafic) ((OtherGrafic)this).OtherAnimation();
                break;
            case AnimationState.STAY:
                //Body
                if (stayFrame == null) stayFrame = Utility.GetSpriteArray2DFromSpriteSheet(texture, stayFrameTextureAnchor, stayFrameTextureSize, stayFrameSpriteSize);

                body.sprite = stayFrame[Utility.FloorRotateToInt(con.moveRotate, 8), Mathf.FloorToInt(stayTime * stayFrameTextureSize.y)];

                break;
            case AnimationState.WALK:
                //Body
                if (walkFrame == null) walkFrame = Utility.GetSpriteArray2DFromSpriteSheet(texture, walkFrameTextureAnchor, walkFrameTextureSize, walkFrameSpriteSize);

                body.sprite = walkFrame[Utility.FloorRotateToInt(con.moveRotate, 8), Mathf.FloorToInt(walkTime * walkFrameTextureSize.y)];

                //Hand
                if (handMode == HandMode.ToHand)
                {
                    if (con.prevMoveVector.magnitude <= con.moveVector.magnitude || con.moveVector.magnitude >= 1)
                        hand.localPosition = new Vector3(con.moveVector.normalized.x * 0.75f, con.moveVector.normalized.y * 0.5f, 0)
                            + (new Vector3(con.moveVector.normalized.y, con.moveVector.normalized.x * -0.5f) * 0.25f);
                    hand.localRotation = Quaternion.identity;
                }

                break;
        }
    }
    void AnimationUpdate(ref float time, float timeScale)
    {
        time += Time.deltaTime / timeScale;
        if (time >= 1) time = 0;
    }
}
