using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PathFinding : MonoBehaviour
{

    public static PathFinding instance;
    [Header("Path Finding")]
    public Transform seeker;
    public Vector2 target;
    // ���� ���ڷ� �����Ѵ�.
    Grid grid;
    // �����Ÿ��� ���� ť ����.
    public Queue<Vector2> wayQueue = new Queue<Vector2>();

    [Header("Player Ctrl")]

    // ������ ��ȣ�ۿ� �ϰ� �������� walkable�� false �� ��.
    public static bool walkable = true;

    // �÷��̾� �̵�/ȸ�� �ӵ� �� ������ ����
    public float moveSpeed;
    public float rotSpeed;
    // ��ֹ�/NPC �Ǵܽ� ���߰� �� ����
    public float range;

    public bool isWalk;
    public bool isWalking;

    private void Awake()
    {
        instance = this;
        // ���� ����
        grid = GetComponent<Grid>();
        walkable = true;
    }
    private void Start()
    {
        // �ʱ갪 �ʱ�ȭ.

        seeker = gameObject.transform;
        isWalking = false;
        moveSpeed = 20f;
        rotSpeed = 5f;
        range = 4f;
    }

    private void FixedUpdate()
    {
        StartFindPath(seeker.position, target);
    }

    // start to target �̵�.
    public void StartFindPath(Vector2 startPos, Vector2 targetPos)
    {
        StopAllCoroutines();
        Debug.Log(startPos + " " + targetPos);
        target = targetPos;
        StartCoroutine(FindPath(startPos, targetPos));
    }

    // ��ã�� ����.
    IEnumerator FindPath(Vector2 startPos, Vector2 targetPos)
    {

        // start, target�� ��ǥ�� grid�� ������ ��ǥ�� ����.
        Node startNode = grid.NodeFromWorldPoint(startPos);
        Node targetNode = grid.NodeFromWorldPoint(targetPos);
        // target�� �����ߴ��� Ȯ���ϴ� ����.
        bool pathSuccess = false;

        if (!startNode.walkable)
            Debug.Log("Unwalkable.");

        // walkable�� targetNode�� ��� ��ã�� ����.
        if (targetNode.walkable)
        {
            // openSet, closedSet ����.
            // closedSet�� �̹� ��� ����� ����.
            // openSet�� ����� ��ġ�� �ִ� ����.
            List<Node> openSet = new List<Node>();
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
                    if (openSet[i].fCost < currentNode.fCost || openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost)
                    {
                        currentNode = openSet[i];
                    }
                }
                // openSet���� current�� �� ��, closedSet�� �߰�.
                openSet.Remove(currentNode);
                closedSet.Add(currentNode);

                // ��� ���� ��尡 ������ �� ���
                if (currentNode == targetNode)
                {
                    // seeker�� ��ġ�� ������ target�� �ƴ� ���
                    if (pathSuccess == false)
                    {
                        // wayQueue�� PATH�� �־��ش�.
                        PushWay(RetracePath(startNode, targetNode));
                    }
                    pathSuccess = true;
                    break;
                }

                // current�� �����¿� ���鿡 ���Ͽ� g,h cost�� ����Ѵ�.
                foreach (Node neighbour in grid.GetNeighbours(currentNode))
                {
                    if (!neighbour.walkable || closedSet.Contains(neighbour))
                        continue;
                    // F cost ����.
                    int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);
                    // �̿����� ���� F cost�� �̿��� G���� ª�ų�, �湮�غ� Openset�� �� ���� ���ٸ�,
                    if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                    {
                        neighbour.gCost = newMovementCostToNeighbour;
                        neighbour.hCost = GetDistance(neighbour, targetNode);
                        neighbour.parent = currentNode;

                        // openSet�� �߰�.
                        if (!openSet.Contains(neighbour))
                        {
                            openSet.Add(neighbour);
                        }
                    }
                }
            }
        }
        Debug.Log("pathsuc: " + pathSuccess);

        //yield return null;

        // ���� ã���� ���(��� �� �������) �̵���Ŵ.
        if (pathSuccess == true)
        {
            // �̵����̶�� ���� ON
            isWalking = true;
            // wayQueue�� ���� �̵���Ų��.
            while (wayQueue.Count > 0)
            {
                Debug.LogFormat("dist: {0}", Vector2.Distance((Vector2)seeker.position, wayQueue.First()));
                var dir = this.wayQueue.First() - (Vector2)this.seeker.transform.position;
                seeker.gameObject.GetComponent<Rigidbody2D>().velocity = dir.normalized * moveSpeed * 5 * Time.deltaTime;
                if (Vector2.Distance((Vector2)seeker.position, wayQueue.First()) <= 1.05f)
                {
                    Debug.Log("HEll");
                    wayQueue.Dequeue();
                }
                yield return new WaitForSeconds(0.02f);
            }
            // �̵����̶�� ���� OFF
            isWalking = false;
        }
    }

    // WayQueue�� ���ο� PATH�� �־��ش�.
    void PushWay(Vector2[] array)
    {
        wayQueue.Clear();
        foreach (Vector2 item in array)
        {
            wayQueue.Enqueue(item);
        }
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
        List<Vector2> wayPoints = new List<Vector2>();
        Vector2 directionOld = Vector2.zero;

        for (int i = 0; i < path.Count; i++)
        {
            wayPoints.Add(path[i].worldPosition);
        }
        return wayPoints.ToArray();
    }

    // custom g cost �Ǵ� �޸���ƽ ����ġ�� ����ϴ� �Լ�.
    // �Ű������� ������ ���� ���� ����� �ٲ�ϴ�.
    int GetDistance(Node nodeA, Node nodeB)
    {
        int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);
        // �밢�� - 14, �����¿� - 10.
        if (dstX > dstY)
            return 14 * dstY + 10 * (dstX - dstY);
        return 14 * dstX + 10 * (dstY - dstX);
    }
}