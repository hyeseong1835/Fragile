using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static UnityEditor.PlayerSettings;

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
}


public class AStar : MonoBehaviour
{
    [Header("Path Finding")]
    public PathFindGrid grid;

    Color[,] debugMap;
    [SerializeField] Color defaultColor = new Color(1, 1, 1, 0);
    [SerializeField] Color openColor;
    [SerializeField] Color startColor;
    [SerializeField] Color targetColor;
    [SerializeField] Color closeColor;
    [SerializeField] Color moveColor;
    public Vector2[] debugPath;

    [DisableInEditorMode]
    [Button(ButtonStyle.Box)]
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
        if (startCell == targetCell)
        {
            return new Vector2[] { startCell.cellPos, targetCell.cellPos };
        }
        if (targetCell.type == CellType.Void)
        {
            Debug.LogWarning("������[ " + targetCell.cellPos + " ]( " + targetCell.worldPos + " )�� Void�Դϴ�.");
            return null;
        }
        
        Node startNode = new Node(startCell, null, 0, Vector2Int.Distance(startCell.cellPos, targetCell.cellPos));
        Node targetNode = new Node(targetCell, null, -1, -1);

        debugMap = new Color[grid.cellCount.x, grid.cellCount.y];
        for(int x = 0; x < grid.cellCount.x; x++)
        {
            for(int y = 0; y < grid.cellCount.y; y++)
            {
                debugMap[x, y] = defaultColor;
            }
        }
        debugMap[startCell.cellPos.x, startCell.cellPos.y] = startColor;
        debugMap[targetCell.cellPos.x, targetCell.cellPos.y] = targetColor;

        List<Node> openSet = new List<Node>(){ startNode };
        HashSet<Node> closedSet = new HashSet<Node>();
        #endregion

        #region �� ã��
        Node curNode;
        int curNodeIndex = 0;
        while (openSet.Count > 0)
        {
            #region �������� ���� ���� ��� => curNode
            curNode = openSet[0];

            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].cost <= curNode.cost && openSet[i].futureCost < curNode.futureCost)
                {
                    curNode = openSet[i];
                    curNodeIndex = i;
                }
            }
            if(debugMap[curNode.cell.cellPos.x, curNode.cell.cellPos.y] == defaultColor) 
                debugMap[curNode.cell.cellPos.x, curNode.cell.cellPos.y] = moveColor;
            #endregion

            #region curNode�� �̿� ��� �˻�
            List<Node> nearNodes = new List<Node>();

            AddNodeInListByLocalPos(curNode.cell.cellPos, new Vector2Int(-1, -1), ref nearNodes);
            AddNodeInListByLocalPos(curNode.cell.cellPos, new Vector2Int(-1, 0), ref nearNodes);
            AddNodeInListByLocalPos(curNode.cell.cellPos, new Vector2Int(-1, 1), ref nearNodes);

            AddNodeInListByLocalPos(curNode.cell.cellPos, new Vector2Int(0, -1), ref nearNodes);
            AddNodeInListByLocalPos(curNode.cell.cellPos, new Vector2Int(0, 1), ref nearNodes);

            AddNodeInListByLocalPos(curNode.cell.cellPos, new Vector2Int(1, -1), ref nearNodes);
            AddNodeInListByLocalPos(curNode.cell.cellPos, new Vector2Int(1, 0), ref nearNodes);
            AddNodeInListByLocalPos(curNode.cell.cellPos, new Vector2Int(1, 1), ref nearNodes);

            foreach (Node nearNode in nearNodes)
            {
                #region �̿� ��尡 �������� ��
                if (nearNode == targetNode)
                {
                    List<Node> path = new List<Node>();

                    Debug.Log("�� ã��: " + nearNode.cell.cellPos);
                    targetNode.parent = curNode;

                    foreach (Node node in openSet)
                    {
                        node.cell.node = null;
                    }
                    while (targetNode != startNode)
                    {
                        path.Add(targetNode);
                        targetNode = targetNode.parent;
                    }
                    Vector2[] wayPoints = new Vector2[path.Count];
                    for (int i = path.Count -1; i >= 0; i--)
                    {
                        wayPoints[i] = path[i].cell.worldPos;
                    }
                    debugPath = wayPoints;
                    return wayPoints;
                }
                #endregion
                
                #region ���� Set ����
                if (nearNode.cell.type == CellType.Void)
                {
                    closedSet.Add(nearNode);
                }
                if (closedSet.Contains(nearNode)) continue;

                // futureCost�� AddNodeInListByLocalPos���� curNode���� nearNode ������ �Ÿ��� ����
                nearNode.pastCost = curNode.pastCost + nearNode.futureCost;

                nearNode.futureCost = Vector2Int.Distance(nearNode.cell.cellPos, targetNode.cell.cellPos); //���� �÷�???????????????????/

                nearNode.parent = curNode;

                if (openSet.Contains(nearNode)) continue;

                openSet.Add(nearNode);
                #endregion
                if (debugMap[curNode.cell.cellPos.x, curNode.cell.cellPos.y] == defaultColor)
                    debugMap[nearNode.cell.cellPos.x, nearNode.cell.cellPos.y] = openColor;
            }
            #endregion

            #region curNode ����.
            openSet.RemoveAt(curNodeIndex);
            
            curNode.cell.node = null;
            closedSet.Add(curNode);

            if (debugMap[curNode.cell.cellPos.x, curNode.cell.cellPos.y] == openColor)
                debugMap[curNode.cell.cellPos.x, curNode.cell.cellPos.y] = closeColor;
            #endregion
        }
        #endregion

        #region ���������� ���� ���� ���� ��
        Debug.LogWarning("���� ã�� �� �����ϴ�.");
        foreach (Node node in openSet)
        {
            node.cell.node = null;
        }
        return null;
        #endregion
    }
    void AddNodeInListByLocalPos(Vector2Int anchor, Vector2Int localPos, ref List<Node> nodeList)
    {
        Vector2Int cellPos = anchor + localPos;
        if (!grid.IsCellExist(cellPos)) return;

        Node node = grid.grid[cellPos.x, cellPos.y].node;
        
        if (node == null)
        {
            if (localPos.x * localPos.y == 0) node = new Node(grid.grid[cellPos.x, cellPos.y], null, -1, 1);
            else node = new Node(grid.grid[cellPos.x, cellPos.y], null, -1, 1.41421356f);
            nodeList.Add(node);
            return;
        }
        else
        {
            if (localPos.x * localPos.y == 0) node.futureCost = 1;
            else node.futureCost = 1.41421356f;
            nodeList.Add(node);
            return;
        }
    }
    void OnDrawGizmos()
    {
        
    }
    void OnDrawGizmosSelected()
    {
        if (debugMap != null)
        {
            for (int x = 0; x < grid.cellCount.x; x++)
            {
                for (int y = 0; y < grid.cellCount.y; y++)
                {
                    Gizmos.color = debugMap[x, y];
                    Gizmos.DrawCube(grid.GetWorldPosByCellPos(new Vector2Int(x, y)), grid.cellSize * 0.5f);
                }
            }
        }
        if (debugPath != null)
        {
            foreach (Vector2 point in debugPath)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawCube(point, grid.cellSize * 0.1f);
            }
        }
    }
}
