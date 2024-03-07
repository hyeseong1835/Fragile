using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public bool walkable;
    public Vector2 worldPosition;
    public int gridX;
    public int gridY;

    public int gCost;
    public int hCost;
    // �� �������� ���� parent����.
    public Node parent;

    // F cost ��� �Ӽ�.
    public int fCost { get { return gCost + hCost; } }

    // Node ������.
    public Node(bool walkable, Vector2 worldPos, int gridX, int gridY)
    {
        this.walkable = walkable;
        this.worldPosition = worldPos;
        this.gridX = gridX;
        this.gridY = gridY;
    }
}

public class PathFindGrid : MonoBehaviour
{
    public bool displayGridGizmos;
    // �÷��̾��� ��ġ
    public Transform monster;
    // ��ֹ� ���̾�
    public LayerMask OBSTACLE;
    // ȭ���� ũ��
    public Vector2 gridWorldSize;
    // ������
    public float nodeRadius;
    Node[,] grid;

    // ������ ����
    float nodeDiameter;
    // x,y�� ������
    int gridSizeX, gridSizeY;

    private void Awake()
    {
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        // ���� ����
        CreateGrid();
    }

    // A*���� ����� PATH.
    [SerializeField]
    public List<Node> path;

    // Scene view ��¿� �����.
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector2(gridWorldSize.x, gridWorldSize.y));
        if (grid != null)
        {
            Node playerNode = NodeFromWorldPoint(monster.position);
            foreach (Node n in grid)
            {
                Gizmos.color = (n.walkable) ? new Color(1, 1, 1, 0.3f) : new Color(1, 0, 0, 0.3f);
                if (n.walkable == false)

                    if (path != null)
                    {
                        if (path.Contains(n))
                        {
                            Gizmos.color = new Color(0, 0, 0, 0.3f);
                            Debug.Log("?");
                        }
                    }
                if (playerNode == n) Gizmos.color = new Color(0, 1, 1, 0.3f);
                Gizmos.DrawCube(n.worldPosition, Vector2.one * (nodeDiameter - 0.1f));
            }
        }
    }

    // ���� ���� �Լ�
    void CreateGrid()
    {
        grid = new Node[gridSizeX, gridSizeY];
        // ���� ������ ���� ���ϴܺ��� ����. transform�� ���� �߾ӿ� ��ġ�Ѵ�. 
        // �̿� x�� y��ǥ�� �ݹ� �� ����, �Ʒ������� �Ű��ش�.
        Vector2 worldBottomLeft = (Vector2)transform.position - Vector2.right * gridWorldSize.x / 2 - Vector2.up * gridWorldSize.y / 2;

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector2 worldPoint = worldBottomLeft + Vector2.right * (x * nodeDiameter + nodeRadius) + Vector2.up * (y * nodeDiameter + nodeRadius);
                // �ش� ���ڰ� Walkable���� �ƴ��� �Ǵ�.
                bool walkable = !(Physics2D.OverlapCircle(worldPoint, nodeRadius, OBSTACLE));
                // ��� �Ҵ�.
                grid[x, y] = new Node(walkable, worldPoint, x, y);
            }
        }
    }

    // node ���� �¿� �밢 ��带 ��ȯ�ϴ� �Լ�.
    public List<Node> GetNeighbours(Node node)
    {
        List<Node> neighbours = new List<Node>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0) continue;

                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                {
                    if (!grid[node.gridX, checkY].walkable && !grid[checkX, node.gridY].walkable) continue;
                    if (!grid[node.gridX, checkY].walkable || !grid[checkX, node.gridY].walkable) continue;

                    neighbours.Add(grid[checkX, checkY]);
                }
            }
        }

        return neighbours;
    }


    // �Է����� ���� ������ǥ�� node��ǥ��� ��ȯ.
    public Node NodeFromWorldPoint(Vector2 worldPosition)
    {
        float percentX = (worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x;
        float percentY = (worldPosition.y + gridWorldSize.y / 2) / gridWorldSize.y;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);
        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);

        return grid[x, y];
    }
}