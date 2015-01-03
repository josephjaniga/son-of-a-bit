using UnityEngine;
using System.Collections;

public class LevelMapController : MonoBehaviour {

	public enum LevelTypes {
		Rooms
	};

	public int levelType = (int)LevelTypes.Rooms;

	public bool useGravity = true;

	public int numberOfRooms = 1;
	private int maxRooms = 9;

	// Use this for initialization
	void Start () {

		numberOfRooms = Random.Range(1, maxRooms);
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
