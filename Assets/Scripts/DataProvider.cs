using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class DataProvider : MonoBehaviour {

	public bool useGravity = true;
	public int minRooms = 1;
	public int maxRooms = 1;

	public int systemSeed = -1;
	public int levelSeed = -1;
	public LevelTypes levelType = LevelTypes.LegacySurface;
	public PlanetTypes planetType = PlanetTypes.EarthLike;

	public List<Material> levelMaterials = new List<Material>();

	public Vector3 playerSystemLastPosition = Vector3.zero;
	public bool playerSystemInShip = true;
	public string lastPlanetName = "";

	public Vector3 playerLocalLastPosition = Vector3.zero;
	public bool playerLocalInShip = true;

/*
	// Use this for initialization
	void Start () {
	}

	// Update is called once per frame
	void Update () {
	}
*/

}
