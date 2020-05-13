using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour {
    public LayerMask unwalkableMask;
    public Vector2 gridWorldSize;
    public float nodeRadius;

    Node[,] grid;

    Vector3 worldLeftBottom;
    float nodeDiameter;
    int gridSizeX, gridSizeY;

    private void Start()
    {
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        worldLeftBottom = transform.position - (Vector3.right * gridWorldSize.x / 2) - (Vector3.forward * gridWorldSize.y / 2);
        CreateGrid();
    }

    void CreateGrid()
    {
        grid = new Node[gridSizeX, gridSizeY];
        for (int x=0; x<gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeX; y++)
            {
                Vector3 pos = worldLeftBottom + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
                bool walkable = !Physics.CheckSphere(pos, nodeRadius, unwalkableMask);
                grid[x, y] = new Node(walkable, pos, x, y);
            }
        }
    }

    //由gameobject的postion转换为地图坐标
    public Node NodeFromWorldPos(Vector3 pos)
    {
        float perX = Mathf.Clamp01((pos.x + gridWorldSize.x / 2) / gridWorldSize.x);
        float perY = Mathf.Clamp01((pos.z + gridWorldSize.y / 2) / gridWorldSize.y);

        int x = Mathf.RoundToInt((gridSizeX - 1) * perX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * perY);

        return grid[x, y];
    }

    public List<Node> GetNeighbors(Node node)
    {
        List<Node> nodes = new List<Node>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                int checkX = node.x + x;
                int checkY = node.y + y;
                if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                    nodes.Add(grid[checkX, checkY]);
            }
        }
        return nodes;
    }

    public List<Node> path;
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));
        if (grid != null)
        {
            //Node playerNode = NodeFromWorldPos(player.transform.position);
            foreach (Node n in grid)
            {
                Gizmos.color = n.waleAble ? Color.green : Color.red;
                //if (n == playerNode)
                //    Gizmos.color = Color.blue;
                if(path != null && path.Contains(n))
                {
                    Gizmos.color = Color.black;
                }
                Gizmos.DrawCube(n.worldPos, Vector3.one * (nodeDiameter - 0.1f));
            }
        }
    }
}
