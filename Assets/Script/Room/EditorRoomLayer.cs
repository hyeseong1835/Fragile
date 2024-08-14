using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EditorRoomLayer : MonoBehaviour
{
    public static string[] moduleNames;
    [LayerDropdown] public int layer;

    public Grid grid;
    public List<EditorTileMapModule> tileMapModules;

    public EditorTileMapModule LoadModule(TileMapModuleData moduleData)
    {
        EditorTileMapModule module = (EditorTileMapModule)(new GameObject(moduleData.name).AddComponent(moduleData.moduleType));
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
        tileMapModules = GetComponentsInChildren<EditorTileMapModule>().ToList();
    }
}
