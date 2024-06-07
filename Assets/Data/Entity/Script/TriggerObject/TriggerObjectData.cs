using System;

public class TriggerObjectData : EntityData
{
#if UNITY_EDITOR
    public override void ResetName()
    {
        throw new NotImplementedException();
    }
#endif
}
