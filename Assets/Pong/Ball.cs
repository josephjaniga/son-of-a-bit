using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Ball : MonoBehaviour {

	public PongMain pm;

	public Vector3 	direction = Vector3.zero;
	public Vector3	targetDirection = Vector3.zero;
	public float	startingSpeed = 11.0f;
	public float 	speed = 11.0f;
	public float	maxSpeed = 44.0f;
	
	// Use this for initialization
	void Start () {

		pm = GameObject.Find("Pong Game").GetComponent<PongMain>();
		transform.position = startPosition;

	}
	
	// Update is called once per frame
	void Update () {
		
		handleMovement();

	}
	
	
	void handleMovement(){
		
		this.rigidbody.velocity = targetDirection * speed;

	}
	
	void OnCollisionEnter(UnityEngine.Collision col){
		
		List<string> h_walls = new List<string> {"North Wall", "South Wall"};
		List<string> v_walls = new List<string> {"East Wall", "West Wall"};

		List<string> players = new List<string> {"Player 1", "Player 2"};


		if ( h_walls.Contains( col.gameObject.name ) ){
			targetDirection.y *= -1.0f;
		}

		if ( v_walls.Contains( col.gameObject.name ) ){
			targetDirection.x *= -1.0f;

			if ( col.gameObject.name == "East Wall" ){
				pm.updateScore( pm.p1_score + 1, pm.p2_score );
			}
			if ( col.gameObject.name == "West Wall" ){
				pm.updateScore( pm.p1_score, pm.p2_score + 1 );
			}

			pm.reset();

		}

		if ( players.Contains( col.gameObject.name ) ){
			targetDirection.x *= -1.0f;

			if ( speed * 1.1f <= maxSpeed ){
				speed *= 1.1f;
			} else {
				speed = maxSpeed;
			}

		}

	}

	public Vector3 startPosition = Vector3.zero;
	public void reset(){
		
		transform.position = startPosition;
		speed = startingSpeed;

	}
	
	
	
}
