using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BoxUtility
{
    public static Box ToBox(this Collider2D coll) => new Box(coll);
}
