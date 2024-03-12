using JetBrains.Annotations;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public enum NodeType
{
    Void,
    Ground,
}
public class Node
{
    PathFindGrid grid;
    
    [ShowInInspector] public NodeType type;
    [ShowInInspector] public Vector2Int pos;
    public Vector2 worldPosition;
    /// <summary>
    /// 길 되추적을 위한 parent변수.
    /// </summary>
    public Node parent;

    //비용-----------------------------------------------------
    /// <summary>
    /// 시작점 -> 현재 노드
    /// </summary>
    public float pastCost;
    /// <summary>
    /// 현재 노드 -> 목표지점 이동비용.
    /// </summary>
    public float futureCost;
    /// <summary>
    /// 이동비용의 총합.
    /// </summary>
    public float cost { get { return pastCost + futureCost; } }
    //---------------------------------------------------------


    #region Tools
    public static Node GetNodeByIndex(PathFindGrid grid, int x, int y)
    {
        if ((x < 0 && y < 0) && (x >= grid.cellCount.x && y >= grid.cellCount.y)) return null;

        return grid.grid[x, y];
    }
    public static Node GetNodeByIndex(PathFindGrid grid, Vector2Int pos)
    {
        if ((pos.x < 0 && pos.y < 0) && (pos.x >= grid.cellCount.x && pos.y >= grid.cellCount.y)) return null;

        return grid.grid[pos.x, pos.y];
    }
    public static Node GetNodeByWorldPos(PathFindGrid grid, Vector2 worldPos)
    {
        return grid.grid[
            Mathf.RoundToInt((worldPos.x - grid.pivot.x) / grid.nodeSize.x), 
            Mathf.RoundToInt((worldPos.y - grid.pivot.y) / grid.nodeSize.y)
            ];
    }
    #endregion
    public Node(PathFindGrid _grid, NodeType _type, int _x, int _y)
    {
        grid = _grid;
        type = _type;
        pos = new Vector2Int(_x, _y);

        worldPosition = grid.pivot + pos * grid.nodeSize + 0.5f * grid.nodeSize;
    }
}

public class PathFindGrid : MonoBehaviour
{
    // 화면의 크기
    [ShowInInspector][TableMatrix(SquareCells = true)] public Node[,] grid;

    [DisableInPlayMode] public Vector2Int cellCount;
    [DisableInPlayMode] public Vector2 nodeSize;

    [HideInEditorMode] public Vector2 pivot;

    [SerializeField] float thickness = 0.5f;


    [SerializeField] public List<Node> path;

    private void Awake()
    {
        pivot = (Vector2)transform.position - 0.5f * nodeSize * cellCount;
        SetGrid();
    }

    public void SetGrid()
    {
        grid = new Node[cellCount.x, cellCount.y];
        for (int x = 0; x < cellCount.x; x++)
        {
            for (int y = 0; y < cellCount.y; y++)
            {
                SetCell(NodeType.Ground, x, y);
            }
        }
    }
    public Node SetCell(NodeType type, int x, int y)
    {
        grid[x, y] = new Node(this, type, x, y);
        return grid[x, y];
    }
    /// <summary>
    /// 기준 노드를 중심으로 근처 8개 중 유효한 노드를 반환.
    /// </summary>
    /// <param name="node">기준 노드</param>
    /// <returns></returns>
    public List<Node> GetNearNodes(Node node)
    {
        List<Node> neighbours = new List<Node>();

        AddNodeInListByLocalPos(node, ref neighbours, -1, -1);
        AddNodeInListByLocalPos(node, ref neighbours, -1, 0);
        AddNodeInListByLocalPos(node, ref neighbours, -1, 1);

        AddNodeInListByLocalPos(node, ref neighbours, 0, -1);
        AddNodeInListByLocalPos(node, ref neighbours, 0, 1);

        AddNodeInListByLocalPos(node, ref neighbours, 1, -1);
        AddNodeInListByLocalPos(node, ref neighbours, 1, 0);
        AddNodeInListByLocalPos(node, ref neighbours, 1, 1);

        return neighbours;
    }
    void AddNodeInListByLocalPos(Node node, ref List<Node> nodes, int x, int y)
    {
        Node temp = Node.GetNodeByIndex(this, node.pos + new Vector2Int(x, y));

        if (temp == null) return;
        else nodes.Add(temp);
    }
    private void OnDrawGizmos()
    {
        if (grid == null) return;
        Gizmos.color = new Color(0, 0, 0, 0.25f);
        // LB (i )
        Gizmos.DrawCube(new Vector2(pivot.x - 0.5f * thickness, transform.position.y - 0.5f * thickness), 
            new Vector2(thickness, nodeSize.y * cellCount.y + thickness));

        // RB ( _)
        Gizmos.DrawCube(new Vector2(transform.position.x + 0.5f * thickness, pivot.y - 0.5f * thickness), 
            new Vector2(nodeSize.x * cellCount.x + thickness, thickness));

        // LT (^ )
        Gizmos.DrawCube(new Vector2(transform.position.x - 0.5f * thickness, pivot.y + nodeSize.y * cellCount.y + 0.5f * thickness),
            new Vector2(nodeSize.x * cellCount.x + thickness, thickness));

        // RT ( !)
        Gizmos.DrawCube(new Vector2(pivot.x + nodeSize.x * cellCount.x + 0.5f * thickness, transform.position.y + 0.5f * thickness),
            new Vector2(thickness, nodeSize.y * cellCount.y + thickness));

    }
    private void OnDrawGizmosSelected()
    {
        if (grid == null) return;

        foreach (Node node in grid)
        {
            switch (node.type)
            {
                case NodeType.Void:
                    Gizmos.color = new Color(0, 0, 0, 0.5f);
                    Gizmos.DrawWireCube(node.worldPosition, nodeSize * 0.99f);
                    Gizmos.color = new Color(0, 0, 0, 0.25f);
                    Gizmos.DrawCube(node.worldPosition, nodeSize * 1);

                    break;
                case NodeType.Ground:
                    Gizmos.color = new Color(0.25f, 0.25f, 0.5f, 0.25f);
                    Gizmos.DrawWireCube(node.worldPosition, nodeSize * 0.99f);
                    break;
            }
        }
    }
}