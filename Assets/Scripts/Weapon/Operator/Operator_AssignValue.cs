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
    public override void OnGUI(UnityEditor.SerializedProperty property)
    {
        UnityEditor.EditorGUILayout.PropertyField(
            property.FindPropertyRelative(nameof(value))
        );
    }
#endif
}
