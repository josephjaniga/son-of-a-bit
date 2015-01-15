using UnityEngine;

public class NoiseTest : MonoBehaviour {
	// Update is called once per frame
	void Update () {
		transform.Translate(Time.deltaTime*.75f, 0f, 0f);
	}
}
