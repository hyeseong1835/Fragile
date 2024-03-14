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
    /// ���۳�� -> ���� ��� �̵����. (g)
    /// </summary>
    public float pastCost { get; set; }
    /// <summary>
    /// ���� ��� -> ��ǥ ��� �̵����.(h)
    /// </summary>
    public float futureCost { get; set; }
    /// <summary>
    /// �̵������ ����. (f)
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
        #region ���� �ʱ�ȭ
        //StartCell
        Cell startCell;
        if (!grid.TryGetCell(grid.UnSafeGetCellPosByWorldPos(startWorldPos), out startCell))
        {
            Debug.LogWarning("������( " + startWorldPos + " )�� ��ȿ���� �ʽ��ϴ�.");
            return null;
        } //LogWarning: �������� ��ȿ���� �ʽ��ϴ�.
        if (startCell.type == CellType.Void)
        {
            Debug.LogWarning("������[ " + startCell.cellPos + " ]( " + startCell.worldPos + " )�� Void�Դϴ�.");
            return null;
        }

        //TargetCell
        Cell targetCell;
        if (!grid.TryGetCell(grid.UnSafeGetCellPosByWorldPos(targetWorldPos), out targetCell))
        {
            Debug.LogWarning("������( " + targetWorldPos + " )�� ��ȿ���� �ʽ��ϴ�.");
            return null;
        } //LogWarning: �������� ��ȿ���� �ʽ��ϴ�.
        if (startCell == targetCell) return new Vector2[] { targetCell.cellPos };

        if (targetCell.type == CellType.Void)
        {
            Debug.LogWarning("������[ " + targetCell.cellPos + " ]( " + targetCell.worldPos + " )�� Void�Դϴ�.");
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

            // �� ���� ���� ���� ����� ���� ���� ��� ���� => currentNode
            for (int i = 1; i < openSet.Count; i++)
            {
                // ���� => ��� ��ȯ
                if (currentNode.pastCost == -1 && currentNode.futureCost == -1) return GetPathBetweenNodes(startNode, targetNode);

                if (openSet[i].cost <= currentNode.cost
                    && openSet[i].futureCost < currentNode.futureCost)
                    currentNode = openSet[i];
            }

            Debug.Log("����: " + currentNode.cell.cellPos);
            debugMap[currentNode.cell.cellPos.x, currentNode.cell.cellPos.y] = moveColor;

            // �̿� ������ �߰� �� �˻�
            foreach (Node nearNode in GetNearNode(currentNode))
            {
                if ((nearNode.cell.type == CellType.Void) || closedSet.Contains(nearNode)) continue;
                Debug.Log("�߰�"+nearNode.cell.cellPos);
                float movementCost = currentNode.pastCost + Vector2Int.Distance(currentNode.cell.cellPos, nearNode.cell.cellPos);

                // ����� �����ϴ� �������� �̵����� ��
                if (movementCost < nearNode.pastCost || !openSet.Contains(nearNode))
                {
                    nearNode.pastCost = movementCost;
                    nearNode.futureCost = Vector2Int.Distance(nearNode.cell.cellPos, targetNode.cell.cellPos);
                    nearNode.parent = currentNode;

                    // openSet�� �߰�.
                    if (!openSet.Contains(nearNode))
                    {
                        openSet.Add(nearNode);
                        Debug.Log("�߰�: " + nearNode.cell.cellPos);
                        debugMap[nearNode.cell.cellPos.x, nearNode.cell.cellPos.y] = openColor;
                    }
                    else Debug.Log("����: " + nearNode.cell.cellPos);
                }
            }
            // currentNode�� ����.
            openSet.Remove(currentNode);
            closedSet.Add(currentNode);
            debugMap[currentNode.cell.cellPos.x, currentNode.cell.cellPos.y] = closeColor;
        }
        Debug.LogWarning("���� ã�� �� �����ϴ�.");
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
            Debug.Log("�ֺ����"+nearNode.cell.cellPos);
        }
        return nearNodes;
    }
    void AddNodeInListByLocalPos(Vector2Int anchor, Vector2Int localPos, ref List<Node> nodeList)
    {
        Debug.Log("Ȯ��"+anchor+localPos);
        Node node;
        if (TryLoadNode(anchor + localPos, out node))
        {
            Debug.Log("�߰�"+node.cell.cellPos);
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
