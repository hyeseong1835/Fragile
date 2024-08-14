using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptOverride
{

}
[CreateAssetMenu(menuName = "Room/ObjectRule", fileName = "New ObjectRule")]
public class RoomObjectRule : ScriptableObject
{
    public GameObject prefab;
}
