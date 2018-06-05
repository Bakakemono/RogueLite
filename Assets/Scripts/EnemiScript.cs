using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiScript : MonoBehaviour {

    public enum EnemiStatus
    {
        Patrol,
        Follow,
        Attack,
        ReturnPatrol,
    }
    [SerializeField] private EnemiStatus _EnemiStatus = EnemiStatus.Patrol;

    private PathFinding pathFinding;
    private Vector2[] PatrolPointsLists;
    private float moveSpeed = 4;
    private int choicePatrol = 0;
    [SerializeField] private float maxDistCheckPlayer = 10.0f;
    [SerializeField] private LayerMask WallLayerMask;

    private Transform playerTransform;
    private Vector2 LastPlayerPos;


    private Rigidbody2D body;
    private int choiceRoomPatrol;
    public Vector3 enemyTarget;
    private bool findPathPatrol = false;
    private bool findPlayer = false;
    RaycastHit2D notSeePlayer;



    // Use this for initialization
    void Start () {
        body = GetComponent<Rigidbody2D>();
        playerTransform = FindObjectOfType<PlayerControler>().transform;
        pathFinding = GetComponent<PathFinding>();
        enemyTarget = playerTransform.position;
        pathFinding.useFindPath(enemyTarget);
    }
	
	// Update is called once per frame
	void Update () {
        if (playerTransform == null)
        {
            playerTransform = FindObjectOfType<PlayerControler>().transform;
            enemyTarget = playerTransform.position;
        }

        if(pathFinding.finalPath == null)
            pathFinding.useFindPath(enemyTarget);

        notSeePlayer = Physics2D.Linecast(transform.position, playerTransform.position, WallLayerMask);
        EnemiStat();
    }

    private void EnemiStat()
    {
        switch(_EnemiStatus)
        {
            case EnemiStatus.Patrol:
                {
                    findPathPatrol = false;
                    Debug.Log("patrol");
                    body.velocity = new Vector2(    PatrolPointsLists[choicePatrol].x - transform.position.x,
                                                    PatrolPointsLists[choicePatrol].y - transform.position.y).normalized
                                                    * moveSpeed;

                    Vector3 diff = new Vector3(PatrolPointsLists[choicePatrol].x, PatrolPointsLists[choicePatrol].y, 0.0f) - transform.position;
                    diff.Normalize();

                    float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
                    transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);

                    if (!notSeePlayer && Vector2.Distance(transform.position, playerTransform.position) < 8)
                    {
                        _EnemiStatus = EnemiStatus.Follow;
                    }

                    if (Vector2.Distance(transform.position, PatrolPointsLists[choicePatrol]) < 0.1f)
                    {
                        choicePatrol++;
                        if (choicePatrol == PatrolPointsLists.Length)
                        {
                            choicePatrol = 0;
                        }
                    }
                }
                break;

            case EnemiStatus.Attack:
                {
                    findPathPatrol = false;
                    Debug.Log("attack");
                    body.velocity = new Vector2(0, 0);
                    Vector3 diff = playerTransform.position - transform.position;
                    diff.Normalize();

                    float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
                    transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);

                    if (notSeePlayer || Vector2.Distance(transform.position, playerTransform.position) > 4)
                    {
                        _EnemiStatus = EnemiStatus.Follow;
                    }
                }
                break;

            case EnemiStatus.Follow:
                {
                    findPathPatrol = false;
                    Debug.Log("follow");
                    if (!notSeePlayer || Vector2.Distance(transform.position, playerTransform.position) < 3)
                    {
                        _EnemiStatus = EnemiStatus.Attack;
                    }

                    if (notSeePlayer || Vector2.Distance(transform.position, playerTransform.position) > 7)
                    {
                            StartCoroutine(LosePlayer());
                    }

                    Vector3 diff = new Vector3(pathFinding.finalPath[1].Position.x - transform.position.x, pathFinding.finalPath[1].Position.y - transform.position.y, 0.0f);
                    diff.Normalize();

                    float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
                    transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);

                    body.velocity = new Vector2(    pathFinding.finalPath[1].Position.x - transform.position.x,
                                                    pathFinding.finalPath[1].Position.y - transform.position.y).normalized * moveSpeed;

                    if (Vector2.Distance(transform.position, pathFinding.finalPath[1].Position) < 0.1f)
                    {
                        pathFinding.useFindPath(enemyTarget);
                    }
                }
                break;

            case EnemiStatus.ReturnPatrol:
                {
                    Debug.Log("Repatrol");
                    if (Vector2.Distance(transform.position, PatrolPointsLists[choicePatrol]) < 2)
                    {
                        _EnemiStatus = EnemiStatus.Patrol;
                    }

                    if (!findPathPatrol)
                    {
                        enemyTarget = PatrolPointsLists[choicePatrol];
                        pathFinding.useFindPath(enemyTarget);
                        findPathPatrol = true;
                    }

                    if (!notSeePlayer && Vector2.Distance(transform.position, playerTransform.position) < 3)
                    {
                        _EnemiStatus = EnemiStatus.Attack;
                    }

                    if(pathFinding.finalPath[0] == null || pathFinding.finalPath[1] == null)
                    {
                        _EnemiStatus = EnemiStatus.Patrol;
                    }

                    Vector3 diff = new Vector3(pathFinding.finalPath[1].Position.x - transform.position.x, pathFinding.finalPath[1].Position.y - transform.position.y, 0.0f);
                    diff.Normalize();

                    float rot_z = Mathf.Atan2(diff.y, -diff.x) * Mathf.Rad2Deg;
                    transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);

                    body.velocity = new Vector2(pathFinding.finalPath[1].Position.x - transform.position.x, pathFinding.finalPath[1].Position.y - transform.position.y).normalized * moveSpeed;

                    if (Vector2.Distance(transform.position, pathFinding.finalPath[1].Position) < 1)
                    {
                        pathFinding.finalPath.Clear();
                        pathFinding.useFindPath(enemyTarget);
                    }
                }
                break;
        }
    }

    private IEnumerator Shoot()
    {
        yield return new WaitForSeconds(3);
    }

    private IEnumerator LosePlayer()
    {
        yield return new WaitForSeconds(3);
        if (notSeePlayer || Vector2.Distance(transform.position, playerTransform.position) > 4)
            _EnemiStatus = EnemiStatus.ReturnPatrol;
    }

    public void setPatrolPoint(Vector2[] _PatrolPointsList)
    {
        PatrolPointsLists = _PatrolPointsList;
    }

    private void Fire()
    {

    }

    private void OnDrawGizmos()
    {
        for(int i = 0; i < PatrolPointsLists.Length; i++)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawCube(PatrolPointsLists[i], Vector3.one * 0.5f);
        }
    }
}
