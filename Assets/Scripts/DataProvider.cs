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

	public List<Material> levelMaterials = new List<Material>();

	public Vector3 playerSystemLastPosition = Vector3.zero;
	public bool playerSystemInShip = true;
	public string lastPlanetName = "";

	public Vector3 playerLocalLastPosition = Vector3.zero;
	public bool playerLocalInShip = true;

	// public void setLevelData(DataProvider dp){
	// 	levelMaterials 				= dp.levelMaterials;
	// 	useGravity 					= dp.useGravity;
	// 	minRooms 					= dp.minRooms;
	// 	maxRooms 					= dp.maxRooms;
	// 	playerSystemLastPosition 	= dp.playerSystemLastPosition;
	// 	playerSystemInShip 			= dp.playerSystemInShip;
	// 	lastPlanetName 				= dp.lastPlanetName;
	// 	playerLocalLastPosition 	= dp.playerLocalLastPosition;
	// 	playerLocalInShip 			= dp.playerLocalInShip;
	// }

/*
	// Use this for initialization
	void Start () {
	}

	// Update is called once per frame
	void Update () {
	}
*/

}
