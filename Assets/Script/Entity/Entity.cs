using UnityEngine;
using System;
using Sirenix.OdinInspector;
using UnityEngine.UIElements;
using UnityEditor;
using UnityEngine.Windows;

public abstract class Entity : MonoBehaviour 
{
    public abstract float MaxHp { get; set; }


    [FoldoutGroup("Stat")]
    #region Foldout Stat - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -|                                         

        [HorizontalGroup("Stat/HP")]
        #region Horizontal HP

            [LabelWidth(Editor.labelWidth)]
            #if UNITY_EDITOR
            [ProgressBar(0, nameof(MaxHp), ColorGetter = nameof(_hpColor))]
            #endif
            public float hp = -1;

            #if UNITY_EDITOR
                                                                                         [HorizontalGroup("Stat/HP", Width = Editor.shortNoLabelPropertyWidth)]
            [ShowInInspector]
            [HideLabel]
            [DelayedProperty]
            float showMaxHp {
                get => MaxHp;
                set {
                    if (value <= 0)
                    {
                        hp = -1;
                        MaxHp = -1;
                        return;
                    }

                    if(hp == MaxHp)
                    {
                        hp = value;
                    }
                    if (value < hp)
                    {
                        hp = value;
                    }
                    MaxHp = value;
                }
            }
                                
            Color _hpColor {
                get {
                    if (MaxHp == -1) return Color.white;

                    Gradient gradient = new Gradient();
                    gradient.SetKeys(
                        new GradientColorKey[] {
                            new GradientColorKey(Color.yellow, 0),
                            new GradientColorKey(Color.red, 1)
                        },
                        new GradientAlphaKey[] { new GradientAlphaKey(1, 0) }//-|
                    );
                    return gradient.Evaluate(hp / MaxHp);
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