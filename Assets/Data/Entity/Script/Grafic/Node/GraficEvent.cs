using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class GraficEvent<T> : EventNode<T>
{
    protected override void SetTarget(GraphReference reference)
    {
        target = reference.gameObject.transform.Find("Grafic").gameObject;
    }
}

[UnitTitle("Grafic Move")]
[UnitCategory("Events/Animation")]
public class GraficMoveEvent : GraficEvent<AnimationType>
{
    public override string eventName => "GraficMove";
}

[UnitTitle("Grafic Attack")]
[UnitCategory("Events/Animation")]
public class GraficAttackEvent : GraficEvent<GraficSkillStyle>
{
    public override string eventName => "GraficAttack";  
}

[UnitTitle("Grafic Special")]
[UnitCategory("Events/Animation")]
public class GraficSpecialEvent : GraficEvent<GraficSkillStyle>
{
    public override string eventName => "GraficSpecial"; 
}