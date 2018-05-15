using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiScript : MonoBehaviour {

    private PointsManager pointsList;
    [SerializeField] private int patrolPointsNumb = 100;
    private Transform[] PatrolPointsLists;
    private Transform PatrolPoints;
    private float moveSpeed = 10;
    private int choice = 0;

    // Use this for initialization
    void Start () {
        PatrolPointsLists = new Transform[patrolPointsNumb];
        pointsList = FindObjectOfType<PointsManager>();
        for (int i = 0; i < patrolPointsNumb; i++)
        {
            int x = Mathf.RoundToInt((1241 * Random.value + 82455) % (pointsList.GetSize().x - 1));
            int y = Mathf.RoundToInt((7433 * Random.value + 28561) % (pointsList.GetSize().y - 1));
            Debug.LogWarning(x + " / " + y);
            PatrolPointsLists[i] = pointsList.PointsTransformArray[x, y];
            
            Debug.Log(PatrolPointsLists[i].position);
        }
        PatrolPoints = PatrolPointsLists[choice];
    }
	
	// Update is called once per frame
	void Update () {
        transform.position = Vector2.MoveTowards(transform.position, PatrolPoints.position, moveSpeed * Time.deltaTime);
        if (Vector2.Distance(transform.position, PatrolPoints.position) < 0.3f)
        {
            choice++;
            if (choice == PatrolPointsLists.Length)
                choice = 0;
            
            PatrolPoints = PatrolPointsLists[choice];
        }
	}

    private float modulo(float val, float mod)
    {
        bool finish = false;
        float _val = val;
        while (!finish)
        {
            if (_val < mod)
            {
                finish = true;
                return _val;
            }
            else
            {
                _val -= mod;
            }

        }
        return 0.0f;
    }
}
