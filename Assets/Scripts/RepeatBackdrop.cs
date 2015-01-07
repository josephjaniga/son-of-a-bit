using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RepeatBackdrop : MonoBehaviour {

	public Camera cam;

	public GameObject tile;

	public int dimmensionX = 300;
	public int dimmensionY = 100;

	public float width = 0;
	public float height = 0;

	public bool useSprites = false;
	public bool useMesh = false;

	public MeshGenerator gen;

	public Color bgColor = new Color(0.05f, 0f, 0.0625f);

	// Use this for initialization
	void Start () {

		cam = Camera.main;

		if ( useSprites ){

			width = tile.GetComponent<SpriteRenderer>().bounds.size.x;
			height = tile.GetComponent<SpriteRenderer>().bounds.size.y;
			
			for ( int i=-dimmensionX/2; i < dimmensionX/2; i++ ){
				for ( int j=-dimmensionY/2; j < dimmensionY/2; j++ ){
					GameObject temp = Instantiate(tile, new Vector3(i*width, j*height, 25f), Quaternion.identity) as GameObject;
					temp.transform.SetParent(gameObject.transform);
				}
			}

		} else if ( useMesh ) {

			gen = new MeshGenerator(transform);
			gen.setMeshScale(1f);
			gen.setMeshPosition(new Vector3(0f, 0f, 360f));
			gen.setMeshSize (200,200);
			gen.setMeshColor(bgColor);
			gen.randomMagnitude = 3f;

			GameObject plane = gen.generateMesh();
			plane.transform.Rotate( new Vector3(329f, 180f, 180f) );
			//plane.transform.Rotate( new Vector3(180f, 0f, 0f) );
			plane.layer = 31; // BG layer

		}
		
	}
	
	// Update is called once per frame
	void Update () {

		//gen.animate(0.01f);

	}



}
