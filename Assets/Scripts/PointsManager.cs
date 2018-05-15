using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointsManager : MonoBehaviour {

    [SerializeField]
    private GameObject child;
    private GameObject[,] PointsArray;
    public Transform[,] PointsTransformArray;

    [SerializeField] private int _sizeX = 1;
    [SerializeField] private int _sizeY = 1;

    private void Awake()
    {
        PointsArray = new GameObject[_sizeX, _sizeY];
        PointsTransformArray = new Transform[_sizeX, _sizeY];
        for (int x = 0 ; x != _sizeX; x++)
        {
            for (int y = 0; y != _sizeY; y++)
            {
                PointsArray[x,y] = Instantiate(child, new Vector2(x, y), Quaternion.identity);
                PointsArray[x, y].transform.parent = gameObject.transform;
                PointsTransformArray[x, y] = PointsArray[x, y].transform;
            }
        }
    }

    // Use this for initialization
    void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        
    }

    public Vector2 GetSize()
    {
        return new Vector2(_sizeX, _sizeY);
    }
}
