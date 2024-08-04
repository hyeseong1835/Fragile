using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyController : EnemyController
{
    public override Type DataType => typeof(DummyControllerData);

    protected void Start()
    {
        
    }

    new protected void Update()
    {
        base.Update();
    }
}
