using UnityEngine;
using System.Collections;

public class Bit : MonoBehaviour {

	public Color 	c = Color.green;


	// Use this for initialization
	void Start () {
	
		setColor(c);

	}
	
	// Update is called once per frame
	void Update () {

	}
	
	public void setColor(Color input){
		transform.renderer.material.color = input;
	}


}
