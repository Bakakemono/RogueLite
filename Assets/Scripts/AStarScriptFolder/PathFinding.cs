using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding : MonoBehaviour {

    private Grid grid;
    public Transform StartPosition;
    private Transform TargetPosition;
    private GameObject Player;
    public List<Node> finalPath;
    private EnemiScript enemy;

    private void Awake()
    {
        grid = FindObjectOfType<Grid>();
        StartPosition = gameObject.transform;
    }

    private void Start()
    {
        if (FindObjectOfType<PlayerControler>())
        {
            Player = FindObjectOfType<PlayerControler>().gameObject;
        }
        TargetPosition = Player.transform;

        enemy = GetComponent<EnemiScript>();
    }

    private void Update()
    {
        if (Player == null)
        {
            Player = FindObjectOfType<PlayerControler>().gameObject;
            TargetPosition = Player.transform;
        }
    }

    public void useFindPath(Vector3 Target)
    {
        if(TargetPosition)
        FindPath(StartPosition.position, Target);
    }

    private void FindPath(Vector2 a_StartPosition, Vector2 a_TargetPosition)
    {
        Node StartNode = grid.NodeFromWorldPosition(a_StartPosition);
        Node TargetNode = grid.NodeFromWorldPosition(a_TargetPosition);

        List<Node> OpenList = new List<Node>();
        HashSet<Node> ClosedList = new HashSet<Node>();

        OpenList.Add(StartNode);

        while (OpenList.Count > 0)
        {
            Node CurrentNode = OpenList[0];

            for (int i = 0; i < OpenList.Count; i++)
            {
                if (OpenList[i].FCost < CurrentNode.FCost || OpenList[i].FCost == CurrentNode.FCost && OpenList[i].hCost < CurrentNode.hCost)
                {
                    CurrentNode = OpenList[i];
                }
            }
            OpenList.Remove(CurrentNode);
            ClosedList.Add(CurrentNode);

            if(CurrentNode == TargetNode)
            {
                GetFinalPath(StartNode, TargetNode);
            }
            
            foreach (Node NeighborNode in grid.GetNeighboringNodes(CurrentNode))
            {
                if(NeighborNode.IsWall || ClosedList.Contains(NeighborNode))
                {
                    continue;
                }
                int MoveCost = CurrentNode.gCost + GetManahattenDistance(CurrentNode, NeighborNode);

                if(MoveCost < NeighborNode.gCost || !OpenList.Contains(NeighborNode))
                {
                    NeighborNode.gCost = MoveCost;
                    NeighborNode.hCost = GetManahattenDistance(NeighborNode, TargetNode);
                    NeighborNode.Parent = CurrentNode;

                    if(!OpenList.Contains(NeighborNode))
                    {
                        OpenList.Add(NeighborNode);
                    }
                }
            }
        }
    }

    private void GetFinalPath(Node a_StartNode, Node a_EndNode)
    {
        List<Node> FinalPath = new List<Node>();
        Node CurrentNode = a_EndNode;

        while(CurrentNode != a_StartNode)
        {
            FinalPath.Add(CurrentNode);
            CurrentNode = CurrentNode.Parent;
        }
            FinalPath.Reverse();

            grid.FinalPath = FinalPath;
        finalPath = FinalPath;
    }

    private int GetManahattenDistance(Node a_NodeA, Node a_NodeB)
    {
        int ix = Mathf.Abs(a_NodeA.gridX - a_NodeB.gridX);
        int iy = Mathf.Abs(a_NodeA.gridY - a_NodeB.gridY);

        return ix + iy;
    }
}
