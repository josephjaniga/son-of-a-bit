using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum LevelTypes {
	Rooms,
	Surface
};

public enum CardinalDirection {
	North,
	South,
	East,
	West
};

public class LevelMapController : MonoBehaviour {

	public LevelData LevelOptions;

	public static List<GameObject> rooms;
	public LevelTypes levelType = LevelTypes.Rooms;
	public bool useGravity = true;
	public int numberOfRooms;
	private int minRooms = 11;
	private int maxRooms = 30;

	// Use this for initialization
	void Start () {

		// put the player in the ship and set the ship as unit in control
		GameObject ship =  GameObject.Find ("PlayerShip");
		GameObject playa =  GameObject.Find ("Player");
		Main m = GameObject.Find ("FatherBit").GetComponent<Main>();
		playa.GetComponent<Motion>().userControlled = false;
		ship.GetComponent<Motion>().userControlled = true;
		m.unitInControl = ship;
		ship.GetComponent<Vehicle>().seat = playa;
		m.inVehicle = true;
		m.v = ship.GetComponent<Vehicle>();
		playa.SetActive(false);


		GameObject ldObj = GameObject.Find ("LevelData");
		if (  ldObj != null ){
			LevelOptions = ldObj.GetComponent<LevelData>();
			if ( LevelOptions != null ){
				setLevelData(LevelOptions);
			}
		}


		GameObject LevelMap = new GameObject("LevelMap");

		if ( levelType == LevelTypes.Rooms ){
			generateRooms ();
		}

		if ( levelType == LevelTypes.Surface ){
			generateSurface ();
		}

	}

	// Update is called once per frame
	void Update () {

	}

	public void setLevelData(LevelData ld){

		levelType 	= ld.levelType;
		useGravity 	= ld.useGravity;
		minRooms 	= ld.minRooms;
		maxRooms 	= ld.maxRooms;

	}

	public void generateRooms(){
		rooms = new List<GameObject>();
		numberOfRooms = Random.Range(minRooms, maxRooms);
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
				rSize = new Vector3(Random.Range(3f, 25f), Random.Range(3f, 22f), 1f);
				if (i==numberOfRooms-1){
					rSize *= 3.05f;
				} else {
					rSize *= 1.05f;
				}
				rSize.z = 1f;	
				// random Y on that edge
				float rightRandomY = Random.Range ( lastPosition.y - lastSize.y/2 + padding, lastPosition.y + lastSize.y/2 - padding );
				float rightRandomYConnection = Random.Range ( -rSize.y/2f, rSize.y/2f );
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

	public void generateSurface(){
		rooms = new List<GameObject>();
		numberOfRooms = 1;
		Vector3 rSize = new Vector3(300f, 15, 1f);
		Vector3 rPosition = new Vector3(0f, 0f, 1f);

		makeRoom(rSize, rPosition).name = "Surface";

		// make the walls
		foreach( GameObject room in rooms ){
			construct(room);
		}
	}

	[ContextMenu("Reset")] 
	public void Reset(){

		foreach(Transform child in GameObject.Find("LevelMap").transform ){
			Destroy(child.gameObject);
		}

		rooms = new List<GameObject>();
		numberOfRooms = Random.Range(minRooms, maxRooms);
		
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
				
				rSize = new Vector3(Random.Range(3f, 25f), Random.Range(3f, 22f), 1f);
				
				// random Y on that edge
				float rightRandomY = Random.Range (
					lastPosition.y - lastSize.y/2 + padding,
					lastPosition.y + lastSize.y/2 - padding
					);
				
				float rightRandomYConnection = Random.Range (
					-rSize.y/2f,
					rSize.y/2f
					);
				
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

}
