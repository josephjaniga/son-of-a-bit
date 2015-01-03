using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MeshGenerator {
	
	// This first list contains every vertex of the mesh that we are going to render
	private List<Vector3> newVertices = new List<Vector3>();
	private List<Vector3> animationVertices = new List<Vector3>();
	private List<Vector3> baseline = new List<Vector3>();
	private List<bool> youGood = new List<bool>();
	private bool allGood = false;
	
	// The triangles tell Unity how to build each section of the mesh joining
	// the vertices
	private List<int> newTriangles = new List<int>();
	
	// The UV list is unimportant right now but it tells Unity how the texture is
	// aligned on each polygon
	private List<Vector2> newUV = new List<Vector2>();
	
	// A mesh is made up of the vertices, triangles and UVs we are going to define,
	// after we make them up we'll save them as this mesh
	public Mesh mesh;

	public GameObject go;
	public MeshFilter meshFilter;
	public MeshRenderer mr;

	public float scale = 1f;

	public int w = 10;
	public int h = 10;

	public Color meshColor = Color.red;
	
	public float animationSpeed = 4f;
	public float lastAnimated = 0f;
	
	public MeshGenerator(Transform parent = null){

		go = new GameObject("Mesh");
		if ( parent != null )
			go.transform.SetParent(parent);
		meshFilter = (MeshFilter)go.AddComponent(typeof(MeshFilter));
		mr = go.AddComponent<MeshRenderer>();
		mr.material.shader = Resources.Load ("Other/Shaders/WindyGrass", typeof(Shader)) as Shader;
		mesh = go.GetComponent<MeshFilter>().mesh;

	}

	public void setMeshColor(Color c){
		meshColor = c;
		if ( mr != null ) {
			Debug.Log (meshColor);
			mr.material.color = meshColor;
		}
	}

	public void setMeshScale(float s = 1f){
		scale = s;
		if ( go != null)
			go.transform.localScale = new Vector3(scale, scale, scale);
	}

	public void setMeshPosition( Vector3 vector ){
		if ( vector == null )
			vector = Vector3.zero;
		go.transform.position = vector;
	}

	public void setMeshSize(int width, int height){
		w = width;
		h = height;
	}

	public GameObject generate(){

		newVertices = new List<Vector3>();
		newTriangles = new List<int>();
		animationVertices = newVertices;

		for ( int i=0; i< newVertices.Count; i++){
			youGood[i] = false;
		}

		baseline = newVertices;
		addVerts(newVertices, w, h);
		addTris(newTriangles, w, h);
		mesh.Clear ();
		mesh.vertices = newVertices.ToArray();
		mesh.triangles = newTriangles.ToArray();
		mesh.Optimize ();
		mesh.RecalculateNormals ();
		
		return go;

	}
	
	public void addVerts(List<Vector3> vertsList, int width, int height){
		for ( int x=-width/2; x < width/2; x++ ){
			for ( int y=height/2; y > -height/2; y-- ){
				vertsList.Add( new Vector3(x, y, Random.Range(0f, 1f)-15f ) );
			}
		}
	}
	
	public void addTris(List<int> trisList, int width, int height){
		int i = 0;
		while ( i < width * height - width - 1 ){
			trisList.Add (i);
			trisList.Add (i + 1);
			trisList.Add (i + width);
			trisList.Add (i + 1);
			trisList.Add (i + width + 1);
			trisList.Add (i + width);
			i++;
		}
	}


	public void animate(float magnitude){

		bool checker = true;
		for ( int i=0; i<youGood.Count; i++){
			if( !youGood[i] ){ allGood = false; }
		}

		if ( checker ){
			allGood = checker;
		}

		if ( allGood ){

			// set new values
			for ( int i=0; i < animationVertices.Count; i++ ){
				
				float x = 0f;
				float y = 0f;
				float z = Random.Range(-2f, 2f);
				
				// clamp magnitude
				x = Mathf.Clamp(x + animationVertices[i].x, baseline[i].x-magnitude, baseline[i].x+magnitude);
				y = Mathf.Clamp(y + animationVertices[i].y, baseline[i].y-magnitude, baseline[i].y+magnitude);
				z = Mathf.Clamp(z + animationVertices[i].z, baseline[i].z-magnitude, baseline[i].z+magnitude);
				
				animationVertices[i] = new Vector3(x, y, z);
				
			}

			for ( int i=0; i< youGood.Count; i++){
				youGood[i] = false;
			}
			allGood = false;

			lastAnimated = 0f;

		}

		lastAnimated += Time.deltaTime;

		// animate position
		for ( int i=0; i < youGood.Count; i++ ){
			if ( !youGood[i] ){
				newVertices[i] = Vector3.Lerp(newVertices[i], animationVertices[i], lastAnimated*animationSpeed);
			}
			if ( newVertices[i] == animationVertices[i] ){
				youGood[i] = true;
			}
		}
		
		mesh.vertices = newVertices.ToArray();
		mesh.Optimize();
		mesh.RecalculateBounds();
		mesh.RecalculateNormals();
		
		
	}



}
