using Sirenix.OdinInspector;
using System;
using UnityEngine;

[Serializable]
public class Animation
{
    [VerticalGroup("Animation")]
    #region Vertical Animation  - - - - - - - - - - - - - - - - - - - - - - - - - - - -|                          
                                                                                        
        #if UNITY_EDITOR      
    
        [SerializeField]
        [HideLabel]
        public string name;

        #endif
                                                                                        [VerticalGroup("Animation")]
        [ShowInInspector]                                             
        [TableMatrix(IsReadOnly = true, SquareCells = true
            , HorizontalTitle = "Rotation", VerticalTitle = "Frame")]     
        Sprite[,] animationSprites;


        [VerticalGroup("Animation/Texture")][ShowIf(nameof(isAnimationSpritesNull))]//-|
        #region Vertical Texture  - - - - - - - - - - - - - - - - - - - - - - - - -|                                                          
            
            [LabelText("Start Sprite")]                                                 
            public Vector2Int textureAnchor = Vector2Int.zero;                  
                                                                                    [VerticalGroup("Animation/Texture")][ShowIf(nameof(isAnimationSpritesNull))]                  
            [LabelText("Array Length")]                           
            public Vector2Int textureSize = Vector2Int.zero;                        
                                                                                    [VerticalGroup("Animation/Texture")][ShowIf(nameof(isAnimationSpritesNull))]                  
            [LabelText("Sprite Size ")]                           
            public Vector2Int spriteSize = Vector2Int.zero;

        #endregion  - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -|


        [HorizontalGroup("Animation/Time")]
        #region Horizontal Time - - - - - - -|                            
                                            
            [Range(0, 0.9999f)]                       
            public float time = 0;                      
                                              [HorizontalGroup("Animation/Time", width: 30)]          
            [HideLabel]                       
            public float timeScale = 0.5f;//-|

        #endregion  - - - - - - - - - - - - -|

    #endregion  - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -|



    public void LoadTexture(ref Texture2D _texture)
    {
        animationSprites = Utility.GetSpriteArray2DFromSpriteSheet(_texture, textureAnchor, textureSize, spriteSize);
    }
    public Sprite GetSprite(ref Texture2D texture, int rotation)
    {
        if(animationSprites == null) LoadTexture(ref texture);

        return animationSprites[rotation, Mathf.FloorToInt(time * textureSize.y)];
    }
    public void AnimationUpdate()
    {
        time += Time.deltaTime / timeScale;
        if (time >= 1) time = 0;
    }

#if UNITY_EDITOR

    public bool isTextureInfoNull { 
        get { 
            return (
            textureSize.x == 0 || textureSize.y == 0 
            || spriteSize.x == 0 || spriteSize.y == 0
            ); 
        } 
    }            
    public bool isAnimationSpritesNull {
        get { return animationSprites == null; } 
    }
    
#endif
}
public abstract class Grafic : MonoBehaviour
{
    [SerializeField] 
    public HandGrafic hand;

    [SerializeField] 
    protected Controller con;

    [SerializeField] 
    protected Texture2D texture;

    [SerializeField]
    [ChildGameObjectsOnly] protected SpriteRenderer body;

    [PropertyOrder(100)]
    [TableList(DrawScrollView = false)] public Animation[] animations;

#if UNITY_EDITOR
    
    bool isAnimationsNull 
    { get { return( 
        animations == null
        || animations.Length == 0
        ); } }

    [Button][HideIf(nameof(isAnimationsNull))][PropertyOrder(99)]
    void LoadAllTexture()
    {
        foreach (Animation animation in animations)
        {
            if (animation.isAnimationSpritesNull)
            {
                if (animation.isTextureInfoNull)
                {
                    Debug.LogWarning(animation.name + "의 텍스쳐 정보가 잘못되었습니다.");
                    continue;
                }
                animation.LoadTexture(ref texture);
            }
        }
        if (animations.Length > 0) body.sprite = animations[0].GetSprite(ref texture, 0);
    }

#endif
}
