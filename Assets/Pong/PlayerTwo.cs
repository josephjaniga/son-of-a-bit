using UnityEngine;
using System.Collections;

public class PlayerTwo : MonoBehaviour {

	public float 	speed = 5.0f;
	
	public PongMain pm;

	// Use this for initialization
	void Start () {

		pm = GameObject.Find("Pong Game").GetComponent<PongMain>();
		reset();
	}
	
	// Update is called once per frame
	void Update () {
		
		handleMovement();
		
	}
	
	void handleMovement(){
		
		rigidbody.velocity = Vector3.zero;
			
		// ball is above
		if ( transform.position.y < pm.ball.transform.position.y )
			rigidbody.velocity = Vector3.up * speed;
		
		// ball is below center		
		if ( transform.position.y > pm.ball.transform.position.y )
			rigidbody.velocity = Vector3.down * speed;
		
	}

	public Vector3 startPosition = Vector3.zero;
	public void reset(){
		
		transform.position = startPosition;
		
	}
	
}
