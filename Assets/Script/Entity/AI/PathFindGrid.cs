using JetBrains.Annotations;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public enum CellType
{
    Void,
    Ground,
}
public class Cell
{
    public Node node;
    public CellType type;
    public Vector2Int cellPos;
    public Vector2 worldPos;
    public Cell(CellType type, Vector2Int cellPos, Vector2 worldPos)
    {
        this.type = type;
        this.cellPos = cellPos;
        this.worldPos = worldPos;
    }
    public Cell(PathFindGrid grid, CellType type, Vector2Int cellPos)
    {
        this.type = type;
        this.cellPos = cellPos;
        this.worldPos = grid.pivot + cellPos * grid.cellSize + 0.5f * grid.cellSize;
    }



    /*
    public Cell(PathFindGrid _grid, CellType _type, int _x, int _y)
    {
        type = _type;
        cellPos = new Vector2Int(_x, _y);

        worldPos = _grid.pivot + cellPos * _grid.cellSize + 0.5f * _grid.cellSize;
    }
    public Cell(PathFindGrid _grid, CellType _type, Vector2Int _cellPos)
    {
        type = _type;
        cellPos = _cellPos;

        worldPos = _grid.pivot + cellPos * _grid.cellSize + 0.5f * _grid.cellSize;
    }
    */
}

public class PathFindGrid : MonoBehaviour
{
    public Cell[,] grid;
    [DisableInPlayMode] public Vector2Int cellCount;
    [DisableInPlayMode] public Vector2 cellSize;
    [DisableInEditorMode] public Vector2 pivot;

    [SerializeField] Color voidColor = new Color(0, 0, 0, 0.5f);
    [SerializeField] Color groundColor = new Color(0.25f, 0.25f, 0.5f, 0.25f);

    private void Awake()
    {
        SetGrid();
    }
    [DisableInEditorMode]
    [Button(ButtonStyle.Box)]
    void SetGrid()
    {
        pivot = (Vector2)transform.position - 0.5f * cellSize * cellCount;

        grid = new Cell[cellCount.x, cellCount.y];
        Debug.Log(grid+" / "+grid.Length);
        for (int x = 0; x < cellCount.x; x++)
        {
            for (int y = 0; y < cellCount.y; y++)
            {
                grid[x, y] = new Cell(this, CellType.Ground, new Vector2Int(x, y));
            }
        }
    }
    void ClearNode()
    {
        for (int x = 0; x < cellCount.x; x++)
        {
            for (int y = 0; y < cellCount.y; y++)
            {
                grid[x, y].node = null;
            }
        }
    }
    public Vector2 GetWorldPosByCellPos(Vector2Int cellPos)
    {
        return pivot + cellPos * cellSize + 0.5f * cellSize;
    }
    public Vector2Int UnSafeGetCellPosByWorldPos(Vector2 worldPos)
    {
        return new Vector2Int(
            Mathf.RoundToInt((worldPos.x - pivot.x) / cellSize.x),
            Mathf.RoundToInt((worldPos.y - pivot.y) / cellSize.y));
    }
    public bool IsCellExist(Vector2Int cellPos)
    {
        return ((cellPos.x >= 0 && cellPos.y >= 0) && (cellPos.x < cellCount.x && cellPos.y < cellCount.y));
    }
    public Cell UnSafeGetCellByWorldPos(Vector2 worldPos)
    {
        Vector2Int cellPos = UnSafeGetCellPosByWorldPos(worldPos);
        if(!IsCellExist(cellPos)) return null;

        return grid[cellPos.x, cellPos.y];
    }
    public bool TryGetCell(Vector2Int cellPos, out Cell cell)
    {
        if (!IsCellExist(cellPos))
        {
            cell = null;
            return false;
        }

        cell = grid[cellPos.x, cellPos.y];
        return true;
    }
    private void OnDrawGizmos()
    {
        if (grid == null) return;
        Gizmos.color = new Color(0, 0, 0, 0.25f);

        Gizmos.DrawWireCube(transform.position, cellSize * cellCount + new Vector2(0.25f, 0.25f));
        Gizmos.DrawWireCube(transform.position, cellSize * cellCount + new Vector2(0.5f, 0.5f));
    }
    private void OnDrawGizmosSelected()
    {
        if (grid == null) return;

        foreach (Cell cell in grid)
        {
            switch (cell.type)
            {
                case CellType.Void:
                    Gizmos.color = voidColor;
                    Gizmos.DrawWireCube(cell.worldPos, cellSize * 0.9f);
                    Gizmos.color = new Color(0, 0, 0, 0.25f);
                    Gizmos.DrawCube(cell.worldPos, cellSize * 1);

                    break;
                case CellType.Ground:
                    Gizmos.color = groundColor;
                    Gizmos.DrawWireCube(cell.worldPos, cellSize * 0.9f);
                    break;
            }
        }
    }
}