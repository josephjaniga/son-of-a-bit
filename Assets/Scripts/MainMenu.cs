using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {

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

		GameObject i = GameObject.Find("Items");
		if ( i != null ){
			Destroy(i);
		}

		Application.LoadLevel("LoadOut");
	}

}