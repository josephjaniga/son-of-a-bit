using UnityEngine;
using System.Collections;

public class PlayerOne : MonoBehaviour {
	
	public Vector3 	direction = Vector3.zero;
	public Vector3	targetDirection = Vector3.zero;
	public float 	speed = 13.0f;
	public bool		userControlled 	= true;


	
	// Use this for initialization
	void Start () {
		reset ();
	}
	
	// Update is called once per frame
	void Update () {
		
		handleMovement();
		
	}
	
	
	void handleMovement(){
		
		rigidbody.velocity = Vector3.zero;
		
		if ( userControlled ){

			// up
			if ( Input.GetKey("w") )
				rigidbody.velocity = Vector3.up * speed;
			
			// down		
			if ( Input.GetKey("s") )
				rigidbody.velocity = Vector3.down * speed;
			
		}

		
	}

	public Vector3 startPosition = Vector3.zero;
	public void reset(){

		transform.position = startPosition;

	}
	
	
	
}
