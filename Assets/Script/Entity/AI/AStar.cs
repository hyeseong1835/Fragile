using Sirenix.OdinInspector;
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


    [ShowInInspector][TableMatrix(SquareCells = true)] Color[,] debugMap;
    [SerializeField] Color openColor = Color.red;
    [SerializeField] Color closeColor = Color.blue;
    [SerializeField] Color moveColor = Color.white;

    void Start()
    {
        FindPath(new Vector2(-2, -2), new Vector2(2, 2));
    }
    public Vector2[] FindPath(Vector2 startWorldPos, Vector2 targetWorldPos)
    {
        #region 변수 초기화
        //StartCell
        Cell startCell;
        if (!grid.TryGetCell(grid.UnSafeGetCellPosByWorldPos(startWorldPos), out startCell))
        {
            Debug.LogWarning("시작점( " + startWorldPos + " )이 유효하지 않습니다.");
            return null;
        } //LogWarning: 시작점이 유효하지 않습니다.
        if (startCell.type == CellType.Void)
        {
            Debug.LogWarning("시작점[ " + startCell.cellPos + " ]( " + startCell.worldPos + " )이 Void입니다.");
            return null;
        }

        //TargetCell
        Cell targetCell;
        if (!grid.TryGetCell(grid.UnSafeGetCellPosByWorldPos(targetWorldPos), out targetCell))
        {
            Debug.LogWarning("목적지( " + targetWorldPos + " )가 유효하지 않습니다.");
            return null;
        } //LogWarning: 목적지가 유효하지 않습니다.
        if (startCell == targetCell) return new Vector2[] { targetCell.cellPos };

        if (targetCell.type == CellType.Void)
        {
            Debug.LogWarning("목적지[ " + targetCell.cellPos + " ]( " + targetCell.worldPos + " )가 Void입니다.");
            return null;
        }
        

        Node startNode = new Node(startCell, null, 0, Vector2Int.Distance(startCell.cellPos, targetCell.cellPos));
        Node targetNode = new Node(targetCell, null, -1, -1);

        nodes = new Dictionary<Vector2Int, Node>() {
                { startCell.cellPos, startNode }
            };
        debugMap = new Color[grid.cellCount.x, grid.cellCount.y];

        List<Node> openSet = new List<Node>(){ startNode };
        HashSet<Node> closedSet = new HashSet<Node>();
        #endregion

        while (openSet.Count > 0)
        {
            Node currentNode = openSet[0];

            // 총 비용과 다음 예상 비용이 가장 낮은 노드 선택 => currentNode
            for (int i = 1; i < openSet.Count; i++)
            {
                // 도착 => 경로 반환
                if (currentNode.pastCost == -1 && currentNode.futureCost == -1) return GetPathBetweenNodes(startNode, targetNode);

                if (openSet[i].cost <= currentNode.cost
                    && openSet[i].futureCost < currentNode.futureCost)
                    currentNode = openSet[i];
            }

            Debug.Log("읽음: " + currentNode.cell.cellPos);
            debugMap[currentNode.cell.cellPos.x, currentNode.cell.cellPos.y] = moveColor;

            // 이웃 노드들을 추가 및 검사
            foreach (Node nearNode in GetNearNode(currentNode))
            {
                if ((nearNode.cell.type == CellType.Void) || closedSet.Contains(nearNode)) continue;
                Debug.Log("추가"+nearNode.cell.cellPos);
                float movementCost = currentNode.pastCost + Vector2Int.Distance(currentNode.cell.cellPos, nearNode.cell.cellPos);

                // 비용이 감소하는 방향으로 이동했을 때
                if (movementCost < nearNode.pastCost || !openSet.Contains(nearNode))
                {
                    nearNode.pastCost = movementCost;
                    nearNode.futureCost = Vector2Int.Distance(nearNode.cell.cellPos, targetNode.cell.cellPos);
                    nearNode.parent = currentNode;

                    // openSet에 추가.
                    if (!openSet.Contains(nearNode))
                    {
                        openSet.Add(nearNode);
                        Debug.Log("추가: " + nearNode.cell.cellPos);
                        debugMap[nearNode.cell.cellPos.x, nearNode.cell.cellPos.y] = openColor;
                    }
                    else Debug.Log("갱신: " + nearNode.cell.cellPos);
                }
            }
            // currentNode를 닫음.
            openSet.Remove(currentNode);
            closedSet.Add(currentNode);
            debugMap[currentNode.cell.cellPos.x, currentNode.cell.cellPos.y] = closeColor;
        }
        Debug.LogWarning("길을 찾을 수 없습니다.");
        return null;
    }
    Node LoadNode(Vector2Int pos)
    {
        Debug.Log("LoadNode"+pos+"/"+ grid.IsCellExist(pos));
        if(!grid.IsCellExist(pos)) return null;

        if (nodes.TryGetValue(pos, out Node node))
        {
            return node;
        }
        else
        {
            nodes.Add(pos, new Node(grid, pos, null, -1, -1));
            return node;
        }
    }
    bool TryLoadNode(Vector2Int pos, out Node node)
    {
        node = LoadNode(pos);

        Debug.Log("TryLoadNode" + pos+"/"+node);
        if (node == null) return false;
        else return true;
    }
    public List<Node> GetNearNode(Node node)
    {
        List<Node> nearNodes = new List<Node>();

        AddNodeInListByLocalPos(node.cell.cellPos, new Vector2Int(-1, -1), ref nearNodes);
        AddNodeInListByLocalPos(node.cell.cellPos, new Vector2Int(-1, 0), ref nearNodes);
        AddNodeInListByLocalPos(node.cell.cellPos, new Vector2Int(-1, 1), ref nearNodes);

        AddNodeInListByLocalPos(node.cell.cellPos, new Vector2Int(0, -1), ref nearNodes);
        AddNodeInListByLocalPos(node.cell.cellPos, new Vector2Int(0, 1), ref nearNodes);
        
        AddNodeInListByLocalPos(node.cell.cellPos, new Vector2Int(1, -1), ref nearNodes);
        AddNodeInListByLocalPos(node.cell.cellPos, new Vector2Int(1, 0), ref nearNodes);
        AddNodeInListByLocalPos(node.cell.cellPos, new Vector2Int(1, 1), ref nearNodes);
        foreach (Node nearNode in nearNodes)
        {
            Debug.Log("주변노드"+nearNode.cell.cellPos);
        }
        return nearNodes;
    }
    void AddNodeInListByLocalPos(Vector2Int anchor, Vector2Int localPos, ref List<Node> nodeList)
    {
        Debug.Log("확인"+anchor+localPos);
        Node node;
        if (TryLoadNode(anchor + localPos, out node))
        {
            Debug.Log("추가"+node.cell.cellPos);
            nodeList.Add(node);
        }
    }
    Vector2[] GetPathBetweenNodes(Node startNode, Node endNode)
    {
        if (!grid.IsCellExist(startNode.cell.cellPos)) return null;

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
            wayPoints[i] = path[i].cell.worldPos;
        }
        return wayPoints;
    }
    void OnDrawGizmos()
    {
        
    }
}
