using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SCT : MonoBehaviour {

	public bool enabled = false;
	public float Timeout = 2.0f;
	public float Timer = 0f;

	// Use this for initialization
	void Start () {
	
		Timer = Timeout;
	}
	
	// Update is called once per frame
	void Update () {
	
		Color c = gameObject.GetComponent<Text>().color;
		c.a = Timer/Timeout;
		gameObject.GetComponent<Text>().color = c; 

		Timer -= Time.deltaTime;
		if ( Timer <= 0f && enabled ){
			Destroy(gameObject);
		}

	}
}
