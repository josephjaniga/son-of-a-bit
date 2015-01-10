using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class DataProvider : MonoBehaviour {

	//	List<System> systemsData;

	// 	SYSTEM SEED
	//	PLANET LEVEL SEEDS
	// 	LAST SEEN COORDS
	// 	LAST SEEN TIME

	public int systemSeed = -1;
	public int levelSeed = -1;

	public Vector3 playerSystemLastPosition = Vector3.zero;
	public bool playerSystemInShip = true;
	public string lastPlanetName = "";

	public Vector3 playerLocalLastPosition = Vector3.zero;
	public bool playerLocalInShip = true;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
