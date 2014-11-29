using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class IntroSequence : MonoBehaviour {
	public GameObject fiya;
	public float duration = 0.75f;
	private float wait = 0.75f;
	private float alpha = 0f;
	private bool playedSound = false;
	private bool play = false;
	private float startTime = 0.0f;
	private float runningTime = 0.0f;
	public string state = "wait";

	// Use this for initialization
	void Start () {
		Screen.SetResolution(1600, 900, true);
		fiya.GetComponent<Image>().color = new Color( 1.0f, 1.0f, 1.0f, 0.0f);
		fiya.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width, Screen.height);
	}
	
	// Update is called once per frame
	void Update () {

		if ( Time.time >= wait && !play && state == "wait" ){
			play = true;
			startTime = Time.time;
			state = "fade in";
		}

		if ( play ){

			if ( state == "fade in" ){

				runningTime = Time.time - startTime;
				fiya.GetComponent<Image>().color = new Color( 1.0f, 1.0f, 1.0f, lerpAlpha (runningTime, duration, 0f, 1f));

				if ( Time.time - startTime >= duration ){
					startTime = Time.time;
					state = "stay";
					duration = 1.25f;
				}

				if (!playedSound ){
					gameObject.GetComponent<AudioSource>().Play();
					playedSound = true;
				}

			} else if ( state == "stay" ){

				runningTime = Time.time - startTime;

				if ( Time.time - startTime >= duration ){
					startTime = Time.time;
					state = "fade out";
					duration = 1.0f;
				}


			} else if ( state == "fade out" ){

				runningTime = Time.time - startTime;
				fiya.GetComponent<Image>().color = new Color( 1.0f, 1.0f, 1.0f, lerpAlpha (runningTime, duration, 1f, 0f));
				
				if ( Time.time - startTime >= duration ){
					play = false;
					state = "done";
					startTime = Time.time;
					duration = 1.25f;
				}

			}

		}

		if ( state == "done" ){
			if ( Time.time - startTime >= duration ){
				Application.LoadLevel("Sandbox");
			}
		}

	}

	public float lerpAlpha (float rt, float d, float start, float end){
		float lerp = Mathf.PingPong(rt, d) / d;
		alpha = Mathf.Lerp(start, end, lerp);
		return alpha;
	}


}
