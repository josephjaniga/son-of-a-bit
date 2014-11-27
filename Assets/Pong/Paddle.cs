using UnityEngine;
using System.Collections;

public class Paddle : MonoBehaviour {

	public Vector3 startPosition = Vector3.zero;

	// Use this for initialization
	void Start () {
	
		transform.position = startPosition;

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void reset(){

	}
}