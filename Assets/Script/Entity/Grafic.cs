using Sirenix.OdinInspector;
using System;
using UnityEditor;
using UnityEngine;

[Serializable]
public class Animation
{
    public Animation(Vector2Int _textureAnchor, Vector2Int _textureSize, Vector2Int _spriteSize)
    {
        textureAnchor = _textureAnchor;
        textureSize = _textureSize;
        spriteSize = _spriteSize;
    }

    [VerticalGroup("Animation")]
    #region Vertical Animation
    
        #if UNITY_EDITOR
        [HorizontalGroup("Animation/Header")]
        #region Vertical Header
    
            [SerializeField][HideLabel] 
            string name;

            [HideLabel]    [HorizontalGroup("Animation/Header", width: 10, marginRight: 7, marginLeft: 2)]
            public bool autoLoad = false;
    
        #endregion
        #endif

        [TableMatrix(IsReadOnly = true, SquareCells = true, HorizontalTitle = "Rotation", VerticalTitle = "Frame")]     [VerticalGroup("Animation")]
        [ShowInInspector] Sprite[,] animationSprites = null;

        [VerticalGroup("Animation/Texture")][ShowIf(nameof(isAnimationSpritesNull))]
        #region Vertical Texture

            [LabelText("Start Sprite")][SerializeField]  
            public Vector2Int textureAnchor = Vector2Int.zero;
                                                                            
            [LabelText("Array Length")][SerializeField]             [VerticalGroup("Animation/Texture")][ShowIf(nameof(isAnimationSpritesNull))]
            Vector2Int textureSize = Vector2Int.zero;
            
            [LabelText("Sprite Size ")][SerializeField]             [VerticalGroup("Animation/Texture")][ShowIf(nameof(isAnimationSpritesNull))]
            public Vector2Int spriteSize = Vector2Int.zero;

            public bool isTextureInfoNull { get { return (
                textureSize.x == 0 || textureSize.y == 0 
                || spriteSize.x == 0 || spriteSize.y == 0); } }
            #if UNITY_EDITOR
            public bool isAnimationSpritesNull { get { return animationSprites == null; } }
            #endif
        
        #endregion

        [VerticalGroup("Animation/Time")]
        #region Vertical Time

            [HorizontalGroup("Animation/Time/Horizontal")]
            #region Horizontal

                [Range(0, 0.9999f)]
                public float time = 0;

                [ShowInInspector][HideLabel]    [HorizontalGroup("Animation/Time/Horizontal", width: 10, marginRight: 5, marginLeft: 2)]
                public bool simulate
                {
                    get { return _simulate; }
                    set
                    {
                        if (value == true) time = UnityEngine.Random.Range(0f, 0.9999f);

                        _simulate = value;
                    }
                } bool _simulate = false;

            #endregion

            [LabelText("Scale")]            [VerticalGroup("Animation/Time")][SerializeField]
            public float timeScale = 0.5f;

        #endregion

    #endregion


    public void LoadTexture(ref Texture2D _texture)
    {
        animationSprites = Utility.GetSpriteArray2DFromSpriteSheet(_texture, textureAnchor, textureSize, spriteSize);
    }
    public Sprite GetSprite(ref Texture2D texture, int rotation)
    {
        if(animationSprites == null) LoadTexture(ref texture);

        return animationSprites[rotation, Mathf.FloorToInt(time * textureSize.y)];
    }
    public void AnimationUpdate(float timeScale)
    {
        time += Time.deltaTime / timeScale;
        if (time >= 1) time = 0;
    }
}
[ExecuteAlways]
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

    [ShowInInspector][PropertyOrder(100)]
    [TableList(DrawScrollView = false)] protected Animation[] animations;

#if UNITY_EDITOR
    void Update()
    {
        if (Selection.Contains(gameObject))
        {
            foreach (Animation animation in animations)
            {
                if (animation.autoLoad && animation.isAnimationSpritesNull)
                {
                    if (animation.isTextureInfoNull)
                    {
                        animation.autoLoad = false;
                        continue;
                    }
                    animation.LoadTexture(ref texture);
                }
            }
        }
    }
#endif
}
