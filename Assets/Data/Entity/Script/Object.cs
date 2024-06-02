using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object : Entity
{
    
    public ObjectData data;
    public override EntityData EntityData
    {
        get => data;
        set { data = (ObjectData)value; }
    }
    public override Type DataType => typeof(ObjectData);

}