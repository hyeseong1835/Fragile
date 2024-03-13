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
    PathFindGrid grid;
    public CellType type;
    public Vector2Int pos;
    public Vector2 worldPosition;

    public static Cell GetCellByIndex(PathFindGrid grid, int x, int y)
    {
        if ((x < 0 && y < 0) && (x >= grid.cellCount.x && y >= grid.cellCount.y)) return null;

        return grid.grid[x, y];
    }
    public static Cell GetCellByIndex(PathFindGrid grid, Vector2Int pos)
    {
        if ((pos.x < 0 && pos.y < 0) && (pos.x >= grid.cellCount.x && pos.y >= grid.cellCount.y)) return null;

        return grid.grid[pos.x, pos.y];
    }
    public Cell(PathFindGrid _grid, CellType _type, int _x, int _y)
    {
        grid = _grid;
        type = _type;
        pos = new Vector2Int(_x, _y);

        worldPosition = grid.pivot + pos * grid.cellSize + 0.5f * grid.cellSize;
    }
    public Cell(PathFindGrid _grid, CellType _type, Vector2Int _pos)
    {
        grid = _grid;
        type = _type;
        pos = _pos;

        worldPosition = grid.pivot + pos * grid.cellSize + 0.5f * grid.cellSize;
    }
}

public class PathFindGrid : MonoBehaviour
{
    public Cell[,] grid;

    [DisableInPlayMode] public Vector2Int cellCount;
    [DisableInPlayMode] public Vector2 cellSize;

    [HideInEditorMode] public Vector2 pivot;

    private void Awake()
    {
        pivot = (Vector2)transform.position - 0.5f * cellSize * cellCount;
        SetGrid();
    }
    void SetGrid()
    {
        grid = new Cell[cellCount.x, cellCount.y];
        for (int x = 0; x < cellCount.x; x++)
        {
            for (int y = 0; y < cellCount.y; y++)
            {
                SetCell(CellType.Ground, x, y);
            }
        }
    }
    Cell SetCell(CellType type, int x, int y)
    {
        grid[x, y] = new Cell(this, type, x, y);
        return grid[x, y];
    }
    Cell SetCell(CellType type, Vector2Int pos)
    {
        grid[pos.x, pos.y] = new Cell(this, type, pos);
        return grid[pos.x, pos.y];
    }
    public Cell GetCellByWorldPos(Vector2 worldPos)
    {
        return grid[
            Mathf.RoundToInt((worldPos.x - pivot.x) / cellSize.x),
            Mathf.RoundToInt((worldPos.y - pivot.y) / cellSize.y)
            ];
    }
    
    private void OnDrawGizmos()
    {
        if (grid == null) return;
        Gizmos.color = new Color(0, 0, 0, 0.25f);

        Gizmos.DrawWireCube(pivot + 0.5f * cellSize * cellCount, cellSize * cellCount + new Vector2(0.25f, 0.25f));
        Gizmos.DrawWireCube(pivot + 0.5f * cellSize * cellCount, cellSize * cellCount + new Vector2(0.5f, 0.5f));
    }
    private void OnDrawGizmosSelected()
    {
        if (grid == null) return;

        foreach (Cell cell in grid)
        {
            switch (cell.type)
            {
                case CellType.Void:
                    Gizmos.color = new Color(0, 0, 0, 0.5f);
                    Gizmos.DrawWireCube(cell.worldPosition, cellSize * 0.9f);
                    Gizmos.color = new Color(0, 0, 0, 0.25f);
                    Gizmos.DrawCube(cell.worldPosition, cellSize * 1);

                    break;
                case CellType.Ground:
                    Gizmos.color = new Color(0.25f, 0.25f, 0.5f, 0.25f);
                    Gizmos.DrawWireCube(cell.worldPosition, cellSize * 0.9f);
                    break;
            }
        }
    }
}