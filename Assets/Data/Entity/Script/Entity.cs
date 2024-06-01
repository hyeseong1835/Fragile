using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;


public abstract class Entity : MonoBehaviour 
{
    public const string folderPath = "Assets/Data/Entity";
    public abstract EntityData EntityData { get; set; }
    public abstract Type DataType { get; }
}
