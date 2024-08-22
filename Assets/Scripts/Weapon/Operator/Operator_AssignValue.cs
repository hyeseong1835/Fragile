using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[OperatorInfo("วาด็")]
public class Operator_AssignValue<TObject> : WeaponOperator<TObject>
{
    public TObject value;

    public Operator_AssignValue()
    {
        Debug.Log("Operator_AssignValue is Created!");
    }
    ~Operator_AssignValue()
    {
        Debug.Log("Operator_AssignValue is Disposed!");
    }
    public override TObject GetValue(WeaponBehavior context)
    {
        return value;
    }

#if UNITY_EDITOR
    public override void OnGUI(UnityEditor.SerializedObject ruleObject, string path)
    {
        if (typeof(TObject) == typeof(int))
        {
            value = (TObject)(object)UnityEditor.EditorGUILayout.IntField($"{path}.{nameof(value)}", (int)(object)value);
        }
        else if (typeof(TObject) == typeof(float))
        {
            value = (TObject)(object)UnityEditor.EditorGUILayout.FloatField($"{path}.{nameof(value)}", (float)(object)value);
        }
        else
        {
            value = (TObject)(object)UnityEditor.EditorGUILayout.ObjectField($"{path}.{nameof(value)}", (UnityEngine.Object)(object)value, typeof(TObject), true);
        }

        /*
        UnityEditor.SerializedProperty property = ruleObject.FindProperty($"{path}.{nameof(value)}");
        if (property == null)
        {
            UnityEditor.EditorGUILayout.LabelField($"{path}.{nameof(value)}");
        }
        else
        {
            UnityEditor.EditorGUILayout.PropertyField(property);
        }
        */
    }
#endif
}
