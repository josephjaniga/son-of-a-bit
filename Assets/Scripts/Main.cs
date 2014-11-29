using UnityEngine;
using System.Collections;

public class Main : MonoBehaviour {

	public bool displayCharacter = false;

	public GameObject characterPanel;
	public GameObject p;

	// Use this for initialization
	void Start () {
	
		characterPanel = GameObject.Find("CharacterPanel");
		p = GameObject.Find("Player");
	}
	
	// Update is called once per frame
	void Update () {

		// hide all Panels
		if ( Input.GetKeyDown(KeyCode.Escape) ){
			displayCharacter = false;
		}

		// keypress c - toggle Character Panel
		if ( Input.GetKeyDown("c") ){
			displayCharacter = !displayCharacter;
		}

		if ( characterPanel != null ) {
			if ( displayCharacter ){
				characterPanel.SetActive(true);
			} else if ( !displayCharacter ){
				characterPanel.SetActive(false);
			}
		}

	}
}
