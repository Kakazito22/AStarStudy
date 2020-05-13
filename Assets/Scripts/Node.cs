using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node {
    public Vector3 worldPos;
    public bool waleAble;
    public int x, y;
    /// <summary>
    /// 父节点
    /// </summary>
    public Node parent;

    /// <summary>
    /// 离起点的距离
    /// </summary>
    public int gCost;
    /// <summary>
    /// 离终点的距离
    /// </summary>
    public int hCost;

    public Node(bool _walkAble, Vector3 _pos, int _x, int _y)
    {
        this.waleAble = _walkAble;
        this.worldPos = _pos;
        this.x = _x;
        this.y = _y;
    }

    /// <summary>
    /// 寻路消耗
    /// </summary>
    public int fCost
    {
        get
        {
            return gCost + hCost;
        }
    }
}
