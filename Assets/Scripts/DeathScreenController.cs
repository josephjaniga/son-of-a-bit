using UnityEngine;
using System.Collections;

public class DeathScreenController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void begin(){
		Application.LoadLevel("MainMenu");
	}

}
