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




	// This first list contains every vertex of the mesh that we are going to render
	public List<Vector3> newVertices = new List<Vector3>();
	
	// The triangles tell Unity how to build each section of the mesh joining
	// the vertices
	public List<int> newTriangles = new List<int>();
	
	// The UV list is unimportant right now but it tells Unity how the texture is
	// aligned on each polygon
	public List<Vector2> newUV = new List<Vector2>();

	// A mesh is made up of the vertices, triangles and UVs we are going to define,
	// after we make them up we'll save them as this mesh
	private Mesh mesh;




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

		} else {

//			GameObject bg = GameObject.CreatePrimitive(PrimitiveType.Plane);
//			bg.transform.SetParent(transform);
//			bg.renderer.material.color = new Color(0.05f, 0f, 0.0625f); // dark purple
//			bg.transform.Rotate( new Vector3(-90f, 0f, 0f) );
//			bg.transform.localScale = new Vector3(500f, 500f, 500f);
//			bg.transform.position =  new Vector3(0f, 0f, 500f);

			GameObject plane = new GameObject("Mesh");
			plane.transform.SetParent(transform);
			MeshFilter meshFilter = (MeshFilter)plane.AddComponent(typeof(MeshFilter));
			MeshRenderer mr = plane.AddComponent<MeshRenderer>();
			mr.material.color = new Color(0.05f, 0f, 0.0625f);

			float scale = 1f;

			plane.transform.localScale = new Vector3(scale, scale, scale);
			
			mesh = plane.GetComponent<MeshFilter> ().mesh;

			plane.transform.position = new Vector3(0f, 0f, 360f);

			int w = 200;
			int h = 200;

			addVerts(newVertices, w, h);
			addTris (newTriangles, w, h);
			
			mesh.Clear ();
			mesh.vertices = newVertices.ToArray();
			mesh.triangles = newTriangles.ToArray();
			mesh.Optimize ();
			mesh.RecalculateNormals ();

			plane.transform.Rotate( new Vector3(235f, 0f, 0f) );

			plane.layer = 31; // BG layer

		}
		
	}
	
	// Update is called once per frame
	void Update () {
	
//		Vector3 temp = cam.transform.position;
//		temp.z = 0;
//		gameObject.transform.position = temp;

	}


	public void addVerts(List<Vector3> vertsList, int width, int height){
		for ( int x=-width/2; x < width/2; x++ ){
			for ( int y=height/2; y > -height/2; y-- ){
				vertsList.Add( new Vector3(x, y, Random.Range(0f, 3f)-15f ) );
			}
		}
	}

	public void addTris(List<int> trisList, int width, int height){
		int i = 0;
		while ( i < width * height - width - 1 ){

//			AddAndLog(trisList, i);
//			AddAndLog(trisList, i + 1);
//			AddAndLog(trisList, i + width);
//			AddAndLog(trisList, i + 1);
//          AddAndLog(trisList, i + width + 1);
//          AddAndLog(trisList, i + width);

			trisList.Add (i);
			trisList.Add (i + 1);
			trisList.Add (i + width);
			trisList.Add (i + 1);
			trisList.Add (i + width + 1);
			trisList.Add (i + width);
			i++;

		}
	}

	public void AddAndLog(List<int> l, int value){
		Debug.Log (value);
		l.Add(value);
	}


}
