using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointsManager : MonoBehaviour {

    [SerializeField]
    private GameObject child;
    private GameObject[,] PointsArray;
    public Vector2[,] PointsTransformArray;
    public List<Vector2Int> PathPoints;
    private int count = 0;
    public Vector2Int targetPos;

    [SerializeField] private int _sizeX = 1;
    [SerializeField] private int _sizeY = 1;
    [SerializeField]
    private MazeGeneratorType _mazeGeneratorType = MazeGeneratorType.DFS;

    public enum MazeGeneratorType
    {
        DFS,
        BFS,
        Kruskal
    }

    private void Awake()
    {
        PointsArray = new GameObject[_sizeX, _sizeY];
        PointsTransformArray = new Vector2[_sizeX, _sizeY];
        for (int x = 0 ; x != _sizeX; x++)
        {
            for (int y = 0; y != _sizeY; y++)
            {
                PointsArray[x,y] = Instantiate(child, new Vector2(x, y), Quaternion.identity);
                PointsArray[x, y].transform.parent = gameObject.transform;
                PointsTransformArray[x, y] = PointsArray[x, y].transform.position;
            }
        }
        MazeGenerator();
        
    }

    // Use this for initialization
    void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        for (int i = 1; i < PathPoints.Count; i++)
        {
            Debug.DrawLine(new Vector3(PathPoints[i - 1].x, PathPoints[i - 1].y, 0.0f), new Vector3(PathPoints[i].x, PathPoints[i].y, 0.0f), Color.red);
        }

    }

    public Vector2Int GetSize()
    {
        return new Vector2Int(_sizeX, _sizeY);
    }

    public void MazeGenerator()
    {
        bool[,] visitedChunk = new bool[_sizeX, _sizeY];
        Vector2Int[,] previousPos = new Vector2Int[_sizeX, _sizeY];
        Vector2Int currentPos = Vector2Int.zero;
        targetPos = new Vector2Int(_sizeX - 1, _sizeY - 1);
        List<Vector2Int> nextPositions = new List<Vector2Int>();
        List<Vector2Int> path = new List<Vector2Int>();


        switch (_mazeGeneratorType)
        {
            case MazeGeneratorType.DFS:
            {
                visitedChunk[currentPos.x, currentPos.y] = true;
                while (currentPos != targetPos)
                {
                    //Check neighbors
                    for (int dx = -1; dx <= 1; dx++)
                    {
                        for (int dy = -1; dy <= 1; dy++)
                        {
                            if (dx == dy || dx == -dy)
                            {
                                continue;
                            }
                            Vector2Int neighborPos = currentPos + new Vector2Int(dx, dy);
                            if (neighborPos.x < 0 ||
                                neighborPos.y < 0 ||
                                neighborPos.x == _sizeX ||
                                neighborPos.y == _sizeY)
                            {
                                continue;
                            }

                            if (visitedChunk[neighborPos.x, neighborPos.y])
                            {
                                continue;
                            }

                            nextPositions.Add(neighborPos);

                        }
                    }
                    Debug.Log("Current Position: " + currentPos);
                    if (nextPositions.Count != 0)
                    {
                        //Random heuristic with neighbors of current pos
                        var newCurrentPos = nextPositions.Random();
                        previousPos[newCurrentPos.x, newCurrentPos.y] = currentPos;
                        currentPos = newCurrentPos;
                        visitedChunk[currentPos.x, currentPos.y] = true;
                        nextPositions.Clear();
                    }
                    else
                    {
                        currentPos = previousPos[currentPos.x, currentPos.y];
                    }
                }
            }
            break;

            //case MazeGeneratorType.BFS:
            //    {
            //        visitedChunk[currentPos.x, currentPos.y] = true;
            //        while (currentPos != targetPos)
            //        {
            //            //Check neighbors
            //            List<Vector2Int> tmpNeighbors = new List<Vector2Int>();
            //            for (int dx = -1; dx <= 1; dx++)
            //            {
            //                for (int dy = -1; dy <= 1; dy++)
            //                {
            //                    if (dx == dy || dx == -dy)
            //                    {
            //                        continue;
            //                    }
            //                    Vector2Int neighborPos = currentPos + new Vector2Int(dx, dy);
            //                    if (neighborPos.x < 0 ||
            //                        neighborPos.y < 0 ||
            //                        neighborPos.x == _sizeX ||
            //                        neighborPos.y == _sizeY)
            //                    {
            //                        continue;
            //                    }

            //                    if (visitedChunk[neighborPos.x, neighborPos.y])
            //                    {
            //                        continue;
            //                    }

            //                    if (nextPositions.Contains(neighborPos))
            //                    {
            //                        continue;
            //                    }

            //                    tmpNeighbors.Add(neighborPos);

            //                }
            //            }
            //            //Random neighbor choose heuristic
            //            while (tmpNeighbors.Count > 0)
            //            {
            //                Vector2Int nextPos = tmpNeighbors.Random();
            //                nextPositions.Add(nextPos);
            //                tmpNeighbors.Remove(nextPos);
            //            }
            //            Debug.Log("Current Position: " + currentPos);
            //            if (nextPositions.Count != 0)
            //            {
            //                var newCurrentPos = nextPositions.First();
            //                previousPos[newCurrentPos.x, newCurrentPos.y] = currentPos;
            //                currentPos = newCurrentPos;
            //                visitedChunk[currentPos.x, currentPos.y] = true;
            //                nextPositions.Remove(currentPos);
            //            }
            //            else
            //            {
            //                break;
            //            }
            //        }
            //    }
            //    break;
        }
        currentPos = targetPos;
        path.Add(targetPos);
        while (currentPos != Vector2Int.zero)
        {
            var parentPos = previousPos[currentPos.x, currentPos.y];
            path.Add(parentPos);
            currentPos = parentPos;
            count++;
            Debug.Log("PATH Current Position: " + currentPos);
        }
        path.Reverse();
        PathPoints = path;
        
        {
            //Debug.LogWarning("PATH: ");
            //for (int i = 1; i < path.Count; i++)
            //{
            //    Vector2Int deltaPos = path[i] - path[i - 1];
            //    Debug.DrawLine(new Vector3(path[i - 1].x, path[i - 1].y, 0.0f), new Vector3(path[i].x, path[i].y, 0.0f), Color.red);
            //    Debug.LogWarning("FROM: " + path[i - 1] + " TO: " + path[i]);
            //}
        }
    }
}
