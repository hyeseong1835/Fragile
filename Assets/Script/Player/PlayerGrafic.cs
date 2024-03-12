using Sirenix.OdinInspector;
using UnityEngine;

public enum AnimationState
{
    STAY, WALK, BATTLE
}
public class PlayerGrafic : MonoBehaviour
{
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

    [ChildGameObjectsOnly] public Transform hand;
    public Transform targetTransform;



    [Title("Stay")]
    [SerializeField] int stayIndex = 0;
    [SerializeField] float stayTimeScale = 0.5f;
    [ReadOnly][ShowInInspector]
    [TableMatrix()] Sprite[,] stayFrame;
    /*
    [ShowInInspector] public Vector3[,] stayRHand = { 
        { new Vector3(0, -0.5f, 0), new Vector3(0.25f, -0.25f, 0), new Vector3(0.5f, 0, 0), new Vector3(0.25f, 0.25f, 0), new Vector3(0, 0.5f, 0), new Vector3(-0.25f, -0.25f, 0), new Vector3(-0.5f, 0, 0), new Vector3(-0.25f, 0.25f, 0), }, 
        { new Vector3(0, -0.5f, 0), new Vector3(0.25f, -0.25f, 0), new Vector3(0.5f, 0, 0), new Vector3(0.25f, 0.25f, 0), new Vector3(0, 0.5f, 0), new Vector3(-0.25f, -0.25f, 0), new Vector3(-0.5f, 0, 0), new Vector3(-0.25f, 0.25f, 0) } 
    };
    */
    const int maxStayAnimateIndex = 2;
    float stayTime = 0;



    [Title("Walk")]
    [SerializeField] int walkIndex = 0;
    [SerializeField] float walkTimeScale = 0.5f;
    [ReadOnly][ShowInInspector]
    [TableMatrix(SquareCells = true)] Sprite[,] walkFrame;
    [ShowInInspector] public Vector3[,] walkRHand;


    const int maxWalkAnimateIndex = 4;
    float walkTime = 0;


    void Awake()
    {
        Player.grafic = this;

        stayFrame = Utility.GetSpriteArray2DFromSpriteSheet(pTexture, new Vector2Int(0, 0), new Vector2Int(1, 7), 16, 16);
        walkFrame = Utility.GetSpriteArray2DFromSpriteSheet(pTexture, new Vector2Int(3, 0), new Vector2Int(6, 7), 16, 16);
    }
    void Update()
    {
        Hand();
        Animation();
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
        AnimationUpdate(ref stayIndex, ref stayTime, stayTimeScale, maxStayAnimateIndex);
        AnimationUpdate(ref walkIndex, ref walkTime, walkTimeScale, maxWalkAnimateIndex);

        //상태 지정
        if (Player.pCon.moveVector.magnitude <= Mathf.Epsilon) state = AnimationState.STAY;
        else
        {
            if (isBattle) state = AnimationState.BATTLE;
            else state = AnimationState.WALK;
        }

        //상태에 따른 애니메이션
        switch (state)
        {
            case AnimationState.STAY:
                //Body
                body.sprite = stayFrame[stayIndex, Player.pCon.moveIntRotate];

                break;
            case AnimationState.WALK:
                //Body
                body.sprite = walkFrame[walkIndex, Player.pCon.moveIntRotate];

                //Hand
                
                if (stateHandAnimation)
                {
                    if (Player.pCon.prevMoveVector.magnitude <= Player.pCon.moveVector.magnitude || Player.pCon.moveVector.magnitude >= 1)
                        hand.localPosition = new Vector3(Player.pCon.moveVector.normalized.x * 0.75f, Player.pCon.moveVector.normalized.y * 0.5f, 0)
                            + (new Vector3(Player.pCon.moveVector.normalized.y, Player.pCon.moveVector.normalized.x * -0.5f) * 0.25f);
                    hand.localRotation = Quaternion.identity;
                }

                break;
        }
    }
    void AnimationUpdate(ref int index, ref float time, float timeScale, int maxAnimateIndex)
    {
        time += Time.deltaTime / timeScale;
        if (time >= 1)
        {
            time = 0;
            if (++index >= maxAnimateIndex) index = 0;
        }
    }
}