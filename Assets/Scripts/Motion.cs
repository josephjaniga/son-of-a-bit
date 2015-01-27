using UnityEngine;
using System.Collections;

public class Motion : MonoBehaviour {

	public Vector3 	direction = Vector3.zero;
	public float 	speed = 1.0f;
	public bool		userControlled 	= false;
	public bool 	shouldLock = true;
	public bool		flight = true;

	private bool	isGrounded = false;

	private bool 	canJump = true;
	
	public Vector3 rotation = Vector3.zero;

	public Bit bit;

	public Joystick leftJoystick;

	// Use this for initializationW
	void Start () {
	
		bit = gameObject.GetComponent<Bit>();

		if ( GameObject.Find("LeftJoystick") != null ){
			leftJoystick = GameObject.Find("LeftJoystick").GetComponent<Joystick>();
		}

	}

	// Update is called once per frame
	void Update () {
		
		handleMovement();

	}

	void OnCollisionEnter(Collision hit) {
//		if( hit.gameObject.tag == "Floor" ) {
//			isGrounded = true;
//		}

		isGrounded = true;
	} 

	void handleMovement(){


		float tempSpeed = speed;
		if ( bit.statManager != null ){
			tempSpeed = bit.statManager.cMovementSpeed;
		}

		if ( shouldLock ){
			rigidbody.velocity = Vector3.zero;
		}

		if ( userControlled ){

			float x = 0.0f;
			float y = 0.0f;

			if ( flight ){
				
				// left
				if ( Input.GetKey("a") )
					x = -1f * tempSpeed;
				
				// right
				if ( Input.GetKey("d") )
					x = 1f * tempSpeed;

				// up
				if ( Input.GetKey("w") )
					y = 1f * tempSpeed;
				
				// down		
				if ( Input.GetKey("s") )
					y = -1f * tempSpeed;

				// joystick
				if ( leftJoystick != null ){
					x = leftJoystick.position.x * tempSpeed;
					y = leftJoystick.position.y * tempSpeed;
				}
				
				if ( rigidbody != null )
					rigidbody.velocity = new Vector3(x, y, 0f);
				else
					rigidbody2D.velocity = new Vector3(x, y);

			}

			if ( !flight ){

				// space
				if ( Input.GetKey("space") ) {
					if ( isGrounded ){
						rigidbody.AddForce(new Vector3(0f, 160f, 0f));
						isGrounded = false;
					}
				}
				
				// left
				if ( Input.GetKey("a") )
					x = -1f * tempSpeed;
				
				// right
				if ( Input.GetKey("d") )
					x = 1f * tempSpeed;


				// joystick
				if ( leftJoystick != null ){
					x = leftJoystick.position.x * tempSpeed;
					//y = leftJoystick.position.y * tempSpeed;
				}
				
				rigidbody.velocity = new Vector3(x, rigidbody.velocity.y, 0f);

			}

		}


	}



}
