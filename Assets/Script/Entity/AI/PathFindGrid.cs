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

    Color[,] debugMap;
    [SerializeField] Color defaultColor = new Color(1, 1, 1, 0);
    [SerializeField] Color trueColor;
    [SerializeField] Color falseColor = Color.green;
    [SerializeField] Color startColor = Color.red;
    [SerializeField] Color endColor = Color.blue;
    [SerializeField] Color lineColor = Color.cyan;
    [SerializeField] Vector2 start;
    [SerializeField] Vector2 end;


    private void Awake()
    {
        SetGrid();

        debugMap = new Color[cellCount.x, cellCount.y];
        for (int x = 0; x < cellCount.x; x++)
        {
            for (int y = 0; y < cellCount.y; y++)
            {
                debugMap[x, y] = defaultColor;
            }
        }
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
    public Vector2 ExtendToCellGrid(Vector2 worldPos)
    {
        return (worldPos - pivot) / cellSize;
    }
    public bool IsCellExist(Vector2Int cellPos)
    {
        return ((cellPos.x >= 0 && cellPos.y >= 0) && (cellPos.x < cellCount.x && cellPos.y < cellCount.y));
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
        //Debug.Log(cell.cellPos);
        debugMap[cell.cellPos.x, cell.cellPos.y] = falseColor;
        return false;
    }
    [DisableInEditorMode]
    [Button(ButtonStyle.Box)]
    public Cell RayCastGrid(Vector2 startWorldPos, Vector2 dir, float distance)
    {
        for (int x = 0; x < cellCount.x; x++)
        {
            for (int y = 0; y < cellCount.y; y++)
            {
                debugMap[x, y] = defaultColor;
            }
        }

        Cell cell;

        #region 길이가 0일 때

        if (distance == 0 || dir.x == 0 && dir.y == 0)
        {
            if (TryGetCell(UnSafeGetCellPosByWorldPos(startWorldPos), out cell) && CheckCell(cell))
            {
                return cell;
            }
            else return null;
        }

        #endregion

        dir = (dir / cellSize).normalized;
        Vector2 startExtendPos = ExtendToCellGrid(startWorldPos);
        Vector2 endExtendPos = ExtendToCellGrid(startWorldPos + dir * distance);

        Vector2Int checkCellPos 
            = new Vector2Int(Mathf.RoundToInt(startExtendPos.x), Mathf.RoundToInt(startExtendPos.y));


        #region 축과 수직일 때

        //X축과 수직일 때
        if (dir.x == 0)
        {
            //위로 체크
            if (dir.y > 0)
            {
                while (checkCellPos.y++ - 0.5f < endExtendPos.y && TryGetCell(checkCellPos, out cell))
                {
                    if(CheckCell(cell)) return cell;
                }
            }
            //아래로 체크
            else
            {
                while (checkCellPos.y-- + 0.5f > endExtendPos.y && TryGetCell(checkCellPos, out cell))
                {
                    if (CheckCell(cell)) return cell;
                }
            }
        }

        //Y축과 수직일 때
        if (dir.y == 0)
        {
            //오른쪽으로 체크
            if (dir.x > 0)
            {
                while (checkCellPos.x++ - 0.5f < endExtendPos.x && TryGetCell(checkCellPos, out cell))
                {
                    if (CheckCell(cell)) return cell;
                }
            }
            //왼쪽으로 체크
            else
            {
                while (checkCellPos.x-- + 0.5f > endExtendPos.x && TryGetCell(checkCellPos, out cell))
                {
                    if (CheckCell(cell)) return cell;
                }
            }
        }

        #endregion

        #region 계산
        float extendCellSlope = dir.y / dir.x;

        //기울기가 1보다 클 때
        if (extendCellSlope >= 1)
        {
            if (startExtendPos.x < 0)
            {

            }
            if (startExtendPos.y < 0)
            {

            }
            //X 순회
            while (checkCellPos.x - 0.5f <= endExtendPos.x) //초과
            {
                int nextCellPosY = Mathf.RoundToInt(extendCellSlope * (checkCellPos.x + 0.5f) - startExtendPos.y);
                Debug.Log("Next: " + nextCellPosY);
                while ((checkCellPos.y <= nextCellPosY) && ((checkCellPos.y + 0.5f) <= endExtendPos.y))
                {
                    Debug.Log("Check: " + checkCellPos.y);
                    if (TryGetCell(checkCellPos, out cell))
                    {
                        if (CheckCell(cell)) return cell;
                        else checkCellPos.y++;
                    }
                    else return null;
                }
                checkCellPos.x++;
                if (TryGetCell(new Vector2Int(checkCellPos.x, checkCellPos.y - 1), out cell))
                {
                    if (CheckCell(cell)) return cell;
                }
            }
        }

        //기울기가 1보다 작을 때
        if(extendCellSlope < -1)
        {
            
        }

        // 기울기가 0~1 사이일 때
        if (extendCellSlope > 0)
        {

        }

        // 기울기가 -1~0 사이일 때
        
        #endregion

        return null;
    }
    private void OnDrawGizmos()
    {
        if (grid == null) return;
        Gizmos.color = new Color(0, 0, 0, 0.25f);

        Gizmos.DrawWireCube(transform.position, cellSize * cellCount + new Vector2(0.25f, 0.25f));
        Gizmos.DrawWireCube(transform.position, cellSize * cellCount + new Vector2(0.5f, 0.5f));

        if (debugMap != null)
        {
            for (int x = 0; x < cellCount.x; x++)
            {
                for (int y = 0; y < cellCount.y; y++)
                {
                    Gizmos.color = debugMap[x, y];
                    Gizmos.DrawCube(GetWorldPosByCellPos(new Vector2Int(x, y)), cellSize * 0.5f);
                }
            }
            Gizmos.color = startColor;
            Gizmos.DrawSphere(start, 0.1f);
            
            Gizmos.color = endColor;
            Gizmos.DrawSphere(end, 0.1f);

            Gizmos.color = lineColor;
            Gizmos.DrawLine(start, end);
        }
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