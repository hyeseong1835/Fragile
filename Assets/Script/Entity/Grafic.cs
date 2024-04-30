using Sirenix.OdinInspector;
using System.Security.Cryptography;
#if UNITY_EDITOR
using UnityEditor;
#endif
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
    public HandGrafic hand;

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
        hand.Hand();

        //플레이 모드
        if (Utility.GetEditorStateByType(Utility.StateType.ISPLAY))
        {
            StateUpdate();
        }
        //에디터 모드
        else
        {
#if UNITY_EDITOR
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
#endif
        }
    }
    public virtual void StateUpdate()
    {
        //Stay => Walk
        if (animationState == AnimationState.STAY &&
            con.moveVector != Vector2.zero) animationState = AnimationState.WALK;

        //Walk => Stay
        if (animationState == AnimationState.WALK &&
            con.moveVector == Vector2.zero) animationState = AnimationState.STAY;
    }
    protected abstract void StateSetToNONE();
    
    void Animation()
    {
        //상태에 따른 애니메이션
        switch (animationState)
        {
            case AnimationState.NONE:
                OtherAnimation();
                break;
            case AnimationState.STAY:
                if (staySimulate == false) break;
                
                //Body
                if (stayFrame == null) stayFrame = Utility.GetSpriteArray2DFromSpriteSheet(texture, stayFrameTextureAnchor, stayFrameTextureSize, stayFrameSpriteSize);

                body.sprite = stayFrame[Utility.FloorRotateToInt(con.lastMoveRotate, 8), Mathf.FloorToInt(stayTime * stayFrameTextureSize.y)];

                //Hand
                if (hand.handMode == HandMode.NONE || hand.handMode == HandMode.ToHand)
                {
                    hand.transform.localPosition = Utility.Vector2TransformToEllipse(con.lastMoveVector.normalized, 0.75f, 0.5f) + con.center;
                    hand.transform.localRotation = Quaternion.identity;
                }
                
                break;
            case AnimationState.WALK:
                if (walkSimulate == false) break;

                //Body
                if (walkFrame == null) walkFrame = Utility.GetSpriteArray2DFromSpriteSheet(texture, walkFrameTextureAnchor, walkFrameTextureSize, walkFrameSpriteSize);

                body.sprite = walkFrame[Utility.FloorRotateToInt(con.moveRotate, 8), Mathf.FloorToInt(walkTime * walkFrameTextureSize.y)];

                //Hand
                if (hand.handMode == HandMode.NONE || hand.handMode == HandMode.ToHand)
                {
                    hand.transform.localPosition = Utility.Vector2TransformToEllipse(con.moveVector.normalized, 0.75f, 0.5f) + con.center;
                    hand.transform.localRotation = Quaternion.identity;
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
