using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Node
{
    public Cell cell;
    public Node parent;

    /// <summary>
    /// 시작노드 -> 현재 노드 이동비용. (g)
    /// </summary>
    public float pastCost { get; set; }
    /// <summary>
    /// 현재 노드 -> 목표 노드 이동비용.(h)
    /// </summary>
    public float futureCost { get; set; }
    /// <summary>
    /// 이동비용의 총합. (f)
    /// </summary>
    public float cost { get { return pastCost + futureCost; } }

    public Node(Cell _cell, Node _parent, float _pastCost, float _futureCost)
    {
        cell = _cell;
        parent = _parent;
        pastCost = _pastCost;
        futureCost = _futureCost;
    }
    public Node(PathFindGrid grid, Vector2Int pos, Node _parent, float _pastCost, float _futureCost)
    {
        cell = grid.grid[pos.x, pos.y];
        parent = _parent;
        pastCost = _pastCost;
        futureCost = _futureCost;
    }
    public Node(PathFindGrid grid, int x, int y, Node _parent, float _pastCost, float _futureCost)
    {
        cell = grid.grid[x, y];
        parent = _parent;
        pastCost = _pastCost;
        futureCost = _futureCost;
    }
}


public class AStar : MonoBehaviour
{
    [Header("Path Finding")]
    public PathFindGrid grid;
        
    Dictionary<Vector2Int, Node> nodes = new Dictionary<Vector2Int, Node>();
    
    public Vector2[] FindPath(Vector2 startPos, Vector2 targetPos)
    {
        Cell startCell = grid.GetCellByWorldPos(startPos);
        if (startCell.type == CellType.Void)
        {
            Debug.LogWarning("시작점이 유효하지 않습니다.");
            return null;
        } //LogWarning: 시작점이 유효하지 않습니다.

        Cell targetCell = grid.GetCellByWorldPos(targetPos);
        if (targetCell.type == CellType.Void)
        {
            Debug.LogWarning("목적지가 유효하지 않습니다.");
            return null;
        } //LogWarning: 목적지가 유효하지 않습니다.

        Node startNode = new Node(startCell, null, 0, Vector2Int.Distance(startCell.pos, targetCell.pos));
        nodes.Add(startCell.pos, startNode);
        startNode.futureCost = Vector2Int.Distance(startNode.cell.pos, grid.GetCellByWorldPos(targetPos).pos);




        Node targetNode = new Node(grid.GetCellByWorldPos(startPos), null, Mathf.Infinity, Mathf.Infinity);
        nodes.Add(targetCell.pos, targetNode);


        if (startNode == targetNode) return new Vector2[] { targetNode.cell.pos };



        List<Node> openSet = new List<Node>(){ startNode };

        HashSet<Node> closedSet = new HashSet<Node>();


        while (openSet.Count > 0)
        {
            Node currentNode = openSet[0];

            // 총 비용과 다음 예상 비용이 가장 낮은 노드 선택 => currentNode
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].cost <= currentNode.cost
                    && openSet[i].futureCost < currentNode.futureCost)
                    currentNode = openSet[i];
            }

            if (currentNode == targetNode) return GetPathByNodeToNode(startNode, targetNode);

            // 이웃 노드들을 검사
            foreach (Node nearNode in GetNearNode(currentNode))
            {
                if ((nearNode.cell.type == CellType.Void) || closedSet.Contains(nearNode)) continue;

                float movementCost = currentNode.pastCost + Vector2Int.Distance(currentNode.cell.pos, nearNode.cell.pos);

                // 비용이 감소하는 방향으로 이동했을 때
                if (movementCost < nearNode.pastCost || !openSet.Contains(nearNode))
                {
                    nearNode.pastCost = movementCost;
                    nearNode.futureCost = Vector2Int.Distance(nearNode.cell.pos, targetNode.cell.pos);
                    nearNode.parent = currentNode;

                    // openSet에 추가.
                    if (!openSet.Contains(nearNode)) openSet.Add(nearNode);
                }
            }
            // openSet에서 제거하고 closedSet에 추가.
            openSet.Remove(currentNode);
            closedSet.Add(currentNode);
        }
        Debug.LogWarning("길을 찾을 수 없습니다.");
        return null;
    }
    public List<Node> GetNearNode(Node node)
    {
        List<Node> nearNodes = new List<Node>();

        AddNodeInListByLocalPos(node, ref nearNodes, -1, -1);
        AddNodeInListByLocalPos(node, ref nearNodes, -1, 0);
        AddNodeInListByLocalPos(node, ref nearNodes, -1, 1);

        AddNodeInListByLocalPos(node, ref nearNodes, 0, -1);
        AddNodeInListByLocalPos(node, ref nearNodes, 0, 1);

        AddNodeInListByLocalPos(node, ref nearNodes, 1, -1);
        AddNodeInListByLocalPos(node, ref nearNodes, 1, 0);
        AddNodeInListByLocalPos(node, ref nearNodes, 1, 1);

        return nearNodes;
    }
    void AddNodeInListByLocalPos(Node anchorNode, ref List<Node> nodeList, int x, int y)
    {
        Node node = nodes[anchorNode.cell.pos + new Vector2Int(x, y)];

        if (node == null) return;
        else
        {
            nodeList.Add(node);
            nodes.Add(node.cell.pos, nodeList[^1]);
        }
    }
    Vector2[] GetPathByNodeToNode(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node curNode = endNode;
        while (curNode != startNode)
        {
            path.Add(curNode);
            curNode = curNode.parent;
        }
        path.Reverse();
        Vector2[] wayPoints = NodeListToVector2Array(path);
        return wayPoints;
    }
    Vector2[] NodeListToVector2Array(List<Node> path)
    {
        Vector2[] wayPoints = new Vector2[path.Count];
        for (int i = 0; i < path.Count; i++)
        {
            wayPoints[i] = path[i].cell.worldPosition;
        }
        return wayPoints;
    }
    void OnDrawGizmos()
    {
        
    }
}
