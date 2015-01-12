using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public enum LevelTypes {
	Rooms,
	Surface,
	LegacySurface
};

public enum CardinalDirection {
	North,
	South,
	East,
	West
};

public class LevelMapController : MonoBehaviour {

	public static List<GameObject> rooms;
	public int numberOfRooms;
	private int minRooms = 1;
	private int maxRooms = 1;

	public Main m;
	public DataProvider dp;
	public GameObject ship;
	public GameObject playa;

	public int seed = -1;

	public Material primary = null;
	public Material secondary = null;
	public Material tertiary = null;

	public List<string> attributes = new List<string>();

	List<GameObject> mountains = new List<GameObject>();

	// Use this for initialization
	void Start () {

		m = GameObject.Find ("FatherBit").GetComponent<Main>();
		dp = GameObject.Find ("DataProvider").GetComponent<DataProvider>();
		ship = GameObject.Find ("PlayerShip");
		playa = GameObject.Find ("Player");


		populateAttributes(dp.planetType);

		if ( dp.levelSeed == -1 ){
			seed = UnityEngine.Random.Range(0,9999);
			dp.levelSeed = seed;
		} else {
			seed = dp.levelSeed;
		}

		UnityEngine.Random.seed = seed;

		getMaterials(dp);

		if ( dp.playerSystemInShip ){
			// put the player in the ship and set the ship as unit in control
			playa.GetComponent<Motion>().userControlled = false;
			ship.GetComponent<Motion>().userControlled = true;
			m.unitInControl = ship;
			ship.GetComponent<Vehicle>().seat = playa;
			m.inVehicle = true;
			m.v = ship.GetComponent<Vehicle>();
			playa.SetActive(false);
			Camera.main.GetComponent<MiniMapCameraFollow>().target = m.unitInControl.transform;
		}

		if ( dp.playerLocalLastPosition != Vector3.zero  ) {
			m.unitInControl.transform.position = dp.playerLocalLastPosition;
		}

		GameObject LevelMap = new GameObject("LevelMap");

		if ( dp.levelType == LevelTypes.Rooms ){
			// generateRooms ();
			generateSurface ();
		}

		/**
		 * DEPRECATED
		 */
		if ( dp.levelType == LevelTypes.LegacySurface ){
			// generateLegacySurface ();
			generateSurface ();
		}

		if ( dp.levelType == LevelTypes.Surface ){
			generateSurface ();
		}
	
		TextTools.clearAlerts();
		TextTools.createAlert("- Surface of " + dp.lastPlanetName + " -");


	}

	// Update is called once per frame
	void Update () {

	}

	public void generateRooms(){
		rooms = new List<GameObject>();
		numberOfRooms = UnityEngine.Random.Range(dp.minRooms, dp.maxRooms);
		Vector3 rSize = new Vector3(10f, 10f, 1f);
		Vector3 rPosition = new Vector3(1f, 1f, 1f);
		for( int i=0; i<numberOfRooms; i++){
			if ( i==0 ){
				rSize = new Vector3(20f,10f, 1f);
				rPosition = new Vector3(0f, 0f, 1f);
			} else {
				// get last room right edge
				Vector3 lastPosition = rooms[i-1].transform.position;
				Vector3 lastSize = rooms[i-1].transform.localScale;
				float padding = lastSize.y/4f;
				// right X
				float rightX = lastPosition.x + lastSize.x/2;
				rSize = new Vector3(UnityEngine.Random.Range(3f, 25f), UnityEngine.Random.Range(3f, 22f), 1f);
				if (i==numberOfRooms-1){
					rSize *= 3.05f;
				} else {
					rSize *= 1.05f;
				}
				rSize.z = 1f;	
				// random Y on that edge
				float rightRandomY = UnityEngine.Random.Range ( lastPosition.y - lastSize.y/2 + padding, lastPosition.y + lastSize.y/2 - padding );
				float rightRandomYConnection = UnityEngine.Random.Range ( -rSize.y/2f, rSize.y/2f );
				rPosition = new Vector3(rightX + rSize.x/2, rightRandomY + rightRandomYConnection, 1f);
			}
			makeRoom(rSize, rPosition).name = "Room_"+i;
		}
		// make the walls
		foreach( GameObject room in rooms ){
			construct(room);
		}
		// starting room
		rooms[0].renderer.material.color = new Color(0f, 0.3f, 0f);
		// boss room
		rooms[rooms.Count-1].renderer.material.color = new Color(0.3f, 0f, 0f);
	}

	public void generateLegacySurface(){

		// make the surface and the floor
		rooms = new List<GameObject>();
		numberOfRooms = 1;
		Vector3 rSize = new Vector3(2000f, 15f, 1f);
		Vector3 rPosition = new Vector3(0f, rSize.y/2f, 1f);
		makeRoom(rSize, rPosition).name = "Surface";
		// hide the background

		GameObject surface = GameObject.Find ("Surface");
		surface.transform.localScale = new Vector3(2000f, 150f, 1f);
		surface.transform.position = new Vector3(0f, 150f/2, 348f);
		surface.renderer.material.color = new Color(0f, 1f, 1f, 103f/255f);

		GameObject subTerrain = GameObject.CreatePrimitive(PrimitiveType.Cube);
		subTerrain.name = "SubTerrain";
		subTerrain.transform.SetParent(GameObject.Find ("LevelMap").transform);
		subTerrain.transform.localScale = new Vector3(rSize.x, rSize.y*2, rSize.z);
		subTerrain.transform.position = new Vector3(rPosition.x, -rSize.y, rPosition.z);
		subTerrain.renderer.material.color = Color.black;

		GameObject atmosphericPerspective = GameObject.CreatePrimitive(PrimitiveType.Quad);
		atmosphericPerspective.name = "AtmosphericPerspective";
		atmosphericPerspective.transform.SetParent(GameObject.Find ("LevelMap").transform);
		atmosphericPerspective.transform.localScale = new Vector3(2000f, 150f, 1f);
		atmosphericPerspective.transform.position = new Vector3(0f, 150f/2, 77f);
		atmosphericPerspective.renderer.material.color = new Color(0f, 0f, 0f, 173f/255f);
		atmosphericPerspective.renderer.material.shader = Shader.Find ("Transparent/Diffuse");

		GameObject T = Instantiate(Resources.Load("Prefabs/TerrainPrefab"), new Vector3(-1000, 0f, -1000), Quaternion.identity) as GameObject;
		int res = 513;
		TerrainData tData = T.GetComponent<Terrain>().terrainData;
		tData.heightmapResolution = res;

		//Debug.Log (tData);

		float[,] heights = new float[res,res];
		for ( int i=0; i<res*res; i++ ){

			// i%res - Y
			// i/res - X

			// FOREGROUND
			if ( i%res >res/2-2 && i%res <res/2+2 ){
				heights[i%res, i/res] = UnityEngine.Random.Range(.005f, .0075f);
				heights[i%res, i/res] = heights[i%res, i/res] + Mathf.Sin( i/res )/525 * UnityEngine.Random.Range(.90f, 1.2f);
				heights[i%res, i/res] = heights[i%res, i/res] + Mathf.Cos( res/i )/525;
				heights[i%res, i/res] = heights[i%res, i/res] + Mathf.Sin( i/res * .2f + UnityEngine.Random.Range(0f, 1f))/255;
			}

			// MIDGROUND rolling hills
			if ( i%res < res *.55f && i%res >= res * .54f ){ 
				heights[i%res, i/res] = UnityEngine.Random.Range(.005f, .0075f);
				heights[i%res, i/res] = heights[i%res, i/res] + Mathf.Sin( i/res * .11f - 30f )/75;
			}
			
			// MIDGROUND rolling hills
			if ( i%res < res *.53f && i%res >= res * .52f ){ 
				//heights[i%res, i/res] = UnityEngine.Random.Range(.005f, .0075f);
				heights[i%res, i/res] = heights[i%res, i/res] +  Mathf.Sin( i/res * .15f )/95;
			}

			// LARGE BG MOUNTAINS FRONT
			if ( i%res < res *.60f && i%res >= res * .56f ){ // large backgro9und peaks
				float r = UnityEngine.Random.Range(.5f, 5.2f);
				if ( Mathf.Sin( i/res * .25f )/175 * r + Mathf.Cos(i%res + i/res )/125 > 0f ){
					heights[i%res, i/res] = UnityEngine.Random.Range(.015f, .027f);
				} else {
					heights[i%res, i/res] = 0f;
				}
				heights[i%res, i/res] = heights[i%res, i/res] + Mathf.Sin( i/res * .25f )/175 * r + Mathf.Cos(i%res + i/res )/125;
			}

			// INCLINE UP TO THE MONTAIN
			if ( i%res < res *.575f && i%res >= res * .57f ){ // large background peaks
				heights[i%res, i/res] *= .75f;
			}
			if ( i%res < res *.57f && i%res >= res * .565f ){ // large background peaks
				heights[i%res, i/res] *= .5f;
			}
			if ( i%res < res *.565f && i%res >= res * .56f ){ // large background peaks
				heights[i%res, i/res] *= .25f;
			}

			// Mountainous Random Spattering
			if ( i%res < res *.66f && i%res >= res * .65f ){ // large backgro9und peaks
				heights[i%res, i/res] = heights[i%res, i/res] + Mathf.Sin( i/res * .08f - .11f )/33 * UnityEngine.Random.Range(.1f, .6f) + Mathf.Cos( i%res + i/res + .3f )/22 * UnityEngine.Random.Range(.3f, .6f);
			}

			// RANDOM  BUMP TEXTURE
			//heights[i%res, i/res] = heights[i%res, i/res] + UnityEngine.Random.Range(-.1f, .1f)/125;

			// FOREGROUND FIX
			// of the foreground make the furthest back the tallest
			if ( i%res > res/2-2 && i%res <res/2 ){
				if ( i/res > 0 && heights[i%res, i/res-1] != 0f )
					heights[i%res, i/res] = heights[i%res, i/res-1] * .99f;
			}


			// SMOOTHING
			if ( i > 0 &&  i < res*res && i%2 != 0 ){
				heights[i%res, i/res] = (heights[i%res, (i+1)/res] + heights[i%res, (i-1)/res] ) /2;
			}


			/*
			// SURFACE OBJECTS NOT OPERATIONAL
			GameObject temp = null;
			float size = 2000.0f;
			float resSpace = size / res;
			int numTrees = 1;

			// SURFACE OBJECTS
			if ( i%res > res/2-3 && i%res < res/2+5 ){
				if ( i%7 == 0 ){
					numTrees = UnityEngine.Random.Range(3, 7);

					for ( int t=0; t<numTrees; t++ ){

						float x = i/res*resSpace-size/2 + UnityEngine.Random.Range(-3f - numTrees, 3f + numTrees);
						float z = i%res*resSpace-size/2 + UnityEngine.Random.Range(-3f - numTrees, 3f + numTrees);

						temp = Instantiate(Resources.Load("Prefabs/SurfaceObjects/FirTree1"), new Vector3(x, 55f, z), Quaternion.identity) as GameObject;

						//temp.transform.SetParent(GameObject.Find("Surface").transform); 

						RaycastHit hit;

						if (Physics.Raycast(temp.transform.position, -Vector3.up, out hit, Mathf.Infinity, 30)){
							temp.transform.position = hit.point;
							temp.transform.Rotate(new Vector3(270, 0f, 0f));
						}

					}
				}
			}
			*/

		}

		// apply the calculated math terrain heights
		tData.SetHeights (0, 0, heights);

		/* Make Procedural Textures and Assign them */

		Texture2D texture = new Texture2D(1, 1, TextureFormat.ARGB32, false);
		texture.SetPixel(0, 0, new Color(.35f, 0f, 0f));
		texture.Apply();

		Texture2D texture2 = new Texture2D(1, 1, TextureFormat.ARGB32, false);
		texture2.SetPixel(0, 0, new Color(0f, .35f, 0f) );
		texture2.Apply();

		Texture2D texture3 = new Texture2D(1, 1, TextureFormat.ARGB32, false);
		texture3.SetPixel(0, 0, new Color(0f, 0f, .35f));
		texture3.Apply();

		Texture2D texture4 = new Texture2D(1, 1, TextureFormat.ARGB32, false);
		texture4.SetPixel(0, 0, Color.white);
		texture4.Apply();

		SplatPrototype[] SPS = new SplatPrototype[4];
		SPS[0] = new SplatPrototype();
		SPS[0].texture = texture;
		SPS[0].tileOffset = new Vector2(0f, 0f);
		SPS[0].tileSize = new Vector2(1f, 1f);
		SPS[0].texture.Apply (true);

		SPS[1] = new SplatPrototype();
		SPS[1].texture = texture2;
		SPS[1].tileOffset = new Vector2(0f, 0f);
		SPS[1].tileSize = new Vector2(1f, 1f);
		SPS[1].texture.Apply (true);

		SPS[2] = new SplatPrototype();
		SPS[2].texture = texture3;
		SPS[2].tileOffset = new Vector2(0f, 0f);
		SPS[2].tileSize = new Vector2(1f, 1f);
		SPS[2].texture.Apply (true);

		SPS[3] = new SplatPrototype();
		SPS[3].texture = texture4;
		SPS[3].tileOffset = new Vector2(0f, 0f);
		SPS[3].tileSize = new Vector2(1f, 1f);
		SPS[3].texture.Apply (true);

		//T.AddComponent<AssignSplatMap>();
		tData.splatPrototypes = SPS; 
		
	}

	
	public void generateSurface(){

		// make the surface and the floor
		rooms = new List<GameObject>();
		numberOfRooms = 1;
		Vector3 rSize = new Vector3(2000f, 15f, 1f);
		Vector3 rPosition = new Vector3(0f, rSize.y/2f, 1f);
		makeRoom(rSize, rPosition).name = "Surface";
		// hide the background
		GameObject.Find ("Background").SetActive(false);
		
		GameObject surface = GameObject.Find ("Surface");
		surface.transform.localScale = new Vector3(2000f, 150f, 1f);
		surface.transform.position = new Vector3(0f, 150f/2, 348f);
		surface.renderer.material.color = new Color(0f, 1f, 1f, 55f/255f);
		
		GameObject subTerrain = GameObject.CreatePrimitive(PrimitiveType.Cube);
		subTerrain.name = "SubTerrain";
		subTerrain.transform.SetParent(GameObject.Find ("LevelMap").transform);
		subTerrain.transform.localScale = new Vector3(rSize.x, rSize.y*2, 26f);
		subTerrain.transform.position = new Vector3(rPosition.x, -rSize.y, 0f);
		if ( false ){ // ground
			subTerrain.renderer.material = primary;
		} else { // water
			subTerrain.renderer.material.shader = Shader.Find("Transparent/Diffuse");
			subTerrain.renderer.material.color = new Color(primary.color.r, primary.color.g-.11f, primary.color.b -.11f, 0.75f);
			subTerrain.collider.enabled = false;
		}
		


		mountains = new List<GameObject>();
		int mountainCounter = 0;
		// populate list of mountains
		if ( attributes.Contains("mountainous") ){
			mountains.Add(Resources.Load("Prefabs/SurfaceObjects/MountainOne") as GameObject);
			mountains.Add(Resources.Load("Prefabs/SurfaceObjects/MountainTwo") as GameObject);
			mountains.Add(Resources.Load("Prefabs/SurfaceObjects/MountainThree") as GameObject);
		}

		GameObject bg = new GameObject("Surface_Background");
		bg.transform.SetParent(GameObject.Find("Surface").transform);

		for (int i=-1000; i < 1000; i++){

			// Icospheric Sub Surface Texture
			if ( attributes.Contains("icospheric") ){
				if ( Mathf.Sin (i*.5f) >= 0.95f){
					float randomY = UnityEngine.Random.Range(-22f, -25f);
					GameObject temp = (GameObject)Instantiate( Resources.Load("Prefabs/SurfaceObjects/Icosphere"), new Vector3( i, randomY, 0 ), Quaternion.identity);
					temp.transform.Rotate ( new Vector3( UnityEngine.Random.Range(0f, 360f), UnityEngine.Random.Range(0f, 360f), UnityEngine.Random.Range(0f, 360f) ) );
					float scale = UnityEngine.Random.Range(44f, 55f);
					temp.transform.localScale = new Vector3(scale, scale, scale);
					temp.renderer.material = secondary; 
					temp.transform.SetParent(bg.transform);
				}
			}

			// Mountainous - Secondary
			if ( attributes.Contains("mountainous") && mountains.Count > 0 && primary != null ){
				if ( Mathf.Sin (i) >= 0.9f){
					GameObject temp = (GameObject)Instantiate(mountains[ mountainCounter % mountains.Count ], new Vector3( i, 0f, 33f ), Quaternion.identity);
					temp.transform.Rotate ( new Vector3( -90f, UnityEngine.Random.Range(0f, 360f), temp.transform.rotation.z ) );
					temp.renderer.material = tertiary;
					float scale = UnityEngine.Random.Range(.25f, 1.5f);
					temp.transform.localScale = new Vector3(	scale * temp.transform.localScale.x,
																scale * temp.transform.localScale.y,
																scale * temp.transform.localScale.z );
					temp.transform.SetParent(bg.transform);
					mountainCounter++;
				}
			}



			// Mountainous - Test JUST A TEST
			if ( attributes.Contains("mountainous") && mountains.Count > 0 && primary != null ){
				if ( Mathf.Sin (i) >= 0.95f){
					GameObject temp = (GameObject)Instantiate(mountains[ mountainCounter % mountains.Count ], new Vector3( i, 0f, 55f ), Quaternion.identity);
					temp.transform.Rotate ( new Vector3( -90f, UnityEngine.Random.Range(0f, 360f), temp.transform.rotation.z ) );
					temp.renderer.material = tertiary; 
					float scale = UnityEngine.Random.Range(.5f, 2f);
					temp.transform.localScale = new Vector3(	scale * temp.transform.localScale.x,
																scale * temp.transform.localScale.y,
																scale * temp.transform.localScale.z );
					temp.transform.SetParent(bg.transform);
					mountainCounter++;
				}
			}



		}
	}


	[ContextMenu("Reset")] 
	public void Reset(){

		foreach(Transform child in GameObject.Find("LevelMap").transform ){
			Destroy(child.gameObject);
		}

		if ( dp.levelType == LevelTypes.Rooms ){
			generateRooms ();
		}
		
		if ( dp.levelType == LevelTypes.Surface ){
			generateSurface ();
		}

	}

	public GameObject makeRoom(Vector3 roomSize, Vector3 roomPosition){
		GameObject room = GameObject.CreatePrimitive(PrimitiveType.Quad);
		if ( roomSize != null ){
			roomSize.x += .2f;
			room.transform.localScale = roomSize;
		}
		if ( roomPosition != null )
			room.transform.position = roomPosition;
		room.renderer.material.shader = Shader.Find("Transparent/Diffuse");
		room.renderer.material.color = new Color(0f, 0.35f, 0.5f, 0.35f);
		rooms.Add(room);
		room.transform.SetParent(GameObject.Find("LevelMap").transform);
		return room;
	}


	public void construct(GameObject parentRoom){

		Vector3 roomSize = parentRoom.transform.localScale;
		Vector3 roomPosition = parentRoom.transform.position;

		// FLOOR AND CEILING
		makeWall(
			new Vector3(roomSize.x+0.5f, 0.5f, 1f),
			"NorthWall",
			parentRoom,
			(int)CardinalDirection.North
			);
		
		makeWall(
			new Vector3(roomSize.x+0.5f, 0.5f, 1f),
			"SouthWall",
			parentRoom,
			(int)CardinalDirection.South
			);

		// left wall
		if ( parentRoom == rooms[0] ){
			makeWall(
				new Vector3(0.5f, roomSize.y-0.5f, 1f),
				"WestWall",
				parentRoom,
				(int)CardinalDirection.West
				);
		}

		// right wall
		if ( parentRoom == rooms[rooms.Count-1] ){
			makeWall(
				new Vector3(0.5f, roomSize.y-0.5f, 1f),
				"EastWall",
				parentRoom,
				(int)CardinalDirection.East
				);
		}

		List<GameObject> intersectingRooms = new List<GameObject>();

		// identify all intersections with other rooms
		foreach( GameObject room in rooms ){
			if ( parentRoom != room ){
				if (	parentRoom.GetComponent<MeshCollider>().bounds.Intersects(
							room.GetComponent<MeshCollider>().bounds
						)
				    	&& room.transform.position.x > parentRoom.transform.position.x){
					intersectingRooms.Add (room); continue;
				}
			}
		}

		if ( intersectingRooms.Count == 0 && parentRoom != rooms[rooms.Count-1] ){
			Debug.Log (parentRoom.name + " we got a problem");
			Debug.Log ( parentRoom.name + " intersects " + intersectingRooms.Count + " room(s). " + intersectingRooms[0].name );

		}

		//Debug.Log ( parentRoom.name + " intersects " + intersectingRooms.Count + " room(s). " + intersectingRooms[0].name );

		foreach ( GameObject room in intersectingRooms ){

			float AT = parentRoom.transform.position.y + parentRoom.transform.localScale.y/2;
			float AB = parentRoom.transform.position.y - parentRoom.transform.localScale.y/2;
			float BT = room.transform.position.y + room.transform.localScale.y/2;
			float BB = room.transform.position.y - room.transform.localScale.y/2;

			//markDoors(parentRoom, room);

			if ( AT >= BT && AB >= BB ){
				/*
				 * AT
				 * X------X
				 * |      |     BT
				 * |  A   X------X
				 * |      |      |
				 * X------X   B  |
				 * AB     |      |
				 *        X------X
				 *              BB
				 */
			
				makePartialWall(
					new Vector3(0.5f, AT-BT - 0.5f, 1f),
						new Vector3(
							parentRoom.transform.position.x + parentRoom.transform.localScale.x/2,
							parentRoom.transform.position.y + parentRoom.transform.localScale.y/2 - (AT-BT)/2,
							0f),
						"EastTopWall",
						parentRoom
					);

				makePartialWall(
					new Vector3(0.5f, BB-AB - 0.5f, 1f),
					new Vector3(
					parentRoom.transform.position.x + parentRoom.transform.localScale.x/2,
					parentRoom.transform.position.y - parentRoom.transform.localScale.y/2 + (BB-AB)/2,
					0f),
					"EastBottomWall",
					parentRoom
					);

			} else if ( AT <= BT && AB <= BB )  {
				/*
				 *              BT
				 *        X------X
				 * AT     |      |
				 * X------X   B  |
				 * |      |      |
				 * |  A   X------X
				 * |      |     BB
				 * X------X
				 * AB
				 */

				makePartialWall(
					new Vector3(0.5f, AT-BT - 0.5f, 1f),
					new Vector3(
					parentRoom.transform.position.x + parentRoom.transform.localScale.x/2,
					parentRoom.transform.position.y + parentRoom.transform.localScale.y/2 - (AT-BT)/2,
					0f),
					"EastTopWall",
					parentRoom
					);

				makePartialWall(
					new Vector3(0.5f, BB-AB - 0.5f, 1f),
					new Vector3(
					parentRoom.transform.position.x + parentRoom.transform.localScale.x/2,
					parentRoom.transform.position.y - parentRoom.transform.localScale.y/2 + (BB-AB)/2,
					0f),
					"EastBottomWall",
					parentRoom
					);

			} else if ( AT >= BT && AB <= BB ) {
				// center big left

				/*
				 * AT     
				 * X------X      BT  
				 * |      X------X
				 * |  A   |	     |
				 * |      |   B  |
   				 * |      X------X
				 * X------X      BB
				 * AB
				 */

				makePartialWall(
					new Vector3(0.5f, AT-BT - 0.5f, 1f),
					new Vector3(
					parentRoom.transform.position.x + parentRoom.transform.localScale.x/2,
					parentRoom.transform.position.y + parentRoom.transform.localScale.y/2 - (AT-BT)/2,
					0f),
					"EastTopWall",
					parentRoom
					);
				
				makePartialWall(
					new Vector3(0.5f, BB-AB - 0.5f, 1f),
					new Vector3(
					parentRoom.transform.position.x + parentRoom.transform.localScale.x/2,
					parentRoom.transform.position.y - parentRoom.transform.localScale.y/2 + (BB-AB)/2,
					0f),
					"EastBottomWall",
					parentRoom
					);

			} else if ( AT <= BT && AB >= BB ) {

				/*
				 * AT     
				 *      X------X  BT  
				 *      |      |
				 * X----X  B   |
				 * |  A |      |
   				 * X----X      |
				 *      X------X  BB
				 * AB
				 */

				makePartialWall(
					new Vector3(0.5f, AT-BT - 0.5f, 1f),
					new Vector3(
					parentRoom.transform.position.x + parentRoom.transform.localScale.x/2,
					parentRoom.transform.position.y + parentRoom.transform.localScale.y/2 - (AT-BT)/2,
					0f),
					"EastTopWall",
					parentRoom
					);
				
				makePartialWall(
					new Vector3(0.5f, BB-AB - 0.5f, 1f),
					new Vector3(
					parentRoom.transform.position.x + parentRoom.transform.localScale.x/2,
					parentRoom.transform.position.y - parentRoom.transform.localScale.y/2 + (BB-AB)/2,
					0f),
					"EastBottomWall",
					parentRoom
					);
			}


		}

	}

	public void makeWall(Vector3 wallSize, string wallName, GameObject parentRoom, int cardDir){

		Vector3 tempWallPosition;
		GameObject tempWall =  GameObject.CreatePrimitive(PrimitiveType.Cube);
		tempWall.transform.localScale = wallSize;
		switch(cardDir){
		default:
		case (int)CardinalDirection.West:
			tempWallPosition = new Vector3(
				parentRoom.transform.position.x - parentRoom.transform.localScale.x/2,
				parentRoom.transform.position.y,
				0f);
			break;
		case (int)CardinalDirection.East:
			tempWallPosition = new Vector3(
				parentRoom.transform.position.x + parentRoom.transform.localScale.x/2,
				parentRoom.transform.position.y,
				0f);
			break;
		case (int)CardinalDirection.North:
			tempWallPosition = new Vector3(
				parentRoom.transform.position.x,
				parentRoom.transform.position.y + parentRoom.transform.localScale.y/2,
				0f);
			break;
		case (int)CardinalDirection.South:
			tempWallPosition = new Vector3(
				parentRoom.transform.position.x,
				parentRoom.transform.position.y - parentRoom.transform.localScale.y/2,
				0f);
			break;
		}

		tempWall.transform.position = tempWallPosition;
		tempWall.renderer.material.color = Color.gray;
		tempWall.name = wallName;
		tempWall.transform.SetParent(parentRoom.transform);

	}


	public void makePartialWall(Vector3 wallSize, Vector3 wallPosition, string wallName, GameObject parentRoom){

		GameObject tempWall =  GameObject.CreatePrimitive(PrimitiveType.Cube);
		tempWall.transform.localScale = wallSize;
		tempWall.transform.position = wallPosition;
		tempWall.renderer.material.color = Color.gray;
		tempWall.name = wallName;
		tempWall.transform.SetParent(parentRoom.transform);

	}


	public void markDoors(GameObject parentRoom, GameObject room){

		float AT = parentRoom.transform.position.y + parentRoom.transform.localScale.y/2;
		float AB = parentRoom.transform.position.y - parentRoom.transform.localScale.y/2;
		float BT = room.transform.position.y + room.transform.localScale.y/2;
		float BB = room.transform.position.y - room.transform.localScale.y/2;

		GameObject q = GameObject.CreatePrimitive(PrimitiveType.Quad);

		if ( AT >= BT && AB >= BB ){
			/*
			 * AT
			 * X------X
			 * |      |     BT
			 * |  A   X------X
			 * |      |      |
			 * X------X   B  |
			 * AB     |      |
			 *        X------X
			 *              BB
			 */
		
			q.transform.position = new Vector3(
				parentRoom.transform.position.x + parentRoom.transform.localScale.x/2,
				parentRoom.transform.position.y + (BT-AT)/2,
				0.5f);
			q.transform.localScale = new Vector3(2f, BT-AB, 1f);
			q.renderer.material.color = Color.red;
			
		} else if ( AT <= BT && AB <= BB )  {
			/*
			 *              BT
			 *        X------X
			 * AT     |      |
			 * X------X   B  |
			 * |      |      |
			 * |  A   X------X
			 * |      |     BB
			 * X------X
			 * AB
			 */

			q.transform.position = new Vector3(
				parentRoom.transform.position.x + parentRoom.transform.localScale.x/2,
				parentRoom.transform.position.y + (BB-AB)/2,
				0.5f);
			q.transform.localScale = new Vector3(2f, AT-BB, 1f);
			q.renderer.material.color = Color.green;

		} else if ( AT >= BT && AB <= BB ) {
			// center big left
			
			/*
			 * AT     
			 * X------X      BT  
			 * |      X------X
			 * |  A   |	     |
			 * |      |   B  |
			 * |      X------X
			 * X------X      BB
			 * AB
			 */

			q.transform.position = new Vector3(
				parentRoom.transform.position.x + parentRoom.transform.localScale.x/2,
				parentRoom.transform.position.y + (BT-AT)/2 + (BB-AB)/2,
				0.5f);
			q.transform.localScale = new Vector3(2f, BT-BB, 1f);
			q.renderer.material.color = Color.blue;

			
		} else if ( AT <= BT && AB >= BB ) {
			/*
			 * AT     
			 *      X------X  BT  
			 *      |      |
			 * X----X  B   |
			 * |  A |      |
		 	 * X----X      |
			 *      X------X  BB
			 * AB
			 */

			// center big right

			q.transform.position = new Vector3(
				parentRoom.transform.position.x + parentRoom.transform.localScale.x/2,
				parentRoom.transform.position.y,
				0.5f);
			q.transform.localScale = new Vector3(2f, AT-AB, 1f);
			q.renderer.material.color = Color.black;


		}

		if ( q != null ){
			q.name = parentRoom.name + "->" + room.name + "_Door";
			q.transform.SetParent(GameObject.Find ("LevelMap").transform);
		}
	}

	public void getMaterials(DataProvider dp){
		// check the dataprovider
		if ( dp == null ){
			dp = GameObject.Find("DataProvider").GetComponent<DataProvider>();
		}
		if ( dp!= null ){	
			// get the materials
			foreach (Material mat in dp.levelMaterials) {
				if ( mat.name.ToLower().Contains("primary") ) {
					primary = mat;
				}
				if ( mat.name.ToLower().Contains("secondary") ) {
					secondary = mat;
				}
				if ( mat.name.ToLower().Contains("tertiary") ) {
					tertiary = mat;
				}
			}
		}
	}

	public void populateAttributes(PlanetTypes pt){
		switch(pt){
			default:
			break;
			case PlanetTypes.EarthLike:
				attributes.Add("mountainous");
				attributes.Add("icospheric");
			break;
			case PlanetTypes.Cube:
			break;
			case PlanetTypes.Moon:
			break;
			case PlanetTypes.IcePlanet:
			break;
		}
	}

}
