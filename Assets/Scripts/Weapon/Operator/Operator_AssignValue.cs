using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
#if UNITY_EDITOR
[@ComponentInfo("할당", "기본적인 연산자입니다.")]
#endif
public class Operator_AssignValue<TObject> : WeaponOperator<TObject>
{
    public TObject value;

    public override TObject GetValue(WeaponBehavior context)
    {
        return value;
    }

#if UNITY_EDITOR
    protected override void DrawHeaderProperty(Rect rect)
    {
        if (typeof(TObject) == typeof(int))
        {
            value = (TObject)(object)UnityEditor.EditorGUI.IntField(rect, (int)(object)value);
        }
        else if (typeof(TObject) == typeof(float))
        {
            value = (TObject)(object)UnityEditor.EditorGUI.FloatField(rect, (float)(object)value);
        }
        else if (typeof(TObject).IsSubclassOf(typeof(UnityEngine.Object)))
        {
            value = (TObject)(object)UnityEditor.EditorGUI.ObjectField(
                rect,
                (UnityEngine.Object)(object)value,
                typeof(TObject),
                true
            );
        }
    }
#endif
}
