using Sirenix.OdinInspector;
using System.Security.Cryptography;
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
public abstract class Grafic : MonoBehaviour
{
    [VerticalGroup("Base")]
    #region Vertical Base
        
        [VerticalGroup("Base")][SerializeField] 
            protected Controller con;
    
        [VerticalGroup("Base")][SerializeField]
            protected Texture2D texture;

        [VerticalGroup("Base")][SerializeField]
            [ChildGameObjectsOnly]
            protected SpriteRenderer body;
    
    #endregion



    [FoldoutGroup("Hand")]
    #region Foldout Hand

        [SerializeField]
            [Required][ChildGameObjectsOnly]
            protected Transform hand;

        [HorizontalGroup("Hand/Horizontal")]
        #region Horizontal

            [VerticalGroup("Hand/Horizontal/Mode", PaddingBottom = 25)][ReadOnly]
            #region Vertical Mode          

                    [LabelText("Mode")]
                    public HandMode handMode = HandMode.NONE;

                [VerticalGroup("Hand/Horizontal/Mode")][ReadOnly]
                    [LabelText("Target")]
                    public Transform targetTransform;

            #endregion
    
        #endregion

    #endregion

    [FoldoutGroup("Animation")]
    #region Foldout Animation

        [BoxGroup("Animation/State", Order = 0)]
        #region Box State

            [ShowInInspector]
                [LabelText("Animation")]
                public AnimationState animationState
                {
                    get { return _animationState; }
                    set
                    {
                        if (value != AnimationState.NONE)
                        {
                            StateSetToNONE();
                        }

                        _animationState = value;
                    } 
                } AnimationState _animationState;

        #endregion

        [FoldoutGroup("Animation/Stay", Order = 1)]
        #region Foldout Stay

            [PropertySpace(5)]
            [BoxGroup("Animation/Stay/Texture")]
            #region Box Texture

                [ShowInInspector]
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
            #region Box Time        

                [HorizontalGroup("Animation/Stay/Time/Horizontal")]
                #region Horizontal

                    [ShowInInspector]
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
                        } protected bool _staySimulate;

                #endregion

                [BoxGroup("Animation/Stay/Time")][SerializeField]
                    [LabelText("Time")]
                    float stayTimeScale = 0.5f;

            #endregion
        
        #endregion
        
        [FoldoutGroup("Animation/Walk", Order = 2)]
        #region Foldout Walk

            [PropertySpace(5)]
            [BoxGroup("Animation/Walk/Texture")]    
            #region Box Texture
                
                [ShowInInspector]
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
            #region Box Time

                [HorizontalGroup("Animation/Walk/Time/Horizontal")]
                #region Horizontal

                    [SerializeField]
                        [Range(0, 0.9999f)] protected float walkTime = 0;

                    [HorizontalGroup("Animation/Walk/Time/Horizontal", width: 10, marginRight: 5, marginLeft: 2)][ShowInInspector]
                        [HideLabel] 
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
        animationState = AnimationState.STAY;

        staySimulate = true;
        walkSimulate = true;
    }
    void Update()
    {
        //애니메이션 재생
        if (staySimulate) AnimationUpdate(ref stayTime, stayTimeScale);
        if (walkSimulate) AnimationUpdate(ref walkTime, walkTimeScale);
        
        //그리기
        Animation();
        OtherAnimation();

        //플레이 모드
        if (EditorApplication.isPlaying)
        {
            StateUpdate();
            Hand();
        }
        //에디터 모드
        else
        {
            //선택 시 즉시 로드
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
                staySimulate = false;
                walkSimulate = false;
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
    protected abstract void StateSetToNONE();
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
    [HorizontalGroup("Hand/Horizontal")]
    [Button]
    public void HandLink(HandMode mode, Transform target)
    {
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
                OtherAnimation();
                break;
            case AnimationState.STAY:
                //Body
                if (stayFrame == null) stayFrame = Utility.GetSpriteArray2DFromSpriteSheet(texture, stayFrameTextureAnchor, stayFrameTextureSize, stayFrameSpriteSize);

                body.sprite = stayFrame[Utility.FloorRotateToInt(con.moveRotate, 8), Mathf.FloorToInt(stayTime * stayFrameTextureSize.y)];

                //Hand
                if (handMode == HandMode.ToHand)
                {
                    hand.localPosition = new Vector3(con.lastMoveVector.normalized.x * 0.75f, con.lastMoveVector.normalized.y * 0.5f, 0);
                    hand.localRotation = Quaternion.identity;
                }
                
                break;
            case AnimationState.WALK:
                //Body
                if (walkFrame == null) walkFrame = Utility.GetSpriteArray2DFromSpriteSheet(texture, walkFrameTextureAnchor, walkFrameTextureSize, walkFrameSpriteSize);

                body.sprite = walkFrame[Utility.FloorRotateToInt(con.moveRotate, 8), Mathf.FloorToInt(walkTime * walkFrameTextureSize.y)];

                //Hand
                if (handMode == HandMode.ToHand)
                {
                    hand.localPosition = new Vector3(con.moveVector.normalized.x * 0.75f, con.moveVector.normalized.y * 0.5f, 0);
                    hand.localRotation = Quaternion.identity;
                }

                break;
        }
    }

    protected abstract void OtherAnimation();
    void AnimationUpdate(ref float time, float timeScale)
    {
        time += Time.deltaTime / timeScale;
        if (time >= 1) time = 0;
    }
}
