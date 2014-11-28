using UnityEngine;
using System.Collections;

public class Bit : MonoBehaviour {

	public Color 	c = Color.green;
	public Color	deadColor = new Color(0.25f, 0.25f, 0.25f, 0.25f);

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
