using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {

    private Transform player;

	// Use this for initialization
	void Start () {
        player = FindObjectOfType<PlayerControler>().transform;
	}
	
	// Update is called once per frame
	void Update () {
        while(player == null)
        {
            player = FindObjectOfType<PlayerControler>().transform;
        }
        if (player != null)
        {
            if (Vector2.Distance(transform.position, player.position) > 0.1f)
            {
                transform.position = new Vector3(Mathf.Lerp(transform.position.x, player.position.x, 0.5f), Mathf.Lerp(transform.position.y, player.position.y, 0.1f), -10.0f);
            }
        }
        
    }
}
