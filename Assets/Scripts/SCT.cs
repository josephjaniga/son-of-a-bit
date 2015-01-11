using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SCT : MonoBehaviour {

	public bool isEnabled;
	public float Timeout = 2.0f;
	public float Timer = 2.0f;

	// Use this for initialization
	void Awake () {

		Timer = Timeout;
		isEnabled = true;
		
	}

	void Start () {

		Timer = Timeout;
		isEnabled = true;

	}

	// Update is called once per frame
	void Update () {

		// set the alpha
		Color c = gameObject.GetComponent<Text>().color;
		c.a = Timer/Timeout;
		gameObject.GetComponent<Text>().color = c;

		if ( isEnabled ){
			Timer -= Time.deltaTime;
		} else {
			Timer = Timeout;
		}

		if ( Timer <= 0f && gameObject != null && isEnabled ){
			Destroy(gameObject);
		}

	}
}
