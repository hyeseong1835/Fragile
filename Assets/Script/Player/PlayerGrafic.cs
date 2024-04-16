using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

public enum AnimationState
{
    STAY, WALK, BATTLE
}
[ExecuteInEditMode]
public class PlayerGrafic : MonoBehaviour
{
    [SerializeField] Controller con;

    [SerializeField]
    [ChildGameObjectsOnly] SpriteRenderer body;
    [SerializeField]
    [AssetsOnly] Texture2D pTexture;

    [Title("State")]
    [SerializeField] AnimationState state = AnimationState.STAY;
    public bool isBattle;

    [Title("Hand")]
    public bool handLink;
    public bool IK;
    public bool stateHandAnimation = true;

    [SerializeField][ChildGameObjectsOnly] Transform hand;
    public Transform targetTransform;

    [Title("Stay")]
    [ShowInInspector][TableMatrix(IsReadOnly = true, SquareCells = true, HorizontalTitle = "Rotation", VerticalTitle = "Frame")] Sprite[,] stayFrame;
    [SerializeField][Range(0, 0.9999f)] float stayTime = 0;
    [SerializeField] int stayFrameLength = 2;
    [SerializeField] float stayTimeScale = 0.5f;

    [Title("Walk")]
    [ShowInInspector][TableMatrix(IsReadOnly = true, SquareCells = true, HorizontalTitle = "Rotation", VerticalTitle = "Frame")] Sprite[,] walkFrame;
    [SerializeField][Range(0, 0.9999f)] float walkTime = 0;
    [SerializeField] float walkFrameLength = 8;
    [SerializeField] float walkTimeScale = 0.5f;

    void Awake()
    {
        Player.grafic = this;

        //con = transform.parent.GetComponent<Controller>();
    }
    void Start()
    {
        if (stayFrame == null) stayFrame = Utility.GetSpriteArray2DFromSpriteSheet(pTexture, new Vector2Int(0, 0), new Vector2Int(1, 7), 16, 16);
        body.sprite = stayFrame[0, 0];
    }
    void Update()
    {
        Animation();

        if (EditorApplication.isPlaying == false) return;

        //플레이 모드에서만 작동
        
        //상태 지정
        if (con.moveVector.magnitude <= Mathf.Epsilon) state = AnimationState.STAY;
        else
        {
            if (isBattle) state = AnimationState.BATTLE;
            else state = AnimationState.WALK;
        }
        Hand();
        
        AnimationUpdate(ref stayTime, stayTimeScale);
        AnimationUpdate(ref walkTime, walkTimeScale);
    }
    public void HandLink(Transform target, bool _IK = false)
    {
        if (target != null && target != targetTransform)
        {
            handLink = true;
            IK = _IK;
            targetTransform = target;
        }
        else
        {
            handLink = false;
            IK = _IK;
            targetTransform = null;
        }
        stateHandAnimation = !_IK;
    }
    void Hand()
    {
        if (!handLink || targetTransform == null) return;

        if(IK)
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
            case AnimationState.STAY:
                //Body
                if (stayFrame == null) stayFrame = Utility.GetSpriteArray2DFromSpriteSheet(pTexture, new Vector2Int(0, 0), new Vector2Int(7, 1), 16, 16);

                body.sprite = stayFrame[Utility.FloorRotateToInt(con.moveRotate, 8), Mathf.FloorToInt(stayTime * stayFrameLength)];

                break;
            case AnimationState.WALK:
                //Body
                if (walkFrame == null) walkFrame = Utility.GetSpriteArray2DFromSpriteSheet(pTexture, new Vector2Int(0, 2), new Vector2Int(7, 5), 16, 16);

                body.sprite = walkFrame[Utility.FloorRotateToInt(con.moveRotate, 8), Mathf.FloorToInt(walkTime * walkFrameLength)];

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
        if (time >= 1)
        {
            time = 0;
        }
    }
}