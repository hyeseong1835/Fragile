using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;
using UnityEngine.UIElements;

public enum AnimationState
{
    STAY, WALK, BATTLE
}
public class PlayerGrafic : MonoBehaviour
{
    [SerializeField] PlayerController pCon;

    const int spriteSheetRow = 8, spriteSheetColumn = 8;
    const int spritePixelWidth = 16, spritePixelHeight = 16;
    [SerializeField] Texture2D pTexture;
    [SerializeField] Sprite[,] spriteSheet = new Sprite[spriteSheetRow, spriteSheetColumn];
    [SerializeField] Transform grafic;
    [SerializeField] SpriteRenderer top;
    [SerializeField] SpriteRenderer bottom;

    [SerializeField] AnimationState state = AnimationState.STAY;

    [SerializeField] int stayIndex = 0;
    int maxStayAnimateIndex = 2;
    [SerializeField] float stayTimeScale = 0.5f;
    float stayTime = 0;

    [SerializeField] int walkIndex = 0;
    int maxWalkAnimateIndex = 4;
    [SerializeField]float walkTimeScale = 0.5f;
    float walkTime = 0;


    public bool isBattle;

    void Awake()
    {
        for (int row = 0; row < spriteSheetRow; row++)
        {
            for (int col = 0; col < spriteSheetColumn; col++)
            {
                // 스프라이트 시트에서 해당하는 영역을 자르기
                Texture2D tex = new Texture2D(spritePixelWidth, spritePixelHeight);
                tex.SetPixels(pTexture.GetPixels(row * spritePixelWidth, (spriteSheetRow - 1 - col) * spritePixelHeight, spritePixelWidth, spritePixelHeight, 0));
                tex.filterMode = FilterMode.Point;
                tex.Apply();

                // Texture2D를 Sprite로 변환하여 배열에 저장
                spriteSheet[row, col] = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.one * 0.5f);
            }
        }
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

        //방향 지정
        if (pCon.moveVector.x > 0) grafic.rotation = Quaternion.Euler(0, 0, 0);
        else if(pCon.moveVector.x < 0) grafic.rotation = Quaternion.Euler(0, 180, 0);

        //상태에 따른 애니메이션
        switch (state)
        {
            case AnimationState.STAY:
                switch (stayIndex)
                {
                    case 0:
                        if (pCon.moveIntRotate == 6) top.sprite = spriteSheet[0, 0];
                        else if (pCon.moveIntRotate == 2) top.sprite = spriteSheet[0, 1];
                        else if (pCon.moveIntRotate == 5 || pCon.moveIntRotate == 7) top.sprite = spriteSheet[0, 2];
                        else if (pCon.moveIntRotate == 1 || pCon.moveIntRotate == 3) top.sprite = spriteSheet[0, 3];
                        else top.sprite = spriteSheet[0, 4];

                        if (pCon.moveIntRotate == 2 || pCon.moveIntRotate == 6) bottom.sprite = spriteSheet[0, 6];
                        else bottom.sprite = spriteSheet[0, 7];

                        break;
                    case 1:
                        if (pCon.moveIntRotate == 6) top.sprite = spriteSheet[1, 0];
                        else if (pCon.moveIntRotate == 2) top.sprite = spriteSheet[1, 1];
                        else if (pCon.moveIntRotate == 5 || pCon.moveIntRotate == 7) top.sprite = spriteSheet[1, 2];
                        else if (pCon.moveIntRotate == 1 || pCon.moveIntRotate == 3) top.sprite = spriteSheet[1, 3];
                        else top.sprite = spriteSheet[1, 4];

                        if (pCon.moveIntRotate == 2 || pCon.moveIntRotate == 6) bottom.sprite = spriteSheet[0, 6];
                        else bottom.sprite = spriteSheet[0, 7];

                        break;
                }
                break;
            case AnimationState.WALK:
            switch (walkIndex)
            {
                    case 0:
                        if (pCon.moveIntRotate == 6) top.sprite = spriteSheet[2, 0];
                        else if (pCon.moveIntRotate == 2) top.sprite = spriteSheet[2, 1];
                        else if (pCon.moveIntRotate == 5 || pCon.moveIntRotate == 7) top.sprite = spriteSheet[2, 2];
                        else if (pCon.moveIntRotate == 1 || pCon.moveIntRotate == 3) top.sprite = spriteSheet[2, 3];
                        else top.sprite = spriteSheet[2, 4];

                        if (pCon.moveIntRotate == 2 || pCon.moveIntRotate == 6) bottom.sprite = spriteSheet[2, 6];
                        else bottom.sprite = spriteSheet[2, 7];

                        break;
                    case 1:
                        if (pCon.moveIntRotate == 6) top.sprite = spriteSheet[3, 0];
                        else if (pCon.moveIntRotate == 2) top.sprite = spriteSheet[3, 1];
                        else if (pCon.moveIntRotate == 5 || pCon.moveIntRotate == 7) top.sprite = spriteSheet[3, 2];
                        else if (pCon.moveIntRotate == 1 || pCon.moveIntRotate == 3) top.sprite = spriteSheet[3, 3];
                        else top.sprite = spriteSheet[3, 4];

                        if (pCon.moveIntRotate == 2 || pCon.moveIntRotate == 6) bottom.sprite = spriteSheet[3, 6];
                        else bottom.sprite = spriteSheet[3, 7];

                        break;
                    case 2:
                        if (pCon.moveIntRotate == 6) top.sprite = spriteSheet[4, 0];
                        else if (pCon.moveIntRotate == 2) top.sprite = spriteSheet[4, 1];
                        else if (pCon.moveIntRotate == 5 || pCon.moveIntRotate == 7) top.sprite = spriteSheet[4, 2];
                        else if (pCon.moveIntRotate == 1 || pCon.moveIntRotate == 3) top.sprite = spriteSheet[4, 3];
                        else top.sprite = spriteSheet[4, 4];

                        if (pCon.moveIntRotate == 2 || pCon.moveIntRotate == 6) bottom.sprite = spriteSheet[4, 6];
                        else bottom.sprite = spriteSheet[4, 7];

                        break;
                    case 3:
                        if (pCon.moveIntRotate == 6) top.sprite = spriteSheet[5, 0];
                        else if (pCon.moveIntRotate == 2) top.sprite = spriteSheet[5, 1];
                        else if (pCon.moveIntRotate == 5 || pCon.moveIntRotate == 7) top.sprite = spriteSheet[5, 2];
                        else if (pCon.moveIntRotate == 1 || pCon.moveIntRotate == 3) top.sprite = spriteSheet[5, 3];
                        else top.sprite = spriteSheet[5, 4];

                        if (pCon.moveIntRotate == 2 || pCon.moveIntRotate == 6) bottom.sprite = spriteSheet[5, 6];
                        else bottom.sprite = spriteSheet[5, 7];

                        break;

                }
                break;
        }
        stayTime += Time.deltaTime / stayTimeScale;
        if (stayTime >= 1)
        {
            stayTime = 0;
            if (++stayIndex >= maxStayAnimateIndex) stayIndex = 0;
        }
        walkTime += Time.deltaTime / walkTimeScale;
        if(walkTime >= 1)
        {
            walkTime = 0;
            if(++walkIndex >= maxWalkAnimateIndex) walkIndex = 0;
        }
    }
}
