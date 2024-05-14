using Sirenix.OdinInspector;
using UnityEngine.Events;

public class Skill_Empty : Skill
{
    [FoldoutGroup("Event")]
    #region Foldout Event - - - - - - - - - - - - - - - - - - - - -|

        public UnityEvent<TriggerObject> startEvent;

    #endregion  - - - - - - - - - - - - - - - - - - - - - - - - - -|

    protected override void Init()
    {

    }

    public override void Execute()
    {

    }

    public override void Break()
    {

    }

    public override void Destroyed()
    {

    }

    public override void Removed()
    {

    }
}
