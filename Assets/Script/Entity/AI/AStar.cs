using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AStar : MonoBehaviour
{
    [Header("Path Finding")]
    public PathFindGrid grid;
    // �����Ÿ��� ���� ť ����.
    public Queue<Vector2> wayQueue = new Queue<Vector2>();

    float[] nodeTypeCost = new float[2];

    /*
    private void FixedUpdate()
    {
        StartFindPath((Vector2)transform.position, (Vector2)Player.transform.position);
    }

    // start to target �̵�.
    public void StartFindPath(Vector2 startPos, Vector2 targetPos)
    {
        StopAllCoroutines();
        StartCoroutine(FindPath(startPos, targetPos));
    }
    */
    
    /// <summary>
    /// ��ã�� ����.
    /// </summary>
    /// <param name="startPos"></param>
    /// <param name="targetPos"></param>
    /// <returns></returns>
    IEnumerator FindPath(Vector2 startPos, Vector2 targetPos)
    {
        // start, target�� ��ǥ�� grid�� ������ ��ǥ�� ����.
        Node startNode = Node.GetNodeByWorldPos(grid, startPos);
        Node targetNode = Node.GetNodeByWorldPos(grid, targetPos);

        // target�� �����ߴ��� Ȯ���ϴ� ����.
        bool pathSuccess = false;

        if (startNode.type == NodeType.Void) Debug.Log("Unwalkable StartNode �Դϴ�.");

        // walkable�� targetNode�� ��� ��ã�� ����.
        if (startNode.type == NodeType.Void) yield break;

        /// <summary> 
        /// ����� ��ġ�� �ִ� ���� 
        /// </summary>
        List<Node> openSet = new List<Node>();

        /// <summary>
        /// �̹� ��� ����� ����
        /// </summary>
        HashSet<Node> closedSet = new HashSet<Node>();

        openSet.Add(startNode);

        // closedSet���� ���� ������ F�� ������ ��带 ������. 
        while (openSet.Count > 0)
        {
            // currentNode�� ��� �� openSet���� ���� �Ѵ�.
            Node currentNode = openSet[0];

            // ��� openSet�� ����, current���� f���� �۰ų�, h(�޸���ƽ)���� ������ �װ��� current�� ����.
            for (int i = 1; i < openSet.Count; i++)
            {
                if ((openSet[i].cost < currentNode.cost)
                    //|| (openSet[i].fCost == currentNode.fCost) && openSet[i].hCost < currentNode.hCost------------????
                    )
                    currentNode = openSet[i];
            }
            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            // ������ �� ���
            if (currentNode == targetNode)
            {
                // seeker�� ��ġ�� ������ target�� �ƴ� ���
                if (!pathSuccess)
                {
                    // wayQueue�� PATH�� �־��ش�.
                    PushWay(RetracePath(startNode, targetNode));
                }
                pathSuccess = true;
                break;
            }

            // current�� �����¿� ���鿡 ���Ͽ� g,h cost�� ����Ѵ�.
            foreach (Node neighbour in grid.GetNearNodes(currentNode))
            {
                if (nodeTypeCost[(int)neighbour.type] == -1 || closedSet.Contains(neighbour)) continue;

                // F cost ����.
                float newMovementCostToNeighbour = currentNode.pastCost + GetDistance(currentNode, neighbour);
                // �̿����� ���� F cost�� �̿��� G���� ª�ų�, �湮�غ� Openset�� �� ���� ���ٸ�,
                if (newMovementCostToNeighbour < neighbour.pastCost || !openSet.Contains(neighbour))
                {
                    neighbour.pastCost = newMovementCostToNeighbour;
                    neighbour.futureCost = GetDistance(neighbour, targetNode);
                    neighbour.parent = currentNode;

                    // openSet�� �߰�.
                    if (!openSet.Contains(neighbour)) openSet.Add(neighbour);
                }
            }

        }

        yield return null;

        // ���� ã���� ���(��� �� �������) �̵���Ŵ.
        if (pathSuccess)
        {
            /*
            // wayQueue�� ���� �̵���Ų��.
            while (this.wayQueue.Count > 0)
            {
                var dir = this.wayQueue.First() - (Vector2)this.transform.position;
                this.gameObject.GetComponent<Rigidbody2D>().velocity = dir.normalized * moveSpeed * 5 * Time.deltaTime;
                if ((Vector2)this.transform.position == this.wayQueue.First())
                {
                    Debug.Log("Dequeue");
                    this.wayQueue.Dequeue();
                }
                yield return new WaitForSeconds(0.02f);
            }
            // �̵����̶�� ���� OFF
            this.isWalking = false;
            */
        }
    }

    // WayQueue�� ���ο� PATH�� �־��ش�.
    void PushWay(Vector2[] array)
    {
        wayQueue.Clear();
        foreach (Vector2 item in array) wayQueue.Enqueue(item);
    }

    // ���� ť�� �Ųٷ� ����Ǿ������Ƿ�, �������� wayQueue�� �������ش�. 
    Vector2[] RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;
        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        path.Reverse();
        // Grid�� path�� ã�� ���� ����Ѵ�.
        grid.path = path;
        Vector2[] wayPoints = SimplifyPath(path);
        return wayPoints;
    }

    // Node���� Vector ������ ������.
    Vector2[] SimplifyPath(List<Node> path)
    {
        Vector2[] wayPoints = new Vector2[path.Count];
        for (int i = 0; i < path.Count; i++)
        {
            wayPoints[i] = path[i].worldPosition;
        }
        return wayPoints;
    }

    // custom g cost �Ǵ� �޸���ƽ ����ġ�� ����ϴ� �Լ�.
    // �Ű������� ������ ���� ���� ����� �ٲ�ϴ�.
    float GetDistance(Node nodeA, Node nodeB)
    {
        return Vector2Int.Distance(nodeA.pos, nodeB.pos);
    }
    private void OnDrawGizmos()
    {
        
    }
}
