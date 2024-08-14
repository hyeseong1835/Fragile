using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RoomLayer : MonoBehaviour
{
    public static string[] moduleNames;
    [LayerDropdown] public int layer;

    public Grid grid;

    public EditorRoom editorRoom;
    public List<TileMapModule> tileMapModules;


    public static RoomLayer Create(EditorRoom editorRoom, TileLayer layerData)
    {
        RoomLayer roomLayer = new GameObject(
            LayerMask.LayerToName(layerData.layer)
        ).AddComponent<RoomLayer>();
        {
            roomLayer.editorRoom = editorRoom;
            roomLayer.layer = layerData.layer;
            roomLayer.grid = roomLayer.AddComponent<Grid>();
            roomLayer.tileMapModules = new List<TileMapModule>();
            {
                foreach (TileMapModuleData moduleData in layerData.tileMapModuleData)
                {
                    roomLayer.tileMapModules.Add(TileMapModule.Create(roomLayer).Load(moduleData));
                }
            }
        }
        return roomLayer;
    }
    public TileMapModule LoadModule(TileMapModuleData moduleData)
    {
        TileMapModule module = (TileMapModule)(new GameObject(moduleData.name).AddComponent(moduleData.moduleType));
        {
            module.tilemap = module.AddComponent<Tilemap>();
            {
                module.tilemap.transform.SetParent(transform);
                foreach (TileInfo tileInfo in module.GetTileInfoArray())
                {
                    module.tilemap.SetTile(
                        (Vector3Int)tileInfo.pos,
                        tileInfo.tile
                    );
                }
            }
            module.tilemapRenderer = module.AddComponent<TilemapRenderer>();
            {
                module.tilemapRenderer.transform.SetParent(transform);
                module.tilemapRenderer.mode = moduleData.renderMode;
                module.tilemapRenderer.material = moduleData.material;
            }

            module.Load(moduleData);
        }
        return module;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    [Button("모듈 다시 로드하기")]
    void RefreshModule()
    {
        tileMapModules = GetComponentsInChildren<TileMapModule>().ToList();
    }
}
