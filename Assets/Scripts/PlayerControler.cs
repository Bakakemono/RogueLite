using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControler : MonoBehaviour {

    private Rigidbody2D body;
    private float horizontalInputMove;
    private float verticalInputMove;
    private float horizontalInputArrow;
    private float verticalInputArrow;
    [SerializeField]
    private float speed = 1;
    [SerializeField] private Vector2 aim = new Vector2 (0,1);

	// Use this for initialization
	void Start () {
        body = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
        horizontalInputMove = Input.GetAxis("Horizontal");
        verticalInputMove = Input.GetAxis("Vertical");
        horizontalInputArrow = Input.GetAxis("HorizontalArrow");
        verticalInputArrow = Input.GetAxis("VerticalArrow");
        Movement();

        Vector3 pos = Camera.main.WorldToScreenPoint(transform.position);
        float angle = Mathf.Atan2(verticalInputArrow, horizontalInputArrow) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    private void Movement()
    {
        Vector2 position = transform.position;
        Vector2 movement = (Vector2.up * verticalInputMove + Vector2.right * horizontalInputMove).normalized * speed * Time.deltaTime;

        body.MovePosition(body.position + movement);
    }
}
