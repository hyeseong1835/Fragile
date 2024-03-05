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

    [Title("Stay")]
    [SerializeField] int stayIndex = 0;
    [SerializeField] float stayTimeScale = 0.5f;
    [ReadOnly][ShowInInspector]
    [TableMatrix(SquareCells = true)] Sprite[,] stayFrame;
    const int maxStayAnimateIndex = 2;
    float stayTime = 0;

    [Title("Walk")]
    [SerializeField] int walkIndex = 0;
    [SerializeField] float walkTimeScale = 0.5f;
    [ReadOnly][ShowInInspector]
    [TableMatrix(SquareCells = true)] Sprite[,] walkFrame;
    const int maxWalkAnimateIndex = 4;
    float walkTime = 0;


    void Awake()
    {
        Player.grafic = this;

        stayFrame = SpriteSheet.GetSpriteArray2DFromSpriteSheet(pTexture, new Vector2Int(0, 0), new Vector2Int(1, 7), 16, 16);
        walkFrame = SpriteSheet.GetSpriteArray2DFromSpriteSheet(pTexture, new Vector2Int(3, 0), new Vector2Int(6, 7), 16, 16);
    }
    void Update()
    {
        Animation();
    }
    void Animation()
    {
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
                body.sprite = stayFrame[stayIndex, Player.pCon.moveIntRotate];
                break;
            case AnimationState.WALK:
                body.sprite = walkFrame[walkIndex, Player.pCon.moveIntRotate];
                break;
        }
        AnimationUpdate(ref stayIndex, ref stayTime, stayTimeScale, maxStayAnimateIndex);
        AnimationUpdate(ref walkIndex, ref walkTime, walkTimeScale, maxWalkAnimateIndex);
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