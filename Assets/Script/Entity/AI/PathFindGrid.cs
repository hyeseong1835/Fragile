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
        else
        {
            cell = grid[cellPos.x, cellPos.y];
            return true;
        }
    }
    bool CheckCell(Cell cell)
    {
        return true;
    }
    public Cell RayCastGrid(Vector2 startWorldPos, Vector2 dir, float distance)
    {
        Vector2Int cellPos = UnSafeGetCellPosByWorldPos(startWorldPos);

        #region 길이가 0일 때
        
        if (distance == 0 || dir.x == 0 && dir.y == 0)
        {
            Cell cell;
            if (TryGetCell(cellPos, out cell) && CheckCell(cell))
            {
                return cell;
            }
            else return null;
        }

        #endregion

        Vector2Int endCellPos = UnSafeGetCellPosByWorldPos(startWorldPos + dir * distance);
        Vector2Int checkCellPos = cellPos;

        #region 축과 수직일 때

        //X축과 수직일 때
        if (dir.x == 0)
        {
            if (dir.y > 0)
            {
                //위로 체크
                for(; ; )
                {
                    Cell cell;
                    if(++checkCellPos.y < endCellPos.y && TryGetCell(checkCellPos, out cell))
                    {
                        CheckCell(cell);
                    }
                    else return null;
                }
            }
            else
            {
                //아래로 체크
                for (; ; )
                {
                    Cell cell;
                    if (--checkCellPos.y > endCellPos.y && TryGetCell(checkCellPos, out cell))
                    {
                        //cell 체크
                    }
                    else return null;
                }
            }
        }

        //Y축과 수직일 때
        if (dir.y == 0)
        {
            if (dir.x > 0)
            {
                //오른쪽으로 체크
                for (; ; )
                {
                    Cell cell;
                    if (++checkCellPos.x < endCellPos.y && TryGetCell(checkCellPos, out cell))
                    {
                        CheckCell(cell);
                    }
                    else return null;
                }
            }
            else
            {
                //왼쪽으로 체크
                for (; ; )
                {
                    Cell cell;
                    if (--checkCellPos.y > endCellPos.y && TryGetCell(checkCellPos, out cell))
                    {
                        CheckCell(cell);
                    }
                    else return null;
                }
            }
        }

        #endregion

        /*
        if (dir.x > 0)
        {
            moveX = 1;
            startX = Mathf.Ceil(startWorldPos.x * cellSize.x);
            slope = dir.y / dir.x;
        }
        else
        {
            moveX = -1;
            startX = Mathf.Floor(startWorldPos.x * cellSize.x);
            slope = dir.y / dir.x;
        }
        //*/

        #region 계산
        float slope = dir.x / dir.y;

        //기울기가 1보다 클 때
        if (slope > 1)
        {
            //체크 시스템 만들어라---
            cellPos.y += slope;

            while (true)
            {
                
                for (int i = cellPos.y; i < destinationY; i++)
                {
                    Cell cell;
                    if (TryGetCell(new Vector2Int(cellPos.x, i), out cell))
                    {
                        //검사
                        //맞으면 리턴 아니면 계속
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }

        //기울기가 1보다 작을 때
        if(slope < -1)
        {

        }

        // 기울기가 0~1 사이일 때
        if (slope > 0)
        {

        }

        // 기울기가 -1~0 사이일 때
        if(slope < 0)
        {

        }
        #endregion
        
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