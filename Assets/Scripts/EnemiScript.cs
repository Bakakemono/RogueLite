using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiScript : MonoBehaviour {

    public enum EnemiStatus
    {
        Patrol,
        Follow,
        Attack,
        Shearch,
    }
    private EnemiStatus _EnemiStatus = EnemiStatus.Patrol;

    private PathFinding pathFinding;
    private PlayerControler player;
    private Vector2[] PatrolPointsLists;
    private Vector2 PatrolPoint;
    private float moveSpeed = 4;
    private int choicePatrol = 0;
    private int ChoiceFollow = 0;
    private int DistanceAttack = 8;
    private int DistanceShoot = 4;

    private Transform playerPosition;
    private Vector2 LastPlayerPos;

    private Rigidbody2D body;
    private BoardCreator2 Board;
    private int choiceRoomPatrol;
    

    // Use this for initialization
    void Start () {
        body = GetComponent<Rigidbody2D>();
        playerPosition = FindObjectOfType<PlayerControler>().transform;
        pathFinding = GetComponent<PathFinding>();
        Board = FindObjectOfType<BoardCreator2>();
        choiceRoomPatrol = Mathf.RoundToInt(Random.Range(1, Board.rooms.Length));
        
    }
	
	// Update is called once per frame
	void Update () {
        if(playerPosition == null)
            playerPosition = FindObjectOfType<PlayerControler>().transform;

        if (Vector2.Distance(transform.position, playerPosition.position) < DistanceAttack)
        {
            _EnemiStatus = EnemiStatus.Attack;
        }
        else
        {
            _EnemiStatus = EnemiStatus.Patrol;
        }
        EnemiStat();
	}

    private void PathFollower()
    {
        ////PatrolPoint = pathFinding.finalPath[ChoiceFollow].Position;
        ////if(Vector2.Distance(transform.position, PatrolPoint) > 2.0f)
        ////{
        ////    pathFinding.finalPath.Remove(pathFinding.finalPath[ChoiceFollow]);
        ////    PatrolPoint =  pathFinding.finalPath[ChoiceFollow].Position;
        ////}
            
        ////body.velocity = new Vector2(PatrolPoint.x - transform.position.x, PatrolPoint.y - transform.position.y).normalized * moveSpeed;
        ////if (Vector2.Distance(transform.position, PatrolPoint) < 0.1f)
        ////{
        ////pathFinding.finalPath.Remove(pathFinding.finalPath[ChoiceFollow]);
        ////}
    }

    private void EnemiStat()
    {
        switch(_EnemiStatus)
        {
            case EnemiStatus.Patrol:
                {
                    //body.velocity = Vector2.MoveTowards(transform.position, PatrolPoint, moveSpeed * Time.deltaTime);
                    //if (Vector2.Distance(transform.position, PatrolPoint) < 0.1f)
                    //{
                    //    choicePatrol++;
                    //    PatrolPoint = PatrolPointsLists[choicePatrol];
                    //    if (choicePatrol + 1 == PatrolPointsLists.Length)
                    //    {
                    //        choicePatrol = 0;
                    //    }

                    //}
                    body.velocity = new Vector2(0, 0);
                }
                break;

            case EnemiStatus.Attack:
                {
                    Vector3 diff = playerPosition.position - transform.position;
                    diff.Normalize();

                    float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
                    transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);

                    if (Vector2.Distance(transform.position, playerPosition.position) > DistanceShoot)
                    {
                        body.velocity = new Vector2(playerPosition.position.x - transform.position.x, playerPosition.position.y - transform.position.y).normalized * moveSpeed;
                    }

                }
                break;

            case EnemiStatus.Follow:
                {
                    if (LastPlayerPos != new Vector2(playerPosition.position.x, playerPosition.position.y))
                       ChoiceFollow = 0;

                    body.velocity = Vector2.MoveTowards(transform.position, pathFinding.finalPath[ChoiceFollow].Position, moveSpeed * Time.deltaTime);
                    if (Vector2.Distance(transform.position, PatrolPoint) < 0.1f)
                    {
                        ChoiceFollow++;
                    }
                }
                break;

            case EnemiStatus.Shearch:
                {

                }
                break;
        }
    }

    private IEnumerator Shoot()
    {
        yield return new WaitForSeconds(3);
    }

    private IEnumerator CheckPlayerPos()
    {
        yield return new WaitForSeconds(0.3f);
        LastPlayerPos = playerPosition.position;
    }
}
