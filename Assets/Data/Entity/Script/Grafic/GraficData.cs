using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraficData : ScriptableObject
{
    public Texture2D spriteSheet;

    public int spritePixelWidth = 16;
    public int spritePixelHeight = 16;

    [ShowInInspector]
    [DisableInEditorMode]
    [TableMatrix(IsReadOnly = true, SquareCells = true, HorizontalTitle = "Frame", VerticalTitle = "Index")]
    public Sprite[,] sprites;
}
