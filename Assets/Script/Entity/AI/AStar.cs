using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AStar : MonoBehaviour
{
    [Header("Path Finding")]
    public PathFindGrid grid;
    // 남은거리를 넣을 큐 생성.
    public Queue<Vector2> wayQueue = new Queue<Vector2>();

    float[] nodeTypeCost = new float[2];

    /*
    private void FixedUpdate()
    {
        StartFindPath((Vector2)transform.position, (Vector2)Player.transform.position);
    }

    // start to target 이동.
    public void StartFindPath(Vector2 startPos, Vector2 targetPos)
    {
        StopAllCoroutines();
        StartCoroutine(FindPath(startPos, targetPos));
    }
    */
    
    /// <summary>
    /// 길찾기 로직.
    /// </summary>
    /// <param name="startPos"></param>
    /// <param name="targetPos"></param>
    /// <returns></returns>
    IEnumerator FindPath(Vector2 startPos, Vector2 targetPos)
    {
        // start, target의 좌표를 grid로 분할한 좌표로 지정.
        Node startNode = Node.GetNodeByWorldPos(grid, startPos);
        Node targetNode = Node.GetNodeByWorldPos(grid, targetPos);

        // target에 도착했는지 확인하는 변수.
        bool pathSuccess = false;

        if (startNode.type == NodeType.Void) Debug.Log("Unwalkable StartNode 입니다.");

        // walkable한 targetNode인 경우 길찾기 시작.
        if (startNode.type == NodeType.Void) yield break;

        /// <summary> 
        /// 계산할 가치가 있는 노드들 
        /// </summary>
        List<Node> openSet = new List<Node>();

        /// <summary>
        /// 이미 계산 고려한 노드들
        /// </summary>
        HashSet<Node> closedSet = new HashSet<Node>();

        openSet.Add(startNode);

        // closedSet에서 가장 최저의 F를 가지는 노드를 빼낸다. 
        while (openSet.Count > 0)
        {
            // currentNode를 계산 후 openSet에서 빼야 한다.
            Node currentNode = openSet[0];

            // 모든 openSet에 대해, current보다 f값이 작거나, h(휴리스틱)값이 작으면 그것을 current로 지정.
            for (int i = 1; i < openSet.Count; i++)
            {
                if ((openSet[i].cost < currentNode.cost)
                    //|| (openSet[i].fCost == currentNode.fCost) && openSet[i].hCost < currentNode.hCost------------????
                    )
                    currentNode = openSet[i];
            }
            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            // 목적지 인 경우
            if (currentNode == targetNode)
            {
                // seeker가 위치한 지점이 target이 아닌 경우
                if (!pathSuccess)
                {
                    // wayQueue에 PATH를 넣어준다.
                    PushWay(RetracePath(startNode, targetNode));
                }
                pathSuccess = true;
                break;
            }

            // current의 상하좌우 노드들에 대하여 g,h cost를 고려한다.
            foreach (Node neighbour in grid.GetNearNodes(currentNode))
            {
                if (nodeTypeCost[(int)neighbour.type] == -1 || closedSet.Contains(neighbour)) continue;

                // F cost 생성.
                float newMovementCostToNeighbour = currentNode.pastCost + GetDistance(currentNode, neighbour);
                // 이웃으로 가는 F cost가 이웃의 G보다 짧거나, 방문해볼 Openset에 그 값이 없다면,
                if (newMovementCostToNeighbour < neighbour.pastCost || !openSet.Contains(neighbour))
                {
                    neighbour.pastCost = newMovementCostToNeighbour;
                    neighbour.futureCost = GetDistance(neighbour, targetNode);
                    neighbour.parent = currentNode;

                    // openSet에 추가.
                    if (!openSet.Contains(neighbour)) openSet.Add(neighbour);
                }
            }

        }

        yield return null;

        // 길을 찾았을 경우(계산 다 끝난경우) 이동시킴.
        if (pathSuccess)
        {
            /*
            // wayQueue를 따라 이동시킨다.
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
            // 이동중이라는 변수 OFF
            this.isWalking = false;
            */
        }
    }

    // WayQueue에 새로운 PATH를 넣어준다.
    void PushWay(Vector2[] array)
    {
        wayQueue.Clear();
        foreach (Vector2 item in array) wayQueue.Enqueue(item);
    }

    // 현재 큐에 거꾸로 저장되어있으므로, 역순으로 wayQueue를 뒤집어준다. 
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
        // Grid의 path에 찾은 길을 등록한다.
        grid.path = path;
        Vector2[] wayPoints = SimplifyPath(path);
        return wayPoints;
    }

    // Node에서 Vector 정보만 빼낸다.
    Vector2[] SimplifyPath(List<Node> path)
    {
        Vector2[] wayPoints = new Vector2[path.Count];
        for (int i = 0; i < path.Count; i++)
        {
            wayPoints[i] = path[i].worldPosition;
        }
        return wayPoints;
    }

    // custom g cost 또는 휴리스틱 추정치를 계산하는 함수.
    // 매개변수로 들어오는 값에 따라 기능이 바뀝니다.
    float GetDistance(Node nodeA, Node nodeB)
    {
        return Vector2Int.Distance(nodeA.pos, nodeB.pos);
    }
    private void OnDrawGizmos()
    {
        
    }
}
