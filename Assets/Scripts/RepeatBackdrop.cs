using UnityEngine;
using System.Collections;

public class RepeatBackdrop : MonoBehaviour {

	public Camera cam;

	public GameObject tile;

	public int dimmensionX = 300;
	public int dimmensionY = 100;

	public float width = 0;
	public float height = 0;

	// Use this for initialization
	void Start () {

		cam = Camera.main;

		width = tile.GetComponent<SpriteRenderer>().bounds.size.x;
		height = tile.GetComponent<SpriteRenderer>().bounds.size.y;

		Debug.Log (width + " " + height);

		for ( int i=-dimmensionX/2; i < dimmensionX/2; i++ ){
			for ( int j=-dimmensionY/2; j < dimmensionY/2; j++ ){
				GameObject temp = Instantiate(tile, new Vector3(i*width, j*height, 25f), Quaternion.identity) as GameObject;
				temp.transform.SetParent(gameObject.transform);
			}
		}

	}
	
	// Update is called once per frame
	void Update () {
	
//		Vector3 temp = cam.transform.position;
//		temp.z = 0;
//		gameObject.transform.position = temp;

	}
}
