using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour {


    public Vector2 GridWorldSize;
    public float NodesRadius;
    public float Distance;
    private BoardCreator2 Board;

    Node[,] grid;

    public List<Node> FinalPath;

    private float NodeDiameter;
    private int gridSizeX, gridSizeY;

    private void Start()
    {
        Board = FindObjectOfType<BoardCreator2>();
        GridWorldSize = new Vector2(Board.columns, Board.rows);
        NodeDiameter = NodesRadius * 2;
        gridSizeX = Board.columns;
        gridSizeY = Board.rows;
        CreateGrid();
    }

    private void CreateGrid()
    {
        grid = new Node[gridSizeX, gridSizeY];
        Vector2 BottomLeft = transform.position - Vector3.right * GridWorldSize.x / 2 - Vector3.up * GridWorldSize.y / 2;

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeX; y++)
            {
                Vector2 worldPoint = Board.TilesPosition[x][y];
                bool Wall = true;

                if (Board.tiles[x][y] == BoardCreator2.TileType.Floor || Board.tiles[x][y] == BoardCreator2.TileType.playerSpawn)
                {
                    Wall = false;
                }
                grid[x, y] = new Node(Wall, worldPoint, x, y);
            }
        }
    }

    public Node NodeFromWorldPosition(Vector2 a_WorldPosition)
    {

        int x = Mathf.RoundToInt((a_WorldPosition.x + GridWorldSize.x) /2);
        int y = Mathf.RoundToInt((a_WorldPosition.y + GridWorldSize.y) /2);

        return grid[x, y];
    }

    public List<Node> GetNeighboringNodes(Node a_Node)
    {
        List<Node> NeighborList = new List<Node>();
        int xCheck;
        int yCheck;

        //Right
        xCheck = a_Node.gridX + 1;
        yCheck = a_Node.gridY;
        if(xCheck >= 0 && xCheck < gridSizeX)
        {
            if (yCheck >= 0 && yCheck < gridSizeY)
            {
                NeighborList.Add(grid[xCheck, yCheck]);
            }
        }

        //Left
        xCheck = a_Node.gridX - 1;
        yCheck = a_Node.gridY;
        if (xCheck >= 0 && xCheck < gridSizeX)
        {
            if (yCheck >= 0 && yCheck < gridSizeY)
            {
                NeighborList.Add(grid[xCheck, yCheck]);
            }
        }

        //Up
        xCheck = a_Node.gridX;
        yCheck = a_Node.gridY + 1;
        if (xCheck >= 0 && xCheck < gridSizeX)
        {
            if (yCheck >= 0 && yCheck < gridSizeY)
            {
                NeighborList.Add(grid[xCheck, yCheck]);
            }
        }

        //Down
        xCheck = a_Node.gridX;
        yCheck = a_Node.gridY - 1;
        if (xCheck >= 0 && xCheck < gridSizeX)
        {
            if (yCheck >= 0 && yCheck < gridSizeY)
            {
                NeighborList.Add(grid[xCheck, yCheck]);
            }
        }

        return NeighborList;
    }


    private void OnDrawGizmos()
    {
        if(Board != null)
        Gizmos.DrawWireCube(transform.position, new Vector3(Board.columns * 2, Board.rows * 2, 1));

        if(grid != null)
        {
            foreach(Node n in grid)
            {
                if(!n.IsWall)
                {
                    Gizmos.color = Color.white;
                }
                else
                {
                    Gizmos.color = Color.yellow;
                }

                if(FinalPath != null)
                {
                    if (FinalPath.Contains(n))
                    {
                        Gizmos.color = Color.red;
                    }
                }
                Gizmos.DrawCube(n.Position, Vector3.one * 0.5f);
            }
        }
    }
}
