using Sirenix.OdinInspector;
using UnityEngine;
using Sirenix.OdinInspector.Editor;
using UnityEngine.UIElements;
using Sirenix.Utilities.Editor;
using UnityEditor;
using static UnityEngine.Rendering.DebugUI.Table;

public enum AnimationState
{
    STAY, WALK, BATTLE
}
[RequireComponent(typeof(Player))]
public class PlayerGrafic : MonoBehaviour
{
    [SerializeField] PlayerController pCon;

    [SerializeField] SpriteRenderer body;

    [SerializeField] Texture2D pTexture;
    [SerializeField] AnimationState state = AnimationState.STAY;

    Sprite[,] stayFrame;
    [SerializeField] int stayIndex = 0;
    int maxStayAnimateIndex = 2;
    [SerializeField] float stayTimeScale = 0.5f;
    float stayTime = 0;

    Sprite[,] walkFrame;
    [SerializeField] int walkIndex = 0;
    int maxWalkAnimateIndex = 4;
    [SerializeField] float walkTimeScale = 0.5f;
    float walkTime = 0;

    bool isBattle;
    
    void Awake()
    {
        stayFrame = SpriteSheet.GetSpriteArray2DFromSpriteSheet(pTexture, new Vector2Int(0, 0), new Vector2Int(1, 7), 16, 16);
        walkFrame = SpriteSheet.GetSpriteArray2DFromSpriteSheet(pTexture, new Vector2Int(3, 0), new Vector2Int(6, 7), 16, 16);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Animation();
    }
    void Animation()
    {
        //상태 지정
        if (pCon.curMoveSpeed <= 0.1f) state = AnimationState.STAY;
        else
        {
            if (isBattle) state = AnimationState.BATTLE;
            else state = AnimationState.WALK;
        }

        //상태에 따른 애니메이션
        switch (state)
        {
            case AnimationState.STAY:
                body.sprite = stayFrame[stayIndex, pCon.moveIntRotate];
                break;
            case AnimationState.WALK:
                body.sprite = walkFrame[walkIndex, pCon.moveIntRotate];
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