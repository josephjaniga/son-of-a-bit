﻿using UnityEngine;
using System.Collections;

public class Motion : MonoBehaviour {

	public Vector3 	direction = Vector3.zero;
	public float 	speed = 1.0f;
	public bool		userControlled 	= false;
	public bool 	shouldLock = true;
	
	public Vector3 rotation = Vector3.zero;

	// Use this for initialization
	void Start () {
	
	}

	// Update is called once per frame
	void Update () {
		
		handleMovement();

	}


	void handleMovement(){

		if ( shouldLock ){
			rigidbody.velocity = Vector3.zero;
		}
		
		if ( userControlled ){

			float x = 0.0f;
			float y = 0.0f;

			// up
			if ( Input.GetKey("w") )
				y = 1f * speed;

			// left
			if ( Input.GetKey("a") )
				x = -1f * speed;
			
			// down		
			if ( Input.GetKey("s") )
				y = -1f * speed;

			// right
			if ( Input.GetKey("d") )
				x = 1f * speed;

			rigidbody.velocity = new Vector3(x, y, 0f);

		}


	}



}
