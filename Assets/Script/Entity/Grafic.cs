using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public enum AnimationState
{
    NONE, STAY, WALK, BATTLE
}
[ExecuteAlways]
public class Grafic : MonoBehaviour
{
#if UNITY_EDITOR
    [SerializeField] protected bool debug = false;
#endif

    [SerializeField] protected Controller con;
    [SerializeField] protected Texture2D texture;

    [SerializeField][ChildGameObjectsOnly] protected SpriteRenderer body;

    [Title("Hand")]
    public bool IK;
    public bool stateHandAnimation = true;

    [SerializeField][ChildGameObjectsOnly] protected Transform hand;
    [ShowIf("debug")] public Transform targetTransform;

    [FoldoutGroup("Stay")]
    #region Stay

        #region Texture
        [BoxGroup("Stay/Texture")][ShowIf("debug")][ShowInInspector]
        [TableMatrix(IsReadOnly = true, SquareCells = true, HorizontalTitle = "Rotation", VerticalTitle = "Frame")]
        Sprite[,] stayFrame;
        [BoxGroup("Stay/Texture")] [LabelText("Start Sprite")][SerializeField] Vector2Int stayFrameTextureAnchor;
        [BoxGroup("Stay/Texture")] [LabelText("Array Length")][SerializeField] Vector2Int stayFrameTextureSize;
        [BoxGroup("Stay/Texture")] [LabelText("Sprite Size ")][SerializeField] int stayFrameSpriteSize = 16;
        #endregion

        #region Time
        [BoxGroup("Stay/Time")] [ShowIf("debug")][ShowInInspector] protected bool staySimulate
            {
                get { return _staySimulate; }
                set
                {
                    if (value == true) stayTime = Random.Range(0f, 0.9999f);

                    _staySimulate = value;
                }
            }protected bool _staySimulate;
        [BoxGroup("Stay/Time")] [SerializeField] float stayTimeScale = 0.5f;
        [BoxGroup("Stay/Time")] [ShowIf("debug")][SerializeField]
                                [Range(0, 0.9999f)] protected float stayTime;
        #endregion

    #endregion

    [FoldoutGroup("Walk")]
    #region Walk

        #region Texture
        [BoxGroup("Walk/Texture")] [ShowIf("debug")][ShowInInspector]
        [TableMatrix(IsReadOnly = true, SquareCells = true, HorizontalTitle = "Rotation", VerticalTitle = "Frame")]
        Sprite[,] walkFrame;
        [BoxGroup("Walk/Texture")] [LabelText("Start Sprite")][SerializeField] Vector2Int walkFrameTextureAnchor;
        [BoxGroup("Walk/Texture")] [LabelText("Array Length")][SerializeField] Vector2Int walkFrameTextureSize;
        [BoxGroup("Walk/Texture")] [LabelText("Sprite Size ")][SerializeField] Vector2Int walkFrameSpriteSize;
        #endregion

        #region Time
        [BoxGroup("Walk/Time")] [ShowIf("debug")][ShowInInspector]
                                protected bool walkSimulate
                                {
                                    get { return _walkSimulate; }
                                    set
                                    {
                                        if (value == true) stayTime = Random.Range(0f, 0.9999f);

                                        _walkSimulate = value;
                                    }
                                }protected bool _walkSimulate;
        [BoxGroup("Walk/Time")] [SerializeField] float walkTimeScale = 0.1f;
        [BoxGroup("Walk/Time")] [ShowIf("debug")][SerializeField]
                                [Range(0, 0.9999f)] protected float walkTime = 0;
        #endregion

    #endregion

    [Title("State")]
    [ShowInInspector] public AnimationState state;

    [SerializeField][ShowIf("debug")] bool useOtherAnimation = false;

    void Awake()
    {
        if (this is OtherGrafic)
        {
            useOtherAnimation = true;
            ((OtherGrafic)this).OtherAnimation();
        }
    }
    void Start()
    {
        staySimulate = true;
        walkSimulate = true;
    }
    void Update()
    {
        if (staySimulate) AnimationUpdate(ref stayTime, stayTimeScale);
        if (walkSimulate) AnimationUpdate(ref walkTime, walkTimeScale);

        Animation();

        if (EditorApplication.isPlaying)
        {
            StateUpdate();
            Hand();
        }
        else
        {
            if (stayFrame == null)
            {
                stayFrame = Utility.GetSpriteArray2DFromSpriteSheet(texture, stayFrameTextureAnchor, stayFrameTextureSize, stayFrameSpriteSize);
                body.sprite = stayFrame[0, 0];
            }
            if (debug == false)
            {
                staySimulate = false;
                walkSimulate = false;
            }
        }
    }
    public virtual void StateUpdate()
    {
        //Stay => Walk
        if (state == AnimationState.STAY &&
            con.moveVector.magnitude < Mathf.Epsilon) state = AnimationState.WALK;

        //Walk => Stay
        if (state == AnimationState.WALK &&
            con.moveVector.magnitude < Mathf.Epsilon) state = AnimationState.STAY;
    }
    /// <summary>
    /// 손을 연결함
    /// </summary>
    /// <param name="target"></param>
    /// <param name="_IK">제어권 [true: 무기, false: 손]</param>
    public void HandLink(Transform target, bool _IK = false)
    {
        if (target != null && target != targetTransform)
        {
            IK = _IK;
            targetTransform = target;
        }
        else
        {
            IK = _IK;
            targetTransform = null;
        }
        stateHandAnimation = !_IK;
    }
    void Hand()
    {
        if (targetTransform == null || targetTransform == null) return;

        if (IK)
        {
            hand.transform.position = targetTransform.position;
            hand.transform.rotation = targetTransform.rotation;
        }
        else
        {
            targetTransform.position = hand.transform.position;
            targetTransform.rotation = hand.transform.rotation;
        }
    }
    void Animation()
    {
        //상태에 따른 애니메이션
        switch (state)
        {
            case AnimationState.NONE:
                if (useOtherAnimation) ((OtherGrafic)this).OtherAnimation();
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
                if (stateHandAnimation)
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
