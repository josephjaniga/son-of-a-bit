using UnityEngine;
using System.Collections;

public class LoadOut : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void quitGame(){
		Application.Quit();
	}

	public void begin(){
		Application.LoadLevel("TowerDefense");
	}

}