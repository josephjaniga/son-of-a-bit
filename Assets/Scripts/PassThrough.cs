using UnityEngine;
using System.Collections;

public class PassThrough : MonoBehaviour {

	public GameObject startingWeapon;

	// Use this for initialization
	void Start () {
		if ( this.transform != null ){
			DontDestroyOnLoad(this.transform);
		}
		if ( this.gameObject != null ){
			DontDestroyOnLoad(this.gameObject);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

}
