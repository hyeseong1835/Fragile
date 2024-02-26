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
        stayFrame = GetSpriteArray2DFromSpriteSheet(pTexture, new Vector2Int(0, 0), new Vector2Int(1, 7), 16, 16);
        walkFrame = GetSpriteArray2DFromSpriteSheet(pTexture, new Vector2Int(3, 0), new Vector2Int(6, 7), 16, 16);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
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

        stayTime += Time.deltaTime / stayTimeScale;
        if (stayTime >= 1)
        {
            stayTime = 0;
            if (++stayIndex >= maxStayAnimateIndex) stayIndex = 0;
        }
        walkTime += Time.deltaTime / walkTimeScale;
        if (walkTime >= 1)
        {
            walkTime = 0;
            if (++walkIndex >= maxWalkAnimateIndex) walkIndex = 0;
        }
    }
    Sprite[,] GetSpriteArray2DFromSpriteSheet(Texture2D spriteSheet, Vector2Int startPos, Vector2Int endPos, int spritePixelWidth, int spritePixelHeight)
    {
        Sprite[,] frames = new Sprite[endPos.x - startPos.x + 1, endPos.y - startPos.y + 1];
        for (int x = startPos.x; x <= endPos.x; x++)
        {
            for (int y = startPos.y; y <= endPos.y; y++)
            {
                Texture2D tex = new Texture2D(spritePixelWidth, spritePixelHeight);
                tex.SetPixels(spriteSheet.GetPixels(x * spritePixelWidth, y * spritePixelHeight, spritePixelWidth, spritePixelHeight));
                tex.filterMode = FilterMode.Point;  
                tex.Apply();

                Debug.Log("( " + (x - startPos.x) + ", " + (y - startPos.y) + " )");
                frames[x - startPos.x, y - startPos.y] = Sprite.Create(tex, new Rect(0, 0, spritePixelWidth, spritePixelHeight), Vector2.one * 0.5f);
            }
        }
        return frames;
    }
}