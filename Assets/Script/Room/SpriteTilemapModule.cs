using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteTilemapModule : TileMapModule
{
    public override TileMapModule Load(TileMapModuleData data)
    {
        return this;
    }

    public override string Save()
    {
        return "";
    }
}
