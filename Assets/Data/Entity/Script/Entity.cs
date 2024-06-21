using UnityEngine;
using System;
using Sirenix.OdinInspector;
using UnityEngine.UIElements;
using System.Reflection;

public enum EntityLayer
{
    Player, Enemy, Obstacle
}
public enum EntityLayerInteraction
{
    None, Friend, Hostile, Neutral
}
public abstract class Entity : MonoBehaviour 
{
    public abstract EntityData EntityData { get; set; }
    public abstract Type DataType { get; }

    public EntityLayer entityLayer;

    [FoldoutGroup("Stat")]
    #region Foldout Stat - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -|                                         

        [HorizontalGroup("Stat/HP")]
        #region Horizontal HP

            [LabelWidth(Editor.propertyLabelWidth)]
            #if UNITY_EDITOR
            [ProgressBar(0, nameof(_maxHp), ColorGetter = nameof(_hpColor))]
            #endif
            public float hp = 1;

            #if UNITY_EDITOR
                                                                                 [HorizontalGroup("Stat/HP", Width = Editor.shortNoLabelPropertyWidth)]
            [ShowInInspector][HideLabel]
            [DelayedProperty]
            float _maxHp{
                get { 
                    if(EntityData == null) return default;
                    return EntityData.maxHp; 
                }
                set {
                    if (hp == EntityData.maxHp || hp > value) hp = value;
            EntityData.maxHp = value;
                }
            }

//          HideInInspector_____________________________________________________|
            Color _hpColor {
                get {
                    if(EntityData == null) return default;
                    
                    Gradient gradient = new Gradient();
                    gradient.SetKeys(
                        new GradientColorKey[] {
                            new GradientColorKey(Color.yellow, 0),
                            new GradientColorKey(Color.red, 1)
                        },
                        new GradientAlphaKey[] { new GradientAlphaKey(1, 0) }//-|
                    );
                    return gradient.Evaluate(hp / EntityData.maxHp);
                }
            }

            #endif

        #endregion  - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -|    

    #endregion - - - - - - - - - - - - - - - - - - - - -|

        
    public void TakeDamage(float damage)
    {
        hp -= damage;
        if (hp <= 0) Die();
    }
    public void Die()
    {
        Destroy(gameObject);
    }
}