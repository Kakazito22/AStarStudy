using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding : MonoBehaviour {
    Grid grid;
    public Transform player, enemy;

    private void Awake()
    {
        grid = GetComponent<Grid>();
    }

    private void Update()
    {
        //PathFind(player.position, enemy.position);
    }

    /// <summary>
    /// 寻路实现算法
    /// </summary>
    public void PathFind(Vector3 startPos, Vector3 endPos)
    {

        Node startNode = grid.NodeFromWorldPos(startPos);
        Node endNode = grid.NodeFromWorldPos(endPos);

        List<Node> openSet = new List<Node>();
        List<Node> closeSet = new List<Node>();

        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            Node curNode = openSet[0];
            foreach (Node n in openSet)
            {
                if (n == curNode) continue;
                // 找出openSet里fCost最小的Node(sort, heap, priorityQueue)
                // 先比较寻路总消耗，消耗相同的情况下看离终点的距离
                if (n.fCost < curNode.fCost || n.fCost == curNode.fCost && n.hCost < curNode.hCost)
                    curNode = n;
            }
            openSet.Remove(curNode);
            closeSet.Add(curNode);

            if (curNode == endNode)
            {
                RetracePath(startNode, endNode);
                return;
            }

            List<Node> neighours = grid.GetNeighbors(curNode);
            foreach (Node n in neighours)
            {
                if (!n.waleAble || closeSet.Contains(n)) continue;
                int newNeighourDis = curNode.gCost + GetDistance(curNode, n);
                if (newNeighourDis < curNode.gCost || !openSet.Contains(n))
                {
                    n.gCost = newNeighourDis;
                    n.hCost = GetDistance(n, endNode);
                    n.parent = curNode;

                    if (!openSet.Contains(n))
                        openSet.Add(n);
                }
            }
        }
    }

    /// <summary>
    /// 计算两个Node之间的距离
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    int GetDistance(Node a, Node b)
    {
        int dstx = Mathf.Abs(a.x - b.x);
        int dsty = Mathf.Abs(a.y - b.y);
        if (dstx > dsty)
            return 14 * dsty + 10 * (dstx - dsty);
        return 14 * dstx + 10 * (dsty - dstx);
    }

    // 根据Node的parent得到最后的路径
    void RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node curNode = endNode;
        while (curNode != startNode)
        {
            path.Add(curNode);
            curNode = curNode.parent;
        }
        path.Reverse();
        grid.path = path;
    }
}
